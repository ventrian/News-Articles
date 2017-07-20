<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucNotAuthenticated.ascx.vb" Inherits="Ventrian.NewsArticles.ucNotAuthenticated" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<article:header id="ucHeader1" SelectedMenu="CurrentArticles" runat="server" MenuPosition="Top"></article:header>
<p><asp:Label ID="lblAuthenticated" Runat="server" CssClass="NormalBold" ResourceKey="Authenticated" /></p>
<ul>
	<li>
		<a href='<%= GetLoginUrl() %>' class="CommandButton">
			<asp:Label ID="lblLogin" Runat="server" ResourceKey="Login" /></a>
	<li>
		<a href='<%= Page.ResolveUrl("~/Default.aspx") %>' class="CommandButton">
			<asp:Label ID="lblHomePage" Runat="server" ResourceKey="HomePage" /></a></li>
</ul>
<article:header id="ucHeader2" SelectedMenu="CurrentArticles" runat="server" MenuPosition="Bottom"></article:header>
