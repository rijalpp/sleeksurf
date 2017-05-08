<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebsiteUnavailable.aspx.cs" Inherits="SleekSurf.Domain.CustomErrorPages.WebsiteUnavailable" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Requested website unavailable</title>
</head>
<body class="changeBodyBackGround">
    <form id="form1" runat="server">
    <div id="Wrapper" class="adminWrapperSettings">
        <div id="TopHeader">
        </div>
        <div id="LoginHeader">
            <h2 class="header" style="color: #777777;">
            <asp:Literal ID="ltrClientName" runat="server"></asp:Literal></h2>
        </div>
        <div class="showErrorMessage">
            <p>
                Website unavailable.
            </p>
            <p>
                <asp:Literal runat="server" ID="ltrError" Visible="false" Text="We are temporarily unavailable. Please try to visit the website again." />
            </p>
            <p>
                If you like to contact us please visit our office or contact us via phone or email.
                <br />
                <br />
                Sorry for any inconvenience.
            </p>
            <div>
                <asp:Image ID="ImageButtonOops" runat="server" ImageUrl="~/App_Themes/Default/Images/Oops.png" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
