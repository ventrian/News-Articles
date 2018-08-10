<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UploadImages.ascx.vb" Inherits="Ventrian.NewsArticles.Controls.UploadImages" %>
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

<dnn:SectionHead ID="dshImages" CssClass="Head" runat="server" Text="Images" Section="tblImages" IsExpanded="false"
	IncludeRule="True"></dnn:SectionHead>
<table id="tblImages" cellspacing="0" cellpadding="2" width="100%" summary="Images Design Table"
	border="0" runat="server">
	<tr>
		<td colspan="3">
			<asp:Label ID="lblImagesHelp" CssClass="Normal" runat="server" EnableViewState="False" /></td>
	</tr>
	<tr>
		<td width="25"></td>
		<td colspan="2">
			<asp:PlaceHolder ID="phImages" runat="server">
				<table width="100%">
					<tr>
						<td valign="top" id="trUpload" runat="server">
							<dnn:SectionHead ID="dshUploadImages" CssClass="Head" runat="server" Text="Images" Section="tblUploadImages"
								IncludeRule="True" />
							<table id="tblUploadImages" cellspacing="0" cellpadding="2" width="100%" summary="Images Design Table"
								border="0" runat="server">
								<tr>
									<td class="SubHead" width="150">
										<dnn:Label ID="plFolder" Text="Folder:" runat="server" ControlName="drpUploadImagesFolder"></dnn:Label>
									</td>
									<td>&nbsp;</td>
								</tr>
								<tr>
									<td>
										<asp:DropDownList ID="drpUploadImageFolder" runat="server" CssClass="NormalTextBox" Width="275px" />
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
							<dnn:SectionHead ID="dshExistingImages" CssClass="Head" runat="server" Text="Select Existing Images" Section="tblExistingImages" IncludeRule="True" />
							<table id="tblExistingImages" style="width: 100%" summary="Existing Images Design Table" runat="server">
								<tr>
									<td>
										<dnn:URL ID="ctlImage" runat="server" Width="275" Required="False" ShowTrack="False" ShowNewWindow="False"
											ShowLog="False" UrlType="F" ShowUrls="False" ShowFiles="True" ShowTabs="False" ShowUpLoad="false" FileFilter=".jpg,.gif,.png"></dnn:URL>
									</td>
								</tr>
								<tr>
									<td>
										<asp:LinkButton ID="cmdAddExistingImage" runat="server" ResourceKey="cmdAddExistingImage" CausesValidation="false" CssClass="CommandButton" /></td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
				<br />
				<dnn:SectionHead ID="dshSelectedImages" CssClass="Head" runat="server" Text="Attached Images" Section="tblSelectedImages"
					IncludeRule="True" />
				<table id="tblSelectedImages" cellspacing="0" cellpadding="2" width="100%" summary="Selected Images Design Table"
					border="0" runat="server">
					<tr>
						<td>
							<asp:DataList CellPadding="4" CellSpacing="0" ID="dlImages" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" DataKeyField="ImageID">
								<ItemStyle CssClass="Normal" HorizontalAlign="Center" VerticalAlign="Top" />
								<ItemTemplate>
									<img src='<%# GetImageUrl(Container.DataItem) %>' alt="Photo"><br />
									<b>
										<%#DataBinder.Eval(Container.DataItem, "Title")%>
									</b>
									<br />
									<asp:ImageButton ID="btnEdit" runat="server" CommandName="Edit" CausesValidation="False" ImageUrl="~/Images/edit.gif" />
									<asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" CausesValidation="False" ImageUrl="~/Images/delete.gif" />
									<asp:ImageButton ID="btnBottom" runat="server" ImageUrl="~/Images/bottom.gif" />
									<asp:ImageButton ID="btnDown" runat="server" ImageUrl="~/Images/dn.gif" />
									<asp:ImageButton ID="btnUp" runat="server" ImageUrl="~/Images/up.gif" />
									<asp:ImageButton ID="btnTop" runat="server" ImageUrl="~/Images/top.gif" />
								</ItemTemplate>
								<EditItemTemplate>
									<img src='<%# GetImageUrl(Container.DataItem) %>' alt="Photo"><br />
									<asp:TextBox ID="txtTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Title")  %>' Width="160" MaxLength="255" />
									<br />
									<asp:TextBox ID="txtDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description")  %>' Width="160" TextMode="MultiLine" Rows="4" />
									<br />
									<asp:LinkButton ID="btnUpdate" runat="server" Text="Update"
										CommandName="Update" ResourceKey="cmdUpdate" CausesValidation="false" />
									<asp:LinkButton ID="btnCancel" runat="server" Text="Cancel"
										CommandName="Cancel" ResourceKey="cmdCancel" CausesValidation="false" />
								</EditItemTemplate>
							</asp:DataList>
							<div align="left">
								<asp:Label ID="lblNoImages" runat="server" CssClass="Normal" />
							</div>
						</td>
					</tr>
				</table>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="phExternalImage" runat="server">
				<br />
				<dnn:SectionHead ID="dshExternalImage" CssClass="Head" runat="server" Text="External Image" Section="tblExternalImage"
					IncludeRule="True" />
				<table id="tblExternalImage" cellspacing="0" cellpadding="2" width="100%" summary="External Image Design Table"
					border="0" runat="server">
					<tr>
						<td class="SubHead" width="150">
							<dnn:Label ID="plImageUrl" Text="Image:" runat="server" ControlName="txtImageExternal"></dnn:Label>
						</td>
						<td>
							<asp:TextBox ID="txtImageExternal" CssClass="NormalTextBox" Width="300" MaxLength="255" runat="server" /></td>
					</tr>
				</table>
			</asp:PlaceHolder>
		</td>
	</tr>
</table>
<Ventrian:RefreshControl ID="cmdRefreshPhotos" runat="server" Text="Refresh Photos" Visible="false" />
