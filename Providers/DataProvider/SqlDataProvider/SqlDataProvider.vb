'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2005
' by Scott McCulloch ( smcculloch@iinet.net.au ) ( http://www.smcculloch.net )
'

Imports System
Imports System.Configuration
Imports System.Data
Imports Microsoft.ApplicationBlocks.Data

Imports DotNetNuke
Imports DotNetNuke.Common.Utilities
Imports System.Web.UI.WebControls

Namespace Ventrian.NewsArticles

    Public Class SqlDataProvider

        Inherits DataProvider

#Region " Private Members "

        Private Const ProviderType As String = "data"

        Private _providerConfiguration As Framework.Providers.ProviderConfiguration = Framework.Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType)
        Private _connectionString As String
        Private _providerPath As String
        Private _objectQualifier As String
        Private _databaseOwner As String

#End Region

#Region " Constructors "

        Public Sub New()

            ' Read the configuration specific information for this provider
            Dim objProvider As Framework.Providers.Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Framework.Providers.Provider)

            _connectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString()

            _providerPath = objProvider.Attributes("providerPath")

            _objectQualifier = objProvider.Attributes("objectQualifier")
            If _objectQualifier <> "" And _objectQualifier.EndsWith("_") = False Then
                _objectQualifier += "_"
            End If

            _databaseOwner = objProvider.Attributes("databaseOwner")
            If _databaseOwner <> "" And _databaseOwner.EndsWith(".") = False Then
                _databaseOwner += "."
            End If

        End Sub

#End Region

#Region " Properties "

        Public ReadOnly Property ConnectionString() As String
            Get
                Return _connectionString
            End Get
        End Property

        Public ReadOnly Property ProviderPath() As String
            Get
                Return _providerPath
            End Get
        End Property

        Public ReadOnly Property ObjectQualifier() As String
            Get
                Return _objectQualifier
            End Get
        End Property

        Public ReadOnly Property DatabaseOwner() As String
            Get
                Return _databaseOwner
            End Get
        End Property

#End Region

#Region " Public Methods "

        Private Function GetNull(ByVal Field As Object) As Object
            Return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value)
        End Function

#Region " Article Methods "

        Public Overrides Function GetArticleListByApproved(ByVal moduleID As Integer, ByVal isApproved As Boolean) As IDataReader
            If (isApproved) Then
                Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetArticleListBySearchCriteria", GetNull(moduleID), DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, isApproved, False, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, "StartDate", "DESC", DBNull.Value, DBNull.Value), IDataReader)
            Else
                Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetArticleListBySearchCriteria", GetNull(moduleID), DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, isApproved, False, DBNull.Value, DBNull.Value, True, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, "StartDate", "DESC", DBNull.Value, DBNull.Value), IDataReader)
            End If

        End Function

        Public Overrides Function GetArticleListBySearchCriteria(ByVal moduleID As Integer, ByVal currentDate As DateTime, ByVal agedDate As DateTime, ByVal categoryID As Integer(), ByVal matchAll As Boolean, ByVal categoryIDExclude As Integer(), ByVal maxCount As Integer, ByVal pageNumber As Integer, ByVal pageSize As Integer, ByVal sortBy As String, ByVal sortDirection As String, ByVal isApproved As Boolean, ByVal isDraft As Boolean, ByVal keywords As String, ByVal authorID As Integer, ByVal showPending As Boolean, ByVal showExpired As Boolean, ByVal showFeaturedOnly As Boolean, ByVal showNotFeaturedOnly As Boolean, ByVal showSecuredOnly As Boolean, ByVal showNotSecuredOnly As Boolean, ByVal articleIDs As String, ByVal tagID As Integer(), ByVal matchAllTag As Boolean, ByVal rssGuid As String, ByVal customFieldID As Integer, ByVal customValue As String, ByVal linkFilter As String) As IDataReader
            Dim categories As String = DotNetNuke.Common.Utilities.Null.NullString
            Dim categoryIDCount As Integer = Null.NullInteger

            If Not (categoryID Is Nothing) Then
                For Each category As Integer In categoryID
                    If Not (categories = DotNetNuke.Common.Utilities.Null.NullString) Then
                        categories = categories & ","
                    End If
                    categories = categories & category.ToString()
                Next

                If (matchAll) Then
                    categoryIDCount = categoryID.Length - 1
                End If
            End If

            Dim tags As String = DotNetNuke.Common.Utilities.Null.NullString
            Dim tagIDCount As Integer = Null.NullInteger

            If Not (tagID Is Nothing) Then
                For Each tag As Integer In tagID
                    If Not (tags = DotNetNuke.Common.Utilities.Null.NullString) Then
                        tags = tags & ","
                    End If
                    tags = tags & tag.ToString()
                Next

                If (matchAllTag) Then
                    tagIDCount = tagID.Length - 1
                End If
            End If

            Dim categoriesExclude As String = DotNetNuke.Common.Utilities.Null.NullString
            If Not (categoryIDExclude Is Nothing) Then
                For Each category As Integer In categoryIDExclude
                    If Not (categoriesExclude = DotNetNuke.Common.Utilities.Null.NullString) Then
                        categoriesExclude = categoriesExclude & ","
                    End If
                    categoriesExclude = categoriesExclude & category.ToString()
                Next
            End If

            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetArticleListBySearchCriteria", moduleID, GetNull(currentDate), GetNull(agedDate), GetNull(categories), GetNull(categoryIDCount), GetNull(categoriesExclude), GetNull(maxCount), GetNull(pageNumber), GetNull(pageSize), sortBy, sortDirection, isApproved, isDraft, GetNull(keywords), GetNull(authorID), GetNull(showPending), GetNull(showExpired), showFeaturedOnly, showNotFeaturedOnly, showSecuredOnly, showNotSecuredOnly, GetNull(articleIDs), GetNull(tags), GetNull(tagIDCount), GetNull(rssGuid), GetNull(customFieldID), GetNull(customValue), GetNull(linkFilter)), IDataReader)
        End Function

        Public Overrides Function GetArticle(ByVal articleID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetArticle", articleID), IDataReader)
        End Function

        Public Overrides Function GetArticleCategories(ByVal articleID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetArticleCategories", articleID), IDataReader)
        End Function

        Public Overrides Function GetNewsArchive(ByVal moduleID As Integer, ByVal categoryID As Integer(), ByVal categoryIDExclude As Integer(), ByVal authorID As Integer, ByVal groupBy As String, ByVal showPending As Boolean) As IDataReader
            Dim categories As String = DotNetNuke.Common.Utilities.Null.NullString

            If Not (categoryID Is Nothing) Then
                For Each category As Integer In categoryID
                    If Not (categories = DotNetNuke.Common.Utilities.Null.NullString) Then
                        categories = categories & ","
                    End If
                    categories = categories & category.ToString()
                Next
            End If

            Dim categoriesExclude As String = DotNetNuke.Common.Utilities.Null.NullString

            If Not (categoryIDExclude Is Nothing) Then
                For Each category As Integer In categoryIDExclude
                    If Not (categoriesExclude = DotNetNuke.Common.Utilities.Null.NullString) Then
                        categoriesExclude = categoriesExclude & ","
                    End If
                    categoriesExclude = categoriesExclude & category.ToString()
                Next
            End If

            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetNewsArchive", moduleID, GetNull(categories), GetNull(categoriesExclude), GetNull(authorID), groupBy, GetNull(showPending)), IDataReader)
        End Function

        Public Overrides Sub DeleteArticle(ByVal articleID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_DeleteArticle", articleID)
        End Sub

        Public Overrides Sub DeleteArticleCategories(ByVal articleID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_DeleteArticleCategories", articleID)
        End Sub

        Public Overrides Function AddArticle(ByVal authorID As Integer, ByVal approverID As Integer, ByVal createdDate As DateTime, ByVal lastUpdate As DateTime, ByVal title As String, ByVal summary As String, ByVal isApproved As Boolean, ByVal numberOfViews As Integer, ByVal isDraft As Boolean, ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal moduleID As Integer, ByVal imageUrl As String, ByVal isFeatured As Boolean, ByVal lastUpdateID As Integer, ByVal url As String, ByVal isSecure As Boolean, ByVal isNewWindow As Boolean, ByVal metaTitle As String, ByVal metaDescription As String, ByVal metaKeywords As String, ByVal pageHeadText As String, ByVal shortUrl As String, ByVal rssGuid As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_AddArticle", authorID, approverID, createdDate, lastUpdate, title, GetNull(summary), isApproved, numberOfViews, isDraft, GetNull(startDate), GetNull(endDate), moduleID, GetNull(imageUrl), isFeatured, GetNull(lastUpdateID), GetNull(url), isSecure, isNewWindow, GetNull(metaTitle), GetNull(metaDescription), GetNull(metaKeywords), GetNull(pageHeadText), GetNull(shortUrl), GetNull(rssGuid)), Integer)
        End Function

        Public Overrides Sub AddArticleCategory(ByVal articleID As Integer, ByVal categoryID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_AddArticleCategory", articleID, categoryID)
        End Sub

        Public Overrides Sub UpdateArticle(ByVal articleID As Integer, ByVal authorID As Integer, ByVal approverID As Integer, ByVal createdDate As DateTime, ByVal lastUpdate As DateTime, ByVal title As String, ByVal summary As String, ByVal isApproved As Boolean, ByVal numberOfViews As Integer, ByVal isDraft As Boolean, ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal moduleID As Integer, ByVal imageUrl As String, ByVal isFeatured As Boolean, ByVal lastUpdateID As Integer, ByVal url As String, ByVal isSecure As Boolean, ByVal isNewWindow As Boolean, ByVal metaTitle As String, ByVal metaDescription As String, ByVal metaKeywords As String, ByVal pageHeadText As String, ByVal shortUrl As String, ByVal rssGuid As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_UpdateArticle", articleID, authorID, approverID, createdDate, lastUpdate, title, GetNull(summary), isApproved, numberOfViews, isDraft, GetNull(startDate), GetNull(endDate), moduleID, GetNull(imageUrl), isFeatured, GetNull(lastUpdateID), GetNull(url), isSecure, isNewWindow, GetNull(metaTitle), GetNull(metaDescription), GetNull(metaKeywords), GetNull(pageHeadText), GetNull(shortUrl), GetNull(rssGuid))
        End Sub

        Public Overrides Sub UpdateArticleCount(ByVal articleID As Integer, ByVal numberOfViews As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_UpdateArticleCount", articleID, numberOfViews)
        End Sub

        Public Overrides Function SecureCheck(ByVal portalID As Integer, ByVal articleID As Integer, ByVal userID As Integer) As Boolean
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_SecureCheck", portalID, articleID, userID), Boolean)
        End Function
#End Region

#Region " Author Methods "

        Public Overrides Function GetAuthorList(ByVal moduleID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetAuthorList", moduleID), IDataReader)
        End Function

        Public Overrides Function GetAuthorStatistics(ByVal moduleID As Integer, ByVal categoryID As Integer(), ByVal categoryIDExclude As Integer(), ByVal authorID As Integer, ByVal sortBy As String, ByVal showPending As Boolean) As IDataReader
            Dim categories As String = DotNetNuke.Common.Utilities.Null.NullString

            If Not (categoryID Is Nothing) Then
                For Each category As Integer In categoryID
                    If Not (categories = DotNetNuke.Common.Utilities.Null.NullString) Then
                        categories = categories & ","
                    End If
                    categories = categories & category.ToString()
                Next
            End If

            Dim categoriesExclude As String = DotNetNuke.Common.Utilities.Null.NullString

            If Not (categoryIDExclude Is Nothing) Then
                For Each category As Integer In categoryIDExclude
                    If Not (categoriesExclude = DotNetNuke.Common.Utilities.Null.NullString) Then
                        categoriesExclude = categoriesExclude & ","
                    End If
                    categoriesExclude = categoriesExclude & category.ToString()
                Next
            End If
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetAuthorStatistics", moduleID, GetNull(categories), GetNull(categoriesExclude), GetNull(authorID), sortBy, GetNull(showPending)), IDataReader)
        End Function

#End Region

#Region " Category Methods "

        Public Overrides Function GetCategoryList(ByVal moduleID As Integer, ByVal parentID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetCategoryList", moduleID, parentID), IDataReader)
        End Function

        Public Overrides Function GetCategoryListAll(ByVal moduleID As Integer, ByVal authorID As Integer, ByVal showPending As Boolean, ByVal sortType As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetCategoryListAll", moduleID, GetNull(authorID), GetNull(showPending), sortType), IDataReader)
        End Function

        Public Overrides Function GetCategory(ByVal categoryID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetCategory", categoryID), IDataReader)
        End Function

        Public Overrides Sub DeleteCategory(ByVal categoryID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_DeleteCategory", categoryID)
        End Sub

        Public Overrides Function AddCategory(ByVal moduleID As Integer, ByVal parentID As Integer, ByVal name As String, ByVal image As String, ByVal description As String, ByVal sortOrder As Integer, ByVal inheritSecurity As Boolean, ByVal categorySecurityType As Integer, ByVal metaTitle As String, ByVal metaDescription As String, ByVal metaKeywords As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_AddCategory", moduleID, parentID, name, image, GetNull(description), sortOrder, inheritSecurity, categorySecurityType, GetNull(metaTitle), GetNull(metaDescription), GetNull(metaKeywords)), Integer)
        End Function

        Public Overrides Sub UpdateCategory(ByVal categoryID As Integer, ByVal moduleID As Integer, ByVal parentID As Integer, ByVal name As String, ByVal image As String, ByVal description As String, ByVal sortOrder As Integer, ByVal inheritSecurity As Boolean, ByVal categorySecurityType As Integer, ByVal metaTitle As String, ByVal metaDescription As String, ByVal metaKeywords As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_UpdateCategory", categoryID, moduleID, parentID, name, image, GetNull(description), sortOrder, inheritSecurity, categorySecurityType, GetNull(metaTitle), GetNull(metaDescription), GetNull(metaKeywords))
        End Sub
#End Region

#Region " Comment Methods "
        Public Overrides Function GetCommentList(ByVal moduleID As Integer, ByVal articleID As Integer, ByVal isApproved As Boolean, ByVal direction As SortDirection, ByVal maxCount As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetCommentList", moduleID, GetNull(articleID), isApproved, direction, GetNull(maxCount)), IDataReader)
        End Function

        Public Overrides Function GetComment(ByVal commentID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetComment", commentID), IDataReader)
        End Function

        Public Overrides Sub DeleteComment(ByVal commentID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_DeleteComment", commentID)
        End Sub

        Public Overrides Function AddComment(ByVal articleID As Integer, ByVal createdDate As DateTime, ByVal userID As Integer, ByVal comment As String, ByVal remoteAddress As String, ByVal type As Integer, ByVal trackbackUrl As String, ByVal trackbackTitle As String, ByVal trackbackBlogName As String, ByVal trackbackExcerpt As String, ByVal anonymousName As String, ByVal anonymousEmail As String, ByVal anonymousURL As String, ByVal notifyMe As Boolean, ByVal isApproved As Boolean, ByVal approvedBy As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_AddComment", articleID, createdDate, userID, comment, GetNull(remoteAddress), type, GetNull(trackbackUrl), GetNull(trackbackTitle), GetNull(trackbackBlogName), GetNull(trackbackExcerpt), GetNull(anonymousName), GetNull(anonymousEmail), GetNull(anonymousURL), notifyMe, isApproved, GetNull(approvedBy)), Integer)
        End Function

        Public Overrides Sub UpdateComment(ByVal commentID As Integer, ByVal articleID As Integer, ByVal userID As Integer, ByVal comment As String, ByVal remoteAddress As String, ByVal type As Integer, ByVal trackbackUrl As String, ByVal trackbackTitle As String, ByVal trackbackBlogName As String, ByVal trackbackExcerpt As String, ByVal anonymousName As String, ByVal anonymousEmail As String, ByVal anonymousURL As String, ByVal notifyMe As Boolean, ByVal isApproved As Boolean, ByVal approvedBy As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_UpdateComment", commentID, articleID, userID, comment, GetNull(remoteAddress), type, GetNull(trackbackUrl), GetNull(trackbackTitle), GetNull(trackbackBlogName), GetNull(trackbackExcerpt), GetNull(anonymousName), GetNull(anonymousEmail), GetNull(anonymousURL), notifyMe, isApproved, GetNull(approvedBy))
        End Sub
#End Region

#Region " Custom Field Methods "
        Public Overrides Function GetCustomField(ByVal customFieldID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetCustomField", customFieldID), IDataReader)
        End Function

        Public Overrides Function GetCustomFieldList(ByVal moduleID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetCustomFieldList", moduleID), IDataReader)
        End Function

        Public Overrides Sub DeleteCustomField(ByVal customFieldID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_DeleteCustomField", customFieldID)
        End Sub

        Public Overrides Function AddCustomField(ByVal moduleID As Integer, ByVal name As String, ByVal fieldType As Integer, ByVal fieldElements As String, ByVal defaultValue As String, ByVal caption As String, ByVal captionHelp As String, ByVal isRequired As Boolean, ByVal isVisible As Boolean, ByVal sortOrder As Integer, ByVal validationType As Integer, ByVal length As Integer, ByVal regularExpression As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_AddCustomField", moduleID, name, fieldType, GetNull(fieldElements), GetNull(defaultValue), GetNull(caption), GetNull(captionHelp), isRequired, isVisible, sortOrder, validationType, length, GetNull(regularExpression)), Integer)
        End Function

        Public Overrides Sub UpdateCustomField(ByVal customFieldID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal fieldType As Integer, ByVal fieldElements As String, ByVal defaultValue As String, ByVal caption As String, ByVal captionHelp As String, ByVal isRequired As Boolean, ByVal isVisible As Boolean, ByVal sortOrder As Integer, ByVal validationType As Integer, ByVal length As Integer, ByVal regularExpression As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_UpdateCustomField", customFieldID, moduleID, name, fieldType, GetNull(fieldElements), GetNull(defaultValue), GetNull(caption), GetNull(captionHelp), isRequired, isVisible, sortOrder, validationType, length, GetNull(regularExpression))
        End Sub
#End Region

#Region " Custom Value Methods "

        Public Overrides Function GetCustomValueList(ByVal articleID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetCustomValueList", articleID), IDataReader)
        End Function

        Public Overrides Sub DeleteCustomValue(ByVal articleID As Integer, ByVal customFieldID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_DeleteCustomValue", articleID, customFieldID)
        End Sub

        Public Overrides Function AddCustomValue(ByVal articleID As Integer, ByVal customFieldID As Integer, ByVal customValue As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_AddCustomValue", articleID, customFieldID, customValue), Integer)
        End Function

        Public Overrides Sub UpdateCustomValue(ByVal customValueID As Integer, ByVal articleID As Integer, ByVal customFieldID As Integer, ByVal customValue As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_UpdateCustomValue", customValueID, articleID, customFieldID, customValue)
        End Sub

#End Region

#Region " Feed Methods "

        Public Overrides Function AddFeed(ByVal moduleID As Integer, ByVal title As String, ByVal url As String, ByVal userID As Integer, ByVal autoFeature As Boolean, ByVal isActive As Boolean, ByVal dateMode As Integer, ByVal autoExpire As Integer, ByVal autoExpireUnit As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_FeedAdd", moduleID, title, url, userID, autoFeature, isActive, dateMode, GetNull(autoExpire), GetNull(autoExpireUnit)), Integer)
        End Function

        Public Overrides Sub UpdateFeed(ByVal feedID As Integer, ByVal moduleID As Integer, ByVal title As String, ByVal url As String, ByVal userID As Integer, ByVal autoFeature As Boolean, ByVal isActive As Boolean, ByVal dateMode As Integer, ByVal autoExpire As Integer, ByVal autoExpireUnit As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_FeedUpdate", feedID, moduleID, title, url, userID, autoFeature, isActive, dateMode, GetNull(autoExpire), GetNull(autoExpireUnit))
        End Sub

        Public Overrides Sub DeleteFeed(ByVal feedID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_FeedDelete", feedID)
        End Sub

        Public Overrides Function GetFeed(ByVal feedID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_FeedGet", feedID), IDataReader)
        End Function

        Public Overrides Function GetFeedList(ByVal moduleID As Integer, ByVal showActiveOnly As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_FeedList", moduleID, showActiveOnly), IDataReader)
        End Function

        Public Overrides Sub AddFeedCategory(ByVal feedID As Integer, ByVal categoryID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_FeedCategoryAdd", feedID, categoryID)
        End Sub

        Public Overrides Function GetFeedCategoryList(ByVal feedID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_FeedCategoryList", feedID), IDataReader)
        End Function

        Public Overrides Sub DeleteFeedCategory(ByVal feedID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_FeedCategoryDelete", feedID)
        End Sub

#End Region

#Region " File Methods "

        Public Overrides Function AddFile(ByVal articleID As Integer, ByVal title As String, ByVal fileName As String, ByVal extension As String, ByVal size As Integer, ByVal contentType As String, ByVal folder As String, ByVal sortOrder As Integer, ByVal fileGuid As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_FileAdd", GetNull(articleID), GetNull(title), GetNull(fileName), GetNull(extension), GetNull(size), GetNull(contentType), GetNull(folder), sortOrder, GetNull(fileGuid)), Integer)
        End Function

        Public Overrides Sub UpdateFile(ByVal fileID As Integer, ByVal articleID As Integer, ByVal title As String, ByVal fileName As String, ByVal extension As String, ByVal size As Integer, ByVal contentType As String, ByVal folder As String, ByVal sortOrder As Integer, ByVal fileGuid As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_FileUpdate", fileID, GetNull(articleID), GetNull(title), GetNull(fileName), GetNull(extension), GetNull(size), GetNull(contentType), GetNull(folder), sortOrder, GetNull(fileGuid))
        End Sub

        Public Overrides Sub DeleteFile(ByVal fileID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_FileDelete", fileID)
        End Sub

        Public Overrides Function GetFile(ByVal fileID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_FileGet", fileID), IDataReader)
        End Function

        Public Overrides Function GetFileList(ByVal articleID As Integer, ByVal fileGuid As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_FileList", GetNull(articleID), GetNull(fileGuid)), IDataReader)
        End Function

#End Region

#Region " Image Methods "

        Public Overrides Function AddImage(ByVal articleID As Integer, ByVal title As String, ByVal fileName As String, ByVal extension As String, ByVal size As Integer, ByVal width As Integer, ByVal height As Integer, ByVal contentType As String, ByVal folder As String, ByVal sortOrder As Integer, ByVal imageGuid As String, ByVal description As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_ImageAdd", GetNull(articleID), GetNull(title), GetNull(fileName), GetNull(extension), GetNull(size), GetNull(width), GetNull(height), GetNull(contentType), GetNull(folder), sortOrder, GetNull(imageGuid), GetNull(description)), Integer)
        End Function

        Public Overrides Sub UpdateImage(ByVal imageID As Integer, ByVal articleID As Integer, ByVal title As String, ByVal fileName As String, ByVal extension As String, ByVal size As Integer, ByVal width As Integer, ByVal height As Integer, ByVal contentType As String, ByVal folder As String, ByVal sortOrder As Integer, ByVal imageGuid As String, ByVal description As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_ImageUpdate", imageID, GetNull(articleID), GetNull(title), GetNull(fileName), GetNull(extension), GetNull(size), GetNull(width), GetNull(height), GetNull(contentType), GetNull(folder), sortOrder, GetNull(imageGuid), GetNull(description))
        End Sub

        Public Overrides Sub DeleteImage(ByVal imageID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_ImageDelete", imageID)
        End Sub

        Public Overrides Function GetImage(ByVal imageID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_ImageGet", imageID), IDataReader)
        End Function

        Public Overrides Function GetImageList(ByVal articleID As Integer, ByVal imageGuid As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_ImageList", GetNull(articleID), GetNull(imageGuid)), IDataReader)
        End Function

#End Region

#Region " Mirror Article Methods "

        Public Overrides Sub AddMirrorArticle(ByVal articleID As Integer, ByVal linkedArticleID As Integer, ByVal linkedPortalID As Integer, ByVal autoUpdate As Boolean)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_AddMirrorArticle", articleID, linkedArticleID, linkedPortalID, autoUpdate)
        End Sub

        Public Overrides Function GetMirrorArticle(ByVal articleID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetMirrorArticle", articleID), IDataReader)
        End Function

        Public Overrides Function GetMirrorArticleList(ByVal linkedArticleID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetMirrorArticleList", linkedArticleID), IDataReader)
        End Function

#End Region

#Region " Page Methods "
        Public Overrides Function GetPageList(ByVal articleID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetPageList", articleID), IDataReader)
        End Function

        Public Overrides Function GetPage(ByVal pageID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetPage", pageID), IDataReader)
        End Function

        Public Overrides Sub DeletePage(ByVal pageID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_DeletePage", pageID)
        End Sub

        Public Overrides Function AddPage(ByVal articleID As Integer, ByVal title As String, ByVal pageText As String, ByVal sortOrder As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_AddPage", articleID, title, pageText, sortOrder), Integer)
        End Function

        Public Overrides Sub UpdatePage(ByVal pageID As Integer, ByVal articleID As Integer, ByVal title As String, ByVal pageText As String, ByVal sortOrder As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_UpdatePage", pageID, articleID, title, pageText, sortOrder)
        End Sub
#End Region

#Region " EmailTemplate Methods "
        Public Overrides Function GetEmailTemplate(ByVal templateID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_EmailTemplateGet", templateID), IDataReader)
        End Function

        Public Overrides Function GetEmailTemplateByName(ByVal moduleID As Integer, ByVal name As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_EmailTemplateGetByName", moduleID, name), IDataReader)
        End Function

        Public Overrides Function ListEmailTemplate(ByVal moduleID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_EmailTemplateList", moduleID), IDataReader)
        End Function

        Public Overrides Function AddEmailTemplate(ByVal moduleID As Integer, ByVal name As String, ByVal subject As String, ByVal template As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_EmailTemplateAdd", moduleID, name, subject, template), Integer)
        End Function

        Public Overrides Sub UpdateEmailTemplate(ByVal templateID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal subject As String, ByVal template As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_EmailTemplateUpdate", templateID, moduleID, name, subject, template)
        End Sub

        Public Overrides Sub DeleteEmailTemplate(ByVal templateID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_EmailTemplateDelete", templateID)
        End Sub
#End Region

#Region " Rating Methods "

        Public Overrides Function AddRating(ByVal articleID As Integer, ByVal userID As Integer, ByVal createdDate As DateTime, ByVal rating As Double) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_RatingAdd", articleID, userID, createdDate, GetNull(rating)), Integer)
        End Function

        Public Overrides Function GetRating(ByVal articleID As Integer, ByVal userID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_RatingGet", articleID, userID), IDataReader)
        End Function

        Public Overrides Function GetRatingByID(ByVal ratingID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_RatingGetByID", ratingID), IDataReader)
        End Function

        Public Overrides Sub DeleteRating(ByVal ratingID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_RatingDelete", ratingID)
        End Sub

#End Region

#Region " Rating Methods "

        Public Overrides Function AddHandout(ByVal moduleID As Integer, ByVal userID As Integer, ByVal name As String, ByVal description As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_AddHandout", moduleID, userID, name, GetNull(description)), Integer)
        End Function

        Public Overrides Sub AddHandoutArticle(ByVal handoutID As Integer, ByVal articleID As Integer, ByVal sortOrder As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_AddHandoutArticle", handoutID, articleID, sortOrder)
        End Sub

        Public Overrides Sub DeleteHandout(ByVal handoutID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_DeleteHandout", handoutID)
        End Sub

        Public Overrides Sub DeleteHandoutArticleList(ByVal ratingID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_DeleteHandoutArticleList", ratingID)
        End Sub

        Public Overrides Function GetHandout(ByVal handoutID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetHandout", handoutID), IDataReader)
        End Function

        Public Overrides Function GetHandoutList(ByVal userID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetHandoutList", userID), IDataReader)
        End Function

        Public Overrides Function GetHandoutArticleList(ByVal handoutID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_GetHandoutArticleList", handoutID), IDataReader)
        End Function

        Public Overrides Sub UpdateHandout(ByVal handoutID As Integer, ByVal moduleID As Integer, ByVal userID As Integer, ByVal name As String, ByVal description As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_UpdateHandout", handoutID, moduleID, userID, name, GetNull(description))
        End Sub

#End Region

#Region " Tag Methods "

        Public Overrides Function GetTag(ByVal tagID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_TagGet", tagID), IDataReader)
        End Function

        Public Overrides Function GetTagByName(ByVal moduleID As Integer, ByVal name As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_TagGetByName", moduleID, name), IDataReader)
        End Function

        Public Overrides Function ListTag(ByVal moduleID As Integer, ByVal maxCount As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_TagList", moduleID, GetNull(maxCount)), IDataReader)
        End Function

        Public Overrides Function AddTag(ByVal moduleID As Integer, ByVal name As String, ByVal nameLowered As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_TagAdd", moduleID, name, nameLowered), Integer)
        End Function

        Public Overrides Sub UpdateTag(ByVal tagID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal nameLowered As String, ByVal usages As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_TagUpdate", tagID, moduleID, name, nameLowered, usages)
        End Sub

        Public Overrides Sub DeleteTag(ByVal tagID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_TagDelete", tagID)
        End Sub

        Public Overrides Sub AddArticleTag(ByVal articleID As Integer, ByVal tagID As Integer, Optional ByVal displayOrder As Integer = 0)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_ArticleTagAdd", articleID, tagID, displayOrder)
        End Sub

        Public Overrides Sub DeleteArticleTag(ByVal articleID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_ArticleTagDelete", articleID)
        End Sub

        Public Overrides Sub DeleteArticleTagByTag(ByVal tagID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_NewsArticles_ArticleTagDeleteByTag", tagID)
        End Sub

#End Region

#End Region

    End Class

End Namespace