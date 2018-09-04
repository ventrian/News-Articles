'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Data

Imports DotNetNuke
Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles

    Public Class CategoryController

#Region " Public Methods "

        Public Shared Sub ClearCache(ByVal moduleID As Integer)

            Dim itemsToRemove As New List(Of String)()

            If (HttpContext.Current IsNot Nothing) Then
                Dim enumerator As IDictionaryEnumerator = HttpContext.Current.Cache.GetEnumerator()
                While enumerator.MoveNext()
                    If enumerator.Key.ToString().ToLower().Contains("ventrian-newsarticles-categories-" & moduleID.ToString()) Then
                        itemsToRemove.Add(enumerator.Key.ToString())
                    End If
                End While

                For Each itemToRemove As String In itemsToRemove
                    DataCache.RemoveCache(itemToRemove.Replace("DNN_", ""))
                Next
            End If

        End Sub

        Public Function GetCategories(ByVal moduleID As Integer, ByVal parentID As Integer) As List(Of CategoryInfo)

            Return GetCategoriesAll(moduleID, parentID, Nothing, Null.NullInteger, 1, Null.NullBoolean, CategorySortType.Name)

        End Function

        Public Function GetCategoriesAll(ByVal moduleID As Integer) As List(Of CategoryInfo)

            Return GetCategoriesAll(moduleID, Null.NullInteger, Nothing, Null.NullInteger, Null.NullInteger, Null.NullBoolean, CategorySortType.Name)

        End Function

        Public Function GetCategoriesAll(ByVal moduleID As Integer, ByVal parentID As Integer) As List(Of CategoryInfo)

            Return GetCategoriesAll(moduleID, parentID, CategorySortType.Name)

        End Function

        Public Function GetCategoriesAll(ByVal moduleID As Integer, ByVal parentID As Integer, ByVal sortType As CategorySortType) As List(Of CategoryInfo)

            Return GetCategoriesAll(moduleID, parentID, Nothing, Null.NullInteger, Null.NullInteger, False, sortType)

        End Function

        Public Function GetCategoriesAll(ByVal moduleID As Integer, ByVal parentID As Integer, ByVal categoryIDFilter As Integer(), ByVal authorID As Integer, ByVal maxDepth As Integer, ByVal showPending As Boolean, ByVal sortType As CategorySortType) As List(Of CategoryInfo)

            Dim cacheKey As String = "Ventrian-NewsArticles-Categories-" & moduleID.ToString() & "-" & sortType.ToString()

            If (authorID <> Null.NullInteger) Then
                cacheKey = cacheKey & "-a-" & authorID.ToString()
            End If

            If (showPending <> Null.NullBoolean) Then
                cacheKey = cacheKey & "-sp-" & showPending.ToString()
            End If

            Dim objCategories As List(Of CategoryInfo) = CType(DataCache.GetCache(cacheKey), List(Of CategoryInfo))

            If (objCategories Is Nothing) Then
                objCategories = CBO.FillCollection(Of CategoryInfo)(DataProvider.Instance().GetCategoryListAll(moduleID, authorID, showPending, sortType))
                DataCache.SetCache(cacheKey, objCategories)
            End If

            If (categoryIDFilter IsNot Nothing) Then
                Dim objNewCategories As New List(Of CategoryInfo)
                For Each id As Integer In categoryIDFilter
                    For Each objCategory As CategoryInfo In objCategories
                        If (objCategory.CategoryID = id) Then
                            objNewCategories.Add(objCategory)
                        End If
                    Next
                Next
                objCategories = objNewCategories
            End If

            Dim objCategoriesCopy As New List(Of CategoryInfo)(objCategories)

            If (parentID <> Null.NullInteger) Then
                Dim level As Integer = Null.NullInteger
                Dim objParentIDs As New List(Of Integer)
                objParentIDs.Add(parentID)
                Dim objNewCategories As New List(Of CategoryInfo)
                For Each objCategory As CategoryInfo In objCategoriesCopy
                    For Each id As Integer In objParentIDs.ToArray()
                        If (objCategory.ParentID = id) Then
                            If (level = Null.NullInteger) Then
                                level = objCategory.Level
                            End If

                            If (maxDepth = Null.NullInteger Or objCategory.Level < (level + maxDepth)) Then
                                Dim objCategoryNew As CategoryInfo = objCategory.Clone()
                                objCategoryNew.Level = objCategory.Level - level + 1
                                objNewCategories.Add(objCategoryNew)
                                If (objParentIDs.Contains(objCategory.CategoryID) = False) Then
                                    objParentIDs.Add(objCategory.CategoryID)
                                End If
                                Exit For
                            End If
                        End If
                    Next
                Next
                objCategoriesCopy = objNewCategories
            Else
                If (maxDepth <> Null.NullInteger) Then
                    Dim objNewCategories As New List(Of CategoryInfo)
                    For Each objCategory As CategoryInfo In objCategoriesCopy
                        If (objCategory.Level <= maxDepth) Then
                            objNewCategories.Add(objCategory)
                        End If
                    Next
                    objCategoriesCopy = objNewCategories
                End If
            End If

            Return objCategoriesCopy

        End Function



        <Obsolete("This method is deprecated, use GetCategories instead.")> _
        Public Function GetCategoryList(ByVal moduleID As Integer, ByVal parentID As Integer) As ArrayList

            Dim objCategories As List(Of CategoryInfo) = GetCategories(moduleID, parentID)
            Dim objCategoriesToReturn As New ArrayList()

            For Each objCategory As CategoryInfo In objCategories
                objCategoriesToReturn.Add(objCategory)
            Next

            Return objCategoriesToReturn

        End Function

        <Obsolete("This method is deprecated, use GetCategoriesAll instead.")> _
        Public Function GetCategoryListAll(ByVal moduleID As Integer) As ArrayList

            Return GetCategoryListAll(moduleID, Null.NullInteger, Nothing, Null.NullInteger, Null.NullInteger, Null.NullBoolean, CategorySortType.Name)

        End Function

        <Obsolete("This method is deprecated, use GetCategoriesAll instead.")> _
        Public Function GetCategoryListAll(ByVal moduleID As Integer, ByVal parentID As Integer) As ArrayList

            Return GetCategoryListAll(moduleID, parentID, CategorySortType.Name)

        End Function

        <Obsolete("This method is deprecated, use GetCategoriesAll instead.")> _
        Public Function GetCategoryListAll(ByVal moduleID As Integer, ByVal parentID As Integer, ByVal sortType As CategorySortType) As ArrayList

            Return GetCategoryListAll(moduleID, parentID, Nothing, Null.NullInteger, Null.NullInteger, False, sortType)

        End Function

        <Obsolete("This method is deprecated, use GetCategoriesAll instead.")> _
        Public Function GetCategoryListAll(ByVal moduleID As Integer, ByVal parentID As Integer, ByVal categoryIDFilter As Integer(), ByVal authorID As Integer, ByVal maxDepth As Integer, ByVal showPending As Boolean, ByVal sortType As CategorySortType) As ArrayList

            Dim objCategories As List(Of CategoryInfo) = GetCategoriesAll(moduleID, parentID, categoryIDFilter, authorID, maxDepth, showPending, sortType)
            Dim objCategoriesToReturn As New ArrayList()

            For Each objCategory As CategoryInfo In objCategories
                objCategoriesToReturn.Add(objCategory)
            Next

            Return objCategoriesToReturn

        End Function

        Public Function GetCategory(ByVal categoryID As Integer, ByVal moduleID As Integer) As CategoryInfo

            Dim objCategories As List(Of CategoryInfo) = GetCategoriesAll(moduleID)

            For Each objCategory As CategoryInfo In objCategories
                If (objCategory.CategoryID = categoryID) Then
                    Return objCategory
                End If
            Next

            Return CBO.FillObject(Of CategoryInfo)(DataProvider.Instance().GetCategory(categoryID))

        End Function

        Public Sub DeleteCategory(ByVal categoryID As Integer, ByVal moduleID As Integer)

            DataProvider.Instance().DeleteCategory(categoryID)
            CategoryController.ClearCache(moduleID)

        End Sub

        Public Function AddCategory(ByVal objCategory As CategoryInfo) As Integer

            Dim categoryID As Integer = CType(DataProvider.Instance().AddCategory(objCategory.ModuleID, objCategory.ParentID, objCategory.Name, objCategory.Image, objCategory.Description, objCategory.SortOrder, objCategory.InheritSecurity, objCategory.CategorySecurityType, objCategory.MetaTitle, objCategory.MetaDescription, objCategory.MetaKeywords), Integer)
            CategoryController.ClearCache(objCategory.ModuleID)
            Return categoryID

        End Function

        Public Sub UpdateCategory(ByVal objCategory As CategoryInfo)

            DataProvider.Instance().UpdateCategory(objCategory.CategoryID, objCategory.ModuleID, objCategory.ParentID, objCategory.Name, objCategory.Image, objCategory.Description, objCategory.SortOrder, objCategory.InheritSecurity, objCategory.CategorySecurityType, objCategory.MetaTitle, objCategory.MetaDescription, objCategory.MetaKeywords)
            CategoryController.ClearCache(objCategory.ModuleID)

        End Sub

#End Region

    End Class

End Namespace
