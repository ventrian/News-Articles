Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports Ventrian.NewsArticles.Components.Social

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucApproveComments
        Inherits NewsArticleModuleBase

#Region " Private Methods "

        Private Sub BindComments()

            Dim objCommentController As New CommentController
            rptApproveComments.DataSource = objCommentController.GetCommentList(Me.ModuleId, Null.NullInteger, False, SortDirection.Ascending, Null.NullInteger)
            rptApproveComments.DataBind()

            If (rptApproveComments.Items.Count = 0) Then
                rptApproveComments.Visible = False
                lblNoComments.Visible = True
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

        Private Sub NotifyAuthor(ByVal objComment As CommentInfo)

            Dim objArticleController As New ArticleController
            Dim objArticle As ArticleInfo = objArticleController.GetArticle(objComment.ArticleID)

            If Not (objArticle Is Nothing) Then
                Dim objEmailTemplateController As New EmailTemplateController

                Try
                    ' Don't send it to the author if it's their own comment.
                    If (objArticle.AuthorID <> objComment.UserID) Then
                        objEmailTemplateController.SendFormattedEmail(Me.ModuleId, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False), objArticle, objComment, EmailTemplateType.CommentNotification, ArticleSettings)
                    End If
                Catch ex As Exception
                    Dim objEventLog As New DotNetNuke.Services.Log.EventLog.EventLogController

                    Dim objUserController As New DotNetNuke.Entities.Users.UserController
                    Dim objUser As DotNetNuke.Entities.Users.UserInfo = objUserController.GetUser(Me.PortalId, objArticle.AuthorID)

                    Dim sendTo As String = ""
                    If Not (objUser Is Nothing) Then
                        sendTo = objUser.Email
                    End If
                    objEventLog.AddLog("News Articles Email Failure", "Failure to send [Author Comment] to '" & sendTo & "' from '" & Me.PortalSettings.Email, PortalSettings, -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)
                End Try

            End If

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function GetAuthor(ByVal obj As Object) As String

            Dim objComment As CommentInfo = CType(obj, CommentInfo)
            If Not (objComment Is Nothing) Then
                If (objComment.UserID <> Null.NullInteger) Then
                    Return objComment.AuthorUserName
                Else
                    Return objComment.AnonymousName
                End If
            Else
                Return ""
            End If

        End Function

        Protected Function GetArticleUrl(ByVal obj As Object) As String

            Dim objComment As CommentInfo = CType(obj, CommentInfo)
            If Not (objComment Is Nothing) Then
                Dim objArticleController As New ArticleController
                Dim objArticle As ArticleInfo = objArticleController.GetArticle(objComment.ArticleID)

                If (objArticle IsNot Nothing) Then
                    Return Common.GetArticleLink(objArticle, Me.PortalSettings.ActiveTab, Me.ArticleSettings, False)
                End If
            End If

            Return ""

        End Function

        Protected Function GetEditCommentUrl(ByVal commentID As String) As String

            Return Common.GetModuleLink(TabId, ModuleId, "EditComment", ArticleSettings, "CommentID=" & commentID, "ReturnUrl=" & Server.UrlEncode(Request.RawUrl))

        End Function

        Protected Function GetTitle(ByVal obj As Object) As String

            Dim objComment As CommentInfo = CType(obj, CommentInfo)
            If Not (objComment Is Nothing) Then

                Dim objArticleController As New ArticleController
                Dim objArticle As ArticleInfo = objArticleController.GetArticle(objComment.ArticleID)

                If (objArticle IsNot Nothing) Then
                    Return objArticle.Title
                End If
            End If

            Return ""

        End Function

        Protected Function GetWebsite(ByVal obj As Object) As String

            Dim objComment As CommentInfo = CType(obj, CommentInfo)
            If Not (objComment Is Nothing) Then
                If (objComment.AnonymousURL <> "") Then
                    Return "<a href='" & DotNetNuke.Common.AddHTTP(objComment.AnonymousURL) & "' target='_blank'>" & objComment.AnonymousURL & "</a>"
                End If
            End If
            Return ""

        End Function

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                CheckSecurity()

                If (Page.IsPostBack = False) Then
                    BindComments()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub rptApproveComments_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptApproveComments.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objComment As CommentInfo = CType(e.Item.DataItem, CommentInfo)
                If (objComment IsNot Nothing) Then
                    Dim chkSelected As CheckBox = CType(e.Item.FindControl("chkSelected"), CheckBox)
                    chkSelected.Attributes.Add("CommentID", objComment.CommentID.ToString())
                End If

            End If

        End Sub

        Protected Sub cmdApprove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApprove.Click

            For Each item As RepeaterItem In rptApproveComments.Items
                If (item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem) Then
                    Dim chkSelected As CheckBox = CType(item.FindControl("chkSelected"), CheckBox)
                    If Not (chkSelected Is Nothing) Then
                        If (chkSelected.Checked) Then
                            Dim commentID As Integer = Convert.ToInt32(chkSelected.Attributes("CommentID").ToString())
                            Dim objCommentController As New CommentController()
                            Dim objComment As CommentInfo = objCommentController.GetComment(commentID)
                            If Not (objComment Is Nothing) Then
                                objComment.IsApproved = True
                                objComment.ApprovedBy = Me.UserId
                                objCommentController.UpdateComment(objComment)

                                Dim objEmailTemplateController As New EmailTemplateController()
                                If (ArticleSettings.NotifyAuthorOnApproval) Then
                                    NotifyAuthor(objComment)
                                End If

                                Dim objArticleController As New ArticleController
                                Dim objArticle As ArticleInfo = objArticleController.GetArticle(objComment.ArticleID)

                                If Not (objArticle Is Nothing) Then

                                    If (ArticleSettings.EnableActiveSocialFeed And objComment.UserID <> Null.NullInteger) Then
                                        If (ArticleSettings.ActiveSocialCommentKey <> "") Then
                                            If IO.File.Exists(HttpContext.Current.Server.MapPath("~/bin/active.modules.social.dll")) Then
                                                Dim ai As Object = Nothing
                                                Dim asm As System.Reflection.Assembly
                                                Dim ac As Object = Nothing
                                                Try
                                                    asm = System.Reflection.Assembly.Load("Active.Modules.Social")
                                                    ac = asm.CreateInstance("Active.Modules.Social.API.Journal")
                                                    If Not ac Is Nothing Then
                                                        ac.AddProfileItem(New Guid(ArticleSettings.ActiveSocialCommentKey), objComment.UserID, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False), objArticle.Title, objComment.Comment, objComment.Comment, 1, "")
                                                    End If
                                                Catch ex As Exception
                                                End Try
                                            End If
                                        End If
                                    End If

                                    If (Request.IsAuthenticated) Then
                                        If (ArticleSettings.JournalIntegration) Then
                                            Dim objJournal As New Journal
                                            objJournal.AddCommentToJournal(objArticle, objComment, PortalId, TabId, UserId, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False))
                                        End If
                                    End If

                                    If (ArticleSettings.EnableSmartThinkerStoryFeed And objComment.UserID <> Null.NullInteger) Then
                                        Dim objStoryFeed As New wsStoryFeed.StoryFeedWS
                                        objStoryFeed.Url = DotNetNuke.Common.Globals.AddHTTP(Request.ServerVariables("HTTP_HOST") & Me.ResolveUrl("~/DesktopModules/Smart-Thinker%20-%20UserProfile/StoryFeed.asmx"))

                                        Dim val As String = GetSharedResource("StoryFeed-AddComment")

                                        Dim delimStr As String = "[]"
                                        Dim delimiter As Char() = delimStr.ToCharArray()
                                        Dim layoutArray As String() = val.Split(delimiter)

                                        Dim valResult As String = ""

                                        For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2

                                            valResult = valResult & layoutArray(iPtr)

                                            If iPtr < layoutArray.Length - 1 Then
                                                Select Case layoutArray(iPtr + 1)

                                                    Case "ARTICLEID"
                                                        valResult = valResult & objComment.ArticleID.ToString()

                                                    Case "AUTHORID"
                                                        valResult = valResult & objComment.UserID.ToString()

                                                    Case "AUTHOR"
                                                        If (objComment.UserID = Null.NullInteger) Then
                                                            valResult = valResult & objComment.AnonymousName
                                                        Else
                                                            valResult = valResult & objComment.AuthorDisplayName
                                                        End If

                                                    Case "ARTICLELINK"
                                                        valResult = valResult & Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False)

                                                    Case "ARTICLETITLE"
                                                        valResult = valResult & objArticle.Title

                                                    Case "ISANONYMOUS"
                                                        If (objComment.UserID <> Null.NullInteger) Then
                                                            While (iPtr < layoutArray.Length - 1)
                                                                If (layoutArray(iPtr + 1) = "/ISANONYMOUS") Then
                                                                    Exit While
                                                                End If
                                                                iPtr = iPtr + 1
                                                            End While
                                                        End If

                                                    Case "/ISANONYMOUS"
                                                        ' Do Nothing

                                                    Case "ISNOTANONYMOUS"
                                                        If (objComment.UserID <> Null.NullInteger) Then
                                                            While (iPtr < layoutArray.Length - 1)
                                                                If (layoutArray(iPtr + 1) = "/ISNOTANONYMOUS") Then
                                                                    Exit While
                                                                End If
                                                                iPtr = iPtr + 1
                                                            End While
                                                        End If

                                                    Case "/ISNOTANONYMOUS"
                                                        ' Do Nothing

                                                End Select
                                            End If
                                        Next

                                        Try
                                            objStoryFeed.AddAction(81, objComment.CommentID, valResult, objComment.UserID, "VE6457624576460436531768")
                                        Catch
                                        End Try

                                    End If

                                    If (ArticleSettings.NotifyEmailOnComment <> "") Then
                                        For Each email As String In ArticleSettings.NotifyEmailOnComment.Split(Convert.ToChar(";"))
                                            If (email <> "") Then
                                                objEmailTemplateController.SendFormattedEmail(Me.ModuleId, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False), objArticle, objComment, EmailTemplateType.CommentNotification, ArticleSettings, email)
                                            End If
                                        Next
                                    End If

                                    If (ArticleSettings.NotifyAuthorOnApproval) Then
                                        objEmailTemplateController.SendFormattedEmail(Me.ModuleId, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False), objArticle, objComment, EmailTemplateType.CommentApproved, ArticleSettings)
                                    End If

                                    Dim objMailList As New Hashtable

                                    Dim objComments As List(Of CommentInfo) = objCommentController.GetCommentList(Me.ModuleId, objComment.ArticleID, True, SortDirection.Ascending, Null.NullInteger)

                                    For Each objNotifyComment As CommentInfo In objComments
                                        If (objNotifyComment.CommentID <> objComment.CommentID And objNotifyComment.NotifyMe) Then
                                            If (objNotifyComment.UserID = Null.NullInteger) Then
                                                If (objNotifyComment.AnonymousEmail <> "") Then
                                                    Try
                                                        If (objMailList.Contains(objNotifyComment.AnonymousEmail) = False) Then
                                                            objEmailTemplateController.SendFormattedEmail(Me.ModuleId, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False), objArticle, objComment, EmailTemplateType.CommentNotification, ArticleSettings, objNotifyComment.AnonymousEmail)
                                                            objMailList.Add(objNotifyComment.AnonymousEmail, objNotifyComment.AnonymousEmail)
                                                        End If
                                                    Catch ex As Exception
                                                        Dim objEventLog As New DotNetNuke.Services.Log.EventLog.EventLogController
                                                        objEventLog.AddLog("News Articles Email Failure", "Failure to send [Anon Comment] to '" & objNotifyComment.AnonymousEmail & "' from '" & Me.PortalSettings.Email, PortalSettings, -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)
                                                    End Try
                                                End If
                                            Else
                                                If (objNotifyComment.AuthorEmail <> "") Then
                                                    Try
                                                        If (objNotifyComment.UserID <> objComment.UserID) Then
                                                            If (objMailList.Contains(objNotifyComment.UserID.ToString()) = False) Then
                                                                objEmailTemplateController.SendFormattedEmail(Me.ModuleId, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False), objArticle, objComment, EmailTemplateType.CommentNotification, ArticleSettings, objNotifyComment.AuthorEmail)
                                                                objMailList.Add(objNotifyComment.UserID.ToString(), objNotifyComment.UserID.ToString())
                                                            End If
                                                        End If
                                                    Catch ex As Exception
                                                        Dim objEventLog As New DotNetNuke.Services.Log.EventLog.EventLogController
                                                        objEventLog.AddLog("News Articles Email Failure", "Failure to send [Author Comment] to '" & objNotifyComment.AuthorEmail & "' from '" & Me.PortalSettings.Email, PortalSettings, -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)
                                                    End Try
                                                End If
                                            End If

                                        End If
                                    Next
                                End If

                            End If
                        End If
                    End If
                End If
            Next

            Response.Redirect(Request.RawUrl, True)

        End Sub

        Protected Sub cmdReject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReject.Click

            For Each item As RepeaterItem In rptApproveComments.Items
                If (item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem) Then
                    Dim chkSelected As CheckBox = CType(item.FindControl("chkSelected"), CheckBox)
                    If Not (chkSelected Is Nothing) Then
                        If (chkSelected.Checked) Then
                            Dim commentID As Integer = Convert.ToInt32(chkSelected.Attributes("CommentID").ToString())
                            Dim objCommentController As New CommentController()
                            Dim objComment As CommentInfo = objCommentController.GetComment(commentID)
                            If Not (objComment Is Nothing) Then
                                objCommentController.DeleteComment(objComment.CommentID, objComment.ArticleID)
                            End If
                        End If
                    End If
                End If
            Next

            Response.Redirect(Request.RawUrl, True)

        End Sub

#End Region

    End Class

End Namespace