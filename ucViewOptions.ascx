<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucViewOptions.ascx.vb" Inherits="Ventrian.NewsArticles.ucViewOptions" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Skin" Src="~/controls/SkinControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Security.Permissions.Controls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<article:Header runat="server" id="ucHeader1" SelectedMenu="AdminOptions" MenuPosition="Top" />
<table class="Settings" cellspacing="2" cellpadding="2" width="560" summary="Module Settings Design Table"
	border="0">
	<tr>
		<td width="560" valign="top">
			<dnn:sectionhead id="dshBasic" cssclass="Head" runat="server" section="tblArticle" resourcekey="ArticleSettings"
				includerule="True" />
			<table id="tblArticle" cellspacing="2" cellpadding="2" summary="Module Details Design Table"
				border="0" runat="server">
				<tr>
					<td colspan="2"><asp:label id="lblArticleSettingsHelp" cssclass="Normal" resourcekey="ArticleSettingsHelp"
							runat="server" enableviewstate="False"></asp:label></td>
				</tr>
				<tr>
					<td width="25"></td>
					<td valign="top" width="475">
						<dnn:sectionhead id="dshDetails" cssclass="Head" runat="server" text="Basic Settings" resourcekey="BasicSettings"
							section="tblDetails" />
						<table id="tblDetails" cellspacing="2" cellpadding="2" summary="Appearance Design Table"
							border="0" runat="server">
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plEnableRatings" resourcekey="EnableRatings" helpkey="EnableRatingsHelp" runat="server"
										controlname="chkEnableRatingsAuthenticated"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkEnableRatingsAuthenticated" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plEnableAnonymousRatings" resourcekey="EnableAnonymousRatings" helpkey="EnableAnonymousRatingsHelp"
										runat="server" controlname="chkEnableRatingsAnonymous"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkEnableRatingsAnonymous" Runat="server"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plEnableCoreSearch" resourcekey="EnableCoreSearch" helpkey="EnableCoreSearchHelp"
										runat="server" controlname="chkEnableCoreSearch"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkEnableCoreSearch" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plEnableNotificationPing" resourcekey="EnableNotificationPing" helpkey="EnableNotificationPingHelp"
										runat="server" controlname="chkEnableNotificationPing"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkEnableNotificationPing" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plEnableAutoTrackback" resourcekey="EnableAutoTrackback" helpkey="EnableAutoTrackbackHelp"
										runat="server" controlname="chkEnableAutoTrackback"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkEnableAutoTrackback" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plEnableIncomingTrackback" resourcekey="EnableIncomingTrackback" helpkey="EnableIncomingTrackbackHelp"
										runat="server" controlname="chkEnableIncomingTrackback"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkEnableIncomingTrackback" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plLaunchLinks" resourcekey="LaunchLinks" helpkey="LaunchLinksHelp" runat="server"
										controlname="chkLaunchLinks"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkLaunchLinks" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plBubbleFeaturedArticles" resourcekey="BubbleFeaturedArticles" helpkey="BubbleFeaturedArticlesHelp"
										runat="server" controlname="chkBubbleFeaturedArticles"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkBubbleFeaturedArticles" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plProcessPostTokens" 
										runat="server" controlname="chkProcessPostTokens"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkProcessPostTokens" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plDisplayType" runat="server" controlname="drpDisplayType"></dnn:label></td>
								<td valign="top"><asp:DropDownList id="drpDisplayType" Runat="server" CssClass="NormalTextBox" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="200" nowrap><dnn:label id="plArticlePageSize" resourcekey="ArticlePageSize" helpkey="ArticlePageSizeHelp"
										runat="server" controlname="drpNumber"></dnn:label></td>
								<td valign="top">
									<asp:dropdownlist id="drpNumber" runat="server" CssClass="NormalTextBox" />
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plTemplate" resourcekey="Template" helpkey="TemplateHelp" runat="server" controlname="drpTemplates"></dnn:label></td>
								<td valign="top"><asp:DropDownList id="drpTemplates" Runat="server" CssClass="NormalTextBox" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plTimeZone" resourcekey="TimeZone" runat="server" controlname="drpTimeZone"></dnn:label></td>
								<td valign="top"><asp:dropdownlist id="drpTimeZone" runat="server" cssclass="NormalTextBox" width="300"></asp:dropdownlist></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plSortBy" resourcekey="SortBy" helpkey="SortByHelp" runat="server" controlname="drpSortBy"></dnn:label></td>
								<td valign="top"><asp:dropdownlist id="drpSortBy" Runat="server" CssClass="NormalTextBox"></asp:dropdownlist></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plSortDirection" resourcekey="SortDirection" runat="server" controlname="drpSortDirection"></dnn:label></td>
								<td valign="top"><asp:dropdownlist id="drpSortDirection" Runat="server" CssClass="NormalTextBox"></asp:dropdownlist></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plMenuPosition" resourcekey="MenuPosition" runat="server" controlname="lstMenuPosition"></dnn:label></td>
								<td valign="top"><asp:RadioButtonList ID="lstMenuPosition" Runat="server" CssClass="Normal" RepeatDirection="Horizontal" /></td>
							</tr>
						</table>
						<br />
						<dnn:sectionhead id="dshArchive" cssclass="Head" runat="server" text="Archive Settings" resourcekey="ArchiveSettings"
							section="tblArchive" isexpanded="False" />
						<table id="tblArchive" cellspacing="2" cellpadding="2" summary="Archive Design Table"
							border="0" runat="server">
							<tr>
								<td class="SubHead" width="200"><dnn:label id="Label1" resourcekey="ArchiveCurrentArticles" runat="server"
										controlname="chkArchiveCurrentArticles"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkArchiveCurrentArticles" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="Label2" resourcekey="ArchiveCategories" runat="server"
										controlname="chkArchiveCategories"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkArchiveCategories" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="Label3" resourcekey="ArchiveAuthor" runat="server"
										controlname="chkArchiveAuthor"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkArchiveAuthor" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="Label4" resourcekey="ArchiveMonth" runat="server"
										controlname="chkArchiveMonth"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkArchiveMonth" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
					    </table>		
						<br />
						<dnn:sectionhead id="dshCategory" cssclass="Head" runat="server" text="Category Settings" resourcekey="CategorySettings"
							section="tblCategory" isexpanded="False" />
						<table id="tblCategory" cellspacing="2" cellpadding="2" summary="Appearance Design Table"
							border="0" runat="server">
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plRequireCategory" resourcekey="RequireCategory" runat="server"
										controlname="chkRequireCategory"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkRequireCategory" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plDefaultCategories" resourcekey="DefaultCategories" runat="server"
										controlname="chkDefaultCategories"></dnn:label></td>
								<td valign="top">
								    <asp:ListBox ID="lstDefaultCategories" runat="server" CssClass="Normal" DataTextField="NameIndented" 
			                            DataValueField="CategoryID" Width="300px" SelectionMode="Multiple" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plCategorySelectionHeight" runat="server" resourcekey="CategorySelectionHeight" suffix=":" controlname="txtCategorySelectionHeight"></dnn:label></td>
								<td valign="top">
									<asp:TextBox id="txtCategorySelectionHeight" Runat="server" CssClass="NormalTextBox" MaxLength="8" />
									<asp:RequiredFieldValidator id="valCategorySelectionHeight" runat="server" CssClass="NormalRed" ErrorMessage="Required"
										Display="Dynamic" ResourceKey="valCategorySelectionHeight.ErrorMessage" ControlToValidate="txtCategorySelectionHeight"></asp:RequiredFieldValidator>
									<asp:CompareValidator id="valCategorySelectionHeightIsValid" runat="server" CssClass="NormalRed" ErrorMessage="Must be a valid number e.g. 100, 200"
										Display="Dynamic" ResourceKey="valCategorySelectionHeightIsValid.ErrorMessage" ControlToValidate="txtCategorySelectionHeight"
										Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plCategoryBreadcrumb" resourcekey="CategoryBreadcrumb" runat="server"
										controlname="chkCategoryBreadcrumb"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkCategoryBreadcrumb" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plCategoryName" resourcekey="CategoryName" runat="server"
										controlname="chkCategoryName"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkCategoryName" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plCategorySortOrder" resourcekey="CategorySortOrder" runat="server" controlname="lstCategorySortOrder"></dnn:label></td>
								<td valign="top"><asp:RadioButtonList ID="lstCategorySortOrder" Runat="server" CssClass="Normal" RepeatDirection="Horizontal" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plCategoryFilterSubmit" resourcekey="CategoryFilterSubmit" runat="server"
										controlname="chkCategoryFilterSubmit"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkCategoryFilterSubmit" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
						</table>
						<br />
						<dnn:sectionhead id="dshTags" cssclass="Head" runat="server" text="Tag Settings" resourcekey="TagSettings"
							section="tblTags" isexpanded="False" />
						<table id="tblTags" cellspacing="2" cellpadding="2" summary="Appearance Design Table"
							border="0" runat="server">
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plUseStaticTagsList" runat="server" resourcekey="UseStaticTagsList" controlname="chkUseStaticTagsList"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkUseStaticTagsList" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
						</table>
						<br />
						<dnn:sectionhead id="dshComments" cssclass="Head" runat="server" text="Comment Settings" resourcekey="CommentSettings"
							section="tblComments" isexpanded="False" />
						<table id="tblComments" cellspacing="2" cellpadding="2" summary="Appearance Design Table"
							border="0" runat="server">
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plEnableComments" resourcekey="EnableComments" helpkey="EnableCommentsHelp"
										runat="server" controlname="chkEnableComments"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkEnableComments" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plEnableAnonymousComments" resourcekey="EnableAnonymousComments" helpkey="EnableAnonymousCommentsHelp"
										runat="server" controlname="chkEnableAnonymousComments"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkEnableAnonymousComments" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plEnableCommentModeration" resourcekey="EnableCommentModeration" 
										runat="server" controlname="chkEnableCommentModeration"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkEnableCommentModeration" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plCaptchaType" resourcekey="CaptchaType" runat="server" controlname="drpCaptchaType"></dnn:label></td>
								<td valign="top">
								    <asp:dropdownlist id="drpCaptchaType" Runat="server" CssClass="NormalTextBox"></asp:dropdownlist>
								</td>
							</tr>
						    <tr>
						        <td class="SubHead" width="200"><dnn:label id="plReCaptchaSiteKey" runat="server" resourcekey="reCaptchaSiteKey" suffix=":" controlname="txtReCaptchaSiteKey"></dnn:label></td>
						        <td valign="top">
						            <asp:TextBox id="txtReCaptchaSiteKey" Runat="server" CssClass="NormalTextBox" MaxLength="50" />
						        </td>
						    </tr>
						    <tr>
						        <td class="SubHead" width="200"><dnn:label id="plReCaptchaSecretKey" runat="server" resourcekey="reCaptchaSecretKey" suffix=":" controlname="txtReCaptchaSecretKey"></dnn:label></td>
						        <td valign="top">
						            <asp:TextBox id="txtReCaptchaSecretKey" Runat="server" CssClass="NormalTextBox" MaxLength="50" />
						        </td>
						    </tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plHideWebsite" resourcekey="HideWebsite" runat="server" controlname="chkHideWebsite"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkHideWebsite" Runat="server"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plRequireName" resourcekey="plRequireName" runat="server" controlname="chkRequireName"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkRequireName" Runat="server"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plRequireEmail" resourcekey="plRequireEmail" runat="server" controlname="chkRequireEmail"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkRequireEmail" Runat="server"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plNotifyDefault" resourcekey="NotifyDefault" runat="server" controlname="chkNotifyDefault"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkNotifyDefault" Runat="server"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plSortDirectionComments" resourcekey="SortDirectionComments" runat="server" controlname="drpSortDirectionComments"></dnn:label></td>
								<td valign="top"><asp:dropdownlist id="drpSortDirectionComments" Runat="server" CssClass="NormalTextBox"></asp:dropdownlist></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plAkismetKey" runat="server" resourcekey="AkismetKey" suffix=":" controlname="txtAkismetKey"></dnn:label></td>
								<td valign="top">
									<asp:TextBox id="txtAkismetKey" Runat="server" CssClass="NormalTextBox" MaxLength="50" />
								</td>
							</tr>
						</table>
						<br />
						<asp:PlaceHolder ID="phContentSharing" runat="server">
						<dnn:sectionhead id="dshContentSharing" cssclass="Head" runat="server" text="Content Sharing Settings" resourcekey="ContentSharingSettings"
							section="tblContentSharing" isexpanded="False" />
						<table id="tblContentSharing" cellspacing="2" cellpadding="2" summary="Content Sharing Design Table"
							border="0" runat="server">
							<tr>
							    <td colspan="2"><asp:Label ID="lblContentSharing" runat="server" EnableViewState="false" ResourceKey="ContentSharing" CssClass="Normal" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plAddArticleInstances" resourcekey="AddArticleInstances" 
										runat="server" controlname="drpContentSharingPortals"></dnn:label></td>
								<td valign="top">
								    <asp:DropDownList ID="drpContentSharingPortals" runat="server" CssClass="NormalTextBox" />
                                    <asp:LinkButton ID="cmdContentSharingAdd" runat="server" ResourceKey="Add" CssClass="CommandButton" />
                                    <asp:Label ID="lblContentSharingNoneAvailable" runat="server" CssClass="Normal" ResourceKey="ContentSharingNoneAvail" EnableViewState="false" Visible="false" />
                                </td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plAvailableArticleInstances" resourcekey="AvailableArticleInstances" 
										runat="server" controlname="drpContentSharingPortals"></dnn:label></td>
								<td valign="top">
								    <asp:datagrid id="grdContentSharing" Border="0" CellPadding="4" CellSpacing="0" Width="100%" AutoGenerateColumns="false" runat="server" summary="Conten Sharing Design Table" GridLines="None">
	                                    <Columns>
		                                    <asp:BoundColumn HeaderText="Portal" DataField="PortalTitle" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" />
		                                    <asp:BoundColumn HeaderText="Module" DataField="ModuleTitle" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" />
	                                        <asp:TemplateColumn ItemStyle-Width="50" HeaderText="Remove" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold">
			                                    <ItemTemplate>
				                                    <asp:LinkButton ID="cmdDelete" runat="server" ResourceKey="Remove" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"LinkedModuleID") %>' />
			                                    </ItemTemplate>
		                                    </asp:TemplateColumn>
	                                    </Columns>
                                    </asp:datagrid>
                                    <asp:Label ID="lblNoContentSharing" Runat="server" CssClass="Normal" ResourceKey="NoContentSharing" />
                                </td>
							</tr>
						</table>
						<br />
						</asp:PlaceHolder>
						<dnn:sectionhead id="dshFileSettings" cssclass="Head" runat="server" text="File Settings" resourcekey="FileSettings"
							section="tblFile" isexpanded="False" />
						<table id="tblFile" cellspacing="2" cellpadding="2" summary="File Design Table"
							border="0" runat="server">
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plDefaultFileFolder" runat="server" controlname="drpDefaultFileFolder"></dnn:label></td>
								<td valign="top"><asp:DropDownList id="drpDefaultFileFolder" Runat="server" width="250px" CssClass="NormalTextBox" /></td>
							</tr>
                            <tr>
                                <td class="SubHead" width="200"><dnn:label id="plEnablePortalFiles" runat="server" resourcekey="EnablePortalFiles" helpkey="EnablePortalFilesHelp"
                                                                           controlname="chkEnablePortalFiles"></dnn:label></td>
                                <td valign="top"><asp:checkbox id="chkEnablePortalFiles" Runat="server"></asp:checkbox></td>
                            </tr>
						</table>	
						<br />
						<dnn:sectionhead id="dshFilter" cssclass="Head" runat="server" text="Filter Settings" section="tblFilter"
							resourceKey="FilterSettings" isexpanded="False" />
						<table id="tblFilter" cellspacing="2" cellpadding="2" summary="Filter Details Design Table"
							border="0" runat="server">
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plMaxArticles" runat="server" resourcekey="MaxArticles" suffix=":" controlname="txtMaxArticles"></dnn:label></td>
								<td valign="top">
									<asp:TextBox id="txtMaxArticles" Runat="server" CssClass="NormalTextBox" MaxLength="8" />
									<asp:CompareValidator id="valMaxArticlesIsValid" runat="server" CssClass="NormalRed" ErrorMessage="Must be a valid number e.g. 10, 20"
										Display="Dynamic" ResourceKey="valMaxArticlesIsValid.ErrorMessage" ControlToValidate="txtMaxArticles"
										Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
								</td>
							</tr>
	                        <tr>
		                        <td class="SubHead" width="200">
			                        <dnn:label id="plMaxAge" runat="server" resourcekey="MaxAge" suffix=":" controlname="txtMaxAge"></dnn:label></td>
		                        <td align="left" width="325">
			                        <asp:textbox id="txtMaxAge" Runat="server" Width="50" CssClass="NormalTextBox"></asp:textbox>&nbsp;<asp:Label ID="lblMaxAge" Runat="server" EnableViewState="False" ResourceKey="MaxAge2.Help"
				                        CssClass="Normal" />
			                        <asp:CompareValidator id="valMaxAgeType" runat="server" ResourceKey="valMaxAgeType.ErrorMessage" ErrorMessage="<br>* Must be a Number"
				                        Display="Dynamic" ControlToValidate="txtMaxAge" Type="Integer" Operator="DataTypeCheck" CssClass="NormalRed"></asp:CompareValidator>
		                        </td>
	                        </tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plCategories" resourcekey="Categories" helpkey="CategoriesHelp" runat="server"
										controlname="chkAllCategories"></dnn:label></td>
								<td valign="top">
								    <asp:RadioButton ID="rdoAllCategories" runat="server" ResourceKey="AllCategories" GroupName="Categories" CssClass="Normal" />
								    <hr />
								    <asp:RadioButton ID="rdoSingleCategory" runat="server" ResourceKey="SingleCategory" GroupName="Categories" CssClass="Normal" />
								    <asp:DropDownList ID="drpCategories" runat="server" CssClass="Normal" DataTextField="NameIndented" 
			                            DataValueField="CategoryID" Width="300px" />
								    <hr />
								    <asp:RadioButton ID="rdoMatchAny" runat="server" ResourceKey="MatchAny" GroupName="Categories" CssClass="Normal" /><br />
								    <asp:RadioButton ID="rdoMatchAll" runat="server" ResourceKey="MatchAll" GroupName="Categories" CssClass="Normal" />
								    <asp:ListBox ID="lstCategories" runat="server" CssClass="Normal" DataTextField="NameIndented" 
			                            DataValueField="CategoryID" Width="300px" SelectionMode="Multiple" Rows="6" />
									<asp:Label ID="lblHoldCtrl" runat="server" ResourceKey="HoldCtrl" CssClass="Normal" />	
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plShowPending" runat="server" controlname="chkShowPending" /></td>
								<td valign="top"><asp:checkbox id="chkShowPending" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plShowFeaturedOnly" resourcekey="ShowFeaturedOnly" helpkey="ShowFeaturedOnlyHelp"
										runat="server" controlname="chkShowFeaturedOnly"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkShowFeaturedOnly" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plShowNotFeaturedOnly" resourcekey="ShowNotFeaturedOnly" helpkey="ShowNotFeaturedOnlyHelp"
										runat="server" controlname="chkShowNotFeaturedOnly"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkShowNotFeaturedOnly" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plShowSecuredOnly" resourcekey="ShowSecuredOnly"
										runat="server" controlname="chkShowSecuredOnly"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkShowSecuredOnly" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plShowNotSecuredOnly" resourcekey="ShowNotSecuredOnly"
										runat="server" controlname="chkShowNotSecuredOnly"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkShowNotSecuredOnly" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
	                            <td colspan="2">
	                                <br />
	                                <dnn:sectionhead id="dshAuthorFilterSettings" runat="server" cssclass="Head" includerule="True" isExpanded="true"
                                        resourcekey="AuthorFilterSettings" section="tblAuthorFilterSettings" text="Template" />
                                    <table id="tblAuthorFilterSettings" runat="server" cellspacing="0" cellpadding="2" width="100%" summary="Author Design Table" border="0">
                	                
							                <tr>
								                <td class="SubHead" width="200"><dnn:label id="plAuthor" resourcekey="Author" runat="server" controlname="cmdSelectAuthor"></dnn:label></td>
								                <td valign="top">
									                <asp:Label ID="lblAuthorFilter" Runat="server" CssClass="Normal" />
									                <asp:LinkButton ID="cmdSelectAuthor" runat="server" ResourceKey="cmdSelectAuthor" CssClass="CommandButton" />
									                <asp:DropDownList ID="ddlAuthor" Runat="server" Visible="False" DataTextField="Username" DataValueField="UserID" />
								                </td>
							                </tr>
                                    <tr>
		                                <td class="SubHead" width="150"><dnn:label id="plQueryStringFilter" resourcekey="plQueryStringFilter" runat="server" controlname="chkQueryStringFilter"></dnn:label></td>
		                                <td valign="top"><asp:checkbox id="chkQueryStringFilter" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
	                                </tr>
	                                <tr>
		                                <td class="SubHead" width="150"><dnn:label id="plQueryStringParam" resourcekey="plQueryStringParam" runat="server" controlname="txtQueryStringParam"></dnn:label></td>
		                                <td valign="top"><asp:textbox id="txtQueryStringParam" Runat="server" Width="150" CssClass="NormalTextBox"></asp:textbox></td>
	                                </tr>
	                                <tr>
		                                <td class="SubHead" width="150"><dnn:label id="plUsernameFilter" runat="server" controlname="chkUsernameFilter"></dnn:label></td>
		                                <td valign="top"><asp:checkbox id="chkUsernameFilter" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
	                                </tr>
	                                <tr>
		                                <td class="SubHead" width="150"><dnn:label id="plUsernameParam" runat="server" controlname="txtUsernameParam"></dnn:label></td>
		                                <td valign="top"><asp:textbox id="txtUsernameParam" Runat="server" Width="150" CssClass="NormalTextBox"></asp:textbox></td>
	                                </tr>
	                                <tr>
		                                <td class="SubHead" width="150"><dnn:label id="plLoggedInUser" runat="server" controlname="chkLoggedInUser"></dnn:label></td>
		                                <td valign="top"><asp:checkbox id="chkLoggedInUser" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
	                                </tr>
                                    </table>
	                            </td>
	                        </tr>
						</table>
						<br />
						<dnn:sectionhead id="dshForm" cssclass="Head" runat="server" text="Form Settings" resourcekey="FormSettings"
							section="tblForm" isexpanded="False" />
						<table id="tblForm" cellspacing="2" cellpadding="2" summary="Form Design Table"
							border="0" runat="server">
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plAuthorSelection" runat="server" controlname="lstAuthorSelection"></dnn:label></td>
								<td valign="top">
									<asp:RadioButtonList ID="lstAuthorSelection" Runat="server" CssClass="Normal" RepeatDirection="Horizontal" />
								</td>
							</tr>
							<tr>
				                <td class="SubHead" width="200"><dnn:label id="plAuthorDefault" resourcekey="AuthorDefault" runat="server" controlname="cmdSelectAuthor"></dnn:label></td>
				                <td valign="top">
				                    <asp:Label ID="lblAuthorDefault" Runat="server" CssClass="Normal" />
									<asp:LinkButton ID="cmdSelectAuthorDefault" runat="server" ResourceKey="cmdSelectAuthor" CssClass="CommandButton" />
									<asp:DropDownList ID="drpAuthorDefault" Runat="server" DataTextField="UserName" Visible="False" DataValueField="UserID" />
				                        
				                </td>
			                </tr>
			                <tr>
								<td class="SubHead" width="200"><dnn:label id="plExpandSummary" runat="server" resourcekey="ExpandSummary"
										controlname="chkExpandSummary"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkExpandSummary" Runat="server"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plTextEditorWidth" runat="server" resourcekey="TextEditorWidth" suffix=":" controlname="txtTextEditorWidth"></dnn:label></td>
								<td valign="top">
									<asp:TextBox id="txtTextEditorWidth" Runat="server" CssClass="NormalTextBox" MaxLength="8" />
									<asp:RequiredFieldValidator id="valEditorWidth" runat="server" CssClass="NormalRed" ErrorMessage="Required"
										Display="Dynamic" ResourceKey="valTextEditorWidth.ErrorMessage" ControlToValidate="txtTextEditorWidth"></asp:RequiredFieldValidator>
									<asp:CustomValidator id="valEditorWidthIsValid" runat="server" CssClass="NormalRed" ErrorMessage="Must be a valid value e.g. 100%, 400"
										Display="Dynamic" ResourceKey="valTextEditorWidthIsValid.ErrorMessage" ControlToValidate="txtTextEditorWidth"></asp:CustomValidator>
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plTextEditorHeight" runat="server" resourcekey="TextEditorHeight" suffix=":"
										controlname="txtTextEditorHeight"></dnn:label></td>
								<td valign="top">
									<asp:TextBox id="txtTextEditorHeight" Runat="server" CssClass="NormalTextBox" MaxLength="8" />
									<asp:RequiredFieldValidator id="valEditorHeight" runat="server" CssClass="NormalRed" ErrorMessage="Required"
										Display="Dynamic" ResourceKey="valEditorHeight.ErrorMessage" ControlToValidate="txtTextEditorHeight"></asp:RequiredFieldValidator>
									<asp:CustomValidator id="valEditorHeightIsvalid" runat="server" CssClass="NormalRed" ErrorMessage="Must be a valid value e.g. 100%, 400"
										Display="Dynamic" ResourceKey="valEditorHeightIsValid.ErrorMessage" ControlToValidate="txtTextEditorHeight"></asp:CustomValidator>
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plTextEditorSummaryMode" resourcekey="TextEditorSummaryMode" runat="server"
										controlname="lstTextEditorSummaryMode"></dnn:label></td>
								<td valign="top">
									<asp:RadioButtonList ID="lstTextEditorSummaryMode" Runat="server" CssClass="Normal" RepeatDirection="Horizontal" />
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plTextEditorSummaryWidth" runat="server" resourcekey="TextEditorSummaryWidth" suffix=":" controlname="txtTextEditorSummaryWidth"></dnn:label></td>
								<td valign="top">
									<asp:TextBox id="txtTextEditorSummaryWidth" Runat="server" CssClass="NormalTextBox" MaxLength="8" />
									<asp:RequiredFieldValidator id="valTextEditorSummaryWidth" runat="server" CssClass="NormalRed" ErrorMessage="Required"
										Display="Dynamic" ResourceKey="valTextEditorSummaryWidth.ErrorMessage" ControlToValidate="txtTextEditorSummaryWidth"></asp:RequiredFieldValidator>
									<asp:CustomValidator id="valTextEditorSummaryWidthIsValid" runat="server" CssClass="NormalRed" ErrorMessage="Must be a valid value e.g. 100%, 400"
										Display="Dynamic" ResourceKey="valTextEditorSummaryWidthIsValid.ErrorMessage" ControlToValidate="txtTextEditorSummaryWidth"></asp:CustomValidator>
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plTextEditorSummaryHeight" runat="server" resourcekey="TextEditorSummaryHeight" suffix=":"
										controlname="txtTextEditorSummaryHeight"></dnn:label></td>
								<td valign="top">
									<asp:TextBox id="txtTextEditorSummaryHeight" Runat="server" CssClass="NormalTextBox" MaxLength="8" />
									<asp:RequiredFieldValidator id="valTextEditorSummaryHeight" runat="server" CssClass="NormalRed" ErrorMessage="Required"
										Display="Dynamic" ResourceKey="valTextEditorSummaryHeight.ErrorMessage" ControlToValidate="txtTextEditorSummaryHeight"></asp:RequiredFieldValidator>
									<asp:CustomValidator id="valTextEditorSummaryHeightIsValid" runat="server" CssClass="NormalRed" ErrorMessage="Must be a valid value e.g. 100%, 400"
										Display="Dynamic" ResourceKey="valTextEditorSummaryHeightIsValid.ErrorMessage" ControlToValidate="txtTextEditorSummaryHeight"></asp:CustomValidator>
								</td>
							</tr>
						</table>
						<br />
						<dnn:sectionhead id="dshImage" cssclass="Head" runat="server" text="Image Settings" resourcekey="ImageSettings"
							section="tblImage" isexpanded="False" />
						<table id="tblImage" cellspacing="2" cellpadding="2" summary="Image Design Table"
							border="0" runat="server">
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plIncludeJQuery" runat="server" controlname="chkIncludeJQuery"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkIncludeJQuery" Runat="server" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plJQueryPath" resourcekey="jQueryPath" runat="server" controlname="txtJQueryPath"></dnn:label></td>
								<td valign="top"><asp:TextBox id="txtJQueryPath" Runat="server" CssClass="NormalTextBox" Width="250px" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plEnableImagesUpload" runat="server" resourcekey="EnableImagesUpload" controlname="chkEnableImagesUpload"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkEnableImagesUpload" Runat="server"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plEnableImages" runat="server" resourcekey="EnableImages" helpkey="EnableImagesHelp"
										controlname="chkEnablePortalImages"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkEnablePortalImages" Runat="server"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plEnableImagesExternal" runat="server" resourcekey="EnableImagesExternal"
										controlname="chkEnableExternalImages"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkEnableExternalImages" Runat="server"></asp:checkbox></td>
							</tr>	
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plDefaultImageFolder" runat="server" controlname="drpDefaultImageFolder"></dnn:label></td>
								<td valign="top"><asp:DropDownList id="drpDefaultImageFolder" Runat="server" width="250px" CssClass="NormalTextBox" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plResizeImages" runat="server" controlname="chkResizeImages" /></td>
								<td valign="top"><asp:checkbox id="chkResizeImages" Runat="server"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plImageMaxWidth" runat="server" suffix=":" controlname="txtImageMaxWidth" /></td>
								<td valign="top">
									<asp:TextBox id="txtImageMaxWidth" Runat="server" CssClass="NormalTextBox" MaxLength="8" />
									<asp:RequiredFieldValidator id="valImageMaxWidth" runat="server" CssClass="NormalRed" ErrorMessage="Required"
										Display="Dynamic" ResourceKey="valImageMaxWidth.ErrorMessage" ControlToValidate="txtImageMaxWidth"></asp:RequiredFieldValidator>
									<asp:CompareValidator id="valImageMaxWidthIsNumber" runat="server" CssClass="NormalRed" ErrorMessage="Must be a valid number e.g. 100, 200"
										Display="Dynamic" ResourceKey="valImageMaxWidthIsNumber.ErrorMessage" ControlToValidate="txtImageMaxWidth"
										Type="Integer" Operator="DataTypeCheck" />
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plImageMaxHeight" runat="server" suffix=":"
										controlname="txtImageMaxHeight"></dnn:label></td>
								<td valign="top">
									<asp:TextBox id="txtImageMaxHeight" Runat="server" CssClass="NormalTextBox" MaxLength="8" />
									<asp:RequiredFieldValidator id="valImageMaxHeight" runat="server" CssClass="NormalRed" ErrorMessage="Required"
										Display="Dynamic" ResourceKey="valImageMaxHeight.ErrorMessage" ControlToValidate="txtImageMaxHeight"></asp:RequiredFieldValidator>
									<asp:CompareValidator id="valImageMaxHeightIsNumber" runat="server" CssClass="NormalRed" ErrorMessage="Must be a valid number e.g. 100, 200"
										Display="Dynamic" ResourceKey="valImageMaxHeightIsNumber.ErrorMessage" ControlToValidate="txtImageMaxHeight"
										Type="Integer" Operator="DataTypeCheck" />
								</td>
							</tr>
                            <tr>
	                            <td class="SubHead" width="200"><dnn:label id="plThumbnailType" runat="server" suffix=":" controlname="rdoThumbnailType"></dnn:label></td>
	                            <td><asp:radiobuttonlist id="rdoThumbnailType" Runat="server" Width="250px" CssClass="Normal" AutoPostBack="False" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:radiobuttonlist></td>
                            </tr>
                            <tr>
	                            <td class="SubHead" width="200"><dnn:label id="plUseWatermark" runat="server" resourcekey="UseWatermark" suffix=":" controlname="chkUseWatermark"></dnn:label></td>
                                <td><asp:CheckBox id="chkUseWatermark" Runat="server" CssClass="NormalTextBox"></asp:CheckBox></td>
                            </tr>
                            <tr>
	                            <td class="SubHead" width="200"><dnn:label id="plWatermarkText" runat="server" resourcekey="WatermarkText" suffix=":" controlname="txtWatermarkText"></dnn:label></td>
                                <td><asp:textbox id="txtWatermarkText" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox></td>
                            </tr>
                            <tr>
	                            <td class="SubHead" width="200"><dnn:label id="plWatermarkImage" runat="server" resourcekey="WatermarkImage" suffix=":" controlname="ctlWatermarkImage"></dnn:label></td>
                                <td><dnn:url id="ctlWatermarkImage" runat="server" width="275" Required="False" showtrack="False" shownewwindow="False"
						                                showlog="False" urltype="F" showUrls="False" showfiles="True" showtabs="False"></dnn:url></td>
                            </tr>
                            <tr>
	                            <td class="SubHead" width="200"><dnn:label id="plWatermarkPosition" runat="server" resourcekey="WatermarkPosition" suffix=":" controlname="drpWatermarkPosition"></dnn:label></td>
                                <td><asp:DropDownList id="drpWatermarkPosition" Runat="server" CssClass="NormalTextBox" width="250px"></asp:DropDownList></td>
                            </tr>
						</table>	
						<br />
						<dnn:sectionhead id="dshNotification" cssclass="Head" runat="server" text="Notification Settings"
							section="tblNotification" resourceKey="NotificationSettings" isexpanded="False" />
						<table id="tblNotification" cellspacing="2" cellpadding="2" summary="Notification Details Design Table"
							border="0" runat="server">
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plNotifySubmission" resourcekey="NotifySubmission" helpkey="NotifySubmissionHelp"
										runat="server" controlname="chkNotifySubmission"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkNotifySubmission" Runat="server"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plNotifySubmissionEmail" resourcekey="NotifySubmissionEmail" helpkey="NotifySubmissionEmailHelp"
										runat="server" controlname="txtSubmissionEmail"></dnn:label></td>
								<td valign="top"><asp:textbox id="txtSubmissionEmail" runat="server" cssclass="NormalTextBox" size="25" maxlength="175"></asp:textbox>
									</td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plNotifyApproval" resourcekey="NotifyApproval" helpkey="NotifyApprovalHelp"
										runat="server" controlname="chkNotifyApproval"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkNotifyApproval" Runat="server"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plNotifyComment" resourcekey="NotifyComment" helpkey="NotifyCommentHelp" runat="server"
										controlname="chkNotifyComment"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkNotifyComment" Runat="server"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plNotifyCommentEmail" resourcekey="plNotifyCommentEmail" runat="server" controlname="txtNotifyCommentEmail"></dnn:label></td>
								<td valign="top">
								    <asp:textbox id="txtNotifyCommentEmail" runat="server" cssclass="NormalTextBox" size="25" maxlength="175"></asp:textbox>
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plNotifyCommentApproval" resourcekey="NotifyCommentApproval" runat="server"
										controlname="chkNotifyCommentApproval"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkNotifyCommentApproval" Runat="server"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plNotifyCommentApprovalEmail" resourcekey="NotifyCommentApprovalEmail" runat="server" controlname="txtNotifyCommentEmail"></dnn:label></td>
								<td valign="top">
								    <asp:textbox id="txtNotifyCommentApprovalEmail" runat="server" cssclass="NormalTextBox" size="25" maxlength="175"></asp:textbox>
								</td>
							</tr>
						</table>
						<br />
						<dnn:sectionhead id="dshRelated" cssclass="Head" runat="server" text="Related Settings"
							section="tblRelated" resourceKey="RelatedSettings" isexpanded="False" />
						<table id="tblRelated" cellspacing="2" cellpadding="2" summary="Related Details Design Table"
							border="0" runat="server">
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plRelatedMode" runat="server" suffix=":" controlname="lstRelatedMode"></dnn:label></td>
								<td valign="top"><asp:RadioButtonList ID="lstRelatedMode" Runat="server" CssClass="Normal" RepeatDirection="Vertical" /></td>
							</tr>
						</table>
						<br />
						<dnn:sectionhead id="dshSecurity" cssclass="Head" runat="server" text="Security Settings" section="tblSecurity"
							resourceKey="SecuritySettings" isexpanded="False" />
						<table id="tblSecurity" cellspacing="2" cellpadding="2" summary="Security Details Design Table"
							border="0" runat="server">
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plRoleGroup" resourcekey="RoleGroup" runat="server" controlname="drpSecurityRoleGroups"></dnn:label></td>
								<td valign="top">
								    <asp:DropDownList ID="drpSecurityRoleGroups" runat="server" DataValueField="RoleGroupID" DataTextField="RoleGroupName" AutoPostBack="true" />
								</td>
							</tr>
							<tr>
								<td class="SubHead" colspan="2"><dnn:label id="plBasicSettings" resourcekey="BasicSettings" runat="server"></dnn:label></td>
							</tr>
							<tr>
							    <td colspan="2" align="left">
			                        <asp:DataGrid ID="grdBasicPermissions" Runat="server" AutoGenerateColumns="False" ItemStyle-CssClass="Normal"
				                        ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-VerticalAlign="Bottom" HeaderStyle-HorizontalAlign="Center"
				                        HeaderStyle-CssClass="NormalBold" CellSpacing="0" CellPadding="0" GridLines="None" BorderWidth="1"
				                        BorderStyle="None" DataKeyField="Value">
				                        <Columns>
					                        <asp:TemplateColumn>
						                        <ItemStyle HorizontalAlign="Left" Wrap="False"/>
						                        <ItemTemplate>
							                        <%# DataBinder.Eval(Container.DataItem, "Text") %>
						                        </ItemTemplate>
					                        </asp:TemplateColumn>
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblSubmit" Runat="server" EnableViewState="False" ResourceKey="Submit" />&nbsp;
							                        <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkSubmit');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkSubmit" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>	
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblSecure" Runat="server" EnableViewState="False" ResourceKey="Secure" />&nbsp;
							                        <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkSecure');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkSecure" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>	
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblAutoSecure" Runat="server" EnableViewState="False" ResourceKey="AutoSecure" />&nbsp;
							                        <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkAutoSecure');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkAutoSecure" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>	
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblApprove" Runat="server" EnableViewState="False" ResourceKey="Approve" />&nbsp;
							                        <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkApprove');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkApprove" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>	
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblAutoApprove" Runat="server" EnableViewState="False" ResourceKey="AutoApprove" />&nbsp;
							                        <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkAutoApproveArticle');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkAutoApproveArticle" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>	
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblAutoApproveComment" Runat="server" EnableViewState="False" ResourceKey="AutoApproveComment" /><br />
							                        <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkAutoApproveComment');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkAutoApproveComment" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>	
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblFeature" Runat="server" EnableViewState="False" ResourceKey="Feature" />&nbsp;
							                        <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkFeature');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkFeature" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>	
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblAutoFeature" Runat="server" EnableViewState="False" ResourceKey="AutoFeature" />&nbsp;
							                        <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkAutoFeature');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkAutoFeature" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>	
				                        </Columns>
			                        </asp:DataGrid>
							    </td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plSecureUrl" runat="server" resourcekey="SecureUrl" suffix=":" controlname="txtSecureUrl"></dnn:label></td>
								<td valign="top">
									<asp:TextBox id="txtSecureUrl" Runat="server" CssClass="NormalTextBox" Width="300px" />
								</td>
							</tr>
							<tr>
								<td class="SubHead" colspan="2"><dnn:label id="plFormSettings" resourcekey="FormSettings" runat="server"></dnn:label></td>
							</tr>
							<tr>
							    <td colspan="2" align="left">
			                        <asp:DataGrid ID="grdFormPermissions" Runat="server" AutoGenerateColumns="False" ItemStyle-CssClass="Normal"
				                        ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-VerticalAlign="Bottom" HeaderStyle-HorizontalAlign="Center"
				                        HeaderStyle-CssClass="NormalBold" CellSpacing="0" CellPadding="0" GridLines="None" BorderWidth="1"
				                        BorderStyle="None" DataKeyField="Value">
				                        <Columns>
					                        <asp:TemplateColumn>
						                        <ItemStyle HorizontalAlign="Left" Wrap="False"/>
						                        <ItemTemplate>
							                        <%# DataBinder.Eval(Container.DataItem, "Text") %>
						                        </ItemTemplate>
					                        </asp:TemplateColumn>
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblCategories" Runat="server" EnableViewState="False" ResourceKey="Category" /><br />
							                        <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkCategories');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkCategories" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>	
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblExcerpt" Runat="server" EnableViewState="False" ResourceKey="Excerpt" /><br />
							                        <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkExcerpt');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkExcerpt" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>	
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblImage" Runat="server" EnableViewState="False" ResourceKey="Image" /><br />
							                        <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkImage');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkImage" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>	
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblFile" Runat="server" EnableViewState="False" ResourceKey="File" /><br />
							                        <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkFile');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkFile" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>	
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblLink" Runat="server" EnableViewState="False" ResourceKey="Link" /><br />
							                        <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkLink');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkLink" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>	
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblPublishDate" Runat="server" EnableViewState="False" ResourceKey="PublishDate" /><br />
							                        <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkPublishDate');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkPublishDate" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>	
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblExpiryDate" Runat="server" EnableViewState="False" ResourceKey="ExpiryDate" /><br />
							                        <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkExpiryDate');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkExpiryDate" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>	
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblMeta" Runat="server" EnableViewState="False" ResourceKey="Meta" /><br />
							                        <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkMeta');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkMeta" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblCustom" Runat="server" EnableViewState="False" ResourceKey="Custom" /><br />
							                        <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkCustom');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkCustom" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>	
				                        </Columns>
			                        </asp:DataGrid>
			                        <asp:Label ID="lblFormSettingsHelp" runat="Server" CssClass="Normal" ResourceKey="FormSettingsHelp" EnableViewState="False" />
							    </td>
							</tr>
							<tr runat="server" id="trAdminSettings1">
								<td class="SubHead" colspan="2"><dnn:label id="plAdminSettings" resourcekey="AdminSettings" runat="server"></dnn:label></td>
							</tr>
							<tr runat="server" id="trAdminSettings2">
							    <td colspan="2" align="left">
			                        <asp:DataGrid ID="grdAdminPermissions" Runat="server" AutoGenerateColumns="False" ItemStyle-CssClass="Normal"
				                        ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
				                        HeaderStyle-CssClass="NormalBold" CellSpacing="0" CellPadding="0" GridLines="None" BorderWidth="1"
				                        BorderStyle="None" DataKeyField="Value">
				                        <Columns>
					                        <asp:TemplateColumn>
						                        <ItemStyle HorizontalAlign="Left" Wrap="False"/>
						                        <ItemTemplate>
							                        <%# DataBinder.Eval(Container.DataItem, "Text") %>
						                        </ItemTemplate>
					                        </asp:TemplateColumn>
					                        <asp:TemplateColumn>
						                        <HeaderTemplate>
							                        &nbsp;
							                        <asp:Label ID="lblSiteTemplates" Runat="server" EnableViewState="False" ResourceKey="SiteTemplates" /><br />
						                            <input id="chkAll" type="checkbox" onclick="SelectAllRoles(this, 'chkSiteTemplates');">
						                        </HeaderTemplate>
						                        <ItemTemplate>
							                        <asp:CheckBox ID="chkSiteTemplates" Runat="server" />
						                        </ItemTemplate>
					                        </asp:TemplateColumn>	
				                        </Columns>
			                        </asp:DataGrid>
							    </td>
							</tr>
						</table>
						<br />
						<dnn:sectionhead id="dshSEOSettings" cssclass="Head" runat="server" text="SEO Settings" resourcekey="SEOSettings"
							section="tblSEO" isexpanded="False" />
						<table id="tblSEO" cellspacing="2" cellpadding="2" summary="SEO Design Table" border="0" runat="server">
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plTitleReplacement" runat="server" suffix=":" controlname="lstTitleReplacement"></dnn:label></td>
								<td valign="top"><asp:RadioButtonList ID="lstTitleReplacement" Runat="server" CssClass="Normal" RepeatDirection="Horizontal" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plAlwaysShowPageID" runat="server" suffix=":" controlname="chkAlwaysShowPageID"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkAlwaysShowPageID" Runat="server"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plUrlMode" resourcekey="UrlMode" suffix=":" runat="server"
										controlname="lstUrlMode"></dnn:label></td>
								<td valign="top">
									<asp:RadioButtonList ID="lstUrlMode" Runat="server" CssClass="Normal" RepeatDirection="Horizontal" />
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plShorternID" runat="server" resourcekey="ShorternID" suffix=":" controlname="txtTextEditorWidth"></dnn:label></td>
								<td valign="top">
									<asp:TextBox id="txtShorternID" Runat="server" CssClass="NormalTextBox" MaxLength="50" />
									<asp:RequiredFieldValidator id="valShorternID" runat="server" CssClass="NormalRed" ErrorMessage="Required"
										Display="Dynamic" ResourceKey="valShorternID.ErrorMessage" ControlToValidate="txtShorternID"></asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plUseCanonicalLink" runat="server" suffix=":" controlname="chkUseCanonicalLink"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkUseCanonicalLink" Runat="server"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plExpandMetaInformation" runat="server" suffix=":" controlname="chkExpandMetaInformation"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkExpandMetaInformation" Runat="server"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plUniquePageTitles" runat="server" suffix=":" controlname="chkUniquePageTitles"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkUniquePageTitles" Runat="server"></asp:checkbox></td>
							</tr>
						</table>
						<br />
						<dnn:sectionhead id="dshSyndicationSettings" cssclass="Head" runat="server" text="Syndication Settings" resourcekey="SyndicationSettings"
							section="tblSyndication" isexpanded="False" />
						<table id="tblSyndication" cellspacing="2" cellpadding="2" summary="Syndication Design Table" border="0" runat="server">
                            <tr>
								<td class="SubHead" width="200"><dnn:label id="plEnableSyndication" resourcekey="EnableSyndication" helpkey="EnableSyndicationHelp"
										runat="server" controlname="chkEnableSyndication"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkEnableSyndication" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plEnableSyndicationEnclosures" resourcekey="EnableSyndicationEnclosures"
										runat="server" controlname="chkEnableSyndicationEnclosures"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkEnableSyndicationEnclosures" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plSyndicationEnclosureType" runat="server" controlname="chkSyndicationEnclosureType"></dnn:label></td>
								<td valign="top"><asp:RadioButtonList ID="lstSyndicationEnclosureType" Runat="server" CssClass="Normal" RepeatDirection="Horizontal" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plEnableSyndicationHtml" resourcekey="EnableSyndicationHtml" helpkey="EnableSyndicationHtmlHelp"
										runat="server" controlname="chkEnableSyndicationHtml"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkEnableSyndicationHtml" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plSyndicationLinkMode" resourcekey="SyndicationLinkMode" runat="server" controlname="chkEnableSyndicationHtml"></dnn:label></td>
								<td valign="top"><asp:RadioButtonList ID="lstSyndicationLinkMode" Runat="server" CssClass="Normal" RepeatDirection="Horizontal" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plSyndicationSummaryLength" resourcekey="SyndicationSummaryLength" runat="server" controlname="txtSyndicationSummaryLength"></dnn:label></td>
								<td valign="top">
								    <asp:TextBox id="txtSyndicationSummaryLength" Runat="server" CssClass="NormalTextBox" MaxLength="8" Width="60px" />
									<asp:CompareValidator id="valSyndicationSummaryLength" runat="server" CssClass="NormalRed" ErrorMessage="Must be a valid number e.g. 100, 200"
										Display="Dynamic" ResourceKey="valSyndicationSummaryLengthIsValid.ErrorMessage" ControlToValidate="txtSyndicationSummaryLength"
										Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
									<asp:Label ID="lblSyndicationSummaryLengthHelp" runat="server" resourceKey="SyndicationSummaryLengthHelp" CssClass="Normal" />
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plSyndicationMaxCount" resourcekey="SyndicationMaxCount" runat="server" controlname="txtSyndicationMaxCount" /></td>
								<td valign="top">
								    <asp:TextBox id="txtSyndicationMaxCount" Runat="server" CssClass="NormalTextBox" MaxLength="8" Width="60px" />
									<asp:RequiredFieldValidator id="valSyndicationMaxCount" runat="server" CssClass="NormalRed" ErrorMessage="Required"
										Display="Dynamic" ResourceKey="valSyndicationMaxCount.ErrorMessage" ControlToValidate="txtSyndicationMaxCount"></asp:RequiredFieldValidator>
									<asp:CompareValidator id="valSyndicationMaxCountIsValid" runat="server" CssClass="NormalRed" ErrorMessage="Must be a valid number e.g. 100, 200"
										Display="Dynamic" ResourceKey="valSyndicationMaxCountIsValid.ErrorMessage" ControlToValidate="txtSyndicationMaxCount"
										Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plSyndicationImagePath" resourcekey="SyndicationImagePath" runat="server" controlname="txtSyndicationImagePath"></dnn:label></td>
								<td valign="top"><asp:TextBox id="txtSyndicationImagePath" Runat="server" CssClass="NormalTextBox" Width="250px" /></td>
							</tr>
						</table>
						<br />
						<dnn:sectionhead id="dshTwitterSettings" cssclass="Head" runat="server" text="Twitter Settings" resourcekey="TwitterSettings"
							section="tblTwitter" isexpanded="False" />
						<table id="tblTwitter" cellspacing="2" cellpadding="2" summary="Twitter Design Table" border="0" runat="server">
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plTwitterName" 
										runat="server" controlname="txtTwitterName"></dnn:label></td>
								<td valign="top"><asp:TextBox id="txtTwitterName" Runat="server" CssClass="NormalTextBox" Width="250px" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plBitLyLogin" 
										runat="server" controlname="txtBitLyLogin"></dnn:label></td>
								<td valign="top"><asp:TextBox id="txtBitLyLogin" Runat="server" CssClass="NormalTextBox" Width="250px" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plBitLyAPIKey" 
										runat="server" controlname="txtBitLyAPIKey"></dnn:label></td>
								<td valign="top"><asp:TextBox id="txtBitLyAPIKey" Runat="server" CssClass="NormalTextBox" Width="250px" /></td>
							</tr>
						</table>
						<br />
						<dnn:sectionhead id="dsh3rdPartySettings" cssclass="Head" runat="server" text="3rd Party Settings" resourcekey="3rdPartySettings"
							section="tbl3rdParty" isexpanded="False" />
						<table id="tbl3rdParty" cellspacing="2" cellpadding="2" summary="3rd Party Design Table" border="0" runat="server">
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plJournalIntegration" runat="server" controlname="chkJournalIntegration"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkJournalIntegration" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plJournalIntegrationGroups" runat="server" controlname="chkJournalIntegrationGroups"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkJournalIntegrationGroups" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plActiveSocial" runat="server" controlname="chkActiveSocial"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkActiveSocial" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
							<tr>
	                            <td class="SubHead" width="150"><dnn:label id="plActiveSocialSubmissionKey" runat="server" suffix=":" controlname="txtActiveSocialSubmissionKey"></dnn:label></td>
	                            <td>
	                                <asp:TextBox ID="txtActiveSocialSubmissionKey" runat="server" CssClass="NormalTextBox" Width="300" />
	                            </td>
                            </tr>
                            <tr>
	                            <td class="SubHead" width="150"><dnn:label id="plActiveSocialRateKey" runat="server" suffix=":" controlname="txtActiveSocialRateKey"></dnn:label></td>
	                            <td>
	                                <asp:TextBox ID="txtActiveSocialRateKey" runat="server" CssClass="NormalTextBox" Width="300" />
	                            </td>
                            </tr>
                            <tr>
	                            <td class="SubHead" width="150"><dnn:label id="plActiveSocialCommentKey" runat="server" suffix=":" controlname="txtActiveSocialCommentKey"></dnn:label></td>
	                            <td>
	                                <asp:TextBox ID="txtActiveSocialCommentKey" runat="server" CssClass="NormalTextBox" Width="300" />
	                            </td>
                            </tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plSmartThinkerStoryFeed" 
										runat="server" controlname="chkSmartThinkerStoryFeed"></dnn:label></td>
								<td valign="top"><asp:checkbox id="chkSmartThinkerStoryFeed" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<p align="center">
	<asp:linkbutton class="CommandButton" id="cmdUpdate" resourcekey="cmdUpdate" runat="server" text="Update"></asp:linkbutton>&nbsp;&nbsp;
	<asp:linkbutton class="CommandButton" id="cmdCancel" resourcekey="cmdCancel" runat="server" text="Cancel"></asp:linkbutton>
</p>

<article:Header runat="server" id="ucHeader2" SelectedMenu="AdminOptions" MenuPosition="Bottom" />

<script type="text/javascript">
function SelectAllRoles(CheckBoxControl, chkName) 
{
if (CheckBoxControl.checked == true) 
{
var i;
for (i=0; i < document.forms[0].elements.length; i++) 
{
if ((document.forms[0].elements[i].type == 'checkbox') && 
(document.forms[0].elements[i].name.indexOf(chkName) > -1)) 
{
document.forms[0].elements[i].checked = true;
}
}
} 
else 
{
var i;
for (i=0; i < document.forms[0].elements.length; i++) 
{
if ((document.forms[0].elements[i].type == 'checkbox') && 
(document.forms[0].elements[i].name.indexOf(chkName) > -1)) 
{
document.forms[0].elements[i].checked = false;
}
}
}
}
</script>