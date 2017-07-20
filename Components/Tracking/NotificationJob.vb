'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.Collections.Specialized

Namespace Ventrian.NewsArticles.Tracking

    Public Class NotificationJob

        Public Article As ArticleInfo
        Public ArticleLink As String
        Public PortalTitle As String

        Public Sub NotifyLinkedSites()

            Try

                Dim links As New StringCollection
                TrackHelper.BuildLinks(Article.Summary, links)
                TrackHelper.BuildLinks(Article.Body, links)

                For Each link As String In links

                    Try

                        Dim pageText As String = TrackHelper.GetPageText(link)

                        If Not (pageText Is Nothing) Then
                            Dim success As Boolean = False

                            Dim objTrackBackProxy As New TrackBackProxy
                            success = objTrackBackProxy.TrackBackPing(pageText, link, Article.Title, ArticleLink, PortalTitle, "")

                            If (success = False) Then
                                ' objEventLog.AddLog("Ping Exception", "Trackback failed ->" & link, DotNetNuke.Common.Globals.GetPortalSettings(), -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)

                                Dim objPingBackProxy As New PingBackProxy
                                objPingBackProxy.Ping(pageText, ArticleLink, link)
                            End If

                        End If

                    Catch
                    End Try

                Next

            Catch
            End Try

        End Sub

    End Class

End Namespace
