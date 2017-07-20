Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports DotNetNuke.Entities.Users


Imports Ventrian.NewsArticles.Base
Imports Ventrian.NewsArticles.Import

Namespace Ventrian.NewsArticles

    Partial Public Class ucImportFeed
        Inherits NewsArticleModuleBase

#Region " Private Members "

        Private _feedID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub BindAutoExpiry()

            For Each value As Integer In System.Enum.GetValues(GetType(FeedExpiryType))
                Dim li As New ListItem
                li.Value = value.ToString()
                li.Text = Localization.GetString(System.Enum.GetName(GetType(FeedExpiryType), value), Me.LocalResourceFile)
                drpAutoExpire.Items.Add(li)
            Next

        End Sub

        Private Sub BindCategories()

            Dim objCategoryController As CategoryController = New CategoryController

            lstCategories.DataSource = objCategoryController.GetCategoriesAll(ModuleId, Null.NullInteger, ArticleSettings.CategorySortType)
            lstCategories.DataBind()

            Me.lstCategories.Height = Unit.Parse(ArticleSettings.CategorySelectionHeight.ToString())

        End Sub

        Private Sub BindDateModes()

            For Each value As Integer In System.Enum.GetValues(GetType(FeedDateMode))
                Dim li As New ListItem
                li.Value = value.ToString()
                li.Text = Localization.GetString(System.Enum.GetName(GetType(FeedDateMode), value), Me.LocalResourceFile)
                lstDateMode.Items.Add(li)
            Next

        End Sub

        Private Sub BindFeed()

            If (_feedID = Null.NullInteger) Then

                chkIsActive.Checked = True
                lblAuthor.Text = Me.UserInfo.Username
                lstDateMode.SelectedValue = Convert.ToInt32(FeedDateMode.ImportDate).ToString()
                drpAutoExpire.SelectedValue = Null.NullInteger.ToString()
                cmdDelete.Visible = False

            Else

                cmdDelete.Visible = True
                cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("Confirmation", LocalResourceFile) & "');")

                Dim objFeedController As New FeedController
                Dim objFeed As FeedInfo = objFeedController.Get(_feedID)

                If Not (objFeed Is Nothing) Then
                    txtTitle.Text = objFeed.Title
                    txtUrl.Text = objFeed.Url
                    chkAutoFeature.Checked = objFeed.AutoFeature
                    chkIsActive.Checked = objFeed.IsActive
                    lstDateMode.SelectedValue = Convert.ToInt32(objFeed.DateMode).ToString()
                    If (objFeed.AutoExpire <> Null.NullInteger) Then
                        txtAutoExpire.Text = objFeed.AutoExpire.ToString()
                    End If
                    drpAutoExpire.SelectedValue = Convert.ToInt32(objFeed.AutoExpireUnit).ToString()

                    Dim objUser As UserInfo = UserController.GetUser(PortalId, objFeed.UserID, True)
                    If (objUser IsNot Nothing) Then
                        lblAuthor.Text = objUser.Username
                    Else
                        lblAuthor.Text = Me.UserInfo.Username
                    End If

                    For Each objCategory As CategoryInfo In objFeed.Categories
                        For Each li As ListItem In lstCategories.Items
                            If (li.Value = objCategory.CategoryID.ToString()) Then
                                li.Selected = True
                            End If
                        Next
                    Next
                Else
                    Response.Redirect(EditUrl("ImportFeeds"), True)
                End If

            End If

        End Sub

        Private Sub ReadQueryString()

            If Not (Request("FeedID") Is Nothing) Then
                _feedID = Convert.ToInt32(Request("FeedID"))
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                ReadQueryString()

                If (IsPostBack = False) Then

                    BindAutoExpiry()
                    BindCategories()
                    BindDateModes()
                    BindFeed()

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click


            Try

                If (Page.IsValid) Then

                    Dim objFeedController As New FeedController

                    Dim objFeed As New FeedInfo

                    If (_feedID <> Null.NullInteger) Then
                        objFeed = objFeedController.Get(_feedID)
                    Else
                        objFeed = CType(CBO.InitializeObject(objFeed, GetType(FeedInfo)), FeedInfo)
                    End If

                    objFeed.ModuleID = Me.ModuleId
                    objFeed.Title = txtTitle.Text
                    objFeed.Url = txtUrl.Text
                    objFeed.AutoFeature = chkAutoFeature.Checked
                    objFeed.IsActive = chkIsActive.Checked
                    objFeed.DateMode = CType(System.Enum.Parse(GetType(FeedDateMode), lstDateMode.SelectedValue), FeedDateMode)
                    objFeed.AutoExpire = Null.NullInteger
                    If (txtAutoExpire.Text <> "") Then
                        If (IsNumeric(txtAutoExpire.Text)) Then
                            If (Convert.ToInt32(txtAutoExpire.Text) > 0) Then
                                objFeed.AutoExpire = Convert.ToInt32(txtAutoExpire.Text)
                            End If
                        End If
                    End If
                    objFeed.AutoExpireUnit = CType(System.Enum.Parse(GetType(FeedExpiryType), drpAutoExpire.SelectedValue), FeedExpiryType)

                    If (pnlAuthor.Visible) Then
                        Dim objUser As UserInfo = UserController.GetUserByName(Me.PortalId, txtAuthor.Text)

                        If (objUser IsNot Nothing) Then
                            objFeed.UserID = objUser.UserID
                        Else
                            objFeed.UserID = Me.UserId
                        End If
                    Else
                        Dim objUser As UserInfo = UserController.GetUserByName(Me.PortalId, lblAuthor.Text)

                        If (objUser IsNot Nothing) Then
                            objFeed.UserID = objUser.UserID
                        Else
                            objFeed.UserID = Me.UserId
                        End If
                    End If

                    Dim objCategories As New List(Of CategoryInfo)
                    For Each li As ListItem In lstCategories.Items
                        If (li.Selected) Then
                            Dim objCategory As New CategoryInfo
                            objCategory.CategoryID = Convert.ToInt32(li.Value)
                            objCategories.Add(objCategory)
                        End If
                    Next
                    objFeed.Categories = objCategories

                    If (_feedID = Null.NullInteger) Then
                        objFeedController.Add(objFeed)
                    Else
                        objFeedController.Update(objFeed)
                    End If

                    Response.Redirect(EditUrl("ImportFeeds"), True)

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

            Try

                Dim objFeedController As New FeedController
                objFeedController.Delete(_feedID)

                Response.Redirect(EditUrl("ImportFeeds"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(EditUrl("ImportFeeds"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub cmdSelectAuthor_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSelectAuthor.Click

            Try

                cmdSelectAuthor.Visible = False
                lblAuthor.Visible = False

                pnlAuthor.Visible = True
                txtAuthor.Text = lblAuthor.Text
                txtAuthor.Focus()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub valAuthor_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valAuthor.ServerValidate

            Try

                args.IsValid = False

                If (txtAuthor.Text <> "") Then
                    Dim objUser As UserInfo = UserController.GetUserByName(Me.PortalId, txtAuthor.Text)

                    If (objUser IsNot Nothing) Then
                        args.IsValid = True
                    End If
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace