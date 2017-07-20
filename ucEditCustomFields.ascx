<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucEditCustomFields.ascx.vb" Inherits="Ventrian.NewsArticles.ucEditCustomFields" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<article:Header id="ucHeader1" SelectedMenu="AdminOptions" runat="server" MenuPosition="Top"></article:Header>
<br />
<asp:datagrid id="grdCustomFields" Border="0" CellPadding="4" width="100%" AutoGenerateColumns="false"
	runat="server" summary="Custom Fields Table" BorderStyle="None" BorderWidth="0px"
	GridLines="None">
	<Columns>
		<asp:TemplateColumn ItemStyle-Width="20">
			<ItemTemplate>
				<asp:HyperLink NavigateUrl='<%# GetCustomFieldEditUrl(DataBinder.Eval(Container.DataItem,"CustomFieldID").ToString()) %>' runat="server" ID="Hyperlink1">
				<asp:Image ImageUrl="~/images/edit.gif" AlternateText="Edit" runat="server" ID="Hyperlink1Image" resourcekey="Edit"/></asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn HeaderText="Name" DataField="Name" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px" />
		<asp:BoundColumn HeaderText="Caption" DataField="Caption" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
		<asp:BoundColumn HeaderText="FieldType" DataField="FieldType" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
		<asp:BoundColumn HeaderText="IsRequired" DataField="IsRequired" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" ItemStyle-Width="75px" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" />
		<asp:BoundColumn HeaderText="IsVisible" DataField="IsVisible" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" ItemStyle-Width="75px" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" />
		<asp:TemplateColumn>
			<HeaderTemplate>
				<asp:Label ID="lblSortOrder" Runat="server" ResourceKey="SortOrder.Header" CssClass="NormalBold" />
			</HeaderTemplate>
			<ItemStyle Width="90px" HorizontalAlign="Center" />
			<ItemTemplate>
				<table cellpadding="0" cellspacing="0">
				<tr>
					<td width="16px"><asp:ImageButton ID="btnDown" Runat="server" ImageUrl="~/Images/dn.gif" /></td>
					<td width="16px"><asp:ImageButton ID="btnUp" Runat="server" ImageUrl="~/Images/up.gif" /></td>
				</tr>
				</table>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>
<div align="center"><asp:Label ID="lblNoCustomFields" Runat="server" CssClass="Normal" ResourceKey="NoCustomFields" EnableViewState="False" Visible="False" /></div>
<p align="center">
	<asp:linkbutton id="cmdAddCustomField" resourcekey="AddCustomField" runat="server" cssclass="CommandButton" text="Add Custom Field" causesvalidation="False" borderstyle="none" />
</p>

