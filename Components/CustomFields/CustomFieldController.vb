Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles.Components.CustomFields

    Public Class CustomFieldController

#Region " Private Members "

        Private Const CACHE_KEY As String = "-NewsArticles-CustomFields-All"

#End Region

#Region " Public Methods "

        Public Function [Get](ByVal customFieldID As Integer) As CustomFieldInfo

            Return CBO.FillObject(Of CustomFieldInfo)(DataProvider.Instance().GetCustomField(customFieldID))

        End Function

        Public Function List(ByVal moduleID As Integer) As ArrayList

            Dim key As String = moduleID.ToString() & CACHE_KEY

            Dim objCustomFields As ArrayList = CType(DataCache.GetCache(key), ArrayList)

            If (objCustomFields Is Nothing) Then
                objCustomFields = CBO.FillCollection(DataProvider.Instance().GetCustomFieldList(moduleID), GetType(CustomFieldInfo))
                DataCache.SetCache(key, objCustomFields)
            End If

            Return objCustomFields

        End Function

        Public Sub Delete(ByVal moduleID As Integer, ByVal customFieldID As Integer)

            DataCache.RemoveCache(moduleID.ToString() & CACHE_KEY)
            DataProvider.Instance().DeleteCustomField(customFieldID)

        End Sub

        Public Function Add(ByVal objCustomField As CustomFieldInfo) As Integer

            DataCache.RemoveCache(objCustomField.ModuleID.ToString() & CACHE_KEY)
            Return CType(DataProvider.Instance().AddCustomField(objCustomField.ModuleID, objCustomField.Name, objCustomField.FieldType, objCustomField.FieldElements, objCustomField.DefaultValue, objCustomField.Caption, objCustomField.CaptionHelp, objCustomField.IsRequired, objCustomField.IsVisible, objCustomField.SortOrder, objCustomField.ValidationType, objCustomField.Length, objCustomField.RegularExpression), Integer)

        End Function

        Public Sub Update(ByVal objCustomField As CustomFieldInfo)

            DataCache.RemoveCache(objCustomField.ModuleID.ToString() & CACHE_KEY)
            DataProvider.Instance().UpdateCustomField(objCustomField.CustomFieldID, objCustomField.ModuleID, objCustomField.Name, objCustomField.FieldType, objCustomField.FieldElements, objCustomField.DefaultValue, objCustomField.Caption, objCustomField.CaptionHelp, objCustomField.IsRequired, objCustomField.IsVisible, objCustomField.SortOrder, objCustomField.ValidationType, objCustomField.Length, objCustomField.RegularExpression)

        End Sub

#End Region

    End Class

End Namespace
