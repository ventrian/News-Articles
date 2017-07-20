Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles.Import

    Public Class FeedInfo

#Region " Private Members "

        Dim _feedID As Integer = Null.NullInteger
        Dim _moduleID As Integer
        Dim _title As String
        Dim _url As String
        Dim _userID As Integer
        Dim _autoFeature As Boolean
        Dim _isActive As Boolean
        Dim _dateMode As FeedDateMode

        Dim _autoExpire As Integer
        Dim _autoExpireUnit As FeedExpiryType

        Dim _categories As List(Of CategoryInfo)

#End Region

#Region " Private Properties "

        Public Property FeedID() As Integer
            Get
                Return _feedID
            End Get
            Set(ByVal Value As Integer)
                _feedID = Value
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

        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal Value As String)
                _title = Value
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

        Public Property UserID() As Integer
            Get
                Return _userID
            End Get
            Set(ByVal Value As Integer)
                _userID = Value
            End Set
        End Property

        Public Property AutoFeature() As Boolean
            Get
                Return _autoFeature
            End Get
            Set(ByVal Value As Boolean)
                _autoFeature = Value
            End Set
        End Property

        Public Property IsActive() As Boolean
            Get
                Return _isActive
            End Get
            Set(ByVal Value As Boolean)
                _isActive = Value
            End Set
        End Property

        Public Property DateMode() As FeedDateMode
            Get
                Return _dateMode
            End Get
            Set(ByVal Value As FeedDateMode)
                _dateMode = Value
            End Set
        End Property

        Public Property AutoExpire() As Integer
            Get
                Return _autoExpire
            End Get
            Set(ByVal Value As Integer)
                _autoExpire = Value
            End Set
        End Property

        Public Property AutoExpireUnit() As FeedExpiryType
            Get
                Return _autoExpireUnit
            End Get
            Set(ByVal Value As FeedExpiryType)
                _autoExpireUnit = Value
            End Set
        End Property

        Public Property Categories() As List(Of CategoryInfo)
            Get
                If (_categories Is Nothing) Then
                    If (_feedID = Null.NullInteger) Then
                        _categories = New List(Of CategoryInfo)
                    Else
                        Dim objFeedController As New FeedController
                        _categories = objFeedController.GetFeedCategoryList(_feedID)
                    End If
                End If
                Return _categories
            End Get
            Set(ByVal Value As List(Of CategoryInfo))
                _categories = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
