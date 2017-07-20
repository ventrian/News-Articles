<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ViewCurrent.ascx.vb" Inherits="Ventrian.NewsArticles.ViewCurrent" %>
<%@ Register TagPrefix="Ventrian" TagName="Header" Src="ucHeader.ascx" %>
<%@ Register TagPrefix="Ventrian" TagName="Listing" Src="Controls/Listing.ascx"%>
<Ventrian:Header id="ucHeader1" SelectedMenu="CurrentArticles" runat="server" MenuPosition="Top" />
<Ventrian:Listing id="ucListing1" runat="server" />
<Ventrian:Header id="ucHeader2" SelectedMenu="CurrentArticles" runat="server" MenuPosition="Bottom" />