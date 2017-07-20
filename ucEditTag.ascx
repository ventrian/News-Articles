<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucEditTag.ascx.vb" Inherits="Ventrian.NewsArticles.ucEditTag" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<article:Header id="ucHeader1" SelectedMenu="AdminOptions" runat="server" MenuPosition="Top"></article:Header>
<table class="Settings" cellspacing="2" cellpadding="2" summary="Edit Tag Design Table"
	border="0" width="100%">
	<tr>
		<td width="100%" valign="top">
			<asp:panel id="pnlSettings" runat="server" cssclass="WorkPanel" visible="True">
				<dnn:sectionhead id="dshTag" cssclass="Head" runat="server" includerule="True" resourcekey="TagSettings"
					section="tblTag" text="Tag Settings"></dnn:sectionhead>
				<TABLE id="tblTag" cellSpacing="0" cellPadding="2" width="100%" summary="Tag Settings Design Table"
					border="0" runat="server">
					<TR>
						<TD colSpan="3">
							<asp:label id="lblTagSettingsHelp" cssclass="Normal" runat="server" resourcekey="TagSettingsDescription"
								enableviewstate="False"></asp:label></TD>
					</TR>
					<TR vAlign="top">
						<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
						<TD class="SubHead" noWrap width="150">
							<dnn:label id="plName" runat="server" resourcekey="Name" suffix=":" controlname="txtName"></dnn:label></TD>
						<TD align="left" width="100%">
							<asp:textbox id="txtName" cssclass="NormalTextBox" runat="server" width="325" columns="30"
								maxlength="255"></asp:textbox>
							<asp:requiredfieldvalidator id="valName" cssclass="NormalRed" runat="server" resourcekey="valName" display="Dynamic"
								errormessage="<br>You Must Enter a Valid Name" controltovalidate="txtName"></asp:requiredfieldvalidator></TD>
					</TR>
				</TABLE>
			</asp:panel>
		</td>
	</tr>
</table>
<p>
	<asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" cssclass="CommandButton" text="Update"
		borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel"
		causesvalidation="False" borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdDelete" resourcekey="cmdDelete" runat="server" cssclass="CommandButton" text="Delete"
		causesvalidation="False" borderstyle="none" />
</p>
