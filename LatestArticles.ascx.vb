'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2005-2012
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Security
Imports Ventrian.NewsArticles.Components.CustomFields

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class LatestArticles
        Inherits NewsArticleModuleBase

#Region " Controls "

        Protected WithEvents rptLatestArticles As System.Web.UI.WebControls.Repeater
        Protected WithEvents dlLatestArticles As System.Web.UI.WebControls.DataList
        Protected WithEvents lblNotConfigured As System.Web.UI.WebControls.Label

#End Region

#Region " Private Members "

        Private _objLayoutController As LayoutController

        Private _objLayoutHeader As LayoutInfo
        Private _objLayoutItem As LayoutInfo
        Private _objLayoutFooter As LayoutInfo

        Private _articleTabID As Integer = Null.NullInteger
        Private _articleTabInfo As DotNetNuke.Entities.Tabs.TabInfo
        Private _articleModuleID As Integer = Null.NullInteger
        Private _rssLink As String = Null.NullString
        Private _layoutMode As LayoutModeType

        Private _pageNumber As Integer = 1

        Private _serverTimeZone As Integer = Null.NullInteger
        Private _articleSettings As ArticleSettings

        Private _settings As Hashtable

#End Region

#Region " Private Properties "

        Public Shadows ReadOnly Property ArticleSettings() As ArticleSettings
            Get
                If (_articleSettings Is Nothing) Then

                    Dim ModuleController As New DotNetNuke.Entities.Modules.ModuleController
                    _settings = ModuleController.GetModuleSettings(_articleModuleID)

                    Dim objModuleController As New ModuleController
                    Dim objModule As ModuleInfo = objModuleController.GetModule(_articleModuleID, _articleTabID)
                    If Not (objModule Is Nothing) Then
                        Dim objSettings As Hashtable = ModuleController.GetTabModuleSettings(objModule.TabModuleID)

                        For Each key As String In objSettings.Keys
                            If (_settings.ContainsKey(key) = False) Then
                                _settings.Add(key, objSettings(key))
                            End If
                        Next
                    End If

                    _articleSettings = New ArticleSettings(_settings, Me.PortalSettings, Me.ModuleConfiguration)

                End If
                Return _articleSettings
            End Get
        End Property

        Private ReadOnly Property ArticleTabInfo() As DotNetNuke.Entities.Tabs.TabInfo
            Get
                If (_articleTabInfo Is Nothing) Then
                    Dim objTabController As New DotNetNuke.Entities.Tabs.TabController
                    _articleTabInfo = objTabController.GetTab(_articleTabID, Me.PortalId, False)
                End If

                Return _articleTabInfo
            End Get
        End Property

        Private ReadOnly Property LatestAuthorID() As Integer
            Get
                Dim id As Integer = Null.NullInteger
                If (Request("laauth-" & TabModuleId.ToString()) <> "") Then
                    If (IsNumeric(Request("laauth-" & TabModuleId.ToString()))) Then
                        id = Convert.ToInt32(Request("laauth-" & TabModuleId.ToString()))
                    End If
                End If

                Return id
            End Get
        End Property

        Private ReadOnly Property CategoryID() As Integer
            Get
                Dim id As Integer = Null.NullInteger
                If (Request("lacat-" & TabModuleId.ToString()) <> "") Then
                    If (IsNumeric(Request("lacat-" & TabModuleId.ToString()))) Then
                        id = Convert.ToInt32(Request("lacat-" & TabModuleId.ToString()))
                    End If
                End If

                Return id
            End Get
        End Property

        Private Shadows ReadOnly Property ServerTimeZone() As Integer
            Get
                If (_serverTimeZone = Null.NullInteger) Then

                    _serverTimeZone = Ventrian.NewsArticles.Common.GetTimeZoneInteger(PortalSettings.TimeZone)

                    Dim objModuleSettingController As New ModuleController
                    Dim newsSettings As Hashtable = objModuleSettingController.GetModuleSettings(_articleModuleID)

                    If Not (newsSettings Is Nothing) Then
                        If (newsSettings.Contains(ArticleConstants.SERVER_TIMEZONE)) Then
                            If (IsNumeric(newsSettings(ArticleConstants.SERVER_TIMEZONE).ToString())) Then
                                _serverTimeZone = Convert.ToInt32(newsSettings(ArticleConstants.SERVER_TIMEZONE).ToString())
                            End If
                        End If
                    End If
                End If

                Return _serverTimeZone
            End Get
        End Property

        Private ReadOnly Property SortBy() As String
            Get
                Dim sort As String = ArticleConstants.DEFAULT_SORT_BY
                If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_SORT_BY)) Then
                    sort = Settings(ArticleConstants.LATEST_ARTICLES_SORT_BY).ToString()
                End If
                If (Request("lasort-" & TabModuleId.ToString()) <> "") Then
                    Select Case Request("lasort-" & TabModuleId.ToString()).ToLower()
                        Case "publishdate"
                            sort = "StartDate"
                            Exit Select
                        Case "expirydate"
                            sort = "EndDate"
                            Exit Select
                        Case "lastupdate"
                            sort = "LastUpdate"
                            Exit Select
                        Case "rating"
                            sort = "Rating DESC, RatingCount"
                            Exit Select
                        Case "commentcount"
                            sort = "CommentCount"
                            Exit Select
                        Case "numberofviews"
                            sort = "NumberOfViews"
                            Exit Select
                        Case "random"
                            sort = "NewID()"
                            Exit Select
                        Case "title"
                            sort = "Title"
                            Exit Select
                    End Select
                End If

                Return sort
            End Get
        End Property

        Private ReadOnly Property StartPoint() As Integer
            Get
                If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_START_POINT)) Then
                    Return Convert.ToInt32(Settings(ArticleConstants.LATEST_ARTICLES_START_POINT).ToString())
                Else
                    Return 0
                End If
            End Get
        End Property

        Private ReadOnly Property Time() As String
            Get
                Dim val As String = ""

                If (Request("latime-" & TabModuleId.ToString()) <> "") Then
                    val = Request("latime-" & TabModuleId.ToString())
                End If

                Return val
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub BindArticles()

            Dim objArticleController As New ArticleController

            _rssLink = "~/DesktopModules/DnnForge - NewsArticles/Rss.aspx?TabID=" & _articleTabID.ToString() & "&amp;ModuleID=" & _articleModuleID.ToString()

            Dim cats As Integer() = Nothing

            Dim showRelated As Boolean = False
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_SHOW_RELATED)) Then
                showRelated = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_SHOW_RELATED).ToString())
            End If

            If (showRelated And Request("CategoryID") <> "") Then
                If (IsNumeric(Request("CategoryID"))) Then
                    Dim categories As New List(Of Integer)
                    categories.Add(Convert.ToInt32(Request("CategoryID")))
                    cats = categories.ToArray()
                    _rssLink = _rssLink & "&CategoryID=" & Request("CategoryID")
                Else
                    showRelated = False
                End If
            End If

            If (showRelated And (ArticleSettings.UrlModeType = Components.Types.UrlModeType.Classic) And Request("ArticleID") <> "") Then
                If (IsNumeric(Request("ArticleID"))) Then
                    Dim categories As ArrayList = objArticleController.GetArticleCategories(Convert.ToInt32(Request("ArticleID")))

                    Dim categoriesRSS As String = ""
                    Dim categoriesList As New List(Of Integer)
                    For Each objCategory As CategoryInfo In categories
                        categoriesList.Add(objCategory.CategoryID)
                        If (categoriesRSS = "") Then
                            categoriesRSS = objCategory.CategoryID.ToString()
                        Else
                            categoriesRSS = categoriesRSS & "," & objCategory.CategoryID.ToString()
                        End If
                    Next
                    cats = categoriesList.ToArray()
                    If (categoriesList.Count > 0) Then
                        _rssLink = _rssLink & "&CategoryID=" & categoriesRSS
                    End If
                End If
            End If

            If (showRelated And (ArticleSettings.UrlModeType = Components.Types.UrlModeType.Shorterned) And Request(ArticleSettings.ShortenedID) <> "") Then
                If (IsNumeric(Request(ArticleSettings.ShortenedID))) Then
                    Dim categories As ArrayList = objArticleController.GetArticleCategories(Convert.ToInt32(Request(ArticleSettings.ShortenedID)))

                    Dim categoriesRSS As String = ""
                    Dim categoriesList As New List(Of Integer)
                    For Each objCategory As CategoryInfo In categories
                        categoriesList.Add(objCategory.CategoryID)
                        If (categoriesRSS = "") Then
                            categoriesRSS = objCategory.CategoryID.ToString()
                        Else
                            categoriesRSS = categoriesRSS & "," & objCategory.CategoryID.ToString()
                        End If
                    Next
                    cats = categoriesList.ToArray()
                    If (categoriesList.Count > 0) Then
                        _rssLink = _rssLink & "&CategoryID=" & categoriesRSS
                    End If
                End If
            End If

            Dim catsExclude As Integer() = Nothing

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_CATEGORIES_EXCLUDE)) Then
                If Not (Settings(ArticleConstants.LATEST_ARTICLES_CATEGORIES_EXCLUDE).ToString = Null.NullString Or Settings(ArticleConstants.LATEST_ARTICLES_CATEGORIES_EXCLUDE).ToString = "-1") Then
                    Dim categories As String() = Settings(ArticleConstants.LATEST_ARTICLES_CATEGORIES_EXCLUDE).ToString().Split(Char.Parse(","))

                    Dim categoriesToExclude(categories.Length - 1) As Integer
                    For i As Integer = 0 To categories.Length - 1
                        categoriesToExclude(i) = Convert.ToInt32(categories(i))
                    Next

                    If (categories.Length > 0) Then
                        _rssLink = _rssLink & "&CategoryIDExclude=" & Settings(ArticleConstants.LATEST_ARTICLES_CATEGORIES_EXCLUDE).ToString()
                    End If

                    catsExclude = categoriesToExclude
                End If
            End If

            Dim objCategoryController As CategoryController = New CategoryController
            Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(_articleModuleID, Null.NullInteger, ArticleSettings.CategorySortType)

            Dim excludeCategoriesRestrictive As New List(Of Integer)

            For Each objCategory As CategoryInfo In objCategories
                If (objCategory.InheritSecurity = False And objCategory.CategorySecurityType = CategorySecurityType.Restrict) Then
                    If (Request.IsAuthenticated) Then
                        If (_settings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                            If (PortalSecurity.IsInRoles(_settings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString()) = False) Then
                                excludeCategoriesRestrictive.Add(objCategory.CategoryID)
                            End If
                        End If
                    Else
                        excludeCategoriesRestrictive.Add(objCategory.CategoryID)
                    End If
                End If
            Next

            If (catsExclude Is Nothing) Then
                If (excludeCategoriesRestrictive.Count > 0) Then
                    catsExclude = excludeCategoriesRestrictive.ToArray()
                End If
            End If

            Dim excludeCategories As New List(Of Integer)

            If (catsExclude Is Nothing) Then

                For Each objCategory As CategoryInfo In objCategories
                    If (objCategory.InheritSecurity = False) Then
                        If (Request.IsAuthenticated) Then
                            If (_settings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                If (PortalSecurity.IsInRoles(_settings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString()) = False) Then
                                    excludeCategories.Add(objCategory.CategoryID)
                                End If
                            End If
                        Else
                            excludeCategories.Add(objCategory.CategoryID)
                        End If
                    End If
                Next

            End If

            If (showRelated = False) Then
                If (CategoryID = Null.NullInteger) Then
                    If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_CATEGORIES)) Then
                        If Not (Settings(ArticleConstants.LATEST_ARTICLES_CATEGORIES).ToString = Null.NullString Or Settings(ArticleConstants.LATEST_ARTICLES_CATEGORIES).ToString = "-1") Then
                            Dim categories As String() = Settings(ArticleConstants.LATEST_ARTICLES_CATEGORIES).ToString().Split(Char.Parse(","))

                            Dim categoriesToDisplay(categories.Length - 1) As Integer
                            For i As Integer = 0 To categories.Length - 1
                                categoriesToDisplay(i) = Convert.ToInt32(categories(i))
                            Next

                            If (categories.Length > 0) Then
                                _rssLink = _rssLink & "&CategoryID=" & Settings(ArticleConstants.LATEST_ARTICLES_CATEGORIES).ToString()
                            End If

                            cats = categoriesToDisplay
                        Else

                            If (excludeCategories.Count > 0) Then
                                Dim includeCategories As New List(Of Integer)

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

                                cats = includeCategories.ToArray()

                            End If

                        End If
                    Else

                        If (excludeCategories.Count > 0) Then
                            Dim includeCategories As New List(Of Integer)

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

                            cats = includeCategories.ToArray()

                        End If

                    End If
                Else
                    Dim categoriesToDisplay(1) As Integer
                    categoriesToDisplay(1) = CategoryID
                    cats = categoriesToDisplay
                    _rssLink = _rssLink & "&CategoryID=" & CategoryID.ToString()
                End If
            End If

            Dim matchOperator As MatchOperatorType = MatchOperatorType.MatchAny
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_MATCH_OPERATOR)) Then
                matchOperator = CType(System.Enum.Parse(GetType(MatchOperatorType), Settings(ArticleConstants.LATEST_ARTICLES_MATCH_OPERATOR).ToString()), MatchOperatorType)
            End If

            Dim matchAll As Boolean = False
            If (matchOperator = MatchOperatorType.MatchAll) Then
                matchAll = True
                _rssLink = _rssLink & "&MatchCat=1"
            End If

            Dim count As Integer = 10
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_COUNT)) Then
                count = Convert.ToInt32(Settings(ArticleConstants.LATEST_ARTICLES_COUNT).ToString())
            End If

            _rssLink = _rssLink & "&MaxCount=" & count.ToString()

            Dim maxAge As Integer = Null.NullInteger
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_MAX_AGE)) Then
                If (Settings(ArticleConstants.LATEST_ARTICLES_MAX_AGE).ToString() <> "") Then
                    maxAge = Convert.ToInt32(Settings(ArticleConstants.LATEST_ARTICLES_MAX_AGE).ToString()) * -1
                End If
            End If

            Dim featuredOnly As Boolean = False
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_FEATURED_ONLY)) Then
                featuredOnly = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_FEATURED_ONLY).ToString())
            End If

            If (featuredOnly) Then
                _rssLink = _rssLink & "&FeaturedOnly=" & featuredOnly.ToString()
            End If

            Dim notFeaturedOnly As Boolean = False
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_NOT_FEATURED_ONLY)) Then
                notFeaturedOnly = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_NOT_FEATURED_ONLY).ToString())
            End If

            If (notFeaturedOnly) Then
                _rssLink = _rssLink & "&NotFeaturedOnly=" & notFeaturedOnly.ToString()
            End If

            Dim securedOnly As Boolean = False
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_SECURED_ONLY)) Then
                securedOnly = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_SECURED_ONLY).ToString())
            End If

            If (securedOnly) Then
                _rssLink = _rssLink & "&SecuredOnly=" & securedOnly.ToString()
            End If

            Dim notSecuredOnly As Boolean = False
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_NOT_SECURED_ONLY)) Then
                notSecuredOnly = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_NOT_SECURED_ONLY).ToString())
            End If

            If (notFeaturedOnly) Then
                _rssLink = _rssLink & "&NotSecuredOnly=" & notSecuredOnly.ToString()
            End If

            Dim sort As String = SortBy
            _rssLink = _rssLink & "&sortBy=" & sort

            Dim sortDirection As String = ArticleConstants.DEFAULT_SORT_DIRECTION
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_SORT_DIRECTION)) Then
                sortDirection = Settings(ArticleConstants.LATEST_ARTICLES_SORT_DIRECTION).ToString()
            End If
            If (sort = "Title" And Request("lasort-" & TabModuleId.ToString()) <> "") Then
                sortDirection = "ASC"
            End If
            If (sort <> "Title" And Request("lasort-" & TabModuleId.ToString()) <> "") Then
                sortDirection = "DESC"
            End If
            _rssLink = _rssLink & "&sortDirection=" & sortDirection

            _layoutMode = ArticleConstants.LATEST_ARTICLES_LAYOUT_MODE_DEFAULT
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_LAYOUT_MODE)) Then
                _layoutMode = CType(System.Enum.Parse(GetType(LayoutModeType), Settings(ArticleConstants.LATEST_ARTICLES_LAYOUT_MODE).ToString()), LayoutModeType)
            End If

            Dim itemsPerRow As Integer = ArticleConstants.LATEST_ARTICLES_ITEMS_PER_ROW_DEFAULT
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_ITEMS_PER_ROW)) Then
                itemsPerRow = Convert.ToInt32(Settings(ArticleConstants.LATEST_ARTICLES_ITEMS_PER_ROW).ToString())
            End If

            Dim showPending As Boolean = False
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_SHOW_PENDING)) Then
                showPending = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_SHOW_PENDING).ToString())
            End If

            Dim authorID As Integer = ArticleConstants.LATEST_ARTICLES_AUTHOR_DEFAULT
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_AUTHOR)) Then
                authorID = Convert.ToInt32(Settings(ArticleConstants.LATEST_ARTICLES_AUTHOR).ToString())
            End If

            Dim userIDFilter As String = ""
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_QUERY_STRING_FILTER) And Settings.Contains(ArticleConstants.LATEST_ARTICLES_QUERY_STRING_PARAM)) Then
                If (Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_QUERY_STRING_FILTER).ToString())) Then
                    authorID = -100
                    Dim param As String = Settings(ArticleConstants.LATEST_ARTICLES_QUERY_STRING_PARAM).ToString()
                    If (param <> "") Then
                        If (Request.QueryString(param) <> "" AndAlso IsNumeric(Request.QueryString(param))) Then
                            userIDFilter = param & "=" & Request.QueryString(param)
                            authorID = Convert.ToInt32(Request(param))
                        End If
                    End If
                End If
            End If

            Dim usernameFilter As String = ""
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_USERNAME_FILTER) And Settings.Contains(ArticleConstants.LATEST_ARTICLES_USERNAME_PARAM)) Then
                If (Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_USERNAME_FILTER).ToString())) Then
                    authorID = -100
                    Dim param As String = Settings(ArticleConstants.LATEST_ARTICLES_USERNAME_PARAM).ToString()
                    If (param <> "") Then
                        If (Request.QueryString(param) <> "") Then
                            usernameFilter = param & "=" & Request.QueryString(param)
                            Dim objUser As DotNetNuke.Entities.Users.UserInfo = DotNetNuke.Entities.Users.UserController.GetUserByName(Me.PortalId, Request.QueryString(param))
                            If (objUser IsNot Nothing) Then
                                authorID = objUser.UserID
                            End If
                        End If
                    End If
                End If
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_LOGGED_IN_USER_FILTER)) Then
                If (Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_LOGGED_IN_USER_FILTER).ToString())) Then
                    authorID = -100
                    If (Request.IsAuthenticated) Then
                        authorID = Me.UserId
                    End If
                End If
            End If

            If (authorID = Null.NullInteger) Then
                authorID = LatestAuthorID
            End If

            Dim startDate As DateTime = DateTime.Now
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_START_DATE)) Then
                If (Settings(ArticleConstants.LATEST_ARTICLES_START_DATE).ToString() <> "") Then
                    startDate = Convert.ToDateTime(Settings(ArticleConstants.LATEST_ARTICLES_START_DATE).ToString())
                End If
            End If

            Dim bubbleFeatured As Boolean = False
            If (Settings.Contains(ArticleConstants.BUBBLE_FEATURED_ARTICLES)) Then
                bubbleFeatured = Convert.ToBoolean(Settings(ArticleConstants.BUBBLE_FEATURED_ARTICLES).ToString())
                If (bubbleFeatured) Then
                    sort = "IsFeatured DESC, " & sort
                End If
            End If

            Dim articleIDs As String = ""
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_IDS)) Then
                If (Settings(ArticleConstants.LATEST_ARTICLES_IDS).ToString() <> "") Then
                    _rssLink = _rssLink & "&ArticleIDs=" & Settings(ArticleConstants.LATEST_ARTICLES_IDS).ToString()
                    articleIDs = Settings(ArticleConstants.LATEST_ARTICLES_IDS).ToString()
                End If
            End If

            Dim agedDate As DateTime = Null.NullDate
            If (maxAge <> Null.NullInteger) Then
                If (startDate = Null.NullDate) Then
                    agedDate = DateTime.Now.AddDays(maxAge)
                Else
                    agedDate = startDate.AddDays(maxAge)
                End If
            Else
                If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_MAX_AGE)) Then
                    If (Settings(ArticleConstants.LATEST_ARTICLES_MAX_AGE).ToString() = "1") Then
                        agedDate = startDate.AddDays(-1)
                    End If
                End If
            End If

            If (Time <> "") Then
                If (Time.ToLower() = "today") Then
                    startDate = DateTime.Now
                    agedDate = DateTime.Today
                End If
                If (Time.ToLower() = "yesterday") Then
                    startDate = DateTime.Today
                    agedDate = DateTime.Today.AddDays(-1)
                End If
                If (Time.ToLower() = "threedays") Then
                    startDate = DateTime.Now
                    agedDate = DateTime.Today.AddDays(-3)
                End If
                If (Time.ToLower() = "sevendays") Then
                    startDate = DateTime.Now
                    agedDate = DateTime.Today.AddDays(-7)
                End If
                If (Time.ToLower() = "thirtydays") Then
                    startDate = DateTime.Now
                    agedDate = DateTime.Today.AddDays(-30)
                End If
                If (Time.ToLower() = "ninetydays") Then
                    startDate = DateTime.Now
                    agedDate = DateTime.Today.AddDays(-90)
                End If
                If (Time.ToLower() = "thisyear") Then
                    startDate = DateTime.Now
                    agedDate = DateTime.Today.AddYears(-1)
                End If
            End If

            Dim tagIDs() As Integer = Nothing
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_TAGS)) Then
                Dim objTags As New List(Of Integer)
                Dim objTagController As New TagController
                Dim tags As String = Settings(ArticleConstants.LATEST_ARTICLES_TAGS).ToString()
                If (tags <> "") Then
                    For Each tag As String In tags.Split(","c)
                        objTags.Add(Convert.ToInt32(tag))
                    Next
                    If (objTags.Count > 0) Then
                        tagIDs = objTags.ToArray()
                        Dim rssTags As String = ""
                        For Each tagID As Integer In tagIDs
                            If (rssTags = "") Then
                                rssTags = tagID.ToString()
                            Else
                                rssTags = rssTags & "," & tagID.ToString()
                            End If
                        Next
                        If (rssTags <> "") Then
                            _rssLink = _rssLink & "&TagIDs=" & rssTags
                        End If
                    End If
                End If
            End If

            Dim matchOperatorTags As MatchOperatorType = MatchOperatorType.MatchAny
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_TAGS_MATCH_OPERATOR)) Then
                matchOperatorTags = CType(System.Enum.Parse(GetType(MatchOperatorType), Settings(ArticleConstants.LATEST_ARTICLES_TAGS_MATCH_OPERATOR).ToString()), MatchOperatorType)
            End If

            Dim matchAllTags As Boolean = False
            If (tagIDs IsNot Nothing) Then
                If (matchOperatorTags = MatchOperatorType.MatchAll) Then
                    matchAllTags = True
                    _rssLink = _rssLink & "&MatchTag=1"
                End If
            End If

            Dim customFieldID As Integer = Null.NullInteger
            Dim customValue As String = Null.NullString
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_CUSTOM_FIELD_FILTER) And Settings.Contains(ArticleConstants.LATEST_ARTICLES_CUSTOM_FIELD_VALUE)) Then
                If (IsNumeric(Settings(ArticleConstants.LATEST_ARTICLES_CUSTOM_FIELD_FILTER).ToString())) Then
                    customFieldID = Convert.ToInt32(Settings(ArticleConstants.LATEST_ARTICLES_CUSTOM_FIELD_FILTER).ToString())
                End If
                customValue = Settings(ArticleConstants.LATEST_ARTICLES_CUSTOM_FIELD_VALUE).ToString()
            End If

            Dim linkFilter As String = Null.NullString
            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_LINK_FILTER)) Then
                linkFilter = Settings(ArticleConstants.LATEST_ARTICLES_LINK_FILTER).ToString()
            End If

            If (Request("lacust-" & Me.TabModuleId.ToString()) <> "") Then
                Dim val As String = Request("lacust-" & Me.TabModuleId.ToString())
                If (val.Split("-"c).Length = 2) Then
                    If (IsNumeric(val.Split("-"c)(0))) Then
                        customFieldID = Convert.ToInt32(val.Split("-"c)(0))
                        customValue = val.Split("-"c)(1)
                    End If
                End If
            End If

            Dim doPaging As Boolean = False
            If (Settings.Contains(ArticleConstants.LATEST_ENABLE_PAGER)) Then
                doPaging = Convert.ToBoolean(Settings(ArticleConstants.LATEST_ENABLE_PAGER).ToString())
            End If

            Dim pageSize As Integer = count
            If (doPaging) Then
                If (Settings.Contains(ArticleConstants.LATEST_PAGE_SIZE)) Then
                    pageSize = Convert.ToInt32(Settings(ArticleConstants.LATEST_PAGE_SIZE).ToString())
                Else
                    pageSize = ArticleConstants.LATEST_PAGE_SIZE_DEFAULT
                End If
            End If

            Dim objModuleController As New ModuleController()
            Dim objModule As ModuleInfo = objModuleController.GetModule(_articleModuleID, _articleTabID)
            ' _objLayoutController = New LayoutController(PortalSettings, ArticleSettings, Page, Me.IsEditable, _articleTabID, _articleModuleID, Me.TabModuleId, Me.PortalId, Null.NullInteger, Me.UserId, Me.ModuleKey)
            _objLayoutController = New LayoutController(PortalSettings, ArticleSettings, objModule, Page)
            Dim objLatestLayoutController As New LatestLayoutController()

            _objLayoutHeader = objLatestLayoutController.GetLayout(LatestLayoutType.Listing_Header_Html, ModuleId, Settings)
            _objLayoutItem = objLatestLayoutController.GetLayout(LatestLayoutType.Listing_Item_Html, ModuleId, Settings)
            _objLayoutFooter = objLatestLayoutController.GetLayout(LatestLayoutType.Listing_Footer_Html, ModuleId, Settings)

            Dim articleCount As Integer = 0
            Dim objArticles As List(Of ArticleInfo) = objArticleController.GetArticleList(_articleModuleID, startDate, agedDate, cats, matchAll, catsExclude, count, _pageNumber, pageSize, sort, sortDirection, True, False, Null.NullString, authorID, showPending, False, featuredOnly, notFeaturedOnly, securedOnly, notSecuredOnly, articleIDs, tagIDs, matchAllTags, Null.NullString, customFieldID, customValue, linkFilter, articleCount)

            If (count < articleCount) Then
                articleCount = count
            End If

            If (_layoutMode = LayoutModeType.Simple) Then
                rptLatestArticles.DataSource = objArticles
                rptLatestArticles.DataBind()
                rptLatestArticles.Visible = (objArticles.Count > 0)
            Else
                dlLatestArticles.RepeatColumns = itemsPerRow
                dlLatestArticles.DataSource = objArticles
                dlLatestArticles.DataBind()
                dlLatestArticles.Visible = (objArticles.Count > 0)
            End If

            Dim pageCount As Integer = ((articleCount - 1) \ pageSize) + 1

            If ((objArticles.Count > 0) AndAlso pageCount > 1) Then
                ctlPagingControl.Visible = True
                ctlPagingControl.PageParam = "lapg-" & Me.TabModuleId.ToString()
                Dim params As New List(Of String)
                If (userIDFilter <> "") Then
                    params.Add(userIDFilter)
                End If
                If (usernameFilter <> "") Then
                    params.Add(usernameFilter)
                End If
                If (Request("lasort-" & TabModuleId.ToString()) <> "") Then
                    params.Add("lasort-" & TabModuleId.ToString() & "=" & Request("lasort-" & TabModuleId.ToString()))
                End If
                If (Request("laauth-" & TabModuleId.ToString()) <> "") Then
                    params.Add("laauth-" & TabModuleId.ToString() & "=" & Request("laauth-" & TabModuleId.ToString()))
                End If
                If (Request("lacat-" & TabModuleId.ToString()) <> "") Then
                    params.Add("lacat-" & TabModuleId.ToString() & "=" & Request("lacat-" & TabModuleId.ToString()))
                End If
                If (Request("lacust-" & TabModuleId.ToString()) <> "") Then
                    params.Add("lacust-" & TabModuleId.ToString() & "=" & Request("lacust-" & TabModuleId.ToString()))
                End If
                If (Request("latime-" & TabModuleId.ToString()) <> "") Then
                    params.Add("latime-" & TabModuleId.ToString() & "=" & Request("latime-" & TabModuleId.ToString()))
                End If
                For Each item As String In params
                    If (ctlPagingControl.QuerystringParams <> "") Then
                        ctlPagingControl.QuerystringParams = ctlPagingControl.QuerystringParams & "&" & item
                    Else
                        ctlPagingControl.QuerystringParams = item
                    End If
                Next
                ctlPagingControl.TotalRecords = articleCount
                ctlPagingControl.PageSize = pageSize
                ctlPagingControl.CurrentPage = _pageNumber
                ctlPagingControl.TabID = TabId
                ctlPagingControl.EnableViewState = False
            End If

            If (objArticles.Count = 0) Then
                Dim objNoArticles As LayoutInfo = objLatestLayoutController.GetLayout(LatestLayoutType.Listing_Empty_Html, ModuleId, Settings)
                ProcessHeaderFooter(phNoArticles.Controls, objNoArticles.Tokens)
            End If

        End Sub

        Private Function FindSettings() As Boolean

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_TAB_ID)) Then
                If (IsNumeric(Settings(ArticleConstants.LATEST_ARTICLES_TAB_ID).ToString())) Then
                    _articleTabID = Convert.ToInt32(Settings(ArticleConstants.LATEST_ARTICLES_TAB_ID).ToString())
                End If
            End If

            If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_MODULE_ID)) Then
                If (IsNumeric(Settings(ArticleConstants.LATEST_ARTICLES_MODULE_ID).ToString())) Then
                    _articleModuleID = Convert.ToInt32(Settings(ArticleConstants.LATEST_ARTICLES_MODULE_ID).ToString())
                    If (_articleModuleID <> Null.NullInteger) Then
                        Return True
                    End If
                End If
            End If

            Return False

        End Function

        Private Sub ProcessHeaderFooter(ByRef controls As ControlCollection, ByVal layoutArray As String())

            Dim moduleKey As String = "la-" & Me.TabModuleId.ToString() & "-Header-"

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                controls.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)

                        Case "AUTHOR"
                            Dim objAuthorController As New AuthorController()
                            Dim drpAuthor As New DropDownList
                            drpAuthor.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                            drpAuthor.DataTextField = "DisplayName"
                            drpAuthor.DataValueField = "UserID"
                            drpAuthor.DataSource = objAuthorController.GetAuthorList(_articleModuleID)
                            drpAuthor.DataBind()
                            drpAuthor.Items.Insert(0, New ListItem(Localization.GetString("SelectAuthor", Me.LocalResourceFile), "-1"))
                            drpAuthor.AutoPostBack = True
                            If (LatestAuthorID <> Null.NullInteger) Then
                                If (drpAuthor.Items.FindByValue(LatestAuthorID.ToString()) IsNot Nothing) Then
                                    drpAuthor.SelectedValue = LatestAuthorID.ToString()
                                End If
                            End If
                            Dim objHandler As New System.EventHandler(AddressOf drpAuthor_SelectedIndexChanged)
                            AddHandler drpAuthor.SelectedIndexChanged, objHandler
                            controls.Add(drpAuthor)

                        Case "CATEGORY"
                            Dim objCategoryController As New CategoryController()
                            Dim drpCategory As New DropDownList
                            drpCategory.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                            drpCategory.DataTextField = "NameIndented"
                            drpCategory.DataValueField = "CategoryID"
                            drpCategory.DataSource = objCategoryController.GetCategoriesAll(_articleModuleID, Null.NullInteger, _articleSettings.CategorySortType)
                            drpCategory.DataBind()
                            drpCategory.Items.Insert(0, New ListItem(Localization.GetString("SelectCategory", Me.LocalResourceFile), "-1"))
                            drpCategory.AutoPostBack = True
                            If (CategoryID <> Null.NullInteger) Then
                                If (drpCategory.Items.FindByValue(CategoryID.ToString()) IsNot Nothing) Then
                                    drpCategory.SelectedValue = CategoryID.ToString()
                                End If
                            End If
                            Dim objHandler As New System.EventHandler(AddressOf drpCategory_SelectedIndexChanged)
                            AddHandler drpCategory.SelectedIndexChanged, objHandler
                            controls.Add(drpCategory)

                        Case "RSSLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = Me.ResolveUrl(_rssLink)
                            controls.Add(objLiteral)

                        Case "SORT"
                            Dim drpSort As New DropDownList
                            drpSort.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                            drpSort.Items.Add(New ListItem(Localization.GetString("PublishDate.Text", Me.LocalResourceFile), "PublishDate"))
                            drpSort.Items.Add(New ListItem(Localization.GetString("ExpiryDate.Text", Me.LocalResourceFile), "ExpiryDate"))
                            drpSort.Items.Add(New ListItem(Localization.GetString("LastUpdate.Text", Me.LocalResourceFile), "LastUpdate"))
                            drpSort.Items.Add(New ListItem(Localization.GetString("HighestRated.Text", Me.LocalResourceFile), "Rating"))
                            drpSort.Items.Add(New ListItem(Localization.GetString("MostCommented.Text", Me.LocalResourceFile), "CommentCount"))
                            drpSort.Items.Add(New ListItem(Localization.GetString("MostViewed.Text", Me.LocalResourceFile), "NumberOfViews"))
                            drpSort.Items.Add(New ListItem(Localization.GetString("Random.Text", Me.LocalResourceFile), "Random"))
                            drpSort.Items.Add(New ListItem(Localization.GetString("SortTitle.Text", Me.LocalResourceFile), "Title"))
                            drpSort.AutoPostBack = True

                            If (Request("lasort-" & TabModuleId.ToString()) <> "") Then
                                If (drpSort.Items.FindByValue(Request("lasort-" & TabModuleId.ToString())) IsNot Nothing) Then
                                    drpSort.SelectedValue = Request("lasort-" & TabModuleId.ToString())
                                End If
                            Else
                                Dim sort As String = SortBy

                                Select Case SortBy.ToLower()
                                    Case "startdate"
                                        sort = "PublishDate"
                                        Exit Select
                                    Case "enddate"
                                        sort = "ExpiryDate"
                                        Exit Select
                                    Case "newid()"
                                        sort = "random"
                                        Exit Select
                                End Select

                                If (drpSort.Items.FindByValue(sort) IsNot Nothing) Then
                                    drpSort.SelectedValue = sort
                                End If
                            End If

                            Dim objHandler As New System.EventHandler(AddressOf drpSort_SelectedIndexChanged)
                            AddHandler drpSort.SelectedIndexChanged, objHandler
                            controls.Add(drpSort)

                        Case "TIME"
                            Dim drpTime As New DropDownList
                            drpTime.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())

                            drpTime.Items.Add(New ListItem(Localization.GetString("Today", Me.LocalResourceFile), "Today"))
                            drpTime.Items.Add(New ListItem(Localization.GetString("Yesterday", Me.LocalResourceFile), "Yesterday"))
                            drpTime.Items.Add(New ListItem(Localization.GetString("ThreeDays", Me.LocalResourceFile), "ThreeDays"))
                            drpTime.Items.Add(New ListItem(Localization.GetString("SevenDays", Me.LocalResourceFile), "SevenDays"))
                            drpTime.Items.Add(New ListItem(Localization.GetString("ThirtyDays", Me.LocalResourceFile), "ThirtyDays"))
                            drpTime.Items.Add(New ListItem(Localization.GetString("NinetyDays", Me.LocalResourceFile), "NinetyDays"))
                            drpTime.Items.Add(New ListItem(Localization.GetString("ThisYear", Me.LocalResourceFile), "ThisYear"))
                            drpTime.Items.Add(New ListItem(Localization.GetString("AllTime", Me.LocalResourceFile), "AllTime"))
                            drpTime.AutoPostBack = True

                            If (Time <> "") Then
                                If (drpTime.Items.FindByValue(Time) IsNot Nothing) Then
                                    drpTime.SelectedValue = Time
                                End If
                            Else
                                drpTime.SelectedValue = "AllTime"
                            End If

                            Dim objHandler As New System.EventHandler(AddressOf drpTime_SelectedIndexChanged)
                            AddHandler drpTime.SelectedIndexChanged, objHandler

                            controls.Add(drpTime)

                        Case Else

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CUSTOM:")) Then
                                Dim customField As String = layoutArray(iPtr + 1).Substring(7, layoutArray(iPtr + 1).Length - 7)

                                Dim objCustomFieldController As New CustomFieldController()
                                Dim objCustomFields As ArrayList = objCustomFieldController.List(_articleModuleID)

                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.Name.ToLower() = customField.ToLower()) Then
                                        If (objCustomField.FieldType = CustomFieldType.DropDownList) Then
                                            Dim drpCustom As New DropDownList
                                            drpCustom.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())

                                            For Each val As String In objCustomField.FieldElements.Split("|"c)
                                                drpCustom.Items.Add(val)
                                            Next

                                            Dim sel As String = Localization.GetString("SelectCustom", Me.LocalResourceFile)
                                            If (sel.IndexOf("{0}") <> -1) Then
                                                sel = sel.Replace("{0}", objCustomField.Caption)
                                            End If
                                            drpCustom.Items.Insert(0, New ListItem(sel, "-1"))
                                            drpCustom.Attributes.Add("CustomFieldID", objCustomField.CustomFieldID.ToString())
                                            drpCustom.AutoPostBack = True


                                            If (Request("lacust-" & TabModuleId.ToString()) <> "") Then
                                                Dim val As String = Request("lacust-" & TabModuleId.ToString())
                                                If (val.Split("-"c).Length = 2) Then
                                                    If (val.Split("-"c)(0) = objCustomField.CustomFieldID.ToString()) Then
                                                        If (drpCustom.Items.FindByValue(val.Split("-"c)(1).ToString()) IsNot Nothing) Then
                                                            drpCustom.SelectedValue = val.Split("-"c)(1).ToString()
                                                        End If
                                                    End If
                                                End If
                                            End If

                                            Dim objHandler As New System.EventHandler(AddressOf drpCustom_SelectedIndexChanged)
                                            AddHandler drpCustom.SelectedIndexChanged, objHandler
                                            controls.Add(drpCustom)

                                        End If
                                        Exit For
                                    End If
                                Next

                                Exit Select

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("SORT:")) Then

                                Dim params As New List(Of String)

                                Dim sortItem As String = layoutArray(iPtr + 1).Substring(5, layoutArray(iPtr + 1).Length - 5)
                                Dim sortValue As String = sortItem

                                Select Case sortItem.ToLower()
                                    Case "highestrated"
                                        sortValue = "Rating"
                                        Exit Select
                                    Case "mostcommented"
                                        sortValue = "CommentCount"
                                        Exit Select
                                    Case "mostviewed"
                                        sortValue = "NumberOfViews"
                                        Exit Select
                                    Case "sorttitle"
                                        sortValue = "Title"
                                        Exit Select
                                End Select

                                Dim sort As String = SortBy

                                Select Case sort.ToLower()
                                    Case "startdate"
                                        sort = "PublishDate"
                                        Exit Select
                                    Case "enddate"
                                        sort = "ExpiryDate"
                                        Exit Select
                                    Case "newid()"
                                        sort = "random"
                                        Exit Select
                                    Case "rating desc, ratingcount"
                                        sort = "rating"
                                        Exit Select
                                End Select

                                If (sortValue.ToLower() = sort.ToLower()) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                                    objLiteral.Text = Localization.GetString(sortItem & ".Text", Me.LocalResourceFile)
                                    controls.Add(objLiteral)
                                Else
                                    Dim objLink As New HyperLink
                                    objLink.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                                    params.Add("lasort-" & TabModuleId.ToString() & "=" & sortValue)
                                    If (Request("laauth-" & TabModuleId.ToString()) <> "") Then
                                        params.Add("laauth-" & TabModuleId.ToString() & "=" & Request("laauth-" & TabModuleId.ToString()))
                                    End If
                                    If (Request("lacat-" & TabModuleId.ToString()) <> "") Then
                                        params.Add("lacat-" & TabModuleId.ToString() & "=" & Request("lacat-" & TabModuleId.ToString()))
                                    End If
                                    If (Request("lacust-" & TabModuleId.ToString()) <> "") Then
                                        params.Add("lacust-" & TabModuleId.ToString() & "=" & Request("lacust-" & TabModuleId.ToString()))
                                    End If
                                    If (Request("latime-" & TabModuleId.ToString()) <> "") Then
                                        params.Add("latime-" & TabModuleId.ToString() & "=" & Request("latime-" & TabModuleId.ToString()))
                                    End If
                                    objLink.NavigateUrl = NavigateURL(Me.TabId, "", params.ToArray())
                                    objLink.Text = Localization.GetString(sortItem & ".Text", Me.LocalResourceFile)
                                    controls.Add(objLink)
                                End If
                                Exit Select

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("TIME:")) Then

                                Dim timeItem As String = layoutArray(iPtr + 1).Substring(5, layoutArray(iPtr + 1).Length - 5)

                                Dim drpTime As New DropDownList
                                drpTime.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())

                                drpTime.Items.Add(New ListItem(Localization.GetString("Today", Me.LocalResourceFile), "Today"))
                                drpTime.Items.Add(New ListItem(Localization.GetString("Yesterday", Me.LocalResourceFile), "Yesterday"))
                                drpTime.Items.Add(New ListItem(Localization.GetString("ThreeDays", Me.LocalResourceFile), "ThreeDays"))
                                drpTime.Items.Add(New ListItem(Localization.GetString("SevenDays", Me.LocalResourceFile), "SevenDays"))
                                drpTime.Items.Add(New ListItem(Localization.GetString("ThirtyDays", Me.LocalResourceFile), "ThirtyDays"))
                                drpTime.Items.Add(New ListItem(Localization.GetString("NinetyDays", Me.LocalResourceFile), "NinetyDays"))
                                drpTime.Items.Add(New ListItem(Localization.GetString("ThisYear", Me.LocalResourceFile), "ThisYear"))
                                drpTime.Items.Add(New ListItem(Localization.GetString("AllTime", Me.LocalResourceFile), "AllTime"))
                                drpTime.AutoPostBack = True

                                If (Time <> "") Then
                                    If (drpTime.Items.FindByValue(Time) IsNot Nothing) Then
                                        drpTime.SelectedValue = Time
                                    End If
                                Else
                                    If (drpTime.Items.FindByValue(timeItem) IsNot Nothing) Then
                                        Dim params As New List(Of String)
                                        If (Request("lasort-" & TabModuleId.ToString()) <> "") Then
                                            params.Add("lasort-" & TabModuleId.ToString() & "=" & Request("lasort-" & TabModuleId.ToString()))
                                        End If
                                        If (Request("laauth-" & TabModuleId.ToString()) <> "") Then
                                            params.Add("laauth-" & TabModuleId.ToString() & "=" & Request("laauth-" & TabModuleId.ToString()))
                                        End If
                                        If (Request("lacat-" & TabModuleId.ToString()) <> "") Then
                                            params.Add("lacat-" & TabModuleId.ToString() & "=" & Request("lacat-" & TabModuleId.ToString()))
                                        End If

                                        params.Add("latime-" & TabModuleId.ToString() & "=" & timeItem)
                                        Response.Redirect(NavigateURL(Me.TabId, "", params.ToArray()), True)
                                    Else
                                        drpTime.SelectedValue = "AllTime"
                                    End If
                                End If

                                Dim objHandler As New System.EventHandler(AddressOf drpTime_SelectedIndexChanged)
                                AddHandler drpTime.SelectedIndexChanged, objHandler
                                controls.Add(drpTime)
                                Exit Select
                            End If

                            Dim objLiteralOther As New Literal
                            objLiteralOther.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                            objLiteralOther.Text = "[" & layoutArray(iPtr + 1) & "]"
                            objLiteralOther.EnableViewState = False
                            controls.Add(objLiteralOther)

                    End Select
                End If
            Next

        End Sub

        Private Sub ProcessBody(ByRef controls As ControlCollection, ByVal objArticle As ArticleInfo, ByVal layoutArray As String())

            If (ArticleTabInfo Is Nothing) Then
                Return
            End If

            _objLayoutController.ProcessArticleItem(controls, _objLayoutItem.Tokens, objArticle)

        End Sub

        Private Sub ReadQueryString()

            If (Request.QueryString("lapg-" & Me.TabModuleId.ToString()) <> "") Then
                _pageNumber = Convert.ToInt32(Request.QueryString("lapg-" & Me.TabModuleId.ToString()))
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                ReadQueryString()

                If (FindSettings()) Then
                    BindArticles()
                Else
                    lblNotConfigured.Visible = True
                    rptLatestArticles.Visible = False
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                If (_articleTabID <> Null.NullInteger) Then
                    If (Settings.Contains(ArticleConstants.LATEST_ARTICLES_INCLUDE_STYLESHEET)) Then
                        If (Convert.ToBoolean(Settings(ArticleConstants.LATEST_ARTICLES_INCLUDE_STYLESHEET).ToString())) Then

                            Dim objCSS As Control = Me.BasePage.FindControl("CSS")

                            If Not (objCSS Is Nothing) Then
                                Dim objLink As New HtmlLink()
                                objLink.ID = "Template_" & Me.ModuleId.ToString()
                                objLink.Attributes("rel") = "stylesheet"
                                objLink.Attributes("type") = "text/css"
                                objLink.Href = Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Templates/" & ArticleSettings.Template & "/Template.css")

                                objCSS.Controls.AddAt(0, objLink)
                            End If

                        End If
                    End If
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub rptLatestArticles_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptLatestArticles.ItemDataBound

            Try

                If (e.Item.ItemType = ListItemType.Header) Then
                    ProcessHeaderFooter(e.Item.Controls, _objLayoutHeader.Tokens())
                End If

                If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                    If (e.Item.ItemIndex >= Me.StartPoint) Then
                        ProcessBody(e.Item.Controls, CType(e.Item.DataItem, ArticleInfo), _objLayoutItem.Tokens)
                    End If
                End If

                If (e.Item.ItemType = ListItemType.Footer) Then
                    ProcessHeaderFooter(e.Item.Controls, _objLayoutFooter.Tokens())
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub dlLatestArticles_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlLatestArticles.ItemDataBound

            Try

                If (e.Item.ItemType = ListItemType.Header) Then
                    ProcessHeaderFooter(e.Item.Controls, _objLayoutHeader.Tokens())
                End If

                If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                    If (e.Item.ItemIndex >= Me.StartPoint) Then
                        ProcessBody(e.Item.Controls, CType(e.Item.DataItem, ArticleInfo), _objLayoutItem.Tokens)
                    End If
                End If

                If (e.Item.ItemType = ListItemType.Footer) Then
                    ProcessHeaderFooter(e.Item.Controls, _objLayoutFooter.Tokens())
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpSort_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Dim params As New List(Of String)

            Dim drpSort As DropDownList = CType(sender, DropDownList)

            If (drpSort IsNot Nothing) Then
                params.Add("lasort-" & TabModuleId.ToString() & "=" & drpSort.SelectedValue)
                If (Request("laauth-" & TabModuleId.ToString()) <> "") Then
                    params.Add("laauth-" & TabModuleId.ToString() & "=" & Request("laauth-" & TabModuleId.ToString()))
                End If
                If (Request("lacat-" & TabModuleId.ToString()) <> "") Then
                    params.Add("lacat-" & TabModuleId.ToString() & "=" & Request("lacat-" & TabModuleId.ToString()))
                End If
                If (Request("latime-" & TabModuleId.ToString()) <> "") Then
                    params.Add("latime-" & TabModuleId.ToString() & "=" & Request("latime-" & TabModuleId.ToString()))
                End If
                Response.Redirect(NavigateURL(Me.TabId, "", params.ToArray()), True)
            End If

        End Sub

        Private Sub drpAuthor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Dim params As New List(Of String)
            If (Request("lasort-" & TabModuleId.ToString()) <> "") Then
                params.Add("lasort-" & TabModuleId.ToString() & "=" & Request("lasort-" & TabModuleId.ToString()))
            End If

            Dim drpAuthor As DropDownList = CType(sender, DropDownList)

            If (drpAuthor IsNot Nothing) Then
                If (drpAuthor.SelectedValue <> "-1") Then
                    params.Add("laauth-" & TabModuleId.ToString() & "=" & drpAuthor.SelectedValue)
                    If (Request("lacat-" & TabModuleId.ToString()) <> "") Then
                        params.Add("lacat-" & TabModuleId.ToString() & "=" & Request("lacat-" & TabModuleId.ToString()))
                    End If
                    If (Request("lacust-" & TabModuleId.ToString()) <> "") Then
                        params.Add("lacust-" & TabModuleId.ToString() & "=" & Request("lacust-" & TabModuleId.ToString()))
                    End If
                    If (Request("latime-" & TabModuleId.ToString()) <> "") Then
                        params.Add("latime-" & TabModuleId.ToString() & "=" & Request("latime-" & TabModuleId.ToString()))
                    End If
                    Response.Redirect(NavigateURL(Me.TabId, "", params.ToArray()), True)
                Else
                    If (Request("lacat-" & TabModuleId.ToString()) <> "") Then
                        params.Add("lacat-" & TabModuleId.ToString() & "=" & Request("lacat-" & TabModuleId.ToString()))
                    End If
                    If (Request("lacust-" & TabModuleId.ToString()) <> "") Then
                        params.Add("lacust-" & TabModuleId.ToString() & "=" & Request("lacust-" & TabModuleId.ToString()))
                    End If
                    If (Request("latime-" & TabModuleId.ToString()) <> "") Then
                        params.Add("latime-" & TabModuleId.ToString() & "=" & Request("latime-" & TabModuleId.ToString()))
                    End If
                    Response.Redirect(NavigateURL(Me.TabId, "", params.ToArray()), True)
                End If
            End If

        End Sub

        Private Sub drpCategory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Dim params As New List(Of String)
            If (Request("lasort-" & TabModuleId.ToString()) <> "") Then
                params.Add("lasort-" & TabModuleId.ToString() & "=" & Request("lasort-" & TabModuleId.ToString()))
            End If

            Dim drpCategory As DropDownList = CType(sender, DropDownList)

            If (drpCategory IsNot Nothing) Then
                If (drpCategory.SelectedValue <> "-1") Then
                    If (Request("laauth-" & TabModuleId.ToString()) <> "") Then
                        params.Add("laauth-" & TabModuleId.ToString() & "=" & Request("laauth-" & TabModuleId.ToString()))
                    End If
                    params.Add("lacat-" & TabModuleId.ToString() & "=" & drpCategory.SelectedValue)
                    If (Request("lacust-" & TabModuleId.ToString()) <> "") Then
                        params.Add("lacust-" & TabModuleId.ToString() & "=" & Request("lacust-" & TabModuleId.ToString()))
                    End If
                    If (Request("latime-" & TabModuleId.ToString()) <> "") Then
                        params.Add("latime-" & TabModuleId.ToString() & "=" & Request("latime-" & TabModuleId.ToString()))
                    End If
                    Response.Redirect(NavigateURL(Me.TabId, "", params.ToArray()), True)
                Else
                    If (Request("laauth-" & TabModuleId.ToString()) <> "") Then
                        params.Add("laauth-" & TabModuleId.ToString() & "=" & Request("laauth-" & TabModuleId.ToString()))
                    End If
                    If (Request("lacust-" & TabModuleId.ToString()) <> "") Then
                        params.Add("lacust-" & TabModuleId.ToString() & "=" & Request("lacust-" & TabModuleId.ToString()))
                    End If
                    If (Request("latime-" & TabModuleId.ToString()) <> "") Then
                        params.Add("latime-" & TabModuleId.ToString() & "=" & Request("latime-" & TabModuleId.ToString()))
                    End If
                    Response.Redirect(NavigateURL(Me.TabId, "", params.ToArray()), True)
                End If
            End If

        End Sub

        Private Sub drpCustom_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Dim params As New List(Of String)
            If (Request("lasort-" & TabModuleId.ToString()) <> "") Then
                params.Add("lasort-" & TabModuleId.ToString() & "=" & Request("lasort-" & TabModuleId.ToString()))
            End If

            Dim drpCustom As DropDownList = CType(sender, DropDownList)

            If (drpCustom IsNot Nothing) Then
                If (drpCustom.SelectedValue <> "-1") Then
                    params.Add("lacust-" & TabModuleId.ToString() & "=" & drpCustom.Attributes("CustomFieldID") & "-" & drpCustom.SelectedValue)
                    If (Request("laauth-" & TabModuleId.ToString()) <> "") Then
                        params.Add("laauth-" & TabModuleId.ToString() & "=" & Request("laauth-" & TabModuleId.ToString()))
                    End If
                    If (Request("lacat-" & TabModuleId.ToString()) <> "") Then
                        params.Add("lacat-" & TabModuleId.ToString() & "=" & Request("lacat-" & TabModuleId.ToString()))
                    End If
                    If (Request("latime-" & TabModuleId.ToString()) <> "") Then
                        params.Add("latime-" & TabModuleId.ToString() & "=" & Request("latime-" & TabModuleId.ToString()))
                    End If
                    Response.Redirect(NavigateURL(Me.TabId, "", params.ToArray()), True)
                Else
                    If (Request("laauth-" & TabModuleId.ToString()) <> "") Then
                        params.Add("laauth-" & TabModuleId.ToString() & "=" & Request("laauth-" & TabModuleId.ToString()))
                    End If
                    If (Request("lacat-" & TabModuleId.ToString()) <> "") Then
                        params.Add("lacat-" & TabModuleId.ToString() & "=" & Request("lacat-" & TabModuleId.ToString()))
                    End If
                    If (Request("latime-" & TabModuleId.ToString()) <> "") Then
                        params.Add("latime-" & TabModuleId.ToString() & "=" & Request("latime-" & TabModuleId.ToString()))
                    End If
                    Response.Redirect(NavigateURL(Me.TabId, "", params.ToArray()), True)
                End If
            End If

        End Sub

        Private Sub drpTime_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Dim params As New List(Of String)
            If (Request("lasort-" & TabModuleId.ToString()) <> "") Then
                params.Add("lasort-" & TabModuleId.ToString() & "=" & Request("lasort-" & TabModuleId.ToString()))
            End If
            If (Request("laauth-" & TabModuleId.ToString()) <> "") Then
                params.Add("laauth-" & TabModuleId.ToString() & "=" & Request("laauth-" & TabModuleId.ToString()))
            End If
            If (Request("lacat-" & TabModuleId.ToString()) <> "") Then
                params.Add("lacat-" & TabModuleId.ToString() & "=" & Request("lacat-" & TabModuleId.ToString()))
            End If
            If (Request("lacust-" & TabModuleId.ToString()) <> "") Then
                params.Add("lacust-" & TabModuleId.ToString() & "=" & Request("lacust-" & TabModuleId.ToString()))
            End If

            Dim drpTime As DropDownList = CType(sender, DropDownList)

            If (drpTime IsNot Nothing) Then
                params.Add("latime-" & TabModuleId.ToString() & "=" & drpTime.SelectedValue)
                Response.Redirect(NavigateURL(Me.TabId, "", params.ToArray()), True)
            End If

        End Sub

#End Region

    End Class

End Namespace
