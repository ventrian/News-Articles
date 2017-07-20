<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ViewSearch.ascx.vb" Inherits="Ventrian.NewsArticles.ViewSearch" %>
<%@ Register TagPrefix="Ventrian" TagName="Header" Src="ucHeader.ascx" %>
<%@ Register TagPrefix="Ventrian" TagName="Listing" Src="Controls/Listing.ascx"%>
<Ventrian:Header id="ucHeader1" SelectedMenu="Search" runat="server" MenuPosition="Top" />
<div align="left" id="articleSearchForm" >
    <h1><asp:Label ID="lblSearch" Runat="server" /></h1>
    <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearch">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="NormalTextBox" />
        <asp:Button ID="btnSearch" runat="server" Text="Search" ResourceKey="Search" />
    </asp:Panel>
</div>
<Ventrian:Listing id="Listing1" runat="server" />
<Ventrian:Header id="ucHeader2" SelectedMenu="Search" runat="server" MenuPosition="Bottom" />
