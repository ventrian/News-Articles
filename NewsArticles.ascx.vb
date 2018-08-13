'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2005-2012
' by Ventrian ( support@ventrian.com ) ( http://www.ventrian.com )
'

Imports Ventrian.NewsArticles.Components.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Framework
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Web.Client
Imports DotNetNuke.Web.Client.ClientResourceManagement

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class NewsArticles
        Inherits NewsArticleModuleBase

#Region " Private Members "

        Private m_controlToLoad As String

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            If Not (Request("articleType") Is Nothing) Then

                ' Load the appropriate Control
                '
                Select Case Request("articleType").ToLower()

                    Case "archives"
                        m_controlToLoad = "Archives.ascx"

                    Case "approvearticles"
                        m_controlToLoad = "ucApproveArticles.ascx"

                    Case "approvecomments"
                        m_controlToLoad = "ucApproveComments.ascx"

                    Case "articleview"
                        m_controlToLoad = "ViewArticle.ascx"

                    Case "archiveview"
                        m_controlToLoad = "ViewArchive.ascx"

                    Case "authorview"
                        m_controlToLoad = "ViewAuthor.ascx"

                    Case "categories"
                        Response.Status = "301 Moved Permanently"
                        Response.AddHeader("Location", Common.GetModuleLink(Me.TabId, Me.ModuleId, "Archives", ArticleSettings))
                        Response.End()

                    Case "categoryview"
                        m_controlToLoad = "ViewCategory.ascx"

                    Case "editcomment"
                        m_controlToLoad = "ucEditComment.ascx"

                    Case "editpage"
                        m_controlToLoad = "ucEditPage.ascx"

                    Case "editpages"
                        m_controlToLoad = "ucEditPages.ascx"

                    Case "editsortorder"
                        m_controlToLoad = "ucEditPageSortOrder.ascx"

                    Case "myarticles"
                        m_controlToLoad = "ucMyArticles.ascx"

                    Case "notauthenticated"
                        m_controlToLoad = "ucNotAuthenticated.ascx"

                    Case "notauthorized"
                        m_controlToLoad = "ucNotAuthorized.ascx"

                    Case "search"
                        m_controlToLoad = "ViewSearch.ascx"

                    Case "submitnews"

                        m_controlToLoad = "ucSubmitNews.ascx"

                    Case "submitnewscomplete"

                        m_controlToLoad = "ucSubmitNewsComplete.ascx"

                    Case "syndication"
                        Response.Status = "301 Moved Permanently"
                        Response.AddHeader("Location", Common.GetModuleLink(Me.TabId, Me.ModuleId, "Archives", ArticleSettings))
                        Response.End()

                    Case "tagview"
                        m_controlToLoad = "ViewTag.ascx"

                    Case Else

                        m_controlToLoad = "ViewCurrent.ascx"

                End Select

            Else

                ' Type parameter not found
                '
                If (ArticleSettings.FilterSingleCategory <> Null.NullInteger) Then
                    m_controlToLoad = "ViewCategory.ascx"
                Else
                    If (ArticleSettings.UrlModeType = Components.Types.UrlModeType.Classic) Then
                        m_controlToLoad = "ViewCurrent.ascx"
                    Else
                        If (Request(ArticleSettings.ShortenedID) <> "") Then
                            m_controlToLoad = "ViewArticle.ascx"
                        Else
                            m_controlToLoad = "ViewCurrent.ascx"
                        End If
                    End If
                End If

            End If

        End Sub

        Private Sub LoadControlType()

            If (m_controlToLoad <> "") Then
                Dim objPortalModuleBase As PortalModuleBase = CType(Me.LoadControl(m_controlToLoad), PortalModuleBase)

                If Not (objPortalModuleBase Is Nothing) Then

                    objPortalModuleBase.ModuleConfiguration = Me.ModuleConfiguration
                    objPortalModuleBase.ID = System.IO.Path.GetFileNameWithoutExtension(m_controlToLoad)
                    plhControls.Controls.Add(objPortalModuleBase)

                End If
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                ReadQueryString()
                LoadControlType()
                
                Dim litLinkLiveWriter As New Literal
                litLinkLiveWriter.Text = "" & vbCrLf _
                    & "<link rel=""wlwmanifest"" type=""application/wlwmanifest+xml"" title=""windows livewriter manifest"" href=""" & ArticleUtilities.ToAbsoluteUrl("~/desktopmodules/dnnforge%20-%20newsarticles/api/metaweblog/wlwmanifest.xml") & """ />" & vbCrLf
                Page.Header.Controls.Add(litLinkLiveWriter)

                Dim litLinkRsd As New Literal
                litLinkRsd.Text = "" & vbCrLf _
                    & "<link type=""application/rsd+xml"" rel=""EditURI"" title=""RSD"" href=""" & ArticleUtilities.ToAbsoluteUrl("~/desktopmodules/dnnforge%20-%20newsarticles/api/rsd.ashx") & "?id=" & TabModuleId & "&url=" & (DotNetNuke.Common.Globals.NavigateURL(TabId)) & """ />" & vbCrLf
                Page.Header.Controls.Add(litLinkRsd)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.PreRender

            Try

                'jQuery.RegisterJQuery(Page)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace