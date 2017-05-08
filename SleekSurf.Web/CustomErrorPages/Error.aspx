<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="SleekSurf.Web.CustomErrorPages.Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Unexpected Error Occorred! - SleekSurf</title>
    <meta http-equiv="Refresh" content="10; url=../Default.aspx" />
</head>
<body class="changeBodyBackGround">
    <form id="form1" runat="server">
    <div id="Wrapper" class="adminWrapperSettings">
        <div id="TopHeader">
        </div>
        <div id="LoginHeader">
            <img src="../App_Themes/SleekTheme/Images/LogoSleekSurf.png" width="250" height="100"
                alt="logo" />
        </div>
        <div class="showErrorMessage">
            <p>
                Unexpected Error Occurred!
            </p>
            <p>
                <asp:Literal runat="server" ID="ltr404" Text="The requested page or resource was not found." />
                <asp:Literal runat="server" ID="ltr408" Text="The request timed out. This may be caused by a too high traffic. Please try again later." />
                <asp:Literal runat="server" ID="ltr505" Text="The server encountered an unexpected condition which prevented it from fulfilling the request. Please try again later." />
                <asp:Literal runat="server" ID="ltrError" Visible="false" Text="There was some problems processing your request. An e-mail with details about this error has already been sent to the administrator." />
            </p>
            <p>
                If you like to contact the webmaster to report the problem with more details, please
                <asp:HyperLink runat="server" ID="lnkContact" Text="Contact Us" NavigateUrl="~/WebPages/ContactUs.aspx" />.
                <br />
                <br />
                Sorry for any inconvenience. You will be redirected to default page in few seconds.
            </p>
            <div>
                <asp:ImageButton ID="ImageButtonOops" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Oops.png"
                    PostBackUrl="~/Default.aspx" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
