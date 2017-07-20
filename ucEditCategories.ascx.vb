'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucEditCategories
        Inherits NewsArticleModuleBase

#Region " Private Methods "

        Private Sub BindCategories()

            Dim objCategoryController As New CategoryController

            lstChildCategories.DataSource = objCategoryController.GetCategoriesAll(ModuleId, Convert.ToInt32(drpParentCategory.SelectedValue), Nothing, Null.NullInteger, 1, False, ArticleSettings.CategorySortType)
            lstChildCategories.DataBind()

            If (lstChildCategories.Items.Count = 0) Then
                pnlSortOrder.Visible = False
                cmdUpdateSortOrder.Visible = False
                lblNoCategories.Visible = True
            Else
                pnlSortOrder.Visible = True
                cmdUpdateSortOrder.Visible = True
                lblNoCategories.Visible = False
            End If

        End Sub

        Private Sub BindParentCategories()

            Dim objCategoryController As New CategoryController

            drpParentCategory.DataSource = objCategoryController.GetCategoriesAll(ModuleId, Null.NullInteger, ArticleSettings.CategorySortType)
            drpParentCategory.DataBind()

            drpParentCategory.Items.Insert(0, New ListItem(Localization.GetString("NoParentCategory", Me.LocalResourceFile), "-1"))

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If (IsPostBack = False) Then
                    BindParentCategories()
                    BindCategories()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdAddNewCategory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddNewCategory.Click

            Try

                If (drpParentCategory.SelectedValue <> "-1") Then
                    Response.Redirect(EditArticleUrl("EditCategory", "ParentID=" & drpParentCategory.SelectedValue), True)
                Else
                    Response.Redirect(EditArticleUrl("EditCategory"), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub cmdUpdateSortOrder_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdateSortOrder.Click

            Dim objCategoryController As New CategoryController

            Dim index As Integer = 0
            For Each item As ListItem In lstChildCategories.Items
                Dim objCategory As CategoryInfo = objCategoryController.GetCategory(Convert.ToInt32(item.Value), ModuleId)

                If Not (objCategory Is Nothing) Then
                    objCategory.SortOrder = index
                    objCategoryController.UpdateCategory(objCategory)
                    index = index + 1
                End If
            Next

            lblCategoryUpdated.Visible = True

            Dim currentCategory As String = drpParentCategory.SelectedValue
            BindParentCategories()
            If (drpParentCategory.Items.FindByValue(currentCategory) IsNot Nothing) Then
                drpParentCategory.SelectedValue = currentCategory
            End If

        End Sub

        Protected Sub drpParentCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles drpParentCategory.SelectedIndexChanged

            Try

                BindCategories()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub cmdUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdUp.Click

            Try

                If (lstChildCategories.Items.Count > 1) Then
                    If (lstChildCategories.SelectedIndex > 0) Then
                        Dim objListItem As New ListItem

                        objListItem.Value = lstChildCategories.SelectedItem.Value
                        objListItem.Text = lstChildCategories.SelectedItem.Text

                        Dim index As Integer = lstChildCategories.SelectedIndex

                        lstChildCategories.Items.RemoveAt(index)
                        lstChildCategories.Items.Insert(index - 1, objListItem)
                        lstChildCategories.SelectedIndex = index - 1
                    End If
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub cmdDown_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdDown.Click

            Try

                If (lstChildCategories.Items.Count > 1) Then
                    If (lstChildCategories.SelectedIndex < (lstChildCategories.Items.Count - 1)) Then
                        Dim objListItem As New ListItem

                        objListItem.Value = lstChildCategories.SelectedItem.Value
                        objListItem.Text = lstChildCategories.SelectedItem.Text

                        Dim index As Integer = lstChildCategories.SelectedIndex

                        lstChildCategories.Items.RemoveAt(index)
                        lstChildCategories.Items.Insert(index + 1, objListItem)
                        lstChildCategories.SelectedIndex = index + 1
                    End If
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub cmdEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdEdit.Click

            If (lstChildCategories.Items.Count > 0) Then
                If Not (lstChildCategories.SelectedItem Is Nothing) Then
                    Response.Redirect(EditArticleUrl("EditCategory", "CategoryID=" & lstChildCategories.SelectedValue), True)
                End If
            End If

        End Sub

        Protected Sub cmdView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdView.Click

            If (lstChildCategories.Items.Count > 0) Then
                If Not (lstChildCategories.SelectedItem Is Nothing) Then
                    Dim objCategoryController As New CategoryController()
                    Dim objCategory As CategoryInfo = objCategoryController.GetCategory(Convert.ToInt32(lstChildCategories.SelectedValue), ModuleId)

                    If (objCategory IsNot Nothing) Then
                        Response.Redirect(Common.GetCategoryLink(TabId, ModuleId, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings), True)
                    End If
                End If
            End If

        End Sub

#End Region

    End Class

End Namespace
