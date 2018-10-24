'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Data

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Search
Imports System.Xml
Imports System.Security.Cryptography
Imports Ventrian.NewsArticles.Components.CustomFields

Namespace Ventrian.NewsArticles

    Public Class ArticleController
        Implements ISearchable
        Implements IPortable

#Region " Private Methods "

        Private Function FillArticleCollection(ByVal dr As IDataReader, ByRef totalRecords As Integer, ByVal maxCount As Integer) As List(Of ArticleInfo)

            Dim objArticles As New List(Of ArticleInfo)
            While dr.Read
                objArticles.Add(CBO.FillObject(Of ArticleInfo)(dr, False))
            End While

            Dim nextResult As Boolean = dr.NextResult()
            totalRecords = 0

            If dr.Read Then
                totalRecords = Convert.ToInt32(Null.SetNull(dr("TotalRecords"), totalRecords))
                If (maxCount <> Null.NullInteger AndAlso maxCount < totalRecords) Then
                    totalRecords = maxCount
                End If
            End If

            If Not dr Is Nothing Then
                dr.Close()
            End If

            Return objArticles

        End Function

#End Region

#Region " Static Methods "

        Public Shared Sub ClearArchiveCache(ByVal moduleID As Integer)

            Dim itemsToRemove As New List(Of String)()

            If (HttpContext.Current IsNot Nothing) Then
                Dim enumerator As IDictionaryEnumerator = HttpContext.Current.Cache.GetEnumerator()
                While enumerator.MoveNext()
                    If enumerator.Key.ToString().ToLower().Contains("ventrian-newsarticles-archive-" & moduleID.ToString()) Then
                        itemsToRemove.Add(enumerator.Key.ToString())
                    End If
                End While

                For Each itemToRemove As String In itemsToRemove
                    DataCache.RemoveCache(itemToRemove.Replace("DNN_", ""))
                Next
            End If

        End Sub

        Public Shared Sub ClearArticleCache(ByVal articleID As Integer)

            Dim cacheKey As String = "ventrian-newsarticles-article-" & articleID.ToString()
            DataCache.RemoveCache(cacheKey)



        End Sub

#End Region

