Imports System.IO

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Security

Imports Ventrian.NewsArticles.Base
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports DotNetNuke.Security.Permissions
Imports DotNetNuke.Services.FileSystem

Namespace Ventrian.NewsArticles.Controls

	Partial Public Class UploadImages
		Inherits NewsArticleControlBase

#Region " Private Members "

		Private _articleID As Integer = Null.NullInteger

		Private _imagesInit As Boolean = False
		Private _objImages As List(Of ImageInfo)

#End Region

#Region " Private Properties "

        Private Overloads ReadOnly Property ArticleModuleBase() As NewsArticleModuleBase
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

		Public Property ArticleGuid() As String
			Get
				If (_articleID = Null.NullInteger) Then
					If (litArticleGuid.Text = Null.NullString) Then
						litArticleGuid.Text = Guid.NewGuid.ToString()
					End If
				End If
				Return litArticleGuid.Text
			End Get
			Set(ByVal value As String)
				litArticleGuid.Text = value
			End Set
		End Property

		Public ReadOnly Property AttachedImages() As List(Of ImageInfo)
			Get

				If (_imagesInit = False) Then
					Dim objImageController As New ImageController
					_objImages = objImageController.GetImageList(_articleID, ArticleGuid)
					_imagesInit = True
				End If

				Return _objImages

			End Get
		End Property

		Public Property ImageExternalUrl() As String
			Get
				Return txtImageExternal.Text
			End Get
			Set(ByVal value As String)
				txtImageExternal.Text = value
			End Set
		End Property

#End Region

#Region " Private Methods "

		Private Sub BindFolders()

			Dim ReadRoles As String = Null.NullString
			Dim WriteRoles As String = Null.NullString

			drpUploadImageFolder.Items.Clear()

			Dim folders As IEnumerable(Of FolderInfo) = FolderManager.Instance.GetFolders(ArticleModuleBase.PortalId)
            For Each folder As DotNetNuke.Services.FileSystem.FolderInfo In folders
                If Not folder.IsProtected Then
                    Dim folderPermissions as FolderPermissionCollection = FolderPermissionController.GetFolderPermissionsCollectionByFolder(ArticleModuleBase.PortalId,folder.FolderPath)
                    Dim FolderItem As New ListItem()
                    If folder.FolderPath = Null.NullString Then
                        FolderItem.Text = ArticleModuleBase.GetSharedResource("Root")
                        ReadRoles = folderPermissions.ToString("READ")
                        WriteRoles = folderPermissions.ToString("WRITE")
                    Else
                        FolderItem.Text = folder.FolderPath
                        ReadRoles = folderPermissions.ToString("READ")
                        WriteRoles = folderPermissions.ToString("WRITE")
                    End If
                    FolderItem.Value = folder.FolderID

                    If PortalSecurity.IsInRoles(ReadRoles) OrElse PortalSecurity.IsInRoles(WriteRoles) Then
                        drpUploadImageFolder.Items.Add(FolderItem)
                    End If
                End If
            Next

            If (drpUploadImageFolder.Items.FindByValue(ArticleSettings.DefaultImagesFolder.ToString()) IsNot Nothing) Then
				drpUploadImageFolder.SelectedValue = ArticleSettings.DefaultImagesFolder.ToString()
			End If

		End Sub

		Private Sub BindImages()

			Dim objImageController As New ImageController()

			dlImages.DataSource = AttachedImages
			dlImages.DataBind()

			dlImages.Visible = (dlImages.Items.Count > 0)
			lblNoImages.Visible = (dlImages.Items.Count = 0)

		End Sub

		Protected Function GetArticleID() As String

			Return _articleID.ToString()

		End Function

		Protected Function GetImageUrl(ByVal objImage As ImageInfo) As String

			Dim thumbWidth As Integer = 150
			Dim thumbHeight As Integer = 150

			Dim width As Integer
			If (objImage.Width > thumbWidth) Then
				width = thumbWidth
			Else
				width = objImage.Width
			End If

			Dim height As Integer = Convert.ToInt32(objImage.Height / (objImage.Width / width))
			If (height > thumbHeight) Then
				height = thumbHeight
				width = Convert.ToInt32(objImage.Width / (objImage.Height / height))
			End If

			Dim settings As PortalSettings = PortalController.Instance.GetCurrentPortalSettings()

			Return Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(settings.HomeDirectory) & "&FileName=" & Server.UrlEncode(objImage.Folder & objImage.FileName) & "&PortalID=" & settings.PortalId.ToString() & "&q=1")

		End Function

		Protected Function GetMaximumFileSize() As String

			Return "20480"

		End Function

		Public Function GetResourceKey(ByVal key As String) As String

			Dim path As String = "~/DesktopModules/DnnForge - NewsArticles/App_LocalResources/UploadImages.ascx.resx"
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

			dshImages.Text = GetResourceKey("Images")
			lblImagesHelp.Text = GetResourceKey("ImagesHelp")

			dshExistingImages.Text = GetResourceKey("SelectExisting")
			dshUploadImages.Text = GetResourceKey("UploadImages")
			dshSelectedImages.Text = GetResourceKey("SelectedImages")
			dshExternalImage.Text = GetResourceKey("ExternalImage")

			lblNoImages.Text = GetResourceKey("NoImages")

			btUpload.Text = GetResourceKey("Upload")

		End Sub

