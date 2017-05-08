<%@ Page Title="" Language="C#" MasterPageFile="~/SleekSurf.Master" AutoEventWireup="true"
    CodeBehind="FAQs.aspx.cs" Inherits="SleekSurf.Web.WebPages.FAQs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/LoginDialog/jquery-1.4.3.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $(".content").each(function () {
                $(this).find(".answer:last").css("padding-bottom", "0px");
            });

            $(".sectionHeader").click(function () {
                $(this).next(".content").slideToggle("slow");
                if ($(this).is(".collapsed")) {
                    $(this).removeClass("collapsed").addClass("expanded");
                }
                else {
                    $(this).removeClass("expanded").addClass("collapsed");
                }
            });

            $(".question").click(function () {
                $(this).next(".answer").slideToggle("slow");
            });

            $("#RightAdvertisementsMain .rightSideAdMain:last").each(function () {
                $(this).css("margin-bottom", "0px");
            });
        }); 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapContent">
        <div id="FAQPanel">
            <div class="pageTitle">
                <p>
                    Frequently Asked Questions</p>
            </div>
            <p class="paragraph">
                Please select the category you require for support from the link below. If you don't
                find or have difficultities about the clarities of the description mentioned below
                please <a href="ContactUs.aspx">contact us</a> for clarification.
            </p>
            <ul class="group">
                <asp:Repeater ID="rptFaqGroups" runat="server" OnItemDataBound="rptFaqGroups_ItemDataBound">
                    <ItemTemplate>
                        <li class="faqGroup">
                            <asp:Label ID="lblFaqGroupID" runat="server" Text='<%# Eval("FaqGroupID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("GroupName") %>' CssClass="sectionHeader collapsed"></asp:Label>
                            <ul class="content">
                                <asp:Repeater ID="rptFaqs" runat="server">
                                    <ItemTemplate>
                                        <li class="faq">
                                            <div class="question paragraph">
                                                <asp:Literal ID="ltrAnswer" runat="server" Text='<%# Eval("Question") %>'></asp:Literal>
                                            </div>
                                            <div class="answer paragraph">
                                                <asp:Literal ID="ltrQuestion" runat="server" Text='<%# Eval("Answer") %>'></asp:Literal>
                                            </div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
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
