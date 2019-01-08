'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Xml

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Security
Imports DotNetNuke.Security.Permissions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.NewsArticles.Base

    Public Class NewsArticleModuleBase

        Inherits PortalModuleBase

#Region " Private Members "

        Private _articleSettings As ArticleSettings

#End Region

#Region " Public Properties "

        <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
        Public ReadOnly Property BasePage() As DotNetNuke.Framework.CDefault
            Get
                Return CType(Page, DotNetNuke.Framework.CDefault)
            End Get
        End Property

        Public ReadOnly Property ArticleSettings() As ArticleSettings
            Get
                If (_articleSettings Is Nothing) Then
                    Try
                        _articleSettings = New ArticleSettings(Settings, PortalSettings, ModuleConfiguration)
                    Catch
                        Dim objSettings As Hashtable = ModuleConfiguration.ModuleSettings
                        Dim objTabSettings As Hashtable = ModuleConfiguration.TabModuleSettings

                        For Each item As DictionaryEntry In objTabSettings
                            If (objSettings.ContainsKey(item.Key) = False) Then
                                objSettings.Add(item.Key, item.Value)
                            End If
                        Next

                        _articleSettings = New ArticleSettings(objSettings, PortalSettings, ModuleConfiguration)
                        ModuleController.Instance.UpdateModuleSetting(ModuleId, "ResetArticleSettings", "true")
                    End Try
                End If
                Return _articleSettings
            End Get
        End Property

        Public ReadOnly Property ModuleKey() As String
            Get
                Return "NewsArticles-" & TabModuleId
            End Get
        End Property

#End Region

