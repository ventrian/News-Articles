'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security
Imports DotNetNuke.Security.Permissions
Imports Ventrian.NewsArticles.Components.Types

Namespace Ventrian.NewsArticles

    Public Class ArticleSettings

#Region " Private Members "

        Private ReadOnly _settings As Hashtable
        Private ReadOnly _portalSettings As PortalSettings
        Private ReadOnly _moduleConfiguration As ModuleInfo

#End Region

#Region " Constructors "

        Public Sub New(ByVal settings As Hashtable, ByVal portalSettings As PortalSettings, ByVal moduleConfiguration As ModuleInfo)
            _settings = settings
            _portalSettings = portalSettings
            _moduleConfiguration = moduleConfiguration
        End Sub

#End Region

#Region " Private Properties "

        Public ReadOnly Property Settings() As Hashtable
            Get
                Return _settings
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Function IsInRoles(ByVal roles As String) As Boolean

            ' Replacement for core IsInRoles check because it doesn't auto-pass super users.

            If Not roles Is Nothing Then
                Dim context As HttpContext = HttpContext.Current
                Dim objUserInfo As UserInfo = UserController.Instance.GetCurrentUserInfo
                Dim role As String

                For Each role In roles.Split(New Char() {";"c})
                    If (role <> "" AndAlso Not role Is Nothing AndAlso
                     ((context.Request.IsAuthenticated = False And role = glbRoleUnauthUserName) Or
                     role = glbRoleAllUsersName Or
                     objUserInfo.IsInRole(role) = True
                     )) Then
                        Return True
                    End If
                Next role

            End If

            Return False

        End Function

#End Region

