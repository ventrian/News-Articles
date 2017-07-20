<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucNotAuthorized.ascx.vb" Inherits="Ventrian.NewsArticles.ucNotAuthorized" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<article:header id="ucHeader1" SelectedMenu="CurrentArticles" runat="server" MenuPosition="Top"></article:header>
<p><asp:Label ID="lblAuthorized" Runat="server" CssClass="NormalBold" ResourceKey="Authorized" /></p>
<ul>
	<li>
		<a href='<%= Page.ResolveUrl("~/Default.aspx") %>' class="CommandButton">
			<asp:Label ID="lblHomePage" Runat="server" ResourceKey="HomePage" /></a></li>
</ul>
<article:header id="ucHeader2" SelectedMenu="CurrentArticles" runat="server" MenuPosition="Bottom"></article:header>
