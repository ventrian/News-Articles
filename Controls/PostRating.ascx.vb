Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Framework
Imports Ventrian.NewsArticles.Components.Social

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles.Controls

    Partial Public Class PostRating
        Inherits System.Web.UI.UserControl

#Region " Private Members "

        Private _articleID As Integer = Null.NullInteger

#End Region

#Region " Private Properties "

        Private ReadOnly Property ArticleModuleBase() As NewsArticleModuleBase
            Get
                Return CType(Parent.Parent, NewsArticleModuleBase)
            End Get
        End Property

        Private ReadOnly Property ArticleSettings() As ArticleSettings
            Get
                Return ArticleModuleBase.ArticleSettings
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub AssignLocalization()

            lblRatingSaved.Text = GetResourceKey("RatingSaved")

        End Sub

        Public Function GetResourceKey(ByVal key As String) As String

            Dim path As String = "~/DesktopModules/DnnForge - NewsArticles/" & Localization.LocalResourceDirectory & "/PostRating.ascx.resx"
            Return DotNetNuke.Services.Localization.Localization.GetString(key, path)

        End Function

        Private Sub ReadQueryString()

            If (ArticleSettings.UrlModeType = Components.Types.UrlModeType.Shorterned) Then
                Try
                    If (IsNumeric(Request(ArticleSettings.ShortenedID))) Then
                        _articleID = Convert.ToInt32(Request(ArticleSettings.ShortenedID))
                    End If
                Catch
                End Try
            End If

            If (IsNumeric(Request("ArticleID"))) Then
                _articleID = Convert.ToInt32(Request("ArticleID"))
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not (TypeOf Parent.Parent Is NewsArticleModuleBase) Then
                Visible = False
                Return
            End If

            ReadQueryString()
            AssignLocalization()

            If (ArticleSettings.IsRateable = False) Then
                Me.Visible = False
                Return
            End If

            If (IsPostBack = False) Then

                Dim objRatingController As New RatingController
                If (Request.IsAuthenticated) Then
                    Dim objRating As RatingInfo = objRatingController.Get(_articleID, ArticleModuleBase.UserId, ArticleModuleBase.ModuleId)

                    If Not (objRating Is Nothing) Then
                        If (objRating.RatingID <> Null.NullInteger) Then
                            If Not (lstRating.Items.FindByValue(Convert.ToDouble(objRating.Rating).ToString()) Is Nothing) Then
                                lstRating.SelectedValue = Convert.ToDouble(objRating.Rating).ToString()
                            End If
                        End If
                    End If
                Else
                    Dim cookie As HttpCookie = Request.Cookies("ArticleRating" & _articleID.ToString())
                    If Not (cookie Is Nothing) Then
                        Dim objRating As RatingInfo = objRatingController.GetByID(Convert.ToInt32(cookie.Value), _articleID, ArticleModuleBase.ModuleId)

                        If Not (objRating Is Nothing) Then
                            If Not (lstRating.Items.FindByValue(Convert.ToDouble(objRating.Rating).ToString()) Is Nothing) Then
                                lstRating.SelectedValue = Convert.ToDouble(objRating.Rating).ToString()
                            End If
                        End If
                    End If
                End If

            End If

        End Sub

        Protected Sub lstRating_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstRating.SelectedIndexChanged

            If (Request.IsAuthenticated) Then

                Dim objRatingController As New RatingController

                Dim objRatingExists As RatingInfo = objRatingController.Get(_articleID, ArticleModuleBase.UserId, ArticleModuleBase.ModuleId)

                If (objRatingExists.RatingID <> Null.NullInteger) Then
                    objRatingController.Delete(objRatingExists.RatingID, _articleID, ArticleModuleBase.ModuleId)
                End If

                Dim objRating As New RatingInfo

                objRating.ArticleID = _articleID
                objRating.CreatedDate = DateTime.Now
                objRating.Rating = Convert.ToDouble(lstRating.SelectedValue)
                objRating.UserID = ArticleModuleBase.UserId

                objRating.RatingID = objRatingController.Add(objRating, ArticleModuleBase.ModuleId)

                Dim objArticleController As New ArticleController
                Dim objArticle As ArticleInfo = objArticleController.GetArticle(_articleID)

                If (ArticleSettings.EnableActiveSocialFeed And Request.IsAuthenticated) Then
                    If (ArticleSettings.ActiveSocialRateKey <> "") Then
                        If IO.File.Exists(HttpContext.Current.Server.MapPath("~/bin/active.modules.social.dll")) Then
                            Dim ai As Object = Nothing
                            Dim asm As System.Reflection.Assembly
                            Dim ac As Object = Nothing
                            Try
                                asm = System.Reflection.Assembly.Load("Active.Modules.Social")
                                ac = asm.CreateInstance("Active.Modules.Social.API.Journal")
                                If Not ac Is Nothing Then
                                    ac.AddProfileItem(New Guid(ArticleSettings.ActiveSocialRateKey), objRating.UserID, Common.GetArticleLink(objArticle, ArticleModuleBase.PortalSettings.ActiveTab, ArticleSettings, False), objArticle.Title, objRating.Rating.ToString(), objRating.Rating.ToString(), 1, "")
                                End If
                            Catch ex As Exception
                            End Try
                        End If
                    End If
                End If

                If (ArticleSettings.JournalIntegration) Then
                    Dim objJournal As New Journal
                    objJournal.AddRatingToJournal(objArticle, objRating, ArticleModuleBase.PortalId, ArticleModuleBase.TabId, ArticleModuleBase.UserId, Common.GetArticleLink(objArticle, ArticleModuleBase.PortalSettings.ActiveTab, ArticleSettings, False))
                End If

                If (ArticleSettings.EnableSmartThinkerStoryFeed) Then
                    Dim objStoryFeed As New wsStoryFeed.StoryFeedWS
                    objStoryFeed.Url = DotNetNuke.Common.Globals.AddHTTP(Request.ServerVariables("HTTP_HOST") & Me.ResolveUrl("~/DesktopModules/Smart-Thinker%20-%20UserProfile/StoryFeed.asmx"))

                    Dim val As String = ArticleModuleBase.GetSharedResource("StoryFeed-AddRating")

                    Dim delimStr As String = "[]"
                    Dim delimiter As Char() = delimStr.ToCharArray()
                    Dim layoutArray As String() = val.Split(delimiter)

                    Dim valResult As String = ""

                    For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2

                        valResult = valResult & layoutArray(iPtr)

                        If iPtr < layoutArray.Length - 1 Then
                            Select Case layoutArray(iPtr + 1)

                                Case "ARTICLEID"
                                    valResult = valResult & objRating.ArticleID.ToString()

                                Case "AUTHORID"
                                    valResult = valResult & objRating.UserID.ToString()

                                Case "AUTHOR"
                                    valResult = valResult & ArticleModuleBase.UserInfo.DisplayName

                                Case "ARTICLELINK"
                                    valResult = valResult & Common.GetArticleLink(objArticle, ArticleModuleBase.PortalSettings.ActiveTab, ArticleSettings, False)

                                Case "ARTICLETITLE"
                                    valResult = valResult & objArticle.Title

                            End Select
                        End If
                    Next

                    Try
                        objStoryFeed.AddAction(82, objRating.RatingID, valResult, objRating.UserID, "VE6457624576460436531768")
                    Catch
                    End Try
                End If

            Else

                Dim objRatingController As New RatingController
                Dim objRating As New RatingInfo
                Dim cookie As HttpCookie = Request.Cookies("ArticleRating" & _articleID.ToString())
                If Not (cookie Is Nothing) Then
                    objRating = objRatingController.GetByID(Convert.ToInt32(cookie.Value), _articleID, ArticleModuleBase.ModuleId)
                    If Not (objRating Is Nothing) Then
                        If (objRating.ArticleID <> Null.NullInteger) Then
                            objRatingController.Delete(objRating.RatingID, _articleID, ArticleModuleBase.ModuleId)
                        End If
                    End If
                End If

                objRating = New RatingInfo

                objRating.ArticleID = _articleID
                objRating.CreatedDate = DateTime.Now
                objRating.Rating = Convert.ToDouble(lstRating.SelectedValue)
                objRating.UserID = -1

                Dim ratingID As Integer = objRatingController.Add(objRating, ArticleModuleBase.ModuleId)

                cookie = New HttpCookie("ArticleRating" & _articleID.ToString())
                cookie.Value = ratingID.ToString()
                cookie.Expires = DateTime.Now.AddDays(7)
                Context.Response.Cookies.Add(cookie)

            End If

            lblRatingSaved.Visible = True

            HttpContext.Current.Items.Add("IgnoreCaptcha", "True")
            Response.Redirect(Request.RawUrl, True)

        End Sub

#End Region

    End Class

End Namespace
