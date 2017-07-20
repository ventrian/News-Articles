<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Listing.ascx.vb" Inherits="Ventrian.NewsArticles.Controls.Listing" %>
<asp:Repeater ID="rptListing" Runat="server">
	<HeaderTemplate></HeaderTemplate>
	<FooterTemplate></FooterTemplate>
</asp:Repeater>
<asp:PlaceHolder ID="phNoArticles" runat="server" EnableViewState="false"></asp:PlaceHolder>