<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucEditCategories.ascx.vb" Inherits="Ventrian.NewsArticles.ucEditCategories" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="HelpButton" Src="~/controls/HelpButtonControl.ascx" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<article:Header id="ucHeader1" SelectedMenu="AdminOptions" runat="server" MenuPosition="Top"></article:Header>
<br />
<table cellspacing="0" cellpadding="0" width="600" summary="Category Sort Order Design Table">
<tr vAlign="top">
	<td class="SubHead" width="150" height="30"><dnn:label id="plParentCategory" runat="server" resourcekey="ParentCategory" controlname="drpParentCategory" suffix=":"></dnn:label></td>
	<td><asp:DropDownList ID="drpParentCategory" Runat="server" DataTextField="NameIndented" DataValueField="CategoryID" AutoPostBack="True" /></td>
</tr>
<tr valign="top">
	<td class="SubHead" width="150" height="30"><dnn:label id="plChildCategories" runat="server" resourcekey="ChildCategories" controlname="lstChildCategories" suffix=":"></dnn:label></td>
	<td>
		<asp:Label ID="lblNoCategories" Runat="server" CssClass="Normal" Visible="False" ResourceKey="NoCategories" />
		<asp:Panel ID="pnlSortOrder" Runat="server">
			<table cellSpacing="0" cellPadding="0" width="450">
			<tr>
				<td>	
					<asp:listbox id="lstChildCategories" runat="server" rows="22" DataTextField="Name" DataValueField="CategoryID" cssclass="NormalTextBox" width="290px"></asp:listbox>
					<asp:Label ID="lblCategoryUpdated" Runat="server" CssClass="NormalBold" Visible="False" ResourceKey="CategoryUpdated" EnableViewState="False" />
				</td>
				<td>&nbsp;</td>
				<td width="150px" valign="top">
					<table summary="Tabs Design Table">
						<tr>
							<td colspan="2" valign="top" class="SubHead">
								<asp:label id="lblMoveCategory" runat="server" resourcekey="MoveCategory">Move Category</asp:label>
								<hr noshade size="1">
							</td>
						</tr>
						<tr>
							<td valign="top" width="10%">
								<asp:imagebutton id="cmdUp" resourcekey="cmdUp.Help" runat="server" alternatetext="Move Category Up In Current Level" commandname="up" imageurl="~/images/up.gif"></asp:imagebutton>
							</td>
							<td valign="top" width="90%">
								<dnn:HelpButton id="hbtnUpHelp" resourcekey="cmdUp" runat="server" /></dnn:helpbutton>
							</td>
						</tr>
						<tr>
							<td valign="top" width="10%">
								<asp:imagebutton id="cmdDown" resourcekey="cmdDown.Help" runat="server" alternatetext="Move Category Down In Current Level" commandname="down" imageurl="~/images/dn.gif"></asp:imagebutton>
							</td>
							<td valign="top" width="90%">
								<dnn:helpbutton id="hbtnDownHelp" resourcekey="cmdDown" runat="server" /></dnn:helpbutton>
							</td>
						</tr>
						<tr>
							<td colspan="2" height="25">&nbsp;</td>
						</tr>
						<tr>
							<td colspan="2" valign="top" class="SubHead">
								<asp:label id="lblActions" runat="server" resourcekey="Actions">Actions</asp:label>
								<hr noshade size="1">
							</td>
						</tr>
						<tr>
							<td valign="top" width="10%">
								<asp:imagebutton id="cmdEdit" resourcekey="cmdEdit.Help" runat="server" alternatetext="Edit Category" imageurl="~/images/edit.gif"></asp:imagebutton>
							</td>
							<td valign="top" width="90%">
								<dnn:helpbutton id="hbtnEditHelp" resourcekey="cmdEdit" runat="server" /></dnn:helpbutton>
							</td>
						</tr>
						<tr>
							<td valign="top" width="10%">
								<asp:imagebutton id="cmdView" resourcekey="cmdView.Help" runat="server" alternatetext="View Category" imageurl="~/images/view.gif"></asp:imagebutton>
							</td>
							<td valign="top" width="90%">
								<dnn:helpbutton id="hbtnViewHelp" resourcekey="cmdView" runat="server" /></dnn:helpbutton>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			</table>
		</asp:Panel>
	</td>
</tr>
</table>

<p>
    <asp:linkbutton id="cmdUpdateSortOrder" resourcekey="cmdUpdateSortOrder" runat="server" cssclass="CommandButton" text="Update Sort Order" borderstyle="none" />&nbsp;<asp:linkbutton id="cmdAddNewCategory" resourcekey="cmdAddNewCategory" runat="server" cssclass="CommandButton" text="Add New Category" borderstyle="none" />
</p>
<article:Header id="ucHeader2" SelectedMenu="AdminOptions" runat="server" MenuPosition="Bottom"></article:Header>
