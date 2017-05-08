<%@ Page Title="" Language="C#" MasterPageFile="~/SleekSurf.Master" AutoEventWireup="true"
    CodeBehind="AccessDenied.aspx.cs" Inherits="SleekSurf.Web.CustomErrorPages.AccessDenied" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="Refresh" content="10; url=../Default.aspx" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapContent">
        <div class="showErrorMessage">
            <p>
                You do not have the appropriate permissions to access that page or area of our site!
                If you feel that you have received this message in error, please try logging off
                our system and then logging back in. If that does not address your issue, please
                feel free to
                <asp:HyperLink runat="server" ID="lnkContact" Text="Contact Us" NavigateUrl="~/WebPages/ContactUs.aspx" />.<br />
                <br />
                Sorry for any inconvenience. You will be redirected to default page in few seconds.
            </p>
            <asp:ImageButton ID="ImageButtonOops" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Oops.png"
                PostBackUrl="~/Default.aspx" />
        </div>
    </div>
</asp:Content>
