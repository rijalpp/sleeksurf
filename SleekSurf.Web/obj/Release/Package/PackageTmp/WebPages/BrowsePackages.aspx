<%@ Page Title="" Language="C#" MasterPageFile="~/SleekSurf.Master" AutoEventWireup="true"
    CodeBehind="BrowsePackages.aspx.cs" Inherits="SleekSurf.Web.WebPages.BrowsePackages" %>

<%@ Register Src="~/WebPageControls/DisplayPackages.ascx" TagPrefix="uc" TagName="DisplayPackage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapContent">
        <div class="sleekPackage">
            <div class="pageTitle">
                <p>
                    Sleek Packages</p>
            </div>
            <p>
                SleekSurf provides different packages to our valuable customers as displayed below.
                Please go through each package in details and determine the package or combination
                of packages that are most suitable for your business/organisation. If you've got
                any enquiries regarding any packages, please <a href="ContactUs.aspx">contact us</a>
                and let us help you.
            </p>
            <uc:DisplayPackage ID="ucDisplayPackages" runat="server" />
            <script type="text/javascript">
                $(function () {
                    $('#packageSlide').anythingSlider({
                        theme: 'metallic',
                        resizeContents: false, // If true, solitary images/objects in the panel will expand to fit the viewport
                        buildArrows: true,
                        buildNavigation: false,
                        buildStartStop: false,
                        autoPlay: true,     // If true, the slideshow will start running; replaces "startStopped" option
                        autoPlayLocked: true,
                        pauseOnHover: true,
                        onSlideBegin: function (e, slider) {
                            // keep the current navigation tab in view
                            slider.navWindow(slider.currentPage);
                        }
                    });
                });
            </script>
        </div>
        <div class="advertisementMain">
            <div id="RightAdvertisementsMain">
                <asp:Repeater ID="rptrRightAds" runat="server" OnItemDataBound="rptrRightAds_ItemDataBound">
                    <ItemTemplate>
                        <div class="rightSideAdMain">
                            <a href='<%# Eval("NavigateUrl") %>' target="_blank">
                                <asp:Image ID="imgAd" runat="server" />
                            </a>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <div class="rightSideAdMain" id="DefaultRightAd" runat="server" clientidmode="Static"
                    visible="false">
                    <script type="text/javascript"><!--
                        google_ad_client = "ca-pub-0955442517630650";
                        /* Vertical160x600 */
                        google_ad_slot = "2867735884";
                        google_ad_width = 160;
                        google_ad_height = 600;
//-->
                    </script>
                    <script type="text/javascript" src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
                    </script>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
