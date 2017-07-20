<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucEditPage.ascx.vb" Inherits="Ventrian.NewsArticles.ucEditPage" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>

<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<article:Header runat="server" id="ucHeader1" SelectedMenu="SubmitArticle" MenuPosition="Top" />

<table class="Settings" cellspacing="2" cellpadding="2" summary="Edit Page Design Table" border="0" width="100%">
  <tr>
    <td width="100%" valign="top">
      <asp:panel id="pnlSettings" runat="server" cssclass="WorkPanel" visible="True">
        <dnn:sectionhead id="dshPage" runat="server" cssclass="Head" includerule="True" resourcekey="PageSettings" section="tblPage" text="Page Settings" />
        <table id="tblPage" runat="server" cellspacing="0" cellpadding="2" width="100%" summary="Page Settings Design Table" border="0">
          <tr>
            <td colspan="3"><asp:label id="lblPageSettingsHelp" resourcekey="PageSettingsDescription" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
          </tr>
	        <tr valign="top">
				<TD width=25><img src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25" height="1"></TD>
				<td class="SubHead" width="150">
					<dnn:label id="plTitle" resourcekey="Title" runat="server" controlname="txtTitle" suffix=":"></dnn:label>
				</td>
		        <td align="left" width="100%">
			        <asp:textbox id="txtTitle" cssclass="NormalTextBox" width="325" columns="30" maxlength="255" runat="server" />
			        <asp:requiredfieldvalidator id="valTitle" resourcekey="valTitle" display="Dynamic" runat="server" errormessage="<br>You Must Enter a Valid Title" controltovalidate="txtTitle" cssclass="NormalRed" />
		        </td>
	        </tr>
			<TR>
				<TD width=25></TD>
				<TD class=SubHead width=150>
					<dnn:label id=plSummary runat="server" resourcekey="Summary" suffix=":" helpkey="SummaryHelp" controlname="txtSummary"></dnn:label><img src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="150" height="1"></TD>
				<TD class=NormalTextBox width=100%></TD>
			</TR>
			<TR>
				<TD width=25></TD>
				<TD align=center width=100% colSpan=2>
					<dnn:texteditor id="txtSummary" width="100%" runat="server" height="400"></dnn:texteditor>
					<asp:requiredfieldvalidator id=valSummary runat="server" cssclass="NormalRed" resourcekey="valSummary.ErrorMessage" display="Dynamic" errormessage="Page Text Is Required" controltovalidate="txtSummary"></asp:requiredfieldvalidator></TD>
			</TR>
        </table>
      </asp:panel>
    </td>
  </tr>
</table>
<p>
	<asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" cssclass="CommandButton" text="Update" borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel" causesvalidation="False" borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdDelete" resourcekey="cmdDelete" runat="server" cssclass="CommandButton" text="Delete" causesvalidation="False" borderstyle="none" />
</p>

<article:Header runat="server" id="ucHeader2" SelectedMenu="SubmitArticle" MenuPosition="Bottom" />
