Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles

    Public Class FileController

        Public Shared Sub ClearCache(ByVal articleID As Integer)

            Dim itemsToRemove As New List(Of String)()

            Dim enumerator As IDictionaryEnumerator = HttpContext.Current.Cache.GetEnumerator()
            While enumerator.MoveNext()
                If enumerator.Key.ToString().ToLower().Contains("ventrian-newsarticles-files-" & articleID.ToString()) Then
                    itemsToRemove.Add(enumerator.Key.ToString())
                End If
            End While

            For Each itemToRemove As String In itemsToRemove
                DataCache.RemoveCache(itemToRemove.Replace("DNN_", ""))
            Next

        End Sub

#Region " Public Methods "

        Public Function [Get](ByVal fileID As Integer) As FileInfo

            Return CBO.FillObject(Of FileInfo)(DataProvider.Instance().GetFile(fileID))

        End Function

        Public Function GetFileList(ByVal articleID As Integer, ByVal fileGuid As String) As List(Of FileInfo)

            If (articleID = Null.NullInteger) Then
                Return CBO.FillCollection(Of FileInfo)(DataProvider.Instance().GetFileList(articleID, fileGuid))
            Else
                Dim cacheKey As String = "ventrian-newsarticles-files-" & articleID.ToString() & "-" & fileGuid.ToString()

                Dim objFiles As List(Of FileInfo) = CType(DataCache.GetCache(cacheKey), List(Of FileInfo))

                If (objFiles Is Nothing) Then
                    objFiles = CBO.FillCollection(Of FileInfo)(DataProvider.Instance().GetFileList(articleID, fileGuid))
                    DataCache.SetCache(cacheKey, objFiles)
                End If

                Return objFiles
            End If

        End Function

        Public Function Add(ByVal objFile As FileInfo) As Integer

            Dim fileID As Integer = CType(DataProvider.Instance().AddFile(objFile.ArticleID, objFile.Title, objFile.FileName, objFile.Extension, objFile.Size, objFile.ContentType, objFile.Folder, objFile.SortOrder, objFile.FileGuid), Integer)

            FileController.ClearCache(objFile.ArticleID)
            ArticleController.ClearArticleCache(objFile.ArticleID)

            Return fileID

        End Function

        Public Sub Update(ByVal objFile As FileInfo)

            DataProvider.Instance().UpdateFile(objFile.FileID, objFile.ArticleID, objFile.Title, objFile.FileName, objFile.Extension, objFile.Size, objFile.ContentType, objFile.Folder, objFile.SortOrder, objFile.FileGuid)

            FileController.ClearCache(objFile.ArticleID)

        End Sub

        Public Sub Delete(ByVal fileID As Integer)

            Dim objFile As FileInfo = [Get](fileID)

            If (objFile IsNot Nothing) Then
                DataProvider.Instance().DeleteFile(fileID)
                FileController.ClearCache(objFile.ArticleID)
            End If

        End Sub

#End Region

    End Class

End Namespace
