'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2005-2012
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.IO

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Framework


Namespace Ventrian.NewsArticles

    Partial Public Class Print
        Inherits PageBase

#Region " Private Members "

        Private _articleID As Integer = Null.NullInteger
        Private _moduleID As Integer = Null.NullInteger
        Private _tabModuleID As Integer = Null.NullInteger
        Private _tabID As Integer = Null.NullInteger
        Private _pageID As Integer = Null.NullInteger
        Private _portalID As Integer = Null.NullInteger
        Private _template As String = Null.NullString

        Private _articleSettings As ArticleSettings

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            If Not (Request("ArticleID") Is Nothing) Then
                _articleID = Convert.ToInt32(Request("ArticleID"))
            End If

            If Not (Request("ModuleID") Is Nothing) Then
                _moduleID = Convert.ToInt32(Request("ModuleID"))
            End If

            If Not (Request("TabID") Is Nothing) Then
                _tabID = Convert.ToInt32(Request("TabID"))
            End If

            If Not (Request("TabModuleID") Is Nothing) Then
                _tabModuleID = Convert.ToInt32(Request("TabModuleID"))
            End If

            If Not (Request("PageID") Is Nothing) Then
                _pageID = Convert.ToInt32(Request("PageID"))
            End If

            If Not (Request("PortalID") Is Nothing) Then
                _portalID = Convert.ToInt32(Request("PortalID"))
            End If

        End Sub

        Private Sub ManageStyleSheets(ByVal PortalCSS As Boolean)

            ' initialize reference paths to load the cascading style sheets
            Dim objCSS As Control = Me.FindControl("CSS")
            Dim objLink As HtmlGenericControl
            Dim ID As String

            Dim objCSSCache As Hashtable = CType(DataCache.GetCache("CSS"), Hashtable)
            If objCSSCache Is Nothing Then
                objCSSCache = New Hashtable
            End If

            If Not objCSS Is Nothing Then
                If PortalCSS = False Then
                    ' module style sheet
                    ID = CreateValidID("PropertyAgent")
                    objLink = New HtmlGenericControl("link")
                    objLink.ID = ID
                    objLink.Attributes("rel") = "stylesheet"
                    objLink.Attributes("type") = "text/css"
                    objLink.Attributes("href") = Me.ResolveUrl("module.css")
                    objCSS.Controls.Add(objLink)

                    ' default style sheet ( required )
                    ID = CreateValidID(DotNetNuke.Common.Globals.HostPath)
                    objLink = New HtmlGenericControl("link")
                    objLink.ID = ID
                    objLink.Attributes("rel") = "stylesheet"
                    objLink.Attributes("type") = "text/css"
                    objLink.Attributes("href") = DotNetNuke.Common.Globals.HostPath & "default.css"
                    objCSS.Controls.Add(objLink)

                    ' skin package style sheet
                    ID = CreateValidID(PortalSettings.ActiveTab.SkinPath)
                    If objCSSCache.ContainsKey(ID) = False Then
                        If File.Exists(Server.MapPath(PortalSettings.ActiveTab.SkinPath) & "skin.css") Then
                            objCSSCache(ID) = PortalSettings.ActiveTab.SkinPath & "skin.css"
                        Else
                            objCSSCache(ID) = ""
                        End If
                        If Not DotNetNuke.Common.Globals.PerformanceSetting = DotNetNuke.Common.Globals.PerformanceSettings.NoCaching Then
                            DataCache.SetCache("CSS", objCSSCache)
                        End If
                    End If
                    If objCSSCache(ID).ToString <> "" Then
                        objLink = New HtmlGenericControl("link")
                        objLink.ID = ID
                        objLink.Attributes("rel") = "stylesheet"
                        objLink.Attributes("type") = "text/css"
                        objLink.Attributes("href") = objCSSCache(ID).ToString
                        objCSS.Controls.Add(objLink)
                    End If

                    ' skin file style sheet
                    ID = CreateValidID(Replace(PortalSettings.ActiveTab.SkinSrc, ".ascx", ".css"))
                    If objCSSCache.ContainsKey(ID) = False Then
                        If File.Exists(Server.MapPath(Replace(PortalSettings.ActiveTab.SkinSrc, ".ascx", ".css"))) Then
                            objCSSCache(ID) = Replace(PortalSettings.ActiveTab.SkinSrc, ".ascx", ".css")
                        Else
                            objCSSCache(ID) = ""
                        End If
                        If Not DotNetNuke.Common.Globals.PerformanceSetting = DotNetNuke.Common.Globals.PerformanceSettings.NoCaching Then
                            DataCache.SetCache("CSS", objCSSCache)
                        End If
                    End If
                    If objCSSCache(ID).ToString <> "" Then
                        objLink = New HtmlGenericControl("link")
                        objLink.ID = ID
                        objLink.Attributes("rel") = "stylesheet"
                        objLink.Attributes("type") = "text/css"
                        objLink.Attributes("href") = objCSSCache(ID).ToString
                        objCSS.Controls.Add(objLink)
                    End If
                Else
                    ' portal style sheet
                    ID = CreateValidID(PortalSettings.HomeDirectory)
                    objLink = New HtmlGenericControl("link")
                    objLink.ID = ID
                    objLink.Attributes("rel") = "stylesheet"
                    objLink.Attributes("type") = "text/css"
                    objLink.Attributes("href") = PortalSettings.HomeDirectory & "portal.css"
                    objCSS.Controls.Add(objLink)
                End If

            End If

        End Sub

        Private Sub BindArticle()

            If (_articleID = Null.NullInteger) Then
                Response.Redirect(NavigateURL(_tabID), True)
            End If

            Dim objArticleController As New ArticleController
            Dim objArticle As ArticleInfo = objArticleController.GetArticle(_articleID)

            If Not (objArticle Is Nothing) Then

                ' Check Article Security
                If (objArticle.IsSecure) Then
                    If (ArticleSettings.IsSecureEnabled = False) Then
                        If (ArticleSettings.SecureUrl <> "") Then
                            Dim url As String = Request.Url.ToString().Replace(AddHTTP(Request.Url.Host), "")
                            If (ArticleSettings.SecureUrl.IndexOf("?") <> -1) Then
                                Response.Redirect(ArticleSettings.SecureUrl & "&returnurl=" & Server.UrlEncode(url), True)
                            Else
                                Response.Redirect(ArticleSettings.SecureUrl & "?returnurl=" & Server.UrlEncode(url), True)
                            End If
                        Else
                            Response.Redirect(NavigateURL(_tabID), True)
                        End If
                    End If
                End If

                Dim objModuleController As New ModuleController
                Dim objModule As ModuleInfo = objModuleController.GetModule(objArticle.ModuleID, _tabID)

                If Not (objModule Is Nothing) Then
                    If (DotNetNuke.Security.PortalSecurity.IsInRoles(objModule.AuthorizedViewRoles) = False) Then
                        Response.Redirect(NavigateURL(_tabID), True)
                    End If

                    If (objModule.PortalID <> PortalSettings.PortalId) Then
                        Response.Redirect(NavigateURL(_tabID), True)
                    End If
                End If

                Dim objLayoutController As New LayoutController(PortalSettings, ArticleSettings, objModule, Page)

                'Dim objLayoutController As New LayoutController(PortalSettings, ArticleSettings, Page, False, _tabID, _moduleID, _tabModuleID, _portalID, _pageID, Null.NullInteger, "Articles-Print-" & _moduleID.ToString())
                Dim objLayoutItem As LayoutInfo = LayoutController.GetLayout(ArticleSettings, objModule, Page, LayoutType.Print_Item_Html)
                objLayoutController.ProcessArticleItem(phArticle.Controls, objLayoutItem.Tokens, objArticle)
                objLayoutController.LoadStyleSheet(ArticleSettings.Template)

                Dim objLayoutTitle As LayoutInfo = LayoutController.GetLayout(ArticleSettings, objModule, Page, LayoutType.View_Title_Html)
                If (objLayoutTitle.Template <> "") Then
                    Dim phPageTitle As New PlaceHolder()
                    objLayoutController.ProcessArticleItem(phPageTitle.Controls, objLayoutTitle.Tokens, objArticle)
                    Me.Title = RenderControlToString(phPageTitle)
                End If

                Dim objLayoutDescription As LayoutInfo = LayoutController.GetLayout(ArticleSettings, objModule, Page, LayoutType.View_Description_Html)
                If (objLayoutDescription.Template <> "") Then
                    Dim phPageDescription As New PlaceHolder()
                    objLayoutController.ProcessArticleItem(phPageDescription.Controls, objLayoutDescription.Tokens, objArticle)
                    Dim meta As New HtmlMeta
                    meta.Name = "MetaDescription"
                    meta.Content = RenderControlToString(phPageDescription)
                    If (meta.Content <> "") Then
                        Me.Header.Controls.Add(meta)
                    End If
                End If

                Dim objLayoutKeyword As LayoutInfo = LayoutController.GetLayout(ArticleSettings, objModule, Page, LayoutType.View_Keyword_Html)
                If (objLayoutKeyword.Template <> "") Then
                    Dim phPageKeyword As New PlaceHolder()
                    objLayoutController.ProcessArticleItem(phPageKeyword.Controls, objLayoutKeyword.Tokens, objArticle)

                    Dim meta As New HtmlMeta
                    meta.Name = "MetaKeywords"
                    meta.Content = RenderControlToString(phPageKeyword)
                    If (meta.Content <> "") Then
                        Me.Header.Controls.Add(meta)
                    End If
                End If

            Else

                Response.Redirect(NavigateURL(), True)

            End If

        End Sub

        Protected Function GetSharedResource(ByVal key As String) As String

            Dim path As String = Me.TemplateSourceDirectory & "/" & DotNetNuke.Services.Localization.Localization.LocalResourceDirectory & "/" & DotNetNuke.Services.Localization.Localization.LocalSharedResourceFile
            path = "~" & path.Substring(path.IndexOf("/DesktopModules/"), path.Length - path.IndexOf("/DesktopModules/"))
            Return DotNetNuke.Services.Localization.Localization.GetString(key, path)

        End Function

        Private Function RenderControlToString(ByVal ctrl As Control) As String

            Dim sb As New StringBuilder()
            Dim tw As New IO.StringWriter(sb)
            Dim hw As New HtmlTextWriter(tw)

            ctrl.RenderControl(hw)

            Return sb.ToString()

        End Function

        Protected Function StripHtml(ByVal html As String) As String

            Dim pattern As String = "<(.|\n)*?>"
            Return Regex.Replace(html, pattern, String.Empty)

        End Function

#End Region

#Region " Private Properties "

        Public ReadOnly Property BasePage() As DotNetNuke.Framework.CDefault
            Get
                Return CType(Me.Page, DotNetNuke.Framework.CDefault)
            End Get
        End Property

        Public ReadOnly Property ArticleSettings() As ArticleSettings
            Get
                If (_articleSettings Is Nothing) Then
                    Dim objModuleController As New ModuleController
                    Dim settings As Hashtable = objModuleController.GetModuleSettings(_moduleID)
                    'Add TabModule Settings
                    settings = DotNetNuke.Entities.Portals.PortalSettings.GetTabModuleSettings(_tabModuleID, settings)
                    Dim objModule As ModuleInfo = objModuleController.GetModule(_moduleID, _tabID)
                    _articleSettings = New ArticleSettings(settings, Me.PortalSettings, objModule)
                End If
                Return _articleSettings
            End Get
        End Property

#End Region

#Region " Event Handlers "

        Private Sub Page_Initialization(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            ManageStyleSheets(False)
            ManageStyleSheets(True)
            ReadQueryString()
            BindArticle()

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        End Sub

#End Region

    End Class

End Namespace