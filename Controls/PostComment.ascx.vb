Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Web.Client.ClientResourceManagement
Imports Joel.Net
Imports Ventrian.NewsArticles.Components.Social

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles.Controls

    Partial Public Class PostComment
        Inherits NewsArticleControlBase

#Region " Private Properties "

        Private Overloads ReadOnly Property ArticleModuleBase() As NewsArticleModuleBase
            Get
                If (TypeOf Parent.Parent Is NewsArticleModuleBase) Then
                    Return CType(Parent.Parent, NewsArticleModuleBase)
                Else
                    Return CType(Parent.Parent.Parent.Parent, NewsArticleModuleBase)
                End If
            End Get
        End Property

        Private ReadOnly Property ArticleSettings() As ArticleSettings
            Get
                Return ArticleModuleBase.ArticleSettings
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub AssignLocalization()

            lblName.Text = GetResourceKey("Name")
            valName.ErrorMessage = GetResourceKey("valName.ErrorMessage")

            If (ArticleSettings.CommentRequireName = False) Then
                valName.Enabled = False
                lblName.Text = GetResourceKey("NameNotRequired")
            End If

            lblEmail.Text = GetResourceKey("Email")
            valEmail.ErrorMessage = GetResourceKey("valEmail.ErrorMessage")
            valEmailIsValid.ErrorMessage = GetResourceKey("valEmailIsValid.ErrorMessage")

            If (ArticleSettings.CommentRequireEmail = False) Then
                valEmail.Enabled = False
                valEmailIsValid.Enabled = False
                lblEmail.Text = GetResourceKey("EmailNotRequired")
            End If

            lblUrl.Text = GetResourceKey("Website")

            valComment.ErrorMessage = GetResourceKey("valComment.ErrorMessage")
            ctlCaptcha.Text = GetResourceKey("ctlCaptcha.Text")
            ctlCaptcha.ErrorMessage = GetResourceKey("ctlCaptcha.ErrorMessage")

            btnAddComment.Text = GetResourceKey("AddComment")
            chkNotifyMe.Text = GetResourceKey("NotifyMe")
            lblRequiresApproval.Text = GetResourceKey("RequiresApproval")

            lblRequiresAccess.Text = GetResourceKey("RequiresAccess")

        End Sub

        Private Sub CheckSecurity()

            If (ArticleSettings.IsCommentsAnonymous = False And Request.IsAuthenticated = False) Then
                phCommentForm.Visible = False
                phCommentAnonymous.Visible = True
            End If

        End Sub

        Private Function FilterInput(ByVal stringToFilter As String) As String

            Dim objPortalSecurity As New PortalSecurity

            stringToFilter = objPortalSecurity.InputFilter(stringToFilter, PortalSecurity.FilterFlag.NoScripting)

            stringToFilter = Replace(stringToFilter, Chr(13), "")
            stringToFilter = Replace(stringToFilter, ControlChars.Lf, "<br />")

            Return stringToFilter

        End Function

        Private Sub GetCookie()

            If (Request.IsAuthenticated = False) Then
                Dim cookie As HttpCookie = Request.Cookies("comment")

                If (cookie IsNot Nothing) Then
                    txtName.Text = cookie.Values("name")
                    txtEmail.Text = cookie.Values("email")
                    txtURL.Text = cookie.Values("url")
                End If
            End If

            chkNotifyMe.Checked = ArticleSettings.NotifyDefault

        End Sub

        Public Function GetResourceKey(ByVal key As String) As String

            Dim path As String = "~/DesktopModules/DnnForge - NewsArticles/" & Localization.LocalResourceDirectory & "/PostComment.ascx.resx"
            Return DotNetNuke.Services.Localization.Localization.GetString(key, path)

        End Function

        Private Sub NotifyAuthor(ByVal objComment As CommentInfo, ByVal objArticle As ArticleInfo)

            Dim objEmailTemplateController As New EmailTemplateController
            objEmailTemplateController.SendFormattedEmail(ArticleModuleBase.ModuleId, Common.GetArticleLink(objArticle, ArticleModuleBase.PortalSettings.ActiveTab, ArticleSettings, False), objArticle, objComment, EmailTemplateType.CommentNotification, ArticleSettings)

        End Sub

        Private Sub NotifyComments(ByVal objComment As CommentInfo, ByVal objArticle As ArticleInfo)

            Dim objEmailTemplateController As New EmailTemplateController
            Dim objMailList As New Hashtable

            Dim objCommentController As New CommentController
            Dim objComments As List(Of CommentInfo) = objCommentController.GetCommentList(ArticleModuleBase.ModuleId, ArticleID, True, SortDirection.Ascending, Null.NullInteger)

            For Each objNotifyComment As CommentInfo In objComments
                If (objNotifyComment.CommentID <> objComment.CommentID And objNotifyComment.NotifyMe) Then
                    If (objNotifyComment.UserID = Null.NullInteger) Then
                        If (objNotifyComment.AnonymousEmail <> "") Then
                            If (objNotifyComment.AnonymousEmail <> objComment.AnonymousEmail) Then
                                If (objMailList.Contains(objNotifyComment.AnonymousEmail) = False) Then
                                    objEmailTemplateController.SendFormattedEmail(ArticleModuleBase.ModuleId, Common.GetArticleLink(objArticle, ArticleModuleBase.PortalSettings.ActiveTab, ArticleSettings, False), objArticle, objComment, EmailTemplateType.CommentNotification, ArticleSettings, objNotifyComment.AnonymousEmail)
                                    objMailList.Add(objNotifyComment.AnonymousEmail, objNotifyComment.AnonymousEmail)
                                End If
                            End If
                        End If
                    Else
                        If (objNotifyComment.AuthorEmail <> "") Then
                            If (objNotifyComment.UserID <> objComment.UserID) Then
                                If (objMailList.Contains(objNotifyComment.AuthorEmail.ToString()) = False) Then
                                    objEmailTemplateController.SendFormattedEmail(ArticleModuleBase.ModuleId, Common.GetArticleLink(objArticle, ArticleModuleBase.PortalSettings.ActiveTab, ArticleSettings, False), objArticle, objComment, EmailTemplateType.CommentNotification, ArticleSettings, objNotifyComment.AuthorEmail)
                                    objMailList.Add(objNotifyComment.AuthorEmail, objNotifyComment.AuthorEmail)
                                End If
                            End If
                        End If
                    End If

                End If
            Next

        End Sub

        Private Sub SetCookie()

            If (Request.IsAuthenticated = False) Then
                Dim objCookie As New HttpCookie("comment")

                objCookie.Expires = DateTime.Now.AddMonths(24)
                objCookie.Values.Add("name", txtName.Text)
                objCookie.Values.Add("email", txtEmail.Text)
                objCookie.Values.Add("url", txtURL.Text)

                Response.Cookies.Add(objCookie)
            End If

        End Sub

        Private Sub SetVisibility()

            pName.Visible = Not Request.IsAuthenticated
            pEmail.Visible = Not Request.IsAuthenticated
            pUrl.Visible = Not Request.IsAuthenticated

            ctlCaptcha.Visible = (ArticleSettings.CaptchaType = CaptchaType.DnnCore And Request.IsAuthenticated = False)
            ctlReCaptcha.Visible = (ArticleSettings.CaptchaType = CaptchaType.ReCaptcha And Request.IsAuthenticated = False)
            ctlHoneypot.Visible = (ArticleSettings.CaptchaType = CaptchaType.Honeypot And Request.IsAuthenticated = False)
            if ctlReCaptcha.Visible Then
                ClientResourceManager.RegisterScript(Page, ResolveUrl("https://www.google.com/recaptcha/api.js"))
            End If

            If (Request.IsAuthenticated = False) Then
                pUrl.Visible = Not ArticleSettings.IsCommentWebsiteHidden
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            If Not (TypeOf Parent.Parent Is NewsArticleModuleBase Or TypeOf Parent.Parent.Parent.Parent Is NewsArticleModuleBase) Then
                Visible = False
                Return
            End If

            CheckSecurity()
            AssignLocalization()
            SetVisibility()

            valName.ValidationGroup = "PostComment-" & ArticleID.ToString()
            valEmail.ValidationGroup = "PostComment-" & ArticleID.ToString()
            valEmailIsValid.ValidationGroup = "PostComment-" & ArticleID.ToString()
            valComment.ValidationGroup = "PostComment-" & ArticleID.ToString()
            btnAddComment.ValidationGroup = "PostComment-" & ArticleID.ToString()

            If ArticleSettings.IsCommentsEnabled AndAlso ArticleSettings.CaptchaType = CaptchaType.ReCaptcha Then
                ctlReCaptcha.SiteKey = ArticleSettings.ReCaptchaSiteKey
                ctlReCaptcha.SecretKey = ArticleSettings.ReCaptchaSecretKey
            End If

            If (Page.IsPostBack = False) Then
                GetCookie()
            End If

        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

            If (HttpContext.Current.Items.Contains("IgnoreCaptcha")) Then
                ctlCaptcha.ErrorMessage = ""
            End If

        End Sub

        Protected Sub btnAddComment_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddComment.Click

            If (Page.IsValid) Then
                If (ctlCaptcha.Visible AndAlso ctlCaptcha.IsValid = False) Then
                    txtComment.Focus()
                    Return
                End If
                If (ctlReCaptcha.Visible AndAlso ctlReCaptcha.RecaptchaIsValid() = False) Then
                    txtComment.Focus()
                    Return
                End If
                If (ctlHoneypot.Visible AndAlso ctlHoneypot.IsValid() = False) Then
                    txtComment.Focus()
                    Return
                End If

                Dim objController As New ArticleController
                Dim objArticle As ArticleInfo = objController.GetArticle(ArticleID)

                Dim objCommentController As New CommentController
                Dim objComments As List(Of CommentInfo) = objCommentController.GetCommentList(ArticleModuleBase.ModuleId, ArticleID, True, SortDirection.Ascending, Null.NullInteger)

                For Each objArticleComment As CommentInfo In objComments
                    If (objArticleComment.CreatedDate > DateTime.Now.AddMinutes(-1)) Then
                        Dim id As Integer = Null.NullInteger
                        If (Request.IsAuthenticated) Then
                            id = ArticleModuleBase.UserId
                        End If
                        If (objArticleComment.Comment = FilterInput(txtComment.Text) And objArticleComment.UserID = id) Then
                            ' Existing Comment just posted - so ignore redirect.
                            If (Request("articleType") <> "ArticleView") Then
                                Response.Redirect(Request.RawUrl & "#Comment" & objArticleComment.CommentID.ToString(), True)
                            Else
                                Response.Redirect(Common.GetArticleLink(objArticle, ArticleModuleBase.PortalSettings.ActiveTab, ArticleSettings, False) & "#Comment" & objArticleComment.CommentID.ToString(), True)
                            End If
                            Return
                        End If
                    End If
                Next

                objComments = objCommentController.GetCommentList(ArticleModuleBase.ModuleId, ArticleID, False, SortDirection.Ascending, Null.NullInteger)

                For Each objArticleComment As CommentInfo In objComments
                    If (objArticleComment.CreatedDate > DateTime.Now.AddMinutes(-1)) Then
                        Dim id As Integer = Null.NullInteger
                        If (Request.IsAuthenticated) Then
                            id = ArticleModuleBase.UserId
                        End If
                        If (objArticleComment.Comment = FilterInput(txtComment.Text) And objArticleComment.UserID = id) Then
                            ' Existing Comment just posted - so ignore redirect.
                            If (Request("articleType") <> "ArticleView") Then
                                Response.Redirect(Request.RawUrl & "#Comment" & objArticleComment.CommentID.ToString(), True)
                            Else
                                Response.Redirect(Common.GetArticleLink(objArticle, ArticleModuleBase.PortalSettings.ActiveTab, ArticleSettings, False) & "#Comment" & objArticleComment.CommentID.ToString(), True)
                            End If
                            Return
                        End If
                    End If
                Next

                Dim objComment As New CommentInfo
                objComment.ArticleID = ArticleID
                objComment.CreatedDate = DateTime.Now
                If (Request.IsAuthenticated) Then
                    objComment.UserID = ArticleModuleBase.UserId
                Else
                    objComment.UserID = Null.NullInteger
                    objComment.AnonymousName = txtName.Text
                    objComment.AnonymousEmail = txtEmail.Text
                    objComment.AnonymousURL = txtURL.Text
                    SetCookie()
                End If
                objComment.Comment = FilterInput(txtComment.Text)
                objComment.RemoteAddress = Request.UserHostAddress
                objComment.NotifyMe = chkNotifyMe.Checked
                objComment.Type = 0

                If (ArticleSettings.IsApprover Or ArticleSettings.IsAutoApproverComment) Then
                    objComment.IsApproved = True
                    objComment.ApprovedBy = ArticleModuleBase.UserId
                Else
                    If (ArticleSettings.CommentModeration) Then
                        objComment.IsApproved = False
                        objComment.ApprovedBy = Null.NullInteger
                    Else
                        objComment.IsApproved = True
                        objComment.ApprovedBy = Null.NullInteger
                    End If
                End If

                ' Akismet
                If (ArticleSettings.CommentAkismetKey <> "") Then
                    Dim api As New Akismet(ArticleSettings.CommentAkismetKey, DotNetNuke.Common.Globals.NavigateURL(CType(Parent.Parent, DotNetNuke.Entities.Modules.PortalModuleBase).TabId), "Test/1.0")
                    If (api.VerifyKey()) Then
                        Dim comment As New AkismetComment()

                        comment.Blog = DotNetNuke.Common.Globals.NavigateURL(CType(Parent.Parent, DotNetNuke.Entities.Modules.PortalModuleBase).TabId)
                        comment.UserIp = objComment.RemoteAddress
                        comment.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)"
                        comment.CommentContent = objComment.Comment
                        comment.CommentType = "comment"

                        If (Request.IsAuthenticated) Then
                            comment.CommentAuthor = CType(Parent.Parent, DotNetNuke.Entities.Modules.PortalModuleBase).UserInfo.DisplayName
                            comment.CommentAuthorEmail = CType(Parent.Parent, DotNetNuke.Entities.Modules.PortalModuleBase).UserInfo.Email
                            comment.CommentAuthorUrl = ""
                        Else
                            comment.CommentAuthor = objComment.AnonymousName
                            comment.CommentAuthorEmail = objComment.AnonymousEmail
                            comment.CommentAuthorUrl = objComment.AnonymousURL
                        End If

                        If (api.CommentCheck(comment)) Then
                            txtComment.Focus()
                            Return
                        End If
                    End If
                End If

                objComment.CommentID = objCommentController.AddComment(objComment)

                ' Re-init for user details.
                objComment = objCommentController.GetComment(objComment.CommentID)

                If Not (objArticle Is Nothing) Then

                    ' Notifications
                    If (objComment.IsApproved) Then
                        If (ArticleSettings.NotifyAuthorOnComment) Then
                            NotifyAuthor(objComment, objArticle)
                        End If
                        NotifyComments(objComment, objArticle)

                        If (ArticleSettings.NotifyEmailOnComment <> "") Then
                            Dim objEmailTemplateController As New EmailTemplateController
                            For Each email As String In ArticleSettings.NotifyEmailOnComment.Split(Convert.ToChar(";"))
                                If (email <> "") Then
                                    objEmailTemplateController.SendFormattedEmail(ArticleModuleBase.ModuleId, Common.GetArticleLink(objArticle, ArticleModuleBase.PortalSettings.ActiveTab, ArticleSettings, False), objArticle, objComment, EmailTemplateType.CommentNotification, ArticleSettings, email)
                                End If
                            Next
                        End If

                        If (ArticleSettings.EnableActiveSocialFeed And Request.IsAuthenticated) Then
                            If (ArticleSettings.ActiveSocialCommentKey <> "") Then
                                If IO.File.Exists(HttpContext.Current.Server.MapPath("~/bin/active.modules.social.dll")) Then
                                    Dim ai As Object = Nothing
                                    Dim asm As System.Reflection.Assembly
                                    Dim ac As Object = Nothing
                                    Try
                                        asm = System.Reflection.Assembly.Load("Active.Modules.Social")
                                        ac = asm.CreateInstance("Active.Modules.Social.API.Journal")
                                        If Not ac Is Nothing Then
                                            ac.AddProfileItem(New Guid(ArticleSettings.ActiveSocialCommentKey), objComment.UserID, Common.GetArticleLink(objArticle, ArticleModuleBase.PortalSettings.ActiveTab, ArticleSettings, False), objArticle.Title, objComment.Comment, objComment.Comment, 1, "")
                                        End If
                                    Catch ex As Exception
                                    End Try
                                End If
                            End If
                        End If

                        If (Request.IsAuthenticated) Then
                            If (ArticleSettings.JournalIntegration) Then
                                Dim objJournal As New Journal
                                objJournal.AddCommentToJournal(objArticle, objComment, ArticleModuleBase.PortalId, ArticleModuleBase.TabId, ArticleModuleBase.UserId, Common.GetArticleLink(objArticle, ArticleModuleBase.PortalSettings.ActiveTab, ArticleSettings, False))
                            End If
                        End If

                        If (ArticleSettings.EnableSmartThinkerStoryFeed And Request.IsAuthenticated) Then
                            Dim objStoryFeed As New wsStoryFeed.StoryFeedWS
                            objStoryFeed.Url = DotNetNuke.Common.Globals.AddHTTP(Request.ServerVariables("HTTP_HOST") & Me.ResolveUrl("~/DesktopModules/Smart-Thinker%20-%20UserProfile/StoryFeed.asmx"))

                            Dim val As String = ArticleModuleBase.GetSharedResource("StoryFeed-AddComment")

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
                                            valResult = valResult & Common.GetArticleLink(objArticle, ArticleModuleBase.PortalSettings.ActiveTab, ArticleSettings, False)

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
                    Else

                        If (ArticleSettings.NotifyApproverForCommentApproval) Then

                            ' Notify Approvers
                            Dim objEmailTemplateController As New EmailTemplateController
                            Dim emails As String = objEmailTemplateController.GetApproverDistributionList(ArticleModuleBase.ModuleId)

                            For Each email As String In emails.Split(Convert.ToChar(";"))
                                If (email <> "") Then
                                    Try
                                        objEmailTemplateController.SendFormattedEmail(ArticleModuleBase.ModuleId, Common.GetArticleLink(objArticle, ArticleModuleBase.PortalSettings.ActiveTab, ArticleSettings, False), objArticle, objComment, EmailTemplateType.CommentRequiringApproval, ArticleSettings, email)
                                    Catch
                                    End Try
                                End If
                            Next

                        End If

                        If (ArticleSettings.NotifyEmailForCommentApproval <> "") Then

                            Dim objEmailTemplateController As New EmailTemplateController

                            For Each email As String In ArticleSettings.NotifyEmailForCommentApproval.Split(Convert.ToChar(";"))
                                If (email <> "") Then
                                    Try
                                        objEmailTemplateController.SendFormattedEmail(ArticleModuleBase.ModuleId, Common.GetArticleLink(objArticle, ArticleModuleBase.PortalSettings.ActiveTab, ArticleSettings, False), objArticle, objComment, EmailTemplateType.CommentRequiringApproval, ArticleSettings, email)
                                    Catch
                                    End Try
                                End If
                            Next

                        End If

                    End If

                    ' Redirect
                    If (objComment.IsApproved) Then
                        If (Request("articleType") <> "ArticleView") Then
                            Response.Redirect(Request.RawUrl & "#Comment" & objComment.CommentID.ToString(), True)
                        Else
                            Response.Redirect(Common.GetArticleLink(objArticle, ArticleModuleBase.PortalSettings.ActiveTab, ArticleSettings, False) & "#Comment" & objComment.CommentID.ToString(), True)
                        End If
                    Else
                        phCommentForm.Visible = False
                        phCommentPosted.Visible = True
                    End If

                Else

                    ' Should never be here.
                    Response.Redirect(DotNetNuke.Common.NavigateURL(), True)

                End If

            End If

        End Sub

#End Region

    End Class

End Namespace