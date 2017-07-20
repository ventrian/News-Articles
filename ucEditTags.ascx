<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucEditTags.ascx.vb" Inherits="Ventrian.NewsArticles.ucEditTags" %>
<%@ Import Namespace="DotNetNuke.Common" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<article:Header id="ucHeader1" SelectedMenu="AdminOptions" runat="server" MenuPosition="Top"></article:Header>

<asp:datagrid id="grdTags" Border="0" CellPadding="4" CellSpacing="0" Width="100%" AutoGenerateColumns="false" EnableViewState="false" runat="server" summary="Pages Design Table" GridLines="None">
	<Columns>
		<asp:TemplateColumn ItemStyle-Width="20">
			<ItemTemplate>
				<asp:HyperLink NavigateUrl='<%# NavigateURL(TabID, "EditTag", "mid=" & ModuleID.ToString(), "TagID=" & DataBinder.Eval(Container.DataItem,"TagID").ToString()) %>' runat="server" ID="Hyperlink1">
				<asp:Image ImageUrl="~/images/edit.gif" AlternateText="Edit" runat="server" ID="Hyperlink1Image" resourcekey="Edit"/></asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn HeaderText="Name" DataField="Name" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" />
	</Columns>
</asp:datagrid>
<asp:Label ID="lblNoTags" Runat="server" CssClass="Normal" />
<p>
	<asp:linkbutton id="cmdAddTag" resourcekey="AddTag" runat="server" cssclass="CommandButton" text="Add Tag" causesvalidation="False" borderstyle="none" />
</p>
