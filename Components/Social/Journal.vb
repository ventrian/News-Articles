Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Journal

Namespace Ventrian.NewsArticles.Components.Social

    Public Class Journal

        Public Const ContentTypeName As String = "Ventrian_Article_"

#Region "Internal Methods"

        Friend Sub AddArticleToJournal(ByVal objArticle As ArticleInfo, ByVal portalId As Integer, ByVal tabId As Integer, ByVal journalUserId As Integer, ByVal journalGroupID As Integer, ByVal url As String)
            Dim objectKey As String = "Ventrian_Article_" + objArticle.ArticleID.ToString() + "_" + journalGroupID.ToString()
            Dim ji As JournalItem = JournalController.Instance.GetJournalItemByKey(portalId, objectKey)

            If Not ji Is Nothing Then
                JournalController.Instance.DeleteJournalItemByKey(portalId, objectKey)
            End If

            Dim mi As ModuleInfo = ModuleController.Instance.GetModule(objArticle.ModuleID, tabId, False)

            ji = New JournalItem

            ji.PortalId = portalId
            ji.ProfileId = journalUserId
            ji.UserId = journalUserId
            ji.ContentItemId = objArticle.ArticleID
            ji.Title = objArticle.Title
            ji.ItemData = New ItemData()
            ji.ItemData.Url = url
            ji.Summary = objArticle.Summary
            ji.Body = Nothing
            ji.JournalTypeId = 15
            ji.ObjectKey = objectKey
            ji.SecuritySet = "E,"
            ji.SocialGroupId = journalGroupID

            JournalController.Instance.SaveJournalItem(ji, mi)
        End Sub

        Friend Sub AddCommentToJournal(ByVal objArticle As ArticleInfo, ByVal objComment As CommentInfo, ByVal portalId As Integer, ByVal tabId As Integer, ByVal journalUserId As Integer, ByVal url As String)
            Dim objectKey As String = "Ventrian_Article_Comment_" + objArticle.ArticleID.ToString() + ":" + objComment.CommentID.ToString()
            Dim ji As JournalItem = JournalController.Instance.GetJournalItemByKey(portalId, objectKey)

            If Not ji Is Nothing Then
                JournalController.Instance.DeleteJournalItemByKey(portalId, objectKey)
            End If

            Dim mi As ModuleInfo = ModuleController.Instance.GetModule(objArticle.ModuleID, tabId, False)

            ji = New JournalItem

            ji.PortalId = portalId
            ji.ProfileId = journalUserId
            ji.UserId = journalUserId
            ji.ContentItemId = objComment.CommentID
            ji.Title = objArticle.Title
            ji.ItemData = New ItemData()
            ji.ItemData.Url = url
            ji.Summary = objComment.Comment
            ji.Body = Nothing
            ji.JournalTypeId = 18
            ji.ObjectKey = objectKey
            ji.SecuritySet = "E,"

            JournalController.Instance.SaveJournalItem(ji, mi)
        End Sub

        Friend Sub AddRatingToJournal(ByVal objArticle As ArticleInfo, ByVal objRating As RatingInfo, ByVal portalId As Integer, ByVal tabId As Integer, ByVal journalUserId As Integer, ByVal url As String)
            Dim objectKey As String = "Ventrian_Article_Rating_" + objArticle.ArticleID.ToString() + ":" + objRating.RatingID.ToString()
            Dim ji As JournalItem = JournalController.Instance.GetJournalItemByKey(portalId, objectKey)

            If Not ji Is Nothing Then
                JournalController.Instance.DeleteJournalItemByKey(portalId, objectKey)
            End If

            Dim mi As ModuleInfo = ModuleController.Instance.GetModule(objArticle.ModuleID, tabId, False)

            ji = New JournalItem

            ji.PortalId = portalId
            ji.ProfileId = journalUserId
            ji.UserId = journalUserId
            ji.ContentItemId = objRating.RatingID
            ji.Title = objArticle.Title
            ji.ItemData = New ItemData()
            ji.ItemData.Url = url
            ji.Summary = objRating.Rating.ToString()
            ji.Body = Nothing
            ji.JournalTypeId = 17
            ji.ObjectKey = objectKey
            ji.SecuritySet = "E,"

            JournalController.Instance.SaveJournalItem(ji, mi)
        End Sub

#End Region

    End Class

End Namespace
