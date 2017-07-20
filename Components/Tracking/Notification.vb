'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common.Utilities

Imports System.Threading

Namespace Ventrian.NewsArticles.Tracking

    Public Class Notification

        Dim t As Thread

        Public Sub NotifyExternalSites(ByVal objArticle As ArticleInfo, ByVal articleLink As String, ByVal portalTitle As String)

            Dim objNotification As New NotificationJob
            objNotification.Article = objArticle
            objNotification.ArticleLink = articleLink
            objNotification.PortalTitle = portalTitle

            Dim t As New Thread(AddressOf objNotification.NotifyLinkedSites)
            t.IsBackground = True
            t.Start()

        End Sub

        Public Sub NotifyWeblogs(ByVal articleLink As String, ByVal portalTitle As String)

            Dim objPing As New PingJob
            Dim t As New Thread(AddressOf objPing.NotifyWeblogs)
            objPing.ArticleLink = articleLink
            objPing.PortalTitle = portalTitle
            t.IsBackground = True
            t.Start()

        End Sub


    End Class

End Namespace
