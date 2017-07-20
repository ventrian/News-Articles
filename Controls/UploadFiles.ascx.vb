Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Security

Imports Ventrian.NewsArticles.Base

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

            Dim folders As ArrayList = FileSystemUtils.GetFolders(ArticleModuleBase.PortalId)
            For Each folder As FolderInfo In folders
                Dim FolderItem As New ListItem()
                If folder.FolderPath = Null.NullString Then
                    FolderItem.Text = ArticleModuleBase.GetSharedResource("Root")
                    ReadRoles = FileSystemUtils.GetRoles("", ArticleModuleBase.PortalId, "READ")
                    WriteRoles = FileSystemUtils.GetRoles("", ArticleModuleBase.PortalId, "WRITE")
                Else
                    FolderItem.Text = folder.FolderPath
                    ReadRoles = FileSystemUtils.GetRoles(FolderItem.Text, ArticleModuleBase.PortalId, "READ")
                    WriteRoles = FileSystemUtils.GetRoles(FolderItem.Text, ArticleModuleBase.PortalId, "WRITE")
                End If
                FolderItem.Value = folder.FolderID

                If PortalSecurity.IsInRoles(ReadRoles) OrElse PortalSecurity.IsInRoles(WriteRoles) Then
                    drpUploadFilesFolder.Items.Add(FolderItem)
                End If
            Next

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

        Protected Function GetPostBackReference() As String

            Return Page.ClientScript.GetPostBackEventReference(cmdRefreshFiles, "Refresh")

        End Function

        Private Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
            Dim Generator As System.Random = New System.Random()
            Return Generator.Next(Min, Max)
        End Function

        Public Function GetResourceKey(ByVal key As String) As String

            Dim path As String = "~/DesktopModules/DnnForge - NewsArticles/App_LocalResources/UploadFiles.ascx.resx"
            Return DotNetNuke.Services.Localization.Localization.GetString(key, path)

        End Function

        Protected Function GetUploadUrl() As String

            Dim link As String = Page.ResolveUrl("~/DesktopModules/DnnForge%20-%20NewsArticles/Controls/SWFUploaderFiles.ashx?PortalID=" & ArticleModuleBase.PortalId.ToString())

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
            '            objCSS.Controls.Add(litLink)
            '        End If
            '        HttpContext.Current.Items.Add("NewsArticles-ScriptsRegistered", "true")
            '    End If
            'End If

        End Sub

        Private Sub SetLocalization()

            dshFiles.Text = GetResourceKey("Files")
            lblFilesHelp.Text = GetResourceKey("FilesHelp")

            dshExistingFiles.Text = GetResourceKey("SelectExisting")
            dshUploadFiles.Text = GetResourceKey("UploadFiles")
            dshSelectedFiles.Text = GetResourceKey("SelectedFiles")

            lblNoFiles.Text = GetResourceKey("NoFiles")

            cmdAddExistingFile.Text = GetResourceKey("cmdAddExistingFile")

        End Sub

#End Region

#Region " Public Methods "

        Public Sub UpdateFiles(ByVal articleID As Integer)

            For Each objFile As FileInfo In AttachedFiles
                objFile.ArticleID = articleID
                FileProvider.Instance().UpdateFile(objFile)
                ' objFileController.Update(objFile)
            Next

        End Sub

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                ReadQueryString()
                SetLocalization()

                'trUpload.Visible = ArticleSettings.EnableImagesUpload
                'trExisting.Visible = ArticleSettings.EnablePortalImages

                'phFiles.Visible = (trUpload.Visible Or trExisting.Visible)

                If (ArticleSettings.IsFilesEnabled = False) Then
                    Me.Visible = False
                    Return
                End If

                If (IsPostBack = False) Then

                    lblSelectFiles.Text = GetResourceKey("SelectFiles")
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
                        'Dim objPortalSettings As PortalSettings = PortalController.GetCurrentPortalSettings()
                        'Dim filePath As String = objPortalSettings.HomeDirectoryMapPath & objFile.Folder & objFile.FileName
                        'If (File.Exists(filePath)) Then
                        '    File.Delete(filePath)
                        'End If
                        FileProvider.Instance().DeleteFile(_articleID, Convert.ToInt32(e.CommandArgument))
                        ' objFileController.Delete(Convert.ToInt32(e.CommandArgument))
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
                            Dim objDnnFileController As New DotNetNuke.Services.FileSystem.FileController
                            Dim objDnnFile As DotNetNuke.Services.FileSystem.FileInfo = objDnnFileController.GetFileById(fileID, ArticleModuleBase.PortalId)
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

#End Region

    End Class

End Namespace