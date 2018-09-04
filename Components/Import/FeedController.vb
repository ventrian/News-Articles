Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles.Import

    Public Class FeedController

#Region " Public Methods "

        Public Function [Get](ByVal feedID As Integer) As FeedInfo

            Return CBO.FillObject(Of FeedInfo)(DataProvider.Instance().GetFeed(feedID))

        End Function

        Public Function GetFeedList(ByVal moduleID As Integer, ByVal showActiveOnly As Boolean) As List(Of FeedInfo)

            Return CBO.FillCollection(Of FeedInfo)(DataProvider.Instance().GetFeedList(moduleID, showActiveOnly))

        End Function

        Public Function Add(ByVal objFeed As FeedInfo) As Integer

            Dim feedID As Integer = CType(DataProvider.Instance().AddFeed(objFeed.ModuleID, objFeed.Title, objFeed.Url, objFeed.UserID, objFeed.AutoFeature, objFeed.IsActive, objFeed.DateMode, objFeed.AutoExpire, objFeed.AutoExpireUnit), Integer)

            For Each objCategory As CategoryInfo In objFeed.Categories
                AddFeedCategory(feedID, objCategory.CategoryID)
            Next

            Return feedID

        End Function

        Public Sub Update(ByVal objFeed As FeedInfo)

            DataProvider.Instance().UpdateFeed(objFeed.FeedID, objFeed.ModuleID, objFeed.Title, objFeed.Url, objFeed.UserID, objFeed.AutoFeature, objFeed.IsActive, objFeed.DateMode, objFeed.AutoExpire, objFeed.AutoExpireUnit)

            DeleteFeedCategory(objFeed.FeedID)
            For Each objCategory As CategoryInfo In objFeed.Categories
                AddFeedCategory(objFeed.FeedID, objCategory.CategoryID)
            Next

        End Sub

        Public Sub Delete(ByVal feedID As Integer)

            DataProvider.Instance().DeleteFeed(feedID)

        End Sub

        Public Function AddFeedCategory(ByVal feedID As Integer, ByVal categoryID As Integer) As Integer

            DataProvider.Instance().AddFeedCategory(feedID, categoryID)

        End Function

        Public Function GetFeedCategoryList(ByVal feedID As Integer) As List(Of CategoryInfo)

            Return CBO.FillCollection(Of CategoryInfo)(DataProvider.Instance().GetFeedCategoryList(feedID))

        End Function

        Public Sub DeleteFeedCategory(ByVal feedID As Integer)

            DataProvider.Instance().DeleteFeedCategory(feedID)

        End Sub

#End Region

    End Class

End Namespace
