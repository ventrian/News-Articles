'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Services.Exceptions
Imports Ventrian.NewsArticles.Components.CustomFields
Imports System.IO

Namespace Ventrian.NewsArticles

    Partial Public Class LatestArticlesOptions
        Inherits ModuleSettingsBase

#Region " Private Methods "

        Private Sub BindMatchOperators()

            For Each value As Integer In System.Enum.GetValues(GetType(MatchOperatorType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(MatchOperatorType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(MatchOperatorType), value), Me.LocalResourceFile)
                rdoMatchOperator.Items.Add(li)
            Next

            For Each value As Integer In System.Enum.GetValues(GetType(MatchOperatorType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(MatchOperatorType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(MatchOperatorType), value), Me.LocalResourceFile)
                rdoTagsMatchOperator.Items.Add(li)
            Next

        End Sub

        Private Sub BindLayoutMode()

            For Each value As Integer In System.Enum.GetValues(GetType(LayoutModeType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(LayoutModeType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(LayoutModeType), value), Me.LocalResourceFile)
                lstLayoutMode.Items.Add(li)
            Next

        End Sub

        Private Sub BindPageMode()

            For Each value As Integer In System.Enum.GetValues(GetType(LinkFilterType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(LinkFilterType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(LinkFilterType), value), Me.LocalResourceFile)
                rdoLinkFilter.Items.Add(li)
            Next

            rdoLinkFilter.SelectedIndex = 0

        End Sub

        Private Sub BindPages()

            Dim objTabController As New TabController()

            Dim objTabs As TabCollection = objTabController.GetTabsByPortal(PortalId)
            For Each objTab As DotNetNuke.Entities.Tabs.TabInfo In objTabs.AsList()
                drpPageFilter.Items.Add(New ListItem(objTab.TabPath.Replace("//", "/").TrimStart("/"c), objTab.TabID.ToString()))
            Next


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

        End Sub

        Private Sub BindCustomFields()

            If (drpModuleID.Items.Count > 0) Then
                Dim objCustomFieldController As New CustomFieldController()
                drpCustomField.DataSource = objCustomFieldController.List(Convert.ToInt32(drpModuleID.SelectedValue.Split("-"c)(1)))
                drpCustomField.DataBind()
                drpCustomField.Items.Insert(0, New ListItem(Localization.GetString("SelectCustomField", Me.LocalResourceFile), "-1"))
            End If

        End Sub

        Private Sub BindModules()

            Dim objDesktopModuleController As New DesktopModuleController
            Dim objDesktopModuleInfo As DesktopModuleInfo = DesktopModuleController.GetDesktopModuleByModuleName("DnnForge - NewsArticles", PortalId)


            If Not (objDesktopModuleInfo Is Nothing) Then

                Dim objTabController As New TabController()
                Dim objTabs As TabCollection = objTabController.GetTabsByPortal(PortalId)
                For Each objTab As DotNetNuke.Entities.Tabs.TabInfo In objTabs.AsList()
                    If Not (objTab Is Nothing) Then
                        If (objTab.IsDeleted = False) Then
                            Dim objModules As New ModuleController
                            For Each pair As KeyValuePair(Of Integer, ModuleInfo) In objModules.GetTabModules(objTab.TabID)
                                Dim objModule As ModuleInfo = pair.Value
                                If (objModule.IsDeleted = False) Then
                                    If (objModule.DesktopModuleID = objDesktopModuleInfo.DesktopModuleID) Then
                                        If DotNetNuke.Security.Permissions.ModulePermissionController.CanEditModuleContent(objModule) = True And objModule.IsDeleted = False Then
                                            Dim strPath As String = objTab.TabName
                                            Dim objTabSelected As TabInfo = objTab
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

            If (drpModuleID.Items.Count > 0) Then
                BindCategories()
                BindCustomFields()
            End If

        End Sub

        Private Sub BindCategories()

            If (drpModuleID.Items.Count > 0) Then
                Dim objCategoryController As New CategoryController
                Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(Convert.ToInt32(drpModuleID.SelectedValue.Split("-"c)(1)), Null.NullInteger, CategorySortType.Name)

                lstCategories.DataSource = objCategories
                lstCategories.DataBind()

                lstCategoriesExclude.DataSource = objCategories
                lstCategoriesExclude.DataBind()
            End If

        End Sub

        Private Sub BindSettings()

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_MODULE_ID) And Settings.Contains(ArticleConstants.LATEST_ARTICLES_TAB_ID)) Then
                If Not (drpModuleID.Items.FindByValue(Settings(ArticleConstants.LATEST_ARTICLES_TAB_ID).ToString() & "-" & Settings(ArticleConstants.LATEST_ARTICLES_MODULE_ID).ToString()) Is Nothing) Then
                    drpModuleID.SelectedValue = Settings(ArticleConstants.LATEST_ARTICLES_TAB_ID).ToString() & "-" & Settings(ArticleConstants.LATEST_ARTICLES_MODULE_ID).ToString()
                End If
                BindCategories()
                BindCustomFields()
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_SORT_BY)) Then
                If Not (drpSortBy.Items.FindByValue(Settings(ArticleConstants.LATEST_ARTICLES_SORT_BY).ToString()) Is Nothing) Then
                    drpSortBy.SelectedValue = Settings(ArticleConstants.LATEST_ARTICLES_SORT_BY).ToString()
                End If
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_SORT_DIRECTION)) Then
                If Not (drpSortDirection.Items.FindByValue(Settings(ArticleConstants.LATEST_ARTICLES_SORT_DIRECTION).ToString()) Is Nothing) Then
                    drpSortDirection.SelectedValue = Settings(ArticleConstants.LATEST_ARTICLES_SORT_DIRECTION).ToString()
                End If
            Else
                If Not (drpSortDirection.Items.FindByValue(ArticleConstants.DEFAULT_SORT_DIRECTION) Is Nothing) Then
                    drpSortDirection.SelectedValue = ArticleConstants.DEFAULT_SORT_DIRECTION
                End If
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_CATEGORIES)) Then
                If Not (Settings(ArticleConstants.LATEST_ARTICLES_CATEGORIES).ToString = Null.NullString Or Settings(ArticleConstants.LATEST_ARTICLES_CATEGORIES).ToString = "-1") Then
                    Dim categories As String() = Settings(ArticleConstants.LATEST_ARTICLES_CATEGORIES).ToString().Split(Char.Parse(","))

                    For Each category As String In categories
                        If Not (lstCategories.Items.FindByValue(category) Is Nothing) Then
                            lstCategories.Items.FindByValue(category).Selected = True
                        End If
                    Next

                    rdoMatchOperator.Enabled = True
                    lstCategories.Enabled = True
                Else
                    rdoMatchOperator.Enabled = False
                    chkAllCategories.Checked = True
                    lstCategories.Enabled = False
                End If
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_CATEGORIES_EXCLUDE)) Then
                If Not (Settings(ArticleConstants.LATEST_ARTICLES_CATEGORIES_EXCLUDE).ToString = Null.NullString Or Settings(ArticleConstants.LATEST_ARTICLES_CATEGORIES_EXCLUDE).ToString = "-1") Then
                    Dim categories As String() = Settings(ArticleConstants.LATEST_ARTICLES_CATEGORIES_EXCLUDE).ToString().Split(Char.Parse(","))

                    For Each category As String In categories
                        If Not (lstCategoriesExclude.Items.FindByValue(category) Is Nothing) Then
                            lstCategoriesExclude.Items.FindByValue(category).Selected = True
                        End If
                    Next
                End If
            End If

            If (rdoMatchOperator.Items.Count > 0) Then
                rdoMatchOperator.Items(0).Selected = True
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_MATCH_OPERATOR)) Then
                If Not (rdoMatchOperator.Items.FindByValue(Settings(ArticleConstants.LATEST_ARTICLES_MATCH_OPERATOR).ToString()) Is Nothing) Then
                    rdoMatchOperator.SelectedValue = Settings(ArticleConstants.LATEST_ARTICLES_MATCH_OPERATOR).ToString()
                End If
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_TAGS)) Then
                Dim objTagController As New TagController()
                Dim tags As String = Settings(ArticleConstants.LATEST_ARTICLES_TAGS).ToString()
                If (tags <> "" And drpModuleID.Items.Count > 0) Then
                    For Each tag As String In tags.Split(","c)
                        Dim objTag As TagInfo = objTagController.Get(Convert.ToInt32(tag))
                        If (objTag IsNot Nothing) Then
                            If (txtTags.Text <> "") Then
                                txtTags.Text = txtTags.Text + "," + objTag.Name
                            Else
                                txtTags.Text = objTag.Name
                            End If
                        End If
                    Next
                End If
            End If

            If (rdoTagsMatchOperator.Items.Count > 0) Then
                rdoTagsMatchOperator.Items(0).Selected = True
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_TAGS_MATCH_OPERATOR)) Then
                If Not (rdoTagsMatchOperator.Items.FindByValue(Settings(ArticleConstants.LATEST_ARTICLES_TAGS_MATCH_OPERATOR).ToString()) Is Nothing) Then
                    rdoTagsMatchOperator.SelectedValue = Settings(ArticleConstants.LATEST_ARTICLES_TAGS_MATCH_OPERATOR).ToString()
                End If
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_AUTHOR)) Then
                Dim authorID As Integer = Convert.ToInt32(Settings(ArticleConstants.LATEST_ARTICLES_AUTHOR))
                If (authorID <> Null.NullInteger) Then
                    Dim objUserController As New DotNetNuke.Entities.Users.UserController
                    Dim objUser As DotNetNuke.Entities.Users.UserInfo = objUserController.GetUser(Me.PortalId, authorID)

                    If Not (objUser Is Nothing) Then
                        lblAuthorFilter.Text = objUser.Username
                    End If
                Else
                    lblAuthorFilter.Text = Localization.GetString("AllAuthors.Text", Me.LocalResourceFile)
                End If
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_LAYOUT_MODE)) Then
                If Not (lstLayoutMode.Items.FindByValue(Settings(ArticleConstants.LATEST_ARTICLES_LAYOUT_MODE).ToString()) Is Nothing) Then
                    lstLayoutMode.SelectedValue = CType(Settings(ArticleConstants.LATEST_ARTICLES_LAYOUT_MODE), String)
                Else
                    lstLayoutMode.SelectedIndex = 0
                End If
            Else
                If Not (lstLayoutMode.Items.FindByValue(ArticleConstants.LATEST_ARTICLES_LAYOUT_MODE_DEFAULT.ToString()) Is Nothing) Then
                    lstLayoutMode.SelectedValue = ArticleConstants.LATEST_ARTICLES_LAYOUT_MODE_DEFAULT.ToString()
                Else
                    lstLayoutMode.SelectedIndex = 0
                End If
            End If

            BindHtml()

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_ITEMS_PER_ROW)) Then
                txtItemsPerRow.Text = CType(Settings(ArticleConstants.LATEST_ARTICLES_ITEMS_PER_ROW), String)
            Else
                txtItemsPerRow.Text = ArticleConstants.LATEST_ARTICLES_ITEMS_PER_ROW_DEFAULT.ToString()
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_IDS)) Then
                txtArticleIDs.Text = CType(Settings(ArticleConstants.LATEST_ARTICLES_IDS), String)
            Else
                txtArticleIDs.Text = ""
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_COUNT)) Then
                txtArticleCount.Text = CType(Settings(ArticleConstants.LATEST_ARTICLES_COUNT), String)
            Else
                txtArticleCount.Text = "10"
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_START_POINT)) Then
                txtStartPoint.Text = CType(Settings(ArticleConstants.LATEST_ARTICLES_START_POINT), String)
            Else
                txtStartPoint.Text = "0"
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_MAX_AGE)) Then
                txtMaxAge.Text = CType(Settings(ArticleConstants.LATEST_ARTICLES_MAX_AGE), String)
            Else
                txtMaxAge.Text = ""
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_START_DATE)) Then
                If (Settings(ArticleConstants.LATEST_ARTICLES_START_DATE).ToString() <> "") Then
                    Dim objStartDate As DateTime = DateTime.Parse(Settings(ArticleConstants.LATEST_ARTICLES_START_DATE).ToString())
                    txtStartDate.Text = objStartDate.ToShortDateString()
                End If
            Else
                txtStartDate.Text = ""
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_QUERY_STRING_FILTER)) Then
                chkQueryStringFilter.Checked = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_QUERY_STRING_FILTER).ToString())
            Else
                chkQueryStringFilter.Checked = ArticleConstants.LATEST_ARTICLES_QUERY_STRING_FILTER_DEFAULT
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_QUERY_STRING_PARAM)) Then
                txtQueryStringParam.Text = CType(Settings(ArticleConstants.LATEST_ARTICLES_QUERY_STRING_PARAM), String)
            Else
                txtQueryStringParam.Text = ArticleConstants.LATEST_ARTICLES_QUERY_STRING_PARAM_DEFAULT
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_USERNAME_FILTER)) Then
                chkUsernameFilter.Checked = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_USERNAME_FILTER).ToString())
            Else
                chkUsernameFilter.Checked = ArticleConstants.LATEST_ARTICLES_USERNAME_FILTER_DEFAULT
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_USERNAME_PARAM)) Then
                txtUsernameParam.Text = CType(Settings(ArticleConstants.LATEST_ARTICLES_USERNAME_PARAM), String)
            Else
                txtUsernameParam.Text = ArticleConstants.LATEST_ARTICLES_USERNAME_PARAM_DEFAULT
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_LOGGED_IN_USER_FILTER)) Then
                chkLoggedInUser.Checked = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_LOGGED_IN_USER_FILTER).ToString())
            Else
                chkLoggedInUser.Checked = ArticleConstants.LATEST_ARTICLES_LOGGED_IN_USER_FILTER_DEFAULT
            End If

            If (Settings.Contains(ArticleConstants.LAUNCH_LINKS)) Then
                chkLaunchLinks.Checked = Convert.ToBoolean(Settings(ArticleConstants.LAUNCH_LINKS).ToString())
            Else
                chkLaunchLinks.Checked = False
            End If

            If (Settings.Contains(ArticleConstants.BUBBLE_FEATURED_ARTICLES)) Then
                chkBubbleFeatured.Checked = Convert.ToBoolean(Settings(ArticleConstants.BUBBLE_FEATURED_ARTICLES).ToString())
            Else
                chkBubbleFeatured.Checked = False
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ENABLE_PAGER)) Then
                chkEnablePager.Checked = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ENABLE_PAGER).ToString())
            Else
                chkEnablePager.Checked = False
            End If

            If (Settings.Contains(ArticleConstants.LATEST_PAGE_SIZE)) Then
                txtPageSize.Text = Settings(ArticleConstants.LATEST_PAGE_SIZE).ToString()
            Else
                txtPageSize.Text = ArticleConstants.LATEST_PAGE_SIZE_DEFAULT.ToString()
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_SHOW_PENDING)) Then
                chkShowPending.Checked = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_SHOW_PENDING).ToString())
            Else
                chkShowPending.Checked = False
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_SHOW_RELATED)) Then
                chkShowRelated.Checked = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_SHOW_RELATED).ToString())
            Else
                chkShowRelated.Checked = False
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_FEATURED_ONLY)) Then
                chkFeaturedOnly.Checked = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_FEATURED_ONLY).ToString())
            Else
                chkFeaturedOnly.Checked = False
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_NOT_FEATURED_ONLY)) Then
                chkNotFeaturedOnly.Checked = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_NOT_FEATURED_ONLY).ToString())
            Else
                chkNotFeaturedOnly.Checked = False
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_SECURED_ONLY)) Then
                chkSecureOnly.Checked = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_SECURED_ONLY).ToString())
            Else
                chkSecureOnly.Checked = False
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_NOT_SECURED_ONLY)) Then
                chkNotSecureOnly.Checked = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_NOT_SECURED_ONLY).ToString())
            Else
                chkNotSecureOnly.Checked = False
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_CUSTOM_FIELD_FILTER)) Then
                If (drpCustomField.Items.FindByValue(Settings(ArticleConstants.LATEST_ARTICLES_CUSTOM_FIELD_FILTER).ToString()) IsNot Nothing) Then
                    drpCustomField.SelectedValue = Settings(ArticleConstants.LATEST_ARTICLES_CUSTOM_FIELD_FILTER).ToString()
                End If
            Else
                drpCustomField.SelectedValue = "-1"
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_CUSTOM_FIELD_VALUE)) Then
                txtCustomFieldValue.Text = Settings(ArticleConstants.LATEST_ARTICLES_CUSTOM_FIELD_VALUE).ToString()
            End If

            drpPageFilter.Visible = False
            txtUrlFilter.Visible = False

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_LINK_FILTER)) Then
                Dim linkFilter As String = Settings(ArticleConstants.LATEST_ARTICLES_LINK_FILTER).ToString().ToLower()
                If (IsNumeric(linkFilter)) Then
                    If (drpPageFilter.Items.FindByValue(linkFilter) IsNot Nothing) Then
                        drpPageFilter.Visible = True
                        drpPageFilter.SelectedValue = linkFilter
                        rdoLinkFilter.SelectedIndex = 2
                    End If
                Else
                    txtUrlFilter.Visible = True
                    txtUrlFilter.Text = linkFilter
                    rdoLinkFilter.SelectedIndex = 1
                End If
            End If

            If (Settings.Contains(ArticleConstants.CATEGORY_SELECTION_HEIGHT_SETTING)) Then
                lstCategories.Height = Unit.Parse(Settings(ArticleConstants.CATEGORY_SELECTION_HEIGHT_SETTING).ToString())
            Else
                lstCategories.Height = ArticleConstants.CATEGORY_SELECTION_HEIGHT_DEFAULT
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_INCLUDE_STYLESHEET)) Then
                chkIncludeStylesheet.Checked = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_INCLUDE_STYLESHEET).ToString())
            Else
                chkIncludeStylesheet.Checked = ArticleConstants.LATEST_ARTICLES_INCLUDE_STYLESHEET_DEFAULT
            End If

        End Sub

        Private Sub BindHtml()

            Dim objLatestLayoutController As New LatestLayoutController()

            Dim objLayoutHeader As LayoutInfo = objLatestLayoutController.GetLayout(LatestLayoutType.Listing_Header_Html, ModuleId, Settings)
            Dim objLayoutItem As LayoutInfo = objLatestLayoutController.GetLayout(LatestLayoutType.Listing_Item_Html, ModuleId, Settings)
            Dim objLayoutFooter As LayoutInfo = objLatestLayoutController.GetLayout(LatestLayoutType.Listing_Footer_Html, ModuleId, Settings)
            Dim objLayoutEmpty As LayoutInfo = objLatestLayoutController.GetLayout(LatestLayoutType.Listing_Empty_Html, ModuleId, Settings)

            txtHtmlHeader.Text = objLayoutHeader.Template
            txtHtmlBody.Text = objLayoutItem.Template
            txtHtmlFooter.Text = objLayoutFooter.Template
            txtHtmlNoArticles.Text = objLayoutEmpty.Template

        End Sub

        Private Sub SaveSettings()

            Dim objModuleController As New ModuleController

            If (drpModuleID.Items.Count > 0) Then

                Dim values As String() = drpModuleID.SelectedValue.Split(Convert.ToChar("-"))

                If (values.Length = 2) Then
                    objModuleController.UpdateTabModuleSetting(Me.TabModuleId, ArticleConstants.LATEST_ARTICLES_TAB_ID, values(0))
                    objModuleController.UpdateTabModuleSetting(Me.TabModuleId, ArticleConstants.LATEST_ARTICLES_MODULE_ID, values(1))
                End If

            End If

            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_SORT_BY, drpSortBy.SelectedValue)
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_SORT_DIRECTION, drpSortDirection.SelectedValue)
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_LAYOUT_MODE, lstLayoutMode.SelectedValue)

            Dim objLatestLayoutController As New LatestLayoutController()

            objLatestLayoutController.UpdateLayout(LatestLayoutType.Listing_Header_Html, ModuleId, txtHtmlHeader.Text)
            objLatestLayoutController.UpdateLayout(LatestLayoutType.Listing_Item_Html, ModuleId, txtHtmlBody.Text)
            objLatestLayoutController.UpdateLayout(LatestLayoutType.Listing_Footer_Html, ModuleId, txtHtmlFooter.Text)
            objLatestLayoutController.UpdateLayout(LatestLayoutType.Listing_Empty_Html, ModuleId, txtHtmlNoArticles.Text)

            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_ITEMS_PER_ROW, txtItemsPerRow.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LAUNCH_LINKS, chkLaunchLinks.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.BUBBLE_FEATURED_ARTICLES, chkBubbleFeatured.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ENABLE_PAGER, chkEnablePager.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_PAGE_SIZE, txtPageSize.Text)

            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_SHOW_PENDING, chkShowPending.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_SHOW_RELATED, chkShowRelated.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_FEATURED_ONLY, chkFeaturedOnly.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_NOT_FEATURED_ONLY, chkNotFeaturedOnly.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_SECURED_ONLY, chkSecureOnly.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_NOT_SECURED_ONLY, chkNotSecureOnly.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_CUSTOM_FIELD_FILTER, drpCustomField.SelectedValue)
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_CUSTOM_FIELD_VALUE, txtCustomFieldValue.Text)
            'objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_LINK_FILTER, ctlLinkFilter.Url)

            If (rdoLinkFilter.SelectedIndex = 0) Then
                objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_LINK_FILTER, "")
            End If
            If (rdoLinkFilter.SelectedIndex = 1) Then
                objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_LINK_FILTER, AddHTTP(txtUrlFilter.Text))
            End If
            If (rdoLinkFilter.SelectedIndex = 2) Then
                objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_LINK_FILTER, drpPageFilter.SelectedValue)
            End If

            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_COUNT, txtArticleCount.Text)
            If (IsNumeric(txtStartPoint.Text) AndAlso Convert.ToInt32(txtStartPoint.Text) >= 0) Then
                objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_START_POINT, txtStartPoint.Text)
            End If

            If (txtArticleIDs.Text <> "") Then
                objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_IDS, txtArticleIDs.Text)
                For Each ID As String In txtArticleIDs.Text.Split(","c)
                    If (IsNumeric(ID) = False) Then
                        objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_IDS, "")
                    End If
                Next
            Else
                objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_IDS, txtArticleIDs.Text)
            End If
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_MAX_AGE, txtMaxAge.Text)
            If (txtStartDate.Text <> "") Then
                Dim objStartDate As DateTime = DateTime.Parse(txtStartDate.Text)
                objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_START_DATE, objStartDate.Year.ToString() + "-" + objStartDate.Month.ToString() + "-" + objStartDate.Day.ToString())
            Else
                objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_START_DATE, Null.NullString)
            End If
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_QUERY_STRING_FILTER, chkQueryStringFilter.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_QUERY_STRING_PARAM, txtQueryStringParam.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_USERNAME_FILTER, chkUsernameFilter.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_USERNAME_PARAM, txtUsernameParam.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_LOGGED_IN_USER_FILTER, chkLoggedInUser.Checked.ToString())

            If (ddlAuthor.Items.Count > 0) Then
                objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_AUTHOR, ddlAuthor.SelectedValue)
            End If

            If (chkAllCategories.Checked = True) Then
                objModuleController.UpdateModuleSetting(ModuleId, ArticleConstants.LATEST_ARTICLES_CATEGORIES, Null.NullInteger.ToString())
            Else
                Dim categories As String = ""
                For Each item As ListItem In lstCategories.Items
                    If item.Selected Then
                        If (categories.Length > 0) Then
                            categories = categories & ","
                        End If
                        categories = categories & item.Value
                    End If
                Next item
                objModuleController.UpdateModuleSetting(ModuleId, ArticleConstants.LATEST_ARTICLES_CATEGORIES, categories)
            End If

            Dim categoriesExclude As String = ""
            For Each item As ListItem In lstCategoriesExclude.Items
                If item.Selected Then
                    If (categoriesExclude.Length > 0) Then
                        categoriesExclude = categoriesExclude & ","
                    End If
                    categoriesExclude = categoriesExclude & item.Value
                End If
            Next item
            objModuleController.UpdateModuleSetting(ModuleId, ArticleConstants.LATEST_ARTICLES_CATEGORIES_EXCLUDE, categoriesExclude)

            Dim tags As String = ""
            If (drpModuleID.Items.Count > 0) Then
                For Each tag As String In txtTags.Text.Split(","c)
                    If (tag <> "") Then
                        Dim objTagController As New TagController()
                        Dim objTag As TagInfo = objTagController.Get(Convert.ToInt32(drpModuleID.SelectedValue.Split("-"c)(1)), tag.ToLower())
                        If (objTag IsNot Nothing) Then
                            If (tags = "") Then
                                tags = objTag.TagID.ToString()
                            Else
                                tags = tags & "," & objTag.TagID.ToString()
                            End If
                        Else
                            objTag = New TagInfo()
                            objTag.ModuleID = Convert.ToInt32(drpModuleID.SelectedValue.Split("-"c)(1))
                            objTag.Name = tag
                            objTag.NameLowered = tag.ToLower()
                            objTag.Usages = 0
                            objTag.TagID = objTagController.Add(objTag)
                            If (tags = "") Then
                                tags = objTag.TagID.ToString()
                            Else
                                tags = tags & "," & objTag.TagID.ToString()
                            End If
                        End If
                    End If
                Next
            End If
            objModuleController.UpdateModuleSetting(ModuleId, ArticleConstants.LATEST_ARTICLES_TAGS, tags)
            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_TAGS_MATCH_OPERATOR, rdoTagsMatchOperator.SelectedValue)

            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_MATCH_OPERATOR, rdoMatchOperator.SelectedValue)

            objModuleController.UpdateModuleSetting(Me.ModuleId, ArticleConstants.LATEST_ARTICLES_INCLUDE_STYLESHEET, chkIncludeStylesheet.Checked.ToString())

        End Sub

        Private Sub PopulateAuthorList()

            If (drpModuleID.Items.Count > 0) Then
                ddlAuthor.DataSource = GetAuthorList(Convert.ToInt32(drpModuleID.SelectedValue.Split("-"c)(1)))
                ddlAuthor.DataBind()
                ddlAuthor.Items.Insert(0, New ListItem(Localization.GetString("AllAuthors.Text", Me.LocalResourceFile), "-1"))

                If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_AUTHOR)) Then
                    Dim authorID As Integer = Convert.ToInt32(Settings(ArticleConstants.LATEST_ARTICLES_AUTHOR))

                    If Not (ddlAuthor.Items.FindByValue(authorID.ToString()) Is Nothing) Then
                        ddlAuthor.SelectedValue = authorID.ToString()
                    End If

                End If
            Else
                ddlAuthor.Items.Clear()
            End If

        End Sub

        Public Function GetAuthorList(ByVal moduleID As Integer) As ArrayList

            Dim ModuleController As New DotNetNuke.Entities.Modules.ModuleController
            Dim moduleSettings As Hashtable = ModuleController.GetModuleSettings(moduleID)
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
                            Dim objUsers As ArrayList = DotNetNuke.Entities.Users.UserController.GetUsers(PortalSettings.PortalId)
                            For Each objUser As DotNetNuke.Entities.Users.UserInfo In objUsers
                                If objUser.IsInRole(role) Then
                                    If (userIDs.Contains(objUser.UserID) = False) Then
                                        Dim objUserController As DotNetNuke.Entities.Users.UserController = New DotNetNuke.Entities.Users.UserController
                                        Dim objSelectedUser As DotNetNuke.Entities.Users.UserInfo = objUserController.GetUser(PortalSettings.PortalId, objUser.UserID)
                                        If Not (objSelectedUser Is Nothing) Then
                                            If (objSelectedUser.Email.Length > 0) Then
                                                userIDs.Add(objUser.UserID, objUser.UserID)
                                                userList.Add(objSelectedUser)
                                            End If
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

