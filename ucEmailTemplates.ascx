<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucEmailTemplates.ascx.vb" Inherits="Ventrian.NewsArticles.ucEmailTemplates" %>

<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>

<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<article:header id="ucHeader1" SelectedMenu="AdminOptions" runat="server" MenuPosition="Top"></article:Header>

<table class="Settings" cellspacing="2" cellpadding="2" summary="Edit Page Design Table" border="0">
  <tr>
    <td width="560" valign="top">
      <asp:panel id="pnlSettings" runat="server" cssclass="WorkPanel" visible="True">
        <dnn:sectionhead id="dshEmailTemplates" runat="server" cssclass="Head" includerule="True" resourcekey="EmailTemplates" section="tblEmailTemplates" text="Email Templates" />
        <table id="tblEmailTemplates" runat="server" cellspacing="0" cellpadding="2" width="525" summary="Page Settings Design Table" border="0">
          <tr>
            <td colspan="3"><asp:label id="lblEmailTemplatesHelp" resourcekey="EmailTemplatesDescription" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
          </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<td class="SubHead" width="150"><dnn:label id="plName" resourcekey="Name" runat="server" controlname="txtName" suffix=":"></dnn:label></td>
		        <td align="left" width="325">
					<asp:DropDownList ID="drpTemplate" Runat="server" CssClass="NormalTextBox" AutoPostBack="True" /> 
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<td class="SubHead" width="150"><dnn:label id="plSubject" resourcekey="Subject" runat="server" controlname="txtSubject" suffix=":"></dnn:label></td>
		        <td align="left" width="325">
			        <asp:textbox id="txtSubject" cssclass="NormalTextBox" width="325" columns="30" maxlength="255" runat="server" />
			        <asp:requiredfieldvalidator id="valSubject" resourcekey="valSubject" display="Dynamic" runat="server" errormessage="<br>You Must Enter a Valid Subject" controltovalidate="txtSubject" cssclass="NormalRed" />
		        </td>
	        </tr>								
			<TR>
				<TD width=25></TD>
				<TD class="SubHead" width="150">
					<dnn:label id="plTemplate" runat="server" resourcekey="Template" controlname="txtTemplate"
						helpkey="TemplateHelp" suffix=":"></dnn:label></TD>
				<TD class="NormalTextBox" width="325">
					<asp:textbox id="txtTemplate" cssclass="NormalTextBox" runat="server" width="300" maxlength="500"
						rows="10" textmode="MultiLine"></asp:textbox></TD>
			</TR>
        </table>
		<br>
		<dnn:sectionhead id="dshEmailTemplateHelp" runat="server" cssclass="Head" includerule="True" isExpanded="false" resourcekey="EmailTemplateHelp" section="tblEmailTemplateHelp" text="Email Template Help" />
        <table id="tblEmailTemplateHelp" runat="server" cellspacing="0" cellpadding="2" width="525" summary="Email Template Help Design Table" border="0">
			<tr>
				<td colspan="3"><asp:label id="lblTemplateArticleHelp" resourcekey="TemplateArticleHelpDescription" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
			</tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[USERNAME]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblUserName" Runat="server" ResourceKey="UserName" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<td width="25"></td>
				<TD class="SubHead" width="150">[EMAIL]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblEmail" Runat="server" ResourceKey="Email" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[DISPLAYNAME]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblDisplayName" Runat="server" ResourceKey="DisplayName" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[FIRSTNAME]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblFirstName" Runat="server" ResourceKey="FirstName" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[LASTNAME]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblLastName" Runat="server" ResourceKey="LastName" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[FULLNAME]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblFullName" Runat="server" ResourceKey="FullName" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[PORTALNAME]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblPortalName" Runat="server" ResourceKey="PortalName" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[CREATEDATE]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblCreateDate" Runat="server" ResourceKey="CreateDate" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[POSTDATE]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblPostDate" Runat="server" ResourceKey="PostDate" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[TITLE]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblTitle" Runat="server" ResourceKey="Title" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[SUMMARY]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblSummary" Runat="server" ResourceKey="Summary" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[LINK]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblLink" Runat="server" ResourceKey="Link" EnableViewState="False" />
				</td>
	        </tr>
			<tr>
				<td colspan="3"><br><asp:label id="lblTemplateCommentHelp" resourcekey="TemplateCommentHelpDescription" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
			</tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[DISPLAYNAME]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblCommentDisplayName" Runat="server" ResourceKey="CommentDisplayName" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[EMAIL]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblCommentEmail" Runat="server" ResourceKey="CommentEmail" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[USERNAME]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblCommentUserName" Runat="server" ResourceKey="CommentUserName" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[FIRSTNAME]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblCommentFirstName" Runat="server" ResourceKey="CommentFirstName" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[LASTNAME]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblCommentLastName" Runat="server" ResourceKey="CommentLastName" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[FULLNAME]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblCommentFullName" Runat="server" ResourceKey="CommentFullName" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[PORTALNAME]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblPortalName2" Runat="server" ResourceKey="PortalName" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[POSTDATE]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblCommentPostDate" Runat="server" ResourceKey="CommentPostDate" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[TITLE]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblTitle2" Runat="server" ResourceKey="Title" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[COMMENT]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblComment" Runat="server" ResourceKey="Comment" EnableViewState="False" />
				</td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<TD class="SubHead" width="150">[LINK]</td>
				<td class="Normal" align="left" width="325">
					<asp:Label ID="lblCommentLink" Runat="server" ResourceKey="CommentLink" EnableViewState="False" />
				</td>
	        </tr>
		</table>
      </asp:panel>
    </td>
  </tr>
</table>
<p>
	<asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" cssclass="CommandButton" text="Update" borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel" causesvalidation="False" borderstyle="none" />
</p>

<article:header id="ucHeader2" SelectedMenu="AdminOptions" runat="server" MenuPosition="Bottom"></article:Header>
