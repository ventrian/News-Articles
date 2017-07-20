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
Imports Ventrian.NewsArticles.Components.CustomFields

Namespace Ventrian.NewsArticles

    Public Class ArticleInfo

#Region " Private Members "

        ' local property declarations
        Dim _articleID As Integer
        Dim _authorID As Integer
        Dim _approverID As Integer
        Dim _createdDate As DateTime
        Dim _lastUpdate As DateTime
        Dim _title As String
        Dim _summary As String
        Dim _articleText As String
        Dim _isApproved As Boolean
        Dim _isDraft As Boolean
        Dim _numberOfViews As Integer
        Dim _startDate As DateTime
        Dim _endDate As DateTime
        Dim _moduleID As Integer
        Dim _isFeatured As Boolean
        Dim _rating As Double
        Dim _ratingCount As Integer
        Dim _lastUpdateID As Integer
        Dim _isSecure As Boolean
        Dim _isNewWindow As Boolean

        Dim _metaTitle As String
        Dim _metaDescription As String
        Dim _metaKeywords As String
        Dim _pageHeadText As String
        Dim _shortUrl As String
        Dim _rssGuid As String

        Dim _imageUrl As String
        Dim _url As String

        Dim _authorEmail As String
        Dim _authorUserName As String
        Dim _authorFirstName As String
        Dim _authorLastName As String
        Dim _authorDisplayName As String

        Dim _lastUpdateEmail As String
        Dim _lastUpdateUserName As String
        Dim _lastUpdateFirstName As String
        Dim _lastUpdateLastName As String
        Dim _lastUpdateDisplayName As String

        Dim _body As String
        Dim _pageCount As Integer
        Dim _commentCount As Integer
        Dim _fileCount As Integer
        Dim _imageCount As Integer
        Dim _imageUrlResolved As String

        Dim _customList As Hashtable

        Dim _tags As String

        Dim _approver As UserInfo

#End Region

#Region " Private Methods "

        Private Sub InitializePropertyList()

            ' Add Caching 
            Dim objCustomFieldController As New CustomFieldController
            Dim objCustomFields As ArrayList = objCustomFieldController.List(Me.ModuleID)

            Dim objCustomValueController As New CustomValueController
            Dim objCustomValues As List(Of CustomValueInfo) = objCustomValueController.List(Me.ArticleID)

            _customList = New Hashtable

            For Each objCustomField As CustomFieldInfo In objCustomFields
                Dim value As String = ""
                For Each objCustomValue As CustomValueInfo In objCustomValues
                    If (objCustomValue.CustomFieldID = objCustomField.CustomFieldID) Then
                        value = objCustomValue.CustomValue
                    End If
                Next
                _customList.Add(objCustomField.CustomFieldID, value)
            Next

        End Sub

#End Region

