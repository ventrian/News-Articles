<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PostComment.ascx.vb" Inherits="Ventrian.NewsArticles.Controls.PostComment" ClassName="NewsArticlesPostCommentControl" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls"%>
<asp:PlaceHolder ID="phCommentForm" runat="Server">
<p id="pName" runat="server">
    <asp:textbox id="txtName" cssclass="NormalTextBox" runat="server" />
    <asp:Label ID="lblName" runat="server" CssClass="Normal" Text="Name (required)" />
    <asp:requiredfieldvalidator id="valName" cssclass="NormalRed" runat="server" 
			    ControlToValidate="txtName" Display="Dynamic" ErrorMessage="Name Is Required" SetFocusOnError="true" ValidationGroup="PostComment" />
</p>
<p id="pEmail" runat="server">
    <asp:textbox id="txtEmail" CssClass="NormalTextBox" runat="server" />
    <asp:Label ID="lblEmail" runat="server" CssClass="Normal" Text="Email (required)" />
    <asp:requiredfieldvalidator id="valEmail" cssclass="NormalRed" runat="server" 
	    controltovalidate="txtEmail" display="Dynamic" ErrorMessage="Email Is Required" SetFocusOnError="true" ValidationGroup="PostComment" />
	<asp:RegularExpressionValidator ID="valEmailIsValid" CssClass="NormalRed" runat="server"
	    ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Invalid Email Address" SetFocusOnError="true" ValidationGroup="PostComment" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$" />    
</p>
<p id="pUrl" runat="server">
    <asp:textbox id="txtURL" cssclass="NormalTextBox" runat="server" />
    <asp:Label ID="lblUrl" runat="server" CssClass="Normal" Text="Website" />
</p>
<p>
    <asp:textbox id="txtComment" cssclass="NormalTextBox" runat="server" textmode="MultiLine" Width="450px" Height="150px"></asp:textbox>
	<asp:requiredfieldvalidator id="valComment" cssclass="NormalRed" runat="server" 
		controltovalidate="txtComment" errormessage="<br>Comment Is Required" display="Dynamic" SetFocusOnError="true" ValidationGroup="PostComment" />
</p>
<dnn:captchacontrol id="ctlCaptcha" captchawidth="130" captchaheight="40" cssclass="Normal" runat="server" errorstyle-cssclass="NormalRed" />
    <div runat="server" id="ctlReCaptcha" >
        <div style="display: none">
            <asp:TextBox runat="server" ID="dummyTextBox"></asp:TextBox>
        </div>
        <asp:PlaceHolder runat="server" id="reCaptchaDiv"></asp:PlaceHolder>    
        <asp:CustomValidator runat="server" CssClass="dnnFormMessage dnnFormError" ControlToValidate="dummyTextBox" ID="RecaptchaValidator" OnServerValidate="RecaptchaValidator_OnServerValidate" />
    </div>
    <div runat="server" id="ctlHoneypot">
        <div style="display: none">
            <asp:Label ID="ConfirmEmailLabel" ControlName="txtConfirmEmail" runat="server" />
            <asp:TextBox runat="server" ID="txtConfirmEmail"></asp:TextBox>
            <asp:CustomValidator runat="server" CssClass="dnnFormMessage dnnFormError" ControlToValidate="txtConfirmEmail" ID="HoneypotValidator" OnServerValidate="HoneypotValidator_OnServerValidate"/>
        </div>
    </div>
<p>
    <asp:Button ID="btnAddComment" runat="server" Text="Add Comment" ValidationGroup="PostComment" UseSubmitBehavior="False" />
</p>
<p id="Notify">
    <asp:checkbox id="chkNotifyMe" CssClass="Normal" runat="server" Text="Notify me of followup comments via e-mail"></asp:checkbox>
</p>
</asp:PlaceHolder>
<asp:PlaceHolder ID="phCommentPosted" runat="Server" Visible="False">
    <asp:Label ID="lblRequiresApproval" runat="server" EnableViewState="False" CssClass="Normal" Text="Your comment has been submitted, but requires approval." />
</asp:PlaceHolder>
<asp:PlaceHolder ID="phCommentAnonymous" runat="Server" Visible="False">
     <asp:Label ID="lblRequiresAccess" runat="server" CssClass="Normal" Text="Only registered users may post comments." />
</asp:PlaceHolder>
