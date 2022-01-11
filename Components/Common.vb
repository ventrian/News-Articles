'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.Globalization
Imports System.Threading
Imports System.Web

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.UI.Modules

Imports Ventrian.NewsArticles.Components.Utility
Imports DotNetNuke.Entities.Controllers
Imports DotNetNuke.Entities.Host
Imports DotNetNuke.Security

Namespace Ventrian.NewsArticles

    Public Class Common

#Region " Friend Shared Methods "

        Friend Shared Sub BindEnum(ByVal objListControl As ListControl, ByRef enumType As Type, ByVal resourceFile As String)

            For Each value As Integer In System.Enum.GetValues(enumType)
                Dim li As New ListItem
                li.Value = System.Enum.GetName(enumType, value)
                li.Text = Localization.GetString(System.Enum.GetName(enumType, value), resourceFile)
                objListControl.Items.Add(li)
            Next

        End Sub

        Friend Shared Function GetModuleInfo(moduleId As Integer, optional tabId As integer = -1) As ModuleInfo
            Dim mdlInfo As ModuleInfo = ModuleController.Instance.GetModule(moduleID, tabId, False)
            return mdlInfo  
        End Function
        Friend Shared Function GetModuleSettings(moduleId As integer) As Hashtable
            return GetModuleInfo(moduleId).ModuleSettings
        End Function
        Friend Shared Function GetTabModuleSettings(moduleId As integer, tabId As integer) As Hashtable
            return GetModuleInfo(moduleId, tabId).ModuleSettings
        End Function

#End Region

