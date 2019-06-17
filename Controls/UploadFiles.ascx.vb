Imports System
Imports System.Linq
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Security

Imports Ventrian.NewsArticles.Base
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Security.Permissions
Imports DotNetNuke.Services.FileSystem

Namespace Ventrian.NewsArticles.Controls

	Partial Public Class UploadFiles
		Inherits NewsArticleControlBase

#Region " Private Members "

		Private _articleID As Integer = Null.NullInteger

		Private _filesInit As Boolean = False
		Private _objFiles As List(Of FileInfo)

#End Region

#Region " Private Properties "

		Private ReadOnly Property ArticleModuleBase() As NewsArticleModuleBase
			Get
				Return CType(Parent.Parent.Parent.Parent.Parent, NewsArticleModuleBase)
			End Get
		End Property

		Private ReadOnly Property ArticleSettings() As ArticleSettings
			Get
				Return ArticleModuleBase.ArticleSettings
			End Get
		End Property

#End Region

#Region " Public Properties "

		Public Property ArticleGuid() As Integer
			Get
				If (_articleID = Null.NullInteger) Then
					If (litArticleGuid.Text = Null.NullString) Then
						litArticleGuid.Text = (GetRandom(1, 100000) * -1).ToString()
					End If
					Return Convert.ToInt32(litArticleGuid.Text)
				End If
				Return _articleID
			End Get
			Set(ByVal value As Integer)
				litArticleGuid.Text = value
			End Set
		End Property

		Public ReadOnly Property AttachedFiles() As List(Of FileInfo)
			Get

				If (_filesInit = False) Then
					'_objFiles = objFileController.GetFileList(_articleID, ArticleGuid)
					If (_articleID = Null.NullInteger) Then
						_objFiles = FileProvider.Instance().GetFiles(ArticleGuid)
					Else
						_objFiles = FileProvider.Instance().GetFiles(_articleID)
					End If
					_filesInit = True
				End If

				Return _objFiles

			End Get
		End Property

#End Region

