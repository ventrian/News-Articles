Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles

    Public Class HandoutController

#Region " Public Methods "

        Public Function AddHandout(ByVal objHandout As HandoutInfo) As Integer

            Dim handoutID As Integer = CType(DataProvider.Instance().AddHandout(objHandout.ModuleID, objHandout.UserID, objHandout.Name, objHandout.Description), Integer)

            Dim i As Integer = 0
            For Each objHandoutArticle As HandoutArticle In objHandout.Articles
                DataProvider.Instance().AddHandoutArticle(handoutID, objHandoutArticle.ArticleID, i)
                i = i + 1
            Next

        End Function

        Public Sub DeleteHandout(ByVal handoutID As Integer)

            DataProvider.Instance().DeleteHandout(handoutID)
            DataProvider.Instance().DeleteHandoutArticleList(handoutID)

        End Sub

        Public Function GetHandout(ByVal handoutID As Integer) As HandoutInfo

            Dim objHandout As HandoutInfo = CBO.FillObject(Of HandoutInfo)(DataProvider.Instance().GetHandout(handoutID))

            objHandout.Articles = CBO.FillCollection(Of HandoutArticle)(DataProvider.Instance().GetHandoutArticleList(handoutID))

            Return objHandout

        End Function

        Public Function ListHandout(ByVal userID As Integer) As List(Of HandoutInfo)

            Return CBO.FillCollection(Of HandoutInfo)(DataProvider.Instance().GetHandoutList(userID))

        End Function

        Public Sub UpdateHandout(ByVal objHandout As HandoutInfo)

            DataProvider.Instance().UpdateHandout(objHandout.HandoutID, objHandout.ModuleID, objHandout.UserID, objHandout.Name, objHandout.Description)
            DataProvider.Instance().DeleteHandoutArticleList(objHandout.HandoutID)

            Dim i As Integer = 0
            For Each objHandoutArticle As HandoutArticle In objHandout.Articles
                DataProvider.Instance().AddHandoutArticle(objHandout.HandoutID, objHandoutArticle.ArticleID, i)
                i = i + 1
            Next

        End Sub

#End Region

    End Class

End Namespace

