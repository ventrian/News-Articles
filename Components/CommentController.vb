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

    Public Class CommentController

        Public Shared Sub ClearCache(ByVal articleID As Integer)

            Dim itemsToRemove As New List(Of String)()

            Dim enumerator As IDictionaryEnumerator = HttpContext.Current.Cache.GetEnumerator()
            While enumerator.MoveNext()
                If enumerator.Key.ToString().ToLower().Contains("ventrian-newsarticles-comments") Then
                    itemsToRemove.Add(enumerator.Key.ToString())
                End If
            End While

            For Each itemToRemove As String In itemsToRemove
                DataCache.RemoveCache(itemToRemove.Replace("DNN_", ""))
            Next

        End Sub

#Region " Public Methods "

        Public Function GetCommentList(ByVal moduleID As Integer, ByVal articleID As Integer, ByVal isApproved As Boolean) As List(Of CommentInfo)

            Return GetCommentList(moduleID, articleID, isApproved, SortDirection.Ascending, Null.NullInteger)

        End Function

        Public Function GetCommentList(ByVal moduleID As Integer, ByVal articleID As Integer, ByVal isApproved As Boolean, ByVal direction As SortDirection, ByVal maxCount As Integer) As List(Of CommentInfo)

            Dim cacheKey As String = "ventrian-newsarticles-comments-" & moduleID.ToString() & "-" & articleID.ToString() & "-" & isApproved.ToString() & "-" & direction.ToString() & "-" & maxCount.ToString()

            Dim objComments As List(Of CommentInfo) = CType(DataCache.GetCache(cacheKey), List(Of CommentInfo))

            If (objComments Is Nothing) Then
                objComments = CBO.FillCollection(Of CommentInfo)(DataProvider.Instance().GetCommentList(moduleID, articleID, isApproved, direction, maxCount))
                DataCache.SetCache(cacheKey, objComments)
            End If

            Return objComments

        End Function

        Public Function GetComment(ByVal commentID As Integer) As CommentInfo

            Return CBO.FillObject(Of CommentInfo)(DataProvider.Instance().GetComment(commentID))

        End Function

        Public Sub DeleteComment(ByVal commentID As Integer, ByVal articleID As Integer)

            DataProvider.Instance().DeleteComment(commentID)

            ArticleController.ClearArticleCache(articleID)
            CommentController.ClearCache(articleID)

        End Sub

        Public Function AddComment(ByVal objComment As CommentInfo) As Integer

            Dim commentID As Integer = CType(DataProvider.Instance().AddComment(objComment.ArticleID, objComment.CreatedDate, objComment.UserID, objComment.Comment, objComment.RemoteAddress, objComment.Type, objComment.TrackbackUrl, objComment.TrackbackTitle, objComment.TrackbackBlogName, objComment.TrackbackExcerpt, objComment.AnonymousName, objComment.AnonymousEmail, objComment.AnonymousURL, objComment.NotifyMe, objComment.IsApproved, objComment.ApprovedBy), Integer)

            ArticleController.ClearArticleCache(objComment.ArticleID)
            CommentController.ClearCache(objComment.ArticleID)

            Return commentID

        End Function

        Public Sub UpdateComment(ByVal objComment As CommentInfo)

            DataProvider.Instance().UpdateComment(objComment.CommentID, objComment.ArticleID, objComment.UserID, objComment.Comment, objComment.RemoteAddress, objComment.Type, objComment.TrackbackUrl, objComment.TrackbackTitle, objComment.TrackbackBlogName, objComment.TrackbackExcerpt, objComment.AnonymousName, objComment.AnonymousEmail, objComment.AnonymousURL, objComment.NotifyMe, objComment.IsApproved, objComment.ApprovedBy)

            ArticleController.ClearArticleCache(objComment.ArticleID)
            CommentController.ClearCache(objComment.ArticleID)

        End Sub

#End Region

    End Class

End Namespace