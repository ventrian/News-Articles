'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Text.RegularExpressions

Imports CookComputing.XmlRpc

Namespace Ventrian.NewsArticles.Tracking

    Public Class PingBackProxy
        Inherits XmlRpcClientProtocol

#Region " Private Members "

        Dim _errorMessage As String

#End Region

#Region " Private Properties "

        Private Property ErrorMessage() As String
            Get
                Return _errorMessage
            End Get
            Set(ByVal Value As String)
                _errorMessage = Value
            End Set
        End Property

#End Region

#Region " Public Methods "

        Public Function Ping(ByVal pageText As String, ByVal sourceURI As String, ByVal targetURI As String) As Boolean
            Dim pingbackURL As String = GetPingBackURL(pageText, targetURI, sourceURI)
            If Not pingbackURL Is Nothing Then
                Me.Url = pingbackURL
                Try
                    Notifiy(sourceURI, targetURI)
                    Return True
                Catch ex As Exception
                    ErrorMessage = "Error: " + ex.Message
                End Try
            End If
            Return False

        End Function

        <XmlRpcMethod("pingback.ping")> _
        Public Sub Notifiy(ByVal sourceURI As String, ByVal targetURI As String)

            Invoke("Notifiy", New Object() {sourceURI, targetURI})

        End Sub

#End Region

#Region " Private Methods "

        Private Function GetPingBackURL(ByVal pageText As String, ByVal url As String, ByVal PostUrl As String) As String
            If Not Regex.IsMatch(pageText, PostUrl, RegexOptions.IgnoreCase Or RegexOptions.Singleline) Then
                If Not pageText Is Nothing Then
                    Dim pat As String = "<link rel=\""pingback\"" href=\""([^\""]+)\"" ?/?>"
                    Dim reg As Regex = New Regex(pat, RegexOptions.IgnoreCase Or RegexOptions.Singleline)
                    Dim m As Match = reg.Match(pageText)
                    If m.Success Then
                        Return m.Result("$1")
                    End If
                End If
            End If
            Return Nothing
        End Function

#End Region

    End Class

End Namespace
