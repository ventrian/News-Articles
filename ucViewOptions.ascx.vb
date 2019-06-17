'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.IO
Imports System.Linq

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.NewsArticles.Components.Types
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Entities.Users

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucViewOptions
        Inherits NewsArticleModuleBase


        '''<summary>
        '''ctlWatermarkImage control.
        '''</summary>
        '''<remarks>
        '''Auto-generated field.
        '''To modify move field declaration from designer file to code-behind file.
        '''</remarks>
        Protected WithEvents ctlWatermarkImage As DotNetNuke.UI.UserControls.UrlControl

#Region " Private Methods "

        Public Class ListItemComparer
            Implements IComparer(Of ListItem)

            Public Function Compare(ByVal x As ListItem, ByVal y As ListItem) As Integer _
                Implements IComparer(Of ListItem).Compare

                Dim c As New CaseInsensitiveComparer
                Return c.Compare(x.Text, y.Text)
            End Function
        End Class

        Public Shared Sub SortDropDown(ByVal cbo As DropDownList)
            Dim lstListItems As New List(Of ListItem)
            For Each li As ListItem In cbo.Items
                lstListItems.Add(li)
            Next
            lstListItems.Sort(New ListItemComparer)
            cbo.Items.Clear()
            cbo.Items.AddRange(lstListItems.ToArray)
        End Sub

#Region " Private Methods - Load Types/Dropdowns "

        Private Sub BindRoleGroups()

            Dim objRole As New RoleController

            drpSecurityRoleGroups.DataSource = RoleController.GetRoleGroups(PortalId)
            drpSecurityRoleGroups.DataBind()
            drpSecurityRoleGroups.Items.Insert(0, New ListItem(Localization.GetString("AllGroups", Me.LocalResourceFile), "-1"))

        End Sub

        Private Sub BindRoles()

            Dim availableRoles As ArrayList = New ArrayList
            Dim roles As IList(Of RoleInfo)
            If (drpSecurityRoleGroups.SelectedValue = "-1") Then
                roles = RoleController.Instance.GetRoles(PortalId)
            Else
                Dim objRole As New RoleController
                roles = objRole.GetRolesByGroup(PortalId, Convert.ToInt32(drpSecurityRoleGroups.SelectedValue))
            End If

            If Not roles Is Nothing Then
                For Each Role As RoleInfo In roles
                    availableRoles.Add(New ListItem(Role.RoleName, Role.RoleName))
                Next
            End If

            grdBasicPermissions.DataSource = availableRoles
            grdBasicPermissions.DataBind()

            grdFormPermissions.DataSource = availableRoles
            grdFormPermissions.DataBind()

            grdAdminPermissions.DataSource = availableRoles
            grdAdminPermissions.DataBind()

        End Sub

        Private Sub BindAuthorSelection()

            For Each value As Integer In System.Enum.GetValues(GetType(AuthorSelectType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(AuthorSelectType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(AuthorSelectType), value), Me.LocalResourceFile)
                lstAuthorSelection.Items.Add(li)
            Next

        End Sub

        Private Sub BindCategorySortOrder()

            For Each value As Integer In System.Enum.GetValues(GetType(CategorySortType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(CategorySortType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(CategorySortType), value), Me.LocalResourceFile)
                lstCategorySortOrder.Items.Add(li)
            Next

        End Sub

        Private Sub BindDisplayTypes()

            For Each value As Integer In System.Enum.GetValues(GetType(DisplayType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(DisplayType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(DisplayType), value), Me.LocalResourceFile)
                drpDisplayType.Items.Add(li)
            Next

        End Sub

        Private Sub BindCaptchaTypes()

            For Each value As Integer In System.Enum.GetValues(GetType(CaptchaType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(CaptchaType), value)
                li.Text = Localization.GetString($"CaptchaType{System.Enum.GetName(GetType(CaptchaType), value)}", Me.LocalResourceFile)
                drpCaptchaType.Items.Add(li)
            Next

        End Sub

        Private Sub BindRelatedTypes()

            For Each value As Integer In System.Enum.GetValues(GetType(RelatedType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(RelatedType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(RelatedType), value), Me.LocalResourceFile)
                lstRelatedMode.Items.Add(li)
            Next

        End Sub

        Private Sub BindFolders()

            Dim folders As List(Of IFolderInfo) = FolderManager.Instance.GetFolders(PortalId)
            For Each folder As FolderInfo In folders
                Dim FolderItem As New ListItem
                If folder.FolderPath = Null.NullString Then
                    FolderItem.Text = Localization.GetString("Root", Me.LocalResourceFile)
                Else
                    FolderItem.Text = folder.FolderPath
                End If
                FolderItem.Value = folder.FolderID.ToString()
                drpDefaultImageFolder.Items.Add(FolderItem)
                drpDefaultFileFolder.Items.Add(New ListItem(FolderItem.Text, FolderItem.Value))
            Next

        End Sub

        Private Sub BindTextEditorMode()

            For Each value As Integer In System.Enum.GetValues(GetType(TextEditorModeType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(TextEditorModeType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(TextEditorModeType), value), Me.LocalResourceFile)
                lstTextEditorSummaryMode.Items.Add(li)
            Next

        End Sub

        Private Sub BindTitleReplacement()

            For Each value As Integer In System.Enum.GetValues(GetType(TitleReplacementType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(TitleReplacementType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(TitleReplacementType), value), Me.LocalResourceFile)
                lstTitleReplacement.Items.Add(li)
            Next

        End Sub

        Private Sub BindUrlMode()

            For Each value As Integer In System.Enum.GetValues(GetType(UrlModeType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(UrlModeType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(UrlModeType), value), Me.LocalResourceFile)
                lstUrlMode.Items.Add(li)
            Next

        End Sub

        Private Sub BindPageSize()

            drpNumber.Items.Add(New ListItem(Localization.GetString("NoRestriction", Me.LocalResourceFile), "-1"))
            drpNumber.Items.Add(New ListItem(Localization.GetString("1", Me.LocalResourceFile), "1"))
            drpNumber.Items.Add(New ListItem(Localization.GetString("2", Me.LocalResourceFile), "2"))
            drpNumber.Items.Add(New ListItem(Localization.GetString("3", Me.LocalResourceFile), "3"))
            drpNumber.Items.Add(New ListItem(Localization.GetString("4", Me.LocalResourceFile), "4"))
            drpNumber.Items.Add(New ListItem(Localization.GetString("5", Me.LocalResourceFile), "5"))
            drpNumber.Items.Add(New ListItem(Localization.GetString("6", Me.LocalResourceFile), "6"))
            drpNumber.Items.Add(New ListItem(Localization.GetString("7", Me.LocalResourceFile), "7"))
            drpNumber.Items.Add(New ListItem(Localization.GetString("8", Me.LocalResourceFile), "8"))
            drpNumber.Items.Add(New ListItem(Localization.GetString("9", Me.LocalResourceFile), "9"))
            drpNumber.Items.Add(New ListItem(Localization.GetString("10", Me.LocalResourceFile), "10"))
            drpNumber.Items.Add(New ListItem(Localization.GetString("15", Me.LocalResourceFile), "15"))
            drpNumber.Items.Add(New ListItem(Localization.GetString("20", Me.LocalResourceFile), "20"))
            drpNumber.Items.Add(New ListItem(Localization.GetString("50", Me.LocalResourceFile), "50"))

        End Sub

        Private Sub BindSortBy()

            drpSortBy.Items.Add(New ListItem(Localization.GetString("PublishDate.Text", Me.LocalResourceFile), "StartDate"))
            drpSortBy.Items.Add(New ListItem(Localization.GetString("ExpiryDate.Text", Me.LocalResourceFile), "EndDate"))
            drpSortBy.Items.Add(New ListItem(Localization.GetString("LastUpdate.Text", Me.LocalResourceFile), "LastUpdate"))
            drpSortBy.Items.Add(New ListItem(Localization.GetString("HighestRated.Text", Me.LocalResourceFile), "Rating"))
            drpSortBy.Items.Add(New ListItem(Localization.GetString("MostCommented.Text", Me.LocalResourceFile), "CommentCount"))
            drpSortBy.Items.Add(New ListItem(Localization.GetString("MostViewed.Text", Me.LocalResourceFile), "NumberOfViews"))
            drpSortBy.Items.Add(New ListItem(Localization.GetString("Random.Text", Me.LocalResourceFile), "NewID()"))
            drpSortBy.Items.Add(New ListItem(Localization.GetString("SortTitle.Text", Me.LocalResourceFile), "Title"))

        End Sub

        Private Sub BindSortDirection()

            drpSortDirection.Items.Add(New ListItem(Localization.GetString("Ascending.Text", Me.LocalResourceFile), "ASC"))
            drpSortDirection.Items.Add(New ListItem(Localization.GetString("Descending.Text", Me.LocalResourceFile), "DESC"))

            drpSortDirectionComments.Items.Add(New ListItem(Localization.GetString("Ascending.Text", Me.LocalResourceFile), "0"))
            drpSortDirectionComments.Items.Add(New ListItem(Localization.GetString("Descending.Text", Me.LocalResourceFile), "1"))

        End Sub

        Private Sub BindSyndicationLinkMode()

            For Each value As Integer In System.Enum.GetValues(GetType(SyndicationLinkType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(SyndicationLinkType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(SyndicationLinkType), value), Me.LocalResourceFile)
                lstSyndicationLinkMode.Items.Add(li)
            Next

        End Sub

        Private Sub BindSyndicationEnclosureType()

            For Each value As Integer In System.Enum.GetValues(GetType(SyndicationEnclosureType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(SyndicationEnclosureType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(SyndicationEnclosureType), value) & "-Enclosure", Me.LocalResourceFile)
                lstSyndicationEnclosureType.Items.Add(li)
            Next

        End Sub

        Private Sub BindMenuPositionType()

            For Each value As Integer In System.Enum.GetValues(GetType(MenuPositionType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(MenuPositionType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(MenuPositionType), value), Me.LocalResourceFile)
                lstMenuPosition.Items.Add(li)
            Next

        End Sub

        Private Sub BindTemplates()

            Dim templateRoot As String = Me.MapPath("Templates")
            If Directory.Exists(templateRoot) Then
                Dim arrFolders() As String = Directory.GetDirectories(templateRoot)
                For Each folder As String In arrFolders
                    Dim folderName As String = folder.Substring(folder.LastIndexOf("\") + 1)
                    Dim objListItem As ListItem = New ListItem
                    objListItem.Text = folderName
                    objListItem.Value = folderName
                    drpTemplates.Items.Add(objListItem)
                Next
            End If

        End Sub

        Private Sub BindCategories()

            Dim objCategoryController As CategoryController = New CategoryController
            Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(Me.ModuleId, Null.NullInteger, ArticleSettings.CategorySortType)

            lstCategories.DataSource = objCategories
            lstCategories.DataBind()

            lstDefaultCategories.DataSource = objCategories
            lstDefaultCategories.DataBind()

            drpCategories.DataSource = objCategories
            drpCategories.DataBind()

        End Sub

        Private Sub BindTimeZone()

            drpTimeZone.Items.Clear()
            For Each tz As TimeZoneInfo In TimeZoneInfo.GetSystemTimeZones()
                drpTimeZone.Items.Add(New ListItem(tz.DisplayName, tz.BaseUtcOffset.TotalMinutes))
            Next
            'DotNetNuke.Services.Localization.Localization.LoadTimeZoneDropDownList(drpTimeZone, BasePage.PageCulture.Name, Convert.ToString(PortalSettings.TimeZoneOffset))

        End Sub

        Private Sub BindThumbnailType()

            For Each value As Integer In System.Enum.GetValues(GetType(ThumbnailType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(ThumbnailType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(ThumbnailType), value), Me.LocalResourceFile)
                rdoThumbnailType.Items.Add(li)
            Next

        End Sub

        Private Sub BindWatermarkPosition()

            For Each value As Integer In System.Enum.GetValues(GetType(WatermarkPosition))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(WatermarkPosition), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(WatermarkPosition), value), Me.LocalResourceFile)
                drpWatermarkPosition.Items.Add(li)
            Next

        End Sub

#End Region

        Private Sub BindSettings()

            BindBasicSettings()
            BindArchiveSettings()
            BindCategorySettings()
            BindCommentSettings()
            BindContentSharingSettings()
            BindFilterSettings()
            BindFormSettings()
            BindImageSettings()
            BindFileSettings()
            BindNotificationSettings()
            BindRelatedSettings()
            BindSecuritySettings()
            BindSEOSettings()
            BindSyndicationSettings()
            BindTwitterSettings()
            BindThirdPartySettings()

        End Sub

#Region " Private Methods - Bind/Save Basic Settings "

        Private Sub BindBasicSettings()

            chkEnableRatingsAuthenticated.Checked = ArticleSettings.EnableRatingsAuthenticated
            chkEnableRatingsAnonymous.Checked = ArticleSettings.EnableRatingsAnonymous
            chkEnableCoreSearch.Checked = ArticleSettings.EnableCoreSearch

            chkEnableNotificationPing.Checked = ArticleSettings.EnableNotificationPing
            chkEnableAutoTrackback.Checked = ArticleSettings.EnableAutoTrackback

            chkProcessPostTokens.Checked = ArticleSettings.ProcessPostTokens

            If (Settings.Contains(ArticleConstants.ENABLE_INCOMING_TRACKBACK_SETTING)) Then
                chkEnableIncomingTrackback.Checked = Convert.ToBoolean(Settings(ArticleConstants.ENABLE_INCOMING_TRACKBACK_SETTING).ToString())
            Else
                chkEnableIncomingTrackback.Checked = True
            End If

            If (Settings.Contains(ArticleConstants.LAUNCH_LINKS)) Then
                chkLaunchLinks.Checked = Convert.ToBoolean(Settings(ArticleConstants.LAUNCH_LINKS).ToString())
            Else
                chkLaunchLinks.Checked = ArticleSettings.LaunchLinks
            End If

            If (Settings.Contains(ArticleConstants.BUBBLE_FEATURED_ARTICLES)) Then
                chkBubbleFeaturedArticles.Checked = Convert.ToBoolean(Settings(ArticleConstants.BUBBLE_FEATURED_ARTICLES).ToString())
            Else
                chkBubbleFeaturedArticles.Checked = False
            End If

            If (Settings.Contains(ArticleConstants.REQUIRE_CATEGORY)) Then
                chkRequireCategory.Checked = Convert.ToBoolean(Settings(ArticleConstants.REQUIRE_CATEGORY).ToString())
            Else
                chkRequireCategory.Checked = False
            End If

            If Not (drpDisplayType.Items.FindByValue(ArticleSettings.DisplayMode.ToString()) Is Nothing) Then
                drpDisplayType.SelectedValue = ArticleSettings.DisplayMode.ToString()
            End If

            If Not (drpTemplates.Items.FindByValue(ArticleSettings.Template) Is Nothing) Then
                drpTemplates.SelectedValue = ArticleSettings.Template
            End If

            If (drpNumber.Items.FindByValue(ArticleSettings.PageSize.ToString()) IsNot Nothing) Then
                drpNumber.SelectedValue = ArticleSettings.PageSize.ToString()
            End If

            If Not (drpTimeZone.Items.FindByValue(ArticleSettings.ServerTimeZone.ToString()) Is Nothing) Then
                drpTimeZone.SelectedValue = ArticleSettings.ServerTimeZone.ToString()
            End If

            If Not (drpSortBy.Items.FindByValue(ArticleSettings.SortBy.ToString()) Is Nothing) Then
                drpSortBy.SelectedValue = ArticleSettings.SortBy.ToString()
            End If

            If Not (drpSortDirection.Items.FindByValue(ArticleSettings.SortDirection.ToString()) Is Nothing) Then
                drpSortDirection.SelectedValue = ArticleSettings.SortDirection.ToString()
            End If

            If (lstMenuPosition.Items.FindByValue(ArticleSettings.MenuPosition.ToString()) IsNot Nothing) Then
                lstMenuPosition.SelectedValue = ArticleSettings.MenuPosition.ToString()
            End If

        End Sub

        Private Sub SaveBasicSettings()

            Dim objModules As New ModuleController

            ' General Configuration
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.ENABLE_RATINGS_AUTHENTICATED_SETTING, chkEnableRatingsAuthenticated.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.ENABLE_RATINGS_ANONYMOUS_SETTING, chkEnableRatingsAnonymous.Checked.ToString())

            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.ENABLE_CORE_SEARCH_SETTING, chkEnableCoreSearch.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PROCESS_POST_TOKENS, chkProcessPostTokens.Checked.ToString())

            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.ENABLE_NOTIFICATION_PING_SETTING, chkEnableNotificationPing.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.ENABLE_AUTO_TRACKBACK_SETTING, chkEnableAutoTrackback.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.ENABLE_INCOMING_TRACKBACK_SETTING, chkEnableIncomingTrackback.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.LAUNCH_LINKS, chkLaunchLinks.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.BUBBLE_FEATURED_ARTICLES, chkBubbleFeaturedArticles.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.REQUIRE_CATEGORY, chkRequireCategory.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.DISPLAY_MODE, CType(System.Enum.Parse(GetType(DisplayType), drpDisplayType.SelectedValue), DisplayType).ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.PAGE_SIZE_SETTING, drpNumber.SelectedValue)
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.TEMPLATE_SETTING, drpTemplates.SelectedValue)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.SERVER_TIMEZONE, drpTimeZone.SelectedValue)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.SORT_BY, drpSortBy.SelectedValue)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.SORT_DIRECTION, drpSortDirection.SelectedValue)
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.MENU_POSITION_TYPE, lstMenuPosition.SelectedValue)

            ' Clear Cache
            DataCache.RemoveCache(TabModuleId.ToString() & TemplateConstants.LISTING_ITEM)
            DataCache.RemoveCache(TabModuleId.ToString() & TemplateConstants.LISTING_FEATURED)
            DataCache.RemoveCache(TabModuleId.ToString() & TemplateConstants.LISTING_HEADER)
            DataCache.RemoveCache(TabModuleId.ToString() & TemplateConstants.LISTING_FOOTER)

            DataCache.RemoveCache(TabModuleId.ToString() & TemplateConstants.VIEW_ITEM)
            DataCache.RemoveCache(TabModuleId.ToString() & TemplateConstants.VIEW_HEADER)
            DataCache.RemoveCache(TabModuleId.ToString() & TemplateConstants.VIEW_FOOTER)

            DataCache.RemoveCache(TabModuleId.ToString() & TemplateConstants.COMMENT_ITEM)
            DataCache.RemoveCache(TabModuleId.ToString() & TemplateConstants.COMMENT_HEADER)
            DataCache.RemoveCache(TabModuleId.ToString() & TemplateConstants.COMMENT_FOOTER)

            DataCache.RemoveCache(TabModuleId.ToString() & TemplateConstants.MENU_ITEM)

        End Sub

#End Region

#Region " Private Methods - Bind/Save Archive Settings "

        Private Sub BindArchiveSettings()

            chkArchiveCurrentArticles.Checked = ArticleSettings.ArchiveCurrentArticles
            chkArchiveCategories.Checked = ArticleSettings.ArchiveCategories
            chkArchiveAuthor.Checked = ArticleSettings.ArchiveAuthor
            chkArchiveMonth.Checked = ArticleSettings.ArchiveMonth

        End Sub

        Private Sub SaveArchiveSettings()

            Dim objModules As New ModuleController

            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.ARCHIVE_CURRENT_ARTICLES_SETTING, chkArchiveCurrentArticles.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.ARCHIVE_CATEGORIES_SETTING, chkArchiveCategories.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.ARCHIVE_AUTHOR_SETTING, chkArchiveAuthor.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.ARCHIVE_MONTH_SETTING, chkArchiveMonth.Checked.ToString())

        End Sub


#End Region

#Region " Private Methods - Bind/Save Category Settings "

        Private Sub BindCategorySettings()

            If (Settings.Contains(ArticleConstants.DEFAULT_CATEGORIES_SETTING)) Then
                If Not (Settings(ArticleConstants.DEFAULT_CATEGORIES_SETTING).ToString = Null.NullString) Then
                    Dim categories As String() = Settings(ArticleConstants.DEFAULT_CATEGORIES_SETTING).ToString().Split(Char.Parse(","))

                    For Each category As String In categories
                        If Not (lstDefaultCategories.Items.FindByValue(category) Is Nothing) Then
                            lstDefaultCategories.Items.FindByValue(category).Selected = True
                        End If
                    Next
                End If
            End If

            txtCategorySelectionHeight.Text = ArticleSettings.CategorySelectionHeight.ToString()
            chkCategoryBreadcrumb.Checked = ArticleSettings.CategoryBreadcrumb
            chkCategoryName.Checked = ArticleSettings.IncludeInPageName
            chkCategoryFilterSubmit.Checked = ArticleSettings.CategoryFilterSubmit

            If (lstCategorySortOrder.Items.FindByValue(ArticleSettings.CategorySortType.ToString()) IsNot Nothing) Then
                lstCategorySortOrder.SelectedValue = ArticleSettings.CategorySortType.ToString()
            End If

        End Sub

        Private Sub SaveCategorySettings()

            Dim objModules As New ModuleController

            Dim categories As String = ""
            For Each item As ListItem In lstDefaultCategories.Items
                If item.Selected Then
                    If (categories.Length > 0) Then
                        categories = categories & ","
                    End If
                    categories = categories & item.Value
                End If
            Next item

            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.DEFAULT_CATEGORIES_SETTING, categories)

            If (IsNumeric(txtCategorySelectionHeight.Text)) Then
                objModules.UpdateModuleSetting(ModuleId, ArticleConstants.CATEGORY_SELECTION_HEIGHT_SETTING, txtCategorySelectionHeight.Text)
            End If

            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.CATEGORY_BREADCRUMB_SETTING, chkCategoryBreadcrumb.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.CATEGORY_NAME_SETTING, chkCategoryName.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.CATEGORY_FILTER_SUBMIT_SETTING, chkCategoryFilterSubmit.Checked.ToString())

            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.CATEGORY_SORT_SETTING, lstCategorySortOrder.SelectedValue)

        End Sub

#End Region

        Private Sub BindCommentSettings()

            If (Settings.Contains(ArticleConstants.ENABLE_COMMENTS_SETTING)) Then
                chkEnableComments.Checked = Convert.ToBoolean(Settings(ArticleConstants.ENABLE_COMMENTS_SETTING).ToString())
            Else
                chkEnableComments.Checked = True
            End If

            If (Settings.Contains(ArticleConstants.ENABLE_ANONYMOUS_COMMENTS_SETTING)) Then
                chkEnableAnonymousComments.Checked = Convert.ToBoolean(Settings(ArticleConstants.ENABLE_ANONYMOUS_COMMENTS_SETTING).ToString())
            Else
                chkEnableAnonymousComments.Checked = True
            End If

            If (Settings.Contains(ArticleConstants.ENABLE_COMMENT_MODERATION_SETTING)) Then
                chkEnableCommentModeration.Checked = Convert.ToBoolean(Settings(ArticleConstants.ENABLE_COMMENT_MODERATION_SETTING).ToString())
            Else
                chkEnableCommentModeration.Checked = False
            End If

            If (Settings.Contains(ArticleConstants.COMMENT_HIDE_WEBSITE_SETTING)) Then
                chkHideWebsite.Checked = Convert.ToBoolean(Settings(ArticleConstants.COMMENT_HIDE_WEBSITE_SETTING).ToString())
            Else
                chkHideWebsite.Checked = False
            End If

            If (Settings.Contains(ArticleConstants.COMMENT_REQUIRE_NAME_SETTING)) Then
                chkRequireName.Checked = Convert.ToBoolean(Settings(ArticleConstants.COMMENT_REQUIRE_NAME_SETTING).ToString())
            Else
                chkRequireName.Checked = True
            End If

            If (Settings.Contains(ArticleConstants.COMMENT_REQUIRE_EMAIL_SETTING)) Then
                chkRequireEmail.Checked = Convert.ToBoolean(Settings(ArticleConstants.COMMENT_REQUIRE_EMAIL_SETTING).ToString())
            Else
                chkRequireEmail.Checked = True
            End If

            Dim selectCaptchaType As CaptchaType = CaptchaType.None
            If (Settings.Contains(ArticleConstants.USE_CAPTCHA_SETTING)) Then
                'there's an existing module setting, so make sure we don't change behavior for that
                If Convert.ToBoolean(Settings(ArticleConstants.USE_CAPTCHA_SETTING).ToString()) Then
                    selectCaptchaType = CaptchaType.DnnCore
                End If
            Else
                If (Settings.Contains(ArticleConstants.CAPTCHATYPE_SETTING)) Then
                    selectCaptchaType =  CType(System.Enum.Parse(GetType(CaptchaType), Settings(ArticleConstants.CAPTCHATYPE_SETTING)), CaptchaType)
                End If
            End If
            Dim selectedItem As ListItem = drpCaptchaType.Items.FindByValue(selectCaptchaType.ToString())
            drpCaptchaType.SelectedIndex = drpCaptchaType.Items.IndexOf(selectedItem)
            
            If (Settings.Contains(ArticleConstants.RECAPTCHA_SITEKEY_SETTING)) Then
                txtReCaptchaSiteKey.Text = Settings(ArticleConstants.RECAPTCHA_SITEKEY_SETTING).ToString()
            End If
            
            If (Settings.Contains(ArticleConstants.RECAPTCHA_SECRETKEY_SETTING)) Then
                txtReCaptchaSecretKey.Text = Settings(ArticleConstants.RECAPTCHA_SECRETKEY_SETTING).ToString()
            End If

            If (Settings.Contains(ArticleConstants.NOTIFY_DEFAULT_SETTING)) Then
                chkNotifyDefault.Checked = Convert.ToBoolean(Settings(ArticleConstants.NOTIFY_DEFAULT_SETTING).ToString())
            Else
                chkNotifyDefault.Checked = False
            End If

            If Not (drpSortDirectionComments.Items.FindByValue(ArticleSettings.SortDirectionComments.ToString()) Is Nothing) Then
                drpSortDirectionComments.SelectedValue = ArticleSettings.SortDirectionComments.ToString()
            End If

            If (Settings.Contains(ArticleConstants.COMMENT_AKISMET_SETTING)) Then
                txtAkismetKey.Text = Settings(ArticleConstants.COMMENT_AKISMET_SETTING).ToString()
            End If

        End Sub

        Private Sub SaveCommentSettings()

            Dim objModules As New ModuleController

            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.ENABLE_COMMENTS_SETTING, chkEnableComments.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.ENABLE_ANONYMOUS_COMMENTS_SETTING, chkEnableAnonymousComments.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.ENABLE_COMMENT_MODERATION_SETTING, chkEnableCommentModeration.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.COMMENT_HIDE_WEBSITE_SETTING, chkHideWebsite.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.COMMENT_REQUIRE_NAME_SETTING, chkRequireName.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.COMMENT_REQUIRE_EMAIL_SETTING, chkRequireEmail.Checked.ToString())
            'This is only for upgraded modules
            If Settings.ContainsKey(ArticleConstants.USE_CAPTCHA_SETTING) Then
                objModules.DeleteModuleSetting(ModuleId, ArticleConstants.USE_CAPTCHA_SETTING)
                objModules.DeleteTabModuleSetting(TabModuleId, ArticleConstants.USE_CAPTCHA_SETTING)
            End If
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.CAPTCHATYPE_SETTING, drpCaptchaType.SelectedValue)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.RECAPTCHA_SITEKEY_SETTING, txtReCaptchaSiteKey.Text)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.RECAPTCHA_SECRETKEY_SETTING, txtReCaptchaSecretKey.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NOTIFY_DEFAULT_SETTING, chkNotifyDefault.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.COMMENT_SORT_DIRECTION_SETTING, drpSortDirectionComments.SelectedValue)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.COMMENT_AKISMET_SETTING, txtAkismetKey.Text)

        End Sub

        Private Sub BindContentSharingSettings()

            If (Me.UserInfo.IsSuperUser = False) Then
                phContentSharing.Visible = False
            Else
                BindAvailableContentSharingPortals()
                BindSelectedContentSharingPortals()
            End If

        End Sub

        Private Sub SaveContentSharingSettings()

        End Sub

        Private Sub BindFilterSettings()

            If (ArticleSettings.MaxArticles <> Null.NullInteger) Then
                txtMaxArticles.Text = ArticleSettings.MaxArticles.ToString()
            End If

            If (ArticleSettings.MaxAge <> Null.NullInteger) Then
                txtMaxAge.Text = ArticleSettings.MaxAge.ToString()
            End If

            If (ArticleSettings.FilterSingleCategory = Null.NullInteger) Then
                If (ArticleSettings.FilterCategories IsNot Nothing) Then
                    For Each category As String In ArticleSettings.FilterCategories
                        If Not (lstCategories.Items.FindByValue(category) Is Nothing) Then
                            lstCategories.Items.FindByValue(category).Selected = True
                        End If
                    Next

                    If (ArticleSettings.MatchCategories = MatchOperatorType.MatchAny) Then
                        rdoMatchAny.Checked = True
                    End If

                    If (ArticleSettings.MatchCategories = MatchOperatorType.MatchAll) Then
                        rdoMatchAll.Checked = True
                    End If
                Else
                    rdoAllCategories.Checked = True
                End If
            Else
                rdoSingleCategory.Checked = True

                If (drpCategories.Items.FindByValue(ArticleSettings.FilterSingleCategory.ToString()) IsNot Nothing) Then
                    drpCategories.SelectedValue = ArticleSettings.FilterSingleCategory.ToString()
                End If
            End If

            chkShowPending.Checked = ArticleSettings.ShowPending

            chkShowFeaturedOnly.Checked = ArticleSettings.FeaturedOnly
            chkShowNotFeaturedOnly.Checked = ArticleSettings.NotFeaturedOnly

            chkShowSecuredOnly.Checked = ArticleSettings.SecuredOnly
            chkShowNotSecuredOnly.Checked = ArticleSettings.NotSecuredOnly

            If (ArticleSettings.Author <> Null.NullInteger) Then
                Dim objUserController As New DotNetNuke.Entities.Users.UserController
                Dim objUser As DotNetNuke.Entities.Users.UserInfo = objUserController.GetUser(Me.PortalId, ArticleSettings.Author)

                If Not (objUser Is Nothing) Then
                    lblAuthorFilter.Text = objUser.Username
                End If
            Else
                lblAuthorFilter.Text = Localization.GetString("SelectAuthor.Text", Me.LocalResourceFile)
            End If

            chkQueryStringFilter.Checked = ArticleSettings.AuthorUserIDFilter
            txtQueryStringParam.Text = ArticleSettings.AuthorUserIDParam
            chkUsernameFilter.Checked = ArticleSettings.AuthorUsernameFilter
            txtUsernameParam.Text = ArticleSettings.AuthorUsernameParam
            chkLoggedInUser.Checked = ArticleSettings.AuthorLoggedInUserFilter

        End Sub

        Private Sub SaveFilterSettings()

            Dim objModules As New ModuleController

            If (txtMaxArticles.Text <> "") Then
                objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.MAX_ARTICLES_SETTING, txtMaxArticles.Text)
            Else
                objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.MAX_ARTICLES_SETTING, Null.NullInteger.ToString())
            End If

            If (txtMaxAge.Text <> "") Then
                If (IsNumeric(txtMaxAge.Text)) Then
                    objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.MAX_AGE_SETTING, txtMaxAge.Text)
                Else
                    objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.MAX_AGE_SETTING, Null.NullInteger.ToString())
                End If
            Else
                objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.MAX_AGE_SETTING, Null.NullInteger.ToString())
            End If

            If (rdoAllCategories.Checked) Then
                objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.CATEGORIES_SETTING & Me.TabId.ToString(), Null.NullInteger.ToString())
            End If

            If (rdoSingleCategory.Checked) Then
                If (drpCategories.SelectedValue <> "") Then
                    objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.CATEGORIES_FILTER_SINGLE_SETTING, drpCategories.SelectedValue)
                Else
                    objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.CATEGORIES_FILTER_SINGLE_SETTING, Null.NullInteger.ToString())
                End If
            Else
                objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.CATEGORIES_FILTER_SINGLE_SETTING, Null.NullInteger.ToString())
            End If

            If (rdoMatchAny.Checked Or rdoMatchAll.Checked) Then
                Dim categories As String = ""
                For Each item As ListItem In lstCategories.Items
                    If item.Selected Then
                        If (categories.Length > 0) Then
                            categories = categories & ","
                        End If
                        categories = categories & item.Value
                    End If
                Next item
                objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.CATEGORIES_SETTING & Me.TabId.ToString(), categories)

                If (rdoMatchAny.Checked) Then
                    objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.MATCH_OPERATOR_SETTING, MatchOperatorType.MatchAny.ToString())
                End If

                If (rdoMatchAll.Checked) Then
                    objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.MATCH_OPERATOR_SETTING, MatchOperatorType.MatchAll.ToString())
                End If
            End If

            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.SHOW_PENDING_SETTING, chkShowPending.Checked.ToString())

            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.SHOW_FEATURED_ONLY_SETTING, chkShowFeaturedOnly.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.SHOW_NOT_FEATURED_ONLY_SETTING, chkShowNotFeaturedOnly.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.SHOW_SECURED_ONLY_SETTING, chkShowSecuredOnly.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.SHOW_NOT_SECURED_ONLY_SETTING, chkShowNotSecuredOnly.Checked.ToString())

            If (ddlAuthor.Visible) Then
                If (ddlAuthor.Items.Count > 0) Then
                    objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.AUTHOR_SETTING, ddlAuthor.SelectedValue)
                End If
            End If

            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.AUTHOR_USERID_FILTER_SETTING, chkQueryStringFilter.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.AUTHOR_USERID_PARAM_SETTING, txtQueryStringParam.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.AUTHOR_USERNAME_FILTER_SETTING, chkUsernameFilter.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.AUTHOR_USERNAME_PARAM_SETTING, txtUsernameParam.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.AUTHOR_LOGGED_IN_USER_FILTER_SETTING, chkLoggedInUser.Checked.ToString())

        End Sub

        Private Sub BindFormSettings()

            If (lstAuthorSelection.Items.FindByValue(ArticleSettings.AuthorSelect.ToString()) IsNot Nothing) Then
                lstAuthorSelection.SelectedValue = ArticleSettings.AuthorSelect.ToString()
            End If

            If (ArticleSettings.AuthorDefault <> Null.NullInteger) Then
                Dim objUserController As New DotNetNuke.Entities.Users.UserController
                Dim objUser As DotNetNuke.Entities.Users.UserInfo = objUserController.GetUser(Me.PortalId, ArticleSettings.AuthorDefault)

                If Not (objUser Is Nothing) Then
                    lblAuthorDefault.Text = objUser.Username
                End If
            End If

            chkExpandSummary.Checked = ArticleSettings.ExpandSummary
            txtTextEditorWidth.Text = ArticleSettings.TextEditorWidth
            txtTextEditorHeight.Text = ArticleSettings.TextEditorHeight
            If (lstTextEditorSummaryMode.Items.FindByValue(ArticleSettings.TextEditorSummaryMode.ToString()) IsNot Nothing) Then
                lstTextEditorSummaryMode.SelectedValue = ArticleSettings.TextEditorSummaryMode.ToString()
            End If
            txtTextEditorSummaryWidth.Text = ArticleSettings.TextEditorSummaryWidth
            txtTextEditorSummaryHeight.Text = ArticleSettings.TextEditorSummaryHeight

        End Sub

        Private Sub SaveFormSettings()

            Dim objModules As New ModuleController

            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.AUTHOR_SELECT_TYPE, lstAuthorSelection.SelectedValue)
            If (drpAuthorDefault.Visible) Then
                If (drpAuthorDefault.Items.Count > 0) Then
                    objModules.UpdateModuleSetting(ModuleId, ArticleConstants.AUTHOR_DEFAULT_SETTING, drpAuthorDefault.SelectedValue)
                End If
            End If
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.EXPAND_SUMMARY_SETTING, chkExpandSummary.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.TEXT_EDITOR_WIDTH, txtTextEditorWidth.Text)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.TEXT_EDITOR_HEIGHT, txtTextEditorHeight.Text)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.TEXT_EDITOR_SUMMARY_MODE, lstTextEditorSummaryMode.SelectedValue)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.TEXT_EDITOR_SUMMARY_WIDTH, txtTextEditorSummaryWidth.Text)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.TEXT_EDITOR_SUMMARY_HEIGHT, txtTextEditorSummaryHeight.Text)

        End Sub

        Private Sub BindImageSettings()

            chkIncludeJQuery.Checked = ArticleSettings.IncludeJQuery
            txtJQueryPath.Text = Me.ArticleSettings.ImageJQueryPath
            chkEnableImagesUpload.Checked = ArticleSettings.EnableImagesUpload
            chkEnablePortalImages.Checked = ArticleSettings.EnablePortalImages
            chkEnableExternalImages.Checked = ArticleSettings.EnableExternalImages
            If (drpDefaultImageFolder.Items.FindByValue(ArticleSettings.DefaultImagesFolder.ToString) IsNot Nothing) Then
                drpDefaultImageFolder.SelectedValue = ArticleSettings.DefaultImagesFolder.ToString
            End If
            chkResizeImages.Checked = ArticleSettings.ResizeImages
            If (rdoThumbnailType.Items.FindByValue(ArticleSettings.ImageThumbnailType.ToString) IsNot Nothing) Then
                rdoThumbnailType.SelectedValue = ArticleSettings.ImageThumbnailType.ToString
            Else
                rdoThumbnailType.SelectedIndex = 0
            End If
            txtImageMaxWidth.Text = ArticleSettings.MaxImageWidth.ToString()
            txtImageMaxHeight.Text = ArticleSettings.MaxImageHeight.ToString()

            chkUseWatermark.Checked = Me.ArticleSettings.WatermarkEnabled
            txtWatermarkText.Text = Me.ArticleSettings.WatermarkText
            ctlWatermarkImage.Url = Me.ArticleSettings.WatermarkImage
            If Not (drpWatermarkPosition.Items.FindByValue(ArticleSettings.WatermarkPosition.ToString()) Is Nothing) Then
                drpWatermarkPosition.SelectedValue = ArticleSettings.WatermarkPosition.ToString()
            End If

        End Sub

        Private Sub SaveImageSettings()

            Dim objModules As New ModuleController

            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.INCLUDE_JQUERY_SETTING, chkIncludeJQuery.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.IMAGE_JQUERY_PATH, txtJQueryPath.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.ENABLE_UPLOAD_IMAGES_SETTING, chkEnableImagesUpload.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.ENABLE_PORTAL_IMAGES_SETTING, chkEnablePortalImages.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.ENABLE_EXTERNAL_IMAGES_SETTING, chkEnableExternalImages.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.DEFAULT_IMAGES_FOLDER_SETTING, drpDefaultImageFolder.SelectedValue)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.IMAGE_RESIZE_SETTING, chkResizeImages.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.IMAGE_THUMBNAIL_SETTING, rdoThumbnailType.SelectedValue)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.IMAGE_MAX_WIDTH_SETTING, txtImageMaxWidth.Text)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.IMAGE_MAX_HEIGHT_SETTING, txtImageMaxHeight.Text)

            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.IMAGE_WATERMARK_ENABLED_SETTING, chkUseWatermark.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.IMAGE_WATERMARK_TEXT_SETTING, txtWatermarkText.Text)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.IMAGE_WATERMARK_IMAGE_SETTING, ctlWatermarkImage.Url)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.IMAGE_WATERMARK_IMAGE_POSITION_SETTING, drpWatermarkPosition.SelectedValue)

        End Sub

        Private Sub BindFileSettings()

            If (drpDefaultFileFolder.Items.FindByValue(ArticleSettings.DefaultFilesFolder.ToString) IsNot Nothing) Then
                drpDefaultFileFolder.SelectedValue = ArticleSettings.DefaultFilesFolder.ToString
            End If
            chkEnablePortalFiles.Checked = ArticleSettings.EnablePortalFiles

        End Sub

        Private Sub SaveFileSettings()

            Dim objModules As New ModuleController
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.DEFAULT_FILES_FOLDER_SETTING, drpDefaultFileFolder.SelectedValue)
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.ENABLE_PORTAL_FILES_SETTING, chkEnablePortalFiles.Checked.ToString())

        End Sub

        Private Sub BindSecuritySettings()

            txtSecureUrl.Text = ArticleSettings.SecureUrl.ToString()
            If (ArticleSettings.RoleGroupID <> Null.NullInteger) Then
                If (drpSecurityRoleGroups.Items.FindByValue(ArticleSettings.RoleGroupID.ToString()) IsNot Nothing) Then
                    drpSecurityRoleGroups.SelectedValue = ArticleSettings.RoleGroupID.ToString()
                End If
            End If

            BindRoles()

        End Sub

        Private Sub SaveSecuritySettings()

            Dim objModules As New ModuleController

            Dim submitRoles As String = ""
            Dim secureRoles As String = ""
            Dim autoSecureRoles As String = ""
            Dim approveRoles As String = ""
            Dim autoApproveRoles As String = ""
            Dim autoApproveCommentRoles As String = ""
            Dim featureRoles As String = ""
            Dim autoFeatureRoles As String = ""
            For Each item As DataGridItem In grdBasicPermissions.Items
                Dim role As String = grdBasicPermissions.DataKeys(item.ItemIndex).ToString()

                Dim chkSubmit As CheckBox = CType(item.FindControl("chkSubmit"), CheckBox)
                If (chkSubmit.Checked) Then
                    If (submitRoles = "") Then
                        submitRoles = role
                    Else
                        submitRoles = submitRoles & ";" & role
                    End If
                End If

                Dim chkSecure As CheckBox = CType(item.FindControl("chkSecure"), CheckBox)
                If (chkSecure.Checked) Then
                    If (secureRoles = "") Then
                        secureRoles = role
                    Else
                        secureRoles = secureRoles & ";" & role
                    End If
                End If

                Dim chkAutoSecure As CheckBox = CType(item.FindControl("chkAutoSecure"), CheckBox)
                If (chkAutoSecure.Checked) Then
                    If (autoSecureRoles = "") Then
                        autoSecureRoles = role
                    Else
                        autoSecureRoles = autoSecureRoles & ";" & role
                    End If
                End If

                Dim chkApprove As CheckBox = CType(item.FindControl("chkApprove"), CheckBox)
                If (chkApprove.Checked) Then
                    If (approveRoles = "") Then
                        approveRoles = role
                    Else
                        approveRoles = approveRoles & ";" & role
                    End If
                End If

                Dim chkAutoApproveArticle As CheckBox = CType(item.FindControl("chkAutoApproveArticle"), CheckBox)
                If (chkAutoApproveArticle.Checked) Then
                    If (autoApproveRoles = "") Then
                        autoApproveRoles = role
                    Else
                        autoApproveRoles = autoApproveRoles & ";" & role
                    End If
                End If

                Dim chkAutoApproveComment As CheckBox = CType(item.FindControl("chkAutoApproveComment"), CheckBox)
                If (chkAutoApproveComment.Checked) Then
                    If (autoApproveCommentRoles = "") Then
                        autoApproveCommentRoles = role
                    Else
                        autoApproveCommentRoles = autoApproveCommentRoles & ";" & role
                    End If
                End If

                Dim chkFeature As CheckBox = CType(item.FindControl("chkFeature"), CheckBox)
                If (chkFeature.Checked) Then
                    If (featureRoles = "") Then
                        featureRoles = role
                    Else
                        featureRoles = featureRoles & ";" & role
                    End If
                End If

                Dim chkAutoFeature As CheckBox = CType(item.FindControl("chkAutoFeature"), CheckBox)
                If (chkAutoFeature.Checked) Then
                    If (autoFeatureRoles = "") Then
                        autoFeatureRoles = role
                    Else
                        autoFeatureRoles = autoFeatureRoles & ";" & role
                    End If
                End If
            Next
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_SUBMISSION_SETTING, submitRoles)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_SECURE_SETTING, secureRoles)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_AUTO_SECURE_SETTING, autoSecureRoles)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_APPROVAL_SETTING, approveRoles)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_AUTO_APPROVAL_SETTING, autoApproveRoles)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_AUTO_APPROVAL_COMMENT_SETTING, autoApproveCommentRoles)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_FEATURE_SETTING, featureRoles)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_AUTO_FEATURE_SETTING, autoFeatureRoles)

            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_ROLE_GROUP_ID, drpSecurityRoleGroups.SelectedValue)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_SECURE_URL_SETTING, txtSecureUrl.Text)

            Dim categoriesRoles As String = ""
            Dim excerptRoles As String = ""
            Dim imageRoles As String = ""
            Dim fileRoles As String = ""
            Dim linkRoles As String = ""
            Dim publishRoles As String = ""
            Dim expiryRoles As String = ""
            Dim metaRoles As String = ""
            Dim customRoles As String = ""

            For Each item As DataGridItem In grdFormPermissions.Items
                Dim role As String = grdFormPermissions.DataKeys(item.ItemIndex).ToString()

                Dim chkCategories As CheckBox = CType(item.FindControl("chkCategories"), CheckBox)
                If (chkCategories.Checked) Then
                    If (categoriesRoles = "") Then
                        categoriesRoles = role
                    Else
                        categoriesRoles = categoriesRoles & ";" & role
                    End If
                End If

                Dim chkExcerpt As CheckBox = CType(item.FindControl("chkExcerpt"), CheckBox)
                If (chkExcerpt.Checked) Then
                    If (excerptRoles = "") Then
                        excerptRoles = role
                    Else
                        excerptRoles = excerptRoles & ";" & role
                    End If
                End If

                Dim chkImage As CheckBox = CType(item.FindControl("chkImage"), CheckBox)
                If (chkImage.Checked) Then
                    If (imageRoles = "") Then
                        imageRoles = role
                    Else
                        imageRoles = imageRoles & ";" & role
                    End If
                End If

                Dim chkFile As CheckBox = CType(item.FindControl("chkFile"), CheckBox)
                If (chkFile.Checked) Then
                    If (fileRoles = "") Then
                        fileRoles = role
                    Else
                        fileRoles = fileRoles & ";" & role
                    End If
                End If

                Dim chkLink As CheckBox = CType(item.FindControl("chkLink"), CheckBox)
                If (chkLink.Checked) Then
                    If (linkRoles = "") Then
                        linkRoles = role
                    Else
                        linkRoles = linkRoles & ";" & role
                    End If
                End If

                Dim chkPublishDate As CheckBox = CType(item.FindControl("chkPublishDate"), CheckBox)
                If (chkPublishDate.Checked) Then
                    If (publishRoles = "") Then
                        publishRoles = role
                    Else
                        publishRoles = publishRoles & ";" & role
                    End If
                End If

                Dim chkExpiryDate As CheckBox = CType(item.FindControl("chkExpiryDate"), CheckBox)
                If (chkExpiryDate.Checked) Then
                    If (expiryRoles = "") Then
                        expiryRoles = role
                    Else
                        expiryRoles = expiryRoles & ";" & role
                    End If
                End If

                Dim chkMeta As CheckBox = CType(item.FindControl("chkMeta"), CheckBox)
                If (chkMeta.Checked) Then
                    If (metaRoles = "") Then
                        metaRoles = role
                    Else
                        metaRoles = metaRoles & ";" & role
                    End If
                End If

                Dim chkCustom As CheckBox = CType(item.FindControl("chkCustom"), CheckBox)
                If (chkCustom.Checked) Then
                    If (customRoles = "") Then
                        customRoles = role
                    Else
                        customRoles = customRoles & ";" & role
                    End If
                End If
            Next
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_CATEGORIES_SETTING, IIf(String.IsNullOrEmpty(categoriesRoles), ";", categoriesRoles))
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_EXCERPT_SETTING, IIf(String.IsNullOrEmpty(excerptRoles), ";", excerptRoles))
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_IMAGE_SETTING, IIf(String.IsNullOrEmpty(imageRoles), ";", imageRoles))
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_FILE_SETTING, IIf(String.IsNullOrEmpty(fileRoles), ";", fileRoles))
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_LINK_SETTING, IIf(String.IsNullOrEmpty(linkRoles), ";", linkRoles))
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_PUBLISH_SETTING, IIf(String.IsNullOrEmpty(publishRoles), ";", publishRoles))
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_EXPIRY_SETTING, IIf(String.IsNullOrEmpty(expiryRoles), ";", expiryRoles))
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_META_SETTING, IIf(String.IsNullOrEmpty(metaRoles), ";", metaRoles))
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_CUSTOM_SETTING, IIf(String.IsNullOrEmpty(customRoles), ";", customRoles))

            Dim siteTemplatesRoles As String = ""
            For Each item As DataGridItem In grdAdminPermissions.Items
                Dim role As String = grdAdminPermissions.DataKeys(item.ItemIndex).ToString()

                Dim chkSiteTemplates As CheckBox = CType(item.FindControl("chkSiteTemplates"), CheckBox)
                If (chkSiteTemplates.Checked) Then
                    If (siteTemplatesRoles = "") Then
                        siteTemplatesRoles = role
                    Else
                        siteTemplatesRoles = siteTemplatesRoles & ";" & role
                    End If
                End If
            Next
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.PERMISSION_SITE_TEMPLATES_SETTING, siteTemplatesRoles)

        End Sub

        Private Sub BindNotificationSettings()

            If (Settings.Contains(ArticleConstants.NOTIFY_SUBMISSION_SETTING)) Then
                chkNotifySubmission.Checked = Convert.ToBoolean(Settings(ArticleConstants.NOTIFY_SUBMISSION_SETTING).ToString())
            Else
                chkNotifySubmission.Checked = True
            End If

            If (Settings.Contains(ArticleConstants.NOTIFY_SUBMISSION_SETTING_EMAIL)) Then
                txtSubmissionEmail.Text = Settings(ArticleConstants.NOTIFY_SUBMISSION_SETTING_EMAIL).ToString()
            End If

            If (Settings.Contains(ArticleConstants.NOTIFY_APPROVAL_SETTING)) Then
                chkNotifyApproval.Checked = Convert.ToBoolean(Settings(ArticleConstants.NOTIFY_APPROVAL_SETTING).ToString())
            Else
                chkNotifyApproval.Checked = True
            End If

            If (Settings.Contains(ArticleConstants.NOTIFY_COMMENT_SETTING)) Then
                chkNotifyComment.Checked = Convert.ToBoolean(Settings(ArticleConstants.NOTIFY_COMMENT_SETTING).ToString())
            Else
                chkNotifyComment.Checked = True
            End If

            If (Settings.Contains(ArticleConstants.NOTIFY_COMMENT_SETTING_EMAIL)) Then
                txtNotifyCommentEmail.Text = Settings(ArticleConstants.NOTIFY_COMMENT_SETTING_EMAIL).ToString()
            End If

            If (Settings.Contains(ArticleConstants.NOTIFY_COMMENT_APPROVAL_SETTING)) Then
                chkNotifyCommentApproval.Checked = Convert.ToBoolean(Settings(ArticleConstants.NOTIFY_COMMENT_APPROVAL_SETTING).ToString())
            Else
                chkNotifyCommentApproval.Checked = True
            End If

            If (Settings.Contains(ArticleConstants.NOTIFY_COMMENT_APPROVAL_EMAIL_SETTING)) Then
                txtNotifyCommentApprovalEmail.Text = Settings(ArticleConstants.NOTIFY_COMMENT_APPROVAL_EMAIL_SETTING).ToString()
            End If

        End Sub

        Private Sub SaveNotificationSettings()

            Dim objModules As New ModuleController

            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.NOTIFY_SUBMISSION_SETTING, chkNotifySubmission.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.NOTIFY_SUBMISSION_SETTING_EMAIL, txtSubmissionEmail.Text.Trim().ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.NOTIFY_APPROVAL_SETTING, chkNotifyApproval.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.NOTIFY_COMMENT_SETTING, chkNotifyComment.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.NOTIFY_COMMENT_SETTING_EMAIL, txtNotifyCommentEmail.Text.Trim().ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.NOTIFY_COMMENT_APPROVAL_SETTING, chkNotifyCommentApproval.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.NOTIFY_COMMENT_APPROVAL_EMAIL_SETTING, txtNotifyCommentApprovalEmail.Text.Trim().ToString())

        End Sub

        Private Sub BindRelatedSettings()

            If (lstRelatedMode.Items.FindByValue(RelatedType.MatchCategoriesAnyTagsAny.ToString()) IsNot Nothing) Then
                lstRelatedMode.SelectedValue = RelatedType.MatchCategoriesAnyTagsAny.ToString()
            End If

            If (Settings.Contains(ArticleConstants.RELATED_MODE)) Then
                If (lstRelatedMode.Items.FindByValue(Settings(ArticleConstants.RELATED_MODE).ToString()) IsNot Nothing) Then
                    lstRelatedMode.SelectedValue = Settings(ArticleConstants.RELATED_MODE).ToString()
                End If
            End If

        End Sub

        Private Sub SaveRelatedSettings()

            Dim objModules As New ModuleController
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.RELATED_MODE, lstRelatedMode.SelectedValue.ToString())

        End Sub

        Private Sub BindSEOSettings()

            If lstTitleReplacement.Items.FindByValue(ArticleSettings.TitleReplacement.ToString()) IsNot Nothing Then
                lstTitleReplacement.SelectedValue = ArticleSettings.TitleReplacement.ToString()
            End If

            chkAlwaysShowPageID.Checked = ArticleSettings.AlwaysShowPageID

            If lstUrlMode.Items.FindByValue(ArticleSettings.UrlModeType.ToString()) IsNot Nothing Then
                lstUrlMode.SelectedValue = ArticleSettings.UrlModeType.ToString()
            End If

            txtShorternID.Text = ArticleSettings.ShortenedID

            chkUseCanonicalLink.Checked = ArticleSettings.UseCanonicalLink
            chkExpandMetaInformation.Checked = ArticleSettings.ExpandMetaInformation
            chkUniquePageTitles.Checked = ArticleSettings.UniquePageTitles

        End Sub

        Private Sub SaveSEOSettings()

            Dim objModules As New ModuleController
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.TITLE_REPLACEMENT_TYPE, lstTitleReplacement.SelectedValue)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.SEO_ALWAYS_SHOW_PAGEID_SETTING, chkAlwaysShowPageID.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.SEO_URL_MODE_SETTING, lstUrlMode.SelectedValue)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.SEO_SHORTEN_ID_SETTING, txtShorternID.Text)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.SEO_USE_CANONICAL_LINK_SETTING, chkUseCanonicalLink.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.SEO_EXPAND_META_INFORMATION_SETTING, chkExpandMetaInformation.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.SEO_UNIQUE_PAGE_TITLES_SETTING, chkUniquePageTitles.Checked.ToString())

        End Sub

        Private Sub BindSyndicationSettings()

            chkEnableSyndication.Checked = ArticleSettings.EnableSyndication
            chkEnableSyndicationEnclosures.Checked = ArticleSettings.EnableSyndicationEnclosures
            chkEnableSyndicationHtml.Checked = ArticleSettings.EnableSyndicationHtml

            If (lstSyndicationLinkMode.Items.FindByValue(ArticleSettings.SyndicationLinkType.ToString()) IsNot Nothing) Then
                lstSyndicationLinkMode.SelectedValue = ArticleSettings.SyndicationLinkType.ToString()
            End If

            If (lstSyndicationEnclosureType.Items.FindByValue(ArticleSettings.SyndicationEnclosureType.ToString()) IsNot Nothing) Then
                lstSyndicationEnclosureType.SelectedValue = ArticleSettings.SyndicationEnclosureType.ToString()
            End If

            If (ArticleSettings.SyndicationSummaryLength <> Null.NullInteger) Then
                txtSyndicationSummaryLength.Text = ArticleSettings.SyndicationSummaryLength.ToString()
            End If

            txtSyndicationMaxCount.Text = ArticleSettings.SyndicationMaxCount.ToString()
            txtSyndicationImagePath.Text = ArticleSettings.SyndicationImagePath

        End Sub

        Private Sub SaveSyndicationSettings()

            Dim objModules As New ModuleController

            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.ENABLE_SYNDICATION_SETTING, chkEnableSyndication.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.ENABLE_SYNDICATION_ENCLOSURES_SETTING, chkEnableSyndicationEnclosures.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.ENABLE_SYNDICATION_HTML_SETTING, chkEnableSyndicationHtml.Checked.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.SYNDICATION_LINK_TYPE, lstSyndicationLinkMode.SelectedValue.ToString())
            objModules.UpdateTabModuleSetting(TabModuleId, ArticleConstants.SYNDICATION_ENCLOSURE_TYPE, lstSyndicationEnclosureType.SelectedValue.ToString())

            objModules.DeleteModuleSetting(ModuleId, ArticleConstants.SYNDICATION_SUMMARY_LENGTH)
            If (txtSyndicationSummaryLength.Text <> "") Then
                If (Convert.ToInt32(txtSyndicationSummaryLength.Text) > 0) Then
                    objModules.UpdateModuleSetting(ModuleId, ArticleConstants.SYNDICATION_SUMMARY_LENGTH, txtSyndicationSummaryLength.Text)
                End If
            End If

            Try
                objModules.UpdateModuleSetting(ModuleId, ArticleConstants.SYNDICATION_MAX_COUNT, txtSyndicationMaxCount.Text)
            Catch
            End Try

            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.SYNDICATION_IMAGE_PATH, txtSyndicationImagePath.Text)

        End Sub

        Private Sub BindTwitterSettings()

            If (Settings.Contains(ArticleConstants.TWITTER_USERNAME)) Then
                txtTwitterName.Text = Settings(ArticleConstants.TWITTER_USERNAME).ToString()
            End If

            If (Settings.Contains(ArticleConstants.TWITTER_BITLY_LOGIN)) Then
                txtBitLyLogin.Text = Settings(ArticleConstants.TWITTER_BITLY_LOGIN).ToString()
            End If

            If (Settings.Contains(ArticleConstants.TWITTER_BITLY_API_KEY)) Then
                txtBitLyAPIKey.Text = Settings(ArticleConstants.TWITTER_BITLY_API_KEY).ToString()
            End If

        End Sub

        Private Sub SaveTwitterSettings()

            Dim objModules As New ModuleController
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.TWITTER_USERNAME, txtTwitterName.Text)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.TWITTER_BITLY_LOGIN, txtBitLyLogin.Text)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.TWITTER_BITLY_API_KEY, txtBitLyAPIKey.Text)

        End Sub

        Private Sub BindThirdPartySettings()

            chkJournalIntegration.Checked = ArticleSettings.JournalIntegration
            chkJournalIntegrationGroups.Checked = ArticleSettings.JournalIntegrationGroups

            If (Settings.Contains(ArticleConstants.ACTIVE_SOCIAL_SETTING)) Then
                chkActiveSocial.Checked = Convert.ToBoolean(Settings(ArticleConstants.ACTIVE_SOCIAL_SETTING).ToString())
            Else
                chkActiveSocial.Checked = False
            End If

            txtActiveSocialSubmissionKey.Text = ArticleSettings.ActiveSocialSubmitKey
            txtActiveSocialCommentKey.Text = ArticleSettings.ActiveSocialCommentKey
            txtActiveSocialRateKey.Text = ArticleSettings.ActiveSocialRateKey

            If (Settings.Contains(ArticleConstants.SMART_THINKER_STORY_FEED_SETTING)) Then
                chkSmartThinkerStoryFeed.Checked = Convert.ToBoolean(Settings(ArticleConstants.SMART_THINKER_STORY_FEED_SETTING).ToString())
            Else
                chkSmartThinkerStoryFeed.Checked = False
            End If

        End Sub

        Private Sub SaveThirdPartySettings()

            Dim objModules As New ModuleController

            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.JOURNAL_INTEGRATION_SETTING, chkJournalIntegration.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.JOURNAL_INTEGRATION_GROUPS_SETTING, chkJournalIntegrationGroups.Checked.ToString())

            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.ACTIVE_SOCIAL_SETTING, chkActiveSocial.Checked.ToString())
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.ACTIVE_SOCIAL_SUBMIT_SETTING, txtActiveSocialSubmissionKey.Text)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.ACTIVE_SOCIAL_COMMENT_SETTING, txtActiveSocialCommentKey.Text)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.ACTIVE_SOCIAL_RATE_SETTING, txtActiveSocialRateKey.Text)
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.SMART_THINKER_STORY_FEED_SETTING, chkSmartThinkerStoryFeed.Checked.ToString())

        End Sub

        Private Function IsInRole(ByVal roleName As String, ByVal roles As String()) As Boolean

            For Each role As String In roles
                If (roleName = role) Then
                    Return True
                End If
            Next

            Return False

        End Function

        Private Sub PopulateAuthorList()

            ddlAuthor.DataSource = GetAuthorList(Me.ModuleId)
            ddlAuthor.DataBind()
            SortDropDown(ddlAuthor)
            ddlAuthor.Items.Insert(0, New ListItem(Localization.GetString("SelectAuthor.Text", Me.LocalResourceFile), "-1"))

            If Not (ddlAuthor.Items.FindByValue(Me.ArticleSettings.Author.ToString()) Is Nothing) Then
                ddlAuthor.SelectedValue = Me.ArticleSettings.Author.ToString()
            End If

        End Sub

        Private Sub PopulateAuthorListDefault()

            drpAuthorDefault.DataSource = GetAuthorList(Me.ModuleId)
            drpAuthorDefault.DataBind()
            SortDropDown(drpAuthorDefault)
            drpAuthorDefault.Items.Insert(0, New ListItem(Localization.GetString("NoDefault.Text", Me.LocalResourceFile), "-1"))

            If Not (drpAuthorDefault.Items.FindByValue(Me.ArticleSettings.AuthorDefault.ToString()) Is Nothing) Then
                drpAuthorDefault.SelectedValue = Me.ArticleSettings.AuthorDefault.ToString()
            End If

        End Sub

        Public Function GetAuthorList(ByVal moduleID As Integer) As ArrayList

            Dim moduleSettings As Hashtable = Common.GetModuleSettings(moduleID)
            Dim distributionList As String = ""
            Dim userList As New ArrayList

            If (moduleSettings.Contains(ArticleConstants.PERMISSION_SUBMISSION_SETTING)) Then

                Dim roles As String = moduleSettings(ArticleConstants.PERMISSION_SUBMISSION_SETTING).ToString()
                Dim rolesArray() As String = roles.Split(Convert.ToChar(";"))
                Dim userIDs As New Hashtable

                For Each role As String In rolesArray
                    If (role.Length > 0) Then

                        Dim objRoleController As New DotNetNuke.Security.Roles.RoleController
                        Dim objRole As DotNetNuke.Security.Roles.RoleInfo = objRoleController.GetRoleByName(PortalSettings.PortalId, role)

                        If Not (objRole Is Nothing) Then
                            Dim objUsers As List(Of UserInfo) = RoleController.Instance.GetUsersByRole(PortalSettings.PortalId, objRole.RoleName)
                            For Each objUser As UserInfo In objUsers
                                If (userIDs.Contains(objUser.UserID) = False) Then
                                    Dim objUserController As UserController = New DotNetNuke.Entities.Users.UserController
                                    Dim objSelectedUser As UserInfo = objUserController.GetUser(PortalSettings.PortalId, objUser.UserID)
                                    If Not (objSelectedUser Is Nothing) Then
                                        If (objSelectedUser.Email.Length > 0) Then
                                            userIDs.Add(objUser.UserID, objUser.UserID)
                                            userList.Add(objSelectedUser)
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                Next

            End If

            Return userList

        End Function

        Private Sub BindAvailableContentSharingPortals()

            drpContentSharingPortals.Items.Clear()

            Dim objContentSharingPortals As List(Of ContentSharingInfo) = GetContentSharingPortals(ArticleSettings.ContentSharingPortals)

            Dim objPortalController As New PortalController()
            Dim objPortals As ArrayList = objPortalController.GetPortals()

            For Each objPortal As PortalInfo In objPortals

                If (objPortal.PortalID <> Me.PortalId) Then

                    Dim objDesktopModuleInfo As DesktopModuleInfo = DesktopModuleController.GetDesktopModuleByModuleName("DnnForge - NewsArticles", PortalSettings.PortalId)

                    If Not (objDesktopModuleInfo Is Nothing) Then

                        Dim objTabController As New DotNetNuke.Entities.Tabs.TabController()
                        Dim objTabs As TabCollection = objTabController.GetTabsByPortal(objPortal.PortalID)
                        For Each objTab As DotNetNuke.Entities.Tabs.TabInfo In objTabs.Values
                            If Not (objTab Is Nothing) Then
                                If (objTab.IsDeleted = False) Then
                                    Dim objModules As New ModuleController
                                    For Each pair As KeyValuePair(Of Integer, ModuleInfo) In objModules.GetTabModules(objTab.TabID)
                                        Dim objModule As ModuleInfo = pair.Value
                                        If (objModule.IsDeleted = False) Then
                                            If (objModule.DesktopModuleID = objDesktopModuleInfo.DesktopModuleID) Then
                                                If objModule.IsDeleted = False Then
                                                    Dim strPath As String = objTab.TabName
                                                    Dim objTabSelected As DotNetNuke.Entities.Tabs.TabInfo = objTab
                                                    While objTabSelected.ParentId <> Null.NullInteger
                                                        objTabSelected = objTabController.GetTab(objTabSelected.ParentId, objTab.PortalID, False)
                                                        If (objTabSelected Is Nothing) Then
                                                            Exit While
                                                        End If
                                                        strPath = objTabSelected.TabName & " -> " & strPath
                                                    End While

                                                    Dim add As Boolean = True

                                                    For Each objContentSharingPortal As ContentSharingInfo In objContentSharingPortals
                                                        If (objContentSharingPortal.LinkedModuleID = objModule.ModuleID And objContentSharingPortal.LinkedPortalID = objPortal.PortalID) Then
                                                            add = False
                                                            Exit For
                                                        End If
                                                    Next

                                                    If (add) Then
                                                        Dim objListItem As New ListItem
                                                        objListItem.Value = objPortal.PortalID.ToString() & "-" & objTab.TabID.ToString() & "-" & objModule.ModuleID.ToString()
                                                        Dim aliases As IEnumerable(Of PortalAliasInfo) = PortalAliasController.Instance.GetPortalAliasesByPortalId(objPortal.PortalID)
                                                        If (aliases.Count > 0) Then
                                                            objListItem.Text = DotNetNuke.Common.AddHTTP(aliases(0).HTTPAlias) & " -> " & strPath & " -> " & objModule.ModuleTitle
                                                        Else
                                                            objListItem.Text = objPortal.PortalName & " -> " & strPath & " -> " & objModule.ModuleTitle
                                                        End If
                                                        drpContentSharingPortals.Items.Add(objListItem)
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        Next

                    End If

                End If

            Next

            If (drpContentSharingPortals.Items.Count = 0) Then
                lblContentSharingNoneAvailable.Visible = True
                drpContentSharingPortals.Visible = False
                cmdContentSharingAdd.Visible = False
            Else
                lblContentSharingNoneAvailable.Visible = False
                drpContentSharingPortals.Visible = True
                cmdContentSharingAdd.Visible = True
            End If

        End Sub

        Private Sub BindSelectedContentSharingPortals()

            If (Page.IsPostBack = False) Then
                Localization.LocalizeDataGrid(grdContentSharing, Me.LocalResourceFile)
            End If

            Dim objContentSharingPortals As List(Of ContentSharingInfo) = GetContentSharingPortals(ArticleSettings.ContentSharingPortals)

            If (objContentSharingPortals.Count > 0) Then
                grdContentSharing.DataSource = objContentSharingPortals
                grdContentSharing.DataBind()
                lblNoContentSharing.Visible = False
                grdContentSharing.Visible = True
            Else
                lblNoContentSharing.Visible = True
                grdContentSharing.Visible = False
            End If

        End Sub

        Private Function GetContentSharingPortals(ByVal linkedPortals As String) As List(Of ContentSharingInfo)

            Dim objPortalController As New PortalController()
            Dim objContentSharingPortals As New List(Of ContentSharingInfo)

            For Each element As String In linkedPortals.Split(","c)

                If (element.Split("-"c).Length = 3) Then

                    Dim objContentSharing As New ContentSharingInfo

                    objContentSharing.LinkedPortalID = Convert.ToInt32(element.Split("-")(0))
                    objContentSharing.LinkedTabID = Convert.ToInt32(element.Split("-")(1))
                    objContentSharing.LinkedModuleID = Convert.ToInt32(element.Split("-")(2))

                    Dim objModuleController As New ModuleController
                    Dim objModule As ModuleInfo = objModuleController.GetModule(objContentSharing.LinkedModuleID, objContentSharing.LinkedTabID)

                    If (objModule IsNot Nothing) Then
                        objContentSharing.ModuleTitle = objModule.ModuleTitle
                        objContentSharingPortals.Add(objContentSharing)
                    End If

                End If

            Next

            Return objContentSharingPortals

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If (Page.IsPostBack = False) Then

                    BindCategorySortOrder()
                    BindDisplayTypes()
                    BindCaptchaTypes()
                    BindAuthorSelection()
                    BindTextEditorMode()
                    BindPageSize()
                    BindTemplates()
                    BindTitleReplacement()
                    BindTimeZone()
                    BindRoleGroups()
                    BindFolders()
                    BindCategories()
                    BindSortBy()
                    BindSortDirection()
                    BindSyndicationLinkMode()
                    BindSyndicationEnclosureType()
                    BindUrlMode()
                    BindMenuPositionType()
                    BindRelatedTypes()
                    BindThumbnailType()
                    BindWatermarkPosition()
                    BindSettings()

                    lstDefaultCategories.Height = Unit.Parse(ArticleSettings.CategorySelectionHeight.ToString())
                    lstCategories.Height = Unit.Parse(ArticleSettings.CategorySelectionHeight.ToString())

                    trAdminSettings1.Visible = Me.UserInfo.IsSuperUser
                    trAdminSettings2.Visible = Me.UserInfo.IsSuperUser

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

            Try

                If (Page.IsValid) Then

                    SaveBasicSettings()
                    SaveArchiveSettings()
                    SaveCategorySettings()
                    SaveCommentSettings()
                    SaveFilterSettings()
                    SaveFormSettings()
                    SaveImageSettings()
                    SaveFileSettings()
                    SaveNotificationSettings()
                    SaveRelatedSettings()
                    SaveSecuritySettings()
                    SaveSEOSettings()
                    SaveSyndicationSettings()
                    SaveTwitterSettings()
                    SaveThirdPartySettings()

                    CategoryController.ClearCache(ModuleId)
                    LayoutController.ClearCache(Me)

                    Response.Redirect(EditArticleUrl("AdminOptions"), True)

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(EditArticleUrl("AdminOptions"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub valEditorWidthIsValid_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valEditorWidthIsValid.ServerValidate

            Try

                Try
                    Unit.Parse(txtTextEditorWidth.Text)
                    args.IsValid = True
                Catch ex As Exception
                    args.IsValid = False
                End Try

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub valEditorHeightIsvalid_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valEditorHeightIsvalid.ServerValidate

            Try

                Try
                    Unit.Parse(txtTextEditorHeight.Text)
                    args.IsValid = True
                Catch ex As Exception
                    args.IsValid = False
                End Try

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdSelectAuthor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectAuthor.Click

            Try

                PopulateAuthorList()
                ddlAuthor.Visible = True
                cmdSelectAuthor.Visible = False
                lblAuthorFilter.Visible = False

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdSelectAuthorDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectAuthorDefault.Click

            Try

                PopulateAuthorListDefault()
                drpAuthorDefault.Visible = True
                cmdSelectAuthorDefault.Visible = False
                lblAuthorDefault.Visible = False

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub grdBasicPermissions_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdBasicPermissions.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                Dim objListItem As ListItem = CType(e.Item.DataItem, ListItem)

                If Not (objListItem Is Nothing) Then

                    Dim role As String = CType(e.Item.DataItem, ListItem).Value

                    Dim chkSubmit As CheckBox = CType(e.Item.FindControl("chkSubmit"), CheckBox)
                    Dim chkSecure As CheckBox = CType(e.Item.FindControl("chkSecure"), CheckBox)
                    Dim chkAutoSecure As CheckBox = CType(e.Item.FindControl("chkAutoSecure"), CheckBox)
                    Dim chkApprove As CheckBox = CType(e.Item.FindControl("chkApprove"), CheckBox)
                    Dim chkAutoApproveArticle As CheckBox = CType(e.Item.FindControl("chkAutoApproveArticle"), CheckBox)
                    Dim chkAutoApproveComment As CheckBox = CType(e.Item.FindControl("chkAutoApproveComment"), CheckBox)
                    Dim chkFeature As CheckBox = CType(e.Item.FindControl("chkFeature"), CheckBox)
                    Dim chkAutoFeature As CheckBox = CType(e.Item.FindControl("chkAutoFeature"), CheckBox)

                    If (objListItem.Value = PortalSettings.AdministratorRoleName.ToString()) Then
                        chkSubmit.Enabled = False
                        chkSubmit.Checked = True
                        chkSecure.Enabled = True
                        If (Settings.Contains(ArticleConstants.PERMISSION_SECURE_SETTING)) Then
                            chkSecure.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_SECURE_SETTING).ToString().Split(";"c))
                        End If
                        chkAutoSecure.Enabled = True
                        If (Settings.Contains(ArticleConstants.PERMISSION_AUTO_SECURE_SETTING)) Then
                            chkAutoSecure.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_AUTO_SECURE_SETTING).ToString().Split(";"c))
                        End If
                        chkApprove.Enabled = False
                        chkApprove.Checked = True
                        chkAutoApproveArticle.Enabled = True
                        If (Settings.Contains(ArticleConstants.PERMISSION_AUTO_APPROVAL_SETTING)) Then
                            chkAutoApproveArticle.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_AUTO_APPROVAL_SETTING).ToString().Split(";"c))
                        End If
                        chkAutoApproveComment.Enabled = True
                        If (Settings.Contains(ArticleConstants.PERMISSION_AUTO_APPROVAL_COMMENT_SETTING)) Then
                            chkAutoApproveComment.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_AUTO_APPROVAL_COMMENT_SETTING).ToString().Split(";"c))
                        End If
                        chkFeature.Enabled = False
                        chkFeature.Checked = True
                        chkAutoFeature.Enabled = True
                        If (Settings.Contains(ArticleConstants.PERMISSION_AUTO_FEATURE_SETTING)) Then
                            chkAutoFeature.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_AUTO_FEATURE_SETTING).ToString().Split(";"c))
                        End If
                    Else
                        If (Settings.Contains(ArticleConstants.PERMISSION_SUBMISSION_SETTING)) Then
                            chkSubmit.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_SUBMISSION_SETTING).ToString().Split(";"c))
                        End If
                        If (Settings.Contains(ArticleConstants.PERMISSION_SECURE_SETTING)) Then
                            chkSecure.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_SECURE_SETTING).ToString().Split(";"c))
                        End If
                        If (Settings.Contains(ArticleConstants.PERMISSION_AUTO_SECURE_SETTING)) Then
                            chkAutoSecure.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_AUTO_SECURE_SETTING).ToString().Split(";"c))
                        End If
                        If (Settings.Contains(ArticleConstants.PERMISSION_APPROVAL_SETTING)) Then
                            chkApprove.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_APPROVAL_SETTING).ToString().Split(";"c))
                        End If
                        If (Settings.Contains(ArticleConstants.PERMISSION_AUTO_APPROVAL_SETTING)) Then
                            chkAutoApproveArticle.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_AUTO_APPROVAL_SETTING).ToString().Split(";"c))
                        End If
                        If (Settings.Contains(ArticleConstants.PERMISSION_AUTO_APPROVAL_COMMENT_SETTING)) Then
                            chkAutoApproveComment.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_AUTO_APPROVAL_COMMENT_SETTING).ToString().Split(";"c))
                        End If
                        If (Settings.Contains(ArticleConstants.PERMISSION_FEATURE_SETTING)) Then
                            chkFeature.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_FEATURE_SETTING).ToString().Split(";"c))
                        End If
                        If (Settings.Contains(ArticleConstants.PERMISSION_AUTO_FEATURE_SETTING)) Then
                            chkAutoFeature.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_AUTO_FEATURE_SETTING).ToString().Split(";"c))
                        End If
                    End If

                End If

            End If

        End Sub

        Private Sub grdFormPermissions_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdFormPermissions.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                Dim objListItem As ListItem = CType(e.Item.DataItem, ListItem)

                If Not (objListItem Is Nothing) Then

                    Dim role As String = CType(e.Item.DataItem, ListItem).Value

                    Dim chkCategories As CheckBox = CType(e.Item.FindControl("chkCategories"), CheckBox)
                    Dim chkExcerpt As CheckBox = CType(e.Item.FindControl("chkExcerpt"), CheckBox)
                    Dim chkImage As CheckBox = CType(e.Item.FindControl("chkImage"), CheckBox)
                    Dim chkFile As CheckBox = CType(e.Item.FindControl("chkFile"), CheckBox)
                    Dim chkLink As CheckBox = CType(e.Item.FindControl("chkLink"), CheckBox)
                    Dim chkPublishDate As CheckBox = CType(e.Item.FindControl("chkPublishDate"), CheckBox)
                    Dim chkExpiryDate As CheckBox = CType(e.Item.FindControl("chkExpiryDate"), CheckBox)
                    Dim chkMeta As CheckBox = CType(e.Item.FindControl("chkMeta"), CheckBox)
                    Dim chkCustom As CheckBox = CType(e.Item.FindControl("chkCustom"), CheckBox)

                    If (Settings.Contains(ArticleConstants.PERMISSION_CATEGORIES_SETTING)) Then
                        chkCategories.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_CATEGORIES_SETTING).ToString().Split(";"c))
                    Else
                        chkCategories.Checked = True
                    End If
                    If (Settings.Contains(ArticleConstants.PERMISSION_EXCERPT_SETTING)) Then
                        chkExcerpt.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_EXCERPT_SETTING).ToString().Split(";"c))
                    Else
                        chkExcerpt.Checked = True
                    End If
                    If (Settings.Contains(ArticleConstants.PERMISSION_IMAGE_SETTING)) Then
                        chkImage.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_IMAGE_SETTING).ToString().Split(";"c))
                    Else
                        chkImage.Checked = True
                    End If
                    If (Settings.Contains(ArticleConstants.PERMISSION_FILE_SETTING)) Then
                        chkFile.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_FILE_SETTING).ToString().Split(";"c))
                    Else
                        chkFile.Checked = True
                    End If
                    If (Settings.Contains(ArticleConstants.PERMISSION_LINK_SETTING)) Then
                        chkLink.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_LINK_SETTING).ToString().Split(";"c))
                    Else
                        chkLink.Checked = True
                    End If
                    If (Settings.Contains(ArticleConstants.PERMISSION_PUBLISH_SETTING)) Then
                        chkPublishDate.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_PUBLISH_SETTING).ToString().Split(";"c))
                    Else
                        chkPublishDate.Checked = True
                    End If
                    If (Settings.Contains(ArticleConstants.PERMISSION_EXPIRY_SETTING)) Then
                        chkExpiryDate.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_EXPIRY_SETTING).ToString().Split(";"c))
                    Else
                        chkExpiryDate.Checked = True
                    End If
                    If (Settings.Contains(ArticleConstants.PERMISSION_META_SETTING)) Then
                        chkMeta.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_META_SETTING).ToString().Split(";"c))
                    Else
                        chkMeta.Checked = True
                    End If
                    If (Settings.Contains(ArticleConstants.PERMISSION_CUSTOM_SETTING)) Then
                        chkCustom.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_CUSTOM_SETTING).ToString().Split(";"c))
                    Else
                        chkCustom.Checked = True
                    End If

                End If

            End If

        End Sub

        Private Sub grdAdminPermissions_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdAdminPermissions.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                Dim objListItem As ListItem = CType(e.Item.DataItem, ListItem)

                If Not (objListItem Is Nothing) Then

                    Dim role As String = CType(e.Item.DataItem, ListItem).Value

                    Dim chkSiteTemplates As CheckBox = CType(e.Item.FindControl("chkSiteTemplates"), CheckBox)

                    If (Settings.Contains(ArticleConstants.PERMISSION_SITE_TEMPLATES_SETTING)) Then
                        chkSiteTemplates.Checked = IsInRole(role, Settings(ArticleConstants.PERMISSION_SITE_TEMPLATES_SETTING).ToString().Split(";"c))
                    Else
                        chkSiteTemplates.Checked = False
                    End If

                End If

            End If

        End Sub

        Private Sub cmdContentSharingAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdContentSharingAdd.Click

            Dim currentPortals As String = Null.NullString()
            If (ArticleSettings.ContentSharingPortals = Null.NullString) Then
                currentPortals = drpContentSharingPortals.SelectedValue
            Else
                currentPortals = ArticleSettings.ContentSharingPortals & "," & drpContentSharingPortals.SelectedValue
            End If

            Dim objModules As New ModuleController
            objModules.UpdateModuleSetting(ModuleId, ArticleConstants.CONTENT_SHARING_SETTING, currentPortals)

            Settings(ArticleConstants.CONTENT_SHARING_SETTING) = currentPortals

            BindSelectedContentSharingPortals()
            BindAvailableContentSharingPortals()

        End Sub

        Private Sub grdContentSharing_ItemCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdContentSharing.ItemCommand

            If (e.CommandName = "Delete") Then

                Dim linkedModuleID As Integer = Convert.ToInt32(e.CommandArgument)

                Dim updateSetting As String = Null.NullString()
                Dim objContentSharingPortals As List(Of ContentSharingInfo) = GetContentSharingPortals(ArticleSettings.ContentSharingPortals)

                For Each objContentSharingPortal As ContentSharingInfo In objContentSharingPortals
                    If (objContentSharingPortal.LinkedModuleID <> linkedModuleID) Then

                        If (updateSetting = "") Then
                            updateSetting = objContentSharingPortal.LinkedPortalID.ToString() & "-" & objContentSharingPortal.LinkedTabID.ToString() & "-" & objContentSharingPortal.LinkedModuleID.ToString()
                        Else
                            updateSetting = updateSetting & "," & objContentSharingPortal.LinkedPortalID.ToString() & "-" & objContentSharingPortal.LinkedTabID.ToString() & "-" & objContentSharingPortal.LinkedModuleID.ToString()
                        End If

                    End If
                Next

                Dim objModules As New ModuleController
                objModules.UpdateModuleSetting(ModuleId, ArticleConstants.CONTENT_SHARING_SETTING, updateSetting)

                Settings(ArticleConstants.CONTENT_SHARING_SETTING) = updateSetting

                BindSelectedContentSharingPortals()
                BindAvailableContentSharingPortals()

            End If

        End Sub

        Private Sub drpSecurityRoleGroups_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpSecurityRoleGroups.SelectedIndexChanged

            Try

                BindRoles()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace
