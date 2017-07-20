<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucEditCategory.ascx.vb" Inherits="Ventrian.NewsArticles.ucEditCategory" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<article:Header runat="server" id="ucHeader1" SelectedMenu="EditCategories" MenuPosition="Top" />
<br />
<table class="Settings" cellspacing="2" cellpadding="2" summary="Edit Category Design Table" border="0">
  <tr>
    <td width="560" valign="top">
      <asp:panel id="pnlSettings" runat="server" cssclass="WorkPanel" visible="True">
        <dnn:sectionhead id="dshCategory" runat="server" cssclass="Head" includerule="True" resourcekey="CategorySettings" section="tblCategory" text="Category Settings" />
        <table id="tblCategory" runat="server" cellspacing="0" cellpadding="2" width="525" summary="Category Design Table" border="0">
          <tr>
            <td colspan="3"><asp:label id="lblCategoryHelp" resourcekey="CategorySettingsDescription" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
          </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<td class="SubHead" width="150"><dnn:label id="plParent" resourcekey="Parent" runat="server" controlname="drpParent" suffix=":"></dnn:label></td>
		        <td align="left" width="325">
			        <asp:DropDownList ID="drpParentCategory" runat="server" CssClass="NormalTextBox" DataTextField="NameIndented" DataValueField="CategoryID" />
			        <asp:CustomValidator id="valInvalidParentCategory" runat="server" ErrorMessage="<br>Invalid Parent Category. Possible Loop Detected."
								ResourceKey="valInvalidParentCategory" ControlToValidate="drpParentCategory" CssClass="NormalRed" Display="Dynamic"></asp:CustomValidator>
			    </td>
	        </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<td class="SubHead" width="150"><dnn:label id="plName" resourcekey="Name" runat="server" controlname="txtName" suffix=":"></dnn:label></td>
		        <td align="left" width="325">
			        <asp:textbox id="txtName" cssclass="NormalTextBox" width="325" columns="30" maxlength="255" runat="server" />
			        <asp:requiredfieldvalidator id="valName" resourcekey="valName" display="Dynamic" runat="server" errormessage="<br>You Must Enter a Valid Category Name" controltovalidate="txtName" cssclass="NormalRed" />
		        </td>
	        </tr>
	        <tr valign="top">
				<td width=25></TD>
				<td class="SubHead" width="150"><dnn:label id="plDescription" resourcekey="Description" runat="server" controlname="txtDescription" suffix=":"></dnn:label></td>
		        <td align="left" width="325"></td>
	        </tr>
	        <tr>
	            <td></td>
	            <td colspan="2">
			        <dnn:texteditor id="txtDescription" width="100%" runat="server" height="400"></dnn:texteditor><br />
	            </td>
	        </tr>
	        <tr>
				<td width="25"></td>
	            <td class="SubHead" width="150" valign="top"><dnn:label id="plCategoryImage" resourcekey="CategoryImage" runat="server" controlname="ctlImageLink" suffix=":"></dnn:label></td>
	            <td>
	                <dnn:url id="ctlIcon" runat="server" width="300" showlog="False" showtabs="false" showurls="false" showtrack="false" FileFilter="jpg,jpeg,jpe,gif,bmp,png" required="false"></dnn:url>
	            </td>
	        </tr>
	        <tr>
	            <td width="25"></td>
	            <td colspan="2">
                    <br />
                    <dnn:sectionhead id="dshSecurity" cssclass="Head" runat="server" text="Security Settings" section="tblSecurity"
	                    resourcekey="Security" includerule="True" IsExpanded="True" />
                    <table id="tblSecurity" cellspacing="2" cellpadding="2" summary="Security Design Table" border="0" runat="server">
                    <tr>
	                    <td class="SubHead" width="150"><dnn:label id="plInheritSecurity" resourcekey="InheritSecurity" runat="server" controlname="chkInheritSecurity" suffix=":"></dnn:label></td>
	                    <td>
	                        <asp:CheckBox ID="chkInheritSecurity" runat="server" AutoPostBack="true" />
	                    </td>
	                </tr>
	                <tr runat="server" id="trPermissions">
	                    <td class="SubHead" width="150"><dnn:label id="plPermissions" resourcekey="Permissions" runat="server" controlname="chkPermissions" suffix=":"></dnn:label></td>
	                    <td>
                            <asp:DataGrid ID="grdCategoryPermissions" Runat="server" AutoGenerateColumns="False" ItemStyle-CssClass="Normal"
                                ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-CssClass="NormalBold" CellSpacing="0" CellPadding="0" GridLines="None" BorderWidth="1"
                                BorderStyle="None" DataKeyField="Value">
                                <Columns>
                                    <asp:TemplateColumn>
	                                    <ItemStyle HorizontalAlign="Left" Wrap="False"/>
	                                    <ItemTemplate>
		                                    <%# DataBinder.Eval(Container.DataItem, "Text") %>
	                                    </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
	                                    <HeaderTemplate>
		                                    &nbsp;
		                                    <asp:Label ID="lblView" Runat="server" EnableViewState="False" ResourceKey="View" />&nbsp;
	                                    </HeaderTemplate>
	                                    <ItemTemplate>
		                                    <asp:CheckBox ID="chkView" Runat="server" />
	                                    </ItemTemplate>
                                    </asp:TemplateColumn>	
                                    <asp:TemplateColumn>
	                                    <HeaderTemplate>
		                                    &nbsp;
		                                    <asp:Label ID="lblSubmit" Runat="server" EnableViewState="False" ResourceKey="Submit" />&nbsp;
	                                    </HeaderTemplate>
	                                    <ItemTemplate>
		                                    <asp:CheckBox ID="chkSubmit" Runat="server" />
	                                    </ItemTemplate>
                                    </asp:TemplateColumn>	
                                </Columns>
                            </asp:DataGrid>
	                    </td>
	                </tr>
	                <tr runat="server" id="trSecurityMode">
	                    <td class="SubHead" width="150"><dnn:label id="plSecurityMode" resourcekey="SecurityMode" runat="server" controlname="chkSecurityMode" suffix=":"></dnn:label></td>
	                    <td>
	                        <asp:RadioButtonList ID="lstSecurityMode" Runat="server" CssClass="Normal" RepeatDirection="Vertical" />
	                    </td>
	                </tr>
	                </table>
                </td>
            </tr>     
	        <tr>
	            <td width="25"></td>
	            <td colspan="2">
                    <br />
                    <dnn:sectionhead id="dshMeta" cssclass="Head" runat="server" text="Meta" section="tblMeta"
	                    resourcekey="Meta" includerule="True" IsExpanded="False" />
                    <table id="tblMeta" cellspacing="2" cellpadding="2" summary="Meta Design Table" border="0" runat="server">
                    <tr>
                        <td class="SubHead" width="150"><dnn:label id="plMetaTitle" runat="server" suffix=":" controlname="txtMetaTitle"></dnn:label></td>
                        <td>
			                <asp:textbox id="txtMetaTitle" cssclass="NormalTextBox" runat="server" maxlength="200" width="300"></asp:textbox>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="150"><dnn:label id="plMetaDescription" runat="server" suffix=":" controlname="txtMetaDescription"></dnn:label></td>
                        <td>
			                <asp:textbox id="txtMetaDescription" cssclass="NormalTextBox" runat="server" maxlength="500" width="300"
									            textmode="MultiLine" rows="3"></asp:textbox>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="150"><dnn:label id="plMetaKeyWords" runat="server" suffix=":" controlname="txtMetaKeyWords"></dnn:label></td>
                        <td>
			                <asp:textbox id="txtMetaKeyWords" cssclass="NormalTextBox" runat="server" maxlength="500" width="300"
									            textmode="MultiLine" rows="3"></asp:textbox>
                        </td>
                    </tr>
                    </table>
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
	&nbsp;
	<asp:linkbutton id="cmdDelete" resourcekey="cmdDelete" runat="server" cssclass="CommandButton" text="Delete" causesvalidation="False" borderstyle="none" />
</p>
<article:Header runat="server" id="ucHeader2" SelectedMenu="EditCategories" MenuPosition="Bottom" />
