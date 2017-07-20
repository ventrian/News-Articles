'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2005-2012
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucMyArticles
        Inherits NewsArticleModuleBase

        Private Enum StatusType

            Draft = 1
            Unapproved = 2
            Approved = 3

        End Enum

        Private _status As StatusType = StatusType.Draft

#Region " Private Properties "

        Private ReadOnly Property CurrentPage() As Integer
            Get
                If (Request("Page") = Null.NullString And Request("CurrentPage") = Null.NullString) Then
                    Return 1
                Else
                    Try
                        If (Request("Page") <> Null.NullString) Then
                            Return Convert.ToInt32(Request("Page"))
                        Else
                            Return Convert.ToInt32(Request("CurrentPage"))
                        End If
                    Catch
                        Return 1
                    End Try
                End If
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            If (Request("Status") <> "") Then
                Select Case Request("Status").ToLower()
                    Case "2"
                        _status = StatusType.Unapproved
                        Exit Select
                    Case "3"
                        _status = StatusType.Approved
                        Exit Select
                End Select
            End If

        End Sub

        Private Sub BindSelection()

            If (ArticleSettings.IsApprover Or ArticleSettings.IsAdmin) Then
                If (Request("ShowAll") <> "") Then
                    chkShowAll.Checked = True
                End If
            Else
                chkShowAll.Visible = False
            End If

        End Sub

        Private Sub BindArticles()

            Dim objArticleController As ArticleController = New ArticleController

            Localization.LocalizeDataGrid(grdMyArticles, Me.LocalResourceFile)

            Dim count As Integer = 0
            Dim authorID As Integer = Me.UserId
            If (chkShowAll.Checked) Then
                authorID = Null.NullInteger
            End If

            grdMyArticles.DataSource = objArticleController.GetArticleList(Me.ModuleId, Null.NullDate, Null.NullDate, Nothing, Null.NullBoolean, Nothing, Null.NullInteger, CurrentPage, 10, ArticleSettings.SortBy, ArticleSettings.SortDirection, False, True, Null.NullString, authorID, True, True, Null.NullBoolean, Null.NullBoolean, False, False, Null.NullString, Nothing, False, Null.NullString, Null.NullInteger, Null.NullString, Null.NullString, count)

            Select Case _status

                Case StatusType.Draft
                    grdMyArticles.DataSource = objArticleController.GetArticleList(Me.ModuleId, Null.NullDate, Null.NullDate, Nothing, Null.NullBoolean, Nothing, Null.NullInteger, CurrentPage, 10, ArticleSettings.SortBy, ArticleSettings.SortDirection, False, True, Null.NullString, authorID, True, True, Null.NullBoolean, Null.NullBoolean, False, False, Null.NullString, Nothing, False, Null.NullString, Null.NullInteger, Null.NullString, Null.NullString, count)
                    Exit Select
                Case StatusType.Unapproved
                    grdMyArticles.DataSource = objArticleController.GetArticleList(Me.ModuleId, Null.NullDate, Null.NullDate, Nothing, Null.NullBoolean, Nothing, Null.NullInteger, CurrentPage, 10, ArticleSettings.SortBy, ArticleSettings.SortDirection, False, False, Null.NullString, authorID, True, True, Null.NullBoolean, Null.NullBoolean, False, False, Null.NullString, Nothing, False, Null.NullString, Null.NullInteger, Null.NullString, Null.NullString, count)
                    Exit Select
                Case StatusType.Approved
                    grdMyArticles.DataSource = objArticleController.GetArticleList(Me.ModuleId, Null.NullDate, Null.NullDate, Nothing, Null.NullBoolean, Nothing, Null.NullInteger, CurrentPage, 10, ArticleSettings.SortBy, ArticleSettings.SortDirection, True, False, Null.NullString, authorID, True, True, Null.NullBoolean, Null.NullBoolean, False, False, Null.NullString, Nothing, False, Null.NullString, Null.NullInteger, Null.NullString, Null.NullString, count)
                    Exit Select

            End Select

            grdMyArticles.DataBind()

            If (grdMyArticles.Items.Count = 0) Then
                phNoArticles.Visible = True
                grdMyArticles.Visible = False

                ctlPagingControl.Visible = False
            Else
                phNoArticles.Visible = False
                grdMyArticles.Visible = True

                If (count > 10) Then
                    ctlPagingControl.Visible = True
                    ctlPagingControl.TotalRecords = count
                    ctlPagingControl.PageSize = 10
                    ctlPagingControl.CurrentPage = CurrentPage
                    ctlPagingControl.QuerystringParams = GetParams()
                    ctlPagingControl.TabID = TabId
                    ctlPagingControl.EnableViewState = False
                End If

                grdMyArticles.Columns(0).Visible = IsEditable
            End If

        End Sub

        Private Sub CheckSecurity()

            If (ArticleSettings.IsSubmitter = False) Then
                Response.Redirect(NavigateURL(), True)
            End If

            If (Request("ShowAll") <> "") Then
                If ((ArticleSettings.IsApprover Or ArticleSettings.IsAdmin) = False) Then
                    Response.Redirect(NavigateURL(), True)
                End If
            End If

        End Sub

        Private Function GetParams() As String

            Dim params As String = ""

            If (Request("ctl") <> "") Then
                If (Request("ctl").ToLower() = "myarticles") Then
                    params += "ctl=" & Request("ctl") & "&mid=" & ModuleId.ToString()
                End If
            End If

            If (Request("articleType") <> "") Then
                If (Request("articleType").ToString().ToLower() = "myarticles") Then
                    params += "articleType=" & Request("articleType")
                End If
            End If

            If (Request("Status") <> "") Then
                params += "&Status=" & Convert.ToInt32(_status).ToString()
            End If

            If (Request("ShowAll") <> "") Then
                params += "&ShowAll=" & Request("ShowAll")
            End If

            Return params

        End Function

