'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ViewAuthor
        Inherits NewsArticleModuleBase

#Region " Constants "

        Private Const PARAM_AUTHOR_ID As String = "AuthorID"

#End Region

#Region " Private Members "

        Private _authorID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub BindAuthorName()

            If (_authorID = Null.NullInteger) Then
                ' Author not specified
                Response.Redirect(NavigateURL(), True)
            End If

            Dim objUserController As New UserController
            Dim objUser As UserInfo = objUserController.GetUser(Me.PortalId, _authorID)

            If (objUser IsNot Nothing) Then

                If (objUser.PortalID <> Me.PortalId) Then
                    'Author does not belong to this portal
                    Response.Redirect(NavigateURL(), True)
                End If

                Dim name As String = ""
                Select Case ArticleSettings.DisplayMode
                    Case DisplayType.FirstName
                        name = objUser.FirstName
                        Exit Select

                    Case DisplayType.FullName
                        name = objUser.DisplayName
                        Exit Select

                    Case DisplayType.LastName
                        name = objUser.LastName
                        Exit Select

                    Case DisplayType.UserName
                        name = objUser.Username
                        Exit Select
                End Select

                Dim entriesFrom As String = Localization.GetString("AuthorEntries", LocalResourceFile)

                If (entriesFrom.Contains("{0}")) Then
                    lblAuthor.Text = String.Format(entriesFrom, name)
                Else
                    lblAuthor.Text = name
                End If
                
                Me.BasePage.Title = name & " | " & PortalSettings.PortalName
                Me.BasePage.Description = lblAuthor.Text

            Else

                ' Author not found.
                Response.Redirect(NavigateURL(), True)

            End If

        End Sub

        Private Sub ReadQueryString()

            If (Request(PARAM_AUTHOR_ID) <> "" AndAlso IsNumeric(Request(PARAM_AUTHOR_ID))) Then
                _authorID = Convert.ToInt32(Request(PARAM_AUTHOR_ID))
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                ReadQueryString()
                BindAuthorName()

                Listing1.Author = _authorID
                Listing1.ShowExpired = True
                Listing1.MaxArticles = Null.NullInteger
                Listing1.IsIndexed = False

                Listing1.BindListing()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace