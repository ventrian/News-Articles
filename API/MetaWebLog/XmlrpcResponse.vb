Imports System.Xml

Namespace Ventrian.NewsArticles.API.MetaWebLog

    Friend Class XmlrpcResponse

#Region "Private Members"

        Private ReadOnly _methodName As String

#End Region

#Region "Constructors and Destructors"

        Public Sub New(methodName As String)
            _methodName = methodName
            Blogs = New List(Of MetaBlogInfo)()
            Categories = New List(Of MetaCategoryInfo)()
            Keywords = New List(Of String)()
            Posts = New List(Of MetaPostInfo)()
        End Sub

#End Region

#Region "Properties"

        Public Property Blogs As List(Of MetaBlogInfo)
        Public Property Categories As List(Of MetaCategoryInfo)
        Public Property Completed As Boolean
        Public Property Fault As MetaFaultInfo
        Public Property Keywords As List(Of String)
        Public Property MediaUrlInfo As MetaMediaUrlInfo
        Public Property Post As MetaPostInfo
        Public Property PostId As String
        Public Property Posts As List(Of MetaPostInfo)

#End Region

#Region "Public Methods"

        Public Sub Response(context As HttpContext)

            context.Response.ContentType = "text/xml"
            Using data As New XmlTextWriter(context.Response.OutputStream, Encoding.UTF8)
                data.Formatting = Formatting.Indented
                data.WriteStartDocument()
                data.WriteStartElement("methodResponse")
                data.WriteStartElement(If(_methodName = "fault", "fault", "params"))

                Select Case _methodName
                    Case "metaWeblog.newPost"
                        WriteNewPost(data)
                        Exit Select
                    Case "metaWeblog.getPost"
                        WritePost(data)
                        Exit Select
                    Case "metaWeblog.newMediaObject"
                        WriteMediaInfo(data)
                        Exit Select
                    Case "metaWeblog.getCategories"
                        WriteGetCategories(data)
                        Exit Select
                    Case "metaWeblog.getRecentPosts"
                        WritePosts(data)
                        Exit Select
                    Case "blogger.getUsersBlogs", "metaWeblog.getUsersBlogs"
                        WriteGetUsersBlogs(data)
                        Exit Select
                    Case "metaWeblog.editPost", "blogger.deletePost"
                        WriteBool(data)
                        Exit Select
                    Case "wp.getTags"
                        WriteKeywords(data)
                        Exit Select
                    Case "fault"
                        WriteFault(data)
                        Exit Select
                End Select

                data.WriteEndElement()
                data.WriteEndElement()
                data.WriteEndDocument()
            End Using

        End Sub

#End Region

#Region "Methods"

        Private Shared Function ConvertDatetoIso8601([date] As DateTime) As String
            Dim temp = String.Format("{0}{1}{2}T{3}:{4}:{5}", [date].Year, [date].Month.ToString().PadLeft(2, "0"c), [date].Day.ToString().PadLeft(2, "0"c), [date].Hour.ToString().PadLeft(2, "0"c), [date].Minute.ToString().PadLeft(2, "0"c), _
                [date].Second.ToString().PadLeft(2, "0"c))
            Return temp
        End Function

        Private Sub WriteBool(data As XmlWriter)
            Dim postValue = "0"
            If Completed Then
                postValue = "1"
            End If

            data.WriteStartElement("param")
            data.WriteStartElement("value")
            data.WriteElementString("boolean", postValue)
            data.WriteEndElement()
            data.WriteEndElement()
        End Sub

        Private Sub WriteFault(data As XmlWriter)
            data.WriteStartElement("value")
            data.WriteStartElement("struct")

            ' faultCode
            data.WriteStartElement("member")
            data.WriteElementString("name", "faultCode")
            data.WriteElementString("value", Fault.FaultCode)
            data.WriteEndElement()

            ' faultString
            data.WriteStartElement("member")
            data.WriteElementString("name", "faultString")
            data.WriteElementString("value", Fault.FaultString)
            data.WriteEndElement()

            data.WriteEndElement()
            data.WriteEndElement()
        End Sub

        Private Sub WriteGetCategories(data As XmlWriter)
            data.WriteStartElement("param")
            data.WriteStartElement("value")
            data.WriteStartElement("array")
            data.WriteStartElement("data")

            For Each category As MetaCategoryInfo In Categories
                data.WriteStartElement("value")
                data.WriteStartElement("struct")

                ' description
                data.WriteStartElement("member")
                data.WriteElementString("name", "description")
                data.WriteElementString("value", category.Description)
                data.WriteEndElement()

                ' categoryid
                data.WriteStartElement("member")
                data.WriteElementString("name", "categoryid")
                data.WriteElementString("value", category.Id)
                data.WriteEndElement()

                ' title
                data.WriteStartElement("member")
                data.WriteElementString("name", "title")
                data.WriteElementString("value", category.Title)
                data.WriteEndElement()

                ' htmlUrl 
                data.WriteStartElement("member")
                data.WriteElementString("name", "htmlUrl")
                data.WriteElementString("value", category.HtmlUrl)
                data.WriteEndElement()

                ' rssUrl
                data.WriteStartElement("member")
                data.WriteElementString("name", "rssUrl")
                data.WriteElementString("value", category.RssUrl)
                data.WriteEndElement()

                data.WriteEndElement()
                data.WriteEndElement()
            Next

            ' Close tags
            data.WriteEndElement()
            data.WriteEndElement()
            data.WriteEndElement()
            data.WriteEndElement()
        End Sub

        Private Sub WriteGetUsersBlogs(data As XmlWriter)
            data.WriteStartElement("param")
            data.WriteStartElement("value")
            data.WriteStartElement("array")
            data.WriteStartElement("data")

            For Each blog As MetaBlogInfo In Blogs
                data.WriteStartElement("value")
                data.WriteStartElement("struct")

                ' url
                data.WriteStartElement("member")
                data.WriteElementString("name", "url")
                data.WriteElementString("value", blog.Url)
                data.WriteEndElement()

                ' blogid
                data.WriteStartElement("member")
                data.WriteElementString("name", "blogid")
                data.WriteElementString("value", blog.BlogId)
                data.WriteEndElement()

                ' blogName
                data.WriteStartElement("member")
                data.WriteElementString("name", "blogName")
                data.WriteElementString("value", blog.BlogName)
                data.WriteEndElement()

                data.WriteEndElement()
                data.WriteEndElement()
            Next

            ' Close tags
            data.WriteEndElement()
            data.WriteEndElement()
            data.WriteEndElement()
            data.WriteEndElement()
        End Sub

        Private Sub WriteKeywords(data As XmlWriter)
            data.WriteStartElement("param")
            data.WriteStartElement("value")
            data.WriteStartElement("array")
            data.WriteStartElement("data")

            For Each keyword As String In Keywords
                data.WriteStartElement("value")
                data.WriteStartElement("struct")

                ' keywordName
                data.WriteStartElement("member")
                data.WriteElementString("name", "name")
                data.WriteElementString("value", keyword)
                data.WriteEndElement()

                data.WriteEndElement()
                data.WriteEndElement()
            Next

            ' Close tags
            data.WriteEndElement()
            data.WriteEndElement()
            data.WriteEndElement()
            data.WriteEndElement()
        End Sub

        Private Sub WriteMediaInfo(data As XmlWriter)
            data.WriteStartElement("param")
            data.WriteStartElement("value")
            data.WriteStartElement("struct")

            ' url
            data.WriteStartElement("member")
            data.WriteElementString("name", "url")
            data.WriteStartElement("value")
            data.WriteElementString("string", MediaUrlInfo.Url)
            data.WriteEndElement()
            data.WriteEndElement()

            data.WriteEndElement()
            data.WriteEndElement()
            data.WriteEndElement()
        End Sub

        Private Sub WriteNewPost(data As XmlWriter)
            data.WriteStartElement("param")
            data.WriteStartElement("value")
            data.WriteElementString("string", PostId)
            data.WriteEndElement()
            data.WriteEndElement()
        End Sub

        Private Sub WritePost(data As XmlWriter)
            data.WriteStartElement("param")
            data.WriteStartElement("value")
            data.WriteStartElement("struct")

            ' postid
            data.WriteStartElement("member")
            data.WriteElementString("name", "postid")
            data.WriteStartElement("value")
            data.WriteElementString("string", Post.PostId)
            data.WriteEndElement()
            data.WriteEndElement()

            ' title
            data.WriteStartElement("member")
            data.WriteElementString("name", "title")
            data.WriteStartElement("value")
            data.WriteElementString("string", Post.Title)
            data.WriteEndElement()
            data.WriteEndElement()

            ' description
            data.WriteStartElement("member")
            data.WriteElementString("name", "description")
            data.WriteStartElement("value")
            data.WriteElementString("string", Post.Description)
            data.WriteEndElement()
            data.WriteEndElement()

            ' link
            data.WriteStartElement("member")
            data.WriteElementString("name", "link")
            data.WriteStartElement("value")
            data.WriteElementString("string", Post.Link)
            data.WriteEndElement()
            data.WriteEndElement()

            ' excerpt
            data.WriteStartElement("member")
            data.WriteElementString("name", "mt_excerpt")
            data.WriteStartElement("value")
            data.WriteElementString("string", Post.Excerpt)
            data.WriteEndElement()
            data.WriteEndElement()

            ' dateCreated
            data.WriteStartElement("member")
            data.WriteElementString("name", "dateCreated")
            data.WriteStartElement("value")
            data.WriteElementString("dateTime.iso8601", ConvertDatetoIso8601(Post.PostDate))
            data.WriteEndElement()
            data.WriteEndElement()

            ' publish
            data.WriteStartElement("member")
            data.WriteElementString("name", "publish")
            data.WriteStartElement("value")
            data.WriteElementString("boolean", If(Post.Publish, "1", "0"))

            data.WriteEndElement()
            data.WriteEndElement()

            ' tags (mt_keywords)
            data.WriteStartElement("member")
            data.WriteElementString("name", "mt_keywords")
            data.WriteStartElement("value")
            Dim tags = New String(Post.Tags.Count - 1) {}
            For i As Integer = 0 To Post.Tags.Count - 1
                tags(i) = Post.Tags(i)
            Next

            Dim tagList = String.Join(",", tags)
            data.WriteElementString("string", tagList)
            data.WriteEndElement()
            data.WriteEndElement()

            ' categories
            If Post.Categories.Count > 0 Then
                data.WriteStartElement("member")
                data.WriteElementString("name", "categories")
                data.WriteStartElement("value")
                data.WriteStartElement("array")
                data.WriteStartElement("data")
                For Each cat As String In Post.Categories
                    data.WriteStartElement("value")
                    data.WriteElementString("string", cat)
                    data.WriteEndElement()
                Next

                data.WriteEndElement()
                data.WriteEndElement()
                data.WriteEndElement()
                data.WriteEndElement()
            End If

            data.WriteEndElement()
            data.WriteEndElement()
            data.WriteEndElement()
        End Sub

        Private Sub WritePosts(data As XmlWriter)
            data.WriteStartElement("param")
            data.WriteStartElement("value")
            data.WriteStartElement("array")
            data.WriteStartElement("data")

            For Each childPost As MetaPostInfo In Posts
                data.WriteStartElement("value")
                data.WriteStartElement("struct")

                ' postid
                data.WriteStartElement("member")
                data.WriteElementString("name", "postid")
                data.WriteStartElement("value")
                data.WriteElementString("string", childPost.PostId)
                data.WriteEndElement()
                data.WriteEndElement()

                ' dateCreated
                data.WriteStartElement("member")
                data.WriteElementString("name", "dateCreated")
                data.WriteStartElement("value")
                data.WriteElementString("dateTime.iso8601", ConvertDatetoIso8601(childPost.PostDate))
                data.WriteEndElement()
                data.WriteEndElement()

                ' title
                data.WriteStartElement("member")
                data.WriteElementString("name", "title")
                data.WriteStartElement("value")
                data.WriteElementString("string", childPost.Title)
                data.WriteEndElement()
                data.WriteEndElement()

                ' description
                data.WriteStartElement("member")
                data.WriteElementString("name", "description")
                data.WriteStartElement("value")
                data.WriteElementString("string", childPost.Description)
                data.WriteEndElement()
                data.WriteEndElement()

                ' link
                data.WriteStartElement("member")
                data.WriteElementString("name", "link")
                data.WriteStartElement("value")
                data.WriteElementString("string", childPost.Link)
                data.WriteEndElement()
                data.WriteEndElement()

                ' excerpt
                data.WriteStartElement("member")
                data.WriteElementString("name", "mt_excerpt")
                data.WriteStartElement("value")
                data.WriteElementString("string", childPost.Excerpt)
                data.WriteEndElement()
                data.WriteEndElement()

                ' tags (mt_keywords)
                data.WriteStartElement("member")
                data.WriteElementString("name", "mt_keywords")
                data.WriteStartElement("value")
                Dim tags = New String(childPost.Tags.Count - 1) {}
                For i As Integer = 0 To childPost.Tags.Count - 1
                    tags(i) = childPost.Tags(i)
                Next

                Dim tagList = String.Join(",", tags)
                data.WriteElementString("string", tagList)
                data.WriteEndElement()
                data.WriteEndElement()

                ' publish
                data.WriteStartElement("member")
                data.WriteElementString("name", "publish")
                data.WriteStartElement("value")
                data.WriteElementString("boolean", If(childPost.Publish, "1", "0"))

                data.WriteEndElement()
                data.WriteEndElement()

                ' categories
                If childPost.Categories.Count > 0 Then
                    data.WriteStartElement("member")
                    data.WriteElementString("name", "categories")
                    data.WriteStartElement("value")
                    data.WriteStartElement("array")
                    data.WriteStartElement("data")
                    For Each cat As String In childPost.Categories
                        data.WriteStartElement("value")
                        data.WriteElementString("string", cat)
                        data.WriteEndElement()
                    Next

                    data.WriteEndElement()
                    data.WriteEndElement()
                    data.WriteEndElement()
                    data.WriteEndElement()
                End If

                data.WriteEndElement()
                data.WriteEndElement()
            Next

            ' Close tags
            data.WriteEndElement()
            data.WriteEndElement()
            data.WriteEndElement()
            data.WriteEndElement()
        End Sub

#End Region

    End Class

End Namespace
