<%@ Page Title="" Language="C#" MasterPageFile="~/ClientPageSite.master" AutoEventWireup="true" CodeBehind="PromotionInDetails.aspx.cs" Inherits="SleekSurf.Domain.PromotionInDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <link href='<%=Page.ResolveClientUrl("~/Scripts/Fancybox/fancybox.css") %>' rel="stylesheet"
        type="text/css" />
    <link href='<%=Page.ResolveClientUrl("~/Scripts/JCarousel/Themes/tango/skin.css") %>'
        rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPage" runat="server">
    <script src='<%=Page.ResolveClientUrl("~/Scripts/Fancybox/Fancybox-1.3.3.js") %>'
        type="text/javascript"></script>
    <script src='<%=Page.ResolveClientUrl("~/Scripts/FancyZoom/Fancyzoom.js") %>' type="text/javascript"></script>
    <script src='<%=Page.ResolveClientUrl("~/Scripts/JCarousel/jquery.jcarousel.min.js") %>'
        type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('img[id$=PromotionImage]').each(function () {
                if ((typeof this.naturalWidth != "undefined" && this.naturalWidth == 0) || this.readyState == 'uninitialized') {
                    $(this).attr("src", "App_Themes/Default/Images/PromotionDefault.png")
                }
            });

            $.fn.fancyzoom.defaultsOptions.imgDir = '<%=Page.ResolveClientUrl("~/Scripts/FancyZoom/Images/") %>';
            $('img[id$=PromotionImage]').fancyzoom();

            function mycarousel_initCallback(carousel) {
                // Disable autoscrolling if the user clicks the prev or next button.
                carousel.buttonNext.bind('click', function () {
                    carousel.startAuto();
                });

                carousel.buttonPrev.bind('click', function () {
                    carousel.startAuto();
                });

                // Pause autoscrolling if the user moves with the cursor over the clip.
                carousel.clip.hover(function () {
                    carousel.stopAuto();
                }, function () {
                    carousel.startAuto();
                });
            };

            $('.GalleryCarousel').jcarousel({
                auto: 2,
                scroll: 1,
                animation: 2000,
                wrap: 'last',
                initCallback: mycarousel_initCallback
            });

            $("a.fancyboxGallery").attr("rel", "ImageGallery").fancybox({ 'padding': 5 });
            $("a.fancyboxVideoGallery").click(function () {
                $.fancybox({
                    'padding': 7,
                    'content': $(this).attr("EmbedSrc")
                });
            });
        });
    </script>
    <div id="PromotionDetails">
        <div>
            <h2 class="header" style="color: #777777;">
                Promotion In Details</h2>
        </div>
        <div class="promo">
            <div class="promoImage">
                <div style="height: 85px; overflow: hidden;">
                    <asp:Image ID="PromotionImage" runat="server" />
                </div>
            </div>
            <div class="promoDetails">
                <h5>
                    <asp:Literal ID="ltrPromoTitle" runat="server" />
                </h5>
                <span class="promoDate"><span>Start Date:</span>
                    <asp:Literal ID="ltrStartDate" runat="server" />
                    - <span>End Date:</span>
                    <asp:Literal ID="ltrEndDate" runat="server" />
                </span>
            </div>
            <div class="promotionInDetailsDescription" style="clear: both; padding-top: 20px;">
                <asp:Literal ID="ltrDescription" runat="server"></asp:Literal>
            </div>
            <div id="PromotionImageGallery" runat="server" class="gallery">
                <div class="titleBar">
                    <div class="titleBarLeft">
                    </div>
                    <div class="titleBarMiddle">
                        Image Gallery
                    </div>
                    <div class="titleBarRight">
                    </div>
                </div>
                <div class="divider">
                </div>
                <div id="ImageGallery">
                    <ul class="GalleryCarousel jcarousel-skin-tango">
                        <asp:Repeater ID="rptrMediaGallery" runat="server" OnItemDataBound="rptrMediaGallery_ItemDataBound">
                            <ItemTemplate>
                                <li>
                                    <asp:HyperLink ID="hlnkMediaGallery" runat="server" ToolTip='<%# Eval("Caption") %>' rel="ImageGallery" CssClass="fancyboxGallery">
                                        <asp:Image ID="imgMediaGallery" runat="server" Width="75px" Height="75px" AlternateText='<%# Eval("Title") %>' BorderWidth="0px" />
                                    </asp:HyperLink>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <div class="footer">
                    <div class="footerLeft">
                    </div>
                    <div class="footerMiddle">
                    </div>
                    <div class="footerRight">
                    </div>
                </div>
            </div>
            <div id="PromotionVideoGallery" runat="server" class="gallery">
                <div class="titleBar">
                    <div class="titleBarLeft">
                    </div>
                    <div class="titleBarMiddle">
                        Video Gallery
                    </div>
                    <div class="titleBarRight">
                    </div>
                </div>
                <div class="divider">
                </div>
                <div id="VideoGallery">
                    <ul class="GalleryCarousel jcarousel-skin-tango">
                        <asp:Repeater ID="rptrVideoGallery" runat="server" OnItemDataBound="rptrVideoGallery_ItemDataBound">
                            <ItemTemplate>
                                <li>
                                    <asp:HyperLink ID="hlnkMediaGallery" runat="server"  CssClass="fancyboxVideoGallery" ToolTip='<%# Eval("Caption") %>' >
                                        <asp:Image ID="imgMediaGallery" runat="server" Width="75px" Height="75px" AlternateText='<%# Eval("Title") %>'
                                            ImageUrl="~/App_Themes/Default/Images/DefaultVideoThumbnail.gif" BorderWidth="0px" />
                                    </asp:HyperLink>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <div class="footer">
                    <div class="footerLeft">
                    </div>
                    <div class="footerMiddle">
                    </div>
                    <div class="footerRight">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

