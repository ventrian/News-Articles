'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common.Utilities

Imports System.Drawing
Imports System.Drawing.Drawing2d
Imports System.Drawing.Imaging
Imports System.Web
Imports System.Web.Services
Imports Ventrian.ImageResizer
Imports System.IO

Namespace Ventrian.NewsArticles

    Public Class ImageHandler
        Implements System.Web.IHttpHandler

#Region " Private Members "

        Private _width As Integer = ArticleConstants.DEFAULT_THUMBNAIL_WIDTH
        Private _height As Integer = ArticleConstants.DEFAULT_THUMBNAIL_HEIGHT
        Private _homeDirectory As String = Null.NullString
        Private _fileName As String = Null.NullString
        Private _quality As Boolean = False
        Private _cropped As Boolean = False

#End Region

#Region " Private Methods "

        Private Function GetPhotoHeight(ByVal objPhoto As Image) As Integer

            Dim width As Integer
            If (objPhoto.Width > _width) Then
                width = _width
            Else
                width = objPhoto.Width
            End If

            Dim height As Integer = Convert.ToInt32(objPhoto.Height / (objPhoto.Width / width))
            If (height > _height) Then
                height = _height
                width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / height))
            End If

            Return height

        End Function

        Private Function GetPhotoWidth(ByVal objPhoto As Image) As Integer

            Dim width As Integer

            If (objPhoto.Width > _width) Then
                width = _width
            Else
                width = objPhoto.Width
            End If

            Dim height As Integer = Convert.ToInt32(objPhoto.Height / (objPhoto.Width / width))
            If (height > _height) Then
                height = _height
                width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / height))
            End If

            Return width

        End Function

        Private Sub ReadQueryString(ByVal context As HttpContext)

            If Not (context.Request("Width") Is Nothing) Then
                If (IsNumeric(context.Request("Width"))) Then
                    _width = Convert.ToInt32(context.Request("Width"))
                End If
            End If

            If Not (context.Request("Height") Is Nothing) Then
                If (IsNumeric(context.Request("Height"))) Then
                    _height = Convert.ToInt32(context.Request("Height"))
                End If
            End If

            If Not (context.Request("HomeDirectory") Is Nothing) Then
                _homeDirectory = context.Server.UrlDecode(context.Request("HomeDirectory"))
            End If

            If Not (context.Request("FileName") Is Nothing) Then
                _fileName = context.Server.UrlDecode(context.Request("FileName"))
            End If

            If Not (context.Request("Q") Is Nothing) Then
                If (context.Request("Q") = "1") Then
                    _quality = True
                End If
            End If

            If Not (context.Request("S") Is Nothing) Then
                If (context.Request("S") = "1") Then
                    _cropped = True
                End If
            End If

        End Sub

#End Region

#Region " Properties "

        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return True
            End Get
        End Property

#End Region

