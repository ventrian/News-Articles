'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Security

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class Archives
        Inherits NewsArticleModuleBase

#Region " Protected Properties "

        Protected ReadOnly Property AuthorID() As Integer
            Get
                Dim id As Integer = Null.NullInteger
                If (ArticleSettings.AuthorUserIDFilter) Then
                    If (Request.QueryString(ArticleSettings.AuthorUserIDParam) <> "") Then
                        Try
                            id = Convert.ToInt32(Request.QueryString(ArticleSettings.AuthorUserIDParam))
                        Catch
                        End Try
                    End If
                End If

                If (ArticleSettings.AuthorUsernameFilter) Then
                    If (Request.QueryString(ArticleSettings.AuthorUsernameParam) <> "") Then
                        Try
                            Dim objUser As DotNetNuke.Entities.Users.UserInfo = DotNetNuke.Entities.Users.UserController.GetUserByName(Me.PortalId, Request.QueryString(ArticleSettings.AuthorUsernameParam))
                            If (objUser IsNot Nothing) Then
                                id = objUser.UserID
                            End If
                        Catch
                        End Try
                    End If
                End If

                If (ArticleSettings.AuthorLoggedInUserFilter) Then
                    If (Request.IsAuthenticated) Then
                        id = Me.UserId
                    Else
                        id = -100
                    End If
                End If

                If (ArticleSettings.Author <> Null.NullInteger) Then
                    id = ArticleSettings.Author
                End If

                Return id
            End Get
        End Property

        Protected ReadOnly Property AuthorIDRSS() As String
            Get
                If (AuthorID <> Null.NullInteger) Then
                    Return "&amp;AuthorID=" & AuthorID.ToString()
                End If
                Return ""
            End Get
        End Property

#End Region

