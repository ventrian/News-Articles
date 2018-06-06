'
' News Articles for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Security

Imports Ventrian.NewsArticles.Base

Namespace Ventrian.NewsArticles

    Partial Public Class ViewCategory
        Inherits NewsArticleModuleBase

#Region " Constants "

        Private Const PARAM_CATEGORY_ID As String = "CategoryID"

#End Region

#Region " Private Members "

        Private _layoutController As LayoutController
        Private _objCategoriesAll As List(Of CategoryInfo)
        Private _categoryID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub BindCategory()

            If (_categoryID = Null.NullInteger) Then
                ' Category not specified
                Return
            End If

            Dim objCategoryController As New CategoryController
            Dim objCategory As CategoryInfo = objCategoryController.GetCategory(_categoryID, ModuleId)

            If Not (objCategory Is Nothing) Then

                If (objCategory.ModuleID <> Me.ModuleId) Then
                    ' Category does not belong to this module.
                    Response.Redirect(NavigateURL(), True)
                End If

                ProcessCategory(objCategory, phCategory.Controls)

            Else

                Response.Redirect(NavigateURL(), True)

            End If

        End Sub

        Private Sub ProcessCategoryChild(ByVal objCategory As CategoryInfo, ByRef objPlaceHolder As ControlCollection, ByVal moduleKey As String, ByVal templateArray As String(), ByVal level As Integer)

            For iPtr As Integer = 0 To templateArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(_layoutController.ProcessImages(templateArray(iPtr).ToString())))

                If iPtr < templateArray.Length - 1 Then
                    Select Case templateArray(iPtr + 1)

                        Case "ARTICLECOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = objCategory.NumberOfArticles.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "CATEGORYID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = objCategory.CategoryID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "DEPTHABS"
                            For Each objCategoryItem As CategoryInfo In _objCategoriesAll
                                If (objCategoryItem.CategoryID = objCategory.CategoryID) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                                    objLiteral.Text = objCategoryItem.Level.ToString()
                                    objPlaceHolder.Add(objLiteral)
                                End If
                            Next

                        Case "DEPTHREL"
                            For Each objCategoryItem As CategoryInfo In _objCategoriesAll
                                If (objCategoryItem.CategoryID = objCategory.CategoryID) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                                    objLiteral.Text = (objCategoryItem.Level - level).ToString()
                                    objPlaceHolder.Add(objLiteral)
                                End If
                            Next

                        Case "DESCRIPTION"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = Server.HtmlDecode(objCategory.Description)
                            objPlaceHolder.Add(objLiteral)

                        Case "HASIMAGE"
                            If (objCategory.Image = "") Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/HASIMAGE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASIMAGE"
                            ' Do Nothing

                        Case "HASNOIMAGE"
                            If (objCategory.Image <> "") Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/HASNOIMAGE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASNOIMAGE"
                            ' Do Nothing

                        Case "IMAGE"
                            If (objCategory.Image <> "") Then

                                If (objCategory.Image.Split("="c).Length = 2) Then
                                    If (IsNumeric(objCategory.Image.Split("="c)(1))) Then
                                        Dim objFile As DotNetNuke.Services.FileSystem.FileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(Convert.ToInt32(objCategory.Image.Split("="c)(1)))

                                        If (objFile IsNot Nothing) Then
                                            Dim objImage As New Image
                                            objImage.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                                            objImage.ImageUrl = PortalSettings.HomeDirectory & objFile.Folder & objFile.FileName
                                            objPlaceHolder.Add(objImage)
                                        End If
                                    End If

                                End If

                            End If

                        Case "IMAGELINK"
                            If (objCategory.Image <> "") Then

                                If (objCategory.Image.Split("="c).Length = 2) Then
                                    If (IsNumeric(objCategory.Image.Split("="c)(1))) Then
                                        Dim objFile As DotNetNuke.Services.FileSystem.FileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(Convert.ToInt32(objCategory.Image.Split("="c)(1)))

                                        If (objFile IsNot Nothing) Then
                                            Dim objLiteral As New Literal
                                            objLiteral.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                                            objLiteral.Text = PortalSettings.HomeDirectory & objFile.Folder & objFile.FileName
                                            objPlaceHolder.Add(objLiteral)
                                        End If
                                    End If

                                End If

                            End If


                        Case "LINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = Common.GetCategoryLink(TabId, ModuleId, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings)
                            objPlaceHolder.Add(objLiteral)

                        Case "METADESCRIPTION"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = objCategory.MetaDescription.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "NAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = objCategory.Name
                            objPlaceHolder.Add(objLiteral)

                        Case "RSSLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/RSS.aspx?TabID=" & TabId.ToString() & "&amp;ModuleID=" & ModuleId.ToString() & "&amp;CategoryID=" & objCategory.CategoryID.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "ORDER"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = objCategory.SortOrder.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "VIEWS"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = objCategory.NumberOfViews.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case Else
                            If (templateArray(iPtr + 1).ToUpper().StartsWith("DESCRIPTION:")) Then

                                Dim description As String = Server.HtmlDecode(objCategory.Description)
                                If (IsNumeric(templateArray(iPtr + 1).Substring(12, templateArray(iPtr + 1).Length - 12))) Then
                                    Dim length As Integer = Convert.ToInt32(templateArray(iPtr + 1).Substring(12, templateArray(iPtr + 1).Length - 12))
                                    If (StripHtml(Server.HtmlDecode(objCategory.Description)).TrimStart().Length > length) Then
                                        description = Left(StripHtml(Server.HtmlDecode(objCategory.Description)).TrimStart(), length) & "..."
                                    Else
                                        description = Left(StripHtml(Server.HtmlDecode(objCategory.Description)).TrimStart(), length)
                                    End If
                                End If

                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(moduleKey & "-" & iPtr.ToString())
                                objLiteral.Text = description
                                objPlaceHolder.Add(objLiteral)
                                Exit Select
                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("IFORDER:")) Then
                                Dim depth As String = templateArray(iPtr + 1).Substring(8, templateArray(iPtr + 1).Length - 8)
                                Dim isOrder As Boolean = False
                                For Each item As String In depth.Split(","c)
                                    If (IsNumeric(item)) Then
                                        If (objCategory.SortOrder = Convert.ToInt32(item)) Then
                                            isOrder = True
                                        End If
                                    End If
                                Next

                                Dim endToken As String = "/" & templateArray(iPtr + 1)
                                If (isOrder = False) Then
                                    While (iPtr < templateArray.Length - 1)
                                        If (templateArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("IFNOTORDER:")) Then
                                Dim depth As String = templateArray(iPtr + 1).Substring(11, templateArray(iPtr + 1).Length - 11)
                                Dim isOrder As Boolean = False
                                For Each item As String In depth.Split(","c)
                                    If (IsNumeric(item)) Then
                                        If (objCategory.SortOrder = Convert.ToInt32(item)) Then
                                            isOrder = True
                                        End If
                                    End If
                                Next

                                Dim endToken As String = "/" & templateArray(iPtr + 1)
                                If (isOrder = True) Then
                                    While (iPtr < templateArray.Length - 1)
                                        If (templateArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                            End If


                            If (templateArray(iPtr + 1).ToUpper().StartsWith("IMAGETHUMB:")) Then

                                If (objCategory.Image <> "") Then

                                    If (objCategory.Image.Split("="c).Length = 2) Then
                                        If (IsNumeric(objCategory.Image.Split("="c)(1))) Then
                                            Dim objFile As DotNetNuke.Services.FileSystem.FileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(Convert.ToInt32(objCategory.Image.Split("="c)(1)))

                                            If (objFile IsNot Nothing) Then

                                                Dim val As String = templateArray(iPtr + 1).Substring(11, templateArray(iPtr + 1).Length - 11)
                                                If (val.IndexOf(":"c) = -1) Then
                                                    Dim length As Integer = Convert.ToInt32(val)

                                                    Dim objImage As New Image
                                                    If (ArticleSettings.ImageThumbnailType = ThumbnailType.Proportion) Then
                                                        objImage.ImageUrl = Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & length.ToString() & "&Height=" & length.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory & objFile.Folder) & "&FileName=" & Server.UrlEncode(objFile.FileName) & "&PortalID=" & PortalId.ToString() & "&q=1")
                                                    Else
                                                        objImage.ImageUrl = Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & length.ToString() & "&Height=" & length.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory & objFile.Folder) & "&FileName=" & Server.UrlEncode(objFile.FileName) & "&PortalID=" & PortalId.ToString() & "&q=1&s=1")
                                                    End If
                                                    objImage.EnableViewState = False
                                                    objPlaceHolder.Add(objImage)

                                                Else

                                                    Dim arr() As String = val.Split(":"c)

                                                    Dim width As Integer = Convert.ToInt32(val.Split(":"c)(0))
                                                    Dim height As Integer = Convert.ToInt32(val.Split(":"c)(1))

                                                    Dim objImage As New Image
                                                    If (ArticleSettings.ImageThumbnailType = ThumbnailType.Proportion) Then
                                                        objImage.ImageUrl = Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory & objFile.Folder) & "&FileName=" & Server.UrlEncode(objFile.FileName) & "&PortalID=" & PortalId.ToString() & "&q=1")
                                                    Else
                                                        objImage.ImageUrl = Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory & objFile.Folder) & "&FileName=" & Server.UrlEncode(objFile.FileName) & "&PortalID=" & PortalId.ToString() & "&q=1&s=1")
                                                    End If
                                                    objImage.EnableViewState = False
                                                    objPlaceHolder.Add(objImage)

                                                End If

                                            End If
                                        End If
                                    End If
                                End If

                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("ISDEPTHABS:")) Then
                                Dim depth As String = templateArray(iPtr + 1).Substring(11, templateArray(iPtr + 1).Length - 11)
                                Dim isDepth As Boolean = False
                                For Each item As String In depth.Split(","c)
                                    If (IsNumeric(item)) Then
                                        For Each objCategoryItem As CategoryInfo In _objCategoriesAll
                                            If (objCategoryItem.CategoryID = objCategory.CategoryID) Then
                                                If (objCategoryItem.Level = Convert.ToInt32(item)) Then
                                                    isDepth = True
                                                End If
                                            End If
                                        Next
                                    End If
                                Next

                                Dim endToken As String = "/" & templateArray(iPtr + 1)
                                If (isDepth = False) Then
                                    While (iPtr < templateArray.Length - 1)
                                        If (templateArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("ISDEPTHREL:")) Then
                                Dim depth As String = templateArray(iPtr + 1).Substring(11, templateArray(iPtr + 1).Length - 11)
                                Dim isDepth As Boolean = False
                                For Each item As String In depth.Split(","c)
                                    If (IsNumeric(item)) Then
                                        For Each objCategoryItem As CategoryInfo In _objCategoriesAll
                                            If (objCategoryItem.CategoryID = objCategory.CategoryID) Then
                                                If ((objCategoryItem.Level - level) = Convert.ToInt32(item)) Then
                                                    isDepth = True
                                                End If
                                            End If
                                        Next
                                    End If
                                Next

                                Dim endToken As String = "/" & templateArray(iPtr + 1)
                                If (isDepth = False) Then
                                    While (iPtr < templateArray.Length - 1)
                                        If (templateArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("ISNOTDEPTHABS:")) Then
                                Dim depth As String = templateArray(iPtr + 1).Substring(14, templateArray(iPtr + 1).Length - 14)
                                Dim isDepth As Boolean = False
                                For Each item As String In depth.Split(","c)
                                    If (IsNumeric(item)) Then
                                        For Each objCategoryItem As CategoryInfo In _objCategoriesAll
                                            If (objCategoryItem.CategoryID = objCategory.CategoryID) Then
                                                If (objCategoryItem.Level = Convert.ToInt32(item)) Then
                                                    isDepth = True
                                                End If
                                            End If
                                        Next
                                    End If
                                Next

                                Dim endToken As String = "/" & templateArray(iPtr + 1)
                                If (isDepth = True) Then
                                    While (iPtr < templateArray.Length - 1)
                                        If (templateArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("ISNOTDEPTHREL:")) Then
                                Dim depth As String = templateArray(iPtr + 1).Substring(14, templateArray(iPtr + 1).Length - 14)
                                Dim isDepth As Boolean = False
                                For Each item As String In depth.Split(","c)
                                    If (IsNumeric(item)) Then
                                        For Each objCategoryItem As CategoryInfo In _objCategoriesAll
                                            If (objCategoryItem.CategoryID = objCategory.CategoryID) Then
                                                If ((objCategoryItem.Level - level) = Convert.ToInt32(item)) Then
                                                    isDepth = True
                                                End If
                                            End If
                                        Next
                                    End If
                                Next

                                Dim endToken As String = "/" & templateArray(iPtr + 1)
                                If (isDepth = True) Then
                                    While (iPtr < templateArray.Length - 1)
                                        If (templateArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                            End If

                    End Select
                End If
            Next

        End Sub

        Private Sub ProcessCategory(ByVal objCategory As CategoryInfo, ByRef objPlaceHolder As ControlCollection)

            _layoutController = New LayoutController(Me)
            Dim layoutCategory As LayoutInfo = LayoutController.GetLayout(Me, LayoutType.Category_Html)
            Dim layoutCategoryChild As LayoutInfo = LayoutController.GetLayout(Me, LayoutType.Category_Child_Html)
            Dim templateArray As String() = layoutCategory.Tokens

            Dim objCategoryController As New CategoryController
            Dim objParentCategory As CategoryInfo = Nothing

            If (objCategory.ParentID <> Null.NullInteger) Then
                objParentCategory = objCategoryController.GetCategory(objCategory.ParentID, ModuleId)
            End If

            Dim objCategoriesChildren As List(Of CategoryInfo) = objCategoryController.GetCategories(Me.ModuleId, objCategory.CategoryID)

            For iPtr As Integer = 0 To templateArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(_layoutController.ProcessImages(templateArray(iPtr).ToString())))

                If iPtr < templateArray.Length - 1 Then
                    Select Case templateArray(iPtr + 1)

                        Case "ARTICLECOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                            objLiteral.Text = objCategory.NumberOfArticles.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "CATEGORYLABEL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                            Dim entriesFrom As String = Localization.GetString("CategoryEntries", LocalResourceFile)
                            If (entriesFrom.Contains("{0}")) Then
                                objLiteral.Text = String.Format(entriesFrom, objCategory.Name)
                            Else
                                objLiteral.Text = objCategory.Name
                            End If
                            objPlaceHolder.Add(objLiteral)

                        Case "CATEGORYID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                            objLiteral.Text = objCategory.CategoryID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "CHILDCATEGORIES"
                            If (objCategoriesChildren.Count > 0) Then
                                For Each objCategoryItem As CategoryInfo In _objCategoriesAll
                                    If (objCategoryItem.CategoryID = objCategory.CategoryID) Then
                                        Dim i As Integer = 0
                                        For Each objCategoryChild As CategoryInfo In objCategoriesChildren
                                            ProcessCategoryChild(objCategoryChild, objPlaceHolder, "ChildCategory-" & i.ToString() & "-" & iPtr.ToString(), layoutCategoryChild.Tokens, objCategoryItem.Level)
                                            i = i + 1
                                        Next
                                    End If
                                Next
                            End If

                        Case "DESCRIPTION"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                            objLiteral.Text = Server.HtmlDecode(objCategory.Description)
                            objPlaceHolder.Add(objLiteral)

                        Case "HASCHILDCATEGORIES"
                            If (objCategoriesChildren.Count = 0) Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/HASCHILDCATEGORIES") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASCHILDCATEGORIES"
                            ' Do Nothing

                        Case "HASNOCHILDCATEGORIES"
                            If (objCategoriesChildren.Count > 0) Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/HASNOCHILDCATEGORIES") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASNOCHILDCATEGORIES"
                            ' Do Nothing

                        Case "HASNOPARENT"
                            If (objParentCategory IsNot Nothing) Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/HASNOPARENT") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASNOPARENT"
                            ' Do Nothing

                        Case "HASPARENT"
                            If (objParentCategory Is Nothing) Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/HASPARENT") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASPARENT"
                            ' Do Nothing

                        Case "HASIMAGE"
                            If (objCategory.Image = "") Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/HASIMAGE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASIMAGE"
                            ' Do Nothing

                        Case "HASNOIMAGE"
                            If (objCategory.Image <> "") Then
                                While (iPtr < templateArray.Length - 1)
                                    If (templateArray(iPtr + 1) = "/HASNOIMAGE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASNOIMAGE"
                            ' Do Nothing

                        Case "IMAGE"
                            If (objCategory.Image <> "") Then

                                If (objCategory.Image.Split("="c).Length = 2) Then
                                    If (IsNumeric(objCategory.Image.Split("="c)(1))) Then
                                        Dim objFile As DotNetNuke.Services.FileSystem.FileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(Convert.ToInt32(objCategory.Image.Split("="c)(1)))

                                        If (objFile IsNot Nothing) Then
                                            Dim objImage As New Image
                                            objImage.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                                            objImage.ImageUrl = PortalSettings.HomeDirectory & objFile.Folder & objFile.FileName
                                            objPlaceHolder.Add(objImage)
                                        End If
                                    End If

                                End If

                            End If

                        Case "IMAGELINK"
                            If (objCategory.Image <> "") Then

                                If (objCategory.Image.Split("="c).Length = 2) Then
                                    If (IsNumeric(objCategory.Image.Split("="c)(1))) Then
                                        Dim objFile As DotNetNuke.Services.FileSystem.FileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(Convert.ToInt32(objCategory.Image.Split("="c)(1)))

                                        If (objFile IsNot Nothing) Then
                                            Dim objLiteral As New Literal
                                            objLiteral.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                                            objLiteral.Text = PortalSettings.HomeDirectory & objFile.Folder & objFile.FileName
                                            objPlaceHolder.Add(objLiteral)
                                        End If
                                    End If

                                End If

                            End If

                        Case "LINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                            objLiteral.Text = Common.GetCategoryLink(TabId, ModuleId, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings)
                            objPlaceHolder.Add(objLiteral)

                        Case "METADESCRIPTION"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                            objLiteral.Text = objCategory.MetaDescription.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "NAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                            objLiteral.Text = objCategory.Name
                            objPlaceHolder.Add(objLiteral)

                        Case "PARENTDESCRIPTION"
                            If (objParentCategory IsNot Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                                objLiteral.Text = Server.HtmlDecode(objParentCategory.Description)
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "PARENTLINK"
                            If (objParentCategory IsNot Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                                objLiteral.Text = Common.GetCategoryLink(TabId, ModuleId, objParentCategory.CategoryID.ToString(), objParentCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings)
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "PARENTNAME"
                            If (objParentCategory IsNot Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                                objLiteral.Text = objParentCategory.Name
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "RSSLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                            objLiteral.Text = Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/RSS.aspx?TabID=" & TabId.ToString() & "&amp;ModuleID=" & ModuleId.ToString() & "&amp;CategoryID=" & objCategory.CategoryID.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "VIEWS"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                            objLiteral.Text = objCategory.NumberOfViews.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case Else
                            If (templateArray(iPtr + 1).ToUpper().StartsWith("CHILDCATEGORIES:")) Then
                                Dim count As String = templateArray(iPtr + 1).Substring(16, templateArray(iPtr + 1).Length - 16)
                                If (IsNumeric(count)) Then
                                    Dim relativeLevel As Integer = Null.NullInteger
                                    For Each objCategoryItem As CategoryInfo In _objCategoriesAll
                                        If (objCategoryItem.CategoryID = objCategory.CategoryID) Then
                                            relativeLevel = objCategoryItem.Level
                                        End If
                                    Next

                                    Dim level As Integer = Null.NullInteger
                                    Dim i As Integer = 0
                                    For Each objCategoryItem As CategoryInfo In _objCategoriesAll
                                        If (level <> Null.NullInteger AndAlso objCategoryItem.Level <= level) Then
                                            Exit For
                                        End If
                                        If (objCategoryItem.CategoryID = objCategory.CategoryID) Then
                                            level = objCategoryItem.Level
                                        Else
                                            If (level <> Null.NullInteger) Then
                                                If (objCategoryItem.Level > level And ((objCategoryItem.Level - relativeLevel) <= Convert.ToInt32(count))) Then
                                                    ProcessCategoryChild(objCategoryItem, objPlaceHolder, "ChildCategory" & i.ToString() & "-" & iPtr.ToString(), layoutCategoryChild.Tokens(), relativeLevel)
                                                    i = i + 1
                                                End If
                                            End If
                                        End If
                                    Next
                                End If
                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("DESCRIPTION:")) Then

                                Dim description As String = Server.HtmlDecode(objCategory.Description)
                                If (IsNumeric(templateArray(iPtr + 1).Substring(12, templateArray(iPtr + 1).Length - 12))) Then
                                    Dim length As Integer = Convert.ToInt32(templateArray(iPtr + 1).Substring(12, templateArray(iPtr + 1).Length - 12))
                                    If (StripHtml(Server.HtmlDecode(objCategory.Description)).TrimStart().Length > length) Then
                                        description = Left(StripHtml(Server.HtmlDecode(objCategory.Description)).TrimStart(), length) & "..."
                                    Else
                                        description = Left(StripHtml(Server.HtmlDecode(objCategory.Description)).TrimStart(), length)
                                    End If
                                End If

                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                                objLiteral.Text = description
                                objPlaceHolder.Add(objLiteral)
                                Exit Select
                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("ISDEPTHABS:")) Then
                                Dim depth As String = templateArray(iPtr + 1).Substring(11, templateArray(iPtr + 1).Length - 11)
                                Dim isDepth As Boolean = False
                                For Each item As String In depth.Split(","c)
                                    If (IsNumeric(item)) Then
                                        For Each objCategoryItem As CategoryInfo In _objCategoriesAll
                                            If (objCategoryItem.CategoryID = objCategory.CategoryID) Then
                                                If (objCategoryItem.Level = Convert.ToInt32(item)) Then
                                                    isDepth = True
                                                End If
                                            End If
                                        Next
                                    End If
                                Next

                                Dim endToken As String = "/" & templateArray(iPtr + 1)
                                If (isDepth = False) Then
                                    While (iPtr < templateArray.Length - 1)
                                        If (templateArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("ISNOTDEPTHABS:")) Then
                                Dim depth As String = templateArray(iPtr + 1).Substring(14, templateArray(iPtr + 1).Length - 14)
                                Dim isDepth As Boolean = False
                                For Each item As String In depth.Split(","c)
                                    If (IsNumeric(item)) Then
                                        For Each objCategoryItem As CategoryInfo In _objCategoriesAll
                                            If (objCategoryItem.CategoryID = objCategory.CategoryID) Then
                                                If (objCategoryItem.Level = Convert.ToInt32(item)) Then
                                                    isDepth = True
                                                End If
                                            End If
                                        Next
                                    End If
                                Next

                                Dim endToken As String = "/" & templateArray(iPtr + 1)
                                If (isDepth = True) Then
                                    While (iPtr < templateArray.Length - 1)
                                        If (templateArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("IMAGETHUMB:")) Then

                                If (objCategory.Image <> "") Then

                                    If (objCategory.Image.Split("="c).Length = 2) Then
                                        If (IsNumeric(objCategory.Image.Split("="c)(1))) Then
                                            Dim objFile As DotNetNuke.Services.FileSystem.FileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(Convert.ToInt32(objCategory.Image.Split("="c)(1)))

                                            If (objFile IsNot Nothing) Then

                                                Dim val As String = templateArray(iPtr + 1).Substring(11, templateArray(iPtr + 1).Length - 11)
                                                If (val.IndexOf(":"c) = -1) Then
                                                    Dim length As Integer = Convert.ToInt32(val)

                                                    Dim objImage As New Image
                                                    If (ArticleSettings.ImageThumbnailType = ThumbnailType.Proportion) Then
                                                        objImage.ImageUrl = Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & length.ToString() & "&Height=" & length.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory & objFile.Folder) & "&FileName=" & Server.UrlEncode(objFile.FileName) & "&PortalID=" & PortalId.ToString() & "&q=1")
                                                    Else
                                                        objImage.ImageUrl = Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & length.ToString() & "&Height=" & length.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory & objFile.Folder) & "&FileName=" & Server.UrlEncode(objFile.FileName) & "&PortalID=" & PortalId.ToString() & "&q=1&s=1")
                                                    End If
                                                    objImage.EnableViewState = False
                                                    objPlaceHolder.Add(objImage)

                                                Else

                                                    Dim arr() As String = val.Split(":"c)

                                                    Dim width As Integer = Convert.ToInt32(val.Split(":"c)(0))
                                                    Dim height As Integer = Convert.ToInt32(val.Split(":"c)(1))

                                                    Dim objImage As New Image
                                                    If (ArticleSettings.ImageThumbnailType = ThumbnailType.Proportion) Then
                                                        objImage.ImageUrl = Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory & objFile.Folder) & "&FileName=" & Server.UrlEncode(objFile.FileName) & "&PortalID=" & PortalId.ToString() & "&q=1")
                                                    Else
                                                        objImage.ImageUrl = Page.ResolveUrl("~/DesktopModules/DnnForge - NewsArticles/ImageHandler.ashx?Width=" & width.ToString() & "&Height=" & height.ToString() & "&HomeDirectory=" & Server.UrlEncode(PortalSettings.HomeDirectory & objFile.Folder) & "&FileName=" & Server.UrlEncode(objFile.FileName) & "&PortalID=" & PortalId.ToString() & "&q=1&s=1")
                                                    End If
                                                    objImage.EnableViewState = False
                                                    objPlaceHolder.Add(objImage)

                                                End If

                                            End If
                                        End If
                                    End If
                                End If

                            End If

                            If (templateArray(iPtr + 1).ToUpper().StartsWith("PARENTDESCRIPTION:")) Then

                                If (objParentCategory IsNot Nothing) Then
                                    Dim description As String = Server.HtmlDecode(objParentCategory.Description)
                                    If (IsNumeric(templateArray(iPtr + 1).Substring(18, templateArray(iPtr + 1).Length - 18))) Then
                                        Dim length As Integer = Convert.ToInt32(templateArray(iPtr + 1).Substring(18, templateArray(iPtr + 1).Length - 18))
                                        If (StripHtml(Server.HtmlDecode(objParentCategory.Description)).TrimStart().Length > length) Then
                                            description = Left(StripHtml(Server.HtmlDecode(objParentCategory.Description)).TrimStart(), length) & "..."
                                        Else
                                            description = Left(StripHtml(Server.HtmlDecode(objParentCategory.Description)).TrimStart(), length)
                                        End If
                                    End If

                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Category-" & iPtr.ToString())
                                    objLiteral.Text = description
                                    objPlaceHolder.Add(objLiteral)
                                    Exit Select
                                End If
                            End If

                    End Select
                End If

            Next

        End Sub

        Private Sub ReadQueryString()

            If (Request(PARAM_CATEGORY_ID) <> "" AndAlso IsNumeric(Request(PARAM_CATEGORY_ID))) Then
                _categoryID = Convert.ToInt32(Request(PARAM_CATEGORY_ID))
            Else
                If (ArticleSettings.FilterSingleCategory <> Null.NullInteger) Then
                    _categoryID = ArticleSettings.FilterSingleCategory
                End If
            End If


        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                ReadQueryString()

                Dim objCategoryController As New CategoryController
                _objCategoriesAll = objCategoryController.GetCategoriesAll(Me.ModuleId, Null.NullInteger, ArticleSettings.CategorySortType)

                If (Request("articleType") <> "" AndAlso Request("articleType").ToLower() = "categoryview") Then

                    For Each objCategory As CategoryInfo In _objCategoriesAll
                        If (objCategory.CategoryID = _categoryID) Then

                            If (ArticleSettings.FilterSingleCategory = objCategory.CategoryID) Then
                                Exit For
                            End If

                            Dim path As String = ""
                            If (ArticleSettings.CategoryBreadcrumb) Then
                                Dim objTab As New DotNetNuke.Entities.Tabs.TabInfo
                                objTab.TabName = objCategory.Name
                                objTab.Url = Common.GetCategoryLink(TabId, ModuleId, objCategory.CategoryID.ToString(), objCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings)
                                PortalSettings.ActiveTab.BreadCrumbs.Add(objTab)

                                Dim parentID As Integer = objCategory.ParentID
                                Dim parentCount As Integer = 0

                                While parentID <> Null.NullInteger
                                    For Each objParentCategory As CategoryInfo In _objCategoriesAll
                                        If (objParentCategory.CategoryID = parentID) Then
                                            If (ArticleSettings.FilterSingleCategory = objParentCategory.CategoryID) Then
                                                parentID = Null.NullInteger
                                                Exit For
                                            End If
                                            Dim objParentTab As New DotNetNuke.Entities.Tabs.TabInfo
                                            objParentTab.TabID = 10000 + objParentCategory.CategoryID
                                            objParentTab.TabName = objParentCategory.Name
                                            objParentTab.Url = Common.GetCategoryLink(TabId, ModuleId, objParentCategory.CategoryID.ToString(), objParentCategory.Name, ArticleSettings.LaunchLinks, ArticleSettings)
                                            PortalSettings.ActiveTab.BreadCrumbs.Insert(PortalSettings.ActiveTab.BreadCrumbs.Count - 1 - parentCount, objParentTab)

                                            If (path.Length = 0) Then
                                                path = " > " & objParentCategory.Name
                                            Else
                                                path = " > " & objParentCategory.Name & path
                                            End If

                                            parentCount = parentCount + 1
                                            parentID = objParentCategory.ParentID
                                        End If
                                    Next
                                End While
                            End If

                            If (Request("articleType") <> "" AndAlso Request("articleType").ToLower() = "categoryview") Then
                                If (PortalSettings.ActiveTab.Title.Length = 0) Then
                                    Me.BasePage.Title = Server.HtmlEncode(PortalSettings.PortalName & " > " & PortalSettings.ActiveTab.TabName & path & " > " & objCategory.Name)
                                Else
                                    Me.BasePage.Title = Server.HtmlEncode(PortalSettings.ActiveTab.Title & path & " > " & objCategory.Name)
                                End If

                                If (objCategory.MetaTitle <> "") Then
                                    Me.BasePage.Title = objCategory.MetaTitle
                                End If
                                If (objCategory.MetaDescription <> "") Then
                                    Me.BasePage.Description = objCategory.MetaDescription
                                End If
                                If (objCategory.MetaKeywords <> "") Then
                                    Me.BasePage.KeyWords = objCategory.MetaKeywords
                                End If

                            End If

                            If (ArticleSettings.IncludeInPageName) Then
                                HttpContext.Current.Items.Add("NA-CategoryName", objCategory.Name)
                            End If

                            Exit For
                        End If
                    Next

                End If

                BindCategory()

                Dim categories() As Integer = {_categoryID}
                Listing1.FilterCategories = categories
                If (ArticleSettings.FilterSingleCategory <> Null.NullInteger) Then
                    Listing1.ShowExpired = False
                Else
                    Listing1.ShowExpired = True
                End If
                Listing1.MaxArticles = Null.NullInteger
                Listing1.ShowMessage = False

                If (ArticleSettings.CategoryBreadcrumb AndAlso Request("CategoryID") <> "") Then
                    Listing1.IncludeCategory = True
                End If

                Listing1.BindListing()
                Listing1.BindArticles = False
                Listing1.IsIndexed = False

                'Listing1.BindListing()

                'ucHeader1.ProcessMenu()
                'ucHeader2.ProcessMenu()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                If (HttpContext.Current.Items.Contains("NA-CategoryName")) Then
                    PortalSettings.ActiveTab.TabName = HttpContext.Current.Items("NA-CategoryName").ToString()
                End If

            Catch ex As Exception

            End Try

        End Sub

#End Region

    End Class

End Namespace