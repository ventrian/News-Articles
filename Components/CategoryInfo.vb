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

    Public Class CategoryInfo

#Region " Private Methods "

        ' local property declarations
        Dim _categoryID As Integer
        Dim _moduleID As Integer
        Dim _parentID As Integer
        Dim _name As String
        Dim _nameIndented As String
        Dim _description As String
        Dim _image As String
        Dim _level As Integer
        Dim _sortOrder As Integer
        Dim _inheritSecurity As Boolean
        Dim _categorySecurityType As CategorySecurityType

        Dim _numberOfArticles As Integer
        Dim _numberOfViews As Integer

        Dim _metaTitle As String
        Dim _metaDescription As String
        Dim _metaKeywords As String

        Dim _levelParent As Integer = Null.NullInteger

#End Region

#Region " Public Properties "

        Public Property CategoryID() As Integer
            Get
                Return _categoryID
            End Get
            Set(ByVal Value As Integer)
                _categoryID = Value
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

        Public Property ParentID() As Integer
            Get
                Return _parentID
            End Get
            Set(ByVal Value As Integer)
                _parentID = Value
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

        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal Value As String)
                _description = Value
            End Set
        End Property

        Public Property NameIndented() As String
            Get
                If (Level = 1) Then
                    Return Name
                Else
                    If ((Level - 1) * 2 > 0) Then
                        Dim a As New String("."c, (Level - 1) * 2)
                        Return a & Name
                    Else
                        Return Name
                    End If
                End If
            End Get
            Set(ByVal Value As String)
                _nameIndented = Value
            End Set
        End Property

        Public Property Image() As String
            Get
                Return _image
            End Get
            Set(ByVal Value As String)
                _image = Value
            End Set
        End Property

        Public Property Level() As Integer
            Get
                Return _level
            End Get
            Set(ByVal Value As Integer)
                _level = Value
            End Set
        End Property

        Public Property LevelParent() As Integer
            Get
                Return _levelParent
            End Get
            Set(ByVal Value As Integer)
                _levelParent = Value
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

        Public Property InheritSecurity() As Boolean
            Get
                Return _inheritSecurity
            End Get
            Set(ByVal Value As Boolean)
                _inheritSecurity = Value
            End Set
        End Property

        Public Property CategorySecurityType() As CategorySecurityType
            Get
                Return _categorySecurityType
            End Get
            Set(ByVal Value As CategorySecurityType)
                _categorySecurityType = Value
            End Set
        End Property

        Public Property NumberOfArticles() As Integer
            Get
                Return _numberOfArticles
            End Get
            Set(ByVal Value As Integer)
                _numberOfArticles = Value
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

#End Region

#Region " Public Methods "

        Public Function Clone() As CategoryInfo
            Return DirectCast(Me.MemberwiseClone(), CategoryInfo)
        End Function

#End Region

    End Class

End Namespace