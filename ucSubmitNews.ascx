<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucSubmitNews.ascx.vb" Inherits="Ventrian.NewsArticles.ucSubmitNews" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register Assembly="Ventrian.NewsArticles" Namespace="Ventrian.NewsArticles.Components.Validators" TagPrefix="Ventrian" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<%@ Register TagPrefix="article" TagName="UploadFiles" Src="Controls/UploadFiles.ascx" %>
<%@ Register TagPrefix="article" TagName="UploadImages" Src="Controls/UploadImages.ascx" %>

<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<article:Header runat="server" id="ucHeader1" SelectedMenu="SubmitArticle" MenuPosition="Top" />

<asp:PlaceHolder ID="phMirrorText" runat="server" Visible="false">
    <div align="center"><asp:Label ID="lblMirrorText" runat="server" cssclass="Normal" /></div>
</asp:PlaceHolder>

<asp:PlaceHolder ID="phMirror" runat="server">
<dnn:sectionhead id="dshMirror" cssclass="Head" runat="server" text="Mirror" section="tblMirror" resourcekey="Mirror" includerule="True" />
<table class="Settings" cellspacing="2" cellpadding="2" summary="Mirror Design Table" border="0" id="tblMirror" runat="Server" width="100%">
<tr>
	<td colspan="2"><asp:label id="lblMirrorHelp" cssclass="Normal" runat="server" resourcekey="MirrorHelp"
			enableviewstate="False">In this section, you can specify your content.</asp:label></td>
</tr>
<tr>
    <td><asp:Image id="Image1" runat="Server" ImageUrl="~/Images/Spacer.gif" Width="25" Height="1" /></td>
	<td valign="top" width="100%">
		<table id="Table2" cellspacing="2" cellpadding="2" summary="Mirror Design Table" border="0" width="100%">
		<tr>
			<td class="SubHead" width="150"><dnn:label id="plMirrorArticle" text="Mirror Article?" runat="server" controlname="chkMirrorArticle"></dnn:label></td>
			<td>    
			    <asp:CheckBox ID="chkMirrorArticle" runat="server" AutoPostBack="true" />
			</td>
		</tr>
		<tr runat="server" id="trMirrorModule" visible="false">
			<td class="SubHead" width="150"><dnn:label id="plMirrorModule" text="Select Module:" runat="server" controlname="drpMirrorModule"></dnn:label></td>
			<td>    
			    <asp:DropDownList ID="drpMirrorModule" runat="server" CssClass="NormalTextBox" DataTextField="Title" DataValueField="LinkedID" Width="400px" AutoPostBack="true" />
			</td>
		</tr>
		<tr runat="server" id="trMirrorArticle" visible="false">
			<td class="SubHead" width="150"><dnn:label id="plMirrorArticleSelect" text="Select Article:" runat="server" controlname="drpMirrorArticle"></dnn:label></td>
			<td>    
			    <asp:DropDownList ID="drpMirrorArticle" runat="server" CssClass="NormalTextBox" DataTextField="TitleAlternate" DataValueField="ArticleID" Width="400px" />
			    <asp:requiredfieldvalidator id="valMirrorArticle" runat="server" cssclass="NormalRed" resourcekey="valMirrorArticle.ErrorMessage"
					display="None" errormessage="<br>You must select an article." controltovalidate="drpMirrorArticle" SetFocusOnError="True"></asp:requiredfieldvalidator>
			</td>
		</tr>
		<tr runat="server" id="trMirrorAutoUpdate" visible="false">
			<td class="SubHead" width="150"><dnn:label id="plMirrorAutoUpdate" text="Select Module:" runat="server" controlname="chkMirrorAutoUpdate"></dnn:label></td>
			<td>    
			    <asp:CheckBox ID="chkMirrorAutoUpdate" runat="server" />
			</td>
		</tr>
		</table>
    </td>
</tr>
</table>
<br />
</asp:PlaceHolder>

<asp:PlaceHolder ID="phCreate" runat="server">
<dnn:sectionhead id="dshCreate" cssclass="Head" runat="server" text="Create" section="tblCreate" resourcekey="Create" includerule="True" />
<table class="Settings" cellspacing="2" cellpadding="2" summary="Create Design Table" border="0" id="tblCreate" runat="Server" width="100%">
<tr>
	<td colspan="2"><asp:label id="lblCreateHelp" cssclass="Normal" runat="server" resourcekey="CreateHelp"
			enableviewstate="False">In this section, you can specify your content.</asp:label></td>
