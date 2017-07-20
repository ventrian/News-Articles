Imports System.Web
Imports System
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.IO
Imports System.Text
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Xml

Namespace Ventrian.NewsArticles.Components.Validators
    Public Class CheckBoxListValidator
        Inherits BaseValidator
        <Description("The minimum number of CheckBoxes that must be checked to be considered valid.")> _
        Public Property MinimumNumberOfSelectedCheckBoxes() As Integer
            Get
                Dim o As Object = ViewState("MinimumNumberOfSelectedCheckBoxes")
                If o Is Nothing Then
                    Return 1
                Else
                    Return CInt(o)
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState("MinimumNumberOfSelectedCheckBoxes") = value
            End Set
        End Property

        Private _ctrlToValidate As CheckBoxList = Nothing
        Protected ReadOnly Property CheckBoxListToValidate() As CheckBoxList
            Get
                If _ctrlToValidate Is Nothing Then
                    _ctrlToValidate = TryCast(FindControl(Me.ControlToValidate), CheckBoxList)
                End If

                Return _ctrlToValidate
            End Get
        End Property

        Protected Overloads Overrides Function ControlPropertiesValid() As Boolean
            ' Make sure ControlToValidate is set
            If Me.ControlToValidate.Length = 0 Then
                Throw New HttpException(String.Format("The ControlToValidate property of '{0}' cannot be blank.", Me.ID))
            End If

            ' Ensure that the control being validated is a CheckBoxList
            If CheckBoxListToValidate Is Nothing Then
                Throw New HttpException(String.Format("The CheckBoxListValidator can only validate controls of type CheckBoxList."))
            End If

            ' ... and that it has at least MinimumNumberOfSelectedCheckBoxes ListItems
            'If CheckBoxListToValidate.Items.Count < MinimumNumberOfSelectedCheckBoxes Then
            '    Throw New HttpException(String.Format("MinimumNumberOfSelectedCheckBoxes must be set to a value greater than or equal to the number of ListItems; MinimumNumberOfSelectedCheckBoxes is set to {0}, but there are only {1} ListItems in '{2}'", MinimumNumberOfSelectedCheckBoxes, CheckBoxListToValidate.Items.Count, CheckBoxListToValidate.ID))
            'End If

            Return True
            ' if we reach here, everything checks out
        End Function

        Protected Overloads Overrides Function EvaluateIsValid() As Boolean
            ' Make sure that the CheckBoxList has at least MinimumNumberOfSelectedCheckBoxes ListItems selected
            Dim selectedItemCount As Integer = 0
            For Each cb As ListItem In CheckBoxListToValidate.Items
                If cb.Selected Then
                    selectedItemCount += 1
                End If
            Next

            Return selectedItemCount >= MinimumNumberOfSelectedCheckBoxes
        End Function

        Protected Overloads Overrides Sub AddAttributesToRender(ByVal writer As HtmlTextWriter)
            MyBase.AddAttributesToRender(writer)

            ' Add the client-side code (if needed)
            If Me.RenderUplevel Then
                ' Indicate the mustBeChecked value and the client-side function to used for evaluation
                ' Use AddAttribute if Helpers.EnableLegacyRendering is true; otherwise, use expando attributes
                If EnableLegacyRendering() Then
                    writer.AddAttribute("evaluationfunction", "CheckBoxListValidatorEvaluateIsValid", False)
                    writer.AddAttribute("minimumNumberOfSelectedCheckBoxes", MinimumNumberOfSelectedCheckBoxes.ToString(), False)
                Else
                    Me.Page.ClientScript.RegisterExpandoAttribute(Me.ClientID, "evaluationfunction", "CheckBoxListValidatorEvaluateIsValid", False)
                    Me.Page.ClientScript.RegisterExpandoAttribute(Me.ClientID, "minimumNumberOfSelectedCheckBoxes", MinimumNumberOfSelectedCheckBoxes.ToString(), False)
                End If
            End If
        End Sub

        Protected Overloads Overrides Sub OnPreRender(ByVal e As EventArgs)
            MyBase.OnPreRender(e)

            ' Register the client-side function using WebResource.axd (if needed)
            ' see: http://aspnet.4guysfromrolla.com/articles/080906-1.aspx
            If Me.RenderUplevel AndAlso Me.Page IsNot Nothing AndAlso Not Me.Page.ClientScript.IsClientScriptIncludeRegistered(Me.[GetType](), "VentrianValidators") Then
                Me.Page.ClientScript.RegisterClientScriptInclude(Me.[GetType](), "VentrianValidators", Me.Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Includes/VentrianValidators.js"))
            End If
        End Sub

        Private Function EnableLegacyRendering() As Boolean
            Dim result As Boolean

            Try
                Dim webConfigFile As String = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "web.config")
                Dim webConfigReader As New XmlTextReader(New StreamReader(webConfigFile))
                result = ((webConfigReader.ReadToFollowing("xhtmlConformance")) AndAlso (webConfigReader.GetAttribute("mode") = "Legacy"))
                webConfigReader.Close()
            Catch
                result = False
            End Try
            Return result
        End Function

    End Class
End Namespace
