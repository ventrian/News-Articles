Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles

    Public Class TagController

#Region " Private Methods "

        Private Sub RemoveCache(ByVal tagID As Integer)

            Dim objTag As TagInfo = [Get](tagID)

            If Not (objTag Is Nothing) Then
                RemoveCache(objTag.ModuleID, objTag.TagID.ToString())
            End If

        End Sub

        Private Sub RemoveCache(ByVal moduleID As Integer, ByVal nameLowered As String)

            If Not (DataCache.GetCache("Tag-" & moduleID.ToString() & "-" & nameLowered) Is Nothing) Then
                DataCache.RemoveCache("Tag-" & moduleID.ToString() & "-" & nameLowered)
            End If

        End Sub

#End Region

#Region " Public Methods "

        Public Function [Get](ByVal tagID As Integer) As TagInfo

            Return CBO.FillObject(Of TagInfo)(DataProvider.Instance().GetTag(tagID))

        End Function

        Public Function [Get](ByVal moduleID As Integer, ByVal nameLowered As String) As TagInfo

            Dim objTag As TagInfo = CType(DataCache.GetCache("Tag-" & moduleID.ToString() & "-" & nameLowered), TagInfo)

            If (objTag Is Nothing) Then
                objTag = CBO.FillObject(Of TagInfo)(DataProvider.Instance().GetTagByName(moduleID, nameLowered))
                If Not (objTag Is Nothing) Then
                    DataCache.SetCache("Tag-" & moduleID.ToString() & "-" & nameLowered, objTag)
                End If
            End If

            Return objTag

        End Function

        Public Function List(ByVal moduleID As Integer, ByVal maxCount As Integer) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().ListTag(moduleID, maxCount), GetType(TagInfo))

        End Function

        Public Function Add(ByVal objTag As TagInfo) As Integer

            Return DataProvider.Instance().AddTag(objTag.ModuleID, objTag.Name, objTag.NameLowered)

        End Function

        Public Sub Update(ByVal objTag As TagInfo)

            RemoveCache(objTag.ModuleID, objTag.NameLowered)
            DataProvider.Instance().UpdateTag(objTag.TagID, objTag.ModuleID, objTag.Name, objTag.NameLowered, objTag.Usages)

        End Sub

        Public Sub Delete(ByVal tagID As Integer)

            RemoveCache(tagID)
            DataProvider.Instance().DeleteTag(tagID)

        End Sub

        Public Sub DeleteArticleTag(ByVal articleID As Integer)

            Dim objArticleController As New ArticleController()
            Dim objArticle As ArticleInfo = objArticleController.GetArticle(articleID)

            If Not (objArticle Is Nothing) Then
                If (objArticle.Tags.Trim() <> "") Then
                    For Each tag As String In objArticle.Tags.Split(","c)
                        RemoveCache(objArticle.ModuleID, tag.ToLower())
                    Next
                End If
            End If
            DataProvider.Instance().DeleteArticleTag(articleID)

        End Sub

        Public Sub DeleteArticleTagByTag(ByVal tagID As Integer)

            RemoveCache(tagID)
            DataProvider.Instance().DeleteArticleTag(tagID)

        End Sub

        Public Sub Add(ByVal articleID As Integer, ByVal tagID As Integer, Optional ByVal displayOrder As Integer = 0)

            RemoveCache(tagID)
            DataProvider.Instance().AddArticleTag(articleID, tagID, displayOrder)

        End Sub

#End Region

    End Class

End Namespace
