<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SleekSurf.Web.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body class="changeBodyBackGround">
    <form id="form1" runat="server">
    <div id="Wrapper" class="adminWrapperSettings">
        <div id="TopHeader">
        </div>
        <div id="LoginHeader">
            <img src="App_Themes/SleekTheme/Images/LogoSleekSurf.png" width="250" height="100" alt="logo" />
        </div>
        <div id="LoginContent">
            <asp:Login ID="LgnLogin" runat="server" DestinationPageUrl="~/Admin/Default.aspx"
                Width="100%" onloginerror="LgnLogin_LoginError">
                <LayoutTemplate>
                    <div id="AdminLogin">
                        <div>
                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" SkinID="AdminLoginBox">Username:</asp:Label>
                            <asp:TextBox ID="UserName" runat="server" SkinID="AdminLoginBox"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="LgnLogin">*</asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" SkinID="AdminLoginBox">Password:</asp:Label>
                            <asp:TextBox ID="Password" runat="server" TextMode="Password" SkinID="AdminLoginBox"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="LgnLogin">*</asp:RequiredFieldValidator>
                        </div>
                        <div style="margin: 15px 0px 0px; padding: 0px 20px;">
                            <span style="margin-right: 20px; display: inline-block; width: 155px;" class="forgetPassword">
                                <a href="RecoverPassword.aspx" class="forgetPassword">Forgot Password</a> ? |</span> <span style="display: inline-block;">
                                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" ValidationGroup="LgnLogin"
                                        Text="Login" SkinID="Button" />
                                </span>
                        </div>
                        <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." Visible="false" />
                        <div style="color: Red; text-align: center; clear: both; display:block;">
                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                        </div>
                    </div>
                </LayoutTemplate>
            </asp:Login>
        </div>
    </div>
    </form>
</body>
</html>
