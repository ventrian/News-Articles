Imports DotNetNuke
Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles

    Public MustInherit Class FileProvider

#Region " Shared/Static Methods "

        ' singleton reference to the instantiated object 
        Private Shared objProvider As FileProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            If (ConfigurationManager.AppSettings("NewsArticlesFileProvider") IsNot Nothing) Then
                objProvider = CType(Framework.Reflection.CreateObject(ConfigurationManager.AppSettings("NewsArticlesFileProvider").ToString(), "Ventrian_NaFileProvider"), FileProvider)
            Else
                objProvider = CType(Framework.Reflection.CreateObject("Ventrian.NewsArticles.CoreFileProvider", "Ventrian_NaFileProvider"), FileProvider)
            End If
         End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As FileProvider
            Return objProvider
        End Function

#End Region

#Region " Abstract methods "

		Public MustOverride Function AddFile(ByVal articleID As Integer, ByVal moduleID As Integer, folderID As Integer, ByVal objPostedFile As System.Web.HttpPostedFile) As Integer
		Public MustOverride Function AddFile(ByVal articleID As Integer, ByVal moduleID As Integer, folderID As Integer, ByVal objPostedFile As System.Web.HttpPostedFile, ByVal providerParams As Object) As Integer
		Public MustOverride Function AddExistingFile(ByVal articleID As Integer, ByVal moduleID As Integer, ByVal providerParams As Object) As Integer
        Public MustOverride Sub DeleteFile(ByVal articleID As Integer, ByVal fileID As Integer)
        Public MustOverride Function GetFile(ByVal fileID As Integer) As FileInfo
        Public MustOverride Function GetFiles(ByVal articleID As Integer) As List(Of FileInfo)
        Public MustOverride Sub UpdateFile(ByVal objFile As FileInfo)
        

#End Region

    End Class

End Namespace