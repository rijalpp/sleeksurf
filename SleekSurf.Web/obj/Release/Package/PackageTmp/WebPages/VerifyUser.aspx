<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerifyUser.aspx.cs" Inherits="SleekSurf.Web.WebPages.VerifyUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/SleekTheme/Default.css" type="text/css" rel="Stylesheet" />
</head>
<body class="changeBodyBackGround specialBackground">
    <form id="form1" runat="server">
    <div id="Wrapper" class="adminWrapperSettings">
        <div id="TopHeader">
        </div>
        <div id="LoginHeader">
            <img id="imgLogo" runat="server" src="../../App_Themes/SleekTheme/Images/LogoSleekSurf.png" width="250" height="100"
                alt="logo" />
        </div>
        <div id="Layout">
            <div id="MessageBox">
                <span class="messageTitle">
                    <asp:Literal ID="ltrVerificationMessage" runat="server"></asp:Literal>
                    <span style="float: right;">
                        <asp:LinkButton ID="lbtnSendLinkToEmail" runat="server" Text="Email me this" OnClick="lbtnSendLinkToEmail_Click"
                            CssClass="sendMeEmail" Visible="false"></asp:LinkButton>
                    </span></span>
                <div style="text-align:right;">
                <asp:Label ID="ltrEmailMessage" runat="server" Visible="false" EnableViewState="false"></asp:Label>
                </div>
                <asp:Literal ID="ltrMessageBoard" runat="server"></asp:Literal>
                <asp:Panel ID="pnlCompanyUrl" runat="server" Visible="false">
                    <asp:Literal ID="ltrCompanyProfile" runat="server"></asp:Literal>
                </asp:Panel>
                <asp:Panel ID="pnlAdminUrl" runat="server" Visible="false">
                    <asp:Literal ID="ltrAdminUrl" runat="server"></asp:Literal>
                </asp:Panel>
                <asp:Panel ID="pnlMainPage" runat="server" Visible="false">
                    <asp:Literal ID="ltrMainPage" runat="server"></asp:Literal>
                </asp:Panel>
                <p style="padding-top: 20px;">
                    Thank you for registering with us.</p>
                <p>
                    <asp:Literal ID="ltrCommanyName" runat="server"></asp:Literal>
                </p>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
