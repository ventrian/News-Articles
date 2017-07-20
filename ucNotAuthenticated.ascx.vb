'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucNotAuthenticated
        Inherits NewsArticleModuleBase

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        End Sub

#End Region

#Region " Public Methods "

        Protected Function GetLoginUrl() As String

            Try

                If PortalSettings.LoginTabId <> Null.NullInteger Then

                    ' User Defined Tab
                    '
                    Return Page.ResolveUrl("~/Default.aspx?tabid=" & PortalSettings.LoginTabId.ToString)

                Else

                    ' Admin Tab
                    '
                    Return Page.ResolveUrl("~/Default.aspx?tabid=" & PortalSettings.ActiveTab.TabID & "&ctl=Login")

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

            Return ""

        End Function

#End Region

    End Class

End Namespace