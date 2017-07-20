'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucEditPageSortOrder
        Inherits NewsArticleModuleBase

#Region " Private Members "

        Private _articleID As Integer

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            If (IsNumeric(Request("ArticleID"))) Then
                _articleID = Convert.ToInt32(Request("ArticleID"))
            End If

        End Sub

        Private Sub CheckSecurity()

            If (HasEditRights(_articleID, Me.ModuleId, Me.TabId) = False) Then
                Response.Redirect(Common.GetModuleLink(Me.TabId, Me.ModuleId, "NotAuthorized", ArticleSettings), True)
            End If

        End Sub

        Private Sub BindOrder()

            Dim objPageController As PageController = New PageController

            lstSortOrder.DataSource = objPageController.GetPageList(_articleID)
            lstSortOrder.DataBind()

            If (lstSortOrder.Items.Count = 0) Then
                Response.Redirect(Common.GetModuleLink(Me.TabId, Me.ModuleId, "EditPages", ArticleSettings, "ArticleID=" & _articleID.ToString()), True)
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                ReadQueryString()

                If (IsPostBack = False) Then

                    CheckSecurity()
                    BindOrder()

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

            Try

                Dim objPageController As PageController = New PageController

                For i As Integer = 0 To lstSortOrder.Items.Count - 1

                    Dim objPage As PageInfo = objPageController.GetPage(Convert.ToInt32(lstSortOrder.Items(i).Value))
                    objPage.SortOrder = i
                    objPageController.UpdatePage(objPage)

                Next

                Response.Redirect(Common.GetModuleLink(Me.TabId, Me.ModuleId, "EditPages", ArticleSettings, "ArticleID=" & _articleID.ToString()), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(Common.GetModuleLink(Me.TabId, Me.ModuleId, "EditPages", ArticleSettings, "ArticleID=" & _articleID.ToString()), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub upBtn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles upBtn.Click

            Try

                If (lstSortOrder.SelectedIndex <> -1) Then

                    If (lstSortOrder.SelectedIndex <> 0) Then

                        Dim tempIndex As Integer = lstSortOrder.SelectedIndex

                        Dim newListItem As ListItem = New ListItem

                        newListItem.Text = lstSortOrder.SelectedItem.Text
                        newListItem.Value = lstSortOrder.SelectedItem.Value

                        lstSortOrder.Items.RemoveAt(tempIndex)

                        lstSortOrder.Items.Insert(tempIndex - 1, newListItem)
                        lstSortOrder.SelectedIndex = tempIndex - 1

                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub downBtn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles downBtn.Click

            Try

                If (lstSortOrder.SelectedIndex <> -1) Then

                    If (lstSortOrder.SelectedIndex <> lstSortOrder.Items.Count - 1) Then

                        Dim tempIndex As Integer = lstSortOrder.SelectedIndex

                        Dim newListItem As ListItem = New ListItem

                        newListItem.Text = lstSortOrder.SelectedItem.Text
                        newListItem.Value = lstSortOrder.SelectedItem.Value

                        lstSortOrder.Items.RemoveAt(tempIndex)

                        lstSortOrder.Items.Insert(tempIndex + 1, newListItem)
                        lstSortOrder.SelectedIndex = tempIndex + 1

                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region
    End Class

End Namespace