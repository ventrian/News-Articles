'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2005-2012
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Namespace Ventrian.NewsArticles

    Public Class ArchiveInfo

#Region " Private Methods "

        ' local property declarations
        Dim _day As Integer
        Dim _month As Integer
        Dim _year As Integer
        Dim _count As Integer

#End Region

#Region " Public Properties "

        Public Property Day() As Integer
            Get
                Return _day
            End Get
            Set(ByVal Value As Integer)
                _day = Value
            End Set
        End Property

        Public Property Month() As Integer
            Get
                Return _month
            End Get
            Set(ByVal Value As Integer)
                _month = Value
            End Set
        End Property

        Public Property Year() As Integer
            Get
                Return _year
            End Get
            Set(ByVal Value As Integer)
                _year = Value
            End Set
        End Property

        Public Property Count() As Integer
            Get
                Return _count
            End Get
            Set(ByVal Value As Integer)
                _count = Value
            End Set
        End Property

#End Region

    End Class

End Namespace