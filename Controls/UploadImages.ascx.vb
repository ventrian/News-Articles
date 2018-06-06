Imports System.IO

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Security

Imports Ventrian.NewsArticles.Base

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

            Dim folders As IEnumerable(Of IFolderInfo) = FolderManager.Instance.GetFolders(ArticleModuleBase.PortalId)
            For Each folder As IFolderInfo In folders
                Dim FolderItem As New ListItem()
                If folder.FolderPath = Null.NullString Then
                    FolderItem.Text = ArticleModuleBase.GetSharedResource("Root")
                    ReadRoles = DotNetNuke.Security.Permissions.FolderPermissionController.GetFolderPermissionsCollectionByFolder(ArticleModuleBase.PortalId, "").ToString("READ")
                    WriteRoles = DotNetNuke.Security.Permissions.FolderPermissionController.GetFolderPermissionsCollectionByFolder(ArticleModuleBase.PortalId, "").ToString("WRITE")
                Else
                    FolderItem.Text = folder.FolderPath
                    ReadRoles = DotNetNuke.Security.Permissions.FolderPermissionController.GetFolderPermissionsCollectionByFolder(ArticleModuleBase.PortalId, FolderItem.Text).ToString("READ")
                    WriteRoles = DotNetNuke.Security.Permissions.FolderPermissionController.GetFolderPermissionsCollectionByFolder(ArticleModuleBase.PortalId, FolderItem.Text).ToString("WRITE")
                End If
                FolderItem.Value = folder.FolderID

                If PortalSecurity.IsInRoles(ReadRoles) OrElse PortalSecurity.IsInRoles(WriteRoles) Then
                    drpUploadImageFolder.Items.Add(FolderItem)
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

            Dim settings As PortalSettings = PortalController.GetCurrentPortalSettings()

            Return Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(settings.HomeDirectory) & "&FileName=" & Server.UrlEncode(objImage.Folder & objImage.FileName) & "&PortalID=" & settings.PortalId.ToString() & "&q=1")

        End Function

        Protected Function GetMaximumFileSize() As String

            Return "20480"

        End Function

        Protected Function GetPostBackReference() As String

            Return Page.ClientScript.GetPostBackEventReference(cmdRefreshPhotos, "Refresh")

        End Function

        Public Function GetResourceKey(ByVal key As String) As String

            Dim path As String = "~/DesktopModules/DnnForge - NewsArticles/App_LocalResources/UploadImages.ascx.resx"
            Return DotNetNuke.Services.Localization.Localization.GetString(key, path)

        End Function

        Protected Function GetUploadUrl() As String

            Dim link As String = Page.ResolveUrl("~/DesktopModules/DnnForge%20-%20NewsArticles/Controls/SWFUploader.ashx?PortalID=" & ArticleModuleBase.PortalId.ToString())

            If (link.ToLower().StartsWith("http")) Then
                Return link
            Else
                If (Request.Url.Port = 80) Then
                    Return DotNetNuke.Common.AddHTTP(Request.Url.Host & link)
                Else
                    Return DotNetNuke.Common.AddHTTP(Request.Url.Host & ":" & Request.Url.Port.ToString() & link)
                End If
            End If

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

            DotNetNuke.Framework.jQuery.RequestRegistration()

            'If (HttpContext.Current.Items("jquery_registered") Is Nothing And HttpContext.Current.Items("jQueryRequested") Is Nothing) Then
            '    If (HttpContext.Current.Items("PropertyAgent-jQuery-ScriptsRegistered") Is Nothing And HttpContext.Current.Items("SimpleGallery-ScriptsRegistered") Is Nothing And HttpContext.Current.Items("NewsArticles-ScriptsRegistered") Is Nothing) Then
            '        Dim objCSS As System.Web.UI.Control = Page.FindControl("CSS")

            '        If Not (objCSS Is Nothing) Then
            '            Dim litLink As New Literal
            '            litLink.Text = "" & vbCrLf _
            '                & "<script type=""text/javascript"" src='" & Me.ResolveUrl("../Includes/Uploader/jquery-1.3.2.min.js") & "'></script>" & vbCrLf
            '            If (HttpContext.Current.Items("NewsArticles-ScriptsRegistered") IsNot Nothing) Then
            '                objCSS.Controls.Add(litLink)
            '            End If
            '        End If
            '        If (HttpContext.Current.Items("NewsArticles-ScriptsRegistered") IsNot Nothing) Then
            '            HttpContext.Current.Items.Add("NewsArticles-ScriptsRegistered", "true")
            '        End If
            '    End If
            'End If

        End Sub

        Private Sub SetLocalization()

            CType(dshImages, DotNetNuke.UI.UserControls.SectionHeadControl).Text = GetResourceKey("Images")
            lblImagesHelp.Text = GetResourceKey("ImagesHelp")

            CType(dshImages, DotNetNuke.UI.UserControls.SectionHeadControl).Text = GetResourceKey("SelectExisting")
            CType(dshImages, DotNetNuke.UI.UserControls.SectionHeadControl).Text = GetResourceKey("UploadImages")
            CType(dshImages, DotNetNuke.UI.UserControls.SectionHeadControl).Text = GetResourceKey("SelectedImages")
            CType(dshImages, DotNetNuke.UI.UserControls.SectionHeadControl).Text = GetResourceKey("ExternalImage")

            lblNoImages.Text = GetResourceKey("NoImages")

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
                    lblSelectImages.Text = GetResourceKey("SelectImages")
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

        Protected Sub cmdRefreshPhotos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdRefreshPhotos.Click

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

        Protected Sub cmdAddExistingImage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAddExistingImage.Click

            Try

                If (CType(ctlImage, DotNetNuke.UI.UserControls.UrlControl).Url <> "") Then
                    If (CType(ctlImage, DotNetNuke.UI.UserControls.UrlControl).Url.ToLower().StartsWith("fileid=")) Then
                        If (IsNumeric(CType(ctlImage, DotNetNuke.UI.UserControls.UrlControl).Url.ToLower().Replace("fileid=", ""))) Then
                            Dim fileID As Integer = Convert.ToInt32(CType(ctlImage, DotNetNuke.UI.UserControls.UrlControl).Url.ToLower().Replace("fileid=", ""))
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
                                    objImage.SortOrder = CType(imagesList(imagesList.Count - 1), ImageInfo).SortOrder + 1
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

#End Region

    End Class

End Namespace