</tr>
<tr>
    <td><asp:Image id="imgSpacer1" runat="Server" ImageUrl="~/Images/Spacer.gif" Width="25" Height="1" /></td>
	<td valign="top" width="100%">
		<table id="tblArticle" cellspacing="2" cellpadding="2" summary="Article Design Table" border="0" width="100%">
		<tr>
			<td class="SubHead" width="150"><dnn:label id="plTitle" text="Title:" runat="server" controlname="txtTitle"></dnn:label></td>
			<td>    
			    <asp:textbox id="txtTitle" width="350px" runat="server" cssclass="NormalTextBox" maxlength="255"></asp:textbox>
				<asp:requiredfieldvalidator id="valTitle" runat="server" cssclass="NormalRed" resourcekey="valTitle.ErrorMessage"
					display="None" errormessage="<br>Title Is Required" controltovalidate="txtTitle" SetFocusOnError="True"></asp:requiredfieldvalidator>
            </td>
		</tr>
		<tr>
			<td colspan="2" class="SubHead">
			    <dnn:label id="plBody" text="Body:" runat="server" controlname="txtDetails"></dnn:label>
			</td>
		</tr>
		<tr>
		    <td colspan="2" valign="top">
				<dnn:texteditor id="txtDetails" runat="server" height="400" width="450"></dnn:texteditor>
				<asp:requiredfieldvalidator id="valBody" runat="server" cssclass="NormalRed" resourcekey="valBody.ErrorMessage"
					display="None" errormessage="<br>Body Is Required" controltovalidate="txtDetails" SetFocusOnError="True"></asp:requiredfieldvalidator>
		    </td>
		</tr>
		<tr>
		    <td colspan="2" valign="top">
	            <asp:PlaceHolder ID="phAttachment" runat="Server">
		        <dnn:sectionhead id="dshAttachment" cssclass="Head" runat="server" text="Attachments" section="tblAttachment"
			        resourcekey="Attachment" includerule="True" IsExpanded="False" />
	            <table id="tblAttachment" cellspacing="2" cellpadding="2" summary="Attachment Design Table" border="0" runat="server">
	            <tr id="trLink" runat="Server">
		            <td class="SubHead" width="150"><dnn:label id="plLink" text="Link:" runat="server" controlname="ctlURL"></dnn:label></td>
	                <td>
						<dnn:url id="ctlUrlLink" runat="server" width="300" showtrack="False" showlog="False" shownone="True"
							shownewwindow="False" ShowFiles="false"></dnn:url>
	                </td>
	            </tr>
	            <tr id="trNewWindow" runat="Server">
		            <td class="SubHead" width="150"><dnn:label id="plNewWindow" text="New Window?" runat="server" controlname="chkNewWindow"></dnn:label></td>
	                <td>
					    <asp:CheckBox id="chkNewWindow" Runat="server" Checked="False"></asp:CheckBox>
	                </td>
	            </tr>
	            </table>
	            </asp:PlaceHolder>
	            <article:UploadFiles runat="server" id="ucUploadFiles" />
	            <article:UploadImages runat="server" id="ucUploadImages" />
	            <asp:PlaceHolder ID="phExcerpt" runat="Server">
	            <br />
		        <dnn:sectionhead id="dshExcerpt" cssclass="Head" runat="server" text="Excerpt" section="tblExcerpt"
			        resourcekey="Excerpt" includerule="True" IsExpanded="False" />
	            <table id="tblExcerpt" cellspacing="2" cellpadding="2" summary="Excerpt Design Table" border="0" runat="server" width="100%">
	            <tr>
		            <td>
		                <asp:TextBox ID="txtExcerptBasic" runat="server" Height="400" Width="450" TextMode="MultiLine" />
		                <dnn:texteditor id="txtExcerptRich" runat="server" height="400" width="450"></dnn:texteditor>
                    </td>
	            </tr>
	            </table>
	            </asp:PlaceHolder>
	            <asp:PlaceHolder ID="phMeta" runat="Server">
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
	            <tr>
		            <td class="SubHead" width="150"><dnn:label id="plPageHeadText" runat="server" suffix=":" controlname="txtPageHeadText"></dnn:label></td>
	                <td>
					    <asp:TextBox id="txtPageHeadText" cssclass="NormalTextBox" runat="server" maxlength="500" width="300" textmode="MultiLine"
											rows="4"></asp:TextBox>
	                </td>
	            </tr>
	            </table>
	            </asp:PlaceHolder>
	            <asp:PlaceHolder ID="phCustomFields" runat="Server">
	            <br />
		        <dnn:sectionhead id="dshCustomFields" cssclass="Head" runat="server" text="Custom Fields" section="tblCustomFields"
			        resourcekey="CustomFields" includerule="False" IsExpanded="True" />
	            <table id="tblCustomFields" cellspacing="2" cellpadding="2" summary="Custom Fields Design Table" border="0" runat="server">
	            <tr>
	                <td>
	            <table id="Table1" cellspacing="2" cellpadding="2" summary="Custom Fields Design Table" border="0">
	                <asp:repeater id="rptCustomFields" Runat="server">
					<ItemTemplate>
						<tr valign="top" runat="server" id="trItem">
							<td class="SubHead" width="150" valign="middle">
								<label id="label" runat="server">
									<asp:linkbutton id="cmdHelp" tabindex="-1" runat="server" CausesValidation="False" enableviewstate="False">
										<asp:image id="imgHelp" tabindex="-1" runat="server" imageurl="~/images/help.gif" enableviewstate="False"></asp:image>
									</asp:linkbutton>
									<asp:label id="lblLabel" runat="server" enableviewstate="False"></asp:label>
								</label>
								<asp:panel id="pnlHelp" runat="server" cssClass="Help" enableviewstate="False">
									<asp:label id="lblHelp" runat="server" enableviewstate="False"></asp:label>
								</asp:panel>
							</td>
							<td align="left">
								<asp:PlaceHolder ID="phValue" Runat="server" />
							</td>
						</tr>
					</ItemTemplate>
				</asp:repeater>
				</table>
	                </td>
	            </tr>
	            
	            </table>
	            </asp:PlaceHolder>
		    </td>
		</tr>
		</table>
	</td>
