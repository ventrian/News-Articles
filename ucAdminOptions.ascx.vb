'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports System.Xml

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucAdminOptions
        Inherits NewsArticleModuleBase

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                trSiteTemplates.Visible = Me.UserInfo.IsSuperUser
                If (Settings.Contains(ArticleConstants.PERMISSION_SITE_TEMPLATES_SETTING)) Then
                    If (Settings(ArticleConstants.PERMISSION_SITE_TEMPLATES_SETTING).ToString() <> "") Then
                        trSiteTemplates.Visible = PortalSecurity.IsInRoles(Settings(ArticleConstants.PERMISSION_SITE_TEMPLATES_SETTING).ToString())
                    End If
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

        Protected Sub cmdImport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdImport.Click

            Dim file As String = PortalSettings.HomeDirectoryMapPath & "import.xml"

            Dim doc As New XmlDocument
            doc.Load(file)

            Dim nsMgr As New XmlNamespaceManager(doc.NameTable)
            nsMgr.AddNamespace("wp", "http://wordpress.org/export/1.2/")
            nsMgr.AddNamespace("content", "http://purl.org/rss/1.0/modules/content/")

            Dim nodeRoot As XmlNode = doc.DocumentElement

            Dim objCategoriesNodes As XmlNodeList = nodeRoot.SelectNodes("/rss/channel/wp:category", nsMgr)

            Dim objCategoryController As New CategoryController()

            For Each objCategoryNode As XmlNode In objCategoriesNodes

                Dim objName As XmlNode = objCategoryNode.SelectSingleNode("wp:cat_name", nsMgr)

                Dim objCategoryInfo As New CategoryInfo

                objCategoryInfo.CategoryID = Null.NullInteger
                objCategoryInfo.ModuleID = ModuleId
                objCategoryInfo.ParentID = Null.NullInteger
                objCategoryInfo.Name = objName.InnerText
                objCategoryInfo.Description = ""
                objCategoryInfo.InheritSecurity = True
                objCategoryInfo.CategorySecurityType = 0

                objCategoryInfo.MetaTitle = ""
                objCategoryInfo.MetaDescription = ""
                objCategoryInfo.MetaKeywords = ""

                objCategoryController.AddCategory(objCategoryInfo)

            Next

            Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(ModuleId, Null.NullInteger)

            Dim objTagNodes As XmlNodeList = nodeRoot.SelectNodes("/rss/channel/wp:tag", nsMgr)

            Dim objTagController As New TagController()

            For Each objTagNode As XmlNode In objTagNodes

                Dim objName As XmlNode = objTagNode.SelectSingleNode("wp:tag_name", nsMgr)

                Dim objTag As New TagInfo

                objTag.ModuleID = Me.ModuleId
                objTag.Name = objName.InnerText
                objTag.NameLowered = objName.InnerText.ToLower()

                objTagController.Add(objTag)

            Next

            Dim objTags As ArrayList = objTagController.List(ModuleId, Null.NullInteger)

            Dim objArticleNodes As XmlNodeList = nodeRoot.SelectNodes("/rss/channel/item", nsMgr)

            Dim objArticleController As New ArticleController()

            For Each objArticleNode As XmlNode In objArticleNodes

                Dim objPostType As XmlNode = objArticleNode.SelectSingleNode("wp:post_type", nsMgr)
                Dim objStatus As XmlNode = objArticleNode.SelectSingleNode("wp:status", nsMgr)

                If (objPostType.InnerText = "post" And objStatus.InnerText <> "draft") Then

                    Dim objTitle As XmlNode = objArticleNode.SelectSingleNode("title", nsMgr)

                    Dim objContent As XmlNode = objArticleNode.SelectSingleNode("content:encoded", nsMgr)

                    Dim objPostID As XmlNode = objArticleNode.SelectSingleNode("wp:post_id", nsMgr)
                    Dim objPostDate As XmlNode = objArticleNode.SelectSingleNode("wp:post_date", nsMgr)
                    Dim dt As DateTime = DateTime.Parse(objPostDate.InnerText)

                    Dim objArticle As New ArticleInfo

                    objArticle.Title = objTitle.InnerText
                    objArticle.CreatedDate = dt
                    objArticle.StartDate = dt

                    objArticle.Status = StatusType.Published
                    objArticle.CommentCount = 0
                    objArticle.FileCount = 0
                    objArticle.RatingCount = 0
                    objArticle.Rating = 0
                    objArticle.ShortUrl = ""
                    objArticle.MetaTitle = ""
                    objArticle.MetaDescription = ""
                    objArticle.MetaKeywords = ""
                    objArticle.PageHeadText = ""
                    objArticle.IsFeatured = False
                    objArticle.IsSecure = False
                    objArticle.LastUpdate = DateTime.Now
                    objArticle.LastUpdateID = Me.UserId
                    objArticle.ModuleID = Me.ModuleId
                    objArticle.AuthorID = Me.UserId

                    objArticle.ArticleID = objArticleController.AddArticle(objArticle)

                    Dim objPageController As New PageController()
                    Dim objPage As New PageInfo
                    objPage.PageText = objContent.InnerText
                    objPage.ArticleID = objArticle.ArticleID
                    objPage.Title = objArticle.Title
                    objPageController.AddPage(objPage)

                    Dim objCategoryNodes As XmlNodeList = objArticleNode.SelectNodes("category", nsMgr)

                    For Each objCategoryNode As XmlNode In objCategoryNodes
                        Select Case objCategoryNode.Attributes("domain").InnerText

                            Case "post_tag"

                                For Each objTag As TagInfo In objTags
                                    If (objTag.Name.ToLower() = objCategoryNode.InnerText.ToLower()) Then
                                        objTagController.Add(objArticle.ArticleID, objTag.TagID)
                                        Exit For
                                    End If
                                Next
                                Exit Select

                            Case "category"
                                For Each objCategory As CategoryInfo In objCategories
                                    If (objCategory.Name.ToLower() = objCategoryNode.InnerText.ToLower()) Then
                                        objArticleController.AddArticleCategory(objArticle.ArticleID, objCategory.CategoryID)
                                        Exit For
                                    End If
                                Next
                                Exit Select

                        End Select
                    Next

                    Dim objCommentNodes As XmlNodeList = objArticleNode.SelectNodes("wp:comment", nsMgr)

                    For Each objCommentNode As XmlNode In objCommentNodes

                        Dim objAuthor As XmlNode = objCommentNode.SelectSingleNode("wp:comment_author", nsMgr)
                        Dim objAuthorEmail As XmlNode = objCommentNode.SelectSingleNode("wp:comment_author_email", nsMgr)
                        Dim objAuthorUrl As XmlNode = objCommentNode.SelectSingleNode("wp:comment_author_url", nsMgr)
                        Dim objAuthorIP As XmlNode = objCommentNode.SelectSingleNode("wp:comment_author_IP", nsMgr)
                        Dim objCommentContent As XmlNode = objCommentNode.SelectSingleNode("wp:comment_content", nsMgr)
                        Dim objCommentDate As XmlNode = objCommentNode.SelectSingleNode("wp:comment_date", nsMgr)
                        Dim dtComment As DateTime = DateTime.Parse(objCommentDate.InnerText)

                        Dim objComment As New CommentInfo
                        objComment.ArticleID = objArticle.ArticleID
                        objComment.UserID = Null.NullInteger
                        objComment.AnonymousName = objAuthor.InnerText
                        objComment.AnonymousEmail = objAuthorEmail.InnerText
                        objComment.AnonymousURL = objAuthorUrl.InnerText
                        objComment.Comment = FilterInput(objCommentContent.InnerText)
                        objComment.RemoteAddress = objAuthorIP.InnerText
                        objComment.NotifyMe = False
                        objComment.Type = 0
                        objComment.IsApproved = True
                        objComment.ApprovedBy = Me.UserId
                        objComment.CreatedDate = dtComment

                        Dim objCommentController As New CommentController
                        objComment.CommentID = objCommentController.AddComment(objComment)

                        objArticle.CommentCount = objArticle.CommentCount + 1
                        objArticleController.UpdateArticle(objArticle)
                    Next

                End If

            Next

        End Sub

        Private Function FilterInput(ByVal stringToFilter As String) As String

            Dim objPortalSecurity As New PortalSecurity

            stringToFilter = objPortalSecurity.InputFilter(stringToFilter, PortalSecurity.FilterFlag.NoScripting)

            stringToFilter = Replace(stringToFilter, Chr(13), "")
            stringToFilter = Replace(stringToFilter, ControlChars.Lf, "<br />")

            Return stringToFilter

        End Function

    End Class

End Namespace