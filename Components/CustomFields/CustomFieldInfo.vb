Namespace Ventrian.NewsArticles.Components.CustomFields

    Public Class CustomFieldInfo

#Region " Private Members "

        Dim _customFieldID As Integer
        Dim _moduleID As Integer
        Dim _name As String
        Dim _fieldType As CustomFieldType
        Dim _fieldElements As String
        Dim _defaultValue As String
        Dim _caption As String
        Dim _captionHelp As String
        Dim _isRequired As Boolean
        Dim _isVisible As Boolean
        Dim _sortOrder As Integer
        Dim _validationType As CustomFieldValidationType
        Dim _regularExpression As String
        Dim _length As Integer

#End Region

#Region " Public Properties "

        Public Property CustomFieldID() As Integer
            Get
                Return _customFieldID
            End Get
            Set(ByVal Value As Integer)
                _customFieldID = Value
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

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal Value As String)
                _name = Value
            End Set
        End Property

        Public Property FieldElements() As String
            Get
                Return _fieldElements
            End Get
            Set(ByVal Value As String)
                _fieldElements = Value
            End Set
        End Property

        Public Property FieldType() As CustomFieldType
            Get
                Return _fieldType
            End Get
            Set(ByVal Value As CustomFieldType)
                _fieldType = Value
            End Set
        End Property

        Public Property DefaultValue() As String
            Get
                Return _defaultValue
            End Get
            Set(ByVal Value As String)
                _defaultValue = Value
            End Set
        End Property

        Public Property Caption() As String
            Get
                Return _caption
            End Get
            Set(ByVal Value As String)
                _caption = Value
            End Set
        End Property

        Public Property CaptionHelp() As String
            Get
                Return _captionHelp
            End Get
            Set(ByVal Value As String)
                _captionHelp = Value
            End Set
        End Property

        Public Property SortOrder() As Integer
            Get
                Return _sortOrder
            End Get
            Set(ByVal Value As Integer)
                _sortOrder = Value
            End Set
        End Property

        Public Property IsRequired() As Boolean
            Get
                Return _isRequired
            End Get
            Set(ByVal Value As Boolean)
                _isRequired = Value
            End Set
        End Property

        Public Property IsVisible() As Boolean
            Get
                Return _isVisible
            End Get
            Set(ByVal Value As Boolean)
                _isVisible = Value
            End Set
        End Property

        Public Property ValidationType() As CustomFieldValidationType
            Get
                Return _validationType
            End Get
            Set(ByVal Value As CustomFieldValidationType)
                _validationType = Value
            End Set
        End Property

        Public Property Length() As Integer
            Get
                Return _length
            End Get
            Set(ByVal Value As Integer)
                _length = Value
            End Set
        End Property

        Public Property RegularExpression() As String
            Get
                Return _regularExpression
            End Get
            Set(ByVal Value As String)
                _regularExpression = Value
            End Set
        End Property

#End Region

    End Class

End Namespace