#Region " Private Methods "

		Private Sub BindFiles()

			dlFiles.DataSource = AttachedFiles
			dlFiles.DataBind()

			dlFiles.Visible = (dlFiles.Items.Count > 0)
			lblNoFiles.Visible = (dlFiles.Items.Count = 0)

		End Sub

		Private Sub BindFolders()
			Dim ReadRoles As String = Null.NullString
			Dim WriteRoles As String = Null.NullString

			drpUploadFilesFolder.Items.Clear()

		    Dim folders As New List(Of IFolderInfo)
		    If ArticleSettings.DefaultFilesFolder > 0 Then
		        Dim defaultFolder as IFolderInfo = FolderManager.Instance.GetFolder(ArticleSettings.DefaultFilesFolder)
		        folders.Add(defaultFolder)
		        folders.AddRange(FolderManager.Instance.GetFolders(defaultFolder))
            Else 
                folders.AddRange(FolderManager.Instance.GetFolders(ArticleModuleBase.PortalId, False))
            End If
            Logger.Debug($"UploadFiles.BindFiles starting to iterate {folders.Count} folders.")
            For Each folder As DotNetNuke.Services.FileSystem.FolderInfo In folders
                If Not folder.IsProtected Then
                    Dim FolderItem As New ListItem()
                    If folder.FolderPath = Null.NullString Then
                        FolderItem.Text = ArticleModuleBase.GetSharedResource("Root")
                        ReadRoles = FolderPermissionController.GetFolderPermissionsCollectionByFolder(ArticleModuleBase.PortalId, "").ToString("READ")
                        WriteRoles = FolderPermissionController.GetFolderPermissionsCollectionByFolder(ArticleModuleBase.PortalId, "").ToString("WRITE")
                    Else
                        FolderItem.Text = folder.FolderPath
                        ReadRoles = FolderPermissionController.GetFolderPermissionsCollectionByFolder(ArticleModuleBase.PortalId, FolderItem.Text).ToString("READ")
                        WriteRoles = FolderPermissionController.GetFolderPermissionsCollectionByFolder(ArticleModuleBase.PortalId, FolderItem.Text).ToString("WRITE")
                    End If
                    FolderItem.Value = folder.FolderID

                    If PortalSecurity.IsInRoles(ReadRoles) OrElse PortalSecurity.IsInRoles(WriteRoles) Then
                        drpUploadFilesFolder.Items.Add(FolderItem)
                    End If
                End If
            Next
		    Logger.Debug($"UploadFiles.BindFiles done iterating {folders.Count} folders.")

            If (drpUploadFilesFolder.Items.FindByValue(ArticleSettings.DefaultFilesFolder.ToString()) IsNot Nothing) Then
				drpUploadFilesFolder.SelectedValue = ArticleSettings.DefaultFilesFolder.ToString()
			End If
		End Sub

		Protected Function GetArticleID() As String

			Return _articleID.ToString()

		End Function

		Protected Function GetMaximumFileSize() As String

			Return "20480"

		End Function

		Private Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
			Dim Generator As System.Random = New System.Random()
			Return Generator.Next(Min, Max)
		End Function

		Public Function GetResourceKey(ByVal key As String) As String

			Dim path As String = "~/DesktopModules/DnnForge - NewsArticles/App_LocalResources/UploadFiles.ascx.resx"
			Return DotNetNuke.Services.Localization.Localization.GetString(key, path)

		End Function

		Private Sub ReadQueryString()

			If (ArticleSettings.UrlModeType = Components.Types.UrlModeType.Shorterned) Then
				Try
					If (IsNumeric(Request(ArticleSettings.ShortenedID))) Then
						_articleID = Convert.ToInt32(Request(ArticleSettings.ShortenedID))
					End If
				Catch
				End Try
			End If

			If (IsNumeric(Request("ArticleID"))) Then
				_articleID = Convert.ToInt32(Request("ArticleID"))
			End If

		End Sub

		Private Sub RegisterScripts()

			DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(DotNetNuke.Framework.JavaScriptLibraries.CommonJs.jQuery)

		End Sub

		Private Sub SetLocalization()

			dshFiles.Text = GetResourceKey("Files")
			lblFilesHelp.Text = GetResourceKey("FilesHelp")

			dshExistingFiles.Text = GetResourceKey("SelectExisting")
			dshUploadFiles.Text = GetResourceKey("UploadFiles")
			dshSelectedFiles.Text = GetResourceKey("SelectedFiles")

			lblNoFiles.Text = GetResourceKey("NoFiles")

			cmdAddExistingFile.Text = GetResourceKey("cmdAddExistingFile")

			btUpload.Text = GetResourceKey("Upload")

		End Sub

#End Region

#Region " Public Methods "

		Public Sub UpdateFiles(ByVal articleID As Integer)

			For Each objFile As FileInfo In AttachedFiles
				objFile.ArticleID = articleID
				FileProvider.Instance().UpdateFile(objFile)
			Next

		End Sub

#End Region

