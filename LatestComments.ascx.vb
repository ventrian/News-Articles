Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class LatestComments
        Inherits NewsArticleModuleBase

#Region " Private Members "

        Private _objLayoutController As LayoutController

        Private _objLayoutHeader As LayoutInfo
        Private _objLayoutItem As LayoutInfo
        Private _objLayoutFooter As LayoutInfo

        Private _articleTabID As Integer = Null.NullInteger
        Private _articleTabInfo As DotNetNuke.Entities.Tabs.TabInfo
        Private _articleModuleID As Integer = Null.NullInteger
        Private _articleSettings As ArticleSettings

#End Region

#Region " Private Properties "

        Public Shadows ReadOnly Property ArticleSettings() As ArticleSettings
            Get
                If (_articleSettings Is Nothing) Then

                    Dim _settings As Hashtable = Common.GetModuleSettings(_articleModuleID)

                    Dim objModuleController As New ModuleController
                    Dim objModule As ModuleInfo = objModuleController.GetModule(_articleModuleID, _articleTabID)
                    If Not (objModule Is Nothing) Then
                        Dim objSettings As Hashtable = objModule.TabModuleSettings

                        For Each key As String In objSettings.Keys
                            If (_settings.ContainsKey(key) = False) Then
                                _settings.Add(key, objSettings(key))
                            End If
                        Next
                    End If

                    _articleSettings = New ArticleSettings(_settings, Me.PortalSettings, Me.ModuleConfiguration)

                End If
                Return _articleSettings
            End Get
        End Property

        Private ReadOnly Property ArticleTabInfo() As DotNetNuke.Entities.Tabs.TabInfo
            Get
                If (_articleTabInfo Is Nothing) Then
                    Dim objTabController As New DotNetNuke.Entities.Tabs.TabController
                    _articleTabInfo = objTabController.GetTab(_articleTabID, Me.PortalId, False)
                End If

                Return _articleTabInfo
            End Get
        End Property

        Private Sub BindComments()

            Dim objModuleController As New ModuleController()
            Dim objModule As ModuleInfo = objModuleController.GetModule(_articleModuleID, _articleTabID)
            _objLayoutController = New LayoutController(PortalSettings, ArticleSettings, objModule, Page)

            Dim layoutHeader As String
            If (Settings.Contains(ArticleConstants.LATEST_COMMENTS_HTML_HEADER)) Then
                layoutHeader = Settings(ArticleConstants.LATEST_COMMENTS_HTML_HEADER).ToString()
            Else
                layoutHeader = ArticleConstants.DEFAULT_LATEST_COMMENTS_HTML_HEADER
            End If
            _objLayoutHeader = _objLayoutController.GetLayoutObject(layoutHeader)

            Dim layoutItem As String
            If (Settings.Contains(ArticleConstants.LATEST_COMMENTS_HTML_BODY)) Then
                layoutItem = Settings(ArticleConstants.LATEST_COMMENTS_HTML_BODY).ToString()
            Else
                layoutItem = ArticleConstants.DEFAULT_LATEST_COMMENTS_HTML_BODY
            End If
            _objLayoutItem = _objLayoutController.GetLayoutObject(layoutItem)

            Dim layoutFooter As String
            If (Settings.Contains(ArticleConstants.LATEST_COMMENTS_HTML_FOOTER)) Then
                layoutFooter = Settings(ArticleConstants.LATEST_COMMENTS_HTML_FOOTER).ToString()
            Else
                layoutFooter = ArticleConstants.DEFAULT_LATEST_COMMENTS_HTML_FOOTER
            End If
            _objLayoutFooter = _objLayoutController.GetLayoutObject(layoutFooter)

            Dim count As Integer = 10
            If (Settings.Contains(ArticleConstants.LATEST_COMMENTS_COUNT)) Then
                count = Convert.ToInt32(Settings(ArticleConstants.LATEST_COMMENTS_COUNT).ToString())
            End If

            Dim objCommentController As New CommentController
            rptLatestComments.DataSource = objCommentController.GetCommentList(_articleModuleID, Null.NullInteger, True, SortDirection.Descending, count)
            rptLatestComments.DataBind()

            If (rptLatestComments.Items.Count = 0) Then
                phNoComments.Visible = True
                rptLatestComments.Visible = False

                Dim noComments As String = ArticleConstants.DEFAULT_LATEST_COMMENTS_HTML_NO_COMMENTS
                If (Settings.Contains(ArticleConstants.LATEST_COMMENTS_HTML_NO_COMMENTS)) Then
                    noComments = Settings(ArticleConstants.LATEST_COMMENTS_HTML_NO_COMMENTS).ToString()
                End If
                noComments = "<div id=""NoRecords"" class=""Normal"">" & noComments & "</div>"
                Dim objNoComments As LayoutInfo = _objLayoutController.GetLayoutObject(noComments)
                ProcessHeaderFooter(phNoComments.Controls, objNoComments.Tokens)
            End If

        End Sub

        Private Sub ProcessBody(ByRef controls As ControlCollection, ByVal objComment As CommentInfo, ByVal layoutArray As String())

            If (ArticleTabInfo Is Nothing) Then
                Return
            End If

            Dim objArticleController As New ArticleController
            Dim objArticle As ArticleInfo = objArticleController.GetArticle(objComment.ArticleID)

            _objLayoutController.ProcessComment(controls, objArticle, objComment, _objLayoutItem.Tokens)

        End Sub

        Private Sub ProcessHeaderFooter(ByRef controls As ControlCollection, ByVal layoutArray As String())


            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                controls.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)

                        Case Else
                            Dim objLiteralOther As New Literal
                            objLiteralOther.Text = "[" & layoutArray(iPtr + 1) & "]"
                            objLiteralOther.EnableViewState = False
                            controls.Add(objLiteralOther)

                    End Select
                End If
            Next

        End Sub

