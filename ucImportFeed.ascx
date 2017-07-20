<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucImportFeed.ascx.vb" Inherits="Ventrian.NewsArticles.ucImportFeed" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<article:Header id="ucHeader1" SelectedMenu="AdminOptions" runat="server" MenuPosition="Top"></article:Header>
<table class="Settings" cellspacing="2" cellpadding="2" summary="Import Feed Design Table" border="0" width="100%">
<tr>
	<td width="100%" valign="top">
		<asp:panel id="pnlSettings" runat="server" cssclass="WorkPanel" visible="True">
			<dnn:sectionhead id="dshFeed" cssclass="Head" runat="server" includerule="True" resourcekey="FeedSettings"
				section="tblFeed" text="Feed Settings"></dnn:sectionhead>
			<table id="tblFeed" cellspacing="0" cellpadding="2" width="100%" summary="Feed Settings Design Table"
				border="0" runat="server">
				<tr>
					<td colspan="3">
						<asp:label id="lblFeedSettingsHelp" cssclass="Normal" runat="server" resourcekey="FeedSettingsDescription"
							enableviewstate="False"></asp:label></td>
				</tr>
				<tr valign="top">
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
					<td class="SubHead" width="150">
						<dnn:label id="plTitle" runat="server" resourcekey="Title" suffix=":" controlname="txtTitle"></dnn:label></td>
					<td align="left">
						<asp:textbox id="txtTitle" cssclass="NormalTextBox" runat="server" width="325" columns="30"
							maxlength="255"></asp:textbox>
						<asp:requiredfieldvalidator id="valTitle" cssclass="NormalRed" runat="server" resourcekey="valTitle" display="Dynamic"
							errormessage="<br>You Must Enter a Valid Title" controltovalidate="txtTitle" /></td>
				</tr>
				<tr valign="top">
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
					<td class="SubHead" width="150">
						<dnn:label id="plUrl" runat="server" resourcekey="Url" suffix=":" controlname="txtUrl"></dnn:label></td>
					<td align="left">
						<asp:textbox id="txtUrl" cssclass="NormalTextBox" runat="server" width="325" columns="30"
							maxlength="255"></asp:textbox>
						<asp:requiredfieldvalidator id="valUrl" cssclass="NormalRed" runat="server" resourcekey="valUrl" display="Dynamic"
							errormessage="<br>You Must Enter a Valid Url" controltovalidate="txtUrl" /></td>
				</tr>
				<tr valign="top">
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
					<td class="SubHead" width="150">
						<dnn:label id="plIsActive" runat="server" resourcekey="IsActive" suffix=":" controlname="chkIsActive"></dnn:label></td>
					<td align="left">
						<asp:CheckBox ID="chkIsActive" runat="server" /></td>
				</tr>
			</table>
			<br />
			<dnn:sectionhead id="dshArticleSettings" cssclass="Head" runat="server" includerule="True" resourcekey="ArticleSettings"
				section="tblArticle" text="Article Settings"></dnn:sectionhead>
            <table id="tblArticle" cellspacing="0" cellpadding="2" width="100%" summary="Article Settings Design Table"
				border="0" runat="server">
				<tr>
					<td colspan="3">
						<asp:label id="plArticleSettings" cssclass="Normal" runat="server" resourcekey="ArticleSettingsDescription"
							enableviewstate="False"></asp:label></td>
				</tr>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
					<td class="SubHead" width="150">
						<dnn:label id="plAutoFeature" runat="server" resourcekey="AutoFeature" suffix=":" controlname="chkAutoFeature"></dnn:label></td>
					<td align="left">
						<asp:CheckBox ID="chkAutoFeature" runat="server" /></td>
				</tr>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
					<td class="SubHead" width="150">
						<dnn:label id="plAutoExpire" runat="server" resourcekey="AutoExpire" suffix=":" controlname="txtAutoExpire"></dnn:label></td>
					<td align="left">
						<asp:TextBox ID="txtAutoExpire" runat="server" CssClass="NormalTextBox" Width="75px" />
					    <asp:DropDownList ID="drpAutoExpire" runat="server" CssClass="NormalTextBox" />	
                    </td>
				</tr>
				<tr>
		            <td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
					<td class="SubHead" width="150"><dnn:label id="plDateMode" resourcekey="DateMode" suffix=":" runat="server"
							controlname="lstDateMode"></dnn:label></td>
					<td valign="top">
						<asp:RadioButtonList ID="lstDateMode" Runat="server" CssClass="Normal" RepeatDirection="Horizontal" RepeatLayout="Flow" />
					</td>
				</tr>
				<tr>
				    <td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
					<td class="SubHead" width="150">
						<dnn:label id="plAuthor" runat="server" resourcekey="Author" suffix=":" controlname="ddlAuthor"></dnn:label></td>
				    <td align="left">
				        <asp:Label ID="lblAuthor" Runat="server" CssClass="Normal" />
				        <asp:LinkButton ID="cmdSelectAuthor" runat="server" ResourceKey="cmdSelectAuthor" CssClass="CommandButton"
					        CausesValidation="False" />
					    <asp:Panel ID="pnlAuthor" runat="server" Visible="false">
                            <asp:TextBox ID="txtAuthor" runat="server" CssClass="NormalTextBox" Width="150px" />
                            <asp:Label ID="lblAuthorUsername" runat="server" ResourceKey="AuthorUsername" CssClass="Normal" />
                            <asp:CustomValidator id="valAuthor" runat="server" CssClass="NormalRed" ErrorMessage="<br />Invalid username."
										Display="Dynamic" ResourceKey="valAuthor.ErrorMessage" ControlToValidate="txtAuthor" />
                        </asp:Panel>				
				    </td>
				</tr>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
					<td class="SubHead" width="150">
						<dnn:label id="plCategories" runat="server" resourcekey="Categories" suffix=":" controlname="lstCategories"></dnn:label></td>
				    <td align="left">
				        <asp:ListBox ID="lstCategories" runat="server" CssClass="Normal" DataTextField="NameIndented" DataValueField="CategoryID" Width="325px" Rows="8" SelectionMode="Multiple" />
			            <asp:Label ID="lblCategories" ResourceKey="CategoriesHelp" runat="server" CssClass="Normal" />
				    </td>
				</tr>
			</table>	
		</asp:panel>
	</td>
</tr>
</table>
<p>
	<asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" cssclass="CommandButton" text="Update"
		borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel"
		causesvalidation="False" borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdDelete" resourcekey="cmdDelete" runat="server" cssclass="CommandButton" text="Delete"
		causesvalidation="False" borderstyle="none" />
</p>
