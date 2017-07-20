<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucApproveComments.ascx.vb" Inherits="Ventrian.NewsArticles.ucApproveComments" %>
<%@ Register TagPrefix="article" TagName="Header" Src="ucHeader.ascx" %>
<script type="text/javascript">
function SelectAll(CheckBoxControl)
{
    if (CheckBoxControl.checked == true)
    {
        var i;
        for (i=0; i < document.forms[0].elements.length; i++)
        {
            if ((document.forms[0].elements[i].type == 'checkbox') &&
            (document.forms[0].elements[i].name.indexOf('rptApproveComments') > -1))
            {
                document.forms[0].elements[i].checked = true;
            }
        }
    }
    else
    {
        var i;
        for (i=0; i < document.forms[0].elements.length; i++)
        {
            if ((document.forms[0].elements[i].type == 'checkbox') &&
            (document.forms[0].elements[i].name.indexOf('rptApproveComments') > -1))
            {
                document.forms[0].elements[i].checked = false;
            }
        }
    }
}
</script>
<article:Header id="ucHeader1" SelectedMenu="ApproveArticles" runat="server" MenuPosition="Top"></article:Header>
<asp:Repeater ID="rptApproveComments" runat="server">
    <HeaderTemplate>
        <table cellpadding="4" cellspacing="0" width="100%">
        <tr>
            <td width="15px">&nbsp;</td>
            <td width="25px"><input type="CheckBox" name="SelectAllCheckBox" onclick="SelectAll(this)"></td>
            <td class="NormalBold"><asp:Label ID="lblTitle" runat="Server" ResourceKey="Title.Header" /></td>
            <td class="NormalBold"><asp:Label ID="lblHeader" runat="Server" ResourceKey="Author.Header" /></td>
            <td class="NormalBold"><asp:Label ID="lblWebsite" runat="Server" ResourceKey="Website.Header" /></td>
            <td class="NormalBold"><asp:Label ID="lblDate" runat="Server" ResourceKey="Date.Header" /></td>
            <td class="NormalBold"><asp:Label ID="lblRemoteAddress" runat="Server" ResourceKey="RemoteAddress.Header" /></td>
        </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td rowspan="2" valign="top"><asp:HyperLink ID="HyperLink1" NavigateUrl='<%# GetEditCommentUrl(DataBinder.Eval(Container.DataItem,"CommentID").ToString()) %>' runat="server">
				<asp:Image ImageUrl="~/images/edit.gif" AlternateText="Edit" runat="server" ID="Hyperlink1Image" resourcekey="Edit"/></asp:HyperLink></td>
            <td rowspan="2" valign="top"><asp:CheckBox id="chkSelected" runat="server" ></asp:CheckBox></td>
            <td class="Normal"><a href="<%#GetArticleUrl(Container.DataItem)%>" class="Normal" target="_blank"><%#GetTitle(Container.DataItem)%></a></td>
            <td class="Normal"><span class="Normal"><%# GetAuthor(Container.DataItem) %></span></td>
            <td class="Normal"><span class="Normal"><%#GetWebsite(Container.DataItem)%></span></td>
            <td class="Normal"><span class="Normal"><%#DataBinder.Eval(Container.DataItem, "CreatedDate", "{0:d}")%></span></td>
            <td class="Normal"><span class="Normal"><%#DataBinder.Eval(Container.DataItem, "RemoteAddress")%></span></td>
        </tr>
        <tr>
            <td colspan="5" class="Normal"><b>Comment:&nbsp;</b><span class="Normal"><%#DataBinder.Eval(Container.DataItem, "Comment")%></span></td>
        </tr>
        <td colspan="7"><hr /></td>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
<asp:label id="lblNoComments" ResourceKey="NoComments" Runat="server" CssClass="Normal" Visible="False"></asp:Label>
<p align="center">
	<asp:linkbutton id="cmdApprove" resourcekey="cmdApprove" runat="server" cssclass="CommandButton" text="Approve" causesvalidation="False" borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdReject" resourcekey="cmdReject" runat="server" cssclass="CommandButton" text="Reject" borderstyle="none" />
</p>
<article:header id="Header1" SelectedMenu="ApproveArticles" runat="server" MenuPosition="Bottom"></article:Header>