#Region " Public Methods "

        Public Function GetArticleList(ByVal moduleID As Integer) As List(Of ArticleInfo)

            Return GetArticleList(moduleID, True)

        End Function

        Public Function GetArticleList(ByVal moduleID As Integer, ByVal isApproved As Boolean) As List(Of ArticleInfo)

            Return GetArticleList(moduleID, Null.NullDate, Null.NullDate, Nothing, Null.NullBoolean, Nothing, Null.NullInteger, Null.NullInteger, Null.NullInteger, "CreatedDate", "DESC", isApproved, Null.NullBoolean, Null.NullString, Null.NullInteger, Null.NullBoolean, Null.NullBoolean, Null.NullBoolean, Null.NullBoolean, Null.NullBoolean, Null.NullBoolean, Null.NullString, Nothing, False, Null.NullString, Null.NullInteger, Null.NullString, Null.NullString, Null.NullInteger)

        End Function

        Public Function GetArticleList(ByVal moduleID As Integer, ByVal isApproved As Boolean, ByVal sort As String) As List(Of ArticleInfo)

            Return GetArticleList(moduleID, Null.NullDate, Null.NullDate, Nothing, Null.NullBoolean, Nothing, Null.NullInteger, Null.NullInteger, Null.NullInteger, sort, "DESC", isApproved, Null.NullBoolean, Null.NullString, Null.NullInteger, Null.NullBoolean, Null.NullBoolean, Null.NullBoolean, Null.NullBoolean, Null.NullBoolean, Null.NullBoolean, Null.NullString, Nothing, False, Null.NullString, Null.NullInteger, Null.NullString, Null.NullString, Null.NullInteger)

        End Function

        Public Function GetArticleList(ByVal moduleID As Integer, ByVal currentDate As DateTime, ByVal agedDate As DateTime, ByVal categoryID As Integer(), ByVal matchAll As Boolean, ByVal maxCount As Integer, ByVal pageNumber As Integer, ByVal pageSize As Integer, ByVal sortBy As String, ByVal sortDirection As String, ByVal isApproved As Boolean, ByVal isDraft As Boolean, ByVal keywords As String, ByVal authorID As Integer, ByVal showPending As Boolean, ByVal showExpired As Boolean, ByVal showFeaturedOnly As Boolean, ByVal showNotFeaturedOnly As Boolean, ByVal showSecuredOnly As Boolean, ByVal showNotSecuredOnly As Boolean, ByVal articleIDs As String, ByRef totalRecords As Integer) As List(Of ArticleInfo)

            Return GetArticleList(moduleID, currentDate, agedDate, categoryID, matchAll, Nothing, maxCount, pageNumber, pageSize, sortBy, sortDirection, isApproved, isDraft, keywords, authorID, showPending, showExpired, showFeaturedOnly, showNotFeaturedOnly, showSecuredOnly, showNotSecuredOnly, articleIDs, Nothing, False, Null.NullString, Null.NullInteger, Null.NullString, Null.NullString, totalRecords)

        End Function

        Public Function GetArticleList(ByVal moduleID As Integer, ByVal currentDate As DateTime, ByVal agedDate As DateTime, ByVal categoryID As Integer(), ByVal matchAll As Boolean, ByVal categoryIDExclude As Integer(), ByVal maxCount As Integer, ByVal pageNumber As Integer, ByVal pageSize As Integer, ByVal sortBy As String, ByVal sortDirection As String, ByVal isApproved As Boolean, ByVal isDraft As Boolean, ByVal keywords As String, ByVal authorID As Integer, ByVal showPending As Boolean, ByVal showExpired As Boolean, ByVal showFeaturedOnly As Boolean, ByVal showNotFeaturedOnly As Boolean, ByVal showSecuredOnly As Boolean, ByVal showNotSecuredOnly As Boolean, ByVal articleIDs As String, ByVal tagID As Integer(), ByVal matchAllTag As Boolean, ByVal rssGuid As String, ByVal customFieldID As Integer, ByVal customValue As String, ByVal linkFilter As String, ByRef totalRecords As Integer) As List(Of ArticleInfo)

            Return FillArticleCollection(DataProvider.Instance().GetArticleListBySearchCriteria(moduleID, currentDate, agedDate, categoryID, matchAll, categoryIDExclude, maxCount, pageNumber, pageSize, sortBy, sortDirection, isApproved, isDraft, keywords, authorID, showPending, showExpired, showFeaturedOnly, showNotFeaturedOnly, showSecuredOnly, showNotSecuredOnly, articleIDs, tagID, matchAllTag, rssGuid, customFieldID, customValue, linkFilter), totalRecords, maxCount)

        End Function

        Public Function GetArticle(ByVal articleID As Integer) As ArticleInfo

            Dim cacheKey As String = "ventrian-newsarticles-article-" & articleID.ToString()

            Dim objArticle As ArticleInfo = CType(DataCache.GetCache(cacheKey), ArticleInfo)

            If (objArticle Is Nothing) Then
                objArticle = CBO.FillObject(Of ArticleInfo)(DataProvider.Instance().GetArticle(articleID))
                If (objArticle Is Nothing) Then
                    Return Nothing
                End If
                DataCache.SetCache(cacheKey, objArticle)
            End If

            Return objArticle

        End Function

        Public Function GetArticleCategories(ByVal articleID As Integer) As ArrayList

            Dim objArticleCategories As ArrayList = CType(DataCache.GetCache(ArticleConstants.CACHE_CATEGORY_ARTICLE & articleID.ToString()), ArrayList)

            If (objArticleCategories Is Nothing) Then
                objArticleCategories = CBO.FillCollection(DataProvider.Instance().GetArticleCategories(articleID), GetType(CategoryInfo))
                DataCache.SetCache(ArticleConstants.CACHE_CATEGORY_ARTICLE & articleID.ToString(), objArticleCategories)
            End If
            Return objArticleCategories

        End Function

        Public Function GetNewsArchive(ByVal moduleID As Integer, ByVal categoryID As Integer(), ByVal categoryIDExclude As Integer(), ByVal authorID As Integer, ByVal groupBy As GroupByType, ByVal showPending As Boolean) As List(Of ArchiveInfo)

            Dim categories As String = Null.NullString

            If Not (categoryID Is Nothing) Then
                For Each category As Integer In categoryID
                    If Not (categories = Null.NullString) Then
                        categories = categories & ","
                    End If
                    categories = categories & category.ToString()
                Next
            End If

            Dim categoriesExclude As String = Null.NullString

            If Not (categoryIDExclude Is Nothing) Then
                For Each category As Integer In categoryIDExclude
                    If Not (categoriesExclude = Null.NullString) Then
                        categoriesExclude = categoriesExclude & ","
                    End If
                    categoriesExclude = categoriesExclude & category.ToString()
                Next
            End If

            Dim hashCategories As String = ""
            If (categories <> "" Or categoriesExclude <> "") Then
                Dim Ue As New UnicodeEncoding()
                Dim ByteSourceText() As Byte = Ue.GetBytes(categories & categoriesExclude)
                Dim Md5 As New MD5CryptoServiceProvider()
                Dim ByteHash() As Byte = Md5.ComputeHash(ByteSourceText)
                hashCategories = Convert.ToBase64String(ByteHash)
            End If

            Dim cacheKey As String = "ventrian-newsarticles-archive-" & moduleID.ToString() & "-" & hashCategories & "-" & authorID.ToString() & "-" & groupBy.ToString() & "-" & showPending.ToString()

            Dim objArchives As List(Of ArchiveInfo) = CType(DataCache.GetCache(cacheKey), List(Of ArchiveInfo))

            If (objArchives Is Nothing) Then
                objArchives = CBO.FillCollection(Of ArchiveInfo)(DataProvider.Instance().GetNewsArchive(moduleID, categoryID, categoryIDExclude, authorID, groupBy.ToString(), showPending))
                DataCache.SetCache(cacheKey, objArchives)
            End If

            Return objArchives

        End Function

        Public Sub DeleteArticle(ByVal articleID As Integer)

            Dim objArticle As ArticleInfo = GetArticle(articleID)

            If (objArticle IsNot Nothing) Then
                DeleteArticle(articleID, objArticle.ModuleID)
            End If

        End Sub

        Public Sub DeleteArticle(ByVal articleID As Integer, ByVal moduleID As Integer)

            DataProvider.Instance().DeleteArticle(articleID)
            CategoryController.ClearCache(moduleID)
            AuthorController.ClearCache(moduleID)
            ArticleController.ClearArchiveCache(moduleID)
            ArticleController.ClearArticleCache(articleID)

        End Sub

        Public Sub DeleteArticleCategories(ByVal articleID As Integer)

            DataProvider.Instance().DeleteArticleCategories(articleID)
            DataCache.RemoveCache(ArticleConstants.CACHE_CATEGORY_ARTICLE & articleID.ToString())

        End Sub

        Public Function AddArticle(ByVal objArticle As ArticleInfo) As Integer

            Dim articleID As Integer = CType(DataProvider.Instance().AddArticle(objArticle.AuthorID, objArticle.ApproverID, objArticle.CreatedDate, objArticle.LastUpdate, objArticle.Title, objArticle.Summary, objArticle.IsApproved, objArticle.NumberOfViews, objArticle.IsDraft, objArticle.StartDate, objArticle.EndDate, objArticle.ModuleID, objArticle.ImageUrl, objArticle.IsFeatured, objArticle.LastUpdateID, objArticle.Url, objArticle.IsSecure, objArticle.IsNewWindow, objArticle.MetaTitle, objArticle.MetaDescription, objArticle.MetaKeywords, objArticle.PageHeadText, objArticle.ShortUrl, objArticle.RssGuid), Integer)

            CategoryController.ClearCache(objArticle.ModuleID)
            AuthorController.ClearCache(objArticle.ModuleID)
            ArticleController.ClearArchiveCache(objArticle.ModuleID)

            Return articleID

        End Function

        Public Sub AddArticleCategory(ByVal articleID As Integer, ByVal categoryID As Integer)

            DataProvider.Instance().AddArticleCategory(articleID, categoryID)
            DataCache.RemoveCache(ArticleConstants.CACHE_CATEGORY_ARTICLE & articleID.ToString())

        End Sub

        Public Sub UpdateArticle(ByVal objArticle As ArticleInfo)

            DataProvider.Instance().UpdateArticle(objArticle.ArticleID, objArticle.AuthorID, objArticle.ApproverID, objArticle.CreatedDate, objArticle.LastUpdate, objArticle.Title, objArticle.Summary, objArticle.IsApproved, objArticle.NumberOfViews, objArticle.IsDraft, objArticle.StartDate, objArticle.EndDate, objArticle.ModuleID, objArticle.ImageUrl, objArticle.IsFeatured, objArticle.LastUpdateID, objArticle.Url, objArticle.IsSecure, objArticle.IsNewWindow, objArticle.MetaTitle, objArticle.MetaDescription, objArticle.MetaKeywords, objArticle.PageHeadText, objArticle.ShortUrl, objArticle.RssGuid)

            CategoryController.ClearCache(objArticle.ModuleID)
            AuthorController.ClearCache(objArticle.ModuleID)
            ArticleController.ClearArchiveCache(objArticle.ModuleID)
            ArticleController.ClearArticleCache(objArticle.ArticleID)

        End Sub

        Public Sub UpdateArticleCount(ByVal articleID As Integer, ByVal count As Integer)

            DataProvider.Instance().UpdateArticleCount(articleID, count)

        End Sub

        Public Function SecureCheck(ByVal portalID As Integer, ByVal articleID As Integer, ByVal userID As Integer) As Boolean

            Return DataProvider.Instance().SecureCheck(portalID, articleID, userID)

        End Function

