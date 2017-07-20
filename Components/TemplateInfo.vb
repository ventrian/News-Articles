'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.Xml

Namespace Ventrian.NewsArticles

    Public Class TemplateInfo

#Region "Private Members"
        Dim _template As String
        Dim _tokens As String()
        Dim _xml As XmlDocument
#End Region

#Region "Public Properties"

        Public Property Template() As String
            Get
                Return _template
            End Get
            Set(ByVal Value As String)
                _template = Value
            End Set
        End Property


        Public Property Tokens() As String()
            Get
                Return _tokens
            End Get
            Set(ByVal Value As String())
                _tokens = Value
            End Set
        End Property

        Public Property Xml() As XmlDocument
            Get
                Return _xml
            End Get
            Set(ByVal Value As XmlDocument)
                _xml = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
