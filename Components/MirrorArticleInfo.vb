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

    Public Class MirrorArticleInfo

#Region " Private Methods "

        ' local property declarations
        Dim _articleID As Integer
        Dim _linkedArticleID As Integer
        Dim _linkedPortalID As Integer
        Dim _autoUpdate As Boolean
        Dim _portalName As String = ""
        Dim _portalID As Integer

#End Region

#Region " Public Properties "

        Public Property ArticleID() As Integer
            Get
                Return _articleID
            End Get
            Set(ByVal Value As Integer)
                _articleID = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _portalID
            End Get
            Set(ByVal Value As Integer)
                _portalID = Value
            End Set
        End Property

        Public Property LinkedArticleID() As Integer
            Get
                Return _linkedArticleID
            End Get
            Set(ByVal Value As Integer)
                _linkedArticleID = Value
            End Set
        End Property

        Public Property LinkedPortalID() As Integer
            Get
                Return _linkedPortalID
            End Get
            Set(ByVal Value As Integer)
                _linkedPortalID = Value
            End Set
        End Property

        Public Property AutoUpdate() As Boolean
            Get
                Return _autoUpdate
            End Get
            Set(ByVal Value As Boolean)
                _autoUpdate = Value
            End Set
        End Property

        Public ReadOnly Property PortalName() As String
            Get
                If (_portalName = "") Then
                    Dim objPortalController As New PortalController()
                    Dim objPortal As PortalInfo = objPortalController.GetPortal(LinkedPortalID)

                    If (objPortal IsNot Nothing) Then
                        _portalName = objPortal.PortalName

                        Dim portalAliases As IEnumerable(Of PortalAliasInfo) = PortalAliasController.Instance.GetPortalAliasesByPortalId(_linkedPortalID)

                        If (portalAliases.Count > 0) Then
                            _portalName = DotNetNuke.Common.AddHTTP(portalAliases(0).HTTPAlias)
                        End If
                    End If
                End If
                Return _portalName
            End Get
        End Property

#End Region

    End Class

End Namespace