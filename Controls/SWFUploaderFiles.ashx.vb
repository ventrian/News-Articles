Imports System.Web
Imports System.Web.Services

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.FileSystem
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

Namespace Ventrian.NewsArticles.Controls

    Public Class SWFUploaderFiles
        Implements System.Web.IHttpHandler

#Region " Private Members "

        Private _articleID As Integer = Null.NullInteger
        Private _moduleID As Integer = Null.NullInteger
        Private _tabID As Integer = Null.NullInteger
        Private _tabModuleID As Integer = Null.NullInteger
        Private _portalID As Integer = Null.NullInteger
        Private _ticket As String = Null.NullString
        Private _userID As Integer = Null.NullInteger
        Private _fileGuid As String = Null.NullString

        Private _articleSettings As Ventrian.NewsArticles.ArticleSettings
        Private _settings As Hashtable
        Private _context As HttpContext

#End Region

#Region " Private Properties "

        Private ReadOnly Property ArticleSettings() As Ventrian.NewsArticles.ArticleSettings
            Get
                If _articleSettings Is Nothing Then
                    Dim objModuleController As New ModuleController
                    Dim objModule As ModuleInfo = objModuleController.GetModule(_moduleID, _tabID)

                    _articleSettings = New Ventrian.NewsArticles.ArticleSettings(Settings, PortalController.GetCurrentPortalSettings(), objModule)
                End If
                Return _articleSettings
            End Get
        End Property

        Private ReadOnly Property Settings() As Hashtable
            Get
                If _settings Is Nothing Then
                    Dim objModuleController As New ModuleController
                    _settings = objModuleController.GetModuleSettings(_moduleID)
                    _settings = GetTabModuleSettings(_tabModuleID, _settings)
                End If
                Return _settings
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub AuthenticateUserFromTicket()

            If (_ticket <> "") Then

                Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(_ticket)
                Dim fi As FormsIdentity = New FormsIdentity(ticket)

                Dim roles As String() = Nothing
                HttpContext.Current.User = New System.Security.Principal.GenericPrincipal(fi, roles)

                Dim objUser As UserInfo = UserController.GetUserByName(_portalID, HttpContext.Current.User.Identity.Name)

                If Not (objUser Is Nothing) Then
                    _userID = objUser.UserID
                    HttpContext.Current.Items("UserInfo") = objUser

                    Dim objUserController As New UserController
                    roles = objUserController.GetUser(_portalID, _userID).Roles

                    Dim strPortalRoles As String = Join(roles, New Char() {";"c})
                    _context.Items.Add("UserRoles", ";" + strPortalRoles + ";")
                End If

            End If

        End Sub

        Private Function GetTabModuleSettings(ByVal TabModuleId As Integer, ByVal settings As Hashtable) As Hashtable

            Dim dr As IDataReader = DotNetNuke.Data.DataProvider.Instance().GetTabModuleSettings(TabModuleId)

            While dr.Read()

                If Not dr.IsDBNull(1) Then
                    settings(dr.GetString(0)) = dr.GetString(1)
                Else
                    settings(dr.GetString(0)) = ""
                End If

            End While

            dr.Close()

            Return settings

        End Function

        Private Sub ReadQueryString()

            If (_context.Request("ModuleID") <> "") Then
                _moduleID = Convert.ToInt32(_context.Request("ModuleID"))
            End If

            If (_context.Request("PortalID") <> "") Then
                _portalID = Convert.ToInt32(_context.Request("PortalID"))
            End If

            If (_context.Request("ArticleID") <> "") Then
                _articleID = Convert.ToInt32(_context.Request("ArticleID"))
            End If

            If (_context.Request("TabModuleID") <> "") Then
                _tabModuleID = Convert.ToInt32(_context.Request("TabModuleID"))
            End If

            If (_context.Request("TabID") <> "") Then
                _tabID = Convert.ToInt32(_context.Request("TabID"))
            End If

            If (_context.Request("Ticket") <> "") Then
                _ticket = _context.Request("Ticket")
            End If

            If (_articleID = Null.NullInteger) Then
                If (_context.Request("ArticleGuid") <> "") Then
                    _fileGuid = _context.Request("ArticleGuid")
                End If
            End If

        End Sub

#End Region

