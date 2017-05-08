<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecoverPassword.aspx.cs" Inherits="SleekSurf.Web.RecoverPassword" %>

<%@ Register Src="~/WebPageControls/PasswordRecovery.ascx" TagName="RecoverPassword"
    TagPrefix="uc" %>
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
        <div id="PasswordRecoveryContent">
            <uc:RecoverPassword runat="server" ID="PwdRecovery" />
        </div>
    </div>
    </form>
</body>
</html>
