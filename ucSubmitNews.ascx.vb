'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.UI.UserControls
Imports DotNetNuke.Security.Roles
Imports Ventrian.NewsArticles.Components.Social
Imports Ventrian.NewsArticles.Components.CustomFields
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Security.Permissions
Imports System.IO

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucSubmitNews
        Inherits NewsArticleModuleBase

#Region " Private Members "

        Private _articleID As Integer = Null.NullInteger
        Private _returnUrl As String = Null.NullString
        Private _richTextValues As New NameValueCollection

#End Region

#Region " Private Properties "

        Private Property PublishDate(ByVal defaultHour As Integer, ByVal defaultMinute As Integer) As DateTime
            Get
                Dim year As Integer = Convert.ToDateTime(txtPublishDate.Text).Year
                Dim month As Integer = Convert.ToDateTime(txtPublishDate.Text).Month
                Dim day As Integer = Convert.ToDateTime(txtPublishDate.Text).Day

                Dim hour As Integer = defaultHour
                If (IsNumeric(txtPublishHour.Text)) Then
                    If (hour >= 0 And hour <= 23) Then
                        hour = Convert.ToInt32(txtPublishHour.Text)
                    End If
                End If

                Dim minute As Integer = defaultMinute
                If (IsNumeric(txtPublishMinute.Text)) Then
                    If (minute >= 0 And minute <= 60) Then
                        minute = Convert.ToInt32(txtPublishMinute.Text)
                    End If
                End If

                Return New DateTime(year, month, day, hour, minute, 0)
            End Get
            Set(ByVal Value As DateTime)
                txtPublishHour.Text = Value.Hour.ToString()
                txtPublishMinute.Text = Value.Minute.ToString()
                txtPublishDate.Text = New DateTime(Value.Year, Value.Month, Value.Day).ToShortDateString()
            End Set
        End Property

        Private Property ExpiryDate(ByVal defaultHour As Integer, ByVal defaultMinute As Integer) As DateTime
            Get
                If (txtExpiryDate.Text.Length = 0) Then
                    Return Null.NullDate
                End If

                Dim year As Integer = Convert.ToDateTime(txtExpiryDate.Text).Year
                Dim month As Integer = Convert.ToDateTime(txtExpiryDate.Text).Month
                Dim day As Integer = Convert.ToDateTime(txtExpiryDate.Text).Day

                Dim hour As Integer = defaultHour
                If (IsNumeric(txtExpiryHour.Text)) Then
                    If (hour >= 0 And hour <= 23) Then
                        hour = Convert.ToInt32(txtExpiryHour.Text)
                    End If
                End If

                Dim minute As Integer = defaultMinute
                If (IsNumeric(txtExpiryMinute.Text)) Then
                    If (minute >= 0 And minute <= 60) Then
                        minute = Convert.ToInt32(txtExpiryMinute.Text)
                    End If
                End If

                Return New DateTime(year, month, day, hour, minute, 0)
            End Get
            Set(ByVal Value As DateTime)
                If (Value = Null.NullDate) Then
                    txtExpiryDate.Text = ""
                    Return
                End If
                txtExpiryHour.Text = Value.Hour.ToString()
                txtExpiryMinute.Text = Value.Minute.ToString()
                txtExpiryDate.Text = New DateTime(Value.Year, Value.Month, Value.Day).ToShortDateString()
            End Set
        End Property

        Private ReadOnly Property Details() As TextEditor
            Get
                Return CType(txtDetails, TextEditor)
            End Get
        End Property

        Private ReadOnly Property UrlLink() As UrlControl
            Get
                Return CType(ctlUrlLink, UrlControl)
            End Get
        End Property

        Private ReadOnly Property ExcerptRich() As DotNetNuke.UI.UserControls.TextEditor
            Get
                Return CType(txtExcerptRich, TextEditor)
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Function IsInRole(ByVal roleName As String, ByVal roles As String()) As Boolean

            For Each role As String In roles
                If (roleName = role) Then
                    Return True
                End If
            Next

            Return False

        End Function

        Private Sub BindArticle()

            If (_articleID <> Null.NullInteger) Then

                Dim objArticleController As ArticleController = New ArticleController

                Dim objArticle As ArticleInfo = objArticleController.GetArticle(_articleID)

                lblAuthor.Text = objArticle.AuthorDisplayName
                txtTitle.Text = objArticle.Title
                If (ArticleSettings.TextEditorSummaryMode = Components.Types.TextEditorModeType.Basic) Then
                    txtExcerptBasic.Text = objArticle.Summary.Replace("&lt;br /&gt;", vbCrLf)
                Else
                    ExcerptRich.Text = objArticle.Summary
                End If
                chkFeatured.Checked = objArticle.IsFeatured
                chkSecure.Checked = objArticle.IsSecure

                txtMetaTitle.Text = objArticle.MetaTitle
                txtMetaDescription.Text = objArticle.MetaDescription
                txtMetaKeyWords.Text = objArticle.MetaKeywords
                txtPageHeadText.Text = objArticle.PageHeadText

                If Not (drpStatus.Items.FindByValue(objArticle.Status.ToString()) Is Nothing) Then
                    drpStatus.SelectedValue = objArticle.Status.ToString()
                End If

                PublishDate(objArticle.StartDate.Hour, objArticle.EndDate.Minute) = objArticle.StartDate

                If Not (objArticle.EndDate = Null.NullDate) Then
                    ExpiryDate(objArticle.EndDate.Hour, objArticle.EndDate.Minute) = objArticle.EndDate
                End If

                If (ArticleSettings.IsImagesEnabled) Then
                    If Not (objArticle.ImageUrl = Null.NullString) Then
                        If (objArticle.ImageUrl.ToLower().StartsWith("http://") Or objArticle.ImageUrl.ToLower().StartsWith("https://")) Then
                            If (ArticleSettings.EnableExternalImages) Then
                                ucUploadImages.ImageExternalUrl = objArticle.ImageUrl
                            End If
                        End If
                    End If
                End If

                UrlLink.Url = objArticle.Url
                chkNewWindow.Checked = objArticle.IsNewWindow

                Dim categories As ArrayList = objArticleController.GetArticleCategories(_articleID)
                For Each category As CategoryInfo In categories
                    Dim li As ListItem = lstCategories.Items.FindByValue(category.CategoryID.ToString())
                    If Not (li Is Nothing) Then
                        li.Selected = True
                    End If
                Next

                If ArticleSettings.UseStaticTagsList Then
                    SelectAllTags(objArticle.Tags)
                    CreateFinalTags(objArticle.Tags)
                Else
                    txtTags.Text = objArticle.Tags
                End If


                Dim objPageController As New PageController
                Dim pages As ArrayList = objPageController.GetPageList(_articleID)
                If (pages.Count > 0) Then
                    Details.Text = CType(pages(0), PageInfo).PageText
                End If

                cmdDelete.Visible = True
                cmdDelete.OnClientClick = "return confirm('" & DotNetNuke.Services.Localization.Localization.GetString("Delete.Text", LocalResourceFile) & "');"

                Dim objMirrorArticleController As New MirrorArticleController
                Dim objMirrorArticleInfo As MirrorArticleInfo = objMirrorArticleController.GetMirrorArticle(_articleID)

                If (objMirrorArticleInfo IsNot Nothing) Then

                    phMirrorText.Visible = True
                    If (objMirrorArticleInfo.AutoUpdate) Then
                        lblMirrorText.Text = Localization.GetString("MirrorTextUpdate", Me.LocalResourceFile)

                        If (lblMirrorText.Text.IndexOf("{0}") <> -1) Then
                            lblMirrorText.Text = lblMirrorText.Text.Replace("{0}", objMirrorArticleInfo.PortalName)
                        End If

                        phCreate.Visible = False
                        phOrganize.Visible = False
                        phPublish.Visible = False

                        cmdPublishArticle.Visible = False
                        cmdAddEditPages.Visible = False
                    Else
                        lblMirrorText.Text = Localization.GetString("MirrorText", Me.LocalResourceFile)

                        If (lblMirrorText.Text.IndexOf("{0}") <> -1) Then
                            lblMirrorText.Text = lblMirrorText.Text.Replace("{0}", objMirrorArticleInfo.PortalName)
                        End If
                    End If

                End If

                Dim objMirrorArticleLinked As ArrayList = objMirrorArticleController.GetMirrorArticleList(_articleID)

                If (objMirrorArticleLinked.Count > 0) Then

                    phMirrorText.Visible = True
                    lblMirrorText.Text = Localization.GetString("MirrorTextLinked", Me.LocalResourceFile)

                    If (lblMirrorText.Text.IndexOf("{0}") <> -1) Then
                        lblMirrorText.Text = lblMirrorText.Text.Replace("{0}", objMirrorArticleLinked.Count.ToString())
                    End If

                End If

            Else

                chkFeatured.Checked = ArticleSettings.IsAutoFeatured
                chkSecure.Checked = ArticleSettings.IsAutoSecured
                If (ArticleSettings.AuthorDefault <> Null.NullInteger) Then
                    Dim objUser As UserInfo = UserController.Instance.GetUser(PortalId, ArticleSettings.AuthorDefault)

                    If (objUser IsNot Nothing) Then
                        lblAuthor.Text = objUser.Username
                    Else
                        lblAuthor.Text = Me.UserInfo.Username
                    End If
                Else
                    lblAuthor.Text = Me.UserInfo.Username
                End If
                PublishDate(DateTime.Now.Hour, DateTime.Now.Minute) = DateTime.Now
                cmdDelete.Visible = False

                If (Settings.Contains(ArticleConstants.DEFAULT_CATEGORIES_SETTING)) Then
                    If Not (Settings(ArticleConstants.DEFAULT_CATEGORIES_SETTING).ToString = Null.NullString) Then
                        Dim categories As String() = Settings(ArticleConstants.DEFAULT_CATEGORIES_SETTING).ToString().Split(Char.Parse(","))

                        For Each category As String In categories
                            If Not (lstCategories.Items.FindByValue(category) Is Nothing) Then
                                lstCategories.Items.FindByValue(category).Selected = True
                            End If
                        Next
                    End If
                End If

            End If

        End Sub

        Private Sub BindCategories()

            Dim objCategoryController As CategoryController = New CategoryController

            Dim objCategoriesSelected As New List(Of CategoryInfo)
            Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(ModuleId, Null.NullInteger, ArticleSettings.CategorySortType)

            If (ArticleSettings.CategoryFilterSubmit) Then
                If (ArticleSettings.FilterSingleCategory <> Null.NullInteger) Then
                    Dim objSelectedCategories As New List(Of CategoryInfo)()
                    For Each objCategory As CategoryInfo In objCategories
                        If (objCategory.CategoryID = ArticleSettings.FilterSingleCategory) Then
                            objSelectedCategories.Add(objCategory)
                            Exit For
                        End If
                    Next
                    objCategories = objSelectedCategories
                End If

                If (ArticleSettings.FilterCategories IsNot Nothing) Then
                    If (ArticleSettings.FilterCategories.Length > 0) Then
                        Dim objSelectedCategories As New List(Of CategoryInfo)()
                        For Each i As Integer In ArticleSettings.FilterCategories
                            For Each objCategory As CategoryInfo In objCategories
                                If (objCategory.CategoryID = i) Then
                                    objSelectedCategories.Add(objCategory)
                                    Exit For
                                End If
                            Next
                        Next
                        objCategories = objSelectedCategories
                    End If
                End If
            End If

            For Each objCategory As CategoryInfo In objCategories
                If (objCategory.InheritSecurity) Then
                    objCategoriesSelected.Add(objCategory)
                Else
                    If (Request.IsAuthenticated) Then
                        If (Settings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_SUBMIT_SETTING)) Then
                            If (PortalSecurity.IsInRoles(Settings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_SUBMIT_SETTING).ToString())) Then
                                objCategoriesSelected.Add(objCategory)
                            End If
                        End If
                    End If
                End If
            Next

            lstCategories.DataSource = objCategoriesSelected
            lstCategories.DataBind()

            If (Settings.Contains(ArticleConstants.REQUIRE_CATEGORY)) Then
                valCategory.Enabled = Convert.ToBoolean(Settings(ArticleConstants.REQUIRE_CATEGORY).ToString())
            End If

        End Sub

        Private Sub BindTags()

            If ArticleSettings.UseStaticTagsList Then
                tdTxtTagsTitle.Visible = False
                tdTxtTags.Visible = False
                FillTagsList()
            Else
                tdAllTagsTitle.Visible = False
                tdAllTagsList.Visible = False
                tdStaticTagsList.Visible = False
            End If

        End Sub

        Private Sub FillTagsList()

            Dim objTagController As New TagController
            Dim objTags As ArrayList = objTagController.List(ModuleId, Null.NullInteger)

            objTags.Sort()
            lstTags.DataSource = objTags
            lstTags.DataBind()

        End Sub

        Private Sub SelectAllTags(ByVal tagList As String)

            Dim objTagController As New TagController
            For Each tag As String In tagList.Split(New Char() {","c}, StringSplitOptions.RemoveEmptyEntries)
                Dim objTag As TagInfo = objTagController.Get(ModuleId, tag)

                If objTag IsNot Nothing Then
                    Dim li As ListItem = lstTags.Items.FindByValue(objTag.Name)
                    If li IsNot Nothing Then
                        li.Selected = True
                    End If
                End If
            Next

        End Sub

        Private Sub CreateFinalTags(ByVal tagList As String)

            Dim objTagController As New TagController
            For Each tag As String In tagList.Split(New Char() {","c}, StringSplitOptions.RemoveEmptyEntries)
                Dim objTag As TagInfo = objTagController.Get(ModuleId, tag)

                If objTag IsNot Nothing Then
                    lstFinalTags.Items.Add(objTag.Name)
                End If
            Next

        End Sub

        Private Sub BindCustomFields()

            Dim objCustomFieldController As New CustomFieldController()
            Dim objCustomFields As ArrayList = objCustomFieldController.List(Me.ModuleId)

            If (objCustomFields.Count = 0) Then
                phCustomFields.Visible = False
            Else
                phCustomFields.Visible = True
                rptCustomFields.DataSource = objCustomFields
                rptCustomFields.DataBind()
            End If

        End Sub

        Private Sub BindStatus()

            For Each value As Integer In System.Enum.GetValues(GetType(StatusType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(StatusType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(StatusType), value), Me.LocalResourceFile)
                drpStatus.Items.Add(li)
            Next

        End Sub

        Private Sub CheckSecurity()

            If (HasEditRights(_articleID, Me.ModuleId, Me.TabId) = False) Then
                Response.Redirect(Common.GetModuleLink(Me.TabId, Me.ModuleId, "NotAuthorized", ArticleSettings), True)
            End If

        End Sub

        Public Function GetAuthorList(ByVal moduleID As Integer) As ArrayList

            Dim moduleSettings As Hashtable = Common.GetModuleSettings(moduleID)
            Dim distributionList As String = ""
            Dim userList As New ArrayList

            If (moduleSettings.Contains(ArticleConstants.PERMISSION_SUBMISSION_SETTING)) Then

                Dim roles As String = moduleSettings(ArticleConstants.PERMISSION_SUBMISSION_SETTING).ToString()
                Dim rolesArray() As String = roles.Split(Convert.ToChar(";"))
                Dim userIDs As New Hashtable

                For Each role As String In rolesArray
                    If (role.Length > 0) Then

                        Dim objRoleController As New DotNetNuke.Security.Roles.RoleController
                        Dim objRole As DotNetNuke.Security.Roles.RoleInfo = objRoleController.GetRoleByName(PortalSettings.PortalId, role)

                        If Not (objRole Is Nothing) Then
                            Dim objUsers As List(Of UserInfo) = RoleController.Instance.GetUsersByRole(PortalSettings.PortalId, objRole.RoleName)
                            For Each objUser As UserInfo In objUsers
                                If (userIDs.Contains(objUser.UserID) = False) Then
                                    Dim objUserController As DotNetNuke.Entities.Users.UserController = New DotNetNuke.Entities.Users.UserController
                                    Dim objSelectedUser As DotNetNuke.Entities.Users.UserInfo = objUserController.GetUser(PortalSettings.PortalId, objUser.UserID)
                                    If Not (objSelectedUser Is Nothing) Then
                                        If (objSelectedUser.Email.Length > 0) Then
                                            userIDs.Add(objUser.UserID, objUser.UserID)
                                            userList.Add(objSelectedUser)
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                Next

            End If

            Return userList

        End Function

        Private Sub PopulateAuthorList()

            ddlAuthor.DataSource = GetAuthorList(Me.ModuleId)
            ddlAuthor.DataBind()
            ddlAuthor.Items.Insert(0, New ListItem(Localization.GetString("SelectAuthor.Text", Me.LocalResourceFile), "-1"))

        End Sub

        Private Sub ReadQueryString()

            If (Request("ArticleID") <> "") Then
                If (IsNumeric(Request("ArticleID"))) Then
                    _articleID = Convert.ToInt32(Request("ArticleID"))
                End If
            End If

            If (Request("ReturnUrl") <> "") Then
                _returnUrl = Request("ReturnUrl")
            End If

        End Sub

        Private Function SaveArticle() As Integer

            If (_articleID <> Null.NullInteger) Then
                Dim objArticleController As New ArticleController
                Dim objArticle As ArticleInfo = objArticleController.GetArticle(_articleID)

                If Not (objArticle Is Nothing) Then
                    Return SaveArticle(objArticle.Status)
                End If
            Else
                Return SaveArticle(StatusType.Draft)
            End If

        End Function

        Private Function SaveArticle(ByVal status As StatusType) As Integer

            Dim objArticleController As ArticleController = New ArticleController
            Dim objArticle As ArticleInfo = New ArticleInfo

            Dim statusChanged As Boolean = False
            Dim publishArticle As Boolean = False

            If (_articleID = Null.NullInteger) Then
                If (pnlAuthor.Visible = False) Then
                    Dim objUser As UserInfo = UserController.GetUserByName(Me.PortalId, lblAuthor.Text)

                    If (objUser IsNot Nothing) Then
                        objArticle.AuthorID = objUser.UserID
                    Else
                        objArticle.AuthorID = Me.UserId
                    End If
                Else
                    Dim objUser As UserInfo = UserController.GetUserByName(Me.PortalId, txtAuthor.Text)

                    If (objUser IsNot Nothing) Then
                        objArticle.AuthorID = objUser.UserID
                    Else
                        objArticle.AuthorID = Me.UserId
                    End If
                End If

                If (ddlAuthor.Visible) Then
                    objArticle.AuthorID = Convert.ToInt32(ddlAuthor.SelectedValue)
                End If
                objArticle.CreatedDate = DateTime.Now
                objArticle.Status = StatusType.Draft
                objArticle.CommentCount = 0
                objArticle.RatingCount = 0
                objArticle.Rating = 0
                objArticle.ShortUrl = ""
            Else
                objArticle = objArticleController.GetArticle(_articleID)
                objArticleController.DeleteArticleCategories(_articleID)

                If (objArticle.Status <> StatusType.Published And status = StatusType.Published) Then
                    ' Article now approved, notify if not an Approver.
                    If (objArticle.AuthorID <> Me.UserId) Then
                        statusChanged = True
                    End If
                End If

                If (pnlAuthor.Visible) Then
                    Dim objUser As UserInfo = UserController.GetUserByName(Me.PortalId, txtAuthor.Text)

                    If (objUser IsNot Nothing) Then
                        objArticle.AuthorID = objUser.UserID
                    End If
                End If

                If (ddlAuthor.Visible) Then
                    objArticle.AuthorID = Convert.ToInt32(ddlAuthor.SelectedValue)
                End If
            End If

            objArticle.MetaTitle = txtMetaTitle.Text.Trim()
            objArticle.MetaDescription = txtMetaDescription.Text.Trim()
            objArticle.MetaKeywords = txtMetaKeyWords.Text.Trim()
            objArticle.PageHeadText = txtPageHeadText.Text.Trim()

            If (chkMirrorArticle.Checked And drpMirrorArticle.Items.Count > 0) Then
                Dim objLinkedArticle As ArticleInfo = objArticleController.GetArticle(Convert.ToInt32(drpMirrorArticle.SelectedValue))

                If (objLinkedArticle IsNot Nothing) Then
                    objArticle.Title = objLinkedArticle.Title
                    objArticle.Summary = objLinkedArticle.Summary
                    objArticle.Url = objLinkedArticle.Url
                    objArticle.IsNewWindow = objLinkedArticle.IsNewWindow
                    objArticle.ImageUrl = objLinkedArticle.ImageUrl

                    objArticle.MetaTitle = objLinkedArticle.MetaTitle
                    objArticle.MetaDescription = objLinkedArticle.MetaDescription
                    objArticle.MetaKeywords = objLinkedArticle.MetaKeywords
                    objArticle.PageHeadText = objLinkedArticle.PageHeadText
                End If
            Else
                objArticle.Title = txtTitle.Text
                If (ArticleSettings.TextEditorSummaryMode = Components.Types.TextEditorModeType.Basic) Then
                    objArticle.Summary = txtExcerptBasic.Text.Replace(vbCrLf, "&lt;br /&gt;")
                Else
                    If (ExcerptRich.Text <> "" AndAlso StripHtml(ExcerptRich.Text).Length > 0 AndAlso ExcerptRich.Text <> "&lt;p&gt;&amp;#160;&lt;/p&gt;") Then
                        objArticle.Summary = ExcerptRich.Text
                    Else
                        objArticle.Summary = Null.NullString
                    End If
                End If
                objArticle.Url = UrlLink.Url
                objArticle.IsNewWindow = chkNewWindow.Checked
                If (ArticleSettings.IsImagesEnabled = True) Then
                    objArticle.ImageUrl = ucUploadImages.ImageExternalUrl
                End If
            End If

            objArticle.IsFeatured = chkFeatured.Checked
            objArticle.IsSecure = chkSecure.Checked
            objArticle.LastUpdate = DateTime.Now
            objArticle.LastUpdateID = Me.UserId
            objArticle.ModuleID = Me.ModuleId

            objArticle.Status = status

            If (objArticle.StartDate <> Null.NullDate) Then
                objArticle.StartDate = PublishDate(objArticle.StartDate.Hour, objArticle.StartDate.Minute)
            Else
                objArticle.StartDate = PublishDate(DateTime.Now.Hour, DateTime.Now.Minute)
            End If

            If (ExpiryDate(0, 0) = Null.NullDate) Then
                objArticle.EndDate = Null.NullDate
            Else
                If (objArticle.EndDate <> Null.NullDate) Then
                    objArticle.EndDate = ExpiryDate(objArticle.EndDate.Hour, objArticle.EndDate.Minute)
                Else
                    objArticle.EndDate = ExpiryDate(0, 0)
                End If
            End If

            If (_articleID = Null.NullInteger) Then
                objArticle.ArticleID = objArticleController.AddArticle(objArticle)
                ucUploadImages.UpdateImages(objArticle.ArticleID)
                ucUploadFiles.UpdateFiles(objArticle.ArticleID)
                objArticleController.UpdateArticle(objArticle)

                If (chkMirrorArticle.Checked And drpMirrorArticle.Items.Count > 0) Then

                    ' Mirrored Article
                    Dim objMirrorArticleInfo As New MirrorArticleInfo

                    objMirrorArticleInfo.ArticleID = objArticle.ArticleID
                    objMirrorArticleInfo.LinkedArticleID = Convert.ToInt32(drpMirrorArticle.SelectedValue)
                    objMirrorArticleInfo.LinkedPortalID = Convert.ToInt32(drpMirrorModule.SelectedValue.Split("-"c)(0))
                    objMirrorArticleInfo.AutoUpdate = chkMirrorAutoUpdate.Checked

                    Dim objMirrorArticleController As New MirrorArticleController()
                    objMirrorArticleController.AddMirrorArticle(objMirrorArticleInfo)

                    'Copy Files
                    Dim folderLinked As String = ""

                    Dim objModuleController As New ModuleController()
                    Dim objSettingsLinked As Hashtable = Common.GetModuleSettings(drpMirrorModule.SelectedValue.Split("-"c)(1))

                    If (objSettingsLinked.ContainsKey(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING)) Then
                        If (IsNumeric(objSettingsLinked(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING))) Then
                            Dim folderID As Integer = Convert.ToInt32(objSettingsLinked(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING))
                            Dim objFolder As FolderInfo = FolderManager.Instance.GetFolder(folderID)
                            If (objFolder IsNot Nothing) Then
                                folderLinked = objFolder.FolderPath
                            End If
                        End If
                    End If

                    Dim objPortalController As New PortalController
                    Dim objPortalLinked As PortalInfo = objPortalController.GetPortal(Convert.ToInt32(drpMirrorModule.SelectedValue.Split("-"c)(0)))

                    Dim filePathLinked As String = objPortalLinked.HomeDirectoryMapPath & folderLinked.Replace("/", "\")

                    Dim folder As String = ""

                    Dim objSettings As Hashtable = Common.GetModuleSettings(ModuleId)

                    If (objSettings.ContainsKey(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING)) Then
                        If (IsNumeric(objSettings(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING))) Then
                            Dim folderID As Integer = Convert.ToInt32(objSettings(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING))
                            Dim objFolder As FolderInfo = FolderManager.Instance.GetFolder(folderID)
                            If (objFolder IsNot Nothing) Then
                                folder = objFolder.FolderPath
                            End If
                        End If
                    End If

                    Dim objFileController As New FileController
                    Dim objFiles As List(Of FileInfo) = objFileController.GetFileList(objMirrorArticleInfo.LinkedArticleID, Null.NullString)

                    For Each objFile As FileInfo In objFiles

                        If (File.Exists(filePathLinked & objFile.FileName)) Then

                            Dim finalCopyPath = filePathLinked & objFile.FileName

                            Dim filePath As String = PortalSettings.HomeDirectoryMapPath & folder.Replace("/", "\")

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

                            File.Copy(finalCopyPath, filePath & objFile.FileName)

                            objFile.ArticleID = objArticle.ArticleID
                            objFileController.Add(objFile)

                        End If

                    Next

                    'Copy Images

                    Dim folderImagesLinked As String = ""

                    If (objSettingsLinked.ContainsKey(ArticleConstants.DEFAULT_IMAGES_FOLDER_SETTING)) Then
                        If (IsNumeric(objSettingsLinked(ArticleConstants.DEFAULT_IMAGES_FOLDER_SETTING))) Then
                            Dim folderID As Integer = Convert.ToInt32(objSettingsLinked(ArticleConstants.DEFAULT_IMAGES_FOLDER_SETTING))
                            Dim objFolder As FolderInfo = FolderManager.Instance.GetFolder(folderID)
                            If (objFolder IsNot Nothing) Then
                                folderImagesLinked = objFolder.FolderPath
                            End If
                        End If
                    End If

                    Dim filePathImagesLinked As String = objPortalLinked.HomeDirectoryMapPath & folderImagesLinked.Replace("/", "\")

                    Dim folderImages As String = ""

                    If (objSettings.ContainsKey(ArticleConstants.DEFAULT_IMAGES_FOLDER_SETTING)) Then
                        If (IsNumeric(objSettings(ArticleConstants.DEFAULT_IMAGES_FOLDER_SETTING))) Then
                            Dim folderID As Integer = Convert.ToInt32(objSettings(ArticleConstants.DEFAULT_IMAGES_FOLDER_SETTING))
                            Dim objFolder As FolderInfo = FolderManager.Instance.GetFolder(folderID)
                            If (objFolder IsNot Nothing) Then
                                folderImages = objFolder.FolderPath
                            End If
                        End If
                    End If

                    Dim objImageController As New ImageController
                    Dim objImages As List(Of ImageInfo) = objImageController.GetImageList(objMirrorArticleInfo.LinkedArticleID, Null.NullString)

                    For Each objImage As ImageInfo In objImages

                        Dim objNewImage As ImageInfo = objImage.Clone

                        If (File.Exists(filePathImagesLinked & objNewImage.FileName)) Then

                            Dim finalCopyPath = filePathImagesLinked & objNewImage.FileName

                            Dim filePath As String = PortalSettings.HomeDirectoryMapPath & folderImages.Replace("/", "\")

                            If Not (Directory.Exists(filePath)) Then
                                Directory.CreateDirectory(filePath)
                            End If

                            If (File.Exists(filePath & objNewImage.FileName)) Then
                                For i As Integer = 1 To 100
                                    If (File.Exists(filePath & i.ToString() & "_" & objNewImage.FileName) = False) Then
                                        objNewImage.FileName = i.ToString() & "_" & objNewImage.FileName
                                        Exit For
                                    End If
                                Next
                            End If

                            File.Copy(finalCopyPath, filePath & objNewImage.FileName)

                            objNewImage.ImageID = Null.NullInteger
                            objNewImage.Folder = folderImages
                            objNewImage.ArticleID = objArticle.ArticleID
                            objImageController.Add(objNewImage)

                        End If

                    Next

                End If

                objArticleController.UpdateArticle(objArticle)
                If (objArticle.Status = StatusType.Published) Then
                    publishArticle = True
                End If
            Else
                objArticleController.UpdateArticle(objArticle)
                If (statusChanged) Then
                    publishArticle = True
                End If
            End If

            SaveCategories(objArticle.ArticleID)
            SaveTags(objArticle.ArticleID)
            SaveDetails(objArticle.ArticleID, objArticle.Title)
            SaveCustomFields(objArticle.ArticleID)

            ' Re-init.
            objArticle = objArticleController.GetArticle(objArticle.ArticleID)

            If (publishArticle) Then
                If (objArticle.IsApproved) Then
                    If (ArticleSettings.JournalIntegration) Then
                        Dim objJournal As New Journal
                        objJournal.AddArticleToJournal(objArticle, PortalId, TabId, Me.UserId, Null.NullInteger, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False))
                    End If

                    If (ArticleSettings.JournalIntegrationGroups) Then

                        Dim objCategories As ArrayList = objArticleController.GetArticleCategories(objArticle.ArticleID)

                        If (objCategories.Count > 0) Then

                            Dim objRoleController As New RoleController()

                            Dim objRoles As IList(Of RoleInfo) = RoleController.Instance.GetRoles(PortalId)
                            For Each objRole As RoleInfo In objRoles
                                Dim roleAccess As Boolean = False

                                If (objRole.SecurityMode = SecurityMode.SocialGroup Or objRole.SecurityMode = SecurityMode.Both) Then

                                    For Each objCategory As CategoryInfo In objCategories

                                        If (objCategory.InheritSecurity = False) Then

                                            If (objCategory.CategorySecurityType = CategorySecurityType.Loose) Then
                                                roleAccess = False
                                                Exit For
                                            Else
                                                If (Settings.Contains(objCategory.CategoryID.ToString() & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                                    If (IsInRole(objRole.RoleName, Settings(objCategory.CategoryID.ToString() & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString().Split(";"c))) Then
                                                        roleAccess = True
                                                    End If
                                                End If
                                            End If

                                        End If

                                    Next

                                End If

                                If (roleAccess) Then
                                    Dim objJournal As New Journal
                                    objJournal.AddArticleToJournal(objArticle, PortalId, TabId, Me.UserId, objRole.RoleID, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False))
                                End If

                            Next

                        End If

                    End If

                    ' Notify Smart Thinker

                    If (ArticleSettings.EnableSmartThinkerStoryFeed) Then
                        Dim objStoryFeed As New wsStoryFeed.StoryFeedWS
                        objStoryFeed.Url = AddHTTP(Request.ServerVariables("HTTP_HOST") & Me.ResolveUrl("~/DesktopModules/Smart-Thinker%20-%20UserProfile/StoryFeed.asmx"))

                        Dim val As String = GetSharedResource("StoryFeed-AddArticle")

                        val = val.Replace("[AUTHOR]", objArticle.AuthorDisplayName)
                        val = val.Replace("[AUTHORID]", objArticle.AuthorID.ToString())
                        val = val.Replace("[ARTICLELINK]", Common.GetArticleLink(objArticle, Me.PortalSettings.ActiveTab, ArticleSettings, False))
                        val = val.Replace("[ARTICLETITLE]", objArticle.Title)

                        Try
                            objStoryFeed.AddAction(80, _articleID, val, objArticle.AuthorID, "VE6457624576460436531768")
                        Catch
                        End Try
                    End If

                    If (ArticleSettings.EnableActiveSocialFeed) Then
                        If (ArticleSettings.ActiveSocialSubmitKey <> "") Then
                            If IO.File.Exists(HttpContext.Current.Server.MapPath("~/bin/active.modules.social.dll")) Then
                                Dim ai As Object = Nothing
                                Dim asm As System.Reflection.Assembly
                                Dim ac As Object = Nothing
                                Try
                                    asm = System.Reflection.Assembly.Load("Active.Modules.Social")
                                    ac = asm.CreateInstance("Active.Modules.Social.API.Journal")
                                    If Not ac Is Nothing Then
                                        ac.AddProfileItem(New Guid(ArticleSettings.ActiveSocialSubmitKey), objArticle.AuthorID, Common.GetArticleLink(objArticle, Me.PortalSettings.ActiveTab, ArticleSettings, False), objArticle.Title, objArticle.Summary, objArticle.Body, 1, "")
                                    End If
                                Catch ex As Exception
                                End Try
                            End If
                        End If
                    End If

                End If
            End If

            If (_articleID <> Null.NullInteger And statusChanged = False) Then

                If (objArticle.Status = StatusType.Published) Then
                    ' Check to see if any articles have been linked

                    Dim objEmailTemplateController As New EmailTemplateController

                    Dim objMirrorArticleController As New MirrorArticleController
                    Dim objMirrorArticleLinked As ArrayList = objMirrorArticleController.GetMirrorArticleList(_articleID)

                    Dim objEventLog As New DotNetNuke.Services.Log.EventLog.EventLogController
                    objEventLog.AddLog("Article Linked Update", objMirrorArticleLinked.Count.ToString(), PortalSettings, -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)

                    If (objMirrorArticleLinked.Count > 0) Then

                        For Each objMirrorArticleInfo As MirrorArticleInfo In objMirrorArticleLinked

                            Dim objArticleMirrored As ArticleInfo = objArticleController.GetArticle(objMirrorArticleInfo.ArticleID)

                            If (objArticleMirrored IsNot Nothing) Then

                                If (objArticleMirrored.AuthorEmail <> "") Then
                                    objEmailTemplateController.SendFormattedEmail(Me.ModuleId, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False), objArticle, EmailTemplateType.ArticleUpdateMirrored, objArticleMirrored.AuthorEmail, ArticleSettings)
                                End If

                                If (objMirrorArticleInfo.AutoUpdate) Then

                                    objArticleMirrored.Title = objArticle.Title
                                    objArticleMirrored.Summary = objArticle.Summary

                                    objArticleMirrored.Url = objArticle.Url
                                    objArticleMirrored.IsNewWindow = objArticle.IsNewWindow
                                    objArticleMirrored.ImageUrl = objArticle.ImageUrl

                                    objArticleMirrored.MetaTitle = objArticle.MetaTitle
                                    objArticleMirrored.MetaDescription = objArticle.MetaDescription
                                    objArticleMirrored.MetaKeywords = objArticle.MetaKeywords
                                    objArticleMirrored.PageHeadText = objArticle.PageHeadText

                                    ' Save Custom Fields
                                    Dim fieldsToUpdate As New Hashtable

                                    Dim objCustomValueController As New CustomValueController
                                    Dim objCustomValues As List(Of CustomValueInfo) = objCustomValueController.List(objArticleMirrored.ArticleID)

                                    For Each objCustomValue As CustomValueInfo In objCustomValues
                                        objCustomValueController.Delete(objArticleMirrored.ArticleID, objCustomValue.CustomFieldID)
                                    Next

                                    Dim objCustomFieldController As New CustomFieldController
                                    Dim objCustomFields As ArrayList = objCustomFieldController.List(ModuleId)
                                    Dim objCustomFieldsLinked As ArrayList = objCustomFieldController.List(objArticleMirrored.ModuleID)

                                    For Each objCustomFieldLinked As CustomFieldInfo In objCustomFieldsLinked
                                        For Each objCustomField As CustomFieldInfo In objCustomFields

                                            If (objCustomFieldLinked.Name.ToLower() = objCustomField.Name.ToLower()) Then

                                                If (objArticle.CustomList.Contains(objCustomField.CustomFieldID)) Then
                                                    fieldsToUpdate.Add(objCustomFieldLinked.CustomFieldID.ToString(), objArticle.CustomList(objCustomField.CustomFieldID).ToString())
                                                End If

                                            End If

                                        Next
                                    Next

                                    For Each key As String In fieldsToUpdate.Keys
                                        Dim val As String = fieldsToUpdate(key).ToString()
                                        Dim objCustomValue As CustomValueInfo = New CustomValueInfo
                                        objCustomValue.CustomFieldID = Convert.ToInt32(key)
                                        objCustomValue.CustomValue = val
                                        objCustomValue.ArticleID = objArticleMirrored.ArticleID
                                        objCustomValueController.Add(objCustomValue)
                                    Next

                                    ' Details

                                    Dim objPageController As PageController = New PageController
                                    Dim currentPages As ArrayList = objPageController.GetPageList(objArticleMirrored.ArticleID)

                                    For Each objPage As PageInfo In currentPages
                                        objPageController.DeletePage(objPage.PageID)
                                    Next

                                    Dim pages As ArrayList = objPageController.GetPageList(objArticle.ArticleID)

                                    For Each objPage As PageInfo In pages
                                        objPage.ArticleID = objArticleMirrored.ArticleID
                                        objPage.PageID = Null.NullInteger
                                        objPageController.AddPage(objPage)
                                    Next

                                    ' Save Tags

                                    Dim objTagController As New TagController
                                    objTagController.DeleteArticleTag(objArticleMirrored.ArticleID)

                                    Dim tagsCurrent As String = objArticle.Tags

                                    If (tagsCurrent <> "") Then
                                        Dim tags As String() = tagsCurrent.Split(","c)
                                        For Each tag As String In tags
                                            If (tag <> "") Then
                                                Dim objTag As TagInfo = objTagController.Get(objArticleMirrored.ModuleID, tag)

                                                If (objTag Is Nothing) Then
                                                    objTag = New TagInfo
                                                    objTag.Name = tag
                                                    objTag.NameLowered = tag.ToLower()
                                                    objTag.ModuleID = objArticleMirrored.ModuleID
                                                    objTag.TagID = objTagController.Add(objTag)
                                                End If

                                                objTagController.Add(objArticleMirrored.ArticleID, objTag.TagID)
                                            End If
                                        Next
                                    End If

                                    'Copy Files
                                    Dim folderLinked As String = ""

                                    Dim objSettingsLinked As Hashtable = Common.GetModuleSettings(objArticleMirrored.ModuleID)

                                    If (objSettingsLinked.ContainsKey(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING)) Then
                                        If (IsNumeric(objSettingsLinked(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING))) Then
                                            Dim folderID As Integer = Convert.ToInt32(objSettingsLinked(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING))
                                            Dim objFolder As FolderInfo = FolderManager.Instance.GetFolder(folderID)
                                            If (objFolder IsNot Nothing) Then
                                                folderLinked = objFolder.FolderPath
                                            End If
                                        End If
                                    End If

                                    Dim objPortalController As New PortalController
                                    Dim objPortalLinked As PortalInfo = objPortalController.GetPortal(objMirrorArticleInfo.PortalID)

                                    Dim filePathLinked As String = objPortalLinked.HomeDirectoryMapPath & folderLinked.Replace("/", "\")

                                    Dim folder As String = ""

                                    Dim objSettings As Hashtable = Common.GetModuleSettings(ModuleId)

                                    If (objSettings.ContainsKey(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING)) Then
                                        If (IsNumeric(objSettings(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING))) Then
                                            Dim folderID As Integer = Convert.ToInt32(objSettings(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING))
                                            Dim objFolder As FolderInfo = FolderManager.Instance.GetFolder(folderID)
                                            If (objFolder IsNot Nothing) Then
                                                folder = objFolder.FolderPath
                                            End If
                                        End If
                                    End If

                                    Dim objFileController As New FileController
                                    Dim objFilesCurrent As List(Of FileInfo) = objFileController.GetFileList(objArticleMirrored.ArticleID, Null.NullString)

                                    For Each objFile As FileInfo In objFilesCurrent
                                        objFileController.Delete(objFile.FileID)
                                    Next

                                    Dim objFiles As List(Of FileInfo) = objFileController.GetFileList(objArticle.ArticleID, Null.NullString)

                                    For Each objFile As FileInfo In objFiles

                                        Dim finalCopyPath = PortalSettings.HomeDirectoryMapPath & folder.Replace("/", "\") & objFile.FileName
                                        Dim filePath As String = objPortalLinked.HomeDirectoryMapPath & folderLinked.Replace("/", "\") & objFile.FileName

                                        If (File.Exists(finalCopyPath)) Then

                                            If Not (Directory.Exists(objPortalLinked.HomeDirectoryMapPath & folderLinked.Replace("/", "\"))) Then
                                                Directory.CreateDirectory(objPortalLinked.HomeDirectoryMapPath & folderLinked.Replace("/", "\"))
                                            End If

                                            If (File.Exists(filePath) = False) Then
                                                File.Copy(finalCopyPath, filePath)
                                            End If

                                            objFile.FileID = Null.NullInteger
                                            objFile.ArticleID = objArticleMirrored.ArticleID
                                            objFileController.Add(objFile)

                                        End If

                                    Next

                                    'Copy Images

                                    Dim folderImagesLinked As String = ""

                                    If (objSettingsLinked.ContainsKey(ArticleConstants.DEFAULT_IMAGES_FOLDER_SETTING)) Then
                                        If (IsNumeric(objSettingsLinked(ArticleConstants.DEFAULT_IMAGES_FOLDER_SETTING))) Then
                                            Dim folderID As Integer = Convert.ToInt32(objSettingsLinked(ArticleConstants.DEFAULT_IMAGES_FOLDER_SETTING))
                                            Dim objFolder As FolderInfo = FolderManager.Instance.GetFolder(folderID)
                                            If (objFolder IsNot Nothing) Then
                                                folderImagesLinked = objFolder.FolderPath
                                            End If
                                        End If
                                    End If

                                    Dim filePathImagesLinked As String = objPortalLinked.HomeDirectoryMapPath & folderImagesLinked.Replace("/", "\")

                                    Dim folderImages As String = ""

                                    If (objSettings.ContainsKey(ArticleConstants.DEFAULT_IMAGES_FOLDER_SETTING)) Then
                                        If (IsNumeric(objSettings(ArticleConstants.DEFAULT_IMAGES_FOLDER_SETTING))) Then
                                            Dim folderID As Integer = Convert.ToInt32(objSettings(ArticleConstants.DEFAULT_IMAGES_FOLDER_SETTING))
                                            Dim objFolder As FolderInfo = FolderManager.Instance.GetFolder(folderID)
                                            If (objFolder IsNot Nothing) Then
                                                folderImages = objFolder.FolderPath
                                            End If
                                        End If
                                    End If

                                    Dim objImageController As New ImageController
                                    Dim objImagesCurrent As List(Of ImageInfo) = objImageController.GetImageList(objArticleMirrored.ArticleID, Null.NullString)

                                    For Each objImage As ImageInfo In objImagesCurrent
                                        objImageController.Delete(objImage.ImageID, _articleID, objImage.ImageGuid)
                                    Next

                                    Dim objImages As List(Of ImageInfo) = objImageController.GetImageList(objArticle.ArticleID, Null.NullString)

                                    For Each objImage As ImageInfo In objImages

                                        Dim finalCopyPath = PortalSettings.HomeDirectoryMapPath & folderImages.Replace("/", "\") & objImage.FileName
                                        Dim filePath As String = objPortalLinked.HomeDirectoryMapPath & folderImagesLinked.Replace("/", "\") & objImage.FileName

                                        If (File.Exists(finalCopyPath)) Then

                                            If Not (Directory.Exists(objPortalLinked.HomeDirectoryMapPath & folderImages.Replace("/", "\"))) Then
                                                Directory.CreateDirectory(objPortalLinked.HomeDirectoryMapPath & folderImages.Replace("/", "\"))
                                            End If

                                            If (File.Exists(filePath) = False) Then
                                                File.Copy(finalCopyPath, filePath)
                                            End If

                                            objImage.Folder = folderImages
                                            objImage.ImageID = Null.NullInteger
                                            objImage.ArticleID = objArticleMirrored.ArticleID
                                            objImageController.Add(objImage)

                                        End If

                                    Next


                                    ' Save

                                    objArticleController.UpdateArticle(objArticleMirrored)

                                End If


                            End If

                        Next

                    End If

                End If

            End If

            If (statusChanged) Then
                Common.NotifyAuthor(objArticle, Me.Settings, Me.ModuleId, PortalSettings.ActiveTab, Me.PortalId, ArticleSettings)
            End If

            ArticleController.ClearArticleCache(objArticle.ArticleID)

            Return objArticle.ArticleID

        End Function

        Private Sub SaveCategories(ByVal articleID As Integer)

            Dim objArticleController As ArticleController = New ArticleController

            If (chkMirrorArticle.Checked And drpMirrorArticle.Items.Count > 0) Then
                Dim objCategories As ArrayList = objArticleController.GetArticleCategories(Convert.ToInt32(drpMirrorArticle.SelectedValue))

                For Each objCategory As CategoryInfo In objCategories
                    For Each item As ListItem In lstCategories.Items
                        If (objCategory.Name.ToLower() = item.Text.TrimStart("."c).ToLower()) Then
                            item.Selected = True
                        End If
                    Next
                Next
            End If

            For Each item As ListItem In lstCategories.Items
                If (item.Selected) Then
                    objArticleController.AddArticleCategory(articleID, Int32.Parse(item.Value))
                End If
            Next

            DataCache.RemoveCache(ArticleConstants.CACHE_CATEGORY_ARTICLE & articleID.ToString())
            DataCache.RemoveCache(ArticleConstants.CACHE_CATEGORY_ARTICLE_NO_LINK & articleID.ToString())

        End Sub

        Private Sub SaveTags(ByVal articleID As Integer)

            If (chkMirrorArticle.Checked And drpMirrorArticle.Items.Count > 0) Then
                Dim objArticleController As New ArticleController()
                Dim objLinkedArticle As ArticleInfo = objArticleController.GetArticle(Convert.ToInt32(drpMirrorArticle.SelectedValue))

                If (objLinkedArticle IsNot Nothing) Then
                    If ArticleSettings.UseStaticTagsList Then
                        SelectAllTags(objLinkedArticle.Tags)
                        CreateFinalTags(objLinkedArticle.Tags)
                    Else
                        txtTags.Text = objLinkedArticle.Tags
                    End If
                End If
            End If

            Dim objTagController As New TagController
            objTagController.DeleteArticleTag(articleID)

            If ArticleSettings.UseStaticTagsList Then
                Dim order As Integer = 0

                For Each li As ListItem In lstFinalTags.Items
                    order = order + 1

                    Dim objTag As TagInfo = objTagController.Get(ModuleId, li.Value)

                    If objTag IsNot Nothing Then
                        objTagController.Add(articleID, objTag.TagID, order)
                    End If
                Next
            Else
                If (txtTags.Text <> "") Then
                    Dim tags As String() = txtTags.Text.Split(","c)
                    For Each tag As String In tags
                        If (tag <> "") Then
                            Dim objTag As TagInfo = objTagController.Get(ModuleId, tag)

                            If (objTag Is Nothing) Then
                                objTag = New TagInfo
                                objTag.Name = tag
                                objTag.NameLowered = tag.ToLower()
                                objTag.ModuleID = ModuleId
                                objTag.TagID = objTagController.Add(objTag)
                            End If

                            objTagController.Add(articleID, objTag.TagID)
                        End If
                    Next
                End If
            End If

        End Sub

        Private Sub SaveDetails(ByVal articleID As Integer, ByVal title As String)

            If (chkMirrorArticle.Checked And drpMirrorArticle.Items.Count > 0) Then
                Dim objPageController As PageController = New PageController
                Dim pages As ArrayList = objPageController.GetPageList(Convert.ToInt32(drpMirrorArticle.SelectedValue))

                For Each objPage As PageInfo In pages
                    objPage.ArticleID = articleID
                    objPageController.AddPage(objPage)
                Next
            Else
                Dim doUpdate As Boolean = True
                If (phMirrorText.Visible = True) Then
                    Dim objMirrorArticleController As New MirrorArticleController
                    Dim objMirrorArticleInfo As MirrorArticleInfo = objMirrorArticleController.GetMirrorArticle(_articleID)

                    If (objMirrorArticleInfo IsNot Nothing) Then

                        If (objMirrorArticleInfo.AutoUpdate) Then
                            doUpdate = False
                        End If
                    End If
                End If

                If (doUpdate) Then
                    If (Details.Text.Trim() <> "") Then
                        Dim objPageController As PageController = New PageController
                        Dim pages As ArrayList = objPageController.GetPageList(articleID)

                        If (pages.Count > 0) Then
                            Dim objPage As PageInfo = CType(pages(0), PageInfo)
                            objPage.PageText = Details.Text
                            objPageController.UpdatePage(objPage)
                        Else
                            Dim objPage As New PageInfo
                            objPage.PageText = Details.Text
                            objPage.ArticleID = articleID
                            objPage.Title = txtTitle.Text
                            objPageController.AddPage(objPage)
                        End If
                    Else
                        Dim objPageController As PageController = New PageController
                        Dim pages As ArrayList = objPageController.GetPageList(articleID)

                        If (pages.Count = 1) Then
                            objPageController.DeletePage(CType(pages(0), PageInfo).PageID)
                        End If
                    End If
                End If
            End If

        End Sub

        Private Sub SaveCustomFields(ByVal articleID As Integer)

            Dim fieldsToUpdate As New Hashtable

            Dim objCustomFieldController As New CustomFieldController()
            Dim objCustomFields As ArrayList = objCustomFieldController.List(Me.ModuleId)

            If (phCustomFields.Visible) Then

                For Each item As RepeaterItem In rptCustomFields.Items
                    Dim phValue As PlaceHolder = CType(item.FindControl("phValue"), PlaceHolder)

                    If Not (phValue Is Nothing) Then
                        If (phValue.Controls.Count > 0) Then

                            Dim objControl As System.Web.UI.Control = phValue.Controls(0)
                            Dim customFieldID As Integer = Convert.ToInt32(objControl.ID)

                            For Each objCustomField As CustomFieldInfo In objCustomFields
                                If (objCustomField.CustomFieldID = customFieldID) Then
                                    Select Case objCustomField.FieldType

                                        Case CustomFieldType.OneLineTextBox
                                            Dim objTextBox As TextBox = CType(objControl, TextBox)
                                            fieldsToUpdate.Add(customFieldID.ToString(), objTextBox.Text)

                                        Case CustomFieldType.MultiLineTextBox
                                            Dim objTextBox As TextBox = CType(objControl, TextBox)
                                            fieldsToUpdate.Add(customFieldID.ToString(), objTextBox.Text)

                                        Case CustomFieldType.RichTextBox
                                            Dim objTextBox As TextEditor = CType(objControl, TextEditor)
                                            fieldsToUpdate.Add(customFieldID.ToString(), objTextBox.Text)

                                        Case CustomFieldType.DropDownList
                                            Dim objDropDownList As DropDownList = CType(objControl, DropDownList)
                                            If (objDropDownList.SelectedValue = "-1") Then
                                                fieldsToUpdate.Add(customFieldID.ToString(), "")
                                            Else
                                                fieldsToUpdate.Add(customFieldID.ToString(), objDropDownList.SelectedValue)
                                            End If

                                        Case CustomFieldType.CheckBox
                                            Dim objCheckBox As CheckBox = CType(objControl, CheckBox)
                                            fieldsToUpdate.Add(customFieldID.ToString(), objCheckBox.Checked.ToString())

                                        Case CustomFieldType.MultiCheckBox
                                            Dim objCheckBoxList As CheckBoxList = CType(objControl, CheckBoxList)
                                            Dim values As String = ""
                                            For Each objCheckBox As ListItem In objCheckBoxList.Items
                                                If (objCheckBox.Selected) Then
                                                    If (values = "") Then
                                                        values = objCheckBox.Value
                                                    Else
                                                        values = values & "|" & objCheckBox.Value
                                                    End If
                                                End If
                                            Next
                                            fieldsToUpdate.Add(customFieldID.ToString(), values)

                                        Case CustomFieldType.RadioButton
                                            Dim objRadioButtonList As RadioButtonList = CType(objControl, RadioButtonList)
                                            fieldsToUpdate.Add(customFieldID.ToString(), objRadioButtonList.SelectedValue)

                                        Case CustomFieldType.ColorPicker
                                            Dim objTextBox As TextBox = CType(objControl, TextBox)
                                            fieldsToUpdate.Add(customFieldID.ToString(), objTextBox.Text)

                                    End Select

                                    Exit For
                                End If
                            Next

                        End If
                    End If
                Next

            End If

            If (chkMirrorArticle.Checked And drpMirrorArticle.Items.Count > 0) Then

                Dim objArticleController As New ArticleController()
                Dim objLinkedArticle As ArticleInfo = objArticleController.GetArticle(Convert.ToInt32(drpMirrorArticle.SelectedValue))

                If (objLinkedArticle IsNot Nothing) Then

                    Dim objCustomFieldsLinked As ArrayList = objCustomFieldController.List(Convert.ToInt32(drpMirrorModule.SelectedValue.Split("-"c)(1)))

                    For Each objCustomFieldLinked As CustomFieldInfo In objCustomFieldsLinked
                        For Each objCustomField As CustomFieldInfo In objCustomFields

                            If (objCustomFieldLinked.Name.ToLower() = objCustomField.Name.ToLower()) Then

                                If (objLinkedArticle.CustomList.Contains(objCustomFieldLinked.CustomFieldID)) Then
                                    fieldsToUpdate.Add(objCustomField.CustomFieldID.ToString(), objLinkedArticle.CustomList(objCustomFieldLinked.CustomFieldID).ToString())
                                End If

                            End If

                        Next
                    Next

                End If

            End If

            For Each key As String In fieldsToUpdate.Keys
                Dim val As String = fieldsToUpdate(key).ToString()

                Dim objCustomValueController As New CustomValueController
                Dim objCustomValue As CustomValueInfo = objCustomValueController.GetByCustomField(articleID, Convert.ToInt32(key))

                If (objCustomValue IsNot Nothing) Then
                    objCustomValueController.Delete(articleID, Convert.ToInt32(key))
                End If

                objCustomValue = New CustomValueInfo
                objCustomValue.CustomFieldID = Convert.ToInt32(key)
                objCustomValue.CustomValue = val
                objCustomValue.ArticleID = articleID
                objCustomValueController.Add(objCustomValue)
            Next

        End Sub

        Private Sub SetVisibility()

            trCategories.Visible = ArticleSettings.IsCategoriesEnabled

            If (lstCategories.Items.Count = 0 Or trCategories.Visible = False) Then
                Dim objTagController As New TagController()
                Dim objTags As ArrayList = objTagController.List(Me.ModuleId, 10)
                If (objTags.Count = 0) Then
                    phOrganize.Visible = False
                End If
            End If

            If (lstCategories.Items.Count = 0) Then
                trCategories.Visible = False
            End If

            phExcerpt.Visible = ArticleSettings.IsExcerptEnabled
            phMeta.Visible = ArticleSettings.IsMetaEnabled
            If (phCustomFields.Visible) Then
                phCustomFields.Visible = ArticleSettings.IsCustomEnabled
            End If

            trLink.Visible = ArticleSettings.IsLinkEnabled
            trNewWindow.Visible = ArticleSettings.IsLinkEnabled

            phAttachment.Visible = (trLink.Visible Or trNewWindow.Visible)

            trFeatured.Visible = ArticleSettings.IsFeaturedEnabled
            trSecure.Visible = ArticleSettings.IsSecureEnabled
            trPublish.Visible = ArticleSettings.IsPublishEnabled
            trExpiry.Visible = ArticleSettings.IsExpiryEnabled

            phPublish.Visible = (trAuthor.Visible Or trFeatured.Visible Or trSecure.Visible Or trPublish.Visible Or trExpiry.Visible)

            drpStatus.Enabled = ArticleSettings.IsApprover

            If (_articleID <> Null.NullInteger) Then
                Dim objArticleController As New ArticleController
                Dim objArticle As ArticleInfo = objArticleController.GetArticle(_articleID)

                If Not (objArticle Is Nothing) Then
                    Select Case objArticle.Status
                        Case StatusType.Draft
                            cmdSaveArticle.Visible = True
                            cmdPublishArticle.Visible = True
                            cmdAddEditPages.Visible = True
                            cmdDelete.Visible = True
                            Return

                        Case StatusType.AwaitingApproval
                            cmdSaveArticle.Visible = True
                            cmdPublishArticle.Visible = False
                            cmdAddEditPages.Visible = True
                            cmdDelete.Visible = True
                            Return

                        Case StatusType.Published
                            cmdSaveArticle.Visible = True
                            cmdPublishArticle.Visible = False
                            cmdAddEditPages.Visible = True
                            cmdDelete.Visible = True
                            Return
                    End Select
                End If
            Else
                cmdSaveArticle.Visible = True
                cmdPublishArticle.Visible = True
                cmdAddEditPages.Visible = True
                cmdDelete.Visible = False
            End If

        End Sub

        Private Sub SetTextEditor()

            Me.txtExcerptBasic.Visible = (ArticleSettings.TextEditorSummaryMode = Components.Types.TextEditorModeType.Basic)
            Me.txtExcerptRich.Visible = (ArticleSettings.TextEditorSummaryMode = Components.Types.TextEditorModeType.Rich)

            'dshExcerpt.IsExpanded = ArticleSettings.ExpandSummary
            'dshMeta.IsExpanded = ArticleSettings.ExpandMetaInformation

            txtExcerptBasic.Width = Unit.Parse(ArticleSettings.TextEditorSummaryWidth)
            Me.txtExcerptBasic.Height = Unit.Parse(ArticleSettings.TextEditorSummaryHeight)
            Me.ExcerptRich.Width = Unit.Parse(ArticleSettings.TextEditorSummaryWidth)
            Me.ExcerptRich.Height = Unit.Parse(ArticleSettings.TextEditorSummaryHeight)

            Me.Details.Width = Unit.Parse(ArticleSettings.TextEditorWidth)
            Me.Details.Height = Unit.Parse(ArticleSettings.TextEditorHeight)

            Me.lstCategories.Height = Unit.Parse(ArticleSettings.CategorySelectionHeight.ToString())

        End Sub

        Private Sub SetValidationGroup()
            Dim vgId As String = Guid.NewGuid().ToString()

            valMirrorArticle.ValidationGroup = vgId
            valTitle.ValidationGroup = vgId
            valBody.ValidationGroup = vgId
            valCategory.ValidationGroup = vgId
            valExpiryDate.ValidationGroup = vgId
            valPublishDateRequired.ValidationGroup = vgId
            valMessageBox.ValidationGroup = vgId
            cmdSaveArticle.ValidationGroup = vgId
            cmdPublishArticle.ValidationGroup = vgId
            cmdAddEditPages.ValidationGroup = vgId
        End Sub


#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                ReadQueryString()

                Dim script As String = "" & vbCrLf _
                    & "<script type=""text/javascript"" src='" & Me.ResolveUrl("Includes/ColorPicker/ColorPicker.js") & "'></script>" & vbCrLf

                Dim CSM As ClientScriptManager = Page.ClientScript
                CSM.RegisterClientScriptBlock(Me.GetType, "ColorPicker", script)

                BindCustomFields()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                Dim objModuleController As ModuleController = New ModuleController
                Dim objModule As ModuleInfo = objModuleController.GetModule(Me.ModuleId, Me.TabId)

                If Not (objModule Is Nothing) Then
                    trAuthor.Visible = (ModulePermissionController.HasModuleAccess(SecurityAccessLevel.Edit, "EDIT", ModuleConfiguration) Or ArticleSettings.IsApprover)
                End If

                cmdExpiryCalendar.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtExpiryDate)
                cmdPublishCalendar.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtPublishDate)

                CheckSecurity()
                SetTextEditor()

                If (_articleID <> Null.NullInteger) Then
                    For Each key As String In _richTextValues
                        For Each item As RepeaterItem In rptCustomFields.Items
                            If Not (item.FindControl(key) Is Nothing) Then
                                Dim objTextBox As TextEditor = CType(item.FindControl(key), TextEditor)
                                objTextBox.Text = _richTextValues(key)
                                Exit For
                            End If
                        Next
                    Next
                End If

                If IsPostBack = False Then

                    BindStatus()
                    BindCategories()
                    BindTags()
                    SetVisibility()
                    BindArticle()
                    SetValidationGroup()

                    If (ArticleSettings.ContentSharingPortals = "" Or _articleID <> Null.NullInteger) Then
                        phMirror.Visible = False
                    End If

                    Page.SetFocus(txtTitle)

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                trNewWindow.Visible = (trLink.Visible And (UrlLink.UrlType <> "N"))

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub rptCustomFields_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptCustomFields.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objArticleController As New ArticleController()
                Dim objArticle As ArticleInfo = Nothing
                If (_articleID <> Null.NullInteger) Then
                    objArticle = objArticleController.GetArticle(_articleID)
                End If

                Dim objCustomField As CustomFieldInfo = CType(e.Item.DataItem, CustomFieldInfo)
                Dim phValue As PlaceHolder = CType(e.Item.FindControl("phValue"), PlaceHolder)
                Dim phLabel As PlaceHolder = CType(e.Item.FindControl("phLabel"), PlaceHolder)

                Dim cmdHelp As LinkButton = CType(e.Item.FindControl("cmdHelp"), LinkButton)
                Dim pnlHelp As Panel = CType(e.Item.FindControl("pnlHelp"), Panel)
                Dim lblLabel As Label = CType(e.Item.FindControl("lblLabel"), Label)
                Dim lblHelp As Label = CType(e.Item.FindControl("lblHelp"), Label)
                Dim imgHelp As Image = CType(e.Item.FindControl("imgHelp"), Image)

                Dim trItem As HtmlControls.HtmlTableRow = CType(e.Item.FindControl("trItem"), HtmlControls.HtmlTableRow)

                If Not (phValue Is Nothing) Then

                    DotNetNuke.UI.Utilities.DNNClientAPI.EnableMinMax(cmdHelp, pnlHelp, True, DotNetNuke.UI.Utilities.DNNClientAPI.MinMaxPersistanceType.None)

                    If (objCustomField.IsRequired) Then
                        lblLabel.Text = objCustomField.Caption & "*:"
                    Else
                        lblLabel.Text = objCustomField.Caption & ":"
                    End If
                    lblHelp.Text = objCustomField.CaptionHelp
                    imgHelp.AlternateText = objCustomField.CaptionHelp

                    Select Case (objCustomField.FieldType)

                        Case CustomFieldType.OneLineTextBox

                            Dim objTextBox As New TextBox
                            objTextBox.CssClass = "NormalTextBox"
                            objTextBox.ID = objCustomField.CustomFieldID.ToString()
                            If (objCustomField.Length <> Null.NullInteger AndAlso objCustomField.Length > 0) Then
                                objTextBox.MaxLength = objCustomField.Length
                            End If
                            If (objCustomField.DefaultValue <> "") Then
                                objTextBox.Text = objCustomField.DefaultValue
                            End If
                            If Not (objArticle Is Nothing) Then
                                If (objArticle.CustomList.Contains(objCustomField.CustomFieldID) And (Page.IsPostBack = False Or objTextBox.Enabled = False)) Then
                                    objTextBox.Text = objArticle.CustomList(objCustomField.CustomFieldID).ToString()
                                End If
                            End If
                            objTextBox.Width = Unit.Pixel(300)
                            phValue.Controls.Add(objTextBox)

                            If (objCustomField.IsRequired) Then
                                Dim valRequired As New RequiredFieldValidator
                                valRequired.ControlToValidate = objTextBox.ID
                                valRequired.ErrorMessage = Localization.GetString("valFieldRequired", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                valRequired.CssClass = "NormalRed"
                                valRequired.Display = ValidatorDisplay.None
                                valRequired.SetFocusOnError = True
                                phValue.Controls.Add(valRequired)
                            End If

                            If (objCustomField.ValidationType <> CustomFieldValidationType.None) Then
                                Dim valCompare As New CompareValidator
                                valCompare.ControlToValidate = objTextBox.ID
                                valCompare.CssClass = "NormalRed"
                                valCompare.Display = ValidatorDisplay.None
                                valCompare.SetFocusOnError = True
                                Select Case objCustomField.ValidationType

                                    Case CustomFieldValidationType.Currency
                                        valCompare.Type = ValidationDataType.Double
                                        valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                        valCompare.ErrorMessage = Localization.GetString("valFieldCurrency", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                        phValue.Controls.Add(valCompare)

                                    Case CustomFieldValidationType.Date
                                        valCompare.Type = ValidationDataType.Date
                                        valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                        valCompare.ErrorMessage = Localization.GetString("valFieldDate", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                        phValue.Controls.Add(valCompare)

                                        Dim objCalendar As New HyperLink
                                        objCalendar.CssClass = "CommandButton"
                                        objCalendar.Text = Localization.GetString("Calendar", Me.LocalResourceFile)
                                        objCalendar.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(objTextBox)
                                        phValue.Controls.Add(objCalendar)

                                    Case CustomFieldValidationType.Double
                                        valCompare.Type = ValidationDataType.Double
                                        valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                        valCompare.ErrorMessage = Localization.GetString("valFieldDecimal", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                        phValue.Controls.Add(valCompare)

                                    Case CustomFieldValidationType.Integer
                                        valCompare.Type = ValidationDataType.Integer
                                        valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                        valCompare.ErrorMessage = Localization.GetString("valFieldNumber", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                        phValue.Controls.Add(valCompare)

                                    Case CustomFieldValidationType.Email
                                        Dim valRegular As New RegularExpressionValidator
                                        valRegular.ControlToValidate = objTextBox.ID
                                        valRegular.ValidationExpression = "\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        valRegular.ErrorMessage = Localization.GetString("valFieldEmail", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                        valRegular.CssClass = "NormalRed"
                                        valRegular.Display = ValidatorDisplay.None
                                        phValue.Controls.Add(valRegular)

                                    Case CustomFieldValidationType.Regex
                                        If (objCustomField.RegularExpression <> "") Then
                                            Dim valRegular As New RegularExpressionValidator
                                            valRegular.ControlToValidate = objTextBox.ID
                                            valRegular.ValidationExpression = objCustomField.RegularExpression
                                            valRegular.ErrorMessage = Localization.GetString("valFieldRegex", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                            valRegular.CssClass = "NormalRed"
                                            valRegular.Display = ValidatorDisplay.None
                                            phValue.Controls.Add(valRegular)
                                        End If

                                End Select
                            End If

                        Case CustomFieldType.MultiLineTextBox

                            Dim objTextBox As New TextBox
                            objTextBox.TextMode = TextBoxMode.MultiLine
                            objTextBox.CssClass = "NormalTextBox"
                            objTextBox.ID = objCustomField.CustomFieldID.ToString()
                            objTextBox.Rows = 4
                            If (objCustomField.Length <> Null.NullInteger AndAlso objCustomField.Length > 0) Then
                                objTextBox.MaxLength = objCustomField.Length
                            End If
                            If (objCustomField.DefaultValue <> "") Then
                                objTextBox.Text = objCustomField.DefaultValue
                            End If
                            If Not (objArticle Is Nothing) Then
                                If (objArticle.CustomList.Contains(objCustomField.CustomFieldID) And (Page.IsPostBack = False Or objTextBox.Enabled = False)) Then
                                    objTextBox.Text = objArticle.CustomList(objCustomField.CustomFieldID).ToString()
                                End If
                            End If
                            objTextBox.Width = Unit.Pixel(300)
                            phValue.Controls.Add(objTextBox)

                            If (objCustomField.IsRequired) Then
                                Dim valRequired As New RequiredFieldValidator
                                valRequired.ControlToValidate = objTextBox.ID
                                valRequired.ErrorMessage = Localization.GetString("valFieldRequired", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                valRequired.CssClass = "NormalRed"
                                valRequired.Display = ValidatorDisplay.None
                                valRequired.SetFocusOnError = True
                                phValue.Controls.Add(valRequired)
                            End If

                        Case CustomFieldType.RichTextBox

                            Dim objTextBox As TextEditor = CType(Me.LoadControl("~/controls/TextEditor.ascx"), TextEditor)
                            objTextBox.ID = objCustomField.CustomFieldID.ToString()
                            If (objCustomField.DefaultValue <> "") Then
                                ' objTextBox.Text = objCustomField.DefaultValue
                            End If
                            If Not (objArticle Is Nothing) Then
                                If (objArticle.CustomList.Contains(objCustomField.CustomFieldID) And Page.IsPostBack = False) Then
                                    ' There is a problem assigned values at init with the RTE, using ArrayList to assign later.
                                    _richTextValues.Add(objCustomField.CustomFieldID.ToString(), objArticle.CustomList(objCustomField.CustomFieldID).ToString())
                                End If
                            End If
                            objTextBox.Width = Unit.Pixel(300)
                            objTextBox.Height = Unit.Pixel(400)

                            phValue.Controls.Add(objTextBox)

                            If (objCustomField.IsRequired) Then
                                Dim valRequired As New RequiredFieldValidator
                                valRequired.ControlToValidate = objTextBox.ID
                                valRequired.ErrorMessage = Localization.GetString("valFieldRequired", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                valRequired.CssClass = "NormalRed"
                                valRequired.SetFocusOnError = True
                                phValue.Controls.Add(valRequired)
                            End If

                        Case CustomFieldType.DropDownList

                            Dim objDropDownList As New DropDownList
                            objDropDownList.CssClass = "NormalTextBox"
                            objDropDownList.ID = objCustomField.CustomFieldID.ToString()

                            Dim values As String() = objCustomField.FieldElements.Split(Convert.ToChar("|"))
                            For Each value As String In values
                                If (value <> "") Then
                                    objDropDownList.Items.Add(value)
                                End If
                            Next

                            Dim selectText As String = Localization.GetString("SelectValue", Me.LocalResourceFile)
                            selectText = selectText.Replace("[VALUE]", objCustomField.Caption)
                            objDropDownList.Items.Insert(0, New ListItem(selectText, "-1"))

                            If (objCustomField.DefaultValue <> "") Then
                                If Not (objDropDownList.Items.FindByValue(objCustomField.DefaultValue) Is Nothing) Then
                                    objDropDownList.SelectedValue = objCustomField.DefaultValue
                                End If
                            End If

                            objDropDownList.Width = Unit.Pixel(300)

                            If Not (objArticle Is Nothing) Then
                                If (objArticle.CustomList.Contains(objCustomField.CustomFieldID) And (Page.IsPostBack = False Or objDropDownList.Enabled = False)) Then
                                    If Not (objDropDownList.Items.FindByValue(objArticle.CustomList(objCustomField.CustomFieldID).ToString()) Is Nothing) Then
                                        objDropDownList.SelectedValue = objArticle.CustomList(objCustomField.CustomFieldID).ToString()
                                    End If
                                End If
                            End If
                            phValue.Controls.Add(objDropDownList)

                            If (objCustomField.IsRequired) Then
                                Dim valRequired As New RequiredFieldValidator
                                valRequired.ControlToValidate = objDropDownList.ID
                                valRequired.ErrorMessage = Localization.GetString("valFieldRequired", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                valRequired.CssClass = "NormalRed"
                                valRequired.Display = ValidatorDisplay.None
                                valRequired.SetFocusOnError = True
                                valRequired.InitialValue = "-1"
                                phValue.Controls.Add(valRequired)
                            End If

                        Case CustomFieldType.CheckBox

                            Dim objCheckBox As New CheckBox
                            objCheckBox.CssClass = "Normal"
                            objCheckBox.ID = objCustomField.CustomFieldID.ToString()
                            If (objCustomField.DefaultValue <> "") Then
                                Try
                                    objCheckBox.Checked = Convert.ToBoolean(objCustomField.DefaultValue)
                                Catch
                                End Try
                            End If

                            If Not (objArticle Is Nothing) Then
                                If (objArticle.CustomList.Contains(objCustomField.CustomFieldID) And (Page.IsPostBack = False Or objCheckBox.Enabled = False)) Then
                                    If (objArticle.CustomList(objCustomField.CustomFieldID).ToString() <> "") Then
                                        Try
                                            objCheckBox.Checked = Convert.ToBoolean(objArticle.CustomList(objCustomField.CustomFieldID).ToString())
                                        Catch
                                        End Try
                                    End If
                                End If
                            End If
                            phValue.Controls.Add(objCheckBox)

                        Case CustomFieldType.MultiCheckBox

                            Dim objCheckBoxList As New CheckBoxList
                            objCheckBoxList.CssClass = "Normal"
                            objCheckBoxList.ID = objCustomField.CustomFieldID.ToString()
                            objCheckBoxList.RepeatColumns = 4
                            objCheckBoxList.RepeatDirection = RepeatDirection.Horizontal
                            objCheckBoxList.RepeatLayout = RepeatLayout.Table

                            Dim values As String() = objCustomField.FieldElements.Split(Convert.ToChar("|"))
                            For Each value As String In values
                                objCheckBoxList.Items.Add(value)
                            Next

                            If (objCustomField.DefaultValue <> "") Then
                                Dim vals As String() = objCustomField.DefaultValue.Split(Convert.ToChar("|"))
                                For Each val As String In vals
                                    For Each item As ListItem In objCheckBoxList.Items
                                        If (item.Value = val) Then
                                            item.Selected = True
                                        End If
                                    Next
                                Next
                            End If

                            If Not (objArticle Is Nothing) Then
                                If (objArticle.CustomList.Contains(objCustomField.CustomFieldID) And (Page.IsPostBack = False Or objCheckBoxList.Enabled = False)) Then
                                    Dim vals As String() = objArticle.CustomList(objCustomField.CustomFieldID).ToString().Split(Convert.ToChar("|"))
                                    For Each val As String In vals
                                        For Each item As ListItem In objCheckBoxList.Items
                                            If (item.Value = val) Then
                                                item.Selected = True
                                            End If
                                        Next
                                    Next
                                End If
                            End If

                            phValue.Controls.Add(objCheckBoxList)

                        Case CustomFieldType.RadioButton

                            Dim objRadioButtonList As New RadioButtonList
                            objRadioButtonList.CssClass = "Normal"
                            objRadioButtonList.ID = objCustomField.CustomFieldID.ToString()
                            objRadioButtonList.RepeatDirection = RepeatDirection.Horizontal
                            objRadioButtonList.RepeatLayout = RepeatLayout.Table
                            objRadioButtonList.RepeatColumns = 4

                            Dim values As String() = objCustomField.FieldElements.Split(Convert.ToChar("|"))
                            For Each value As String In values
                                objRadioButtonList.Items.Add(value)
                            Next

                            If (objCustomField.DefaultValue <> "") Then
                                If Not (objRadioButtonList.Items.FindByValue(objCustomField.DefaultValue) Is Nothing) Then
                                    objRadioButtonList.SelectedValue = objCustomField.DefaultValue
                                End If
                            End If

                            If Not (objArticle Is Nothing) Then
                                If (objArticle.CustomList.Contains(objCustomField.CustomFieldID) And (Page.IsPostBack = False Or objRadioButtonList.Enabled = False)) Then
                                    If Not (objRadioButtonList.Items.FindByValue(objArticle.CustomList(objCustomField.CustomFieldID).ToString()) Is Nothing) Then
                                        objRadioButtonList.SelectedValue = objArticle.CustomList(objCustomField.CustomFieldID).ToString()
                                    End If
                                End If
                            End If

                            phValue.Controls.Add(objRadioButtonList)

                            If (objCustomField.IsRequired) Then
                                Dim valRequired As New RequiredFieldValidator
                                valRequired.ControlToValidate = objRadioButtonList.ID
                                valRequired.ErrorMessage = Localization.GetString("valFieldRequired", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                valRequired.CssClass = "NormalRed"
                                valRequired.Display = ValidatorDisplay.None
                                valRequired.SetFocusOnError = True
                                phValue.Controls.Add(valRequired)
                            End If

                        Case CustomFieldType.ColorPicker

                            Dim objTextBox As New TextBox
                            objTextBox.CssClass = "NormalTextBox"
                            objTextBox.ID = objCustomField.CustomFieldID.ToString()
                            If (objCustomField.Length <> Null.NullInteger AndAlso objCustomField.Length > 0) Then
                                objTextBox.MaxLength = objCustomField.Length
                            End If
                            If (objCustomField.DefaultValue <> "") Then
                                objTextBox.Text = objCustomField.DefaultValue
                            End If
                            If Not (objArticle Is Nothing) Then
                                If (objArticle.CustomList.Contains(objCustomField.CustomFieldID) And (Page.IsPostBack = False Or objTextBox.Enabled = False)) Then
                                    objTextBox.Text = objArticle.CustomList(objCustomField.CustomFieldID).ToString()
                                End If
                            End If
                            phValue.Controls.Add(objTextBox)

                            If (objCustomField.IsRequired) Then
                                Dim valRequired As New RequiredFieldValidator
                                valRequired.ControlToValidate = objTextBox.ID
                                valRequired.ErrorMessage = Localization.GetString("valFieldRequired", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                valRequired.CssClass = "NormalRed"
                                valRequired.Display = ValidatorDisplay.None
                                valRequired.SetFocusOnError = True
                                phValue.Controls.Add(valRequired)
                            End If

                            Dim script As String = "" _
                                & "<script type=""text/javascript"" charset=""utf-8"">" _
                                & "jQuery(function($)" _
                                & "{" _
                                & "     $(""#" & objTextBox.ClientID & """).attachColorPicker();" _
                                & "});" _
                                & "</script>"

                            Dim CSM As ClientScriptManager = Page.ClientScript
                            CSM.RegisterClientScriptBlock(Me.GetType, "Picker" + objTextBox.ID, script)

                    End Select

                End If

            End If

        End Sub

        Private Sub replaceTags_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles replaceTags.Click

            If lstTags.Items.Count > 0 Then
                lstFinalTags.Items.Clear()
            End If

            For Each li As ListItem In lstTags.Items
                If (li.Selected) Then
                    li.Selected = False
                    lstFinalTags.Items.Add(li)
                End If
            Next

        End Sub

        Private Sub addTags_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addTags.Click

            For Each li As ListItem In lstTags.Items
                If (li.Selected) Then

                    li.Selected = False
                    If Not lstFinalTags.Items.Contains(li) Then

                        lstFinalTags.Items.Add(li)

                    End If

                End If
            Next

        End Sub


        Private Sub cmdUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUp.Click

            moveListItem(-1)

        End Sub

        Private Sub cmdDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDown.Click

            moveListItem(1)

        End Sub

        Private Sub moveListItem(direction As Integer)

            Try
                With Me.lstFinalTags
                    Dim selectedIndex As Integer = .SelectedIndex
                    Dim selectedItem As ListItem = .SelectedItem
                    Dim totalItems As Integer = .Items.Count

                    ' No selected item
                    If (selectedItem Is Nothing Or selectedIndex < 0) Then
                        Return
                    End If

                    ' Calculate New index using direction
                    Dim newIndex As Integer = selectedIndex + direction

                    '  New Index out of range
                    If (newIndex < 0 Or newIndex >= totalItems) Then
                        Return
                    End If

                    ' Remove old element
                    .Items.Remove(selectedItem)

                    ' Insert into New position
                    .Items.Insert(newIndex, selectedItem)

                    ' Restore selection
                    .SelectedIndex = newIndex
                End With
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdDeleteTag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDeleteTag.Click

            If lstFinalTags.SelectedItem IsNot Nothing Then
                lstFinalTags.Items.Remove(lstFinalTags.SelectedItem)
            End If

        End Sub

        Private Sub cmdSaveArticle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveArticle.Click

            Try

                If (Page.IsValid) Then
                    Dim objStatusType As StatusType = CType(System.Enum.Parse(GetType(StatusType), drpStatus.SelectedValue), StatusType)
                    Dim articleID As Integer = SaveArticle(objStatusType)

                    If (objStatusType = StatusType.Draft) Then
                        Response.Redirect(Common.GetModuleLink(Me.TabId, Me.ModuleId, "MyArticles", ArticleSettings), True)
                    End If

                    If (objStatusType = StatusType.AwaitingApproval) Then

                    End If

                    If (objStatusType = StatusType.Published) Then
                        Dim objArticleController As New ArticleController
                        Dim objArticle As ArticleInfo = objArticleController.GetArticle(articleID)

                        If Not (objArticle Is Nothing) Then
                            If (_returnUrl <> "") Then
                                Response.Redirect(_returnUrl, True)
                            Else
                                Response.Redirect(Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False), True)
                            End If
                        End If
                    End If

                    If (_returnUrl <> "") Then
                        Response.Redirect(_returnUrl, True)
                    Else
                        Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "", ArticleSettings), True)
                    End If
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdPublishArticle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPublishArticle.Click

            Try

                If (Page.IsValid) Then
                    If (ArticleSettings.IsApprover Or ArticleSettings.IsAutoApprover) Then
                        Dim articleID As Integer = SaveArticle(StatusType.Published)
                        Dim objArticleController As New ArticleController
                        Dim objArticle As ArticleInfo = objArticleController.GetArticle(articleID)

                        If Not (objArticle Is Nothing) Then
                            Response.Redirect(Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False), True)
                        Else
                            Response.Redirect(NavigateURL(), True)
                        End If
                    Else
                        Dim articleID As Integer = SaveArticle(StatusType.AwaitingApproval)

                        Dim objArticleController As New ArticleController
                        Dim objArticle As ArticleInfo = objArticleController.GetArticle(articleID)

                        If Not (objArticle Is Nothing) Then
                            If (Settings.Contains(ArticleConstants.NOTIFY_SUBMISSION_SETTING)) Then
                                If (Convert.ToBoolean(Settings(ArticleConstants.NOTIFY_SUBMISSION_SETTING)) = True) Then
                                    Dim objEmailTemplateController As New EmailTemplateController
                                    Dim emails As String = objEmailTemplateController.GetApproverDistributionList(ModuleId)

                                    For Each email As String In emails.Split(Convert.ToChar(";"))
                                        If (email <> "") Then
                                            objEmailTemplateController.SendFormattedEmail(Me.ModuleId, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False), objArticle, EmailTemplateType.ArticleSubmission, email, ArticleSettings)
                                        End If
                                    Next
                                End If
                            End If

                            If (Settings.Contains(ArticleConstants.NOTIFY_SUBMISSION_SETTING_EMAIL)) Then
                                If (Settings(ArticleConstants.NOTIFY_SUBMISSION_SETTING_EMAIL).ToString() <> "") Then
                                    Dim objEmailTemplateController As New EmailTemplateController
                                    For Each email As String In Settings(ArticleConstants.NOTIFY_SUBMISSION_SETTING_EMAIL).ToString().Split(","c)
                                        objEmailTemplateController.SendFormattedEmail(Me.ModuleId, Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False), objArticle, EmailTemplateType.ArticleSubmission, email, ArticleSettings)
                                    Next
                                End If
                            End If
                        End If

                        Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "SubmitNewsComplete", ArticleSettings), True)
                    End If
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

            Try

                Dim objArticleController As New ArticleController
                objArticleController.DeleteArticle(_articleID, ModuleId)
                If (_returnUrl <> "") Then
                    Response.Redirect(_returnUrl, True)
                Else
                    Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "", ArticleSettings), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdAddEditPages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddEditPages.Click

            Try

                If (Page.IsValid) Then
                    Dim articleID As Integer = SaveArticle()
                    Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "EditPages", ArticleSettings, "ArticleID=" & articleID.ToString()), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdSelectAuthor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectAuthor.Click

            Try

                cmdSelectAuthor.Visible = False
                lblAuthor.Visible = False

                If (ArticleSettings.AuthorSelect = Components.Types.AuthorSelectType.ByDropdown) Then
                    PopulateAuthorList()
                    ddlAuthor.Visible = True

                    If (_articleID <> Null.NullInteger) Then

                        Dim objArticleController As New ArticleController
                        Dim objArticle As ArticleInfo = objArticleController.GetArticle(_articleID)

                        If (objArticle IsNot Nothing) Then
                            If (ddlAuthor.Items.FindByValue(objArticle.AuthorID.ToString()) IsNot Nothing) Then
                                ddlAuthor.SelectedValue = objArticle.AuthorID.ToString()
                            End If
                        End If
                    Else
                        If (ArticleSettings.AuthorDefault) Then
                            If (ddlAuthor.Items.FindByValue(ArticleSettings.AuthorDefault) IsNot Nothing) Then
                                ddlAuthor.SelectedValue = ArticleSettings.AuthorDefault.ToString()
                            End If
                        End If
                    End If
                Else
                    pnlAuthor.Visible = True
                    txtAuthor.Text = lblAuthor.Text
                    txtAuthor.Focus()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try
                If (_returnUrl <> "") Then
                    Response.Redirect(_returnUrl, True)
                Else
                    Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "", ArticleSettings), True)
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub valAuthor_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valAuthor.ServerValidate

            Try

                args.IsValid = False

                If (txtAuthor.Text <> "") Then
                    Dim objUser As UserInfo = UserController.GetUserByName(Me.PortalId, txtAuthor.Text)

                    If (objUser IsNot Nothing) Then
                        args.IsValid = True
                    End If
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub chkMirrorArticle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMirrorArticle.CheckedChanged

            Try

                phCreate.Visible = Not chkMirrorArticle.Checked
                trMirrorModule.Visible = chkMirrorArticle.Checked
                trMirrorArticle.Visible = chkMirrorArticle.Checked
                trMirrorAutoUpdate.Visible = chkMirrorArticle.Checked

                If (chkMirrorArticle.Checked) Then

                    drpMirrorModule.Items.Clear()
                    drpMirrorModule.DataSource = GetContentSharingPortals(ArticleSettings.ContentSharingPortals)
                    drpMirrorModule.DataBind()

                    If (drpMirrorModule.Items.Count > 0) Then

                        Dim objArticleController As New ArticleController()
                        drpMirrorArticle.DataSource = objArticleController.GetArticleList(Convert.ToInt32(drpMirrorModule.SelectedValue.Split("-"c)(1)), True, "StartDate")
                        drpMirrorArticle.DataBind()

                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpMirrorModule_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpMirrorModule.SelectedIndexChanged

            Try

                Dim objArticleController As New ArticleController()
                drpMirrorArticle.DataSource = objArticleController.GetArticleList(Convert.ToInt32(drpMirrorModule.SelectedValue.Split("-"c)(1)), True, "StartDate")
                drpMirrorArticle.DataBind()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Function GetContentSharingPortals(ByVal linkedPortals As String) As List(Of ContentSharingInfo)

            Dim objPortalController As New PortalController()
            Dim objContentSharingPortals As New List(Of ContentSharingInfo)

            For Each element As String In linkedPortals.Split(","c)

                If (element.Split("-"c).Length = 3) Then

                    Dim objContentSharing As New ContentSharingInfo

                    objContentSharing.LinkedPortalID = Convert.ToInt32(element.Split("-")(0))
                    objContentSharing.LinkedTabID = Convert.ToInt32(element.Split("-")(1))
                    objContentSharing.LinkedModuleID = Convert.ToInt32(element.Split("-")(2))

                    Dim objTabController As New TabController
                    Dim objTab As TabInfo = objTabController.GetTab(objContentSharing.LinkedTabID, objContentSharing.LinkedPortalID, False)

                    If (objTab IsNot Nothing) Then
                        objContentSharing.TabTitle = objTab.TabName
                    End If

                    Dim objModuleController As New ModuleController
                    Dim objModule As ModuleInfo = objModuleController.GetModule(objContentSharing.LinkedModuleID, objContentSharing.LinkedTabID)

                    If (objModule IsNot Nothing) Then
                        objContentSharing.ModuleTitle = objModule.ModuleTitle
                        objContentSharingPortals.Add(objContentSharing)
                    End If

                End If

            Next

            Return objContentSharingPortals

        End Function

#End Region

    End Class

End Namespace
