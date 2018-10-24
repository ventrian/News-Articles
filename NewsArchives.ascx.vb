'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2005-2012
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Security
Imports DotNetNuke.Entities.Users

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class NewsArchives
        Inherits NewsArticleModuleBase

#Region " Private Members "

        Private _archiveSettings As ArchiveSettings
        Private _articleSettings As ArticleSettings
        Private _categories As List(Of CategoryInfo)

#End Region

#Region " Private Property "

        Private ReadOnly Property ArchiveSettings() As ArchiveSettings
            Get
                If (_archiveSettings Is Nothing) Then
                    _archiveSettings = New ArchiveSettings(Settings)
                End If
                Return _archiveSettings
            End Get
        End Property

        Public Shadows ReadOnly Property ArticleSettings() As ArticleSettings
            Get
                If (_articleSettings Is Nothing) Then

                    Dim _settings As Hashtable = Common.GetModuleSettings(ArchiveSettings.ModuleId)

                    Dim objModule As ModuleInfo = Common.GetModuleInfo(ArchiveSettings.ModuleId, ArchiveSettings.TabId)
                    If Not (objModule Is Nothing) Then
                        Dim tabModuleSettings As Hashtable = objModule.TabModuleSettings
                        For Each strKey As String In tabModuleSettings.Keys
                            _settings(strKey) = tabModuleSettings(strKey)
                        Next
                    End If

                    _articleSettings = New ArticleSettings(_settings, PortalSettings, objModule)

                End If
                Return _articleSettings
            End Get
        End Property

        Private ReadOnly Property Categories() As List(Of CategoryInfo)
            Get
                If (_categories Is Nothing) Then
                    Dim objCategoryController As New CategoryController
                    _categories = objCategoryController.GetCategoriesAll(ArchiveSettings.ModuleId, Null.NullInteger, _articleSettings.CategorySortType)
                End If

                Return _categories
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub BindArchive()

            Select Case ArchiveSettings.Mode

                Case ArchiveModeType.Date
                    BindDateArchive()

                Case ArchiveModeType.Category
                    BindCategoryArchive()

                Case ArchiveModeType.Author
                    BindAuthorArchive()

            End Select

        End Sub

        Private Function FindSettings() As Boolean

            If (ArchiveSettings.ModuleId <> Null.NullInteger) Then
                Return True
            End If

            If (Settings.Contains("na_StartupCheck") = False) Then

                Dim objModuleController As New ModuleController
                Dim objModules As List(Of ModuleInfo) = Common.GetArticleModules(PortalId)

                For Each objModule As ModuleInfo In objModules
                    If (objModule.TabID = TabId) Then

                        objModuleController.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_TAB_ID, objModule.TabID)
                        objModuleController.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_MODULE_ID, objModule.ModuleID)
                        objModuleController.UpdateModuleSetting(ModuleId, "na_StartupCheck", 1)

                        _archiveSettings = Nothing

                        Return True
                    End If
                Next

                For Each objModule As ModuleInfo In objModules

                    objModuleController.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_TAB_ID, objModule.TabID)
                    objModuleController.UpdateTabModuleSetting(TabModuleId, ArticleConstants.NEWS_ARCHIVES_MODULE_ID, objModule.ModuleID)
                    objModuleController.UpdateModuleSetting(ModuleId, "na_StartupCheck", 1)

                    _archiveSettings = Nothing

                    Return True
                Next

                objModuleController.UpdateModuleSetting(ModuleId, "na_StartupCheck", 1)

            End If

            Return False

        End Function

        Private Sub BindDateArchive()

            Dim objArticleController As New ArticleController
            Dim objModuleSettingController As New ModuleController

            Dim mi As ModuleInfo = objModuleSettingController.GetModule(ArchiveSettings.ModuleId, ArchiveSettings.TabId)
            If Not (mi Is Nothing) Then

                Dim authorId As Integer = Null.NullInteger
                If (ArticleSettings.AuthorLoggedInUserFilter) Then
                    authorId = -100
                    If (Request.IsAuthenticated) Then
                        authorId = UserId
                    End If
                End If

                If (ArticleSettings.AuthorUserIDFilter) Then
                    If (ArticleSettings.AuthorUserIDParam <> "") Then
                        If (Request(ArticleSettings.AuthorUserIDParam) <> "") Then
                            If (IsNumeric(Request(ArticleSettings.AuthorUserIDParam))) Then
                                authorId = Convert.ToInt32(Request(ArticleSettings.AuthorUserIDParam))
                            End If
                        End If
                    End If
                End If

                If (ArticleSettings.AuthorUsernameFilter) Then
                    If (ArticleSettings.AuthorUsernameParam <> "") Then
                        If (Request(ArticleSettings.AuthorUsernameParam) <> "") Then
                            Dim objUser As UserInfo = UserController.GetUserByName(PortalId, Request(ArticleSettings.AuthorUsernameParam))
                            If (objUser IsNot Nothing) Then
                                authorId = objUser.UserID
                            End If
                        End If
                    End If
                End If

                If (ArticleSettings.FilterSingleCategory <> Null.NullInteger) Then
                    Dim categoriesToDisplay(1) As Integer
                    categoriesToDisplay(1) = ArticleSettings.FilterSingleCategory

                    If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                        rptNewsArchives.DataSource = objArticleController.GetNewsArchive(ArchiveSettings.ModuleId, categoriesToDisplay, Nothing, authorId, ArchiveSettings.GroupBy, ArticleSettings.ShowPending)
                        rptNewsArchives.DataBind()
                    Else
                        dlNewsArchives.RepeatColumns = ArchiveSettings.ItemsPerRow
                        dlNewsArchives.DataSource = objArticleController.GetNewsArchive(ArchiveSettings.ModuleId, categoriesToDisplay, Nothing, authorId, ArchiveSettings.GroupBy, ArticleSettings.ShowPending)
                        dlNewsArchives.DataBind()
                    End If
                    Exit Sub
                End If

                If (ArticleSettings.FilterCategories IsNot Nothing) Then
                    If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                        rptNewsArchives.DataSource = objArticleController.GetNewsArchive(ArchiveSettings.ModuleId, ArticleSettings.FilterCategories, Nothing, authorId, ArchiveSettings.GroupBy, ArticleSettings.ShowPending)
                        rptNewsArchives.DataBind()
                    Else
                        dlNewsArchives.RepeatColumns = ArchiveSettings.ItemsPerRow
                        dlNewsArchives.DataSource = objArticleController.GetNewsArchive(ArchiveSettings.ModuleId, ArticleSettings.FilterCategories, Nothing, authorId, ArchiveSettings.GroupBy, ArticleSettings.ShowPending)
                        dlNewsArchives.DataBind()
                    End If
                    Exit Sub
                End If

                Dim newsSettings As Hashtable = mi.TabModuleSettings
                Dim excludeCategoriesRestrictive As New List(Of Integer)

                For Each objCategory As CategoryInfo In Categories
                    If (objCategory.InheritSecurity = False And objCategory.CategorySecurityType = CategorySecurityType.Restrict) Then
                        If (Request.IsAuthenticated) Then
                            If (newsSettings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                If (PortalSecurity.IsInRoles(newsSettings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString()) = False) Then
                                    excludeCategoriesRestrictive.Add(objCategory.CategoryID)
                                End If
                            End If
                        Else
                            excludeCategoriesRestrictive.Add(objCategory.CategoryID)
                        End If
                    End If
                Next

                Dim excludeCategories As New List(Of Integer)

                For Each objCategory As CategoryInfo In Categories
                    If (objCategory.InheritSecurity = False And objCategory.CategorySecurityType = CategorySecurityType.Loose) Then
                        If (Request.IsAuthenticated) Then
                            If (newsSettings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                If (PortalSecurity.IsInRoles(newsSettings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString()) = False) Then
                                    excludeCategories.Add(objCategory.CategoryID)
                                End If
                            End If
                        Else
                            excludeCategories.Add(objCategory.CategoryID)
                        End If
                    End If
                Next

                Dim includeCategories As New List(Of Integer)

                If (excludeCategories.Count > 0) Then

                    For Each objCategoryToInclude As CategoryInfo In Categories

                        Dim includeCategory As Boolean = True

                        For Each exclCategory As Integer In excludeCategories
                            If (exclCategory = objCategoryToInclude.CategoryID) Then
                                includeCategory = False
                            End If
                        Next

                        If (includeCategory) Then
                            includeCategories.Add(objCategoryToInclude.CategoryID)
                        End If

                    Next

                    If (includeCategories.Count > 0) Then
                        includeCategories.Add(-1)
                    End If

                End If

                If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                    rptNewsArchives.DataSource = objArticleController.GetNewsArchive(ArchiveSettings.ModuleId, includeCategories.ToArray(), excludeCategoriesRestrictive.ToArray(), authorId, ArchiveSettings.GroupBy, ArticleSettings.ShowPending)
                    rptNewsArchives.DataBind()
                Else
                    dlNewsArchives.RepeatColumns = ArchiveSettings.ItemsPerRow
                    dlNewsArchives.DataSource = objArticleController.GetNewsArchive(ArchiveSettings.ModuleId, includeCategories.ToArray(), excludeCategoriesRestrictive.ToArray(), authorId, ArchiveSettings.GroupBy, ArticleSettings.ShowPending)
                    dlNewsArchives.DataBind()
                End If
            End If

        End Sub

        Private Sub BindCategoryArchive()

            Dim objCategoryController As New CategoryController
            Dim objModuleSettingController As New ModuleController

            Dim parentID As Integer = ArticleConstants.NEWS_ARCHIVES_PARENT_CATEGORY_DEFAULT

            If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_PARENT_CATEGORY)) Then
                parentID = Convert.ToInt32(Settings(ArticleConstants.NEWS_ARCHIVES_PARENT_CATEGORY))
            End If

            Dim mi As ModuleInfo = objModuleSettingController.GetModule(ArchiveSettings.ModuleId, ArchiveSettings.TabId)
            If Not (mi Is Nothing) Then
                
                Dim authorId As Integer = Null.NullInteger
                If (ArticleSettings.AuthorLoggedInUserFilter) Then
                    authorId = -100
                    If (Request.IsAuthenticated) Then
                        authorId = UserId
                    End If
                End If
                
                If (ArticleSettings.AuthorUserIDFilter) Then
                    If (ArticleSettings.AuthorUserIDParam <> "") Then
                        If (Request(ArticleSettings.AuthorUserIDParam) <> "") Then
                            If (IsNumeric(Request(ArticleSettings.AuthorUserIDParam))) Then
                                authorId = Convert.ToInt32(Request(ArticleSettings.AuthorUserIDParam))
                            End If
                        End If
                    End If
                End If

                If (ArticleSettings.AuthorUsernameFilter) Then
                    If (ArticleSettings.AuthorUsernameParam <> "") Then
                        If (Request(ArticleSettings.AuthorUsernameParam) <> "") Then
                            Dim objUser As UserInfo = UserController.GetUserByName(PortalId, Request(ArticleSettings.AuthorUsernameParam))
                            If (objUser IsNot Nothing) Then
                                authorId = objUser.UserID
                            End If
                        End If
                    End If
                End If

                If (ArticleSettings.FilterSingleCategory <> Null.NullInteger) Then
                    Dim categoriesToDisplay(1) As Integer
                    categoriesToDisplay(1) = ArticleSettings.FilterSingleCategory

                    If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                        rptNewsArchives.DataSource = objCategoryController.GetCategoriesAll(ArchiveSettings.ModuleId, parentID, categoriesToDisplay, authorId, ArchiveSettings.CategoryMaxDepth, ArticleSettings.ShowPending, ArticleSettings.CategorySortType)
                        rptNewsArchives.DataBind()
                    Else
                        dlNewsArchives.RepeatColumns = ArchiveSettings.ItemsPerRow
                        dlNewsArchives.DataSource = objCategoryController.GetCategoriesAll(ArchiveSettings.ModuleId, parentID, categoriesToDisplay, authorId, ArchiveSettings.CategoryMaxDepth, ArticleSettings.ShowPending, ArticleSettings.CategorySortType)
                        dlNewsArchives.DataBind()
                    End If
                    Exit Sub
                End If

                If (ArticleSettings.FilterCategories IsNot Nothing) Then
                    If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                        rptNewsArchives.DataSource = objCategoryController.GetCategoriesAll(ArchiveSettings.ModuleId, parentID, ArticleSettings.FilterCategories, authorId, ArchiveSettings.CategoryMaxDepth, ArticleSettings.ShowPending, ArticleSettings.CategorySortType)
                        rptNewsArchives.DataBind()
                    Else
                        dlNewsArchives.RepeatColumns = ArchiveSettings.ItemsPerRow
                        dlNewsArchives.DataSource = objCategoryController.GetCategoriesAll(ArchiveSettings.ModuleId, parentID, ArticleSettings.FilterCategories, authorId, ArchiveSettings.CategoryMaxDepth, ArticleSettings.ShowPending, ArticleSettings.CategorySortType)
                        dlNewsArchives.DataBind()
                    End If
                    Exit Sub
                End If

                Dim moduleSettings As Hashtable = mi.ModuleSettings

                Dim objCategoriesSelected As New List(Of CategoryInfo)
                Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(ArchiveSettings.ModuleId, parentID, Nothing, authorId, ArchiveSettings.CategoryMaxDepth, ArticleSettings.ShowPending, ArticleSettings.CategorySortType)

                For Each objCategory As CategoryInfo In objCategories
                    If (objCategory.InheritSecurity) Then
                        objCategoriesSelected.Add(objCategory)
                    Else
                        If (Request.IsAuthenticated) Then
                            If (moduleSettings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                If (PortalSecurity.IsInRoles(moduleSettings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString())) Then
                                    objCategoriesSelected.Add(objCategory)
                                End If
                            End If
                        End If
                    End If
                Next

                If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                    rptNewsArchives.DataSource = objCategoriesSelected
                    rptNewsArchives.DataBind()
                Else
                    dlNewsArchives.RepeatColumns = ArchiveSettings.ItemsPerRow
                    dlNewsArchives.DataSource = objCategoriesSelected
                    dlNewsArchives.DataBind()
                End If
            End If

        End Sub

        Private Sub BindAuthorArchive()

            Dim sortBy As String = ArticleConstants.NEWS_ARCHIVES_AUTHOR_SORT_BY_DEFAULT.ToString()
            If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_AUTHOR_SORT_BY)) Then
                sortBy = CType(System.Enum.Parse(GetType(AuthorSortByType), Settings(ArticleConstants.NEWS_ARCHIVES_AUTHOR_SORT_BY).ToString()), AuthorSortByType).ToString()
            End If

            Dim objAuthorController As New AuthorController
            Dim objModuleSettingController As New ModuleController

            Dim mi As ModuleInfo = objModuleSettingController.GetModule(ArchiveSettings.ModuleId, ArchiveSettings.TabId)
            If Not (mi Is Nothing) Then
                Dim newsSettings As Hashtable = mi.TabModuleSettings
                
                Dim authorId As Integer = Null.NullInteger
                If (ArticleSettings.AuthorLoggedInUserFilter) Then
                    authorId = -100
                    If (Request.IsAuthenticated) Then
                        authorId = UserId
                    End If
                End If

                If (ArticleSettings.AuthorUserIDFilter) Then
                    If (ArticleSettings.AuthorUserIDParam <> "") Then
                        If (Request(ArticleSettings.AuthorUserIDParam) <> "") Then
                            If (IsNumeric(Request(ArticleSettings.AuthorUserIDParam))) Then
                                authorID = Convert.ToInt32(Request(ArticleSettings.AuthorUserIDParam))
                            End If
                        End If
                    End If
                End If
                

                If (ArticleSettings.AuthorUsernameFilter) Then
                    If (ArticleSettings.AuthorUsernameParam <> "") Then
                        If (Request(ArticleSettings.AuthorUsernameParam) <> "") Then
                            Dim objUser As UserInfo = UserController.GetUserByName(PortalId, Request(ArticleSettings.AuthorUsernameParam))
                            If (objUser IsNot Nothing) Then
                                authorId = objUser.UserID
                            End If
                        End If
                    End If
                End If

                If (ArticleSettings.FilterSingleCategory <> Null.NullInteger) Then
                    Dim categoriesToDisplay(1) As Integer
                    categoriesToDisplay(1) = ArticleSettings.FilterSingleCategory

                    If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                        rptNewsArchives.DataSource = objAuthorController.GetAuthorStatistics(ArchiveSettings.ModuleId, categoriesToDisplay, Nothing, authorId, sortBy, ArticleSettings.ShowPending)
                        rptNewsArchives.DataBind()
                    Else
                        dlNewsArchives.RepeatColumns = ArchiveSettings.ItemsPerRow
                        dlNewsArchives.DataSource = objAuthorController.GetAuthorStatistics(ArchiveSettings.ModuleId, categoriesToDisplay, Nothing, authorId, sortBy, ArticleSettings.ShowPending)
                        dlNewsArchives.DataBind()
                    End If
                    Exit Sub
                End If

                If (ArticleSettings.FilterCategories IsNot Nothing) Then
                    If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                        rptNewsArchives.DataSource = objAuthorController.GetAuthorStatistics(ArchiveSettings.ModuleId, ArticleSettings.FilterCategories, Nothing, authorId, sortBy, ArticleSettings.ShowPending)
                        rptNewsArchives.DataBind()
                    Else
                        dlNewsArchives.RepeatColumns = ArchiveSettings.ItemsPerRow
                        dlNewsArchives.DataSource = objAuthorController.GetAuthorStatistics(ArchiveSettings.ModuleId, ArticleSettings.FilterCategories, Nothing, authorId, sortBy, ArticleSettings.ShowPending)
                        dlNewsArchives.DataBind()
                    End If
                    Exit Sub
                End If

                Dim objCategoryController As New CategoryController
                Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(ArchiveSettings.ModuleId, Null.NullInteger)

                Dim excludeCategoriesRestrictive As New List(Of Integer)

                For Each objCategory As CategoryInfo In objCategories
                    If (objCategory.InheritSecurity = False And objCategory.CategorySecurityType = CategorySecurityType.Restrict) Then
                        If (Request.IsAuthenticated) Then
                            If (newsSettings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                If (PortalSecurity.IsInRoles(newsSettings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString()) = False) Then
                                    excludeCategoriesRestrictive.Add(objCategory.CategoryID)
                                End If
                            End If
                        Else
                            excludeCategoriesRestrictive.Add(objCategory.CategoryID)
                        End If
                    End If
                Next


                Dim excludeCategories As New List(Of Integer)

                For Each objCategory As CategoryInfo In objCategories
                    If (objCategory.InheritSecurity = False And objCategory.CategorySecurityType = CategorySecurityType.Loose) Then
                        If (Request.IsAuthenticated) Then
                            If (newsSettings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                If (PortalSecurity.IsInRoles(newsSettings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString()) = False) Then
                                    excludeCategories.Add(objCategory.CategoryID)
                                End If
                            End If
                        Else
                            excludeCategories.Add(objCategory.CategoryID)
                        End If
                    End If
                Next

                Dim includeCategories As New List(Of Integer)

                If (excludeCategories.Count > 0) Then

                    For Each objCategoryToInclude As CategoryInfo In objCategories

                        Dim includeCategory As Boolean = True

                        For Each exclCategory As Integer In excludeCategories
                            If (exclCategory = objCategoryToInclude.CategoryID) Then
                                includeCategory = False
                            End If
                        Next

                        If (includeCategory) Then
                            includeCategories.Add(objCategoryToInclude.CategoryID)
                        End If

                    Next

                    If (includeCategories.Count > 0) Then
                        includeCategories.Add(-1)
                    End If

                End If

                If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                    rptNewsArchives.DataSource = objAuthorController.GetAuthorStatistics(ArchiveSettings.ModuleId, includeCategories.ToArray(), excludeCategoriesRestrictive.ToArray(), authorId, sortBy, ArticleSettings.ShowPending)
                    rptNewsArchives.DataBind()
                Else
                    dlNewsArchives.RepeatColumns = ArchiveSettings.ItemsPerRow
                    dlNewsArchives.DataSource = objAuthorController.GetAuthorStatistics(ArchiveSettings.ModuleId, includeCategories.ToArray(), excludeCategoriesRestrictive.ToArray(), authorId, sortBy, ArticleSettings.ShowPending)
                    dlNewsArchives.DataBind()
                End If
            End If

        End Sub

        Private Sub ProcessBody(ByRef dataControls As ControlCollection, ByVal objArchive As ArchiveInfo)

            Dim literal As New Literal

            If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                literal.Text = ArchiveSettings.TemplateDateBody
            Else
                literal.Text = ArchiveSettings.TemplateDateAdvancedBody
            End If

            Const delimStr As String = "[]"
            Dim delimiter As Char() = delimStr.ToCharArray()
            ProcessArchive(dataControls, literal.Text.Split(delimiter), objArchive)

        End Sub

        Private Sub ProcessArchive(ByRef dataControls As ControlCollection, ByVal layoutArray As String(), ByVal objArchive As ArchiveInfo)

            Dim archiveDate As DateTime = New DateTime(objArchive.Year, objArchive.Month, objArchive.Day)

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                dataControls.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)

                        Case "COUNT"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArchive.Count.ToString()
                            dataControls.Add(objLiteral)

                        Case "ISSELECTEDMONTH"
                            Dim isValid As Boolean = False
                            If (ArchiveSettings.GroupBy = GroupByType.Month) Then
                                If (Request("month") <> "") Then
                                    If (Request("month") = objArchive.Month.ToString()) Then
                                        isValid = True
                                    End If
                                End If
                            End If
                            If (isValid = False) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISSELECTEDMONTH") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "ISNOTSELECTEDMONTH"
                            Dim isValid As Boolean = False
                            If (ArchiveSettings.GroupBy = GroupByType.Month) Then
                                If (Request("month") <> "") Then
                                    If (Request("month") = objArchive.Month.ToString()) Then
                                        isValid = True
                                    End If
                                End If
                            End If
                            If (isValid = True) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISNOTSELECTEDMONTH") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "ISSELECTEDYEAR"
                            Dim isValid As Boolean = False
                            If (ArchiveSettings.GroupBy = GroupByType.Year Or ArchiveSettings.GroupBy = GroupByType.Month) Then
                                If (Request("year") <> "") Then
                                    If (Request("year") = objArchive.Year.ToString()) Then
                                        isValid = True
                                    End If
                                End If
                            End If
                            If (isValid = False) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISSELECTEDYEAR") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "ISNOTSELECTEDYEAR"
                            Dim isValid As Boolean = False
                            If (Request("year") <> "") Then
                                If (Request("year") = objArchive.Year.ToString()) Then
                                    isValid = True
                                End If
                            End If
                            If (isValid = True) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISNOTSELECTEDYEAR") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "LINK"
                            Dim objLiteral As New Literal
                            If (ArchiveSettings.GroupBy = GroupByType.Month) Then
                                objLiteral.Text = Common.GetModuleLink(ArchiveSettings.TabId, ArchiveSettings.ModuleId, "ArchiveView", ArticleSettings, "month=" & objArchive.Month.ToString(), "year=" & objArchive.Year.ToString())
                            Else
                                objLiteral.Text = Common.GetModuleLink(ArchiveSettings.TabId, ArchiveSettings.ModuleId, "ArchiveView", ArticleSettings, "year=" & objArchive.Year.ToString())
                            End If
                            dataControls.Add(objLiteral)

                        Case "MONTH"
                            Dim objLiteral As New Literal
                            If (ArchiveSettings.GroupBy = GroupByType.Month) Then
                                objLiteral.Text = archiveDate.ToString("MMMM")
                            Else
                                objLiteral.Text = ""
                            End If
                            dataControls.Add(objLiteral)

                        Case "YEAR"
                            Dim objLiteral As New Literal
                            objLiteral.Text = archiveDate.Year.ToString()
                            dataControls.Add(objLiteral)

                    End Select
                End If

            Next

        End Sub

        Private Sub ProcessBody(ByRef dataControls As ControlCollection, ByVal objCategory As CategoryInfo)

            Dim literal As New Literal

            If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                literal.Text = ArchiveSettings.TemplateCategoryBody
            Else
                literal.Text = ArchiveSettings.TemplateCategoryAdvancedBody
            End If

            If (literal.Text.Contains("[DEPTHABS]")) Then

                For Each objCategorySelected As CategoryInfo In Categories
                    If (objCategorySelected.CategoryID = objCategory.CategoryID) Then
                        literal.Text = literal.Text.Replace("[DEPTHABS]", objCategorySelected.Level.ToString())
                    End If
                Next

            End If

            literal.Text = literal.Text.Replace("[CATEGORYID]", objCategory.CategoryID.ToString())
            literal.Text = literal.Text.Replace("[CATEGORY]", objCategory.NameIndented.ToString())
            literal.Text = literal.Text.Replace("[CATEGORYNOTINDENTED]", objCategory.Name.ToString())
            literal.Text = literal.Text.Replace("[COUNT]", objCategory.NumberOfArticles.ToString())
            literal.Text = literal.Text.Replace("[DEPTHREL]", objCategory.Level.ToString())
            literal.Text = literal.Text.Replace("[DESCRIPTION]", Server.HtmlDecode(objCategory.Description))

            literal.Text = literal.Text.Replace("[LINK]", Common.GetCategoryLink(ArchiveSettings.TabId, ArchiveSettings.ModuleId, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings))

            If (ArchiveSettings.CategoryHideZeroCategories And objCategory.NumberOfArticles = 0) Then
                Return
            End If

            dataControls.Add(literal)

        End Sub

        Private Sub ProcessBody(ByRef dataControls As ControlCollection, ByVal objAuthor As AuthorInfo)

            Dim literal As New Literal

            If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                literal.Text = ArchiveSettings.TemplateAuthorBody
            Else
                literal.Text = ArchiveSettings.TemplateAuthorAdvancedBody
            End If


            literal.Text = literal.Text.Replace("[AUTHORID]", objAuthor.UserID.ToString())
            literal.Text = literal.Text.Replace("[AUTHORUSERNAME]", objAuthor.UserName.ToString())
            literal.Text = literal.Text.Replace("[AUTHORDISPLAYNAME]", objAuthor.DisplayName.ToString())
            literal.Text = literal.Text.Replace("[AUTHORFIRSTNAME]", objAuthor.FirstName.ToString())
            literal.Text = literal.Text.Replace("[AUTHORLASTNAME]", objAuthor.LastName.ToString())
            literal.Text = literal.Text.Replace("[AUTHORFULLNAME]", objAuthor.FullName.ToString())
            literal.Text = literal.Text.Replace("[COUNT]", objAuthor.ArticleCount.ToString())
            literal.Text = literal.Text.Replace("[LINK]", Common.GetAuthorLink(ArchiveSettings.TabId, ArchiveSettings.ModuleId, objAuthor.UserID, objAuthor.UserName, ArticleSettings.LaunchLinks, ArticleSettings))

            dataControls.Add(literal)

        End Sub

        Private Sub ProcessTemplate(ByRef listControls As ControlCollection, ByVal li As ListItemType, ByVal obj As Object)

            If (li = ListItemType.Header) Then

                Dim objLiteral As New Literal

                Select Case ArchiveSettings.Mode

                    Case ArchiveModeType.Date
                        If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                            objLiteral.Text = ArchiveSettings.TemplateDateHeader
                        Else
                            objLiteral.Text = ArchiveSettings.TemplateDateAdvancedHeader
                        End If

                    Case ArchiveModeType.Category
                        If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                            objLiteral.Text = ArchiveSettings.TemplateCategoryHeader
                        Else
                            objLiteral.Text = ArchiveSettings.TemplateCategoryAdvancedHeader
                        End If

                    Case ArchiveModeType.Author
                        If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                            objLiteral.Text = ArchiveSettings.TemplateAuthorHeader
                        Else
                            objLiteral.Text = ArchiveSettings.TemplateAuthorAdvancedHeader
                        End If

                End Select

                listControls.Add(objLiteral)

            End If

            If (li = ListItemType.Item Or li = ListItemType.AlternatingItem) Then

                Select Case ArchiveSettings.Mode

                    Case ArchiveModeType.Date
                        ProcessBody(listControls, CType(obj, ArchiveInfo))

                    Case ArchiveModeType.Category
                        ProcessBody(listControls, CType(obj, CategoryInfo))

                    Case ArchiveModeType.Author
                        ProcessBody(listControls, CType(obj, AuthorInfo))

                End Select

            End If

            If (li = ListItemType.Footer) Then

                Dim objLiteral As New Literal

                Select Case ArchiveSettings.Mode

                    Case ArchiveModeType.Date
                        If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                            objLiteral.Text = ArchiveSettings.TemplateDateFooter
                        Else
                            objLiteral.Text = ArchiveSettings.TemplateDateAdvancedFooter
                        End If

                    Case ArchiveModeType.Category
                        If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                            objLiteral.Text = ArchiveSettings.TemplateCategoryFooter
                        Else
                            objLiteral.Text = ArchiveSettings.TemplateCategoryAdvancedFooter
                        End If

                    Case ArchiveModeType.Author
                        If (ArchiveSettings.LayoutMode = LayoutModeType.Simple) Then
                            objLiteral.Text = ArchiveSettings.TemplateAuthorFooter
                        Else
                            objLiteral.Text = ArchiveSettings.TemplateAuthorAdvancedFooter
                        End If

                End Select

                listControls.Add(objLiteral)

            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Load

            Try
                If (FindSettings()) Then
                    BindArchive()
                Else
                    divNotConfigured.Visible = True
                    rptNewsArchives.Visible = False
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub rptNewsArchives_OnItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptNewsArchives.ItemDataBound

            Try

                ProcessTemplate(e.Item.Controls, e.Item.ItemType, e.Item.DataItem)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub dlNewsArchives_OnItemDataBound(ByVal sender As Object, ByVal e As DataListItemEventArgs) Handles dlNewsArchives.ItemDataBound

            Try

                ProcessTemplate(e.Item.Controls, e.Item.ItemType, e.Item.DataItem)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace