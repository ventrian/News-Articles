<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UploadImages.ascx.vb" Inherits="Ventrian.NewsArticles.Controls.UploadImages" %>
<%@ Register TagPrefix="Ventrian" Assembly="Ventrian.NewsArticles" Namespace="Ventrian.NewsArticles.Components.WebControls" %>

<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>

<script type="text/javascript">

    jQuery(document).ready(function() {
        UploadImages();
        
        jQuery('#<%= drpUploadImageFolder.ClientID %>').change(function() {
          var paramsImages = getParamsImagesObject();
          swfu.setPostParams(paramsImages);
        });
    });
    
    
	var swfu;
	
	function getParamsImagesObject() {
		var params = {};
		params["ArticleID" ] = '<%= GetArticleID() %>';
		params["TabID" ] = '<%= GetArticleID() %>';
		params["ModuleID" ] = '<asp:literal id="litModuleID" runat="server" />';
		params["TabModuleID" ] = '<asp:literal id="litTabModuleID" runat="server" />';
		params["Ticket" ] = '<asp:literal id="litTicketID" runat="server" />';
		params["ArticleGuid" ] = '<asp:literal id="litArticleGuid" runat="server" />';
		var imagesFolderID = jQuery('#<%= drpUploadImageFolder.ClientID %>').find('option:selected').val();
		params["FolderID" ] = imagesFolderID;
		return params;
	}
	
	function UploadImages() {
	    swfu = new SWFUpload({
		    // Backend Settings
		    upload_url: "<%= GetUploadUrl() %>",	// Relative to the SWF file
		    post_params: getParamsImagesObject(),	// Relative to the SWF file

		    // File Upload Settings
		    file_size_limit : "<%= GetMaximumFileSize() %>",	
		    file_types : "*.gif;*.jpg;*.png;",
		    file_types_description : "Common Web Image Formats (gif, jpg, png)",
		    file_upload_limit : 0,    // Zero means unlimited

		    // Event Handler Settings - these functions as defined in Handlers.js
		    //  The handlers are not part of SWFUpload but are part of my website and control how
		    //  my website reacts to the SWFUpload events.
		    file_queue_error_handler : fileQueueError,
		    file_dialog_complete_handler : fileDialogComplete,
		    upload_progress_handler : uploadProgress,
		    upload_error_handler : uploadError,
		    upload_success_handler : uploadSuccess,
		    upload_complete_handler : uploadComplete,

		    // Button Settings
		    button_image_url : '<%= ResolveUrl("../Images/Uploader/XPButtonNoText_160x22.png") %>',	// Relative to the SWF file
		    button_placeholder_id : "spanButtonPlaceholder",
		    button_width: 160,
		    button_height: 22,
		    button_text : '<span class="button"><asp:Label ID="lblSelectImages" Runat="server" /></span>',
		    button_text_style : '.button { font-family: Tahoma,Arial,Helvetica; font-size: 11px; font-weight:bold; text-align: center; }',
		    button_text_top_padding: 2,
		    button_text_left_padding: 5,

		    // Flash Settings
		    flash_url : '<%= ResolveUrl("../Includes/Uploader/swfupload.00.07.61.swf") %>',	// Relative to this file

		    custom_settings : {
			    upload_target : "pa_progress_container",
			    image_path : '<%= ResolveUrl("Images/Uploader/") %>',
			    upload_type : 'Image'
		    },
    		
		    // Debug Settings
		    debug: false
	    });
	}
</script>
<br />
<dnn:sectionhead id="dshImages" cssclass="Head" runat="server" text="Images" section="tblImages" IsExpanded="false" 
	includerule="True"></dnn:sectionhead>
