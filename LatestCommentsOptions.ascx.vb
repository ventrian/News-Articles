Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions

Namespace Ventrian.NewsArticles

    Partial Public Class LatestCommentsOptions
        Inherits ModuleSettingsBase

#Region " Private Methods "

        Private Sub BindModules()

            Dim objDesktopModuleController As New DesktopModuleController
            Dim objDesktopModuleInfo As DesktopModuleInfo = objDesktopModuleController.GetDesktopModuleByModuleName("DnnForge - NewsArticles")

            If Not (objDesktopModuleInfo Is Nothing) Then

                Dim objTabController As New TabController()
                Dim objTabs As ArrayList = objTabController.GetTabs(PortalId)
                For Each objTab As DotNetNuke.Entities.Tabs.TabInfo In objTabs
                    If Not (objTab Is Nothing) Then
                        If (objTab.IsDeleted = False) Then
                            Dim objModules As New ModuleController
                            For Each pair As KeyValuePair(Of Integer, ModuleInfo) In objModules.GetTabModules(objTab.TabID)
                                Dim objModule As ModuleInfo = pair.Value
                                If (objModule.IsDeleted = False) Then
                                    If (objModule.DesktopModuleID = objDesktopModuleInfo.DesktopModuleID) Then
                                        If PortalSecurity.IsInRoles(objModule.AuthorizedEditRoles) = True And objModule.IsDeleted = False Then
                                            Dim strPath As String = objTab.TabName
                                            Dim objTabSelected As DotNetNuke.Entities.Tabs.TabInfo = objTab
                                            While objTabSelected.ParentId <> Null.NullInteger
                                                objTabSelected = objTabController.GetTab(objTabSelected.ParentId, objTab.PortalID, False)
                                                If (objTabSelected Is Nothing) Then
                                                    Exit While
                                                End If
                                                strPath = objTabSelected.TabName & " -> " & strPath
                                            End While

                                            Dim objListItem As New ListItem

                                            objListItem.Value = objModule.TabID.ToString() & "-" & objModule.ModuleID.ToString()
                                            objListItem.Text = strPath & " -> " & objModule.ModuleTitle

                                            drpModuleID.Items.Add(objListItem)
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                Next

            End If

        End Sub

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

#End Region

#Region " Base Method Implementations "

        Public Overrides Sub LoadSettings()

            If (IsPostBack = False) Then
                BindModules()

                If (Settings.Contains(ArticleConstants.LATEST_COMMENTS_MODULE_ID) And Settings.Contains(ArticleConstants.LATEST_COMMENTS_TAB_ID)) Then
                    If Not (drpModuleID.Items.FindByValue(Settings(ArticleConstants.LATEST_COMMENTS_TAB_ID).ToString() & "-" & Settings(ArticleConstants.LATEST_COMMENTS_MODULE_ID).ToString()) Is Nothing) Then
                        drpModuleID.SelectedValue = Settings(ArticleConstants.LATEST_COMMENTS_TAB_ID).ToString() & "-" & Settings(ArticleConstants.LATEST_COMMENTS_MODULE_ID).ToString()
                    End If
                End If

                If (Settings.Contains(ArticleConstants.LATEST_COMMENTS_COUNT)) Then
                    txtCommentCount.Text = CType(Settings(ArticleConstants.LATEST_COMMENTS_COUNT), String)
                Else
                    txtCommentCount.Text = "10"
                End If

                If (Settings.Contains(ArticleConstants.LATEST_COMMENTS_HTML_HEADER)) Then
                    txtHtmlHeader.Text = CType(Settings(ArticleConstants.LATEST_COMMENTS_HTML_HEADER), String)
                Else
                    txtHtmlHeader.Text = ArticleConstants.DEFAULT_LATEST_COMMENTS_HTML_HEADER
                End If

                If (Settings.Contains(ArticleConstants.LATEST_COMMENTS_HTML_BODY)) Then
                    txtHtmlBody.Text = CType(Settings(ArticleConstants.LATEST_COMMENTS_HTML_BODY), String)
                Else
                    txtHtmlBody.Text = ArticleConstants.DEFAULT_LATEST_COMMENTS_HTML_BODY
                End If

                If (Settings.Contains(ArticleConstants.LATEST_COMMENTS_HTML_FOOTER)) Then
                    txtHtmlFooter.Text = CType(Settings(ArticleConstants.LATEST_COMMENTS_HTML_FOOTER), String)
                Else
                    txtHtmlFooter.Text = ArticleConstants.DEFAULT_LATEST_COMMENTS_HTML_FOOTER
                End If

                If (Settings.Contains(ArticleConstants.LATEST_COMMENTS_HTML_NO_COMMENTS)) Then
                    txtHtmlNoComments.Text = CType(Settings(ArticleConstants.LATEST_COMMENTS_HTML_NO_COMMENTS), String)
                Else
                    txtHtmlNoComments.Text = ArticleConstants.DEFAULT_LATEST_COMMENTS_HTML_NO_COMMENTS
                End If

                If (Settings.Contains(ArticleConstants.LATEST_COMMENTS_INCLUDE_STYLESHEET)) Then
                    chkIncludeStylesheet.Checked = Convert.ToBoolean(Settings(ArticleConstants.LATEST_COMMENTS_INCLUDE_STYLESHEET).ToString())
                Else
                    chkIncludeStylesheet.Checked = ArticleConstants.DEFAULT_LATEST_COMMENTS_INCLUDE_STYLESHEET
                End If

            End If

        End Sub

        Public Overrides Sub UpdateSettings()

            Try

                Dim objModuleController As New ModuleController

                If (drpModuleID.Items.Count > 0) Then

                    Dim values As String() = drpModuleID.SelectedValue.Split(Convert.ToChar("-"))

                    If (values.Length = 2) Then
                        objModuleController.UpdateTabModuleSetting(Me.TabModuleId, ArticleConstants.LATEST_COMMENTS_TAB_ID, values(0))
                        objModuleController.UpdateTabModuleSetting(Me.TabModuleId, ArticleConstants.LATEST_COMMENTS_MODULE_ID, values(1))
                    End If

                End If
                objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_COMMENTS_COUNT, txtCommentCount.Text)

                objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_COMMENTS_HTML_HEADER, txtHtmlHeader.Text)
                objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_COMMENTS_HTML_BODY, txtHtmlBody.Text)
                objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_COMMENTS_HTML_FOOTER, txtHtmlFooter.Text)
                objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_COMMENTS_HTML_NO_COMMENTS, txtHtmlNoComments.Text)
                objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_COMMENTS_INCLUDE_STYLESHEET, chkIncludeStylesheet.Checked.ToString())

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace