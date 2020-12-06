Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Services.FileSystem
Imports System.IO
Imports DotNetNuke.Entities.Modules

Namespace Ventrian.NewsArticles

    Public Class CoreFileProvider

        Inherits FileProvider

#Region " Public Methods "

		Public Overrides Function AddFile(ByVal articleID As Integer, ByVal moduleID As Integer, folderID As Integer, ByVal objPostedFile As System.Web.HttpPostedFile) As Integer

			Dim objFile As New FileInfo

			objFile.ArticleID = articleID
			objFile.FileName = CleanFilename(objPostedFile.FileName)
			objFile.SortOrder = 0

			Dim filesList As List(Of FileInfo) = GetFiles(articleID)

			If (filesList.Count > 0) Then
				objFile.SortOrder = CType(filesList(filesList.Count - 1), FileInfo).SortOrder + 1
			End If

			Dim objPortalSettings As PortalSettings = PortalController.Instance.GetCurrentPortalSettings()

			Dim folder As String = ""

			If (folderID <> Null.NullInteger) Then
				Dim objFolder As FolderInfo = FolderManager.Instance.GetFolder(folderID)
				If (objFolder IsNot Nothing) Then
					folder = objFolder.FolderPath
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

			If (articleID > 0) Then
				Dim objArticleController As New ArticleController
				Dim objArticle As ArticleInfo = objArticleController.GetArticle(articleID)
				objArticle.FileCount = objArticle.FileCount + 1
				objArticleController.UpdateArticle(objArticle)
			End If

			Return objFile.FileID

		End Function

		Public Overrides Function AddFile(ByVal articleID As Integer, ByVal moduleID As Integer, folderID As Integer, ByVal objPostedFile As HttpPostedFile, ByVal providerParams As Object) As Integer
			Return AddFile(articleID, moduleID, folderID, objPostedFile)
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
            objFile.Link = GetFileUrl(objFile)
            Return objFile
        End Function

        Public Overrides Function GetFiles(ByVal articleID As Integer) As System.Collections.Generic.List(Of FileInfo)
            Dim objFileController As New FileController()
            Dim objFiles As List(Of FileInfo) = objFileController.GetFileList(articleID, Null.NullString())
            For Each objFile As FileInfo In objFiles
                objFile.Link = GetFileUrl(objFile)
            Next
            Return objFiles
        End Function

        Public Overrides Sub UpdateFile(ByVal objFile As FileInfo)
            Dim objFileController As New FileController()
            objFileController.Update(objFile)
        End Sub

#End Region
        
		''' <summary>
		''' Cleans a filename from forbidden characters on Windows Filesystems
		''' </summary>
		''' <param name="filename"></param>
		''' <returns></returns>
		public shared Function CleanFilename(ByVal filename As String) As String
		    ' stuk vanaf de laatste forward of backslash is de bestandsnaam
		    ' bestandsnaam zonder extensie moet worden beperkt tot max 200 karakters.            
		    Dim retval As String = ""
		    Dim folderChars = "\/"

		    If filename.LastIndexOfAny(folderChars.ToCharArray()) >= 0 Then
		        retval = filename.Substring(filename.LastIndexOfAny(folderChars.ToCharArray()) + 1)
		    Else
		        retval = filename
		    End If
		    ' forbidden characters are: \/:*?"<>|
		    Dim regex = New Regex("[:\\/\*\?""<>\|]", RegexOptions.CultureInvariant Or RegexOptions.Compiled)
		    ' Replace the matched text in the InputText using the replacement pattern
		    retval = regex.Replace(retval, "-")

		    'If retval.Length > 200 Then retval = retval.Substring(0, 200)
		    Return retval
		End Function

        Private Shared Function GetFileUrl(objFile As FileInfo) As String

            Dim portalSettings As PortalSettings = PortalController.Instance.GetCurrentPortalSettings()
            Dim folderManager As IFolderManager = DotNetNuke.Services.FileSystem.FolderManager.Instance
            Dim fileManager As IFileManager = DotNetNuke.Services.FileSystem.FileManager.Instance
            Dim folder As IFolderInfo = folderManager.GetFolder(portalSettings.PortalId, objFile.Folder)
            If folder IsNot Nothing Then
                Dim file As IFileInfo = fileManager.GetFile(folder, objFile.FileName)
                If file IsNot Nothing Then
                    Return fileManager.GetUrl(file)
                End If
            End If 

            Return portalSettings.HomeDirectory() & objFile.Folder & objFile.FileName
        End Function
    End Class

End Namespace
