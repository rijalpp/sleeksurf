<%@ Page Title="" Language="C#" MasterPageFile="~/ClientPageSite.master" AutoEventWireup="true" CodeBehind="FAQs.aspx.cs" Inherits="SleekSurf.Domain.FAQs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPage" runat="server">
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
        });
    </script>
    <div id="FAQPanel">
        <div>
            <h2 class="header" style="color: #777777;">
                Frequently Asked Questions</h2>
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
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("GroupName") %>' CssClass="sectionHeader collapsed"
                            ForeColor="#897e6e"></asp:Label>
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
</asp:Content>
