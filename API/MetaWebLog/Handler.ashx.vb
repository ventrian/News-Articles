Imports System.Web
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Common.Utilities
Imports System.Linq
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Services.Log.EventLog
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Entities.Users
Imports System.IO
Imports DotNetNuke.Security.Membership
Imports DotNetNuke.Entities.Tabs

Namespace Ventrian.NewsArticles.API.MetaWebLog

    Public Class Handler
        Implements IHttpHandler

        ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

        Private ReadOnly Property PortalSettings() As PortalSettings
            Get
                Return PortalController.GetCurrentPortalSettings()
            End Get
        End Property

        Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            Dim methodName As String = ""
            Try
                Dim input As New XmlrpcRequest(context)
                Dim output As New XmlrpcResponse(input.MethodName)

                Dim objLoginStatus As UserLoginStatus
                UserController.ValidateUser(PortalSettings.PortalId, input.UserName, input.Password, "", PortalSettings.PortalName, context.Request.UserHostAddress, objLoginStatus)

                If (objLoginStatus = UserLoginStatus.LOGIN_SUCCESS Or objLoginStatus = UserLoginStatus.LOGIN_SUPERUSER) = False Then
                    Throw New MetaException("10", "Unauthorized")
                End If

                methodName = input.MethodName

                Select Case methodName
                    Case "metaWeblog.newPost"
                        output.PostId = NewPost(input.BlogId, input.UserName, input.Password, input.Post, input.Publish)
                        Exit Select
                    Case "metaWeblog.editPost"
                        output.Completed = EditPost(input.BlogId, input.PostId, input.UserName, input.Password, input.Post, input.Publish)
                        Exit Select
                    Case "metaWeblog.getPost"
                        output.Post = GetPost(input.PostId, input.UserName, input.Password)
                        Exit Select
                    Case "metaWeblog.newMediaObject"
                        output.MediaUrlInfo = NewMediaObject(input.BlogId, input.UserName, input.Password, input.MediaObject, context)
                        Exit Select
                    Case "metaWeblog.getCategories"
                        output.Categories = GetCategories(input.BlogId, input.UserName, input.Password)
                        Exit Select
                    Case "metaWeblog.getRecentPosts"
                        output.Posts = GetRecentPosts(input.BlogId, input.UserName, input.Password, input.NumberOfPosts)
                        Exit Select
                    Case "blogger.getUsersBlogs", "metaWeblog.getUsersBlogs"
                        output.Blogs = GetUserBlogs(input.AppKey, input.UserName, input.Password)
                        Exit Select
                    Case "blogger.deletePost"
                        output.Completed = DeletePost(input.AppKey, input.PostId, input.UserName, input.Password)
                        Exit Select
                    Case "blogger.getUserInfo"
                        ' Not implemented.  Not planned.
                        Throw New MetaException("10", "The method GetUserInfo is not implemented.")
                    Case "wp.getTags"
                        output.Keywords = GetKeywords(input.BlogId, input.UserName, input.Password)
                        Exit Select
                End Select

                output.Response(context)
            Catch mex As MetaException
                Dim objEventLog As New EventLogController
                objEventLog.AddLog(methodName, mex.Message, PortalSettings, -1, EventLogController.EventLogType.ADMIN_ALERT)
                Dim output = New XmlrpcResponse("fault")
                Dim fault = New MetaFaultInfo() With { _
                    .FaultCode = mex.Code, _
                    .FaultString = mex.Message _
                }
                output.Fault = fault
                output.Response(context)
            Catch ex As Exception
                ' Log DNN Event
                Dim objEventLog As New EventLogController
                objEventLog.AddLog(methodName, ex.Message + ex.StackTrace, PortalSettings, -1, EventLogController.EventLogType.ADMIN_ALERT)

                ' Raise RPC Fault
                Dim outputFault = New XmlrpcResponse("fault")
                Dim fault = New MetaFaultInfo() With { _
                    .FaultCode = "0", _
                    .FaultString = ex.StackTrace _
                }
                outputFault.Fault = fault
                outputFault.Response(context)
            End Try

        End Sub

        Private Shared Function CheckPermission(permission As String, moduleId As Integer, user As UserInfo)

            Dim check As Boolean = False

            Dim objModuleController As New ModuleController
            Dim settings As Hashtable = objModuleController.GetModuleSettings(moduleId)

            If (user.IsSuperUser) Then
                Return True
            End If

            If (settings.Contains(permission)) Then

                If settings(permission).ToString().Split(New Char() {";"c}).Any(Function(role) user.IsInRole(role)) Then
                    check = True
                End If

            End If

            Return check

        End Function

        Private Shared Function IsAuthor(moduleId As Integer, user As UserInfo)

            Return CheckPermission(ArticleConstants.PERMISSION_SUBMISSION_SETTING, moduleId, user)

        End Function

        Private Shared Function IsAutoApprover(moduleId As Integer, user As UserInfo)

            Return CheckPermission(ArticleConstants.PERMISSION_AUTO_APPROVAL_SETTING, moduleId, user)

        End Function

        Private Shared Function IsApprover(moduleId As Integer, user As UserInfo)

            Return CheckPermission(ArticleConstants.PERMISSION_APPROVAL_SETTING, moduleId, user)

        End Function

        Friend Function DeletePost(appKey As String, postId As String, userName As String, password As String) As Boolean

            Try

                Dim objArticleController As New ArticleController()
                Dim objArticle As ArticleInfo = objArticleController.GetArticle(postId)

                If (objArticle Is Nothing) Then
                    Throw New MetaException("01", "Article Not Found")
                End If

                Dim objUser As UserInfo = UserController.GetUserByName(PortalSettings.PortalId, userName)

                If (objUser Is Nothing) Then
                    Throw New MetaException("01", "Username not found")
                End If

                If (IsAuthor(objArticle.ModuleID, objUser) = False) Then
                    Throw New MetaException("01", "You do not have permission to delete posts")
                End If

                If (objUser.UserID <> objArticle.AuthorID) Then
                    If (IsApprover(objArticle.ModuleID, objUser) = False) Then
                        Throw New MetaException("01", "You do not have permission to delete other's posts")
                    End If
                End If

                objArticleController.DeleteArticle(postId)

            Catch ex As Exception
                Throw New MetaException("12", String.Format("DeletePost failed.  Error: {0}", ex.Message))
            End Try

            Return True

        End Function

        Friend Function EditPost(blogId As Integer, postId As String, userName As String, password As String, sentPost As MetaPostInfo, publish As Boolean) As Boolean

            Dim objArticleController As New ArticleController()
            Dim objArticle As ArticleInfo = objArticleController.GetArticle(postId)

            If (objArticle Is Nothing) Then
                Throw New MetaException("01", "Article Not Found")
            End If

            Dim objUser As UserInfo = UserController.GetUserByName(PortalSettings.PortalId, userName)

            If (objUser Is Nothing) Then
                Throw New MetaException("01", "Username not found")
            End If

            If (IsAuthor(objArticle.ModuleID, objUser) = False) Then
                Throw New MetaException("01", "You do not have permission to edit posts")
            End If

            If (objUser.UserID <> objArticle.AuthorID) Then
                If (IsApprover(objArticle.ModuleID, objUser) = False) Then
                    Throw New MetaException("01", "You do not have permission to edit other's posts")
                End If
            End If

            objArticle.Title = sentPost.Title
            objArticle.LastUpdate = DateTime.Now
            objArticle.LastUpdateID = objUser.UserID

            objArticleController.UpdateArticle(objArticle)

            Dim objPageController As PageController = New PageController
            Dim pages As ArrayList = objPageController.GetPageList(objArticle.ArticleID)

            If (pages.Count > 0) Then
                Dim objPage As PageInfo = pages(0)
                objPage.PageText = sentPost.Description
                objPageController.UpdatePage(objPage)
            End If

            objArticleController.DeleteArticleCategories(objArticle.ArticleID)

            Dim objCategoryController As New CategoryController()
            Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(objArticle.ModuleID)

            For Each item As String In sentPost.Categories.Where(Function(c) c IsNot Nothing AndAlso c.Trim() <> String.Empty)

                Dim categoryId As Integer = Null.NullInteger

                For Each objCategory As CategoryInfo In objCategories
                    If (objCategory.Name.ToLower() = item.ToLower()) Then
                        categoryId = objCategory.CategoryID
                        Exit For
                    End If
                Next

                If (categoryId <> Null.NullInteger) Then
                    objArticleController.AddArticleCategory(objArticle.ArticleID, categoryId)
                End If

            Next

            Dim objTagController As New TagController
            objTagController.DeleteArticleTag(objArticle.ArticleID)

            For Each itemTag As String In sentPost.Tags.Where(Function(item) item IsNot Nothing AndAlso item.Trim() <> String.Empty)
                Dim objTag As TagInfo = objTagController.Get(objArticle.ModuleID, itemTag)

                If (objTag Is Nothing) Then
                    objTag = New TagInfo
                    objTag.Name = itemTag
                    objTag.NameLowered = itemTag.ToLower()
                    objTag.ModuleID = objArticle.ModuleID
                    objTag.TagID = objTagController.Add(objTag)
                End If

                objTagController.Add(objArticle.ArticleID, objTag.TagID)
            Next

            Return True
        End Function

        Friend Function GetCategories(blogId As String, userName As String, password As String) As List(Of MetaCategoryInfo)

            Dim objModuleController As New ModuleController()
            Dim objTabModule As ModuleInfo = objModuleController.GetTabModule(blogId)

            Dim objCategoryController As New CategoryController()
            Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(objTabModule.ModuleID)
            Dim objMetaCategories As New List(Of MetaCategoryInfo)

            For Each objCategory As CategoryInfo In objCategories
                Dim objMetaCategory As New MetaCategoryInfo
                objMetaCategory.Title = objCategory.Name
                objMetaCategory.Description = objCategory.Description
                objMetaCategory.HtmlUrl = "#"
                objMetaCategory.RssUrl = "#"
                objMetaCategories.Add(objMetaCategory)
            Next

            Return objMetaCategories

        End Function

        Friend Function GetKeywords(blogId As String, userName As String, password As String) As List(Of String)

            Dim objModuleController As New ModuleController()
            Dim objTabModule As ModuleInfo = objModuleController.GetTabModule(blogId)

            Dim objTagController As New TagController
            Dim objTags As ArrayList = objTagController.List(objTabModule.ModuleID, 1000)

            Return (From objTag As TagInfo In objTags Where (objTag.Name.Trim() <> "") Select objTag.Name).ToList()

        End Function

        Friend Function GetPost(postId As String, userName As String, password As String) As MetaPostInfo

            Dim objArticleController As New ArticleController()
            Dim objArticle As ArticleInfo = objArticleController.GetArticle(postId)

            If (objArticle Is Nothing) Then
                Throw New MetaException("01", "Article Not Found")
            End If

            Dim sendPost As New MetaPostInfo()

            sendPost.PostId = objArticle.ArticleID.ToString()
            sendPost.PostDate = objArticle.StartDate
            sendPost.Title = objArticle.Title
            sendPost.Description = HttpUtility.HtmlDecode(objArticle.Body)
            sendPost.Link = objArticle.Url
            sendPost.Excerpt = objArticle.Summary
            sendPost.Publish = (objArticle.Status = StatusType.Published)

            Dim objArticleCategories As ArrayList = objArticleController.GetArticleCategories(objArticle.ArticleID)
            sendPost.Categories = (From objCategory As CategoryInfo In objArticleCategories Select objCategory.Name).ToList()
            sendPost.Tags = objArticle.Tags.Split(","c).ToList()

            Return sendPost
        End Function

        Friend Function GetRecentPosts(blogId As String, userName As String, password As String, numberOfPosts As Integer) As List(Of MetaPostInfo)

            Dim objUser As UserInfo = UserController.GetUserByName(PortalSettings.PortalId, userName)

            If (objUser Is Nothing) Then
                Throw New MetaException("01", "Username not found")
            End If

            Dim objModuleController As New ModuleController()
            Dim objTabModule As ModuleInfo = objModuleController.GetTabModule(blogId)

            Dim authorId As Integer = objUser.UserID
            If (IsApprover(objTabModule.ModuleID, objUser)) Then
                authorId = Null.NullInteger
            End If

            Dim sendPosts As New List(Of MetaPostInfo)()

            Dim objArticleController As New ArticleController
            Dim objArticles As List(Of ArticleInfo) = objArticleController.GetArticleList(objTabModule.ModuleID, DateTime.Now, Null.NullDate, Nothing, False, Nothing, numberOfPosts, 1, numberOfPosts, "StartDate", "DESC", True, False, Null.NullString, authorId, False, False, False, False, False, False, Null.NullString, Nothing, False, Null.NullString, Null.NullInteger, Null.NullString, Null.NullString, Nothing)

            For Each objArticle As ArticleInfo In objArticles

                Dim tempPost As New MetaPostInfo() With { _
                    .PostId = objArticle.ArticleID, _
                    .PostDate = DateTime.Now, _
                    .Title = objArticle.Title, _
                    .Description = objArticle.ArticleText, _
                    .Link = objArticle.Url, _
                    .Excerpt = objArticle.Summary, _
                    .Publish = (objArticle.Status = StatusType.Published) _
                }

                Dim objArticleCategories As ArrayList = objArticleController.GetArticleCategories(objArticle.ArticleID)
                tempPost.Categories = (From objCategory As CategoryInfo In objArticleCategories Select objCategory.Name).ToList()
                tempPost.Tags = objArticle.Tags.Split(","c).ToList()

                sendPosts.Add(tempPost)
            Next

            Return sendPosts

        End Function

        Friend Function GetUserBlogs(appKey As String, userName As String, password As String) As List(Of MetaBlogInfo)

            Dim blogs As New List(Of MetaBlogInfo)()
            For Each objModule As ModuleInfo In Common.GetArticleModules(PortalSettings.PortalId)
                Dim modulePath As String = objModule.ParentTab.TabPath.Replace("//", "/")
                If (modulePath.Length > 1 And modulePath.Substring(0, 1) = "/") Then
                    modulePath = modulePath.Substring(1)
                End If
                Dim temp As New MetaBlogInfo() With { _
                    .Url = DotNetNuke.Common.Globals.NavigateURL(objModule.TabID), _
                    .BlogId = objModule.TabModuleID, _
                    .BlogName = objModule.ModuleTitle + " (" + modulePath + ")" _
                }
                blogs.Add(temp)
            Next

            Return blogs

        End Function

        Friend Function NewMediaObject(blogId As String, userName As String, password As String, mediaObject As MetaMediaInfo, request As HttpContext) As MetaMediaUrlInfo

            Dim objUser As UserInfo = UserController.GetUserByName(PortalSettings.PortalId, userName)

            If (objUser Is Nothing) Then
                Throw New MetaException("01", "Username not found")
            End If

            Dim folders As IFolderManager = FolderManager.Instance()
            Dim files As IFileManager = FileManager.Instance

            Dim userFolder = folders.GetUserFolder(objUser)
            Dim ms As New MemoryStream(mediaObject.Bits)
            Dim fileInfo As DotNetNuke.Services.FileSystem.FileInfo = files.AddFile(userFolder, mediaObject.Name.Replace(" ", "_").Replace(":", "-"), ms, True)
            ms.Close()

            Dim mediaInfo As New MetaMediaUrlInfo()
            mediaInfo.Url = FileManager.Instance.GetUrl(fileInfo)

            Return mediaInfo

        End Function

        Friend Function NewPost(blogId As String, userName As String, password As String, sentPost As MetaPostInfo, publish As Boolean) As String

            Dim objModuleController As New ModuleController()
            Dim objTabModule As ModuleInfo = objModuleController.GetTabModule(blogId)

            If (objTabModule Is Nothing) Then
                Throw New MetaException("11", "BlogID not found.")
            End If

            Dim objTabController As New TabController()
            Dim objTab As TabInfo = objTabController.GetTab(objTabModule.TabID, PortalSettings.PortalId, False)

            Dim objUser As UserInfo = UserController.GetUserByName(PortalSettings.PortalId, userName)

            If (objUser Is Nothing) Then
                Throw New MetaException("01", "Username not found")
            End If

            Dim author As Boolean = IsAuthor(objTabModule.ModuleID, objUser)
            Dim autoApprove As Boolean = IsAutoApprover(objTabModule.ModuleID, objUser)
            Dim approver As Boolean = IsApprover(objTabModule.ModuleID, objUser)

            If (author = False) Then
                Throw New MetaException("01", "You do not have permission to post articles")
            End If

            Dim objSettings As Hashtable = objModuleController.GetModuleSettings(objTabModule.ModuleID)
            Dim objArticleSettings As New ArticleSettings(objSettings, PortalSettings, objTabModule)

            Dim objArticle As New ArticleInfo

            objArticle.ModuleID = objTabModule.ModuleID
            objArticle.Title = sentPost.Title
            objArticle.ArticleText = HttpUtility.HtmlEncode(sentPost.Description)
            objArticle.AuthorID = objUser.UserID
            objArticle.CreatedDate = DateTime.Now
            objArticle.StartDate = Null.NullDate
            objArticle.EndDate = Null.NullDate
            objArticle.LastUpdate = DateTime.Now
            objArticle.LastUpdateID = objUser.UserID
            objArticle.Summary = HttpUtility.HtmlEncode(sentPost.Excerpt)

            objArticle.CommentCount = 0
            objArticle.RatingCount = 0
            objArticle.FileCount = 0
            objArticle.Rating = 0
            objArticle.ShortUrl = ""

            If (publish) Then
                objArticle.Status = StatusType.Published
                objArticle.StartDate = DateTime.Now
                If (approver = False) Then
                    If (autoApprove = False) Then
                        objArticle.Status = StatusType.AwaitingApproval

                        If (objSettings.Contains(ArticleConstants.NOTIFY_SUBMISSION_SETTING)) Then
                            If (Convert.ToBoolean(objSettings(ArticleConstants.NOTIFY_SUBMISSION_SETTING)) = True) Then
                                Dim objEmailTemplateController As New EmailTemplateController
                                Dim emails As String = objEmailTemplateController.GetApproverDistributionList(objTabModule.ModuleID)

                                For Each email As String In emails.Split(Convert.ToChar(";"))
                                    If (email <> "") Then
                                        objEmailTemplateController.SendFormattedEmail(objTabModule.ModuleID, Common.GetArticleLink(objArticle, objTab, objArticleSettings, False), objArticle, EmailTemplateType.ArticleSubmission, email, objArticleSettings)
                                    End If
                                Next
                            End If
                        End If

                        If (objSettings.Contains(ArticleConstants.NOTIFY_SUBMISSION_SETTING_EMAIL)) Then
                            If (objSettings(ArticleConstants.NOTIFY_SUBMISSION_SETTING_EMAIL).ToString() <> "") Then
                                Dim objEmailTemplateController As New EmailTemplateController
                                For Each email As String In objSettings(ArticleConstants.NOTIFY_SUBMISSION_SETTING_EMAIL).ToString().Split(","c)
                                    objEmailTemplateController.SendFormattedEmail(objTabModule.ModuleID, Common.GetArticleLink(objArticle, objTab, objArticleSettings, False), objArticle, EmailTemplateType.ArticleSubmission, email, objArticleSettings)
                                Next
                            End If
                        End If

                    End If
                End If
            Else
                objArticle.Status = StatusType.Draft
            End If

            Dim objArticleController As New ArticleController()
            objArticle.ArticleID = objArticleController.AddArticle(objArticle)

            Dim objPage As New PageInfo
            objPage.PageText = HttpUtility.HtmlEncode(sentPost.Description)
            objPage.ArticleID = objArticle.ArticleID
            objPage.Title = sentPost.Title

            Dim objPageController As New PageController()
            objPageController.AddPage(objPage)

            Return objArticle.ArticleID.ToString()

        End Function

    End Class

End Namespace