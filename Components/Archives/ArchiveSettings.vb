'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2005-2012
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Namespace Ventrian.NewsArticles

    Friend Class ArchiveSettings

#Region " Private Properties "

        Private Property Settings As Hashtable

#End Region

#Region " Constructors "

        Public Sub New(ByVal moduleSettings As Hashtable)
            Settings = moduleSettings
        End Sub

#End Region

#Region " Public Properties "

        Public ReadOnly Property AuthorSortBy As AuthorSortByType
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_AUTHOR_SORT_BY)) Then
                    Return CType(System.Enum.Parse(GetType(AuthorSortByType), Settings(ArticleConstants.NEWS_ARCHIVES_AUTHOR_SORT_BY).ToString()), AuthorSortByType)
                End If
                Return ArticleConstants.NEWS_ARCHIVES_AUTHOR_SORT_BY_DEFAULT
            End Get
        End Property

        Public ReadOnly Property CategoryHideZeroCategories As Boolean
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_HIDE_ZERO_CATEGORIES)) Then
                    Return Convert.ToBoolean(Settings(ArticleConstants.NEWS_ARCHIVES_HIDE_ZERO_CATEGORIES).ToString())
                End If
                Return ArticleConstants.NEWS_ARCHIVES_HIDE_ZERO_CATEGORIES_DEFAULT
            End Get
        End Property

        Public ReadOnly Property CategoryMaxDepth As Integer
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_MAX_DEPTH)) Then
                    If (IsNumeric(Settings(ArticleConstants.NEWS_ARCHIVES_MAX_DEPTH).ToString())) Then
                        Return Convert.ToInt32(Settings(ArticleConstants.NEWS_ARCHIVES_MAX_DEPTH).ToString())
                    End If
                End If
                Return ArticleConstants.NEWS_ARCHIVES_MAX_DEPTH_DEFAULT
            End Get
        End Property

        Public ReadOnly Property CategoryParent As Integer
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_PARENT_CATEGORY)) Then
                    If (IsNumeric(Settings(ArticleConstants.NEWS_ARCHIVES_PARENT_CATEGORY).ToString())) Then
                        Return Convert.ToInt32(Settings(ArticleConstants.NEWS_ARCHIVES_PARENT_CATEGORY).ToString())
                    End If
                End If
                Return ArticleConstants.NEWS_ARCHIVES_PARENT_CATEGORY_DEFAULT
            End Get
        End Property

        Public ReadOnly Property GroupBy As GroupByType
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_GROUP_BY)) Then
                    Return CType(System.Enum.Parse(GetType(GroupByType), Settings(ArticleConstants.NEWS_ARCHIVES_GROUP_BY).ToString()), GroupByType)
                End If
                Return ArticleConstants.NEWS_ARCHIVES_GROUP_BY_DEFAULT
            End Get
        End Property

        Public ReadOnly Property ItemsPerRow As Integer
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_ITEMS_PER_ROW)) Then
                    If (IsNumeric(Settings(ArticleConstants.NEWS_ARCHIVES_ITEMS_PER_ROW).ToString())) Then
                        Return Convert.ToInt32(Settings(ArticleConstants.NEWS_ARCHIVES_ITEMS_PER_ROW).ToString())
                    End If
                End If
                Return ArticleConstants.NEWS_ARCHIVES_ITEMS_PER_ROW_DEFAULT
            End Get
        End Property

        Public ReadOnly Property LayoutMode As LayoutModeType
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_LAYOUT_MODE)) Then
                    Return CType(System.Enum.Parse(GetType(LayoutModeType), Settings(ArticleConstants.NEWS_ARCHIVES_LAYOUT_MODE).ToString()), LayoutModeType)
                End If
                Return ArticleConstants.NEWS_ARCHIVES_LAYOUT_MODE_DEFAULT
            End Get
        End Property

        Public ReadOnly Property Mode As ArchiveModeType
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_MODE)) Then
                    Return CType(System.Enum.Parse(GetType(ArchiveModeType), Settings(ArticleConstants.NEWS_ARCHIVES_MODE).ToString()), ArchiveModeType)
                End If
                Return ArticleConstants.NEWS_ARCHIVES_MODE_DEFAULT
            End Get
        End Property

        Public ReadOnly Property ModuleId As Integer
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_MODULE_ID)) Then
                    If (IsNumeric(Settings(ArticleConstants.NEWS_ARCHIVES_MODULE_ID).ToString())) Then
                        Return Convert.ToInt32(Settings(ArticleConstants.NEWS_ARCHIVES_MODULE_ID).ToString())
                    End If
                End If
                Return ArticleConstants.NEWS_ARCHIVES_MODULE_ID_DEFAULT
            End Get
        End Property

        Public ReadOnly Property TabId As Integer
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_TAB_ID)) Then
                    If (IsNumeric(Settings(ArticleConstants.NEWS_ARCHIVES_TAB_ID).ToString())) Then
                        Return Convert.ToInt32(Settings(ArticleConstants.NEWS_ARCHIVES_TAB_ID).ToString())
                    End If
                End If
                Return ArticleConstants.NEWS_ARCHIVES_TAB_ID_DEFAULT
            End Get
        End Property

        Public ReadOnly Property TemplateAuthorAdvancedBody
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_BODY_ADVANCED)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_BODY_ADVANCED).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_AUTHOR_HTML_BODY_ADVANCED
            End Get
        End Property

        Public ReadOnly Property TemplateAuthorAdvancedFooter
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_FOOTER_ADVANCED)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_FOOTER_ADVANCED).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_AUTHOR_HTML_FOOTER_ADVANCED
            End Get
        End Property

        Public ReadOnly Property TemplateAuthorAdvancedHeader
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_HEADER_ADVANCED)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_HEADER_ADVANCED).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_AUTHOR_HTML_HEADER_ADVANCED
            End Get
        End Property

        Public ReadOnly Property TemplateAuthorBody
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_BODY)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_BODY).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_AUTHOR_HTML_BODY
            End Get
        End Property

        Public ReadOnly Property TemplateAuthorFooter
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_FOOTER)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_FOOTER).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_AUTHOR_HTML_FOOTER
            End Get
        End Property

        Public ReadOnly Property TemplateAuthorHeader
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_HEADER)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_AUTHOR_HTML_HEADER).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_AUTHOR_HTML_HEADER
            End Get
        End Property

        Public ReadOnly Property TemplateCategoryAdvancedBody
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_BODY_ADVANCED)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_BODY_ADVANCED).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_CATEGORY_HTML_BODY_ADVANCED
            End Get
        End Property

        Public ReadOnly Property TemplateCategoryAdvancedFooter
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_FOOTER_ADVANCED)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_FOOTER_ADVANCED).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_CATEGORY_HTML_FOOTER_ADVANCED
            End Get
        End Property

        Public ReadOnly Property TemplateCategoryAdvancedHeader
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_HEADER_ADVANCED)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_HEADER_ADVANCED).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_CATEGORY_HTML_HEADER_ADVANCED
            End Get
        End Property

        Public ReadOnly Property TemplateCategoryBody
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_BODY)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_BODY).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_CATEGORY_HTML_BODY
            End Get
        End Property

        Public ReadOnly Property TemplateCategoryFooter
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_FOOTER)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_FOOTER).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_CATEGORY_HTML_FOOTER
            End Get
        End Property

        Public ReadOnly Property TemplateCategoryHeader
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_HEADER)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_CATEGORY_HTML_HEADER).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_CATEGORY_HTML_HEADER
            End Get
        End Property

        Public ReadOnly Property TemplateDateAdvancedBody
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_BODY_ADVANCED)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_BODY_ADVANCED).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_HTML_BODY_ADVANCED
            End Get
        End Property

        Public ReadOnly Property TemplateDateAdvancedFooter
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_FOOTER_ADVANCED)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_FOOTER_ADVANCED).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_HTML_FOOTER_ADVANCED
            End Get
        End Property

        Public ReadOnly Property TemplateDateAdvancedHeader
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_HEADER_ADVANCED)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_HEADER_ADVANCED).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_HTML_HEADER_ADVANCED
            End Get
        End Property

        Public ReadOnly Property TemplateDateBody
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_BODY)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_BODY).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_HTML_BODY
            End Get
        End Property

        Public ReadOnly Property TemplateDateFooter
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_FOOTER)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_FOOTER).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_HTML_FOOTER
            End Get
        End Property

        Public ReadOnly Property TemplateDateHeader
            Get
                If (Settings.Contains(ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_HEADER)) Then
                    Return Settings(ArticleConstants.NEWS_ARCHIVES_SETTING_HTML_HEADER).ToString()
                End If
                Return ArticleConstants.NEWS_ARCHIVES_DEFAULT_HTML_HEADER
            End Get
        End Property

#End Region

    End Class

End Namespace
