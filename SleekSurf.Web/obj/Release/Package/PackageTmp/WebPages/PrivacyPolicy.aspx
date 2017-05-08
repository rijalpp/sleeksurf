<%@ Page Title="" Language="C#" MasterPageFile="~/SleekSurf.Master" AutoEventWireup="true"
    CodeBehind="PrivacyPolicy.aspx.cs" Inherits="SleekSurf.Web.WebPages.PrivacyPolicy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapContent">
        <div id="PrivacyPolicy">
            <div class="pageTitle">
                <p>
                    Privacy Policy</p>
            </div>
            <%=SleekSurf.Manager.ClientManager.SelectDataExtenderWithClientNull().EntityList[0].PrivacyAndPolicy%>
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
