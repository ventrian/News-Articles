'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ViewTag
        Inherits NewsArticleModuleBase

#Region " Constants "

        Private Const PARAM_TAG As String = "Tag"

#End Region

#Region " Private Members "

        Private _tag As String = Null.NullString

#End Region

#Region " Private Methods "

        Private Sub BindTag()

            If (_tag = Null.NullString) Then
                ' Author not specified
                Response.Redirect(NavigateURL(), True)
            End If

            Dim objTagController As New TagController
            Dim objTag As TagInfo = objTagController.Get(Me.ModuleId, _tag.ToLower())

            If (objTag IsNot Nothing) Then

                Dim entriesFrom As String = Localization.GetString("TagEntries", LocalResourceFile)

                If (entriesFrom.Contains("{0}")) Then
                    lblTag.Text = String.Format(entriesFrom, _tag)
                Else
                    lblTag.Text = _tag
                End If

                Me.BasePage.Title = _tag & " | " & PortalSettings.PortalName
                Me.BasePage.Description = entriesFrom

                ' We never want to index the tag pages. 


            Else

                ' Author not found.
                Response.Redirect(NavigateURL(), True)

            End If

        End Sub

        Private Sub ReadQueryString()

            If (Request(PARAM_TAG) <> "") Then
                _tag = Server.UrlDecode(Request(PARAM_TAG))
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                ReadQueryString()
                BindTag()

                Listing1.Tag = _tag
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