#Region " Public Properties "

        Public ReadOnly Property AlwaysShowPageID() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.SEO_ALWAYS_SHOW_PAGEID_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.SEO_ALWAYS_SHOW_PAGEID_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property Author() As Integer
            Get
                If (Settings.Contains(ArticleConstants.AUTHOR_SETTING)) Then
                    If (IsNumeric(Settings(ArticleConstants.AUTHOR_SETTING).ToString())) Then
                        Return Convert.ToInt32(Settings(ArticleConstants.AUTHOR_SETTING).ToString())
                    Else
                        Return Null.NullInteger
                    End If
                Else
                    Return Null.NullInteger
                End If
            End Get
        End Property

        Public ReadOnly Property AuthorDefault() As Integer
            Get
                If (Settings.Contains(ArticleConstants.AUTHOR_DEFAULT_SETTING)) Then
                    If (IsNumeric(Settings(ArticleConstants.AUTHOR_DEFAULT_SETTING).ToString())) Then
                        Return Convert.ToInt32(Settings(ArticleConstants.AUTHOR_DEFAULT_SETTING).ToString())
                    Else
                        Return Null.NullInteger
                    End If
                Else
                    Return Null.NullInteger
                End If
            End Get
        End Property

        Public ReadOnly Property AuthorSelect() As AuthorSelectType
            Get
                If (Settings.Contains(ArticleConstants.AUTHOR_SELECT_TYPE)) Then
                    Return CType([Enum].Parse(GetType(AuthorSelectType), Settings(ArticleConstants.AUTHOR_SELECT_TYPE).ToString()), AuthorSelectType)
                Else
                    Return AuthorSelectType.ByDropdown
                End If
            End Get
        End Property

        Public ReadOnly Property AuthorUserIDFilter() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.AUTHOR_USERID_FILTER_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.AUTHOR_USERID_FILTER_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property AuthorUserIDParam() As String
            Get
                If (Settings.Contains(ArticleConstants.AUTHOR_USERID_PARAM_SETTING)) Then
                    Return Settings(ArticleConstants.AUTHOR_USERID_PARAM_SETTING).ToString()
                Else
                    Return "ID"
                End If
            End Get
        End Property

        Public ReadOnly Property AuthorUsernameFilter() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.AUTHOR_USERNAME_FILTER_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.AUTHOR_USERNAME_FILTER_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property AuthorUsernameParam() As String
            Get
                If (Settings.Contains(ArticleConstants.AUTHOR_USERNAME_PARAM_SETTING)) Then
                    Return Settings(ArticleConstants.AUTHOR_USERNAME_PARAM_SETTING).ToString()
                Else
                    Return "Username"
                End If
            End Get
        End Property

        Public ReadOnly Property AuthorLoggedInUserFilter() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.AUTHOR_LOGGED_IN_USER_FILTER_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.AUTHOR_LOGGED_IN_USER_FILTER_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property ArchiveAuthor() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ARCHIVE_AUTHOR_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ARCHIVE_AUTHOR_SETTING).ToString())
                Else
                    Return ArticleConstants.ARCHIVE_AUTHOR_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ArchiveCategories() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ARCHIVE_CATEGORIES_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ARCHIVE_CATEGORIES_SETTING).ToString())
                Else
                    Return ArticleConstants.ARCHIVE_CATEGORIES_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ArchiveCurrentArticles() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ARCHIVE_CURRENT_ARTICLES_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ARCHIVE_CURRENT_ARTICLES_SETTING).ToString())
                Else
                    Return ArticleConstants.ARCHIVE_CURRENT_ARTICLES_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ArchiveMonth() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ARCHIVE_MONTH_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ARCHIVE_MONTH_SETTING).ToString())
                Else
                    Return ArticleConstants.ARCHIVE_MONTH_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property BubbleFeatured() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.BUBBLE_FEATURED_ARTICLES)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.BUBBLE_FEATURED_ARTICLES).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property CategoryBreadcrumb() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.CATEGORY_BREADCRUMB_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.CATEGORY_BREADCRUMB_SETTING).ToString())
                Else
                    Return ArticleConstants.CATEGORY_BREADCRUMB_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CategoryFilterSubmit() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.CATEGORY_FILTER_SUBMIT_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.CATEGORY_FILTER_SUBMIT_SETTING).ToString())
                Else
                    Return ArticleConstants.CATEGORY_FILTER_SUBMIT_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property UseStaticTagsList() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.USE_STATIC_TAGS_LIST_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.USE_STATIC_TAGS_LIST_SETTING).ToString())
                Else
                    Return ArticleConstants.USE_STATIC_TAGS_LIST_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property IncludeInPageName() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.CATEGORY_NAME_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.CATEGORY_NAME_SETTING).ToString())
                Else
                    Return ArticleConstants.CATEGORY_NAME_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CategorySelectionHeight() As Integer
            Get
                If (Settings.Contains(ArticleConstants.CATEGORY_SELECTION_HEIGHT_SETTING)) Then
                    Return Convert.ToInt32(Settings(ArticleConstants.CATEGORY_SELECTION_HEIGHT_SETTING).ToString())
                Else
                    Return ArticleConstants.CATEGORY_SELECTION_HEIGHT_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CategorySortType() As CategorySortType
            Get
                If (Settings.Contains(ArticleConstants.CATEGORY_SORT_SETTING)) Then
                    Try
                        Return CType([Enum].Parse(GetType(CategorySortType), Settings(ArticleConstants.CATEGORY_SORT_SETTING).ToString()), CategorySortType)
                    Catch
                        Return ArticleConstants.CATEGORY_SORT_SETTING_DEFAULT
                    End Try
                Else
                    Return ArticleConstants.CATEGORY_SORT_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CommentAkismetKey() As String
            Get
                If (Settings.Contains(ArticleConstants.COMMENT_AKISMET_SETTING)) Then
                    Return Settings(ArticleConstants.COMMENT_AKISMET_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property CommentModeration() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_COMMENT_MODERATION_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_COMMENT_MODERATION_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property CommentRequireName() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.COMMENT_REQUIRE_NAME_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.COMMENT_REQUIRE_NAME_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property CommentRequireEmail() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.COMMENT_REQUIRE_EMAIL_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.COMMENT_REQUIRE_EMAIL_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property ContentSharingPortals() As String
            Get
                If (Settings.Contains(ArticleConstants.CONTENT_SHARING_SETTING)) Then
                    Return Settings(ArticleConstants.CONTENT_SHARING_SETTING).ToString()
                Else
                    Return Null.NullString()
                End If
            End Get
        End Property

        Public ReadOnly Property DefaultImagesFolder() As Integer
            Get
                If (Settings.Contains(ArticleConstants.DEFAULT_IMAGES_FOLDER_SETTING)) Then
                    If (IsNumeric(Settings(ArticleConstants.DEFAULT_IMAGES_FOLDER_SETTING).ToString())) Then
                        Return Convert.ToInt32(Settings(ArticleConstants.DEFAULT_IMAGES_FOLDER_SETTING).ToString())
                    Else
                        Return Null.NullInteger
                    End If
                Else
                    Return Null.NullInteger
                End If
            End Get
        End Property

        Public ReadOnly Property DefaultFilesFolder() As Integer
            Get
                If (Settings.Contains(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING)) Then
                    If (IsNumeric(Settings(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING).ToString())) Then
                        Return Convert.ToInt32(Settings(ArticleConstants.DEFAULT_FILES_FOLDER_SETTING).ToString())
                    Else
                        Return Null.NullInteger
                    End If
                Else
                    Return Null.NullInteger
                End If
            End Get
        End Property

        Public ReadOnly Property EnablePortalFiles() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_PORTAL_FILES_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_PORTAL_FILES_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property EnableActiveSocialFeed() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ACTIVE_SOCIAL_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ACTIVE_SOCIAL_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property JournalIntegration() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.JOURNAL_INTEGRATION_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.JOURNAL_INTEGRATION_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property JournalIntegrationGroups() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.JOURNAL_INTEGRATION_GROUPS_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.JOURNAL_INTEGRATION_GROUPS_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property ActiveSocialSubmitKey() As String
            Get
                If (Settings.Contains(ArticleConstants.ACTIVE_SOCIAL_SUBMIT_SETTING)) Then
                    Return Settings(ArticleConstants.ACTIVE_SOCIAL_SUBMIT_SETTING).ToString()
                Else
                    Return "9F02B914-F565-4E5A-9194-8423431056CD"
                End If
            End Get
        End Property

        Public ReadOnly Property ActiveSocialCommentKey() As String
            Get
                If (Settings.Contains(ArticleConstants.ACTIVE_SOCIAL_COMMENT_SETTING)) Then
                    Return Settings(ArticleConstants.ACTIVE_SOCIAL_COMMENT_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property ActiveSocialRateKey() As String
            Get
                If (Settings.Contains(ArticleConstants.ACTIVE_SOCIAL_RATE_SETTING)) Then
                    Return Settings(ArticleConstants.ACTIVE_SOCIAL_RATE_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property EnableAutoTrackback() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_AUTO_TRACKBACK_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_AUTO_TRACKBACK_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property EnableCoreSearch() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_CORE_SEARCH_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_CORE_SEARCH_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property EnableExternalImages() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_EXTERNAL_IMAGES_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_EXTERNAL_IMAGES_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property EnableNotificationPing() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_NOTIFICATION_PING_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_NOTIFICATION_PING_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property EnableImagesUpload() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_UPLOAD_IMAGES_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_UPLOAD_IMAGES_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property EnablePortalImages() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_PORTAL_IMAGES_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_PORTAL_IMAGES_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property EnableRatings() As Boolean
            Get
                If (EnableRatingsAnonymous Or EnableRatingsAuthenticated) Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property EnableRatingsAnonymous() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_RATINGS_ANONYMOUS_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_RATINGS_ANONYMOUS_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property EnableRatingsAuthenticated() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_RATINGS_AUTHENTICATED_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_RATINGS_AUTHENTICATED_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property EnableSmartThinkerStoryFeed() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.SMART_THINKER_STORY_FEED_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.SMART_THINKER_STORY_FEED_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property EnableSyndication() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_SYNDICATION_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_SYNDICATION_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property EnableSyndicationEnclosures() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_SYNDICATION_ENCLOSURES_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_SYNDICATION_ENCLOSURES_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property EnableSyndicationHtml() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_SYNDICATION_HTML_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_SYNDICATION_HTML_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property ExpandSummary() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.EXPAND_SUMMARY_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.EXPAND_SUMMARY_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property FeaturedOnly() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.SHOW_FEATURED_ONLY_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.SHOW_FEATURED_ONLY_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property FilterCategories() As Integer()
            Get
                If (Settings.Contains(ArticleConstants.CATEGORIES_SETTING & _moduleConfiguration.TabID.ToString())) Then
                    If Not (Settings(ArticleConstants.CATEGORIES_SETTING & _moduleConfiguration.TabID.ToString()).ToString = Null.NullString Or Settings(ArticleConstants.CATEGORIES_SETTING & _moduleConfiguration.TabID.ToString()).ToString = "-1") Then
                        Dim categories As String() = Settings(ArticleConstants.CATEGORIES_SETTING & _moduleConfiguration.TabID.ToString()).ToString().Split(","c)
                        Dim cats As New List(Of Integer)

                        For Each category As String In categories
                            If (IsNumeric(category)) Then
                                cats.Add(Convert.ToInt32(category))
                            End If
                        Next

                        Return cats.ToArray()
                    Else
                        Return Nothing
                    End If
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public ReadOnly Property FilterSingleCategory() As Integer
            Get
                If (Settings.Contains(ArticleConstants.CATEGORIES_FILTER_SINGLE_SETTING)) Then
                    If (IsNumeric(Settings(ArticleConstants.CATEGORIES_FILTER_SINGLE_SETTING))) Then
                        Return Convert.ToInt32(Settings(ArticleConstants.CATEGORIES_FILTER_SINGLE_SETTING))
                    Else
                        Return Null.NullInteger
                    End If
                Else
                    Return Null.NullInteger
                End If
            End Get
        End Property

        Public ReadOnly Property ImageJQueryPath() As String
            Get
                If (Settings.Contains(ArticleConstants.IMAGE_JQUERY_PATH)) Then
                    Return Settings(ArticleConstants.IMAGE_JQUERY_PATH).ToString()
                Else
                    Return "includes/fancybox/jquery.fancybox.pack.js"
                End If
            End Get
        End Property

        Public ReadOnly Property ImageThumbnailType() As ThumbnailType
            Get
                If (Settings.Contains(ArticleConstants.IMAGE_THUMBNAIL_SETTING)) Then
                    Return CType([Enum].Parse(GetType(ThumbnailType), Settings(ArticleConstants.IMAGE_THUMBNAIL_SETTING).ToString()), ThumbnailType)
                Else
                    Return ArticleConstants.DEFAULT_IMAGE_THUMBNAIL
                End If
            End Get
        End Property

        Public ReadOnly Property IncludeJQuery() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.INCLUDE_JQUERY_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.INCLUDE_JQUERY_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property MatchCategories() As MatchOperatorType
            Get
                If (Settings.Contains(ArticleConstants.MATCH_OPERATOR_SETTING)) Then
                    Return CType([Enum].Parse(GetType(MatchOperatorType), Settings(ArticleConstants.MATCH_OPERATOR_SETTING).ToString()), MatchOperatorType)
                Else
                    Return MatchOperatorType.MatchAny
                End If
            End Get
        End Property

        Public ReadOnly Property MaxArticles() As Integer
            Get
                If (Settings.Contains(ArticleConstants.MAX_ARTICLES_SETTING)) Then
                    If (IsNumeric(Settings(ArticleConstants.MAX_ARTICLES_SETTING).ToString())) Then
                        Return Convert.ToInt32(Settings(ArticleConstants.MAX_ARTICLES_SETTING).ToString())
                    Else
                        Return Null.NullInteger
                    End If
                Else
                    Return Null.NullInteger
                End If
            End Get
        End Property

        Public ReadOnly Property MaxAge() As Integer
            Get
                If (Settings.Contains(ArticleConstants.MAX_AGE_SETTING)) Then
                    If (IsNumeric(Settings(ArticleConstants.MAX_AGE_SETTING).ToString())) Then
                        Return Convert.ToInt32(Settings(ArticleConstants.MAX_AGE_SETTING).ToString())
                    Else
                        Return Null.NullInteger
                    End If
                Else
                    Return Null.NullInteger
                End If
            End Get
        End Property

        Public ReadOnly Property MaxImageHeight() As Integer
            Get
                If (Settings.Contains(ArticleConstants.IMAGE_MAX_HEIGHT_SETTING)) Then
                    If (IsNumeric(Settings(ArticleConstants.IMAGE_MAX_HEIGHT_SETTING).ToString())) Then
                        Return Convert.ToInt32(Settings(ArticleConstants.IMAGE_MAX_HEIGHT_SETTING).ToString())
                    Else
                        Return ArticleConstants.DEFAULT_IMAGE_MAX_HEIGHT
                    End If
                Else
                    Return ArticleConstants.DEFAULT_IMAGE_MAX_HEIGHT
                End If
            End Get
        End Property

        Public ReadOnly Property MaxImageWidth() As Integer
            Get
                If (Settings.Contains(ArticleConstants.IMAGE_MAX_WIDTH_SETTING)) Then
                    If (IsNumeric(Settings(ArticleConstants.IMAGE_MAX_WIDTH_SETTING).ToString())) Then
                        Return Convert.ToInt32(Settings(ArticleConstants.IMAGE_MAX_WIDTH_SETTING).ToString())
                    Else
                        Return ArticleConstants.DEFAULT_IMAGE_MAX_WIDTH
                    End If
                Else
                    Return ArticleConstants.DEFAULT_IMAGE_MAX_WIDTH
                End If
            End Get
        End Property

        Public ReadOnly Property MenuPosition() As MenuPositionType
            Get
                If (Settings.Contains(ArticleConstants.MENU_POSITION_TYPE)) Then
                    Return CType([Enum].Parse(GetType(MenuPositionType), Settings(ArticleConstants.MENU_POSITION_TYPE).ToString()), MenuPositionType)
                Else
                    Return MenuPositionType.Top
                End If
            End Get
        End Property

        Public ReadOnly Property NotFeaturedOnly() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.SHOW_NOT_FEATURED_ONLY_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.SHOW_NOT_FEATURED_ONLY_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property NotifyAuthorOnComment() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.NOTIFY_COMMENT_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.NOTIFY_COMMENT_SETTING))
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property NotifyAuthorOnApproval() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.NOTIFY_APPROVAL_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.NOTIFY_APPROVAL_SETTING))
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property NotifyDefault() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.NOTIFY_DEFAULT_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.NOTIFY_DEFAULT_SETTING))
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property NotifyEmailOnComment() As String
            Get
                If (Settings.Contains(ArticleConstants.NOTIFY_COMMENT_SETTING_EMAIL)) Then
                    Return Settings(ArticleConstants.NOTIFY_COMMENT_SETTING_EMAIL).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property NotifyApproverForCommentApproval() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.NOTIFY_COMMENT_APPROVAL_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.NOTIFY_COMMENT_APPROVAL_SETTING))
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property NotifyEmailForCommentApproval() As String
            Get
                If (Settings.Contains(ArticleConstants.NOTIFY_COMMENT_APPROVAL_EMAIL_SETTING)) Then
                    Return Settings(ArticleConstants.NOTIFY_COMMENT_APPROVAL_EMAIL_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property NotSecuredOnly() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.SHOW_NOT_SECURED_ONLY_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.SHOW_NOT_SECURED_ONLY_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property ProcessPostTokens() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.PROCESS_POST_TOKENS)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.PROCESS_POST_TOKENS).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property ResizeImages() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.IMAGE_RESIZE_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.IMAGE_RESIZE_SETTING).ToString())
                Else
                    Return ArticleConstants.DEFAULT_IMAGE_RESIZE
                End If
            End Get
        End Property

        Public ReadOnly Property SecuredOnly() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.SHOW_SECURED_ONLY_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.SHOW_SECURED_ONLY_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property ShortenedID() As String
            Get
                If (Settings.Contains(ArticleConstants.SEO_SHORTEN_ID_SETTING)) Then
                    Return Settings(ArticleConstants.SEO_SHORTEN_ID_SETTING).ToString()
                Else
                    Return "ID"
                End If
            End Get
        End Property

        Public ReadOnly Property SortBy() As String
            Get
                If (Settings.Contains(ArticleConstants.SORT_BY)) Then
                    Return Settings(ArticleConstants.SORT_BY).ToString()
                Else
                    Return ArticleConstants.DEFAULT_SORT_BY
                End If
            End Get
        End Property

        Public ReadOnly Property SortDirection() As String
            Get
                If (Settings.Contains(ArticleConstants.SORT_DIRECTION)) Then
                    Return Settings(ArticleConstants.SORT_DIRECTION).ToString()
                Else
                    Return ArticleConstants.DEFAULT_SORT_DIRECTION
                End If
            End Get
        End Property

        Public ReadOnly Property SortDirectionComments() As Integer
            Get
                If (Settings.Contains(ArticleConstants.COMMENT_SORT_DIRECTION_SETTING)) Then
                    Return Convert.ToInt32(Settings(ArticleConstants.COMMENT_SORT_DIRECTION_SETTING).ToString())
                Else
                    Return 0
                End If
            End Get
        End Property

        Public ReadOnly Property SyndicationLinkType() As SyndicationLinkType
            Get
                If (Settings.Contains(ArticleConstants.SYNDICATION_LINK_TYPE)) Then
                    Return CType([Enum].Parse(GetType(SyndicationLinkType), Settings(ArticleConstants.SYNDICATION_LINK_TYPE).ToString()), SyndicationLinkType)
                Else
                    Return SyndicationLinkType.Article
                End If
            End Get
        End Property

        Public ReadOnly Property SyndicationEnclosureType() As SyndicationEnclosureType
            Get
                If (Settings.Contains(ArticleConstants.SYNDICATION_ENCLOSURE_TYPE)) Then
                    Return CType([Enum].Parse(GetType(SyndicationEnclosureType), Settings(ArticleConstants.SYNDICATION_ENCLOSURE_TYPE).ToString()), SyndicationEnclosureType)
                Else
                    Return SyndicationEnclosureType.Attachment
                End If
            End Get
        End Property

        Public ReadOnly Property SyndicationSummaryLength() As Integer
            Get
                If (Settings.Contains(ArticleConstants.SYNDICATION_SUMMARY_LENGTH)) Then
                    Return Convert.ToInt32(Settings(ArticleConstants.SYNDICATION_SUMMARY_LENGTH).ToString())
                Else
                    Return Null.NullInteger
                End If
            End Get
        End Property

        Public ReadOnly Property SyndicationMaxCount() As Integer
            Get
                If (Settings.Contains(ArticleConstants.SYNDICATION_MAX_COUNT)) Then
                    Return Convert.ToInt32(Settings(ArticleConstants.SYNDICATION_MAX_COUNT).ToString())
                Else
                    Return 50
                End If
            End Get
        End Property

        Public ReadOnly Property SyndicationImagePath() As String
            Get
                If (Settings.Contains(ArticleConstants.SYNDICATION_IMAGE_PATH)) Then
                    Return Settings(ArticleConstants.SYNDICATION_IMAGE_PATH).ToString()
                Else
                    Return "~/DesktopModules/DnnForge - NewsArticles/Images/rssbutton.gif"
                End If
            End Get
        End Property

        Public ReadOnly Property UseCanonicalLink() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.SEO_USE_CANONICAL_LINK_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.SEO_USE_CANONICAL_LINK_SETTING))
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property ExpandMetaInformation() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.SEO_EXPAND_META_INFORMATION_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.SEO_EXPAND_META_INFORMATION_SETTING))
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property UniquePageTitles() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.SEO_UNIQUE_PAGE_TITLES_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.SEO_UNIQUE_PAGE_TITLES_SETTING))
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property CaptchaType() As CaptchaType
            Get
                Dim retval As CaptchaType = CaptchaType.None
                If (Settings.Contains(ArticleConstants.USE_CAPTCHA_SETTING)) Then
                    'there's an existing module setting, so make sure we don't change behavior for that
                    If Convert.ToBoolean(Settings(ArticleConstants.USE_CAPTCHA_SETTING).ToString()) Then
                        retval = CaptchaType.DnnCore
                    End If
                Else
                    If (Settings.Contains(ArticleConstants.CAPTCHATYPE_SETTING)) Then
                        retval = CType(System.Enum.Parse(GetType(CaptchaType), Settings(ArticleConstants.CAPTCHATYPE_SETTING)), CaptchaType)
                    End If
                End If
                Return retval
            End Get
        End Property

        Public ReadOnly Property ReCaptchaSiteKey() As String
            Get
                If Settings.ContainsKey(ArticleConstants.RECAPTCHA_SITEKEY_SETTING) Then
                    Return Settings(ArticleConstants.RECAPTCHA_SITEKEY_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property ReCaptchaSecretKey() As String
            Get
                If Settings.ContainsKey(ArticleConstants.RECAPTCHA_SECRETKEY_SETTING) Then
                    Return Settings(ArticleConstants.RECAPTCHA_SECRETKEY_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property UrlModeType() As UrlModeType
            Get
                If (Settings.Contains(ArticleConstants.SEO_URL_MODE_SETTING)) Then
                    Try
                        Return CType([Enum].Parse(GetType(UrlModeType), Settings(ArticleConstants.SEO_URL_MODE_SETTING).ToString()), UrlModeType)
                    Catch
                        Return UrlModeType.Shorterned
                    End Try
                Else
                    Return UrlModeType.Shorterned
                End If
            End Get
        End Property

        Public ReadOnly Property WatermarkEnabled() As Boolean
            Get
                If (_settings.Contains(ArticleConstants.IMAGE_WATERMARK_ENABLED_SETTING)) Then
                    Return Convert.ToBoolean(_settings(ArticleConstants.IMAGE_WATERMARK_ENABLED_SETTING).ToString())
                Else
                    Return ArticleConstants.IMAGE_WATERMARK_ENABLED_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property WatermarkText() As String
            Get
                If (_settings.Contains(ArticleConstants.IMAGE_WATERMARK_TEXT_SETTING)) Then
                    Return _settings(ArticleConstants.IMAGE_WATERMARK_TEXT_SETTING).ToString()
                Else
                    Return ArticleConstants.IMAGE_WATERMARK_TEXT_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property WatermarkImage() As String
            Get
                If (_settings.Contains(ArticleConstants.IMAGE_WATERMARK_IMAGE_SETTING)) Then
                    Return _settings(ArticleConstants.IMAGE_WATERMARK_IMAGE_SETTING).ToString()
                Else
                    Return ArticleConstants.IMAGE_WATERMARK_IMAGE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property WatermarkPosition() As WatermarkPosition
            Get
                If (_settings.Contains(ArticleConstants.IMAGE_WATERMARK_IMAGE_POSITION_SETTING)) Then
                    Try
                        Return CType([Enum].Parse(GetType(WatermarkPosition), _settings(ArticleConstants.IMAGE_WATERMARK_IMAGE_POSITION_SETTING).ToString()), WatermarkPosition)
                    Catch
                        Return ArticleConstants.IMAGE_WATERMARK_IMAGE_POSITION_SETTING_DEFAULT
                    End Try
                Else
                    Return ArticleConstants.IMAGE_WATERMARK_IMAGE_POSITION_SETTING_DEFAULT
                End If
            End Get
        End Property






















        Public ReadOnly Property PageSize() As Integer
            Get
                If (Settings.Contains("Number")) Then
                    If (Convert.ToInt32(Settings("Number").ToString()) > 0) Then
                        Return Convert.ToInt32(Settings("Number").ToString())
                    Else
                        If (Convert.ToInt32(Settings("Number").ToString()) = -1) Then
                            Return 10000
                        Else
                            Return 10
                        End If
                    End If
                Else
                    Return 10
                End If
            End Get
        End Property

        Public ReadOnly Property IsCommentsAnonymous() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_ANONYMOUS_COMMENTS_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_ANONYMOUS_COMMENTS_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property IsCommentsEnabled() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_COMMENTS_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_COMMENTS_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property IsCommentModerationEnabled() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_COMMENT_MODERATION_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_COMMENT_MODERATION_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property IsCommentWebsiteHidden() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.COMMENT_HIDE_WEBSITE_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.COMMENT_HIDE_WEBSITE_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property IsRateable() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated) Then
                    Return EnableRatingsAuthenticated
                Else
                    Return EnableRatingsAnonymous
                End If
            End Get
        End Property

        Public ReadOnly Property IsSyndicationEnabled() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_SYNDICATION_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_SYNDICATION_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property IsIncomingTrackbackEnabled() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.ENABLE_INCOMING_TRACKBACK_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.ENABLE_INCOMING_TRACKBACK_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property LaunchLinks() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.LAUNCH_LINKS)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.LAUNCH_LINKS).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property DisplayMode() As DisplayType
            Get
                If (Settings.Contains(ArticleConstants.DISPLAY_MODE)) Then
                    Return CType([Enum].Parse(GetType(DisplayType), Settings(ArticleConstants.DISPLAY_MODE).ToString()), DisplayType)
                Else
                    Return DisplayType.FullName
                End If
            End Get
        End Property

        Public ReadOnly Property RelatedMode() As RelatedType
            Get
                If (Settings.Contains(ArticleConstants.RELATED_MODE)) Then
                    Return CType([Enum].Parse(GetType(RelatedType), Settings(ArticleConstants.RELATED_MODE).ToString()), RelatedType)
                Else
                    Return RelatedType.MatchCategoriesAnyTagsAny
                End If
            End Get
        End Property

        Public ReadOnly Property Template() As String
            Get
                If (Settings.Contains(ArticleConstants.TEMPLATE_SETTING)) Then
                    Return Settings(ArticleConstants.TEMPLATE_SETTING).ToString()
                Else
                    Return "Standard"
                End If
            End Get
        End Property

        Public ReadOnly Property SEOTitleSpecified() As Boolean
            Get
                Return Settings.Contains(ArticleConstants.SEO_TITLE_SETTING)
            End Get
        End Property

        Public ReadOnly Property ShowPending() As Boolean
            Get
                If (Settings.Contains(ArticleConstants.SHOW_PENDING_SETTING)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.SHOW_PENDING_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property TitleReplacement() As TitleReplacementType
            Get
                If (Settings.Contains(ArticleConstants.TITLE_REPLACEMENT_TYPE)) Then
                    Return CType([Enum].Parse(GetType(TitleReplacementType), Settings(ArticleConstants.TITLE_REPLACEMENT_TYPE).ToString()), TitleReplacementType)
                Else
                    Return TitleReplacementType.Dash
                End If
            End Get
        End Property

        Public ReadOnly Property TextEditorWidth() As String
            Get
                If (Settings.Contains(ArticleConstants.TEXT_EDITOR_WIDTH)) Then
                    Return Settings(ArticleConstants.TEXT_EDITOR_WIDTH).ToString()
                Else
                    Return "100%"
                End If
            End Get
        End Property

        Public ReadOnly Property TextEditorHeight() As String
            Get
                If (Settings.Contains(ArticleConstants.TEXT_EDITOR_HEIGHT)) Then
                    Return Settings(ArticleConstants.TEXT_EDITOR_HEIGHT).ToString()
                Else
                    Return "400"
                End If
            End Get
        End Property

        Public ReadOnly Property TextEditorSummaryMode() As TextEditorModeType
            Get
                If (Settings.Contains(ArticleConstants.TEXT_EDITOR_SUMMARY_MODE)) Then
                    Return CType([Enum].Parse(GetType(TextEditorModeType), Settings(ArticleConstants.TEXT_EDITOR_SUMMARY_MODE).ToString()), TextEditorModeType)
                Else
                    Return TextEditorModeType.Rich
                End If
            End Get
        End Property

        Public ReadOnly Property TextEditorSummaryWidth() As String
            Get
                If (Settings.Contains(ArticleConstants.TEXT_EDITOR_SUMMARY_WIDTH)) Then
                    Return Settings(ArticleConstants.TEXT_EDITOR_SUMMARY_WIDTH).ToString()
                Else
                    Return "100%"
                End If
            End Get
        End Property

        Public ReadOnly Property TextEditorSummaryHeight() As String
            Get
                If (Settings.Contains(ArticleConstants.TEXT_EDITOR_SUMMARY_HEIGHT)) Then
                    Return Settings(ArticleConstants.TEXT_EDITOR_SUMMARY_HEIGHT).ToString()
                Else
                    Return "400"
                End If
            End Get
        End Property

        Public ReadOnly Property TwitterName() As String
            Get
                If (Settings.Contains(ArticleConstants.TWITTER_USERNAME)) Then
                    Return Settings(ArticleConstants.TWITTER_USERNAME).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property TwitterBitLyLogin() As String
            Get
                If (Settings.Contains(ArticleConstants.TWITTER_BITLY_LOGIN)) Then
                    Return Settings(ArticleConstants.TWITTER_BITLY_LOGIN).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property TwitterBitLyAPIKey() As String
            Get
                If (Settings.Contains(ArticleConstants.TWITTER_BITLY_API_KEY)) Then
                    Return Settings(ArticleConstants.TWITTER_BITLY_API_KEY).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property ServerTimeZone() As Integer
            Get
                If (Settings.Contains(ArticleConstants.SERVER_TIMEZONE)) Then
                    If (IsNumeric(Settings(ArticleConstants.SERVER_TIMEZONE).ToString())) Then
                        Return Convert.ToInt32(Settings(ArticleConstants.SERVER_TIMEZONE).ToString())
                    Else
                        Return _portalSettings.TimeZone.GetUtcOffset(DateTime.Now).TotalHours
                    End If
                Else
                    Return _portalSettings.TimeZone.GetUtcOffset(DateTime.Now).TotalHours
                End If
            End Get
        End Property

        Public ReadOnly Property TemplatePath() As String
            Get
                Return _portalSettings.HomeDirectoryMapPath & "DnnForge - NewsArticles/Templates/" & Template & "/"
            End Get
        End Property

        Public ReadOnly Property SecureUrl() As String
            Get
                If (Settings.Contains(ArticleConstants.PERMISSION_SECURE_URL_SETTING)) Then
                    Return Settings(ArticleConstants.PERMISSION_SECURE_URL_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property RoleGroupID() As Integer
            Get
                If (Settings.Contains(ArticleConstants.PERMISSION_ROLE_GROUP_ID)) Then
                    Return Convert.ToInt32(Settings(ArticleConstants.PERMISSION_ROLE_GROUP_ID).ToString())
                Else
                    Return -1
                End If
            End Get
        End Property

        Public ReadOnly Property IsApprover() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                If (IsAdmin) Then
                    Return True
                End If

                If (Settings.Contains(ArticleConstants.PERMISSION_APPROVAL_SETTING)) Then
                    Return PortalSecurity.IsInRoles(Settings(ArticleConstants.PERMISSION_APPROVAL_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property IsCategoriesEnabled() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                If (Settings.Contains(ArticleConstants.PERMISSION_CATEGORIES_SETTING)) Then
                    Return IsInRoles(Settings(ArticleConstants.PERMISSION_CATEGORIES_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property IsExcerptEnabled() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                If (Settings.Contains(ArticleConstants.PERMISSION_EXCERPT_SETTING)) Then
                    Return IsInRoles(Settings(ArticleConstants.PERMISSION_EXCERPT_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property IsImagesEnabled() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                If (Settings.Contains(ArticleConstants.PERMISSION_IMAGE_SETTING)) Then
                    Return IsInRoles(Settings(ArticleConstants.PERMISSION_IMAGE_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property IsFilesEnabled() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                If (Settings.Contains(ArticleConstants.PERMISSION_FILE_SETTING)) Then
                    Return IsInRoles(Settings(ArticleConstants.PERMISSION_FILE_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property IsLinkEnabled() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                If (Settings.Contains(ArticleConstants.PERMISSION_LINK_SETTING)) Then
                    Return IsInRoles(Settings(ArticleConstants.PERMISSION_LINK_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property IsFeaturedEnabled() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                If (Settings.Contains(ArticleConstants.PERMISSION_FEATURE_SETTING)) Then
                    Return PortalSecurity.IsInRoles(Settings(ArticleConstants.PERMISSION_FEATURE_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property IsSecureEnabled() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                If (Settings.Contains(ArticleConstants.PERMISSION_SECURE_SETTING)) Then
                    Return PortalSecurity.IsInRoles(Settings(ArticleConstants.PERMISSION_SECURE_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property IsPublishEnabled() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                If (Settings.Contains(ArticleConstants.PERMISSION_PUBLISH_SETTING)) Then
                    Return PortalSecurity.IsInRoles(Settings(ArticleConstants.PERMISSION_PUBLISH_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property IsExpiryEnabled() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                If (Settings.Contains(ArticleConstants.PERMISSION_EXPIRY_SETTING)) Then
                    Return PortalSecurity.IsInRoles(Settings(ArticleConstants.PERMISSION_EXPIRY_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property IsMetaEnabled() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                If (Settings.Contains(ArticleConstants.PERMISSION_META_SETTING)) Then
                    If (Settings(ArticleConstants.PERMISSION_META_SETTING).ToString() = "") Then
                        Return False
                    End If
                    Return PortalSecurity.IsInRoles(Settings(ArticleConstants.PERMISSION_META_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property IsCustomEnabled() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                If (Settings.Contains(ArticleConstants.PERMISSION_CUSTOM_SETTING)) Then
                    If (Settings(ArticleConstants.PERMISSION_CUSTOM_SETTING).ToString() = "") Then
                        Return False
                    End If
                    Return PortalSecurity.IsInRoles(Settings(ArticleConstants.PERMISSION_CUSTOM_SETTING).ToString())
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property IsAutoApprover() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                If (IsAdmin) Then
                    Return True
                End If

                If (Settings.Contains(ArticleConstants.PERMISSION_AUTO_APPROVAL_SETTING)) Then
                    Return PortalSecurity.IsInRoles(Settings(ArticleConstants.PERMISSION_AUTO_APPROVAL_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property IsAutoApproverComment() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                If (IsAdmin) Then
                    Return True
                End If

                If (Settings.Contains(ArticleConstants.PERMISSION_AUTO_APPROVAL_COMMENT_SETTING)) Then
                    Return PortalSecurity.IsInRoles(Settings(ArticleConstants.PERMISSION_AUTO_APPROVAL_COMMENT_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property IsAutoFeatured() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                If (Settings.Contains(ArticleConstants.PERMISSION_AUTO_FEATURE_SETTING)) Then

                    For Each role As String In Settings(ArticleConstants.PERMISSION_AUTO_FEATURE_SETTING).ToString().Split(New Char() {";"c})
                        If (role <> "") Then
                            If (UserController.Instance.GetCurrentUserInfo.IsInRole(role)) Then
                                Return True
                            End If
                        End If
                    Next
                    Return False
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property IsAutoSecured() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                If (Settings.Contains(ArticleConstants.PERMISSION_AUTO_SECURE_SETTING)) Then

                    For Each role As String In Settings(ArticleConstants.PERMISSION_AUTO_SECURE_SETTING).ToString().Split(New Char() {";"c})
                        If (role <> "") Then
                            If (UserController.Instance.GetCurrentUserInfo.IsInRole(role)) Then
                                Return True
                            End If
                        End If
                    Next
                    Return False
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property IsSubmitter() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                If (IsAdmin) Then
                    Return True
                End If

                If (Settings.Contains(ArticleConstants.PERMISSION_SUBMISSION_SETTING)) Then
                    Return PortalSecurity.IsInRoles(Settings(ArticleConstants.PERMISSION_SUBMISSION_SETTING).ToString())
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property IsAdmin() As Boolean
            Get
                If (HttpContext.Current.Request.IsAuthenticated = False) Then
                    Return False
                End If

                Dim blnHasModuleEditPermissions As Boolean = ModulePermissionController.CanAdminModule(_moduleConfiguration)

                If (blnHasModuleEditPermissions = False) Then
                    blnHasModuleEditPermissions = ModulePermissionController.CanEditModuleContent(_moduleConfiguration)
                End If

                If (blnHasModuleEditPermissions = False) Then
                    blnHasModuleEditPermissions = PortalSecurity.IsInRoles(_portalSettings.AdministratorRoleName)
                End If

                Return blnHasModuleEditPermissions
            End Get
        End Property

#End Region

    End Class

End Namespace