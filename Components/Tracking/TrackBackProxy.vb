'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions

Namespace Ventrian.NewsArticles.Tracking

    Public Class TrackBackProxy

#Region " Public Methods "

        Public Function TrackBackPing(ByVal pageText As String, ByVal url As String, ByVal title As String, ByVal link As String, ByVal blogname As String, ByVal description As String) As Boolean

            Dim objLogController As New DotNetNuke.Services.Log.EventLog.EventLogController
            Dim objEventLog As New DotNetNuke.Services.Log.EventLog.EventLogController

            ' objEventLog.AddLog("Ping Exception", "Ping with a Return URL of ->" & link, DotNetNuke.Common.Globals.GetPortalSettings(), -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)

            Dim trackBackItem As String = GetTrackBackText(pageText, url, link)

            If Not trackBackItem Is Nothing Then

                If Not trackBackItem.ToLower().StartsWith("http://") Then
                    trackBackItem = "http://" + trackBackItem
                End If

                Dim parameters As String = "title=" + HtmlEncode(title) + "&url=" + HtmlEncode(link) + "&blog_name=" + HtmlEncode(blogname) + "&excerpt=" + HtmlEncode(description)
                SendPing(trackBackItem, parameters)

            Else

                ' objEventLog.AddLog("Ping Exception", "Pinging ->" & link & " -> Trackback Text not found on this page!", DotNetNuke.Common.Globals.GetPortalSettings(), -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)

            End If

            Return True

        End Function

#End Region

#Region " Private Methods "

        Private Function GetTrackBackText(ByVal pageText As String, ByVal url As String, ByVal PostUrl As String) As String
            If Not Regex.IsMatch(pageText, PostUrl, RegexOptions.IgnoreCase Or RegexOptions.Singleline) Then

                Dim sPattern As String = "<rdf:\w+\s[^>]*?>(</rdf:rdf>)?"
                Dim r As Regex = New Regex(sPattern, RegexOptions.IgnoreCase)
                Dim m As Match

                m = r.Match(pageText)
                While (m.Success)
                    If m.Groups.ToString().Length > 0 Then

                        Dim text As String = m.Groups(0).ToString()
                        If text.IndexOf(url) > 0 Then
                            Dim tbPattern As String = "trackback:ping=\""([^\""]+)\"""
                            Dim reg As Regex = New Regex(tbPattern, RegexOptions.IgnoreCase)
                            Dim m2 As Match = reg.Match(text)
                            If m2.Success Then
                                Return m2.Result("$1")
                            End If

                            Return text
                        End If
                    End If
                    m = m.NextMatch
                End While
            End If

            Return Nothing

        End Function

        Private Function HtmlEncode(ByVal text As String) As String

            Return System.Web.HttpUtility.HtmlEncode(text)

        End Function

        Private Sub SendPing(ByVal trackBackItem As String, ByVal parameters As String)

            Dim myWriter As StreamWriter = Nothing

            Dim request As HttpWebRequest = CType(HttpWebRequest.Create(trackBackItem), HttpWebRequest)
            If Not (request Is Nothing) Then
                request.UserAgent = "My User Agent String"
                request.Referer = "http://www.smcculloch.net/"
                request.Timeout = 60000
            End If

            request.Method = "POST"
            request.ContentLength = parameters.Length
            request.ContentType = "application/x-www-form-urlencoded"
            request.KeepAlive = False

            ' Try
            myWriter = New StreamWriter(request.GetRequestStream())
            myWriter.Write(parameters)
            'Finally
            myWriter.Close()
            '   End Try
        End Sub

#End Region

    End Class

End Namespace
