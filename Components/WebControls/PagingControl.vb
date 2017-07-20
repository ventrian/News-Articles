'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by DotNetNuke Corporation
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'

Imports System.ComponentModel
Imports System.Web.UI
Imports DotNetNuke
Imports DotNetNuke.Common

Namespace Ventrian.NewsArticles.Components.WebControls

    <ToolboxData("<{0}:PagingControl runat=server></{0}:PagingControl>")> Public Class PagingControl
        Inherits System.Web.UI.WebControls.WebControl

        Protected tablePageNumbers As System.Web.UI.WebControls.Table
        Protected WithEvents PageNumbers As System.Web.UI.WebControls.Repeater
        Protected cellDisplayStatus As System.Web.UI.WebControls.TableCell
        Protected cellDisplayLinks As System.Web.UI.WebControls.TableCell

        Private TotalPages As Integer = -1

        Private _TotalRecords As Integer
        Private _PageSize As Integer
        Private _CurrentPage As Integer
        Private _QuerystringParams As String
        Private _PageParam As String = "lapg"
        Private _TabID As Integer
        Private _CSSClassLinkActive As String
        Private _CSSClassLinkInactive As String
        Private _CSSClassPagingStatus As String

        <Bindable(True), Category("Behavior"), DefaultValue("0")> Property TotalRecords() As Integer
            Get
                Return _TotalRecords
            End Get

            Set(ByVal Value As Integer)
                _TotalRecords = Value
            End Set
        End Property
        <Bindable(True), Category("Behavior"), DefaultValue("10")> Property PageSize() As Integer
            Get
                Return _PageSize
            End Get

            Set(ByVal Value As Integer)
                _PageSize = Value
            End Set
        End Property
        <Bindable(True), Category("Behavior"), DefaultValue("1")> Property CurrentPage() As Integer
            Get
                Return _CurrentPage
            End Get

            Set(ByVal Value As Integer)
                _CurrentPage = Value
            End Set
        End Property
        <Bindable(True), Category("Behavior"), DefaultValue("")> Property QuerystringParams() As String
            Get
                Return _QuerystringParams
            End Get

            Set(ByVal Value As String)
                _QuerystringParams = Value
            End Set
        End Property
        <Bindable(True), Category("Behavior"), DefaultValue("-1")> Property TabID() As Integer
            Get
                Return _TabID
            End Get

            Set(ByVal Value As Integer)
                _TabID = Value
            End Set
        End Property
        <Bindable(True), Category("Behavior"), DefaultValue("lapg")> Property PageParam() As String
            Get
                Return _PageParam
            End Get

            Set(ByVal Value As String)
                _PageParam = Value
            End Set
        End Property
        <Bindable(True), Category("Behavior"), DefaultValue("Normal")> Property CSSClassLinkActive() As String
            Get
                If _CSSClassLinkActive = "" Then
                    Return "CommandButton"
                Else
                    Return _CSSClassLinkActive
                End If
            End Get

            Set(ByVal Value As String)
                _CSSClassLinkActive = Value
            End Set
        End Property
        <Bindable(True), Category("Behavior"), DefaultValue("CommandButton")> Property CSSClassLinkInactive() As String
            Get
                If _CSSClassLinkInactive = "" Then
                    Return "NormalDisabled"
                Else
                    Return _CSSClassLinkInactive
                End If
            End Get

            Set(ByVal Value As String)
                _CSSClassLinkInactive = Value
            End Set
        End Property
        <Bindable(True), Category("Behavior"), DefaultValue("Normal")> Property CSSClassPagingStatus() As String
            Get
                If _CSSClassPagingStatus = "" Then
                    Return "Normal"
                Else
                    Return _CSSClassPagingStatus
                End If
            End Get

            Set(ByVal Value As String)
                _CSSClassPagingStatus = Value
            End Set
        End Property

        Protected Overrides Sub CreateChildControls()
            tablePageNumbers = New System.Web.UI.WebControls.Table
            cellDisplayStatus = New System.Web.UI.WebControls.TableCell
            cellDisplayLinks = New System.Web.UI.WebControls.TableCell
            cellDisplayStatus.CssClass = "Normal"
            cellDisplayLinks.CssClass = "Normal"

            If Me.CssClass = "" Then
                tablePageNumbers.CssClass = "PagingTable"
            Else
                tablePageNumbers.CssClass = Me.CssClass
            End If

            Dim intRowIndex As Integer = tablePageNumbers.Rows.Add(New TableRow)

            PageNumbers = New Repeater
            Dim I As New PageNumberLinkTemplate(Me)
            PageNumbers.ItemTemplate = I
            BindPageNumbers(TotalRecords, PageSize)

            cellDisplayStatus.HorizontalAlign = HorizontalAlign.Left
            cellDisplayStatus.Width = New Unit("50%")
            cellDisplayLinks.HorizontalAlign = HorizontalAlign.Right
            cellDisplayLinks.Width = New Unit("50%")
            Dim intTotalPages As Integer = TotalPages
            If intTotalPages = 0 Then intTotalPages = 1

            Dim str As String
            str = String.Format(Services.Localization.Localization.GetString("Pages"), CurrentPage.ToString, intTotalPages.ToString)
            Dim lit As New LiteralControl(str)
            cellDisplayStatus.Controls.Add(lit)

            tablePageNumbers.Rows(intRowIndex).Cells.Add(cellDisplayStatus)
            tablePageNumbers.Rows(intRowIndex).Cells.Add(cellDisplayLinks)

        End Sub

        Protected Overrides Sub Render(ByVal output As System.Web.UI.HtmlTextWriter)
            If PageNumbers Is Nothing Then
                CreateChildControls()
            End If

            Dim str As New System.Text.StringBuilder

            str.Append(GetFirstLink() + "&nbsp;&nbsp;&nbsp;")
            str.Append(GetPreviousLink() + "&nbsp;&nbsp;&nbsp;")
            Dim result As System.Text.StringBuilder = New System.Text.StringBuilder(1024)
            PageNumbers.RenderControl(New HtmlTextWriter(New System.IO.StringWriter(result)))
            str.Append(result.ToString())
            str.Append(GetNextLink() + "&nbsp;&nbsp;&nbsp;")
            str.Append(GetLastLink() + "&nbsp;&nbsp;&nbsp;")
            cellDisplayLinks.Controls.Add(New LiteralControl(str.ToString))

            tablePageNumbers.RenderControl(output)

        End Sub


        Private Sub BindPageNumbers(ByVal TotalRecords As Integer, ByVal RecordsPerPage As Integer)
            Dim PageLinksPerPage As Integer = 10
            If TotalRecords / RecordsPerPage >= 1 Then
                TotalPages = Convert.ToInt32(Math.Ceiling(CType(TotalRecords / RecordsPerPage, Double)))
            Else
                TotalPages = 0
            End If

            If TotalPages > 0 Then
                Dim ht As New DataTable
                ht.Columns.Add("PageNum")
                Dim tmpRow As DataRow

                Dim LowNum As Integer = 1
                Dim HighNum As Integer = CType(TotalPages, Integer)

                Dim tmpNum As Double
                tmpNum = CurrentPage - PageLinksPerPage / 2
                If tmpNum < 1 Then tmpNum = 1

                If CurrentPage > (PageLinksPerPage / 2) Then
                    LowNum = CType(Math.Floor(tmpNum), Integer)
                End If

                If CType(TotalPages, Integer) <= PageLinksPerPage Then
                    HighNum = CType(TotalPages, Integer)
                Else
                    HighNum = LowNum + PageLinksPerPage - 1
                End If

                If HighNum > CType(TotalPages, Integer) Then
                    HighNum = CType(TotalPages, Integer)
                    If HighNum - LowNum < PageLinksPerPage Then
                        LowNum = HighNum - PageLinksPerPage + 1
                    End If
                End If

                If HighNum > CType(TotalPages, Integer) Then HighNum = CType(TotalPages, Integer)
                If LowNum < 1 Then LowNum = 1

                Dim i As Integer
                For i = LowNum To HighNum
                    tmpRow = ht.NewRow
                    tmpRow("PageNum") = i
                    ht.Rows.Add(tmpRow)
                Next

                PageNumbers.DataSource = ht
                PageNumbers.DataBind()
            End If

        End Sub

        Private Function CreateURL(ByVal CurrentPage As String) As String

            If QuerystringParams <> "" Then
                If CurrentPage <> "" Then
                    Return Globals.NavigateURL(TabID, "", QuerystringParams, PageParam & "=" & CurrentPage)
                Else
                    Return Globals.NavigateURL(TabID, "", QuerystringParams)
                End If
            Else
                If CurrentPage <> "" Then
                    Return Globals.NavigateURL(TabID, "", PageParam & "=" & CurrentPage)
                Else
                    Return Globals.NavigateURL(TabID)
                End If
            End If

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetLink returns the page number links for paging.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[dancaron]	10/28/2004	Initial Version
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function GetLink(ByVal PageNum As Integer) As String
            If PageNum = CurrentPage Then
                Return "<span class=""" + CSSClassLinkInactive + """>[" + PageNum.ToString + "]</span>"
            Else
                Return "<a href=""" + CreateURL(PageNum.ToString) + """ class=""" + CSSClassLinkActive + """>" + PageNum.ToString + "</a>"
            End If
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetPreviousLink returns the link for the Previous page for paging.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[dancaron]	10/28/2004	Initial Version
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function GetPreviousLink() As String
            If CurrentPage > 1 AndAlso TotalPages > 0 Then
                Return "<a href=""" + CreateURL((CurrentPage - 1).ToString) + """ class=""" + CSSClassLinkActive + """>" & Services.Localization.Localization.GetString("Previous", DotNetNuke.Services.Localization.Localization.SharedResourceFile) & "</a>"
            Else
                Return "<span class=""" + CSSClassLinkInactive + """>" & Services.Localization.Localization.GetString("Previous", DotNetNuke.Services.Localization.Localization.SharedResourceFile) & "</span>"
            End If
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetNextLink returns the link for the Next Page for paging.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[dancaron]	10/28/2004	Initial Version
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function GetNextLink() As String
            If CurrentPage <> TotalPages And TotalPages > 0 Then
                Return "<a href=""" + CreateURL((CurrentPage + 1).ToString) + """ class=""" + CSSClassLinkActive + """>" & Services.Localization.Localization.GetString("Next", DotNetNuke.Services.Localization.Localization.SharedResourceFile) & "</a>"
            Else
                Return "<span class=""" + CSSClassLinkInactive + """>" & Services.Localization.Localization.GetString("Next", DotNetNuke.Services.Localization.Localization.SharedResourceFile) & "</span>"
            End If
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetFirstLink returns the First Page link for paging.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[dancaron]	10/28/2004	Initial Version
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function GetFirstLink() As String
            If CurrentPage > 1 AndAlso TotalPages > 0 Then
                Return "<a href=""" + CreateURL("1") + """ class=""" + CSSClassLinkActive + """>" & Services.Localization.Localization.GetString("First", DotNetNuke.Services.Localization.Localization.SharedResourceFile) & "</a>"
            Else
                Return "<span class=""" + CSSClassLinkInactive + """>" & Services.Localization.Localization.GetString("First", DotNetNuke.Services.Localization.Localization.SharedResourceFile) & "</span>"
            End If
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetLastLink returns the Last Page link for paging.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[dancaron]	10/28/2004	Initial Version
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function GetLastLink() As String
            If CurrentPage <> TotalPages And TotalPages > 0 Then
                Return "<a href=""" + CreateURL(TotalPages.ToString) + """ class=""" + CSSClassLinkActive + """>" & Services.Localization.Localization.GetString("Last", DotNetNuke.Services.Localization.Localization.SharedResourceFile) & "</a>"
            Else
                Return "<span class=""" + CSSClassLinkInactive + """>" & Services.Localization.Localization.GetString("Last", DotNetNuke.Services.Localization.Localization.SharedResourceFile) & "</span>"
            End If
        End Function

        Public Class PageNumberLinkTemplate
            Implements ITemplate
            Shared itemcount As Integer = 0
            Private _PagingControl As PagingControl

            Sub New(ByVal ctlPagingControl As PagingControl)
                _PagingControl = ctlPagingControl
            End Sub

            Sub InstantiateIn(ByVal container As Control) _
                  Implements ITemplate.InstantiateIn

                Dim l As New Literal
                AddHandler l.DataBinding, AddressOf Me.BindData
                container.Controls.Add(l)
            End Sub

            Private Sub BindData(ByVal sender As Object, ByVal e As System.EventArgs)
                Dim lc As Literal
                lc = CType(sender, Literal)
                Dim container As RepeaterItem
                container = CType(lc.NamingContainer, RepeaterItem)
                lc.Text = _PagingControl.GetLink(Convert.ToInt32(DataBinder.Eval(container.DataItem, "PageNum"))) + "&nbsp;&nbsp;"
            End Sub

        End Class

    End Class


End Namespace