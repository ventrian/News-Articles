'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2008
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.IO
Imports System.Xml

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Services.Localization

Namespace Ventrian.NewsArticles.Components.Utility

    Public Class LocalizationUtil

#Region " Private Members "

        Private Shared _strUseLanguageInUrlDefault As String = Null.NullString

#End Region

#Region " Public Methods "

        Private Shared Function GetHostSettingAsBoolean(ByVal key As String, ByVal defaultValue As Boolean) As Boolean
            Dim retValue As Boolean = defaultValue
            Try
                Dim setting As String = DotNetNuke.Entities.Controllers.HostController.Instance.GetString(key)
                If String.IsNullOrEmpty(setting) = False Then
                    retValue = (setting.ToUpperInvariant().StartsWith("Y") OrElse setting.ToUpperInvariant = "TRUE")
                End If
            Catch ex As Exception
                'we just want to trap the error as we may not be installed so there will be no Settings
            End Try
            Return retValue
        End Function

        Private Shared Function GetPortalSettingAsBoolean(ByVal portalID As Integer, ByVal key As String, ByVal defaultValue As Boolean) As Boolean
            Dim retValue As Boolean = defaultValue
            Try
                Dim objPortalController As New PortalController
                Dim setting As String = PortalController.GetPortalSetting(key, portalID, "")
                If String.IsNullOrEmpty(setting) = False Then
                    retValue = (setting.ToUpperInvariant().StartsWith("Y") OrElse setting.ToUpperInvariant = "TRUE")
                End If
            Catch ex As Exception
                'we just want to trap the error as we may not be installed so there will be no Settings
            End Try
            Return retValue
        End Function

        Public Shared Function UseLanguageInUrl() As Boolean

            Dim hostSetting As String = DotNetNuke.Entities.Controllers.HostController.Instance.GetString("EnableUrlLanguage")
            If (hostSetting <> "") Then
                Return GetHostSettingAsBoolean("EnableUrlLanguage", True)
            End If

            Dim objSettings As PortalSettings = PortalController.GetCurrentPortalSettings()
            Dim portalSetting As String = PortalController.GetPortalSetting("EnableUrlLanguage", objSettings.PortalId, "")

            If (portalSetting <> "") Then
                Return GetPortalSettingAsBoolean(objSettings.PortalId, "EnableUrlLanguage", True)
            End If

            If (File.Exists(HttpContext.Current.Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.xml")) = False) Then
                Return GetHostSettingAsBoolean("EnableUrlLanguage", True)
            End If

            Dim cacheKey As String = ""
            Dim objPortalSettings As PortalSettings = PortalController.GetCurrentPortalSettings()
            Dim useLanguage As Boolean = False

            ' check default host setting
            If String.IsNullOrEmpty(_strUseLanguageInUrlDefault) Then
                Dim xmldoc As New XmlDocument
                Dim languageInUrl As XmlNode

                xmldoc.Load(HttpContext.Current.Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.xml"))
                languageInUrl = xmldoc.SelectSingleNode("//root/languageInUrl")
                If Not languageInUrl Is Nothing Then
                    _strUseLanguageInUrlDefault = languageInUrl.Attributes("enabled").InnerText
                Else
                    Try
                        Dim version As Integer = Convert.ToInt32(PortalController.GetCurrentPortalSettings().Version.Replace(".", ""))
                        If (version >= 490) Then
                            _strUseLanguageInUrlDefault = "true"
                        Else
                            _strUseLanguageInUrlDefault = "false"
                        End If
                    Catch
                        _strUseLanguageInUrlDefault = "false"
                    End Try
                End If
            End If
            useLanguage = Boolean.Parse(_strUseLanguageInUrlDefault)

            ' check current portal setting
            Dim FilePath As String = HttpContext.Current.Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.Portal-" + objPortalSettings.PortalId.ToString + ".xml")
            If File.Exists(FilePath) Then
                cacheKey = "dotnetnuke-uselanguageinurl" & objPortalSettings.PortalId.ToString
                Try
                    Dim o As Object = DataCache.GetCache(cacheKey)
                    If o Is Nothing Then
                        Dim xmlLocales As New XmlDocument
                        Dim bXmlLoaded As Boolean = False

                        xmlLocales.Load(FilePath)
                        bXmlLoaded = True

                        Dim d As New XmlDocument
                        d.Load(FilePath)

                        If bXmlLoaded AndAlso Not xmlLocales.SelectSingleNode("//locales/languageInUrl") Is Nothing Then
                            useLanguage = Boolean.Parse(xmlLocales.SelectSingleNode("//locales/languageInUrl").Attributes("enabled").InnerText)
                        End If
                        If DotNetNuke.Entities.Host.Host.PerformanceSetting <> Globals.PerformanceSettings.NoCaching Then
                            Dim dp As New DotNetNuke.Services.Cache.DNNCacheDependency(FilePath)
                            DataCache.SetCache(cacheKey, useLanguage, dp)
                        End If
                    Else
                        useLanguage = CType(o, Boolean)
                    End If
                Catch ex As Exception
                End Try

                Return useLanguage
            Else
                Return useLanguage
            End If

        End Function

#End Region

    End Class

End Namespace