#Region " Public Properties "

        Public Property ArticleID() As Integer
            Get
                Return _articleID
            End Get
            Set(ByVal Value As Integer)
                _articleID = Value
            End Set
        End Property

        Public Property AuthorID() As Integer
            Get
                Return _authorID
            End Get
            Set(ByVal Value As Integer)
                _authorID = Value
            End Set
        End Property

        Public Property ApproverID() As Integer
            Get
                Return _approverID
            End Get
            Set(ByVal Value As Integer)
                _approverID = Value
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

        Public Property LastUpdate() As DateTime
            Get
                Return _lastUpdate
            End Get
            Set(ByVal Value As DateTime)
                _lastUpdate = Value
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

        Public Property Summary() As String
            Get
                Return _summary
            End Get
            Set(ByVal Value As String)
                _summary = Value
            End Set
        End Property

        Public Property ArticleText() As String
            Get
                Return _articleText
            End Get
            Set(ByVal Value As String)
                _articleText = Value
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

        Public Property IsDraft() As Boolean
            Get
                Return _isDraft
            End Get
            Set(ByVal Value As Boolean)
                _isDraft = Value
            End Set
        End Property

        Public Property NumberOfViews() As Integer
            Get
                Return _numberOfViews
            End Get
            Set(ByVal Value As Integer)
                _numberOfViews = Value
            End Set
        End Property

        Public Property StartDate() As DateTime
            Get
                Return _startDate
            End Get
            Set(ByVal Value As DateTime)
                _startDate = Value
            End Set
        End Property

        Public Property EndDate() As DateTime
            Get
                Return _endDate
            End Get
            Set(ByVal Value As DateTime)
                _endDate = Value
            End Set
        End Property

        Public Property ModuleID() As Integer
            Get
                Return _moduleID
            End Get
            Set(ByVal Value As Integer)
                _moduleID = Value
            End Set
        End Property

        Public Property ImageUrl() As String
            Get
                Return _imageUrl
            End Get
            Set(ByVal Value As String)
                _imageUrl = Value
            End Set
        End Property

        Public Property Url() As String
            Get
                Return _url
            End Get
            Set(ByVal Value As String)
                _url = Value
            End Set
        End Property

        Public Property IsFeatured() As Boolean
            Get
                Return _isFeatured
            End Get
            Set(ByVal Value As Boolean)
                _isFeatured = Value
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

        Public Property RatingCount() As Integer
            Get
                Return _ratingCount
            End Get
            Set(ByVal Value As Integer)
                _ratingCount = Value
            End Set
        End Property

        Public Property LastUpdateID() As Integer
            Get
                Return _lastUpdateID
            End Get
            Set(ByVal Value As Integer)
                _lastUpdateID = Value
            End Set
        End Property

        Public Property IsSecure() As Boolean
            Get
                Return _isSecure
            End Get
            Set(ByVal Value As Boolean)
                _isSecure = Value
            End Set
        End Property

        Public Property IsNewWindow() As Boolean
            Get
                Return _isNewWindow
            End Get
            Set(ByVal Value As Boolean)
                _isNewWindow = Value
            End Set
        End Property

        Public Property MetaTitle() As String
            Get
                Return _metaTitle
            End Get
            Set(ByVal Value As String)
                _metaTitle = Value
            End Set
        End Property

        Public Property MetaDescription() As String
            Get
                Return _metaDescription
            End Get
            Set(ByVal Value As String)
                _metaDescription = Value
            End Set
        End Property

        Public Property MetaKeywords() As String
            Get
                Return _metaKeywords
            End Get
            Set(ByVal Value As String)
                _metaKeywords = Value
            End Set
        End Property

        Public Property PageHeadText() As String
            Get
                Return _pageHeadText
            End Get
            Set(ByVal Value As String)
                _pageHeadText = Value
            End Set
        End Property

        Public Property ShortUrl() As String
            Get
                Return _shortUrl
            End Get
            Set(ByVal Value As String)
                _shortUrl = Value
            End Set
        End Property

        Public Property RssGuid() As String
            Get
                Return _rssGuid
            End Get
            Set(ByVal Value As String)
                _rssGuid = Value
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

        Public Property Status() As StatusType
            Get
                If (IsDraft = True) Then
                    Return StatusType.Draft
                End If

                If (IsApproved = False) Then
                    Return StatusType.AwaitingApproval
                Else
                    Return StatusType.Published
                End If
            End Get
            Set(ByVal Value As StatusType)
                Select Case Value
                    Case StatusType.Draft
                        _isDraft = True
                        _isApproved = False
                        Exit Select
                    Case StatusType.AwaitingApproval
                        _isDraft = False
                        _isApproved = False
                        Exit Select
                    Case StatusType.Published
                        _isDraft = False
                        _isApproved = True
                        Exit Select
                    Case Else
                        Exit Select
                End Select
            End Set
        End Property

        Public ReadOnly Property AuthorFullName() As String
            Get
                Return _authorFirstName & " " & _authorLastName
            End Get
        End Property

        Public Property LastUpdateUserName() As String
            Get
                Return _lastUpdateUserName
            End Get
            Set(ByVal Value As String)
                _lastUpdateUserName = Value
            End Set
        End Property

        Public Property LastUpdateEmail() As String
            Get
                Return _lastUpdateEmail
            End Get
            Set(ByVal Value As String)
                _lastUpdateEmail = Value
            End Set
        End Property

        Public Property LastUpdateFirstName() As String
            Get
                Return _lastUpdateFirstName
            End Get
            Set(ByVal Value As String)
                _lastUpdateFirstName = Value
            End Set
        End Property

        Public Property LastUpdateLastName() As String
            Get
                Return _lastUpdateLastName
            End Get
            Set(ByVal Value As String)
                _lastUpdateLastName = Value
            End Set
        End Property

        Public Property LastUpdateDisplayName() As String
            Get
                Return _lastUpdateDisplayName
            End Get
            Set(ByVal Value As String)
                _lastUpdateDisplayName = Value
            End Set
        End Property

        Public ReadOnly Property LastUpdateFullName() As String
            Get
                Return _lastUpdateFirstName & " " & _lastUpdateLastName
            End Get
        End Property

        Public Property Body() As String
            Get
                Return _body
            End Get
            Set(ByVal Value As String)
                _body = Value
            End Set
        End Property

        Public Property PageCount() As Integer
            Get
                Return _pageCount
            End Get
            Set(ByVal Value As Integer)
                _pageCount = Value
            End Set
        End Property

        Public Property CommentCount() As Integer
            Get
                Return _commentCount
            End Get
            Set(ByVal Value As Integer)
                _commentCount = Value
            End Set
        End Property

        Public Property FileCount() As Integer
            Get
                Return _fileCount
            End Get
            Set(ByVal Value As Integer)
                _fileCount = Value
            End Set
        End Property

        Public Property ImageCount() As Integer
            Get
                Return _imageCount
            End Get
            Set(ByVal Value As Integer)
                _imageCount = Value
            End Set
        End Property

        Public ReadOnly Property CustomList() As Hashtable
            Get
                If (_customList Is Nothing) Then
                    InitializePropertyList()
                End If
                Return _customList
            End Get
        End Property

        Public Property Tags() As String
            Get
                Return _tags
            End Get
            Set(ByVal Value As String)
                _tags = Value
            End Set
        End Property

        Public ReadOnly Property Approver(ByVal portalID As Integer) As UserInfo
            Get
                If (_approver Is Nothing And ApproverID <> Null.NullInteger) Then
                    Dim objUserController As New UserController
                    _approver = objUserController.GetUser(portalID, ApproverID)
                End If
                Return _approver
            End Get
        End Property

        Public ReadOnly Property TitleAlternate() As String
            Get
                Return "[" & StartDate.Year.ToString() & "-" & StartDate.Month.ToString() & "-" & StartDate.Day.ToString() & "] " & Title
            End Get
        End Property

#End Region

    End Class

End Namespace
