'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Namespace Ventrian.NewsArticles

    Public Class TagInfo
        Implements IComparable

#Region " Private Members "

        Dim _tagID As Integer
        Dim _moduleID As Integer
        Dim _name As String
        Dim _nameLowered As String
        Dim _usages As Integer

#End Region

#Region " Public Properties "

        Public Property TagID() As Integer
            Get
                Return _tagID
            End Get
            Set(ByVal Value As Integer)
                _tagID = Value
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

        Public Property NameLowered() As String
            Get
                Return _nameLowered
            End Get
            Set(ByVal Value As String)
                _nameLowered = Value
            End Set
        End Property

        Public Property Usages() As Integer
            Get
                Return _usages
            End Get
            Set(ByVal Value As Integer)
                _usages = Value
            End Set
        End Property

#End Region

#Region " Optional Interfaces "

        Public Function CompareTo(ByVal obj As Object) As Integer _
              Implements System.IComparable.CompareTo

            If Not TypeOf obj Is TagInfo Then
                Throw New Exception("Object is not TagInfo")
            End If

            Dim Compare As TagInfo = CType(obj, TagInfo)
            Dim result As Integer = Me.Name.CompareTo(Compare.Name)

            If result = 0 Then
                result = Me.Name.CompareTo(Compare.Name)
            End If
            Return result
        End Function

#End Region

    End Class

End Namespace