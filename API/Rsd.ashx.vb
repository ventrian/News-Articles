Imports System.Web
Imports System.Xml
Imports Ventrian.NewsArticles.Components.Common

Namespace Ventrian.NewsArticles.API

    Public Class Rsd
        Implements IHttpHandler

        Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            context.Response.ContentType = "text/xml"

            Using data As New XmlTextWriter(context.Response.OutputStream, Encoding.UTF8)
                data.Formatting = Formatting.Indented
                data.WriteStartDocument()
                data.WriteStartElement("rsd")
                data.WriteAttributeString("version", "1.0")
                
                data.WriteStartElement("service")
                
                data.WriteElementString("engineName", "Ventrian News Articles")
                data.WriteElementString("engineLink", "http://www.ventrian.com/")
                data.WriteElementString("homePageLink", context.Request("url"))

                data.WriteStartElement("apis")

                data.WriteStartElement("api")
                data.WriteAttributeString("name", "MetaWeblog")
                data.WriteAttributeString("preferred", "true")
                data.WriteAttributeString("apiLink", ArticleUtilities.ToAbsoluteUrl("~/desktopmodules/dnnforge%20-%20newsarticles/api/metaweblog/handler.ashx"))
                data.WriteAttributeString("blogID", context.Request("id"))
                data.WriteEndElement()

                data.WriteEndElement()

                data.WriteEndElement()

                data.WriteEndElement()
                data.WriteEndDocument()
            End Using

        End Sub

        ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class

End Namespace