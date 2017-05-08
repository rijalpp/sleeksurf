<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DisplayPackages.ascx.cs"
    Inherits="SleekSurf.Web.WebPageControls.DisplayPackages" %>
<link href='<%= Page.ResolveClientUrl("~/Scripts/AnythingSlider/css/anythingslider.css") %>'
    rel="stylesheet" type="text/css" />
<link href='<%= Page.ResolveClientUrl("~/Scripts/AnythingSlider/css/theme-metallic.css") %>'
    rel="stylesheet" type="text/css" />
<script src='<%= Page.ResolveClientUrl("~/Scripts/jquery-1.7.js") %>' type="text/javascript"></script>
<script src='<%= Page.ResolveClientUrl("~/Scripts/AnythingSlider/jquery.anythingslider.js") %>'
    type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("[id*=rbtnlPackageOption] input").addClass("styled");
        $("[id*=rbtnlPackageOption] input").next("label").css("width", "300px");

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
<asp:MultiView ID="mViewPackage" ActiveViewIndex="0" runat="server">
    <asp:View ID="vPackages" runat="server">
        <ul id="packageSlide">
            <asp:Repeater ID="rptrPackages" runat="server" OnItemDataBound="rptrPackages_ItemDataBound"
                OnItemCommand="rptrPackages_ItemCommand">
                <ItemTemplate>
                    <li>
                        <div class="package">
                            <div id="header">
                                <div class="packageHeaderLeft">
                                </div>
                                <div class="packageHeaderMiddle">
                                </div>
                                <div class="packageHeaderRight">
                                </div>
                            </div>
                            <asp:Label ID="lblPackageCode" runat="server" Text='<%# Eval("PackageCode") %>' Visible="false"></asp:Label>
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
                                        <asp:Literal ID="ltrPackageTitle" runat="server" Text='<%# Eval("PackageName") %>'></asp:Literal>
                                    </div>
                                    <div class="packageTitleMirror">
                                    </div>
                                    <div class="packageFeatures">
                                        <h3>
                                            Features:</h3>
                                        <asp:Literal ID="ltrDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Literal>
                                    </div>
                                    <div class="packageOptions">
                                        <h3>
                                            Package Options:</h3>
                                        <asp:RadioButtonList ID="rbtnlPackageOption" runat="server">
                                        </asp:RadioButtonList>
                                        <span style="padding-left: 28px">
                                            <asp:RequiredFieldValidator ID="rfvPackageOption" runat="server" ControlToValidate="rbtnlPackageOption"
                                                ErrorMessage="Package option required.">
                                            </asp:RequiredFieldValidator>
                                        </span>
                                        <div style="padding-left: 28px">
                                            <asp:TextBox ID="txtPromoCode" runat="server" CssClass="promoCode"></asp:TextBox>
                                            <span>
                                                <asp:Label ID="lblPromoCodeMessage" runat="server" EnableViewState="false" SkinID="Error"></asp:Label></span>
                                        </div>
                                    </div>
                                    <div class="packageSelection" style="float: right">
                                        <asp:Button ID="btnPurchase" runat="server" Text="Purchase" CommandName='<%# Eval("PackageCode") %>'
                                            SkinID="Button"></asp:Button>
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
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </asp:View>
    <asp:View ID="vConfirmation" runat="server">
        <div class="package">
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
                        <asp:Literal ID="ltrPacakageTitle" runat="server"></asp:Literal>
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
                                <asp:Literal ID="ltrPackageOption" runat="server"></asp:Literal>
                            </div>
                            <div class="textSummary">
                                <span>Package Cost</span> -
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
        </div>
    </asp:View>
</asp:MultiView>
