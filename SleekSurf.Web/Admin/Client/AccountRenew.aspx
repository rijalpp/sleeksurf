<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="AccountRenew.aspx.cs" Inherits="SleekSurf.Web.Admin.Client.AccountRenew" %>

<%@ Register Src="~/WebPageControls/DisplayPackages.ascx" TagName="Package" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: 100%">
                    <asp:Label ID="lblTitle" runat="server" Text="Account Management Portal" EnableViewState="false"></asp:Label></div>
            </div>
            <uc:Package ID="DisplayPackage" runat="server" />
            <script type="text/javascript">
                $(document).ready(function () {
                    $('#packageSlide').anythingSlider({
                        theme: "metallic",
                        buildArrows: false,
                        resizeContents: false, // If true, solitary images/objects in the panel will expand to fit the viewport
                        toggleControls: false,
                        autoPlay: true,     // If true, the slideshow will start running; replaces "startStopped" option
                        autoPlayLocked: true,
                        pauseOnHover: true,
                        onSlideBegin: function (e, slider) {
                            // keep the current navigation tab in view
                            slider.navWindow(slider.currentPage);
                        }
                    });

                    $('div.anythingSlider-metallic').css("padding", "0px");
                });
</script>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks" runat="server" id="dvRightLinks">
                <asp:HyperLink ID="hlRenewAccount" runat="server" Text="Renew Account" NavigateUrl="~/Admin/Client/AccountRenew.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="hlBuySMSCredit" runat="server" Text="Buy SMS Credit" NavigateUrl="~/Admin/Client/RechargeSMS.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="hlTransactionHistory" runat="server" Text="Transaction History"
                    NavigateUrl="~/Admin/Client/AccountTransactionManagement.aspx" CssClass="hyperLink addHyperLink"></asp:HyperLink>
                 <asp:HyperLink ID="hlMatchProfile" runat="server" Text="Add Unique Profile"
                    NavigateUrl="~/Admin/Client/MatchProfile.aspx" CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="hlMatchDomain" runat="server" Text="Add Domain"
                    NavigateUrl="~/Admin/Client/MatchDomain.aspx" CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