#End Region

#Region " Public Methods "

		Public Sub UpdateImages(ByVal articleID As Integer)

			Dim objImageController As New ImageController
			For Each objImage As ImageInfo In AttachedImages
				objImage.ArticleID = articleID
				objImageController.Update(objImage)
			Next

		End Sub

#End Region

#Region " Event Handlers "

		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

			Try
				ReadQueryString()
				SetLocalization()

				trUpload.Visible = ArticleSettings.EnableImagesUpload
				trExisting.Visible = ArticleSettings.EnablePortalImages

				phImages.Visible = (trUpload.Visible Or trExisting.Visible)

				phExternalImage.Visible = ArticleSettings.EnableExternalImages

				If (ArticleSettings.IsImagesEnabled = False Or (trUpload.Visible = False And trExisting.Visible = False And phExternalImage.Visible = False)) Then
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
					BindImages()
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

		Protected Sub cmdRefreshPhotos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRefreshPhotos.Click

			Try

				BindImages()

			Catch exc As Exception    'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try

		End Sub


		Private Sub dlImages_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlImages.ItemDataBound

			Try

				If (e.Item.ItemType = Web.UI.WebControls.ListItemType.Item Or e.Item.ItemType = Web.UI.WebControls.ListItemType.AlternatingItem) Then

					Dim objImage As ImageInfo = CType(e.Item.DataItem, ImageInfo)

					Dim btnEdit As System.Web.UI.WebControls.ImageButton = CType(e.Item.FindControl("btnEdit"), System.Web.UI.WebControls.ImageButton)
					Dim btnDelete As System.Web.UI.WebControls.ImageButton = CType(e.Item.FindControl("btnDelete"), System.Web.UI.WebControls.ImageButton)
					Dim btnTop As System.Web.UI.WebControls.ImageButton = CType(e.Item.FindControl("btnTop"), System.Web.UI.WebControls.ImageButton)
					Dim btnUp As System.Web.UI.WebControls.ImageButton = CType(e.Item.FindControl("btnUp"), System.Web.UI.WebControls.ImageButton)
					Dim btnDown As System.Web.UI.WebControls.ImageButton = CType(e.Item.FindControl("btnDown"), System.Web.UI.WebControls.ImageButton)
					Dim btnBottom As System.Web.UI.WebControls.ImageButton = CType(e.Item.FindControl("btnBottom"), System.Web.UI.WebControls.ImageButton)

					If Not (btnDelete Is Nothing) Then
						btnDelete.Attributes.Add("onClick", "javascript:return confirm('" & GetResourceKey("DeleteImage") & "');")

						If Not (objImage Is Nothing) Then
							btnDelete.CommandArgument = objImage.ImageID.ToString()
						End If

					End If

					If Not (btnEdit Is Nothing) Then

						If Not (objImage Is Nothing) Then
							btnEdit.CommandArgument = objImage.ImageID.ToString()
						End If

					End If

					If Not (btnUp Is Nothing And btnDown Is Nothing) Then

						If (objImage.ImageID = CType(AttachedImages(0), ImageInfo).ImageID) Then
							btnUp.Visible = False
							btnTop.Visible = False
						End If

						If (objImage.ImageID = CType(AttachedImages(AttachedImages.Count - 1), ImageInfo).ImageID) Then
							btnDown.Visible = False
							btnBottom.Visible = False
						End If

						btnTop.CommandArgument = objImage.ImageID.ToString()
						btnTop.CommandName = "Top"
						btnTop.CausesValidation = False

						btnUp.CommandArgument = objImage.ImageID.ToString()
						btnUp.CommandName = "Up"
						btnUp.CausesValidation = False

						btnDown.CommandArgument = objImage.ImageID.ToString()
						btnDown.CommandName = "Down"
						btnDown.CausesValidation = False

						btnBottom.CommandArgument = objImage.ImageID.ToString()
						btnBottom.CommandName = "Bottom"
						btnBottom.CausesValidation = False

					End If

				End If

			Catch exc As Exception    'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try

		End Sub

		Private Sub dlImages_OnItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlImages.ItemCommand

			Try

				Dim objImageController As New ImageController

				If (e.CommandName = "Delete") Then

					Dim objImage As ImageInfo = objImageController.Get(Convert.ToInt32(e.CommandArgument))

					If Not (objImage Is Nothing) Then
						objImageController.Delete(Convert.ToInt32(e.CommandArgument), _articleID, objImage.ImageGuid)
					End If

				End If

				If (e.CommandName = "Edit") Then

					dlImages.EditItemIndex = e.Item.ItemIndex

				End If

				If (e.CommandName = "Top") Then

					Dim imageID As Integer = Convert.ToInt32(e.CommandArgument)

					Dim objImagesSorted As New List(Of ImageInfo)

					For i As Integer = 0 To AttachedImages.Count - 1
						Dim objImage As ImageInfo = CType(AttachedImages(i), ImageInfo)
						If (imageID = objImage.ImageID) Then
							objImagesSorted.Insert(0, objImage)
						Else
							objImagesSorted.Add(objImage)
						End If
					Next

					Dim sortOrder As Integer = 0
					For Each objImage As ImageInfo In objImagesSorted
						objImage.SortOrder = sortOrder
						objImageController.Update(objImage)
						sortOrder = sortOrder + 1
					Next

				End If

				If (e.CommandName = "Up") Then

					Dim imageID As Integer = Convert.ToInt32(e.CommandArgument)

					For i As Integer = 0 To AttachedImages.Count - 1
						Dim objImage As ImageInfo = CType(AttachedImages(i), ImageInfo)
						If (imageID = objImage.ImageID) Then

							Dim objImageToSwap As ImageInfo = CType(AttachedImages(i - 1), ImageInfo)

							Dim sortOrder As Integer = objImage.SortOrder
							Dim sortOrderPrevious As Integer = objImageToSwap.SortOrder

							objImage.SortOrder = sortOrderPrevious
							objImageToSwap.SortOrder = sortOrder

							objImageController.Update(objImage)
							objImageController.Update(objImageToSwap)

						End If
					Next

				End If

				If (e.CommandName = "Down") Then

					Dim imageID As Integer = Convert.ToInt32(e.CommandArgument)

					For i As Integer = 0 To AttachedImages.Count - 1
						Dim objImage As ImageInfo = CType(AttachedImages(i), ImageInfo)
						If (imageID = objImage.ImageID) Then
							Dim objImageToSwap As ImageInfo = CType(AttachedImages(i + 1), ImageInfo)

							Dim sortOrder As Integer = objImage.SortOrder
							Dim sortOrderNext As Integer = objImageToSwap.SortOrder

							objImage.SortOrder = sortOrderNext
							objImageToSwap.SortOrder = sortOrder

							objImageController.Update(objImage)
							objImageController.Update(objImageToSwap)
						End If
					Next

				End If

				If (e.CommandName = "Bottom") Then

					Dim imageID As Integer = Convert.ToInt32(e.CommandArgument)

					Dim objImageEnd As ImageInfo = Nothing
					Dim objImagesSorted As New List(Of ImageInfo)

					For i As Integer = 0 To AttachedImages.Count - 1
						Dim objImage As ImageInfo = CType(AttachedImages(i), ImageInfo)
						If (imageID = objImage.ImageID) Then
							objImageEnd = objImage
						Else
							objImagesSorted.Add(objImage)
						End If
					Next

					If (objImageEnd IsNot Nothing) Then
						objImagesSorted.Add(objImageEnd)

						Dim sortOrder As Integer = 0
						For Each objImage As ImageInfo In objImagesSorted
							objImage.SortOrder = sortOrder
							objImageController.Update(objImage)
							sortOrder = sortOrder + 1
						Next
					End If

				End If

				If (e.CommandName = "Cancel") Then

					dlImages.EditItemIndex = -1

				End If

				If (e.CommandName = "Update") Then

					Dim txtTitle As TextBox = CType(e.Item.FindControl("txtTitle"), TextBox)
					Dim txtDescription As TextBox = CType(e.Item.FindControl("txtDescription"), TextBox)

					Dim objImage As ImageInfo = objImageController.Get(Convert.ToInt32(dlImages.DataKeys(e.Item.ItemIndex)))

					If Not (objImage Is Nothing) Then
						objImage.Title = txtTitle.Text
						objImage.Description = txtDescription.Text
						objImageController.Update(objImage)
					End If

					dlImages.EditItemIndex = -1

				End If

				_imagesInit = False
				BindImages()

			Catch exc As Exception    'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try

		End Sub

		Protected Sub cmdAddExistingImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddExistingImage.Click

			Try

				If (ctlImage.Url <> "") Then
					If (ctlImage.Url.ToLower().StartsWith("fileid=")) Then
						If (IsNumeric(ctlImage.Url.ToLower().Replace("fileid=", ""))) Then
							Dim fileID As Integer = Convert.ToInt32(ctlImage.Url.ToLower().Replace("fileid=", ""))
							Dim objFile As DotNetNuke.Services.FileSystem.FileInfo = FileManager.Instance.GetFile(fileID)
							If (objFile IsNot Nothing) Then

								Dim objImageController As New ImageController
								Dim objImage As New ImageInfo

								objImage.ArticleID = _articleID
								If (_articleID = Null.NullInteger) Then
									objImage.ImageGuid = ArticleGuid
								End If
								objImage.FileName = objFile.FileName
								objImage.ContentType = objFile.ContentType
								objImage.Width = objFile.Width
								objImage.Height = objFile.Height
								objImage.SortOrder = 0
								Dim imagesList As List(Of ImageInfo) = objImageController.GetImageList(_articleID, ArticleGuid)
								If (imagesList.Count > 0) Then
									objImage.SortOrder = imagesList(imagesList.Count - 1).SortOrder + 1
								End If
								objImage.Folder = objFile.Folder
								objImage.Extension = objFile.Extension
								objImage.Title = objFile.FileName.Replace("." & objImage.Extension, "")
								objImage.Size = objFile.Size
								objImage.Description = ""

								objImageController.Add(objImage)
								BindImages()

							End If
						End If
					End If
				End If

			Catch exc As Exception    'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try

		End Sub

		Protected Sub btUpload_Click(sender As Object, e As System.EventArgs)
			Dim objImageController As New ImageController
			Dim objPortalController As New PortalController()

			For Each objFile As HttpPostedFile In fupFile.PostedFiles
				If objPortalController.HasSpaceAvailable(ArticleModuleBase.PortalId, objFile.ContentLength) Then

					Dim objImage As New ImageInfo

					objImage.ArticleID = _articleID
					If (_articleID = Null.NullInteger) Then
						objImage.ImageGuid = ArticleGuid
					End If
					objImage.FileName = objFile.FileName

					If (objFile.FileName.ToLower().EndsWith(".jpg")) Then
						objImage.ContentType = "image/jpeg"
					End If

					If (objFile.FileName.ToLower().EndsWith(".gif")) Then
						objImage.ContentType = "image/gif"
					End If

					If (objFile.FileName.ToLower().EndsWith(".png")) Then
						objImage.ContentType = "image/png"
					End If

					Dim maxWidth As Integer = ArticleSettings.MaxImageWidth
					Dim maxHeight As Integer = ArticleSettings.MaxImageHeight

					Dim photo As Drawing.Image = Drawing.Image.FromStream(objFile.InputStream)

					objImage.Width = photo.Width
					objImage.Height = photo.Height

					If (objImage.Width > maxWidth) Then
						objImage.Width = maxWidth
						objImage.Height = Convert.ToInt32(objImage.Height / (photo.Width / maxWidth))
					End If

					If (objImage.Height > maxHeight) Then
						objImage.Height = maxHeight
						objImage.Width = Convert.ToInt32(photo.Width / (photo.Height / maxHeight))
					End If

					objImage.SortOrder = 0

					Dim imagesList As List(Of ImageInfo) = objImageController.GetImageList(_articleID, ArticleGuid)

					If (imagesList.Count > 0) Then
						objImage.SortOrder = CType(imagesList(imagesList.Count - 1), ImageInfo).SortOrder + 1
					End If

					Dim objPortalSettings As PortalSettings = PortalController.Instance.GetCurrentPortalSettings()

					Dim folder As String = ""
					Dim folderId As Integer = Integer.Parse(drpUploadImageFolder.SelectedValue)

					If (folderId <> Null.NullInteger) Then
						Dim objFolder As FolderInfo = FolderManager.Instance.GetFolder(folderId)
						If (objFolder IsNot Nothing) Then
							folder = objFolder.FolderPath
						End If
					End If

					objImage.Folder = folder

					Select Case objImage.ContentType.ToLower()
						Case "image/jpeg"
							objImage.Extension = "jpg"
							Exit Select
						Case "image/gif"
							objImage.Extension = "gif"
							Exit Select
						Case "image/png"
							objImage.Extension = "png"
							Exit Select
					End Select

					objImage.Title = objFile.FileName.Replace("." & objImage.Extension, "")

					Dim filePath As String = objPortalSettings.HomeDirectoryMapPath & folder.Replace("/", "\")

					If Not (Directory.Exists(filePath)) Then
						Directory.CreateDirectory(filePath)
					End If

					If (File.Exists(filePath & objImage.FileName)) Then
						For i As Integer = 1 To 100
							If (File.Exists(filePath & i.ToString() & "_" & objImage.FileName) = False) Then
								objImage.FileName = i.ToString() & "_" & objImage.FileName
								Exit For
							End If
						Next
					End If

					objImage.Size = objFile.ContentLength
					If ((photo.Width < maxWidth And photo.Height < maxHeight) Or (ArticleSettings.ResizeImages = False)) Then
						objFile.SaveAs(filePath & objImage.FileName)
					Else
						Dim bmp As New Bitmap(objImage.Width, objImage.Height)
						Dim g As Graphics = Graphics.FromImage(DirectCast(bmp, Drawing.Image))

						g.InterpolationMode = InterpolationMode.HighQualityBicubic
						g.SmoothingMode = SmoothingMode.HighQuality
						g.PixelOffsetMode = PixelOffsetMode.HighQuality
						g.CompositingQuality = CompositingQuality.HighQuality

						g.DrawImage(photo, 0, 0, objImage.Width, objImage.Height)

						If (ArticleSettings.WatermarkEnabled And ArticleSettings.WatermarkText <> "") Then
							Dim crSize As SizeF = New SizeF
							Dim brushColor As Brush = Brushes.Yellow
							Dim fnt As Font = New Font("Verdana", 11, FontStyle.Bold)
							Dim strDirection As StringFormat = New StringFormat

							strDirection.Alignment = StringAlignment.Center
							crSize = g.MeasureString(ArticleSettings.WatermarkText, fnt)

							Dim yPixelsFromBottom As Integer = Convert.ToInt32(Convert.ToDouble(objImage.Height) * 0.05)
							Dim yPosFromBottom As Single = Convert.ToSingle((objImage.Height - yPixelsFromBottom) - (crSize.Height / 2))
							Dim xCenterOfImage As Single = Convert.ToSingle((objImage.Width / 2))

							g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

							Dim semiTransBrush2 As SolidBrush = New SolidBrush(Color.FromArgb(153, 0, 0, 0))
							g.DrawString(ArticleSettings.WatermarkText, fnt, semiTransBrush2, New PointF(xCenterOfImage + 1, yPosFromBottom + 1), strDirection)

							Dim semiTransBrush As SolidBrush = New SolidBrush(Color.FromArgb(153, 255, 255, 255))
							g.DrawString(ArticleSettings.WatermarkText, fnt, semiTransBrush, New PointF(xCenterOfImage, yPosFromBottom), strDirection)
						End If

						If (ArticleSettings.WatermarkEnabled And ArticleSettings.WatermarkImage <> "") Then
							Dim watermark As String = objPortalSettings.HomeDirectoryMapPath & ArticleSettings.WatermarkImage
							If (File.Exists(watermark)) Then
								Dim imgWatermark As Image = New Bitmap(watermark)
								Dim wmWidth As Integer = imgWatermark.Width
								Dim wmHeight As Integer = imgWatermark.Height

								Dim objImageAttributes As New ImageAttributes()
								Dim objColorMap As New ColorMap()
								objColorMap.OldColor = Color.FromArgb(255, 0, 255, 0)
								objColorMap.NewColor = Color.FromArgb(0, 0, 0, 0)
								Dim remapTable As ColorMap() = {objColorMap}
								objImageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap)

								Dim colorMatrixElements As Single()() = {New Single() {1.0F, 0.0F, 0.0F, 0.0F, 0.0F}, New Single() {0.0F, 1.0F, 0.0F, 0.0F, 0.0F}, New Single() {0.0F, 0.0F, 1.0F, 0.0F, 0.0F}, New Single() {0.0F, 0.0F, 0.0F, 0.3F, 0.0F}, New Single() {0.0F, 0.0F, 0.0F, 0.0F, 1.0F}}
								Dim wmColorMatrix As New ColorMatrix(colorMatrixElements)
								objImageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.[Default], ColorAdjustType.Bitmap)

								Dim xPosOfWm As Integer = ((objImage.Width - wmWidth) - 10)
								Dim yPosOfWm As Integer = 10

								Select Case ArticleSettings.WatermarkPosition
									Case WatermarkPosition.TopLeft
										xPosOfWm = 10
										yPosOfWm = 10
										Exit Select

									Case WatermarkPosition.TopRight
										xPosOfWm = ((objImage.Width - wmWidth) - 10)
										yPosOfWm = 10
										Exit Select

									Case WatermarkPosition.BottomLeft
										xPosOfWm = 10
										yPosOfWm = ((objImage.Height - wmHeight) - 10)

									Case WatermarkPosition.BottomRight
										xPosOfWm = ((objImage.Width - wmWidth) - 10)
										yPosOfWm = ((objImage.Height - wmHeight) - 10)
								End Select

								g.DrawImage(imgWatermark, New Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight), 0, 0, wmWidth, wmHeight,
								 GraphicsUnit.Pixel, objImageAttributes)
								imgWatermark.Dispose()
							End If
						End If

						photo.Dispose()

						Select Case objFile.ContentType.ToLower()
							Case "image/jpeg"
								Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
								Dim encoderParameters As New EncoderParameters(1)
								encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
								bmp.Save(filePath & objImage.FileName, info(1), encoderParameters)

							Case "image/gif"
								'Dim quantizer As New ImageQuantization.OctreeQuantizer(255, 8)
								'Dim bmpQuantized As Bitmap = quantizer.Quantize(bmp)
								'bmpQuantized.Save(filePath & objPhoto.Filename, ImageFormat.Gif)
								' Not working in medium trust.
								bmp.Save(filePath & objImage.FileName, ImageFormat.Gif)

							Case Else
								'Shouldn't get to here because of validators.                                
								Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
								Dim encoderParameters As New EncoderParameters(1)
								encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
								bmp.Save(filePath & objImage.FileName, info(1), encoderParameters)
						End Select

						bmp.Dispose()

						If (File.Exists(filePath & objImage.FileName)) Then
							Dim fi As New IO.FileInfo(filePath & objImage.FileName)
							If (fi IsNot Nothing) Then
								objImage.Size = Convert.ToInt32(fi.Length)
							End If
						End If
					End If

					objImage.ImageID = objImageController.Add(objImage)

					If (_articleID <> Null.NullInteger) Then
						Dim objArticleController As New ArticleController
						Dim objArticle As ArticleInfo = objArticleController.GetArticle(_articleID)
						If (objArticle IsNot Nothing) Then
							objArticle.ImageCount = objArticle.ImageCount + 1
							objArticleController.UpdateArticle(objArticle)
						End If
					End If

				End If
			Next

            DotNetNuke.Services.FileSystem.FolderManager.Instance.Synchronize(PortalController.Instance.GetCurrentPortalSettings().PortalId, drpUploadImageFolder.SelectedItem.Text, True, True)

            BindImages()
		End Sub
#End Region

	End Class

End Namespace