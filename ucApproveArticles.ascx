<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucApproveArticles.ascx.vb" Inherits="Ventrian.NewsArticles.ucApproveArticles" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<article:header id=ucHeader1 SelectedMenu="ApproveArticles" runat="server" MenuPosition="Top"></article:Header>
<h2><asp:Label ID="lblMyArticles" runat="server" EnableViewState="false" ResourceKey="ApproveArticles" /></h2>

<asp:datagrid id=grdArticles runat="server" summary="Articles Design Table" EnableViewState="True" AutoGenerateColumns="false" cellspacing="2" CellPadding="2" Border="0" DataKeyField="ArticleID" Width="100%" GridLines="None">
	<Columns>
		<asp:TemplateColumn ItemStyle-Width="20">
			<ItemTemplate>
				<asp:HyperLink NavigateUrl='<%# GetEditUrl(DataBinder.Eval(Container.DataItem,"ArticleID").ToString) %>' runat="server" ID="Hyperlink1">
				<asp:Image ImageUrl="~/images/edit.gif" AlternateText="Edit" runat="server" ID="Hyperlink1Image" resourcekey="Edit"/></asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn ItemStyle-Width="125">
		    <HeaderStyle CssClass="NormalBold"  />
			<ItemStyle CssClass="Normal" />
			<HeaderTemplate>
			    <asp:Label ID="lblCreateDate" runat="server" ResourceKey="CreatedDate.Header" />
			</HeaderTemplate>
			<ItemTemplate>
			    <%#GetAdjustedCreateDate(Container.DataItem)%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn ItemStyle-Width="125">
		    <HeaderStyle CssClass="NormalBold"  />
			<ItemStyle CssClass="Normal" />
			<HeaderTemplate>
			    <asp:Label ID="lblPublishDate" runat="server" ResourceKey="PublishDate.Header" />
			</HeaderTemplate>
			<ItemTemplate>
			    <%#GetAdjustedPublishDate(Container.DataItem)%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
		    <HeaderStyle CssClass="NormalBold"  />
			<ItemStyle CssClass="Normal" />
			<HeaderTemplate>
			    <asp:Label ID="lblTitle" runat="server" ResourceKey="Title.Header" />
			</HeaderTemplate>
			<ItemTemplate>
			    <a href='<%# GetArticleLink(Container.DataItem) %>' target="_blank"><%#DataBinder.Eval(Container.DataItem, "Title")%></a>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn HeaderText="AuthorFullName" DataField="AuthorFullName" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" ItemStyle-Wrap="False" HeaderStyle-Wrap="False" ItemStyle-Width="150" />
		<asp:TemplateColumn ItemStyle-Width="50" HeaderText="Approve" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold">
			<ItemTemplate>
				<asp:CheckBox Runat="server" ID="chkArticle" />
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>
<asp:label id=lblNoArticles Runat="server" CssClass="Normal" Visible="False"></asp:Label>

<dnn:pagingcontrol id=ctlPagingControl runat="server"></dnn:pagingcontrol>
<p align="center">
	<asp:linkbutton id="cmdApproveSelected" resourcekey="cmdApproveSelected" runat="server" cssclass="CommandButton" text="Approve Selected Articles" causesvalidation="False" borderstyle="none" />&nbsp;
	<asp:linkbutton id="cmdApproveAll" resourcekey="cmdApproveAll" runat="server" cssclass="CommandButton" text="Approve All Articles" borderstyle="none" />
</p>
<article:header id=Header1 SelectedMenu="ApproveArticles" runat="server" MenuPosition="Bottom"></article:Header>
