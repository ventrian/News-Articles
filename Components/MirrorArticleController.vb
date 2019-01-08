'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Data

Imports DotNetNuke
Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles

    Public Class MirrorArticleController

#Region " Public Methods "

        Public Sub AddMirrorArticle(ByVal objMirrorArticle As MirrorArticleInfo)

            DataProvider.Instance().AddMirrorArticle(objMirrorArticle.ArticleID, objMirrorArticle.LinkedArticleID, objMirrorArticle.LinkedPortalID, objMirrorArticle.AutoUpdate)

        End Sub

        Public Function GetMirrorArticle(ByVal articleID As Integer) As MirrorArticleInfo

            Return CBO.FillObject(Of MirrorArticleInfo)(DataProvider.Instance().GetMirrorArticle(articleID))

        End Function

        Public Function GetMirrorArticleList(ByVal linkedArticleID As Integer) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().GetMirrorArticleList(linkedArticleID), GetType(MirrorArticleInfo))

        End Function

#End Region

    End Class

End Namespace