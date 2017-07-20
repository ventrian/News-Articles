<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucMyArticles.ascx.vb" Inherits="Ventrian.NewsArticles.ucMyArticles" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<%@ Register TagPrefix="Ventrian" TagName="Listing" Src="Controls/Listing.ascx"%>
<article:header id=ucHeader1 SelectedMenu="myArticles" runat="server" MenuPosition="Top"></article:header>


<div class="dnnForm" id="tabs-myarticles">
    <div class="dnnRight"><asp:CheckBox ID="chkShowAll" runat="server" AutoPostBack="true" ResourceKey="ShowAll" CssClass="Normal" /></div>
    <ul class="dnnAdminTabNav ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all">
        <li class="ui-state-default ui-corner-top <%= IsSelected(1) %>"><a href="<%= GetModuleLink("MyArticles", 1)%>"><%= LocalizeString("MyArticles") %></a></li>
        <li class="ui-state-default ui-corner-top <%= IsSelected(2) %>"><a href="<%= GetModuleLink("MyArticles", 2)%>"><%= LocalizeString("Unapproved") %></a></li>
        <li class="ui-state-default ui-corner-top <%= IsSelected(3) %>"><a href="<%= GetModuleLink("MyArticles", 3)%>"><%= LocalizeString("Approved")%></a></li>
    </ul>
    <div id="MyArticles" class="dnnClear">
        <asp:datagrid id="grdMyArticles" Width="100%" AutoGenerateColumns="false" EnableViewState="false" runat="server" BorderStyle="None" GridLines="None" CssClass="dnnGrid">
            <headerstyle cssclass="dnnGridHeader" verticalalign="Top"/>
            <itemstyle cssclass="dnnGridItem" horizontalalign="Left" />
            <alternatingitemstyle cssclass="dnnGridAltItem" />
            <columns>
                <asp:templatecolumn>
                    <ItemStyle Width="20" />
                    <itemtemplate>
                      <a href="<%# GetEditUrl(DataBinder.Eval(Container.DataItem, "ArticleID").ToString()) %>"><img src="<%= Page.ResolveUrl("~/images/edit.gif") %>" alt="Edit" width="16" height="16"/></a>
                    </itemtemplate>
                </asp:templatecolumn>
                <asp:templatecolumn>    
                    <HeaderTemplate><%= LocalizeString("Title.Header") %></HeaderTemplate>
                    <itemtemplate>
                      <a href="<%# GetArticleLink(Container.DataItem) %>"><%#Eval("Title")%></a>
                    </itemtemplate>
                </asp:templatecolumn>
                <asp:boundcolumn DataField="AuthorDisplayName" HeaderText="AuthorFullName" ItemStyle-Width="175" />
                <asp:templatecolumn>    
                    <ItemStyle Width="135" />
                    <HeaderTemplate><%= LocalizeString("CreatedDate.Header") %></HeaderTemplate>
                    <itemtemplate>
                      <%#GetAdjustedCreateDate(Container.DataItem)%>
                    </itemtemplate>
                </asp:templatecolumn>
                <asp:templatecolumn>    
                    <ItemStyle Width="135" />
                    <HeaderTemplate><%= LocalizeString("PublishDate.Header") %></HeaderTemplate>
                    <itemtemplate>
                      <%#GetAdjustedPublishDate(Container.DataItem)%>
                    </itemtemplate>
                </asp:templatecolumn>
            </columns>
        </asp:datagrid>
        <dnn:pagingcontrol id="ctlPagingControl" runat="server" visible="false"></dnn:pagingcontrol>

        <asp:PlaceHolder ID="phNoArticles" EnableViewState="false" runat="server" Visible="false">
            <div class="dnnFormMessage dnnFormInfo"><%= LocalizeString("NoArticlesMessage") %></div>
        </asp:PlaceHolder>
    </div>
</div>   
    
<article:header id=Header1 SelectedMenu="myArticles" runat="server" MenuPosition="Bottom"></article:header>
