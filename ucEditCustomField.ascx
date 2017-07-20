<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucEditCustomField.ascx.vb" Inherits="Ventrian.NewsArticles.ucEditCustomField" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<article:Header id="ucHeader1" SelectedMenu="AdminOptions" runat="server" MenuPosition="Top"></article:Header>
<br />
<table class="Settings" cellspacing="2" cellpadding="2" summary="Edit Custom Field Design Table" border="0">
<tr>
    <td width="560" valign="top">
        <dnn:sectionhead id="dshCustomFieldDetails" cssclass="Head" runat="server" text="Custom Field Details" section="tblCustomFieldDetails"
	        resourcekey="CustomFieldDetails" includerule="True"></dnn:sectionhead>
        <TABLE id="tblCustomFieldDetails" cellSpacing="0" cellPadding="2" width="100%" summary="Custom Field Details Design Table"
	        border="0" runat="server">
	        <TR>
		        <TD colSpan="3">
			        <asp:label id="lblCustomFieldDetailsHelp" cssclass="Normal" runat="server" resourcekey="CustomFieldDetailsDescription"
				        enableviewstate="False"></asp:label></TD>
	        </TR>
	        <TR vAlign="top">
		        <TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		        <TD class="SubHead" noWrap width="150"><dnn:label id="plName" runat="server" resourcekey="Name" suffix=":" controlname="txtName"></dnn:label></TD>
		        <TD align="left" width="100%">
			        <asp:textbox id="txtName" cssclass="NormalTextBox" runat="server" maxlength="255" columns="30"
				        width="325"></asp:textbox>
			        <asp:requiredfieldvalidator id="valName" cssclass="NormalRed" runat="server" resourcekey="valName" controltovalidate="txtName"
				        errormessage="<br>You Must Enter a Valid Name" display="Dynamic"></asp:requiredfieldvalidator>
		        </TD>
	        </TR>
	        <TR vAlign="top">
		        <TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		        <TD class="SubHead" noWrap width="150"><dnn:label id="plCaption" runat="server" resourcekey="Caption" suffix=":" controlname="txtCaption"></dnn:label></TD>
		        <TD align="left" width="100%">
			        <asp:textbox id="txtCaption" cssclass="NormalTextBox" runat="server" maxlength="255" columns="30"
				        width="325"></asp:textbox>
			        <asp:requiredfieldvalidator id="valCaption" cssclass="NormalRed" runat="server" resourcekey="valCaption" controltovalidate="txtCaption"
				        errormessage="<br>You Must Enter a Valid Caption" display="Dynamic"></asp:requiredfieldvalidator>
		        </TD>
	        </TR>
	        <TR vAlign="top">
		        <TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		        <TD class="SubHead" noWrap width="150"><dnn:label id="plCaptionHelp" runat="server" resourcekey="CaptionHelp" suffix=":" controlname="txtCaptionHelp"></dnn:label></TD>
		        <TD align="left" width="100%">
			        <asp:textbox id="txtCaptionHelp" cssclass="NormalTextBox" runat="server" maxlength="255" columns="30"
				        width="325"></asp:textbox>
		        </TD>
	        </TR>
	        <TR vAlign="top">
		        <TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		        <TD class="SubHead" noWrap width="150"><dnn:label id="plFieldType" runat="server" resourcekey="FieldType" suffix=":" controlname="drpFieldType"></dnn:label></TD>
		        <TD align="left" width="100%">
			        <asp:dropdownlist id="drpFieldType" cssclass="NormalTextBox" runat="server" width="325" AutoPostBack="True"></asp:dropdownlist>
		        </TD>
	        </TR>
	        <TR vAlign="top" runat="server" id="trFieldElements">
		        <TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		        <TD class="SubHead" noWrap width="150"><dnn:label id="plFieldElements" runat="server" resourcekey="FieldElements" suffix=":" controlname="txtFieldElements"></dnn:label></TD>
		        <TD align="left" width="100%">
		            <asp:RadioButtonList ID="lstFieldElementType" runat="server" CssClass="Normal" RepeatLayout="Table" RepeatDirection="Horizontal" AutoPostBack="true" />
			        <asp:Panel ID="pnlFieldElements" runat="server">
			            <asp:textbox id="txtFieldElements" cssclass="NormalTextBox" runat="server" columns="30" width="325"></asp:textbox>
			            <asp:requiredfieldvalidator id="valFieldElements" cssclass="NormalRed" runat="server" resourcekey="valFieldElements" controltovalidate="txtFieldElements"
				            errormessage="<br>You Must Enter a Valid Field Element" display="Dynamic"></asp:requiredfieldvalidator>
			        </asp:Panel>
			        <asp:Label ID="lblFieldElementHelp" Runat="server" ResourceKey="FieldElementHelp" CssClass="Normal" />
		        </TD>
	        </TR>
	        <TR vAlign="top">
		        <TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		        <TD class="SubHead" noWrap width="150"><dnn:label id="plDefaultValue" runat="server" resourcekey="DefaultValue" suffix=":" controlname="txtDefaultValue"></dnn:label></TD>
		        <TD align="left" width="100%">
			        <asp:textbox id="txtDefaultValue" cssclass="NormalTextBox" runat="server" maxlength="255" columns="30"
				        width="325"></asp:textbox>
		        </TD>
	        </TR>
	        <TR vAlign="top">
		        <TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		        <TD class="SubHead" noWrap width="150"><dnn:label id="plVisible" runat="server" resourcekey="Visible" suffix=":" controlname="chkVisible"></dnn:label></TD>
		        <TD align="left" width="100%">
			        <asp:checkbox id="chkVisible" cssclass="NormalTextBox" runat="server" Checked="True"></asp:checkbox>
		        </TD>
	        </TR>
            <tr id="trMaximumLength" runat="Server">
	            <TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
	            <td class="SubHead" width="150"><dnn:label id="plMaximumLength" runat="server" controlname="txtMaximumLength" suffix=":"></dnn:label></td>
	            <td valign="bottom">
		            <asp:TextBox ID="txtMaximumLength" Runat="server" CssClass="NormalTextBox" width="250"
			            columns="10" maxlength="6" />
			        <asp:CompareValidator ID="valMaximumLengthIsNumber" Runat="server" ControlToValidate="txtMaximumLength"
			            Display="Dynamic" ResourceKey="valMaximumLengthIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
	            </td>
            </tr>
        </TABLE>
        <br />
        <asp:PlaceHolder ID="phRequired" runat="Server">
        <dnn:sectionhead id="dshRequiredDetails" cssclass="Head" runat="server" text="Required Details" section="tblRequiredDetails"
	        resourcekey="RequiredDetails" includerule="True"></dnn:sectionhead>
        <TABLE id="tblRequiredDetails" cellSpacing="0" cellPadding="2" width="100%" summary="Required Details Design Table"
	        border="0" runat="server">
	        <TR>
		        <TD colSpan="3">
			        <asp:label id="lblRequiredDetails" cssclass="Normal" runat="server" resourcekey="RequiredDetailsDescription"
				        enableviewstate="False"></asp:label></TD>
	        </TR>
	        <TR vAlign="top">
		        <TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		        <TD class="SubHead" noWrap width="150"><dnn:label id="plRequired" runat="server" resourcekey="Required" suffix=":" controlname="chkRequired"></dnn:label></TD>
		        <TD align="left" width="100%">
			        <asp:checkbox id="chkRequired" cssclass="NormalTextBox" runat="server"></asp:checkbox>
		        </TD>
	        </TR>
	        <TR vAlign="top">
		        <TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		        <TD class="SubHead" noWrap width="150"><dnn:label id="plValidationType" runat="server" resourcekey="ValidationType" suffix=":" controlname="drpValidationType"></dnn:label></TD>
		        <TD align="left" width="100%">
			        <asp:dropdownlist id="drpValidationType" cssclass="NormalTextBox" runat="server" width="325" AutoPostBack="true"></asp:dropdownlist>
		        </TD>
	        </TR>
	        <tr valign="top" runat="server" id="trRegex">
		        <td width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		        <td class="SubHead" noWrap width="150"><dnn:label id="plRegex" runat="server" resourcekey="plRegex" suffix=":" controlname="txtRegex"></dnn:label></td>
		        <td align="left" width="100%">
			        <asp:textbox id="txtRegex" cssclass="NormalTextBox" runat="server" maxlength="500" columns="30"
				        width="325"></asp:textbox>
		        </td>
	        </tr>
        </table>
        <br />
        </asp:PlaceHolder>
        <p align=center>
	        <asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" cssclass="CommandButton" text="Update"
		        borderstyle="none" />
	        &nbsp;
	        <asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel"
		        causesvalidation="False" borderstyle="none" />
	        &nbsp;
	        <asp:linkbutton id="cmdDelete" resourcekey="cmdDelete" runat="server" cssclass="CommandButton" text="Delete"
		        causesvalidation="False" borderstyle="none" />
        </p>
    </td>
</tr>
</table>