'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2008
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ViewArchive
        Inherits NewsArticleModuleBase

#Region " Constants "

        Private Const PARAM_YEAR As String = "Year"
        Private Const PARAM_MONTH As String = "Month"

#End Region

#Region " Private Members "

        Private _year As Integer = Null.NullInteger
        Private _month As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub BindArchiveName()

            If (_month = Null.NullInteger AndAlso _year = Null.NullInteger) Then
                ' No archive to view. 
                Response.Redirect(NavigateURL(), True)
            End If

            If (_year = Null.NullInteger) Then
                ' No archive to view. 
                Response.Redirect(NavigateURL(), True)
            End If

            If (_month <> Null.NullInteger) Then
                Dim entriesFrom As String = Localization.GetString("MonthYearEntries", LocalResourceFile)
                Dim objDate As DateTime = New DateTime(_year, _month, 1)
                If (entriesFrom.Contains("{0}") And entriesFrom.Contains("{1}")) Then
                    lblArchive.Text = String.Format(entriesFrom, objDate.ToString("MMMM"), objDate.ToString("yyyy"))
                Else
                    If (entriesFrom.Contains("{0}")) Then
                        lblArchive.Text = String.Format(entriesFrom, objDate.ToString("MMMM"))
                    Else
                        lblArchive.Text = objDate.ToString("MMMM yyyy")
                    End If
                End If
                Me.BasePage.Title = objDate.ToString("MMMM yyyy") & " " & Localization.GetString("Archive", Me.LocalResourceFile) & " | " & Me.BasePage.Title

            Else
                Dim entriesFrom As String = Localization.GetString("YearEntries", LocalResourceFile)
                If (entriesFrom.Contains("{0}")) Then
                    lblArchive.Text = String.Format(entriesFrom, _year.ToString())
                Else
                    lblArchive.Text = _year.ToString()
                End If

                Me.BasePage.Title = _year.ToString & " " & Localization.GetString("Archive", Me.LocalResourceFile) & " | " & Me.BasePage.Title
            End If

        End Sub

        Private Sub ReadQueryString()

            If (Request(PARAM_YEAR) <> "" AndAlso IsNumeric(Request(PARAM_YEAR))) Then
                _year = Convert.ToInt32(Request(PARAM_YEAR))
            End If

            If (Request(PARAM_MONTH) <> "" AndAlso IsNumeric(Request(PARAM_MONTH))) Then
                _month = Convert.ToInt32(Request(PARAM_MONTH))
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                ReadQueryString()
                BindArchiveName()

                Listing1.Month = _month
                Listing1.Year = _year
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