#Region " Protected Methods "

        Protected Function EditArticleUrl(ByVal ctl As String) As String

            If (ArticleSettings.AuthorUserIDFilter) Then
                If (ArticleSettings.AuthorUserIDParam <> "") Then
                    If (HttpContext.Current.Request(ArticleSettings.AuthorUserIDParam) <> "") Then
                        Return EditUrl(ArticleSettings.AuthorUserIDParam, HttpContext.Current.Request(ArticleSettings.AuthorUserIDParam), ctl)
                    End If
                End If
            End If

            If (ArticleSettings.AuthorUsernameFilter) Then
                If (ArticleSettings.AuthorUsernameParam <> "") Then
                    If (HttpContext.Current.Request(ArticleSettings.AuthorUsernameParam) <> "") Then
                        Return EditUrl(ArticleSettings.AuthorUsernameParam, HttpContext.Current.Request(ArticleSettings.AuthorUsernameParam), ctl)
                    End If
                End If
            End If

            Return EditUrl(ctl)

        End Function

        Protected Function EditArticleUrl(ByVal ctl As String, ByVal ParamArray params() As String) As String

            Dim parameters As New List(Of String)

            parameters.Add("mid=" & ModuleId.ToString())
            parameters.AddRange(params)

            If (ArticleSettings.AuthorUserIDFilter) Then
                If (ArticleSettings.AuthorUserIDParam <> "") Then
                    If (HttpContext.Current.Request(ArticleSettings.AuthorUserIDParam) <> "") Then
                        parameters.Add(ArticleSettings.AuthorUserIDParam & "=" & HttpContext.Current.Request(ArticleSettings.AuthorUserIDParam))
                    End If
                End If
            End If

            If (ArticleSettings.AuthorUsernameFilter) Then
                If (ArticleSettings.AuthorUsernameParam <> "") Then
                    If (HttpContext.Current.Request(ArticleSettings.AuthorUsernameParam) <> "") Then
                        parameters.Add(ArticleSettings.AuthorUsernameParam & "=" & HttpContext.Current.Request(ArticleSettings.AuthorUsernameParam))
                    End If
                End If
            End If

            Return NavigateURL(TabId, ctl, parameters.ToArray)

        End Function

        Public Sub LoadStyleSheet()


            Dim objCSS As Control = BasePage.FindControl("CSS")

            If Not (objCSS Is Nothing) Then
                Dim objLink As New HtmlLink()
                objLink.ID = "Template_" & ModuleId.ToString()
                objLink.Attributes("rel") = "stylesheet"
                objLink.Attributes("type") = "text/css"
                objLink.Href = Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Templates/" & _articleSettings.Template & "/Template.css")

                objCSS.Controls.AddAt(0, objLink)
            End If

        End Sub

        Public Function GetSkinAttribute(ByVal xDoc As XmlDocument, ByVal tag As String, ByVal attrib As String, ByVal defaultValue As String) As String
            Dim retValue As String = defaultValue
            Dim xmlSkinAttributeRoot As XmlNode = xDoc.SelectSingleNode("descendant::Object[Token='[" & tag & "]']")
            ' if the token is found
            If Not xmlSkinAttributeRoot Is Nothing Then
                ' process each token attribute
                Dim xmlSkinAttribute As XmlNode
                For Each xmlSkinAttribute In xmlSkinAttributeRoot.SelectNodes(".//Settings/Setting")
                    If xmlSkinAttribute.SelectSingleNode("Value").InnerText <> "" Then
                        ' append the formatted attribute to the inner contents of the control statement
                        If xmlSkinAttribute.SelectSingleNode("Name").InnerText = attrib Then
                            retValue = xmlSkinAttribute.SelectSingleNode("Value").InnerText
                        End If
                    End If
                Next
            End If
            Return retValue
        End Function


        Protected Function FormatImageUrl(ByVal imageUrlResolved As String) As String

            Return PortalSettings.HomeDirectory & imageUrlResolved

        End Function

        Protected Function IsRated(ByVal objArticle As ArticleInfo) As Boolean

            If (objArticle.Rating = Null.NullDouble) Then
                Return False
            Else
                Return True
            End If

        End Function

        Protected Function IsRated(ByVal objDataItem As Object) As Boolean

            Dim objArticle As ArticleInfo = CType(objDataItem, ArticleInfo)

            Return IsRated(objArticle)

        End Function

        Protected Function GetRatingImage(ByVal objArticle As ArticleInfo) As String

            If (objArticle.Rating = Null.NullDouble) Then
                Return ResolveUrl("Images\Rating\stars-0-0.gif")
            Else

                Select Case RoundToUnit(objArticle.Rating, 0.5, False)

                    Case 1
                        Return ResolveUrl("Images\Rating\stars-1-0.gif")

                    Case 1.5
                        Return ResolveUrl("Images\Rating\stars-1-5.gif")

                    Case 2
                        Return ResolveUrl("Images\Rating\stars-2-0.gif")

                    Case 2.5
                        Return ResolveUrl("Images\Rating\stars-2-5.gif")

                    Case 3
                        Return ResolveUrl("Images\Rating\stars-3-0.gif")

                    Case 3.5
                        Return ResolveUrl("Images\Rating\stars-3-5.gif")

                    Case 4
                        Return ResolveUrl("Images\Rating\stars-4-0.gif")

                    Case 4.5
                        Return ResolveUrl("Images\Rating\stars-4-5.gif")

                    Case 5
                        Return ResolveUrl("Images\Rating\stars-5-0.gif")

                End Select

                Return ResolveUrl("Images\Rating\stars-0-0.gif")

            End If

        End Function

		Public Function StripHtml(ByVal html As String) As String

			Return StripHtml(html, False)

		End Function

		Public Function StripHtml(ByVal html As String, ByVal cleanLineBreaks As Boolean) As String

			Dim pattern As String = "<(.|\n)*?>"
			Return Regex.Replace(html, pattern, String.Empty).Replace(System.Environment.NewLine, " ")

		End Function

		Private Function RoundToUnit(ByVal d As Double, ByVal unit As Double, ByVal roundDown As Boolean) As Double

			If (roundDown) Then
				Return Math.Round(Math.Round((d / unit) - 0.5, 0) * unit, 2)
			Else
				Return Math.Round(Math.Round((d / unit) + 0.5, 0) * unit, 2)
			End If

		End Function

		Protected Function GetRatingImage(ByVal objDataItem As Object) As String

            Dim objArticle As ArticleInfo = CType(objDataItem, ArticleInfo)

            Return GetRatingImage(objArticle)

        End Function

        Protected Function GetUserName(ByVal dataItem As Object) As String

            Dim objArticle As ArticleInfo = CType(dataItem, ArticleInfo)

            If Not (objArticle Is Nothing) Then

                Select Case ArticleSettings.DisplayMode

                    Case DisplayType.UserName
                        Return objArticle.AuthorUserName

                    Case DisplayType.FirstName
                        Return objArticle.AuthorFirstName

                    Case DisplayType.LastName
                        Return objArticle.AuthorLastName

                    Case DisplayType.FullName
                        Return objArticle.AuthorFullName

                End Select

            End If

            Return Null.NullString

        End Function

        Protected Function GetUserName(ByVal dataItem As Object, ByVal type As Integer) As String

            Dim objArticle As ArticleInfo = CType(dataItem, ArticleInfo)

            If Not (objArticle Is Nothing) Then

                Select Case type

                    Case 1 'Last Updated

                        Select Case ArticleSettings.DisplayMode

                            Case DisplayType.UserName
                                Return objArticle.LastUpdateUserName

                            Case DisplayType.FirstName
                                Return objArticle.LastUpdateFirstName

                            Case DisplayType.LastName
                                Return objArticle.LastUpdateLastName

                            Case DisplayType.FullName
                                Return objArticle.LastUpdateDisplayName

                        End Select

                    Case Else

                        Select Case ArticleSettings.DisplayMode

                            Case DisplayType.UserName
                                Return objArticle.AuthorUserName

                            Case DisplayType.FirstName
                                Return objArticle.AuthorFirstName

                            Case DisplayType.LastName
                                Return objArticle.AuthorLastName

                            Case DisplayType.FullName
                                Return objArticle.AuthorDisplayName

                        End Select

                End Select

            End If

            Return Null.NullString

        End Function

        Protected Function HasEditRights(ByVal articleId As Integer, ByVal moduleID As Integer, ByVal tabID As Integer) As Boolean

            ' Unauthenticated User
            '
            If (Request.IsAuthenticated = False) Then

                Return False

            End If

            Dim objModuleController As ModuleController = New ModuleController

            Dim objModule As ModuleInfo = objModuleController.GetModule(moduleID, tabID)

            If Not (objModule Is Nothing) Then

                ' Admin of Module
                '
                If (ModulePermissionController.HasModuleAccess(SecurityAccessLevel.Edit, "EDIT" , objModule)) Then

                    Return True

                End If

            End If


            ' Approver
            '
            If (ArticleSettings.IsApprover) Then
                Return True
            End If

            ' Submitter of New Article
            '
            If (articleId = Null.NullInteger And ArticleSettings.IsSubmitter) Then
                Return True
            End If

            ' Owner of Article
            '
            Dim objArticleController As ArticleController = New ArticleController
            Dim objArticle As ArticleInfo = objArticleController.GetArticle(articleId)
            If Not (objArticle Is Nothing) Then
                If (objArticle.AuthorID = UserId And (objArticle.Status = StatusType.Draft Or ArticleSettings.IsAutoApprover)) Then
                    Return True
                End If
            End If

            Return False

        End Function

        Public Function GetSharedResource(ByVal key As String) As String

            Dim path As String = "~/DesktopModules/DnnForge - NewsArticles/" & Localization.LocalResourceDirectory & "/" & Localization.LocalSharedResourceFile
            Return Localization.GetString(key, path)

        End Function

#End Region

#Region " Shadowed Methods "

        Public Shadows Function ResolveUrl(ByVal url As String) As String

            Return MyBase.ResolveUrl(url).Replace(" ", "%20")

        End Function

#End Region

    End Class

End Namespace
