Namespace Ventrian.NewsArticles.Base

    Public Class NewsArticleControlBase

        Inherits UserControl

#Region " Private Members "

        Private _articleID As Integer

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

        Protected ReadOnly Property ArticleModuleBase() As NewsArticleModuleBase
            Get
                Return CType(Parent, NewsArticleModuleBase)
            End Get
        End Property

#End Region

    End Class

End Namespace
