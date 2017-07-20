'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2010
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Security

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ucEditComment
        Inherits NewsArticleModuleBase

#Region " Private Members "

        Private _commentID As Integer = Null.NullInteger
        Private _returnUrl As String = Null.NullString

#End Region

#Region " Private Methods "

        Private Sub BindComment()

            If (_commentID = Null.NullInteger) Then
                Response.Redirect(NavigateURL(), True)
            End If

            Dim objCommentController As New CommentController()
            Dim objComment As CommentInfo = objCommentController.GetComment(_commentID)

            If (ArticleSettings.IsAdmin() = False And ArticleSettings.IsApprover() = False) Then

                Dim objArticleController As New ArticleController()
                Dim objArticle As ArticleInfo = objArticleController.GetArticle(objComment.ArticleID)

                If (objArticle Is Nothing) Then
                    Response.Redirect(NavigateURL(), True)
                End If

                If (objArticle.AuthorID <> Me.UserId) Then
                    Response.Redirect(NavigateURL(), True)
                End If

            End If

            If (objComment.UserID <> Null.NullInteger) Then
                trName.Visible = False
                trEmail.Visible = False
                trUrl.Visible = False
            Else
                txtName.Text = objComment.AnonymousName
                txtEmail.Text = objComment.AnonymousEmail
                txtURL.Text = objComment.AnonymousURL
            End If

            txtComment.Text = objComment.Comment.Replace("<br />", vbCrLf)

        End Sub

        Private Function FilterInput(ByVal stringToFilter As String) As String

            Dim objPortalSecurity As New PortalSecurity

            stringToFilter = objPortalSecurity.InputFilter(stringToFilter, PortalSecurity.FilterFlag.NoScripting)

            stringToFilter = Replace(stringToFilter, Chr(13), "")
            stringToFilter = Replace(stringToFilter, ControlChars.Lf, "<br />")

            Return stringToFilter

        End Function

        Private Sub ReadQueryString()

            If (IsNumeric(Request("CommentID"))) Then
                _commentID = Convert.ToInt32(Request("CommentID"))
            End If

            If (Request("ReturnUrl") <> "") Then
                _returnUrl = Request("ReturnUrl")
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                ReadQueryString()

                If IsPostBack = False Then
                    BindComment()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click

            Try

                If (Page.IsValid) Then


                    Dim objCommentController As New CommentController()
                    Dim objComment As CommentInfo = objCommentController.GetComment(_commentID)

                    If (objComment IsNot Nothing) Then

                        If (objComment.UserID = Null.NullInteger) Then
                            objComment.AnonymousName = txtName.Text
                            objComment.AnonymousEmail = txtEmail.Text
                            objComment.AnonymousURL = txtURL.Text
                        End If

                        objComment.Comment = FilterInput(txtComment.Text)
                        objCommentController.UpdateComment(objComment)

                    End If

                    If (_returnUrl <> "") Then
                        Response.Redirect(_returnUrl, True)
                    Else
                        Response.Redirect(NavigateURL(), True)
                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click

            Try

                If (_returnUrl <> "") Then
                    Response.Redirect(_returnUrl, True)
                Else
                    Response.Redirect(NavigateURL(), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDelete.Click

            Try

                Dim objCommentController As New CommentController()
                Dim objComment As CommentInfo = objCommentController.GetComment(_commentID)

                If (objComment IsNot Nothing) Then
                    objCommentController.DeleteComment(_commentID, objComment.ArticleID)
                End If

                If (_returnUrl <> "") Then
                    Response.Redirect(_returnUrl, True)
                Else
                    Response.Redirect(NavigateURL(), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region


    End Class

End Namespace