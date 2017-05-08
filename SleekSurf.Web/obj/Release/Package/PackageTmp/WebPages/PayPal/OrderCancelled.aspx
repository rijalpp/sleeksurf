<%@ Page Title="" Language="C#" MasterPageFile="~/SleekSurf.Master" AutoEventWireup="true"
    CodeBehind="OrderCancelled.aspx.cs" Inherits="SleekSurf.Web.WebPages.PayPal.OrderCancelled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="Refresh" content="10; url=../BrowsePackages.aspx" />
    <script src='<%= Page.ResolveClientUrl("~/Scripts/jquery-1.7.js") %>' type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapContent">
        <div class="showErrorMessage">
            <p>
                We found that you have just cancelled the purchase of your package. If you have
                encountered any problem or related isuue with the purchase, please 
                <asp:HyperLink runat="server" ID="lnkContact" Text="Contact Us" NavigateUrl="~/WebPages/ContactUs.aspx" /> immediately.<br />
                <br />
                Thanks you for browsing our packages. You will be redirected to package list in few seconds.
            </p>
        </div>
    </div>
</asp:Content>
