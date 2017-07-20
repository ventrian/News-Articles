Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.NewsArticles.Components.CustomFields

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucEditCustomField
        Inherits NewsArticleModuleBase

#Region " Private Members "

        Private _customFieldID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub AdjustFieldElements()

            Dim objFieldType As CustomFieldType = CType(System.Enum.Parse(GetType(CustomFieldValidationType), drpFieldType.SelectedIndex.ToString()), CustomFieldType)

            Select Case objFieldType

                Case CustomFieldType.CheckBox
                    phRequired.Visible = True
                    trMaximumLength.Visible = False
                    trFieldElements.Visible = False

                Case CustomFieldType.DropDownList
                    phRequired.Visible = True
                    trMaximumLength.Visible = False
                    trFieldElements.Visible = True

                Case CustomFieldType.MultiCheckBox
                    phRequired.Visible = True
                    trMaximumLength.Visible = False
                    trFieldElements.Visible = True

                Case CustomFieldType.MultiLineTextBox
                    phRequired.Visible = True
                    trMaximumLength.Visible = True
                    trFieldElements.Visible = False

                Case CustomFieldType.OneLineTextBox
                    phRequired.Visible = True
                    trMaximumLength.Visible = True
                    trFieldElements.Visible = False

                Case CustomFieldType.RadioButton
                    phRequired.Visible = True
                    trMaximumLength.Visible = False
                    trFieldElements.Visible = True

                Case CustomFieldType.RichTextBox
                    phRequired.Visible = True
                    trMaximumLength.Visible = False
                    trFieldElements.Visible = False

            End Select

        End Sub

        Private Sub AdjustValidationType()

            If (drpValidationType.SelectedValue = CType(CustomFieldValidationType.Regex, Integer).ToString()) Then
                trRegex.Visible = True
            Else
                trRegex.Visible = False
            End If

        End Sub

        Private Sub BindCustomField()

            If (_customFieldID = Null.NullInteger) Then

                AdjustFieldElements()
                AdjustValidationType()
                cmdDelete.Visible = False

            Else

                Dim objCustomFieldController As New CustomFieldController
                Dim objCustomFieldInfo As CustomFieldInfo = objCustomFieldController.Get(_customFieldID)

                If Not (objCustomFieldInfo Is Nothing) Then

                    txtName.Text = objCustomFieldInfo.Name
                    txtCaption.Text = objCustomFieldInfo.Caption
                    txtCaptionHelp.Text = objCustomFieldInfo.CaptionHelp
                    If Not (drpFieldType.Items.FindByValue(objCustomFieldInfo.FieldType.ToString()) Is Nothing) Then
                        drpFieldType.SelectedValue = objCustomFieldInfo.FieldType.ToString()
                    End If
                    txtFieldElements.Text = objCustomFieldInfo.FieldElements
                    AdjustFieldElements()

                    txtDefaultValue.Text = objCustomFieldInfo.DefaultValue
                    chkVisible.Checked = objCustomFieldInfo.IsVisible
                    If (objCustomFieldInfo.Length <> Null.NullInteger) Then
                        txtMaximumLength.Text = objCustomFieldInfo.Length.ToString()
                    End If

                    chkRequired.Checked = objCustomFieldInfo.IsRequired
                    If Not (drpValidationType.Items.FindByValue(CType(objCustomFieldInfo.ValidationType, Integer).ToString()) Is Nothing) Then
                        drpValidationType.SelectedValue = CType(objCustomFieldInfo.ValidationType, Integer).ToString()
                    End If
                    txtRegex.Text = objCustomFieldInfo.RegularExpression
                    AdjustValidationType()

                End If

            End If


        End Sub

        Private Sub BindFieldTypes()

            For Each value As Integer In System.Enum.GetValues(GetType(CustomFieldType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(CustomFieldType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(CustomFieldType), value), Me.LocalResourceFile)
                drpFieldType.Items.Add(li)
            Next

        End Sub

        Private Sub BindValidationTypes()

            For Each value As Integer In System.Enum.GetValues(GetType(CustomFieldValidationType))
                Dim li As New ListItem
                li.Value = value.ToString()
                li.Text = Localization.GetString(System.Enum.GetName(GetType(CustomFieldValidationType), value), Me.LocalResourceFile)
                drpValidationType.Items.Add(li)
            Next

        End Sub

        Private Sub ReadQueryString()

            If Not (Request("CustomFieldID") Is Nothing) Then
                _customFieldID = Convert.ToInt32(Request("CustomFieldID"))
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                ReadQueryString()

                If (Page.IsPostBack = False) Then

                    BindFieldTypes()
                    BindValidationTypes()
                    BindCustomField()

                    Page.SetFocus(txtName)
                    cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("Confirmation", LocalResourceFile) & "');")

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

            Try

                If (Page.IsValid) Then

                    Dim objCustomFieldController As New CustomFieldController
                    Dim objCustomFieldInfo As New CustomFieldInfo

                    objCustomFieldInfo.ModuleID = Me.ModuleId

                    objCustomFieldInfo.Name = txtName.Text
                    objCustomFieldInfo.Caption = txtCaption.Text
                    objCustomFieldInfo.CaptionHelp = txtCaptionHelp.Text
                    objCustomFieldInfo.FieldType = CType(System.Enum.Parse(GetType(CustomFieldType), drpFieldType.SelectedIndex.ToString()), CustomFieldType)
                    objCustomFieldInfo.FieldElements = txtFieldElements.Text

                    objCustomFieldInfo.DefaultValue = txtDefaultValue.Text
                    objCustomFieldInfo.IsVisible = chkVisible.Checked
                    If (txtMaximumLength.Text.Trim() = "") Then
                        objCustomFieldInfo.Length = Null.NullInteger
                    Else
                        objCustomFieldInfo.Length = Convert.ToInt32(txtMaximumLength.Text)
                        If (objCustomFieldInfo.Length <= 0) Then
                            objCustomFieldInfo.Length = Null.NullInteger
                        End If
                    End If

                    objCustomFieldInfo.IsRequired = chkRequired.Checked
                    objCustomFieldInfo.ValidationType = CType(System.Enum.Parse(GetType(CustomFieldValidationType), drpValidationType.SelectedIndex.ToString()), CustomFieldValidationType)
                    objCustomFieldInfo.RegularExpression = txtRegex.Text

                    If (_customFieldID = Null.NullInteger) Then

                        Dim objCustomFields As ArrayList = objCustomFieldController.List(Me.ModuleId)

                        If (objCustomFields.Count = 0) Then
                            objCustomFieldInfo.SortOrder = 0
                        Else
                            objCustomFieldInfo.SortOrder = CType(objCustomFields(objCustomFields.Count - 1), CustomFieldInfo).SortOrder + 1
                        End If

                        objCustomFieldController.Add(objCustomFieldInfo)

                    Else

                        Dim objCustomFieldInfoOld As CustomFieldInfo = objCustomFieldController.Get(_customFieldID)

                        objCustomFieldInfo.SortOrder = objCustomFieldInfoOld.SortOrder
                        objCustomFieldInfo.CustomFieldID = _customFieldID
                        objCustomFieldController.Update(objCustomFieldInfo)

                    End If

                    Response.Redirect(EditUrl("EditCustomFields"), True)

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub


        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(EditUrl("EditCustomFields"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

            Try

                Dim objCustomFieldController As New CustomFieldController
                objCustomFieldController.Delete(Me.ModuleId, _customFieldID)

                Response.Redirect(EditUrl("EditCustomFields"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpFieldType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpFieldType.SelectedIndexChanged

            Try

                AdjustFieldElements()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpValidationType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpValidationType.SelectedIndexChanged

            Try

                AdjustValidationType()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace