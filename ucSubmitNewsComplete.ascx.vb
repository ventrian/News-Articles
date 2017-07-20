'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucSubmitNewsComplete
        Inherits NewsArticleModuleBase

#Region " Event Handlers "

        Private Sub cmdSubmitArticle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmitArticle.Click

            Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "SubmitNews", ArticleSettings), True)

        End Sub

        Private Sub cmdViewMyArticles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdViewMyArticles.Click

            Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "MyArticles", ArticleSettings), True)

        End Sub

        Private Sub cmdCurrentArticles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCurrentArticles.Click

            Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "", ArticleSettings), True)

        End Sub

#End Region

    End Class

End Namespace
