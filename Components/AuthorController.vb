'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports DotNetNuke.Common.Utilities
Imports System.Security.Cryptography

Namespace Ventrian.NewsArticles

    Public Class AuthorController

        Public Shared Sub ClearCache(ByVal moduleID As Integer)

            Dim itemsToRemove As New List(Of String)()

            If (HttpContext.Current IsNot Nothing) Then
                Dim enumerator As IDictionaryEnumerator = HttpContext.Current.Cache.GetEnumerator()
                While enumerator.MoveNext()
                    If enumerator.Key.ToString().ToLower().Contains("ventrian-newsarticles-authors-" & moduleID.ToString()) Then
                        itemsToRemove.Add(enumerator.Key.ToString())
                    End If
                End While

                For Each itemToRemove As String In itemsToRemove
                    DataCache.RemoveCache(itemToRemove.Replace("DNN_", ""))
                Next
            End If

        End Sub

        Public Function GetAuthorList(ByVal moduleID As Integer) As List(Of AuthorInfo)

            Dim cacheKey As String = "ventrian-newsarticles-authors-" & moduleID.ToString()

            Dim objAuthors As List(Of AuthorInfo) = CType(DataCache.GetCache(cacheKey), List(Of AuthorInfo))

            If (objAuthors Is Nothing) Then
                objAuthors = CBO.FillCollection(Of AuthorInfo)(DataProvider.Instance().GetAuthorList(moduleID))
                DataCache.SetCache(cacheKey, objAuthors)
            End If

            Return objAuthors

        End Function

        Public Function GetAuthorStatistics(ByVal moduleID As Integer, ByVal categoryID As Integer(), ByVal categoryIDExclude As Integer(), ByVal authorID As Integer, ByVal sortBy As String, ByVal showPending As Boolean) As List(Of AuthorInfo)

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

            If (sortBy = "ArticleCount") Then
                sortBy = "ArticleCount DESC"
            End If

            Dim cacheKey As String = "ventrian-newsarticles-authors-" & moduleID.ToString() & "-" & hashCategories & "-" & authorID.ToString() & "-" & sortBy.ToString() & "-" & showPending.ToString()

            Dim objAuthorStatistics As List(Of AuthorInfo) = CType(DataCache.GetCache(cacheKey), List(Of AuthorInfo))

            If (objAuthorStatistics Is Nothing) Then
                objAuthorStatistics = CBO.FillCollection(Of AuthorInfo)(DataProvider.Instance().GetAuthorStatistics(moduleID, categoryID, categoryIDExclude, authorID, sortBy, showPending))
                DataCache.SetCache(cacheKey, objAuthorStatistics)
            End If

            Return objAuthorStatistics

        End Function


    End Class

End Namespace
