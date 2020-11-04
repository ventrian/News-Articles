'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Data

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports Dotnetnuke.Entities.Portals
Imports Dotnetnuke.Entities.Users
Imports DotNetNuke.Framework
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions

Namespace Ventrian.NewsArticles

    Public Class EmailTemplateController

#Region " Private Methods "

        Public Function FormatArticleEmail(ByVal template As String, ByVal link As String, ByVal objArticle As ArticleInfo, ByVal articleSettings As ArticleSettings) As String

            Dim formatted As String = template

            formatted = formatted.Replace("[USERNAME]", objArticle.AuthorUserName)
            formatted = formatted.Replace("[FIRSTNAME]", objArticle.AuthorFirstName)
            formatted = formatted.Replace("[LASTNAME]", objArticle.AuthorLastName)
            formatted = formatted.Replace("[FULLNAME]", objArticle.AuthorFullName)
            formatted = formatted.Replace("[EMAIL]", objArticle.AuthorEmail)
            formatted = formatted.Replace("[DISPLAYNAME]", objArticle.AuthorDisplayName)

            Dim settings As PortalSettings = PortalController.Instance.GetCurrentPortalSettings()

            formatted = formatted.Replace("[PORTALNAME]", settings.PortalName)
            formatted = formatted.Replace("[CREATEDATE]", objArticle.CreatedDate.ToString("d") & " " & objArticle.CreatedDate.ToString("t"))
            formatted = formatted.Replace("[POSTDATE]", objArticle.StartDate.ToString("d") & " " & objArticle.CreatedDate.ToString("t"))

            formatted = formatted.Replace("[TITLE]", objArticle.Title)
            formatted = formatted.Replace("[SUMMARY]", System.Web.HttpContext.Current.Server.HtmlDecode(objArticle.Summary))
            formatted = formatted.Replace("[LINK]", link)

            Return formatted

        End Function

        Public Function FormatCommentEmail(ByVal template As String, ByVal link As String, ByVal objArticle As ArticleInfo, ByVal objComment As CommentInfo, ByVal articleSettings As ArticleSettings) As String

            Dim formatted As String = template

            If (objComment.UserID = Null.NullInteger) Then
                ' Anonymous Comment
                formatted = formatted.Replace("[USERNAME]", objComment.AnonymousName)
                formatted = formatted.Replace("[EMAIL]", objComment.AnonymousEmail)
                formatted = formatted.Replace("[FIRSTNAME]", objComment.AnonymousName)
                formatted = formatted.Replace("[LASTNAME]", objComment.AnonymousName)
                formatted = formatted.Replace("[FULLNAME]", objComment.AnonymousName)
                formatted = formatted.Replace("[DISPLAYNAME]", objComment.AnonymousName)
            Else
                ' Authenticated Comment
                formatted = formatted.Replace("[USERNAME]", objComment.AuthorUserName)
                formatted = formatted.Replace("[EMAIL]", objComment.AuthorEmail)
                formatted = formatted.Replace("[FIRSTNAME]", objComment.AuthorFirstName)
                formatted = formatted.Replace("[LASTNAME]", objComment.AuthorLastName)
                formatted = formatted.Replace("[FULLNAME]", objComment.AuthorFirstName & " " & objComment.AuthorLastName)
                formatted = formatted.Replace("[DISPLAYNAME]", objComment.AuthorDisplayName)
            End If

            Dim settings As PortalSettings = PortalController.Instance.GetCurrentPortalSettings()

            formatted = formatted.Replace("[PORTALNAME]", settings.PortalName)
            formatted = formatted.Replace("[POSTDATE]", DateTime.Now.ToShortDateString & " " & DateTime.Now.ToShortTimeString)

            formatted = formatted.Replace("[TITLE]", objArticle.Title)
            formatted = formatted.Replace("[COMMENT]", objComment.Comment.Replace("<br />", vbCrLf))
            formatted = formatted.Replace("[LINK]", link)
            formatted = formatted.Replace("[APPROVELINK]", Common.GetModuleLink(settings.ActiveTab.TabID, objArticle.ModuleID, "ApproveComments", articleSettings))

            Return formatted

        End Function

        Public Function GetApproverDistributionList(ByVal moduleID As Integer) As String

            Dim settings As PortalSettings = PortalSettings.Current
            Dim moduleSettings As Hashtable = Common.GetModuleSettings(moduleID)
            Dim distributionList As String = ""

            If (moduleSettings.Contains(ArticleConstants.PERMISSION_APPROVAL_SETTING)) Then

                Dim roles As String = moduleSettings(ArticleConstants.PERMISSION_APPROVAL_SETTING).ToString()
                Dim rolesArray() As String = roles.Split(Convert.ToChar(";"))
                Dim userList As Hashtable = New Hashtable

                For Each role As String In rolesArray
                    If (role.Length > 0) Then
                        Dim objRoleController As RoleController = New RoleController
                        Dim objRole As RoleInfo = objRoleController.GetRoleByName(settings.PortalId, role)

                        If Not (objRole Is Nothing) Then
                            Dim lstUsers As List(Of UserInfo) = RoleController.Instance.GetUsersByRole(settings.PortalId, objRole.RoleName)
                            For Each objUser As UserInfo In lstUsers
                                If (userList.Contains(objUser.UserID) = False) Then
                                    Dim objUserController As UserController = New UserController
                                    Dim objSelectedUser As UserInfo = objUserController.GetUser(settings.PortalId, objUser.UserID)
                                    If Not (objSelectedUser Is Nothing) Then
                                        If (objSelectedUser.Email.Length > 0) Then
                                            userList.Add(objUser.UserID, objSelectedUser.Email)
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                Next

                For Each email As DictionaryEntry In userList
                    If (distributionList.Length > 0) Then
                        distributionList += "; "
                    End If
                    distributionList += email.Value.ToString()
                Next

            End If

            Return distributionList

        End Function


