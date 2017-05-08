<%@ Page Title="" Language="C#" MasterPageFile="~/Client/ClientSite.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="SleekSurf.Web.Client.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href='<%=Page.ResolveClientUrl("~/Scripts/EasySlider/screen.css") %>' rel="stylesheet"
        type="text/css" />
    <link href='<%=Page.ResolveClientUrl("~/Scripts/Fancybox/fancybox.css") %>' rel="stylesheet"
        type="text/css" />
    <link href='<%=Page.ResolveClientUrl("~/Scripts/BxSlider/bx_styles/bx_styles.css") %>'
        rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"> </script>
    <script src='<%=Page.ResolveClientUrl("~/Scripts/JScript/site.js") %>' type="text/javascript"></script>
    <script src='<%=Page.ResolveClientUrl("~/Scripts/BxSlider/jquery.bxSlider.min.js") %>'
        type="text/javascript"></script>
    <script src='<%=Page.ResolveClientUrl("~/Scripts/EasySlider/easySlider1.7.js") %>'
        type="text/javascript"></script>
    <script src='<%=Page.ResolveClientUrl("~/Scripts/Fancybox/Fancybox-1.3.3.js") %>'
        type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#Slider").easySlider({
                auto: false,
                continuous: true,
                numeric: true
            });

            $("a.FancyboxNewEdit").fancybox({
                'padding': 0,
                'autoDimensions': true,
                'width': 1000,
                'height': 800,
                'modal': false,
                'scrolling': 'auto',
                'centerOnScroll': true,
                'type': 'iframe'
            });
            $('#PromoSlider').bxSlider({
                auto: true,
                autoControls: true,
                autoHover: true,
                pager: false,
                controls: false
            });

            $('img[id*=PromotionImage]').each(function () {
                if ((typeof this.naturalWidth != "undefined" && this.naturalWidth == 0)) {
                    $(this).attr("src", "../App_Themes/Default/Images/PromotionDefault.png");
                }
            });
        });
    </script>
    <div id="ClientContentPage">
        <div id="ClientContent">
            <div class="featuredInside">
                <span class='fancyborder fancyborderTop'></span><span class='fancyborder fancyborderLeft'>
                </span><span class='fancyborder fancyborderRight'></span><span class='fancyborder fancyborderBottom'>
                </span>
                <ul id="PromoSlider">
                    <li id="PromotionSlide1" runat="server" visible="false">
                        <img src="../Client/Images/Promotion1Default.png" alt="Promotion 1" />
                    </li>
                    <li id="PromotionSlide2" runat="server" visible="false">
                        <img src="../Client/Images/Promotional2Default.png" alt="Promotion 2" />
                    </li>
                    <asp:Repeater ID="rptrPromotionSlider" runat="server">
                        <ItemTemplate>
                            <li class="promotionSlide">
                                <div class="promotionImage">
                                    <div style="min-height:100px;max-height:172px;overflow:hidden;">
                                       <asp:Image ID="PromotionImage" ImageUrl='<%# Bind("PromotionID","~/DisplayImage.aspx?ID={0}&SECTION=TITLE") %>'
                                            Width="325px" runat="server" AlternateText="Product Image"  />
                                    </div>
                                </div>
                                <div class="promotionInfo">
                                    <h2><%# Eval("Title") %></h2>
                                    <p class="promotiondate">Start Date :<%# Eval("StartDate","{0:dd/MM/yyyy}")%></p>
                                    <p class="promotiondate">End Date :<%# Eval("EndDate","{0:dd/MM/yyyy}")%></p>
                                    <p class="detailbutton">
                                        <asp:LinkButton ID="lbtnViewInDetails" runat="server" CommandArgument='<%# Eval("PromotionID") %>'
                                            OnCommand="Promotion_Command">View In Detail</asp:LinkButton></p>
                                </div>
                                <div class="promotionDescription"><h3>Description</h3>
                                    <p class="descriptiontext"><%# this.GetShortDescription(Eval("Description").ToString(), 60)%></p>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>
        <div id="ClientContentPromotionLocation">
            <div id="ClientContentPromotion">
                <div class="promoHeader">
                    <h2><asp:Label ID="lblPromotionHeader" runat="server"></asp:Label></h2>
                </div>
                <div id="ContactDetails" runat="server">
                    <div>
                        <label>Contact Person:</label>
                        <asp:Label ID="lblContactPerson" runat="server" Font-Bold="true"></asp:Label>
                    </div>
                    <div>
                        <label>Business Email:</label>
                        <asp:Label ID="lblBusinessEmail" runat="server" Font-Bold="true"></asp:Label>
                    </div>
                    <div>
                        <label>Contact Number:</label>
                        <asp:Label ID="lblContactNumber" runat="server" Font-Bold="true"></asp:Label>
                    </div>
                    <div>
                        <label>Fax Number:</label>
                        <asp:Label ID="lblFax" runat="server" Font-Bold="true"></asp:Label>
                    </div>
                </div>
                <div id="Slider" runat="server" clientidmode="Static">
                    <ul>
                        <asp:Repeater ID="rptrPageBlock" runat="server" OnItemDataBound="rptrPageBlock_ItemDataBound">
                            <ItemTemplate>
                                <li>
                                    <asp:Repeater ID="rptrPromoList" runat="server">
                                        <ItemTemplate>
                                            <span class="promo">
                                                <div class="promoImage">
                                                    <div style="height:85px;overflow:hidden;">
                                                        <asp:Image ID="PromotionImage" ImageUrl='<%# Bind("PromotionID","~/DisplayImage.aspx?ID={0}&SECTION=TITLE") %>'
                                                            runat="server" ToolTip='<%# Eval("PromotionID") %>'  AlternateText="Product Image" />
                                                    </div>
                                                </div>
                                                <div class="promoDetails">
                                                    <h5>
                                                        <asp:LinkButton ID="lbtnPromoTitle" runat="server" CssClass="promoTitle" CommandArgument='<%# Eval("PromotionID") %>'
                                                            OnCommand="Promotion_Command"><%# Eval("Title") %></asp:LinkButton>
                                                    </h5>
                                                    <span class="promoDate"><span>Start Date:</span>
                                                        <%# Eval("StartDate","{0:dd/MM/yyyy}")%>
                                                        - <span>End Date:</span>
                                                        <%# Eval("EndDate","{0:dd/MM/yyyy}")%></span> <span class="promoDescription">
                                                            <%# this.GetShortDescription(Eval("Description").ToString(), 20)%>
                                                        </span>
                                                    <asp:LinkButton ID="lbtnReadMore" runat="server" CssClass="readMore" CommandArgument='<%# Eval("PromotionID") %>'
                                                        OnCommand="Promotion_Command">View in Detail ...</asp:LinkButton>
                                                </div>
                                            </span>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
            <div id="ClientContentLocation">
                <div class="locationHeader">
                    <h2 style="width:135px;">Location</h2>
                </div>
                <div id="location">
                    <div id="map_canvas" style="height: 330px; width: 330px; position: relative; z-index: 10;">
                        <div id="loadingImage" style="width:180px;height:180;padding-top:100px;margin: 0px auto;visibility:visible;">
                            <img src="../App_Themes/Default/Images/Loading.gif" alt="loading" />
                        </div>
                    </div>
                    <div style="text-align:center;display:block;padding-top:10px">
                        <asp:Label CssClass="address" ID="lblAddress" runat="server" Text="4 Marlborough Road, Homebush West, NSW 2140, Australia"></asp:Label>
                    </div>
                    <div id="GetDirection">
                        <div>
                            <label style="width:120px">From: </label>
                            <asp:TextBox ID="txtFromAddress" runat="server" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorAddress" ErrorMessage="*" ControlToValidate="txtFromAddress"
                                runat="server" ValidationGroup="GetDirection" Display="Dynamic" />
                        </div>
                        <div>
                            <label style="width:120px"> Mode of Travel: </label>
                            <asp:DropDownList ID="ddlMode" runat="server" ClientIDMode="Static" ValidationGroup="GetDirection"
                                CausesValidation="true">
                                <asp:ListItem Value="Select Travelling Mode" Text="Select Travelling Mode"></asp:ListItem>
                                <asp:ListItem Value="Driving" Text="Driving"></asp:ListItem>
                                <asp:ListItem Value="Walking" Text="Walking"></asp:ListItem>
                                <asp:ListItem Value="Bicycling" Text="Bicycling"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorMode" ErrorMessage="*" ControlToValidate="ddlMode"
                                runat="server" ValidationGroup="GetDirection" Display="Dynamic" InitialValue="Select Travelling Mode" />
                        </div>
                        <div class="viewInLargePanel">
                            <asp:HyperLink ID="HyperLinkViewLargeMap" runat="server" Text="View In Large" CssClass="FancyboxNewEdit"
                                ClientIDMode="Static"></asp:HyperLink>
                        </div>
                        <div id="DirectionsPanel" style="width:330px;"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
