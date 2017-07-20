Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles.Components.CustomFields

    Public Class CustomValueController

#Region " Private Members "

        Private Const CACHE_KEY As String = "-NewsArticles-CustomValues-All"

#End Region

#Region " Public Methods "

        Public Function GetByCustomField(ByVal articleID As Integer, ByVal customFieldID As Integer) As CustomValueInfo

            Dim objCustomValues As List(Of CustomValueInfo) = List(articleID)

            For Each objCustomValue As CustomValueInfo In objCustomValues
                If (objCustomValue.CustomFieldID = customFieldID) Then
                    Return objCustomValue
                End If
            Next

            Return Nothing

        End Function

        Public Function List(ByVal articleID As Integer) As List(Of CustomValueInfo)

            Dim key As String = articleID.ToString() & CACHE_KEY

            Dim objCustomValues As List(Of CustomValueInfo) = CType(DataCache.GetCache(key), List(Of CustomValueInfo))

            If (objCustomValues Is Nothing) Then
                objCustomValues = CBO.FillCollection(Of CustomValueInfo)(DataProvider.Instance().GetCustomValueList(articleID))
                DataCache.SetCache(key, objCustomValues)
            End If

            Return objCustomValues

        End Function

        Public Function Add(ByVal objCustomValue As CustomValueInfo) As Integer

            DataCache.RemoveCache(objCustomValue.ArticleID.ToString() & CACHE_KEY)
            Return CType(DataProvider.Instance().AddCustomValue(objCustomValue.ArticleID, objCustomValue.CustomFieldID, objCustomValue.CustomValue), Integer)

        End Function

        Public Sub Update(ByVal objCustomValue As CustomValueInfo)

            DataCache.RemoveCache(objCustomValue.ArticleID.ToString() & CACHE_KEY)
            DataProvider.Instance().UpdateCustomValue(objCustomValue.CustomValueID, objCustomValue.ArticleID, objCustomValue.CustomFieldID, objCustomValue.CustomValue)

        End Sub

        Public Sub Delete(ByVal articleID As Integer, ByVal customFieldID As Integer)

            DataCache.RemoveCache(articleID.ToString() & CACHE_KEY)
            DataProvider.Instance().DeleteCustomValue(articleID, customFieldID)

        End Sub

#End Region

    End Class

End Namespace