#End Region

#Region " Public Methods "

        Public Function [Get](ByVal templateID As Integer) As EmailTemplateInfo

            Return CBO.FillObject(Of EmailTemplateInfo)(DataProvider.Instance().GetEmailTemplate(templateID))

        End Function

        Public Function [Get](ByVal moduleID As Integer, ByVal type As EmailTemplateType) As EmailTemplateInfo

            Dim objEmailTemplate As EmailTemplateInfo = CBO.FillObject(of EmailTemplateInfo)(DataProvider.Instance().GetEmailTemplateByName(moduleID, type.ToString()))

            If (objEmailTemplate Is Nothing) Then

                objEmailTemplate = New EmailTemplateInfo
                objEmailTemplate.ModuleID = moduleID
                objEmailTemplate.Name = type.ToString()

                Select Case type

                    Case EmailTemplateType.ArticleApproved
                        objEmailTemplate.Subject = "[PORTALNAME]: Your Article Has Been Approved"
                        objEmailTemplate.Template = "" _
                                & "Your article titled [TITLE] has been approved." & vbCrLf & vbCrLf _
                                & "To visit the live article, please visit:" & vbCrLf _
                                & "[LINK]" & vbCrLf & vbCrLf _
                                & "Thank you," & vbCrLf _
                                & "[PORTALNAME]"
                        Exit Select

                    Case EmailTemplateType.ArticleSubmission
                        objEmailTemplate.Subject = "[PORTALNAME]: New Article Requires Approval"
                        objEmailTemplate.Template = "" _
                                & "At [POSTDATE] an article title [TITLE] has been submitted for approval." & vbCrLf & vbCrLf _
                                & "[SUMMARY]" & vbCrLf & vbCrLf _
                                & "To view the complete article and approve, please visit:" & vbCrLf _
                                & "[LINK]" & vbCrLf & vbCrLf _
                                & "Thank you," & vbCrLf _
                                & "[PORTALNAME]"
                        Exit Select

                    Case EmailTemplateType.ArticleUpdateMirrored
                        objEmailTemplate.Subject = "[PORTALNAME]: An article has been updated"
                        objEmailTemplate.Template = "" _
                                & "At [POSTDATE] an article you have mirrored '[TITLE]' was updated." & vbCrLf & vbCrLf _
                                & "To visit the mirrored article, please visit:" & vbCrLf _
                                & "[LINK]" & vbCrLf & vbCrLf _
                                & "Thank you," & vbCrLf _
                                & "[PORTALNAME]"
                        Exit Select

                    Case EmailTemplateType.CommentNotification
                        objEmailTemplate.Subject = "[PORTALNAME]: Comment Notification"
                        objEmailTemplate.Template = "" _
                                & "At [POSTDATE] a comment was posted to the article [TITLE]." & vbCrLf & vbCrLf _
                                & "[COMMENT]" & vbCrLf & vbCrLf _
                                & "To view the complete article and reply, please visit:" & vbCrLf _
                                & "[LINK]" & vbCrLf & vbCrLf _
                                & "Thank you," & vbCrLf _
                                & "[PORTALNAME]"
                        Exit Select

                    Case EmailTemplateType.CommentRequiringApproval
                        objEmailTemplate.Subject = "[PORTALNAME]: Comment requiring approval"
                        objEmailTemplate.Template = "" _
                                & "At [POSTDATE], a comment requiring approval was posted to your article [TITLE]." & vbCrLf & vbCrLf _
                                & "[COMMENT]" & vbCrLf & vbCrLf _
                                & "To approve this comment, please visit:" & vbCrLf _
                                & "[APPROVELINK]" & vbCrLf & vbCrLf _
                                & "Thank you," & vbCrLf _
                                & "[PORTALNAME]"
                        Exit Select

                    Case EmailTemplateType.CommentApproved
                        objEmailTemplate.Subject = "[PORTALNAME]: Your Comment Has Been Approved"
                        objEmailTemplate.Template = "" _
                                & "Your comment posted to [TITLE] has been approved." & vbCrLf & vbCrLf _
                                & "To visit the live article, please visit:" & vbCrLf _
                                & "[LINK]" & vbCrLf & vbCrLf _
                                & "Thank you," & vbCrLf _
                                & "[PORTALNAME]"
                        Exit Select

                End Select

                objEmailTemplate.TemplateID = Add(objEmailTemplate)

            End If

            Return objEmailTemplate

        End Function

        Public Function List(ByVal moduleID As Integer) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().ListEmailTemplate(moduleID), GetType(EmailTemplateInfo))

        End Function

        Public Function Add(ByVal objEmailTemplate As EmailTemplateInfo) As Integer

            Return CType(DataProvider.Instance().AddEmailTemplate(objEmailTemplate.ModuleID, objEmailTemplate.Name, objEmailTemplate.Subject, objEmailTemplate.Template), Integer)

        End Function

        Public Sub Update(ByVal objEmailTemplate As EmailTemplateInfo)

            DataProvider.Instance().UpdateEmailTemplate(objEmailTemplate.TemplateID, objEmailTemplate.ModuleID, objEmailTemplate.Name, objEmailTemplate.Subject, objEmailTemplate.Template)

        End Sub

        Public Sub Delete(ByVal templateID As Integer)

            DataProvider.Instance().DeleteEmailTemplate(templateID)

        End Sub

        Public Sub SendFormattedEmail(ByVal moduleID As Integer, ByVal link As String, ByVal objArticle As ArticleInfo, ByVal type As EmailTemplateType, ByVal sendTo As String, ByVal articleSettings As ArticleSettings)

            Dim settings As PortalSettings = PortalController.Instance.GetCurrentPortalSettings()

            Dim subject As String = ""
            Dim template As String = ""

            Select Case type

                Case EmailTemplateType.ArticleSubmission
                    Dim objTemplate As EmailTemplateInfo = Me.Get(moduleID, EmailTemplateType.ArticleSubmission)
                    subject = objTemplate.Subject
                    template = objTemplate.Template

                    Exit Select

                Case EmailTemplateType.ArticleApproved
                    Dim objTemplate As EmailTemplateInfo = Me.Get(moduleID, EmailTemplateType.ArticleApproved)
                    subject = objTemplate.Subject
                    template = objTemplate.Template

                    Exit Select

                Case EmailTemplateType.ArticleUpdateMirrored
                    Dim objTemplate As EmailTemplateInfo = Me.Get(moduleID, EmailTemplateType.ArticleUpdateMirrored)
                    subject = objTemplate.Subject
                    template = objTemplate.Template

                    Exit Select

                Case Else
                    Exit Sub

            End Select

            subject = FormatArticleEmail(subject, link, objArticle, articleSettings)
            template = FormatArticleEmail(template, link, objArticle, articleSettings)

            ' SendNotification(settings.Email, sendTo, Null.NullString, subject, template)
            Try
                DotNetNuke.Services.Mail.Mail.SendMail(settings.Email, sendTo, "", subject, template, "", "", "", "", "", "")
            Catch exc As Exception
                LogException(exc)
            End Try

        End Sub

        Public Sub SendFormattedEmail(ByVal moduleID As Integer, ByVal link As String, ByVal objArticle As ArticleInfo, ByVal objComment As CommentInfo, ByVal type As EmailTemplateType, ByVal articleSettings As ArticleSettings)

            Dim settings As PortalSettings = PortalController.Instance.GetCurrentPortalSettings()

            Dim sendTo As String = ""

            Select Case type

                Case EmailTemplateType.CommentNotification

                    Dim objUserController As New UserController
                    Dim objUser As UserInfo = objUserController.GetUser(settings.PortalId, objArticle.AuthorID)

                    If Not (objUser Is Nothing) Then
                        sendTo = objUser.Email
                        SendFormattedEmail(moduleID, link, objArticle, objComment, EmailTemplateType.CommentNotification, articleSettings, sendTo)
                    End If

                    Exit Select

                Case EmailTemplateType.CommentApproved

                    If (objComment.UserID <> Null.NullInteger) Then
                        Dim objUserController As New UserController
                        Dim objUser As UserInfo = objUserController.GetUser(settings.PortalId, objComment.UserID)

                        If Not (objUser Is Nothing) Then
                            sendTo = objUser.Email
                            SendFormattedEmail(moduleID, link, objArticle, objComment, EmailTemplateType.CommentApproved, articleSettings, sendTo)
                        End If
                    Else
                        SendFormattedEmail(moduleID, link, objArticle, objComment, EmailTemplateType.CommentApproved, articleSettings, objComment.AnonymousEmail)
                    End If

                    Exit Select

                Case EmailTemplateType.CommentRequiringApproval

                    Dim objUserController As New UserController
                    Dim objUser As UserInfo = objUserController.GetUser(settings.PortalId, objArticle.AuthorID)

                    If Not (objUser Is Nothing) Then
                        sendTo = objUser.Email
                        SendFormattedEmail(moduleID, link, objArticle, objComment, EmailTemplateType.CommentRequiringApproval, articleSettings, sendTo)
                    End If

                    Exit Select

                Case Else
                    Exit Sub

            End Select

        End Sub

        Public Sub SendFormattedEmail(ByVal moduleID As Integer, ByVal link As String, ByVal objArticle As ArticleInfo, ByVal objComment As CommentInfo, ByVal type As EmailTemplateType, ByVal articleSettings As ArticleSettings, ByVal email As String)

            Dim settings As PortalSettings = PortalController.Instance.GetCurrentPortalSettings()

            Dim subject As String = ""
            Dim template As String = ""
            Dim sendTo As String = email

            Select Case type

                Case EmailTemplateType.CommentNotification
                    Dim objTemplate As EmailTemplateInfo = Me.Get(moduleID, EmailTemplateType.CommentNotification)
                    subject = objTemplate.Subject
                    template = objTemplate.Template

                    Exit Select

                Case EmailTemplateType.CommentApproved
                    Dim objTemplate As EmailTemplateInfo = Me.Get(moduleID, EmailTemplateType.CommentApproved)
                    subject = objTemplate.Subject
                    template = objTemplate.Template

                    Exit Select

                Case EmailTemplateType.CommentRequiringApproval
                    Dim objTemplate As EmailTemplateInfo = Me.Get(moduleID, EmailTemplateType.CommentRequiringApproval)
                    subject = objTemplate.Subject
                    template = objTemplate.Template

                    Exit Select

                Case Else
                    Exit Sub

            End Select

            subject = FormatCommentEmail(subject, link, objArticle, objComment, articleSettings)
            template = FormatCommentEmail(template, link, objArticle, objComment, articleSettings)

            Try
                DotNetNuke.Services.Mail.Mail.SendMail(settings.Email, sendTo, "", subject, template, "", "", "", "", "", "")
            Catch
            End Try

        End Sub

#End Region

    End Class

End Namespace
