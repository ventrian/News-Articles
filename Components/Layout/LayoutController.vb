'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Ventrian.NewsArticles.Components.Common

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Security
Imports DotNetNuke.Entities.Profile
Imports DotNetNuke.Common.Lists
Imports DotNetNuke.Entities.Portals
Imports Ventrian.NewsArticles.Components.CustomFields

Imports Ventrian.NewsArticles.Base
Imports DotNetNuke.Web.Client.ClientResourceManagement
Imports DotNetNuke.Web.Client
Imports DotNetNuke.Services.Cache
Imports DotNetNuke.Services.FileSystem

Namespace Ventrian.NewsArticles

    Public Class LayoutController

#Region " Constructors "

        Public Sub New(ByVal portalSettings As PortalSettings, ByVal articleSettings As ArticleSettings, ByVal objModule As ModuleInfo, ByVal objPage As Page)

            _portalSettings = portalSettings
            _articleSettings = articleSettings
            _articleModule = objModule
            _page = objPage

        End Sub

        Public Sub New(ByVal moduleContext As NewsArticleModuleBase)

            _portalSettings = moduleContext.PortalSettings
            _articleSettings = moduleContext.ArticleSettings
            _articleModule = moduleContext.ModuleContext.Configuration
            _page = moduleContext.Page

        End Sub

        Public Sub New(ByVal moduleContext As NewsArticleModuleBase, ByVal pageId As Integer)

            _portalSettings = moduleContext.PortalSettings
            _articleSettings = moduleContext.ArticleSettings
            _articleModule = moduleContext.ModuleContext.Configuration
            _page = moduleContext.Page
            _pageId = pageId

        End Sub

#End Region

#Region " Private Members "

        Private ReadOnly _portalSettings As PortalSettings
        Private ReadOnly _articleSettings As ArticleSettings
        Private ReadOnly _articleModule As ModuleInfo
        Private ReadOnly _page As Page

        Private _pageId As Integer = Null.NullInteger
        Private _tab As DotNetNuke.Entities.Tabs.TabInfo

        Private _objPages As ArrayList
        Private _objRelatedArticles As List(Of ArticleInfo)

        Dim _author As UserInfo

        Dim _includeCategory As Boolean = False

        Dim _profileProperties As ProfilePropertyDefinitionCollection

#End Region

#Region " Private Properties "

        Private ReadOnly Property PortalSettings() As PortalSettings
            Get
                Return _portalSettings
            End Get
        End Property

        Private ReadOnly Property ArticleSettings() As ArticleSettings
            Get
                Return _articleSettings
            End Get
        End Property

        Private ReadOnly Property ArticleModule() As ModuleInfo
            Get
                Return _articleModule
            End Get
        End Property

        Private ReadOnly Property Page() As Page
            Get
                Return _page
            End Get
        End Property

        Private ReadOnly Property Pages(ByVal articleId As Integer) As ArrayList
            Get
                If (_objPages Is Nothing) Then
                    Dim objPageController As New PageController
                    _objPages = objPageController.GetPageList(articleId)
                End If
                Return _objPages
            End Get
        End Property

        Private ReadOnly Property Server() As HttpServerUtility
            Get
                Return _page.Server
            End Get
        End Property

        Private ReadOnly Property Request() As HttpRequest
            Get
                Return _page.Request
            End Get
        End Property

        Private ReadOnly Property UserId() As Integer
            Get
                Return UserController.Instance.GetCurrentUserInfo().UserID
            End Get
        End Property

        Private ReadOnly Property Tab() As DotNetNuke.Entities.Tabs.TabInfo
            Get
                If (_tab Is Nothing) Then
                    Dim objTabController As New DotNetNuke.Entities.Tabs.TabController()
                    _tab = objTabController.GetTab(ArticleModule.TabID, PortalSettings.PortalId, False)
                End If

                Return _tab
            End Get
        End Property

        Private ReadOnly Property IsEditable() As Boolean
            Get
                If (Permissions.ModulePermissionController.CanEditModuleContent(ArticleModule)) Then
                    Return True
                End If
                Return False
            End Get
        End Property

#End Region

#Region " Public Properties "


        Public Property IncludeCategory() As Boolean
            Get
                Return _includeCategory
            End Get
            Set(ByVal value As Boolean)
                _includeCategory = value
            End Set
        End Property

#End Region