<table id="tblImages" cellspacing="0" cellpadding="2" width="100%" summary="Images Design Table"
	border="0" runat="server">
	<tr>
		<td colspan="3">
			<asp:label id="lblImagesHelp" cssclass="Normal" runat="server" enableviewstate="False" /></td>
	</tr>
	<tr>
		<td width="25"></td>
		<td colspan="2">
            <asp:PlaceHolder ID="phImages" runat="server">
            <table width="100%">
            <tr>
                <td valign="top" id="trUpload" runat="server">
		            <dnn:sectionhead id="dshUploadImages" cssclass="Head" runat="server" text="Images" section="tblUploadImages"
	                    includerule="True" />
                    <table id="tblUploadImages" cellspacing="0" cellpadding="2" width="100%" summary="Images Design Table"
	                    border="0" runat="server">
		                <tr>
			                <td class="SubHead" width="150"><dnn:label id="plFolder" text="Folder:" runat="server" controlname="drpUploadImagesFolder"></dnn:label></td>
		                    <td>&nbsp;</td>
		                </tr>
	                    <tr>
	                        <td>
	                            <asp:DropDownList ID="drpUploadImageFolder" runat="server" CssClass="NormalTextBox" Width="275px" />
                                <div id="pa_upload_container" style="margin: 0px 10px;">
                                    <div>
                                        <span id="spanButtonPlaceholder"></span>
                                    </div>
	                                <div id="pa_progress_container" class="Normal" style="margin-top: 10px;"></div>
                                </div>
	                        </td>
		                </tr>
		            </table>   
                </td>
                <td valign="top" id="trExisting" runat="server">
		            <dnn:sectionhead id="dshExistingImages" cssclass="Head" runat="server" text="Select Existing Images" section="tblExistingImages"
	                    includerule="True" />
                    <table id="tblExistingImages" cellspacing="0" cellpadding="2" width="100%" summary="Existing Images Design Table"
	                    border="0" runat="server">
	                    <tr>
	                        <td>
	                            <table>
	                            <tr>
	                                <td><dnn:url id="ctlImage" runat="server" width="275" Required="False" showtrack="False" shownewwindow="False"
							        showlog="False" urltype="F" showUrls="False" showfiles="True" showtabs="False" ShowUpLoad="false" FileFilter=".jpg,.gif,.png"></dnn:url></td>
							        <td valign="top"><br />&nbsp;<asp:LinkButton ID="cmdAddExistingImage" runat="server" ResourceKey="cmdAddExistingImage" CausesValidation="false" CssClass="CommandButton" /></td>
	                            </tr>
	                            </table>
						    </td>
		                </tr>
		            </table>   
                </td>
            </tr>
            </table>
		    <br /> 
		    <dnn:sectionhead id="dshSelectedImages" cssclass="Head" runat="server" text="Attached Images" section="tblSelectedImages"
	            includerule="True" />
            <table id="tblSelectedImages" cellspacing="0" cellpadding="2" width="100%" summary="Selected Images Design Table"
	            border="0" runat="server">
	            <tr>
	                <td>
	                    <asp:DataList CellPadding="4" CellSpacing="0" ID="dlImages" Runat="server" RepeatColumns="4" RepeatDirection="Horizontal" DataKeyField="ImageID">
				            <ItemStyle CssClass="Normal" HorizontalAlign="Center" VerticalAlign="Top" />
				            <ItemTemplate>
					            <img src='<%# GetImageUrl(Container.DataItem) %>' alt="Photo"><br />
					            <b>
						            <%#DataBinder.Eval(Container.DataItem, "Title")%>
					            </b>
					            <br />
					            <asp:ImageButton ID="btnEdit" Runat="server" CommandName="Edit" CausesValidation="False" ImageUrl="~/Images/edit.gif" />
					            <asp:ImageButton ID="btnDelete" Runat="server" CommandName="Delete" CausesValidation="False" ImageUrl="~/Images/delete.gif" />
					            <asp:ImageButton ID="btnBottom" Runat="server" ImageUrl="~/Images/bottom.gif" />
					            <asp:ImageButton ID="btnDown" Runat="server" ImageUrl="~/Images/dn.gif" />
					            <asp:ImageButton ID="btnUp" Runat="server" ImageUrl="~/Images/up.gif" />
					            <asp:ImageButton ID="btnTop" Runat="server" ImageUrl="~/Images/top.gif" />
				            </ItemTemplate>
				            <EditItemTemplate>
					            <img src='<%# GetImageUrl(Container.DataItem) %>' alt="Photo"><br />
					            <asp:TextBox id="txtTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Title")  %>' Width="160" MaxLength="255" />
                                <br />
					            <asp:TextBox id="txtDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description")  %>' Width="160" TextMode="MultiLine" Rows="4" />
                                <br />
					            <asp:LinkButton id="btnUpdate" runat="server" Text="Update" 
                                    CommandName="Update" ResourceKey="cmdUpdate" CausesValidation="false" />
                                <asp:LinkButton id="btnCancel" runat="server" Text="Cancel" 
                                    CommandName="Cancel" ResourceKey="cmdCancel" CausesValidation="false" />
				            </EditItemTemplate>
			            </asp:DataList>
			            <div align="left"><asp:Label ID="lblNoImages" Runat="server" CssClass="Normal" /></div>
	                </td>
	            </tr>
            </table>
            </asp:PlaceHolder>      
            <asp:PlaceHolder ID="phExternalImage" runat="server">
		    <br />
		    <dnn:sectionhead id="dshExternalImage" cssclass="Head" runat="server" text="External Image" section="tblExternalImage"
	            includerule="True" />
            <table id="tblExternalImage" cellspacing="0" cellpadding="2" width="100%" summary="External Image Design Table"
	            border="0" runat="server">
	            <tr>
	                <td class="SubHead" width="150"><dnn:label id="plImageUrl" text="Image:" runat="server" controlname="txtImageExternal"></dnn:label></td>
	                <td><asp:textbox id="txtImageExternal" cssclass="NormalTextBox" width="300" maxlength="255" runat="server" /></td>
	            </tr>
            </table>	
            </asp:PlaceHolder>                
        </td>
    </tr>
</table>    
<script type="text/javascript">
    function articleQueueComplete() {
	    eval("<%= GetPostBackReference() %>");
    }
</script>
<Ventrian:RefreshControl ID="cmdRefreshPhotos" runat="server" Text="Refresh Photos" Visible="false" />