</tr>
</table>		
<br />
</asp:PlaceHolder>
<asp:PlaceHolder id="phOrganize" runat="Server">
<dnn:sectionhead id="dshOrganize" cssclass="Head" runat="server" text="Organize" section="tblOrganize"
	resourcekey="Organize" includerule="True" />
<table class="Settings" cellspacing="2" cellpadding="2" summary="Organize Design Table" border="0" id="tblOrganize" runat="Server" width="100%">
<tr>
	<td colspan="2"><asp:label id="plOrganizeHelp" cssclass="Normal" runat="server" resourcekey="OrganizeHelp"
			enableviewstate="False">In this section, you can organize your content.</asp:label></td>
</tr>
<tr>
    <td><asp:Image id="imgSpacer2" runat="Server" ImageUrl="~/Images/Spacer.gif" Width="25" Height="1" /></td>
	<td valign="top" width="100%">
		<table id="tblOrganizeDetail" cellspacing="2" cellpadding="2" summary="Organize Detail Design Table" border="0" width="100%">
		<tr runat="Server" id="trCategories">
			<td class="SubHead" width="150"><dnn:label id="plCategories" text="Categories:" runat="server" controlname="lstCategories"></dnn:label></td>
			<td>    
			    <asp:ListBox ID="lstCategories" runat="server" CssClass="Normal" DataTextField="NameIndented" 
			        DataValueField="CategoryID" Width="300px" SelectionMode="Multiple" />
			    <asp:RequiredFieldValidator ID="valCategory" runat="server" ControlToValidate="lstCategories" 
			        Display="None" ErrorMessage="Category is Required." Enabled="False" ResourceKey="valCategoriesRequired.ErrorMessage" SetFocusOnError="true" />
			</td>
		</tr>
		<tr runat="Server" id="trTags">
			<td class="SubHead" id="tdTxtTagsTitle" width="150"><dnn:label id="plTags" text="Tags:" runat="server" controlname="txtTags"></dnn:label></td>
			<td runat="server" id="tdTxtTags">
				<asp:textbox id="txtTags" cssclass="NormalTextBox" width="300" maxlength="255" runat="server" /><br />
			    <asp:Label ID="lblTags" ResourceKey="TagsHelp" runat="server" CssClass="Normal" />
			</td>
			<td class="SubHead" id="tdAllTagsTitle" width="150"><dnn:label id="plAllTags" text="All Tags:" runat="server"></dnn:label></td>
			<td runat="server" id="tdAllTagsList">
				<asp:ListBox ID="lstTags" runat="server" CssClass="Normal" DataTextField="Name" DataValueField="Name" Width="300px" Height="150px" SelectionMode="Multiple" />
				<asp:imagebutton id="addTags" resourcekey="btnAddTags.Help" runat="server" alternatetext="Add Tags To Article" commandname="add" imageurl="~/images/action_import.gif"></asp:imagebutton>
			</td>
			<td runat="server" id="tdStaticTagsList"> 
				<dnn:label id="plArticleTags" text="Article Tags:" runat="server" controlname="txtArticleTags"></dnn:label>
				<asp:ListBox ID="lstFinalTags" runat="server" CssClass="Normal" DataTextField="Name" DataValueField="Name" Width="300px" Height="150px" SelectionMode="Single" />

				<asp:imagebutton id="cmdUp" resourcekey="cmdUp.Help" runat="server" alternatetext="Move Tag Up In Tags List" commandname="up" imageurl="~/images/up.gif"></asp:imagebutton>
				<asp:imagebutton id="cmdDown" resourcekey="cmdDown.Help" runat="server" alternatetext="Move Tag Down In Tags List" commandname="down" imageurl="~/images/dn.gif"></asp:imagebutton>
				<asp:imagebutton id="cmdDeleteTag" resourcekey="cmdDeleteTag.Help" runat="server" alternatetext="Delete Tag From Tags List" commandname="cmdDeleteTag" imageurl="~/images/action_delete.gif"></asp:imagebutton>
			</td>
		</tr>
		</table>
    </td>
</tr>		
</table>
<br />
</asp:PlaceHolder>
<asp:PlaceHolder ID="phPublish" runat="Server">
<dnn:sectionhead id="dshPublish" cssclass="Head" runat="server" text="Publish" section="tblPublish"
	resourcekey="Publish" includerule="True" />
<table class="Settings" cellspacing="2" cellpadding="2" summary="Publish Design Table" border="0" id="tblPublish" runat="Server" width="100%">
<tr>
	<td colspan="2"><asp:label id="plPublishHelp" cssclass="Normal" runat="server" resourcekey="PublishHelp"
			enableviewstate="False">In this section, you can specify how and when your content is published.</asp:label></td>
</tr>
<tr>
    <td><asp:Image id="imgSpacer3" runat="Server" ImageUrl="~/Images/Spacer.gif" Width="25" Height="1" /></td>
	<td valign="top" width="100%">
		<table id="tblPublishDetail" cellspacing="2" cellpadding="2" summary="Publish Detail Design Table" border="0" width="100%">
		<tr id="trAuthor" runat="Server">
			<td class="SubHead" width="150"><dnn:label id="plAuthor" text="Author:" runat="server" controlname="lblAuthor"></dnn:label></td>
			<td>
				<asp:Label ID="lblAuthor" Runat="server" CssClass="Normal" />
				<asp:LinkButton ID="cmdSelectAuthor" runat="server" ResourceKey="cmdSelectAuthor" CssClass="CommandButton"
					CausesValidation="False" />
				<asp:DropDownList ID="ddlAuthor" Runat="server" Visible="False" DataTextField="DisplayName" DataValueField="UserID" />	
				<asp:Panel ID="pnlAuthor" runat="server" Visible="false">
                    <asp:TextBox ID="txtAuthor" runat="server" CssClass="NormalTextBox" Width="150px" />
                    <asp:Label ID="lblAuthorUsername" runat="server" ResourceKey="AuthorUsername" CssClass="Normal" />
                    <asp:CustomValidator id="valAuthor" runat="server" CssClass="NormalRed" ErrorMessage="<br />Invalid username."
								Display="Dynamic" ResourceKey="valAuthor.ErrorMessage" ControlToValidate="txtAuthor" />
                </asp:Panel>		
			</td>
		</tr>
		<tr id="trFeatured" runat="Server">
			<td class="SubHead" width="150"><dnn:label id="plFeatured" text="Featured:" runat="server" controlname="chkFeatured"></dnn:label></td>
			<td>    
			    <asp:CheckBox id="chkFeatured" Runat="server"></asp:CheckBox>
            </td>
		</tr>
		<tr id="trSecure" runat="Server">
			<td class="SubHead" width="150"><dnn:label id="plSecure" text="Secure:" runat="server" controlname="chkSecure"></dnn:label></td>
			<td>    
			    <asp:CheckBox id="chkSecure" Runat="server"></asp:CheckBox>
            </td>
		</tr>
		<tr id="trPublish" runat="Server">
			<td class="SubHead" width="150"><dnn:label id="plStartDate" text="Start Date:" runat="server" controlname="txtPublishDate"></dnn:label></td>
			<td>    	
			    <asp:textbox id="txtPublishDate" cssclass="NormalTextBox" runat="server" width="150" maxlength="15"></asp:textbox>
				<asp:hyperlink id="cmdPublishCalendar" cssclass="CommandButton" runat="server" resourcekey="Calendar">Calendar</asp:hyperlink>
				<asp:requiredfieldvalidator id="valPublishDateRequired" runat="server" cssclass="NormalRed" resourcekey="valStartDateRequired.ErrorMessage"
				    display="None" errormessage="<br>Publish Date Is Required" controltovalidate="txtPublishDate" SetFocusOnError="True"></asp:requiredfieldvalidator>
				<asp:comparevalidator id="valPublishDate" cssclass="NormalRed" runat="server" controltovalidate="txtPublishDate"
				    errormessage="<br>Invalid publish date!" operator="DataTypeCheck" type="Date" display="None" ResourceKey="valStartDate.ErrorMessage" SetFocusOnError="True"></asp:comparevalidator>
                <span class="Normal">@&nbsp;<asp:TextBox ID="txtPublishHour" runat="Server" MaxLength="2" Width="25px" CssClass="NormalTextBox" />&nbsp;:&nbsp;<asp:TextBox ID="txtPublishMinute" runat="Server" MaxLength="2" Width="25px" CssClass="NormalTextBox" /></span>
		    </td>
		</tr>
		<tr id="trExpiry" runat="Server">
			<td class="SubHead" width="150"><dnn:label id="plEndDate" text="End Date:" runat="server" controlname="txtEndDate"></dnn:label></td>
		    <td>
        		<asp:textbox id="txtExpiryDate" cssclass="NormalTextBox" runat="server" width="150" maxlength="15"></asp:textbox>
				<asp:hyperlink id="cmdExpiryCalendar" cssclass="CommandButton" runat="server" resourcekey="Calendar">Calendar</asp:hyperlink>
				<asp:comparevalidator id="valExpiryDate" cssclass="NormalRed" runat="server" controltovalidate="txtExpiryDate"
					errormessage="<br>Invalid expiry date!" operator="DataTypeCheck" type="Date" display="None" ResourceKey="valEndDate.ErrorMessage" SetFocusOnError="True"></asp:comparevalidator>
		        <span class="Normal">@&nbsp;<asp:TextBox ID="txtExpiryHour" runat="Server" MaxLength="2" Width="25px" CssClass="NormalTextBox" />&nbsp;:&nbsp;<asp:TextBox ID="txtExpiryMinute" runat="Server" MaxLength="2" Width="25px" CssClass="NormalTextBox" /></span>
		    </td>
		</tr>
		</table>
    </td>
