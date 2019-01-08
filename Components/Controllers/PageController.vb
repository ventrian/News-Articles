'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles

    Public Class PageController

#Region " Public Methods "

        Public Function GetPageList(ByVal articleId As Integer) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().GetPageList(articleId), GetType(PageInfo))

        End Function

        Public Function GetPage(ByVal pageId As Integer) As PageInfo

            Return CBO.FillObject(Of PageInfo)(DataProvider.Instance().GetPage(pageId))

        End Function

        Public Sub DeletePage(ByVal pageId As Integer)

            DataProvider.Instance().DeletePage(pageId)

        End Sub

        Public Function AddPage(ByVal objPage As PageInfo) As Integer

            Dim pageId As Integer = DataProvider.Instance().AddPage(objPage.ArticleID, objPage.Title, objPage.PageText, objPage.SortOrder)
            ArticleController.ClearArticleCache(objPage.ArticleID)
            Return pageId

        End Function

        Public Sub UpdatePage(ByVal objPage As PageInfo)

            DataProvider.Instance().UpdatePage(objPage.PageID, objPage.ArticleID, objPage.Title, objPage.PageText, objPage.SortOrder)

            ArticleController.ClearArticleCache(objPage.ArticleID)

        End Sub

#End Region

    End Class

End Namespace