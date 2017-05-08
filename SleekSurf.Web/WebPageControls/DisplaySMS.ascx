<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DisplaySMS.ascx.cs"
    Inherits="SleekSurf.Web.WebPageControls.DisplaySMS" %>
<script type="text/javascript">
    $(function () {
        $("[id*=rbtnlSMSOption] input").addClass("styled");
        $("[id*=rbtnlSMSOption] input").next("label").css("width", "300px");

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
<script src='<%= Page.ResolveClientUrl("~/Scripts/JScript/CustomFormElements.js") %>'
    type="text/javascript"></script>
<div id="Package">
    <asp:MultiView ID="mViewSMS" ActiveViewIndex="0" runat="server">
        <asp:View ID="vSMS" runat="server">
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
                        <asp:Image ID="Image3" ImageUrl="~/App_Themes/SleekTheme/Images/RechargeSMS.png"
                            runat="server" />
                    </div>
                    <div class="packageTitle">
                        <asp:Literal runat="server" Text="Recharge SMS"></asp:Literal>
                    </div>
                    <div class="packageTitleMirror">
                    </div>
                    <div class="packageFeatures">
                        <h3>
                            Description:</h3>
                        <asp:Literal ID="ltrDescription" runat="server" Text="Please select SMS credit from the options provided below to suit your business need."></asp:Literal>
                    </div>
                    <div class="packageOptions">
                        <h3>
                            SMS Options:</h3>
                        <asp:RadioButtonList ID="rbtnlSMSOption" runat="server">
                        </asp:RadioButtonList>
                        <span style="padding-left: 28px">
                            <asp:RequiredFieldValidator ID="rfvPackageOption" runat="server" ControlToValidate="rbtnlSMSOption"
                                ErrorMessage="Recharge option required.">
                            </asp:RequiredFieldValidator>
                        </span>
                        <div style="padding-left: 28px;">
                            <asp:TextBox ID="txtPromoCode" runat="server" CssClass="promoCode"></asp:TextBox>
                            <span>
                                <asp:Label ID="lblPromoCodeMessage" runat="server" EnableViewState="false" SkinID="Error"></asp:Label></span>
                        </div>
                    </div>
                    <div class="packageSelection" style="float: right">
                        <asp:Button ID="btnRecharge" runat="server" Text="Recharge" OnClick="btnRecharge_Click"
                            SkinID="Button" />
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
        </asp:View>
        <asp:View ID="vConfirmation" runat="server">
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
                        <asp:Image ID="Image5" ImageUrl="~/App_Themes/SleekTheme/Images/PackageIcons/UniqueUrl.png"
                            runat="server" />
                        <asp:Image ID="Image6" ImageUrl="~/App_Themes/SleekTheme/Images/PackageIcons/MultiUsers.png"
                            runat="server" />
                        <asp:Image ID="Image7" ImageUrl="~/App_Themes/SleekTheme/Images/PackageIcons/Share.png"
                            runat="server" />
                        <asp:Image ID="Image8" ImageUrl="~/App_Themes/SleekTheme/Images/PackageIcons/EmailSMS.png"
                            runat="server" />
                        <asp:Image ID="Image9" ImageUrl="~/App_Themes/SleekTheme/Images/PackageIcons/ContactUs.png"
                            runat="server" />
                    </div>
                    <div class="packageTitle">
                        <asp:Literal runat="server" Text="Recharge SMS"></asp:Literal>
                    </div>
                    <div class="packageTitleMirror">
                    </div>
                    <div class="packageFeatures">
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                        <h3>
                            Confirmation:</h3>
                        <div class="packageConfirmation">
                            <div class="textSummary">
                                <span>Recharge Option</span> -
                                <asp:Literal ID="ltrRechargeOption" runat="server"></asp:Literal>
                            </div>
                            <div class="textSummary">
                                <span>Recharge Cost</span> -
                                <asp:Literal ID="ltrStandardPrice" runat="server"></asp:Literal></div>
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
                            <div class="packageSelection" style="float: right">
                                <span style="margin: 0px;">
                                    <asp:Button ID="btnChange" runat="server" Text="Change" SkinID="Button" OnClick="btnChange_Click" />
                                </span><span style="margin: 0px 5px">
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
        </asp:View>
    </asp:MultiView>
</div>
