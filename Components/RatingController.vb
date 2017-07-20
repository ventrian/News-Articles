'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Framework

Namespace Ventrian.NewsArticles

    Public Class RatingController

#Region " Static Methods "

        Public Shared Sub ClearCache(ByVal moduleID As Integer)

            Dim itemsToRemove As New List(Of String)()

            Dim enumerator As IDictionaryEnumerator = HttpContext.Current.Cache.GetEnumerator()
            While enumerator.MoveNext()
                If enumerator.Key.ToString().ToLower().StartsWith("ventrian-newsarticles-rating-" & moduleID.ToString()) Then
                    itemsToRemove.Add(enumerator.Key.ToString())
                End If
            End While

            For Each itemToRemove As String In itemsToRemove
                DataCache.RemoveCache(itemToRemove.Replace("DNN_", ""))
            Next

        End Sub

#End Region

#Region " Public Methods "

        Public Function Add(ByVal objRating As RatingInfo, ByVal moduleID As Integer) As Integer

            Dim ratingID As Integer = CType(DataProvider.Instance().AddRating(objRating.ArticleID, objRating.UserID, objRating.CreatedDate, objRating.Rating), Integer)

            ArticleController.ClearArticleCache(objRating.ArticleID)

            Dim cacheKey As String = "ventrian-newsarticles-rating-" & moduleID.ToString() & "-aid-" & objRating.ArticleID.ToString() & "-" & objRating.UserID.ToString()
            DataCache.RemoveCache(cacheKey)

            Return ratingID

        End Function

        Public Function [Get](ByVal articleID As Integer, ByVal userID As Integer, ByVal moduleID As Integer) As RatingInfo

            Dim cacheKey As String = "ventrian-newsarticles-rating-" & moduleID.ToString() & "-aid-" & articleID.ToString() & "-" & userID.ToString()

            Dim objRating As RatingInfo = CType(DataCache.GetCache(cacheKey), RatingInfo)

            If (objRating Is Nothing) Then
                objRating = CType(CBO.FillObject(DataProvider.Instance().GetRating(articleID, userID), GetType(RatingInfo)), RatingInfo)
                If (objRating IsNot Nothing) Then
                    DataCache.SetCache(cacheKey, objRating)
                Else
                    objRating = New RatingInfo()
                    objRating.RatingID = Null.NullInteger
                    DataCache.SetCache(cacheKey, objRating)
                End If
            End If

            Return objRating

        End Function

        Public Function [GetByID](ByVal ratingID As Integer, ByVal articleID As Integer, ByVal moduleID As Integer) As RatingInfo

            Dim cacheKey As String = "ventrian-newsarticles-rating-" & moduleID.ToString() & "-id-" & ratingID.ToString()

            Dim objRating As RatingInfo = CType(DataCache.GetCache(cacheKey), RatingInfo)

            If (objRating Is Nothing) Then
                objRating = CType(CBO.FillObject(DataProvider.Instance().GetRatingByID(ratingID), GetType(RatingInfo)), RatingInfo)
                DataCache.SetCache(cacheKey, objRating)
            End If

            Return objRating

        End Function

        Public Sub Delete(ByVal ratingID As Integer, ByVal articleID As Integer, ByVal moduleID As Integer)

            DataProvider.Instance().DeleteRating(ratingID)

            ArticleController.ClearArticleCache(articleID)

            Dim cacheKey As String = "ventrian-newsarticles-rating-" & moduleID.ToString() & "-id-" & ratingID.ToString()
            DataCache.RemoveCache(cacheKey)

        End Sub

#End Region

    End Class

End Namespace
