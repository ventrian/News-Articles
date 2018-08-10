

Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Ventrian.NewsArticles.Components.Common

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Services.Localization

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

	Public Class TokenProcessor

#Region " Private Methods "

		Private Shared Function GetModuleLink(ByVal key As String, ByVal moduleContext As NewsArticleModuleBase) As String
			If moduleContext IsNot Nothing AndAlso
				moduleContext.ModuleConfiguration IsNot Nothing AndAlso
				Not String.IsNullOrEmpty(moduleContext.ModuleConfiguration.CultureCode) AndAlso
				moduleContext.ModuleConfiguration.CultureCode.Trim() <> "" Then

				Return Common.GetModuleLink(moduleContext.TabId, moduleContext.ModuleId, key, moduleContext.ArticleSettings, "language=" + moduleContext.ModuleConfiguration.CultureCode)

			Else

				Return Common.GetModuleLink(moduleContext.TabId, moduleContext.ModuleId, key, moduleContext.ArticleSettings)

			End If
		End Function

#End Region

#Region " Process Menu "

		Public Shared Sub ProcessMenu(ByRef placeHolder As ControlCollection, ByRef moduleContext As NewsArticleModuleBase, ByVal selectedMenu As MenuOptionType)

			Dim objLayoutController As New LayoutController(moduleContext)
			Dim objLayout As LayoutInfo = LayoutController.GetLayout(moduleContext, LayoutType.Menu_Item_Html)

			For iPtr As Integer = 0 To objLayout.Tokens.Length - 1 Step 2

				placeHolder.Add(New LiteralControl(objLayoutController.ProcessImages(objLayout.Tokens(iPtr).ToString())))

				If iPtr < objLayout.Tokens.Length - 1 Then
					ProcessMenuItem(objLayout.Tokens(iPtr + 1), placeHolder, objLayoutController, moduleContext, iPtr, objLayout.Tokens, selectedMenu)
				End If

			Next

		End Sub

		Public Shared Sub ProcessMenuItem(ByVal token As String, ByRef objPlaceHolder As ControlCollection, ByVal objLayoutController As LayoutController, ByVal moduleContext As NewsArticleModuleBase, ByRef iPtr As Integer, ByVal templateArray As String(), ByVal selectedMenu As MenuOptionType)

			'Dim path As String = objPage.TemplateSourceDirectory & "/DesktopModules/DnnForge - NewsArticles/" & Localization.LocalResourceDirectory & "/" & Localization.LocalSharedResourceFile
			'path = "~" & path.Substring(path.IndexOf("/DesktopModules/"), path.Length - path.IndexOf("/DesktopModules/"))

			Dim path As String = "~/DesktopModules/DnnForge - NewsArticles/" & Localization.LocalResourceDirectory & "/" & Localization.LocalSharedResourceFile

			Select Case token

				Case "ADMINLINK"
					Dim objLiteral As New Literal

					Dim parameters As New List(Of String)
					parameters.Add("mid=" & moduleContext.ModuleId)

					If (moduleContext.ArticleSettings.AuthorUserIDFilter) Then
						If (moduleContext.ArticleSettings.AuthorUserIDParam <> "") Then
							If (HttpContext.Current.Request(moduleContext.ArticleSettings.AuthorUserIDParam) <> "") Then
								parameters.Add(moduleContext.ArticleSettings.AuthorUserIDParam & "=" & HttpContext.Current.Request(moduleContext.ArticleSettings.AuthorUserIDParam))
							End If
						End If
					End If

					If (moduleContext.ArticleSettings.AuthorUsernameFilter) Then
						If (moduleContext.ArticleSettings.AuthorUsernameParam <> "") Then
							If (HttpContext.Current.Request(moduleContext.ArticleSettings.AuthorUsernameParam) <> "") Then
								parameters.Add(moduleContext.ArticleSettings.AuthorUsernameParam & "=" & HttpContext.Current.Request(moduleContext.ArticleSettings.AuthorUsernameParam))
							End If
						End If
					End If

					objLiteral.Text = NavigateURL(moduleContext.TabId, "AdminOptions", parameters.ToArray())
					objPlaceHolder.Add(objLiteral)

				Case "ARCHIVESLINK"
					Dim objLiteral As New Literal
					objLiteral.Text = GetModuleLink("Archives", moduleContext)
					objPlaceHolder.Add(objLiteral)

				Case "APPROVEARTICLESLINK"
					Dim objLiteral As New Literal
					objLiteral.Text = GetModuleLink("ApproveArticles", moduleContext)
					objPlaceHolder.Add(objLiteral)

				Case "APPROVECOMMENTSLINK"
					Dim objLiteral As New Literal
					objLiteral.Text = GetModuleLink("ApproveComments", moduleContext)
					objPlaceHolder.Add(objLiteral)

				Case "CATEGORIESLINK"
					Dim objLiteral As New Literal
					objLiteral.Text = GetModuleLink("Archives", moduleContext)
					objPlaceHolder.Add(objLiteral)

				Case "CURRENTARTICLESLINK"
					Dim objLiteral As New Literal
					objLiteral.Text = GetModuleLink("", moduleContext)
					objPlaceHolder.Add(objLiteral)

				Case "HASCOMMENTSENABLED"
					If (moduleContext.ArticleSettings.IsCommentsEnabled = False Or moduleContext.ArticleSettings.IsCommentModerationEnabled = False) Then
						While (iPtr < templateArray.Length - 1)
							If (templateArray(iPtr + 1) = "/HASCOMMENTSENABLED") Then
								Exit While
							End If
							iPtr = iPtr + 1
						End While
					End If

				Case "/HASCOMMENTSENABLED"
					' Do Nothing

				Case "ISADMIN"
					If (moduleContext.ArticleSettings.IsAdmin = False) Then
						While (iPtr < templateArray.Length - 1)
							If (templateArray(iPtr + 1) = "/ISADMIN") Then
								Exit While
							End If
							iPtr = iPtr + 1
						End While
					End If

				Case "/ISADMIN"
					' Do Nothing

				Case "ISAPPROVER"
					If (moduleContext.ArticleSettings.IsApprover = False) Then
						While (iPtr < templateArray.Length - 1)
							If (templateArray(iPtr + 1) = "/ISAPPROVER") Then
								Exit While
							End If
							iPtr = iPtr + 1
						End While
					End If

				Case "/ISAPPROVER"
					' Do Nothing

				Case "ISSELECTEDADMIN"
					If (selectedMenu <> MenuOptionType.AdminOptions) Then
						While (iPtr < templateArray.Length - 1)
							If (templateArray(iPtr + 1) = "/ISSELECTEDADMIN") Then
								Exit While
							End If
							iPtr = iPtr + 1
						End While
					End If

				Case "/ISSELECTEDADMIN"
					' Do Nothing

				Case "ISSELECTEDAPPROVEARTICLES"
					If (selectedMenu <> MenuOptionType.ApproveArticles) Then
						While (iPtr < templateArray.Length - 1)
							If (templateArray(iPtr + 1) = "/ISSELECTEDAPPROVEARTICLES") Then
								Exit While
							End If
							iPtr = iPtr + 1
						End While
					End If

				Case "/ISSELECTEDAPPROVEARTICLES"
					' Do Nothing

				Case "ISSELECTEDAPPROVECOMMENTS"
					If (selectedMenu <> MenuOptionType.ApproveComments) Then
						While (iPtr < templateArray.Length - 1)
							If (templateArray(iPtr + 1) = "/ISSELECTEDAPPROVECOMMENTS") Then
								Exit While
							End If
							iPtr = iPtr + 1
						End While
					End If

				Case "/ISSELECTEDAPPROVECOMMENTS"
					' Do Nothing

				Case "ISSELECTEDCATEGORIES"
					If (selectedMenu <> MenuOptionType.Categories) Then
						While (iPtr < templateArray.Length - 1)
							If (templateArray(iPtr + 1) = "/ISSELECTEDCATEGORIES") Then
								Exit While
							End If
							iPtr = iPtr + 1
						End While
					End If

				Case "/ISSELECTEDCATEGORIES"
					' Do Nothing

				Case "ISSELECTEDCURRENTARTICLES"
					If (selectedMenu <> MenuOptionType.CurrentArticles) Then
						While (iPtr < templateArray.Length - 1)
							If (templateArray(iPtr + 1) = "/ISSELECTEDCURRENTARTICLES") Then
								Exit While
							End If
							iPtr = iPtr + 1
						End While
					End If

				Case "/ISSELECTEDCURRENTARTICLES"
					' Do Nothing

				Case "ISSELECTEDMYARTICLES"
					If (selectedMenu <> MenuOptionType.MyArticles) Then
						While (iPtr < templateArray.Length - 1)
							If (templateArray(iPtr + 1) = "/ISSELECTEDMYARTICLES") Then
								Exit While
							End If
							iPtr = iPtr + 1
						End While
					End If

				Case "/ISSELECTEDMYARTICLES"
					' Do Nothing

				Case "ISSELECTEDSEARCH"
					If (selectedMenu <> MenuOptionType.Search) Then
						While (iPtr < templateArray.Length - 1)
							If (templateArray(iPtr + 1) = "/ISSELECTEDSEARCH") Then
								Exit While
							End If
							iPtr = iPtr + 1
						End While
					End If

				Case "/ISSELECTEDSEARCH"
					' Do Nothing

				Case "ISSELECTEDSYNDICATION"
					If (selectedMenu <> MenuOptionType.Syndication) Then
						While (iPtr < templateArray.Length - 1)
							If (templateArray(iPtr + 1) = "/ISSELECTEDSYNDICATION") Then
								Exit While
							End If
							iPtr = iPtr + 1
						End While
					End If

				Case "/ISSELECTEDSYNDICATION"
					' Do Nothing

				Case "ISSELECTEDSUBMITARTICLE"
					If (selectedMenu <> MenuOptionType.SubmitArticle) Then
						While (iPtr < templateArray.Length - 1)
							If (templateArray(iPtr + 1) = "/ISSELECTEDSUBMITARTICLE") Then
								Exit While
							End If
							iPtr = iPtr + 1
						End While
					End If

				Case "/ISSELECTEDSUBMITARTICLE"
					' Do Nothing

				Case "ISSYNDICATIONENABLED"
					If (moduleContext.ArticleSettings.IsSyndicationEnabled = False) Then
						While (iPtr < templateArray.Length - 1)
							If (templateArray(iPtr + 1) = "/ISSYNDICATIONENABLED") Then
								Exit While
							End If
							iPtr = iPtr + 1
						End While
					End If

				Case "/ISSYNDICATIONENABLED"
					' Do Nothing

				Case "ISSUBMITTER"
					If (moduleContext.ArticleSettings.IsSubmitter = False) Then
						While (iPtr < templateArray.Length - 1)
							If (templateArray(iPtr + 1) = "/ISSUBMITTER") Then
								Exit While
							End If
							iPtr = iPtr + 1
						End While
					End If

				Case "/ISSUBMITTER"
					' Do Nothing

				Case "MYARTICLESLINK"
					Dim objLiteral As New Literal
					objLiteral.Text = GetModuleLink("MyArticles", moduleContext)
					objPlaceHolder.Add(objLiteral)

				Case "RSSLATESTLINK"
					Dim objLiteral As New Literal
					Dim authorIDParam As String = ""
					If (moduleContext.ArticleSettings.AuthorUserIDFilter) Then
						If (moduleContext.ArticleSettings.AuthorUserIDParam <> "") Then
							If (HttpContext.Current.Request(moduleContext.ArticleSettings.AuthorUserIDParam) <> "") Then
								authorIDParam = "&amp;AuthorID=" & HttpContext.Current.Request(moduleContext.ArticleSettings.AuthorUserIDParam)
							End If
						End If
					End If

					If (moduleContext.ArticleSettings.AuthorUsernameFilter) Then
						If (moduleContext.ArticleSettings.AuthorUsernameParam <> "") Then
							If (HttpContext.Current.Request(moduleContext.ArticleSettings.AuthorUsernameParam) <> "") Then
								Try
									Dim objUser As Entities.Users.UserInfo = Entities.Users.UserController.GetUserByName(PortalController.GetCurrentPortalSettings().PortalId, HttpContext.Current.Request.QueryString(moduleContext.ArticleSettings.AuthorUsernameParam))
									If (objUser IsNot Nothing) Then
										authorIDParam = "&amp;AuthorID=" & objUser.UserID.ToString()
									End If
								Catch
								End Try
							End If
						End If
					End If
					objLiteral.Text = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge%20-%20NewsArticles/Rss.aspx") & "?TabID=" & moduleContext.TabId & "&amp;ModuleID=" & moduleContext.ModuleId & "&amp;MaxCount=25" & authorIDParam
					objPlaceHolder.Add(objLiteral)

				Case "SEARCHLINK"
					Dim objLiteral As New Literal
					objLiteral.Text = GetModuleLink("Search", moduleContext)
					objPlaceHolder.Add(objLiteral)

				Case "SUBMITARTICLELINK"
					Dim objLiteral As New Literal
					If (moduleContext.ArticleSettings.LaunchLinks) Then
						objLiteral.Text = GetModuleLink("Edit", moduleContext)
					Else
						objLiteral.Text = GetModuleLink("SubmitNews", moduleContext)
					End If
					objPlaceHolder.Add(objLiteral)

				Case "SYNDICATIONLINK"
					Dim objLiteral As New Literal
					objLiteral.Text = GetModuleLink("Syndication", moduleContext)
					objPlaceHolder.Add(objLiteral)

				Case Else
					Dim isRendered As Boolean = False

					If (templateArray(iPtr + 1).ToUpper().StartsWith("RESX:")) Then
						Dim key As String = templateArray(iPtr + 1).Substring(5, templateArray(iPtr + 1).Length - 5)
						Dim objLiteral As New Literal
						Try
							objLiteral.Text = Localization.GetString(key & ".Text", path)
							If (objLiteral.Text = "") Then
								objLiteral.Text = templateArray(iPtr + 1).Substring(5, templateArray(iPtr + 1).Length - 5)
							End If
						Catch
							objLiteral.Text = templateArray(iPtr + 1).Substring(5, templateArray(iPtr + 1).Length - 5)
						End Try
						objLiteral.EnableViewState = False
						objPlaceHolder.Add(objLiteral)
						isRendered = True
					End If

					If (isRendered = False) Then
						Dim objLiteralOther As New Literal
						objLiteralOther.Text = "[" & templateArray(iPtr + 1) & "]"
						objLiteralOther.EnableViewState = False
						objPlaceHolder.Add(objLiteralOther)
					End If

			End Select

		End Sub


#End Region

	End Class

End Namespace