</tr>	
</table>
<br />
</asp:PlaceHolder>
<dnn:sectionhead id="dshAction" cssclass="Head" runat="server" text="Action" section="tblAction"
	resourcekey="Action" includerule="True" />
<table class="Settings" cellspacing="2" cellpadding="2" summary="Action Design Table" border="0" id="tblAction" runat="Server" width="100%">
<tr>
	<td colspan="2"><asp:label id="plAction" cssclass="Normal" runat="server" resourcekey="ActionHelp"
			enableviewstate="False">In this section, you can perform actions relating to the content.</asp:label></td>
</tr>
<tr>
    <td><asp:Image id="imgSpacer4" runat="Server" ImageUrl="~/Images/Spacer.gif" Width="25" Height="1" /></td>
	<td valign="top" width="100%">
		<table id="tblActionDetail" cellspacing="2" cellpadding="2" summary="Action Detail Design Table" border="0" width="100%">
        <tr>
	        <td class="SubHead" width="150"><dnn:label id="plStatus" text="Status:" runat="server" controlname="drpStatus"></dnn:label></td>
	        <td>
        	    <asp:DropDownList ID="drpStatus" runat="Server" CssClass="NormalTextBox" />
	        </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:LinkButton ID="cmdSaveArticle" runat="Server" CssClass="CommandButton" ResourceKey="cmdUpdate" />&nbsp;&nbsp;<asp:LinkButton ID="cmdPublishArticle" runat="Server" CssClass="CommandButton" ResourceKey="cmdPublish" />&nbsp;&nbsp;<asp:LinkButton ID="cmdAddEditPages" runat="Server" CssClass="CommandButton" ResourceKey="cmdAddEditPages" />&nbsp;&nbsp;<asp:LinkButton ID="cmdCancel" runat="Server" CssClass="CommandButton" ResourceKey="cmdCancel" CausesValidation="False" />&nbsp;&nbsp;<asp:LinkButton ID="cmdDelete" runat="Server" CssClass="CommandButton" ResourceKey="cmdDelete" CausesValidation="false" />
            </td>
        </tr>
        </table>
    </td>
</tr>
</table>
<asp:ValidationSummary ID="valMessageBox" Runat="server" ShowSummary="False" ShowMessageBox="True" CssClass="NormalRed" />

<article:Header runat="server" id="ucHeader2" SelectedMenu="SubmitArticle" MenuPosition="Bottom" />
