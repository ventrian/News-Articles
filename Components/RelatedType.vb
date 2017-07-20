'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Configuration
Imports System.Data

Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles

    Public Enum RelatedType

        None
        MatchCategoriesAny
        MatchCategoriesAll
        MatchTagsAny
        MatchTagsAll
        MatchCategoriesAnyTagsAny
        MatchCategoriesAllTagsAny
        MatchCategoriesAnyTagsAll

    End Enum

End Namespace
