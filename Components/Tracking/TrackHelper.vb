'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Collections.Specialized
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions

Namespace Ventrian.NewsArticles.Tracking

    Public Class TrackHelper

        Public Shared Function BuildLinks(ByVal text As String, ByRef links As StringCollection) As StringCollection

            Dim pattern As String = "(?:[hH][rR][eE][fF]\s*=)(?:[\s""""']*)(?!#|[Mm]ailto|[lL]ocation.|[jJ]avascript|.*css|.*this\.)(.*?)(?:[\s>""""'])"

            Dim r As New Regex(pattern, RegexOptions.IgnoreCase)

            Dim m As Match = r.Match(Common.HtmlDecode(text))
            Dim link As String = ""
            While (m.Success)
                If (m.Groups.ToString().Length > 0) Then
                    link = m.Groups(1).ToString()
                    If (links.Contains(link) = False) Then
                        links.Add(link)
                    End If
                End If
                m = m.NextMatch
            End While

            Return links

        End Function

        Public Shared Function GetPageText(ByVal inURL As String) As String

            Dim req As WebRequest = WebRequest.Create(inURL)
            Dim wreq As HttpWebRequest = CType(req, HttpWebRequest)
            If Not (wreq Is Nothing) Then
                wreq.UserAgent = "My User Agent String"
                wreq.Referer = "http://www.wwwcoder.com/"
                wreq.Timeout = 60000
            End If
            Dim response As HttpWebResponse = CType(wreq.GetResponse, HttpWebResponse)
            Dim s As Stream = response.GetResponseStream
            Dim enc As String = response.ContentEncoding.Trim
            If enc = "" Then enc = "us-ascii"
            Dim encode As Encoding = System.Text.Encoding.GetEncoding(enc)
            Dim sr As StreamReader = New StreamReader(s, encode)
            Return sr.ReadToEnd

        End Function

    End Class

End Namespace
