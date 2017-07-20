'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Data

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Roles

Namespace Ventrian.NewsArticles

    Public Class AuthorInfo

#Region " Private Members "

        ' local property declarations
        Dim _userName As String
        Dim _userID As Integer
        Dim _firstName As String
        Dim _lastName As String
        Dim _displayName As String
        Dim _articleCount As Integer

#End Region

#Region " Public Properties "

        Public Property UserName() As String
            Get
                Return _userName
            End Get
            Set(ByVal Value As String)
                _userName = Value
            End Set
        End Property

        Public Property UserID() As Integer
            Get
                Return _userID
            End Get
            Set(ByVal Value As Integer)
                _userID = Value
            End Set
        End Property

        Public Property FirstName() As String
            Get
                Return _firstName
            End Get
            Set(ByVal Value As String)
                _firstName = Value
            End Set
        End Property

        Public Property LastName() As String
            Get
                Return _lastName
            End Get
            Set(ByVal Value As String)
                _lastName = Value
            End Set
        End Property

        Public Property DisplayName() As String
            Get
                Return _displayName
            End Get
            Set(ByVal Value As String)
                _displayName = Value
            End Set
        End Property

        Public ReadOnly Property FullName() As String
            Get
                Return FirstName & " " & LastName
            End Get
        End Property

        Public Property ArticleCount() As Integer
            Get
                Return _articleCount
            End Get
            Set(ByVal Value As Integer)
                _articleCount = Value
            End Set
        End Property

#End Region

    End Class

End Namespace