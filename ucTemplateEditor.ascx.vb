'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.IO

Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Security

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucTemplateEditor
        Inherits NewsArticleModuleBase

#Region " Private Methods "

        Private Sub BindTemplates(ByVal selectedValue As String)

            drpTemplate.Items.Clear()

            Dim templateRoot As String = Me.MapPath("Templates")
            If Directory.Exists(templateRoot) Then
                Dim arrFolders() As String = Directory.GetDirectories(templateRoot)
                For Each folder As String In arrFolders
                    Dim folderName As String = folder.Substring(folder.LastIndexOf("\") + 1)
                    Dim objListItem As ListItem = New ListItem
                    objListItem.Text = folderName
                    objListItem.Value = folderName
                    drpTemplate.Items.Add(objListItem)
                Next
            End If

            If Not (drpTemplate.Items.FindByValue(ArticleSettings.Template) Is Nothing) Then
                drpTemplate.SelectedValue = ArticleSettings.Template
            End If

            If (selectedValue <> "") Then
                If Not (drpTemplate.Items.FindByValue(selectedValue) Is Nothing) Then
                    drpTemplate.SelectedValue = selectedValue
                End If
            End If

        End Sub

        Private Sub BindFile()

            Dim pathToTemplate As String = Me.MapPath("Templates/" & drpTemplate.SelectedItem.Text & "/")
            Dim path As String = pathToTemplate & drpFile.SelectedItem.Text

            If (File.Exists(path) = False) Then
                pathToTemplate = Me.MapPath("Templates/Standard/")
                path = pathToTemplate & drpFile.SelectedItem.Text
            End If

            If (File.Exists(path)) Then
                Dim sr As StreamReader = New StreamReader(path)
                Try
                    txtTemplate.Text = sr.ReadToEnd()
                Catch ex As Exception

                Finally
                    If Not sr Is Nothing Then sr.Close()
                End Try
            Else
                txtTemplate.Text = ""
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If (Me.UserInfo.IsSuperUser = False) Then
                    If (Settings.Contains(ArticleConstants.PERMISSION_SITE_TEMPLATES_SETTING)) Then
                        If (Settings(ArticleConstants.PERMISSION_SITE_TEMPLATES_SETTING).ToString() <> "") Then
                            If (PortalSecurity.IsInRoles(Settings(ArticleConstants.PERMISSION_SITE_TEMPLATES_SETTING).ToString()) = False) Then
                                Response.Redirect(EditArticleUrl("AdminOptions"), True)
                            End If
                        Else
                            Response.Redirect(EditArticleUrl("AdminOptions"), True)
                        End If
                    Else
                        Response.Redirect(EditArticleUrl("AdminOptions"), True)
                    End If
                End If

                If (Page.IsPostBack = False) Then
                    BindTemplates("")
                    BindFile()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpTemplate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpTemplate.SelectedIndexChanged

            Try

                BindFile()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpFile_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpFile.SelectedIndexChanged

            Try

                BindFile()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

            Try

                Dim pathToTemplate As String = Me.MapPath("Templates/" & drpTemplate.SelectedItem.Text & "/")
                Dim path As String = pathToTemplate & drpFile.SelectedItem.Text

                Dim sw As New StreamWriter(path)
                Try
                    sw.Write(txtTemplate.Text)
                Catch
                Finally
                    If Not sw Is Nothing Then sw.Close()
                End Try

                lblUpdated.Visible = True

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click

            Try

                If (txtNewTemplate.Text <> "") Then
                    Dim pathToTemplate As String = Me.MapPath("Templates/" & txtNewTemplate.Text & "/")
                    System.IO.Directory.CreateDirectory(pathToTemplate)

                    If (Directory.Exists(Me.MapPath("Templates/Standard/"))) Then

                        Dim copyDirectory As DirectoryInfo = New DirectoryInfo(Me.MapPath("Templates/Standard/"))
                        Dim DestDir As DirectoryInfo = New DirectoryInfo(pathToTemplate)

                        Dim ChildFile As System.IO.FileInfo

                        For Each ChildFile In copyDirectory.GetFiles()
                            ChildFile.CopyTo(Path.Combine(DestDir.FullName, ChildFile.Name), True)
                        Next

                    End If

                    pathToTemplate = pathToTemplate & "Images/"
                    System.IO.Directory.CreateDirectory(pathToTemplate)

                    If (Directory.Exists(Me.MapPath("Templates/Standard/Images/"))) Then

                        Dim imagesDirectory As DirectoryInfo = New DirectoryInfo(Me.MapPath("Templates/Standard/Images/"))
                        Dim DestDir As DirectoryInfo = New DirectoryInfo(pathToTemplate)

                        Dim ChildFile As System.IO.FileInfo

                        For Each ChildFile In imagesDirectory.GetFiles()
                            ChildFile.CopyTo(Path.Combine(DestDir.FullName, ChildFile.Name), True)
                        Next

                    End If


                    lblTemplateCreated.Visible = True
                    BindTemplates(txtNewTemplate.Text)
                    BindFile()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(EditArticleUrl("AdminOptions"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace