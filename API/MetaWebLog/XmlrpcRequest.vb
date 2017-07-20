Imports System.Xml
Imports System.Globalization
Imports System.Linq

Namespace Ventrian.NewsArticles.API.MetaWebLog

    Friend Class XmlrpcRequest

#Region "Private Members"

        Private _inputParams As List(Of XmlNode)

#End Region

#Region "Constructor"

        Public Sub New(input As HttpContext)

            Dim inputXml = ParseRequest(input)
            LoadXmlRequest(inputXml)

        End Sub

#End Region

#Region "Properties"

        Public Property AppKey As String
        Public Property BlogId As String
        Public Property MediaObject As MetaMediaInfo
        Public Property MethodName As String
        Public Property NumberOfPosts As Integer
        Public Property Password As String
        Public Property Post As MetaPostInfo
        Public Property PostId As String
        Public Property Publish As Boolean
        Public Property UserName As String

#End Region

#Region "Methods"

        Private Shared Function GetMediaObject(node As XmlNode) As MetaMediaInfo
            Dim name = node.SelectSingleNode("value/struct/member[name='name']")
            Dim type = node.SelectSingleNode("value/struct/member[name='type']")
            Dim bits = node.SelectSingleNode("value/struct/member[name='bits']")
            Dim temp = New MetaMediaInfo() With { _
                .name = If(name Is Nothing, String.Empty, name.LastChild.InnerText), _
                .type = If(type Is Nothing, "notsent", type.LastChild.InnerText), _
                .bits = Convert.FromBase64String(If(bits Is Nothing, String.Empty, bits.LastChild.InnerText)) _
            }

            Return temp
        End Function

        Private Shared Function GetPost(node As XmlNode) As MetaPostInfo
            Dim temp = New MetaPostInfo()

            ' Require Title and Description
            Dim title = node.SelectSingleNode("value/struct/member[name='title']")
            If title Is Nothing Then
                Throw New MetaException("05", "Page Struct Element, Title, not Sent.")
            End If

            temp.title = title.LastChild.InnerText

            Dim description = node.SelectSingleNode("value/struct/member[name='description']")
            If description Is Nothing Then
                Throw New MetaException("05", "Page Struct Element, Description, not Sent.")
            End If

            temp.description = description.LastChild.InnerText

            Dim link = node.SelectSingleNode("value/struct/member[name='link']")
            temp.link = If(link Is Nothing, String.Empty, link.LastChild.InnerText)

            Dim excerpt = node.SelectSingleNode("value/struct/member[name='mt_excerpt']")
            temp.excerpt = If(excerpt Is Nothing, String.Empty, excerpt.LastChild.InnerText)

            Dim cats = New List(Of String)()
            Dim categories = node.SelectSingleNode("value/struct/member[name='categories']")
            If categories IsNot Nothing Then
                Dim categoryArray = categories.LastChild
                Dim categoryArrayNodes As XmlNodeList = categoryArray.SelectNodes("array/data/value/string")
                If categoryArrayNodes IsNot Nothing Then
                    For Each objNode As XmlNode In categoryArrayNodes
                        cats.Add(objNode.InnerText)
                    Next
                End If
            End If

            temp.categories = cats

            ' postDate has a few different names to worry about
            Dim dateCreated = node.SelectSingleNode("value/struct/member[name='dateCreated']")
            Dim pubDate = node.SelectSingleNode("value/struct/member[name='pubDate']")
            If dateCreated IsNot Nothing Then
                Try
                    Dim tempDate = dateCreated.LastChild.InnerText
                    temp.postDate = DateTime.ParseExact(tempDate, "yyyyMMdd'T'HH':'mm':'ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
                Catch ex As Exception
                    ' Ignore PubDate Error
                    Debug.WriteLine(ex.Message)
                End Try
            ElseIf pubDate IsNot Nothing Then
                Try
                    Dim tempPubDate = pubDate.LastChild.InnerText
                    temp.postDate = DateTime.ParseExact(tempPubDate, "yyyyMMdd'T'HH':'mm':'ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
                Catch ex As Exception
                    ' Ignore PubDate Error
                    Debug.WriteLine(ex.Message)
                End Try
            End If

            ' WLW tags implementation using mt_keywords
            Dim tags = New List(Of String)()
            Dim keywords = node.SelectSingleNode("value/struct/member[name='mt_keywords']")
            If keywords IsNot Nothing Then
                Dim tagsList = keywords.LastChild.InnerText
                For Each item As String In tagsList.Split(","c)
                    If (item.Trim() <> "") Then
                        tags.Add(item.Trim())
                    End If
                Next
            End If

            temp.tags = tags

            Return temp
        End Function

        Private Sub LoadXmlRequest(xml As String)
            Dim request = New XmlDocument()
            Try
                If Not (xml.StartsWith("<?xml") OrElse xml.StartsWith("<method")) Then
                    xml = xml.Substring(xml.IndexOf("<?xml", StringComparison.Ordinal))
                End If

                request.LoadXml(xml)
            Catch ex As Exception
                Throw New MetaException("01", String.Format("Invalid XMLRPC Request. ({0})", ex.Message))
            End Try

            ' Method name is always first
            If request.DocumentElement IsNot Nothing Then
                MethodName = request.DocumentElement.ChildNodes(0).InnerText
            End If
            ' Throw New MetaException("01", xml.ToString())
            ' Parameters are next (and last)
            Dim xmlParams As XmlNodeList = request.SelectNodes("/methodCall/params/param")
            If xmlParams IsNot Nothing Then
                _inputParams = xmlParams.Cast(Of XmlNode)().ToList()
            End If

            ' Determine what params are what by method name
            Select Case MethodName
                Case "metaWeblog.newPost"
                    BlogId = _inputParams(0).InnerText
                    UserName = _inputParams(1).InnerText
                    Password = _inputParams(2).InnerText
                    Post = GetPost(_inputParams(3))
                    Publish = _inputParams(4).InnerText <> "0" AndAlso _inputParams(4).InnerText <> "false"
                    Exit Select
                Case "metaWeblog.editPost"
                    PostId = _inputParams(0).InnerText
                    UserName = _inputParams(1).InnerText
                    Password = _inputParams(2).InnerText
                    Post = GetPost(_inputParams(3))
                    Publish = _inputParams(4).InnerText <> "0" AndAlso _inputParams(4).InnerText <> "false"
                    Exit Select
                Case "metaWeblog.getPost"
                    PostId = _inputParams(0).InnerText
                    UserName = _inputParams(1).InnerText
                    Password = _inputParams(2).InnerText
                    Exit Select
                Case "metaWeblog.newMediaObject"
                    BlogId = _inputParams(0).InnerText
                    UserName = _inputParams(1).InnerText
                    Password = _inputParams(2).InnerText
                    MediaObject = GetMediaObject(_inputParams(3))
                    Exit Select
                Case "metaWeblog.getCategories", "wp.getTags"
                    BlogId = _inputParams(0).InnerText
                    UserName = _inputParams(1).InnerText
                    Password = _inputParams(2).InnerText
                    Exit Select
                Case "metaWeblog.getRecentPosts"
                    BlogId = _inputParams(0).InnerText
                    UserName = _inputParams(1).InnerText
                    Password = _inputParams(2).InnerText
                    NumberOfPosts = Int32.Parse(_inputParams(3).InnerText, CultureInfo.InvariantCulture)
                    Exit Select
                Case "blogger.getUsersBlogs", "metaWeblog.getUsersBlogs"
                    AppKey = _inputParams(0).InnerText
                    UserName = _inputParams(1).InnerText
                    Password = _inputParams(2).InnerText
                    Exit Select
                Case "blogger.deletePost"
                    AppKey = _inputParams(0).InnerText
                    PostId = _inputParams(1).InnerText
                    UserName = _inputParams(2).InnerText
                    Password = _inputParams(3).InnerText
                    Publish = _inputParams(4).InnerText <> "0" AndAlso _inputParams(4).InnerText <> "false"
                    Exit Select
                Case Else
                    Throw New MetaException("02", String.Format("Unknown Method. ({0})", MethodName))
            End Select
        End Sub

        Private Shared Function ParseRequest(context As HttpContext) As String
            Dim buffer = New Byte(context.Request.InputStream.Length - 1) {}

            context.Request.InputStream.Position = 0
            context.Request.InputStream.Read(buffer, 0, buffer.Length)

            Return Encoding.UTF8.GetString(buffer)
        End Function

#End Region

    End Class

End Namespace