#Region " Interface Methods "

        Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            _context = context
            context.Response.ContentType = "text/plain"

            ReadQueryString()
            AuthenticateUserFromTicket()

            If (_context.Request.IsAuthenticated = False) Then
                _context.Response.Write("-2")
                _context.Response.End()
            End If

            Dim objFileController As New FileController

            Dim objFilePosted As HttpPostedFile = _context.Request.Files("Filedata")

            If Not (objFilePosted Is Nothing) Then

                Dim objPortalController As New PortalController()
                If (objPortalController.HasSpaceAvailable(_portalID, objFilePosted.ContentLength) = False) Then
                    _context.Response.Write("-1")
                    _context.Response.End()
                End If

                Dim username As String = _context.User.Identity.Name

                If (_articleID <> Null.NullInteger) Then
                    FileProvider.Instance().AddFile(_articleID, _moduleID, objFilePosted)
                Else
                    FileProvider.Instance().AddFile(_fileGuid, _moduleID, objFilePosted)
                End If

                'Dim objFile As New FileInfo

                'objFile.ArticleID = _articleID
                'If (_articleID = Null.NullInteger) Then
                '    objFile.FileGuid = _fileGuid
                'End If
                'objFile.FileName = objFilePosted.FileName
                'objFile.SortOrder = 0

                'Dim filesList As List(Of FileInfo) = objFileController.GetFileList(_articleID, _fileGuid)

                'If (filesList.Count > 0) Then
                '    objFile.SortOrder = CType(filesList(filesList.Count - 1), FileInfo).SortOrder + 1
                'End If

                'Dim objPortalSettings As PortalSettings = PortalController.GetCurrentPortalSettings()

                'Dim folder As String = ""
                'If (ArticleSettings.DefaultFilesFolder <> Null.NullInteger) Then
                '    Dim objFolderController As New FolderController
                '    Dim objFolder As FolderInfo = objFolderController.GetFolderInfo(_portalID, ArticleSettings.DefaultFilesFolder)
                '    If (objFolder IsNot Nothing) Then
                '        folder = objFolder.FolderPath
                '    End If
                'End If

                'objFile.Folder = folder
                'objFile.ContentType = objFilePosted.ContentType

                'If (objFile.FileName.Split("."c).Length > 0) Then
                '    objFile.Extension = objFile.FileName.Split("."c)(objFile.FileName.Split("."c).Length - 1)

                '    If (objFile.Extension.ToLower() = "jpg") Then
                '        objFile.ContentType = "image/jpeg"
                '    End If
                '    If (objFile.Extension.ToLower() = "gif") Then
                '        objFile.ContentType = "image/gif"
                '    End If
                '    If (objFile.Extension.ToLower() = "txt") Then
                '        objFile.ContentType = "text/plain"
                '    End If
                '    If (objFile.Extension.ToLower() = "html") Then
                '        objFile.ContentType = "text/html"
                '    End If
                '    If (objFile.Extension.ToLower() = "mp3") Then
                '        objFile.ContentType = "audio/mpeg"
                '    End If

                'End If
                'objFile.Title = objFile.FileName.Replace("." & objFile.Extension, "")

                'Dim filePath As String = objPortalSettings.HomeDirectoryMapPath & folder.Replace("/", "\")

                'If Not (Directory.Exists(filePath)) Then
                '    Directory.CreateDirectory(filePath)
                'End If

                'If (File.Exists(filePath & objFile.FileName)) Then
                '    For i As Integer = 1 To 100
                '        If (File.Exists(filePath & i.ToString() & "_" & objFile.FileName) = False) Then
                '            objFile.FileName = i.ToString() & "_" & objFile.FileName
                '            Exit For
                '        End If
                '    Next
                'End If

                'objFile.Size = objFilePosted.ContentLength
                'objFilePosted.SaveAs(filePath & objFile.FileName)

                'objFile.FileID = objFileController.Add(objFile)

                'If (_articleID <> Null.NullInteger) Then
                '    Dim objArticleController As New ArticleController
                '    Dim objArticle As ArticleInfo = objArticleController.GetArticle(_articleID)
                '    If (objArticle IsNot Nothing) Then
                '        objArticle.FileCount = objArticle.FileCount + 1
                '        objArticleController.UpdateArticle(objArticle)
                '    End If
                'End If

            End If

            _context.Response.Write("0")
            _context.Response.End()

        End Sub

        ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

#End Region

    End Class

End Namespace