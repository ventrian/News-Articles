'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2009
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class NewsSearch
        Inherits NewsArticleModuleBase

#Region " Constants "

        Private Const PARAM_SEARCH_ID As String = "Search"

#End Region

#Region " Private Members "

        Private _articleTabID As Integer = Null.NullInteger
        Private _articleModuleID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Function FindSettings() As Boolean

            If (Settings.Contains(ArticleConstants.NEWS_SEARCH_TAB_ID)) Then
                If (IsNumeric(Settings(ArticleConstants.NEWS_SEARCH_TAB_ID).ToString())) Then
                    _articleTabID = Convert.ToInt32(Settings(ArticleConstants.NEWS_SEARCH_TAB_ID).ToString())
                End If
            End If

            If (Settings.Contains(ArticleConstants.NEWS_SEARCH_MODULE_ID)) Then
                If (IsNumeric(Settings(ArticleConstants.NEWS_SEARCH_MODULE_ID).ToString())) Then
                    _articleModuleID = Convert.ToInt32(Settings(ArticleConstants.NEWS_SEARCH_MODULE_ID).ToString())
                    If (_articleModuleID <> Null.NullInteger) Then
                        Return True
                    End If
                End If
            End If

            Return False

        End Function

        Private Sub ReadQueryString()

            If (Request(PARAM_SEARCH_ID) <> "") Then
                txtSearch.Text = Server.UrlDecode(Request(PARAM_SEARCH_ID))
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try
                If (IsPostBack = False) Then
                    ReadQueryString()
                End If

                If (FindSettings()) Then
                    phSearchForm.Visible = True
                    lblNotConfigured.Visible = False
                Else
                    lblNotConfigured.Visible = True
                    phSearchForm.Visible = False
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click

            Try

                If (txtSearch.Text.Trim() <> "") Then
                    Response.Redirect(Common.GetModuleLink(_articleTabID, _articleModuleID, "Search", ArticleSettings, "Search=" & Server.UrlEncode(txtSearch.Text)), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace