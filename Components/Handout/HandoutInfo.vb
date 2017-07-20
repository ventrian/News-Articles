Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles

    Public Class HandoutInfo

#Region " Private Members "

        Private _handoutID As Integer = Null.NullInteger
        Private _moduleID As Integer = Null.NullInteger
        Private _userID As Integer = Null.NullInteger
        Private _name As String = Null.NullString
        Private _description As String = Null.NullString

        Private _articles As List(Of HandoutArticle)

#End Region

#Region " Public Properties "

        Public Property HandoutID() As Integer
            Get
                Return _handoutID
            End Get
            Set(ByVal value As Integer)
                _handoutID = value
            End Set
        End Property

        Public Property ModuleID() As Integer
            Get
                Return _moduleID
            End Get
            Set(ByVal value As Integer)
                _moduleID = value
            End Set
        End Property

        Public Property UserID() As Integer
            Get
                Return _userID
            End Get
            Set(ByVal value As Integer)
                _userID = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

        Public Property Articles() As List(Of HandoutArticle)
            Get
                Return _articles
            End Get
            Set(ByVal value As List(Of HandoutArticle))
                _articles = value
            End Set
        End Property

#End Region

    End Class

End Namespace

