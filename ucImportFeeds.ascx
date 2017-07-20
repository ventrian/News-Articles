<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucImportFeeds.ascx.vb" Inherits="Ventrian.NewsArticles.ucImportFeeds" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<article:Header id="ucHeader1" SelectedMenu="AdminOptions" runat="server" MenuPosition="Top"></article:Header>

<asp:datagrid id="grdFeeds" Border="0" CellPadding="4" CellSpacing="0" Width="100%" AutoGenerateColumns="false" EnableViewState="false" runat="server" summary="Feeds Design Table" GridLines="None">
	<Columns>
		<asp:TemplateColumn ItemStyle-Width="20">
			<ItemTemplate>
				<asp:HyperLink NavigateUrl='<%# NavigateURL(Me.TabID, "ImportFeed", "mid=" & Me.ModuleID.ToString(), "FeedID=" & DataBinder.Eval(Container.DataItem,"FeedID").ToString()) %>' runat="server">
				<asp:Image ImageUrl="~/images/edit.gif" AlternateText="Edit" runat="server" ID="Hyperlink1Image" resourcekey="Edit"/></asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn HeaderText="Title" DataField="Title" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" />
		<asp:BoundColumn HeaderText="Url" DataField="Url" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" />
		<asp:BoundColumn HeaderText="IsActive" DataField="IsActive" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" />
	</Columns>
</asp:datagrid>

<asp:Label ID="lblNoFeeds" Runat="server" CssClass="Normal" />
<p>
	<asp:linkbutton id="cmdAddFeed" resourcekey="AddFeed" runat="server" cssclass="CommandButton" text="Add Feed" causesvalidation="False" borderstyle="none" />
</p>
<asp:PlaceHolder ID="phScheduler" runat="server">
<div align="left">
<dnn:sectionhead id="dshSchedulerSettings" cssclass="Head" runat="server" text="Scheduler Settings"
	section="tblSchedulerSettings" resourcekey="SchedulerSettings" includerule="True"></dnn:sectionhead>
<table id="tblSchedulerSettings" cellSpacing="0" cellPadding="2" width="100%" summary="Scheduler Settings Details Design Table"
	border="0" runat="server">
	<tr>
		<td colSpan="3">
			<asp:label id="lblSchedulerSettingsHelp" cssclass="Normal" runat="server" resourcekey="SchedulerSettingsHelp"
				enableviewstate="False"></asp:label></td>
	</tr>
	<tr>
		<td width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="200"><dnn:label id="plSchedulerEnabled" text="Enabled?" runat="server" controlname="chkEnabled"></dnn:label></td>
		<td valign="top">
			<asp:CheckBox ID="chkEnabled" Runat="server" />
		</td>
	</tr>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" width="200">
			<dnn:label id="plTimeLapse" runat="server" controlname="txtTimeLapse" suffix=":" text="Time Lapse:"></dnn:label></TD>
		<TD class="Normal">
			<asp:textbox id="txtTimeLapse" runat="server" maxlength="10" width="50" cssclass="NormalTextBox"></asp:textbox>
			<asp:dropdownlist id="drpTimeLapseMeasurement" runat="server" cssclass="NormalTextBox">
				<asp:listitem resourcekey="Seconds" value="s">Seconds</asp:listitem>
				<asp:listitem resourcekey="Minutes" value="m">Minutes</asp:listitem>
				<asp:listitem resourcekey="Hours" value="h">Hours</asp:listitem>
				<asp:listitem resourcekey="Days" value="d">Days</asp:listitem>
			</asp:dropdownlist></TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" width="200">
			<dnn:label id="plRetryTimeLapse" runat="server" controlname="txtRetryTimeLapse" suffix=":"
				text="Retry Frequency:"></dnn:label></TD>
		<TD class="Normal">
			<asp:textbox id="txtRetryTimeLapse" runat="server" maxlength="10" width="50" cssclass="NormalTextBox"></asp:textbox>
			<asp:dropdownlist id="drpRetryTimeLapseMeasurement" runat="server" cssclass="NormalTextBox">
				<asp:listitem resourcekey="Seconds" value="s">Seconds</asp:listitem>
				<asp:listitem resourcekey="Minutes" value="m">Minutes</asp:listitem>
				<asp:listitem resourcekey="Hours" value="h">Hours</asp:listitem>
				<asp:listitem resourcekey="Days" value="d">Days</asp:listitem>
			</asp:dropdownlist></TD>
	</TR>
	<tr>
		<td width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="200"><dnn:label id="plDeleteArticles" text="Delet Articles?" runat="server" controlname="chkDeleteArticles"></dnn:label></td>
		<td valign="top">
			<asp:CheckBox ID="chkDeleteArticles" Runat="server" />
		</td>
	</tr>
