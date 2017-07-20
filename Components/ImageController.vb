Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles

    Public Class ImageController

#Region " Public Methods "

        Public Function [Get](ByVal imageID As Integer) As ImageInfo

            Return CType(CBO.FillObject(DataProvider.Instance().GetImage(imageID), GetType(ImageInfo)), ImageInfo)

        End Function

        Public Function GetImageList(ByVal articleID As Integer, ByVal imageGuid As String) As List(Of ImageInfo)

            Dim objImages As List(Of ImageInfo) = CType(DataCache.GetCache(ArticleConstants.CACHE_IMAGE_ARTICLE & articleID.ToString()), List(Of ImageInfo))

            If (objImages Is Nothing) Then
                objImages = CBO.FillCollection(Of ImageInfo)(DataProvider.Instance().GetImageList(articleID, imageGuid))
                DataCache.SetCache(ArticleConstants.CACHE_IMAGE_ARTICLE & articleID.ToString() & imageGuid, objImages)
            End If
            Return objImages

        End Function

        Public Function Add(ByVal objImage As ImageInfo) As Integer

            DataCache.RemoveCache(ArticleConstants.CACHE_IMAGE_ARTICLE & objImage.ArticleID.ToString() & objImage.ImageGuid)
            Dim imageID As Integer = CType(DataProvider.Instance().AddImage(objImage.ArticleID, objImage.Title, objImage.FileName, objImage.Extension, objImage.Size, objImage.Width, objImage.Height, objImage.ContentType, objImage.Folder, objImage.SortOrder, objImage.ImageGuid, objImage.Description), Integer)
            ArticleController.ClearArticleCache(objImage.ArticleID)
            Return imageID

        End Function

        Public Sub Update(ByVal objImage As ImageInfo)

            DataProvider.Instance().UpdateImage(objImage.ImageID, objImage.ArticleID, objImage.Title, objImage.FileName, objImage.Extension, objImage.Size, objImage.Width, objImage.Height, objImage.ContentType, objImage.Folder, objImage.SortOrder, objImage.ImageGuid, objImage.Description)
            DataCache.RemoveCache(ArticleConstants.CACHE_IMAGE_ARTICLE & objImage.ArticleID.ToString() & objImage.ImageGuid)
            DataCache.RemoveCache(ArticleConstants.CACHE_IMAGE_ARTICLE & objImage.ArticleID.ToString())
            ArticleController.ClearArticleCache(objImage.ArticleID)

        End Sub

        Public Sub Delete(ByVal imageID As Integer, ByVal articleID As Integer, ByVal imageGuid As String)

            DataProvider.Instance().DeleteImage(imageID)
            DataCache.RemoveCache(ArticleConstants.CACHE_IMAGE_ARTICLE & articleID.ToString() & imageGuid)
            ArticleController.ClearArticleCache(articleID)

        End Sub

#End Region

    End Class

End Namespace
