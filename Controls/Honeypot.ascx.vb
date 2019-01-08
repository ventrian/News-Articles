Imports System.IO
Imports System.Net
Imports System.Web.Script.Serialization
Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles.Controls
    Public Class Honeypot
        Inherits System.Web.UI.UserControl

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

        Protected Sub HoneypotValidator_OnServerValidate(source As Object, args As ServerValidateEventArgs)
            args.IsValid = IsValid()
        End Sub

        Public Function IsValid() As Boolean
            Return txtConfirmEmail.Text = ""
        End Function
    End Class

End Namespace