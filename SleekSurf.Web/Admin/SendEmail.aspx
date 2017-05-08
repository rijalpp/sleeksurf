<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true" CodeBehind="SendEmail.aspx.cs" Inherits="SleekSurf.Web.Admin.SendEmail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/AutoComplete/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ToggleCcBcc a").click(function () {
                $(this).text($(this).text() == "Show Cc & Bcc" ? "Hide Cc & Bcc" : "Show Cc & Bcc");
                $(".email").slideToggle("slow", function () {
                    $(".email :text").val('').focus().blur();
                });
            })
            .hover(function () {
                $(this).css("text-decoration", "underline");
            },
            function () {
                $(this).css("text-decoration", "none");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="SendEmailPanel">
            <fieldset>
                <legend>Compose Email</legend>
                <div style="padding-left: 80px">
                    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" /></div>
                <div>
                    <label>
                        From:</label>
                    <span style="width: 665px; display: inline-block; padding-left: 5px;">
                        <asp:Label ID="lblMyEmail" runat="server"></asp:Label></span> <span id="ToggleCcBcc"
                            style="width: 185px; display: inline-block; text-align: right; padding-right: 10px;">
                            <a style="color: #2a96cc; cursor: pointer;">Show Cc & Bcc</a></span>
                </div>
                <div>
                    <label>
                        <span class="mandatory">*</span> To:</label>
                    <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
                    <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderTo" runat="server" WatermarkText="Separate email address(es) by ';'"
                        WatermarkCssClass="waterMark" TargetControlID="txtTo">
                    </ajax:TextBoxWatermarkExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorTo" runat="server" ErrorMessage="*"
                        ControlToValidate="txtTo" Display="Dynamic" ValidationGroup="SendEmailGroup">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidatorTo" runat="server"
                        Display="none" ControlToValidate="txtTo" ErrorMessage="Invalid Email(s) Format"
                        ValidationGroup="SendEmailGroup" ValidationExpression="(([ ]*)(\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)([ ]*)([;,]?)([ ]*))+">
                    </asp:RegularExpressionValidator>
                    <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtenderTo" runat="server" TargetControlID="RegularExpressionValidatorTo">
                    </ajax:ValidatorCalloutExtender>
                </div>
                <div class="email" style="display: none;">
                    <label>
                        Cc:</label>
                    <asp:TextBox ID="txtCc" runat="server"></asp:TextBox>
                    <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderCc" runat="server" WatermarkText="Separate email address(es) by ';'"
                        WatermarkCssClass="waterMark" TargetControlID="txtCc">
                    </ajax:TextBoxWatermarkExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidatorCc" runat="server"
                        Display="none" ControlToValidate="txtCc" ErrorMessage="Invalid Email(s) Format"
                        ValidationGroup="SendEmailGroup" ValidationExpression="(([ ]*)(\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)([ ]*)([;,]?)([ ]*))+">
                    </asp:RegularExpressionValidator>
                    <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtenderCc" runat="server" TargetControlID="RegularExpressionValidatorCc">
                    </ajax:ValidatorCalloutExtender>
                </div>
                <div class="email" style="display: none;">
                    <label>
                        Bcc:</label>
                    <asp:TextBox ID="txtBcc" runat="server"></asp:TextBox>
                    <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderBcc" runat="server" WatermarkText="Separate email address(es) by ';'"
                        WatermarkCssClass="waterMark" TargetControlID="txtBcc">
                    </ajax:TextBoxWatermarkExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidatorBcc" runat="server"
                        Display="none" ControlToValidate="txtBcc" ErrorMessage="Invalid Email(s) Format"
                        ValidationGroup="SendEmailGroup" ValidationExpression="(([ ]*)(\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)([ ]*)([;,]?)([ ]*))+">
                    </asp:RegularExpressionValidator>
                    <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtenderBcc" runat="server" TargetControlID="RegularExpressionValidatorBcc">
                    </ajax:ValidatorCalloutExtender>
                </div>
                <div>
                    <label>
                        Subject:</label>
                    <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
                </div>
                <div>
                    <label>
                    </label>
                    <cc1:Editor ID="editorBody" runat="server" Height="300px" Style="width: 860px; display: inline-block" />
                </div>
                <div>
                    <label style="vertical-align:top; padding-top: 5px;">
                        Attach Files:</label>
                    <div style="display:inline-block">
                        <asp:FileUpload ID="fuAttachments" runat="server" SkinID="Multiple" />
                    </div>
                </div>
            </fieldset>
        </div>
        <div style="margin: 20px 20px 0px 0px;">
            <asp:Panel ID="pnlNavigation" runat="server">
                <div style="float: left; margin-right: 20px; padding-left: 15px;">
                    <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                </div>
                <div style="float: right;">
                    <span style="margin: 0px;" class="button">
                        <asp:Button ID="btnSend" runat="server" Text="Send" SkinID="Button" ValidationGroup="SendEmailGroup"
                            OnClick="btnSend_Click" /></span>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
