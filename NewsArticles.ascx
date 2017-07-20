<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="NewsArticles.ascx.vb" Inherits="Ventrian.NewsArticles.NewsArticles" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnJsInclude runat="server" FilePath="~/desktopmodules/dnnforge - newsarticles/includes/shadowbox/shadowbox.js" />
<dnn:DnnCssInclude runat="server" FilePath="~/desktopmodules/dnnforge - newsarticles/includes/shadowbox/shadowbox.css" />

<div class="NewsArticles">
    <asp:PlaceHolder id="plhControls" runat="Server" />
</div>