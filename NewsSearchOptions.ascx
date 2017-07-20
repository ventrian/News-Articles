<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="NewsSearchOptions.ascx.vb" Inherits="Ventrian.NewsArticles.NewsSearchOptions" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table cellspacing="0" cellpadding="2" summary="Edit Links Design Table" border="0">
	<tr valign="top">
		<td class="SubHead" width="150"><dnn:label id="plModuleID" runat="server" resourcekey="Module" suffix=":" controlname="drpModuleID" /></td>
		<td align="left" width="325">
			<asp:dropdownlist id="drpModuleID" Runat="server" Width="325" datavaluefield="ModuleID" datatextfield="ModuleTitle" 
				CssClass="NormalTextBox" />
        </td>
	</tr>
</table>