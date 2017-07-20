Namespace Ventrian.NewsArticles.API.MetaWebLog

    <Serializable> _
    Public Class MetaException
        Inherits Exception

#Region "Constructors"

        Public Sub New(code As String, message As String)
            MyBase.New(message)
            Me.Code = code
        End Sub

#End Region

#Region "Properties"

        Public Property Code As String

#End Region

    End Class

End Namespace