#Region " Public Shared Methods "


        Public Shared Function FormatTitle(ByVal title As String) As String

            If (title = "") Then
                Return "Default.aspx"
            End If

            Dim returnTitle As String = OnlyAlphaNumericChars(title, TitleReplacementType.Dash)
            If (returnTitle = "") Then
                Return "Default.aspx"
            End If

            If (returnTitle.Replace("-", "").Replace("_", "") = "") Then
                Return "Default.aspx"
            End If

            Return returnTitle.Replace("--", "-") & ".aspx"

        End Function

        Public Shared Function FormatTitle(ByVal title As String, ByVal objArticleSettings As ArticleSettings) As String

            If (title = "") Then
                Return "Default.aspx"
            End If

            Dim returnTitle As String = OnlyAlphaNumericChars(title, objArticleSettings.TitleReplacement)
            If (returnTitle = "") Then
                Return "Default.aspx"
            End If

            If (returnTitle.Replace("-", "").Replace("_", "") = "") Then
                Return "Default.aspx"
            End If

            Return returnTitle.Replace("--", "-") & ".aspx"

        End Function

        Public Shared Function OnlyAlphaNumericChars(ByVal OrigString As String, ByVal objReplacementType As TitleReplacementType) As String

            '***********************************************************
            'INPUT:  Any String
            'OUTPUT: The Input String with all non-alphanumeric characters 
            '        removed
            'EXAMPLE Debug.Print OnlyAlphaNumericChars("Hello World!")
            'output = "HelloWorld")
            'NOTES:  Not optimized for speed and will run slow on long
            '        strings.  If you plan on using long strings, consider 
            '        using alternative method of appending to output string,
            '        such as the method at
            '        http://www.freevbcode.com/ShowCode.Asp?ID=154
            '***********************************************************
            Dim lLen As Integer
            Dim sAns As String = ""
            Dim lCtr As Integer
            Dim sChar As String

            OrigString = RemoveDiacritics(Trim(OrigString))

            lLen = Len(OrigString)
            For lCtr = 1 To lLen
                sChar = Mid(OrigString, lCtr, 1)
                If IsAlphaNumeric(Mid(OrigString, lCtr, 1)) Or Mid(OrigString, lCtr, 1) = "-" Or Mid(OrigString, lCtr, 1) = "_" Then
                    sAns = sAns & sChar
                End If
            Next

            If (objReplacementType = TitleReplacementType.Dash) Then
                OnlyAlphaNumericChars = Replace(sAns, " ", "-")
            Else
                OnlyAlphaNumericChars = Replace(sAns, " ", "_")
            End If

        End Function

        Public Shared Function RemoveDiacritics(ByVal s As String) As String
            s = s.Normalize(System.Text.NormalizationForm.FormD)
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            Dim i As Integer
            For i = 0 To s.Length - 1
                If s(i) = ChrW(305) Then
                    sb.Append("i"c)
                Else
                    If CharUnicodeInfo.GetUnicodeCategory(s(i)) <> UnicodeCategory.NonSpacingMark Then
                        sb.Append(s(i))
                    End If
                End If
            Next
            Return sb.ToString()
        End Function
        
        Public Shared Function IsAlphaNumeric(ByVal sChr As String) As Boolean
            IsAlphaNumeric = sChr Like "[0-9A-Za-z ]"
        End Function

        Public Shared Function GetAdjustedUserTime(ByVal dateString As String, ByVal format As String, ByVal timeZone As Integer) As String

            Dim dateToFormat As DateTime = DateTime.Parse(dateString)

            Return dateToFormat.ToString(format)

        End Function

        Public Shared Function GetAdjustedUserTime(ByVal articleSettings As ArticleSettings, ByVal dateString As String, ByVal format As String) As String

            Return GetAdjustedUserTime(dateString, format, articleSettings.ServerTimeZone)

        End Function

        Public Shared Function GetAdjustedUserTime(ByVal articleSettings As ArticleSettings, ByVal objDateTime As DateTime) As DateTime

            Return objDateTime

        End Function

        Public Shared Function GetAdjustedServerTime(ByVal articleSettings As ArticleSettings, ByVal objDateTime As DateTime) As DateTime

            Return objDateTime

        End Function

        Public Shared Function GetArticleLink(ByVal objArticle As ArticleInfo, ByVal objTab As DotNetNuke.Entities.Tabs.TabInfo, ByVal articleSettings As ArticleSettings, ByVal includeCategory As Boolean) As String
            Return GetArticleLink(objArticle, objTab, articleSettings, includeCategory, Null.NullInteger)
        End Function

        Public Shared Function GetArticleLink(ByVal objArticle As ArticleInfo, ByVal objTab As DotNetNuke.Entities.Tabs.TabInfo, ByVal articleSettings As ArticleSettings, ByVal includeCategory As Boolean, ByVal pageID As Integer) As String

            If (objTab Is Nothing) Then
                Return ""
            End If

            If Host.UseFriendlyUrls Then

                Dim strURL As String = ApplicationURL(objTab.TabID)
                Dim settings As PortalSettings = PortalController.Instance.GetCurrentPortalSettings

                If (articleSettings.LaunchLinks) Then
                    strURL = strURL & "&ctl=ArticleView"
                    strURL = strURL & "&mid=" & objArticle.ModuleID.ToString
                    strURL = strURL & "&articleId=" & objArticle.ArticleID
                Else
                    If (articleSettings.UrlModeType = Components.Types.UrlModeType.Classic) Then
                        strURL = strURL & "&articleType=ArticleView"
                        strURL = strURL & "&articleId=" & objArticle.ArticleID
                    Else
                        strURL = strURL & "&" & articleSettings.ShortenedID & "=" & objArticle.ArticleID
                    End If
                End If

                If (articleSettings.AlwaysShowPageID) Then
                    If (pageID <> Null.NullInteger) Then
                        strURL = strURL & "&PageID=" & pageID.ToString()
                    End If
                End If

                If (includeCategory) Then
                    If (HttpContext.Current.Request("CategoryID") <> "") Then
                        strURL = strURL & "&categoryId=" & HttpContext.Current.Request("CategoryID")
                    End If
                End If

                If (articleSettings.AuthorUserIDFilter) Then
                    If (articleSettings.AuthorUserIDParam <> "") Then
                        If (HttpContext.Current.Request(articleSettings.AuthorUserIDParam) <> "") Then
                            strURL = strURL & "&" & articleSettings.AuthorUserIDParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUserIDParam)
                        End If
                    End If
                End If

                If (articleSettings.AuthorUsernameFilter) Then
                    If (articleSettings.AuthorUsernameParam <> "") Then
                        If (HttpContext.Current.Request(articleSettings.AuthorUsernameParam) <> "") Then
                            strURL = strURL & "&" & articleSettings.AuthorUsernameParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUsernameParam)
                        End If
                    End If
                End If

                Dim title As String = FormatTitle(objArticle.Title, articleSettings)

                Dim link As String = FriendlyUrl(objTab, strURL, title, settings)

                If (link.ToLower().StartsWith("http://") Or link.ToLower().StartsWith("https://")) Then
                    Return link
                Else
                    If (System.Web.HttpContext.Current.Request.Url.Port = 80) Then
                        Return AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & link)
                    Else
                        Return AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & ":" & System.Web.HttpContext.Current.Request.Url.Port.ToString() & link)
                    End If
                End If
            Else
                If (articleSettings.LaunchLinks) Then
                    Dim parameters As New List(Of String)
                    parameters.Add("mid=" & objArticle.ModuleID.ToString)
                    parameters.Add("ctl=ArticleView")
                    parameters.Add("articleId=" & objArticle.ArticleID)

                    If (articleSettings.AlwaysShowPageID) Then
                        If (pageID <> Null.NullInteger) Then
                            parameters.Add("PageID=" & pageID.ToString())
                        End If
                    End If

                    If (articleSettings.AuthorUserIDFilter) Then
                        If (articleSettings.AuthorUserIDParam <> "") Then
                            If (HttpContext.Current.Request(articleSettings.AuthorUserIDParam) <> "") Then
                                parameters.Add(articleSettings.AuthorUserIDParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUserIDParam))
                            End If
                        End If
                    End If

                    If (articleSettings.AuthorUsernameFilter) Then
                        If (articleSettings.AuthorUsernameParam <> "") Then
                            If (HttpContext.Current.Request(articleSettings.AuthorUsernameParam) <> "") Then
                                parameters.Add(articleSettings.AuthorUsernameParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUsernameParam))
                            End If
                        End If
                    End If

                    If (System.Web.HttpContext.Current.Request.Url.Port = 80) Then
                        Return AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & NavigateURL(objTab.TabID, Null.NullString, parameters.ToArray).Replace("&", "&amp;"))
                    Else
                        Return AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & ":" & System.Web.HttpContext.Current.Request.Url.Port.ToString() & NavigateURL(objTab.TabID, Null.NullString, parameters.ToArray).Replace("&", "&amp;"))
                    End If
                Else
                    Dim parameters As New List(Of String)

                    If (articleSettings.UrlModeType = Components.Types.UrlModeType.Classic) Then
                        parameters.Add("articleType=ArticleView")
                        parameters.Add("articleId=" & objArticle.ArticleID)
                    Else
                        parameters.Add(articleSettings.ShortenedID & "=" & objArticle.ArticleID)
                    End If

                    If (articleSettings.AlwaysShowPageID) Then
                        If (pageID <> Null.NullInteger) Then
                            parameters.Add("PageID=" & pageID.ToString())
                        End If
                    End If

                    If (articleSettings.AuthorUserIDFilter) Then
                        If (articleSettings.AuthorUserIDParam <> "") Then
                            If (HttpContext.Current.Request(articleSettings.AuthorUserIDParam) <> "") Then
                                parameters.Add(articleSettings.AuthorUserIDParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUserIDParam))
                            End If
                        End If
                    End If

                    If (articleSettings.AuthorUsernameFilter) Then
                        If (articleSettings.AuthorUsernameParam <> "") Then
                            If (HttpContext.Current.Request(articleSettings.AuthorUsernameParam) <> "") Then
                                parameters.Add(articleSettings.AuthorUsernameParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUsernameParam))
                            End If
                        End If
                    End If

                    If (System.Web.HttpContext.Current.Request.Url.Port = 80) Then
                        Return AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & NavigateURL(objTab.TabID, Null.NullString, parameters.ToArray))
                    Else
                        Return AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & ":" & System.Web.HttpContext.Current.Request.Url.Port.ToString() & NavigateURL(objTab.TabID, Null.NullString, parameters.ToArray))
                    End If
                End If

            End If

        End Function

        Public Shared Function GetArticleLink(ByVal objArticle As ArticleInfo, ByVal objTab As DotNetNuke.Entities.Tabs.TabInfo, ByVal articleSettings As ArticleSettings, ByVal includeCategory As Boolean, ByVal ParamArray additionalParameters As String()) As String
            Return GetArticleLink(objArticle, objTab, articleSettings, includeCategory, Null.NullInteger, additionalParameters)
        End Function

        Public Shared Function GetArticleLink(ByVal objArticle As ArticleInfo, ByVal objTab As DotNetNuke.Entities.Tabs.TabInfo, ByVal articleSettings As ArticleSettings, ByVal includeCategory As Boolean, ByVal pageID As Integer, ByVal ParamArray additionalParameters As String()) As String

            If HostController.Instance.GetString("UseFriendlyUrls") = "Y" Then

                Dim strURL As String = ApplicationURL(objTab.TabID)
                Dim settings As PortalSettings = PortalController.Instance.GetCurrentPortalSettings

                If (articleSettings.LaunchLinks) Then
                    strURL = strURL & "&ctl=ArticleView"
                    strURL = strURL & "&mid=" & objArticle.ModuleID.ToString
                    strURL = strURL & "&articleId=" & objArticle.ArticleID

                Else
                    If (articleSettings.UrlModeType = Components.Types.UrlModeType.Classic) Then
                        strURL = strURL & "&articleType=ArticleView"
                        strURL = strURL & "&articleId=" & objArticle.ArticleID
                    Else
                        strURL = strURL & "&" & articleSettings.ShortenedID & "=" & objArticle.ArticleID
                    End If
                End If

                If (articleSettings.AlwaysShowPageID) Then
                    If (pageID <> Null.NullInteger) Then

                        Dim found As Boolean = False
                        For Each param As String In additionalParameters
                            If (param.ToLower().StartsWith("pageid")) Then
                                found = True
                            End If
                        Next

                        If (found = False) Then
                            strURL = strURL & "&PageID=" & pageID.ToString()
                        End If

                    End If
                End If

                For Each param As String In additionalParameters
                    strURL = strURL & "&" & param
                Next

                If (articleSettings.AuthorUserIDFilter) Then
                    If (articleSettings.AuthorUserIDParam <> "") Then
                        If (HttpContext.Current.Request(articleSettings.AuthorUserIDParam) <> "") Then
                            strURL = strURL & "&" & articleSettings.AuthorUserIDParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUserIDParam)
                        End If
                    End If
                End If

                If (articleSettings.AuthorUsernameFilter) Then
                    If (articleSettings.AuthorUsernameParam <> "") Then
                        If (HttpContext.Current.Request(articleSettings.AuthorUsernameParam) <> "") Then
                            strURL = strURL & "&" & articleSettings.AuthorUsernameParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUsernameParam)
                        End If
                    End If
                End If

                Return FriendlyUrl(objTab, strURL, FormatTitle(objArticle.Title, articleSettings), settings)


            Else

                If (articleSettings.LaunchLinks) Then
                    Dim parameters As New List(Of String)
                    Dim pageFound As Boolean = False
                    For i As Integer = 0 To additionalParameters.Length - 1
                        parameters.Add(additionalParameters(i))
                        If (additionalParameters(i).ToLower().StartsWith("pageid=")) Then
                            pageFound = True
                        End If
                    Next
                    parameters.Add("mid=" & objArticle.ModuleID.ToString)
                    parameters.Add("ctl=ArticleView")
                    parameters.Add("articleId=" & objArticle.ArticleID)

                    If (articleSettings.AlwaysShowPageID) Then
                        If (pageID <> Null.NullInteger) Then
                            If (pageFound = False) Then
                                parameters.Add("PageID=" & pageID.ToString())
                            End If
                        End If
                    End If

                    If (articleSettings.AuthorUserIDFilter) Then
                        If (articleSettings.AuthorUserIDParam <> "") Then
                            If (HttpContext.Current.Request(articleSettings.AuthorUserIDParam) <> "") Then
                                parameters.Add(articleSettings.AuthorUserIDParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUserIDParam))
                            End If
                        End If
                    End If

                    If (articleSettings.AuthorUsernameFilter) Then
                        If (articleSettings.AuthorUsernameParam <> "") Then
                            If (HttpContext.Current.Request(articleSettings.AuthorUsernameParam) <> "") Then
                                parameters.Add(articleSettings.AuthorUsernameParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUsernameParam))
                            End If
                        End If
                    End If

                    If (System.Web.HttpContext.Current.Request.Url.Port = 80) Then
                        Return AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & NavigateURL(objTab.TabID, Null.NullString, parameters.ToArray))
                    Else
                        Return AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & ":" & System.Web.HttpContext.Current.Request.Url.Port.ToString() & NavigateURL(objTab.TabID, Null.NullString, parameters.ToArray))
                    End If
                Else
                    Dim parameters As New List(Of String)
                    Dim pageFound As Boolean = False
                    For i As Integer = 0 To additionalParameters.Length - 1
                        parameters.Add(additionalParameters(i))
                        If (additionalParameters(i).ToLower().StartsWith("pageid=")) Then
                            pageFound = True
                        End If
                    Next

                    If (articleSettings.UrlModeType = Components.Types.UrlModeType.Classic) Then
                        parameters.Add("articleType=ArticleView")
                        parameters.Add("articleId=" & objArticle.ArticleID)
                    Else
                        parameters.Add(articleSettings.ShortenedID & "=" & objArticle.ArticleID)
                    End If

                    If (articleSettings.AlwaysShowPageID) Then
                        If (pageID <> Null.NullInteger) Then
                            If (pageFound = False) Then
                                parameters.Add("PageID=" & pageID.ToString())
                            End If
                        End If
                    End If

                    If (articleSettings.AuthorUserIDFilter) Then
                        If (articleSettings.AuthorUserIDParam <> "") Then
                            If (HttpContext.Current.Request(articleSettings.AuthorUserIDParam) <> "") Then
                                parameters.Add(articleSettings.AuthorUserIDParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUserIDParam))
                            End If
                        End If
                    End If

                    If (articleSettings.AuthorUsernameFilter) Then
                        If (articleSettings.AuthorUsernameParam <> "") Then
                            If (HttpContext.Current.Request(articleSettings.AuthorUsernameParam) <> "") Then
                                parameters.Add(articleSettings.AuthorUsernameParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUsernameParam))
                            End If
                        End If
                    End If

                    If (System.Web.HttpContext.Current.Request.Url.Port = 80) Then
                        Return AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & NavigateURL(objTab.TabID, Null.NullString, parameters.ToArray))
                    Else
                        Return AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & ":" & System.Web.HttpContext.Current.Request.Url.Port.ToString() & NavigateURL(objTab.TabID, Null.NullString, parameters.ToArray))
                    End If
                End If

            End If

        End Function

        Public Shared Function GetArticleModules(ByVal portalID As Integer) As List(Of ModuleInfo)

            Dim objModulesFound As New List(Of ModuleInfo)

            Dim objDesktopModuleInfo As DesktopModuleInfo = DesktopModuleController.GetDesktopModuleByModuleName("DnnForge - NewsArticles", portalID)

            If Not (objDesktopModuleInfo Is Nothing) Then

                Dim objTabController As New TabController()
                Dim objTabs As TabCollection = objTabController.GetTabsByPortal(portalID)
                For Each objTab As DotNetNuke.Entities.Tabs.TabInfo In objTabs.Values
                    If Not (objTab Is Nothing) Then
                        If (objTab.IsDeleted = False) Then
                            Dim objModules As New ModuleController
                            For Each pair As KeyValuePair(Of Integer, ModuleInfo) In objModules.GetTabModules(objTab.TabID)
                                Dim objModule As ModuleInfo = pair.Value
                                If (objModule.IsDeleted = False) Then
                                    If (objModule.DesktopModuleID = objDesktopModuleInfo.DesktopModuleID) Then
                                        If objModule.IsDeleted = False Then
                                            Dim strPath As String = objTab.TabName
                                            Dim objTabSelected As TabInfo = objTab
                                            While objTabSelected.ParentId <> Null.NullInteger
                                                objTabSelected = objTabController.GetTab(objTabSelected.ParentId, objTab.PortalID, False)
                                                If (objTabSelected Is Nothing) Then
                                                    Exit While
                                                End If
                                                strPath = objTabSelected.TabName & " -> " & strPath
                                            End While

                                            objModulesFound.Add(objModule)
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                Next

            End If

            Return objModulesFound

        End Function

        Public Shared Function GetAuthorLink(ByVal tabID As Integer, ByVal moduleID As Integer, ByVal authorID As Integer, ByVal username As String, ByVal launchLinks As Boolean, ByVal articleSettings As ArticleSettings) As String

            Dim objTab As TabInfo = PortalController.Instance.GetCurrentPortalSettings.ActiveTab
            If (tabID <> PortalController.Instance.GetCurrentPortalSettings.ActiveTab.TabID) Then
                Dim objTabController As New TabController
                objTab = objTabController.GetTab(tabID, PortalController.Instance.GetCurrentPortalSettings.PortalId, False)
            End If
            Return GetAuthorLink(tabID, moduleID, authorID, username, launchLinks, objTab, articleSettings)

        End Function

        Public Shared Function GetAuthorLink(ByVal tabID As Integer, ByVal moduleID As Integer, ByVal authorID As Integer, ByVal username As String, ByVal launchLinks As Boolean, ByVal targetTab As TabInfo, ByVal articleSettings As ArticleSettings) As String

            If Host.UseFriendlyUrls Then

                Dim strURL As String = ApplicationURL(tabID)

                If (launchLinks) Then
                    strURL = strURL & "&ctl=AuthorView"
                    strURL = strURL & "&mid=" & moduleID.ToString
                Else
                    strURL = strURL & "&articleType=AuthorView"
                End If
                strURL = strURL & "&authorID=" & authorID.ToString()

                ' TODO: Remove at a later date when minimum version raised.
                If LocaleController.Instance.GetLocales(PortalSettings.Current.PortalId).Count > 1 AndAlso LocalizationUtil.UseLanguageInUrl Then
                    strURL += "&language=" & Thread.CurrentThread.CurrentCulture.Name
                End If

                If (articleSettings.AuthorUserIDFilter) Then
                    If (articleSettings.AuthorUserIDParam <> "") Then
                        If (HttpContext.Current.Request(articleSettings.AuthorUserIDParam) <> "") Then
                            strURL = strURL & "&" & articleSettings.AuthorUserIDParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUserIDParam)
                        End If
                    End If
                End If

                If (articleSettings.AuthorUsernameFilter) Then
                    If (articleSettings.AuthorUsernameParam <> "") Then
                        If (HttpContext.Current.Request(articleSettings.AuthorUsernameParam) <> "") Then
                            strURL = strURL & "&" & articleSettings.AuthorUsernameParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUsernameParam)
                        End If
                    End If
                End If

                Return FriendlyUrl(targetTab, strURL, Common.FormatTitle("", articleSettings), PortalController.Instance.GetCurrentPortalSettings)

            Else

                Return Common.GetModuleLink(tabID, moduleID, "AuthorView", articleSettings, "AuthorID=" & authorID.ToString())

            End If

        End Function

        Public Shared Function GetCategoryLink(ByVal tabID As Integer, ByVal moduleID As Integer, ByVal categoryID As String, ByVal title As String, ByVal launchLinks As Boolean, ByVal articleSettings As ArticleSettings) As String

            Dim objTab As TabInfo = PortalController.Instance.GetCurrentPortalSettings.ActiveTab
            If (tabID <> PortalController.Instance.GetCurrentPortalSettings.ActiveTab.TabID) Then
                Dim objTabController As New TabController
                objTab = objTabController.GetTab(tabID, PortalController.Instance.GetCurrentPortalSettings.PortalId, False)
            End If
            Return GetCategoryLink(tabID, moduleID, categoryID, title, launchLinks, objTab, articleSettings)

        End Function

        Public Shared Function GetCategoryLink(ByVal tabID As Integer, ByVal moduleID As Integer, ByVal categoryID As String, ByVal title As String, ByVal launchLinks As Boolean, ByVal targetTab As TabInfo, ByVal articleSettings As ArticleSettings) As String

            If Host.UseFriendlyUrls Then
                
                Dim strURL As String = ApplicationURL(tabID)
                Dim settings As PortalSettings = PortalController.Instance.GetCurrentPortalSettings

                If (launchLinks) Then
                    strURL = strURL & "&ctl=CategoryView"
                    strURL = strURL & "&mid=" & moduleID.ToString
                Else
                    strURL = strURL & "&articleType=CategoryView"
                End If
                strURL = strURL & "&categoryId=" & categoryID

                ' TODO: Remove at a later date when minimum version raised.
                If LocaleController.Instance.GetLocales(settings.PortalId).Count > 1 AndAlso LocalizationUtil.UseLanguageInUrl Then
                    strURL += "&language=" & Thread.CurrentThread.CurrentCulture.Name
                End If

                If (articleSettings.AuthorUserIDFilter) Then
                    If (articleSettings.AuthorUserIDParam <> "") Then
                        If (HttpContext.Current.Request(articleSettings.AuthorUserIDParam) <> "") Then
                            strURL = strURL & "&" & articleSettings.AuthorUserIDParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUserIDParam)
                        End If
                    End If
                End If

                If (articleSettings.AuthorUsernameFilter) Then
                    If (articleSettings.AuthorUsernameParam <> "") Then
                        If (HttpContext.Current.Request(articleSettings.AuthorUsernameParam) <> "") Then
                            strURL = strURL & "&" & articleSettings.AuthorUsernameParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUsernameParam)
                        End If
                    End If
                End If

                Return FriendlyUrl(targetTab, strURL, Common.FormatTitle(title, articleSettings), settings)

            Else

                Return Common.GetModuleLink(tabID, moduleID, "CategoryView", articleSettings, "categoryId=" & categoryID)

            End If

        End Function

        Public Shared Function GetModuleLink(ByVal tabID As Integer, ByVal moduleID As Integer, ByVal ctl As String, ByVal articleSettings As ArticleSettings, ByVal ParamArray additionalParameters As String()) As String

            Dim parameters As New List(Of String)
            For Each item As String In additionalParameters
                parameters.Add(item)
            Next

            If (articleSettings.AuthorUserIDFilter) Then
                If (articleSettings.AuthorUserIDParam <> "") Then
                    If (HttpContext.Current.Request(articleSettings.AuthorUserIDParam) <> "") Then
                        parameters.Add(articleSettings.AuthorUserIDParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUserIDParam))
                    End If
                End If
            End If

            If (articleSettings.AuthorUsernameFilter) Then
                If (articleSettings.AuthorUsernameParam <> "") Then
                    If (HttpContext.Current.Request(articleSettings.AuthorUsernameParam) <> "") Then
                        parameters.Add(articleSettings.AuthorUsernameParam & "=" & HttpContext.Current.Request(articleSettings.AuthorUsernameParam))
                    End If
                End If
            End If

            Dim link As String = ""

            If (ctl <> "") Then

                If (articleSettings.LaunchLinks) Then
                    parameters.Insert(0, "mid=" & moduleID.ToString())
                    If (ctl.ToLower() = "submitnews") Then
                        link = NavigateURL(tabID, "edit", parameters.ToArray())
                    Else
                        link = NavigateURL(tabID, ctl, parameters.ToArray())
                    End If
                Else
                    parameters.Insert(0, "articleType=" & ctl)
                    link = NavigateURL(tabID, Null.NullString, parameters.ToArray())
                End If

            Else

                link = NavigateURL(tabID, Null.NullString, parameters.ToArray())

            End If

            If Not (link.ToLower().StartsWith("http://") Or link.ToLower().StartsWith("https://")) Then

                If (System.Web.HttpContext.Current.Request.Url.Port = 80) Then
                    link = AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & link)
                Else
                    link = AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & ":" & System.Web.HttpContext.Current.Request.Url.Port.ToString() & link)
                End If

            End If

            Return link

        End Function

        Public Shared Sub NotifyAuthor(ByVal objArticle As ArticleInfo, ByVal settings As Hashtable, ByVal moduleID As Integer, ByVal objTab As DotNetNuke.Entities.Tabs.TabInfo, ByVal portalID As Integer, ByVal articleSettings As ArticleSettings)

            If (settings.Contains(ArticleConstants.NOTIFY_APPROVAL_SETTING)) Then
                If (Convert.ToBoolean(settings(ArticleConstants.NOTIFY_APPROVAL_SETTING))) Then

                    Dim objUserController As New UserController
                    Dim objUser As UserInfo = objUserController.GetUser(portalID, objArticle.AuthorID)

                    Dim objEmailTemplateController As New EmailTemplateController
                    If Not (objUser Is Nothing) Then
                        objEmailTemplateController.SendFormattedEmail(moduleID, Common.GetArticleLink(objArticle, objTab, articleSettings, False), objArticle, EmailTemplateType.ArticleApproved, objUser.Email, articleSettings)
                    End If

                End If
            End If

        End Sub

        Public Shared Function HtmlEncode(ByVal text As String) As String

            Return System.Web.HttpUtility.HtmlEncode(text)

        End Function

        Public Shared Function HtmlDecode(ByVal text As String) As String

            Return System.Web.HttpUtility.HtmlDecode(text)

        End Function

        Public Shared Function ProcessPostTokens(ByVal content As String, ByVal objTab As TabInfo, ByVal objArticleSettings As ArticleSettings) As String

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

                                    Dim objArticleController As New ArticleController()
                                    Dim objArticle As ArticleInfo = objArticleController.GetArticle(articleID)

                                    If (objArticle IsNot Nothing) Then
                                        Dim link As String = Common.GetArticleLink(objArticle, objTab, objArticleSettings, False)
                                        formattedContent += "<a href=""" & link & """ rel=""nofollow"">" & objArticle.Title & "</a>"
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

        Public Shared Function StripTags(ByVal HTML As String) As String
            If (HTML Is Nothing OrElse HTML.Trim() = "") Then
                Return ""
            End If
            Dim pattern As String = "<(.|\n)*?>"
            Return Regex.Replace(HTML, pattern, String.Empty)
        End Function

        public Shared Function JoinHashTables(ParamArray ht As Hashtable()) As Hashtable
            dim retval as New Hashtable

            For Each objHt As Hashtable In ht
                For Each key As Object In objHt.Keys
                    If retval.ContainsKey(key) Then
                        retval(key) = objHt(key)
                    Else 
                        retval.Add(key, objHt(key))
                    End If
                Next
            Next
            return retval
        End Function

        Public shared Function ToList(Of T As Class)(objArrayList As ArrayList) As List(Of T)
            Dim retval As New List(Of T)()

            For Each objItem As Object In objArrayList
                Dim newItem As T = TryCast(objItem, T)
                If Not newItem Is Nothing Then
                    retval.Add(newItem)
                End If
            Next

            Return retval
        End Function

#End Region

    End Class

End Namespace
