'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2005-2012
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.NewsArticles

    Partial Public Class NewsArchivesOptions
        Inherits ModuleSettingsBase

#Region " Private Members "

        Private _archiveSettings As ArchiveSettings

#End Region

#Region " Private Property "

        Private ReadOnly Property ArchiveMode() As ArchiveModeType
            Get
                Return CType(System.Enum.Parse(GetType(ArchiveModeType), drpMode.SelectedValue), ArchiveModeType)
            End Get
        End Property

        Private ReadOnly Property ArchiveSettings() As ArchiveSettings
            Get
                If (_archiveSettings Is Nothing) Then
                    _archiveSettings = New ArchiveSettings(Settings)
                End If
                Return _archiveSettings
            End Get
        End Property

        Private ReadOnly Property LayoutMode() As LayoutModeType
            Get
                Return CType(System.Enum.Parse(GetType(LayoutModeType), rdoLayoutMode.SelectedValue), LayoutModeType)
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub BindLookups()

            Common.BindEnum(drpAuthorSortBy, GetType(AuthorSortByType), LocalResourceFile)
            Common.BindEnum(rdoLayoutMode, GetType(LayoutModeType), LocalResourceFile)
            Common.BindEnum(drpGroupBy, GetType(GroupByType), LocalResourceFile)
            Common.BindEnum(drpMode, GetType(ArchiveModeType), LocalResourceFile)

        End Sub

        Private Sub BindModules()

            Dim objModules As List(Of ModuleInfo) = Common.GetArticleModules(PortalId)

            For Each objModule As ModuleInfo In objModules
                Dim objListItem As New ListItem

                objListItem.Value = objModule.TabID.ToString() & "-" & objModule.ModuleID.ToString()
                objListItem.Text = objModule.ParentTab.TabName & " -> " & objModule.ModuleTitle

                drpModuleID.Items.Add(objListItem)
            Next

        End Sub

        Private Sub BindParentCategories()

            drpParentCategory.Items.Clear()

            If (drpModuleID.Items.Count > 0) Then

                Dim values As String() = drpModuleID.SelectedValue.Split(Convert.ToChar("-"))

                If (values.Length = 2) Then
                    Dim objCategoryController As New CategoryController
                    drpParentCategory.DataSource = objCategoryController.GetCategoriesAll(Convert.ToInt32(values(1)), Null.NullInteger, CategorySortType.Name)
                    drpParentCategory.DataBind()
                End If

            End If

            drpParentCategory.Items.Insert(0, New ListItem(Localization.GetString("NoParentCategory", LocalResourceFile), "-1"))

        End Sub

        Private Sub BindTemplates()

            Select Case ArchiveMode
                Case ArchiveModeType.Date

                    If (LayoutMode = LayoutModeType.Simple) Then

                        txtHtmlHeader.Text = ArchiveSettings.TemplateDateHeader
                        txtHtmlBody.Text = ArchiveSettings.TemplateDateBody
                        txtHtmlFooter.Text = ArchiveSettings.TemplateDateFooter

                    Else

                        txtHtmlHeader.Text = ArchiveSettings.TemplateDateAdvancedHeader
                        txtHtmlBody.Text = ArchiveSettings.TemplateDateAdvancedBody
                        txtHtmlFooter.Text = ArchiveSettings.TemplateDateAdvancedFooter

                    End If

                Case ArchiveModeType.Category

                    If (LayoutMode = LayoutModeType.Simple) Then

                        txtHtmlHeader.Text = ArchiveSettings.TemplateCategoryHeader
                        txtHtmlBody.Text = ArchiveSettings.TemplateCategoryBody
                        txtHtmlFooter.Text = ArchiveSettings.TemplateCategoryFooter

                    Else

                        txtHtmlHeader.Text = ArchiveSettings.TemplateCategoryAdvancedHeader
                        txtHtmlBody.Text = ArchiveSettings.TemplateCategoryAdvancedBody
                        txtHtmlFooter.Text = ArchiveSettings.TemplateCategoryAdvancedFooter

                    End If

                Case ArchiveModeType.Author

                    If (LayoutMode = LayoutModeType.Simple) Then

                        txtHtmlHeader.Text = ArchiveSettings.TemplateAuthorHeader
                        txtHtmlBody.Text = ArchiveSettings.TemplateAuthorBody
                        txtHtmlFooter.Text = ArchiveSettings.TemplateAuthorFooter

                    Else

                        txtHtmlHeader.Text = ArchiveSettings.TemplateAuthorAdvancedHeader
                        txtHtmlBody.Text = ArchiveSettings.TemplateAuthorAdvancedBody
                        txtHtmlFooter.Text = ArchiveSettings.TemplateAuthorAdvancedFooter

                    End If

            End Select

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.PreRender

            Try

                ' Date Settings
                divGroupBy.Visible = (drpMode.SelectedValue = ArchiveModeType.Date.ToString())

                ' Category Settings
                divHideZeroCategories.Visible = (ArchiveMode = ArchiveModeType.Category)
                divParentCategory.Visible = (ArchiveMode = ArchiveModeType.Category)
                divMaxDepth.Visible = (ArchiveMode = ArchiveModeType.Category)

                ' Author Settings
                divAuthorSortBy.Visible = (drpMode.SelectedValue = ArchiveModeType.Author.ToString())

                ' Template Settings
                divItemsPerRow.Visible = (LayoutMode = LayoutModeType.Advanced)

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub chkMode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles drpMode.SelectedIndexChanged

            Try

                BindTemplates()

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub lstLayoutMode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles rdoLayoutMode.SelectedIndexChanged

            Try

                BindTemplates()

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub drpModuleID_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles drpModuleID.SelectedIndexChanged

            Try

                BindParentCategories()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

