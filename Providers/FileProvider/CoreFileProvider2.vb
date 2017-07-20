Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Services.FileSystem
Imports System.IO
Imports DotNetNuke.Entities.Modules

Namespace Ventrian.NewsArticles

    Public Class CoreFileProvider2

        Inherits FileProvider

#Region " Public Methods "

        Public Overrides Function AddFile(ByVal articleID As Integer, ByVal moduleID As Integer, ByVal objPostedFile As System.Web.HttpPostedFile) As Integer

            Dim objFile As New FileInfo

            objFile.ArticleID = articleID
            objFile.FileName = objPostedFile.FileName
            objFile.SortOrder = 0

            Dim filesList As List(Of FileInfo) = GetFiles(articleID)

            If (filesList.Count > 0) Then
                objFile.SortOrder = CType(filesList(filesList.Count - 1), FileInfo).SortOrder + 1
            End If

            Dim objPortalSettings As PortalSettings = PortalController.GetCurrentPortalSettings()

            Dim folder As String = ""

            Dim objModuleController As New ModuleController()
            Dim objSettings As Hashtable = objModuleController.GetModuleSettings(moduleID)

            If (objSettings.ContainsKey(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING)) Then
                If (IsNumeric(objSettings(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING))) Then
                    Dim folderID As Integer = Convert.ToInt32(objSettings(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING))
                    Dim objFolderController As New FolderController
                    Dim objFolder As FolderInfo = objFolderController.GetFolderInfo(objPortalSettings.PortalId, folderID)
                    If (objFolder IsNot Nothing) Then
                        folder = objFolder.FolderPath
                    End If
                End If
            End If

            objFile.Folder = folder
            objFile.ContentType = objPostedFile.ContentType

            If (objFile.FileName.Split("."c).Length > 0) Then
                objFile.Extension = objFile.FileName.Split("."c)(objFile.FileName.Split("."c).Length - 1)

                If (objFile.Extension.ToLower() = "jpg") Then
                    objFile.ContentType = "image/jpeg"
                End If
                If (objFile.Extension.ToLower() = "gif") Then
                    objFile.ContentType = "image/gif"
                End If
                If (objFile.Extension.ToLower() = "txt") Then
                    objFile.ContentType = "text/plain"
                End If
                If (objFile.Extension.ToLower() = "html") Then
                    objFile.ContentType = "text/html"
                End If
                If (objFile.Extension.ToLower() = "mp3") Then
                    objFile.ContentType = "audio/mpeg"
                End If

            End If
            objFile.Title = objFile.FileName.Replace("." & objFile.Extension, "")

            Dim filePath As String = objPortalSettings.HomeDirectoryMapPath & folder.Replace("/", "\")

            If Not (Directory.Exists(filePath)) Then
                Directory.CreateDirectory(filePath)
            End If

            If (File.Exists(filePath & objFile.FileName)) Then
                For i As Integer = 1 To 100
                    If (File.Exists(filePath & i.ToString() & "_" & objFile.FileName) = False) Then
                        objFile.FileName = i.ToString() & "_" & objFile.FileName
                        Exit For
                    End If
                Next
            End If

            objFile.Size = objPostedFile.ContentLength
            objPostedFile.SaveAs(filePath & objFile.FileName)

            Dim objFileController As New FileController
            objFile.FileID = objFileController.Add(objFile)

            If (articleID <> Null.NullInteger) Then
                Dim objArticleController As New ArticleController
                Dim objArticle As ArticleInfo = objArticleController.GetArticle(articleID)
                If (objArticle IsNot Nothing) Then
                    objArticle.FileCount = objArticle.FileCount + 1
                    objArticleController.UpdateArticle(objArticle)
                End If
            End If

            Return objFile.FileID

        End Function

        Public Overrides Function AddFile(ByVal articleID As Integer, ByVal moduleID As Integer, ByVal objPostedFile As HttpPostedFile, ByVal providerParams As Object) As Integer
            Return AddFile(articleID, moduleID, objPostedFile)
        End Function

        Public Overrides Function AddExistingFile(ByVal articleID As Integer, ByVal moduleID As Integer, ByVal providerParams As Object) As Integer
            Throw New NotImplementedException()
        End Function

        Public Overrides Sub DeleteFile(ByVal articleID As Integer, ByVal fileID As Integer)
            Dim objFileController As New FileController()
            objFileController.Delete(fileID)
        End Sub

        Public Overrides Function GetFile(ByVal fileID As Integer) As FileInfo
            Dim objFileController As New FileController()
            Dim objFile As FileInfo = objFileController.Get(fileID)
            objFile.Link = PortalController.GetCurrentPortalSettings().HomeDirectory() & objFile.Folder & objFile.FileName & "1"
            Return objFile
        End Function

        Public Overrides Function GetFiles(ByVal articleID As Integer) As System.Collections.Generic.List(Of FileInfo)
            Dim objFileController As New FileController()
            Dim objFiles As List(Of FileInfo) = objFileController.GetFileList(articleID, Null.NullString())
            For Each objFile As FileInfo In objFiles
                objFile.Link = PortalController.GetCurrentPortalSettings().HomeDirectory() & objFile.Folder & objFile.FileName & "1"
            Next
            Return objFiles
        End Function

        Public Overrides Sub UpdateFile(ByVal objFile As FileInfo)
            Dim objFileController As New FileController()
            objFileController.Update(objFile)
        End Sub

#End Region

    End Class

End Namespace
