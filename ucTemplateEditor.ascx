<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucTemplateEditor.ascx.vb" Inherits="Ventrian.NewsArticles.ucTemplateEditor" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<article:header id="ucHeader1" runat="server" SelectedMenu="AdminOptions" MenuPosition="Top"></article:header>
<asp:Label ID="lblUpdated" runat="server" CssClass="Normal" ResourceKey="Updated" EnableViewState="false" Visible="false" />
<table class="Settings" cellSpacing="2" cellPadding="2" summary="Edit Page Design Table"
	border="0" width="100%">
	<tr>
		<td vAlign="top"><asp:panel id="pnlSettings" runat="server" cssclass="WorkPanel" visible="True">
				<dnn:sectionhead id="dshSiteTemplates" runat="server" cssclass="Head" includerule="True" section="tblSiteTemplates"
					text="Site Templates" resourcekey="SiteTemplates"></dnn:sectionhead>
				<TABLE id="tblSiteTemplates" cellSpacing="0" cellPadding="2" summary="Page Settings Design Table"
					border="0" runat="server">
					<TR>
						<TD colSpan="3">
							<asp:label id="lblSiteTemplatesHelp" runat="server" cssclass="Normal" resourcekey="SiteTemplatesDescription"
								enableviewstate="False"></asp:label></TD>
					</TR>
					<TR vAlign="top">
						<TD width="25"></TD>
						<TD class="SubHead" width="150">
							<dnn:label id="plTemplate" runat="server" resourcekey="Template" controlname="drpTemplate"
								suffix=":"></dnn:label></TD>
						<TD align="left">
							<asp:DropDownList id="drpTemplate" Runat="server" CssClass="NormalTextBox" AutoPostBack="True"></asp:DropDownList></TD>
					</TR>
					<TR vAlign="top">
						<TD width="25"></TD>
						<TD class="SubHead" width="150">
							<dnn:label id="plFile" runat="server" resourcekey="File" controlname="drpFile" suffix=":"></dnn:label></TD>
						<TD align="left">
							<asp:DropDownList id="drpFile" Runat="server" CssClass="NormalTextBox" AutoPostBack="True">
								<asp:ListItem>Category.Child.Html</asp:ListItem>
								<asp:ListItem>Category.Html</asp:ListItem>
								<asp:ListItem>Comment.Item.Html</asp:ListItem>
								<asp:ListItem>File.Header.Html</asp:ListItem>
								<asp:ListItem>File.Item.Html</asp:ListItem>
								<asp:ListItem>File.Footer.Html</asp:ListItem>
								<asp:ListItem>Handout.Cover.Html</asp:ListItem>
								<asp:ListItem>Handout.Header.Html</asp:ListItem>
								<asp:ListItem>Handout.Item.Html</asp:ListItem>
								<asp:ListItem>Handout.Footer.Html</asp:ListItem>
								<asp:ListItem>Handout.End.Html</asp:ListItem>
								<asp:ListItem>Image.Header.Html</asp:ListItem>
								<asp:ListItem>Image.Item.Html</asp:ListItem>
								<asp:ListItem>Image.Footer.Html</asp:ListItem>
								<asp:ListItem>Listing.Header.Html</asp:ListItem>
								<asp:ListItem>Listing.Item.Html</asp:ListItem>
								<asp:ListItem>Listing.Featured.Html</asp:ListItem>
								<asp:ListItem>Listing.Footer.Html</asp:ListItem>
								<asp:ListItem>Listing.Empty.Html</asp:ListItem>
								<asp:ListItem>Menu.Item.Html</asp:ListItem>
								<asp:ListItem>Print.Item.Html</asp:ListItem>
								<asp:ListItem>Related.Header.Html</asp:ListItem>
								<asp:ListItem>Related.Item.Html</asp:ListItem>
								<asp:ListItem>Related.Footer.Html</asp:ListItem>
								<asp:ListItem>Rss.Header.Html</asp:ListItem>
								<asp:ListItem>Rss.Item.Html</asp:ListItem>
								<asp:ListItem>Rss.Footer.Html</asp:ListItem>
								<asp:ListItem>Rss.Comment.Header.Html</asp:ListItem>
								<asp:ListItem>Rss.Comment.Item.Html</asp:ListItem>
								<asp:ListItem>Rss.Comment.Footer.Html</asp:ListItem>
								<asp:ListItem>View.Item.Html</asp:ListItem>
								<asp:ListItem>View.Title.Html</asp:ListItem>
								<asp:ListItem>View.Description.Html</asp:ListItem>
								<asp:ListItem>View.Keyword.Html</asp:ListItem>
								<asp:ListItem>View.PageHeader.Html</asp:ListItem>
								<asp:ListItem>Template.css</asp:ListItem>
							</asp:DropDownList></TD>
					</TR>
					<TR>
						<TD width="25"></TD>
						<TD class="SubHead" width="150">
							<dnn:label id="plTemplateText" runat="server" resourcekey="TemplateText" controlname="txtTemplate"
								suffix=":" helpkey="TemplateTextHelp"></dnn:label></TD>
						<TD class="NormalTextBox">
							<asp:textbox id="txtTemplate" runat="server" cssclass="NormalTextBox" width="450px" rows="20"
								textmode="MultiLine"></asp:textbox></TD>
					</TR>
				</TABLE>
				<BR>
				<dnn:sectionhead id="dshTemplateHelp" runat="server" cssclass="Head" includerule="True" section="tblSiteTemplateHelp"
					text="Site Template Help" resourcekey="SiteTemplateHelp" isExpanded="false"></dnn:sectionhead>
				<TABLE id="tblSiteTemplateHelp" cellSpacing="0" cellPadding="2" width="525" summary="Site Template Help Design Table"
					border="0" runat="server">
					<TR>
						<TD colSpan="3">
							<asp:label id="lblTemplateHelp" runat="server" cssclass="Normal" resourcekey="SiteTemplateHelpDescription"
								enableviewstate="False"></asp:label></TD>
					</TR>
				</TABLE>
				<BR>
				<dnn:sectionhead id="dshTemplateNew" runat="server" cssclass="Head" includerule="True" section="tblTemplateNew"
					text="Create New Template" resourcekey="NewTemplate" isExpanded="false"></dnn:sectionhead>
				<TABLE id="tblTemplateNew" cellSpacing="0" cellPadding="2" width="525" summary="New Template Help Design Table"
					border="0" runat="server">
					<TR>
						<TD width="25"></TD>
						<TD class="SubHead" width="150">
							<dnn:label id="plNewTemplateName" runat="server" resourcekey="NewTemplateName" controlname="txtNewTemplate"
								suffix=":" helpkey="NewTemplateNameHelp"></dnn:label></TD>
						<TD class="NormalTextBox" width="325">
							<asp:textbox id="txtNewTemplate" runat="server" cssclass="NormalTextBox" MaxLength="100" Width="200px"></asp:textbox>&nbsp;
							<asp:linkbutton id="cmdCreate" runat="server" cssclass="CommandButton" text="Create" resourcekey="cmdCreate"
								borderstyle="none"></asp:linkbutton>
							<asp:Label ID="lblTemplateCreated" runat="Server" EnableViewState="False" ResourceKey="TemplateCreated" Visible="False" CssClass="NormalBold" />	
						</TD>
					</TR>
				</TABLE>
			</asp:panel></td>
	</tr>
</table>
<p><asp:linkbutton id="cmdUpdate" runat="server" cssclass="CommandButton" resourcekey="cmdUpdate" text="Update"
		borderstyle="none"></asp:linkbutton>&nbsp;
	<asp:linkbutton id="cmdCancel" runat="server" cssclass="CommandButton" resourcekey="cmdCancel" text="Cancel"
		borderstyle="none" causesvalidation="False"></asp:linkbutton></p>

<article:header id="ucHeader2" runat="server" SelectedMenu="AdminOptions" MenuPosition="Bottom"></article:header>
