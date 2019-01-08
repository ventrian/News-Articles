<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ReCaptcha.ascx.vb" Inherits="Ventrian.NewsArticles.Controls.ReCaptcha" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div>
    <div style="display: none">
        <asp:TextBox runat="server" ID="dummyTextBox"></asp:TextBox>
    </div>
    <asp:PlaceHolder runat="server" id="reCaptchaDiv"></asp:PlaceHolder>    
    <asp:CustomValidator runat="server" CssClass="dnnFormMessage dnnFormError" ControlToValidate="dummyTextBox" ID="RecaptchaValidator" OnServerValidate="RecaptchaValidator_OnServerValidate" />
</div>