#Region " Event Handlers "

        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            ' Set up the response settings
            context.Response.ContentType = "image/jpeg"

            ' Caching 
            context.Response.Cache.SetCacheability(HttpCacheability.Public)
            context.Response.Cache.SetExpires(DateTime.Now.AddDays(30))
            context.Response.Cache.VaryByParams("FileName") = True
            context.Response.Cache.VaryByParams("HomeDirectory") = True
            context.Response.Cache.VaryByParams("Width") = True
            context.Response.Cache.VaryByParams("Height") = True
            context.Response.Cache.VaryByParams("s") = True
            context.Response.Cache.AppendCacheExtension("max-age=86400")

            ReadQueryString(context)

            Dim path As String = ""
            If _fileName = "placeholder-600.jpg" Then
                path = "Images/placeholder-600.jpg"
            Else
                path = _homeDirectory & "/" & _fileName
            End If

            context.Items.Add("httpcompress.attemptedinstall", "true")

            If Not (System.IO.File.Exists(context.Server.MapPath(path))) Then
                path = path & ".resources"
                If Not (System.IO.File.Exists(context.Server.MapPath(path))) Then
                    Return
                End If
            End If

            If (_cropped = False) Then
                Dim photo As Image = Image.FromFile(context.Server.MapPath(path))

                Dim width As Integer = GetPhotoWidth(photo)
                Dim height As Integer = GetPhotoHeight(photo)

                photo.Dispose()

                _width = width
                _height = height

            End If

            Dim objQueryString As New NameValueCollection()

            For Each key As String In context.Request.QueryString.Keys
                Dim values() As String = context.Request.QueryString.GetValues(key)
                For Each value As String In values

                    If (key.ToLower() = "width" Or key.ToLower() = "height") Then
                        If (key.ToLower() = "width") Then
                            objQueryString.Add("maxwidth", _width.ToString())
                            objQueryString.Add(key, _width.ToString())
                        End If
                        If (key.ToLower() = "height") Then
                            objQueryString.Add("maxheight", _height.ToString())
                            objQueryString.Add(key, _height.ToString())
                        End If
                    Else
                        objQueryString.Add(key, value)
                    End If
                Next
            Next

            If (_cropped) Then
                objQueryString.Add("crop", "auto")
            End If

            Dim objImage As Bitmap = ImageManager.getBestInstance().BuildImage(context.Server.MapPath(path), objQueryString, New WatermarkSettings(objQueryString))
            If (path.ToLower().EndsWith("jpg")) Then
                objImage.Save(context.Response.OutputStream, ImageFormat.Jpeg)
            Else
                If (path.ToLower().EndsWith("gif")) Then
                    context.Response.ContentType = "image/gif"
                    Dim ios As ImageOutputSettings = New ImageOutputSettings(ImageOutputSettings.GetImageFormatFromPhysicalPath(context.Server.MapPath(path)), objQueryString)
                    ios.SaveImage(context.Response.OutputStream, objImage)
                Else
                    If (path.ToLower().EndsWith("png")) Then
                        Dim objMemoryStream As New MemoryStream()
                        context.Response.ContentType = "image/png"
                        objImage.Save(objMemoryStream, ImageFormat.Png)
                        objMemoryStream.WriteTo(context.Response.OutputStream)
                    Else
                        objImage.Save(context.Response.OutputStream, ImageFormat.Jpeg)
                    End If
                End If
            End If

            'Dim photo As Image = Image.FromFile(context.Server.MapPath(path))

            'Dim width As Integer = GetPhotoWidth(photo)
            'Dim height As Integer = GetPhotoHeight(photo)

            'Dim bmp As New Bitmap(width, height)
            'Dim g As Graphics = Graphics.FromImage(DirectCast(bmp, Image))

            'If (_quality) Then
            '    g.InterpolationMode = InterpolationMode.HighQualityBicubic
            '    g.SmoothingMode = SmoothingMode.HighQuality
            '    g.PixelOffsetMode = PixelOffsetMode.HighQuality
            '    g.CompositingQuality = CompositingQuality.HighQuality
            'End If

            'g.FillRectangle(Brushes.White, 0, 0, width, height)
            'g.DrawImage(photo, 0, 0, width, height)

            'photo.Dispose()

            'If (_quality) Then
            '    Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
            '    Dim params As New EncoderParameters
            '    params.Param(0) = New EncoderParameter(Encoder.Quality, 90L)
            '    bmp.Save(context.Response.OutputStream, info(1), params)
            '    bmp.Dispose()
            'Else
            '    bmp.Save(context.Response.OutputStream, Imaging.ImageFormat.Jpeg)
            'End If

        End Sub

        Public Shared Function GetEncoderInfo(ByVal mimeType As String) As ImageCodecInfo
            Dim codecs() As ImageCodecInfo = ImageCodecInfo.GetImageEncoders()

            Dim i As Integer
            For i = 0 To codecs.Length - 1 Step i + 1
                If codecs(i).MimeType = mimeType Then
                    Return codecs(i)
                End If
            Next

            Return Nothing
        End Function

#End Region

    End Class

End Namespace