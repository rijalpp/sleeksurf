<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="NewEditPackageOption.aspx.cs" Inherits="SleekSurf.Web.Admin.SuperAdmin.NewEditPackageOption" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: 100%">
                    <asp:Label ID="lblTitle" runat="server" Text="Package Option Management Portal - "
                        EnableViewState="false"></asp:Label></div>
            </div>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <div id="PackageOptionDetails" style="margin: 0px; width: 800px;">
                <fieldset>
                    <legend>Package Option Details</legend>
                    <div class="div">
                        <label>
                            <span class="mandatory">*</span> Duration:</label>
                        <asp:TextBox ID="txtDuration" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDuration"
                            ErrorMessage="*" Display="Dynamic" ValidationGroup="valgPackageOption"></asp:RequiredFieldValidator>
                        <asp:FilteredTextBoxExtender ID="ftxtEDuration" runat="server" TargetControlID="txtDuration"
                            FilterMode="ValidChars" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                        <div>
                            <label>
                            </label>
                            <label class="extraSmallLabel" style="padding-left: 0px;">
                                (Numbers only in Months)</label>
                        </div>
                    </div>
                    <div class="div">
                        <label>
                            <span class="mandatory">*</span> Price:</label>
                        <asp:TextBox ID="txtPrice" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPrice"
                            ErrorMessage="*" Display="Dynamic" ValidationGroup="valgPackageOption"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="regxStandardPrice" runat="server" ControlToValidate="txtPrice"
                            ErrorMessage="*" ValidationExpression="[0-9]+(\.[0-9][0-9]?)?" Display="Dynamic"
                            ValidationGroup="valgPackageOption">
                        </asp:RegularExpressionValidator>
                    </div>
                    <div class="div">
                        <label>
                            Discount %:
                        </label>
                        <asp:TextBox ID="txtDiscountPercentage" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDiscountPercentage"
                            ErrorMessage="*" ValidationExpression="[0-9]+(\.[0-9][0-9]?)?" Display="Dynamic"
                            ValidationGroup="valgPackageOption">
                        </asp:RegularExpressionValidator>
                    </div>
                    <div class="div">
                        <label>
                            Promo Code:</label>
                        <asp:TextBox ID="txtPromoCode" runat="server"></asp:TextBox>
                    </div>
                    <div class="div">
                        <span style="display: inline-block;">
                            <label>
                                PC StartDate:</label>
                            <asp:TextBox ID="txtPromoStartDate" runat="server"></asp:TextBox>
                            <asp:Image ID="imgStartDate" ImageUrl="~/App_Themes/SleekTheme/Images/Calendar.png"
                                runat="server" Height="28px" />
                        </span>
                        <asp:TextBoxWatermarkExtender ID="TextBoxWaterMarkDOB" runat="server" WatermarkText=" dd/MM/yyyy "
                            TargetControlID="txtPromoStartDate" WatermarkCssClass="waterMark">
                        </asp:TextBoxWatermarkExtender>
                        <asp:CalendarExtender ID="CalendarExtenderDOB" runat="server" Format="dd/MM/yyyy"
                            CssClass="calandarTheme" TargetControlID="txtPromoStartDate" PopupButtonID="imgStartDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender AutoComplete="true" ID="MaskedEditExtenderDOB" runat="server"
                            TargetControlID="txtPromoStartDate" MaskType="Date" Mask="99/99/9999" CultureName="en-AU">
                        </asp:MaskedEditExtender>
                        <asp:MaskedEditValidator ID="MaskedEditValidatorDOB" runat="server" ControlExtender="MaskedEditExtenderDOB"
                            ControlToValidate="txtPromoStartDate" IsValidEmpty="false" ErrorMessage="Invalid Date Format"
                            InvalidValueMessage="Invalid Date Format (dd/MM/yyyy)" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$"
                            Display="Dynamic" ValidationGroup="PromotionValidation" ForeColor="Red">
                        </asp:MaskedEditValidator>
                    </div>
                    <div class="div">
                        <span style="display: inline-block;">
                            <label>
                                PC EndDate:</label>
                            <asp:TextBox ID="txtPromoEndDate" runat="server"></asp:TextBox>
                            <asp:Image ID="imgEndDate" ImageUrl="~/App_Themes/SleekTheme/Images/Calendar.png" runat="server"
                                Height="28px" />
                        </span>
                        <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkText=" dd/MM/yyyy "
                            TargetControlID="txtPromoEndDate" WatermarkCssClass="waterMark">
                        </asp:TextBoxWatermarkExtender>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" CssClass="calandarTheme"
                            TargetControlID="txtPromoEndDate" PopupButtonID="imgEndDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender AutoComplete="true" ID="MaskedEditExtender1" runat="server"
                            TargetControlID="txtPromoEndDate" MaskType="Date" Mask="99/99/9999" CultureName="en-AU">
                        </asp:MaskedEditExtender>
                        <asp:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtenderDOB"
                            ControlToValidate="txtPromoEndDate" IsValidEmpty="false" ErrorMessage="Invalid Date Format"
                            InvalidValueMessage="Invalid Date Format (dd/MM/yyyy)" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$"
                            Display="Dynamic" ValidationGroup="PromotionValidation" ForeColor="Red">
                        </asp:MaskedEditValidator>
                    </div>
                    <div class="div separator"></div>
                    <div class="div" style="display: table">
                        <div class="cell">
                            <label>
                                <span class="mandatory">*</span> Description:</label>
                        </div>
                        <div class="cell" style="padding-right: 5px;">
                            <cc1:Editor ID="editorDescription" runat="server" Width="640px" />
                        </div>
                    </div>
                    <div class="div">
                        <label>
                            <span class="mandatory">*</span> Published:</label>
                        <asp:CheckBox ID="chkPublished" runat="server" />
                    </div>
                </fieldset>
            </div>
            <div id="SignUpNavigation" style="width: 800px;">
                <div style="float: left; margin-right: 20px; padding-left: 15px;">
                    <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                </div>
                <div style="float: right;">
                    <span style="margin: 0 15px">
                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" ValidationGroup="valgPackageOption"
                            SkinID="Button" /></span> <span style="margin: 0;">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                    SkinID="Button" CausesValidation="false" />
                            </span>
                </div>
            </div>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks">
                <asp:HyperLink ID="hlDisplayMedia" runat="server" Text="View All Options" NavigateUrl="~/Admin/SuperAdmin/PackageOptionManagement.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
