<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucAdminOptions.ascx.vb" Inherits="Ventrian.NewsArticles.ucAdminOptions" %>
<%@ Register TagPrefix="Ventrian" TagName="Header" Src="ucHeader.ascx" %>
<Ventrian:Header id="ucHeader1" SelectedMenu="AdminOptions" runat="server" MenuPosition="Top" />
<table cellpadding="2" cellspacing="4">
	<tr>
		<td valign="middle">
			<a href='<%= EditArticleUrl("Admin") %>'><img src='<%= ResolveUrl("Images/Admin/MainOptions.gif") %>' border="0">
			</a>
		</td>
		<td>
			<asp:label id="lblMainOptions" resourcekey="MainOptions" cssclass="NormalBold" runat="server"
				enableviewstate="False"></asp:label><br>
			<asp:label id="lblMainOptionsDescription" resourcekey="MainOptionsDescription" cssclass="Normal"
				runat="server" enableviewstate="False"></asp:label>
		</td>
	</tr>
	<tr>
		<td valign="middle">
			<a href='<%= EditArticleUrl("EditCategories") %>'><img src='<%= ResolveUrl("Images/Admin/Categories.gif") %>' border="0">
			</a>
		</td>
		<td>
			<asp:label id="lblCategories" resourcekey="Categories" cssclass="NormalBold" runat="server"
				enableviewstate="False"></asp:label><br>
			<asp:label id="lblCategoriesDescription" resourcekey="CategoriesDescription" cssclass="Normal"
				runat="server" enableviewstate="False"></asp:label>
		</td>
	</tr>
	<tr>
		<td valign="middle">
			<a href='<%= EditArticleUrl("EditCustomFields") %>'><img src='<%= ResolveUrl("Images/Admin/Categories.gif") %>' border="0">
			</a>
		</td>
		<td>
			<asp:label id="lblCustomFields" resourcekey="CustomFields" cssclass="NormalBold" runat="server"
				enableviewstate="False"></asp:label><br />
			<asp:label id="lblCustomFieldsDescription" resourcekey="CustomFieldsDescription" cssclass="Normal"
				runat="server" enableviewstate="False"></asp:label>
		</td>
	</tr>
	<tr>
		<td valign="middle">
			<a href='<%= EditArticleUrl("ImportFeeds") %>'><img src='<%= ResolveUrl("Images/Admin/Categories.gif") %>' border="0">
			</a>
		</td>
		<td>
			<asp:label id="lblImportFeeds" resourcekey="ImportFeeds" cssclass="NormalBold" runat="server"
				enableviewstate="False" /><br />
			<asp:label id="lblImportFeedsDescription" resourcekey="ImportFeedsDescription" cssclass="Normal"
				runat="server" enableviewstate="False" />
		</td>
	</tr>
	<tr>
		<td valign="middle">
			<a href='<%= EditArticleUrl("EditTags") %>'><img src='<%= ResolveUrl("Images/Admin/Categories.gif") %>' border="0">
			</a>
		</td>
		<td>
			<asp:label id="lblTags" resourcekey="Tags" cssclass="NormalBold" runat="server"
				enableviewstate="False"></asp:label><br>
			<asp:label id="lblTagsDescription" resourcekey="TagsDescription" cssclass="Normal"
				runat="server" enableviewstate="False"></asp:label>
		</td>
	</tr>
	<tr>
		<td valign="middle">
			<a href='<%= EditArticleUrl("EmailTemplates") %>'><img src='<%= ResolveUrl("Images/Admin/Templates.gif") %>' border="0">
			</a>
		</td>
		<td>
			<asp:label id="lblEmailTemplates" resourcekey="EmailTemplates" cssclass="NormalBold" runat="server"
				enableviewstate="False"></asp:label><br>
			<asp:label id="lblEmailTemplatesDescription" resourcekey="EmailTemplatesDescription" cssclass="Normal"
				runat="server" enableviewstate="False"></asp:label>
		</td>
	</tr>
	<tr runat="server" id="trSiteTemplates">
		<td valign="middle">
			<a href='<%= EditArticleUrl("SiteTemplates") %>'><img src='<%= ResolveUrl("Images/Admin/Templates.gif") %>' border="0">
			</a>
		</td>
		<td>
			<asp:label id="lblSiteTemplates" resourcekey="SiteTemplates" cssclass="NormalBold" runat="server"
				enableviewstate="False"></asp:label><br>
			<asp:label id="lblSiteTemplatesDescription" resourcekey="SiteTemplatesDescription" cssclass="Normal"
				runat="server" enableviewstate="False"></asp:label>
		</td>
	</tr>
</table>
<Ventrian:Header id="ucHeader2" SelectedMenu="AdminOptions" runat="server" MenuPosition="Bottom" />

<asp:LinkButton id="cmdImport" runat="server" Text="import" Visible="false" />