#End Region

        Private Function GetTabModuleSettings(ByVal TabModuleId As Integer, ByVal settings As Hashtable) As Hashtable

            Dim dr As IDataReader = DotNetNuke.Data.DataProvider.Instance().GetTabModuleSettings(TabModuleId)

            While dr.Read()

                If Not dr.IsDBNull(1) Then
                    settings(dr.GetString(0)) = dr.GetString(1)
                Else
                    settings(dr.GetString(0)) = ""
                End If

            End While

            dr.Close()

            Return settings

        End Function

#Region " Optional Interfaces "

        Public Function GetSearchItems(ByVal ModInfo As DotNetNuke.Entities.Modules.ModuleInfo) As DotNetNuke.Services.Search.SearchItemInfoCollection Implements DotNetNuke.Entities.Modules.ISearchable.GetSearchItems

            Dim settings As Hashtable = Common.GetModuleSettings(ModInfo.ModuleID)
            settings = GetTabModuleSettings(ModInfo.TabModuleID, settings)

            Dim doSearch As Boolean = False

            If (settings.Contains(ArticleConstants.ENABLE_CORE_SEARCH_SETTING)) Then
                doSearch = Convert.ToBoolean(settings(ArticleConstants.ENABLE_CORE_SEARCH_SETTING))
            End If

            Dim SearchItemCollection As New SearchItemInfoCollection

            If (doSearch) Then

                Dim objArticles As List(Of ArticleInfo) = GetArticleList(ModInfo.ModuleID)

                For Each objArticle As ArticleInfo In objArticles
                    With CType(objArticle, ArticleInfo)

                        If (objArticle.IsApproved) Then

                            Dim objPageController As New PageController
                            Dim objPages As ArrayList = objPageController.GetPageList(objArticle.ArticleID)
                            For i As Integer = 0 To objPages.Count - 1
                                Dim SearchItem As SearchItemInfo
                                Dim objPage As PageInfo = CType(objPages(i), PageInfo)
                                Dim pageContent As String = HttpUtility.HtmlDecode(objArticle.Title) + " " + System.Web.HttpUtility.HtmlDecode(objPage.PageText)

                                For Each Item As DictionaryEntry In objArticle.CustomList
                                    If (Item.Value.ToString() <> "") Then
                                        pageContent = pageContent & vbCrLf & Item.Value.ToString()
                                    End If
                                Next

                                Dim pageDescription As String = HtmlUtils.Shorten(HtmlUtils.Clean(System.Web.HttpUtility.HtmlDecode(objPage.PageText), False), 100, "...")

                                Dim title As String = objArticle.Title & " - " & objPage.Title
                                If (objArticle.Title = objPage.Title) Then
                                    title = objArticle.Title
                                End If
                                If (i = 0) Then
                                    SearchItem = New SearchItemInfo(title, pageDescription, objArticle.AuthorID, objArticle.LastUpdate, ModInfo.ModuleID, ModInfo.ModuleID.ToString() & "_" & .ArticleID.ToString & "_" & objPage.PageID.ToString, pageContent, "ArticleType=ArticleView&ArticleID=" & .ArticleID.ToString)
                                Else
                                    SearchItem = New SearchItemInfo(title, pageDescription, objArticle.AuthorID, objArticle.LastUpdate, ModInfo.ModuleID, ModInfo.ModuleID.ToString() & "_" & .ArticleID.ToString & "_" & objPage.PageID.ToString, pageContent, "ArticleType=ArticleView&ArticleID=" & .ArticleID.ToString & "&PageID=" + objPage.PageID.ToString())
                                End If
                                SearchItemCollection.Add(SearchItem)
                            Next

                        End If

                    End With
                Next

            End If

            Return SearchItemCollection

        End Function

        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements IPortable.ExportModule

            Dim strXML As String = ""
            strXML += WriteCategories(ModuleID, Null.NullInteger)
            strXML += WriteTags(ModuleID)
            strXML += WriteCustomFields(ModuleID)
            strXML += WriteArticles(ModuleID)
            Return strXML

        End Function

        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserId As Integer) Implements IPortable.ImportModule

            Dim objXmlDocument As New XmlDocument()
            objXmlDocument.LoadXml("<xml>" & Content & "</xml>")

            For Each xmlChildNode As XmlNode In objXmlDocument.ChildNodes(0).ChildNodes
                If (xmlChildNode.Name = "categories") Then
                    Dim sortOrder As Integer = 0
                    For Each xmlCategory As XmlNode In xmlChildNode.ChildNodes
                        ReadCategory(ModuleID, xmlCategory, Null.NullInteger, sortOrder)
                        sortOrder = sortOrder + 1
                    Next
                End If

                If (xmlChildNode.Name = "tags") Then
                    For Each xmlTag As XmlNode In xmlChildNode.ChildNodes
                        ReadTag(ModuleID, xmlTag)
                    Next
                End If

                If (xmlChildNode.Name = "customfields") Then
                    For Each xmlCustomField As XmlNode In xmlChildNode.ChildNodes
                        ReadCustomField(ModuleID, xmlCustomField)
                    Next
                End If

                If (xmlChildNode.Name = "articles") Then
                    For Each xmlArticle As XmlNode In xmlChildNode.ChildNodes
                        ReadArticle(ModuleID, xmlArticle)
                    Next
                End If
            Next

        End Sub

        Private Function WriteCategories(ByVal ModuleID As Integer, ByVal parentID As Integer) As String

            Dim strXML As String = ""

            Dim objCategoryController As New CategoryController
            Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategories(ModuleID, parentID)

            If objCategories.Count <> 0 Then
                strXML += "<categories>"
                For Each objCategory As CategoryInfo In objCategories
                    strXML += "<category>"
                    strXML += "<name>" & XmlUtils.XMLEncode(objCategory.Name) & "</name>"
                    strXML += "<description>" & XmlUtils.XMLEncode(objCategory.Description) & "</description>"
                    strXML += "<metaTitle>" & XmlUtils.XMLEncode(objCategory.MetaTitle) & "</metaTitle>"
                    strXML += "<metaDescription>" & XmlUtils.XMLEncode(objCategory.MetaDescription) & "</metaDescription>"
                    strXML += "<metaKeywords>" & XmlUtils.XMLEncode(objCategory.MetaKeywords) & "</metaKeywords>"
                    strXML += WriteCategories(ModuleID, objCategory.CategoryID)
                    strXML += "</category>"
                Next
                strXML += "</categories>"
            End If

            Return strXML

        End Function

        Public Sub ReadCategory(ByVal ModuleID As Integer, ByVal xmlCategory As XmlNode, ByVal parentCategoryID As Integer, ByVal sortOrder As Integer)

            Dim objCategory As New CategoryInfo
            objCategory.ParentID = parentCategoryID
            objCategory.ModuleID = ModuleID
            objCategory.Name = xmlCategory.Item("name").InnerText
            objCategory.Description = xmlCategory.Item("description").InnerText

            objCategory.InheritSecurity = True
            objCategory.CategorySecurityType = CategorySecurityType.Loose

            objCategory.MetaTitle = xmlCategory.Item("metaTitle").InnerText
            objCategory.MetaDescription = xmlCategory.Item("metaDescription").InnerText
            objCategory.MetaKeywords = xmlCategory.Item("metaKeywords").InnerText

            objCategory.Image = Null.NullString()
            objCategory.SortOrder = sortOrder

            Dim objCategoryController As New CategoryController
            objCategory.CategoryID = objCategoryController.AddCategory(objCategory)

            Dim childSortOrder As Integer = 0
            For Each xmlChildNode As XmlNode In xmlCategory.ChildNodes
                If (xmlChildNode.Name.ToLower() = "categories") Then
                    For Each xmlChildCategory As XmlNode In xmlChildNode.ChildNodes
                        ReadCategory(ModuleID, xmlChildCategory, objCategory.CategoryID, childSortOrder)
                    Next
                End If
                childSortOrder = childSortOrder + 1
            Next

        End Sub

        Private Function WriteTags(ByVal ModuleID As Integer) As String

            Dim strXML As String = ""

            Dim objTagController As New TagController()
            Dim objTags As ArrayList = objTagController.List(ModuleID, Null.NullInteger)

            If objTags.Count <> 0 Then
                strXML += "<tags>"
                For Each objTag As TagInfo In objTags
                    strXML += "<tag>"
                    strXML += "<name>" & XmlUtils.XMLEncode(objTag.Name) & "</name>"
                    strXML += "<usage>" & XmlUtils.XMLEncode(objTag.Usages.ToString()) & "</usage>"
                    strXML += "</tag>"
                Next
                strXML += "</tags>"
            End If

            Return strXML

        End Function

        Public Sub ReadTag(ByVal ModuleID As Integer, ByVal xmlTag As XmlNode)

            Dim objTag As New TagInfo

            objTag.ModuleID = ModuleID
            objTag.Name = xmlTag.Item("name").InnerText
            objTag.NameLowered = objTag.Name.ToLower()
            objTag.Usages = Convert.ToInt32(xmlTag.Item("usage").InnerText)

            Dim objTagController As New TagController
            objTagController.Add(objTag)

        End Sub

        Private Function WriteCustomFields(ByVal ModuleID As Integer) As String

            Dim strXML As String = ""

            Dim objCustomFieldController As New CustomFieldController()
            Dim objCustomFields = objCustomFieldController.List(ModuleID)

            If objCustomFields.Count <> 0 Then
                strXML += "<customfields>"
                For Each objCustomField As CustomFieldInfo In objCustomFields
                    strXML += "<customfield>"
                    strXML += "<name>" & XmlUtils.XMLEncode(objCustomField.Name) & "</name>"
                    strXML += "<fieldtype>" & XmlUtils.XMLEncode(CType(objCustomField.FieldType, Integer)) & "</fieldtype>"
                    strXML += "<fieldelements>" & XmlUtils.XMLEncode(objCustomField.FieldElements) & "</fieldelements>"
                    strXML += "<defaultvalue>" & XmlUtils.XMLEncode(objCustomField.DefaultValue) & "</defaultvalue>"
                    strXML += "<caption>" & XmlUtils.XMLEncode(objCustomField.Caption) & "</caption>"
                    strXML += "<captionhelp>" & XmlUtils.XMLEncode(objCustomField.CaptionHelp) & "</captionhelp>"
                    strXML += "<isrequired>" & XmlUtils.XMLEncode(objCustomField.IsRequired) & "</isrequired>"
                    strXML += "<isvisible>" & XmlUtils.XMLEncode(objCustomField.IsVisible) & "</isvisible>"
                    strXML += "<sortorder>" & XmlUtils.XMLEncode(objCustomField.SortOrder) & "</sortorder>"
                    strXML += "<validationType>" & XmlUtils.XMLEncode(CType(objCustomField.ValidationType, Integer)) & "</validationType>"
                    strXML += "<regularexpression>" & XmlUtils.XMLEncode(objCustomField.RegularExpression) & "</regularexpression>"
                    strXML += "<length>" & XmlUtils.XMLEncode(objCustomField.Length) & "</length>"
                    strXML += "</customfield>"
                Next
                strXML += "</customfields>"
            End If

            Return strXML

        End Function

        Public Sub ReadCustomField(ByVal ModuleID As Integer, ByVal xmlCustomField As XmlNode)

            Dim objCustomField As New CustomFieldInfo

            objCustomField.ModuleID = ModuleID
            objCustomField.Name = xmlCustomField.Item("name").InnerText
            objCustomField.FieldType = [Enum].Parse(GetType(CustomFieldType), xmlCustomField.Item("fieldtype").InnerText)
            objCustomField.FieldElements = xmlCustomField.Item("fieldelements").InnerText
            objCustomField.DefaultValue = xmlCustomField.Item("defaultvalue").InnerText
            objCustomField.Caption = xmlCustomField.Item("caption").InnerText
            objCustomField.CaptionHelp = xmlCustomField.Item("captionhelp").InnerText
            objCustomField.IsRequired = Convert.ToBoolean(xmlCustomField.Item("isrequired").InnerText)
            objCustomField.IsVisible = Convert.ToBoolean(xmlCustomField.Item("isvisible").InnerText)
            objCustomField.SortOrder = Convert.ToInt32(xmlCustomField.Item("sortorder").InnerText)
            objCustomField.ValidationType = [Enum].Parse(GetType(CustomFieldValidationType), xmlCustomField.Item("validationType").InnerText)
            objCustomField.RegularExpression = xmlCustomField.Item("regularexpression").InnerText
            objCustomField.Length = Convert.ToInt32(xmlCustomField.Item("length").InnerText)

            Dim objCustomFieldController As New CustomFieldController
            objCustomFieldController.Add(objCustomField)

        End Sub

        Private Function WriteArticles(ByVal ModuleID As Integer) As String

            Dim strXML As String = ""

            Dim objCustomFieldController As New CustomFieldController
            Dim objCustomFields As ArrayList = objCustomFieldController.List(ModuleID)

            Dim objArticleController As New ArticleController
            Dim objArticles As List(Of ArticleInfo) = objArticleController.GetArticleList(ModuleID, DateTime.Now, Null.NullDate, Nothing, True, 10000, 1, 10000, "CreatedDate", "DESC", True, False, Null.NullString, Null.NullInteger, True, True, False, False, False, False, Null.NullString, Nothing)

            If objArticles.Count <> 0 Then
                strXML += "<articles>"
                For Each objArticle As ArticleInfo In objArticles
                    strXML += "<article>"
                    strXML += "<createdDate>" & XmlUtils.XMLEncode(objArticle.CreatedDate.ToString("O")) & "</createdDate>"
                    strXML += "<lastUpdate>" & XmlUtils.XMLEncode(objArticle.LastUpdate.ToString("O")) & "</lastUpdate>"
                    strXML += "<title>" & XmlUtils.XMLEncode(objArticle.Title) & "</title>"
                    strXML += "<isApproved>" & XmlUtils.XMLEncode(objArticle.IsApproved.ToString()) & "</isApproved>"
                    strXML += "<numberOfViews>" & XmlUtils.XMLEncode(objArticle.NumberOfViews.ToString()) & "</numberOfViews>"
                    strXML += "<isDraft>" & XmlUtils.XMLEncode(objArticle.IsDraft.ToString()) & "</isDraft>"
                    strXML += "<startDate>" & XmlUtils.XMLEncode(objArticle.StartDate.ToString("O")) & "</startDate>"
                    strXML += "<endDate>" & XmlUtils.XMLEncode(objArticle.EndDate.ToString("O")) & "</endDate>"
                    strXML += "<imageUrl>" & XmlUtils.XMLEncode(objArticle.ImageUrl.ToString()) & "</imageUrl>"
                    strXML += "<isFeatured>" & XmlUtils.XMLEncode(objArticle.IsFeatured.ToString()) & "</isFeatured>"
                    strXML += "<url>" & XmlUtils.XMLEncode(objArticle.Url) & "</url>"
                    strXML += "<isSecure>" & XmlUtils.XMLEncode(objArticle.IsSecure.ToString()) & "</isSecure>"
                    strXML += "<isNewWindow>" & XmlUtils.XMLEncode(objArticle.IsNewWindow.ToString()) & "</isNewWindow>"
                    strXML += "<commentCount>" & XmlUtils.XMLEncode(objArticle.CommentCount.ToString()) & "</commentCount>"
                    strXML += "<pageCount>" & XmlUtils.XMLEncode(objArticle.PageCount.ToString()) & "</pageCount>"
                    strXML += "<fileCount>" & XmlUtils.XMLEncode(objArticle.FileCount.ToString()) & "</fileCount>"
                    strXML += "<imageCount>" & XmlUtils.XMLEncode(objArticle.ImageCount.ToString()) & "</imageCount>"
                    If (objArticle.Rating <> Null.NullDouble) Then
                        strXML += "<rating>" & XmlUtils.XMLEncode(objArticle.Rating.ToString()) & "</rating>"
                    End If
                    strXML += "<ratingCount>" & XmlUtils.XMLEncode(objArticle.RatingCount.ToString()) & "</ratingCount>"
                    strXML += "<summary>" & XmlUtils.XMLEncode(objArticle.Url) & "</summary>"
                    strXML += "<metaTitle>" & XmlUtils.XMLEncode(objArticle.MetaTitle) & "</metaTitle>"
                    strXML += "<metaDescription>" & XmlUtils.XMLEncode(objArticle.MetaDescription) & "</metaDescription>"
                    strXML += "<metaKeywords>" & XmlUtils.XMLEncode(objArticle.MetaKeywords) & "</metaKeywords>"
                    strXML += "<pageHeadText>" & XmlUtils.XMLEncode(objArticle.PageHeadText) & "</pageHeadText>"
                    strXML += "<shortUrl>" & XmlUtils.XMLEncode(objArticle.ShortUrl) & "</shortUrl>"

                    Dim objArticleCategories As ArrayList = objArticleController.GetArticleCategories(objArticle.ArticleID)
                    If (objArticleCategories.Count > 0) Then
                        strXML += "<categories>"
                        For Each objCategory As CategoryInfo In objArticleCategories
                            strXML += "<category>"
                            strXML += "<name>" & XmlUtils.XMLEncode(objCategory.Name) & "</name>"
                            strXML += "</category>"
                        Next
                        strXML += "</categories>"
                    End If

                    If (objArticle.Tags <> "") Then
                        strXML += "<tags>"
                        For Each tag As String In objArticle.Tags.Split(","c)
                            strXML += "<tag>"
                            strXML += "<name>" & XmlUtils.XMLEncode(tag) & "</name>"
                            strXML += "</tag>"
                        Next
                        strXML += "</tags>"
                    End If

                    If (objArticle.CustomList.Count > 0) Then
                        strXML += "<customfields>"
                        For Each item As DictionaryEntry In objArticle.CustomList

                            For Each objCustomField As CustomFieldInfo In objCustomFields
                                If (objCustomField.CustomFieldID = item.Key) Then
                                    strXML += "<customfield>"
                                    strXML += "<name>" & XmlUtils.XMLEncode(objCustomField.Name) & "</name>"
                                    strXML += "<value>" & XmlUtils.XMLEncode(item.Value) & "</value>"
                                    strXML += "</customfield>"
                                    Exit For
                                End If
                            Next

                        Next
                        strXML += "</customfields>"
                    End If

                    If (objArticle.PageCount > 0) Then
                        Dim objPageController As New PageController
                        Dim objPages As ArrayList = objPageController.GetPageList(objArticle.ArticleID)

                        If (objPages.Count > 0) Then
                            strXML += "<pages>"
                            For Each objPage As PageInfo In objPages
                                strXML += "<page>"
                                strXML += "<title>" & XmlUtils.XMLEncode(objPage.Title) & "</title>"
                                strXML += "<pageText>" & XmlUtils.XMLEncode(objPage.PageText) & "</pageText>"
                                strXML += "</page>"
                            Next
                            strXML += "</pages>"
                        End If
                    End If

                    If (objArticle.ImageCount > 0) Then
                        Dim objImageController As New ImageController
                        Dim objImages As List(Of ImageInfo) = objImageController.GetImageList(objArticle.ArticleID, Null.NullString())

                        If (objImages.Count > 0) Then
                            strXML += "<images>"
                            For Each objImage As ImageInfo In objImages
                                strXML += "<image>"
                                strXML += "<title>" & XmlUtils.XMLEncode(objImage.Title) & "</title>"
                                strXML += "<filename>" & XmlUtils.XMLEncode(objImage.FileName) & "</filename>"
                                strXML += "<extension>" & XmlUtils.XMLEncode(objImage.Extension) & "</extension>"
                                strXML += "<size>" & XmlUtils.XMLEncode(objImage.Size.ToString()) & "</size>"
                                strXML += "<width>" & XmlUtils.XMLEncode(objImage.Width.ToString()) & "</width>"
                                strXML += "<height>" & XmlUtils.XMLEncode(objImage.Height.ToString()) & "</height>"
                                strXML += "<contentType>" & XmlUtils.XMLEncode(objImage.ContentType) & "</contentType>"
                                strXML += "<folder>" & XmlUtils.XMLEncode(objImage.Folder) & "</folder>"
                                strXML += "</image>"
                            Next
                            strXML += "</images>"
                        End If
                    End If

                    If (objArticle.FileCount > 0) Then
                        Dim objFileController As New FileController
                        Dim objFiles As List(Of FileInfo) = objFileController.GetFileList(objArticle.ArticleID, Null.NullString)

                        If (objFiles.Count > 0) Then
                            strXML += "<files>"
                            For Each objFile As FileInfo In objFiles
                                strXML += "<file>"
                                strXML += "<title>" & XmlUtils.XMLEncode(objFile.Title) & "</title>"
                                strXML += "<filename>" & XmlUtils.XMLEncode(objFile.FileName) & "</filename>"
                                strXML += "<extension>" & XmlUtils.XMLEncode(objFile.Extension) & "</extension>"
                                strXML += "<size>" & XmlUtils.XMLEncode(objFile.Size.ToString()) & "</size>"
                                strXML += "<contentType>" & XmlUtils.XMLEncode(objFile.ContentType) & "</contentType>"
                                strXML += "<folder>" & XmlUtils.XMLEncode(objFile.Folder) & "</folder>"
                                strXML += "</file>"
                            Next
                            strXML += "</files>"
                        End If
                    End If

                    If (objArticle.CommentCount > 0) Then
                        Dim objCommentController As New CommentController
                        Dim objComments As List(Of CommentInfo) = objCommentController.GetCommentList(ModuleID, objArticle.ArticleID, True)

                        If (objComments.Count > 0) Then
                            strXML += "<comments>"
                            For Each objComment As CommentInfo In objComments
                                strXML += "<comment>"
                                strXML += "<createdDate>" & XmlUtils.XMLEncode(objArticle.CreatedDate.ToString("O")) & "</createdDate>"
                                strXML += "<commentText>" & XmlUtils.XMLEncode(objComment.Comment) & "</commentText>"
                                strXML += "<remoteAddress>" & XmlUtils.XMLEncode(objComment.RemoteAddress) & "</remoteAddress>"
                                strXML += "<type>" & XmlUtils.XMLEncode(objComment.Type.ToString()) & "</type>"
                                strXML += "<trackbackUrl>" & XmlUtils.XMLEncode(objComment.TrackbackUrl) & "</trackbackUrl>"
                                strXML += "<trackbackTitle>" & XmlUtils.XMLEncode(objComment.TrackbackTitle) & "</trackbackTitle>"
                                strXML += "<trackbackBlogName>" & XmlUtils.XMLEncode(objComment.TrackbackBlogName) & "</trackbackBlogName>"
                                strXML += "<trackbackExcerpt>" & XmlUtils.XMLEncode(objComment.TrackbackExcerpt) & "</trackbackExcerpt>"
                                strXML += "<anonymousName>" & XmlUtils.XMLEncode(objComment.AnonymousName) & "</anonymousName>"
                                strXML += "<anonymousEmail>" & XmlUtils.XMLEncode(objComment.AnonymousEmail) & "</anonymousEmail>"
                                strXML += "<anonymousUrl>" & XmlUtils.XMLEncode(objComment.AnonymousURL) & "</anonymousUrl>"
                                strXML += "<notifyMe>" & XmlUtils.XMLEncode(objComment.NotifyMe.ToString()) & "</notifyMe>"
                                strXML += "</comment>"
                            Next
                            strXML += "</comments>"
                        End If
                    End If

                    strXML += "</article>"
                Next
                strXML += "</articles>"
            End If

            Return strXML

        End Function

        Public Sub ReadArticle(ByVal ModuleID As Integer, ByVal xmlArticle As XmlNode)

            Dim objCustomFieldController As New CustomFieldController
            Dim objCustomFields As ArrayList = objCustomFieldController.List(ModuleID)

            Dim objArticle As New ArticleInfo

            objArticle.ModuleID = ModuleID

            objArticle.CreatedDate = DateTime.Parse(xmlArticle.Item("createdDate").InnerText)
            objArticle.LastUpdate = DateTime.Parse(xmlArticle.Item("lastUpdate").InnerText)

            objArticle.Title = xmlArticle.Item("title").InnerText
            objArticle.IsApproved = Convert.ToBoolean(xmlArticle.Item("isApproved").InnerText)
            objArticle.NumberOfViews = Convert.ToInt32(xmlArticle.Item("numberOfViews").InnerText)
            objArticle.IsDraft = Convert.ToBoolean(xmlArticle.Item("isDraft").InnerText)

            objArticle.StartDate = DateTime.Parse(xmlArticle.Item("startDate").InnerText)
            objArticle.EndDate = DateTime.Parse(xmlArticle.Item("endDate").InnerText)

            objArticle.ImageUrl = xmlArticle.Item("imageUrl").InnerText
            objArticle.IsFeatured = Convert.ToBoolean(xmlArticle.Item("isFeatured").InnerText)
            objArticle.Url = xmlArticle.Item("url").InnerText
            objArticle.IsSecure = Convert.ToBoolean(xmlArticle.Item("isSecure").InnerText)
            objArticle.IsNewWindow = Convert.ToBoolean(xmlArticle.Item("isNewWindow").InnerText)

            objArticle.CommentCount = Convert.ToInt32(xmlArticle.Item("commentCount").InnerText)
            objArticle.PageCount = Convert.ToInt32(xmlArticle.Item("pageCount").InnerText)
            objArticle.FileCount = Convert.ToInt32(xmlArticle.Item("fileCount").InnerText)
            objArticle.ImageCount = Convert.ToInt32(xmlArticle.Item("imageCount").InnerText)

            objArticle.Rating = Null.NullInteger
            objArticle.RatingCount = 0
            objArticle.Summary = xmlArticle.Item("summary").InnerText

            objArticle.MetaTitle = xmlArticle.Item("metaTitle").InnerText
            objArticle.MetaDescription = xmlArticle.Item("metaDescription").InnerText
            objArticle.MetaKeywords = xmlArticle.Item("metaKeywords").InnerText
            objArticle.PageHeadText = xmlArticle.Item("pageHeadText").InnerText
            objArticle.ShortUrl = xmlArticle.Item("shortUrl").InnerText

            Dim objArticleController As New ArticleController
            objArticle.ArticleID = objArticleController.AddArticle(objArticle)

            Dim objCategoryController As New CategoryController
            Dim objCategories As List(Of CategoryInfo) = objCategoryController.GetCategoriesAll(ModuleID, Null.NullInteger)
            For Each xmlChildNode As XmlNode In xmlArticle.ChildNodes

                If (xmlChildNode.Name = "categories") Then
                    For Each xmlCategoryNode As XmlNode In xmlChildNode.ChildNodes
                        Dim name As String = xmlCategoryNode.Item("name").InnerText
                        For Each objCategory As CategoryInfo In objCategories
                            If (objCategory.Name.ToLower() = name.ToLower()) Then
                                objArticleController.AddArticleCategory(objArticle.ArticleID, objCategory.CategoryID)
                                Exit For
                            End If
                        Next
                    Next
                End If

                If (xmlChildNode.Name = "tags") Then
                    For Each xmlTagNode As XmlNode In xmlChildNode.ChildNodes
                        Dim name As String = xmlTagNode.Item("name").InnerText
                        Dim objTagController As New TagController()
                        Dim objTags As ArrayList = objTagController.List(ModuleID, Null.NullInteger)
                        For Each objTag As TagInfo In objTags
                            If (objTag.Name.ToLower() = name.ToLower()) Then
                                objTagController.Add(objArticle.ArticleID, objTag.TagID)
                                Exit For
                            End If
                        Next
                    Next
                End If

                If (xmlChildNode.Name = "customfields") Then
                    For Each xmlTagNode As XmlNode In xmlChildNode.ChildNodes
                        
                        Dim name As String = xmlTagNode.Item("name").InnerText
                        Dim value As String = xmlTagNode.Item("value").InnerText

                        For Each objCustomField As CustomFieldInfo In objCustomFields
                            If (objCustomField.Name.ToLower() = name.ToLower()) Then
                                Dim objCustomValue As New CustomValueInfo
                                objCustomValue.CustomFieldID = objCustomField.CustomFieldID
                                objCustomValue.ArticleID = objArticle.ArticleID
                                objCustomValue.CustomValue = value

                                Dim objCustomValueController As New CustomValueController()
                                objCustomValueController.Add(objCustomValue)
                                Exit For
                            End If
                        Next
                    Next
                End If

                If (xmlChildNode.Name = "images") Then
                    Dim sortOrder As Integer = 0
                    For Each xmlImageNode As XmlNode In xmlChildNode.ChildNodes
                        Dim objImage As New ImageInfo

                        objImage.ArticleID = objArticle.ArticleID
                        objImage.Title = xmlImageNode.Item("title").InnerText
                        objImage.FileName = xmlImageNode.Item("filename").InnerText
                        objImage.Extension = xmlImageNode.Item("extension").InnerText
                        objImage.Size = Convert.ToInt32(xmlImageNode.Item("size").InnerText)
                        objImage.Width = Convert.ToInt32(xmlImageNode.Item("width").InnerText)
                        objImage.Height = Convert.ToInt32(xmlImageNode.Item("height").InnerText)
                        objImage.ContentType = xmlImageNode.Item("contentType").InnerText
                        objImage.Folder = xmlImageNode.Item("folder").InnerText
                        objImage.SortOrder = sortOrder

                        Dim objImageController As New ImageController()
                        objImageController.Add(objImage)

                        sortOrder = sortOrder + 1
                    Next
                    If (sortOrder > 0) Then
                        objArticle.ImageCount = sortOrder
                        objArticleController.UpdateArticle(objArticle)
                    End If
                End If

                If (xmlChildNode.Name = "files") Then
                    Dim sortOrder As Integer = 0
                    For Each xmlImageNode As XmlNode In xmlChildNode.ChildNodes
                        Dim objFile As New FileInfo

                        objFile.ArticleID = objArticle.ArticleID
                        objFile.Title = xmlImageNode.Item("title").InnerText
                        objFile.FileName = xmlImageNode.Item("filename").InnerText
                        objFile.Extension = xmlImageNode.Item("extension").InnerText
                        objFile.Size = Convert.ToInt32(xmlImageNode.Item("size").InnerText)
                        objFile.ContentType = xmlImageNode.Item("contentType").InnerText
                        objFile.Folder = xmlImageNode.Item("folder").InnerText
                        objFile.SortOrder = sortOrder

                        Dim objFileController As New FileController()
                        objFileController.Add(objFile)

                        sortOrder = sortOrder + 1
                    Next
                    If (sortOrder > 0) Then
                        objArticle.FileCount = sortOrder
                        objArticleController.UpdateArticle(objArticle)
                    End If
                End If

                If (xmlChildNode.Name = "pages") Then
                    Dim sortOrder As Integer = 0
                    For Each xmlImageNode As XmlNode In xmlChildNode.ChildNodes

                        Dim objPage As New PageInfo

                        objPage.ArticleID = objArticle.ArticleID
                        objPage.Title = xmlImageNode.Item("title").InnerText
                        objPage.PageText = xmlImageNode.Item("pageText").InnerText
                        objPage.SortOrder = sortOrder

                        Dim objPageController As New PageController()
                        objPageController.AddPage(objPage)

                        sortOrder = sortOrder + 1
                    Next
                    If (sortOrder > 0) Then
                        objArticle.PageCount = sortOrder
                        objArticleController.UpdateArticle(objArticle)
                    End If
                End If

                If (xmlChildNode.Name = "comments") Then
                    Dim sortOrder As Integer = 0
                    For Each xmlImageNode As XmlNode In xmlChildNode.ChildNodes

                        Dim objComment As New CommentInfo

                        objComment.UserID = Null.NullInteger
                        objComment.ArticleID = objArticle.ArticleID
                        objComment.CreatedDate = DateTime.Parse(xmlImageNode.Item("createdDate").InnerText)
                        objComment.Comment = xmlImageNode.Item("commentText").InnerText
                        objComment.RemoteAddress = xmlImageNode.Item("remoteAddress").InnerText
                        objComment.Type = Convert.ToInt32(xmlImageNode.Item("type").InnerText)
                        objComment.TrackbackUrl = xmlImageNode.Item("trackbackUrl").InnerText
                        objComment.TrackbackTitle = xmlImageNode.Item("trackbackTitle").InnerText
                        objComment.TrackbackBlogName = xmlImageNode.Item("trackbackBlogName").InnerText
                        objComment.TrackbackExcerpt = xmlImageNode.Item("trackbackExcerpt").InnerText
                        objComment.AnonymousName = xmlImageNode.Item("anonymousName").InnerText
                        objComment.AnonymousEmail = xmlImageNode.Item("anonymousEmail").InnerText
                        objComment.AnonymousURL = xmlImageNode.Item("anonymousUrl").InnerText
                        objComment.NotifyMe = Convert.ToBoolean(xmlImageNode.Item("notifyMe").InnerText)
                        objComment.IsApproved = True
                        objComment.ApprovedBy = Null.NullInteger

                        Dim objCommentController As New CommentController()
                        objCommentController.AddComment(objComment)

                        sortOrder = sortOrder + 1
                    Next
                    If (sortOrder > 0) Then
                        objArticle.CommentCount = sortOrder
                        objArticleController.UpdateArticle(objArticle)
                    End If
                End If
            Next

        End Sub

#End Region

    End Class

End Namespace
