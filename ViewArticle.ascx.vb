Imports Ventrian.NewsArticles.Components.Common
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports System.Text
Imports DotNetNuke.Security.Permissions
Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ViewArticle
        Inherits NewsArticleModuleBase

#Region " Private Members "

        Private _articleID As Integer = Null.NullInteger
        Private _pageID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub BindArticle()

            If (_articleID = Null.NullInteger) Then
                Response.Redirect(NavigateURL(), True)
            End If

            Dim objArticleController As New ArticleController
            Dim objArticle As ArticleInfo = objArticleController.GetArticle(_articleID)

            If (objArticle Is Nothing) Then
                ' Article doesn't exist.
                Response.Redirect(NavigateURL(), True)
            End If

            Dim includeCategory As Boolean = False
            If (ArticleSettings.CategoryBreadcrumb And Request("CategoryID") <> "") Then
                includeCategory = True
            End If

            Dim targetUrl As String = ""
            If (_pageID <> Null.NullInteger) Then
                Dim objPageController As New PageController()
                Dim objPages As ArrayList = objPageController.GetPageList(objArticle.ArticleID)

                Dim pageFound As Boolean = False
                For Each objPage As PageInfo In objPages
                    If (objPage.PageID = _pageID) Then
                        pageFound = True
                    End If
                Next
                If (pageFound = False) Then
                    ' redirect
                    Response.Status = "301 Moved Permanently"
                    Response.AddHeader("Location", Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False))
                    Response.End()
                End If

                targetUrl = Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False, "PageID=" & _pageID.ToString())
            Else
                targetUrl = Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False)
            End If

            If (ArticleSettings.UseCanonicalLink) Then
                Dim litCanonical As New Literal
                litCanonical.Text = "<link rel=""canonical"" href=""" & targetUrl & """/>"
                Me.BasePage.Header.Controls.Add(litCanonical)
            End If

            If (objArticle.ModuleID <> Me.ModuleId) Then
                ' Article in the wrong ModuleID
                Response.Redirect(NavigateURL(), True)
            End If

            ' Check Article Security
            If (objArticle.IsSecure) Then
                If (ArticleSettings.IsSecureEnabled = False) Then
                    If (ArticleSettings.SecureUrl <> "") Then
                        Dim url As String = Request.Url.ToString().Replace(AddHTTP(Request.Url.Host), "")
                        If (ArticleSettings.SecureUrl.IndexOf("?") <> -1) Then
                            Response.Redirect((ArticleSettings.SecureUrl & "&returnurl=" & Server.UrlEncode(url)).Replace("[ARTICLEID]", objArticle.ArticleID.ToString()), True)
                        Else
                            Response.Redirect((ArticleSettings.SecureUrl & "?returnurl=" & Server.UrlEncode(url)).Replace("[ARTICLEID]", objArticle.ArticleID.ToString()), True)
                        End If
                    Else
                        Response.Redirect(NavigateURL(Me.TabId), True)
                    End If
                End If
            End If

            ' Is Article Published?
            If (objArticle.Status = StatusType.AwaitingApproval Or objArticle.Status = StatusType.Draft Or (objArticle.StartDate > DateTime.Now And ArticleSettings.ShowPending = False)) Then
                If Not (ArticleSettings.IsAdmin() Or ArticleSettings.IsApprover() Or Me.UserId = objArticle.AuthorID) Then
                    Response.Redirect(NavigateURL(Me.TabId), True)
                End If
            End If

            If (objArticle.IsSecure) Then
                If (ArticleSettings.SecureUrl <> "") Then
                    If (objArticleController.SecureCheck(PortalId, _articleID, UserId) = False And IsEditable = False And UserInfo.IsSuperUser = False And UserInfo.IsInRole("Administrators") = False) Then
                        Dim url As String = Request.Url.ToString().Replace(AddHTTP(Request.Url.Host), "")
                        If (ArticleSettings.SecureUrl.IndexOf("?") <> -1) Then
                            Response.Redirect((ArticleSettings.SecureUrl & "&returnurl=" & Server.UrlEncode(url)).Replace("[ARTICLEID]", objArticle.ArticleID.ToString()), True)
                        Else
                            Response.Redirect((ArticleSettings.SecureUrl & "?returnurl=" & Server.UrlEncode(url)).Replace("[ARTICLEID]", objArticle.ArticleID.ToString()), True)
                        End If
                    End If
                End If
            End If

            ' Permission to view category?
            Dim objCategoryController As New CategoryController
            Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(ModuleId, Null.NullInteger)

            Dim objArticleCategories As ArrayList = objArticleController.GetArticleCategories(objArticle.ArticleID)
            For Each objArticleCategory As CategoryInfo In objArticleCategories
                For Each objCategory As CategoryInfo In objCategories
                    If (objCategory.CategoryID = objArticleCategory.CategoryID) Then
                        If (objCategory.InheritSecurity = False) Then
                            If (Request.IsAuthenticated) Then

                                If (objCategory.CategorySecurityType = CategorySecurityType.Loose) Then
                                    Dim doCheck As Boolean = True
                                    ' Ensure there are no inherit security 
                                    For Each objCategoryOther As CategoryInfo In objArticleCategories
                                        For Each objCategoryOther2 As CategoryInfo In objCategories
                                            If (objCategoryOther.CategoryID = objCategoryOther2.CategoryID) Then
                                                If (objCategoryOther2.InheritSecurity) Then
                                                    doCheck = False
                                                    Exit For
                                                End If
                                            End If
                                        Next
                                    Next

                                    If (doCheck) Then
                                        If (Settings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                            If (PortalSecurity.IsInRoles(Settings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString()) = False) Then
                                                Response.Redirect(NavigateURL(Me.TabId), True)
                                            End If
                                        End If
                                    End If
                                Else
                                    If (Settings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                        If (PortalSecurity.IsInRoles(Settings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString()) = False) Then
                                            Response.Redirect(NavigateURL(Me.TabId), True)
                                        End If
                                    End If
                                End If
                            Else
                                If (objCategory.CategorySecurityType = CategorySecurityType.Loose) Then
                                    Dim doCheck As Boolean = True
                                    ' Ensure there are no inherit security 
                                    For Each objCategoryOther As CategoryInfo In objArticleCategories
                                        For Each objCategoryOther2 As CategoryInfo In objCategories
                                            If (objCategoryOther.CategoryID = objCategoryOther2.CategoryID) Then
                                                If (objCategoryOther2.InheritSecurity) Then
                                                    doCheck = False
                                                    Exit For
                                                End If
                                            End If
                                        Next
                                    Next

                                    If (doCheck) Then
                                        Response.Redirect(NavigateURL(Me.TabId), True)
                                    End If
                                Else
                                    Response.Redirect(NavigateURL(Me.TabId), True)
                                End If
                            End If
                        End If
                    End If
                Next
            Next

            ' Check module security
            Dim objModuleController As New ModuleController
            Dim objModule As ModuleInfo = objModuleController.GetModule(objArticle.ModuleID, Me.TabId)

            If Not (objModule Is Nothing) Then
                If (ModulePermissionController.CanViewModule(objModule) = False) Then
                'If (DotNetNuke.Security.PortalSecurity.IsInRoles(objModule.AuthorizedViewRoles) = False) Then
                    Response.Redirect(NavigateURL(Me.TabId), True)
                End If
            End If

            ' Increment View Count
            Dim cookie As HttpCookie = Request.Cookies("Article" & _articleID.ToString())
            If (cookie Is Nothing) Then

                objArticle.NumberOfViews = objArticle.NumberOfViews + 1
                objArticleController.UpdateArticleCount(objArticle.ArticleID, objArticle.NumberOfViews)

                cookie = New HttpCookie("Article" & _articleID.ToString())
                cookie.Value = "1"
                cookie.Expires = DateTime.Now.AddMinutes(20)
                Context.Response.Cookies.Add(cookie)

            End If

            Dim objLayoutController As New LayoutController(Me)
            If (ArticleSettings.CategoryBreadcrumb & Request("CategoryID") <> "") Then
                objLayoutController.IncludeCategory = True
            End If

            Dim objLayoutItem As LayoutInfo = LayoutController.GetLayout(Me, LayoutType.View_Item_Html)
            objLayoutController.ProcessArticleItem(phArticle.Controls, objLayoutItem.Tokens, objArticle)

            If (objArticle.MetaTitle <> "") Then
                Me.BasePage.Title = objArticle.MetaTitle
            Else
                Dim objLayoutTitle As LayoutInfo = LayoutController.GetLayout(Me, LayoutType.View_Title_Html)
                If (objLayoutTitle.Template <> "") Then
                    Dim phPageTitle As New PlaceHolder()
                    objLayoutController.ProcessArticleItem(phPageTitle.Controls, objLayoutTitle.Tokens, objArticle)
                    Me.BasePage.Title = RenderControlToString(phPageTitle)
                End If
            End If

            If (ArticleSettings.UniquePageTitles) Then
                If (_pageID <> Null.NullInteger) Then
                    Me.BasePage.Title = Me.BasePage.Title & " " & _pageID.ToString()
                End If
            End If

            If (objArticle.MetaDescription <> "") Then
                Me.BasePage.Description = objArticle.MetaDescription
            Else
                Dim objLayoutDescription As LayoutInfo = LayoutController.GetLayout(Me, LayoutType.View_Description_Html)
                If (objLayoutDescription.Template <> "") Then
                    Dim phPageDescription As New PlaceHolder()
                    objLayoutController.ProcessArticleItem(phPageDescription.Controls, objLayoutDescription.Tokens, objArticle)
                    Me.BasePage.Description = RenderControlToString(phPageDescription)
                End If
            End If

            If (objArticle.MetaKeywords <> "") Then
                Me.BasePage.KeyWords = objArticle.MetaKeywords
            Else
                Dim objLayoutKeyword As LayoutInfo = LayoutController.GetLayout(Me, LayoutType.View_Keyword_Html)
                If (objLayoutKeyword.Template <> "") Then
                    Dim phPageKeyword As New PlaceHolder()
                    objLayoutController.ProcessArticleItem(phPageKeyword.Controls, objLayoutKeyword.Tokens, objArticle)
                    Me.BasePage.KeyWords = RenderControlToString(phPageKeyword)
                End If
            End If

            If (objArticle.PageHeadText <> "") Then
                Dim litPageHeadText As New Literal()
                litPageHeadText.Text = objArticle.PageHeadText
                Me.BasePage.Header.Controls.Add(litPageHeadText)
            End If

            Dim objLayoutPageHeader As LayoutInfo = LayoutController.GetLayout(Me, LayoutType.View_PageHeader_Html)
            If (objLayoutPageHeader.Template <> "") Then
                Dim phPageHeaderTitle As New PlaceHolder()
                objLayoutController.ProcessArticleItem(phPageHeaderTitle.Controls, objLayoutPageHeader.Tokens, objArticle)
                Me.BasePage.Header.Controls.Add(phPageHeaderTitle)
            End If


			' set metatags for sharing
			Dim sb As New StringBuilder()
			Dim desc As String = StripHtml(Server.HtmlDecode(objArticle.Summary.Trim()), True)
			If String.IsNullOrEmpty(desc) Then
				Dim pageController As New PageController
				Dim pageList As ArrayList = pageController.GetPageList(objArticle.ArticleID)
				If pageList.Count > 0 Then
					Dim p As PageInfo = pageList(0)
					desc = StripHtml(Server.HtmlDecode(p.PageText), True)
				End If
			End If
			desc = desc.Substring(0, Math.Min(300, desc.Length))

			Dim img As String = ""
			If (objArticle.ImageUrl <> "") Then
				img = FormatImageUrl(objArticle.ImageUrl)
			Else
				If (objArticle.ImageCount > 0) Then
					Dim objImageController As New ImageController
					Dim objImages As List(Of ImageInfo) = objImageController.GetImageList(objArticle.ArticleID, Null.NullString())
					If (objImages.Count > 0) Then
						img = AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & PortalSettings.HomeDirectory & objImages(0).Folder & objImages(0).FileName)
					End If
				End If
			End If

			sb.AppendFormat("<meta property=""og:type"" content=""{0}"" />{1}", "article", System.Environment.NewLine)
			sb.AppendFormat("<meta property=""og:title"" content=""{0}"" />{1}", objArticle.Title, System.Environment.NewLine)
			sb.AppendFormat("<meta property=""og:description"" content=""{0}"" />{1}", desc, System.Environment.NewLine)
			sb.AppendFormat("<meta property=""og:url"" content=""{0}"" />{1}", targetUrl, System.Environment.NewLine)
			If (Not String.IsNullOrEmpty(img)) Then
				sb.AppendFormat("<meta property=""og:image"" content=""{0}"" />{1}", img, System.Environment.NewLine)
			End If
			Me.Parent.Page.Header.Controls.Add(New LiteralControl(sb.ToString()))

		End Sub

        Private Sub ReadQueryString()

            If (ArticleSettings.UrlModeType = Components.Types.UrlModeType.Shorterned) Then
                Try
                    If (IsNumeric(Request(ArticleSettings.ShortenedID))) Then
                        _articleID = Convert.ToInt32(Request(ArticleSettings.ShortenedID))
                    End If
                Catch
                End Try
            End If

            If (IsNumeric(Request("ArticleID"))) Then
                _articleID = Convert.ToInt32(Request("ArticleID"))
            End If

            If (IsNumeric(Request("PageID"))) Then
                _pageID = Convert.ToInt32(Request("PageID"))
            End If

        End Sub

        Private Function RenderControlToString(ByVal ctrl As Control) As String

            Dim sb As New StringBuilder()
            Dim tw As New IO.StringWriter(sb)
            Dim hw As New HtmlTextWriter(tw)

            ctrl.RenderControl(hw)

            Return sb.ToString()

        End Function

#End Region

#Region " Protected Methods "

        Protected Function GetArticleID() As String

            Return _articleID.ToString()

        End Function

        Protected Function GetLocalizedValue(ByVal key As String) As String

            Return Localization.GetString(key, Me.LocalResourceFile)

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Initialization(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try
                ReadQueryString()
                BindArticle()

                If (Request("CategoryID") <> "" AndAlso IsNumeric(Request("CategoryID"))) Then
                    Dim _categoryID As Integer = Convert.ToInt32(Request("CategoryID"))

                    Dim objCategoryController As New CategoryController
                    Dim objCategoriesAll As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(Me.ModuleId, Null.NullInteger, ArticleSettings.CategorySortType)

                    For Each objCategory As CategoryInfo In objCategoriesAll
                        If (objCategory.CategoryID = _categoryID) Then

                            If (ArticleSettings.FilterSingleCategory = objCategory.CategoryID) Then
                                Exit For
                            End If

                            Dim path As String = ""
                            If (ArticleSettings.CategoryBreadcrumb) Then
                                Dim objTab As New DotNetNuke.Entities.Tabs.TabInfo
                                objTab.TabName = objCategory.Name
                                objTab.Url = Common.GetCategoryLink(TabId, ModuleId, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings)
                                PortalSettings.ActiveTab.BreadCrumbs.Add(objTab)

                                Dim parentID As Integer = objCategory.ParentID
                                Dim parentCount As Integer = 0

                                While parentID <> Null.NullInteger
                                    For Each objParentCategory As CategoryInfo In objCategoriesAll
                                        If (objParentCategory.CategoryID = parentID) Then
                                            If (ArticleSettings.FilterSingleCategory = objParentCategory.CategoryID) Then
                                                parentID = Null.NullInteger
                                                Exit For
                                            End If
                                            Dim objParentTab As New DotNetNuke.Entities.Tabs.TabInfo
                                            objParentTab.TabID = 10000 + objParentCategory.CategoryID
                                            objParentTab.TabName = objParentCategory.Name
                                            objParentTab.Url = Common.GetCategoryLink(TabId, ModuleId, objParentCategory.CategoryID.ToString(), objParentCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings)
                                            PortalSettings.ActiveTab.BreadCrumbs.Insert(PortalSettings.ActiveTab.BreadCrumbs.Count - 1 - parentCount, objParentTab)

                                            If (path.Length = 0) Then
                                                path = " > " & objParentCategory.Name
                                            Else
                                                path = " > " & objParentCategory.Name & path
                                            End If

                                            parentCount = parentCount + 1
                                            parentID = objParentCategory.ParentID
                                        End If
                                    Next
                                End While
                            End If

                            If (ArticleSettings.IncludeInPageName) Then
                                HttpContext.Current.Items.Add("NA1-CategoryName", objCategory.Name)
                            End If

                            Exit For
                        End If
                    Next
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                LoadStyleSheet()

                If (HttpContext.Current.Items.Contains("NA1-CategoryName")) Then
                    PortalSettings.ActiveTab.TabName = HttpContext.Current.Items("NA1-CategoryName").ToString()
                End If
                
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace