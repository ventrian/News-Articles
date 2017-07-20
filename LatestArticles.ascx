<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="LatestArticles.ascx.vb" Inherits="Ventrian.NewsArticles.LatestArticles" %>
<%@ Register TagPrefix="ventrian" Namespace="Ventrian.NewsArticles.Components.WebControls" Assembly="Ventrian.NewsArticles" %>
<asp:Repeater ID="rptLatestArticles" Runat="server" EnableViewState="False">
	<HeaderTemplate />
	<ItemTemplate />
	<FooterTemplate />
</asp:Repeater>
<asp:DataList ID="dlLatestArticles" Runat="server" EnableViewState="False" RepeatDirection="Horizontal" ItemStyle-VerticalAlign="Top">
	<HeaderTemplate />
	<ItemTemplate />
	<FooterTemplate />
</asp:DataList>
<asp:PlaceHolder ID="phNoArticles" runat="Server" />
<asp:Label Runat="server" EnableViewState="False" ID="lblNotConfigured" ResourceKey="NotConfigured" Visible="False" CssClass="Normal" />
<ventrian:pagingcontrol id="ctlPagingControl" runat="server" Visible="false"></ventrian:pagingcontrol>
