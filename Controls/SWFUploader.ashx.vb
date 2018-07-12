Imports System.Web
Imports System.Web.Services

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.FileSystem
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

Namespace Ventrian.NewsArticles.Controls

    Public Class SWFUploader
        Implements System.Web.IHttpHandler

#Region " Private Members "

        Private _articleID As Integer = Null.NullInteger
        Private _moduleID As Integer = Null.NullInteger
        Private _tabID As Integer = Null.NullInteger
        Private _tabModuleID As Integer = Null.NullInteger
        Private _portalID As Integer = Null.NullInteger
        Private _ticket As String = Null.NullString
        Private _userID As Integer = Null.NullInteger
        Private _imageGuid As String = Null.NullString

        Private _articleSettings As Ventrian.NewsArticles.ArticleSettings
        Private _settings As Hashtable
        Private _context As HttpContext

#End Region

#Region " Private Properties "

        Private ReadOnly Property ArticleSettings() As Ventrian.NewsArticles.ArticleSettings
            Get
                If _articleSettings Is Nothing Then
                    Dim objModuleController As New ModuleController
                    Dim objModule As ModuleInfo = objModuleController.GetModule(_moduleID, _tabID)

                    _articleSettings = New Ventrian.NewsArticles.ArticleSettings(Settings, PortalController.GetCurrentPortalSettings(), objModule)
                End If
                Return _articleSettings
            End Get
        End Property

        Private ReadOnly Property Settings() As Hashtable
            Get
                If _settings Is Nothing Then
                    Dim objModuleController As New ModuleController
                    _settings = objModuleController.GetModuleSettings(_moduleID)
                    _settings = GetTabModuleSettings(_tabModuleID, _settings)
                End If
                Return _settings
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub AuthenticateUserFromTicket()

            If (_ticket <> "") Then

                Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(_ticket)
                Dim fi As FormsIdentity = New FormsIdentity(ticket)

                Dim roles As String() = Nothing
                HttpContext.Current.User = New System.Security.Principal.GenericPrincipal(fi, roles)

                Dim objUser As UserInfo = UserController.GetUserByName(_portalID, HttpContext.Current.User.Identity.Name)

                If Not (objUser Is Nothing) Then
                    _userID = objUser.UserID
                    HttpContext.Current.Items("UserInfo") = objUser

                    Dim objUserController As New UserController
                    roles = objUserController.GetUser(_portalID, _userID).Roles

                    Dim strPortalRoles As String = Join(roles, New Char() {";"c})
                    _context.Items.Add("UserRoles", ";" + strPortalRoles + ";")
                End If

            End If

        End Sub

        Private Function GetTabModuleSettings(ByVal TabModuleId As Integer, ByVal settings As Hashtable) As Hashtable

            Dim dr As IDataReader = DotNetNuke.Data.DataProvider.Instance().GetTabModuleSettings(TabModuleId)

            While dr.Read()

                If Not dr.IsDBNull(1) Then
                    settings(dr.GetString(0)) = dr.GetString(1)
                Else
                    settings(dr.GetString(0)) = ""
                End If

            End While

            dr.Close()

            Return settings

        End Function

        Private Sub ReadQueryString()

            If (_context.Request("ModuleID") <> "") Then
                _moduleID = Convert.ToInt32(_context.Request("ModuleID"))
            End If

            If (_context.Request("PortalID") <> "") Then
                _portalID = Convert.ToInt32(_context.Request("PortalID"))
            End If

            If (_context.Request("ArticleID") <> "") Then
                _articleID = Convert.ToInt32(_context.Request("ArticleID"))
            End If

            If (_context.Request("TabModuleID") <> "") Then
                _tabModuleID = Convert.ToInt32(_context.Request("TabModuleID"))
            End If

            If (_context.Request("TabID") <> "") Then
                _tabID = Convert.ToInt32(_context.Request("TabID"))
            End If

            If (_context.Request("Ticket") <> "") Then
                _ticket = _context.Request("Ticket")
            End If

            If (_articleID = Null.NullInteger) Then
                If (_context.Request("ArticleGuid") <> "") Then
                    _imageGuid = _context.Request("ArticleGuid")
                End If
            End If

        End Sub

#End Region