#Region " Base Method Implementations "

        Public Overrides Sub LoadSettings()
            Try
                If (Page.IsPostBack = False) Then

                    BindModules()
                    BindLookups()

                    If (drpMode.Items.FindByValue(ArchiveSettings.Mode.ToString()) IsNot Nothing) Then
                        drpMode.SelectedValue = ArchiveSettings.Mode.ToString()
                    End If

                    If (drpModuleID.Items.FindByValue(ArchiveSettings.TabId.ToString() + "-" + ArchiveSettings.ModuleId.ToString()) IsNot Nothing) Then
                        drpModuleID.SelectedValue = ArchiveSettings.TabId.ToString() + "-" + ArchiveSettings.ModuleId.ToString()
                    End If

                    BindParentCategories()

                    ' Date Settings 
                    If (drpGroupBy.Items.FindByValue(ArchiveSettings.GroupBy.ToString()) IsNot Nothing) Then
                        drpGroupBy.SelectedValue = ArchiveSettings.GroupBy.ToString()
                    End If

                    ' Category Settings
                    If ArchiveSettings.CategoryMaxDepth <> Null.NullInteger Then
                        txtMaxDepth.Text = ArchiveSettings.CategoryMaxDepth.ToString()
                    End If
                    If (drpParentCategory.Items.FindByValue(ArchiveSettings.CategoryParent.ToString()) IsNot Nothing) Then
                        drpParentCategory.SelectedValue = ArchiveSettings.CategoryParent.ToString()
                    End If
                    chkHideZeroCategories.Checked = ArchiveSettings.CategoryHideZeroCategories

                    ' Author Settings
                    If (drpAuthorSortBy.Items.FindByValue(ArchiveSettings.AuthorSortBy.ToString()) IsNot Nothing) Then
                        drpAuthorSortBy.SelectedValue = ArchiveSettings.AuthorSortBy.ToString()
                    End If

                    ' Template settings
                    rdoLayoutMode.SelectedValue = ArchiveSettings.LayoutMode.ToString()
                    BindTemplates()
                    txtItemsPerRow.Text = ArchiveSettings.ItemsPerRow.ToString()

                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Public Overrides Sub UpdateSettings()
            Try
                Dim objModules As New ModuleController

                objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_MODE, drpMode.SelectedValue)

                If (drpModuleID.Items.Count > 0) Then

                    Dim values As String() = drpModuleID.SelectedValue.Split(Convert.ToChar("-"))

                    If (values.Length = 2) Then
                        objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_TAB_ID, values(0))
                        objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_MODULE_ID, values(1))
                    End If

                End If

                objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_GROUP_BY, drpGroupBy.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_LAYOUT_MODE, rdoLayoutMode.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_ITEMS_PER_ROW, txtItemsPerRow.Text)

                Dim currentType As ArchiveModeType = CType(System.Enum.Parse(GetType(ArchiveModeType), drpMode.SelectedValue), ArchiveModeType)

                Select Case currentType

                    Case ArchiveModeType.Date
                        
                        If (LayoutMode = LayoutModeType.Simple) Then
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_HEADER, txtHtmlHeader.Text)
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_BODY, txtHtmlBody.Text)
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_FOOTER, txtHtmlFooter.Text)
                        Else
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_HEADER_ADVANCED, txtHtmlHeader.Text)
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_BODY_ADVANCED, txtHtmlBody.Text)
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_FOOTER_ADVANCED, txtHtmlFooter.Text)
                        End If

                    Case ArchiveModeType.Category
                        
                        If (LayoutMode = LayoutModeType.Simple) Then
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_HEADER, txtHtmlHeader.Text)
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_BODY, txtHtmlBody.Text)
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_FOOTER, txtHtmlFooter.Text)
                        Else
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_HEADER_ADVANCED, txtHtmlHeader.Text)
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_BODY_ADVANCED, txtHtmlBody.Text)
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_FOOTER_ADVANCED, txtHtmlFooter.Text)
                        End If
                        objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_HIDE_ZERO_CATEGORIES, chkHideZeroCategories.Checked.ToString())
                        objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_PARENT_CATEGORY, drpParentCategory.SelectedValue)
                        If (txtMaxDepth.Text <> "") Then
                            If (Convert.ToInt32(txtMaxDepth.Text) > 0) Then
                                objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_MAX_DEPTH, txtMaxDepth.Text)
                            End If
                        Else
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_MAX_DEPTH, "-1")
                        End If

                    Case ArchiveModeType.Author
                        
                        If (LayoutMode = LayoutModeType.Simple) Then
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_HEADER, txtHtmlHeader.Text)
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_BODY, txtHtmlBody.Text)
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_FOOTER, txtHtmlFooter.Text)
                        Else
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_HEADER_ADVANCED, txtHtmlHeader.Text)
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_BODY_ADVANCED, txtHtmlBody.Text)
                            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_FOOTER_ADVANCED, txtHtmlFooter.Text)
                        End If
                        objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_AUTHOR_SORT_BY, drpAuthorSortBy.SelectedValue)

                End Select

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region

    End Class

End Namespace