#End Region

#Region " Private Methods "

        Private Function FindSettings() As Boolean

            If (Settings.Contains(ArticleConstants.LATEST_COMMENTS_TAB_ID)) Then
                If (IsNumeric(Settings(ArticleConstants.LATEST_COMMENTS_TAB_ID).ToString())) Then
                    _articleTabID = Convert.ToInt32(Settings(ArticleConstants.LATEST_COMMENTS_TAB_ID).ToString())
                End If
            End If

            If (Settings.Contains(ArticleConstants.LATEST_COMMENTS_MODULE_ID)) Then
                If (IsNumeric(Settings(ArticleConstants.LATEST_COMMENTS_MODULE_ID).ToString())) Then
                    _articleModuleID = Convert.ToInt32(Settings(ArticleConstants.LATEST_COMMENTS_MODULE_ID).ToString())
                    If (_articleModuleID <> Null.NullInteger) Then
                        Return True
                    End If
                End If
            End If

            Return False

        End Function

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                If (FindSettings()) Then
                    BindComments()
                Else
                    lblNotConfigured.Visible = True
                    rptLatestComments.Visible = False
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                If (_articleTabID <> Null.NullInteger) Then
                    If (Settings.Contains(ArticleConstants.LATEST_COMMENTS_INCLUDE_STYLESHEET)) Then
                        If (Convert.ToBoolean(Settings(ArticleConstants.LATEST_COMMENTS_INCLUDE_STYLESHEET).ToString())) Then
                            LoadStyleSheet()
                        End If
                    End If
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub rptLatestComments_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptLatestComments.ItemDataBound

            Try

                If (e.Item.ItemType = ListItemType.Header) Then
                    ProcessHeaderFooter(e.Item.Controls, _objLayoutHeader.Tokens())
                End If

                If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                    ProcessBody(e.Item.Controls, CType(e.Item.DataItem, CommentInfo), _objLayoutItem.Tokens)
                End If

                If (e.Item.ItemType = ListItemType.Footer) Then
                    ProcessHeaderFooter(e.Item.Controls, _objLayoutFooter.Tokens())
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace
