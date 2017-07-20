'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Text.RegularExpressions

Imports CookComputing.XmlRpc

Namespace Ventrian.NewsArticles.Tracking

    Public Class PingProxy
        Inherits XmlRpcClientProtocol

#Region " Public Methods "

        Public Function Ping(ByVal WeblogName As String, ByVal WeblogURL As String) As WeblogsUpdateResponse
            Dim proxy As IWebLogsUpdate = CType(XmlRpcProxyGen.Create(GetType(IWebLogsUpdate)), IWebLogsUpdate)
            Return proxy.Ping(WeblogName, WeblogURL)
        End Function

        Structure WeblogsUpdateResponse
            Public flerror As Boolean
            Public message As String
        End Structure

        <XmlRpcUrl("http://rpc.weblogs.com/RPC2")> _
        Public Interface IWebLogsUpdate
            <XmlRpcMethod("weblogUpdates.ping")> _
            Function Ping(ByVal WeblogName As String, ByVal WeblogURL As String) As WeblogsUpdateResponse
        End Interface

#End Region

    End Class

End Namespace