</TABLE>
<p align="center">
	<asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" cssclass="CommandButton" text="Update"
		borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel"
		causesvalidation="False" borderstyle="none" />
</p>
</div>
<div align="left">
<dnn:sectionhead id="dshSchedulerHistory" cssclass="Head" runat="server" text="Scheduler History"
	section="tblSchedulerHistory" resourcekey="SchedulerHistory" includerule="True"></dnn:sectionhead>
<table id="tblSchedulerHistory" cellSpacing="0" cellPadding="2" width="100%" summary="Scheduler History Details Design Table"
	border="0" runat="server">
	<tr>
		<td colSpan="3">
			<asp:label id="lblSchedulerHistoryHelp" cssclass="Normal" runat="server" resourcekey="SchedulerHistoryHelp"
				enableviewstate="False"></asp:label></td>
	</tr>
	<tr>
		<td>
			<asp:Label ID="lblNoHistory" Runat="server" CssClass="Normal" ResourceKey="NoHistory" />
			<asp:datagrid id="dgScheduleHistory" runat="server" autogeneratecolumns="false" cellpadding="4"
				cellspacing="2" datakeyfield="ScheduleID" enableviewstate="false" border="1" summary="This table shows the schedule of events for the portal."
				BorderColor="gray" BorderStyle="Solid" BorderWidth="1px" GridLines="Both">
				<Columns>
					<asp:TemplateColumn HeaderText="Description">
						<HeaderStyle CssClass="NormalBold" />
						<ItemStyle CssClass="Normal" VerticalAlign="Top" />
						<ItemTemplate>
							<table border="0" width="100%">
								<tr>
									<td nowrap Class="Normal">
										<i>
											<%# DataBinder.Eval(Container.DataItem,"TypeFullName")%>
										</i>
									</td>
								</tr>
							</table>
							<asp:Label runat=server visible='<%# DataBinder.Eval(Container.DataItem,"LogNotes")<>""%>' ID="Label1" NAME="Label1">
								<textarea rows="2" cols="65"><%# DataBinder.Eval(Container.DataItem,"LogNotes")%></textarea>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="ElapsedTime" HeaderText="Duration">
						<HeaderStyle CssClass="NormalBold"></HeaderStyle>
						<ItemStyle Wrap="False" CssClass="Normal" VerticalAlign="Top"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Succeeded" HeaderText="Succeeded">
						<HeaderStyle CssClass="NormalBold"></HeaderStyle>
						<ItemStyle Wrap="False" CssClass="Normal" VerticalAlign="Top"></ItemStyle>
					</asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Start/End/Next Start">
						<HeaderStyle CssClass="NormalBold"></HeaderStyle>
						<ItemStyle Wrap="False" CssClass="Normal" VerticalAlign="Top"></ItemStyle>
						<ItemTemplate>
							S:&nbsp;<%# DataBinder.Eval(Container.DataItem,"StartDate")%>
							<br>
							E:&nbsp;<%# DataBinder.Eval(Container.DataItem,"EndDate")%>
							<br>
							N:&nbsp;<%# DataBinder.Eval(Container.DataItem,"NextStart")%>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:datagrid>
		</td>
	</tr>
</table>
</div>
</asp:PlaceHolder>
<asp:Label ID="lblScheduler" runat="server" CssClass="Normal" Visible="false" />