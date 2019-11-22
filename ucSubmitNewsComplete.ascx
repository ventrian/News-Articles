<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucSubmitNewsComplete.ascx.vb" Inherits="Ventrian.NewsArticles.ucSubmitNewsComplete" %>
<%@ Import Namespace="DotNetNuke.Common" %>
<%@ Import Namespace="DotNetNuke.Common.Utilities" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<article:Header runat="server" id="ucHeader1" SelectedMenu="SubmitArticle" MenuPosition="Top" />

<p><asp:label id="lblSubmitComplete" runat="server" cssclass="NormalBold" resourcekey="SubmitComplete" 
								enableviewstate="False"></asp:label></p>

<ul>
	<li><asp:linkbutton cssclass="CommandButton" id="cmdSubmitArticle" resourcekey="cmdSubmitArticle" runat="server"></asp:linkbutton></li>
	<li><asp:linkbutton cssclass="CommandButton" id="cmdViewMyArticles" resourcekey="cmdViewMyArticles" runat="server"></asp:linkbutton></li>
	<li><asp:linkbutton cssclass="CommandButton" id="cmdCurrentArticles" resourcekey="cmdCurrentArticles" runat="server" CausesValidation="False" BorderStyle="none"></asp:linkbutton></li>
</ul>

<article:Header runat="server" id="Header1" SelectedMenu="SubmitArticle" MenuPosition="Bottom" />
