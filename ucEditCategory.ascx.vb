'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Entities.Modules

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucEditCategory
        Inherits NewsArticleModuleBase

#Region " Private Members "

        Private _categoryID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            If (IsNumeric(Request("CategoryID"))) Then
                _categoryID = Convert.ToInt32(Request("CategoryID"))
            End If

        End Sub

        Private Sub BindCategory()

            If (_categoryID = Null.NullInteger) Then
                cmdDelete.Visible = False
                trPermissions.Visible = False
                trSecurityMode.Visible = False
                chkInheritSecurity.Checked = True
                lstSecurityMode.SelectedIndex = 0
                Return
            End If

            Dim objCategoryController As CategoryController = New CategoryController
            Dim objCategoryInfo As CategoryInfo = objCategoryController.GetCategory(_categoryID, ModuleId)

            If Not (objCategoryInfo Is Nothing) Then
                If (drpParentCategory.Items.FindByValue(objCategoryInfo.ParentID.ToString()) IsNot Nothing) Then
                    drpParentCategory.SelectedValue = objCategoryInfo.ParentID.ToString()
                End If
                txtName.Text = objCategoryInfo.Name
                txtDescription.Text = objCategoryInfo.Description
                ctlIcon.Url = objCategoryInfo.Image
                cmdDelete.Visible = True
                chkInheritSecurity.Checked = objCategoryInfo.InheritSecurity
                trPermissions.Visible = Not chkInheritSecurity.Checked
                trSecurityMode.Visible = Not chkInheritSecurity.Checked
                lstSecurityMode.SelectedValue = Convert.ToInt32(objCategoryInfo.CategorySecurityType).ToString()

                txtMetaTitle.Text = objCategoryInfo.MetaTitle
                txtMetaDescription.Text = objCategoryInfo.MetaDescription
                txtMetaKeyWords.Text = objCategoryInfo.MetaKeywords
            End If

        End Sub

        Private Sub BindParentCategories()

            Dim objCategoryController As New CategoryController()
            drpParentCategory.DataSource = objCategoryController.GetCategoriesAll(Me.ModuleId, Null.NullInteger, ArticleSettings.CategorySortType)
            drpParentCategory.DataBind()

            drpParentCategory.Items.Insert(0, New ListItem(Localization.GetString("NoParentCategory", Me.LocalResourceFile), Null.NullInteger.ToString()))

            If (Request("ParentID") <> "") Then
                If (drpParentCategory.Items.FindByValue(Request("ParentID")) IsNot Nothing) Then
                    drpParentCategory.SelectedValue = Request("ParentID")
                End If
            End If

        End Sub

        Private Sub BindRoles()

            Dim objRole As New RoleController
            Dim availableRoles As New ArrayList
            Dim roles As ArrayList = objRole.GetPortalRoles(PortalId)

            If Not roles Is Nothing Then
                For Each Role As RoleInfo In roles
                    availableRoles.Add(New ListItem(Role.RoleName, Role.RoleName))
                Next
            End If

            grdCategoryPermissions.DataSource = availableRoles
            grdCategoryPermissions.DataBind()

        End Sub

        Private Sub BindSecurityMode()

            For Each value As Integer In System.Enum.GetValues(GetType(CategorySecurityType))
                Dim li As New ListItem
                li.Value = value
                li.Text = Localization.GetString(System.Enum.GetName(GetType(CategorySecurityType), value), Me.LocalResourceFile)
                lstSecurityMode.Items.Add(li)
            Next

        End Sub

        Private Function IsInRole(ByVal roleName As String, ByVal roles As String()) As Boolean

            For Each role As String In roles
                If (roleName = role) Then
                    Return True
                End If
            Next

            Return False

        End Function

        Private Sub SaveCategory()

            Dim objCategoryInfo As New CategoryInfo

            objCategoryInfo.CategoryID = _categoryID
            objCategoryInfo.ModuleID = ModuleId
            objCategoryInfo.ParentID = Convert.ToInt32(drpParentCategory.SelectedValue)
            objCategoryInfo.Name = txtName.Text
            objCategoryInfo.Description = txtDescription.Text
            objCategoryInfo.Image = ctlIcon.Url
            objCategoryInfo.InheritSecurity = chkInheritSecurity.Checked
            objCategoryInfo.CategorySecurityType = lstSecurityMode.SelectedValue

            objCategoryInfo.MetaTitle = txtMetaTitle.Text
            objCategoryInfo.MetaDescription = txtMetaDescription.Text
            objCategoryInfo.MetaKeywords = txtMetaKeyWords.Text

            Dim objCategoryController As New CategoryController

            If (objCategoryInfo.CategoryID = Null.NullInteger) Then
                Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategories(ModuleId, objCategoryInfo.ParentID)

                objCategoryInfo.SortOrder = 0
                If (objCategories.Count > 0) Then
                    objCategoryInfo.SortOrder = CType(objCategories(objCategories.Count - 1), CategoryInfo).SortOrder + 1
                End If
                objCategoryInfo.CategoryID = objCategoryController.AddCategory(objCategoryInfo)
            Else
                Dim objCategoryOld As CategoryInfo = objCategoryController.GetCategory(objCategoryInfo.CategoryID, ModuleId)

                If (objCategoryOld IsNot Nothing) Then
                    objCategoryInfo.SortOrder = objCategoryOld.SortOrder
                    If (objCategoryInfo.ParentID <> objCategoryOld.ParentID) Then
                        Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategories(ModuleId, objCategoryInfo.ParentID)
                        If (objCategories.Count > 0) Then
                            objCategoryInfo.SortOrder = CType(objCategories(objCategories.Count - 1), CategoryInfo).SortOrder + 1
                        End If
                    End If
                End If
                objCategoryController.UpdateCategory(objCategoryInfo)
            End If

            Dim viewRoles As String = ""
            Dim submitRoles As String = ""

            For Each item As DataGridItem In grdCategoryPermissions.Items
                Dim role As String = grdCategoryPermissions.DataKeys(item.ItemIndex).ToString()

                Dim chkView As CheckBox = CType(item.FindControl("chkView"), CheckBox)
                If (chkView.Checked) Then
                    If (viewRoles = "") Then
                        viewRoles = role
                    Else
                        viewRoles = viewRoles & ";" & role
                    End If
                End If

                Dim chkSubmit As CheckBox = CType(item.FindControl("chkSubmit"), CheckBox)
                If (chkSubmit.Checked) Then
                    If (submitRoles = "") Then
                        submitRoles = role
                    Else
                        submitRoles = submitRoles & ";" & role
                    End If
                End If
            Next

            Dim objModuleController As New ModuleController()
            objModuleController.UpdateModuleSetting(Me.ModuleId, objCategoryInfo.CategoryID.ToString() & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING, viewRoles)
            objModuleController.UpdateModuleSetting(Me.ModuleId, objCategoryInfo.CategoryID.ToString() & "-" & ArticleConstants.PERMISSION_CATEGORY_SUBMIT_SETTING, submitRoles)

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                ReadQueryString()

                If (IsPostBack = False) Then
                    BindSecurityMode()
                    BindRoles()
                    BindParentCategories()
                    BindCategory()
                    Page.SetFocus(txtName)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

            Try

                If (Page.IsValid) Then

                    SaveCategory()

                    Response.Redirect(EditArticleUrl("EditCategories"), True)

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(EditArticleUrl("EditCategories"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

            Try

                Dim objCategoryController As New CategoryController
                Dim objCategory As CategoryInfo = objCategoryController.GetCategory(_categoryID, ModuleId)

                If (objCategory IsNot Nothing) Then

                    Dim objChildCategories As List(Of CategoryInfo) = objCategoryController.GetCategories(Me.ModuleId, _categoryID)

                    For Each objChildCategory As CategoryInfo In objChildCategories
                        objChildCategory.ParentID = objCategory.ParentID
                        objCategoryController.UpdateCategory(objChildCategory)
                    Next

                    objCategoryController.DeleteCategory(_categoryID, ModuleId)

                End If

                Response.Redirect(EditArticleUrl("EditCategories"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub valInvalidParentCategory_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valInvalidParentCategory.ServerValidate

            Try

                If (_categoryID = Null.NullInteger Or drpParentCategory.SelectedValue = "-1") Then
                    args.IsValid = True
                    Return
                End If

                Dim objCategoryController As New CategoryController
                Dim objCategory As CategoryInfo = objCategoryController.GetCategory(Convert.ToInt32(drpParentCategory.SelectedValue), ModuleId)

                While Not objCategory Is Nothing
                    If (_categoryID = objCategory.CategoryID) Then
                        args.IsValid = False
                        Return
                    End If
                    objCategory = objCategoryController.GetCategory(objCategory.ParentID, objCategory.ModuleID)
                End While

                args.IsValid = True

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub grdCategoryPermissions_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdCategoryPermissions.ItemDataBound

            Try

                If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                    Dim objListItem As ListItem = CType(e.Item.DataItem, ListItem)

                    If Not (objListItem Is Nothing) Then

                        Dim role As String = CType(e.Item.DataItem, ListItem).Value

                        Dim chkView As CheckBox = CType(e.Item.FindControl("chkView"), CheckBox)
                        Dim chkSubmit As CheckBox = CType(e.Item.FindControl("chkSubmit"), CheckBox)

                        If (objListItem.Value = PortalSettings.AdministratorRoleName.ToString()) Then
                            chkView.Enabled = False
                            chkView.Checked = True
                            chkSubmit.Enabled = False
                            chkSubmit.Checked = True
                        Else
                            If (_categoryID <> Null.NullInteger) Then
                                If (Settings.Contains(_categoryID.ToString() & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                    chkView.Checked = IsInRole(role, Settings(_categoryID.ToString() & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString().Split(";"c))
                                End If
                                If (Settings.Contains(_categoryID.ToString() & "-" & ArticleConstants.PERMISSION_CATEGORY_SUBMIT_SETTING)) Then
                                    chkSubmit.Checked = IsInRole(role, Settings(_categoryID.ToString() & "-" & ArticleConstants.PERMISSION_CATEGORY_SUBMIT_SETTING).ToString().Split(";"c))
                                End If
                            End If
                        End If

                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub chkInheritSecurity_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkInheritSecurity.CheckedChanged

            Try

                trPermissions.Visible = Not chkInheritSecurity.Checked
                trSecurityMode.Visible = Not chkInheritSecurity.Checked

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace