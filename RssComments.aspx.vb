'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.Text
Imports System.Web

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals

Namespace Ventrian.NewsArticles

    Partial Public Class RssComments
        Inherits Page

#Region " Private Members "

        Private m_articleID As Integer = Null.NullInteger
        Private m_tabID As Integer = Null.NullInteger
        Private m_TabInfo As DotNetNuke.Entities.Tabs.TabInfo
        Private m_moduleID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub ProcessHeaderFooter(ByRef objPlaceHolder As ControlCollection, ByVal templateArray As String())

            For iPtr As Integer = 0 To templateArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(templateArray(iPtr).ToString()))

                If iPtr < templateArray.Length - 1 Then

                    Select Case templateArray(iPtr + 1)

                        Case "PORTALEMAIL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & iPtr.ToString())
                            objLiteral.Text = PortalController.GetCurrentPortalSettings().Email
                            objPlaceHolder.Add(objLiteral)

                        Case "PORTALNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & iPtr.ToString())
                            objLiteral.Text = Server.HtmlEncode(PortalController.GetCurrentPortalSettings().PortalName)
                            objPlaceHolder.Add(objLiteral)

                        Case "PORTALURL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & iPtr.ToString())
                            objLiteral.Text = Server.HtmlEncode(AddHTTP(PortalController.GetCurrentPortalSettings().PortalAlias.HTTPAlias))
                            objPlaceHolder.Add(objLiteral)

                    End Select

                End If

            Next

        End Sub

        Private Sub ProcessItem(ByRef objPlaceHolder As ControlCollection, ByVal templateArray As String(), ByVal objArticle As ArticleInfo, ByVal objComment As CommentInfo, ByVal articleSettings As ArticleSettings)

            Dim portalSettings As PortalSettings = PortalController.GetCurrentPortalSettings()

            For iPtr As Integer = 0 To templateArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(templateArray(iPtr).ToString()))

                If iPtr < templateArray.Length - 1 Then

                    Select Case templateArray(iPtr + 1)

                        Case "CREATEDATE"

                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("RssComment-" & objComment.CommentID.ToString() & iPtr.ToString())
                            objLiteral.Text = objComment.CreatedDate.ToUniversalTime().ToString("r")
                            objPlaceHolder.Add(objLiteral)

                        Case "DESCRIPTION"

                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("RssComment-" & objComment.CommentID.ToString() & iPtr.ToString())
                            objLiteral.Text = Server.HtmlEncode(objComment.Comment)
                            objPlaceHolder.Add(objLiteral)

                        Case "GUID"

                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("RssComment-" & objComment.CommentID.ToString() & iPtr.ToString())
                            objLiteral.Text = "f1397696-738c-4295-afcd-943feb885714:" & objComment.CommentID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "TITLE"

                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("RssComment-" & objComment.CommentID.ToString() & iPtr.ToString())
                            objLiteral.Text = Server.HtmlEncode(objArticle.Title)
                            objPlaceHolder.Add(objLiteral)

                        Case Else

                            Dim objLiteralOther As New Literal
                            objLiteralOther.ID = Globals.CreateValidID("RssComment-" & objComment.CommentID.ToString() & iPtr.ToString())
                            objLiteralOther.Text = "[" & templateArray(iPtr + 1) & "]"
                            objLiteralOther.EnableViewState = False
                            objPlaceHolder.Add(objLiteralOther)

                    End Select

                End If

            Next

        End Sub

        Private Sub ReadQueryString()

            If (Request("ArticleID") <> "") Then
                If (IsNumeric(Request("ArticleID"))) Then
                    m_articleID = Convert.ToInt32(Request("ArticleID"))
                End If
            End If

            If (Request("TabID") <> "") Then
                If (IsNumeric(Request("TabID"))) Then
                    m_tabID = Convert.ToInt32(Request("TabID"))
                    Dim objTabController As New DotNetNuke.Entities.Tabs.TabController
                    m_TabInfo = objTabController.GetTab(m_tabID, Globals.GetPortalSettings().PortalId, False)
                End If
            End If

            If (Request("ModuleID") <> "") Then
                If (IsNumeric(Request("ModuleID"))) Then
                    m_moduleID = Convert.ToInt32(Request("ModuleID"))
                End If
            End If

        End Sub

        Private Function RenderControlToString(ByVal ctrl As Control) As String

            Dim sb As New StringBuilder()
            Dim tw As New IO.StringWriter(sb)
            Dim hw As New HtmlTextWriter(tw)

            ctrl.RenderControl(hw)

            Return sb.ToString()

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            ReadQueryString()

            Dim launchLinks As Boolean = False
            Dim enableSyndicationHtml As Boolean = False

            Dim _portalSettings As PortalSettings = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            Dim objModuleController As New ModuleController
            Dim objModule As ModuleInfo = objModuleController.GetModule(m_moduleID, m_tabID)
            Dim articleSettings As ArticleSettings

            If Not (objModule Is Nothing) Then
                Dim settings As Hashtable = objModule.TabModuleSettings
                articleSettings = New ArticleSettings(settings, _portalSettings, objModule)
                If (settings.Contains(ArticleConstants.LAUNCH_LINKS)) Then
                    launchLinks = Convert.ToBoolean(settings(ArticleConstants.LAUNCH_LINKS).ToString())
                End If
                If (settings.Contains(ArticleConstants.ENABLE_SYNDICATION_HTML_SETTING)) Then
                    enableSyndicationHtml = Convert.ToBoolean(settings(ArticleConstants.ENABLE_SYNDICATION_HTML_SETTING).ToString())
                End If
                If (settings.Contains(ArticleConstants.DISPLAY_MODE)) Then
                End If

                Response.ContentType = "text/xml"
                Response.ContentEncoding = Encoding.UTF8

                Dim objArticleController As New ArticleController()
                Dim objArticle As ArticleInfo = objArticleController.GetArticle(m_articleID)

                Dim objLayoutController As New LayoutController(_portalSettings, articleSettings, objModule, Page)
                ' Dim objLayoutController As New LayoutController(_portalSettings, articleSettings, Me, False, m_tabID, m_moduleID, objModule.TabModuleID, _portalSettings.PortalId, Null.NullInteger, Null.NullInteger, "RssComment-" & m_tabID.ToString())

                Dim layoutHeader As LayoutInfo = LayoutController.GetLayout(articleSettings, objModule, Page, LayoutType.Rss_Comment_Header_Html)
                Dim layoutItem As LayoutInfo = LayoutController.GetLayout(articleSettings, objModule, Page, LayoutType.Rss_Comment_Item_Html)
                Dim layoutFooter As LayoutInfo = LayoutController.GetLayout(articleSettings, objModule, Page, (LayoutType.Rss_Comment_Footer_Html))

                Dim phRSS As New PlaceHolder

                ProcessHeaderFooter(phRSS.Controls, layoutHeader.Tokens)

                Dim objCommentController As CommentController = New CommentController
                Dim commentList As List(Of CommentInfo) = objCommentController.GetCommentList(m_moduleID, m_articleID, True, SortDirection.Ascending, Null.NullInteger)

                For Each objComment As CommentInfo In commentList

                    Dim delimStr As String = "[]"
                    Dim delimiter As Char() = delimStr.ToCharArray()

                    Dim phItem As New PlaceHolder
                    ProcessItem(phItem.Controls, layoutItem.Tokens, objArticle, objComment, articleSettings)

                    objLayoutController.ProcessComment(phRSS.Controls, objArticle, objComment, RenderControlToString(phItem).Split(delimiter))
                Next

                ProcessHeaderFooter(phRSS.Controls, layoutFooter.Tokens)

                Response.Write(RenderControlToString(phRSS))

            End If

            Response.End()

        End Sub

#End Region

    End Class

End Namespace