'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Services.Exceptions

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucEmailTemplates
        Inherits NewsArticleModuleBase

#Region " Private Methods "

        Private Sub BindTemplateTypes()

            drpTemplate.DataSource = System.Enum.GetValues(GetType(EmailTemplateType))
            drpTemplate.DataBind()

        End Sub

        Private Sub BindTemplate()

            Dim objTemplateController As New EmailTemplateController

            Dim objTemplate As EmailTemplateInfo = objTemplateController.Get(Me.ModuleId, CType(System.Enum.Parse(GetType(EmailTemplateType), drpTemplate.SelectedValue), EmailTemplateType))

            If Not (objTemplate Is Nothing) Then

                txtSubject.Text = objTemplate.Subject
                txtTemplate.Text = objTemplate.Template

            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If (IsPostBack = False) Then
                    BindTemplateTypes()
                    BindTemplate()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpTemplate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpTemplate.SelectedIndexChanged

            Try

                BindTemplate()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

            Try

                Dim objTemplateController As New EmailTemplateController

                Dim objTemplate As EmailTemplateInfo = objTemplateController.Get(Me.ModuleId, CType(System.Enum.Parse(GetType(EmailTemplateType), drpTemplate.SelectedValue), EmailTemplateType))

                objTemplate.Subject = txtSubject.Text
                objTemplate.Template = txtTemplate.Text

                objTemplateController.Update(objTemplate)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(EditArticleUrl("AdminOptions"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace