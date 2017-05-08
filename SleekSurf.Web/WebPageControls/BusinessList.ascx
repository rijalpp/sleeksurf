<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BusinessList.ascx.cs"
    Inherits="SleekSurf.Web.WebPageControls.BusinessList" %>
<%@ Register Src="~/WebPageControls/Pager.ascx" TagName="Pages" TagPrefix="uc1" %>
<script type="text/javascript">
    $(document).ready(function () {

        $(document).click(function () {
            $(".icon, .options").hide();
        });

        $(".business").click(function (e) {
            e.stopPropagation();
            //$(this).find(".options").hide();
        });
        $(".business").hover(function () {
            $(".icon, .options").not($(this).children().find(".icon, .options")).hide();
            $(this).find(".icon").show();
        }, function () {
           // $(this).addClass("hoverItem");
        });
        var option = $(".option");
        $(".icon").click(function (e) {
            $(this).next(".options").toggle("slow");
            $(this).prev().find("[id*=lblMessage]").removeClass("success");
            $(this).prev().find("[id*=lblMessage]").removeClass("error");
            $(this).prev().find("[id*=lblMessage]").text("");
            option.blur();
            e.stopPropagation();
        });

        $(".options").click(function (e) {
            e.stopPropagation();
        });
        option.focus(function () {
            if ($(this).val() == this.title) {
                $(this).removeClass("waterMark");
                $(this).val("");
                $(this).parent().next().next().show();
            }
        }).click(function (e) {
            e.stopPropagation();
        });

        option.blur(function () {
            if ($(this).val() == "" || $(this).val() == "Send Me Email  ") {
                $(this).addClass("waterMark");
                $(this).val(this.title);
                $(this).parent().next().next().hide();
                ValidatorEnable($($(this).parent().next().find("[id*=rfvSend]"))[0], true);
            }
            else
                ValidatorEnable($($(this).parent().next().find("[id*=rfvSend]"))[0], false);
        });

        option.blur();
    });
</script>
<div id="BusinessListings">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
    <div id="TopNavigation" runat="server" clientidmode="Static">
        <uc1:Pages ID="topPager" runat="server" Visible="false" />
    </div>
    <asp:Repeater ID="rptrBusinessList" runat="server" OnItemDataBound="rptrBusinessList_ItemDataBound">
        <ItemTemplate>
            <div class="business"><div class="detailsOption"><div class="messageStatus">
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" EnableTheming="false"></asp:Label>
                    </div>
                    <div class="icon"><img src="../../App_Themes/SleekTheme/Images/DropDownButton.png" alt="Options" title="Options" /></div>
                    <div class="options">
                        <ul><li>
                                <asp:Panel runat="server" DefaultButton="ibtnSendEmail">
                                    <span>
                                        <asp:TextBox ID="txtSendEmail" runat="server" CssClass="option" EnableTheming="false"
                                            ToolTip="Send Me Email  " EnableViewState="false" />
                                    </span><span>
                                        <asp:RequiredFieldValidator ID="rfvSendEmail" ErrorMessage="*" ControlToValidate="txtSendEmail"
                                            runat="server" />
                                    </span><span>
                                        <asp:ImageButton ID="ibtnSendEmail" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Enter.png"
                                            CausesValidation="true" OnCommand="ibtnSendEmail_Command" CommandArgument="<%# Container.ItemIndex %>"
                                            CommandName='<%# Eval("ClientID") %>' />
                                    </span><span style="text-align:left;display:block;padding-left:10px;">
                                        <asp:RegularExpressionValidator ID="rexBusinessEmail" runat="server" Display="Dynamic"
                                            EnableTheming="false" ControlToValidate="txtSendEmail" ErrorMessage="Invalid Email Format"
                                            Font-Size="10px" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                        </asp:RegularExpressionValidator>
                                    </span>
                                </asp:Panel>
                            </li></ul>
                    </div>
                </div>
                <div class="businessLogo">
                    <a id="anchorLogo" runat="server" target="_blank">
                        <div style="height:85px;overflow:hidden;">
                            <asp:Image ID="imgLogo" ImageUrl='<%# ProfileImageSource(Eval("ClientID").ToString(), Eval("LogoUrl").ToString()) %>'
                                runat="server" Width="190px" alt='logo' Style="border: none;" />
                        </div>
                    </a>
                </div>
                <div class="businessDetails">
                    <p class="businessName">
                        <a id="anchorBusinessName" runat="server" target="_blank">
                            <%# HttpUtility.HtmlEncode(Eval("ClientName").ToString())%></a></p>
                    <span class="businessLabel"><span>Contact Person:</span> <span>
                        <%# HttpUtility.HtmlEncode(GetContactPerson(Eval("ContactPerson").ToString()))%></span>
                    </span><span class="businessLabel"><span>Contact Number:</span> <span>
                        <%# HttpUtility.HtmlEncode(Eval("ContactOffice"))%></span> </span><span class="businessLabel">
                            <span>Business Email:</span> <span>
                                <%# HttpUtility.HtmlEncode(Eval("BusinessEmail").ToString())%></span>
                    </span><span class="businessLabel"><span>Address:</span> <span>
                        <%# HttpUtility.HtmlEncode(Eval("Address"))%>,
                        <%# HttpUtility.HtmlEncode(GetCountry(Eval("CountryID"))) %></span> </span><span
                            class="businessLabel" id="Website" runat="server"><span>Website:</span> <span>
                                <%# HttpUtility.HtmlEncode(Eval("BusinessUrl").ToString())%></span>
                    </span><span class="businessLabel" id="ContactFax" runat="server"><span>Fax:</span>
                        <span>
                            <%# HttpUtility.HtmlEncode(Eval("ContactFax").ToString())%></span> </span>
                    <div class="viewProfile">
                        <asp:Button ID="btnViewProfile" Text="No Profile" runat="server" Enabled="false" SkinID="Button"
                            OnClientClick="window.document.forms[0].target='_blank';" OnCommand="btnViewProfile_Command"
                            CommandArgument='<%# Eval("UniqueIdentity") %>' />
                    </div>
                </div>
            </div>
        </ItemTemplate>
        <SeparatorTemplate>
            <div style="border-top:1px solid #DDDDDD"></div>
        </SeparatorTemplate>
    </asp:Repeater>
    <div id="BottomNavigation" runat="server" clientidmode="Static">
        <div class="pagination">
            <uc1:Pages ID="bottomPager" runat="server" Visible="false" />
        </div>
    </div>
</div>
