<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="LatestCommentsOptions.ascx.vb" Inherits="Ventrian.NewsArticles.LatestCommentsOptions" %>
<%@ Register TagPrefix="dnn" TagName="Skin" Src="~/controls/SkinControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>

<table id="tblLatestCommentsDetail" cellspacing="2" cellpadding="2" summary="Appearance Design Table"
	border="0" runat="server">
	<tr valign="top">
		<td class="SubHead" width="150">
			<dnn:label id="plModuleID" runat="server" resourcekey="Module" suffix=":" controlname="drpModuleID"></dnn:label></td>
		<td align="left" width="325">
			<asp:dropdownlist id="drpModuleID" Runat="server" Width="325" datavaluefield="ModuleID" datatextfield="ModuleTitle" 
				CssClass="NormalTextBox" AutoPostBack="True"></asp:dropdownlist></td>
	</tr>
    <tr valign="top">
        <td class="SubHead" width="200">
	        <dnn:label id="plCommentCount" runat="server" resourcekey="Count" suffix=":" controlname="txtCommentCount"></dnn:label></td>
        <td align="left">
	        <asp:textbox id="txtCommentCount" Runat="server" Width="50" CssClass="NormalTextBox">10</asp:textbox>
	        <asp:RequiredFieldValidator id="valCount" runat="server" ResourceKey="valCount.ErrorMessage" ErrorMessage="<br>* Required"
		        Display="Dynamic" ControlToValidate="txtCommentCount" CssClass="NormalRed"></asp:RequiredFieldValidator>
	        <asp:CompareValidator id="valCountType" runat="server" ResourceKey="valCountType.ErrorMessage" ErrorMessage="<br>* Must be a Number"
		        Display="Dynamic" ControlToValidate="txtCommentCount" Type="Integer" Operator="DataTypeCheck" CssClass="NormalRed"></asp:CompareValidator>
        </td>
    </tr>
	<tr>
	    <td colspan="2">
	        <dnn:sectionhead id="dshTemplate" runat="server" cssclass="Head" includerule="True" isExpanded="false"
	            resourcekey="Template" section="tblTemplate" text="Template" />
	        <table id="tblTemplate" runat="server" cellspacing="0" cellpadding="2" width="100%" summary="Template Design Table" border="0">
            <tr>
	            <td class="SubHead" width="150"><dnn:label id="Label2" resourcekey="IncludeStylesheet" runat="server"
			            controlname="chkIncludeStylesheet"></dnn:label></td>
	            <td valign="top"><asp:checkbox id="chkIncludeStylesheet" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
            </tr>
            <TR vAlign="top">
	            <TD class="SubHead" width="150">
		            <dnn:label id="plHtmlHeader" runat="server" resourcekey="HtmlHeader" suffix=":" controlname="txtHtmlHeader"></dnn:label></TD>
	            <TD align="left" width="325">
		            <asp:textbox id="txtHtmlHeader" cssclass="NormalTextBox" runat="server" Rows="2" TextMode="MultiLine"
			            maxlength="50" width="300"></asp:textbox></TD>
            </TR>
	        <TR vAlign="top">
		        <TD class="SubHead" width="150">
			        <dnn:label id="plHtmlBody" runat="server" resourcekey="HtmlBody" suffix=":" controlname="txtHtmlBody"></dnn:label></TD>
		        <TD align="left" width="325">
			        <asp:textbox id="txtHtmlBody" cssclass="NormalTextBox" runat="server" Rows="6" TextMode="MultiLine"
				        maxlength="50" width="300"></asp:textbox></TD>
	        </TR>
	        <TR vAlign="top">
		        <TD class="SubHead" width="150">
			        <dnn:label id="plHtmlFooter" runat="server" resourcekey="HtmlFooter" suffix=":" controlname="txtHtmlFooter"></dnn:label></TD>
		        <TD align="left" width="325">
			        <asp:textbox id="txtHtmlFooter" cssclass="NormalTextBox" runat="server" Rows="2" TextMode="MultiLine"
				        maxlength="50" width="300"></asp:textbox></TD>
	        </TR>
	        <TR vAlign="top">
		        <TD class="SubHead" width="150">
			        <dnn:label id="plHtmlNoComments" runat="server" resourcekey="HtmlNoArticles" suffix=":" controlname="txtHtmlNoArticles"></dnn:label></TD>
		        <TD align="left" width="325">
			        <asp:textbox id="txtHtmlNoComments" cssclass="NormalTextBox" runat="server" Rows="6" TextMode="MultiLine"
				        maxlength="50" width="300"></asp:textbox></TD>
	        </TR>
            </table>
	    </td>
	</tr>
</table>
