Imports DotNetNuke.Entities.Portals

Namespace Ventrian.NewsArticles

    Public Class FileInfo

#Region " Private Members "

        Dim _fileID As Integer
        Dim _articleID As Integer

        Dim _title As String
        Dim _fileName As String
        Dim _extension As String
        Dim _size As Integer
        Dim _contentType As String
        Dim _folder As String
        Dim _sortOrder As Integer
        Dim _fileGuid As String
        Dim _link As String

#End Region

#Region " Public Properties "

        Public Property FileID() As Integer
            Get
                Return _fileID
            End Get
            Set(ByVal value As Integer)
                _fileID = value
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

        Public Property FileGuid() As String
            Get
                Return _fileGuid
            End Get
            Set(ByVal value As String)
                _fileGuid = value
            End Set
        End Property

        Public Property Link() As String
            Get
                Return _link
            End Get
            Set(ByVal value As String)
                _link = value
            End Set
        End Property

#End Region

    End Class

End Namespace
