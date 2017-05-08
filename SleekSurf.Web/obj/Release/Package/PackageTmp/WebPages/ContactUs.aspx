<%@ Page Title="" Language="C#" MasterPageFile="~/SleekSurf.Master" AutoEventWireup="true"
    CodeBehind="ContactUs.aspx.cs" Inherits="SleekSurf.Web.WebPages.ContactUs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapContent">
        <div style="display: inline-block;">
            <div class="pageTitle">
                <p>
                    Contact Us</p>
            </div>
            <div id="ContactUsForm">
                <p class="paragraph">
                    If you like to know how we can be of your assistance to publicise your business/organisation
                    or any other queries, please contact us with your details.<br />
                    <br />
                    <span style="font-weight: bold">We value your all your enqueries, comments and feedbacks!</span><br />
                    <br />
                    Please note that our staff responds to e-mail inquiries from <span style="font-weight: bold">
                        8:00 a.m. - 5:00 p.m. Monday-Friday AST (excluding holidays)</span>. Please
                    allow 1 to 2 business days for a response during normal business hours.
                    <br />
                    <br />
                    If you require immediate assistance, please contact us at: +(61) 416099377
                </p>
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                <fieldset>
                    <legend>Your Details</legend>
                    <div>
                        <label>
                            Business Name:</label>
                        <asp:TextBox ID="txtBusinessName" runat="server" SkinID="ContactUs"></asp:TextBox>
                    </div>
                    <div>
                        <label>
                            <span class="mandatory">*</span> Name:</label>
                        <asp:TextBox ID="txtName" runat="server" SkinID="ContactUs"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                            ControlToValidate="txtName" Display="Dynamic" ValidationGroup="ContactFormValidation"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="Dynamic"
                            ControlToValidate="txtName" ErrorMessage="Alphabets only" ValidationGroup="ContactFormValidation"
                            ValidationExpression="^\s*([A-Za-z]{2,4}\.?\s*)?(['\-A-Za-z]+\s*){1,2}([A-Za-z]+\.?\s*)?(['\-A-Za-z]+\s*){1,2}(([jJsSrR]{2}\.)|([XIV]{1,6}))?\s*$">
                        </asp:RegularExpressionValidator>
                        <asp:CompareValidator ID="CompareValidator1" ErrorMessage="Only Text" ControlToValidate="txtName"
                            Type="String" Operator="DataTypeCheck" runat="server" ValidationGroup="ContactFormValidation"
                            Display="Dynamic" />
                    </div>
                    <div>
                        <label>
                            <span class="mandatory">*</span> Contact:</label>
                        <asp:TextBox ID="txtContact" runat="server" SkinID="ContactUs"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                            ControlToValidate="txtContact" Display="Dynamic" ValidationGroup="ContactFormValidation"></asp:RequiredFieldValidator>
                    </div>
                    <div>
                        <label>
                            <span class="mandatory">*</span> Email:</label>
                        <asp:TextBox ID="txtEmail" runat="server" SkinID="ContactUs"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                            ControlToValidate="txtEmail" Display="Dynamic" ValidationGroup="ContactFormValidation"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic"
                            ControlToValidate="txtEmail" ErrorMessage="Invalid Format" ValidationGroup="ContactFormValidation"
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                        </asp:RegularExpressionValidator>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWaterMarkName" runat="server" WatermarkText="userid@domain.com"
                            WatermarkCssClass="waterMark" TargetControlID="txtEmail">
                        </cc1:TextBoxWatermarkExtender>
                    </div>
                    <div>
                        <label>
                            Website:</label>
                        <asp:TextBox ID="txtWebsite" runat="server" SkinID="ContactUs"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorWebsite" runat="server"
                            ValidationGroup="ContactFormValidation" ErrorMessage="Invalid Format" ControlToValidate="txtWebsite"
                            Display="Dynamic" ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$">
                        </asp:RegularExpressionValidator>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkText="http://www.domain.com"
                            WatermarkCssClass="waterMark" TargetControlID="txtWebsite">
                        </cc1:TextBoxWatermarkExtender>
                    </div>
                    <div>
                        <label>
                            Address:</label>
                        <asp:TextBox ID="txtAddress" runat="server" SkinID="ContactUs"></asp:TextBox>
                    </div>
                    <div>
                        <label>
                            <span class="mandatory">*</span> Subject:</label>
                        <asp:TextBox ID="txtSubject" runat="server" SkinID="ContactUs"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
                            ControlToValidate="txtSubject" Display="Dynamic" ValidationGroup="ContactFormValidation"></asp:RequiredFieldValidator>
                    </div>
                    <div>
                        <label style="vertical-align: top;">
                            <span class="mandatory">*</span> Message:</label>
                        <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="8" SkinID="ContactUs"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*"
                            ControlToValidate="txtMessage" Display="Dynamic" ValidationGroup="ContactFormValidation"
                            Style="vertical-align: top;"></asp:RequiredFieldValidator>
                    </div>
                </fieldset>
            </div>
            <div id="SignUpNavigation" style="width: 700px;">
                <div style="float: left; margin-right: 20px; padding-left: 15px;">
                    <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                </div>
                <div style="float: right;">
                    <span style="margin: 0 15px">
                        <asp:Button ID="btnSave" runat="server" Text="Submit" ValidationGroup="ContactFormValidation"
                            SkinID="Button" OnClick="btnSave_Click" /></span>
                </div>
            </div>
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
