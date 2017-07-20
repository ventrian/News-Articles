Namespace Ventrian.NewsArticles

    <Serializable()> _
    Public Class HandoutArticle

#Region " Private Members "

        Private _articleID As Integer
        Private _title As String
        Private _summary As String
        Private _sortOrder As Integer

#End Region

#Region " Public Properties "

        Public Property ArticleID() As Integer
            Get
                Return _articleID
            End Get
            Set(ByVal value As Integer)
                _articleID = value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal value As String)
                _title = value
            End Set
        End Property

        Public Property Summary() As String
            Get
                Return _summary
            End Get
            Set(ByVal value As String)
                _summary = value
            End Set
        End Property

        Public Property SortOrder() As Integer
            Get
                Return _sortOrder
            End Get
            Set(ByVal value As Integer)
                _sortOrder = value
            End Set
        End Property

#End Region

    End Class

End Namespace
