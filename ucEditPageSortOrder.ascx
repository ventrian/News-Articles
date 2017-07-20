<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucEditPageSortOrder.ascx.vb" Inherits="Ventrian.NewsArticles.ucEditPageSortOrder" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<article:Header runat="server" id="ucHeader1" SelectedMenu="SubmitArticle" MenuPosition="Top" />

<table class="Settings" cellspacing="2" cellpadding="2" summary="Edit Sort Order Design Table" border="0">
  <tr>
    <td width="560" valign="top">
      <asp:panel id="pnlSettings" runat="server" cssclass="WorkPanel" visible="True">
        <dnn:sectionhead id="dshPage" runat="server" cssclass="Head" includerule="True" resourcekey="SortOrderSettings" section="tblSortOrder" text="Sort Order" />
        <table id="tblSortOrder" runat="server" cellspacing="0" cellpadding="2" width="525" summary="Sort Order Design Table" border="0">
          <tr>
            <td colspan="3"><asp:label id="lblSortOrderHelp" resourcekey="SortOrderDescription" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
          </tr>
	        <tr valign="top">
				<TD width=25></TD>
				<td class="SubHead" width="150"><dnn:label id="plSortOrder" resourcekey="SortOrder" runat="server" controlname="lstSortOrder" suffix=":"></dnn:label></td>
		        <td align="left" width="325">
			       	<table cellpadding=0 cellspacing=0 border=0>
						<tr valign="top">
							<td>
								<asp:ListBox ID="lstSortOrder" Runat="server" DataTextField="Title" DataValueField="PageID" CssClass="CommandButton" Width="200" Rows="8" />
							</td>
							<td>
					                
							</td>
							<td>
								<table>
									<tr>
										<td>
											<asp:ImageButton id="upBtn" ImageUrl="~/images/up.gif" CommandName="up" AlternateText="Move selected page up in list" runat="server" />
										</td>
									</tr>
									<tr>
										<td>
											<asp:ImageButton id="downBtn" ImageUrl="~/images/dn.gif" CommandName="down" AlternateText="Move selected page down in list" runat="server" />
										</td>
									</tr>
								</table>
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
</p>

<article:Header runat="server" id="ucHeader2" SelectedMenu="SubmitArticle" MenuPosition="Bottom" />