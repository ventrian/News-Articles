'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Namespace Ventrian.NewsArticles

    Public Class RatingInfo

#Region " Private Members "

        Dim _ratingID As Integer
        Dim _articleID As Integer
        Dim _userID As Integer
        Dim _createdDate As DateTime
        Dim _rating As Double

#End Region

#Region " Public Properties "

        Public Property RatingID() As Integer
            Get
                Return _ratingID
            End Get
            Set(ByVal Value As Integer)
                _ratingID = Value
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

        Public Property UserID() As Integer
            Get
                Return _userID
            End Get
            Set(ByVal Value As Integer)
                _userID = Value
            End Set
        End Property

        Public Property CreatedDate() As DateTime
            Get
                Return _createdDate
            End Get
            Set(ByVal Value As DateTime)
                _createdDate = Value
            End Set
        End Property

        Public Property Rating() As Double
            Get
                Return _rating
            End Get
            Set(ByVal Value As Double)
                _rating = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
