'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Namespace Ventrian.NewsArticles

    Public Class LayoutInfo

#Region " Private Members "

        Dim _template As String
        Dim _tokens As String()

#End Region

#Region " Public Properties "

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

#End Region

    End Class

End Namespace
