'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Services.Scheduling
Imports DotNetNuke.UI.UserControls

Imports Ventrian.NewsArticles.Import
Imports DotNetNuke.Entities.Modules

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucImportFeeds
        Inherits NewsArticleModuleBase

#Region " Private Methods "

        Private Sub BindFeeds()

            Dim objFeedController As New FeedController

            Localization.LocalizeDataGrid(grdFeeds, Me.LocalResourceFile)

            grdFeeds.DataSource = objFeedController.GetFeedList(ModuleId, False)
            grdFeeds.DataBind()

            If (grdFeeds.Items.Count > 0) Then
                grdFeeds.Visible = True
                lblNoFeeds.Visible = False
            Else
                grdFeeds.Visible = False
                lblNoFeeds.Visible = True
                lblNoFeeds.Text = Localization.GetString("NoFeedsMessage.Text", LocalResourceFile)
            End If

        End Sub

        Private Sub BindHistory()

            Dim typeName As String = "Ventrian.NewsArticles.Import.RssImportJob, Ventrian.NewsArticles"
            Dim objSchedule As ScheduleItem = SchedulingProvider.Instance().GetSchedule(typeName, Null.NullString)

            If (objSchedule IsNot Nothing) Then

                Dim arrSchedule As ArrayList = SchedulingProvider.Instance.GetScheduleHistory(objSchedule.ScheduleID)

                If (arrSchedule.Count > 0) Then

                    arrSchedule.Sort(New ScheduleHistorySortStartDate)

                    'Localize Grid
                    Localization.LocalizeDataGrid(dgScheduleHistory, Me.LocalResourceFile)

                    dgScheduleHistory.DataSource = arrSchedule
                    dgScheduleHistory.DataBind()

                    lblNoHistory.Visible = False
                    dgScheduleHistory.Visible = True
                Else
                    lblNoHistory.Visible = True
                    dgScheduleHistory.Visible = False
                End If

            Else

                lblNoHistory.Visible = True
                dgScheduleHistory.Visible = False

            End If

        End Sub

        Private Sub BindSchedulerSettings()

            Dim typeName As String = "Ventrian.NewsArticles.Import.RssImportJob, Ventrian.NewsArticles"

            Dim objSchedule As ScheduleItem = SchedulingProvider.Instance().GetSchedule(typeName, Null.NullString)

            If (objSchedule IsNot Nothing) Then
                chkEnabled.Checked = objSchedule.Enabled

                txtTimeLapse.Text = objSchedule.TimeLapse.ToString()
                If (drpTimeLapseMeasurement.Items.FindByValue(objSchedule.TimeLapseMeasurement) IsNot Nothing) Then
                    drpTimeLapseMeasurement.SelectedValue = objSchedule.TimeLapseMeasurement
                Else
                    drpTimeLapseMeasurement.SelectedValue = "m"
                End If

                txtRetryTimeLapse.Text = objSchedule.RetryTimeLapse.ToString()
                If (drpRetryTimeLapseMeasurement.Items.FindByValue(objSchedule.RetryTimeLapseMeasurement) IsNot Nothing) Then
                    drpRetryTimeLapseMeasurement.SelectedValue = objSchedule.RetryTimeLapseMeasurement
                Else
                    drpRetryTimeLapseMeasurement.SelectedValue = "m"
                End If
            Else
                txtTimeLapse.Text = "30"
                drpTimeLapseMeasurement.SelectedValue = "m"

                txtRetryTimeLapse.Text = "60"
                drpRetryTimeLapseMeasurement.SelectedValue = "m"
            End If

            Dim objModuleController As New ModuleController
            If (Settings.Contains("NewsArticles-Import-Clear-" & Me.ModuleId)) Then
                chkDeleteArticles.Checked = Convert.ToBoolean(Settings("NewsArticles-Import-Clear-" & Me.ModuleId).ToString())
            Else
                chkDeleteArticles.Checked = False
            End If


        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                phScheduler.Visible = Me.UserInfo.IsSuperUser


                If (IsPostBack = False) Then

                    BindFeeds()

                    If (phScheduler.Visible) Then
                        BindSchedulerSettings()
                        BindHistory()
                    Else
                        Dim typeName As String = "Ventrian.NewsArticles.Import.RssImportJob, Ventrian.NewsArticles"
                        Dim objSchedule As ScheduleItem = SchedulingProvider.Instance().GetSchedule(typeName, Null.NullString)

                        If (objSchedule IsNot Nothing) Then
                            If (objSchedule.Enabled) Then
                                lblScheduler.Visible = False
                            Else
                                lblScheduler.Text = Localization.GetString("SchedulerNotEnabled", Me.LocalResourceFile)
                                lblScheduler.Visible = True
                            End If
                        Else
                            lblScheduler.Text = Localization.GetString("SchedulerNotEnabled", Me.LocalResourceFile)
                            lblScheduler.Visible = True
                        End If
                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub


        Protected Sub cmdAddFeed_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAddFeed.Click

            Try

                Response.Redirect(EditUrl("ImportFeed"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click

            Try

                Dim typeName As String = "Ventrian.NewsArticles.Import.RssImportJob, Ventrian.NewsArticles"

                Dim objSchedule As ScheduleItem = SchedulingProvider.Instance().GetSchedule(typeName, Null.NullString)

                If (objSchedule IsNot Nothing) Then

                    objSchedule.Enabled = chkEnabled.Checked

                    If (IsNumeric(txtTimeLapse.Text)) Then
                        objSchedule.TimeLapse = Convert.ToInt32(txtTimeLapse.Text)
                    Else
                        objSchedule.TimeLapse = 30
                    End If
                    objSchedule.TimeLapseMeasurement = drpTimeLapseMeasurement.SelectedValue

                    If (IsNumeric(txtTimeLapse.Text)) Then
                        objSchedule.RetryTimeLapse = Convert.ToInt32(txtRetryTimeLapse.Text)
                    Else
                        objSchedule.RetryTimeLapse = 60
                    End If
                    objSchedule.RetryTimeLapseMeasurement = drpRetryTimeLapseMeasurement.SelectedValue

                    SchedulingProvider.Instance().UpdateSchedule(objSchedule)

                Else

                    objSchedule = New ScheduleItem()

                    objSchedule.TypeFullName = typeName
                    objSchedule.Enabled = chkEnabled.Checked

                    If (IsNumeric(txtTimeLapse.Text)) Then
                        objSchedule.TimeLapse = Convert.ToInt32(txtTimeLapse.Text)
                    Else
                        objSchedule.TimeLapse = 30
                    End If
                    objSchedule.TimeLapseMeasurement = drpTimeLapseMeasurement.SelectedValue

                    If (IsNumeric(txtTimeLapse.Text)) Then
                        objSchedule.RetryTimeLapse = Convert.ToInt32(txtRetryTimeLapse.Text)
                    Else
                        objSchedule.RetryTimeLapse = 60
                    End If
                    objSchedule.RetryTimeLapseMeasurement = drpRetryTimeLapseMeasurement.SelectedValue

                    objSchedule.RetainHistoryNum = 10
                    objSchedule.AttachToEvent = ""
                    objSchedule.CatchUpEnabled = False
                    objSchedule.Enabled = chkEnabled.Checked
                    objSchedule.ObjectDependencies = ""
                    objSchedule.Servers = ""

                    objSchedule.ScheduleID = SchedulingProvider.Instance().AddSchedule(objSchedule)

                End If

                Dim objModuleController As New ModuleController
                objModuleController.UpdateModuleSetting(Me.ModuleId, "NewsArticles-Import-Clear-" & Me.ModuleId, chkDeleteArticles.Checked.ToString())
                SchedulingProvider.Instance().AddScheduleItemSetting(objSchedule.ScheduleID, "NewsArticles-Import-Clear-" & Me.ModuleId, chkDeleteArticles.Checked.ToString())

                Response.Redirect(EditUrl("AdminOptions"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(EditUrl("AdminOptions"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace