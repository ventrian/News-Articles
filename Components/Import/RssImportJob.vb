Imports System.Xml.XPath

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Scheduling
Imports System.Xml
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals

Namespace Ventrian.NewsArticles.Import

    Public Class RssImportJob
        Inherits SchedulerClient

#Region " Private Methods "

        Private Sub ImportFeed(ByVal objFeed As FeedInfo)

            Dim doc As XPathDocument
            Dim navigator As XPathNavigator
            Dim nodes As XPathNodeIterator
            Dim node As XPathNavigator

            ' Create a new XmlDocument  
            doc = New XPathDocument(objFeed.Url)

            ' Create navigator  
            navigator = doc.CreateNavigator()

            Dim mngr As New XmlNamespaceManager(navigator.NameTable)
            mngr.AddNamespace("content", "http://purl.org/rss/1.0/modules/content/")

            ' Get forecast with XPath  
            nodes = navigator.Select("/rss/channel/item")

            If (nodes.Count = 0) Then
                ImportFeedRDF(objFeed)
            End If

            Dim publishedDate As DateTime = DateTime.Now

            While nodes.MoveNext()
                node = nodes.Current

                Dim nodeTitle As XPathNavigator
                Dim nodeDescription As XPathNavigator
                Dim nodeLink As XPathNavigator
                Dim nodeDate As XPathNavigator
                Dim nodeGuid As XPathNavigator
                Dim nodeEncoded As XPathNavigator

                nodeTitle = node.SelectSingleNode("title")
                nodeDescription = node.SelectSingleNode("description")
                nodeLink = node.SelectSingleNode("link")
                nodeDate = node.SelectSingleNode("pubDate")
                nodeGuid = node.SelectSingleNode("guid")
                nodeEncoded = node.SelectSingleNode("content:encoded", mngr)

                Dim summary As String = ""
                If (nodeDescription IsNot Nothing) Then
                    summary = nodeDescription.Value
                End If

                Dim pageDetail As String = ""
                If (nodeEncoded IsNot Nothing) Then
                    pageDetail = nodeEncoded.Value
                Else
                    If (nodeDescription IsNot Nothing) Then
                        pageDetail = nodeDescription.Value
                    End If
                End If

                Dim guid As String = ""
                If (nodeGuid IsNot Nothing) Then
                    guid = nodeGuid.Value
                Else
                    guid = nodeLink.Value
                End If

                Dim objArticleController As New ArticleController()
                Dim objArticles As List(Of ArticleInfo) = objArticleController.GetArticleList(objFeed.ModuleID, DateTime.Now, Null.NullDate, Nothing, False, Nothing, 25, 1, 25, ArticleConstants.DEFAULT_SORT_BY, ArticleConstants.DEFAULT_SORT_DIRECTION, True, False, Null.NullString, Null.NullInteger, True, True, False, False, False, False, Null.NullString, Nothing, False, guid, Null.NullInteger, Null.NullString, Null.NullString, Nothing)

                If (objArticles.Count = 0) Then

                    Dim objArticle As New ArticleInfo

                    objArticle.AuthorID = objFeed.UserID
                    objArticle.CreatedDate = DateTime.Now
                    objArticle.Status = StatusType.Published
                    objArticle.CommentCount = 0
                    objArticle.RatingCount = 0
                    objArticle.Rating = 0
                    objArticle.ShortUrl = ""

                    objArticle.Title = nodeTitle.Value
                    objArticle.IsFeatured = objFeed.AutoFeature
                    objArticle.IsSecure = False
                    objArticle.Summary = summary

                    objArticle.LastUpdate = objArticle.CreatedDate
                    objArticle.LastUpdateID = objFeed.UserID
                    objArticle.ModuleID = objFeed.ModuleID

                    objArticle.Url = nodeLink.Value
                    If (objFeed.DateMode = FeedDateMode.ImportDate) Then
                        objArticle.StartDate = publishedDate
                    Else
                        Try
                            Dim val As String = nodeDate.Value

                            val = val.Replace("PST", "-0800")
                            val = val.Replace("MST", "-0700")
                            val = val.Replace("CST", "-0600")
                            val = val.Replace("EST", "-0500")

                            objArticle.StartDate = DateTime.Parse(val)
                        Catch
                            objArticle.StartDate = publishedDate
                        End Try
                    End If

                    objArticle.EndDate = Null.NullDate
                    If (objFeed.AutoExpire <> Null.NullInteger And objFeed.AutoExpireUnit <> FeedExpiryType.None) Then
                        Select Case objFeed.AutoExpireUnit

                            Case FeedExpiryType.Minute
                                objArticle.EndDate = DateTime.Now.AddMinutes(objFeed.AutoExpire)
                                Exit Select

                            Case FeedExpiryType.Hour
                                objArticle.EndDate = DateTime.Now.AddHours(objFeed.AutoExpire)
                                Exit Select

                            Case FeedExpiryType.Day
                                objArticle.EndDate = DateTime.Now.AddDays(objFeed.AutoExpire)
                                Exit Select

                            Case FeedExpiryType.Month
                                objArticle.EndDate = DateTime.Now.AddMonths(objFeed.AutoExpire)
                                Exit Select

                            Case FeedExpiryType.Year
                                objArticle.EndDate = DateTime.Now.AddYears(objFeed.AutoExpire)
                                Exit Select

                        End Select
                    End If

                    objArticle.RssGuid = guid

                    objArticle.ArticleID = objArticleController.AddArticle(objArticle)

                    Dim objPage As New PageInfo
                    objPage.PageText = pageDetail
                    objPage.ArticleID = objArticle.ArticleID
                    objPage.Title = objArticle.Title

                    Dim objPageController As New PageController()
                    objPageController.AddPage(objPage)

                    For Each objCategory As CategoryInfo In objFeed.Categories
                        objArticleController.AddArticleCategory(objArticle.ArticleID, objCategory.CategoryID)
                    Next

                    publishedDate = publishedDate.AddSeconds(-1)

                Else

                    If (objArticles.Count = 1) Then

                        objArticles(0).Title = nodeTitle.Value
                        objArticles(0).Summary = summary
                        objArticles(0).LastUpdate = DateTime.Now
                        objArticleController.UpdateArticle(objArticles(0))

                        Dim objPageController As New PageController()
                        Dim objPages As ArrayList = objPageController.GetPageList(objArticles(0).ArticleID)

                        If (objPages.Count > 0) Then
                            objPages(0).PageText = pageDetail
                            objPageController.UpdatePage(objPages(0))
                        End If

                    End If

                End If

            End While

        End Sub

        Private Sub ImportFeedRDF(ByVal objFeed As FeedInfo)

            Dim doc As XPathDocument
            Dim navigator As XPathNavigator
            Dim nodes As XPathNodeIterator
            Dim node As XPathNavigator

            ' Create a new XmlDocument  
            doc = New XPathDocument(objFeed.Url)

            ' Create navigator  
            navigator = doc.CreateNavigator()

            Dim mngr As New XmlNamespaceManager(navigator.NameTable)
            mngr.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#")
            mngr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/")

            ' Get forecast with XPath  
            nodes = navigator.Select("/rdf:RDF/*", mngr)

            While nodes.MoveNext()
                node = nodes.Current

                If (node.Name.ToLower() = "item") Then

                    Dim title As String = ""
                    Dim description As String = ""
                    Dim link As String = ""
                    Dim dateNode As String = ""
                    Dim guid As String = ""

                    Dim objChildNodes As XPathNodeIterator = node.SelectChildren(XPathNodeType.All)

                    While objChildNodes.MoveNext()
                        Dim objChildNode As XPathNavigator = objChildNodes.Current

                        Select Case objChildNode.Name.ToLower()

                            Case "title"
                                title = objChildNode.Value
                                Exit Select

                            Case "description"
                                description = objChildNode.Value
                                Exit Select

                            Case "link"
                                link = objChildNode.Value
                                Exit Select

                            Case "date"
                                dateNode = objChildNode.Value
                                Exit Select

                            Case "guid"
                                guid = objChildNode.Value
                                Exit Select

                        End Select
                    End While

                    If (title <> "" And link <> "") Then

                        If (guid = "") Then
                            guid = link
                        End If

                        Dim objArticleController As New ArticleController()
                        Dim objArticles As List(Of ArticleInfo) = objArticleController.GetArticleList(objFeed.ModuleID, DateTime.Now, Null.NullDate, Nothing, False, Nothing, 1, 1, 10, ArticleConstants.DEFAULT_SORT_BY, ArticleConstants.DEFAULT_SORT_DIRECTION, True, False, Null.NullString, Null.NullInteger, True, True, False, False, False, False, Null.NullString, Nothing, False, guid, Null.NullInteger, Null.NullString, Null.NullString, Nothing)

                        If (objArticles.Count = 0) Then

                            Dim publishedDate As DateTime = DateTime.Now

                            Dim objArticle As New ArticleInfo

                            objArticle.AuthorID = objFeed.UserID
                            objArticle.CreatedDate = DateTime.Now
                            objArticle.Status = StatusType.Published
                            objArticle.CommentCount = 0
                            objArticle.RatingCount = 0
                            objArticle.Rating = 0
                            objArticle.ShortUrl = ""

                            objArticle.Title = title
                            objArticle.IsFeatured = objFeed.AutoFeature
                            objArticle.IsSecure = False
                            objArticle.Summary = description

                            objArticle.LastUpdate = publishedDate
                            objArticle.LastUpdateID = objFeed.UserID
                            objArticle.ModuleID = objFeed.ModuleID

                            objArticle.Url = link
                            If (objFeed.DateMode = FeedDateMode.ImportDate) Then
                                objArticle.StartDate = publishedDate
                            Else
                                Try
                                    Dim val As String = dateNode

                                    val = val.Replace("PST", "-0800")
                                    val = val.Replace("MST", "-0700")
                                    val = val.Replace("CST", "-0600")
                                    val = val.Replace("EST", "-0500")

                                    objArticle.StartDate = DateTime.Parse(val)
                                Catch
                                    objArticle.StartDate = publishedDate
                                End Try
                            End If

                            objArticle.EndDate = Null.NullDate
                            If (objFeed.AutoExpire <> Null.NullInteger And objFeed.AutoExpireUnit <> FeedExpiryType.None) Then
                                Select Case objFeed.AutoExpireUnit

                                    Case FeedExpiryType.Minute
                                        objArticle.EndDate = DateTime.Now.AddMinutes(objFeed.AutoExpire)
                                        Exit Select

                                    Case FeedExpiryType.Hour
                                        objArticle.EndDate = DateTime.Now.AddHours(objFeed.AutoExpire)
                                        Exit Select

                                    Case FeedExpiryType.Day
                                        objArticle.EndDate = DateTime.Now.AddDays(objFeed.AutoExpire)
                                        Exit Select

                                    Case FeedExpiryType.Month
                                        objArticle.EndDate = DateTime.Now.AddMonths(objFeed.AutoExpire)
                                        Exit Select

                                    Case FeedExpiryType.Year
                                        objArticle.EndDate = DateTime.Now.AddYears(objFeed.AutoExpire)
                                        Exit Select

                                End Select
                            End If

                            objArticle.RssGuid = guid

                            objArticle.ArticleID = objArticleController.AddArticle(objArticle)

                            Dim objPage As New PageInfo
                            objPage.PageText = description
                            objPage.ArticleID = objArticle.ArticleID
                            objPage.Title = objArticle.Title

                            Dim objPageController As New PageController()
                            objPageController.AddPage(objPage)

                            For Each objCategory As CategoryInfo In objFeed.Categories
                                objArticleController.AddArticleCategory(objArticle.ArticleID, objCategory.CategoryID)
                            Next

                            publishedDate = publishedDate.AddSeconds(-1)

                        End If

                    End If

                End If

            End While

        End Sub

#End Region

#Region " Public Methods "

        Public Sub ImportFeeds()

            Dim objFeedController As New FeedController
            Dim objFeeds As List(Of FeedInfo) = objFeedController.GetFeedList(Null.NullInteger, True)


            For Each objFeed As FeedInfo In objFeeds
                If (Me.ScheduleHistoryItem.GetSetting("NewsArticles-Import-Clear-" & objFeed.ModuleID) <> "") Then
                    If (Convert.ToBoolean(Me.ScheduleHistoryItem.GetSetting("NewsArticles-Import-Clear-" & objFeed.ModuleID))) Then
                        ' Delete Articles
                        Dim objArticleController As New ArticleController
                        Dim objArticles As List(Of ArticleInfo) = objArticleController.GetArticleList(objFeed.ModuleID, DateTime.Now, Null.NullDate, Nothing, False, Nothing, 1000, 1, 1000, ArticleConstants.DEFAULT_SORT_BY, ArticleConstants.DEFAULT_SORT_DIRECTION, True, False, Null.NullString, Null.NullInteger, True, True, False, False, False, False, Null.NullString, Nothing, False, Null.NullString, Null.NullInteger, Null.NullString, Null.NullString, Nothing)

                        Me.ScheduleHistoryItem.AddLogNote(objArticles.Count.ToString())
                        For Each objArticle As ArticleInfo In objArticles
                            objArticleController.DeleteArticleCategories(objArticle.ArticleID)
                            objArticleController.DeleteArticle(objArticle.ArticleID, objFeed.ModuleID)
                        Next

                    End If
                End If
            Next

            For Each objFeed As FeedInfo In objFeeds
                Try
                    Me.ScheduleHistoryItem.AddLogNote(objFeed.Url)     'OPTIONAL
                    ImportFeed(objFeed)
                Catch ex As Exception
                    Me.ScheduleHistoryItem.AddLogNote("News Articles -> Failure to import feed: " + objFeed.Url + ex.ToString())     'OPTIONAL
                End Try
            Next

        End Sub

#End Region

#Region " Constructors "

        Public Sub New(ByVal objScheduleHistoryItem As DotNetNuke.Services.Scheduling.ScheduleHistoryItem)

            MyBase.new()
            Me.ScheduleHistoryItem = objScheduleHistoryItem

        End Sub

#End Region

#Region " Interface Methods "

        Public Overrides Sub DoWork()


            Try
                'notification that the event is progressing
                Me.Progressing()    'OPTIONAL
                ImportFeeds()
                Me.ScheduleHistoryItem.Succeeded = True    'REQUIRED

            Catch exc As Exception    'REQUIRED

                Me.ScheduleHistoryItem.Succeeded = False    'REQUIRED
                Me.ScheduleHistoryItem.AddLogNote("News Articles -> Import RSS job failed. " + exc.ToString)     'OPTIONAL
                'notification that we have errored
                Me.Errored(exc)    'REQUIRED
                'log the exception
                LogException(exc)    'OPTIONAL

            End Try

        End Sub

#End Region

    End Class

End Namespace
