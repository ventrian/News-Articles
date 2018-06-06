'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Security.Roles
Imports Ventrian.NewsArticles.Components.Social

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucApproveArticles
        Inherits NewsArticleModuleBase

#Region " Private Properties "

        Private ReadOnly Property CurrentPage() As Integer
            Get
                If (Request("Page") = Null.NullString And Request("CurrentPage") = Null.NullString) Then
                    Return 1
                Else
                    Try
                        If (Request("Page") <> Null.NullString) Then
                            Return Convert.ToInt32(Request("Page"))
                        Else
                            Return Convert.ToInt32(Request("CurrentPage"))
                        End If
                    Catch
                        Return 1
                    End Try
                End If
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Function IsInRole(ByVal roleName As String, ByVal roles As String()) As Boolean

            For Each role As String In roles
                If (roleName = role) Then
                    Return True
                End If
            Next

            Return False

        End Function

        Private Sub BindArticles()

            Dim count As Integer = 0

            Dim objArticleController As New ArticleController

            DotNetNuke.Services.Localization.Localization.LocalizeDataGrid(grdArticles, Me.LocalResourceFile)

            grdArticles.DataSource = objArticleController.GetArticleList(Me.ModuleId, Null.NullDate, Null.NullDate, Nothing, Null.NullBoolean, Nothing, Null.NullInteger, CurrentPage, 20, "StartDate", "DESC", False, Null.NullBoolean, Null.NullString, Null.NullInteger, True, True, Null.NullBoolean, Null.NullBoolean, False, False, Null.NullString, Nothing, False, Null.NullString, Null.NullInteger, Null.NullString, Null.NullString, count)
            grdArticles.DataBind()

            If (grdArticles.Items.Count = 0) Then
                lblNoArticles.Visible = True
                lblNoArticles.Text = DotNetNuke.Services.Localization.Localization.GetString("NoArticlesMessage.Text", LocalResourceFile)
                grdArticles.Visible = False

                ctlPagingControl.Visible = False
            Else
                lblNoArticles.Visible = False
                grdArticles.Visible = True

                ctlPagingControl.Visible = True
                ctlPagingControl.TotalRecords = count
                ctlPagingControl.PageSize = 20
                ctlPagingControl.CurrentPage = CurrentPage
                ctlPagingControl.QuerystringParams = GetParams()
                ctlPagingControl.TabID = TabId
                ctlPagingControl.EnableViewState = False
            End If

        End Sub

        Private Function GetParams() As String

            Dim params As String = ""

            If (Request("ctl") <> "") Then
                If (Request("ctl").ToLower = "approvearticles") Then
                    params += "ctl=" & Request("ctl") & "&mid=" & ModuleId.ToString()
                End If
            End If

            If (Request("articleType") <> "") Then
                If (Request("articleType").ToString().ToLower = "approvearticles") Then
                    params += "articleType=" & Request("articleType")
                End If
            End If

            Return params

        End Function

        Private Sub NotifyAuthor(ByVal objArticle As ArticleInfo)

            If (Settings.Contains(ArticleConstants.NOTIFY_APPROVAL_SETTING)) Then
                If (Convert.ToBoolean(Settings(ArticleConstants.NOTIFY_APPROVAL_SETTING))) Then
                    Dim objUserController As New UserController
                    Dim objUser As UserInfo = objUserController.GetUser(Me.PortalId, objArticle.AuthorID)

                    Dim objEmailTemplateController As New EmailTemplateController
                    If Not (objUser Is Nothing) Then
                        objEmailTemplateController.SendFormattedEmail(Me.ModuleId, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False), objArticle, EmailTemplateType.ArticleApproved, objUser.Email, ArticleSettings)
                    End If

                End If
            End If

        End Sub

        Private Sub CheckSecurity()

            If (Request.IsAuthenticated = False) Then
                Response.Redirect(Common.GetModuleLink(Me.TabId, Me.ModuleId, "NotAuthenticated", ArticleSettings), True)
            End If

            If (ArticleSettings.IsApprover) Then
                Return
            End If

            Response.Redirect(Common.GetModuleLink(Me.TabId, Me.ModuleId, "NotAuthorized", ArticleSettings), True)

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function GetAdjustedCreateDate(ByVal objItem As Object) As String

            Dim objArticle As ArticleInfo = CType(objItem, ArticleInfo)
            Return objArticle.CreatedDate.ToString("d") & " " & objArticle.CreatedDate.ToString("t")

        End Function

        Protected Function GetAdjustedPublishDate(ByVal objItem As Object) As String

            Dim objArticle As ArticleInfo = CType(objItem, ArticleInfo)
            Return objArticle.StartDate.ToString("d") & " " & objArticle.StartDate.ToString("t")

        End Function

        Protected Function GetArticleLink(ByVal objItem As Object) As String

            Dim objArticle As ArticleInfo = CType(objItem, ArticleInfo)
            Return Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False)

        End Function

        Protected Function GetEditUrl(ByVal articleID As String) As String
            If (ArticleSettings.LaunchLinks) Then
                Return Common.GetModuleLink(Me.TabId, Me.ModuleId, "Edit", ArticleSettings, "ArticleID=" & articleID, "returnurl=" & Server.UrlEncode(Request.Url.ToString()))
            Else
                Return Common.GetModuleLink(Me.TabId, Me.ModuleId, "SubmitNews", ArticleSettings, "ArticleID=" & articleID, "returnurl=" & Server.UrlEncode(Request.Url.ToString()))
            End If
        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                CheckSecurity()

                If IsPostBack = False Then
                    BindArticles()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdApproveSelected_OnClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApproveSelected.Click

            Try

                Dim objArticleController As New ArticleController

                For i As Integer = 0 To grdArticles.Items.Count - 1

                    Dim currentItem As DataGridItem = grdArticles.Items(i)

                    If Not (currentItem.FindControl("chkArticle") Is Nothing) Then
                        Dim chkArticle As CheckBox = CType(currentItem.FindControl("chkArticle"), CheckBox)

                        If (chkArticle.Checked) Then
                            Dim objArticle As ArticleInfo = objArticleController.GetArticle(Convert.ToInt32(grdArticles.DataKeys(i)))

                            objArticle.Status = StatusType.Published
                            objArticleController.UpdateArticle(objArticle)

                            NotifyAuthor(objArticle)

                            If (ArticleSettings.EnableAutoTrackback) Then
                                Dim objNotifications As New Tracking.Notification
                                objNotifications.NotifyExternalSites(objArticle, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False), Me.PortalSettings.PortalName)
                            End If

                            If (ArticleSettings.EnableNotificationPing) Then
                                Dim objNotifications As New Tracking.Notification
                                objNotifications.NotifyWeblogs(AddHTTP(NavigateURL()), Me.PortalSettings.PortalName)
                            End If

                            If (ArticleSettings.JournalIntegration) Then
                                Dim objJournal As New Journal
                                objJournal.AddArticleToJournal(objArticle, PortalId, TabId, Me.UserId, Null.NullInteger, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False))
                            End If

                            If (ArticleSettings.JournalIntegrationGroups) Then

                                Dim objCategories As ArrayList = objArticleController.GetArticleCategories(objArticle.ArticleID)

                                If (objCategories.Count > 0) Then

                                    Dim objRoleController As New RoleController()

                                    Dim objRoles As ArrayList = objRoleController.GetRoles()
                                    For Each objRole As RoleInfo In objRoles
                                        Dim roleAccess As Boolean = False

                                        If (objRole.SecurityMode = SecurityMode.SocialGroup Or objRole.SecurityMode = SecurityMode.Both) Then

                                            For Each objCategory As CategoryInfo In objCategories

                                                If (objCategory.InheritSecurity = False) Then

                                                    If (objCategory.CategorySecurityType = CategorySecurityType.Loose) Then
                                                        roleAccess = False
                                                        Exit For
                                                    Else
                                                        If (Settings.Contains(objCategory.CategoryID.ToString() & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                                            If (IsInRole(objRole.RoleName, Settings(objCategory.CategoryID.ToString() & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString().Split(";"c))) Then
                                                                roleAccess = True
                                                            End If
                                                        End If
                                                    End If

                                                End If

                                            Next

                                        End If

                                        If (roleAccess) Then
                                            Dim objJournal As New Journal
                                            objJournal.AddArticleToJournal(objArticle, PortalId, TabId, Me.UserId, objRole.RoleID, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False))
                                        End If

                                    Next

                                End If

                            End If

                            If (ArticleSettings.EnableSmartThinkerStoryFeed) Then
                                Dim objStoryFeed As New wsStoryFeed.StoryFeedWS
                                objStoryFeed.Url = DotNetNuke.Common.Globals.AddHTTP(Request.ServerVariables("HTTP_HOST") & Me.ResolveUrl("~/DesktopModules/Smart-Thinker%20-%20UserProfile/StoryFeed.asmx"))

                                Dim val As String = GetSharedResource("StoryFeed-AddArticle")

                                val = val.Replace("[AUTHOR]", objArticle.AuthorDisplayName)
                                val = val.Replace("[AUTHORID]", objArticle.AuthorID.ToString())
                                val = val.Replace("[ARTICLELINK]", Common.GetArticleLink(objArticle, Me.PortalSettings.ActiveTab, ArticleSettings, False))
                                val = val.Replace("[ARTICLETITLE]", objArticle.Title)

                                Try
                                    objStoryFeed.AddAction(80, objArticle.ArticleID, val, objArticle.AuthorID, "VE6457624576460436531768")
                                Catch
                                End Try
                            End If

                            If (ArticleSettings.EnableActiveSocialFeed) Then
                                If (ArticleSettings.ActiveSocialSubmitKey <> "") Then
                                    If IO.File.Exists(HttpContext.Current.Server.MapPath("~/bin/active.modules.social.dll")) Then
                                        Dim ai As Object = Nothing
                                        Dim asm As System.Reflection.Assembly
                                        Dim ac As Object = Nothing
                                        Try
                                            asm = System.Reflection.Assembly.Load("Active.Modules.Social")
                                            ac = asm.CreateInstance("Active.Modules.Social.API.Journal")
                                            If Not ac Is Nothing Then
                                                ac.AddProfileItem(New Guid(ArticleSettings.ActiveSocialSubmitKey), objArticle.AuthorID, Common.GetArticleLink(objArticle, Me.PortalSettings.ActiveTab, ArticleSettings, False), objArticle.Title, objArticle.Summary, objArticle.Body, 1, "")
                                            End If
                                        Catch ex As Exception
                                        End Try
                                    End If
                                End If
                            End If

                        End If

                    End If

                Next

                Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "ApproveArticles", ArticleSettings), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdApproveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApproveAll.Click

            Try

                Dim objArticleController As New ArticleController

                For i As Integer = 0 To grdArticles.Items.Count - 1

                    Dim currentItem As DataGridItem = grdArticles.Items(i)
                    Dim objArticle As ArticleInfo = objArticleController.GetArticle(Convert.ToInt32(grdArticles.DataKeys(i)))

                    objArticle.Status = StatusType.Published
                    objArticleController.UpdateArticle(objArticle)

                    NotifyAuthor(objArticle)

                    If (ArticleSettings.EnableAutoTrackback) Then
                        Dim objNotifications As New Tracking.Notification
                        objNotifications.NotifyExternalSites(objArticle, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False), Me.PortalSettings.PortalName)
                    End If

                    If (ArticleSettings.EnableNotificationPing) Then
                        Dim objNotifications As New Tracking.Notification
                        objNotifications.NotifyWeblogs(AddHTTP(NavigateURL()), Me.PortalSettings.PortalName)
                    End If

                Next

                Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "ApproveArticles", ArticleSettings), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace
