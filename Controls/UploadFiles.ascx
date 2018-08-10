<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UploadFiles.ascx.vb" Inherits="Ventrian.NewsArticles.Controls.UploadFiles" %>
<%@ Register TagPrefix="Ventrian" Assembly="Ventrian.NewsArticles" Namespace="Ventrian.NewsArticles.Components.WebControls" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>

<div style="display: none">
	<asp:Literal ID="litModuleID" runat="server" />
	<asp:Literal ID="litTabModuleID" runat="server" />
	<asp:Literal ID="litTicketID" runat="server" />
	<asp:Literal ID="litArticleGuid" runat="server" />
</div>

<dnn:SectionHead ID="dshFiles" CssClass="Head" runat="server" Text="Images" Section="tblFiles" IsExpanded="false" IncludeRule="True"></dnn:SectionHead>
<table id="tblFiles" style="width: 100%" summary="Images Design Table" runat="server">
	<tr>
		<td>
			<asp:Label ID="lblFilesHelp" CssClass="Normal" runat="server" EnableViewState="False" /></td>
	</tr>
	<tr>
		<td>
			<table style="width: 100%">
				<tr>
					<td valign="top" id="trUpload" runat="server">
						<dnn:SectionHead ID="dshUploadFiles" CssClass="Head" runat="server" Text="Images" Section="tblUploadFiles" IncludeRule="True" />
						<table id="tblUploadFiles" style="width: 100%" summary="Images Design Table" runat="server">
							<tr>
								<td class="SubHead" style="width: 150px">
									<dnn:Label ID="plFolder" Text="Folder:" runat="server" ControlName="drpUploadFilesFolder"></dnn:Label>
								</td>
							</tr>
							<tr>
								<td>
									<asp:DropDownList ID="drpUploadFilesFolder" runat="server" CssClass="NormalTextBox" Style="width: 275px" />
								</td>
							</tr>
							<tr>
								<td class="SubHead" style="width: 150px">
									<dnn:Label ID="Label1" Text="Folder:" runat="server" ControlName="drpUploadFilesFolder"></dnn:Label>
								</td>
							</tr>
							<tr>
								<td>
									<asp:FileUpload ID="fupFile" runat="server" AllowMultiple="true" />
									<asp:Button ID="btUpload" runat="server" OnClick="btUpload_Click" />
								</td>
							</tr>

						</table>
					</td>
					<td valign="top" id="trExisting" runat="server">
						<dnn:SectionHead ID="dshExistingFiles" CssClass="Head" runat="server" Text="Select Existing Files" Section="tblExistingFiles" IncludeRule="True" />
						<table id="tblExistingFiles" style="width: 100%" summary="Existing Files Design Table" runat="server">
							<tr>
								<td>
									<dnn:URL ID="ctlFile" runat="server" style="width: 275px" Required="False" ShowTrack="False" ShowNewWindow="False"
										ShowLog="False" UrlType="F" ShowUrls="False" ShowFiles="True" ShowTabs="False" ShowUpLoad="false" />
								</td>
							</tr>
							<tr>
								<td>
									<asp:LinkButton ID="cmdAddExistingFile" runat="server" CausesValidation="false" CssClass="CommandButton" /></td>
				</tr>
			</table>
			</td>
				</tr>
			</table>
			<br />
		<dnn:SectionHead ID="dshSelectedFiles" CssClass="Head" runat="server" Text="Attached Files" Section="tblSelectedFiles" IncludeRule="True" />
		<table id="tblSelectedFiles" style="width: 100%" summary="Selected Files Design Table" runat="server">
			<tr>
				<td>
					<asp:DataList CellPadding="4" CellSpacing="0" ID="dlFiles" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" DataKeyField="FileID">
						<ItemStyle CssClass="Normal" HorizontalAlign="Center" />
						<ItemTemplate>
							<b>
								<%#DataBinder.Eval(Container.DataItem, "Title")%>
							</b>
							<br />
							<asp:ImageButton ID="btnEdit" runat="server" CommandName="Edit" CausesValidation="False" ImageUrl="~/Images/edit.gif" />
							<asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" CausesValidation="False" ImageUrl="~/Images/delete.gif" />
							<asp:ImageButton ID="btnDown" runat="server" ImageUrl="~/Images/dn.gif" />
							<asp:ImageButton ID="btnUp" runat="server" ImageUrl="~/Images/up.gif" />
						</ItemTemplate>
						<EditItemTemplate>
							<b>
								<%#DataBinder.Eval(Container.DataItem, "Title")%>
							</b>
							<br />
							<asp:TextBox ID="txtTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Title")  %>' Style="width: 125px" MaxLength="100" />
							<br />
							<asp:LinkButton ID="btnUpdate" runat="server" Text="Update"
								CommandName="Update" ResourceKey="cmdUpdate" CausesValidation="false" />
							<asp:LinkButton ID="btnCancel" runat="server" Text="Cancel"
								CommandName="Cancel" ResourceKey="cmdCancel" CausesValidation="false" />
						</EditItemTemplate>
					</asp:DataList>
					<div align="left">
						<asp:Label ID="lblNoFiles" runat="server" CssClass="Normal" />
					</div>
				</td>
			</tr>
		</table>
		</td>
	</tr>
</table>

<Ventrian:RefreshControl ID="cmdRefreshFiles" runat="server" Text="Refresh Files" Visible="false" />

