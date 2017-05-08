<%@ Page Title="" Language="C#" MasterPageFile="~/ClientPageSite.master" AutoEventWireup="true" CodeBehind="Promotion.aspx.cs" Inherits="SleekSurf.Domain.Promotion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPage" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('img[id*=PromotionImage]').each(function () {
                if ((typeof this.naturalWidth != "undefined" && this.naturalWidth == 0) || this.readyState == 'uninitialized') {
                    $(this).attr("src", "App_Themes/Default/Images/PromotionDefault.png")
                }
            });
        });
    </script>
    <div id="ClientContentPromotion">
        <div>
            <h2 class="header" style="color: #777777;">
                Our Promotions</h2>
        </div>
        <div id="PromotionList" runat="server" clientidmode="Static">
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            <div class="paginationSummary">
                <asp:Panel ID="pnlgvPersonNavigatorTop" runat="server" Visible="false">
                    <!-- wrapper of upper part  -->
                    <div class="summary">
                        <span style="margin-right: 2px">
                            <asp:Label ID="Label7" runat="server" Font-Bold="true" Text="Page "></asp:Label></span>
                        <span style="margin-right: 5px">
                            <asp:Label ID="lblStartPage" runat="server"></asp:Label></span> <span style="margin-right: 5px;
                                font-weight: bold;">of</span> <span style="margin-right: 5px">
                                    <asp:Label ID="lblTotalPages" runat="server"></asp:Label></span>
                    </div>
                    <div class="summary" style="float: right;">
                        <span style="margin-right: 2px; font-weight: bold;">Total Record(s):</span> <span>
                            <asp:Label ID="lblTotalNo" runat="server"></asp:Label></span>
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:Repeater ID="rptrPromoList" runat="server">
                    <ItemTemplate>
                        <span class="promo">
                            <div class="promoImage">
                                <div style="height: 85px; overflow: hidden;">
                                    <asp:Image ID="PromotionImage" ImageUrl='<%# Bind("PromotionID","~/DisplayImage.aspx?ID={0}&SECTION=TITLE") %>'
                                        runat="server" AlternateText="Product Image" />
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
            </div>
            <div class="pagination">
                <asp:Panel ID="pnlNavigatorBottom" runat="server" Visible="false">
                    <!-- wrapper of lower part  -->
                    <div>
                        <asp:LinkButton ID="lbtnPrevious" runat="server" Text="Previous" CommandName="Previous"
                            OnCommand="ChangePage" SkinID="Pagination"></asp:LinkButton>
                    </div>
                    <div style="padding: 0px 20px;">
                        <asp:Repeater ID="rptPager" runat="server" OnItemCommand="rptPager_ItemCommand">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnPagerButton" runat="server" Text='<%# Eval("Page") %>' CommandName='<%# Eval("Page") %>'
                                    SkinID="Pagination">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div>
                        <asp:LinkButton ID="lbtnNext" runat="server" Text="Next" CommandName="Next" OnCommand="ChangePage"
                            SkinID="Pagination"></asp:LinkButton>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
