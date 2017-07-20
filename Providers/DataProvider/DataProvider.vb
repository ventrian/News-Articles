'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Web.Caching
Imports System.Reflection

Imports DotNetNuke
Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles

    Public MustInherit Class DataProvider

#Region " Shared/Static Methods "

        ' singleton reference to the instantiated object 
        Private Shared objProvider As DataProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            objProvider = CType(Framework.Reflection.CreateObject("data", "Ventrian.NewsArticles", "Ventrian.NewsArticles"), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region " Abstract methods "

        Public MustOverride Function GetArticleListByApproved(ByVal moduleID As Integer, ByVal isApproved As Boolean) As IDataReader
        Public MustOverride Function GetArticleListBySearchCriteria(ByVal moduleID As Integer, ByVal currentDate As DateTime, ByVal agedDate As DateTime, ByVal categoryID As Integer(), ByVal matchAll As Boolean, ByVal categoryIDExclude As Integer(), ByVal maxCount As Integer, ByVal pageNumber As Integer, ByVal pageSize As Integer, ByVal sortBy As String, ByVal sortDirection As String, ByVal isApproved As Boolean, ByVal isDraft As Boolean, ByVal keywords As String, ByVal authorID As Integer, ByVal showPending As Boolean, ByVal showExpired As Boolean, ByVal showFeaturedOnly As Boolean, ByVal showNotFeaturedOnly As Boolean, ByVal showSecuredOnly As Boolean, ByVal showNotSecuredOnly As Boolean, ByVal articleIDs As String, ByVal tagID As Integer(), ByVal matchAllTag As Boolean, ByVal rssGuid As String, ByVal customFieldID As Integer, ByVal customValue As String, ByVal linkFilter As String) As IDataReader
        Public MustOverride Function GetArticle(ByVal articleID As Integer) As IDataReader
        Public MustOverride Function GetArticleCategories(ByVal articleID As Integer) As IDataReader
        Public MustOverride Function GetNewsArchive(ByVal moduleID As Integer, ByVal categoryID As Integer(), ByVal categoryIDExclude As Integer(), ByVal authorID As Integer, ByVal groupBy As String, ByVal showPending As Boolean) As IDataReader
        Public MustOverride Sub DeleteArticle(ByVal articleID As Integer)
        Public MustOverride Sub DeleteArticleCategories(ByVal articleID As Integer)
        Public MustOverride Function AddArticle(ByVal authorID As Integer, ByVal approverID As Integer, ByVal createdDate As DateTime, ByVal lastUpdate As DateTime, ByVal title As String, ByVal summary As String, ByVal isApproved As Boolean, ByVal numberOfViews As Integer, ByVal isDraft As Boolean, ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal moduleID As Integer, ByVal imageUrl As String, ByVal isFeatured As Boolean, ByVal lastUpdateID As Integer, ByVal url As String, ByVal isSecure As Boolean, ByVal isNewWindow As Boolean, ByVal metaTitle As String, ByVal metaDescription As String, ByVal metaKeywords As String, ByVal pageHeadText As String, ByVal shortUrl As String, ByVal rssGuid As String) As Integer
        Public MustOverride Sub AddArticleCategory(ByVal articleID As Integer, ByVal categoryID As Integer)
        Public MustOverride Sub UpdateArticle(ByVal articleID As Integer, ByVal authorID As Integer, ByVal approverID As Integer, ByVal createdDate As DateTime, ByVal lastUpdate As DateTime, ByVal title As String, ByVal summary As String, ByVal isApproved As Boolean, ByVal numberOfViews As Integer, ByVal isDraft As Boolean, ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal moduleID As Integer, ByVal imageUrl As String, ByVal isFeatured As Boolean, ByVal lastUpdateID As Integer, ByVal url As String, ByVal isSecure As Boolean, ByVal isNewWindow As Boolean, ByVal metaTitle As String, ByVal metaDescription As String, ByVal metaKeywords As String, ByVal pageHeadText As String, ByVal shortUrl As String, ByVal rssGuid As String)
        Public MustOverride Sub UpdateArticleCount(ByVal articleID As Integer, ByVal numberOfViews As Integer)

        Public MustOverride Function SecureCheck(ByVal portalID As Integer, ByVal articleID As Integer, ByVal userID As Integer) As Boolean

        Public MustOverride Function GetAuthorList(ByVal moduleID As Integer) As IDataReader
        Public MustOverride Function GetAuthorStatistics(ByVal moduleID As Integer, ByVal categoryID As Integer(), ByVal categoryIDExclude As Integer(), ByVal authorID As Integer, ByVal sortBy As String, ByVal showPending As Boolean) As IDataReader

        Public MustOverride Function GetCategoryList(ByVal moduleID As Integer, ByVal parentID As Integer) As IDataReader
        Public MustOverride Function GetCategoryListAll(ByVal moduleID As Integer, ByVal authorID As Integer, ByVal showPending As Boolean, ByVal sortType As Integer) As IDataReader
        Public MustOverride Function GetCategory(ByVal categoryID As Integer) As IDataReader
        Public MustOverride Sub DeleteCategory(ByVal categoryID As Integer)
        Public MustOverride Function AddCategory(ByVal moduleID As Integer, ByVal parentID As Integer, ByVal name As String, ByVal image As String, ByVal description As String, ByVal sortOrder As Integer, ByVal inheritSecurity As Boolean, ByVal categorySecurityType As Integer, ByVal metaTitle As String, ByVal metaDescription As String, ByVal metaKeywords As String) As Integer
        Public MustOverride Sub UpdateCategory(ByVal categoryID As Integer, ByVal moduleID As Integer, ByVal parentID As Integer, ByVal name As String, ByVal image As String, ByVal description As String, ByVal sortOrder As Integer, ByVal inheritSecurity As Boolean, ByVal categorySecurityType As Integer, ByVal metaTitle As String, ByVal metaDescription As String, ByVal metaKeywords As String)

        Public MustOverride Function GetCommentList(ByVal moduleID As Integer, ByVal articleID As Integer, ByVal isApproved As Boolean, ByVal direction As SortDirection, ByVal maxCount As Integer) As IDataReader
        Public MustOverride Function GetComment(ByVal commentID As Integer) As IDataReader
        Public MustOverride Sub DeleteComment(ByVal commentID As Integer)
        Public MustOverride Function AddComment(ByVal articleID As Integer, ByVal createdDate As DateTime, ByVal userID As Integer, ByVal comment As String, ByVal remoteAddress As String, ByVal type As Integer, ByVal trackbackUrl As String, ByVal trackbackTitle As String, ByVal trackbackBlogName As String, ByVal trackbackExcerpt As String, ByVal anonymousName As String, ByVal anonymousEmail As String, ByVal anonymousURL As String, ByVal notifyMe As Boolean, ByVal isApproved As Boolean, ByVal approvedBy As Integer) As Integer
        Public MustOverride Sub UpdateComment(ByVal commentID As Integer, ByVal articleID As Integer, ByVal userID As Integer, ByVal comment As String, ByVal remoteAddress As String, ByVal type As Integer, ByVal trackbackUrl As String, ByVal trackbackTitle As String, ByVal trackbackBlogName As String, ByVal trackbackExcerpt As String, ByVal anonymousName As String, ByVal anonymousEmail As String, ByVal anonymousURL As String, ByVal notifyMe As Boolean, ByVal isApproved As Boolean, ByVal approvedBy As Integer)

        Public MustOverride Function GetCustomField(ByVal customFieldID As Integer) As IDataReader
        Public MustOverride Function GetCustomFieldList(ByVal moduleID As Integer) As IDataReader
        Public MustOverride Sub DeleteCustomField(ByVal commentID As Integer)
        Public MustOverride Function AddCustomField(ByVal moduleID As Integer, ByVal name As String, ByVal fieldType As Integer, ByVal fieldElements As String, ByVal defaultValue As String, ByVal caption As String, ByVal captionHelp As String, ByVal isRequired As Boolean, ByVal isVisible As Boolean, ByVal sortOrder As Integer, ByVal validationType As Integer, ByVal length As Integer, ByVal regularExpression As String) As Integer
        Public MustOverride Sub UpdateCustomField(ByVal customFieldID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal fieldType As Integer, ByVal fieldElements As String, ByVal defaultValue As String, ByVal caption As String, ByVal captionHelp As String, ByVal isRequired As Boolean, ByVal isVisible As Boolean, ByVal sortOrder As Integer, ByVal validationType As Integer, ByVal length As Integer, ByVal regularExpression As String)

        Public MustOverride Function GetCustomValueList(ByVal articleID As Integer) As IDataReader
        Public MustOverride Function AddCustomValue(ByVal articleID As Integer, ByVal customFieldID As Integer, ByVal customValue As String) As Integer
        Public MustOverride Sub UpdateCustomValue(ByVal customValueID As Integer, ByVal articleID As Integer, ByVal customFieldID As Integer, ByVal customValue As String)
        Public MustOverride Sub DeleteCustomValue(ByVal articleID As Integer, ByVal customFieldID As Integer)

        Public MustOverride Function GetFeed(ByVal feedID As Integer) As IDataReader
        Public MustOverride Function GetFeedList(ByVal moduleID As Integer, ByVal showActiveOnly As Boolean) As IDataReader
        Public MustOverride Function AddFeed(ByVal moduleID As Integer, ByVal title As String, ByVal url As String, ByVal userID As Integer, ByVal autoFeature As Boolean, ByVal isActive As Boolean, ByVal dateMode As Integer, ByVal autoExpire As Integer, ByVal autoExpireUnit As Integer) As Integer
        Public MustOverride Sub UpdateFeed(ByVal feedID As Integer, ByVal moduleID As Integer, ByVal title As String, ByVal url As String, ByVal userID As Integer, ByVal autoFeature As Boolean, ByVal isActive As Boolean, ByVal dateMode As Integer, ByVal autoExpire As Integer, ByVal autoExpireUnit As Integer)
        Public MustOverride Sub DeleteFeed(ByVal feedID As Integer)

        Public MustOverride Function GetFeedCategoryList(ByVal feedID As Integer) As IDataReader
        Public MustOverride Sub AddFeedCategory(ByVal feedID As Integer, ByVal categoryID As Integer)
        Public MustOverride Sub DeleteFeedCategory(ByVal feedID As Integer)

        Public MustOverride Function GetFile(ByVal fileID As Integer) As IDataReader
        Public MustOverride Function GetFileList(ByVal articleID As Integer, ByVal fileGuid As String) As IDataReader
        Public MustOverride Function AddFile(ByVal articleID As Integer, ByVal title As String, ByVal fileName As String, ByVal extension As String, ByVal size As Integer, ByVal contentType As String, ByVal folder As String, ByVal sortOrder As Integer, ByVal fileGuid As String) As Integer
        Public MustOverride Sub UpdateFile(ByVal fileID As Integer, ByVal articleID As Integer, ByVal title As String, ByVal fileName As String, ByVal extension As String, ByVal size As Integer, ByVal contentType As String, ByVal folder As String, ByVal sortOrder As Integer, ByVal fileGuid As String)
        Public MustOverride Sub DeleteFile(ByVal fileID As Integer)

        Public MustOverride Function GetImage(ByVal imageID As Integer) As IDataReader
        Public MustOverride Function GetImageList(ByVal articleID As Integer, ByVal imageGuid As String) As IDataReader
        Public MustOverride Function AddImage(ByVal articleID As Integer, ByVal title As String, ByVal fileName As String, ByVal extension As String, ByVal size As Integer, ByVal width As Integer, ByVal height As Integer, ByVal contentType As String, ByVal folder As String, ByVal sortOrder As Integer, ByVal imageGuid As String, ByVal description As String) As Integer
        Public MustOverride Sub UpdateImage(ByVal imageID As Integer, ByVal articleID As Integer, ByVal title As String, ByVal fileName As String, ByVal extension As String, ByVal size As Integer, ByVal width As Integer, ByVal height As Integer, ByVal contentType As String, ByVal folder As String, ByVal sortOrder As Integer, ByVal imageGuid As String, ByVal description As String)
        Public MustOverride Sub DeleteImage(ByVal imageID As Integer)

        Public MustOverride Sub AddMirrorArticle(ByVal articleID As Integer, ByVal linkedArticleID As Integer, ByVal linkedPortalID As Integer, ByVal autoUpdate As Boolean)
        Public MustOverride Function GetMirrorArticle(ByVal articleID As Integer) As IDataReader
        Public MustOverride Function GetMirrorArticleList(ByVal linkedArticleID As Integer) As IDataReader

        Public MustOverride Function GetPageList(ByVal articleID As Integer) As IDataReader
        Public MustOverride Function GetPage(ByVal pageID As Integer) As IDataReader
        Public MustOverride Sub DeletePage(ByVal pageID As Integer)
        Public MustOverride Function AddPage(ByVal articleID As Integer, ByVal title As String, ByVal pageText As String, ByVal sortOrder As Integer) As Integer
        Public MustOverride Sub UpdatePage(ByVal pageID As Integer, ByVal articleID As Integer, ByVal title As String, ByVal pageText As String, ByVal sortOrder As Integer)

        Public MustOverride Function AddRating(ByVal articleID As Integer, ByVal userID As Integer, ByVal createdDate As DateTime, ByVal rating As Double) As Integer
        Public MustOverride Function GetRating(ByVal articleID As Integer, ByVal userID As Integer) As IDataReader
        Public MustOverride Function GetRatingByID(ByVal ratingID As Integer) As IDataReader
        Public MustOverride Sub DeleteRating(ByVal ratingID As Integer)

        Public MustOverride Function AddHandout(ByVal moduleID As Integer, ByVal userID As Integer, ByVal name As String, ByVal description As String) As Integer
        Public MustOverride Sub AddHandoutArticle(ByVal handoutID As Integer, ByVal articleID As Integer, ByVal sortOrder As Integer)
        Public MustOverride Sub DeleteHandout(ByVal handoutID As Integer)
        Public MustOverride Sub DeleteHandoutArticleList(ByVal handoutID As Integer)
        Public MustOverride Function GetHandout(ByVal handoutID As Integer) As IDataReader
        Public MustOverride Function GetHandoutList(ByVal userID As Integer) As IDataReader
        Public MustOverride Function GetHandoutArticleList(ByVal handoutID As Integer) As IDataReader
        Public MustOverride Sub UpdateHandout(ByVal handoutID As Integer, ByVal moduleID As Integer, ByVal userID As Integer, ByVal name As String, ByVal description As String)

        Public MustOverride Function GetTag(ByVal tagID As Integer) As IDataReader
        Public MustOverride Function GetTagByName(ByVal moduleID As Integer, ByVal nameLowered As String) As IDataReader
        Public MustOverride Function ListTag(ByVal moduleID As Integer, ByVal maxCount As Integer) As IDataReader
        Public MustOverride Function AddTag(ByVal moduleID As Integer, ByVal name As String, ByVal nameLowered As String) As Integer
        Public MustOverride Sub UpdateTag(ByVal tagID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal nameLowered As String, ByVal usages As Integer)
        Public MustOverride Sub DeleteTag(ByVal tagID As Integer)

        Public MustOverride Sub AddArticleTag(ByVal articleID As Integer, ByVal tagID As Integer)
        Public MustOverride Sub DeleteArticleTag(ByVal articleID As Integer)
        Public MustOverride Sub DeleteArticleTagByTag(ByVal tagID As Integer)

#Region " EmailTemplate Methods "

        Public MustOverride Function GetEmailTemplate(ByVal templateID As Integer) As IDataReader
        Public MustOverride Function GetEmailTemplateByName(ByVal moduleID As Integer, ByVal name As String) As IDataReader
        Public MustOverride Function ListEmailTemplate(ByVal moduleID As Integer) As IDataReader
        Public MustOverride Function AddEmailTemplate(ByVal moduleID As Integer, ByVal name As String, ByVal subject As String, ByVal template As String) As Integer
        Public MustOverride Sub UpdateEmailTemplate(ByVal templateID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal subject As String, ByVal template As String)
        Public MustOverride Sub DeleteEmailTemplate(ByVal templateID As Integer)

#End Region

#End Region

    End Class

End Namespace