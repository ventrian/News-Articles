Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Localization

Imports Ventrian.NewsArticles.Components.CustomFields

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucEditCustomFields
        Inherits NewsArticleModuleBase

#Region " Private Members "

        Dim _customFields As ArrayList

#End Region

#Region " Private Methods "

        Private Sub BindCustomFields()

            Localization.LocalizeDataGrid(grdCustomFields, Me.LocalResourceFile)

            Dim objCustomFieldController As New CustomFieldController()

            _customFields = objCustomFieldController.List(Me.ModuleId)
            grdCustomFields.DataSource = _customFields

            grdCustomFields.DataBind()

            If (grdCustomFields.Items.Count = 0) Then
                grdCustomFields.Visible = False
                lblNoCustomFields.Visible = True
            Else
                grdCustomFields.Visible = True
                lblNoCustomFields.Visible = False
            End If

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function GetCustomFieldEditUrl(ByVal customFieldID As String) As String

            Return EditArticleUrl("EditCustomField", "CustomFieldID=" & customFieldID)

        End Function

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If (Page.IsPostBack = False) Then
                BindCustomFields()
            End If

        End Sub

        Protected Sub cmdAddCustomField_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAddCustomField.Click

            Response.Redirect(EditArticleUrl("EditCustomField"), True)

        End Sub

        Private Sub grdCustomFields_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdCustomFields.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim btnUp As ImageButton = CType(e.Item.FindControl("btnUp"), ImageButton)
                Dim btnDown As ImageButton = CType(e.Item.FindControl("btnDown"), ImageButton)

                Dim objCustomField As CustomFieldInfo = CType(e.Item.DataItem, CustomFieldInfo)

                If Not (btnUp Is Nothing And btnDown Is Nothing) Then

                    If (objCustomField.CustomFieldID = CType(_customFields(0), CustomFieldInfo).CustomFieldID) Then
                        btnUp.Visible = False
                    End If

                    If (objCustomField.CustomFieldID = CType(_customFields(_customFields.Count - 1), CustomFieldInfo).CustomFieldID) Then
                        btnDown.Visible = False
                    End If

                    btnUp.CommandArgument = objCustomField.CustomFieldID.ToString()
                    btnUp.CommandName = "Up"

                    btnDown.CommandArgument = objCustomField.CustomFieldID.ToString()
                    btnDown.CommandName = "Down"

                End If

            End If

        End Sub

        Private Sub grdCustomFields_ItemCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdCustomFields.ItemCommand

            Dim objCustomFieldController As New CustomFieldController
            _customFields = objCustomFieldController.List(Me.ModuleId)

            Dim customFieldID As Integer = Convert.ToInt32(e.CommandArgument)

            For i As Integer = 0 To _customFields.Count - 1

                Dim objCustomField As CustomFieldInfo = CType(_customFields(i), CustomFieldInfo)

                If (customFieldID = objCustomField.CustomFieldID) Then

                    If (e.CommandName = "Up") Then

                        Dim objCustomFieldToSwap As CustomFieldInfo = CType(_customFields(i - 1), CustomFieldInfo)

                        Dim sortOrder As Integer = objCustomField.SortOrder
                        Dim sortOrderPrevious As Integer = objCustomFieldToSwap.SortOrder

                        objCustomField.SortOrder = sortOrderPrevious
                        objCustomFieldToSwap.SortOrder = sortOrder

                        objCustomFieldController.Update(objCustomField)
                        objCustomFieldController.Update(objCustomFieldToSwap)

                    End If


                    If (e.CommandName = "Down") Then

                        Dim objCustomFieldToSwap As CustomFieldInfo = CType(_customFields(i + 1), CustomFieldInfo)

                        Dim sortOrder As Integer = objCustomField.SortOrder
                        Dim sortOrderNext As Integer = objCustomFieldToSwap.SortOrder

                        objCustomField.SortOrder = sortOrderNext
                        objCustomFieldToSwap.SortOrder = sortOrder

                        objCustomFieldController.Update(objCustomField)
                        objCustomFieldController.Update(objCustomFieldToSwap)

                    End If

                End If

            Next

            Response.Redirect(Request.RawUrl, True)

        End Sub

#End Region

    End Class

End Namespace