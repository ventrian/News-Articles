'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Namespace Ventrian.NewsArticles

    Public Class ArticleConstants

#Region " Constants "

        'Token Settings
        Public Const DEFAULT_TEMPLATE = "Standard"

        ' Basic Settings
        Public Const EXPAND_SUMMARY_SETTING As String = "ExpandSummary"

        Public Const ENABLE_RATINGS_AUTHENTICATED_SETTING As String = "EnableRatings"
        Public Const ENABLE_RATINGS_ANONYMOUS_SETTING As String = "EnableAnonymousRatings"

        Public Const JOURNAL_INTEGRATION_SETTING As String = "JournalIntegration"
        Public Const JOURNAL_INTEGRATION_GROUPS_SETTING As String = "JournalIntegrationGroups"

        Public Const ACTIVE_SOCIAL_SETTING As String = "ActiveSocialFeed"
        Public Const ACTIVE_SOCIAL_SUBMIT_SETTING As String = "ActiveSocialFeedSubmit"
        Public Const ACTIVE_SOCIAL_COMMENT_SETTING As String = "ActiveSocialFeedComment"
        Public Const ACTIVE_SOCIAL_RATE_SETTING As String = "ActiveSocialFeedRate"
        Public Const AUTHOR_SELECT_TYPE As String = "AuthorSelectType"
        Public Const ENABLE_CORE_SEARCH_SETTING As String = "EnableCoreSearch"
        Public Const ENABLE_SYNDICATION_SETTING As String = "EnableSyndication"
        Public Const ENABLE_SYNDICATION_ENCLOSURES_SETTING As String = "EnableSyndicationEnclosures"
        Public Const ENABLE_SYNDICATION_HTML_SETTING As String = "EnableSyndicationHtml"
        Public Const ENABLE_NOTIFICATION_PING_SETTING As String = "NotificationPing"
        Public Const ENABLE_AUTO_TRACKBACK_SETTING As String = "AutoTrackback"
        Public Const ENABLE_INCOMING_TRACKBACK_SETTING As String = "IncomingTrackback"
        Public Const PAGE_SIZE_SETTING As String = "Number"
        Public Const LAUNCH_LINKS As String = "LaunchLinks"
        Public Const BUBBLE_FEATURED_ARTICLES As String = "BubbleFeaturedArticles"
        Public Const REQUIRE_CATEGORY As String = "RequireCategory"
        Public Const DISPLAY_MODE As String = "DisplayMode"
        Public Const PROCESS_POST_TOKENS As String = "ProcessPostTokens"
        Public Const RELATED_MODE As String = "RelatedMode"
        Public Const TEMPLATE_SETTING As String = "Template"
        Public Const ENABLE_SEO_TITLE_SETTING As String = "EnableSEOTitle"
        Public Const SEO_TITLE_SETTING As String = "SEOTitle"
        Public Const TITLE_REPLACEMENT_TYPE As String = "TitleReplacementType"
        Public Const SMART_THINKER_STORY_FEED_SETTING As String = "EnableSmartThinkerStoryFeed"
        Public Const TEXT_EDITOR_WIDTH As String = "TextEditorWidth"
        Public Const TEXT_EDITOR_HEIGHT As String = "TextEditorHeight"
        Public Const TEXT_EDITOR_SUMMARY_MODE As String = "TextEditorSummaryMode"
        Public Const TEXT_EDITOR_SUMMARY_WIDTH As String = "TextEditorSummaryWidth"
        Public Const TEXT_EDITOR_SUMMARY_HEIGHT As String = "TextEditorSummaryHeight"
        Public Const SERVER_TIMEZONE As String = "ServerTimeZone"
        Public Const SORT_BY As String = "SortBy"
        Public Const SORT_DIRECTION As String = "SortDirection"
        Public Const SYNDICATION_LINK_TYPE As String = "SyndicationLinkType"
        Public Const SYNDICATION_SUMMARY_LENGTH As String = "SyndicationSummaryLength"
        Public Const SYNDICATION_MAX_COUNT As String = "SyndicationMaxCount"
        Public Const SYNDICATION_ENCLOSURE_TYPE As String = "SyndicationEnclosureType"
        Public Const SYNDICATION_IMAGE_PATH As String = "SyndicationImagePath"
        Public Const MENU_POSITION_TYPE As String = "MenuPositionType"

        ' Image Settings
        Public Const INCLUDE_JQUERY_SETTING As String = "IncludeJQuery"
        Public Const IMAGE_JQUERY_PATH As String = "JQueryPath"
        Public Const ENABLE_EXTERNAL_IMAGES_SETTING As String = "EnableImagesExternal"
        Public Const ENABLE_UPLOAD_IMAGES_SETTING As String = "EnableImagesUpload"
        Public Const ENABLE_PORTAL_IMAGES_SETTING As String = "EnableImages"
        Public Const DEFAULT_IMAGES_FOLDER_SETTING As String = "DefaultImagesFolder"
        Public Const DEFAULT_FILES_FOLDER_SETTING As String = "DefaultFilesFolder"
        Public Const IMAGE_RESIZE_SETTING As String = "ResizeImages"
        Public Const IMAGE_THUMBNAIL_SETTING As String = "ImageThumbnailType"
        Public Const IMAGE_MAX_WIDTH_SETTING As String = "ImageMaxWidth"
        Public Const IMAGE_MAX_HEIGHT_SETTING As String = "ImageMaxHeight"

        Public Const IMAGE_WATERMARK_ENABLED_SETTING As String = "ImageWatermarkEnabled"
        Public Const IMAGE_WATERMARK_ENABLED_SETTING_DEFAULT As Boolean = False
        Public Const IMAGE_WATERMARK_TEXT_SETTING As String = "ImageWatermarkText"
        Public Const IMAGE_WATERMARK_TEXT_SETTING_DEFAULT As String = ""
        Public Const IMAGE_WATERMARK_IMAGE_SETTING As String = "ImageWatermarkImage"
        Public Const IMAGE_WATERMARK_IMAGE_SETTING_DEFAULT As String = ""
        Public Const IMAGE_WATERMARK_IMAGE_POSITION_SETTING As String = "ImageWatermarkImagePosition"
        Public Const IMAGE_WATERMARK_IMAGE_POSITION_SETTING_DEFAULT As WatermarkPosition = WatermarkPosition.BottomRight

        Public Const DEFAULT_IMAGE_RESIZE As Boolean = True
        Public Const DEFAULT_IMAGE_THUMBNAIL As ThumbnailType = ThumbnailType.Proportion
        Public Const DEFAULT_IMAGE_MAX_WIDTH As Integer = 600
        Public Const DEFAULT_IMAGE_MAX_HEIGHT As Integer = 480
        Public Const DEFAULT_THUMBNAIL_HEIGHT As Integer = 100
        Public Const DEFAULT_THUMBNAIL_WIDTH As Integer = 100

        ' Category Settings
        Public Const ARCHIVE_CURRENT_ARTICLES_SETTING As String = "ArchiveCurrentArticles"
        Public Const ARCHIVE_CURRENT_ARTICLES_SETTING_DEFAULT As Boolean = True
        Public Const ARCHIVE_CATEGORIES_SETTING As String = "ArchiveCategories"
        Public Const ARCHIVE_CATEGORIES_SETTING_DEFAULT As Boolean = True
        Public Const ARCHIVE_AUTHOR_SETTING As String = "ArchiveAuthor"
        Public Const ARCHIVE_AUTHOR_SETTING_DEFAULT As Boolean = True
        Public Const ARCHIVE_MONTH_SETTING As String = "ArchiveMonth"
        Public Const ARCHIVE_MONTH_SETTING_DEFAULT As Boolean = True

        ' Category Settings
        Public Const DEFAULT_CATEGORIES_SETTING As String = "DefaultCategories"
        Public Const CATEGORY_SELECTION_HEIGHT_SETTING As String = "CategorySelectionHeight"
        Public Const CATEGORY_SELECTION_HEIGHT_DEFAULT As Integer = 150
        Public Const CATEGORY_BREADCRUMB_SETTING As String = "CategoryBreadcrumb"
        Public Const CATEGORY_BREADCRUMB_SETTING_DEFAULT As Boolean = True
        Public Const CATEGORY_NAME_SETTING As String = "CategoryName"
        Public Const CATEGORY_NAME_SETTING_DEFAULT As Boolean = True
        Public Const CATEGORY_FILTER_SUBMIT_SETTING As String = "CategoryFilterSubmit"
        Public Const CATEGORY_FILTER_SUBMIT_SETTING_DEFAULT As Boolean = False
        Public Const CATEGORY_SORT_SETTING As String = "CategorySortType"
        Public Const CATEGORY_SORT_SETTING_DEFAULT As CategorySortType = CategorySortType.SortOrder

        ' Category Security Settings
        Public Const PERMISSION_CATEGORY_VIEW_SETTING As String = "PermissionCategoryView"
        Public Const PERMISSION_CATEGORY_SUBMIT_SETTING As String = "PermissionCategorySubmit"

        ' Comment Settings
        Public Const ENABLE_COMMENTS_SETTING As String = "EnableComments"
        Public Const ENABLE_ANONYMOUS_COMMENTS_SETTING As String = "EnableAnonymousComments"
        Public Const ENABLE_COMMENT_MODERATION_SETTING As String = "EnableCommentModeration"
        Public Const COMMENT_HIDE_WEBSITE_SETTING As String = "CommentHideWebsite"
        Public Const COMMENT_REQUIRE_NAME_SETTING As String = "CommentRequireName"
        Public Const COMMENT_REQUIRE_EMAIL_SETTING As String = "CommentRequireEmail"
        Public Const USE_CAPTCHA_SETTING As String = "UseCaptcha"
        Public Const NOTIFY_DEFAULT_SETTING As String = "NotifyDefault"
        Public Const COMMENT_SORT_DIRECTION_SETTING As String = "CommentSortDirection"
        Public Const COMMENT_AKISMET_SETTING As String = "CommentAkismet"

        ' Content Sharing Settings
        Public Const CONTENT_SHARING_SETTING As String = "ContentSharingPortals"

        ' Filter Settings
        Public Const MAX_ARTICLES_SETTING As String = "MaxArticles"
        Public Const MAX_AGE_SETTING As String = "MaxArticlesAge"
        Public Const CATEGORIES_SETTING As String = "Categories"
        Public Const CATEGORIES_FILTER_SINGLE_SETTING As String = "CategoriesFilterSingle"
        Public Const SHOW_PENDING_SETTING As String = "ShowPending"
        Public Const SHOW_FEATURED_ONLY_SETTING As String = "ShowFeaturedOnly"
        Public Const SHOW_NOT_FEATURED_ONLY_SETTING As String = "ShowNotFeaturedOnly"
        Public Const SHOW_SECURED_ONLY_SETTING As String = "ShowSecuredOnly"
        Public Const SHOW_NOT_SECURED_ONLY_SETTING As String = "ShowNotSecuredOnly"
        Public Const AUTHOR_SETTING As String = "Author"
        Public Const AUTHOR_DEFAULT_SETTING As String = "AuthorDefault"
        Public Const MATCH_OPERATOR_SETTING As String = "MatchOperator"

        Public Const AUTHOR_USERID_FILTER_SETTING As String = "AuthorUserIDFilter"
        Public Const AUTHOR_USERID_PARAM_SETTING As String = "AuthorUserIDParam"
        Public Const AUTHOR_USERNAME_FILTER_SETTING As String = "AuthorUsernameFilter"
        Public Const AUTHOR_USERNAME_PARAM_SETTING As String = "AuthorUsernameParam"
        Public Const AUTHOR_LOGGED_IN_USER_FILTER_SETTING As String = "AuthorLoggedInUserFilter"

        ' Security Settings
        Public Const PERMISSION_ROLE_GROUP_ID As String = "RoleGroupIDFilter"
        Public Const PERMISSION_SECURE_SETTING As String = "SecureRoles"
        Public Const PERMISSION_SECURE_URL_SETTING As String = "SecureUrl"
        Public Const PERMISSION_AUTO_SECURE_SETTING As String = "AutoSecureRoles"
        Public Const PERMISSION_SUBMISSION_SETTING As String = "SubmissionRoles"
        Public Const PERMISSION_APPROVAL_SETTING As String = "ApprovalRoles"
        Public Const PERMISSION_AUTO_APPROVAL_SETTING As String = "AutoApprovalRoles"
        Public Const PERMISSION_AUTO_APPROVAL_COMMENT_SETTING As String = "AutoApprovalCommentRoles"
        Public Const PERMISSION_FEATURE_SETTING As String = "FeatureRoles"
        Public Const PERMISSION_AUTO_FEATURE_SETTING As String = "AutoFeatureRoles"

        ' Security Form Settings
        Public Const PERMISSION_CATEGORIES_SETTING As String = "PermissionCategoriesRoles"
        Public Const PERMISSION_EXCERPT_SETTING As String = "PermissionExcerptRoles"
        Public Const PERMISSION_IMAGE_SETTING As String = "PermissionImageRoles"
        Public Const PERMISSION_FILE_SETTING As String = "PermissionFileRoles"
        Public Const PERMISSION_LINK_SETTING As String = "PermissionLinkRoles"
        Public Const PERMISSION_PUBLISH_SETTING As String = "PermissionPublishRoles"
        Public Const PERMISSION_EXPIRY_SETTING As String = "PermissionExpiryRoles"
        Public Const PERMISSION_META_SETTING As String = "PermissionMetaRoles"
        Public Const PERMISSION_CUSTOM_SETTING As String = "PermissionCustomRoles"

        ' Admin Settings
        Public Const PERMISSION_SITE_TEMPLATES_SETTING As String = "PermissionSiteTemplates"

        ' Notification Settings
        Public Const NOTIFY_SUBMISSION_SETTING As String = "NotifySubmission"
        Public Const NOTIFY_SUBMISSION_SETTING_EMAIL As String = "NotifySubmissionEmail"
        Public Const NOTIFY_APPROVAL_SETTING As String = "NotifyApproval"
        Public Const NOTIFY_COMMENT_SETTING As String = "NotifyComment"
        Public Const NOTIFY_COMMENT_SETTING_EMAIL As String = "NotifyCommentEmail"
        Public Const NOTIFY_COMMENT_APPROVAL_SETTING As String = "NotifyCommentApproval"
        Public Const NOTIFY_COMMENT_APPROVAL_EMAIL_SETTING As String = "NotifyCommentApprovalEmail"

        ' SEO Settings
        Public Const SEO_ALWAYS_SHOW_PAGEID_SETTING As String = "AlwaysShowPageID"
        Public Const SEO_URL_MODE_SETTING As String = "SEOUrlMode"
        Public Const SEO_SHORTEN_ID_SETTING As String = "SEOShorternID"
        Public Const SEO_USE_CANONICAL_LINK_SETTING As String = "SEOUseCanonicalLink"
        Public Const SEO_EXPAND_META_INFORMATION_SETTING As String = "SEOExpandMetaInformation"
        Public Const SEO_UNIQUE_PAGE_TITLES_SETTING As String = "SEOUniquePageTitles"

        ' SEO Settings
        Public Const TWITTER_USERNAME As String = "NA-TwitterUsername"
        Public Const TWITTER_BITLY_LOGIN As String = "NA-BitLyLogin"
        Public Const TWITTER_BITLY_API_KEY As String = "NA-BitLyAPI"

        ' Latest Articles 
        Public Const LATEST_ARTICLES_TAB_ID As String = "LatestArticlesTabID"
        Public Const LATEST_ARTICLES_MODULE_ID As String = "LatestArticlesModuleID"
        Public Const LATEST_ARTICLES_CATEGORIES As String = "LatestArticlesCategories"
        Public Const LATEST_ARTICLES_CATEGORIES_EXCLUDE As String = "LatestArticlesCategoriesExclude"
        Public Const LATEST_ARTICLES_MATCH_OPERATOR As String = "LatestArticlesMatchOperator"
        Public Const LATEST_ARTICLES_TAGS As String = "LatestArticlesTags"
        Public Const LATEST_ARTICLES_TAGS_MATCH_OPERATOR As String = "LatestArticlesTagsMatchOperator"
        Public Const LATEST_ARTICLES_IDS As String = "LatestArticlesIDS"
        Public Const LATEST_ARTICLES_COUNT As String = "LatestArticlesCount"
        Public Const LATEST_ARTICLES_START_POINT As String = "LatestArticlesStartPoint"
        Public Const LATEST_ARTICLES_MAX_AGE As String = "LatestArticlesMaxAge"
        Public Const LATEST_ARTICLES_START_DATE As String = "LatestArticlesStartDate"
        Public Const LATEST_ARTICLES_SHOW_PENDING As String = "LatestArticlesShowPending"
        Public Const LATEST_ARTICLES_SHOW_RELATED As String = "LatestArticlesShowRelated"
        Public Const LATEST_ARTICLES_FEATURED_ONLY As String = "LatestArticlesFeaturedOnly"
        Public Const LATEST_ARTICLES_NOT_FEATURED_ONLY As String = "LatestArticlesNotFeaturedOnly"
        Public Const LATEST_ARTICLES_SECURED_ONLY As String = "LatestArticlesSecuredOnly"
        Public Const LATEST_ARTICLES_NOT_SECURED_ONLY As String = "LatestArticlesNotSecuredOnly"
        Public Const LATEST_ARTICLES_CUSTOM_FIELD_FILTER As String = "LatestArticlesCustomFieldFilter"
        Public Const LATEST_ARTICLES_CUSTOM_FIELD_VALUE As String = "LatestArticlesCustomFieldValue"
        Public Const LATEST_ARTICLES_LINK_FILTER As String = "LatestArticlesLinkFilter"
        Public Const LATEST_ARTICLES_SORT_BY As String = "LatestArticlesSortBy"
        Public Const LATEST_ARTICLES_SORT_DIRECTION As String = "LatestArticlesSortDirection"
        Public Const LATEST_ARTICLES_ITEMS_PER_ROW As String = "ItemsPerRow"
        Public Const LATEST_ARTICLES_ITEMS_PER_ROW_DEFAULT As Integer = 1
        Public Const LATEST_ARTICLES_AUTHOR As String = "LatestArticlesAuthor"
        Public Const LATEST_ARTICLES_AUTHOR_DEFAULT As Integer = -1
        Public Const LATEST_ARTICLES_QUERY_STRING_FILTER As String = "LatestArticlesQueryStringFilter"
        Public Const LATEST_ARTICLES_QUERY_STRING_FILTER_DEFAULT As Boolean = False
        Public Const LATEST_ARTICLES_QUERY_STRING_PARAM As String = "LatestArticlesQueryStringParam"
        Public Const LATEST_ARTICLES_QUERY_STRING_PARAM_DEFAULT As String = "ID"
        Public Const LATEST_ARTICLES_USERNAME_FILTER As String = "LatestArticlesUsernameFilter"
        Public Const LATEST_ARTICLES_USERNAME_FILTER_DEFAULT As Boolean = False
        Public Const LATEST_ARTICLES_USERNAME_PARAM As String = "LatestArticlesUsernameParam"
        Public Const LATEST_ARTICLES_USERNAME_PARAM_DEFAULT As String = "Username"
        Public Const LATEST_ARTICLES_LOGGED_IN_USER_FILTER As String = "LatestArticlesLoggedInUserFilter"
        Public Const LATEST_ARTICLES_LOGGED_IN_USER_FILTER_DEFAULT As Boolean = False
        Public Const LATEST_ARTICLES_INCLUDE_STYLESHEET As String = "LatestArticlesIncludeStylesheet"
        Public Const LATEST_ARTICLES_INCLUDE_STYLESHEET_DEFAULT As Boolean = False

        Public Const LATEST_ENABLE_PAGER As String = "LatestEnablePager"
        Public Const LATEST_ENABLE_PAGER_DEFAULT As Boolean = False
        Public Const LATEST_PAGE_SIZE As String = "LatestPageSize"
        Public Const LATEST_PAGE_SIZE_DEFAULT As Integer = 10

        Public Const LATEST_ARTICLES_LAYOUT_MODE As String = "LayoutMode"
        Public Const LATEST_ARTICLES_LAYOUT_MODE_DEFAULT As LayoutModeType = LayoutModeType.Simple

        Public Const SETTING_HTML_HEADER As String = "HtmlHeader"
        Public Const SETTING_HTML_BODY As String = "HtmlBody"
        Public Const SETTING_HTML_FOOTER As String = "HtmlFooter"

        Public Const SETTING_HTML_HEADER_ADVANCED As String = "HtmlHeaderAdvanced"
        Public Const SETTING_HTML_BODY_ADVANCED As String = "HtmlBodyAdvanced"
        Public Const SETTING_HTML_FOOTER_ADVANCED As String = "HtmlFooterAdvanced"

        Public Const SETTING_HTML_NO_ARTICLES As String = "HtmlNoArticles"

        Public Const DEFAULT_HTML_HEADER As String = "<table cellpadding=0 cellspacing=4>"
        Public Const DEFAULT_HTML_BODY As String = "<TR><TD>[EDIT]<span class=normal><a href=""[LINK]"">[TITLE]</a> by [AUTHORUSERNAME]</span></TD></TR><TR><TD><span class=normal>[SUMMARY]</span></TD></TR>"
        Public Const DEFAULT_HTML_FOOTER As String = "</table>"
        Public Const DEFAULT_HTML_HEADER_ADVANCED As String = ""
        Public Const DEFAULT_HTML_BODY_ADVANCED As String = "[EDIT]<span class=normal><a href=""[LINK]"">[TITLE]</a> by [AUTHORUSERNAME]</span><br><span class=normal>[SUMMARY]</span>"
        Public Const DEFAULT_HTML_FOOTER_ADVANCED As String = ""
        Public Const DEFAULT_HTML_NO_ARTICLES As String = "No articles match criteria."
        Public Const DEFAULT_SORT_BY As String = "StartDate"
        Public Const DEFAULT_SORT_DIRECTION As String = "DESC"

        ' Latest Comments 
        Public Const LATEST_COMMENTS_TAB_ID As String = "LatestCommentsTabID"
        Public Const LATEST_COMMENTS_MODULE_ID As String = "LatestCommentsModuleID"
        Public Const LATEST_COMMENTS_COUNT As String = "LatestCommentsCount"

        Public Const LATEST_COMMENTS_HTML_HEADER As String = "HtmlHeaderLatestComments"
        Public Const LATEST_COMMENTS_HTML_BODY As String = "HtmlBodyLatestComments"
        Public Const LATEST_COMMENTS_HTML_FOOTER As String = "HtmlFooterLatestComments"
        Public Const LATEST_COMMENTS_HTML_NO_COMMENTS As String = "HtmlNoCommentsLatestComments"
        Public Const LATEST_COMMENTS_INCLUDE_STYLESHEET As String = "LatestCommentsIncludeStylesheet"

        Public Const DEFAULT_LATEST_COMMENTS_HTML_HEADER As String = "<table cellpadding=""0"" cellspacing=""4"">"
        Public Const DEFAULT_LATEST_COMMENTS_HTML_BODY As String = "<TR><TD><span class=normal><a href=""[COMMENTLINK]"">[ARTICLETITLE]</a><br /> [COMMENT:50] by [AUTHOR]</span></TD></TR>"
        Public Const DEFAULT_LATEST_COMMENTS_HTML_FOOTER As String = "</table>"
        Public Const DEFAULT_LATEST_COMMENTS_HTML_NO_COMMENTS As String = "No comments match criteria."
        Public Const DEFAULT_LATEST_COMMENTS_INCLUDE_STYLESHEET As Boolean = False

        ' News Archives
        Public Const NEWS_ARCHIVES_TAB_ID As String = "NewsArchivesTabID"
        Public Const NEWS_ARCHIVES_TAB_ID_DEFAULT As Integer = -1
        Public Const NEWS_ARCHIVES_MODULE_ID As String = "NewsArchivesModuleID"
        Public Const NEWS_ARCHIVES_MODULE_ID_DEFAULT As Integer = -1
        Public Const NEWS_ARCHIVES_MODE As String = "NewsArchivesMode"
        Public Const NEWS_ARCHIVES_MODE_DEFAULT As ArchiveModeType = ArchiveModeType.Date
        Public Const NEWS_ARCHIVES_HIDE_ZERO_CATEGORIES As String = "NewsArchivesHideZeroCategories"
        Public Const NEWS_ARCHIVES_PARENT_CATEGORY As String = "NewsArchivesParentCategory"
        Public Const NEWS_ARCHIVES_PARENT_CATEGORY_DEFAULT As Integer = -1
        Public Const NEWS_ARCHIVES_MAX_DEPTH As String = "NewsArchivesMaxDepth"
        Public Const NEWS_ARCHIVES_MAX_DEPTH_DEFAULT As Integer = -1
        Public Const NEWS_ARCHIVES_AUTHOR_SORT_BY As String = "NewsArchivesAuthorSortBy"
        Public Const NEWS_ARCHIVES_AUTHOR_SORT_BY_DEFAULT As AuthorSortByType = AuthorSortByType.DisplayName
        Public Const NEWS_ARCHIVES_HIDE_ZERO_CATEGORIES_DEFAULT As Boolean = False
        Public Const NEWS_ARCHIVES_GROUP_BY As String = "GroupBy"
        Public Const NEWS_ARCHIVES_GROUP_BY_DEFAULT As GroupByType = GroupByType.Month
        Public Const NEWS_ARCHIVES_LAYOUT_MODE As String = "LayoutMode"
        Public Const NEWS_ARCHIVES_LAYOUT_MODE_DEFAULT As LayoutModeType = LayoutModeType.Simple
        Public Const NEWS_ARCHIVES_ITEMS_PER_ROW As String = "ItemsPerRow"
        Public Const NEWS_ARCHIVES_ITEMS_PER_ROW_DEFAULT As Integer = 1

        Public Const NEWS_ARCHIVES_SETTING_HTML_HEADER As String = "NewsArchivesHtmlHeader"
        Public Const NEWS_ARCHIVES_SETTING_HTML_BODY As String = "NewsArchivesHtmlBody"
        Public Const NEWS_ARCHIVES_SETTING_HTML_FOOTER As String = "NewsArchivesHtmlFooter"

        Public Const NEWS_ARCHIVES_SETTING_HTML_HEADER_ADVANCED As String = "NewsArchivesHtmlHeaderAdvanced"
        Public Const NEWS_ARCHIVES_SETTING_HTML_BODY_ADVANCED As String = "NewsArchivesHtmlBodyAdvanced"
        Public Const NEWS_ARCHIVES_SETTING_HTML_FOOTER_ADVANCED As String = "NewsArchivesHtmlFooterAdvanced"

        Public Const NEWS_ARCHIVES_SETTING_CATEGORY_HTML_HEADER As String = "NewsArchivesCategoryHtmlHeader"
        Public Const NEWS_ARCHIVES_SETTING_CATEGORY_HTML_BODY As String = "NewsArchivesCategoryHtmlBody"
        Public Const NEWS_ARCHIVES_SETTING_CATEGORY_HTML_FOOTER As String = "NewsArchivesCategoryHtmlFooter"

        Public Const NEWS_ARCHIVES_SETTING_CATEGORY_HTML_HEADER_ADVANCED As String = "NewsArchivesCategoryHtmlHeaderAdvanced"
        Public Const NEWS_ARCHIVES_SETTING_CATEGORY_HTML_BODY_ADVANCED As String = "NewsArchivesCategoryHtmlBodyAdvanced"
        Public Const NEWS_ARCHIVES_SETTING_CATEGORY_HTML_FOOTER_ADVANCED As String = "NewsArchivesCategoryHtmlFooterAdvanced"

        Public Const NEWS_ARCHIVES_SETTING_AUTHOR_HTML_HEADER As String = "NewsArchivesAuthorHtmlHeader"
        Public Const NEWS_ARCHIVES_SETTING_AUTHOR_HTML_BODY As String = "NewsArchivesAuthorHtmlBody"
        Public Const NEWS_ARCHIVES_SETTING_AUTHOR_HTML_FOOTER As String = "NewsArchivesAuthorHtmlFooter"

        Public Const NEWS_ARCHIVES_SETTING_AUTHOR_HTML_HEADER_ADVANCED As String = "NewsArchivesAuthorHtmlHeaderAdvanced"
        Public Const NEWS_ARCHIVES_SETTING_AUTHOR_HTML_BODY_ADVANCED As String = "NewsArchivesAuthorHtmlBodyAdvanced"
        Public Const NEWS_ARCHIVES_SETTING_AUTHOR_HTML_FOOTER_ADVANCED As String = "NewsArchivesAuthorHtmlFooterAdvanced"

        Public Const NEWS_ARCHIVES_DEFAULT_HTML_HEADER As String = "<table cellpadding=0 cellspacing=4>"
        Public Const NEWS_ARCHIVES_DEFAULT_HTML_BODY As String = "<TR><TD class=normal><a href=""[LINK]"">[MONTH] [YEAR] ([COUNT])</a></TD></TR>"
        Public Const NEWS_ARCHIVES_DEFAULT_HTML_FOOTER As String = "</table>"

        Public Const NEWS_ARCHIVES_DEFAULT_HTML_HEADER_ADVANCED As String = ""
        Public Const NEWS_ARCHIVES_DEFAULT_HTML_BODY_ADVANCED As String = "<span class=normal><a href=""[LINK]"">[MONTH] [YEAR] ([COUNT])</a></span>"
        Public Const NEWS_ARCHIVES_DEFAULT_HTML_FOOTER_ADVANCED As String = ""

        Public Const NEWS_ARCHIVES_DEFAULT_CATEGORY_HTML_HEADER As String = "<table cellpadding=0 cellspacing=4>"
        Public Const NEWS_ARCHIVES_DEFAULT_CATEGORY_HTML_BODY As String = "<TR><TD class=normal><a href=""[LINK]"">[CATEGORY] ([COUNT])</a></TD></TR>"
        Public Const NEWS_ARCHIVES_DEFAULT_CATEGORY_HTML_FOOTER As String = "</table>"

        Public Const NEWS_ARCHIVES_DEFAULT_CATEGORY_HTML_HEADER_ADVANCED As String = ""
        Public Const NEWS_ARCHIVES_DEFAULT_CATEGORY_HTML_BODY_ADVANCED As String = "<span class=normal><a href=""[LINK]"">[CATEGORY] ([COUNT])</a></span>"
        Public Const NEWS_ARCHIVES_DEFAULT_CATEGORY_HTML_FOOTER_ADVANCED As String = ""

        Public Const NEWS_ARCHIVES_DEFAULT_AUTHOR_HTML_HEADER As String = "<table cellpadding=0 cellspacing=4>"
        Public Const NEWS_ARCHIVES_DEFAULT_AUTHOR_HTML_BODY As String = "<TR><TD class=normal><a href=""[LINK]"">[AUTHORDISPLAYNAME] ([COUNT])</a></TD></TR>"
        Public Const NEWS_ARCHIVES_DEFAULT_AUTHOR_HTML_FOOTER As String = "</table>"

        Public Const NEWS_ARCHIVES_DEFAULT_AUTHOR_HTML_HEADER_ADVANCED As String = ""
        Public Const NEWS_ARCHIVES_DEFAULT_AUTHOR_HTML_BODY_ADVANCED As String = "<span class=normal><a href=""[LINK]"">[AUTHORDISPLAYNAME] ([COUNT])</a></span>"
        Public Const NEWS_ARCHIVES_DEFAULT_AUTHOR_HTML_FOOTER_ADVANCED As String = ""

        ' News Search

        Public Const NEWS_SEARCH_TAB_ID As String = "NewsSearchTabID"
        Public Const NEWS_SEARCH_MODULE_ID As String = "NewsSearchModuleID"

        ' Caching Constants
        Public Const CACHE_CATEGORY_ARTICLE As String = "NewsCategory_Cache_"
        Public Const CACHE_IMAGE_ARTICLE As String = "NewsImage_Cache_"
        Public Const CACHE_CATEGORY_ARTICLE_NO_LINK As String = "NewsCategory_Cache_NoLink"
        Public Const CACHE_CATEGORY_ARTICLE_LATEST As String = "NewsCategory_Cache_Latest_"

#End Region

    End Class

End Namespace

