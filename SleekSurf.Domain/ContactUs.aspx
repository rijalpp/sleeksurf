<%@ Page Title="" Language="C#" MasterPageFile="~/ClientPageSite.master" AutoEventWireup="true" CodeBehind="ContactUs.aspx.cs" Inherits="SleekSurf.Domain.ContactUs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <link href='<%=Page.ResolveClientUrl("~/Scripts/Fancybox/fancybox.css") %>' rel="stylesheet"
        type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPage" runat="server">
    <script src='<%=Page.ResolveClientUrl("~/Scripts/JScript/site.js") %>' type="text/javascript"></script>
    <script src='<%=Page.ResolveClientUrl("~/Scripts/Fancybox/Fancybox-1.3.3.js") %>'
        type="text/javascript"></script>
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"> </script>
    <script type="text/javascript">
        $(document).ready(function () {

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

            $("#PageMainContent").css("width", "1024px");
            $(".advertisement, #ClientFooterAdWrapper").hide();
        });
    </script>
    <div id="ContactForm">
        <h2 class="header" style="color: #777777;">
            Contact Us</h2>
        <p style="text-align: justify; padding: 10px 0px;">
            If you have any queries, please feel free to send us an email. We'll get back to
            you as soon as possible.<br />
            <br />
            Thank you for visiting Us.</p>
        <asp:UpdatePanel ID="upSendEmail" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                <fieldset style="border: none; padding: 0px;">
                    <label style="font-size: 16px; color: #111; padding-top: 15px; width: 100%">
                        Send us your query.</label>
                    <div style="padding: 10px 0px;">
                        <asp:TextBox ID="txtName" runat="server" Width="588px" SkinID="Comment">&nbsp;</asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWaterMarkName" runat="server" WatermarkText="Enter your name*"
                            TargetControlID="txtName">
                        </cc1:TextBoxWatermarkExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorName" runat="server" ErrorMessage="Name is required"
                            ControlToValidate="txtName" ValidationGroup="Comment" Display="None"></asp:RequiredFieldValidator>
                        <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" TargetControlID="RequiredFieldValidatorName"
                            runat="server">
                        </cc1:ValidatorCalloutExtender>
                        <asp:TextBox ID="txtEmail" runat="server" Width="588px" SkinID="Comment">&nbsp;</asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderEmail" runat="server" WatermarkText="Enter your email*"
                            TargetControlID="txtEmail">
                        </cc1:TextBoxWatermarkExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmail" runat="server" ErrorMessage="Email is required"
                            ValidationGroup="Comment" Display="None" ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail" runat="server"
                            ControlToValidate="txtEmail" ErrorMessage="Invalid Email Expression" Display="None"
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" TargetControlID="RequiredFieldValidatorEmail"
                            runat="server">
                        </cc1:ValidatorCalloutExtender>
                        <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" TargetControlID="RegularExpressionValidatorEmail"
                            runat="server">
                        </cc1:ValidatorCalloutExtender>
                        <asp:TextBox ID="txtWebsite" runat="server" Width="588px" SkinID="Comment" ClientIDMode="Static">&nbsp;</asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderWebsite" runat="server"
                            WatermarkText="Enter your website" TargetControlID="txtWebsite">
                        </cc1:TextBoxWatermarkExtender>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorWebsite" runat="server"
                            ErrorMessage="Invalid URL Expression" Display="None" ControlToValidate="txtWebsite"
                            ValidationExpression="^(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ;,./?%&=]*)?$"></asp:RegularExpressionValidator>
                        <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" TargetControlID="RegularExpressionValidatorWebsite"
                            runat="server">
                        </cc1:ValidatorCalloutExtender>
                        <asp:TextBox ID="txtComment" runat="server" Width="588px" TextMode="MultiLine" Height="200px"
                            SkinID="Comment">
                        </asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWaterMarkComment" runat="server" WatermarkText="Your Message Here*"
                            TargetControlID="txtComment">
                        </cc1:TextBoxWatermarkExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorComment" runat="server" ErrorMessage="Comment is required"
                            ControlToValidate="txtComment" ValidationGroup="Comment" Display="None">
                        </asp:RequiredFieldValidator>
                        <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" TargetControlID="RequiredFieldValidatorComment"
                            runat="server">
                        </cc1:ValidatorCalloutExtender>
                        <br />
                        <br />
                        <asp:Button ID="btnSubmit" runat="server" ValidationGroup="Comment" Text="Submit"
                            SkinID="ClientProfile" OnClick="btnSubmit_Click" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="ContactExtension">
        <h2 class="header" style="color: #777777;">
            Contact Info</h2>
        <p>
            <label>
                Phone:
            </label>
            <%=SleekSurf.FrameWork.WebContext.ClientProfile.ContactOffice%></p>
        <p>
            <label>
                Fax:
            </label>
            <%=SleekSurf.FrameWork.WebContext.ClientProfile.ContactFax %></p>
        <p>
            <label>
                Email:
            </label>
            <%=SleekSurf.FrameWork.WebContext.ClientProfile.BusinessEmail%></p>
        <div id="ClientContentLocation">
            <div class="locationHeader">
                <h2 style="width: 135px; color: #777777;">
                    Location</h2>
            </div>
            <div id="location">
                <div id="map_canvas" style="height: 330px; width: 330px; position: relative; z-index: 10;">
                    <div id="loadingImage" style="width: 180px; height: 180; padding-top: 100px; margin: 0px auto;
                        visibility: visible;">
                        <img src="App_Themes/Default/Images/Loading.gif" alt="loading" />
                    </div>
                </div>
                <div style="text-align: center; display: block; padding-top: 10px">
                    <asp:Label CssClass="address" ID="lblAddress" runat="server" Text="Address couldn't be mapped"></asp:Label>
                </div>
                <div id="GetDirection">
                    <div>
                        <label style="width: 120px">
                            From:
                        </label>
                        <asp:TextBox ID="txtFromAddress" runat="server" ClientIDMode="Static"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorAddress" ErrorMessage="*" ControlToValidate="txtFromAddress"
                            runat="server" ValidationGroup="GetDirection" Display="Dynamic" />
                    </div>
                    <div>
                        <label style="width: 120px">
                            Mode of Travel:
                        </label>
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
                    <div id="DirectionsPanel" style="width: 330px;">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

