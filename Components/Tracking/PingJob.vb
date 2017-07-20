'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.Collections.Specialized

Namespace Ventrian.NewsArticles.Tracking

    Public Class PingJob

        Public ArticleLink As String
        Public PortalTitle As String

        Public Sub NotifyWeblogs()

            Try

                Dim objPing As New PingProxy
                objPing.Ping(PortalTitle, ArticleLink)

            Catch
                ' Anything can happen here, so just swallow exception
            Finally
                Threading.Thread.CurrentThread.Abort()
            End Try


        End Sub

    End Class

End Namespace
