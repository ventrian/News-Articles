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

            Dim _portalSettings As PortalSettings = PortalController.GetCurrentPortalSettings

            Return dt.AddMinutes(ClientTimeZone)

        End Function

        Public Function ConvertToServerTime(ByVal dt As DateTime, ByVal ClientTimeZone As Double) As DateTime

            Dim _portalSettings As PortalSettings = PortalController.GetCurrentPortalSettings

            Return dt.AddMinutes(ClientTimeZone * -1)


        End Function

        Public ReadOnly Property ClientToServerTimeZoneFactor(ByVal serverTimeZoneOffet As Integer) As Double

            Get

                Dim objUserInfo As UserInfo = UserController.GetCurrentUserInfo
                Return FromClientToServerFactor(objUserInfo.Profile.TimeZone, serverTimeZoneOffet)

            End Get

        End Property

        Public ReadOnly Property PortalToServerTimeZoneFactor(ByVal serverTimeZoneOffet As Integer) As Double

            Get

                Dim _portalSettings As PortalSettings = PortalController.GetCurrentPortalSettings
                Return FromClientToServerFactor(_portalSettings.TimeZoneOffset, serverTimeZoneOffet)

            End Get

        End Property


        Public ReadOnly Property ServerToClientTimeZoneFactor() As Double

            Get

                Dim objUser As UserInfo = UserController.GetCurrentUserInfo()
                Dim _portalSettings As PortalSettings = PortalController.GetCurrentPortalSettings
                Return FromServerToClientFactor(objUser.Profile.TimeZone, _portalSettings.TimeZoneOffset)

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

