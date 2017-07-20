<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UploadFiles.ascx.vb" Inherits="Ventrian.NewsArticles.Controls.UploadFiles" %>
<%@ Register TagPrefix="Ventrian" Assembly="Ventrian.NewsArticles" Namespace="Ventrian.NewsArticles.Components.WebControls" %>

<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>

<script type="text/javascript">

    jQuery(document).ready(function() {
        UploadFiles();
        
        jQuery('#<%= drpUploadFilesFolder.ClientID %>').change(function() {
          var paramsFiles = getParamsFilesObject();
          swfuFiles.setPostParams(paramsFiles);
        });
    });
    
	var swfuFiles;
	
	function getParamsFilesObject() {
		var params = {};
		params["ArticleID" ] = '<%= GetArticleID() %>';
		params["TabID" ] = '<%= GetArticleID() %>';
		params["ModuleID" ] = '<asp:literal id="litModuleID" runat="server" />';
		params["TabModuleID" ] = '<asp:literal id="litTabModuleID" runat="server" />';
		params["Ticket" ] = '<asp:literal id="litTicketID" runat="server" />';
		params["ArticleGuid" ] = '<asp:literal id="litArticleGuid" runat="server" />';
		var filesFolderID = jQuery('#<%= drpUploadFilesFolder.ClientID %>').find('option:selected').val();
		params["FolderID" ] = filesFolderID;
		return params;
	}
	
	function UploadFiles() {
	    swfuFiles = new SWFUpload({
		    // Backend Settings
		    upload_url: "<%= GetUploadUrl() %>",	// Relative to the SWF file
		    post_params: getParamsFilesObject(),	// Relative to the SWF file

		    // File Upload Settings
		    file_size_limit : "<%= GetMaximumFileSize() %>",	
		    file_types : "*.*",
			file_types_description : "All Files",
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
		    button_placeholder_id : "spanButtonPlaceholderFiles",
		    button_width: 160,
		    button_height: 22,
		    button_text : '<span class="button"><asp:Label ID="lblSelectFiles" Runat="server" /></span>',
		    button_text_style : '.button { font-family: Tahoma,Arial,Helvetica; font-size: 11px; font-weight:bold; text-align: center; }',
		    button_text_top_padding: 2,
		    button_text_left_padding: 5,

		    // Flash Settings
		    flash_url : '<%= ResolveUrl("../Includes/Uploader/swfupload.00.07.61.swf") %>',	// Relative to this file

		    custom_settings : {
			    upload_target : "pa_progress_container_files",
			    image_path : '<%= ResolveUrl("Images/Uploader/") %>',
			    upload_type : 'File'
		    },
    		
		    // Debug Settings
		    debug: false
	    });
	}
</script>
<br />
<dnn:sectionhead id="dshFiles" cssclass="Head" runat="server" text="Images" section="tblFiles" IsExpanded="false" 
	includerule="True"></dnn:sectionhead>
<table id="tblFiles" cellspacing="0" cellpadding="2" width="100%" summary="Images Design Table"
	border="0" runat="server">
	<tr>
		<td colspan="3">
			<asp:label id="lblFilesHelp" cssclass="Normal" runat="server" enableviewstate="False" /></td>
	</tr>
	<tr>
		<td width="25"></td>
		<td colspan="2">
            <asp:PlaceHolder ID="phFiles" runat="server">
            <table width="100%">
            <tr>
                <td valign="top" id="trUpload" runat="server">
		            <dnn:sectionhead id="dshUploadFiles" cssclass="Head" runat="server" text="Images" section="tblUploadFiles"
	                    includerule="True" />
                    <table id="tblUploadFiles" cellspacing="0" cellpadding="2" width="100%" summary="Images Design Table"
	                    border="0" runat="server">
		                <tr>
			                <td class="SubHead" width="150"><dnn:label id="plFolder" text="Folder:" runat="server" controlname="drpUploadFilesFolder"></dnn:label></td>
			                <td>&nbsp;</td>
			            </tr>
	                    <tr>
	                        <td>
	                            <asp:DropDownList ID="drpUploadFilesFolder" runat="server" CssClass="NormalTextBox" Width="275px" />
                                <div id="pa_upload_container_files" style="margin: 0px 10px;">
                                    <div>
                                        <span id="spanButtonPlaceholderFiles"></span>
                                    </div>
	                                <div id="pa_progress_container_files" class="Normal" style="margin-top: 10px;"></div>
                                </div>
	                        </td>
		                </tr>
		            </table>   
                </td>
                <td valign="top" id="trExisting" runat="server">
		            <dnn:sectionhead id="dshExistingFiles" cssclass="Head" runat="server" text="Select Existing Files" section="tblExistingFiles"
	                    includerule="True" />
                    <table id="tblExistingFiles" cellspacing="0" cellpadding="2" width="100%" summary="Existing Files Design Table"
	                    border="0" runat="server">
	                    <tr>
	                        <td>
	                            <table>
	                            <tr>
	                                <td><dnn:url id="ctlFile" runat="server" width="275" Required="False" showtrack="False" shownewwindow="False"
							        showlog="False" urltype="F" showUrls="False" showfiles="True" showtabs="False" ShowUpLoad="false" /></td>
							        <td valign="top"><br />&nbsp;<asp:LinkButton ID="cmdAddExistingFile" runat="server" CausesValidation="false" CssClass="CommandButton" /></td>
	                            </tr>
	                            </table>
						    </td>
		                </tr>
		            </table>   
                </td>
            </tr>
            </table>
		    <br /> 
		    <dnn:sectionhead id="dshSelectedFiles" cssclass="Head" runat="server" text="Attached Files" section="tblSelectedFiles"
	            includerule="True" />
            <table id="tblSelectedFiles" cellspacing="0" cellpadding="2" width="100%" summary="Selected Files Design Table"
	            border="0" runat="server">
	            <tr>
	                <td>
	                    <asp:DataList CellPadding="4" CellSpacing="0" ID="dlFiles" Runat="server" RepeatColumns="4" RepeatDirection="Horizontal" DataKeyField="FileID">
				            <ItemStyle CssClass="Normal" HorizontalAlign="Center" />
				            <ItemTemplate>
					            <b>
						            <%#DataBinder.Eval(Container.DataItem, "Title")%>
					            </b>
					            <br />
					            <asp:ImageButton ID="btnEdit" Runat="server" CommandName="Edit" CausesValidation="False" ImageUrl="~/Images/edit.gif" />
					            <asp:ImageButton ID="btnDelete" Runat="server" CommandName="Delete" CausesValidation="False" ImageUrl="~/Images/delete.gif" />
					            <asp:ImageButton ID="btnDown" Runat="server" ImageUrl="~/Images/dn.gif" />
					            <asp:ImageButton ID="btnUp" Runat="server" ImageUrl="~/Images/up.gif" />
				            </ItemTemplate>
				            <EditItemTemplate>
					            <b>
						            <%#DataBinder.Eval(Container.DataItem, "Title")%>
					            </b>
					            <br />
					            <asp:TextBox id="txtTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Title")  %>' Width="125" MaxLength="100" />
                                <br />
                                <asp:LinkButton id="btnUpdate" runat="server" Text="Update" 
                                    CommandName="Update" ResourceKey="cmdUpdate" CausesValidation="false" />
                                <asp:LinkButton id="btnCancel" runat="server" Text="Cancel" 
                                    CommandName="Cancel" ResourceKey="cmdCancel" CausesValidation="false" />
				            </EditItemTemplate>
			            </asp:DataList>
			            <div align="left"><asp:Label ID="lblNoFiles" Runat="server" CssClass="Normal" /></div>
	                </td>
	            </tr>
            </table>
            </asp:PlaceHolder>                  
        </td>
    </tr>
</table>    
<script type="text/javascript">
    function articleQueueComplete2() {
	    eval("<%= GetPostBackReference() %>");
    }
</script>
<Ventrian:RefreshControl ID="cmdRefreshFiles" runat="server" Text="Refresh Files" Visible="false" />