#Region " Interface Methods "

        Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            _context = context
            context.Response.ContentType = "text/plain"

            ReadQueryString()
            AuthenticateUserFromTicket()

            If (_context.Request.IsAuthenticated = False) Then
                _context.Response.Write("-2")
                _context.Response.End()
            End If

            Dim objImageController As New ImageController()
            Dim objFile As HttpPostedFile = _context.Request.Files("Filedata")

            If Not (objFile Is Nothing) Then

                Dim objPortalController As New PortalController()
                If (objPortalController.HasSpaceAvailable(_portalID, objFile.ContentLength) = False) Then
                    _context.Response.Write("-1")
                    _context.Response.End()
                End If

                Dim username As String = _context.User.Identity.Name

                Dim objImage As New ImageInfo

                objImage.ArticleID = _articleID
                If (_articleID = Null.NullInteger) Then
                    objImage.ImageGuid = _imageGuid
                End If
                objImage.FileName = objFile.FileName

                If (objFile.FileName.ToLower().EndsWith(".jpg")) Then
                    objImage.ContentType = "image/jpeg"
                End If

                If (objFile.FileName.ToLower().EndsWith(".gif")) Then
                    objImage.ContentType = "image/gif"
                End If

                If (objFile.FileName.ToLower().EndsWith(".png")) Then
                    objImage.ContentType = "image/png"
                End If

                Dim maxWidth As Integer = ArticleSettings.MaxImageWidth
                Dim maxHeight As Integer = ArticleSettings.MaxImageHeight

                Dim photo As Drawing.Image = Drawing.Image.FromStream(objFile.InputStream)

                objImage.Width = photo.Width
                objImage.Height = photo.Height

                If (objImage.Width > maxWidth) Then
                    objImage.Width = maxWidth
                    objImage.Height = Convert.ToInt32(objImage.Height / (photo.Width / maxWidth))
                End If

                If (objImage.Height > maxHeight) Then
                    objImage.Height = maxHeight
                    objImage.Width = Convert.ToInt32(photo.Width / (photo.Height / maxHeight))
                End If

                objImage.SortOrder = 0

                Dim imagesList As List(Of ImageInfo) = objImageController.GetImageList(_articleID, _imageGuid)

                If (imagesList.Count > 0) Then
                    objImage.SortOrder = CType(imagesList(imagesList.Count - 1), ImageInfo).SortOrder + 1
                End If

                Dim objPortalSettings As PortalSettings = PortalController.GetCurrentPortalSettings()

                Dim folder As String = ""
                Dim folderID As Integer = Null.NullInteger
                If (IsNumeric(context.Request.Form("FolderID"))) Then
                    folderID = Convert.ToInt32(context.Request.Form("FolderID"))
                End If

                If (folderID <> Null.NullInteger) Then
                    Dim objFolder As FolderInfo = DotNetNuke.Services.FileSystem.FolderManager.Instance.GetFolder(folderID)
                    If (objFolder IsNot Nothing) Then
                        folder = objFolder.FolderPath
                    End If
                End If

                objImage.Folder = folder

                Select Case objImage.ContentType.ToLower()
                    Case "image/jpeg"
                        objImage.Extension = "jpg"
                        Exit Select
                    Case "image/gif"
                        objImage.Extension = "gif"
                        Exit Select
                    Case "image/png"
                        objImage.Extension = "png"
                        Exit Select
                End Select

                objImage.Title = objFile.FileName.Replace("." & objImage.Extension, "")

                Dim filePath As String = objPortalSettings.HomeDirectoryMapPath & folder.Replace("/", "\")

                If Not (Directory.Exists(filePath)) Then
                    Directory.CreateDirectory(filePath)
                End If

                If (File.Exists(filePath & objImage.FileName)) Then
                    For i As Integer = 1 To 100
                        If (File.Exists(filePath & i.ToString() & "_" & objImage.FileName) = False) Then
                            objImage.FileName = i.ToString() & "_" & objImage.FileName
                            Exit For
                        End If
                    Next
                End If

                objImage.Size = objFile.ContentLength
                If ((photo.Width < maxWidth And photo.Height < maxHeight) Or (ArticleSettings.ResizeImages = False)) Then
                    objFile.SaveAs(filePath & objImage.FileName)
                Else
                    Dim bmp As New Bitmap(objImage.Width, objImage.Height)
                    Dim g As Graphics = Graphics.FromImage(DirectCast(bmp, Drawing.Image))

                    g.InterpolationMode = InterpolationMode.HighQualityBicubic
                    g.SmoothingMode = SmoothingMode.HighQuality
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality
                    g.CompositingQuality = CompositingQuality.HighQuality

                    g.DrawImage(photo, 0, 0, objImage.Width, objImage.Height)

                    If (ArticleSettings.WatermarkEnabled And ArticleSettings.WatermarkText <> "") Then
                        Dim crSize As SizeF = New SizeF
                        Dim brushColor As Brush = Brushes.Yellow
                        Dim fnt As Font = New Font("Verdana", 11, FontStyle.Bold)
                        Dim strDirection As StringFormat = New StringFormat

                        strDirection.Alignment = StringAlignment.Center
                        crSize = g.MeasureString(ArticleSettings.WatermarkText, fnt)

                        Dim yPixelsFromBottom As Integer = Convert.ToInt32(Convert.ToDouble(objImage.Height) * 0.05)
                        Dim yPosFromBottom As Single = Convert.ToSingle((objImage.Height - yPixelsFromBottom) - (crSize.Height / 2))
                        Dim xCenterOfImage As Single = Convert.ToSingle((objImage.Width / 2))

                        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

                        Dim semiTransBrush2 As SolidBrush = New SolidBrush(Color.FromArgb(153, 0, 0, 0))
                        g.DrawString(ArticleSettings.WatermarkText, fnt, semiTransBrush2, New PointF(xCenterOfImage + 1, yPosFromBottom + 1), strDirection)

                        Dim semiTransBrush As SolidBrush = New SolidBrush(Color.FromArgb(153, 255, 255, 255))
                        g.DrawString(ArticleSettings.WatermarkText, fnt, semiTransBrush, New PointF(xCenterOfImage, yPosFromBottom), strDirection)
                    End If

                    If (ArticleSettings.WatermarkEnabled And ArticleSettings.WatermarkImage <> "") Then
                        Dim watermark As String = objPortalSettings.HomeDirectoryMapPath & ArticleSettings.WatermarkImage
                        If (File.Exists(watermark)) Then
                            Dim imgWatermark As Image = New Bitmap(watermark)
                            Dim wmWidth As Integer = imgWatermark.Width
                            Dim wmHeight As Integer = imgWatermark.Height

                            Dim objImageAttributes As New ImageAttributes()
                            Dim objColorMap As New ColorMap()
                            objColorMap.OldColor = Color.FromArgb(255, 0, 255, 0)
                            objColorMap.NewColor = Color.FromArgb(0, 0, 0, 0)
                            Dim remapTable As ColorMap() = {objColorMap}
                            objImageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap)

                            Dim colorMatrixElements As Single()() = {New Single() {1.0F, 0.0F, 0.0F, 0.0F, 0.0F}, New Single() {0.0F, 1.0F, 0.0F, 0.0F, 0.0F}, New Single() {0.0F, 0.0F, 1.0F, 0.0F, 0.0F}, New Single() {0.0F, 0.0F, 0.0F, 0.3F, 0.0F}, New Single() {0.0F, 0.0F, 0.0F, 0.0F, 1.0F}}
                            Dim wmColorMatrix As New ColorMatrix(colorMatrixElements)
                            objImageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.[Default], ColorAdjustType.Bitmap)

                            Dim xPosOfWm As Integer = ((objImage.Width - wmWidth) - 10)
                            Dim yPosOfWm As Integer = 10

                            Select Case ArticleSettings.WatermarkPosition
                                Case WatermarkPosition.TopLeft
                                    xPosOfWm = 10
                                    yPosOfWm = 10
                                    Exit Select

                                Case WatermarkPosition.TopRight
                                    xPosOfWm = ((objImage.Width - wmWidth) - 10)
                                    yPosOfWm = 10
                                    Exit Select

                                Case WatermarkPosition.BottomLeft
                                    xPosOfWm = 10
                                    yPosOfWm = ((objImage.Height - wmHeight) - 10)

                                Case WatermarkPosition.BottomRight
                                    xPosOfWm = ((objImage.Width - wmWidth) - 10)
                                    yPosOfWm = ((objImage.Height - wmHeight) - 10)
                            End Select

                            g.DrawImage(imgWatermark, New Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight), 0, 0, wmWidth, wmHeight, _
                             GraphicsUnit.Pixel, objImageAttributes)
                            imgWatermark.Dispose()
                        End If
                    End If

                    photo.Dispose()

                    Select Case objFile.ContentType.ToLower()
                        Case "image/jpeg"
                            Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
                            Dim encoderParameters As New EncoderParameters(1)
                            encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                            bmp.Save(filePath & objImage.FileName, info(1), encoderParameters)

                        Case "image/gif"
                            'Dim quantizer As New ImageQuantization.OctreeQuantizer(255, 8)
                            'Dim bmpQuantized As Bitmap = quantizer.Quantize(bmp)
                            'bmpQuantized.Save(filePath & objPhoto.Filename, ImageFormat.Gif)
                            ' Not working in medium trust.
                            bmp.Save(filePath & objImage.FileName, ImageFormat.Gif)

                        Case Else
                            'Shouldn't get to here because of validators.                                
                            Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
                            Dim encoderParameters As New EncoderParameters(1)
                            encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                            bmp.Save(filePath & objImage.FileName, info(1), encoderParameters)
                    End Select

                    bmp.Dispose()

                    If (File.Exists(filePath & objImage.FileName)) Then
                        Dim fi As New IO.FileInfo(filePath & objImage.FileName)
                        If (fi IsNot Nothing) Then
                            objImage.Size = Convert.ToInt32(fi.Length)
                        End If
                    End If
                End If

                objImage.ImageID = objImageController.Add(objImage)

                If (_articleID <> Null.NullInteger) Then
                    Dim objArticleController As New ArticleController
                    Dim objArticle As ArticleInfo = objArticleController.GetArticle(_articleID)
                    If (objArticle IsNot Nothing) Then
                        objArticle.ImageCount = objArticle.ImageCount + 1
                        objArticleController.UpdateArticle(objArticle)
                    End If
                End If

            End If

            _context.Response.Write("0")
            _context.Response.End()

        End Sub

        ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

#End Region

    End Class

End Namespace