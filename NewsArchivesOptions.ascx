<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="NewsArchivesOptions.ascx.vb" Inherits="Ventrian.NewsArticles.NewsArchivesOptions" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<div class="dnnArchiveForm" id="form-archive">
    <div class="dnnFormItem dnnFormHelp dnnClear">
    <h2 id="dnnPanel-ArchivesBasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("BasicSettings")%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="drpModuleID" ResourceKey="Module" />
            <asp:dropdownlist id="drpModuleID" Runat="server" 
                DataValueFielduefield="ModuleID" DataValueFieldtextfield="ModuleTitle" 
				AutoPostBack="True"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="rdoMode" ResourceKey="Mode" />
            <asp:dropdownlist id="drpMode" AutoPostBack="True" Runat="server"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem" id="divHideZeroCategories" runat="server">
            <dnn:Label runat="server" ControlName="chkHideZeroCategories" ResourceKey="HideZeroCategories" />
            <asp:checkbox id="chkHideZeroCategories" Runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem" id="divParentCategory" runat="server">
            <dnn:Label runat="server" ControlName="drpParentCategory" ResourceKey="ParentCategory" />
            <asp:DropDownList ID="drpParentCategory" Runat="server" DataTextField="NameIndented" DataValueField="CategoryID" />
        </div>
        <div class="dnnFormItem" id="divMaxDepth" runat="server">
            <dnn:Label runat="server" ControlName="txtMaxDepth" ResourceKey="MaxDepth" />
            <asp:textbox id="txtMaxDepth" Runat="server" Width="50" /> 
	        <asp:CompareValidator id="valMaxDepth" runat="server" ResourceKey="valMaxDepth.ErrorMessage" ErrorMessage="Must be a Number"
		        Display="Dynamic" ControlToValidate="txtMaxDepth" Type="Integer" Operator="DataTypeCheck" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
        </div>
        <div class="dnnFormItem" id="divAuthorSortBy" runat="server">
            <dnn:Label runat="server" ControlName="lstAuthorSortBy" ResourceKey="plAuthorSortBy" />
            <asp:DropDownList ID="drpAuthorSortBy" Runat="server" />
        </div>
        <div class="dnnFormItem" id="divGroupBy" runat="server">
            <dnn:Label runat="server" ControlName="drpGroupBy" ResourceKey="plGroupBy" />
            <asp:DropDownList ID="drpGroupBy" Runat="server" />
        </div>
    </fieldset>
    <h2 id="dnnPanel-ArchivesTemplateSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("TemplateSettings")%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="rdoLayoutMode" ResourceKey="plLayoutMode" />
			<asp:RadioButtonList ID="rdoLayoutMode" Runat="server" AutoPostBack="True" CssClass="dnnFormRadioButtons" RepeatDirection="Horizontal" />
        </div>
        <div class="dnnFormItem" runat="server" id="divItemsPerRow">
            <dnn:Label runat="server" ControlName="txtItemsPerRow" ResourceKey="plItemsPerRow" />
            <asp:TextBox ID="txtItemsPerRow" Runat="server" maxlength="6" CssClass="dnnFormRequired" />
            <asp:RequiredFieldValidator ID="valItemsPerRow" Runat="server" ControlToValidate="txtItemsPerRow" Display="Dynamic"
				ResourceKey="valItemsPerRow.ErrorMessage" CssClass="dnnFormMessage dnnFormError" />
			<asp:CompareValidator ID="valItemsPerRowIsNumber" Runat="server" ControlToValidate="txtItemsPerRow" Display="Dynamic"
				ResourceKey="valItemsPerRowIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="txtHtmlHeader" ResourceKey="HtmlHeader" />
            <asp:textbox id="txtHtmlHeader" runat="server" TextMode="MultiLine" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="txtHtmlBody" ResourceKey="HtmlBody" />
            <asp:textbox id="txtHtmlBody" runat="server" TextMode="MultiLine" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="txtHtmlFooter" ResourceKey="HtmlFooter" />
            <asp:textbox id="txtHtmlFooter" runat="server" TextMode="MultiLine" />
        </div>
    </fieldset>
</div>

