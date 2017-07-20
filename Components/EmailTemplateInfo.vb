'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Namespace Ventrian.NewsArticles

    Public Class EmailTemplateInfo

#Region "Private Members"
        Dim _templateID As Integer
        Dim _moduleID As Integer
        Dim _name As String
        Dim _subject As String
        Dim _template As String
#End Region

#Region "Constructors"
        Public Sub New()
        End Sub

        Public Sub New(ByVal templateID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal subject As String, ByVal template As String)
            Me.TemplateID = templateID
            Me.ModuleID = moduleID
            Me.Name = name
            Me.Subject = subject
            Me.Template = template
        End Sub
#End Region

#Region "Public Properties"
        Public Property TemplateID() As Integer
            Get
                Return _templateID
            End Get
            Set(ByVal Value As Integer)
                _templateID = Value
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

        Public Property Subject() As String
            Get
                Return _subject
            End Get
            Set(ByVal Value As String)
                _subject = Value
            End Set
        End Property

        Public Property Template() As String
            Get
                Return _template
            End Get
            Set(ByVal Value As String)
                _template = Value
            End Set
        End Property
#End Region

    End Class

End Namespace
