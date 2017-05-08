<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="MatchDomain.aspx.cs" Inherits="SleekSurf.Web.Admin.Client.MatchDomain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".promoCode").focus(function () {
                if ($(this).val() == "I have promo code!") {
                    $(this).removeClass("waterMark");
                    $(this).val("");
                }
            });

            $(".promoCode").blur(function () {
                if ($(this).val() == "" || $(this).val() == "I have promo code!") {
                    $(this).addClass("waterMark");
                    $(this).val("I have promo code!");
                    $("[id*=lblPromoCodeMessage]").text("");
                }
            });

            $(".promoCode").blur();
        });
    </script>
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: 100%">
                    <asp:Label ID="lblTitle" runat="server" Text="Account Management Portal" EnableViewState="false"></asp:Label></div>
            </div>
            <div>
                <div id="header">
                    <div class="packageHeaderLeft">
                    </div>
                    <div class="packageHeaderMiddle">
                    </div>
                    <div class="packageHeaderRight">
                    </div>
                </div>
                <div id="Content">
                    <div class="packageContentLeft">
                    </div>
                    <div class="packageContentMiddle">
                        <div class="packageIcons">
                            <asp:Repeater ID="rptPackageIcons" runat="server">
                                <ItemTemplate>
                                    <asp:Image ID="Image" runat="server" ImageUrl='<%# Bind("PictureID", "~/Uploads/PackageImages/{0}") %>'
                                        ToolTip='<%# Eval("PictureDescription") %>' />
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div class="packageTitle">
                            <asp:Literal ID="Literal1" runat="server" Text="Add Domain"></asp:Literal>
                        </div>
                        <div class="packageTitleMirror">
                        </div>
                        <div class="packageFeatures">
                            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                            <h3>
                                Confirmation:</h3>
                            <div class="packageConfirmation">
                                <div class="textSummary">
                                    <span>Package Option</span> -
                                    <asp:Literal ID="ltrMatch" runat="server" Text="Match"></asp:Literal>
                                </div>
                                <div class="textSummary">
                                    <span>Adding Cost</span> -
                                    <asp:Literal ID="ltrAddingCost" runat="server"></asp:Literal></div>
                                <asp:Panel ID="pnlPromoCodeEntered" runat="server" Visible="false">
                                    <div class="textSummary">
                                        <span>Discounted Value</span> -
                                        <asp:Literal ID="ltrDiscountedValue" runat="server"></asp:Literal>
                                        <asp:Literal ID="ltrDiscountOffer" runat="server"></asp:Literal></div>
                                </asp:Panel>
                                <hr style="margin: 10px 0px; height: 3px;" />
                                <div class="textSummary">
                                    <span>Final Cost</span> -
                                    <asp:Literal ID="ltrTotalCost" runat="server"></asp:Literal>
                                </div>
                                <div style="padding-left: 28px">
                                    <asp:TextBox ID="txtPromoCode" runat="server" CssClass="promoCode" OnTextChanged="txtPromoCode_TextChanged"
                                        AutoPostBack="true"></asp:TextBox>
                                    <span>
                                        <asp:Label ID="lblPromoCodeMessage" runat="server" EnableViewState="false" SkinID="Error"></asp:Label></span>
                                </div>
                                <div class="packageSelection" style="float: right">
                                    <span style="margin: 0px 5px">
                                        <asp:Button ID="btnConfirm" runat="server" Text="Confirm" SkinID="Button" OnClick="btnConfirm_Click">
                                        </asp:Button>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="packageContentRight">
                    </div>
                </div>
                <div id="Footer">
                    <div class="packageFooterLeft">
                    </div>
                    <div class="packageFooterMiddle">
                    </div>
                    <div class="packageFooterRight">
                    </div>
                </div>
            </div>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks" runat="server" id="dvRightLinks">
                <asp:HyperLink ID="hlRenewAccount" runat="server" Text="Renew Account" NavigateUrl="~/Admin/Client/AccountRenew.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="hlBuySMSCredit" runat="server" Text="Buy SMS Credit" NavigateUrl="~/Admin/Client/RechargeSMS.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="hlTransactionHistory" runat="server" Text="Transaction History"
                    NavigateUrl="~/Admin/Client/AccountTransactionManagement.aspx" CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="hlMatchProfile" runat="server" Text="Add Unique Profile" NavigateUrl="~/Admin/Client/MatchProfile.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="hlMatchDomain" runat="server" Text="Add Domain" NavigateUrl="~/Admin/Client/MatchDomain.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
