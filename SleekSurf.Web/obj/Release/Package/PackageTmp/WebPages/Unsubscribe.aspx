<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Unsubscribe.aspx.cs" Inherits="SleekSurf.Web.WebPages.Unsubscribe" %>

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
            <img src="../../App_Themes/SleekTheme/Images/LogoSleekSurf.png" width="250" height="100"
                alt="logo" />
        </div>
        <div id="Layout">
            <div id="MessageBox">
                <span class="messageTitle"><asp:Literal ID="ltrVerificationMessage" runat="server"></asp:Literal></span>
                <p>
                    <asp:Literal ID="ltrMessageBoard" runat="server"></asp:Literal>
               </p>
                <p>
                    With regards.</p>
                <p>
                    <asp:Literal ID="ltrCompanyName" runat="server"></asp:Literal>
                    </p>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
