<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="LatestArticlesOptions.ascx.vb" Inherits="Ventrian.NewsArticles.LatestArticlesOptions" %>
<%@ Register TagPrefix="dnn" TagName="Skin" Src="~/controls/SkinControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>

<table id="tblLatestArticlesDetail" cellspacing="2" cellpadding="2" summary="Appearance Design Table"
	border="0" runat="server">
	<tr valign="top">
		<td class="SubHead" width="150">
			<dnn:label id="plModuleID" runat="server" resourcekey="Module" suffix=":" controlname="drpModuleID"></dnn:label></td>
		<td align="left" width="325">
			<asp:dropdownlist id="drpModuleID" Runat="server" Width="325" datavaluefield="ModuleID" datatextfield="ModuleTitle" 
				CssClass="NormalTextBox" AutoPostBack="True"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td class="SubHead" width="200"><dnn:label id="plSortBy" resourcekey="SortBy" helpkey="SortByHelp" runat="server" controlname="drpSortBy"></dnn:label></td>
		<td valign="top"><asp:dropdownlist id="drpSortBy" Runat="server" CssClass="NormalTextBox"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td class="SubHead" width="200"><dnn:label id="plSortDirection" resourcekey="SortDirection" runat="server" controlname="drpSortDirection"></dnn:label></td>
		<td valign="top"><asp:dropdownlist id="drpSortDirection" Runat="server" CssClass="NormalTextBox"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td class="SubHead" width="200"><dnn:label id="plLaunchLinks" resourcekey="LaunchLinks" helpkey="LaunchLinksHelp" runat="server"
				controlname="chkLaunchLinks"></dnn:label></td>
		<td valign="top"><asp:checkbox id="chkLaunchLinks" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
	</tr>
	<tr>
		<td class="SubHead" width="200"><dnn:label id="plBubbleFeatured" resourcekey="BubbleFeatured" runat="server"
				controlname="chkBubbleFeatured"></dnn:label></td>
		<td valign="top"><asp:checkbox id="chkBubbleFeatured" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
	</tr>
    <tr valign="top">
        <td class="SubHead" width="200">
	        <dnn:label id="plArticleCount" runat="server" resourcekey="Count" suffix=":" controlname="txtArticleCount"></dnn:label></td>
        <td align="left">
	        <asp:textbox id="txtArticleCount" Runat="server" Width="50" CssClass="NormalTextBox">10</asp:textbox>
	        <asp:RequiredFieldValidator id="valCount" runat="server" ResourceKey="valCount.ErrorMessage" ErrorMessage="<br>* Required"
		        Display="Dynamic" ControlToValidate="txtArticleCount" CssClass="NormalRed"></asp:RequiredFieldValidator>
	        <asp:CompareValidator id="valCountType" runat="server" ResourceKey="valCountType.ErrorMessage" ErrorMessage="<br>* Must be a Number"
		        Display="Dynamic" ControlToValidate="txtArticleCount" Type="Integer" Operator="DataTypeCheck" CssClass="NormalRed"></asp:CompareValidator>
        </td>
    </tr>
    <tr>
        <td colspan="2">
	        <dnn:sectionhead id="dshFilterSettings" runat="server" cssclass="Head" includerule="True" isExpanded="false"
	            resourcekey="FilterSettings" section="tblFilterSettings" text="Template" />
	        <table id="tblFilterSettings" runat="server" cellspacing="0" cellpadding="2" width="100%" summary="Template Design Table" border="0">
	        <tr>
		        <td class="SubHead" width="150"><dnn:label id="plArticleIDs" resourcekey="plArticleIDs" runat="server" controlname="txtArticleIDs"></dnn:label></td>
		        <td valign="top"><asp:textbox id="txtArticleIDs" Runat="server" Width="150" CssClass="NormalTextBox"></asp:textbox></td>
	        </tr>
            <TR vAlign="top">
	            <TD class="SubHead" width="150">
		            <dnn:label id="plCategories" runat="server" resourcekey="Categories" suffix=":" controlname="lstCategories"></dnn:label>
	            </TD>
	            <TD align="left" width="325">
		            <asp:CheckBox ID="chkAllCategories" Runat="server" AutoPostBack="True" ResourceKey="AllCategories"
			            CssClass="Normal" />
		            <hr>
		            <asp:RadioButtonList ID="rdoMatchOperator" Runat="server" CssClass="Normal" RepeatDirection="Horizontal" />
		            <asp:ListBox ID="lstCategories" runat="server" CssClass="Normal" DataTextField="NameIndented" 
			                            DataValueField="CategoryID" Width="300px" SelectionMode="Multiple" Rows="6" />
					<asp:Label ID="lblHoldCtrl" runat="server" ResourceKey="HoldCtrl" CssClass="Normal" />	
	            </TD>
            </TR>
             <tr vAlign="top">
	            <td class="SubHead" width="150">
		            <dnn:label id="plCategoriesExclude" runat="server" resourcekey="CategoriesExclude" suffix=":" controlname="lstCategoriesExclude"></dnn:label>
	            </td>
	            <td align="left" width="325">
		            <asp:ListBox ID="lstCategoriesExclude" runat="server" CssClass="Normal" DataTextField="NameIndented" 
			                            DataValueField="CategoryID" Width="300px" SelectionMode="Multiple" Rows="6" />
					<asp:Label ID="lblHoldCtrl2" runat="server" ResourceKey="HoldCtrl" CssClass="Normal" />	
	            </td>
            </tr>
            <TR vAlign="top">
		        <TD class="SubHead" width="150">
			        <dnn:label id="plStartPoint" runat="server" resourcekey="StartPoint" suffix=":" controlname="txtStartPoint"></dnn:label></TD>
		        <TD align="left" width="325">
			        <asp:textbox id="txtStartPoint" Runat="server" Width="50" CssClass="NormalTextBox">0</asp:textbox>&nbsp;<asp:Label ID="lblStartPoint2" Runat="server" EnableViewState="False" ResourceKey="StartPoint2.Help"
				        CssClass="Normal" />
			        <asp:RequiredFieldValidator id="valStartPoint" runat="server" ResourceKey="txtStartPoint.ErrorMessage" ErrorMessage="<br>* Required"
				        Display="Dynamic" ControlToValidate="txtStartPoint" CssClass="NormalRed"></asp:RequiredFieldValidator>
			        <asp:CompareValidator id="valStartPointIsNumber" runat="server" ResourceKey="valStartPointIsNumber.ErrorMessage" ErrorMessage="<br>* Must be a Number"
				        Display="Dynamic" ControlToValidate="txtStartPoint" Type="Integer" Operator="DataTypeCheck" CssClass="NormalRed"></asp:CompareValidator>
		        </TD>
	        </TR>
	        <TR vAlign="top">
		        <TD class="SubHead" width="150">
			        <dnn:label id="plStartDate" runat="server" resourcekey="StartDate" suffix=":" controlname="txtStartDate"></dnn:label></TD>
		        <TD align="left" width="325">
			        <asp:textbox id="txtStartDate" cssclass="NormalTextBox" runat="server" width="150" maxlength="15"></asp:textbox>
			        <asp:hyperlink id="cmdStartDate" cssclass="CommandButton" runat="server" resourcekey="Calendar">Calendar</asp:hyperlink>&nbsp;<asp:Label ID="lblStartDate" Runat="server" EnableViewState="False" ResourceKey="StartDate2.Help"
				        CssClass="Normal" />
			        <asp:comparevalidator id="valStartDate" cssclass="NormalRed" runat="server" controltovalidate="txtStartDate"
				        errormessage="<br>Invalid start date!" operator="DataTypeCheck" type="Date" display="Dynamic" ResourceKey="valStartDate.ErrorMessage"></asp:comparevalidator>
		        </TD>
	        </TR>
	        <TR vAlign="top">
		        <TD class="SubHead" width="150">
			        <dnn:label id="plMaxAge" runat="server" resourcekey="MaxAge" suffix=":" controlname="txtMaxAge"></dnn:label></TD>
		        <TD align="left" width="325">
			        <asp:textbox id="txtMaxAge" Runat="server" Width="50" CssClass="NormalTextBox"></asp:textbox>&nbsp;<asp:Label ID="lblMaxAge" Runat="server" EnableViewState="False" ResourceKey="MaxAge2.Help"
				        CssClass="Normal" />
			        <asp:CompareValidator id="valMaxAgeType" runat="server" ResourceKey="valMaxAgeType.ErrorMessage" ErrorMessage="<br>* Must be a Number"
				        Display="Dynamic" ControlToValidate="txtMaxAge" Type="Integer" Operator="DataTypeCheck" CssClass="NormalRed"></asp:CompareValidator>
		        </TD>
	        </TR>
            <tr>
	            <td class="SubHead" width="150"><dnn:label id="plEnablePager" resourcekey="EnablePager" runat="server"
			            controlname="chkEnablePager"></dnn:label></td>
	            <td valign="top"><asp:checkbox id="chkEnablePager" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
            </tr>
            <tr vAlign="top">
                <TD class="SubHead" width="150">
                    <dnn:label id="plPageSize" runat="server" resourcekey="PageSize" suffix=":" controlname="txtPageSize"></dnn:label></TD>
                <TD align="left" width="325">
                    <asp:textbox id="txtPageSize" Runat="server" Width="50" CssClass="NormalTextBox">10</asp:textbox>
                    <asp:RequiredFieldValidator id="valPageSize" runat="server" ResourceKey="valPageSize.ErrorMessage" ErrorMessage="<br>* Required"
	                    Display="Dynamic" ControlToValidate="txtPageSize" CssClass="NormalRed"></asp:RequiredFieldValidator>
                    <asp:CompareValidator id="valPageSizeIsNumber" runat="server" ResourceKey="valPageSizeIsNumber.ErrorMessage" ErrorMessage="<br>* Must be a Number"
	                    Display="Dynamic" ControlToValidate="txtPageSize" Type="Integer" Operator="DataTypeCheck" CssClass="NormalRed"></asp:CompareValidator>
                </TD>
            </tr>
	        <tr>
		        <td class="SubHead" width="150"><dnn:label id="plShowPending" runat="server" controlname="chkShowPending"></dnn:label></td>
		        <td valign="top"><asp:checkbox id="chkShowPending" Runat="server" /></td>
	        </tr>
	        <tr>
		        <td class="SubHead" width="150"><dnn:label id="plShowRelated" runat="server" controlname="chkShowRelated"></dnn:label></td>
		        <td valign="top"><asp:checkbox id="chkShowRelated" Runat="server" /></td>
	        </tr>
	        <tr>
		        <td class="SubHead" width="150"><dnn:label id="plFeaturedOnly" resourcekey="FeaturedOnly" helpkey="FeaturedOnlyHelp" runat="server"
				        controlname="chkFeaturedOnly"></dnn:label></td>
		        <td valign="top"><asp:checkbox id="chkFeaturedOnly" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
	        </tr>
	        <tr>
		        <td class="SubHead" width="150"><dnn:label id="plNotFeaturedOnly" resourcekey="NotFeaturedOnly" helpkey="NotFeaturedOnlyHelp"
				        runat="server" controlname="chkNotFeaturedOnly"></dnn:label></td>
		        <td valign="top"><asp:checkbox id="chkNotFeaturedOnly" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
	        </tr>
	        <tr>
		        <td class="SubHead" width="150"><dnn:label id="plSecureOnly" resourcekey="SecureOnly" runat="server"
				        controlname="chkSecureOnly"></dnn:label></td>
		        <td valign="top"><asp:checkbox id="chkSecureOnly" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
	        </tr>
	        <tr>
		        <td class="SubHead" width="150"><dnn:label id="plNotSecureOnly" resourcekey="NotSecureOnly"
				        runat="server" controlname="chkNotSecureOnly"></dnn:label></td>
		        <td valign="top"><asp:checkbox id="chkNotSecureOnly" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
	        </tr>
	        <tr>
		        <td class="SubHead" width="150"><dnn:label id="plCustomField" resourcekey="CustomField"
				        runat="server" controlname="chkNotSecureOnly"></dnn:label></td>
		        <td valign="top">
		            <asp:DropDownList ID="drpCustomField" runat="server" DataValueField="CustomFieldID" DataTextField="Name" Width="150" />&nbsp;<asp:textbox id="txtCustomFieldValue" cssclass="NormalTextBox" runat="server" width="150" />
                </td>
	        </tr>
	        <tr>
		        <td class="SubHead" width="150"><dnn:label id="plLinkFilter" resourcekey="LinkFilter"
				        runat="server" controlname="ctlUrlLink"></dnn:label></td>
		        <td valign="top">
		            <asp:RadioButtonList ID="rdoLinkFilter" runat="server" CssClass="Normal" RepeatDirection="Horizontal" AutoPostBack="true" />
		            <asp:TextBox ID="txtUrlFilter" runat="server" Width="300" CssClass="NormalTextBox" Text="http://" />
		            <asp:DropDownList ID="drpPageFilter" runat="server" CssClass="NormalTextBox" DataTextField="TabPath" DataValueField="TabID" Width="300" />
		            <dnn:url id="ctlLinkFilter" runat="server" width="300" showtrack="False" showlog="False" shownone="True"
							shownewwindow="False" ShowFiles="false" visible="false"></dnn:url>
                </td>
	        </tr>
	        <tr>
	            <td colspan="2">
	                <br />
	                <dnn:sectionhead id="dshAuthorFilterSettings" runat="server" cssclass="Head" includerule="True" isExpanded="true"
                        resourcekey="AuthorFilterSettings" section="tblAuthorFilterSettings" text="Template" />
                    <table id="tblAuthorFilterSettings" runat="server" cellspacing="0" cellpadding="2" width="100%" summary="Author Design Table" border="0">
	                <tr>
		                <td class="SubHead" width="150"><dnn:label id="plAuthor" resourcekey="AuthorFilter" runat="server" controlname="cmdSelectAuthor"></dnn:label></td>
		                <td valign="top">
			                <asp:Label ID="lblAuthorFilter" Runat="server" CssClass="Normal" />
			                <asp:LinkButton ID="cmdSelectAuthor" runat="server" ResourceKey="cmdSelectAuthor" CssClass="CommandButton" />
			                <asp:DropDownList ID="ddlAuthor" Runat="server" Visible="False" DataTextField="UserName" DataValueField="UserID" />
		                </td>
	                </tr>
                    <tr>
		                <td class="SubHead" width="150"><dnn:label id="plQueryStringFilter" resourcekey="plQueryStringFilter" runat="server" controlname="chkQueryStringFilter"></dnn:label></td>
		                <td valign="top"><asp:checkbox id="chkQueryStringFilter" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
	                </tr>
	                <tr>
		                <td class="SubHead" width="150"><dnn:label id="plQueryStringParam" resourcekey="plQueryStringParam" runat="server" controlname="txtQueryStringParam"></dnn:label></td>
		                <td valign="top"><asp:textbox id="txtQueryStringParam" Runat="server" Width="150" CssClass="NormalTextBox"></asp:textbox></td>
	                </tr>
	                <tr>
		                <td class="SubHead" width="150"><dnn:label id="plUsernameFilter" runat="server" controlname="chkUsernameFilter"></dnn:label></td>
		                <td valign="top"><asp:checkbox id="chkUsernameFilter" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
	                </tr>
	                <tr>
		                <td class="SubHead" width="150"><dnn:label id="plUsernameParam" runat="server" controlname="txtUsernameParam"></dnn:label></td>
		                <td valign="top"><asp:textbox id="txtUsernameParam" Runat="server" Width="150" CssClass="NormalTextBox"></asp:textbox></td>
	                </tr>
	                <tr>
		                <td class="SubHead" width="150"><dnn:label id="plLoggedInUser" runat="server" controlname="chkLoggedInUser"></dnn:label></td>
		                <td valign="top"><asp:checkbox id="chkLoggedInUser" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
	                </tr>
                    </table>
	            </td>
	        </tr>
	        <tr>
	            <td colspan="2">
	                <br />
	                <dnn:sectionhead id="dshTagFilterSettings" runat="server" cssclass="Head" includerule="True" isExpanded="true"
                        resourcekey="TagFilterSettings" section="tblTagFilterSettings" text="Template" />
                    <table id="tblTagFilterSettings" runat="server" cellspacing="0" cellpadding="2" width="100%" summary="Tag Filter Design Table" border="0">
		            <tr valign="top">
			            <td class="SubHead" width="150"><dnn:label id="plTags" text="Tags:" runat="server" controlname="txtTags"></dnn:label></td>
			            <td>    
			                <asp:RadioButtonList ID="rdoTagsMatchOperator" Runat="server" CssClass="Normal" RepeatDirection="Horizontal" />
			                <asp:textbox id="txtTags" cssclass="NormalTextBox" width="300" maxlength="255" runat="server" /><br />
			                <asp:Label ID="lblTags" ResourceKey="TagsHelp" runat="server" CssClass="Normal" />
			            </td>
		            </tr>
                    </table>
                </td>
            </tr> 
	        </table>
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
	        <tr>
		        <td class="SubHead" width="150"><dnn:label id="plLayoutMode" runat="server" controlname="lstLayoutMode" suffix=":"></dnn:label></td>
		        <td valign="bottom">
			        <asp:RadioButtonList ID="lstLayoutMode" Runat="server" RepeatDirection="Horizontal" CssClass="Normal"
				        AutoPostBack="True" />
		        </td>
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
	        <tr runat="server" id="trItemsPerRow">
		        <td class="SubHead" width="150"><dnn:label id="plItemsPerRow" runat="server" controlname="txtItemsPerRow" suffix=":"></dnn:label></td>
		        <td valign="bottom">
			        <asp:TextBox ID="txtItemsPerRow" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				        maxlength="6" /><asp:RequiredFieldValidator ID="valItemsPerRow" Runat="server" ControlToValidate="txtItemsPerRow" Display="Dynamic"
				        ResourceKey="valItemsPerRow.ErrorMessage" CssClass="NormalRed" /><asp:CompareValidator ID="valItemsPerRowIsNumber" Runat="server" ControlToValidate="txtItemsPerRow" Display="Dynamic"
				        ResourceKey="valItemsPerRowIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
		        </td>
	        </tr>
	        <tr valign="top">
		        <td class="SubHead" width="150">
			        <dnn:label id="plHtmlNoArticles" runat="server" resourcekey="HtmlNoArticles" suffix=":" controlname="txtHtmlNoArticles"></dnn:label></td>
		        <td align="left" width="325">
			        <asp:textbox id="txtHtmlNoArticles" cssclass="NormalTextBox" runat="server" Rows="6" TextMode="MultiLine"
				        maxlength="50" width="300"></asp:textbox></td>
	        </tr>
	        <tr valign="top">
	            <td colspan="2">
	            <br />
	                <dnn:sectionhead id="dshTemplateSaves" runat="server" cssclass="Head" includerule="True" isExpanded="true"
	                    resourcekey="TemplateSaves" section="tblTemplateSaves" text="Template Saves" />
                    <table id="tblTemplateSaves" runat="server" cellspacing="0" cellpadding="2" width="100%"
	                    summary="Template Saves Design Table" border="0">
	                <tr runat="server" id="trLoadFromTemplate"> 
		                <td class="SubHead" width="150">
			                <dnn:label id="plLoadFromTemplate" runat="server" resourcekey="LoadFromTemplate" suffix=":" controlname="drpLoadFromTemplate"></dnn:label></td>
		                <td align="left" width="325">
			                <asp:DropDownList ID="drpLoadFromTemplate" runat="server" Width="200" AutoPostBack="true" /><br />
			                <asp:LinkButton ID="cmdLoadTemplate" runat="server" ResourceKey="cmdLoadTemplate" CausesValidation="false" CssClass="CommandButton" />
			            </td>
	                </tr>
	                <tr> 
		                <td class="SubHead" width="150">
			                <dnn:label id="plSaveTemplate" runat="server" resourcekey="SaveTemplate" suffix=":" controlname="txtSaveTemplate"></dnn:label></td>
		                <td align="left" width="325">
			                <asp:textbox id="txtSaveTemplate" cssclass="NormalTextBox" runat="server" maxlength="50" width="200"></asp:textbox>
			                <asp:Label ID="lblSaveTemplateText" runat="server" CssClass="Normal" ResourceKey="SaveTemplateText" /><br />
                            <asp:LinkButton ID="cmdSaveTemplate" runat="server" ResourceKey="cmdSaveTemplate" CausesValidation="false" CssClass="CommandButton" />
                        </td>
	                </tr>
                    </table>
                </td>
	        </tr>
            </table>
	    </td>
	</tr>
	<tr>
	    <td colspan="2">
	        <dnn:sectionhead id="dshTemplateHelp" runat="server" cssclass="Head" includerule="True" isExpanded="false"
	            resourcekey="TemplateHelp" section="tblTemplateHelp" text="Template Help" />
            <table id="tblTemplateHelp" runat="server" cellspacing="0" cellpadding="2" width="100%"
	            summary="Template Help Design Table" border="0">
	            <tr>
		            <td><asp:label id="lblTemplateHelp" resourcekey="TemplateHelpDescription" cssclass="Normal" runat="server"
				            enableviewstate="False"></asp:label></td>
	            </tr>
	            <tr>
	                <td>
	                    <asp:Label ID="lblHeaderFooter" runat="server" ResourceKey="HeaderFooter" Text="Header/Footer Tokens" class="Head" />
                        <table id="Table2" runat="server" cellspacing="0" cellpadding="2" width="100%"
                            summary="Template Help Design Table" border="0">
                        <tr valign="top">
                            <td class="SubHead" width="150">[AUTHOR]</td>
                            <td class="Normal" align="left">
                                <asp:label id="lblAuthorToken" resourcekey="AuthorToken" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="SubHead" width="150">[CATEGORY]</td>
                            <td class="Normal" align="left">
                                <asp:label id="lblCategory" resourcekey="Category" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="SubHead" width="150">[CUSTOM:XXX]</td>
                            <td class="Normal" align="left">
                                <asp:label id="lblCustomXXX" resourcekey="CustomXXX" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="SubHead" width="150">[RSSLINK]</td>
                            <td class="Normal" align="left">
                                <asp:label id="lblRssLink" resourcekey="RssLink" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="SubHead" width="150">[SORT]</td>
                            <td class="Normal" align="left">
                                <asp:label id="lblSort" resourcekey="Sort" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="SubHead" width="150">[SORT:XXX]</td>
                            <td class="Normal" align="left">
                                <asp:label id="lblSortXXX" resourcekey="SortXXX" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="SubHead" width="150">[TIME]</td>
                            <td class="Normal" align="left">
                                <asp:label id="lblTime" resourcekey="Time" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="SubHead" width="150">[TIME:XXX]</td>
                            <td class="Normal" align="left">
                                <asp:label id="lblTimeXXX" resourcekey="TimeXXX" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
                            </td>
                        </tr>
                        </table>
	                    <br />
	                    <asp:Label ID="lblItem" runat="server" ResourceKey="Item" Text="Item Tokens" class="Head" />
	                    <table id="Table1" runat="server" cellspacing="0" cellpadding="2" width="100%"
	                        summary="Template Help Design Table" border="0">
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[ARTICLEID]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblArticleID" resourcekey="ArticleID" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[ARTICLELINK]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblArticleLink" resourcekey="ArticleLink" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[AUTHOR]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblAuthor" resourcekey="Author" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[AUTHOREMAIL]</TD>
		                        <td class="Normal" align="left" width="325">
			                        <asp:label id="lblAuthorEmail" resourcekey="AuthorEmail" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[AUTHORUSERNAME]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblAuthorUserName" resourcekey="AuthorUserName" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[AUTHORFIRSTNAME]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblAuthorFirstName" resourcekey="AuthorFirstName" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[AUTHORLASTNAME]</TD>
		                        <td class="Normal" align="left" width="325">
			                        <asp:label id="lblAuthorLastName" resourcekey="AuthorLastName" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[AUTHORFULLNAME]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblAuthorFullName" resourcekey="AuthorFullName" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[AUTHORID]</TD>
		                        <td class="Normal" align="left" width="325">
			                        <asp:label id="lblAuthorID" resourcekey="AuthorID" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[CATEGORIES]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblCategories" resourcekey="CategoriesToken" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[COMMENTLINK]</TD>
		                        <td class="Normal" align="left" width="325">
			                        <asp:label id="lblCommentLink" resourcekey="CommentLink" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[CREATEDATE]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblCreateDate" resourcekey="CreateDate" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[CREATEDATE:XXX]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblCreateDateXXX" resourcekey="CreateDateXXX" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[CREATETIME]</TD>
		                        <td class="Normal" align="left" width="325">
			                        <asp:label id="lblCreateTime" resourcekey="CreateTime" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[COMMENTCOUNT]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblCommentCount" resourcekey="CommentCount" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[DETAILS]</TD>
		                        <td class="Normal" align="left" width="325">
			                        <asp:label id="lblDetails" resourcekey="Details" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[EDIT]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="plEdit" resourcekey="EditToken" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[HASCATEGORIES]<br />[/HASCATEGORIES]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblHasCategories" resourcekey="HasCategories" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[HASCOMMENTSENABLED]<br />[/HASCOMMENTSENABLED]</TD>
		                        <td class="Normal" align="left" width="325">
			                        <asp:label id="lblHasCommentsEnabled" resourcekey="HasCommentsEnabled" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[HASLINK]<br />[/HASLINK]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblHasLink" resourcekey="HasLink" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[HASMOREDETAIL]<br />[/HASMOREDETAIL]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblHasMoreDetail" resourcekey="HasMoreDetail" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[HASMULTIPLEPAGES]<br />[/HASMULTIPLEPAGES]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblHasMultiplePages" resourcekey="HasMultiplePages" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[HASRATINGSENABLED]<br />[/HASRATINGSENABLED]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblHasRatingsEnabled" resourcekey="HasRatingsEnabled" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[IMAGE]</TD>
		                        <td class="Normal" align="left" width="325">
			                        <asp:label id="lblImage" resourcekey="Image" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[IMAGELINK]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblImageLink" resourcekey="ImageLink" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[IMAGETHUMB:XXX]</TD>
		                        <td class="Normal" align="left" width="325">
			                        <asp:label id="lblImageThumb" resourcekey="ImageThumb" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[IMAGETHUMBLINK:XXX]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblImageThumbLink" resourcekey="ImageThumbLink" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[LINK]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblLink" resourcekey="Link" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[LINKTARGET]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblLinkTarget" resourcekey="LinkTarget" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[MODULEID]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblModuleID" resourcekey="ModuleID" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[PAGES]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblPages" resourcekey="Pages" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[PAGESLIST]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="Label1" resourcekey="PagesList" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[PAGECOUNT]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblPageCount" resourcekey="PageCount" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[PAGETEXT]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblPageText" resourcekey="PageText" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[PRINT]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblPrint" resourcekey="Print" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[PUBLISHSTARTDATE]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblPublishStartDate" resourcekey="PublishDateTag" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[PUBLISHSTARTDATE:XXX]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblPublishStartDateXXX" resourcekey="PublishDateXXX" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[PUBLISHSTARTTIME]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblPublishStartTime" resourcekey="PublishTime" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[PUBLISHENDDATE]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblPublishEndDate" resourcekey="PublishEndDate" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[PUBLISHENDTIME]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblPublishEndTime" resourcekey="PublishEndTime" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[RATING]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblRating" resourcekey="Rating" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[RATINGCOUNT]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblRatingCount" resourcekey="RatingCount" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[RATINGDETAIL]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblRatingDetail" resourcekey="RatingDetail" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[SUMMARY]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblSummary" resourcekey="Summary" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[SUMMARY:XXX]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblSummaryXXX" resourcekey="SummaryXXX" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[TITLE]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblTitle" resourcekey="Title" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[UPDATEDATE]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblUpdateDate" resourcekey="UpdateDate" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[UPDATEDATE:XXX]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblUpdateDateXXX" resourcekey="UpdateDateXXX" cssclass="Normal" runat="server"
				                        enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[UPDATETIME]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblUpdateTime" resourcekey="UpdateTime" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                        <tr valign="top">
		                        <TD class="SubHead" width="150">[VIEWCOUNT]</TD>
		                        <td class="Normal" align="left">
			                        <asp:label id="lblViewCount" resourcekey="ViewCount" cssclass="Normal" runat="server" enableviewstate="False"></asp:label>
		                        </td>
	                        </tr>
	                     </table>
	                </td>
	            </tr>
	            
            </table>
	    </td>
	</tr>
</table>
