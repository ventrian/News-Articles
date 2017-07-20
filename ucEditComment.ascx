<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucEditComment.ascx.vb" Inherits="Ventrian.NewsArticles.ucEditComment" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>

<article:header id=ucHeader1 SelectedMenu="ApproveArticles" runat="server" MenuPosition="Top"></article:Header>

<table class="Settings" cellspacing="2" cellpadding="2" summary="Edit Page Design Table" border="0" width="600px">
  <tr>
    <td valign="top">
      <asp:panel id="pnlSettings" runat="server" cssclass="WorkPanel" visible="True">
        <dnn:sectionhead id="dshEditComment" runat="server" cssclass="Head" includerule="True" resourcekey="EditComment" section="tblEditComment" text="Edit Comment" />
        <table id="tblEditComment" runat="server" cellspacing="0" cellpadding="2" width="600px" summary="Page Settings Design Table" border="0">
            <tr valign="top" id="trName" runat="server">
	            <td width="25"><img src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25" height="1"></td>
	            <td class="SubHead" width="150">
		            <dnn:label id="plName" resourcekey="Name" runat="server" controlname="txtName" suffix=":"></dnn:label>
	            </td>
                <td align="left" width="425px">
                    <asp:textbox id="txtName" cssclass="NormalTextBox" runat="server" width="250px" />
                    <asp:requiredfieldvalidator id="valName" cssclass="NormalRed" runat="server" 
			            ControlToValidate="txtName" Display="Dynamic" ErrorMessage="Name Is Required" SetFocusOnError="true" ResourceKey="valName.ErrorMessage" />
                </td>
            </tr>
            <tr valign="top" id="trEmail" runat="server">
	            <td width="25"><img src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25" height="1"></td>
	            <td class="SubHead" width="150">
		            <dnn:label id="plEmail" resourcekey="Email" runat="server" controlname="txtEmail" suffix=":"></dnn:label>
	            </td>
                <td align="left" width="425px">
                    <asp:textbox id="txtEmail" CssClass="NormalTextBox" runat="server" width="250px" />
                    <asp:requiredfieldvalidator id="valEmail" cssclass="NormalRed" runat="server" 
	                    controltovalidate="txtEmail" display="Dynamic" ErrorMessage="Email Is Required" SetFocusOnError="true" ResourceKey="valEmail.ErrorMessage" />
	                <asp:RegularExpressionValidator ID="valEmailIsValid" CssClass="NormalRed" runat="server"
	                    ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Invalid Email Address" SetFocusOnError="true" ResourceKey="valEmailIsValid.ErrorMessage" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$" />    
                </td>
            </tr>
            <tr valign="top" id="trUrl" runat="server">
	            <td width="25"><img src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25" height="1"></td>
	            <td class="SubHead" width="150">
		            <dnn:label id="plUrl" resourcekey="Url" runat="server" controlname="txtUrl" suffix=":"></dnn:label>
	            </td>
                <td align="left" width="425px">
                    <asp:textbox id="txtURL" cssclass="NormalTextBox" runat="server" width="250px" />
                </td>
            </tr>
            <tr valign="top">
	            <td width="25"><img src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25" height="1"></td>
	            <td class="SubHead" width="150">
		            <dnn:label id="plComment" resourcekey="Comment" runat="server" controlname="txtComment" suffix=":"></dnn:label>
	            </td>
                <td align="left" width="425px">
                    <asp:textbox id="txtComment" cssclass="NormalTextBox" runat="server" textmode="MultiLine" Width="450px" Height="150px"></asp:textbox>
                    <asp:requiredfieldvalidator id="valComment" cssclass="NormalRed" runat="server" 
                        controltovalidate="txtComment" errormessage="<br>Comment Is Required" display="Dynamic" SetFocusOnError="true" ResourceKey="valComment.ErrorMessage" />
                </td>
            </tr>
        </table>
      </asp:panel>
    </td>
  </tr>
</table>

<p align="center" style="width: 600px;">
	<asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" cssclass="CommandButton" text="Update" borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel" causesvalidation="False" borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdDelete" resourcekey="cmdDelete" runat="server" cssclass="CommandButton" text="Delete" causesvalidation="False" borderstyle="none" />
</p>
