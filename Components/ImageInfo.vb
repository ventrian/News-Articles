Namespace Ventrian.NewsArticles

    Public Class ImageInfo
        Implements ICloneable

#Region " Private Members "

        Dim _imageID As Integer
        Dim _articleID As Integer

        Dim _title As String

        Dim _fileName As String
        Dim _extension As String
        Dim _size As Integer
        Dim _width As Integer
        Dim _height As Integer
        Dim _contentType As String
        Dim _folder As String
        Dim _sortOrder As Integer
        Dim _imageGuid As String
        Dim _description As String

#End Region

#Region " Public Properties "

        Public Property ImageID() As Integer
            Get
                Return _imageID
            End Get
            Set(ByVal value As Integer)
                _imageID = value
            End Set
        End Property

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

        Public Property FileName() As String
            Get
                Return _fileName
            End Get
            Set(ByVal value As String)
                _fileName = value
            End Set
        End Property

        Public Property Extension() As String
            Get
                Return _extension
            End Get
            Set(ByVal value As String)
                _extension = value
            End Set
        End Property

        Public Property Size() As Integer
            Get
                Return _size
            End Get
            Set(ByVal value As Integer)
                _size = value
            End Set
        End Property

        Public Property Width() As Integer
            Get
                Return _width
            End Get
            Set(ByVal value As Integer)
                _width = value
            End Set
        End Property

        Public Property Height() As Integer
            Get
                Return _height
            End Get
            Set(ByVal value As Integer)
                _height = value
            End Set
        End Property

        Public Property ContentType() As String
            Get
                Return _contentType
            End Get
            Set(ByVal value As String)
                _contentType = value
            End Set
        End Property

        Public Property Folder() As String
            Get
                Return _folder
            End Get
            Set(ByVal value As String)
                _folder = value
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

        Public Property ImageGuid() As String
            Get
                Return _imageGuid
            End Get
            Set(ByVal value As String)
                _imageGuid = value
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

#End Region

        Public Function Clone() As Object Implements System.ICloneable.Clone
            Return Me.MemberwiseClone
        End Function

    End Class

End Namespace
