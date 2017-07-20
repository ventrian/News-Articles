'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Data

Imports DotNetNuke
Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles

    Public Class PageInfo

#Region " Private Methods "

        ' local property declarations
        Dim _pageID As Integer
        Dim _articleID As Integer
        Dim _title As String
        Dim _pageText As String
        Dim _sortOrder As Integer

#End Region

#Region " Public Properties "

        Public Property PageID() As Integer
            Get
                Return _pageID
            End Get
            Set(ByVal Value As Integer)
                _pageID = Value
            End Set
        End Property

        Public Property ArticleID() As Integer
            Get
                Return _articleID
            End Get
            Set(ByVal Value As Integer)
                _articleID = Value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal Value As String)
                _title = Value
            End Set
        End Property

        Public Property PageText() As String
            Get
                Return _pageText
            End Get
            Set(ByVal Value As String)
                _pageText = Value
            End Set
        End Property

        Public Property SortOrder() As Integer
            Get
                Return _sortOrder
            End Get
            Set(ByVal Value As Integer)
                _sortOrder = Value
            End Set
        End Property

#End Region

    End Class

End Namespace