#Region " Private Methods "
        
        Private Function Author(ByVal authorID As Integer) As UserInfo

            If (authorID = Null.NullInteger) Then
                Return Nothing
            End If

            If (_author Is Nothing) Then
                _author = UserController.GetUserById(PortalSettings.PortalId, authorID)
            Else
                If (_author.UserID = authorID) Then
                    Return _author
                Else
                    _author = UserController.GetUserById(PortalSettings.PortalId, authorID)
                End If
            End If

            Return _author

        End Function

        Public Function BBCode(ByVal strTextToReplace As String) As String

            '//Define regex
            Dim regExp As Regex

            '//Regex for URL tag without anchor
            regExp = New Regex("\[url\]([^\]]+)\[\/url\]", RegexOptions.IgnoreCase)
            Dim m As Match = regExp.Match(strTextToReplace)
            Do While m.Success
                strTextToReplace = strTextToReplace.Replace(m.Value, "<a href=""" & AddHTTP(m.Groups(1).Value) & """ target=""_blank"">" & m.Groups(1).Value & "</a>")
                m = m.NextMatch()
            Loop

            '//Regex for URL with anchor
            regExp = New Regex("\[url=([^\]]+)\]([^\]]+)\[\/url\]", RegexOptions.IgnoreCase)
            m = regExp.Match(strTextToReplace)
            Do While m.Success
                strTextToReplace = strTextToReplace.Replace(m.Value, "<a href=""" & AddHTTP(m.Groups(1).Value) & """ target=""_blank"">" & m.Groups(2).Value & "</a>")
                m = m.NextMatch()
            Loop

            '//Quote text
            regExp = New Regex("\[quote\](.+?)\[\/quote\]", RegexOptions.IgnoreCase)
            strTextToReplace = regExp.Replace(strTextToReplace, "<cite title=""Quote"">$1</cite>")

            '//Bold text
            regExp = New Regex("\[b\](.+?)\[\/b\]", RegexOptions.IgnoreCase)
            strTextToReplace = regExp.Replace(strTextToReplace, "<b>$1</b>")

            '//Italic text
            regExp = New Regex("\[i\](.+?)\[\/i\]", RegexOptions.IgnoreCase)
            strTextToReplace = regExp.Replace(strTextToReplace, "<i>$1</i>")

            '//Underline text
            regExp = New Regex("\[u\](.+?)\[\/u\]", RegexOptions.IgnoreCase)
            strTextToReplace = regExp.Replace(strTextToReplace, "<u>$1</u>")

            Return strTextToReplace

        End Function

        ' utility function to convert a byte array into a hex string
        Private Function ByteArrayToString(ByVal arrInput() As Byte) As String

            Dim strOutput As New System.Text.StringBuilder(arrInput.Length)

            For i As Integer = 0 To arrInput.Length - 1
                strOutput.Append(arrInput(i).ToString("X2"))
            Next

            Return strOutput.ToString().ToLower

        End Function

        Protected Function EncodeComment(ByVal objComment As CommentInfo) As String

            If (objComment.Type = 0) Then
                Dim body As String = objComment.Comment
                Return BBCode(body)
            Else
                Return objComment.Comment
            End If

        End Function

        ' Returns string with binary notation of b bytes,
        ' rounded to 2 decimal places , eg
        ' 123="123 Bytes", 2345="2.29 KB",
        ' 1234567="1.18 MB", etc
        ' b : double : numeric to convert
        Function Numeric2Bytes(ByVal b As Double) As String
            Dim bSize(8) As String
            Dim i As Integer

            bSize(0) = "Bytes"
            bSize(1) = "KB" 'Kilobytes
            bSize(2) = "MB" 'Megabytes
            bSize(3) = "GB" 'Gigabytes
            bSize(4) = "TB" 'Terabytes
            bSize(5) = "PB" 'Petabytes
            bSize(6) = "EB" 'Exabytes
            bSize(7) = "ZB" 'Zettabytes
            bSize(8) = "YB" 'Yottabytes

            b = CDbl(b) ' Make sure var is a Double (not just
            ' variant)
            For i = UBound(bSize) To 0 Step -1
                If b >= (1024 ^ i) Then
                    Numeric2Bytes = ThreeNonZeroDigits(b / (1024 ^ _
                        i)) & " " & bSize(i)
                    Return Numeric2Bytes
                End If
            Next

            Return ""
        End Function

        ' Return the value formatted to include at most three
        ' non-zero digits and at most two digits after the
        ' decimal point. Examples:
        '         1
        '       123
        '        12.3
        '         1.23
        '         0.12
        Private Function ThreeNonZeroDigits(ByVal value As Double) _
            As String
            If value >= 100 Then
                ' No digits after the decimal.
                Return Format$(CInt(value))
            ElseIf value >= 10 Then
                ' One digit after the decimal.
                Return Format$(value, "0.0")
            Else
                ' Two digits after the decimal.
                Return Format$(value, "0.00")
            End If
        End Function

        Private Function FormatImageUrl(ByVal imageUrl As String) As String

            If (imageUrl.ToLower().StartsWith("http://") Or imageUrl.ToLower().StartsWith("https://")) Then
                Return imageUrl
            Else
                If (imageUrl.ToLower().StartsWith("fileid=")) Then
                    Dim objFile As DotNetNuke.Services.FileSystem.FileInfo = FileManager.Instance.GetFile(Convert.ToInt32(UrlUtils.GetParameterValue(imageUrl)))
                    If Not (objFile Is Nothing) Then
                        If (objFile.StorageLocation = 1) Then
                            ' Secure Url
                            Dim url As String = LinkClick(imageUrl, ArticleModule.TabID, ArticleModule.ModuleID)

                            If (HttpContext.Current.Request.Url.Port = 80) Then
                                Return AddHTTP(HttpContext.Current.Request.Url.Host & url)
                            Else
                                Return AddHTTP(HttpContext.Current.Request.Url.Host & ":" & System.Web.HttpContext.Current.Request.Url.Port.ToString() & url)
                            End If
                        Else
                            If (HttpContext.Current.Request.Url.Port = 80) Then
                                Return AddHTTP(HttpContext.Current.Request.Url.Host & PortalSettings.HomeDirectory & objFile.Folder & objFile.FileName)
                            Else
                                Return AddHTTP(HttpContext.Current.Request.Url.Host & ":" & HttpContext.Current.Request.Url.Port.ToString() & PortalSettings.HomeDirectory & objFile.Folder & objFile.FileName)
                            End If
                        End If
                    End If
                End If
            End If

            Return ""

        End Function

        Public Function GetArticleResource(ByVal key As String) As String

            Dim path As String = "~/DesktopModules/DnnForge - NewsArticles/" & Localization.LocalResourceDirectory & "/ViewArticle.ascx.resx"
            Return Localization.GetString(key, path)

        End Function

        Private Function GetFieldValue(ByVal objCustomField As CustomFieldInfo, ByVal objArticle As ArticleInfo, ByVal showCaption As Boolean) As String

            Dim value As String = objArticle.CustomList(objCustomField.CustomFieldID).ToString()
            If (objCustomField.FieldType = CustomFieldType.RichTextBox) Then
                value = Server.HtmlDecode(objArticle.CustomList(objCustomField.CustomFieldID).ToString())

            Else
                If (objCustomField.FieldType = CustomFieldType.MultiCheckBox) Then
                    value = objArticle.CustomList(objCustomField.CustomFieldID).ToString().Replace("|", ", ")
                End If
                If (objCustomField.FieldType = CustomFieldType.MultiLineTextBox) Then
                    value = objArticle.CustomList(objCustomField.CustomFieldID).ToString().Replace(vbCrLf, "<br />")
                End If
                If (value <> "" And objCustomField.ValidationType = CustomFieldValidationType.Date) Then
                    Try
                        value = DateTime.Parse(value).ToShortDateString()
                    Catch
                        value = objArticle.CustomList(objCustomField.CustomFieldID).ToString()
                    End Try
                End If

                If (value <> "" And objCustomField.ValidationType = CustomFieldValidationType.Currency) Then
                    Try
                        Dim culture As String = "en-US"

                        Dim portalFormat As System.Globalization.CultureInfo = New System.Globalization.CultureInfo(culture)
                        Dim format As String = "{0:C2}"
                        value = String.Format(portalFormat.NumberFormat, format, Double.Parse(value))

                    Catch
                        value = objArticle.CustomList(objCustomField.CustomFieldID).ToString()
                    End Try
                End If
            End If

            If (showCaption) Then
                value = "<b>" & objCustomField.Caption & "</b>:&nbsp;" & value
            End If

            Return value

        End Function

        Private Function GetRelatedArticles(ByVal objArticle As ArticleInfo, ByVal count As Integer) As List(Of ArticleInfo)

            If (_objRelatedArticles IsNot Nothing) Then
                Return _objRelatedArticles
            End If

            Dim matchAllCategories As Boolean = False
            If (ArticleSettings.RelatedMode = RelatedType.MatchCategoriesAll Or ArticleSettings.RelatedMode = RelatedType.MatchCategoriesAllTagsAny) Then
                matchAllCategories = True
            End If

            Dim matchAllTags As Boolean = False
            If (ArticleSettings.RelatedMode = RelatedType.MatchCategoriesAnyTagsAll Or ArticleSettings.RelatedMode = RelatedType.MatchTagsAll) Then
                matchAllTags = True
            End If

            Dim objArticleController As New ArticleController()

            Dim categoriesArray() As Integer = Nothing
            If (ArticleSettings.RelatedMode = RelatedType.MatchCategoriesAll Or ArticleSettings.RelatedMode = RelatedType.MatchCategoriesAllTagsAny Or ArticleSettings.RelatedMode = RelatedType.MatchCategoriesAny Or ArticleSettings.RelatedMode = RelatedType.MatchCategoriesAnyTagsAll Or ArticleSettings.RelatedMode = RelatedType.MatchCategoriesAnyTagsAny) Then
                Dim categories As ArrayList = objArticleController.GetArticleCategories(objArticle.ArticleID)
                Dim categoriesRelated As New List(Of Integer)
                For Each objCategory As CategoryInfo In categories
                    categoriesRelated.Add(objCategory.CategoryID)
                Next
                If (categories.Count = 0) Then
                    categoriesRelated.Add(-1)
                End If
                categoriesArray = categoriesRelated.ToArray()
            End If

            Dim tagsArray() As Integer = Nothing
            If (ArticleSettings.RelatedMode = RelatedType.MatchCategoriesAllTagsAny Or ArticleSettings.RelatedMode = RelatedType.MatchCategoriesAnyTagsAll Or ArticleSettings.RelatedMode = RelatedType.MatchCategoriesAnyTagsAny Or ArticleSettings.RelatedMode = RelatedType.MatchTagsAll Or ArticleSettings.RelatedMode = RelatedType.MatchTagsAny) Then
                Dim tagsRelated As New List(Of Integer)
                If (objArticle.Tags <> "") Then
                    Dim objTagController As New TagController()
                    For Each tag As String In objArticle.Tags.Split(","c)
                        Dim objTag As TagInfo = objTagController.Get(objArticle.ModuleID, tag.ToLower().Trim())
                        If (objTag IsNot Nothing) Then
                            tagsRelated.Add(objTag.TagID)
                        End If
                    Next
                End If
                If (tagsRelated.Count = 0) Then
                    tagsRelated.Add(-1)
                End If
                tagsArray = tagsRelated.ToArray()
            End If

            _objRelatedArticles = objArticleController.GetArticleList(objArticle.ModuleID, DateTime.Now, Null.NullDate, categoriesArray, matchAllCategories, Nothing, (count + 1), 1, (count + 1), ArticleSettings.SortBy, ArticleSettings.SortDirection, True, False, Null.NullString, Null.NullInteger, ArticleSettings.ShowPending, True, False, False, False, False, Null.NullString, tagsArray, matchAllTags, Null.NullString, Null.NullInteger, Null.NullString, Null.NullString, Nothing)

            Dim positionToRemove As Integer = Null.NullInteger

            Dim i As Integer = 0
            For Each objRelatedArticle As ArticleInfo In _objRelatedArticles
                If (objArticle.ArticleID = objRelatedArticle.ArticleID) Then
                    positionToRemove = i
                End If
                i = i + 1
            Next

            If (positionToRemove <> Null.NullInteger) Then
                _objRelatedArticles.RemoveAt(positionToRemove)
            End If

            If (_objRelatedArticles.Count = (count + 1)) Then
                _objRelatedArticles.RemoveAt(count)
            End If

            Return _objRelatedArticles

        End Function

        Private Function GetRatingImage(ByVal objArticle As ArticleInfo) As String

            If (objArticle.Rating = Null.NullDouble) Then
                Return ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Images/Rating/stars-0-0.gif")
            Else

                Select Case RoundToUnit(objArticle.Rating, 0.5, False)

                    Case 1
                        Return ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Images/Rating/stars-1-0.gif")

                    Case 1.5
                        Return ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Images/Rating/stars-1-5.gif")

                    Case 2
                        Return ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Images/Rating/stars-2-0.gif")

                    Case 2.5
                        Return ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Images/Rating/stars-2-5.gif")

                    Case 3
                        Return ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Images/Rating/stars-3-0.gif")

                    Case 3.5
                        Return ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Images/Rating/stars-3-5.gif")

                    Case 4
                        Return ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Images/Rating/stars-4-0.gif")

                    Case 4.5
                        Return ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Images/Rating/stars-4-5.gif")

                    Case 5
                        Return ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Images/Rating/stars-5-0.gif")

                End Select

                Return ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Images/Rating/stars-0-0.gif")

            End If

        End Function

        Public Function GetSharedResource(ByVal key As String) As String

            Dim path As String = "~/DesktopModules/DnnForge - NewsArticles/" & Localization.LocalResourceDirectory & "/" & Localization.LocalSharedResourceFile
            Return Localization.GetString(key, path)

        End Function

        ' calculate the MD5 hash of a given string 
        ' the string is first converted to a byte array
        Public Function MD5CalcString(ByVal strData As String) As String

            Dim objMD5 As New System.Security.Cryptography.MD5CryptoServiceProvider
            Dim arrData() As Byte
            Dim arrHash() As Byte

            ' first convert the string to bytes (using UTF8 encoding for unicode characters)
            arrData = Text.Encoding.UTF8.GetBytes(strData)

            ' hash contents of this byte array
            arrHash = objMD5.ComputeHash(arrData)

            ' thanks objects
            objMD5 = Nothing

            ' return formatted hash
            Return ByteArrayToString(arrHash)

        End Function

        Private Function ProcessPostTokens(ByVal content As String, ByVal objArticle As ArticleInfo, ByRef generator As System.Random, ByVal objArticleSettings As ArticleSettings) As String

            If (objArticleSettings.ProcessPostTokens = False) Then
                Return content
            End If

            Dim delimStr As String = "[]"
            Dim delimiter As Char() = delimStr.ToCharArray()

            Dim layoutArray As String() = content.Split(delimiter)
            Dim formattedContent As String = ""

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2

                formattedContent += layoutArray(iPtr).ToString()

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)
                        Case Else
                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("ARTICLELINK:")) Then
                                If (IsNumeric(layoutArray(iPtr + 1).Substring(12, layoutArray(iPtr + 1).Length - 12))) Then
                                    Dim articleID As Integer = Convert.ToInt32(layoutArray(iPtr + 1).Substring(12, layoutArray(iPtr + 1).Length - 12))

                                    Dim objArticleController As New ArticleController
                                    Dim objArticleTarget As ArticleInfo = objArticleController.GetArticle(articleID)

                                    If (objArticleTarget IsNot Nothing) Then
                                        Dim link As String = Common.GetArticleLink(objArticleTarget, Tab, ArticleSettings, False)
                                        formattedContent += "<a href=""" & link & """ rel=""nofollow"">" & objArticleTarget.Title & "</a>"
                                    End If
                                End If
                                Exit Select
                            End If


                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("IMAGETHUMBRANDOM:")) Then

                                If (objArticle.ImageCount > 0) Then

                                    Dim objImageController As New ImageController
                                    Dim objImages As List(Of ImageInfo) = objImageController.GetImageList(objArticle.ArticleID, Null.NullString)
                                    If (objImages.Count > 0) Then

                                        Dim randomImage As ImageInfo = objImages(generator.Next(0, objImages.Count - 1))

                                        Dim val As String = layoutArray(iPtr + 1).Substring(17, layoutArray(iPtr + 1).Length - 17)
                                        If (val.IndexOf(":"c) = -1) Then
                                            Dim length As Integer = Convert.ToInt32(val)

                                            Dim objImage As New Image
                                            objImage.ImageUrl = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & length.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(randomImage.Folder & randomImage.FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1")
                                            objImage.EnableViewState = False
                                            objImage.AlternateText = objArticle.Title

                                            formattedContent += RenderControlAsString(objImage)
                                        Else

                                            Dim arr() As String = val.Split(":"c)

                                            If (arr.Length = 2) Then
                                                Dim width As Integer = Convert.ToInt32(val.Split(":"c)(0))
                                                Dim height As Integer = Convert.ToInt32(val.Split(":"c)(1))

                                                Dim objImage As New Image
                                                objImage.ImageUrl = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(randomImage.Folder & randomImage.FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1")
                                                objImage.EnableViewState = False
                                                objImage.AlternateText = objArticle.Title

                                                formattedContent += RenderControlAsString(objImage)
                                            End If
                                        End If

                                    End If

                                End If
                                Exit Select
                            End If

                            formattedContent += "[" & layoutArray(iPtr + 1) & "]"
                    End Select
                End If

            Next

            Return formattedContent

        End Function

        Private ReadOnly Property ProfileProperties() As ProfilePropertyDefinitionCollection
            Get
                If (_profileProperties Is Nothing) Then
                    _profileProperties = ProfileController.GetPropertyDefinitionsByPortal(PortalSettings.PortalId)
                End If
                Return _profileProperties
            End Get
        End Property

        Private Function RenderControlAsString(ByVal objControl As Control) As String

            Dim sb As New StringBuilder
            Dim tw As New StringWriter(sb)
            Dim hw As New HtmlTextWriter(tw)

            objControl.RenderControl(hw)

            Return sb.ToString()

        End Function

        Private Function RoundToUnit(ByVal d As Double, ByVal unit As Double, ByVal roundDown As Boolean) As Double

            If (roundDown) Then
                Return Math.Round(Math.Round((d / unit) - 0.5, 0) * unit, 2)
            Else
                Return Math.Round(Math.Round((d / unit) + 0.5, 0) * unit, 2)
            End If

        End Function

        Private Function StripHtml(ByVal html As String) As String

            Dim pattern As String = "<(.|\n)*?>"
            Return Regex.Replace(html, pattern, String.Empty)

        End Function

#End Region


        Public Shared Function GetLayout(ByVal moduleContext As NewsArticleModuleBase, ByVal type As LayoutType) As LayoutInfo

            Return GetLayout(moduleContext.ArticleSettings, moduleContext.ModuleConfiguration, moduleContext.Page, type)

        End Function

        Public Shared Function GetLayout(ByVal articleSettings As ArticleSettings, ByVal articleModule As ModuleInfo, ByVal page As Page, ByVal type As LayoutType) As LayoutInfo

            Dim cacheKey As String = "NewsArticles-" & articleModule.TabModuleID.ToString() & type.ToString()
            Dim objLayout As LayoutInfo = CType(DataCache.GetCache(cacheKey), LayoutInfo)

            If (objLayout Is Nothing) Then

                Const delimStr As String = "[]"
                Dim delimiter As Char() = delimStr.ToCharArray()

                objLayout = New LayoutInfo
                Dim path As String = page.MapPath("~\DesktopModules\DnnForge - NewsArticles\Templates\" & articleSettings.Template & "\" & type.ToString().Replace("_", "."))

                If (File.Exists(path) = False) Then
                    ' Need to find a default... 
                    path = page.MapPath("~\DesktopModules\DnnForge - NewsArticles\Templates\" & ArticleConstants.DEFAULT_TEMPLATE & "\" & type.ToString().Replace("_", "."))
                End If

                objLayout.Template = File.ReadAllText(path)
                objLayout.Tokens = objLayout.Template.Split(delimiter)

                DataCache.SetCache(cacheKey, objLayout, New DNNCacheDependency(path))

            End If

            Return objLayout

        End Function

        Public Shared Sub ClearCache(ByVal moduleContext As NewsArticleModuleBase)

            For Each type As String In System.Enum.GetNames(GetType(LayoutType))
                Dim cacheKey As String = "NewsArticles-" & moduleContext.TabModuleId.ToString() & type.ToString()
                DataCache.RemoveCache(cacheKey)
            Next

        End Sub

#Region " Public Methods "

        Public Function GetLayoutObject(ByVal templateData As String) As LayoutInfo

            Dim delimStr As String = "[]"
            Dim delimiter As Char() = delimStr.ToCharArray()
            Dim objLayout As New LayoutInfo

            objLayout.Template = templateData
            objLayout.Tokens = objLayout.Template.Split(delimiter)

            Return objLayout

        End Function

        Public Function GetStylesheet(ByVal template As String) As String

            Dim value As String = ""

            Dim path As String = ArticleUtilities.MapPath("~\DnnForge - NewsArticles\Templates\" & template & "\Template.css")

            If (File.Exists(path) = False) Then
                ' Need to find a default... 
            End If

            File.ReadAllText(path)

            Return value

        End Function

        Public Sub UpdateStylesheet(ByVal template As String, ByVal text As String)

            Dim path As String = ArticleUtilities.MapPath("~\DnnForge - NewsArticles\Templates\" & template & "\Template.css")
            File.WriteAllText(path, text)

        End Sub

        Public Sub UpdateLayout(ByVal template As String, ByVal type As LayoutType, ByVal text As String)

            Dim path As String = ArticleUtilities.MapPath("~\DnnForge - NewsArticles\Templates\" & template & "\" & type.ToString().Replace("_", "."))
            File.WriteAllText(path, text)

        End Sub

        Public Sub LoadStyleSheet(ByVal template As String)

            ClientResourceManager.RegisterStyleSheet(Page, ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Templates/" & template & "/Template.css"), FileOrder.Css.ModuleCss)

        End Sub

        Public Function ProcessImages(ByVal html As String) As String

            If (html.ToLower().Contains("src=""images/") Or html.ToLower().Contains("src=""~/images/")) Then
                html = html.Replace("src=""images/", "src=""" & ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Templates/" & ArticleSettings.Template & "/Images/"))
                html = html.Replace("src=""Images/", "src=""" & ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Templates/" & ArticleSettings.Template & "/Images/"))
                html = html.Replace("src=""~/images/", "src=""" & ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Images") & "/")
                html = html.Replace("src=""~/Images/", "src=""" & ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Images") & "/")
            End If

            Return html

        End Function

#Region " Process Article Item "


        Public Sub ProcessHeaderFooter(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String(), ByVal objArticle As ArticleInfo)

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Add(New LiteralControl(ProcessImages(layoutArray(iPtr).ToString())))
            Next

        End Sub

        Private articleItemIndex As Integer = 0
        Public Sub ProcessArticleItem(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String(), ByVal objArticle As ArticleInfo)

            articleItemIndex = articleItemIndex + 1
            _objRelatedArticles = Nothing

            Static Generator As System.Random = New System.Random()

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(ProcessImages(layoutArray(iPtr).ToString())))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)

                        Case "APPROVERDISPLAYNAME"
                            If (objArticle.Approver(PortalSettings.PortalId) IsNot Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.Text = objArticle.Approver(PortalSettings.PortalId).DisplayName
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "APPROVERFIRSTNAME"
                            If (objArticle.Approver(PortalSettings.PortalId) IsNot Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.Text = objArticle.Approver(PortalSettings.PortalId).FirstName
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "APPROVERLASTNAME"
                            If (objArticle.Approver(PortalSettings.PortalId) IsNot Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.Text = objArticle.Approver(PortalSettings.PortalId).LastName
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "APPROVERUSERNAME"
                            If (objArticle.Approver(PortalSettings.PortalId) IsNot Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.Text = objArticle.Approver(PortalSettings.PortalId).Username
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "ARTICLEID"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.ArticleID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "ARTICLELINK"
                            Dim objLiteral As New Literal
                            Dim pageID As Integer = Null.NullInteger
                            If (ArticleSettings.AlwaysShowPageID) Then
                                If (Pages(objArticle.ArticleID).Count > 0) Then
                                    pageID = CType(Pages(objArticle.ArticleID)(0), PageInfo).PageID
                                End If
                            End If
                            objLiteral.Text = Common.GetArticleLink(objArticle, Tab, ArticleSettings, IncludeCategory, pageID)
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "AUTHOR"
                            Dim objLiteral As New Literal
                            Select Case ArticleSettings.DisplayMode
                                Case DisplayType.FirstName
                                    objLiteral.Text = objArticle.AuthorFirstName
                                    Exit Select
                                Case DisplayType.LastName
                                    objLiteral.Text = objArticle.AuthorLastName
                                    Exit Select
                                Case DisplayType.UserName
                                    objLiteral.Text = objArticle.AuthorUserName
                                    Exit Select
                                Case DisplayType.FullName
                                    objLiteral.Text = objArticle.AuthorDisplayName
                                    Exit Select
                                Case Else
                                    objLiteral.Text = objArticle.AuthorUserName
                                    Exit Select
                            End Select
                            objPlaceHolder.Add(objLiteral)

                        Case "AUTHORDISPLAYNAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.AuthorDisplayName
                            objPlaceHolder.Add(objLiteral)

                        Case "AUTHOREMAIL"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.AuthorEmail.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "AUTHORFIRSTNAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.AuthorFirstName
                            objPlaceHolder.Add(objLiteral)

                        Case "AUTHORFULLNAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.AuthorFullName
                            objPlaceHolder.Add(objLiteral)

                        Case "AUTHORID"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.AuthorID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "AUTHORLASTNAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.AuthorLastName
                            objPlaceHolder.Add(objLiteral)

                        Case "AUTHORLINK"
                            Dim objLiteral As New Literal
                            objLiteral.Text = Common.GetAuthorLink(ArticleModule.TabID, ArticleModule.ModuleID, objArticle.AuthorID, objArticle.AuthorUserName, ArticleSettings.LaunchLinks, ArticleSettings)
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "AUTHORUSERNAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.AuthorUserName.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "CATEGORIES"
                            Dim objLiteral As New Literal

                            Dim objArticleCategories As ArrayList = CType(DataCache.GetCache(ArticleConstants.CACHE_CATEGORY_ARTICLE & objArticle.ArticleID.ToString()), ArrayList)
                            If (objArticleCategories Is Nothing) Then
                                Dim objArticleController As New ArticleController
                                objArticleCategories = objArticleController.GetArticleCategories(objArticle.ArticleID)
                                For Each objCategory As CategoryInfo In objArticleCategories
                                    If (objCategory.InheritSecurity) Then
                                        If (objLiteral.Text <> "") Then
                                            objLiteral.Text = objLiteral.Text & ", <a href=""" & Common.GetCategoryLink(ArticleModule.TabID, ArticleModule.ModuleID, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings) & """ > " & objCategory.Name & "</a>"
                                        Else
                                            objLiteral.Text = "<a href=""" & Common.GetCategoryLink(ArticleModule.TabID, ArticleModule.ModuleID, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings) & """ > " & objCategory.Name & "</a>"
                                        End If
                                    Else
                                        If (ArticleSettings.Settings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                            If (PortalSecurity.IsInRoles(ArticleSettings.Settings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString())) Then
                                                If (objLiteral.Text <> "") Then
                                                    objLiteral.Text = objLiteral.Text & ", <a href=""" & Common.GetCategoryLink(ArticleModule.TabID, ArticleModule.ModuleID, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings) & """ > " & objCategory.Name & "</a>"
                                                Else
                                                    objLiteral.Text = "<a href=""" & Common.GetCategoryLink(ArticleModule.TabID, ArticleModule.ModuleID, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings) & """ > " & objCategory.Name & "</a>"
                                                End If
                                            End If
                                        End If
                                    End If
                                Next
                                DataCache.SetCache(ArticleConstants.CACHE_CATEGORY_ARTICLE & objArticle.ArticleID.ToString(), objArticleCategories)
                            Else
                                For Each objCategory As CategoryInfo In objArticleCategories
                                    If (objCategory.InheritSecurity) Then
                                        If (objLiteral.Text <> "") Then
                                            objLiteral.Text = objLiteral.Text & ", <a href=""" & Common.GetCategoryLink(ArticleModule.TabID, ArticleModule.ModuleID, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings) & """ > " & objCategory.Name & "</a>"
                                        Else
                                            objLiteral.Text = "<a href=""" & Common.GetCategoryLink(ArticleModule.TabID, ArticleModule.ModuleID, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings) & """ > " & objCategory.Name & "</a>"
                                        End If
                                    Else

                                        If (ArticleSettings.Settings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                            If (PortalSecurity.IsInRoles(ArticleSettings.Settings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString())) Then
                                                If (objLiteral.Text <> "") Then
                                                    objLiteral.Text = objLiteral.Text & ", <a href=""" & Common.GetCategoryLink(ArticleModule.TabID, ArticleModule.ModuleID, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings) & """ > " & objCategory.Name & "</a>"
                                                Else
                                                    objLiteral.Text = "<a href=""" & Common.GetCategoryLink(ArticleModule.TabID, ArticleModule.ModuleID, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings) & """ > " & objCategory.Name & "</a>"
                                                End If
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                            objPlaceHolder.Add(objLiteral)

                        Case "CATEGORIESNOLINK"
                            Dim objLiteral As New Literal
                            Dim objArticleCategories As ArrayList = CType(DataCache.GetCache(ArticleConstants.CACHE_CATEGORY_ARTICLE & objArticle.ArticleID.ToString()), ArrayList)
                            If (objArticleCategories Is Nothing) Then
                                Dim objArticleController As New ArticleController
                                objArticleCategories = (objArticleController.GetArticleCategories(objArticle.ArticleID))
                                For Each objCategory As CategoryInfo In objArticleCategories
                                    If (objCategory.InheritSecurity) Then
                                        If (objLiteral.Text <> "") Then
                                            objLiteral.Text = objLiteral.Text & ", " & objCategory.Name
                                        Else
                                            objLiteral.Text = objCategory.Name
                                        End If
                                    Else
                                        If (ArticleSettings.Settings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                            If (PortalSecurity.IsInRoles(ArticleSettings.Settings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString())) Then

                                                If (objLiteral.Text <> "") Then
                                                    objLiteral.Text = objLiteral.Text & ", " & objCategory.Name
                                                Else
                                                    objLiteral.Text = objCategory.Name
                                                End If
                                            End If
                                        End If
                                    End If
                                Next
                                DataCache.SetCache(ArticleConstants.CACHE_CATEGORY_ARTICLE_NO_LINK & objArticle.ArticleID.ToString(), objArticleCategories)
                            Else
                                For Each objCategory As CategoryInfo In objArticleCategories
                                    If (objCategory.InheritSecurity) Then
                                        If (objLiteral.Text <> "") Then
                                            objLiteral.Text = objLiteral.Text & ", " & objCategory.Name
                                        Else
                                            objLiteral.Text = objCategory.Name
                                        End If
                                    Else
                                        If (ArticleSettings.Settings.Contains(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING)) Then
                                            If (PortalSecurity.IsInRoles(ArticleSettings.Settings(objCategory.CategoryID & "-" & ArticleConstants.PERMISSION_CATEGORY_VIEW_SETTING).ToString())) Then
                                                If (objLiteral.Text <> "") Then
                                                    objLiteral.Text = objLiteral.Text & ", " & objCategory.Name
                                                Else
                                                    objLiteral.Text = objCategory.Name
                                                End If
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                            objPlaceHolder.Add(objLiteral)

                        Case "COMMENTCOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.CommentCount.ToString()
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "COMMENTLINK"
                            Dim objLiteral As New Literal
                            objLiteral.Text = Common.GetArticleLink(objArticle, Tab, ArticleSettings, IncludeCategory) & "#Comments"
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "COMMENTRSS"
                            Dim objLiteral As New Literal
                            objLiteral.Text = AddHTTP(Request.Url.Host & ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/RssComments.aspx?TabID=" & ArticleModule.TabID.ToString() & "&amp;ModuleID=" & ArticleModule.ModuleID.ToString() & "&amp;ArticleID=" & objArticle.ArticleID.ToString()).Replace(" ", "%20"))
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "COMMENTS"
                            commentItemIndex = 0

                            Dim phComments As New PlaceHolder
                            Dim objLayoutCommentItem As LayoutInfo = GetLayout(ArticleSettings, ArticleModule, Page, LayoutType.Comment_Item_Html)

                            Dim direction As SortDirection = SortDirection.Ascending
                            If (ArticleSettings.SortDirectionComments = 1) Then
                                direction = SortDirection.Descending
                            End If
                            Dim objCommentController As New CommentController
                            Dim objComments As List(Of CommentInfo) = objCommentController.GetCommentList(objArticle.ModuleID, objArticle.ArticleID, True, direction, Null.NullInteger)

                            For Each objComment As CommentInfo In objComments
                                ProcessComment(phComments.Controls, objArticle, objComment, objLayoutCommentItem.Tokens)
                            Next

                            objPlaceHolder.Add(phComments)

                        Case "CREATEDATE"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.CreatedDate.ToString("D")
                            objPlaceHolder.Add(objLiteral)

                        Case "CREATETIME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.CreatedDate.ToString("t")
                            objPlaceHolder.Add(objLiteral)

                        Case "CURRENTPAGE"
                            Dim objLiteral As New Literal
                            If (objArticle.PageCount <= 1) Then
                                objLiteral.Text = "1"
                            Else
                                Dim pageID As Integer = Null.NullInteger
                                If (Request("PageID") <> "" AndAlso IsNumeric(Request("PageID"))) Then
                                    pageID = Convert.ToInt32(Request("PageID"))
                                End If
                                If (pageID = Null.NullInteger) Then
                                    pageID = CType(Pages(objArticle.ArticleID)(0), PageInfo).PageID
                                End If
                                For i As Integer = 0 To Pages(objArticle.ArticleID).Count - 1
                                    Dim objPage As PageInfo = CType(Pages(objArticle.ArticleID)(i), PageInfo)
                                    If (pageID = objPage.PageID) Then
                                        objLiteral.Text = (i + 1).ToString()
                                        Exit For
                                    End If
                                Next
                                If (objLiteral.Text = Null.NullString) Then
                                    objLiteral.Text = "1"
                                End If
                            End If
                            objPlaceHolder.Add(objLiteral)

                        Case "CUSTOMFIELDS"
                            Dim objCustomFieldController As New CustomFieldController()
                            Dim objCustomFields As ArrayList = objCustomFieldController.List(objArticle.ModuleID)
                            Dim i As Integer = 0
                            For Each objCustomField As CustomFieldInfo In objCustomFields
                                If (objCustomField.IsVisible = True) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.Text = GetFieldValue(objCustomField, objArticle, True) & "<br />"
                                    If (objLiteral.Text <> "") Then
                                        objPlaceHolder.Add(objLiteral)
                                    End If
                                    i = i + 1
                                End If
                            Next

                        Case "DETAILS"
                            Dim objLiteral As New Literal
                            If (objArticle.PageCount > 0) Then
                                Dim pageID As Integer = Null.NullInteger
                                If (IsNumeric(Request("PageID"))) Then
                                    pageID = Convert.ToInt32(Request("PageID"))
                                End If
                                If (pageID = Null.NullInteger) Then
                                    objLiteral.Text = ProcessPostTokens(Server.HtmlDecode(objArticle.Body), objArticle, Generator, ArticleSettings)
                                Else
                                    Dim pageController As New PageController
                                    Dim pageList As ArrayList = pageController.GetPageList(objArticle.ArticleID)
                                    For Each objPage As PageInfo In pageList
                                        If (objPage.PageID = pageID) Then
                                            objLiteral.Text = ProcessPostTokens(Server.HtmlDecode(objPage.PageText), objArticle, Generator, ArticleSettings)
                                            Exit For
                                        End If
                                    Next
                                    If (objLiteral.Text = Null.NullString) Then
                                        objLiteral.Text = ProcessPostTokens(Server.HtmlDecode(objArticle.Body), objArticle, Generator, ArticleSettings)
                                    End If
                                End If
                            End If
                            objPlaceHolder.Add(objLiteral)

                        Case "DETAILSDATA"
                            Dim objLiteral As New Literal
                            If (objArticle.PageCount > 0) Then
                                Dim pageID As Integer = Null.NullInteger
                                If (IsNumeric(Request("PageID"))) Then
                                    pageID = Convert.ToInt32(Request("PageID"))
                                End If
                                If (pageID = Null.NullInteger) Then
                                    objLiteral.Text = "<![CDATA[" & ProcessPostTokens(Server.HtmlDecode(objArticle.Body), objArticle, Generator, ArticleSettings) & "]]>"
                                Else
                                    Dim pageController As New PageController
                                    Dim pageList As ArrayList = pageController.GetPageList(objArticle.ArticleID)
                                    For Each objPage As PageInfo In pageList
                                        If (objPage.PageID = pageID) Then
                                            objLiteral.Text = "<![CDATA[" & ProcessPostTokens(Server.HtmlDecode(objPage.PageText), objArticle, Generator, ArticleSettings) & "]]>"
                                            Exit For
                                        End If
                                    Next
                                    If (objLiteral.Text = Null.NullString) Then
                                        objLiteral.Text = "<![CDATA[" & ProcessPostTokens(Server.HtmlDecode(objArticle.Body), objArticle, Generator, ArticleSettings) & "]]>"
                                    End If
                                End If
                            End If
                            objPlaceHolder.Add(objLiteral)

                        Case "EDIT"
                            If IsEditable OrElse (ArticleSettings.IsApprover) OrElse (objArticle.AuthorID = UserId And ArticleSettings.IsAutoApprover) Then
                                Dim objHyperLink As New HyperLink
                                objHyperLink.NavigateUrl = Common.GetModuleLink(ArticleModule.TabID, ArticleModule.ModuleID, "SubmitNews", ArticleSettings, "ArticleID=" & objArticle.ArticleID.ToString(), "ReturnUrl=" & Server.UrlEncode(Request.RawUrl))
                                objHyperLink.ImageUrl = "~/images/edit.gif"
                                objHyperLink.ToolTip = "Click to edit"
                                objHyperLink.EnableViewState = False
                                objPlaceHolder.Add(objHyperLink)
                            End If

                        Case "FILECOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.FileCount.ToString()
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "FILELINK"
                            If (objArticle.FileCount > 0) Then
                                Dim objFiles As List(Of FileInfo) = FileProvider.Instance().GetFiles(objArticle.ArticleID)

                                If (objFiles.Count > 0) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.Text = CType(objFiles(0), FileInfo).Link
                                    objLiteral.EnableViewState = False
                                    objPlaceHolder.Add(objLiteral)
                                End If

                            End If

                        Case "FILES"
                            ' File Count Check
                            If (objArticle.FileCount > 0) Then
                                ' Dim objFileController As New FileController
                                Dim objFiles As List(Of FileInfo) = FileProvider.Instance().GetFiles(objArticle.ArticleID)

                                If (objFiles.Count > 0) Then
                                    Dim objLayoutFileHeader As LayoutInfo = GetLayout(ArticleSettings, ArticleModule, Page, LayoutType.File_Header_Html)
                                    Dim objLayoutFileItem As LayoutInfo = GetLayout(ArticleSettings, ArticleModule, Page, LayoutType.File_Item_Html)
                                    Dim objLayoutFileFooter As LayoutInfo = GetLayout(ArticleSettings, ArticleModule, Page, LayoutType.File_Footer_Html)

                                    ProcessHeaderFooter(objPlaceHolder, objLayoutFileHeader.Tokens, objArticle)
                                    For Each objFile As FileInfo In objFiles
                                        ProcessFile(objPlaceHolder, objArticle, objFile, objLayoutFileItem.Tokens)
                                    Next
                                    ProcessHeaderFooter(objPlaceHolder, objLayoutFileFooter.Tokens, objArticle)
                                End If
                            End If

                        Case "GRAVATARURL"
                            If (objArticle.AuthorEmail <> "") Then
                                Dim objLiteral As New Literal
                                If Request.IsSecureConnection Then
                                    objLiteral.Text = AddHTTP("secure.gravatar.com/avatar/" & MD5CalcString(objArticle.AuthorEmail.ToLower()))
                                Else
                                    objLiteral.Text = AddHTTP("www.gravatar.com/avatar/" & MD5CalcString(objArticle.AuthorEmail.ToLower()))
                                End If
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "HASAUTHOR"
                            If (objArticle.AuthorID = Null.NullInteger) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASAUTHOR") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASAUTHOR"
                            ' Do Nothing

                        Case "HASNOAUTHOR"
                            If (objArticle.AuthorID <> Null.NullInteger) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNOAUTHOR") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASNOAUTHOR"
                            ' Do Nothing

                        Case "HASCATEGORIES"
                            Dim objLiteral As New Literal
                            If (DataCache.GetCache(ArticleConstants.CACHE_CATEGORY_ARTICLE & objArticle.ArticleID.ToString()) Is Nothing) Then
                                Dim objArticleController As New ArticleController
                                Dim objArticleCategories As ArrayList = objArticleController.GetArticleCategories(objArticle.ArticleID)
                                For Each objCategory As CategoryInfo In objArticleCategories
                                    If (objLiteral.Text <> "") Then
                                        objLiteral.Text = objLiteral.Text & ", <a href=""" & Common.GetCategoryLink(ArticleModule.TabID, ArticleModule.ModuleID, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings) & """ > " & objCategory.Name & "</a>"
                                    Else
                                        objLiteral.Text = "<a href=""" & Common.GetCategoryLink(ArticleModule.TabID, ArticleModule.ModuleID, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings) & """ > " & objCategory.Name & "</a>"
                                    End If
                                Next
                                DataCache.SetCache(ArticleConstants.CACHE_CATEGORY_ARTICLE & objArticle.ArticleID.ToString(), objArticleCategories)
                            Else
                                Dim objArticleCategories As ArrayList = CType(DataCache.GetCache(ArticleConstants.CACHE_CATEGORY_ARTICLE & objArticle.ArticleID.ToString()), ArrayList)
                                For Each objCategory As CategoryInfo In objArticleCategories
                                    If (objLiteral.Text <> "") Then
                                        objLiteral.Text = objLiteral.Text & ", <a href=""" & Common.GetCategoryLink(ArticleModule.TabID, ArticleModule.ModuleID, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings) & """ > " & objCategory.Name & "</a>"
                                    Else
                                        objLiteral.Text = "<a href=""" & Common.GetCategoryLink(ArticleModule.TabID, ArticleModule.ModuleID, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings) & """ > " & objCategory.Name & "</a>"
                                    End If
                                Next
                            End If
                            If (objLiteral.Text = "") Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASCATEGORIES") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASCATEGORIES"
                            ' Do Nothing

                        Case "HASCOMMENTS"
                            If (objArticle.CommentCount = 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASCOMMENTS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASCOMMENTS"
                            ' Do Nothing

                        Case "HASCOMMENTSENABLED"
                            If (ArticleSettings.IsCommentsEnabled = False) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASCOMMENTSENABLED") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASCOMMENTSENABLED"
                            ' Do Nothing

                        Case "HASCUSTOMFIELDS"
                            Dim objCustomFieldController As New CustomFieldController()
                            Dim objCustomFields As ArrayList = objCustomFieldController.List(objArticle.ModuleID)

                            If (objCustomFields.Count = 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASCUSTOMFIELDS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASCUSTOMFIELDS"
                            ' Do Nothing

                        Case "HASDETAILS"
                            Dim objLiteral As New Literal
                            objLiteral.Text = ""
                            If (objArticle.PageCount > 0) Then
                                Dim pageID As Integer = Null.NullInteger
                                If (IsNumeric(Request("PageID"))) Then
                                    pageID = Convert.ToInt32(Request("PageID"))
                                End If
                                If (pageID = Null.NullInteger) Then
                                    objLiteral.Text = ProcessPostTokens(Server.HtmlDecode(objArticle.Body), objArticle, Generator, ArticleSettings)
                                Else
                                    Dim pageController As New PageController
                                    Dim pageList As ArrayList = pageController.GetPageList(objArticle.ArticleID)
                                    For Each objPage As PageInfo In pageList
                                        If (objPage.PageID = pageID) Then
                                            objLiteral.Text = ProcessPostTokens(Server.HtmlDecode(objPage.PageText), objArticle, Generator, ArticleSettings)
                                            Exit For
                                        End If
                                    Next
                                    If (objLiteral.Text = Null.NullString) Then
                                        objLiteral.Text = ProcessPostTokens(Server.HtmlDecode(objArticle.Body), objArticle, Generator, ArticleSettings)
                                    End If
                                End If
                            End If

                            If (objLiteral.Text.Replace("<p>&#160;</p>", "").Trim() = "") Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASDETAILS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASDETAILS"
                            ' Do Nothing

                        Case "HASNODETAILS"
                            Dim objLiteral As New Literal
                            objLiteral.Text = ""
                            If (objArticle.PageCount > 0) Then
                                Dim pageId As Integer = Null.NullInteger
                                If (IsNumeric(Request("PageID"))) Then
                                    pageId = Convert.ToInt32(Request("PageID"))
                                End If
                                If (pageId = Null.NullInteger) Then
                                    objLiteral.Text = ProcessPostTokens(Server.HtmlDecode(objArticle.Body), objArticle, Generator, ArticleSettings)
                                Else
                                    Dim pageController As New PageController
                                    Dim pageList As ArrayList = pageController.GetPageList(objArticle.ArticleID)
                                    For Each objPage As PageInfo In pageList
                                        If (objPage.PageID = pageId) Then
                                            objLiteral.Text = ProcessPostTokens(Server.HtmlDecode(objPage.PageText), objArticle, Generator, ArticleSettings)
                                            Exit For
                                        End If
                                    Next
                                    If (objLiteral.Text = Null.NullString) Then
                                        objLiteral.Text = ProcessPostTokens(Server.HtmlDecode(objArticle.Body), objArticle, Generator, ArticleSettings)
                                    End If
                                End If
                            End If

                            If (objLiteral.Text.Replace("<p>&#160;</p>", "").Trim() <> "") Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNODETAILS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASNODETAILS"
                            ' Do Nothing

                        Case "HASFILES"
                            If (objArticle.FileCount = 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASFILES") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASFILES"
                            ' Do Nothing

                        Case "HASIMAGE"
                            If (objArticle.ImageUrl = "" And objArticle.ImageCount = 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASIMAGE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASIMAGE"
                            ' Do Nothing

                        Case "HASIMAGES"
                            If (objArticle.ImageCount = 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASIMAGES") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASIMAGES"
                            ' Do Nothing

                        Case "HASLINK"
                            If (objArticle.Url = Null.NullString()) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASLINK") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASLINK"
                            ' Do Nothing

                        Case "HASMOREDETAIL"
                            If (objArticle.Url = Null.NullString() And StripHtml(objArticle.Summary.Trim()) = "") Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASMOREDETAIL") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASMOREDETAIL"
                            ' Do Nothing

                        Case "HASMULTIPLEIMAGES"
                            If (objArticle.ImageCount <= 1) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASMULTIPLEIMAGES") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASMULTIPLEIMAGES"
                            ' Do Nothing

                        Case "HASMULTIPLEPAGES"
                            If (objArticle.PageCount <= 1) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASMULTIPLEPAGES") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASMULTIPLEPAGES"
                            ' Do Nothing

                        Case "HASNEXTPAGE"
                            If (Pages(objArticle.ArticleID).Count <= 1) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNEXTPAGE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            Else
                                If (IsNumeric(Request("PageID"))) Then
                                    _pageId = Convert.ToInt32(Request("PageID"))
                                End If

                                If (_pageId = Null.NullInteger) Then
                                    _pageId = Pages(objArticle.ArticleID)(0).PageID
                                End If
                                If (_pageId = Pages(objArticle.ArticleID)(Pages(objArticle.ArticleID).Count - 1).PageID) Then
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = "/HASNEXTPAGE") Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                            End If
                        Case "/HASNEXTPAGE"
                            ' Do Nothing

                        Case "HASNOCOMMENTS"
                            If (objArticle.CommentCount > 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNOCOMMENTS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASNOCOMMENTS"
                            ' Do Nothing

                        Case "HASNOFILES"
                            If (objArticle.FileCount > 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNOFILES") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASNOFILES"
                            ' Do Nothing

                        Case "HASNOIMAGE"
                            If (objArticle.ImageUrl <> "" Or objArticle.ImageCount > 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNOIMAGE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASNOIMAGE"
                            ' Do Nothing

                        Case "HASNOIMAGES"
                            If (objArticle.ImageCount > 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNOIMAGES") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASNOIMAGES"
                            ' Do Nothing

                        Case "HASNOLINK"
                            If (objArticle.Url <> Null.NullString()) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNOLINK") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASNOLINK"
                            ' Do Nothing

                        Case "HASPREVPAGE"
                            If (objArticle.PageCount <= 1) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASPREVPAGE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            Else
                                If (IsNumeric(Request("PageID"))) Then
                                    _pageId = Convert.ToInt32(Request("PageID"))
                                End If

                                If (_pageId = Null.NullInteger) Then
                                    _pageId = Pages(objArticle.ArticleID)(0).PageID
                                End If
                                If (_pageId = Pages(objArticle.ArticleID)(0).PageID) Then
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = "/HASPREVPAGE") Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                            End If
                        Case "/HASPREVPAGE"
                            ' Do Nothing

                        Case "HASRATING"
                            If (ArticleSettings.EnableRatings = False OrElse objArticle.RatingCount = 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASRATING") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASRATING"
                            ' Do Nothing

                        Case "HASRATINGSENABLED"
                            If (ArticleSettings.EnableRatings = False) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASRATINGSENABLED") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASRATINGSENABLED"
                            ' Do Nothing

                        Case "HASRELATED"
                            Dim objRelatedArticles As List(Of ArticleInfo) = GetRelatedArticles(objArticle, 5)
                            If (objRelatedArticles.Count = 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASRELATED") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASRELATED"
                            ' Do Nothing

                        Case "HASSUMMARY"
                            If (StripHtml(objArticle.Summary.Trim()) = "") Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASSUMMARY") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASSUMMARY"
                            ' Do Nothing

                        Case "HASNOSUMMARY"
                            If (StripHtml(objArticle.Summary.Trim()) <> "") Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNOSUMMARY") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASNOSUMMARY"
                            ' Do Nothing

                        Case "HASTAGS"
                            If (objArticle.Tags = "") Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASTAGS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASTAGS"
                            ' Do Nothing

                        Case "HASNOTAGS"
                            If (objArticle.Tags <> "") Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNOTAGS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASNOTAGS"
                            ' Do Nothing

                        Case "IMAGE"
                            If (objArticle.ImageUrl <> "") Then
                                Dim objImage As New Image
                                objImage.ImageUrl = FormatImageUrl(objArticle.ImageUrl)
                                objImage.EnableViewState = False
                                objImage.AlternateText = objArticle.Title
                                objPlaceHolder.Add(objImage)
                            Else
                                If (objArticle.ImageCount > 0) Then
                                    Dim objImageController As New ImageController
                                    Dim objImages As List(Of ImageInfo) = objImageController.GetImageList(objArticle.ArticleID, Null.NullString())

                                    If (objImages.Count > 0) Then
                                        Dim objImage As New Image
                                        objImage.ImageUrl = PortalSettings.HomeDirectory & objImages(0).Folder & objImages(0).FileName
                                        objImage.EnableViewState = False
                                        objImage.AlternateText = objArticle.Title
                                        objPlaceHolder.Add(objImage)
                                    End If

                                End If
                            End If

                        Case "IMAGETITLE"
                            If (objArticle.ImageCount > 0) Then
                                Dim objImageController As New ImageController
                                Dim objImages As List(Of ImageInfo) = objImageController.GetImageList(objArticle.ArticleID, Null.NullString())

                                If (objImages.Count > 0) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.Text = objImages(0).Title
                                    objPlaceHolder.Add(objLiteral)
                                End If
                            End If

                        Case "IMAGECOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.ImageCount.ToString()
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "IMAGELINK"
                            If (objArticle.ImageUrl <> "") Then
                                Dim objLiteral As New Literal
                                objLiteral.Text = FormatImageUrl(objArticle.ImageUrl)
                                objPlaceHolder.Add(objLiteral)
                            Else
                                Dim objImageController As New ImageController
                                Dim objImages As List(Of ImageInfo) = objImageController.GetImageList(objArticle.ArticleID, Null.NullString())

                                If (objImages.Count > 0) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.Text = PortalSettings.HomeDirectory & objImages(0).Folder & objImages(0).FileName
                                    objPlaceHolder.Add(objLiteral)
                                End If
                            End If

                        Case "IMAGES"

                            ' Image Count Check
                            If (objArticle.ImageCount > 0) Then
                                imageIndex = 0

                                Dim objImageController As New ImageController
                                Dim objImages As List(Of ImageInfo) = objImageController.GetImageList(objArticle.ArticleID, Null.NullString())

                                If (objImages.Count > 0) Then
                                    Dim objLayoutImageHeader As LayoutInfo = GetLayout(ArticleSettings, ArticleModule, Page, LayoutType.Image_Header_Html)
                                    Dim objLayoutImageItem As LayoutInfo = GetLayout(ArticleSettings, ArticleModule, Page, LayoutType.Image_Item_Html)
                                    Dim objLayoutImageFooter As LayoutInfo = GetLayout(ArticleSettings, ArticleModule, Page, LayoutType.Image_Footer_Html)

                                    ProcessHeaderFooter(objPlaceHolder, objLayoutImageHeader.Tokens, objArticle)
                                    For Each objImage As ImageInfo In objImages
                                        ProcessImage(objPlaceHolder, objArticle, objImage, objLayoutImageItem.Tokens)
                                    Next
                                    ProcessHeaderFooter(objPlaceHolder, objLayoutImageFooter.Tokens, objArticle)
                                End If

                                'Dim script As String = "" _
                                '& "<script type=""text/javascript"">" & vbCrLf _
                                '& "jQuery(function() {" & vbCrLf _
                                '& "jQuery('a[rel*=lightbox" & objArticle.ArticleID.ToString() & "]').lightBox({" & vbCrLf _
                                '& "imageLoading: '" & ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Images/Lightbox/lightbox-ico-loading.gif") & "'," & vbCrLf _
                                '& "imageBlank: '" & ArticleUtilities.ResolveUrl("~/images/spacer.gif") & "'," & vbCrLf _
                                '& "txtImage: '" & GetSharedResource("Image") & "'," & vbCrLf _
                                '& "txtOf: '" & GetSharedResource("Of") & "'," & vbCrLf _
                                '& "next: '" & GetSharedResource("Next") & "'," & vbCrLf _
                                '& "previous: '" & GetSharedResource("Previous") & "'," & vbCrLf _
                                '& "close: '" & GetSharedResource("Close") & "'" & vbCrLf _
                                '& "});" & vbCrLf _
                                '& "});" & vbCrLf _
                                '& "</script>" & vbCrLf

                                'Dim objScript As New Literal
                                'objScript.Text = script
                                'objPlaceHolder.AddAt(0, objScript)
                            End If

                        Case "ISAUTHOR"
                            Dim isAuthor As Boolean = False

                            If (Request.IsAuthenticated) Then
                                Dim objUser As UserInfo = UserController.Instance.GetCurrentUserInfo()
                                If (objUser IsNot Nothing) Then
                                    If (objUser.UserID = objArticle.AuthorID) Then
                                        isAuthor = True
                                    End If
                                End If
                            End If

                            If (isAuthor = False) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISAUTHOR") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISAUTHOR"
                            ' Do Nothing

                        Case "ISANONYMOUS"
                            If (Request.IsAuthenticated) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISANONYMOUS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISANONYMOUS"
                            ' Do Nothing

                        Case "ISDRAFT"
                            If (objArticle.Status <> StatusType.Draft) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISDRAFT") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISDRAFT"
                            ' Do Nothing

                        Case "ISFEATURED"
                            If (objArticle.IsFeatured = False) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISFEATURED") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISFEATURED"
                            ' Do Nothing

                        Case "ISNOTFEATURED"
                            If (objArticle.IsFeatured) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISNOTFEATURED") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISNOTFEATURED"
                            ' Do Nothing

                        Case "ISFIRST"
                            If (articleItemIndex > 1) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISFIRST") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISFIRST"
                            ' Do Nothing

                        Case "ISFIRST2"
                            If (articleItemIndex > 1 Or (Request("currentpage") <> "" And Request("currentpage") <> "1")) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISFIRST2") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISFIRST2"
                            ' Do Nothing

                        Case "ISNOTFIRST"
                            If (articleItemIndex = 1) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISNOTFIRST") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISNOTFIRST"
                            ' Do Nothing

                        Case "ISNOTFIRST2"
                            If (articleItemIndex = 1 And (Request("currentpage") = "" Or Request("currentpage") = "1")) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISNOTFIRST2") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISNOTFIRST2"
                            ' Do Nothing

                        Case "ISSECOND"
                            If (articleItemIndex <> 2) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISSECOND") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISSECOND"
                            ' Do Nothing

                        Case "ISNOTSECOND"
                            If (articleItemIndex = 2) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISNOTSECOND") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISNOTSECOND"
                            ' Do Nothing

                        Case "ISNOTANONYMOUS"
                            If (Request.IsAuthenticated = False) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISNOTANONYMOUS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISNOTANONYMOUS"
                            ' Do Nothing

                        Case "ISNOTSECURE"
                            If (objArticle.IsSecure) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISNOTSECURE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISNOTSECURE"
                            ' Do Nothing

                        Case "ISPUBLISHED"
                            If (objArticle.Status <> StatusType.Published) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISPUBLISHED") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISPUBLISHED"
                            ' Do Nothing

                        Case "ISRATEABLE"
                            If (ArticleSettings.IsRateable = False) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISRATEABLE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISRATEABLE"
                            ' Do Nothing

                        Case "ISRSSITEM"
                            If (objArticle.RssGuid = Null.NullString) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISRSSITEM") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISRSSITEM"
                            ' Do Nothing

                        Case "ISNOTRSSITEM"
                            If (objArticle.RssGuid <> Null.NullString) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISNOTRSSITEM") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISNOTRSSITEM"
                            ' Do Nothing

                        Case "ISSECURE"
                            If (objArticle.IsSecure = False) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISSECURE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISSECURE"
                            ' Do Nothing

                        Case "ISSYNDICATIONENABLED"
                            If (ArticleSettings.IsSyndicationEnabled = False) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISSYNDICATIONENABLED") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISSYNDICATIONENABLED"
                            ' Do Nothing

                        Case "ITEMINDEX"
                            Dim objLiteral As New Literal
                            objLiteral.Text = articleItemIndex.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "LASTUPDATEDATE"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.LastUpdate.ToString("D")
                            objPlaceHolder.Add(objLiteral)

                        Case "LASTUPDATEEMAIL"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.LastUpdateEmail.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "LASTUPDATEFIRSTNAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.LastUpdateFirstName.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "LASTUPDATELASTNAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.LastUpdateLastName.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "LASTUPDATEUSERNAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.LastUpdateUserName.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "LASTUPDATEFULLNAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.LastUpdateFullName.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "LASTUPDATEID"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.LastUpdateID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "LINK"
                            Dim objLiteral As New Literal
                            If objArticle.Url = "" Then
                                Dim pageID As Integer = Null.NullInteger
                                If (ArticleSettings.AlwaysShowPageID) Then
                                    If (Pages(objArticle.ArticleID).Count > 0) Then
                                        pageID = CType(Pages(objArticle.ArticleID)(0), PageInfo).PageID
                                    End If
                                End If
                                objLiteral.Text = Common.GetArticleLink(objArticle, Tab, ArticleSettings, IncludeCategory, pageID)
                            Else
                                objLiteral.Text = Globals.LinkClick(objArticle.Url, Tab.TabID, objArticle.ModuleID, False)
                            End If
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "LINKNEXT"
                            Dim objLink As New HyperLink
                            objLink.CssClass = "CommandButton"
                            If (Pages(objArticle.ArticleID).Count <= 1) Then
                                objLink.Enabled = False
                            Else
                                If (IsNumeric(Request("PageID"))) Then
                                    _pageId = Convert.ToInt32(Request("PageID"))
                                End If

                                If (_pageId = Null.NullInteger) Then
                                    _pageId = CType(Pages(objArticle.ArticleID)(0), PageInfo).PageID
                                End If
                                If (_pageId = CType(Pages(objArticle.ArticleID)(Pages(objArticle.ArticleID).Count - 1), PageInfo).PageID) Then
                                    objLink.Enabled = False
                                Else
                                    objLink.Enabled = True
                                End If
                            End If
                            If (objLink.Enabled = True) Then
                                If (_pageId = Null.NullInteger) Then
                                    _pageId = CType(Pages(objArticle.ArticleID)(0), PageInfo).PageID
                                End If
                                For i As Integer = 0 To Pages(objArticle.ArticleID).Count - 1
                                    Dim objPage As PageInfo = CType(Pages(objArticle.ArticleID)(i), PageInfo)
                                    If (_pageId = objPage.PageID) Then
                                        objLink.NavigateUrl = Common.GetArticleLink(objArticle, Tab, ArticleSettings, IncludeCategory, "PageID=" + CType(Pages(objArticle.ArticleID)(i + 1), PageInfo).PageID.ToString())
                                    End If
                                Next
                            End If
                            objLink.Text = GetSharedResource("NextPage")
                            objPlaceHolder.Add(objLink)

                        Case "LINKPREVIOUS"
                            Dim objLink As New HyperLink
                            objLink.CssClass = "CommandButton"
                            If (Pages(objArticle.ArticleID).Count <= 1) Then
                                objLink.Enabled = False
                            Else
                                If (IsNumeric(Request("PageID"))) Then
                                    _pageId = Convert.ToInt32(Request("PageID"))
                                End If

                                If (_pageId = Null.NullInteger) Then
                                    _pageId = CType(Pages(objArticle.ArticleID)(0), PageInfo).PageID
                                End If
                                If (_pageId = CType(Pages(objArticle.ArticleID)(0), PageInfo).PageID) Then
                                    objLink.Enabled = False
                                Else
                                    objLink.Enabled = True
                                End If
                            End If
                            If (objLink.Enabled = True) Then
                                For i As Integer = 0 To Pages(objArticle.ArticleID).Count - 1
                                    Dim objPage As PageInfo = CType(Pages(objArticle.ArticleID)(i), PageInfo)
                                    If (_pageId = objPage.PageID) Then
                                        If (CType(Pages(objArticle.ArticleID)(i - 1), PageInfo).PageID.ToString() = CType(Pages(objArticle.ArticleID)(0), PageInfo).PageID.ToString()) Then
                                            objLink.NavigateUrl = Common.GetArticleLink(objArticle, Tab, ArticleSettings, IncludeCategory)
                                        Else
                                            objLink.NavigateUrl = Common.GetArticleLink(objArticle, Tab, ArticleSettings, IncludeCategory, "PageID=" + CType(Pages(objArticle.ArticleID)(i - 1), PageInfo).PageID.ToString())
                                        End If
                                        Exit For
                                    End If
                                Next
                            End If
                            objLink.Text = GetSharedResource("PreviousPage")
                            objPlaceHolder.Add(objLink)

                        Case "LINKTARGET"
                            If (objArticle.Url <> "") Then
                                Dim objLiteral As New Literal
                                If (objArticle.IsNewWindow) Then
                                    objLiteral.Text = "_blank"
                                Else
                                    objLiteral.Text = "_self"
                                End If
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "MODULEID"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.ModuleID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "PAGECOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.PageCount.ToString()
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "PAGETEXT"
                            Dim objLiteral As New Literal
                            If (objArticle.PageCount > 0) Then
                                Dim pageID As Integer = Null.NullInteger
                                If (IsNumeric(Request("PageID"))) Then
                                    pageID = Convert.ToInt32(Request("PageID"))
                                End If
                                If (pageID = Null.NullInteger) Then
                                    objLiteral.Text = ProcessPostTokens(Server.HtmlDecode(objArticle.Body), objArticle, Generator, ArticleSettings)
                                Else
                                    Dim pageController As New PageController
                                    Dim pageList As ArrayList = pageController.GetPageList(objArticle.ArticleID)
                                    For Each objPage As PageInfo In pageList
                                        If (objPage.PageID = pageID) Then
                                            objLiteral.Text = ProcessPostTokens(Server.HtmlDecode(objPage.PageText), objArticle, Generator, ArticleSettings)
                                            Exit For
                                        End If
                                    Next
                                    If (objLiteral.Text = Null.NullString) Then
                                        objLiteral.Text = ProcessPostTokens(Server.HtmlDecode(objArticle.Body), objArticle, Generator, ArticleSettings)
                                    End If
                                End If
                            Else
                                objLiteral.Text = ProcessPostTokens(Server.HtmlDecode(objArticle.Summary), objArticle, Generator, ArticleSettings)
                            End If
                            objPlaceHolder.Add(objLiteral)

                        Case "PAGETITLE"
                            Dim objLiteral As New Literal
                            If (objArticle.PageCount > 0) Then
                                Dim pageController As New PageController
                                Dim pageList As ArrayList = pageController.GetPageList(objArticle.ArticleID)
                                Dim pageID As Integer = Null.NullInteger
                                If (IsNumeric(Request("PageID"))) Then
                                    pageID = Convert.ToInt32(Request("PageID"))
                                End If
                                If (pageID = Null.NullInteger) Then
                                    objLiteral.Text = CType(pageList(0), PageInfo).Title
                                Else
                                    For Each objPage As PageInfo In pageList
                                        If (objPage.PageID = pageID) Then
                                            objLiteral.Text = objPage.Title
                                            Exit For
                                        End If
                                    Next
                                    If (objLiteral.Text = Null.NullString) Then
                                        objLiteral.Text = CType(pageList(0), PageInfo).Title
                                    End If
                                End If
                            Else
                                objLiteral.Text = objArticle.Title
                            End If
                            objPlaceHolder.Add(objLiteral)

                        Case "PAGETITLENEXT"
                            Dim objLiteral As New Literal
                            If (Pages(objArticle.ArticleID).Count <= 1) Then
                                objLiteral.Visible = False
                            Else
                                If (_pageId = Null.NullInteger) Then
                                    _pageId = CType(Pages(objArticle.ArticleID)(0), PageInfo).PageID
                                End If
                                If (_pageId = CType(Pages(objArticle.ArticleID)(Pages(objArticle.ArticleID).Count - 1), PageInfo).PageID) Then
                                    objLiteral.Visible = False
                                Else
                                    objLiteral.Visible = True
                                End If
                            End If
                            If (objLiteral.Visible = True) Then
                                If (_pageId = Null.NullInteger) Then
                                    _pageId = CType(Pages(objArticle.ArticleID)(0), PageInfo).PageID
                                End If
                                For i As Integer = 0 To Pages(objArticle.ArticleID).Count - 1
                                    Dim objPage As PageInfo = CType(Pages(objArticle.ArticleID)(i), PageInfo)
                                    If (_pageId = objPage.PageID) Then
                                        objLiteral.Text = CType(Pages(objArticle.ArticleID)(i + 1), PageInfo).Title
                                    End If
                                Next
                            End If
                            If (objLiteral.Visible = True And objLiteral.Text <> "") Then
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "PAGETITLEPREV"
                            Dim objLiteral As New Literal
                            If (Pages(objArticle.ArticleID).Count <= 1) Then
                                objLiteral.Visible = False
                            Else
                                If (_pageId = Null.NullInteger) Then
                                    _pageId = CType(Pages(objArticle.ArticleID)(0), PageInfo).PageID
                                End If
                                If (_pageId = CType(Pages(objArticle.ArticleID)(0), PageInfo).PageID) Then
                                    objLiteral.Visible = False
                                Else
                                    objLiteral.Visible = True
                                End If
                            End If
                            If (objLiteral.Visible = True) Then
                                For i As Integer = 0 To Pages(objArticle.ArticleID).Count - 1
                                    Dim objPage As PageInfo = CType(Pages(objArticle.ArticleID)(i), PageInfo)
                                    If (_pageId = objPage.PageID) Then
                                        If (CType(Pages(objArticle.ArticleID)(i - 1), PageInfo).PageID.ToString() = CType(Pages(objArticle.ArticleID)(0), PageInfo).PageID.ToString()) Then
                                            objLiteral.Text = objArticle.Title
                                        Else
                                            objLiteral.Text = CType(Pages(objArticle.ArticleID)(i - 1), PageInfo).Title
                                        End If
                                        Exit For
                                    End If
                                Next
                            End If
                            If (objLiteral.Visible And objLiteral.Text <> "") Then
                                objPlaceHolder.Add(objLiteral)
                            End If


                        Case "PAGES"
                            Dim drpPages As New DropDownList
                            Dim pageController As New PageController
                            Dim pageList As ArrayList = pageController.GetPageList(objArticle.ArticleID)
                            drpPages.Attributes.Add("onChange", "window.location.href=this.options[this.selectedIndex].value;")
                            drpPages.CssClass = "Normal"
                            Dim pageID As Integer = Null.NullInteger
                            If (IsNumeric(Request("PageID"))) Then
                                pageID = Convert.ToInt32(Request("PageID"))
                            End If
                            For Each objPage As PageInfo In pageList
                                Dim item As New ListItem

                                item.Value = Common.GetModuleLink(ArticleModule.TabID, ArticleModule.ModuleID, "ArticleView", ArticleSettings, "ArticleID=" & objArticle.ArticleID.ToString(), "PageID=" + objPage.PageID.ToString())
                                item.Text = objPage.Title

                                If (objPage.PageID = pageID) Then
                                    item.Selected = True
                                End If
                                drpPages.Items.Add(item)
                            Next
                            If (drpPages.Items.Count > 1) Then
                                objPlaceHolder.Add(drpPages)
                            End If

                        Case "PAGER"

                            Dim pageController As New PageController
                            Dim pageList As ArrayList = pageController.GetPageList(objArticle.ArticleID)

                            Dim pageID As Integer = Null.NullInteger
                            If (IsNumeric(Request("PageID"))) Then
                                pageID = Convert.ToInt32(Request("PageID"))
                            End If

                            Dim pager As String = "" _
                                & "<table class=""PagingTable"" border=""0"">" _
                                & "<tbody>" _
                                & "<tr>"

                            Dim pageNo As Integer = 1

                            Dim y As Integer = 1
                            For Each objPage As PageInfo In pageList
                                If (objPage.PageID = pageID) Then
                                    pageNo = y
                                    Exit For
                                End If
                                y = y + 1
                            Next

                            pager = pager & "<td align=""left"" style=""width: 50%;"" class=""Normal"">" & GetSharedResource("Page") & " " & pageNo.ToString() & " " & GetSharedResource("Of") & " " & pageList.Count.ToString() & "</td>" _
                                & "<td align=""right"" style=""width: 50%;"" class=""Normal"">"

                            If (pageList.Count > 1) Then
                                If (pageNo = 1) Then
                                    pager = pager & "<span class=""NormalDisabled"">" & GetSharedResource("First") & "</span>&nbsp;&nbsp;"
                                Else
                                    pager = pager & "<a class=""CommandButton"" href=""" & Common.GetModuleLink(ArticleModule.TabID, ArticleModule.ModuleID, "ArticleView", ArticleSettings, "ArticleID=" & objArticle.ArticleID.ToString(), "PageID=" + CType(pageList(0), PageInfo).PageID.ToString()) & """>" & GetSharedResource("First") & "</a>&nbsp;&nbsp;"
                                End If
                            Else
                                pager = pager & "<span class=""NormalDisabled"">" & GetSharedResource("First") & "</span>&nbsp;&nbsp;"
                            End If

                            If (pageList.Count > 1) Then
                                If (pageNo = 1) Then
                                    pager = pager & "<span class=""NormalDisabled"">" & GetSharedResource("Previous") & "</span>&nbsp;&nbsp;"
                                Else
                                    Dim x As Integer = 0
                                    For Each objPage As PageInfo In pageList
                                        If (objPage.PageID = pageID) Then
                                            pager = pager & "<a class=""CommandButton"" href=""" & Common.GetModuleLink(ArticleModule.TabID, ArticleModule.ModuleID, "ArticleView", ArticleSettings, "ArticleID=" & objArticle.ArticleID.ToString(), "PageID=" + CType(pageList(x - 1), PageInfo).PageID.ToString()) & """>" & GetSharedResource("Previous") & "</a>&nbsp;&nbsp;"
                                            Exit For
                                        End If
                                        x = x + 1
                                    Next
                                End If
                            Else
                                pager = pager & "<span class=""NormalDisabled"">" & GetSharedResource("Previous") & "</span>&nbsp;&nbsp;"
                            End If

                            Dim i As Integer = 1
                            For Each objPage As PageInfo In pageList
                                If (objPage.PageID = pageID Or (pageID = Null.NullInteger And i = 1)) Then
                                    pager = pager & "<span class=""NormalDisabled"">" & i.ToString() & "</span>&nbsp;&nbsp;"
                                Else
                                    pager = pager & "<a class=""CommandButton"" href=""" & Common.GetModuleLink(ArticleModule.TabID, ArticleModule.ModuleID, "ArticleView", ArticleSettings, "ArticleID=" & objArticle.ArticleID.ToString(), "PageID=" + objPage.PageID.ToString()) & """>" & i.ToString() & "</a>&nbsp;&nbsp;"
                                End If
                                i = i + 1
                            Next

                            If (pageList.Count > 1) Then
                                If (pageID <> Null.NullInteger) Then
                                    If (CType(pageList(pageList.Count - 1), PageInfo).PageID = pageID) Then
                                        pager = pager & "<span class=""NormalDisabled"">" & GetSharedResource("Next") & "</span>&nbsp;&nbsp;"
                                    Else
                                        Dim x As Integer = 0
                                        For Each objPage As PageInfo In pageList
                                            If (objPage.PageID = pageID) Then
                                                pager = pager & "<a class=""CommandButton"" href=""" & Common.GetModuleLink(ArticleModule.TabID, ArticleModule.ModuleID, "ArticleView", ArticleSettings, "ArticleID=" & objArticle.ArticleID.ToString(), "PageID=" + CType(pageList(x + 1), PageInfo).PageID.ToString()) & """>" & GetSharedResource("Next") & "</a>&nbsp;&nbsp;"
                                                Exit For
                                            End If
                                            x = x + 1
                                        Next
                                    End If
                                Else
                                    pager = pager & "<a class=""CommandButton"" href=""" & Common.GetModuleLink(ArticleModule.TabID, ArticleModule.ModuleID, "ArticleView", ArticleSettings, "ArticleID=" & objArticle.ArticleID.ToString(), "PageID=" + CType(pageList(1), PageInfo).PageID.ToString()) & """>" & GetSharedResource("Next") & "</a>&nbsp;&nbsp;"
                                End If
                            Else
                                pager = pager & "<span class=""NormalDisabled"">" & GetSharedResource("Next") & "</span>&nbsp;&nbsp;"
                            End If

                            If (pageList.Count > 1) Then
                                If (CType(pageList(pageList.Count - 1), PageInfo).PageID = pageID) Then
                                    pager = pager & "<span class=""NormalDisabled"">" & GetSharedResource("Last") & "</span>&nbsp;&nbsp;"
                                Else
                                    pager = pager & "<a class=""CommandButton"" href=""" & Common.GetModuleLink(ArticleModule.TabID, ArticleModule.ModuleID, "ArticleView", ArticleSettings, "ArticleID=" & objArticle.ArticleID.ToString(), "PageID=" + CType(pageList(pageList.Count - 1), PageInfo).PageID.ToString()) & """>" & GetSharedResource("Last") & "</a>&nbsp;&nbsp;"
                                End If
                            Else
                                pager = pager & "<span class=""NormalDisabled"">" & GetSharedResource("Last") & "</span>&nbsp;&nbsp;"
                            End If

                            pager = pager & "" _
                                & "</td>" _
                                & "</tr>" _
                                & "</tbody>" _
                                & "</table>"

                            Dim objLiteral As New Literal
                            objLiteral.Text = pager
                            objPlaceHolder.Add(objLiteral)

                        Case "PAGESLIST"
                            Dim pages As String = ""
                            Dim pageController As New PageController
                            Dim pageList As ArrayList = pageController.GetPageList(objArticle.ArticleID)
                            Dim pageID As Integer = Null.NullInteger
                            If (IsNumeric(Request("PageID"))) Then
                                pageID = Convert.ToInt32(Request("PageID"))
                            End If

                            pages = "<ul>"
                            For Each objPage As PageInfo In pageList
                                pages = pages & "<li><a href='" & Common.GetModuleLink(ArticleModule.TabID, ArticleModule.ModuleID, "ArticleView", ArticleSettings, "ArticleID=" & objArticle.ArticleID.ToString(), "PageID=" + objPage.PageID.ToString()) & "'>" & objPage.Title & "</a></li>"
                            Next
                            pages = pages & "</ul>"

                            If (pageList.Count > 1) Then
                                Dim objLiteral As New Literal
                                objLiteral.Text = pages
                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "PAGESLIST2"
                            Dim pages As String = ""
                            Dim pageController As New PageController
                            Dim pageList As ArrayList = pageController.GetPageList(objArticle.ArticleID)
                            Dim pageID As Integer = Null.NullInteger
                            If (IsNumeric(Request("PageID"))) Then
                                pageID = Convert.ToInt32(Request("PageID"))
                            End If

                            pages = "<ul>"
                            Dim isFirst As Boolean = True
                            For Each objPage As PageInfo In pageList
                                If (pageID = objPage.PageID Or (pageID = Null.NullInteger And isFirst)) Then
                                    pages = pages & "<li>" & objPage.Title & "</li>"
                                Else
                                    pages = pages & "<li><a href='" & Common.GetModuleLink(ArticleModule.TabID, ArticleModule.ModuleID, "ArticleView", ArticleSettings, "ArticleID=" & objArticle.ArticleID.ToString(), "PageID=" + objPage.PageID.ToString()) & "'>" & objPage.Title & "</a></li>"
                                End If
                                isFirst = False
                            Next
                            pages = pages & "</ul>"

                            If (pageList.Count > 1) Then
                                Dim objLiteral As New Literal
                                objLiteral.Text = pages
                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "PORTALROOT"
                            Dim objLiteral As New Literal
                            objLiteral.Text = PortalSettings.HomeDirectory
                            objPlaceHolder.Add(objLiteral)

                        Case "POSTCOMMENT"
                            If (ArticleSettings.IsCommentsEnabled) Then
                                Dim objControl As Control = Page.LoadControl("~/DesktopModules/DnnForge - NewsArticles/Controls/PostComment.ascx")
                                CType(objControl, NewsArticleControlBase).ArticleID = objArticle.ArticleID
                                objPlaceHolder.Add(objControl)
                            End If

                        Case "POSTRATING"
                            Dim objControl As Control = Page.LoadControl("~/DesktopModules/DnnForge - NewsArticles/Controls/PostRating.ascx")
                            objPlaceHolder.Add(objControl)

                        Case "PRINT"
                            Dim objHyperLink As New HyperLink
                            If (_pageId <> Null.NullInteger) Then
                                objHyperLink.NavigateUrl = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Print.aspx?tabid=" & ArticleModule.TabID.ToString() & "&tabmoduleid=" & ArticleModule.TabModuleID.ToString() & "&articleId=" & objArticle.ArticleID.ToString() & "&moduleId=" & objArticle.ModuleID.ToString() & "&PortalID=" & PortalSettings.PortalId.ToString() & "&PageID=" & _pageId.ToString())
                            Else
                                objHyperLink.NavigateUrl = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Print.aspx?tabid=" & ArticleModule.TabID.ToString() & "&tabmoduleid=" & ArticleModule.TabModuleID.ToString() & "&articleId=" & objArticle.ArticleID.ToString() & "&moduleId=" & objArticle.ModuleID.ToString() & "&PortalID=" & PortalSettings.PortalId.ToString())
                            End If
                            objHyperLink.ImageUrl = "~/images/print.gif"
                            objHyperLink.ToolTip = GetArticleResource("ClickPrint")
                            objHyperLink.EnableViewState = False
                            objHyperLink.Target = "_blank"
                            objHyperLink.Attributes.Add("rel", "nofollow")
                            objPlaceHolder.Add(objHyperLink)

                        Case "PRINTLINK"
                            Dim objLiteral As New Literal
                            If (_pageId <> Null.NullInteger) Then
                                objLiteral.Text = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Print.aspx?tabid=" & ArticleModule.TabID.ToString() & "&tabmoduleid=" & ArticleModule.TabModuleID.ToString() & "&articleId=" & objArticle.ArticleID.ToString() & "&moduleId=" & objArticle.ModuleID.ToString() & "&PortalID=" & PortalSettings.PortalId.ToString() & "&PageID=" & _pageId.ToString())
                            Else
                                objLiteral.Text = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/Print.aspx?tabid=" & ArticleModule.TabID.ToString() & "&tabmoduleid=" & ArticleModule.TabModuleID.ToString() & "&articleId=" & objArticle.ArticleID.ToString() & "&moduleId=" & objArticle.ModuleID.ToString() & "&PortalID=" & PortalSettings.PortalId.ToString())
                            End If
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "PUBLISHDATE"
                            Dim objLiteral As New Literal
                            If (objArticle.StartDate = Null.NullDate) Then
                                objLiteral.Text = objArticle.CreatedDate.ToString("D")
                            Else
                                objLiteral.Text = objArticle.StartDate.ToString("D")
                            End If
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "PUBLISHSTARTDATE"
                            Dim objLiteral As New Literal
                            If (objArticle.StartDate = Null.NullDate) Then
                                objLiteral.Text = objArticle.CreatedDate.ToString("D")
                            Else
                                objLiteral.Text = objArticle.StartDate.ToString("D")
                            End If
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "PUBLISHTIME"
                            Dim objLiteral As New Literal
                            If (objArticle.StartDate = Null.NullDate) Then
                                objLiteral.Text = objArticle.CreatedDate.ToString("t")
                            Else
                                objLiteral.Text = objArticle.StartDate.ToString("t")
                            End If
                            objLiteral.EnableViewState = False

                        Case "PUBLISHSTARTTIME"
                            Dim objLiteral As New Literal
                            If (objArticle.StartDate = Null.NullDate) Then
                                objLiteral.Text = objArticle.CreatedDate.ToString("t")
                            Else
                                objLiteral.Text = objArticle.StartDate.ToString("t")
                            End If
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "PUBLISHENDDATE"
                            Dim objLiteral As New Literal
                            If (objArticle.EndDate = Null.NullDate) Then
                                objLiteral.Text = ""
                            Else
                                objLiteral.Text = objArticle.EndDate.ToString("D")
                            End If
                            objPlaceHolder.Add(objLiteral)

                        Case "PUBLISHENDTIME"
                            Dim objLiteral As New Literal
                            If (objArticle.EndDate = Null.NullDate) Then
                                objLiteral.Text = ""
                            Else
                                objLiteral.Text = objArticle.EndDate.ToString("t")
                            End If
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "RATING"
                            Dim objImage As New Image
                            objImage.ImageUrl = GetRatingImage(objArticle)
                            objImage.EnableViewState = False
                            objImage.ToolTip = "Article Rating"
                            objImage.AlternateText = "Article Rating"
                            objPlaceHolder.Add(objImage)

                        Case "RATINGCOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.RatingCount.ToString()
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "RATINGDETAIL"
                            If (objArticle.Rating <> Null.NullDouble) Then
                                Dim objLiteral As New Literal
                                objLiteral.Text = objArticle.Rating.ToString("R1")
                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "RELATED"
                            If (ArticleSettings.RelatedMode <> RelatedType.None) Then
                                Dim phRelated As New PlaceHolder

                                Dim objArticles As List(Of ArticleInfo) = GetRelatedArticles(objArticle, 5)

                                If (objArticles.Count > 0) Then
                                    Dim _objLayoutRelatedHeader As LayoutInfo = GetLayout(ArticleSettings, ArticleModule, Page, LayoutType.Related_Header_Html)
                                    Dim _objLayoutRelatedItem As LayoutInfo = GetLayout(ArticleSettings, ArticleModule, Page, LayoutType.Related_Item_Html)
                                    Dim _objLayoutRelatedFooter As LayoutInfo = GetLayout(ArticleSettings, ArticleModule, Page, LayoutType.Related_Footer_Html)

                                    ProcessArticleItem(phRelated.Controls, _objLayoutRelatedHeader.Tokens, objArticle)
                                    For Each objRelatedArticle As ArticleInfo In objArticles
                                        ProcessArticleItem(phRelated.Controls, _objLayoutRelatedItem.Tokens, objRelatedArticle)
                                    Next
                                    ProcessArticleItem(phRelated.Controls, _objLayoutRelatedFooter.Tokens, objArticle)

                                    objPlaceHolder.Add(phRelated)
                                End If
                            End If

                        Case "SITEROOT"
                            Dim objLiteral As New Literal
                            objLiteral.Text = ArticleUtilities.ResolveUrl("~/")
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "SITETITLE"
                            Dim objLiteral As New Literal
                            objLiteral.Text = PortalSettings.PortalName
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "SHORTLINK"
                            If (objArticle.ShortUrl <> "") Then
                                Dim objLiteral As New Literal
                                objLiteral.Text = objArticle.ShortUrl
                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                            Else
                                If (ArticleSettings.TwitterBitLyAPIKey <> "" And ArticleSettings.TwitterBitLyLogin <> "") Then
                                    Dim link As String = Common.GetArticleLink(objArticle, Tab, ArticleSettings, False)
                                    Dim b As New bitly(ArticleSettings.TwitterBitLyLogin, ArticleSettings.TwitterBitLyAPIKey)
                                    Dim shortUrl As String = b.Shorten(link)

                                    If (shortUrl <> "") Then
                                        Dim objLiteral As New Literal
                                        objLiteral.Text = shortUrl
                                        objLiteral.EnableViewState = False
                                        objPlaceHolder.Add(objLiteral)

                                        objArticle.ShortUrl = shortUrl
                                        Dim objArticleController As New ArticleController()
                                        objArticleController.UpdateArticle(objArticle)
                                    End If
                                End If
                            End If

                        Case "SUMMARY"
                            Dim objLiteral As New Literal
                            objLiteral.Text = ProcessPostTokens(Server.HtmlDecode(objArticle.Summary), objArticle, Generator, ArticleSettings)
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "TABID"
                            Dim objLiteral As New Literal
                            objLiteral.Text = ArticleModule.TabID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "TABTITLE"
                            Dim objLiteral As New Literal
                            If (PortalSettings.ActiveTab.Title.Length = 0) Then
                                objLiteral.Text = PortalSettings.ActiveTab.TabName
                            Else
                                objLiteral.Text = PortalSettings.ActiveTab.Title
                            End If
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "TAGS"
                            If (objArticle.Tags.Trim() <> "") Then
                                Dim objLiteral As New Literal
                                For Each tag As String In objArticle.Tags.Split(","c)
                                    If (objLiteral.Text = "") Then
                                        objLiteral.Text = "<a href=""" + Common.GetModuleLink(ArticleModule.TabID, objArticle.ModuleID, "TagView", ArticleSettings, "Tag=" + Server.UrlEncode(tag)) + """>" + tag + "</a>"
                                    Else
                                        objLiteral.Text = objLiteral.Text + ", " + "<a href=""" + Common.GetModuleLink(ArticleModule.TabID, objArticle.ModuleID, "TagView", ArticleSettings, "Tag=" + Server.UrlEncode(tag)) + """>" + tag + "</a>"
                                    End If
                                Next
                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "TAGSNOLINK"
                            If (objArticle.Tags.Trim() <> "") Then
                                Dim objLiteral As New Literal
                                For Each tag As String In objArticle.Tags.Split(","c)
                                    If (objLiteral.Text = "") Then
                                        objLiteral.Text = tag
                                    Else
                                        objLiteral.Text = objLiteral.Text + ", " + tag
                                    End If
                                Next
                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "TEMPLATEPATH"
                            Dim objLiteral As New Literal
                            objLiteral.Text = ArticleUtilities.ResolveUrl("~/DnnForge - NewsArticles/Templates/" & ArticleSettings.Template & "/")
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "TITLE"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.Title
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "TITLESAFEJS"
                            If (objArticle.Title <> "") Then
                                Dim objLiteral As New Literal
                                objLiteral.Text = objArticle.Title.Replace("""", "")
                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "TITLEURLENCODED"
                            Dim objLiteral As New Literal
                            objLiteral.Text = Server.UrlEncode(objArticle.Title)
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "TWITTERNAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = ArticleSettings.TwitterName
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "UPDATEDATE"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.LastUpdate.ToString("D")
                            objPlaceHolder.Add(objLiteral)

                        Case "UPDATETIME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.LastUpdate.ToString("t")
                            objPlaceHolder.Add(objLiteral)

                        Case "VIEWCOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.NumberOfViews.ToString()
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case Else

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("AUTHOR:")) Then
                                If Author(objArticle.AuthorID) IsNot Nothing Then
                                    ' token to be processed
                                    Dim field As String = layoutArray(iPtr + 1).Substring(7, layoutArray(iPtr + 1).Length - 7).ToLower().Trim()

                                    'Gets the DNN profile property named like the token (field)
                                    Dim profilePropertyFound As Boolean = False
                                    Dim profilePropertyDataType As String = String.Empty
                                    Dim profilePropertyName As String = String.Empty
                                    Dim profilePropertyValue As String = String.Empty

                                    For Each objProfilePropertyDefinition As ProfilePropertyDefinition In ProfileProperties
                                        If (objProfilePropertyDefinition.PropertyName.ToLower().Trim() = field) Then

                                            'Gets the dnn profile property's datatype
                                            Dim objListController As New ListController
                                            Dim definitionEntry As ListEntryInfo = objListController.GetListEntryInfo(objProfilePropertyDefinition.DataType)
                                            If Not definitionEntry Is Nothing Then
                                                profilePropertyDataType = definitionEntry.Value
                                            Else
                                                profilePropertyDataType = "Unknown"
                                            End If

                                            'Gets the dnn profile property's name and current value for the given user (Agent = AuthorID)
                                            profilePropertyName = objProfilePropertyDefinition.PropertyName
                                            profilePropertyValue = Author(objArticle.AuthorID).Profile.GetPropertyValue(profilePropertyName)

                                            profilePropertyFound = True

                                        End If
                                    Next

                                    If profilePropertyFound Then

                                        Select Case profilePropertyDataType.ToLower()
                                            Case "truefalse"
                                                Dim objTrueFalse As New CheckBox
                                                If profilePropertyValue = String.Empty Then
                                                    objTrueFalse.Checked = False
                                                Else
                                                    objTrueFalse.Checked = CType(profilePropertyValue, Boolean)
                                                End If
                                                objTrueFalse.Enabled = False
                                                objTrueFalse.EnableViewState = False
                                                objPlaceHolder.Add(objTrueFalse)

                                            Case "richtext"
                                                Dim objLiteral As New Literal
                                                If profilePropertyValue = String.Empty Then
                                                    objLiteral.Text = String.Empty
                                                Else
                                                    objLiteral.Text = Server.HtmlDecode(profilePropertyValue)
                                                End If
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)

                                            Case "list"
                                                Dim objLiteral As New Literal
                                                objLiteral.Text = profilePropertyValue
                                                Dim objListController As New ListController
                                                Dim objListEntryInfoCollection As IEnumerable(Of ListEntryInfo) = objListController.GetListEntryInfoItems(profilePropertyName)
                                                For Each objListEntryInfo As ListEntryInfo In objListEntryInfoCollection
                                                    If objListEntryInfo.Value = profilePropertyValue Then
                                                        objLiteral.Text = objListEntryInfo.Text
                                                        Exit For
                                                    End If
                                                Next
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)

                                            Case "image"
                                                Dim objLiteral As New Literal
                                                If profilePropertyValue = String.Empty Then
                                                    objLiteral.Text = String.Empty
                                                Else
                                                    objLiteral.Text = UrlUtils.EncryptParameter(UrlUtils.GetParameterValue($"fileid={profilePropertyValue}"), PortalSettings.GUID.ToString())
                                                End If
                                                objPlaceHolder.Add(objLiteral)

                                            Case Else
                                                Dim objLiteral As New Literal
                                                If profilePropertyValue = String.Empty Then
                                                    objLiteral.Text = String.Empty
                                                Else
                                                    If profilePropertyName.ToLower() = "website" Then
                                                        Dim url As String = profilePropertyValue
                                                        If url.ToLower.StartsWith("http://") Then
                                                            url = url.Substring(7) ' removes the "http://"
                                                        End If
                                                        objLiteral.Text = url
                                                    Else
                                                        objLiteral.Text = profilePropertyValue
                                                    End If
                                                End If
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)
                                        End Select 'profilePropertyDataType

                                    End If ' DNN Profile property processing
                                End If
                                Exit Select
                            End If ' "AUTHOR:" token

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CAPTION:")) Then

                                Dim objCustomFieldController As New CustomFieldController
                                Dim objCustomFields As ArrayList = objCustomFieldController.List(objArticle.ModuleID)

                                Dim field As String = layoutArray(iPtr + 1).Substring(8, layoutArray(iPtr + 1).Length - 8)

                                Dim i As Integer = 0
                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                        If (objArticle.CustomList.Contains(objCustomField.CustomFieldID)) Then
                                            Dim objLiteral As New Literal
                                            objLiteral.Text = objCustomField.Caption
                                            objLiteral.EnableViewState = False
                                            objPlaceHolder.Add(objLiteral)
                                            i = i + 1
                                        End If
                                    End If
                                Next

                                Exit Select

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CATEGORIESSUB:")) Then
                                Dim values As String = layoutArray(iPtr + 1).Substring(14, layoutArray(iPtr + 1).Length - 14)

                                Dim splitValues As String() = values.Split(":"c)

                                If (splitValues.Length = 2) Then

                                    Dim category As String = splitValues(0)
                                    Dim number As String = splitValues(1)

                                    If (IsNumeric(number)) Then
                                        If (Convert.ToInt32(number) > 0) Then

                                            ' Find category

                                            Dim objCategoryController As New CategoryController
                                            Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(ArticleModule.ModuleID, Null.NullInteger)

                                            Dim categoryID As Integer = Null.NullInteger
                                            For Each objCategory As CategoryInfo In objCategories

                                                If (objCategory.Name.ToLower() = category.ToLower()) Then
                                                    categoryID = objCategory.CategoryID
                                                    Exit For
                                                End If

                                            Next

                                            If (categoryID <> Null.NullInteger) Then

                                                Dim objCategoriesSelected As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(ArticleModule.ModuleID, categoryID, Nothing, Null.NullInteger, Convert.ToInt32(number), False, CategorySortType.Name)

                                                Dim objArticleCategories As ArrayList = CType(DataCache.GetCache(ArticleConstants.CACHE_CATEGORY_ARTICLE & objArticle.ArticleID.ToString()), ArrayList)
                                                If (objArticleCategories Is Nothing) Then
                                                    Dim objArticleController As New ArticleController
                                                    objArticleCategories = objArticleController.GetArticleCategories(objArticle.ArticleID)
                                                    DataCache.SetCache(ArticleConstants.CACHE_CATEGORY_ARTICLE & objArticle.ArticleID.ToString(), objArticleCategories)
                                                End If


                                                Dim objLiteral As New Literal
                                                For Each objCategory As CategoryInfo In objArticleCategories

                                                    For Each objCategorySel As CategoryInfo In objCategoriesSelected
                                                        If (objCategory.CategoryID = objCategorySel.CategoryID) Then
                                                            If (objLiteral.Text <> "") Then
                                                                objLiteral.Text = objLiteral.Text & ", <a href=""" & Common.GetCategoryLink(ArticleModule.TabID, ArticleModule.ModuleID, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings) & """ > " & objCategory.Name & "</a>"
                                                            Else
                                                                objLiteral.Text = "<a href=""" & Common.GetCategoryLink(ArticleModule.TabID, ArticleModule.ModuleID, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings) & """ > " & objCategory.Name & "</a>"
                                                            End If
                                                        End If
                                                    Next

                                                Next
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)
                                                Exit Select

                                            End If
                                        End If

                                    End If

                                End If

                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CREATEDATE:")) Then
                                Dim formatExpression As String = layoutArray(iPtr + 1).Substring(11, layoutArray(iPtr + 1).Length - 11)

                                Dim objLiteral As New Literal

                                Try
                                    If (objArticle.CreatedDate = Null.NullDate) Then
                                        objLiteral.Text = ""
                                    Else
                                        objLiteral.Text = objArticle.CreatedDate.ToString(formatExpression)
                                    End If
                                Catch
                                    If (objArticle.CreatedDate = Null.NullDate) Then
                                        objLiteral.Text = ""
                                    Else
                                        objLiteral.Text = objArticle.CreatedDate.ToString("D")
                                    End If
                                End Try

                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CREATEDATELESSTHAN:")) Then
                                Dim length As Integer = Convert.ToInt32(layoutArray(iPtr + 1).Substring(19, layoutArray(iPtr + 1).Length - 19))

                                If (objArticle.CreatedDate < DateTime.Now.AddDays(length * -1)) Then
                                    Dim endVal As String = layoutArray(iPtr + 1).ToUpper()
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = ("/" & endVal)) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/CREATEDATELESSTHAN:")) Then
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CUSTOM:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(7, layoutArray(iPtr + 1).Length - 7).ToLower()

                                Dim customFieldID As Integer = Null.NullInteger
                                Dim objCustomFieldSelected As New CustomFieldInfo
                                Dim isLink As Boolean = False

                                Dim objCustomFieldController As New CustomFieldController
                                Dim objCustomFields As ArrayList = objCustomFieldController.List(objArticle.ModuleID)

                                Dim maxLength As Integer = Null.NullInteger
                                If (field.IndexOf(":"c) <> -1) Then
                                    Try
                                        maxLength = Convert.ToInt32(field.Split(":"c)(1))
                                    Catch
                                        maxLength = Null.NullInteger
                                    End Try
                                    field = field.Split(":"c)(0)
                                End If
                                If (customFieldID = Null.NullInteger) Then
                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                            customFieldID = objCustomField.CustomFieldID
                                            objCustomFieldSelected = objCustomField
                                        End If
                                    Next
                                End If

                                If (customFieldID <> Null.NullInteger) Then

                                    Dim i As Integer = 0
                                    If (objArticle.CustomList.Contains(customFieldID)) Then
                                        Dim objLiteral As New Literal
                                        Dim fieldValue As String = GetFieldValue(objCustomFieldSelected, objArticle, False)
                                        If (maxLength <> Null.NullInteger) Then
                                            If (fieldValue.Length > maxLength) Then
                                                fieldValue = fieldValue.Substring(0, maxLength)
                                            End If
                                        End If
                                        objLiteral.Text = fieldValue.TrimStart("#"c)
                                        objLiteral.EnableViewState = False
                                        objPlaceHolder.Add(objLiteral)
                                        i = i + 1
                                    End If
                                End If
                                Exit Select
                            End If


                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("DETAILS:")) Then
                                Dim length As Integer = Convert.ToInt32(layoutArray(iPtr + 1).Substring(8, layoutArray(iPtr + 1).Length - 8))

                                Dim objLiteral As New Literal
                                If (StripHtml(Server.HtmlDecode(objArticle.Body)).TrimStart().Length > length) Then
                                    objLiteral.Text = ProcessPostTokens(Left(StripHtml(Server.HtmlDecode(objArticle.Body)).TrimStart(), length), objArticle, Generator, ArticleSettings) & "..."
                                Else
                                    objLiteral.Text = ProcessPostTokens(Left(StripHtml(Server.HtmlDecode(objArticle.Body)).TrimStart(), length), objArticle, Generator, ArticleSettings)
                                End If

                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("EXPRESSION:")) Then

                                Dim objCustomFieldController As New CustomFieldController
                                Dim objCustomFields As ArrayList = objCustomFieldController.List(objArticle.ModuleID)

                                Dim field As String = layoutArray(iPtr + 1).Substring(11, layoutArray(iPtr + 1).Length - 11)

                                Dim params As String() = field.Split(":"c)

                                If (params.Length <> 3) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                    Exit Select
                                End If

                                Dim customField As String = params(0)
                                Dim customExpression As String = params(1)
                                Dim customValue As String = params(2)

                                Dim fieldValue As String = ""

                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.Name.ToLower() = customField.ToLower()) Then
                                        If (objArticle.CustomList.Contains(objCustomField.CustomFieldID)) Then
                                            fieldValue = GetFieldValue(objCustomField, objArticle, False)
                                        End If
                                    End If
                                Next

                                Dim isValid As Boolean = False
                                Select Case customExpression
                                    Case "="
                                        If (customValue.ToLower() = fieldValue.ToLower()) Then
                                            isValid = True
                                        End If
                                        Exit Select

                                    Case "!="
                                        If (customValue.ToLower() <> fieldValue.ToLower()) Then
                                            isValid = True
                                        End If
                                        Exit Select

                                    Case "<"
                                        If (IsNumeric(customValue) AndAlso IsNumeric(fieldValue)) Then
                                            If (Convert.ToInt32(fieldValue) < Convert.ToInt32(customValue)) Then
                                                isValid = True
                                            End If
                                        End If
                                        Exit Select

                                    Case "<="
                                        If (IsNumeric(customValue) AndAlso IsNumeric(fieldValue)) Then
                                            If (Convert.ToInt32(fieldValue) <= Convert.ToInt32(customValue)) Then
                                                isValid = True
                                            End If
                                        End If
                                        Exit Select

                                    Case ">"
                                        If (IsNumeric(customValue) AndAlso IsNumeric(fieldValue)) Then
                                            If (Convert.ToInt32(fieldValue) > Convert.ToInt32(customValue)) Then
                                                isValid = True
                                            End If
                                        End If
                                        Exit Select

                                    Case ">="
                                        If (IsNumeric(customValue) AndAlso IsNumeric(fieldValue)) Then
                                            If (Convert.ToInt32(fieldValue) >= Convert.ToInt32(customValue)) Then
                                                isValid = True
                                            End If
                                        End If
                                        Exit Select

                                End Select

                                If (isValid = False) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If

                                Exit Select

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/EXPRESSION:")) Then
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("HASIMAGES:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(10, layoutArray(iPtr + 1).Length - 10)

                                If (IsNumeric(field)) Then

                                    If (objArticle.ImageCount < Convert.ToInt32(field)) Then
                                        Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                        While (iPtr < layoutArray.Length - 1)
                                            If (layoutArray(iPtr + 1) = endToken) Then
                                                Exit While
                                            End If
                                            iPtr = iPtr + 1
                                        End While
                                    End If

                                End If

                                Exit Select

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/HASIMAGES:")) Then
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("HASMOREDETAIL:")) Then
                                Dim length As Integer = Convert.ToInt32(layoutArray(iPtr + 1).Substring(14, layoutArray(iPtr + 1).Length - 14))
                                Dim endToken As String = "/" & layoutArray(iPtr + 1)

                                If (objArticle.Url = Null.NullString() And StripHtml(objArticle.Summary.Trim()) = "" And StripHtml(Server.HtmlDecode(objArticle.Body)).TrimStart().Length <= length) Then
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/HASMOREDETAIL:")) Then
                                ' Do Nothing
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("HASNOVALUE:")) Then

                                Dim objCustomFieldController As New CustomFieldController
                                Dim objCustomFields As ArrayList = objCustomFieldController.List(objArticle.ModuleID)

                                Dim field As String = layoutArray(iPtr + 1).Substring(11, layoutArray(iPtr + 1).Length - 11)

                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                        If (objArticle.CustomList.Contains(objCustomField.CustomFieldID)) Then
                                            Dim fieldValue As String = GetFieldValue(objCustomField, objArticle, False)
                                            If (fieldValue.Trim() <> "") Then
                                                Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                                While (iPtr < layoutArray.Length - 1)
                                                    If (layoutArray(iPtr + 1) = endToken) Then
                                                        Exit While
                                                    End If
                                                    iPtr = iPtr + 1
                                                End While
                                            End If
                                        End If
                                    End If
                                Next

                                Exit Select

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/HASNOVALUE:")) Then
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("HASVALUE:")) Then

                                Dim objCustomFieldController As New CustomFieldController
                                Dim objCustomFields As ArrayList = objCustomFieldController.List(objArticle.ModuleID)

                                Dim field As String = layoutArray(iPtr + 1).Substring(9, layoutArray(iPtr + 1).Length - 9)

                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                        If (objArticle.CustomList.Contains(objCustomField.CustomFieldID)) Then
                                            Dim fieldValue As String = GetFieldValue(objCustomField, objArticle, False)
                                            If (fieldValue.Trim() = "") Then
                                                Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                                While (iPtr < layoutArray.Length - 1)
                                                    If (layoutArray(iPtr + 1) = endToken) Then
                                                        Exit While
                                                    End If
                                                    iPtr = iPtr + 1
                                                End While
                                            End If
                                        End If
                                    End If
                                Next

                                Exit Select

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/HASVALUE:")) Then
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("HASAUTHORVALUE:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(15, layoutArray(iPtr + 1).Length - 15).ToLower().Trim()

                                'Gets the DNN profile property named like the token (field)
                                Dim profilePropertyName As String = String.Empty
                                Dim profilePropertyValue As String = String.Empty

                                For Each objProfilePropertyDefinition As ProfilePropertyDefinition In ProfileProperties
                                    If (objProfilePropertyDefinition.PropertyName.ToLower().Trim() = field) Then

                                        'Gets the dnn profile property's datatype
                                        Dim objListController As New ListController
                                        Dim definitionEntry As ListEntryInfo = objListController.GetListEntryInfo(objProfilePropertyDefinition.DataType)
                                        If Not definitionEntry Is Nothing Then
                                        Else
                                        End If

                                        'Gets the dnn profile property's name and current value for the given user (Agent = AuthorID)
                                        profilePropertyName = objProfilePropertyDefinition.PropertyName
                                        If (Author(objArticle.AuthorID) IsNot Nothing) Then
                                            profilePropertyValue = Author(objArticle.AuthorID).Profile.GetPropertyValue(profilePropertyName)
                                            Exit For
                                        End If

                                    End If
                                Next

                                If (profilePropertyValue = "") Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If

                                Exit Select

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/HASAUTHORVALUE:")) Then
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("IMAGE:")) Then
                                If (objArticle.ImageCount > 0) Then
                                    Dim val As String = layoutArray(iPtr + 1).Substring(6, layoutArray(iPtr + 1).Length - 6)

                                    If (IsNumeric(val)) Then
                                        Dim objImageController As New ImageController
                                        Dim objImages As List(Of ImageInfo) = objImageController.GetImageList(objArticle.ArticleID, Null.NullString())

                                        If (objImages.Count > 0) Then
                                            Dim count As Integer = 1
                                            For Each objChildImage As ImageInfo In objImages
                                                If (count = Convert.ToInt32(val)) Then
                                                    Dim objImage As New Image
                                                    objImage.ImageUrl = PortalSettings.HomeDirectory & objChildImage.Folder & objChildImage.FileName
                                                    objImage.EnableViewState = False
                                                    objImage.AlternateText = objArticle.Title
                                                    objPlaceHolder.Add(objImage)
                                                End If
                                                count = count + 1
                                            Next
                                        End If
                                    End If
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("IMAGETHUMB:")) Then

                                If (objArticle.ImageUrl <> "") Then
                                    Dim objImage As New Image
                                    objImage.ImageUrl = objArticle.ImageUrl
                                    objImage.EnableViewState = False
                                    objImage.AlternateText = objArticle.Title
                                    objPlaceHolder.Add(objImage)
                                Else
                                    If (objArticle.ImageCount > 0) Then

                                        Dim objImageController As New ImageController
                                        Dim objImages As List(Of ImageInfo) = objImageController.GetImageList(objArticle.ArticleID, Null.NullString)
                                        If (objImages.Count > 0) Then

                                            Dim val As String = layoutArray(iPtr + 1).Substring(11, layoutArray(iPtr + 1).Length - 11)
                                            If (val.IndexOf(":"c) = -1) Then
                                                Dim length As Integer = Convert.ToInt32(val)

                                                Dim objImage As New Image
                                                If (ArticleSettings.ImageThumbnailType = ThumbnailType.Proportion) Then
                                                    objImage.ImageUrl = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & length.ToString() & "&Height=" & length.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(objImages(0).Folder & objImages(0).FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1")
                                                Else
                                                    objImage.ImageUrl = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & length.ToString() & "&Height=" & length.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(objImages(0).Folder & objImages(0).FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1&s=1")
                                                End If
                                                objImage.EnableViewState = False
                                                objImage.AlternateText = objArticle.Title
                                                objPlaceHolder.Add(objImage)
                                            Else

                                                Dim arr() As String = val.Split(":"c)

                                                If (arr.Length = 2) Then
                                                    Dim width As Integer = Convert.ToInt32(val.Split(":"c)(0))
                                                    Dim height As Integer = Convert.ToInt32(val.Split(":"c)(1))

                                                    Dim objImage As New Image
                                                    If (ArticleSettings.ImageThumbnailType = ThumbnailType.Proportion) Then
                                                        objImage.ImageUrl = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(objImages(0).Folder & objImages(0).FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1")
                                                    Else
                                                        objImage.ImageUrl = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(objImages(0).Folder & objImages(0).FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1&s=1")
                                                    End If
                                                    objImage.EnableViewState = False
                                                    objImage.AlternateText = objArticle.Title
                                                    objPlaceHolder.Add(objImage)
                                                Else
                                                    If (arr.Length = 3) Then
                                                        Dim width As Integer = Convert.ToInt32(val.Split(":"c)(0))
                                                        Dim height As Integer = Convert.ToInt32(val.Split(":"c)(1))
                                                        Dim item As Integer = Convert.ToInt32(val.Split(":"c)(2))

                                                        If (objImages.Count > 0) Then
                                                            Dim count As Integer = 1
                                                            For Each objChildImage As ImageInfo In objImages
                                                                If (count = item) Then
                                                                    Dim objImage As New Image
                                                                    If (ArticleSettings.ImageThumbnailType = ThumbnailType.Proportion) Then
                                                                        objImage.ImageUrl = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(objChildImage.Folder & objChildImage.FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1")
                                                                    Else
                                                                        objImage.ImageUrl = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(objChildImage.Folder & objChildImage.FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1&s=1")
                                                                    End If
                                                                    objImage.EnableViewState = False
                                                                    objImage.AlternateText = objArticle.Title
                                                                    objPlaceHolder.Add(objImage)
                                                                End If
                                                                count = count + 1
                                                            Next
                                                        End If

                                                    End If
                                                End If
                                            End If

                                        End If

                                    End If
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("IMAGETHUMBLINK:")) Then

                                If (objArticle.ImageUrl <> "") Then
                                    Dim objImage As New Image
                                    objImage.ImageUrl = objArticle.ImageUrl
                                    objImage.EnableViewState = False
                                    objImage.AlternateText = objArticle.Title
                                    objPlaceHolder.Add(objImage)
                                Else
                                    If (objArticle.ImageCount > 0) Then

                                        Dim objImageController As New ImageController
                                        Dim objImages As List(Of ImageInfo) = objImageController.GetImageList(objArticle.ArticleID, Null.NullString)

                                        If (objImages.Count > 0) Then

                                            Dim val As String = layoutArray(iPtr + 1).Substring(15, layoutArray(iPtr + 1).Length - 15)
                                            If (val.IndexOf(":"c) = -1) Then
                                                Dim length As Integer = Convert.ToInt32(val)

                                                Dim objLiteral As New Literal
                                                If (ArticleSettings.ImageThumbnailType = ThumbnailType.Proportion) Then
                                                    objLiteral.Text = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & length.ToString() & "&Height=" & length.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(objImages(0).Folder & objImages(0).FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1")
                                                Else
                                                    objLiteral.Text = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & length.ToString() & "&Height=" & length.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(objImages(0).Folder & objImages(0).FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1&s=1")
                                                End If
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)
                                            Else
                                                Dim arr() As String = val.Split(":"c)

                                                If (arr.Length = 2) Then
                                                    Dim width As Integer = Convert.ToInt32(val.Split(":"c)(0))
                                                    Dim height As Integer = Convert.ToInt32(val.Split(":"c)(1))

                                                    Dim objLiteral As New Literal
                                                    If (objArticle.ImageUrl.ToLower().StartsWith("http://") Or objArticle.ImageUrl.ToLower().StartsWith("https://")) Then
                                                        objLiteral.Text = objArticle.ImageUrl
                                                    Else
                                                        If (ArticleSettings.ImageThumbnailType = ThumbnailType.Proportion) Then
                                                            objLiteral.Text = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(objImages(0).Folder & objImages(0).FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1")
                                                        Else
                                                            objLiteral.Text = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(objImages(0).Folder & objImages(0).FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1&s=1")
                                                        End If
                                                    End If
                                                    objLiteral.EnableViewState = False
                                                    objPlaceHolder.Add(objLiteral)
                                                Else
                                                    If (arr.Length = 3) Then
                                                        Dim width As Integer = Convert.ToInt32(val.Split(":"c)(0))
                                                        Dim height As Integer = Convert.ToInt32(val.Split(":"c)(1))
                                                        Dim item As Integer = Convert.ToInt32(val.Split(":"c)(2))

                                                        If (objImages.Count > 0) Then
                                                            Dim count As Integer = 1
                                                            For Each objChildImage As ImageInfo In objImages
                                                                If (count = item) Then
                                                                    Dim objLiteral As New Literal
                                                                    If (objArticle.ImageUrl.ToLower().StartsWith("http://") Or objArticle.ImageUrl.ToLower().StartsWith("https://")) Then
                                                                        objLiteral.Text = objArticle.ImageUrl
                                                                    Else
                                                                        If (ArticleSettings.ImageThumbnailType = ThumbnailType.Proportion) Then
                                                                            objLiteral.Text = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(objChildImage.Folder & objChildImage.FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1")
                                                                        Else
                                                                            objLiteral.Text = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(objChildImage.Folder & objChildImage.FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1&s=1")
                                                                        End If
                                                                    End If
                                                                    objLiteral.EnableViewState = False
                                                                    objPlaceHolder.Add(objLiteral)
                                                                End If
                                                                count = count + 1
                                                            Next
                                                        End If

                                                    End If
                                                End If

                                            End If
                                        End If
                                    End If
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("IMAGETHUMBRANDOM:")) Then

                                If (objArticle.ImageCount > 0) Then

                                    Dim objImageController As New ImageController
                                    Dim objImages As List(Of ImageInfo) = objImageController.GetImageList(objArticle.ArticleID, Null.NullString)
                                    If (objImages.Count > 0) Then

                                        Dim randomImage As ImageInfo = objImages(Generator.Next(0, objImages.Count - 1))

                                        Dim val As String = layoutArray(iPtr + 1).Substring(17, layoutArray(iPtr + 1).Length - 17)
                                        If (val.IndexOf(":"c) = -1) Then
                                            Dim length As Integer = Convert.ToInt32(val)

                                            Dim objImage As New Image
                                            If (ArticleSettings.ImageThumbnailType = ThumbnailType.Proportion) Then
                                                objImage.ImageUrl = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & length.ToString() & "&Height=" & length.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(randomImage.Folder & randomImage.FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1")
                                            Else
                                                objImage.ImageUrl = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & length.ToString() & "&Height=" & length.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(randomImage.Folder & randomImage.FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1&s=1")
                                            End If
                                            objImage.EnableViewState = False
                                            objImage.AlternateText = objArticle.Title
                                            objPlaceHolder.Add(objImage)
                                        Else

                                            Dim arr() As String = val.Split(":"c)

                                            If (arr.Length = 2) Then
                                                Dim width As Integer = Convert.ToInt32(val.Split(":"c)(0))
                                                Dim height As Integer = Convert.ToInt32(val.Split(":"c)(1))

                                                Dim objImage As New Image
                                                If (ArticleSettings.ImageThumbnailType = ThumbnailType.Proportion) Then
                                                    objImage.ImageUrl = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(randomImage.Folder & randomImage.FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1")
                                                Else
                                                    objImage.ImageUrl = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(randomImage.Folder & randomImage.FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1&s=1")
                                                End If
                                                objImage.EnableViewState = False
                                                objImage.AlternateText = objArticle.Title
                                                objPlaceHolder.Add(objImage)
                                            End If
                                        End If

                                    End If

                                End If
                                Exit Select
                            End If


                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("ISINROLE:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(9, layoutArray(iPtr + 1).Length - 9)
                                If (PortalSecurity.IsInRole(field) = False) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/ISINROLE:")) Then
                                ' Do Nothing
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("ISITEMINDEX:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(12, layoutArray(iPtr + 1).Length - 12)
                                Try
                                    If (Convert.ToInt32(field) <> articleItemIndex) Then
                                        Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                        While (iPtr < layoutArray.Length - 1)
                                            If (layoutArray(iPtr + 1) = endToken) Then
                                                Exit While
                                            End If
                                            iPtr = iPtr + 1
                                        End While
                                    End If
                                Catch
                                End Try
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/ISITEMINDEX:")) Then
                                ' Do Nothing
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("ISPAGE:")) Then
                                Dim page As String = layoutArray(iPtr + 1).Substring(7, layoutArray(iPtr + 1).Length - 7)
                                If (IsNumeric(page)) Then
                                    If (Convert.ToInt32(page) = 1) Then
                                        If (Request("PageID") <> "") Then
                                            Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                            While (iPtr < layoutArray.Length - 1)
                                                If (layoutArray(iPtr + 1) = endToken) Then
                                                    Exit While
                                                End If
                                                iPtr = iPtr + 1
                                            End While
                                        End If
                                    Else
                                        Dim objPageController As New PageController()
                                        Dim objPages As ArrayList = objPageController.GetPageList(objArticle.ArticleID)

                                        Dim found As Boolean = False
                                        If (Request("PageID") <> "") Then
                                            Dim pageNumber As Integer = 1
                                            For Each objPage As PageInfo In objPages
                                                If (Convert.ToInt32(page) = pageNumber) Then
                                                    If (Request("PageID") = objPage.PageID.ToString()) Then
                                                        found = True
                                                    End If
                                                    Exit For
                                                End If
                                                pageNumber = pageNumber + 1
                                            Next
                                        End If

                                        If (found = False) Then
                                            Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                            While (iPtr < layoutArray.Length - 1)
                                                If (layoutArray(iPtr + 1) = endToken) Then
                                                    Exit While
                                                End If
                                                iPtr = iPtr + 1
                                            End While
                                        End If
                                    End If
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/ISPAGE:")) Then
                                ' Do Nothing
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("ISNOTPAGE:")) Then
                                Dim page As String = layoutArray(iPtr + 1).Substring(10, layoutArray(iPtr + 1).Length - 10)
                                If (IsNumeric(page)) Then
                                    If (Convert.ToInt32(page) = 1) Then
                                        If (Request("PageID") = "") Then
                                            Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                            While (iPtr < layoutArray.Length - 1)
                                                If (layoutArray(iPtr + 1) = endToken) Then
                                                    Exit While
                                                End If
                                                iPtr = iPtr + 1
                                            End While
                                        End If
                                    Else
                                        Dim objPageController As New PageController()
                                        Dim objPages As ArrayList = objPageController.GetPageList(objArticle.ArticleID)

                                        Dim found As Boolean = False
                                        If (Request("PageID") <> "") Then
                                            Dim pageNumber As Integer = 1
                                            For Each objPage As PageInfo In objPages
                                                If (Convert.ToInt32(page) = pageNumber) Then
                                                    If (Request("PageID") = objPage.PageID.ToString()) Then
                                                        found = True
                                                    End If
                                                    Exit For
                                                End If
                                                pageNumber = pageNumber + 1
                                            Next
                                        End If

                                        If (found = True) Then
                                            Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                            While (iPtr < layoutArray.Length - 1)
                                                If (layoutArray(iPtr + 1) = endToken) Then
                                                    Exit While
                                                End If
                                                iPtr = iPtr + 1
                                            End While
                                        End If
                                    End If
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/ISNOTPAGE:")) Then
                                ' Do Nothing
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("HASCATEGORY:")) Then
                                Dim category As String = layoutArray(iPtr + 1).Substring(12, layoutArray(iPtr + 1).Length - 12)

                                Dim objArticleCategories As ArrayList = CType(DataCache.GetCache(ArticleConstants.CACHE_CATEGORY_ARTICLE & objArticle.ArticleID.ToString()), ArrayList)
                                If (objArticleCategories Is Nothing) Then
                                    Dim objArticleController As New ArticleController
                                    objArticleCategories = objArticleController.GetArticleCategories(objArticle.ArticleID)
                                    DataCache.SetCache(ArticleConstants.CACHE_CATEGORY_ARTICLE & objArticle.ArticleID.ToString(), objArticleCategories)
                                End If

                                Dim found As Boolean = False
                                If (category <> "") Then
                                    For Each objCategory As CategoryInfo In objArticleCategories
                                        If (category.ToLower() = objCategory.Name.ToLower()) Then
                                            found = True
                                        End If
                                    Next
                                End If

                                If (found = False) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/HASCATEGORY:")) Then
                                ' Do Nothing
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("HASTAG:")) Then

                                Dim tagSelected As String = layoutArray(iPtr + 1).Substring(7, layoutArray(iPtr + 1).Length - 7)

                                Dim found As Boolean = False
                                If (objArticle.Tags.Trim() <> "") Then
                                    For Each tag As String In objArticle.Tags.Split(","c)
                                        If (tag.ToLower() = tagSelected.ToLower()) Then
                                            found = True
                                        End If
                                    Next
                                End If

                                If (found = False) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/HASTAG:")) Then
                                ' Do Nothing
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("ISLOCALE:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(9, layoutArray(iPtr + 1).Length - 9)

                                If (CType(Page, DotNetNuke.Framework.PageBase).PageCulture.Name.ToLower() <> field.ToLower()) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/ISLOCALE:")) Then
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("PAGE:")) Then
                                If (IsNumeric(layoutArray(iPtr + 1).Substring(5, layoutArray(iPtr + 1).Length - 5))) Then
                                    Dim pageNumber As Integer = Convert.ToInt32(layoutArray(iPtr + 1).Substring(5, layoutArray(iPtr + 1).Length - 5))

                                    Dim objLiteral As New Literal
                                    If (pageNumber > 0) Then
                                        Dim pageController As New PageController
                                        Dim pageList As ArrayList = pageController.GetPageList(objArticle.ArticleID)

                                        If (pageList.Count >= pageNumber) Then
                                            objLiteral.Text = ProcessPostTokens(Server.HtmlDecode(CType(pageList(pageNumber - 1), PageInfo).PageText), objArticle, Generator, ArticleSettings)
                                        End If
                                    End If
                                    objLiteral.EnableViewState = False
                                    objPlaceHolder.Add(objLiteral)
                                    Exit Select
                                End If
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("PUBLISHENDDATE:")) Then
                                Dim formatExpression As String = layoutArray(iPtr + 1).Substring(15, layoutArray(iPtr + 1).Length - 15)

                                Dim objLiteral As New Literal

                                Try
                                    If (objArticle.EndDate = Null.NullDate) Then
                                        objLiteral.Text = ""
                                    Else
                                        objLiteral.Text = objArticle.EndDate.ToString(formatExpression)
                                    End If
                                Catch
                                    If (objArticle.EndDate = Null.NullDate) Then
                                        objLiteral.Text = ""
                                    Else
                                        objLiteral.Text = objArticle.EndDate.ToString("D")
                                    End If
                                End Try

                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("PUBLISHDATE:")) Then
                                Dim formatExpression As String = layoutArray(iPtr + 1).Substring(12, layoutArray(iPtr + 1).Length - 12)

                                Dim objLiteral As New Literal

                                Try
                                    If (objArticle.StartDate = Null.NullDate) Then
                                        objLiteral.Text = objArticle.CreatedDate.ToString(formatExpression)
                                    Else
                                        objLiteral.Text = objArticle.StartDate.ToString(formatExpression)
                                    End If
                                Catch
                                    If (objArticle.StartDate = Null.NullDate) Then
                                        objLiteral.Text = objArticle.CreatedDate.ToString("D")
                                    Else
                                        objLiteral.Text = objArticle.StartDate.ToString("D")
                                    End If
                                End Try

                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("PUBLISHSTARTDATE:")) Then
                                Dim formatExpression As String = layoutArray(iPtr + 1).Substring(17, layoutArray(iPtr + 1).Length - 17)

                                Dim objLiteral As New Literal

                                Try
                                    If (objArticle.StartDate = Null.NullDate) Then
                                        objLiteral.Text = objArticle.CreatedDate.ToString(formatExpression)
                                    Else
                                        objLiteral.Text = objArticle.StartDate.ToString(formatExpression)
                                    End If
                                Catch
                                    If (objArticle.StartDate = Null.NullDate) Then
                                        objLiteral.Text = objArticle.CreatedDate.ToString("D")
                                    Else
                                        objLiteral.Text = objArticle.StartDate.ToString("D")
                                    End If
                                End Try

                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("QUERYSTRING:")) Then
                                Dim variable As String = layoutArray(iPtr + 1).Substring(12, layoutArray(iPtr + 1).Length - 12)

                                Dim objLiteral As New Literal
                                objLiteral.Text = Request.QueryString(variable)
                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("RELATED:")) Then
                                _objRelatedArticles = Nothing
                                If (IsNumeric(layoutArray(iPtr + 1).Substring(8, layoutArray(iPtr + 1).Length - 8))) Then
                                    Dim count As Integer = Convert.ToInt32(layoutArray(iPtr + 1).Substring(8, layoutArray(iPtr + 1).Length - 8))
                                    If (count > 0) Then
                                        If (ArticleSettings.RelatedMode <> RelatedType.None) Then
                                            Dim phRelated As New PlaceHolder
                                            Dim objArticles As List(Of ArticleInfo) = GetRelatedArticles(objArticle, count)
                                            If (objArticles.Count > 0) Then
                                                Dim _objLayoutRelatedHeader As LayoutInfo = GetLayout(ArticleSettings, ArticleModule, Page, LayoutType.Related_Header_Html)
                                                Dim _objLayoutRelatedItem As LayoutInfo = GetLayout(ArticleSettings, ArticleModule, Page, LayoutType.Related_Item_Html)
                                                Dim _objLayoutRelatedFooter As LayoutInfo = GetLayout(ArticleSettings, ArticleModule, Page, LayoutType.Related_Footer_Html)

                                                ProcessArticleItem(phRelated.Controls, _objLayoutRelatedHeader.Tokens, objArticle)
                                                For Each objRelatedArticle As ArticleInfo In objArticles
                                                    ProcessArticleItem(phRelated.Controls, _objLayoutRelatedItem.Tokens, objRelatedArticle)
                                                Next
                                                ProcessArticleItem(phRelated.Controls, _objLayoutRelatedFooter.Tokens, objArticle)

                                                objPlaceHolder.Add(phRelated)
                                            End If
                                        End If
                                    End If
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("SUMMARY:")) Then
                                Dim summary As String = objArticle.Summary
                                If (IsNumeric(layoutArray(iPtr + 1).Substring(8, layoutArray(iPtr + 1).Length - 8))) Then
                                    Dim length As Integer = Convert.ToInt32(layoutArray(iPtr + 1).Substring(8, layoutArray(iPtr + 1).Length - 8))
                                    If (StripHtml(Server.HtmlDecode(objArticle.Summary)).TrimStart().Length > length) Then
                                        summary = ProcessPostTokens(Left(StripHtml(Server.HtmlDecode(objArticle.Summary)).TrimStart(), length), objArticle, Generator, ArticleSettings) & "..."
                                    Else
                                        summary = ProcessPostTokens(Left(StripHtml(Server.HtmlDecode(objArticle.Summary)).TrimStart(), length), objArticle, Generator, ArticleSettings)
                                    End If
                                End If

                                Dim objLiteral As New Literal
                                objLiteral.Text = summary
                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("TITLE:")) Then
                                Dim title As String = objArticle.Title
                                If (IsNumeric(layoutArray(iPtr + 1).Substring(6, layoutArray(iPtr + 1).Length - 6))) Then
                                    Dim length As Integer = Convert.ToInt32(layoutArray(iPtr + 1).Substring(6, layoutArray(iPtr + 1).Length - 6))
                                    If (objArticle.Title.Length > length) Then
                                        title = Left(objArticle.Title, length) & "..."
                                    Else
                                        title = Left(objArticle.Title, length)
                                    End If
                                End If

                                Dim objLiteral As New Literal
                                objLiteral.Text = title
                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("UPDATEDATE:")) Then
                                Dim formatExpression As String = layoutArray(iPtr + 1).Substring(11, layoutArray(iPtr + 1).Length - 11)

                                Dim objLiteral As New Literal

                                Try
                                    If (objArticle.CreatedDate = Null.NullDate) Then
                                        objLiteral.Text = ""
                                    Else
                                        objLiteral.Text = objArticle.LastUpdate.ToString(formatExpression)
                                    End If
                                Catch
                                    If (objArticle.CreatedDate = Null.NullDate) Then
                                        objLiteral.Text = ""
                                    Else
                                        objLiteral.Text = objArticle.LastUpdate.ToString("D")
                                    End If
                                End Try

                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("UPDATEDATELESSTHAN:")) Then
                                Dim length As Integer = Convert.ToInt32(layoutArray(iPtr + 1).Substring(19, layoutArray(iPtr + 1).Length - 19))

                                If (objArticle.LastUpdate < DateTime.Now.AddDays(length * -1)) Then
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = "/UPDATEDATELESSTHAN") Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/UPDATEDATELESSTHAN:")) Then
                                Exit Select
                            End If

                            Dim objLiteralOther As New Literal
                            objLiteralOther.Text = "[" & layoutArray(iPtr + 1) & "]"
                            objLiteralOther.EnableViewState = False
                            objPlaceHolder.Add(objLiteralOther)

                    End Select
                End If

            Next

        End Sub

#End Region

#Region " Process Comment Item "

        Dim commentItemIndex As Integer = 0
        Public Sub ProcessComment(ByRef objPlaceHolder As ControlCollection, ByVal objArticle As ArticleInfo, ByVal objComment As CommentInfo, ByVal templateArray As String())

            commentItemIndex = commentItemIndex + 1
            Dim isAnonymous As Boolean = Null.IsNull(objComment.UserID)

            For iPtr As Integer = 0 To templateArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(ProcessImages(templateArray(iPtr).ToString())))

                If iPtr < templateArray.Length - 1 Then
                    Select Case templateArray(iPtr + 1)
                        Case "ANONYMOUSURL"
                            Dim objLiteral As New Literal
                            objLiteral.Text = AddHTTP(objComment.AnonymousURL)
                            objPlaceHolder.Add(objLiteral)
                        Case "ARTICLETITLE"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objArticle.Title
                            objPlaceHolder.Add(objLiteral)
                        Case "AUTHOREMAIL"
                            Dim objLiteral As New Literal
                            If (objComment.UserID = Null.NullInteger) Then
                                objLiteral.Text = objComment.AnonymousEmail.ToString()
                            Else
                                objLiteral.Text = objComment.AuthorEmail.ToString()
                            End If
                            objPlaceHolder.Add(objLiteral)
                        Case "AUTHOR"
                            Dim objLiteral As New Literal
                            Select Case ArticleSettings.DisplayMode
                                Case DisplayType.FirstName
                                    If (isAnonymous) Then
                                        If (objComment.AnonymousName <> "") Then
                                            objLiteral.Text = objComment.AnonymousName
                                        Else
                                            objLiteral.Text = GetArticleResource("AnonymousFirstName")
                                        End If
                                    Else
                                        objLiteral.Text = objComment.AuthorFirstName
                                    End If
                                    Exit Select
                                Case DisplayType.LastName
                                    If (isAnonymous) Then
                                        If (objComment.AnonymousName <> "") Then
                                            objLiteral.Text = objComment.AnonymousName
                                        Else
                                            objLiteral.Text = GetArticleResource("AnonymousLastName")
                                        End If
                                    Else
                                        objLiteral.Text = objComment.AuthorLastName
                                    End If
                                    Exit Select
                                Case DisplayType.UserName
                                    If (isAnonymous) Then
                                        If (objComment.AnonymousName <> "") Then
                                            objLiteral.Text = objComment.AnonymousName
                                        Else
                                            objLiteral.Text = GetArticleResource("AnonymousUserName")
                                        End If
                                    Else
                                        objLiteral.Text = objComment.AuthorUserName
                                    End If
                                    Exit Select
                                Case DisplayType.FullName
                                    If (isAnonymous) Then
                                        If (objComment.AnonymousName <> "") Then
                                            objLiteral.Text = objComment.AnonymousName
                                        Else
                                            objLiteral.Text = GetArticleResource("AnonymousFullName")
                                        End If
                                    Else
                                        objLiteral.Text = objComment.AuthorDisplayName
                                    End If
                                    Exit Select
                                Case Else
                                    If (isAnonymous) Then
                                        If (objComment.AnonymousName <> "") Then
                                            objLiteral.Text = objComment.AnonymousName
                                        Else
                                            objLiteral.Text = GetArticleResource("AnonymousUserName")
                                        End If
                                    Else
                                        objLiteral.Text = objComment.AuthorUserName
                                    End If
                                    Exit Select
                            End Select
                            objPlaceHolder.Add(objLiteral)
                        Case "AUTHORID"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.UserID.ToString()
                            objPlaceHolder.Add(objLiteral)
                        Case "AUTHORUSERNAME"
                            Dim objLiteral As New Literal
                            If (objComment.UserID = Null.NullInteger) Then
                                If (objComment.AnonymousName <> "") Then
                                    objLiteral.Text = objComment.AnonymousName
                                Else
                                    objLiteral.Text = GetArticleResource("AnonymousUserName")
                                End If
                            Else
                                objLiteral.Text = objComment.AuthorUserName.ToString()
                            End If
                            objPlaceHolder.Add(objLiteral)
                        Case "AUTHORDISPLAYNAME"
                            Dim objLiteral As New Literal
                            If (objComment.UserID = Null.NullInteger) Then
                                If (objComment.AnonymousName <> "") Then
                                    objLiteral.Text = objComment.AnonymousName
                                Else
                                    objLiteral.Text = GetArticleResource("AnonymousFullName")
                                End If
                            Else
                                objLiteral.Text = objComment.AuthorDisplayName.ToString()
                            End If
                            objPlaceHolder.Add(objLiteral)
                        Case "AUTHORFIRSTNAME"
                            Dim objLiteral As New Literal
                            If (objComment.UserID = Null.NullInteger) Then
                                If (objComment.AnonymousName <> "") Then
                                    objLiteral.Text = objComment.AnonymousName
                                Else
                                    objLiteral.Text = GetArticleResource("AnonymousFirstName")
                                End If
                            Else
                                objLiteral.Text = objComment.AuthorFirstName.ToString()
                            End If
                            objPlaceHolder.Add(objLiteral)
                        Case "AUTHORLASTNAME"
                            Dim objLiteral As New Literal
                            If (objComment.UserID = Null.NullInteger) Then
                                If (objComment.AnonymousName <> "") Then
                                    objLiteral.Text = objComment.AnonymousName
                                Else
                                    objLiteral.Text = GetArticleResource("AnonymousLastName")
                                End If
                            Else
                                objLiteral.Text = objComment.AuthorLastName.ToString()
                            End If
                            objPlaceHolder.Add(objLiteral)
                        Case "AUTHORFULLNAME"
                            Dim objLiteral As New Literal
                            If (objComment.UserID = Null.NullInteger) Then
                                If (objComment.AnonymousName <> "") Then
                                    objLiteral.Text = objComment.AnonymousName
                                Else
                                    objLiteral.Text = GetArticleResource("AnonymousFullName")
                                End If
                            Else
                                objLiteral.Text = objComment.AuthorFirstName.ToString() & " " & objComment.AuthorLastName.ToString()
                            End If
                            objPlaceHolder.Add(objLiteral)
                        Case "COMMENTID"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.CommentID.ToString()
                            objPlaceHolder.Add(objLiteral)
                        Case "COMMENT"
                            Dim objLiteral As New Literal
                            objLiteral.Text = ProcessPostTokens(EncodeComment(objComment), objArticle, Nothing, ArticleSettings)
                            objPlaceHolder.Add(objLiteral)
                        Case "COMMENTLINK"
                            Dim objLiteral As New Literal
                            objLiteral.Text = Common.GetArticleLink(objArticle, Tab, ArticleSettings, IncludeCategory) & "#Comments"
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)
                        Case "CREATEDATE"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.CreatedDate.ToString("D")
                            objPlaceHolder.Add(objLiteral)
                        Case "CREATETIME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.CreatedDate.ToString("t")
                            objPlaceHolder.Add(objLiteral)
                        Case "DELETE"
                            If (ArticleSettings.IsAdmin() Or ArticleSettings.IsApprover() Or (UserId = objArticle.AuthorID And objArticle.AuthorID <> Null.NullInteger)) Then
                                Dim cmdDelete As New LinkButton
                                cmdDelete.CssClass = "CommandButton"
                                cmdDelete.Text = GetArticleResource("Delete")
                                cmdDelete.Attributes.Add("onClick", "javascript:return confirm('Are You Sure You Wish To Delete This Item ?');")
                                cmdDelete.CommandArgument = objComment.CommentID.ToString()
                                cmdDelete.CommandName = "DeleteComment"
                                Dim objHandler As New System.Web.UI.WebControls.CommandEventHandler(AddressOf Comment_Command)
                                AddHandler cmdDelete.Command, objHandler
                                objPlaceHolder.Add(cmdDelete)
                            End If
                        Case "EDIT"
                            If (ArticleSettings.IsAdmin() Or ArticleSettings.IsApprover() Or (UserId = objArticle.AuthorID And objArticle.AuthorID <> Null.NullInteger)) Then
                                Dim objHyperLink As New HyperLink
                                objHyperLink.CssClass = "CommandButton"
                                objHyperLink.Text = GetArticleResource("Edit")
                                objHyperLink.NavigateUrl = Common.GetModuleLink(ArticleModule.TabID, ArticleModule.ModuleID, "EditComment", ArticleSettings, "CommentID=" & objComment.CommentID.ToString(), "ReturnUrl=" & Server.UrlEncode(Request.RawUrl))
                                objHyperLink.EnableViewState = False
                                objPlaceHolder.Add(objHyperLink)
                            End If
                        Case "GRAVATARURL"
                            Dim objLiteral As New Literal
                            If Request.IsSecureConnection Then
                                If (objComment.UserID = Null.NullInteger) Then
                                    objLiteral.Text = AddHTTP("secure.gravatar.com/avatar/" & MD5CalcString(objComment.AnonymousEmail.ToLower()))
                                Else
                                    objLiteral.Text = AddHTTP("secure.gravatar.com/avatar/" & MD5CalcString(objComment.AuthorEmail.ToLower()))
                                End If
                            Else
                                If (objComment.UserID = Null.NullInteger) Then
                                    objLiteral.Text = AddHTTP("www.gravatar.com/avatar/" & MD5CalcString(objComment.AnonymousEmail.ToLower()))
                                Else
                                    objLiteral.Text = AddHTTP("www.gravatar.com/avatar/" & MD5CalcString(objComment.AuthorEmail.ToLower()))
                                End If
                            End If
                            objPlaceHolder.Add(objLiteral)
                        Case "HASANONYMOUSURL"
                            If (objComment.AnonymousURL = "") Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/HASANONYMOUSURL") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASANONYMOUSURL"
                            ' Do Nothing
                        Case "IPADDRESS"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.RemoteAddress
                            objPlaceHolder.Add(objLiteral)
                        Case "ISANONYMOUS"
                            If (objComment.UserID <> -1) Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/ISANONYMOUS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISANONYMOUS"
                            ' Do Nothing
                        Case "ISNOTANONYMOUS"
                            If (objComment.UserID = -1) Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/ISNOTANONYMOUS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISNOTANONYMOUS"
                            ' Do Nothing
                        Case "ISCOMMENT"
                            If (objComment.Type <> 0) Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/ISCOMMENT") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISCOMMENT"
                            ' Do Nothing
                        Case "ISPINGBACK"
                            If (objComment.Type <> 2) Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/ISPINGBACK") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISTRACKBACK"
                            ' Do Nothing
                        Case "ISTRACKBACK"
                            If (objComment.Type <> 1) Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/ISTRACKBACK") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISTRACKBACK"
                            ' Do Nothing
                        Case "ISAUTHOR"
                            If (objComment.UserID = Null.NullInteger Or (objComment.UserID <> objArticle.AuthorID)) Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/ISAUTHOR") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISAUTHOR"
                            ' Do Nothing
                        Case "ITEMINDEX"
                            Dim objLiteral As New Literal
                            objLiteral.Text = Me.commentItemIndex.ToString()
                            objPlaceHolder.Add(objLiteral)
                        Case "MODULEID"
                            Dim objLiteral As New Literal
                            objLiteral.Text = ArticleModule.ModuleID
                            objPlaceHolder.Add(objLiteral)
                        Case "PINGBACKURL"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.TrackbackUrl
                            objPlaceHolder.Add(objLiteral)
                        Case "TRACKBACKBLOGNAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.TrackbackBlogName
                            objPlaceHolder.Add(objLiteral)
                        Case "TRACKBACKEXCERPT"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.TrackbackExcerpt
                            objPlaceHolder.Add(objLiteral)
                        Case "TRACKBACKTITLE"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.TrackbackTitle
                            objPlaceHolder.Add(objLiteral)
                        Case "TRACKBACKURL"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.TrackbackUrl
                            objPlaceHolder.Add(objLiteral)

                        Case Else
                            If (templateArray(iPtr + 1).ToUpper().StartsWith("AUTHOR:")) Then
                                If Author(objComment.UserID) IsNot Nothing Then
                                    ' token to be processed
                                    Dim field As String = templateArray(iPtr + 1).Substring(7, templateArray(iPtr + 1).Length - 7).ToLower().Trim()

                                    'Gets the DNN profile property named like the token (field)
                                    Dim profilePropertyFound As Boolean = False
                                    Dim profilePropertyDataType As String = String.Empty
                                    Dim profilePropertyName As String = String.Empty
                                    Dim profilePropertyValue As String = String.Empty

                                    For Each objProfilePropertyDefinition As ProfilePropertyDefinition In ProfileProperties
                                        If (objProfilePropertyDefinition.PropertyName.ToLower().Trim() = field) Then

                                            'Gets the dnn profile property's datatype
                                            Dim objListController As New ListController
                                            Dim definitionEntry As ListEntryInfo = objListController.GetListEntryInfo(objProfilePropertyDefinition.DataType)
                                            If Not definitionEntry Is Nothing Then
                                                profilePropertyDataType = definitionEntry.Value
                                            Else
                                                profilePropertyDataType = "Unknown"
                                            End If

                                            'Gets the dnn profile property's name and current value for the given user (Agent = AuthorID)
                                            profilePropertyName = objProfilePropertyDefinition.PropertyName
                                            profilePropertyValue = Author(objComment.UserID).Profile.GetPropertyValue(profilePropertyName)

                                            profilePropertyFound = True

                                        End If
                                    Next

                                    If profilePropertyFound Then

                                        Select Case profilePropertyDataType.ToLower()
                                            Case "truefalse"
                                                Dim objTrueFalse As New CheckBox
                                                If profilePropertyValue = String.Empty Then
                                                    objTrueFalse.Checked = False
                                                Else
                                                    objTrueFalse.Checked = CType(profilePropertyValue, Boolean)
                                                End If
                                                objTrueFalse.Enabled = False
                                                objTrueFalse.EnableViewState = False
                                                objPlaceHolder.Add(objTrueFalse)

                                            Case "richtext"
                                                Dim objLiteral As New Literal
                                                If profilePropertyValue = String.Empty Then
                                                    objLiteral.Text = String.Empty
                                                Else
                                                    objLiteral.Text = Page.Server.HtmlDecode(profilePropertyValue)
                                                End If
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)

                                            Case "list"
                                                Dim objLiteral As New Literal
                                                objLiteral.Text = profilePropertyValue
                                                Dim objListController As New ListController
                                                Dim objListEntryInfoCollection As IEnumerable(Of ListEntryInfo) = objListController.GetListEntryInfoItems(profilePropertyName)
                                                For Each objListEntryInfo As ListEntryInfo In objListEntryInfoCollection
                                                    If objListEntryInfo.Value = profilePropertyValue Then
                                                        objLiteral.Text = objListEntryInfo.Text
                                                        Exit For
                                                    End If
                                                Next
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)

                                            Case Else
                                                Dim objLiteral As New Literal
                                                If profilePropertyValue = String.Empty Then
                                                    objLiteral.Text = String.Empty
                                                Else
                                                    If profilePropertyName.ToLower() = "website" Then
                                                        Dim url As String = profilePropertyValue
                                                        If url.ToLower.StartsWith("http://") Then
                                                            url = url.Substring(7) ' removes the "http://"
                                                        End If
                                                        objLiteral.Text = url
                                                    Else
                                                        objLiteral.Text = profilePropertyValue
                                                    End If
                                                End If
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)
                                        End Select 'profilePropertyDataType

                                    End If ' DNN Profile property processing
                                End If
                                Exit Select
                            End If ' "AUTHOR:" token

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("CREATEDATE:")) Then
                                Dim formatExpression As String = templateArray(iPtr + 1).Substring(11, templateArray(iPtr + 1).Length - 11)
                                Dim objLiteral As New Literal
                                objLiteral.Text = objComment.CreatedDate.ToString(formatExpression)
                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("COMMENT:")) Then
                                Dim count As String = templateArray(iPtr + 1).Substring(8, templateArray(iPtr + 1).Length - 8)
                                If (IsNumeric(count)) Then
                                    Dim comment As String = objComment.Comment
                                    If (StripHtml(objComment.Comment).TrimStart().Length > Convert.ToInt32(count)) Then
                                        comment = Left(StripHtml(Server.HtmlDecode(objComment.Comment)).TrimStart(), Convert.ToInt32(count)) & "..."
                                    End If
                                    Dim objLiteral As New Literal
                                    objLiteral.Text = comment
                                    objLiteral.EnableViewState = False
                                    objPlaceHolder.Add(objLiteral)
                                End If
                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("ISINROLE:")) Then
                                Dim field As String = templateArray(iPtr + 1).Substring(9, templateArray(iPtr + 1).Length - 9)
                                If (PortalSecurity.IsInRole(field) = False) Then
                                    Dim endToken As String = "/" & templateArray(iPtr + 1)
                                    While (iPtr < templateArray.Length - 1)
                                        If (templateArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                            End If

                    End Select
                End If

            Next

        End Sub

        Public Sub ProcessFile(ByRef objPlaceHolder As ControlCollection, ByVal objArticle As ArticleInfo, ByVal objFile As FileInfo, ByVal templateArray As String())

            For iPtr As Integer = 0 To templateArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(ProcessImages(templateArray(iPtr).ToString())))

                If iPtr < templateArray.Length - 1 Then
                    Select Case templateArray(iPtr + 1)

                        Case "ARTICLEID"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objFile.ArticleID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "FILEID"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objFile.FileID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "FILENAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objFile.FileName.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "FILELINK"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objFile.Link
                            objPlaceHolder.Add(objLiteral)

                        Case "SIZE"
                            Dim objLiteral As New Literal
                            objLiteral.Text = Numeric2Bytes(objFile.Size)
                            objPlaceHolder.Add(objLiteral)

                        Case "SORTORDER"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objFile.SortOrder.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "TITLE"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objFile.Title
                            objPlaceHolder.Add(objLiteral)

                        Case Else

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("ISEXTENSION:")) Then
                                Dim field As String = templateArray(iPtr + 1).Substring(12, templateArray(iPtr + 1).Length - 12)

                                If (objFile.FileName.ToUpper().EndsWith(field.ToUpper()) = False) Then
                                    Dim endToken As String = "/" & templateArray(iPtr + 1)
                                    While (iPtr < templateArray.Length - 1)
                                        If (templateArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("ISNOTEXTENSION:")) Then
                                Dim field As String = templateArray(iPtr + 1).Substring(15, templateArray(iPtr + 1).Length - 15)

                                If (objFile.FileName.ToUpper().EndsWith(field.ToUpper()) = True) Then
                                    Dim endToken As String = "/" & templateArray(iPtr + 1)
                                    While (iPtr < templateArray.Length - 1)
                                        If (templateArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                            End If

                    End Select
                End If

            Next

        End Sub

        Private imageIndex As Integer = 0
        Public Sub ProcessImage(ByRef objPlaceHolder As ControlCollection, ByVal objArticle As ArticleInfo, ByVal objImage As ImageInfo, ByVal templateArray As String())

            imageIndex = imageIndex + 1

            For iPtr As Integer = 0 To templateArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(ProcessImages(templateArray(iPtr).ToString()).Replace("{|", "[").Replace("|}", "]")))

                If iPtr < templateArray.Length - 1 Then
                    Select Case templateArray(iPtr + 1)

                        Case "ARTICLEID"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objImage.ArticleID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "DESCRIPTION"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objImage.Description
                            objPlaceHolder.Add(objLiteral)

                        Case "FILENAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objImage.FileName.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "HEIGHT"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objImage.Height.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "IMAGEID"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objImage.ImageID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "IMAGELINK"
                            Dim objLiteral As New Literal
                            objLiteral.Text = PortalSettings.HomeDirectory & objImage.Folder & objImage.FileName
                            objPlaceHolder.Add(objLiteral)

                        Case "ITEMINDEX"
                            Dim objLiteral As New Literal
                            objLiteral.Text = imageIndex.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "SIZE"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objImage.Size.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "SORTORDER"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objImage.SortOrder.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "TITLE"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objImage.Title
                            objPlaceHolder.Add(objLiteral)

                        Case "WIDTH"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objImage.Width.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case Else

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("IMAGETHUMB:")) Then
                                Dim val As String = templateArray(iPtr + 1).Substring(11, templateArray(iPtr + 1).Length - 11)
                                If (val.IndexOf(":"c) <> -1) Then
                                    Dim width As Integer = Convert.ToInt32(val.Split(":"c)(0))
                                    Dim height As Integer = Convert.ToInt32(val.Split(":"c)(1))

                                    Dim objImageItem As New Image
                                    If (ArticleSettings.ImageThumbnailType = ThumbnailType.Proportion) Then
                                        objImageItem.ImageUrl = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(objImage.Folder & objImage.FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1")
                                    Else
                                        objImageItem.ImageUrl = ArticleUtilities.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & width.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory) & "&FileName=" & Server.UrlEncode(objImage.Folder & objImage.FileName) & "&PortalID=" & PortalSettings.PortalId.ToString() & "&q=1&s=1")
                                    End If
                                    objImageItem.EnableViewState = False
                                    objImageItem.AlternateText = objArticle.Title
                                    objPlaceHolder.Add(objImageItem)
                                End If
                                Exit Select
                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("ISITEMINDEX:")) Then
                                Dim field As String = templateArray(iPtr + 1).Substring(12, templateArray(iPtr + 1).Length - 12)
                                If (field <> imageIndex.ToString()) Then
                                    Dim endToken As String = "/" & templateArray(iPtr + 1)
                                    While (iPtr < templateArray.Length - 1)
                                        If (templateArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                Exit Select
                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("/ISITEMINDEX:")) Then
                                Exit Select
                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("ISNOTITEMINDEX:")) Then
                                Dim field As String = templateArray(iPtr + 1).Substring(15, templateArray(iPtr + 1).Length - 15)
                                If (field = imageIndex.ToString()) Then
                                    Dim endToken As String = "/" & templateArray(iPtr + 1)
                                    While (iPtr < templateArray.Length - 1)
                                        If (templateArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                Exit Select
                            End If


                            If (templateArray(iPtr + 1).ToUpper().StartsWith("/ISNOTITEMINDEX:")) Then
                                Exit Select
                            End If

                            Dim objLiteralOther As New Literal
                            objLiteralOther.Text = "[" & templateArray(iPtr + 1) & "]"
                            objLiteralOther.EnableViewState = False
                            objPlaceHolder.Add(objLiteralOther)

                    End Select
                End If

            Next

        End Sub

#End Region

#End Region

#Region " Event Handlers "

        Private Sub Comment_Command(ByVal sender As Object, ByVal e As CommandEventArgs)

            Select Case e.CommandName.ToLower()

                Case "deletecomment"
                    Dim objArticleController As New ArticleController()
                    Dim objArticle As ArticleInfo = objArticleController.GetArticle(Convert.ToInt32(Request("ArticleID")))

                    If (objArticle IsNot Nothing) Then
                        Dim objCommentController As New CommentController
                        objCommentController.DeleteComment(Convert.ToInt32(e.CommandArgument), objArticle.ArticleID)
                    End If

                    HttpContext.Current.Response.Redirect(Request.RawUrl, True)

            End Select

        End Sub

#End Region

    End Class

End Namespace
