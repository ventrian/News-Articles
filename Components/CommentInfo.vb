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

    Public Class CommentInfo

#Region " Private Methods "

        ' local property declarations
        Dim _commentID As Integer
        Dim _articleID As Integer
        Dim _userID As Integer
        Dim _comment As String
        Dim _createdDate As DateTime
        Dim _remoteAddress As String
        Dim _type As Integer
        Dim _trackbackUrl As String
        Dim _trackbackTitle As String
        Dim _trackbackBlogName As String
        Dim _trackbackExcerpt As String
        Dim _anonymousName As String
        Dim _anonymousEmail As String
        Dim _anonymousURL As String
        Dim _notifyMe As Boolean = Null.NullBoolean
        Dim _isApproved As Boolean = Null.NullBoolean
        Dim _approvedBy As Integer

        Dim _authorEmail As String
        Dim _authorUserName As String
        Dim _authorFirstName As String
        Dim _authorLastName As String
        Dim _authorDisplayName As String

#End Region

#Region " Public Properties "

        Public Property CommentID() As Integer
            Get
                Return _commentID
            End Get
            Set(ByVal Value As Integer)
                _commentID = Value
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


        Public Property Comment() As String
            Get
                Return _comment
            End Get
            Set(ByVal Value As String)
                _comment = Value
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

        Public Property RemoteAddress() As String
            Get
                Return _remoteAddress
            End Get
            Set(ByVal Value As String)
                _remoteAddress = Value
            End Set
        End Property

        Public Property Type() As Integer
            Get
                Return _type
            End Get
            Set(ByVal Value As Integer)
                _type = Value
            End Set
        End Property

        Public Property TrackbackUrl() As String
            Get
                Return _trackbackUrl
            End Get
            Set(ByVal Value As String)
                _trackbackUrl = Value
            End Set
        End Property

        Public Property TrackbackTitle() As String
            Get
                Return _trackbackTitle
            End Get
            Set(ByVal Value As String)
                _trackbackTitle = Value
            End Set
        End Property

        Public Property TrackbackBlogName() As String
            Get
                Return _trackbackBlogName
            End Get
            Set(ByVal Value As String)
                _trackbackBlogName = Value
            End Set
        End Property

        Public Property TrackbackExcerpt() As String
            Get
                Return _trackbackExcerpt
            End Get
            Set(ByVal Value As String)
                _trackbackExcerpt = Value
            End Set
        End Property

        Public Property NotifyMe() As Boolean
            Get
                Return _notifyMe
            End Get
            Set(ByVal Value As Boolean)
                _notifyMe = Value
            End Set
        End Property

        Public Property IsApproved() As Boolean
            Get
                Return _isApproved
            End Get
            Set(ByVal Value As Boolean)
                _isApproved = Value
            End Set
        End Property

        Public Property ApprovedBy() As Integer
            Get
                Return _approvedBy
            End Get
            Set(ByVal Value As Integer)
                _approvedBy = Value
            End Set
        End Property

        Public Property AuthorUserName() As String
            Get
                Return _authorUserName
            End Get
            Set(ByVal Value As String)
                _authorUserName = Value
            End Set
        End Property

        Public Property AuthorEmail() As String
            Get
                Return _authorEmail
            End Get
            Set(ByVal Value As String)
                _authorEmail = Value
            End Set
        End Property

        Public Property AuthorFirstName() As String
            Get
                Return _authorFirstName
            End Get
            Set(ByVal Value As String)
                _authorFirstName = Value
            End Set
        End Property

        Public Property AuthorLastName() As String
            Get
                Return _authorLastName
            End Get
            Set(ByVal Value As String)
                _authorLastName = Value
            End Set
        End Property

        Public Property AuthorDisplayName() As String
            Get
                Return _authorDisplayName
            End Get
            Set(ByVal Value As String)
                _authorDisplayName = Value
            End Set
        End Property

        Public Property AnonymousName() As String
            Get
                Return _anonymousName
            End Get
            Set(ByVal Value As String)
                _anonymousName = Value
            End Set
        End Property

        Public Property AnonymousEmail() As String
            Get
                Return _anonymousEmail
            End Get
            Set(ByVal Value As String)
                _anonymousEmail = Value
            End Set
        End Property

        Public Property AnonymousURL() As String
            Get
                Return _anonymousURL
            End Get
            Set(ByVal Value As String)
                _anonymousURL = Value
            End Set
        End Property

#End Region

    End Class

End Namespace