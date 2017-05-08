<%@ Page Title="" Language="C#" MasterPageFile="~/SleekSurf.Master" AutoEventWireup="true"
    CodeBehind="BusinessCatalog.aspx.cs" Inherits="SleekSurf.Web.WebPages.BusinessCatalog" %>

<%@ MasterType VirtualPath="~/SleekSurf.Master" %>
<%@ Register Src="~/WebPageControls/BusinessList.ascx" TagName="BusinessList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('img[id*=imgLogo]').each(function () {
                if ((typeof this.naturalWidth != "undefined" && this.naturalWidth == 0) || this.readyState == 'uninitialized') {
                    $(this).attr("src", "../App_Themes/SleekTheme/Images/BusinessLogoDefault.png");
                }
            });
        });
    </script>
    <div class="wrapContent">
    <div style="display:inline-block;">
        <uc1:BusinessList ID="ucBusinessList" runat="server" />
    </div>
       <div id="divAdRight" runat="server" class="advertisementMain">
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
                <%--<div class="rightSideAdMain" id="DefaultRightAd" runat="server" clientidmode="Static"
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
                </div>--%>
            </div>
        </div>
    </div>
</asp:Content>
