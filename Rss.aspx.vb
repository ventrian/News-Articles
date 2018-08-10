'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.IO
Imports System.Xml

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Framework
Imports System.Net

Namespace Ventrian.NewsArticles

    Partial Public Class Rss
        Inherits System.Web.UI.Page

#Region " Private Members "

        Private m_articleIDs As String = Null.NullString
        Private m_count As Integer = Null.NullInteger
        Private m_categoryID() As Integer = Nothing
        Private m_categoryIDExclude() As Integer = Nothing
        Private m_tabID As Integer = Null.NullInteger
        Private m_TabInfo As DotNetNuke.Entities.Tabs.TabInfo
        Private m_moduleID As Integer = Null.NullInteger
        Private m_authorID As Integer = Null.NullInteger
        Private m_featuredOnly As Boolean = False
        Private m_matchAll As Boolean = False
        Private m_notFeaturedOnly As Boolean = False
        Private m_securedOnly As Boolean = False
        Private m_notSecuredOnly As Boolean = False
        Private m_showExpired As Boolean = False
        Private m_sortBy As String = ArticleConstants.DEFAULT_SORT_BY
        Private m_sortDirection As String = ArticleConstants.DEFAULT_SORT_DIRECTION
        Private m_tagID() As Integer = Nothing
        Private m_tagMatch As Boolean = False

        Private m_month As Integer = Null.NullInteger
        Private m_year As Integer = Null.NullInteger

        Private _template As String = "Standard"

        Private _enableSyndicationEnclosures As Boolean = True
        Private _enableSyndicationHtml As Boolean = False
        Private _enclosureType As SyndicationEnclosureType = SyndicationEnclosureType.Attachment
        Private _syndicationSummaryLength As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            If (Request("TabID") <> "") Then

                If (IsNumeric(Request("TabID"))) Then

                    m_tabID = Convert.ToInt32(Request("TabID"))

                    Dim objTabController As New TabController
                    m_TabInfo = objTabController.GetTab(m_tabID, Globals.GetPortalSettings().PortalId, False)

                End If

            End If

            If (Request("ModuleID") <> "") Then

                If (IsNumeric(Request("ModuleID"))) Then

                    m_moduleID = Convert.ToInt32(Request("ModuleID"))

                End If

            End If

            If (Request("CategoryID") <> "") Then

                Dim categories As String() = Request("CategoryID").ToString().Split(Convert.ToChar(","))

                If (categories.Length > 0) Then
                    ReDim m_categoryID(categories.Length - 1)
                    For i As Integer = 0 To categories.Length - 1
                        m_categoryID(i) = Convert.ToInt32(categories(i))
                    Next
                End If

            End If

            If (Request("CategoryIDExclude") <> "") Then

                Dim categories As String() = Request("CategoryIDExclude").ToString().Split(Convert.ToChar(","))

                If (categories.Length > 0) Then
                    ReDim m_categoryIDExclude(categories.Length - 1)
                    For i As Integer = 0 To categories.Length - 1
                        m_categoryIDExclude(i) = Convert.ToInt32(categories(i))
                    Next
                End If

            End If

            If (Request("MaxCount") <> "") Then

                If (IsNumeric(Request("MaxCount"))) Then

                    m_count = Convert.ToInt32(Request("MaxCount"))

                End If

            End If

            If (Request("AuthorID") <> "") Then

                If (IsNumeric(Request("AuthorID"))) Then

                    m_authorID = Convert.ToInt32(Request("AuthorID"))

                End If

            End If

            If (Request("FeaturedOnly") <> "") Then

                m_featuredOnly = Convert.ToBoolean(Request("FeaturedOnly"))

            End If

            If (Request("NotFeaturedOnly") <> "") Then

                m_notFeaturedOnly = Convert.ToBoolean(Request("NotFeaturedOnly"))

            End If

            If (Request("ShowExpired") <> "") Then
                m_showExpired = Convert.ToBoolean(Request("ShowExpired"))
            End If

            If (Request("SecuredOnly") <> "") Then
                m_securedOnly = Convert.ToBoolean(Request("SecuredOnly"))
            End If

            If (Request("NotSecuredOnly") <> "") Then
                m_notSecuredOnly = Convert.ToBoolean(Request("NotSecuredOnly"))
            End If

            If (Request("ArticleIDs") <> "") Then
                m_articleIDs = Request("ArticleIDs")
            End If

            If (Request("SortBy") <> "") Then

                m_sortBy = Request("SortBy").ToString()

            End If

            If (Request("SortDirection") <> "") Then

                m_sortDirection = Request("SortDirection").ToString()

            End If

            If (Request("Month") <> "") Then
                If (IsNumeric(Request("Month"))) Then
                    m_month = Convert.ToInt32(Request("Month"))
                End If
            End If

            If (Request("Year") <> "") Then
                If (IsNumeric(Request("Year"))) Then
                    m_year = Convert.ToInt32(Request("Year"))
                End If
            End If

            If (Request("MatchTag") <> "") Then
                If (Request("MatchTag") = "1") Then
                    m_tagMatch = True
                End If
            End If

            If (Request("TagIDs") <> "") Then
                Dim tagIDs() As String = Request("TagIDs").Split(","c)
                If (tagIDs.Length > 0) Then
                    Dim tags As New List(Of Integer)
                    For Each tag As String In tagIDs
                        If (IsNumeric(tag)) Then
                            tags.Add(Convert.ToInt32(tag))
                        End If
                    Next
                    m_tagID = tags.ToArray()
                End If
            End If

            If (Request("Tags") <> "") Then
                Dim tags As New List(Of Integer)
                For Each tag As String In Request("Tags").Split("|"c)
                    If (tag <> "") Then
                        Dim objTagController As New TagController()
                        Dim objTag As TagInfo = objTagController.Get(m_moduleID, Server.UrlDecode(tag).ToLower())
                        If (objTag IsNot Nothing) Then
                            tags.Add(objTag.TagID)
                        Else
                            If (m_tagMatch) Then
                                tags.Add(Null.NullInteger)
                            End If
                        End If
                    End If
                Next
                If (tags.Count > 0) Then
                    m_tagID = tags.ToArray()
                End If
            End If

        End Sub

        Private Function GetParentPortal(ByVal sportalalias As String) As String
            If (sportalalias.IndexOf("localhost") < 0) Then
                If (sportalalias.IndexOf("/") > 0) Then
                    sportalalias = sportalalias.Substring(0, sportalalias.IndexOf("/"))
                End If
            End If

            GetParentPortal = sportalalias
        End Function

        Private Function FormatTitle(ByVal title As String) As String

            Return OnlyAlphaNumericChars(title) & ".aspx"

        End Function

        Public Function OnlyAlphaNumericChars(ByVal OrigString As String) As String
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

            OrigString = Trim(OrigString)
            lLen = Len(OrigString)
            For lCtr = 1 To lLen
                sChar = Mid(OrigString, lCtr, 1)
                If IsAlphaNumeric(Mid(OrigString, lCtr, 1)) Then
                    sAns = sAns & sChar
                End If
            Next

            OnlyAlphaNumericChars = sAns

        End Function

        Private Function IsAlphaNumeric(ByVal sChr As String) As Boolean
            IsAlphaNumeric = sChr Like "[0-9A-Za-z]"
        End Function

        Private Sub ProcessHeaderFooter(ByRef objPlaceHolder As ControlCollection, ByVal templateArray As String())

            For iPtr As Integer = 0 To templateArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(templateArray(iPtr).ToString()))

                If iPtr < templateArray.Length - 1 Then

                    Select Case templateArray(iPtr + 1)

                        Case "PORTALNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & iPtr.ToString())
                            objLiteral.Text = Server.HtmlEncode(PortalController.GetCurrentPortalSettings().PortalName)
                            objPlaceHolder.Add(objLiteral)

                        Case "PORTALURL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & iPtr.ToString())
                            objLiteral.Text = Server.HtmlEncode(AddHTTP(PortalController.GetCurrentPortalSettings().PortalAlias.HTTPAlias))
                            objPlaceHolder.Add(objLiteral)

                    End Select

                End If

            Next

        End Sub

        Private Function ProcessItem(ByVal item As String) As String

            If (item.Contains("&lt;")) Then
                ' already encoded?
                Return item
            End If
            Return Server.HtmlEncode(item)

        End Function

        Private Sub ProcessItem(ByRef objPlaceHolder As ControlCollection, ByVal templateArray As String(), ByVal objArticle As ArticleInfo, ByVal articleSettings As ArticleSettings, ByVal objTab As TabInfo)

            Dim portalSettings As PortalSettings = PortalController.GetCurrentPortalSettings()

            Dim enclosureLink As String = ""
            Dim enclosureType As String = ""
            Dim enclosureLength As String = ""

            If (_enableSyndicationEnclosures) Then
                If (_enclosureType = SyndicationEnclosureType.Attachment) Then
                    If (objArticle.FileCount > 0 Or objArticle.Url.ToLower().StartsWith("http://") Or objArticle.Url.ToLower().StartsWith("https://")) Then
                        If (objArticle.FileCount > 0) Then

                            Dim objFileController As New FileController()
                            Dim objFiles As List(Of FileInfo) = objFileController.GetFileList(objArticle.ArticleID, "")

                            If (objFiles.Count > 0) Then
                                If (System.Web.HttpContext.Current.Request.Url.Port = 80) Then
                                    enclosureLink = AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & portalSettings.HomeDirectory & objFiles(0).Folder & objFiles(0).FileName)
                                Else
                                    enclosureLink = AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & ":" & System.Web.HttpContext.Current.Request.Url.Port.ToString() & portalSettings.HomeDirectory & objFiles(0).Folder & objFiles(0).FileName)
                                End If
                                enclosureType = objFiles(0).ContentType
                                enclosureLength = objFiles(0).Size.ToString()
                            End If

                        Else
                            If (objArticle.Url.ToLower().StartsWith("http://") Or objArticle.Url.ToLower().StartsWith("https://")) Then

                                Dim objFileInfo As Hashtable = CType(DataCache.GetCache("NA-" & objArticle.Url), Hashtable)

                                If (objFileInfo Is Nothing) Then

                                    objFileInfo = New Hashtable

                                    Try

                                        Dim Url As New Uri(objArticle.Url)

                                        Dim myHttpWebRequest As HttpWebRequest = DirectCast(WebRequest.Create(Url), HttpWebRequest)
                                        Dim myHttpWebResponse As HttpWebResponse = DirectCast(myHttpWebRequest.GetResponse(), HttpWebResponse)

                                        objFileInfo.Add("ContentType", myHttpWebResponse.ContentType)
                                        objFileInfo.Add("ContentLength", myHttpWebResponse.ContentLength)

                                        myHttpWebResponse.Close()

                                    Catch
                                    End Try

                                    DataCache.SetCache("NA-" & objArticle.Url, objFileInfo)

                                End If

                                If (objFileInfo.Count > 0) Then
                                    enclosureLink = objArticle.Url
                                    enclosureType = objFileInfo("ContentType").ToString()
                                    enclosureLength = objFileInfo("ContentLength").ToString()
                                End If
                            End If
                        End If
                    End If
                Else
                    If (objArticle.ImageCount > 0) Then
                        Dim objImageController As New ImageController
                        Dim objImages As List(Of ImageInfo) = objImageController.GetImageList(objArticle.ArticleID, Null.NullString())

                        If (objImages.Count > 0) Then

                            If (System.Web.HttpContext.Current.Request.Url.Port = 80) Then
                                enclosureLink = AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & portalSettings.HomeDirectory & objImages(0).Folder & objImages(0).FileName)
                            Else
                                enclosureLink = AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & ":" & System.Web.HttpContext.Current.Request.Url.Port.ToString() & portalSettings.HomeDirectory & objImages(0).Folder & objImages(0).FileName)
                            End If

                            enclosureType = objImages(0).ContentType
                            enclosureLength = objImages(0).Size.ToString()
                        End If
                    End If
                End If
            End If

            Dim hasEnclosure As Boolean = False

            If (enclosureLink <> "") Then
                hasEnclosure = True
            End If

            For iPtr As Integer = 0 To templateArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(templateArray(iPtr).ToString()))

                If iPtr < templateArray.Length - 1 Then

                    Select Case templateArray(iPtr + 1)

                        Case "ARTICLELINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & objArticle.ArticleID.ToString() & iPtr.ToString())
                            Dim pageID As Integer = Null.NullInteger
                            If (articleSettings.SyndicationLinkType = SyndicationLinkType.Attachment And (objArticle.Url <> "" Or objArticle.FileCount > 0)) Then
                                If (objArticle.FileCount > 0) Then
                                    Dim objFileController As New FileController()
                                    Dim objFiles As List(Of FileInfo) = objFileController.GetFileList(objArticle.ArticleID, "")

                                    If (objFiles.Count > 0) Then
                                        If (System.Web.HttpContext.Current.Request.Url.Port = 80) Then
                                            objLiteral.Text = AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & portalSettings.HomeDirectory & objFiles(0).Folder & objFiles(0).FileName).Replace("&", "&amp;")
                                        Else
                                            objLiteral.Text = AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & ":" & System.Web.HttpContext.Current.Request.Url.Port.ToString() & portalSettings.HomeDirectory & objFiles(0).Folder & objFiles(0).FileName).Replace("&", "&amp;")
                                        End If
                                    End If
                                Else
                                    objLiteral.Text = DotNetNuke.Common.Globals.LinkClick(objArticle.Url, m_tabID, objArticle.ModuleID, False).Replace("&", "&amp;")
                                    If (objLiteral.Text.ToLower().StartsWith("http") = False) Then
                                        If (System.Web.HttpContext.Current.Request.Url.Port = 80) Then
                                            objLiteral.Text = AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & objLiteral.Text)
                                        Else
                                            objLiteral.Text = AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & ":" & System.Web.HttpContext.Current.Request.Url.Port.ToString() & objLiteral.Text)
                                        End If
                                    End If
                                End If
                            Else
                                objLiteral.Text = Common.GetArticleLink(objArticle, m_TabInfo, articleSettings, False).Replace("&", "&amp;")
                            End If
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "COMMENTLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & objArticle.ArticleID.ToString() & iPtr.ToString())
                            objLiteral.Text = Common.GetArticleLink(objArticle, m_TabInfo, articleSettings, False).Replace("&", "&amp;") & "#Comments"
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "DESCRIPTION"
                            Dim description As String = ""
                            If (Common.StripTags(Server.HtmlDecode(objArticle.Summary)) <> "") Then
                                If (_enableSyndicationHtml) Then
                                    description = ProcessItem(Common.ProcessPostTokens(Server.HtmlDecode(objArticle.Summary), m_TabInfo, articleSettings))
                                Else
                                    If (_syndicationSummaryLength <> Null.NullInteger) Then
                                        Dim summary As String = Common.ProcessPostTokens(Common.StripTags(Server.HtmlDecode(objArticle.Summary)), m_TabInfo, articleSettings)
                                        If (summary.Length > _syndicationSummaryLength) Then
                                            description = ProcessItem(Common.ProcessPostTokens(Common.StripTags(Server.HtmlDecode(objArticle.Summary)), m_TabInfo, articleSettings).Substring(0, _syndicationSummaryLength) & "...")
                                        Else
                                            description = ProcessItem(Common.ProcessPostTokens(Common.StripTags(Server.HtmlDecode(objArticle.Summary)), m_TabInfo, articleSettings))
                                        End If
                                    Else
                                        description = ProcessItem(Common.ProcessPostTokens(Common.StripTags(Server.HtmlDecode(objArticle.Summary)), m_TabInfo, articleSettings))
                                    End If
                                End If
                            Else
                                If (_enableSyndicationHtml) Then
                                    description = ProcessItem(Common.ProcessPostTokens(Server.HtmlDecode(objArticle.Body), m_TabInfo, articleSettings))
                                Else
                                    If (_syndicationSummaryLength <> Null.NullInteger) Then
                                        Dim summary As String = ProcessItem(Common.ProcessPostTokens(Common.StripTags(Server.HtmlDecode(objArticle.Body)), m_TabInfo, articleSettings))
                                        If (summary.Length > _syndicationSummaryLength) Then
                                            description = ProcessItem(Common.ProcessPostTokens(Common.StripTags(Server.HtmlDecode(objArticle.Body)), m_TabInfo, articleSettings).Substring(0, _syndicationSummaryLength) & "...")
                                        Else
                                            description = ProcessItem(Common.ProcessPostTokens(Common.StripTags(Server.HtmlDecode(objArticle.Body)), m_TabInfo, articleSettings))
                                        End If
                                    Else
                                        description = ProcessItem(Common.ProcessPostTokens(Common.StripTags(Server.HtmlDecode(objArticle.Body)), m_TabInfo, articleSettings))
                                    End If
                                End If
                            End If

                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & objArticle.ArticleID.ToString() & iPtr.ToString())
                            objLiteral.Text = description
                            objPlaceHolder.Add(objLiteral)

                        Case "DETAILS"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & objArticle.ArticleID.ToString() & iPtr.ToString())
                            If (objArticle.PageCount > 0) Then
                                Dim pageID As Integer = Null.NullInteger
                                If (IsNumeric(Request("PageID"))) Then
                                    pageID = Convert.ToInt32(Request("PageID"))
                                End If
                                If (pageID = Null.NullInteger) Then
                                    objLiteral.Text = ProcessItem(Common.ProcessPostTokens(objArticle.Body, objTab, articleSettings))
                                Else
                                    Dim pageController As New PageController
                                    Dim pageList As ArrayList = pageController.GetPageList(objArticle.ArticleID)
                                    For Each objPage As PageInfo In pageList
                                        If (objPage.PageID = pageID) Then
                                            objLiteral.Text = ProcessItem(Common.ProcessPostTokens(objPage.PageText, objTab, articleSettings))
                                            Exit For
                                        End If
                                    Next
                                    If (objLiteral.Text = Null.NullString) Then
                                        objLiteral.Text = ProcessItem(Common.ProcessPostTokens(objArticle.Body, objTab, articleSettings))
                                    End If
                                End If
                            End If
                            objPlaceHolder.Add(objLiteral)

                        Case "ENCLOSURELENGTH"

                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & objArticle.ArticleID.ToString() & iPtr.ToString())
                            objLiteral.Text = enclosureLength
                            objPlaceHolder.Add(objLiteral)

                        Case "ENCLOSURELINK"

                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & objArticle.ArticleID.ToString() & iPtr.ToString())
                            objLiteral.Text = enclosureLink.Replace("&amp;", "&").Replace("&", "&amp;").Replace(" ", "%20")
                            objPlaceHolder.Add(objLiteral)

                        Case "ENCLOSURETYPE"

                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & objArticle.ArticleID.ToString() & iPtr.ToString())
                            objLiteral.Text = enclosureType
                            objPlaceHolder.Add(objLiteral)

                        Case "GUID"

                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & objArticle.ArticleID.ToString() & iPtr.ToString())
                            objLiteral.Text = "f1397696-738c-4295-afcd-943feb885714:" & objArticle.ArticleID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "HASENCLOSURE"
                            If (hasEnclosure = False) Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/HASENCLOSURE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASENCLOSURE"
                            ' Do Nothing

                        Case "IMAGELINK"
                            If (objArticle.ImageUrl <> "") Then
                                Dim objLiteral As New Literal
                                objLiteral.Text = objArticle.ImageUrl
                                objPlaceHolder.Add(objLiteral)
                            Else
                                Dim objImageController As New ImageController
                                Dim objImages As List(Of ImageInfo) = objImageController.GetImageList(objArticle.ArticleID, Null.NullString())

                                If (objImages.Count > 0) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.Text = AddHTTP(Request.Url.Host & ":" & System.Web.HttpContext.Current.Request.Url.Port.ToString() & portalSettings.HomeDirectory & objImages(0).Folder & objImages(0).FileName)
                                    objPlaceHolder.Add(objLiteral)
                                End If
                            End If

                        Case "PUBLISHDATE"

                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & objArticle.ArticleID.ToString() & iPtr.ToString())
                            objLiteral.Text = objArticle.StartDate.ToUniversalTime().ToString("r")
                            objPlaceHolder.Add(objLiteral)

                        Case "SUMMARY"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & objArticle.ArticleID.ToString() & iPtr.ToString())
                            objLiteral.Text = ProcessItem(Common.ProcessPostTokens(objArticle.Summary, objTab, articleSettings))
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "TITLE"

                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & objArticle.ArticleID.ToString() & iPtr.ToString())
                            objLiteral.Text = Server.HtmlEncode(objArticle.Title)
                            objPlaceHolder.Add(objLiteral)

                        Case "TITLEURL"

                            Dim title As String = Common.FormatTitle(objArticle.Title, articleSettings)

                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & objArticle.ArticleID.ToString() & iPtr.ToString())
                            objLiteral.Text = Server.HtmlEncode(title)
                            objPlaceHolder.Add(objLiteral)

                        Case "TRACKBACKLINK"

                            Dim link As String = ""
                            If (System.Web.HttpContext.Current.Request.Url.Port = 80) Then
                                link = AddHTTP(Request.Url.Host & Me.ResolveUrl("Tracking/Trackback.aspx?ArticleID=" & objArticle.ArticleID.ToString() & "&amp;PortalID=" & portalSettings.PortalId.ToString() & "&amp;TabID=" & portalSettings.ActiveTab.TabID.ToString()).Replace(" ", "%20"))
                            Else
                                link = AddHTTP(Request.Url.Host & ":" & System.Web.HttpContext.Current.Request.Url.Port.ToString() & Me.ResolveUrl("Tracking/Trackback.aspx?ArticleID=" & objArticle.ArticleID.ToString() & "&amp;PortalID=" & portalSettings.PortalId.ToString() & "&amp;TabID=" & portalSettings.ActiveTab.TabID.ToString()).Replace(" ", "%20"))
                            End If

                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Rss-" & objArticle.ArticleID.ToString() & iPtr.ToString())
                            objLiteral.Text = link
                            objPlaceHolder.Add(objLiteral)

                        Case Else

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("DETAILS:")) Then
                                Dim length As Integer = Convert.ToInt32(templateArray(iPtr + 1).Substring(8, templateArray(iPtr + 1).Length - 8))

                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID("Rss-" & objArticle.ArticleID.ToString() & iPtr.ToString())
                                If (StripHtml(Server.HtmlDecode(objArticle.Body)).TrimStart().Length > length) Then
                                    objLiteral.Text = ProcessItem(Common.ProcessPostTokens(Left(StripHtml(Server.HtmlDecode(objArticle.Body)).TrimStart(), length), objTab, articleSettings) & "...")
                                Else
                                    objLiteral.Text = ProcessItem(Common.ProcessPostTokens(Left(StripHtml(Server.HtmlDecode(objArticle.Body)).TrimStart(), length), objTab, articleSettings))
                                End If

                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                                Exit Select
                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("SUMMARY:")) Then
                                Dim summary As String = objArticle.Summary
                                If (IsNumeric(templateArray(iPtr + 1).Substring(8, templateArray(iPtr + 1).Length - 8))) Then
                                    Dim length As Integer = Convert.ToInt32(templateArray(iPtr + 1).Substring(8, templateArray(iPtr + 1).Length - 8))
                                    If (StripHtml(Server.HtmlDecode(objArticle.Summary)).TrimStart().Length > length) Then
                                        summary = ProcessItem(Common.ProcessPostTokens(Left(StripHtml(Server.HtmlDecode(objArticle.Summary)).TrimStart(), length), objTab, articleSettings) & "...")
                                    Else
                                        summary = ProcessItem(Common.ProcessPostTokens(Left(StripHtml(Server.HtmlDecode(objArticle.Summary)).TrimStart(), length), objTab, articleSettings))
                                    End If
                                End If

                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID("Rss-" & objArticle.ArticleID.ToString() & iPtr.ToString())
                                objLiteral.Text = summary
                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                                Exit Select
                            End If

                            Dim objLiteralOther As New Literal
                            objLiteralOther.ID = Globals.CreateValidID("Rss-" & objArticle.ArticleID.ToString() & iPtr.ToString())
                            objLiteralOther.Text = "[" & templateArray(iPtr + 1) & "]"
                            objLiteralOther.EnableViewState = False
                            objPlaceHolder.Add(objLiteralOther)

                    End Select

                End If

            Next

        End Sub

        Private Function RenderControlToString(ByVal ctrl As Control) As String

            Dim sb As New StringBuilder()
            Dim tw As New IO.StringWriter(sb)
            Dim hw As New HtmlTextWriter(tw)

            ctrl.RenderControl(hw)

            Return sb.ToString()

        End Function

        Private Function StripHtml(ByVal html As String) As String

            Dim pattern As String = "<(.|\n)*?>"
            Return Regex.Replace(html, pattern, String.Empty)

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            ReadQueryString()

            Dim displayType As DisplayType = displayType.UserName
            Dim launchLinks As Boolean = False
            Dim showPending As Boolean = False

            Dim _portalSettings As PortalSettings = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            Dim objModuleController As New ModuleController
            Dim objModule As ModuleInfo = Common.GetModuleInfo(m_moduleID, m_tabID)
            Dim articleSettings As ArticleSettings

            If Not (objModule Is Nothing) Then
                Dim objTabController As New TabController()
                Dim objTab As TabInfo = objTabController.GetTab(objModule.TabID, _portalSettings.PortalId, False)

                Dim settings As Hashtable =Common.JoinHashTables(objModule.ModuleSettings, objModule.TabModuleSettings)
                articleSettings = New ArticleSettings(settings, _portalSettings, objModule)
                If (settings.Contains(ArticleConstants.LAUNCH_LINKS)) Then
                    launchLinks = Convert.ToBoolean(settings(ArticleConstants.LAUNCH_LINKS).ToString())
                End If
                If (settings.Contains(ArticleConstants.TEMPLATE_SETTING)) Then
                    _template = settings(ArticleConstants.TEMPLATE_SETTING).ToString()
                End If
                If (settings.Contains(ArticleConstants.ENABLE_SYNDICATION_ENCLOSURES_SETTING)) Then
                    _enableSyndicationEnclosures = Convert.ToBoolean(settings(ArticleConstants.ENABLE_SYNDICATION_ENCLOSURES_SETTING).ToString())
                End If
                If (settings.Contains(ArticleConstants.SYNDICATION_ENCLOSURE_TYPE)) Then
                    _enclosureType = CType(System.Enum.Parse(GetType(SyndicationEnclosureType), settings(ArticleConstants.SYNDICATION_ENCLOSURE_TYPE).ToString()), SyndicationEnclosureType)
                End If
                If (settings.Contains(ArticleConstants.ENABLE_SYNDICATION_HTML_SETTING)) Then
                    _enableSyndicationHtml = Convert.ToBoolean(settings(ArticleConstants.ENABLE_SYNDICATION_HTML_SETTING).ToString())
                End If
                If (objModule.ModuleSettings.Contains(ArticleConstants.SYNDICATION_SUMMARY_LENGTH)) Then
                    _syndicationSummaryLength = Convert.ToInt32(objModule.ModuleSettings(ArticleConstants.SYNDICATION_SUMMARY_LENGTH).ToString())
                End If
                If (settings.Contains(ArticleConstants.SHOW_PENDING_SETTING)) Then
                    showPending = Convert.ToBoolean(settings(ArticleConstants.SHOW_PENDING_SETTING).ToString())
                End If
                If (settings.Contains(ArticleConstants.DISPLAY_MODE)) Then
                    displayType = CType(System.Enum.Parse(GetType(DisplayType), settings(ArticleConstants.DISPLAY_MODE).ToString()), DisplayType)
                End If
                If (m_count = Null.NullInteger) Then
                    If (settings.Contains(ArticleConstants.SYNDICATION_MAX_COUNT)) Then
                        Try
                            m_count = Convert.ToInt32(settings(ArticleConstants.SYNDICATION_MAX_COUNT).ToString())
                        Catch
                            m_count = 50
                        End Try
                    Else
                        m_count = 50
                    End If
                End If
                If (m_categoryID Is Nothing) Then
                    If (settings.Contains(ArticleConstants.CATEGORIES_SETTING & m_tabID.ToString())) Then
                        If Not (settings(ArticleConstants.CATEGORIES_SETTING & m_tabID.ToString()).ToString = Null.NullString Or settings(ArticleConstants.CATEGORIES_SETTING & m_tabID.ToString()).ToString = "-1") Then
                            Dim categories As String() = settings(ArticleConstants.CATEGORIES_SETTING & m_tabID.ToString()).ToString().Split(","c)
                            Dim cats As New List(Of Integer)

                            For Each category As String In categories
                                If (IsNumeric(category)) Then
                                    cats.Add(Convert.ToInt32(category))
                                End If
                            Next

                            m_categoryID = cats.ToArray()
                        End If
                    End If
                End If

                If (m_categoryID IsNot Nothing) Then
                    If (m_categoryID.Length > 0) Then
                        If (settings.Contains(ArticleConstants.MATCH_OPERATOR_SETTING)) Then
                            Dim objMatchOperator As MatchOperatorType = CType(System.Enum.Parse(GetType(MatchOperatorType), settings(ArticleConstants.MATCH_OPERATOR_SETTING).ToString()), MatchOperatorType)
                            If (objMatchOperator = MatchOperatorType.MatchAll) Then
                                m_matchAll = True
                            End If
                        End If

                        If (Request("MatchCat") <> "" And Request("CategoryID") <> "") Then
                            m_matchAll = True
                        End If
                    End If
                End If

                Dim objLayoutController As New LayoutController(_portalSettings, articleSettings, objModule, Page)
                'Dim objLayoutController As New LayoutController(_portalSettings, articleSettings, Me, False, m_tabID, m_moduleID, objModule.TabModuleID, _portalSettings.PortalId, Null.NullInteger, Null.NullInteger, "Rss-" & m_tabID.ToString())

                Dim layoutHeader As LayoutInfo = LayoutController.GetLayout(articleSettings, objModule, Page, LayoutType.Rss_Header_Html)
                Dim layoutItem As LayoutInfo = LayoutController.GetLayout(articleSettings, objModule, Page, LayoutType.Rss_Item_Html)
                Dim layoutFooter As LayoutInfo = LayoutController.GetLayout(articleSettings, objModule, Page, LayoutType.Rss_Footer_Html)

                Dim phRSS As New PlaceHolder

                Response.ContentType = "text/xml"
                Response.ContentEncoding = Encoding.UTF8

                ProcessHeaderFooter(phRSS.Controls, layoutHeader.Tokens)

                Dim agedDate As DateTime = Null.NullDate
                Dim startDate As DateTime = DateTime.Now.AddMinutes(1)
                If (m_year <> Null.NullInteger AndAlso m_month <> Null.NullInteger) Then
                    agedDate = New DateTime(m_year, m_month, 1)
                    startDate = agedDate.AddMonths(1).AddSeconds(-1)
                End If

                If (m_categoryID Is Nothing) Then

                    ' Permission to view category?
                    Dim objCategoryController As New CategoryController
                    Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(m_moduleID, Null.NullInteger)
                    Dim checkCategory = False

                    Dim excludeCategories As New List(Of Integer)
                    For Each objCategory As CategoryInfo In objCategories
                        If (objCategory.InheritSecurity = False And objCategory.CategorySecurityType = CategorySecurityType.Restrict) Then
                            excludeCategories.Add(objCategory.CategoryID)
                        End If
                    Next
                    If (excludeCategories.Count > 0) Then
                        m_categoryIDExclude = excludeCategories.ToArray()
                    End If

                    For Each objCategory As CategoryInfo In objCategories
                        If (objCategory.InheritSecurity = False And objCategory.CategorySecurityType = CategorySecurityType.Loose) Then
                            checkCategory = True
                        End If
                    Next

                    If (checkCategory) Then
                        If (m_categoryID Is Nothing) Then
                            Dim includeCategories As New List(Of Integer)

                            For Each objCategory As CategoryInfo In objCategories
                                If (objCategory.InheritSecurity) Then
                                    includeCategories.Add(objCategory.CategoryID)
                                End If
                            Next

                            If (includeCategories.Count > 0) Then
                                includeCategories.Add(-1)
                            End If

                            m_categoryID = includeCategories.ToArray()
                        Else
                            Dim includeCategories As New List(Of Integer)

                            For Each i As Integer In m_categoryID
                                For Each objCategory As CategoryInfo In objCategories
                                    If (i = objCategory.CategoryID) Then
                                        If (objCategory.InheritSecurity) Then
                                            includeCategories.Add(objCategory.CategoryID)
                                        End If
                                    End If
                                Next
                            Next

                            m_categoryID = includeCategories.ToArray()
                        End If
                    End If

                End If

                Dim objArticleController As New ArticleController
                Dim articleList As List(Of ArticleInfo) = objArticleController.GetArticleList(m_moduleID, startDate, agedDate, m_categoryID, m_matchAll, m_categoryIDExclude, m_count, 1, m_count, m_sortBy, m_sortDirection, True, False, Null.NullString, m_authorID, showPending, m_showExpired, m_featuredOnly, m_notFeaturedOnly, m_securedOnly, m_notSecuredOnly, m_articleIDs, m_tagID, m_tagMatch, Null.NullString, Null.NullInteger, Null.NullString, Null.NullString, Null.NullInteger)

                For Each objArticle As ArticleInfo In articleList

                    Dim delimStr As String = "[]"
                    Dim delimiter As Char() = delimStr.ToCharArray()

                    Dim phItem As New PlaceHolder
                    ProcessItem(phItem.Controls, layoutItem.Tokens, objArticle, articleSettings, objTab)
                    objLayoutController.ProcessArticleItem(phRSS.Controls, RenderControlToString(phItem).Split(delimiter), objArticle)

                Next

                ProcessHeaderFooter(phRSS.Controls, layoutFooter.Tokens)

                Response.Write(RenderControlToString(phRSS))

            End If

            Response.End()

        End Sub

#End Region

    End Class

End Namespace