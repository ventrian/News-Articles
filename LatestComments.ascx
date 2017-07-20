<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="LatestComments.ascx.vb" Inherits="Ventrian.NewsArticles.LatestComments" %>
<asp:Repeater ID="rptLatestComments" Runat="server" EnableViewState="False">
	<HeaderTemplate />
	<ItemTemplate />
	<FooterTemplate />
</asp:Repeater>
<asp:PlaceHolder ID="phNoComments" runat="Server" />
<asp:Label Runat="server" EnableViewState="False" ID="lblNotConfigured" ResourceKey="NotConfigured" Visible="False" CssClass="Normal" />
