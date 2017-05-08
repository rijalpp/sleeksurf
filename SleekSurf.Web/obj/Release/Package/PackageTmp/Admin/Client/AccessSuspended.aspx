<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccessSuspended.aspx.cs"
    Inherits="SleekSurf.Web.Admin.Client.AccessSuspended" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../App_Themes/SleekTheme/Default.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        
    </style>
</head>
<body class="changeBodyBackGround specialBackground">
    <form id="form1" runat="server">
    <div id="Wrapper" class="adminWrapperSettings">
        <div id="TopHeader">
        </div>
        <div id="LoginHeader">
            <div class="statusPanel">
                <div>
                    <img src="../../App_Themes/SleekTheme/Images/LogoSleekSurf.png" width="250" height="100"
                        alt="logo" />
                </div>
                <div style="float: right; margin-right: 10px; color: #000000;">
                    <asp:LoginView ID="lvBackEndMasterPage" runat="server">
                        <LoggedInTemplate>
                            <asp:LoginName ID="LoginName1" runat="server" Font-Bold="true" FormatString="Welcome  {0} " />
                            <span style="border: none">
                                <asp:LoginStatus ID="LoginStatusLS" runat="server" LogoutAction="Redirect" LogoutText="Log Out"
                                    OnLoggingOut="LoginStatusLS_LoggingOut" LogoutPageUrl="~/Login.aspx" Font-Bold="True" />
                            </span>
                        </LoggedInTemplate>
                    </asp:LoginView>
                </div>
            </div>
            <div id="Layout">
                <div id="MessageBox">
                    <span class="messageTitle">
                        <asp:Literal ID="ltrAccountMessage" runat="server"></asp:Literal></span>
                    <p>
                        <asp:Literal ID="ltrMessageBoard" runat="server"></asp:Literal></p>
                    <br />
                    <asp:Panel ID="pnlInActiveBySuperAdmin" runat="server" Visible="false">
                        <asp:Button CssClass="suspendedButton" ID="btnInActiveBySuperAdmin" runat="server"
                            Text="Contact Us" OnClick="btnInActiveBySuperAdmin_Click" />
                    </asp:Panel>
                    <asp:Panel ID="pnlInActiveByAccountExpiration" runat="server" Visible="false">
                        <asp:Button CssClass="suspendedButton" ID="btnInActiveByAccountExpiration" runat="server"
                            Text="Renew Account" OnClick="btnInActiveByAccountExpiration_Click" />
                    </asp:Panel>
                    <asp:Panel ID="pnlInActiveByDefault" runat="server" Visible="false">
                        <asp:Button CssClass="suspendedButton" ID="btnInActiveByDefault" runat="server" Text="Buy Package"
                            OnClick="btnInActiveByDefault_Click" />
                    </asp:Panel>
                    <asp:Panel ID="pnlTechnicalError" runat="server" Visible="false">
                        <asp:Button CssClass="suspendedButton" ID="btnTechnicalError" runat="server" Text="Contact Us"
                            OnClick="btnTechnicalError_Click" />
                    </asp:Panel>
                    <br />
                    <p>
                        Thanks</p>
                    <p>
                        SleekSurf Team</p>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