#Region " Protected Methods "

        Protected Function GetAuthorLink(ByVal authorID As Integer, ByVal username As String) As String

            Return Common.GetAuthorLink(Me.TabId, Me.ModuleId, authorID, username, ArticleSettings.LaunchLinks, ArticleSettings)

        End Function

        Protected Function GetAuthorLinkRss(ByVal authorID As String) As String

            Return Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/RSS.aspx?TabID=" & TabId.ToString() & "&amp;ModuleID=" & ModuleId.ToString() & "&amp;AuthorID=" & authorID)

        End Function

        Protected Function GetCategoryLink(ByVal categoryID As String, ByVal name As String) As String

            Return Common.GetCategoryLink(Me.TabId, Me.ModuleId, categoryID, name, ArticleSettings.LaunchLinks, ArticleSettings)

        End Function

        Protected Function GetCategoryLinkRss(ByVal categoryID As String) As String

            Return Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/RSS.aspx?TabID=" & TabId.ToString() & "&amp;ModuleID=" & ModuleId.ToString() & "&amp;CategoryID=" & categoryID & AuthorIDRSS)

        End Function

        Protected Function GetCurrentLinkRss() As String

            Return Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/RSS.aspx?TabID=" & TabId.ToString() & "&amp;ModuleID=" & ModuleId.ToString() & "&amp;MaxCount=25" & AuthorIDRSS)

        End Function

        Protected Function GetCurrentLink() As String

            Return Common.GetModuleLink(TabId, ModuleId, "", ArticleSettings)

        End Function

        Protected Function GetMonthLink(ByVal month As Integer, ByVal year As Integer) As String

            Return Common.GetModuleLink(Me.TabId, Me.ModuleId, "ArchiveView", ArticleSettings, "month=" & month.ToString(), "year=" & year.ToString())

        End Function

        Protected Function GetMonthLinkRss(ByVal month As Integer, ByVal year As Integer) As String

            Return Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/RSS.aspx?TabID=" & TabId.ToString() & "&amp;ModuleID=" & ModuleId.ToString() & "&amp;Month=" & month.ToString() & "&amp;Year=" & year.ToString() & AuthorIDRSS)

        End Function

        Protected Function GetMonthName(ByVal month As Integer) As String

            Dim dt As New DateTime(2008, month, 1)
            Return dt.ToString("MMMM")

        End Function

        Protected Function GetRssPath() As String

            Return Page.ResolveUrl(ArticleSettings.SyndicationImagePath)

        End Function

        Protected Function IsSyndicationEnabled() As Boolean

            Return ArticleSettings.IsSyndicationEnabled

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                Dim objCategoryController As New CategoryController
                If (ArticleSettings.FilterSingleCategory <> Null.NullInteger) Then
                    Dim categoriesToDisplay(1) As Integer
                    categoriesToDisplay(1) = ArticleSettings.FilterSingleCategory

                    rptCategories.DataSource = objCategoryController.GetCategoriesAll(Me.ModuleId, Null.NullInteger, categoriesToDisplay, AuthorID, Null.NullInteger, ArticleSettings.ShowPending, ArticleSettings.CategorySortType)
                    rptCategories.DataBind()
                Else
                    Dim objCategoriesSelected As New List(Of CategoryInfo)
                    Dim objCategoriesDisplay As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(Me.ModuleId, Null.NullInteger, ArticleSettings.FilterCategories, AuthorID, Null.NullInteger, ArticleSettings.ShowPending, ArticleSettings.CategorySortType)
                    For Each objCategory As CategoryInfo In objCategoriesDisplay
                        If (objCategory.InheritSecurity) Then
                            objCategoriesSelected.Add(objCategory)
                        Else
                            If (Request.IsAuthenticated) Then
                                If (Settings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                    If (PortalSecurity.IsInRoles(Settings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString())) Then
                                        objCategoriesSelected.Add(objCategory)
                                    End If
                                End If
                            End If
                        End If
                    Next

                    rptCategories.DataSource = objCategoriesSelected
                    rptCategories.DataBind()
                End If

                Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(ModuleId, Null.NullInteger)

                Dim excludeCategoriesRestrictive As New List(Of Integer)

                For Each objCategory As CategoryInfo In objCategories
                    If (objCategory.InheritSecurity = False And objCategory.CategorySecurityType = CategorySecurityType.Restrict) Then
                        If (Request.IsAuthenticated) Then
                            If (Settings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                If (PortalSecurity.IsInRoles(Settings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString()) = False) Then
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
                            If (Settings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                If (PortalSecurity.IsInRoles(Settings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString()) = False) Then
                                    excludeCategories.Add(objCategory.CategoryID)
                                End If
                            End If
                        Else
                            excludeCategories.Add(objCategory.CategoryID)
                        End If
                    End If
                Next

                Dim filterCategories As Integer() = ArticleSettings.FilterCategories
                Dim includeCategories As New List(Of Integer)

                If (excludeCategories.Count > 0) Then

                    For Each objCategoryToInclude As CategoryInfo In objCategories

                        Dim includeCategory As Boolean = True

                        For Each exclCategory As Integer In excludeCategories
                            If (exclCategory = objCategoryToInclude.CategoryID) Then
                                includeCategory = False
                            End If
                        Next

                        If (ArticleSettings.FilterCategories IsNot Nothing) Then
                            If (ArticleSettings.FilterCategories.Length > 0) Then
                                Dim filter As Boolean = False
                                For Each cat As Integer In ArticleSettings.FilterCategories
                                    If (cat = objCategoryToInclude.CategoryID) Then
                                        filter = True
                                    End If
                                Next
                                If (filter = False) Then
                                    includeCategory = False
                                End If
                            End If
                        End If

                        If (includeCategory) Then
                            includeCategories.Add(objCategoryToInclude.CategoryID)
                        End If

                    Next

                    If (includeCategories.Count > 0) Then
                        includeCategories.Add(-1)
                    End If

                    filterCategories = includeCategories.ToArray()

                End If


                If (ArticleSettings.FilterSingleCategory <> Null.NullInteger) Then
                    Dim categoriesToDisplay(1) As Integer
                    categoriesToDisplay(1) = ArticleSettings.FilterSingleCategory

                    Dim objAuthorController As New AuthorController
                    rptAuthors.DataSource = objAuthorController.GetAuthorStatistics(Me.ModuleId, categoriesToDisplay, excludeCategoriesRestrictive.ToArray(), AuthorID, "DisplayName", ArticleSettings.ShowPending)
                    rptAuthors.DataBind()
                Else
                    Dim objAuthorController As New AuthorController
                    rptAuthors.DataSource = objAuthorController.GetAuthorStatistics(Me.ModuleId, filterCategories, excludeCategoriesRestrictive.ToArray(), AuthorID, "DisplayName", ArticleSettings.ShowPending)
                    rptAuthors.DataBind()
                End If

                If (ArticleSettings.FilterSingleCategory <> Null.NullInteger) Then
                    Dim categoriesToDisplay(1) As Integer
                    categoriesToDisplay(1) = ArticleSettings.FilterSingleCategory
                    Dim objArticleController As New ArticleController
                    rptMonth.DataSource = objArticleController.GetNewsArchive(Me.ModuleId, categoriesToDisplay, excludeCategoriesRestrictive.ToArray(), AuthorID, GroupByType.Month, ArticleSettings.ShowPending)
                    rptMonth.DataBind()
                Else
                    Dim objArticleController As New ArticleController
                    rptMonth.DataSource = objArticleController.GetNewsArchive(Me.ModuleId, filterCategories, excludeCategoriesRestrictive.ToArray(), AuthorID, GroupByType.Month, ArticleSettings.ShowPending)
                    rptMonth.DataBind()
                End If

                Me.BasePage.Title = "Archives | " & Me.BasePage.Title

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                LoadStyleSheet()

                phCurrentArticles.Visible = ArticleSettings.ArchiveCurrentArticles
                phCategory.Visible = ArticleSettings.ArchiveCategories
                phAuthor.Visible = ArticleSettings.ArchiveAuthor
                phMonth.Visible = ArticleSettings.ArchiveMonth

                phArchives.Visible = IsSyndicationEnabled()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace