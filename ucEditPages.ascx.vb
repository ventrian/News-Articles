'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucEditPages
        Inherits NewsArticleModuleBase

#Region " Private Members "

        Private _articleID As Integer

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            If (IsNumeric(Request("ArticleID"))) Then
                _articleID = Convert.ToInt32(Request("ArticleID"))
            End If


        End Sub

        Private Sub CheckSecurity()

            If (HasEditRights(_articleID, Me.ModuleId, Me.TabId) = False) Then
                Response.Redirect(Common.GetModuleLink(Me.TabId, Me.ModuleId, "NotAuthorized", ArticleSettings), True)
            End If

        End Sub

        Private Sub BindArticle()

            If (_articleID = Null.NullInteger) Then
                Response.Redirect(Common.GetModuleLink(Me.TabId, Me.ModuleId, "NotAuthorized", ArticleSettings), True)
            End If

            Dim objArticleController As ArticleController = New ArticleController

            Dim objArticle As ArticleInfo = objArticleController.GetArticle(_articleID)

            If (objArticle Is Nothing) Then
                Response.Redirect(Common.GetModuleLink(Me.TabId, Me.ModuleId, "NotAuthorized", ArticleSettings), True)
            End If

            lblTitle.Text = String.Format(DotNetNuke.Services.Localization.Localization.GetString("EditPages.Text", LocalResourceFile), objArticle.Title)

            If (objArticle.IsDraft) Then
                cmdSubmitApproval.Visible = True
                cmdSubmitApproval.Attributes.Add("onClick", "javascript:return confirm('" & DotNetNuke.Services.Localization.Localization.GetString("SubmitApproval.Text", LocalResourceFile) & "');")
            Else
                cmdSubmitApproval.Visible = False
            End If

        End Sub

        Private Sub BindPages()

            Dim objPageController As PageController = New PageController

            DotNetNuke.Services.Localization.Localization.LocalizeDataGrid(grdPages, Me.LocalResourceFile)

            grdPages.DataSource = objPageController.GetPageList(_articleID)
            grdPages.DataBind()

            If (grdPages.Items.Count > 0) Then
                grdPages.Visible = True
                lblNoPages.Visible = False
            Else
                grdPages.Visible = False
                lblNoPages.Visible = True
                lblNoPages.Text = DotNetNuke.Services.Localization.Localization.GetString("NoPagesMessage.Text", LocalResourceFile)
            End If

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function GetEditPageUrl(ByVal articleID As String, ByVal pageID As String) As String

            Return Common.GetModuleLink(TabId, ModuleId, "EditPage", ArticleSettings, "ArticleID=" & articleID, "PageID=" & pageID)

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                ReadQueryString()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                CheckSecurity()
                BindPages()

                If (IsPostBack = False) Then
                    BindArticle()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdAddPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddPage.Click

            Try
                Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "EditPage", ArticleSettings, "ArticleID=" & _articleID.ToString()), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdSortOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSortOrder.Click

            Try

                Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "EditSortOrder", ArticleSettings, "ArticleID=" & _articleID.ToString()), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdSummary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSummary.Click

            Try

                Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "SubmitNews", ArticleSettings, "ArticleID=" & _articleID.ToString()), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdSubmitApproval_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmitApproval.Click

            Try

                Dim objLogController As New DotNetNuke.Services.Log.EventLog.EventLogController

                Dim objController As New ArticleController
                Dim objArticle As ArticleInfo = objController.GetArticle(_articleID)

                If Not (objArticle Is Nothing) Then
                    objArticle.Status = StatusType.AwaitingApproval
                    objController.UpdateArticle(objArticle)

                    If (Settings.Contains(ArticleConstants.NOTIFY_SUBMISSION_SETTING)) Then
                        If (Convert.ToBoolean(Settings(ArticleConstants.NOTIFY_SUBMISSION_SETTING)) = True) Then
                            Dim objEmailTemplateController As New EmailTemplateController
                            Dim emails As String = objEmailTemplateController.GetApproverDistributionList(ModuleId)

                            For Each email As String In emails.Split(Convert.ToChar(";"))
                                If (email <> "") Then
                                    objEmailTemplateController.SendFormattedEmail(Me.ModuleId, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False), objArticle, EmailTemplateType.ArticleSubmission, email, ArticleSettings)
                                End If
                            Next
                        End If
                    End If

                    If (Settings.Contains(ArticleConstants.NOTIFY_SUBMISSION_SETTING_EMAIL)) Then
                        If (Settings(ArticleConstants.NOTIFY_SUBMISSION_SETTING_EMAIL).ToString() <> "") Then
                            Dim objEmailTemplateController As New EmailTemplateController
                            objEmailTemplateController.SendFormattedEmail(Me.ModuleId, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False), objArticle, EmailTemplateType.ArticleSubmission, Settings(ArticleConstants.NOTIFY_SUBMISSION_SETTING_EMAIL).ToString(), ArticleSettings)
                        End If
                    End If

                    Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "SubmitNewsComplete", ArticleSettings, "ArticleID=" & _articleID.ToString()), True)

                Else
                    ProcessModuleLoadException(Me, New Exception("Unable to Retrieve Article to Submit for Approval"))
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace