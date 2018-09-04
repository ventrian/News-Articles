'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Data
Imports System.Linq

Imports DotNetNuke
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals

Namespace Ventrian.NewsArticles

    Public Class ContentSharingInfo

#Region " Private Methods "

        ' local property declarations
        Dim _linkedPortalID As Integer
        Dim _portalTitle As String
        Dim _linkedTabID As Integer
        Dim _tabTitle As String
        Dim _linkedModuleID As Integer
        Dim _moduleTitle As String

#End Region

#Region " Public Properties "

        Public Property LinkedPortalID() As Integer
            Get
                Return _linkedPortalID
            End Get
            Set(ByVal Value As Integer)
                _linkedPortalID = Value
            End Set
        End Property

        Public ReadOnly Property PortalTitle() As String
            Get
                If (_portalTitle = "") Then
                    Dim objPortalController As New PortalController()
                    Dim objPortal As PortalInfo = objPortalController.GetPortal(LinkedPortalID)

                    If (objPortal IsNot Nothing) Then
                        _portalTitle = objPortal.PortalName

                        Dim o As New PortalAliasController
                        Dim portalAliases As IEnumerable(Of PortalAliasInfo) = PortalAliasController.Instance.GetPortalAliasesByPortalId(_linkedPortalID)

                        If (portalAliases.Count > 0) Then
                            _portalTitle = DotNetNuke.Common.AddHTTP(portalAliases(0).HTTPAlias)
                        End If
                    End If

                End If
                Return _portalTitle
            End Get
        End Property

        Public Property LinkedTabID() As Integer
            Get
                Return _linkedTabID
            End Get
            Set(ByVal Value As Integer)
                _linkedTabID = Value
            End Set
        End Property

        Public Property TabTitle() As String
            Get
                Return _tabTitle
            End Get
            Set(ByVal Value As String)
                _tabTitle = Value
            End Set
        End Property

        Public Property LinkedModuleID() As Integer
            Get
                Return _linkedModuleID
            End Get
            Set(ByVal Value As Integer)
                _linkedModuleID = Value
            End Set
        End Property

        Public Property ModuleTitle() As String
            Get
                Return _moduleTitle
            End Get
            Set(ByVal Value As String)
                _moduleTitle = Value
            End Set
        End Property

        Public ReadOnly Property Title() As String
            Get
                Return PortalTitle & " -> " & _tabTitle & " -> " & _moduleTitle
            End Get
        End Property

        Public ReadOnly Property LinkedID() As String
            Get
                Return _linkedPortalID & "-" & _linkedModuleID
            End Get
        End Property

#End Region

    End Class

End Namespace