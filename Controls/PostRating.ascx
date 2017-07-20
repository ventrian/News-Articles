<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PostRating.ascx.vb" Inherits="Ventrian.NewsArticles.Controls.PostRating" %>
<asp:RadioButtonList ID="lstRating" runat="server" CssClass="Normal" RepeatDirection="Horizontal" AutoPostBack="true" RepeatLayout="Flow" CausesValidation="false">
    <asp:ListItem Value="1">1</asp:ListItem>
    <asp:ListItem Value="2">2</asp:ListItem>
    <asp:ListItem Value="3">3</asp:ListItem>
    <asp:ListItem Value="4">4</asp:ListItem>
    <asp:ListItem Value="5">5</asp:ListItem>
</asp:RadioButtonList>
<asp:Label ID="lblRatingSaved" runat="server" Text="Rating Saved!" CssClass="Normal" EnableViewState="false" Visible="false" />