#End Region

#Region " Event Handlers "

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try
                trItemsPerRow.Visible = (lstLayoutMode.SelectedValue = LayoutModeType.Advanced.ToString())
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpModuleID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpModuleID.SelectedIndexChanged

            Try
                BindCategories()
                BindCustomFields()
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub chkAllCategories_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAllCategories.CheckedChanged

            Try
                rdoMatchOperator.Enabled = Not chkAllCategories.Checked
                lstCategories.Enabled = Not chkAllCategories.Checked
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub lstLayoutMode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstLayoutMode.SelectedIndexChanged

            Try
                ' BindHtml()
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub rdoLinkFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoLinkFilter.SelectedIndexChanged

            Try

                If (rdoLinkFilter.SelectedIndex = 0) Then
                    drpPageFilter.Visible = False
                    txtUrlFilter.Visible = False
                End If

                If (rdoLinkFilter.SelectedIndex = 1) Then
                    drpPageFilter.Visible = False
                    txtUrlFilter.Visible = True
                End If

                If (rdoLinkFilter.SelectedIndex = 2) Then
                    drpPageFilter.Visible = True
                    txtUrlFilter.Visible = False
                End If

            Catch exc As Exception 'Module failed to load
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

        Private Sub cmdSaveTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveTemplate.Click

            Try

                If (txtSaveTemplate.Text <> "") Then

                    Dim pathToTemplate As String = Me.MapPath("Templates/Portals/" & PortalId.ToString() & "/" & txtSaveTemplate.Text & "/")

                    If (Directory.Exists(pathToTemplate) = False) Then
                        Directory.CreateDirectory(pathToTemplate)
                    End If

                    Dim sw As New StreamWriter(pathToTemplate & "header.html")
                    Try
                        sw.Write(txtHtmlHeader.Text)
                    Catch
                    Finally
                        If Not sw Is Nothing Then sw.Close()
                    End Try

                    sw = New StreamWriter(pathToTemplate & "body.html")
                    Try
                        sw.Write(txtHtmlBody.Text)
                    Catch
                    Finally
                        If Not sw Is Nothing Then sw.Close()
                    End Try

                    sw = New StreamWriter(pathToTemplate & "footer.html")
                    Try
                        sw.Write(txtHtmlFooter.Text)
                    Catch
                    Finally
                        If Not sw Is Nothing Then sw.Close()
                    End Try

                    sw = New StreamWriter(pathToTemplate & "empty.html")
                    Try
                        sw.Write(txtHtmlNoArticles.Text)
                    Catch
                    Finally
                        If Not sw Is Nothing Then sw.Close()
                    End Try

                    BindTemplateSaves(txtSaveTemplate.Text)

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdLoadTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLoadTemplate.Click

            Try

                If (txtSaveTemplate.Text <> "") Then

                    Dim pathToTemplate As String = Me.MapPath("Templates/Portals/" & PortalId.ToString() & "/" & drpLoadFromTemplate.SelectedValue & "/")

                    If (File.Exists(pathToTemplate & "header.html")) Then
                        Dim sr As StreamReader = New StreamReader(pathToTemplate & "header.html")
                        Try
                            txtHtmlHeader.Text = sr.ReadToEnd()
                        Catch ex As Exception
                        Finally
                            If Not sr Is Nothing Then sr.Close()
                        End Try
                    End If

                    If (File.Exists(pathToTemplate & "body.html")) Then
                        Dim sr As StreamReader = New StreamReader(pathToTemplate & "body.html")
                        Try
                            txtHtmlBody.Text = sr.ReadToEnd()
                        Catch ex As Exception
                        Finally
                            If Not sr Is Nothing Then sr.Close()
                        End Try
                    End If

                    If (File.Exists(pathToTemplate & "footer.html")) Then
                        Dim sr As StreamReader = New StreamReader(pathToTemplate & "footer.html")
                        Try
                            txtHtmlFooter.Text = sr.ReadToEnd()
                        Catch ex As Exception
                        Finally
                            If Not sr Is Nothing Then sr.Close()
                        End Try
                    End If

                    If (File.Exists(pathToTemplate & "empty.html")) Then
                        Dim sr As StreamReader = New StreamReader(pathToTemplate & "empty.html")
                        Try
                            txtHtmlNoArticles.Text = sr.ReadToEnd()
                        Catch ex As Exception
                        Finally
                            If Not sr Is Nothing Then sr.Close()
                        End Try
                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpLoadFromTemplate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpLoadFromTemplate.SelectedIndexChanged

            Try
                txtSaveTemplate.Text = drpLoadFromTemplate.SelectedValue
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub BindTemplateSaves(ByVal template As String)

            drpLoadFromTemplate.Items.Clear()

            Dim templateRoot As String = Me.MapPath("Templates/Portals/" & PortalId.ToString() & "/")
            If Directory.Exists(templateRoot) Then
                Dim arrFolders() As String = Directory.GetDirectories(templateRoot)
                For Each folder As String In arrFolders
                    Dim folderName As String = folder.Substring(folder.LastIndexOf("\") + 1)
                    Dim objListItem As ListItem = New ListItem
                    objListItem.Text = folderName
                    objListItem.Value = folderName
                    drpLoadFromTemplate.Items.Add(objListItem)
                Next
            End If

            If (drpLoadFromTemplate.Items.Count = 0) Then
                trLoadFromTemplate.Visible = False
            Else
                trLoadFromTemplate.Visible = True

                If (template <> "") Then
                    If (drpLoadFromTemplate.Items.FindByValue(template) IsNot Nothing) Then
                        drpLoadFromTemplate.SelectedValue = template
                    End If
                End If

                txtSaveTemplate.Text = drpLoadFromTemplate.SelectedValue
            End If

        End Sub

#End Region

#Region " Base Method Implementations "

        Public Overrides Sub LoadSettings()

            cmdStartDate.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtStartDate)

            If (IsPostBack = False) Then
                BindModules()
                BindLayoutMode()
                BindMatchOperators()
                BindPageMode()
                BindPages()
                BindSortBy()
                BindSortDirection()
                BindSettings()
                BindTemplateSaves("")
            End If

        End Sub

        Public Overrides Sub UpdateSettings()

            Try

                SaveSettings()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace