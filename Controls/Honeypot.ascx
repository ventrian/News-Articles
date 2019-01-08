<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Honeypot.ascx.vb" Inherits="Ventrian.NewsArticles.Controls.Honeypot" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
    <div style="display: none">
        <dnn:Label ID="ConfirmEmailLabel" ControlName="txtConfirmEmail" runat="server" />
        <asp:TextBox runat="server" ID="txtConfirmEmail"></asp:TextBox>
        <asp:CustomValidator runat="server" CssClass="dnnFormMessage dnnFormError" ControlToValidate="txtConfirmEmail" ID="RecaptchaValidator" OnServerValidate="HoneypotValidator_OnServerValidate"/>
    </div>
