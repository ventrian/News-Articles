Imports System.IO

Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles

    Public Class LatestLayoutController

#Region " Public Methods "

        Public Function GetLayout(ByVal type As LatestLayoutType, ByVal moduleID As Integer, ByVal settings As Hashtable) As LayoutInfo

            Dim cacheKey As String = "LatestArticles-" & moduleID.ToString() & "-" & type.ToString()
            Dim objLayout As LayoutInfo = CType(DataCache.GetCache(cacheKey), LayoutInfo)

            If (objLayout Is Nothing) Then

                Dim delimStr As String = "[]"
                Dim delimiter As Char() = delimStr.ToCharArray()

                objLayout = New LayoutInfo
                Dim folderPath As String = HttpContext.Current.Server.MapPath("~\DesktopModules\DnnForge - LatestArticles\Templates\" & moduleID.ToString())
                Dim filePath As String = HttpContext.Current.Server.MapPath("~\DesktopModules\DnnForge - LatestArticles\Templates\" & moduleID.ToString() & "\" & type.ToString().Replace("_", "."))

                If (File.Exists(filePath) = False) Then
                    ' Load from settings... 

                    Dim _layoutMode As LayoutModeType = ArticleConstants.LATEST_ARTICLES_LAYOUT_MODE_DEFAULT
                    If (settings.Contains(ArticleConstants.LATEST_ARTICLES_LAYOUT_MODE)) Then
                        _layoutMode = CType(System.Enum.Parse(GetType(LayoutModeType), settings(ArticleConstants.LATEST_ARTICLES_LAYOUT_MODE).ToString()), LayoutModeType)
                    End If

                    Select Case type

                        Case LatestLayoutType.Listing_Header_Html

                            Dim layoutHeader As String

                            If (_layoutMode = LayoutModeType.Simple) Then
                                If (settings.Contains(ArticleConstants.SETTING_HTML_HEADER)) Then
                                    layoutHeader = settings(ArticleConstants.SETTING_HTML_HEADER).ToString()
                                Else
                                    layoutHeader = ArticleConstants.DEFAULT_HTML_HEADER
                                End If
                            Else
                                If (settings.Contains(ArticleConstants.SETTING_HTML_HEADER_ADVANCED)) Then
                                    layoutHeader = settings(ArticleConstants.SETTING_HTML_HEADER_ADVANCED).ToString()
                                Else
                                    layoutHeader = ArticleConstants.DEFAULT_HTML_HEADER_ADVANCED
                                End If
                            End If

                            If Not (Directory.Exists(folderPath)) Then
                                Directory.CreateDirectory(folderPath)
                            End If

                            File.WriteAllText(filePath, layoutHeader)
                            Exit Select

                        Case LatestLayoutType.Listing_Item_Html

                            Dim layoutItem As String

                            If (_layoutMode = LayoutModeType.Simple) Then
                                If (settings.Contains(ArticleConstants.SETTING_HTML_BODY)) Then
                                    layoutItem = settings(ArticleConstants.SETTING_HTML_BODY).ToString()
                                Else
                                    layoutItem = ArticleConstants.DEFAULT_HTML_BODY
                                End If
                            Else
                                If (settings.Contains(ArticleConstants.SETTING_HTML_BODY_ADVANCED)) Then
                                    layoutItem = settings(ArticleConstants.SETTING_HTML_BODY_ADVANCED).ToString()
                                Else
                                    layoutItem = ArticleConstants.DEFAULT_HTML_BODY_ADVANCED
                                End If
                            End If

                            If Not (Directory.Exists(folderPath)) Then
                                Directory.CreateDirectory(folderPath)
                            End If

                            File.WriteAllText(filePath, layoutItem)
                            Exit Select

                        Case LatestLayoutType.Listing_Footer_Html

                            Dim layoutFooter As String

                            If (_layoutMode = LayoutModeType.Simple) Then
                                If (settings.Contains(ArticleConstants.SETTING_HTML_FOOTER)) Then
                                    layoutFooter = settings(ArticleConstants.SETTING_HTML_FOOTER).ToString()
                                Else
                                    layoutFooter = ArticleConstants.DEFAULT_HTML_FOOTER
                                End If
                            Else
                                If (settings.Contains(ArticleConstants.SETTING_HTML_FOOTER_ADVANCED)) Then
                                    layoutFooter = settings(ArticleConstants.SETTING_HTML_FOOTER_ADVANCED).ToString()
                                Else
                                    layoutFooter = ArticleConstants.DEFAULT_HTML_FOOTER_ADVANCED
                                End If
                            End If

                            If Not (Directory.Exists(folderPath)) Then
                                Directory.CreateDirectory(folderPath)
                            End If

                            File.WriteAllText(filePath, layoutFooter)
                            Exit Select

                        Case LatestLayoutType.Listing_Empty_Html

                            Dim noArticles As String = ArticleConstants.SETTING_HTML_NO_ARTICLES
                            If (settings.Contains(ArticleConstants.SETTING_HTML_NO_ARTICLES)) Then
                                noArticles = settings(ArticleConstants.SETTING_HTML_NO_ARTICLES).ToString()
                            End If
                            noArticles = "<div id=""NoRecords"" class=""Normal"">" & noArticles & "</div>"

                            If Not (Directory.Exists(folderPath)) Then
                                Directory.CreateDirectory(folderPath)
                            End If

                            File.WriteAllText(filePath, noArticles)
                            Exit Select

                    End Select

                End If

                objLayout.Template = File.ReadAllText(filePath)
                objLayout.Tokens = objLayout.Template.Split(delimiter)

                DataCache.SetCache(cacheKey, objLayout, New DotNetNuke.Services.Cache.DNNCacheDependency(filePath))
            End If

            Return objLayout

        End Function

        Public Sub UpdateLayout(ByVal type As LatestLayoutType, ByVal moduleID As Integer, ByVal content As String)

            Dim folderPath As String = HttpContext.Current.Server.MapPath("~\DesktopModules\DnnForge - LatestArticles\Templates\" & moduleID.ToString())
            Dim filePath As String = HttpContext.Current.Server.MapPath("~\DesktopModules\DnnForge - LatestArticles\Templates\" & moduleID.ToString() & "\" & type.ToString().Replace("_", "."))

            If Not (Directory.Exists(folderPath)) Then
                Directory.CreateDirectory(folderPath)
            End If

            File.WriteAllText(filePath, content)

            Dim cacheKey As String = "LatestArticles-" & moduleID.ToString() & "-" & type.ToString()
            DataCache.RemoveCache(cacheKey)

        End Sub

#End Region

    End Class

End Namespace
