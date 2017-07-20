Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucEditTags
        Inherits NewsArticleModuleBase

#Region " Private Methods "

        Private Sub BindTags()

            Dim objTagController As New TagController

            Localization.LocalizeDataGrid(grdTags, Me.LocalResourceFile)

            grdTags.DataSource = objTagController.List(Me.ModuleId, Null.NullInteger)
            grdTags.DataBind()

            If (grdTags.Items.Count > 0) Then
                grdTags.Visible = True
                lblNoTags.Visible = False
            Else
                grdTags.Visible = False
                lblNoTags.Visible = True
                lblNoTags.Text = Localization.GetString("NoTagsMessage.Text", LocalResourceFile)
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                BindTags()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdAddTag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddTag.Click

            Response.Redirect(EditUrl("EditTag"), True)

        End Sub

#End Region

    End Class

End Namespace