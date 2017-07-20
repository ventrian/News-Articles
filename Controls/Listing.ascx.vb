'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.UI.WebControls
Imports Ventrian.NewsArticles.Components.CustomFields
Imports DotNetNuke.Security

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles.Controls

    Partial Public Class Listing
        Inherits System.Web.UI.UserControl

#Region " Private Members "

        Private _objLayoutController As LayoutController

        Private _objLayoutHeader As LayoutInfo
        Private _objLayoutItem As LayoutInfo
        Private _objLayoutFeatured As LayoutInfo
        Private _objLayoutFooter As LayoutInfo
        Private _objLayoutEmpty As LayoutInfo

        Private _articleList As List(Of ArticleInfo)
        Private _articleCount As Integer

        Private _agedDate As DateTime
        Private _author As Integer
        Private _bindArticles As Boolean
        Private _featuredOnly As Boolean
        Private _filterCategories As Integer()
        Private _includeCategory As Boolean
        Private _matchCategories As MatchOperatorType
        Private _maxArticles As Integer
        Private _month As Integer
        Private _notFeaturedOnly As Boolean
        Private _notSecuredOnly As Boolean
        Private _searchText As String
        Private _securedOnly As Boolean
        Private _showExpired As Boolean
        Private _showMessage As Boolean
        Private _showPending As Boolean
        Private _sortBy As String
        Private _sortDirection As String
        Private _startDate As DateTime
        Private _tag As String
        Private _year As Integer

        Public IsIndexed As Boolean = True

        Private _customFieldID As Integer = Null.NullInteger
        Private _customValue As String = Null.NullString

#End Region

#Region " Private Properties "

        Private ReadOnly Property ArticleModuleBase() As NewsArticleModuleBase
            Get
                Return CType(Parent, NewsArticleModuleBase)
            End Get
        End Property

        Private ReadOnly Property ArticleSettings() As ArticleSettings
            Get
                Return ArticleModuleBase.ArticleSettings
            End Get
        End Property

        Private ReadOnly Property CurrentPage() As Integer
            Get
                If (Request("Page") = Null.NullString And Request("CurrentPage") = Null.NullString) Then
                    Return 1
                Else
                    IsIndexed = False
                    Try
                        If (Request("Page") <> Null.NullString) Then
                            Return Convert.ToInt32(Request("Page"))
                        Else
                            Return Convert.ToInt32(Request("CurrentPage"))
                        End If
                    Catch
                        Return 1
                    End Try
                End If
            End Get
        End Property

#End Region

#Region " Public Properties "

        Public Property AgedDate() As DateTime
            Get
                Return _agedDate
            End Get
            Set(ByVal Value As DateTime)
                _agedDate = Value
            End Set
        End Property

        Public Property Author() As Integer
            Get
                Return _author
            End Get
            Set(ByVal Value As Integer)
                _author = Value
            End Set
        End Property

        Public Property BindArticles() As Boolean
            Get
                Return _bindArticles
            End Get
            Set(ByVal Value As Boolean)
                _bindArticles = Value
            End Set
        End Property

        Private ReadOnly Property DynamicAuthorID() As Integer
            Get
                Dim id As Integer = Null.NullInteger
                If (Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                    If (IsNumeric(Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()))) Then
                        id = Convert.ToInt32(Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                End If

                Return id
            End Get
        End Property

        Private ReadOnly Property DynamicAZ() As String
            Get
                Dim id As String = Null.NullString
                If (Request("naaz-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                    id = Request("naaz-" & ArticleModuleBase.TabModuleId.ToString())
                End If
                Return id
            End Get
        End Property

        Private ReadOnly Property DynamicCategoryID() As Integer
            Get
                Dim id As Integer = Null.NullInteger
                If (Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                    If (IsNumeric(Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()))) Then
                        id = Convert.ToInt32(Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                End If

                Return id
            End Get
        End Property

        Private ReadOnly Property DynamicSortBy() As String
            Get
                Dim sort As String = ""

                If (Request("nasort-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                    Select Case Request("nasort-" & ArticleModuleBase.TabModuleId.ToString()).ToLower()
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

        Private ReadOnly Property DynamicTime() As String
            Get
                Dim val As String = ""

                If (Request("natime-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                    val = Request("natime-" & ArticleModuleBase.TabModuleId.ToString())
                End If

                Return val
            End Get
        End Property

        Public Property FeaturedOnly() As Boolean
            Get
                Return _featuredOnly
            End Get
            Set(ByVal Value As Boolean)
                _featuredOnly = Value
            End Set
        End Property

        Public Property FilterCategories() As Integer()
            Get
                Return _filterCategories
            End Get
            Set(ByVal Value As Integer())
                _filterCategories = Value
            End Set
        End Property

        Public Property IncludeCategory() As Boolean
            Get
                Return _includeCategory
            End Get
            Set(ByVal Value As Boolean)
                _includeCategory = Value
            End Set
        End Property

        Public Property MatchCategories() As MatchOperatorType
            Get
                Return _matchCategories
            End Get
            Set(ByVal Value As MatchOperatorType)
                _matchCategories = Value
            End Set
        End Property

        Public Property MaxArticles() As Integer
            Get
                Return _maxArticles
            End Get
            Set(ByVal Value As Integer)
                _maxArticles = Value
            End Set
        End Property

        Public Property Month() As Integer
            Get
                Return _month
            End Get
            Set(ByVal Value As Integer)
                _month = Value
            End Set
        End Property

        Public Property NotFeaturedOnly() As Boolean
            Get
                Return _notFeaturedOnly
            End Get
            Set(ByVal Value As Boolean)
                _notFeaturedOnly = Value
            End Set
        End Property

        Public Property NotSecuredOnly() As Boolean
            Get
                Return _notSecuredOnly
            End Get
            Set(ByVal Value As Boolean)
                _notSecuredOnly = Value
            End Set
        End Property

        Public Property SearchText() As String
            Get
                Return _searchText
            End Get
            Set(ByVal Value As String)
                _searchText = Value
            End Set
        End Property

        Public Property SecuredOnly() As Boolean
            Get
                Return _securedOnly
            End Get
            Set(ByVal Value As Boolean)
                _securedOnly = Value
            End Set
        End Property

        Public Property ShowExpired() As Boolean
            Get
                Return _showExpired
            End Get
            Set(ByVal Value As Boolean)
                _showExpired = Value
            End Set
        End Property

        Public Property ShowMessage() As Boolean
            Get
                Return _showMessage
            End Get
            Set(ByVal Value As Boolean)
                _showMessage = Value
            End Set
        End Property

        Public Property ShowPending() As Boolean
            Get
                Return _showPending
            End Get
            Set(ByVal Value As Boolean)
                _showPending = Value
            End Set
        End Property

        Public Property SortBy() As String
            Get
                Return _sortBy
            End Get
            Set(ByVal Value As String)
                _sortBy = Value
            End Set
        End Property

        Public Property SortDirection() As String
            Get
                Return _sortDirection
            End Get
            Set(ByVal Value As String)
                _sortDirection = Value
            End Set
        End Property

        Public Property StartDate() As DateTime
            Get
                Return _startDate
            End Get
            Set(ByVal Value As DateTime)
                _startDate = Value
            End Set
        End Property

        Public Property Tag() As String
            Get
                Return _tag
            End Get
            Set(ByVal Value As String)
                _tag = Value
            End Set
        End Property

        Public Property Year() As Integer
            Get
                Return _year
            End Get
            Set(ByVal Value As Integer)
                _year = Value
            End Set
        End Property

#End Region

#Region " Private Methods "

        Public Sub BindListing()

            InitializeTemplate()

            If (_year <> Null.NullInteger AndAlso _month <> Null.NullInteger) Then
                _agedDate = New DateTime(_year, _month, 1)
                StartDate = AgedDate.AddMonths(1).AddSeconds(-1)
            End If

            If (_year <> Null.NullInteger AndAlso _month = Null.NullInteger) Then
                _agedDate = New DateTime(_year, 1, 1)
                StartDate = AgedDate.AddYears(1).AddSeconds(-1)
            End If

            Dim objTags() As Integer = Nothing
            If (_tag <> Null.NullString) Then
                Dim objTagController As New TagController()
                Dim objTag As TagInfo = objTagController.Get(ArticleModuleBase.ModuleId, _tag.ToLower())
                If (objTag IsNot Nothing) Then
                    Dim tags As New List(Of Integer)
                    tags.Add(objTag.TagID)
                    objTags = tags.ToArray()
                End If
            End If

            If (FilterCategories IsNot Nothing AndAlso FilterCategories.Length = 1) Then

                Dim objCategoryController As New CategoryController
                Dim objCategory As CategoryInfo = objCategoryController.GetCategory(FilterCategories(0), ArticleModuleBase.ModuleId)

                If Not (objCategory Is Nothing) Then

                    If (objCategory.InheritSecurity = False) Then
                        If (ArticleModuleBase.Settings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                            If (PortalSecurity.IsInRoles(ArticleModuleBase.Settings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString()) = False) Then
                                Response.Redirect(NavigateURL(ArticleModuleBase.TabId), True)
                            End If
                        End If
                    End If

                    Dim objArticleController As New ArticleController
                    _articleList = objArticleController.GetArticleList(ArticleModuleBase.ModuleId, StartDate, _agedDate, FilterCategories, (MatchCategories = MatchOperatorType.MatchAll), Nothing, MaxArticles, CurrentPage, ArticleSettings.PageSize, SortBy, SortDirection, True, False, SearchText.Replace("'", "''"), Author, ShowPending, ShowExpired, FeaturedOnly, NotFeaturedOnly, SecuredOnly, NotSecuredOnly, Null.NullString, objTags, False, Null.NullString, _customFieldID, _customValue, Null.NullString, _articleCount)

                End If
            Else
                Dim objCategoryController As New CategoryController
                Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(ArticleModuleBase.ModuleId, Null.NullInteger)

                Dim excludeCategoriesRestrictive As New List(Of Integer)

                For Each objCategory As CategoryInfo In objCategories
                    If (objCategory.InheritSecurity = False And objCategory.CategorySecurityType = CategorySecurityType.Restrict) Then
                        If (Request.IsAuthenticated) Then
                            If (ArticleModuleBase.Settings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                If (PortalSecurity.IsInRoles(ArticleModuleBase.Settings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString()) = False) Then
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
                            If (ArticleModuleBase.Settings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                If (PortalSecurity.IsInRoles(ArticleModuleBase.Settings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString()) = False) Then
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

                        Dim includeCategorySecurity As Boolean = True

                        For Each exclCategory As Integer In excludeCategories
                            If (exclCategory = objCategoryToInclude.CategoryID) Then
                                includeCategorySecurity = False
                            End If
                        Next


                        If (FilterCategories IsNot Nothing) Then
                            If (FilterCategories.Length > 0) Then
                                Dim filter As Boolean = False
                                For Each cat As Integer In FilterCategories
                                    If (cat = objCategoryToInclude.CategoryID) Then
                                        filter = True
                                    End If
                                Next
                                If (filter = False) Then
                                    includeCategorySecurity = False
                                End If
                            End If
                        End If

                        If (includeCategorySecurity) Then
                            includeCategories.Add(objCategoryToInclude.CategoryID)
                        End If

                    Next

                    If (includeCategories.Count > 0) Then
                        includeCategories.Add(-1)
                    End If

                    FilterCategories = includeCategories.ToArray()

                End If

                Dim objArticleController As New ArticleController
                _articleList = objArticleController.GetArticleList(ArticleModuleBase.ModuleId, StartDate, _agedDate, FilterCategories, (MatchCategories = MatchOperatorType.MatchAll), excludeCategoriesRestrictive.ToArray(), MaxArticles, CurrentPage, ArticleSettings.PageSize, SortBy, SortDirection, True, False, SearchText.Replace("'", "''"), Author, ShowPending, ShowExpired, FeaturedOnly, NotFeaturedOnly, SecuredOnly, NotSecuredOnly, Null.NullString, objTags, False, Null.NullString, _customFieldID, _customValue, Null.NullString, _articleCount)
            End If


            If (_articleList.Count = 0) Then
                If (ShowMessage) Then
                    ProcessHeader(phNoArticles.Controls, _objLayoutEmpty.Tokens)
                End If
            Else
                rptListing.DataSource = _articleList
                rptListing.DataBind()
            End If

        End Sub

        Private Function GetParams(ByVal addDynamicFields As Boolean) As String

            Dim params As String = ""

            If (Request("ctl") <> "") Then
                If (Request("ctl").ToLower = "categoryview" OrElse Request("ctl").ToLower = "authorview" OrElse Request("ctl").ToLower = "archiveview" OrElse Request("ctl").ToLower() = "search") Then
                    params += "ctl=" & Request("ctl") & "&mid=" & ArticleModuleBase.ModuleId.ToString()
                End If
            End If

            If (Request("articleType") <> "") Then
                If (Request("articleType").ToString().ToLower = "categoryview" OrElse Request("articleType").ToString().ToLower() = "authorview" OrElse Request("articleType").ToString().ToLower() = "archiveview" OrElse Request("articleType").ToString().ToLower() = "search" OrElse Request("articleType").ToString().ToLower() = "myarticles" OrElse Request("articleType").ToString().ToLower() = "tagview") Then
                    params += "articleType=" & Request("articleType")
                End If
            End If

            If (FilterCategories IsNot Nothing AndAlso FilterCategories.Length > 0) Then
                If (FilterCategories IsNot ArticleSettings.FilterCategories) Then
                    params += "&CategoryID=" & FilterCategories(0)
                End If
            End If

            Dim authorSet As Boolean = False
            If (ArticleSettings.AuthorUserIDFilter) Then
                If (ArticleSettings.AuthorUserIDParam <> "") Then
                    If (HttpContext.Current.Request(ArticleSettings.AuthorUserIDParam) <> "") Then
                        params += "&" & ArticleSettings.AuthorUserIDParam & "=" & HttpContext.Current.Request(ArticleSettings.AuthorUserIDParam)
                        authorSet = True
                    End If
                End If
            End If

            If (ArticleSettings.AuthorUsernameFilter) Then
                If (ArticleSettings.AuthorUsernameParam <> "") Then
                    If (HttpContext.Current.Request(ArticleSettings.AuthorUsernameParam) <> "") Then
                        params += "&" & ArticleSettings.AuthorUsernameParam & "=" & HttpContext.Current.Request(ArticleSettings.AuthorUsernameParam)
                        authorSet = True
                    End If
                End If
            End If

            If (authorSet = False) Then
                If (Author <> ArticleSettings.Author) Then
                    params += "&AuthorID=" & Author.ToString()
                End If
            End If

            If (Year <> Null.NullInteger) Then
                params += "&Year=" & Year.ToString()
            End If

            If (Month <> Null.NullInteger) Then
                params += "&Month=" & Month.ToString()
            End If

            If (Tag <> Null.NullString) Then
                params += "&Tag=" & Server.UrlEncode(Tag)
            End If

            If (SearchText <> Null.NullString) Then
                If (Request("naaz-" & ArticleModuleBase.TabModuleId.ToString()) = "") Then
                    params += "&Search=" & ArticleModuleBase.Server.UrlEncode(SearchText)
                End If
            End If

            If (addDynamicFields) Then
                If (Request("nasort-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                    params += "&nasort-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nasort-" & ArticleModuleBase.TabModuleId.ToString())
                End If
                If (Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                    params += "&naauth-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("naauth-" & ArticleModuleBase.TabModuleId.ToString())
                End If
                If (Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                    params += "&nacat-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacat-" & ArticleModuleBase.TabModuleId.ToString())
                End If
                If (Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                    params += "&nacust-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacust-" & ArticleModuleBase.TabModuleId.ToString())
                End If
                If (Request("natime-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                    params += "&natime-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("natime-" & ArticleModuleBase.TabModuleId.ToString())
                End If
                If (Request("naaz-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                    params += "&naaz-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("naaz-" & ArticleModuleBase.TabModuleId.ToString())
                End If
            End If

            Return params

        End Function

        Private Sub InitializeTemplate()

            _objLayoutController = New LayoutController(ArticleModuleBase)
            _objLayoutController.IncludeCategory = IncludeCategory

            _objLayoutHeader = LayoutController.GetLayout(ArticleModuleBase, LayoutType.Listing_Header_Html)
            _objLayoutFeatured = LayoutController.GetLayout(ArticleModuleBase, LayoutType.Listing_Featured_Html)
            _objLayoutItem = LayoutController.GetLayout(ArticleModuleBase, LayoutType.Listing_Item_Html)
            _objLayoutFooter = LayoutController.GetLayout(ArticleModuleBase, LayoutType.Listing_Footer_Html)
            _objLayoutEmpty = LayoutController.GetLayout(ArticleModuleBase, LayoutType.Listing_Empty_Html)

            If (_objLayoutFeatured.Template.Trim().Length = 0) Then
                ' Featured Template Empty or does not exist, use standard item.
                _objLayoutFeatured = _objLayoutItem
            End If

        End Sub

        Private Sub InitSettings()

            _author = Null.NullInteger

            If (ArticleSettings.AuthorUserIDFilter) Then
                _author = -100
                If (Request.QueryString(ArticleSettings.AuthorUserIDParam) <> "") Then
                    Try
                        _author = Convert.ToInt32(Request.QueryString(ArticleSettings.AuthorUserIDParam))
                    Catch
                    End Try
                End If
            End If

            If (ArticleSettings.AuthorUsernameFilter) Then
                _author = -100
                If (Request.QueryString(ArticleSettings.AuthorUsernameParam) <> "") Then
                    Try
                        Dim objUser As DotNetNuke.Entities.Users.UserInfo = DotNetNuke.Entities.Users.UserController.GetUserByName(ArticleModuleBase.PortalId, Request.QueryString(ArticleSettings.AuthorUsernameParam))
                        If (objUser IsNot Nothing) Then
                            _author = objUser.UserID
                        End If
                    Catch
                    End Try
                End If
            End If

            If (ArticleSettings.AuthorLoggedInUserFilter) Then
                _author = -100
                If (Request.IsAuthenticated) Then
                    _author = ArticleModuleBase.UserId
                End If
            End If

            If (ArticleSettings.Author <> Null.NullInteger) Then
                _author = ArticleSettings.Author
            End If

            If (DynamicAuthorID <> Null.NullInteger) Then
                _author = DynamicAuthorID
            End If

            _agedDate = Null.NullDate
            _bindArticles = True
            _featuredOnly = ArticleSettings.FeaturedOnly
            If (ArticleSettings.FilterSingleCategory <> Null.NullInteger) Then
                Dim cats As New List(Of Integer)
                cats.Add(ArticleSettings.FilterSingleCategory)
                _filterCategories = cats.ToArray()
            Else
                _filterCategories = ArticleSettings.FilterCategories
            End If
            If (DynamicCategoryID <> Null.NullInteger) Then
                Dim cats As New List(Of Integer)
                cats.Add(DynamicCategoryID)
                _filterCategories = cats.ToArray()
            End If
            _includeCategory = False
            _matchCategories = ArticleSettings.MatchCategories
            _maxArticles = ArticleSettings.MaxArticles
            _month = Null.NullInteger
            _notFeaturedOnly = ArticleSettings.NotFeaturedOnly
            _notSecuredOnly = ArticleSettings.NotSecuredOnly
            _searchText = ""
            If (DynamicAZ <> Null.NullString) Then
                _searchText = DynamicAZ
            End If
            _securedOnly = ArticleSettings.SecuredOnly
            _showExpired = False
            _showMessage = True
            _showPending = ArticleSettings.ShowPending
            _sortBy = ArticleSettings.SortBy
            If (ArticleSettings.BubbleFeatured) Then
                _sortBy = "IsFeatured DESC, " & ArticleSettings.SortBy
            End If
            _sortDirection = ArticleSettings.SortDirection
            _startDate = DateTime.Now.AddMinutes(1)
            _tag = Null.NullString
            _year = Null.NullInteger

            If (DynamicSortBy <> "") Then
                _sortBy = DynamicSortBy
            End If

            If (DynamicTime <> "") Then
                If (DynamicTime.ToLower() = "today") Then
                    _startDate = DateTime.Now
                    _agedDate = DateTime.Today
                End If
                If (DynamicTime.ToLower() = "yesterday") Then
                    _startDate = DateTime.Today
                    _agedDate = DateTime.Today.AddDays(-1)
                End If
                If (DynamicTime.ToLower() = "threedays") Then
                    _startDate = DateTime.Now
                    _agedDate = DateTime.Today.AddDays(-3)
                End If
                If (DynamicTime.ToLower() = "sevendays") Then
                    _startDate = DateTime.Now
                    _agedDate = DateTime.Today.AddDays(-7)
                End If
                If (DynamicTime.ToLower() = "thirtydays") Then
                    _startDate = DateTime.Now
                    _agedDate = DateTime.Today.AddDays(-30)
                End If
                If (DynamicTime.ToLower() = "ninetydays") Then
                    _startDate = DateTime.Now
                    _agedDate = DateTime.Today.AddDays(-90)
                End If
                If (DynamicTime.ToLower() = "thisyear") Then
                    _startDate = DateTime.Now
                    _agedDate = DateTime.Today.AddYears(-1)
                End If
            End If

            If (Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                Dim val As String = Request("nacust-" & ArticleModuleBase.TabModuleId.ToString())
                If (val.Split("-"c).Length = 2) Then
                    If (IsNumeric(val.Split("-"c)(0))) Then
                        _customFieldID = Convert.ToInt32(val.Split("-"c)(0))
                        _customValue = val.Split("-"c)(1)
                    End If
                End If
            End If

        End Sub

        Private Sub ProcessHeader(ByRef objPlaceHolder As ControlCollection, ByVal templateArray As String())

            Dim pageCount As Integer = ((_articleCount - 1) \ ArticleSettings.PageSize) + 1

            For iPtr As Integer = 0 To templateArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(_objLayoutController.ProcessImages(templateArray(iPtr).ToString())))

                If iPtr < templateArray.Length - 1 Then
                    Select Case templateArray(iPtr + 1)

                        Case "AUTHOR"
                            Dim objAuthorController As New AuthorController()
                            Dim drpAuthor As New DropDownList
                            drpAuthor.ID = Globals.CreateValidID("Article-Header-" & iPtr.ToString())
                            drpAuthor.DataTextField = "DisplayName"
                            drpAuthor.DataValueField = "UserID"
                            drpAuthor.DataSource = objAuthorController.GetAuthorList(ArticleModuleBase.ModuleId)
                            drpAuthor.DataBind()
                            drpAuthor.Items.Insert(0, New ListItem(ArticleModuleBase.GetSharedResource("SelectAuthor.Text"), "-1"))
                            drpAuthor.AutoPostBack = True
                            If (DynamicAuthorID <> Null.NullInteger) Then
                                If (drpAuthor.Items.FindByValue(DynamicAuthorID.ToString()) IsNot Nothing) Then
                                    drpAuthor.SelectedValue = DynamicAuthorID.ToString()
                                End If
                            End If
                            Dim objHandler As New System.EventHandler(AddressOf drpAuthor_SelectedIndexChanged)
                            AddHandler drpAuthor.SelectedIndexChanged, objHandler
                            objPlaceHolder.Add(drpAuthor)

                        Case "AZ"

                            Dim list As String = ""
                            For Each c As Char In "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()

                                Dim params As New List(Of String)

                                Dim args As String = GetParams(False)
                                For Each arg As String In args.Split("&"c)
                                    params.Add(arg)
                                Next

                                If (Request("nasort-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                                    params.Add("nasort-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nasort-" & ArticleModuleBase.TabModuleId.ToString()))
                                End If
                                If (Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                                    params.Add("nacust-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()))
                                End If
                                If (Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                                    params.Add("naauth-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()))
                                End If
                                If (Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                                    params.Add("nacat-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()))
                                End If
                                If (Request("natime-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                                    params.Add("natime-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("natime-" & ArticleModuleBase.TabModuleId.ToString()))
                                End If

                                If (list = "") Then
                                    If (Request("naaz-" & ArticleModuleBase.TabModuleId.ToString()) = c) Then
                                        list = c
                                    Else
                                        Dim paramsCopy As List(Of String) = params
                                        paramsCopy.Add("naaz-" & ArticleModuleBase.TabModuleId.ToString() & "=" & c)
                                        list = "<a href=""" & NavigateURL(ArticleModuleBase.TabId, "", paramsCopy.ToArray()) & """>" & c & "</a>"
                                    End If
                                Else
                                    If (Request("naaz-" & ArticleModuleBase.TabModuleId.ToString()) = c) Then
                                        list = list & "&nbsp;" & c
                                    Else
                                        Dim paramsCopy As List(Of String) = params
                                        paramsCopy.Add("naaz-" & ArticleModuleBase.TabModuleId.ToString() & "=" & c)
                                        list = list & "&nbsp;" & "<a href=""" & NavigateURL(ArticleModuleBase.TabId, "", paramsCopy.ToArray()) & """>" & c & "</a>"
                                    End If
                                End If
                            Next
                            If (Request("naaz-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                                list = list & "&nbsp;" & "<a href=""" & NavigateURL(ArticleModuleBase.TabId, "", GetParams(False)) & """>" & "All" & "</a>"
                            Else
                                list = list & "&nbsp;" & "All"
                            End If

                            Dim objLiteral As New Literal()
                            objLiteral.Text = list
                            objPlaceHolder.Add(objLiteral)

                        Case "CATEGORY"
                            Dim objCategoryController As New CategoryController()
                            Dim drpCategory As New DropDownList
                            drpCategory.ID = Globals.CreateValidID("Article-Header-" & iPtr.ToString())
                            drpCategory.DataTextField = "NameIndented"
                            drpCategory.DataValueField = "CategoryID"
                            drpCategory.DataSource = objCategoryController.GetCategoriesAll(ArticleModuleBase.ModuleId, Null.NullInteger, ArticleSettings.CategorySortType)
                            drpCategory.DataBind()
                            drpCategory.Items.Insert(0, New ListItem(ArticleModuleBase.GetSharedResource("SelectCategory.Text"), "-1"))
                            drpCategory.AutoPostBack = True
                            If (DynamicCategoryID <> Null.NullInteger) Then
                                If (drpCategory.Items.FindByValue(DynamicCategoryID.ToString()) IsNot Nothing) Then
                                    drpCategory.SelectedValue = DynamicCategoryID.ToString()
                                End If
                            End If
                            Dim objHandler As New System.EventHandler(AddressOf drpCategory_SelectedIndexChanged)
                            AddHandler drpCategory.SelectedIndexChanged, objHandler
                            objPlaceHolder.Add(drpCategory)

                        Case "CATEGORYFILTER"
                            If (_filterCategories IsNot Nothing) Then
                                Dim categories As String = ""
                                Dim objCategoryController As New CategoryController
                                For Each ID As Integer In _filterCategories
                                    Dim objCategory As CategoryInfo = objCategoryController.GetCategory(ID, ArticleModuleBase.ModuleId)
                                    If (objCategory IsNot Nothing) Then
                                        If (categories = "") Then
                                            categories = objCategory.Name
                                        Else
                                            categories = categories & " | " & objCategory.Name
                                        End If
                                    End If
                                Next
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID("Article-Header-" & iPtr.ToString())
                                objLiteral.Text = categories
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "CATEGORYSELECTED"
                            If (Request("articleType") <> "" AndAlso Request("articleType").ToLower() <> "categoryview") Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/CATEGORYSELECTED") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            Else
                                If (Request("articleType") = "") Then
                                    While (iPtr < templateArray.Length - 1)
                                        If (templateArray(iPtr + 1) = "/CATEGORYSELECTED") Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                            End If

                        Case "/CATEGORYSELECTED"
                            ' Do Nothing

                        Case "CATEGORYNOTSELECTED"
                            If (Request("articleType") <> "" AndAlso Request("articleType").ToLower() = "categoryview") Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/CATEGORYNOTSELECTED") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/CATEGORYNOTSELECTED"
                            ' Do Nothing

                        Case "CATEGORYNOTSELECTED2"
                            If (Request("articleType") <> "" AndAlso Request("articleType").ToLower() = "categoryview") Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/CATEGORYNOTSELECTED") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/CATEGORYNOTSELECTED2"
                            ' Do Nothing

                        Case "CURRENTPAGE"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Article-Header-" & iPtr.ToString())
                            objLiteral.Text = CurrentPage.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "HASMULTIPLEPAGES"
                            If (pageCount = 1) Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/HASMULTIPLEPAGES") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASMULTIPLEPAGES"
                            ' Do Nothing

                        Case "HASNEXTPAGE"

                            If (CurrentPage = pageCount) Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/HASNEXTPAGE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASNEXTPAGE"
                            ' Do Nothing

                        Case "HASPREVPAGE"
                            If (CurrentPage = 1) Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/HASPREVPAGE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASPREVPAGE"
                            ' Do Nothing

                        Case "LINKNEXT"
                            Dim objLink As New HyperLink
                            objLink.ID = Globals.CreateValidID("Article-Header-" & iPtr.ToString())
                            objLink.CssClass = "CommandButton"
                            objLink.Enabled = (CurrentPage < pageCount)
                            objLink.NavigateUrl = NavigateURL(ArticleModuleBase.TabId, "", GetParams(True), "CurrentPage=" & (CurrentPage + 1).ToString())
                            objLink.Text = ArticleModuleBase.GetSharedResource("NextPage.Text")
                            objPlaceHolder.Add(objLink)

                        Case "LINKNEXTURL"
                            If (CurrentPage < pageCount) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID("Article-Header-" & iPtr.ToString())
                                objLiteral.Text = NavigateURL(ArticleModuleBase.TabId, "", GetParams(True), "CurrentPage=" & (CurrentPage + 1).ToString())
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "LINKPREVIOUS"
                            Dim objLink As New HyperLink
                            objLink.ID = Globals.CreateValidID("Article-Header-" & iPtr.ToString())
                            objLink.CssClass = "CommandButton"
                            objLink.Enabled = (CurrentPage > 1)
                            objLink.NavigateUrl = NavigateURL(ArticleModuleBase.TabId, "", GetParams(True), "CurrentPage=" & (CurrentPage - 1).ToString())
                            objLink.Text = ArticleModuleBase.GetSharedResource("PreviousPage.Text")
                            objPlaceHolder.Add(objLink)

                        Case "LINKPREVIOUSURL"
                            If (CurrentPage > 1) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID("Article-Header-" & iPtr.ToString())
                                objLiteral.Text = NavigateURL(ArticleModuleBase.TabId, "", GetParams(True), "CurrentPage=" & (CurrentPage - 1).ToString())
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "PAGECOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Article-Header-" & iPtr.ToString())
                            objLiteral.Text = pageCount.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "PAGER"
                            Dim ctlPagingControl As New PagingControl
                            ctlPagingControl.Visible = True
                            ctlPagingControl.TotalRecords = _articleCount
                            ctlPagingControl.PageSize = ArticleSettings.PageSize
                            ctlPagingControl.CurrentPage = CurrentPage
                            ctlPagingControl.QuerystringParams = GetParams(True)
                            ctlPagingControl.TabID = ArticleModuleBase.TabId
                            ctlPagingControl.EnableViewState = False
                            objPlaceHolder.Add(ctlPagingControl)

                        Case "PAGER2"
                            Dim objLiteral As New Literal
                            If (_articleCount > 0) Then
                                Dim pages As Integer = _articleCount / ArticleSettings.PageSize
                                objLiteral.Text = objLiteral.Text & "<ul>"
                                For i As Integer = 1 To pages
                                    If (CurrentPage = i) Then
                                        objLiteral.Text = objLiteral.Text & "<li>" & i.ToString() & "</li>"
                                    Else
                                        Dim params As String = GetParams(True)
                                        If (i > 1) Then
                                            params += "&currentpage=" & i.ToString()
                                        End If
                                        objLiteral.Text = objLiteral.Text & "<li><a href=""" & NavigateURL(ArticleModuleBase.TabId, "", params) & """>" & i.ToString() & "</a></li>"
                                    End If
                                Next
                                objLiteral.Text = objLiteral.Text & "</ul>"
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "SORT"
                            Dim drpSort As New DropDownList
                            drpSort.ID = Globals.CreateValidID("Article-Header-" & iPtr.ToString())
                            drpSort.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("PublishDate.Text"), "PublishDate"))
                            drpSort.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("ExpiryDate.Text"), "ExpiryDate"))
                            drpSort.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("LastUpdate.Text"), "LastUpdate"))
                            drpSort.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("HighestRated.Text"), "Rating"))
                            drpSort.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("MostCommented.Text"), "CommentCount"))
                            drpSort.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("MostViewed.Text"), "NumberOfViews"))
                            drpSort.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("Random.Text"), "Random"))
                            drpSort.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("SortTitle.Text"), "Title"))
                            drpSort.AutoPostBack = True

                            If (Request("nasort-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                                If (drpSort.Items.FindByValue(Request("nasort-" & ArticleModuleBase.TabModuleId.ToString())) IsNot Nothing) Then
                                    drpSort.SelectedValue = Request("nasort-" & ArticleModuleBase.TabModuleId.ToString())
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
                            objPlaceHolder.Add(drpSort)

                        Case "TABID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Article-Header-" & iPtr.ToString())
                            objLiteral.Text = ArticleModuleBase.TabId.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "TIME"
                            Dim drpTime As New DropDownList
                            drpTime.ID = Globals.CreateValidID("Article-Header-" & iPtr.ToString())

                            drpTime.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("Today.Text"), "Today"))
                            drpTime.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("Yesterday.Text"), "Yesterday"))
                            drpTime.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("ThreeDays.Text"), "ThreeDays"))
                            drpTime.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("SevenDays.Text"), "SevenDays"))
                            drpTime.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("ThirtyDays.Text"), "ThirtyDays"))
                            drpTime.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("NinetyDays.Text"), "NinetyDays"))
                            drpTime.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("ThisYear.Text"), "ThisYear"))
                            drpTime.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("AllTime.Text"), "AllTime"))
                            drpTime.AutoPostBack = True

                            If (DynamicTime <> "") Then
                                If (drpTime.Items.FindByValue(DynamicTime) IsNot Nothing) Then
                                    drpTime.SelectedValue = DynamicTime
                                End If
                            Else
                                drpTime.SelectedValue = "AllTime"
                            End If

                            Dim objHandler As New System.EventHandler(AddressOf drpTime_SelectedIndexChanged)
                            AddHandler drpTime.SelectedIndexChanged, objHandler

                            objPlaceHolder.Add(drpTime)

                        Case Else

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("CUSTOM:")) Then
                                Dim customField As String = templateArray(iPtr + 1).Substring(7, templateArray(iPtr + 1).Length - 7)

                                Dim objCustomFieldController As New CustomFieldController()
                                Dim objCustomFields As ArrayList = objCustomFieldController.List(ArticleModuleBase.ModuleId)

                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.Name.ToLower() = customField.ToLower()) Then
                                        If (objCustomField.FieldType = CustomFieldType.DropDownList) Then
                                            Dim drpCustom As New DropDownList
                                            drpCustom.ID = Globals.CreateValidID("Article-Header-" & iPtr.ToString())

                                            For Each val As String In objCustomField.FieldElements.Split("|"c)
                                                drpCustom.Items.Add(val)
                                            Next

                                            Dim sel As String = ArticleModuleBase.GetSharedResource("SelectCustom.Text")
                                            If (sel.IndexOf("{0}") <> -1) Then
                                                sel = sel.Replace("{0}", objCustomField.Caption)
                                            End If
                                            drpCustom.Items.Insert(0, New ListItem(sel, "-1"))
                                            drpCustom.Attributes.Add("CustomFieldID", objCustomField.CustomFieldID.ToString())
                                            drpCustom.AutoPostBack = True

                                            If (Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                                                Dim val As String = Request("nacust-" & ArticleModuleBase.TabModuleId.ToString())
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
                                            objPlaceHolder.Add(drpCustom)

                                        End If
                                        Exit For
                                    End If
                                Next

                                Exit Select

                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("RESX:")) Then
                                Dim entry As String = templateArray(iPtr + 1).Substring(5, templateArray(iPtr + 1).Length - 5)

                                If (entry <> "") Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Article-Header-" & iPtr.ToString())
                                    objLiteral.Text = ArticleModuleBase.GetSharedResource(entry)
                                    objPlaceHolder.Add(objLiteral)
                                End If

                                Exit Select

                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("SORT:")) Then

                                Dim params As New List(Of String)

                                Dim sortItem As String = templateArray(iPtr + 1).Substring(5, templateArray(iPtr + 1).Length - 5)
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
                                    objLiteral.ID = Globals.CreateValidID("Article-Header-" & iPtr.ToString())
                                    objLiteral.Text = ArticleModuleBase.GetSharedResource(sortItem & ".Text")
                                    objPlaceHolder.Add(objLiteral)
                                Else
                                    Dim objLink As New HyperLink
                                    objLink.ID = Globals.CreateValidID("Article-Header-" & iPtr.ToString())
                                    params.Add("nasort-" & ArticleModuleBase.TabModuleId.ToString() & "=" & sortValue)
                                    If (Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                                        params.Add("naauth-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()))
                                    End If
                                    If (Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                                        params.Add("nacat-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()))
                                    End If
                                    If (Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                                        params.Add("nacust-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()))
                                    End If
                                    If (Request("natime-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                                        params.Add("natime-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("natime-" & ArticleModuleBase.TabModuleId.ToString()))
                                    End If
                                    objLink.NavigateUrl = NavigateURL(ArticleModuleBase.TabId, "", params.ToArray())
                                    objLink.Text = ArticleModuleBase.GetSharedResource(sortItem & ".Text")
                                    objPlaceHolder.Add(objLink)
                                End If
                                Exit Select

                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("TIME:")) Then

                                Dim timeItem As String = templateArray(iPtr + 1).Substring(5, templateArray(iPtr + 1).Length - 5)

                                Dim drpTime As New DropDownList
                                drpTime.ID = Globals.CreateValidID("Article-Header-" & iPtr.ToString())

                                drpTime.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("Today"), "Today"))
                                drpTime.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("Yesterday"), "Yesterday"))
                                drpTime.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("ThreeDays"), "ThreeDays"))
                                drpTime.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("SevenDays"), "SevenDays"))
                                drpTime.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("ThirtyDays"), "ThirtyDays"))
                                drpTime.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("NinetyDays"), "NinetyDays"))
                                drpTime.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("ThisYear"), "ThisYear"))
                                drpTime.Items.Add(New ListItem(ArticleModuleBase.GetSharedResource("AllTime"), "AllTime"))
                                drpTime.AutoPostBack = True

                                If (DynamicTime <> "") Then
                                    If (drpTime.Items.FindByValue(DynamicTime) IsNot Nothing) Then
                                        drpTime.SelectedValue = DynamicTime
                                    End If
                                Else
                                    If (drpTime.Items.FindByValue(timeItem) IsNot Nothing) Then
                                        Dim params As New List(Of String)
                                        If (Request("nasort-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                                            params.Add("nasort-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nasort-" & ArticleModuleBase.TabModuleId.ToString()))
                                        End If
                                        If (Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                                            params.Add("naauth-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()))
                                        End If
                                        If (Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                                            params.Add("nacat-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()))
                                        End If

                                        params.Add("natime-" & ArticleModuleBase.TabModuleId.ToString() & "=" & timeItem)
                                        Response.Redirect(NavigateURL(ArticleModuleBase.TabId, "", params.ToArray()), True)
                                    Else
                                        drpTime.SelectedValue = "AllTime"
                                    End If
                                End If

                                Dim objHandler As New System.EventHandler(AddressOf drpTime_SelectedIndexChanged)
                                AddHandler drpTime.SelectedIndexChanged, objHandler
                                objPlaceHolder.Add(drpTime)
                                Exit Select
                            End If

                            TokenProcessor.ProcessMenuItem(templateArray(iPtr + 1), objPlaceHolder, _objLayoutController, ArticleModuleBase, iPtr, templateArray, MenuOptionType.CurrentArticles)

                    End Select
                End If

            Next

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                InitSettings()

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                If (BindArticles) Then
                    BindListing()
                End If
                ArticleModuleBase.LoadStyleSheet()

                If (IsIndexed = False) Then
                    ' no index but follow links

                    Try
                        'remove the existing MetaRobots entry 
                        Page.Header.Controls.Remove(Page.Header.FindControl("MetaRobots"))

                        'build our own new entry 
                        Dim mymetatag As New System.Web.UI.HtmlControls.HtmlMeta
                        mymetatag.Name = "robots"
                        mymetatag.Content = "NOINDEX, FOLLOW"
                        Page.Header.Controls.Add(mymetatag)
                    Catch ex As Exception
                        'catch an exception if MetaRobots is not present 

                    End Try


                End If

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub rptListing_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptListing.ItemDataBound

            Try

                If (e.Item.ItemType = ListItemType.Header) Then
                    ProcessHeader(e.Item.Controls, _objLayoutHeader.Tokens)
                End If

                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

                    Dim objArticle As ArticleInfo = CType(e.Item.DataItem, ArticleInfo)

                    If (objArticle.IsFeatured) Then
                        _objLayoutController.ProcessArticleItem(e.Item.Controls, _objLayoutFeatured.Tokens, objArticle)
                    Else
                        _objLayoutController.ProcessArticleItem(e.Item.Controls, _objLayoutItem.Tokens, objArticle)
                    End If

                End If

                If (e.Item.ItemType = ListItemType.Footer) Then
                    ProcessHeader(e.Item.Controls, _objLayoutFooter.Tokens)
                End If

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpAuthor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Dim params As New List(Of String)

            Dim args As String = GetParams(False)
            For Each arg As String In args.Split("&"c)
                params.Add(arg)
            Next

            If (Request("nasort-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                params.Add("nasort-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nasort-" & ArticleModuleBase.TabModuleId.ToString()))
            End If

            Dim drpAuthor As DropDownList = CType(sender, DropDownList)

            If (drpAuthor IsNot Nothing) Then
                If (drpAuthor.SelectedValue <> "-1") Then
                    params.Add("naauth-" & ArticleModuleBase.TabModuleId.ToString() & "=" & drpAuthor.SelectedValue)
                    If (Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("nacat-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    If (Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("nacust-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    If (Request("natime-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("natime-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("natime-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    Response.Redirect(NavigateURL(ArticleModuleBase.TabId, "", params.ToArray()), True)
                Else
                    If (Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("nacat-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    If (Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("nacust-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    If (Request("natime-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("natime-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("natime-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    Response.Redirect(NavigateURL(ArticleModuleBase.TabId, "", params.ToArray()), True)
                End If
            End If

        End Sub

        Private Sub drpCategory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Dim params As New List(Of String)

            Dim args As String = GetParams(False)
            For Each arg As String In args.Split("&"c)
                params.Add(arg)
            Next

            If (Request("nasort-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                params.Add("nasort-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nasort-" & ArticleModuleBase.TabModuleId.ToString()))
            End If

            Dim drpCategory As DropDownList = CType(sender, DropDownList)

            If (drpCategory IsNot Nothing) Then
                If (drpCategory.SelectedValue <> "-1") Then
                    If (Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("naauth-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    params.Add("nacat-" & ArticleModuleBase.TabModuleId.ToString() & "=" & drpCategory.SelectedValue)
                    If (Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("nacust-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    If (Request("natime-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("natime-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("natime-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    Response.Redirect(NavigateURL(ArticleModuleBase.TabId, "", params.ToArray()), True)
                Else
                    If (Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("naauth-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    If (Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("nacust-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    If (Request("natime-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("natime-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("natime-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    Response.Redirect(NavigateURL(ArticleModuleBase.TabId, "", params.ToArray()), True)
                End If
            End If

        End Sub

        Private Sub drpCustom_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Dim params As New List(Of String)

            Dim args As String = GetParams(False)
            For Each arg As String In args.Split("&"c)
                params.Add(arg)
            Next

            If (Request("nasort-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                params.Add("nasort-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nasort-" & ArticleModuleBase.TabModuleId.ToString()))
            End If

            Dim drpCustom As DropDownList = CType(sender, DropDownList)

            If (drpCustom IsNot Nothing) Then
                If (drpCustom.SelectedValue <> "-1") Then
                    params.Add("nacust-" & ArticleModuleBase.TabModuleId.ToString() & "=" & drpCustom.Attributes("CustomFieldID") & "-" & drpCustom.SelectedValue)
                    If (Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("naauth-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    If (Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("nacat-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    If (Request("natime-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("natime-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("natime-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    Response.Redirect(NavigateURL(ArticleModuleBase.TabId, "", params.ToArray()), True)
                Else
                    If (Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("naauth-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    If (Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("nacat-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    If (Request("natime-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                        params.Add("natime-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("natime-" & ArticleModuleBase.TabModuleId.ToString()))
                    End If
                    Response.Redirect(NavigateURL(ArticleModuleBase.TabId, "", params.ToArray()), True)
                End If
            End If

        End Sub

        Private Sub drpSort_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Dim params As New List(Of String)

            Dim args As String = GetParams(False)
            For Each arg As String In args.Split("&"c)
                params.Add(arg)
            Next

            Dim drpSort As DropDownList = CType(sender, DropDownList)

            If (drpSort IsNot Nothing) Then
                params.Add("nasort-" & ArticleModuleBase.TabModuleId.ToString() & "=" & drpSort.SelectedValue)
                If (Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                    params.Add("naauth-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()))
                End If
                If (Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                    params.Add("nacat-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()))
                End If
                If (Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                    params.Add("nacust-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()))
                End If
                If (Request("natime-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                    params.Add("natime-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("natime-" & ArticleModuleBase.TabModuleId.ToString()))
                End If
                Response.Redirect(NavigateURL(ArticleModuleBase.TabId, "", params.ToArray()), True)
            End If

        End Sub

        Private Sub drpTime_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Dim params As New List(Of String)

            Dim args As String = GetParams(False)
            For Each arg As String In args.Split("&"c)
                params.Add(arg)
            Next

            If (Request("nasort-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                params.Add("nasort-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nasort-" & ArticleModuleBase.TabModuleId.ToString()))
            End If
            If (Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                params.Add("naauth-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("naauth-" & ArticleModuleBase.TabModuleId.ToString()))
            End If
            If (Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                params.Add("nacat-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacat-" & ArticleModuleBase.TabModuleId.ToString()))
            End If
            If (Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()) <> "") Then
                params.Add("nacust-" & ArticleModuleBase.TabModuleId.ToString() & "=" & Request("nacust-" & ArticleModuleBase.TabModuleId.ToString()))
            End If

            Dim drpTime As DropDownList = CType(sender, DropDownList)

            If (drpTime IsNot Nothing) Then
                params.Add("natime-" & ArticleModuleBase.TabModuleId.ToString() & "=" & drpTime.SelectedValue)
                Response.Redirect(NavigateURL(ArticleModuleBase.TabId, "", params.ToArray()), True)
            End If

        End Sub

#End Region

    End Class

End Namespace