#Region " Event Handlers "

		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

			Try
				ReadQueryString()
				SetLocalization()

			    trExisting.Visible = ArticleSettings.EnablePortalFiles

				If (ArticleSettings.IsFilesEnabled = False) Then
					Me.Visible = False
					Return
				End If

				If (IsPostBack = False) Then

					litModuleID.Text = Me.ArticleModuleBase.ModuleId.ToString()
					litTabModuleID.Text = Me.ArticleModuleBase.TabModuleId.ToString()

					If (Request.IsAuthenticated) Then
						litTicketID.Text = Request.Cookies(System.Web.Security.FormsAuthentication.FormsCookieName()).Value
					End If
					litArticleGuid.Text = ArticleGuid.ToString()

					BindFolders()
					BindFiles()
				End If

			Catch exc As Exception    'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try

		End Sub

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

			Try

				RegisterScripts()

			Catch exc As Exception    'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try

		End Sub

		Protected Sub cmdRefreshPhotos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdRefreshFiles.Click

			Try

				BindFiles()

			Catch exc As Exception    'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try

		End Sub


		Private Sub dlFiles_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlFiles.ItemDataBound

			Try

				If (e.Item.ItemType = Web.UI.WebControls.ListItemType.Item Or e.Item.ItemType = Web.UI.WebControls.ListItemType.AlternatingItem) Then

					Dim objFile As FileInfo = CType(e.Item.DataItem, FileInfo)

					Dim btnEdit As System.Web.UI.WebControls.ImageButton = CType(e.Item.FindControl("btnEdit"), System.Web.UI.WebControls.ImageButton)
					Dim btnDelete As System.Web.UI.WebControls.ImageButton = CType(e.Item.FindControl("btnDelete"), System.Web.UI.WebControls.ImageButton)
					Dim btnUp As System.Web.UI.WebControls.ImageButton = CType(e.Item.FindControl("btnUp"), System.Web.UI.WebControls.ImageButton)
					Dim btnDown As System.Web.UI.WebControls.ImageButton = CType(e.Item.FindControl("btnDown"), System.Web.UI.WebControls.ImageButton)

					If Not (btnDelete Is Nothing) Then
						btnDelete.Attributes.Add("onClick", "javascript:return confirm('" & GetResourceKey("DeleteFile") & "');")

						If Not (objFile Is Nothing) Then
							btnDelete.CommandArgument = objFile.FileID.ToString()
						End If

					End If

					If Not (btnEdit Is Nothing) Then

						If Not (objFile Is Nothing) Then
							btnEdit.CommandArgument = objFile.FileID.ToString()
						End If

					End If

					If Not (btnUp Is Nothing And btnDown Is Nothing) Then

						If (objFile.FileID = CType(AttachedFiles(0), FileInfo).FileID) Then
							btnUp.Visible = False
						End If

						If (objFile.FileID = CType(AttachedFiles(AttachedFiles.Count - 1), FileInfo).FileID) Then
							btnDown.Visible = False
						End If

						btnUp.CommandArgument = objFile.FileID.ToString()
						btnUp.CommandName = "Up"
						btnUp.CausesValidation = False

						btnDown.CommandArgument = objFile.FileID.ToString()
						btnDown.CommandName = "Down"
						btnDown.CausesValidation = False

					End If

				End If

			Catch exc As Exception    'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try

		End Sub

		Private Sub dlFiles_OnItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlFiles.ItemCommand

			Try

				If (e.CommandName = "Delete") Then

					Dim objFile As FileInfo = FileProvider.Instance().GetFile(Convert.ToInt32(e.CommandArgument))

					If Not (objFile Is Nothing) Then
						FileProvider.Instance().DeleteFile(_articleID, Convert.ToInt32(e.CommandArgument))
					End If

				End If

				If (e.CommandName = "Edit") Then

					dlFiles.EditItemIndex = e.Item.ItemIndex

				End If

				If (e.CommandName = "Up") Then

					Dim fileID As Integer = Convert.ToInt32(e.CommandArgument)

					For i As Integer = 0 To AttachedFiles.Count - 1
						Dim objFile As FileInfo = CType(AttachedFiles(i), FileInfo)
						If (fileID = objFile.FileID) Then

							Dim objFileToSwap As FileInfo = CType(AttachedFiles(i - 1), FileInfo)

							Dim sortOrder As Integer = objFile.SortOrder
							Dim sortOrderPrevious As Integer = objFileToSwap.SortOrder

							objFile.SortOrder = sortOrderPrevious
							objFileToSwap.SortOrder = sortOrder

							FileProvider.Instance().UpdateFile(objFile)
							FileProvider.Instance().UpdateFile(objFileToSwap)

						End If
					Next

				End If

				If (e.CommandName = "Down") Then

					Dim fileID As Integer = Convert.ToInt32(e.CommandArgument)

					For i As Integer = 0 To AttachedFiles.Count - 1
						Dim objFile As FileInfo = CType(AttachedFiles(i), FileInfo)
						If (fileID = objFile.FileID) Then
							Dim objFileToSwap As FileInfo = CType(AttachedFiles(i + 1), FileInfo)

							Dim sortOrder As Integer = objFile.SortOrder
							Dim sortOrderNext As Integer = objFileToSwap.SortOrder

							objFile.SortOrder = sortOrderNext
							objFileToSwap.SortOrder = sortOrder

							FileProvider.Instance().UpdateFile(objFile)
							FileProvider.Instance().UpdateFile(objFileToSwap)
						End If
					Next

				End If

				If (e.CommandName = "Cancel") Then

					dlFiles.EditItemIndex = -1

				End If

				If (e.CommandName = "Update") Then

					Dim txtTitle As TextBox = CType(e.Item.FindControl("txtTitle"), TextBox)

					Dim objFile As FileInfo = FileProvider.Instance().GetFile(Convert.ToInt32(dlFiles.DataKeys(e.Item.ItemIndex)))

					If Not (objFile Is Nothing) Then
						objFile.Title = txtTitle.Text
						FileProvider.Instance().UpdateFile(objFile)
					End If

					dlFiles.EditItemIndex = -1

				End If

				_filesInit = False
				BindFiles()

			Catch exc As Exception    'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try

		End Sub

		Protected Sub cmdAddExistingFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAddExistingFile.Click

			Try

				If (ctlFile.Url <> "") Then
					If (ctlFile.Url.ToLower().StartsWith("fileid=")) Then
						If (IsNumeric(ctlFile.Url.ToLower().Replace("fileid=", ""))) Then
							Dim fileID As Integer = Convert.ToInt32(ctlFile.Url.ToLower().Replace("fileid=", ""))
							Dim objDnnFile As DotNetNuke.Services.FileSystem.FileInfo = FileManager.Instance.GetFile(fileID, True)
							If (objDnnFile IsNot Nothing) Then

								Dim objFileController As New FileController

								Dim objFile As New FileInfo

								objFile.ArticleID = _articleID
								If (_articleID = Null.NullInteger) Then
									objFile.ArticleID = ArticleGuid
								End If
								objFile.FileName = objDnnFile.FileName
								objFile.ContentType = objDnnFile.ContentType
								objFile.SortOrder = 0
								Dim filesList As List(Of FileInfo) = objFileController.GetFileList(_articleID, ArticleGuid)
								If (filesList.Count > 0) Then
									objFile.SortOrder = CType(filesList(filesList.Count - 1), FileInfo).SortOrder + 1
								End If
								objFile.Folder = objDnnFile.Folder
								objFile.Extension = objDnnFile.Extension
								objFile.Size = objDnnFile.Size
								objFile.Title = objFile.FileName.Replace("." & objFile.Extension, "")

								objFileController.Add(objFile)
								BindFiles()

							End If
						End If
					End If
				End If

			Catch exc As Exception    'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try

		End Sub

		Protected Sub btUpload_Click(sender As Object, e As EventArgs)
			Dim objFileController As New FileController
			Dim objPortalController As New PortalController()

			For Each file As HttpPostedFile In fupFile.PostedFiles
				If objPortalController.HasSpaceAvailable(ArticleModuleBase.PortalId, file.ContentLength) Then

					Dim folderId As Integer = Integer.Parse(drpUploadFilesFolder.SelectedValue)
					If (_articleID <> Null.NullInteger) Then
						FileProvider.Instance().AddFile(_articleID, ArticleModuleBase.ModuleId, folderId, file)
					Else
						FileProvider.Instance().AddFile(ArticleGuid, ArticleModuleBase.ModuleId, folderId, file)
					End If
				End If
			Next

            DotNetNuke.Services.FileSystem.FolderManager.Instance.Synchronize(PortalController.Instance.GetCurrentPortalSettings().PortalId, drpUploadFilesFolder.SelectedItem.Text, True, True)

            BindFiles()
		End Sub

#End Region

	End Class

End Namespace