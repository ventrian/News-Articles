'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.UI.UserControls

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucEditPage
        Inherits NewsArticleModuleBase

#Region " Private Properties "

        Private ReadOnly Property Summary() As TextEditor
            Get
                Return CType(txtSummary, TextEditor)
            End Get
        End Property

#End Region

#Region " Private Members "

        Private _pageID As Integer = Null.NullInteger
        Private _articleID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            If (IsNumeric(Request("ArticleID"))) Then
                _articleID = Convert.ToInt32(Request("ArticleID"))
            End If

            If (IsNumeric(Request("PageID"))) Then
                _pageID = Convert.ToInt32(Request("PageID"))
            End If


        End Sub

        Private Sub CheckSecurity()

            If (HasEditRights(_articleID, Me.ModuleId, Me.TabId) = False) Then
                Response.Redirect(Common.GetModuleLink(Me.TabId, Me.ModuleId, "NotAuthorized", ArticleSettings), True)
            End If

        End Sub

        Private Sub BindPage()

            If (_pageID = Null.NullInteger) Then

                cmdDelete.Visible = False
            Else

                cmdDelete.Visible = True
                cmdDelete.Attributes.Add("onClick", "javascript:return confirm('Are You Sure You Wish To Delete This Item ?');")

                Dim objPageController As New PageController
                Dim objPage As PageInfo = objPageController.GetPage(_pageID)

                If Not (objPage Is Nothing) Then
                    txtTitle.Text = objPage.Title
                    Summary.Text = objPage.PageText
                End If

            End If

        End Sub

        Private Sub SetTextEditor()

            Summary.Width = Unit.Parse(ArticleSettings.TextEditorWidth)
            Summary.Height = Unit.Parse(ArticleSettings.TextEditorHeight)

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                ReadQueryString()
                CheckSecurity()

                If (IsPostBack = False) Then

                    BindPage()
                    SetTextEditor()

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

            Try

                If (Page.IsValid) Then

                    Dim objPageController As PageController = New PageController

                    Dim objPage As PageInfo = New PageInfo

                    If (_pageID <> Null.NullInteger) Then

                        objPage = objPageController.GetPage(_pageID)

                    Else

                        objPage = CType(CBO.InitializeObject(objPage, GetType(PageInfo)), PageInfo)

                    End If

                    objPage.Title = txtTitle.Text
                    objPage.PageText = Summary.Text
                    objPage.ArticleID = _articleID

                    If (_pageID = Null.NullInteger) Then

                        objPageController.AddPage(objPage)

                    Else

                        If (objPage.SortOrder = 0) Then
                            Dim objArticleController As New ArticleController()
                            Dim objArticle As ArticleInfo = objArticleController.GetArticle(_articleID)

                            If (objArticle IsNot Nothing) Then
                                If (objArticle.Title <> objPage.Title) Then
                                    objArticle.Title = objPage.Title
                                    objArticleController.UpdateArticle(objArticle)
                                End If
                            End If
                        End If

                        objPageController.UpdatePage(objPage)

                    End If

                    Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "EditPages", ArticleSettings, "ArticleID=" & _articleID.ToString()), True)

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

            Try

                Dim objPageController As New PageController
                objPageController.DeletePage(_pageID)

                Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "EditPages", ArticleSettings, "ArticleID=" & _articleID.ToString()), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(Common.GetModuleLink(TabId, ModuleId, "EditPages", ArticleSettings, "ArticleID=" & _articleID.ToString()), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace