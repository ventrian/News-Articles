Imports DotNetNuke.Services.Exceptions
Imports Ventrian.NewsArticles.Components.Types

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucHeader
        Inherits NewsArticleControlBase

#Region " Private Members "

        Private _selectedMenu As MenuOptionType = MenuOptionType.CurrentArticles
        Private _menuPosition As MenuPositionType = MenuPositionType.Top
        Private _processMenu As Boolean = False

#End Region

#Region " Public Properties "

        Public WriteOnly Property MenuPosition() As String
            Set(ByVal value As String)
                Select Case value.ToLower()

                    Case "top"
                        _menuPosition = MenuPositionType.Top
                        Return

                    Case "bottom"
                        _menuPosition = MenuPositionType.Bottom
                        Return

                End Select
            End Set
        End Property

        Public WriteOnly Property SelectedMenu() As String
            Set(ByVal value As String)
                Select Case Value.ToLower()

                    Case "adminoptions"
                        _selectedMenu = MenuOptionType.AdminOptions
                        Return

                    Case "approvearticles"
                        _selectedMenu = MenuOptionType.ApproveArticles
                        Return

                    Case "approvecomments"
                        _selectedMenu = MenuOptionType.ApproveComments
                        Return

                    Case "categories"
                        _selectedMenu = MenuOptionType.Categories
                        Return

                    Case "currentarticles"
                        _selectedMenu = MenuOptionType.CurrentArticles
                        Return

                    Case "myarticles"
                        _selectedMenu = MenuOptionType.MyArticles
                        Return

                    Case "search"
                        _selectedMenu = MenuOptionType.Search
                        Return

                    Case "syndication"
                        _selectedMenu = MenuOptionType.Syndication
                        Return

                    Case "submitarticle"
                        _selectedMenu = MenuOptionType.SubmitArticle
                        Return

                End Select
            End Set
        End Property

#End Region

#Region " Event Handlers "

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                If (_processMenu = False And _menuPosition = ArticleModuleBase.ArticleSettings.MenuPosition) Then
                    TokenProcessor.ProcessMenu(plhControls.Controls, ArticleModuleBase, _selectedMenu)
                    _processMenu = True
                End If

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace
