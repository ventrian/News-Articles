'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Security

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ViewSearch
        Inherits NewsArticleModuleBase

#Region " Constants "

        Private Const PARAM_SEARCH_ID As String = "Search"

#End Region

#Region " Private Members "

        Private _searchText As String = Null.NullString

#End Region

#Region " Private Methods "

        Private Sub BindSearch()

            Me.BasePage.Title = "Search | " & Me.BasePage.Title

            If (_searchText = "") Then
                lblSearch.Text = Localization.GetString("SearchArticles", Me.LocalResourceFile)
                Listing1.BindArticles = False
                Return
            Else
                Dim articlesFor As String = Localization.GetString("ArticlesFor", Me.LocalResourceFile)
                If (articlesFor.Contains("{0}")) Then
                    lblSearch.Text = String.Format(articlesFor, _searchText)
                Else
                    lblSearch.Text = articlesFor
                End If
                txtSearch.Text = _searchText
                Listing1.SearchText = _searchText
                Listing1.BindArticles = True
                Listing1.BindListing()
                Listing1.BindArticles = False
                Return
            End If

        End Sub

        Private Sub ReadQueryString()

            If (Request(PARAM_SEARCH_ID) <> "") Then
                _searchText = Server.UrlDecode(Request(PARAM_SEARCH_ID))
                Dim objSecurity As New PortalSecurity
                _searchText = objSecurity.InputFilter(_searchText, PortalSecurity.FilterFlag.NoScripting)
                _searchText = StripTags(_searchText)
            End If

        End Sub

        Function StripTags(ByVal html As String) As String
            ' Remove HTML tags.
            Return Regex.Replace(html, "<.*?>", "")
        End Function


#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                Dim enumerator As IDictionaryEnumerator = HttpContext.Current.Cache.GetEnumerator()
                Dim itemsToRemove As New List(Of String)()

                While enumerator.MoveNext()
                    If enumerator.Key.ToString().ToLower().Contains("ventrian-newsarticles-categories-" & ModuleId.ToString()) Then
                        itemsToRemove.Add(enumerator.Key.ToString())
                    End If
                End While

                For Each itemToRemove As String In itemsToRemove
                    DataCache.RemoveCache(itemToRemove.Replace("DNN_", ""))
                Next

                enumerator = HttpContext.Current.Cache.GetEnumerator()
                While enumerator.MoveNext()
                    If enumerator.Key.ToString().ToLower().Contains("ventrian-newsarticles-categories-" & ModuleId.ToString()) Then
                        Response.Write(enumerator.Key.ToString() & "<BR>")
                    End If
                End While
                ReadQueryString()
                Listing1.ShowExpired = True
                Listing1.MaxArticles = Null.NullInteger
                Listing1.IsIndexed = False

                BindSearch()
                Page.SetFocus(txtSearch)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click

            Try

                If (txtSearch.Text.Trim() <> "") Then
                    Dim objSecurity As New PortalSecurity
                    Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "Search", ArticleSettings, "Search=" & Server.UrlEncode(objSecurity.InputFilter(txtSearch.Text, PortalSecurity.FilterFlag.NoScripting))), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace