'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Data
Imports System.web

Imports DotNetNuke
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Users

Namespace Ventrian.NewsArticles

    Public Class UserTime1

        Public Sub New()

        End Sub

        Public Function ConvertToUserTime(ByVal dt As DateTime, ByVal ClientTimeZone As Double) As DateTime

            Return dt.AddMinutes(ClientTimeZone)

        End Function

        Public Function ConvertToServerTime(ByVal dt As DateTime, ByVal ClientTimeZone As Double) As DateTime

            Return dt.AddMinutes(ClientTimeZone * -1)


        End Function

        Public ReadOnly Property ClientToServerTimeZoneFactor(ByVal serverTimeZoneOffet As Integer) As Double

            Get

                Dim objUserInfo As UserInfo = UserController.Instance.GetCurrentUserInfo
                Return FromClientToServerFactor(objUserInfo.Profile.PreferredTimeZone.GetUtcOffset(DateTime.Now).TotalHours, serverTimeZoneOffet)

            End Get

        End Property

        Public ReadOnly Property PortalToServerTimeZoneFactor(ByVal serverTimeZoneOffet As Integer) As Double

            Get

                Dim _portalSettings As PortalSettings = PortalController.Instance.GetCurrentPortalSettings
                Return FromClientToServerFactor(_portalSettings.TimeZone.GetUtcOffset(DateTime.Now).TotalHours, serverTimeZoneOffet)

            End Get

        End Property


        Public ReadOnly Property ServerToClientTimeZoneFactor() As Double

            Get

                Dim objUser As UserInfo = UserController.Instance.GetCurrentUserInfo()
                Dim _portalSettings As PortalSettings = PortalController.Instance.GetCurrentPortalSettings
                Return FromServerToClientFactor(objUser.Profile.PreferredTimeZone.GetUtcOffset(DateTime.Now).TotalHours, _portalSettings.TimeZone.GetUtcOffset(DateTime.Now).TotalHours)

            End Get

        End Property

        Private Function FromClientToServerFactor(ByVal Client As Double, ByVal Server As Double) As Double

            Return Client - Server

        End Function

        Private Function FromServerToClientFactor(ByVal Client As Double, ByVal Server As Double) As Double

            Return Server - Client

        End Function

    End Class

End Namespace