#End Region

#Region " Protected Methods "

        Protected Function GetAdjustedCreateDate(ByVal objItem As Object) As String

            Dim objArticle As ArticleInfo = CType(objItem, ArticleInfo)
            Return objArticle.CreatedDate.ToString("d") & " " & objArticle.CreatedDate.ToString("t")

        End Function

        Protected Function GetAdjustedPublishDate(ByVal objItem As Object) As String

            Dim objArticle As ArticleInfo = CType(objItem, ArticleInfo)
            Return objArticle.StartDate.ToString("d") & " " & objArticle.StartDate.ToString("t")

        End Function

        Protected Function GetArticleLink(ByVal objItem As Object) As String

            Dim objArticle As ArticleInfo = CType(objItem, ArticleInfo)
            Return Common.GetArticleLink(objArticle, PortalSettings.ActiveTab, ArticleSettings, False)

        End Function

        Protected Function GetEditUrl(ByVal articleID As String) As String
            If (ArticleSettings.LaunchLinks) Then
                Return Common.GetModuleLink(Me.TabId, Me.ModuleId, "Edit", ArticleSettings, "ArticleID=" & articleID)
            Else
                Return Common.GetModuleLink(Me.TabId, Me.ModuleId, "SubmitNews", ArticleSettings, "ArticleID=" & articleID)
            End If
        End Function

        Protected Function GetModuleLink(ByVal key As String, ByVal status As Integer) As String

            If (status = 1) Then
                Return Common.GetModuleLink(TabId, ModuleId, "MyArticles", ArticleSettings)
            Else
                Return Common.GetModuleLink(TabId, ModuleId, "MyArticles", ArticleSettings, "Status=" & status.ToString())
            End If

        End Function

        Public Shadows ReadOnly Property IsEditable() As Boolean
            Get
                If (_status = StatusType.Draft) Then
                    Return True
                Else
                    If (ArticleSettings.IsApprover Or ArticleSettings.IsAutoApprover) Then
                        Return True
                    End If
                End If

            End Get
        End Property

        Protected Function IsSelected(ByVal status As Integer)

            If (status = _status) Then
                Return "ui-tabs-selected ui-state-active"
            Else
                Return ""
            End If

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                CheckSecurity()
                ReadQueryString()

                If (IsPostBack = False) Then
                    BindSelection()
                    BindArticles()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub chkShowAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkShowAll.CheckedChanged

            Try

                If (chkShowAll.Checked) Then
                    If (_status <> StatusType.Draft) Then
                        Response.Redirect(Common.GetModuleLink(Me.TabId, Me.ModuleId, "MyArticles", ArticleSettings, "ShowAll=1", "Status=" & Convert.ToInt32(_status).ToString()), True)
                    Else
                        Response.Redirect(Common.GetModuleLink(Me.TabId, Me.ModuleId, "MyArticles", ArticleSettings, "ShowAll=1"), True)
                    End If
                Else
                    If (_status <> StatusType.Draft) Then
                        Response.Redirect(Common.GetModuleLink(Me.TabId, Me.ModuleId, "MyArticles", ArticleSettings, "Status=" & Convert.ToInt32(_status).ToString()), True)
                    Else
                        Response.Redirect(Common.GetModuleLink(Me.TabId, Me.ModuleId, "MyArticles", ArticleSettings), True)
                    End If
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace