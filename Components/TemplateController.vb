'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.IO
Imports System.Web.Caching
Imports System.Xml

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals

Namespace Ventrian.NewsArticles

    Public Class TemplateController

#Region " Public Methods "

        Public Function GetTemplate(ByVal name As String, ByVal portalSettings As PortalSettings, ByVal template As String, ByVal tabModuleID As Integer) As TemplateInfo
            ' Me.MapPath("Templates/" & Template & "/")
            ' Dim pathToTemplate As String = articleModuleBase.TemplatePath

            Dim pathToTemplate As String = portalSettings.HomeDirectoryMapPath & "DnnForge - News Articles\Templates\Templates\" & template & "\"

            Dim cacheKey As String = tabModuleID.ToString() & name
            Dim cacheKeyXml As String = tabModuleID.ToString() & name & ".xml"

            Dim objTemplate As TemplateInfo = CType(DataCache.GetCache(cacheKey), TemplateInfo)
            Dim objTemplateXml As TemplateInfo = CType(DataCache.GetCache(cacheKeyXml), TemplateInfo)

            If (objTemplate Is Nothing Or objTemplateXml Is Nothing) Then
                Dim delimStr As String = "[]"
                Dim delimiter As Char() = delimStr.ToCharArray()

                objTemplate = New TemplateInfo

                Dim path As String = pathToTemplate & name & ".html"
                Dim pathXml As String = pathToTemplate & name & ".xml"

                If (System.IO.File.Exists(path) = False) Then
                    ' path = articleModuleBase.MapPath("Templates/Default/") & name & ".html"
                    path = pathToTemplate & "Standard\" & name & ".html"
                End If

                Dim sr As System.IO.StreamReader = New System.IO.StreamReader(path)
                Try
                    objTemplate.Template = sr.ReadToEnd()
                Catch
                    objTemplate.Template = "<br>ERROR: UNABLE TO READ '" & name & "' TEMPLATE:"
                Finally
                    If Not sr Is Nothing Then sr.Close()
                End Try

                Dim doc As New XmlDocument
                Try
                    doc.Load(pathXml)
                Catch
                    ' Do Nothing 
                Finally
                    objTemplate.Xml = doc
                End Try

                objTemplate.Tokens = objTemplate.Template.Split(delimiter)

                DataCache.SetCache(cacheKey, objTemplate, New DotNetNuke.Services.Cache.DNNCacheDependency(path))
                DataCache.SetCache(cacheKeyXml, objTemplate, New DotNetNuke.Services.Cache.DNNCacheDependency(pathXml))
            End If

            Return objTemplate

        End Function

#End Region

    End Class

End Namespace

