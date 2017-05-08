<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="AccountManagement.aspx.cs" Inherits="SleekSurf.Web.Admin.Client.AccountManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: 45%">
                    Account Management Portal</div>
                <div class="search" style="width: 55%; text-align: right">
                    <asp:Panel ID="pnlExpressPurchase" runat="server" DefaultButton="btnTopUp" Visible="false"
                        Width="100%">
                        <asp:DropDownList ID="ddlExpressPackage" runat="server" Width="150px">
                        </asp:DropDownList>
                        <span class="marginSideBySide" style="width: 100px; text-align: left; display: inline-block;">
                            <asp:TextBox ID="txtNoOfDays" runat="server" Width="70px"></asp:TextBox>
                            <asp:TextBoxWatermarkExtender ID="txtNoOfDaysWaterMark" TargetControlID="txtNoOfDays"
                                runat="server" WatermarkText="Days Only" WatermarkCssClass="waterMark">
                            </asp:TextBoxWatermarkExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                ControlToValidate="txtNoOfDays" Display="Dynamic" ValidationGroup="valgEvaluationUse">
                            </asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtNoOfDays"
                                Type="Integer" Operator="DataTypeCheck" ValidationGroup="valgEvaluationUse" Display="Dynamic" />
                            <asp:RangeValidator ID="rangNoOfdays" runat="server" MinimumValue="0" MaximumValue="30"
                                ControlToValidate="txtNoOfDays" Type="Integer" ValidationGroup="valgEvaluationUse"
                                ErrorMessage="*"></asp:RangeValidator>
                        </span>
                        <asp:Button ID="btnTopUp" runat="server" SkinID="SearchButtonSmall" Text="Top Up"
                            OnClick="btnTopUp_Click" ValidationGroup="valgEvaluationUse" />
                    </asp:Panel>
                </div>
            </div>
            <asp:UpdatePanel ID="upnlAccountForSuperAdminView" runat="server" RenderMode="Block">
                <ContentTemplate>
                    <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                    <asp:Panel ID="panelTransactionReceipt" runat="server" ClientIDMode="Static" Visible="false">
                        <div id="PackageListContent">
                            <div id="PackageSelection">
                                <fieldset>
                                    <legend>Confirmation Details</legend>
                                    <div class="accountSummaryDiv">
                                        <p style="margin: 0px;">
                                            Your confirmation of purchase are shown below. Please record and keep it in safe
                                            place.</p>
                                        <p class="sectionSummaryHeader">
                                            <asp:Label ID="Label17" runat="server" Font-Bold="true" Text="PACKAGE DETAILS" ForeColor="#5C9FFF"></asp:Label>
                                        </p>
                                        <span class="sectionSummary" style="margin: 0px;"><span style="display: block; padding: 2px 0px;">
                                            <asp:Label ID="Label27" runat="server" Text="Package Name: " SkinID="Confirmation"></asp:Label><asp:Label
                                                ID="lblPackageName" runat="server" SkinID="DisplayLabel"></asp:Label>
                                        </span><span style="display: block; padding: 2px 0px;">
                                            <asp:Label ID="Label2" runat="server" Text="Package Option: " SkinID="Confirmation"></asp:Label><asp:Label
                                                ID="lblPackageOption" runat="server" SkinID="DisplayLabel"></asp:Label>
                                        </span><span style="display: block; padding: 2px 0px;">
                                            <asp:Label ID="Label4" runat="server" Text="Registration Date: " SkinID="Confirmation"></asp:Label><asp:Label
                                                ID="lblRegistrationDate" runat="server" SkinID="DisplayLabel"></asp:Label>
                                        </span><span style="display: block; padding: 2px 0px;">
                                            <asp:Label ID="Label6" runat="server" Text="Expiry Date: " SkinID="Confirmation"></asp:Label><asp:Label
                                                ID="lblExpiryDate" runat="server" SkinID="DisplayLabel"></asp:Label>
                                        </span></span>
                                        <p class="sectionSummaryHeader">
                                            <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="TRANSACTION DETAILS"
                                                ForeColor="#5C9FFF"></asp:Label>
                                        </p>
                                        <span class="sectionSummary" style="margin: 0px;"><span style="display: block; padding: 2px 0px;">
                                            <asp:Label ID="Label5" runat="server" Text="Invoice ID: " SkinID="Confirmation"></asp:Label><asp:Label
                                                ID="lblInvoiceID" runat="server" SkinID="DisplayLabel"></asp:Label>
                                        </span><span style="display: block; padding: 2px 0px;">
                                            <asp:Label ID="Label3" runat="server" Text="Transaction ID: " SkinID="Confirmation"></asp:Label><asp:Label
                                                ID="lblTransactionID" runat="server" SkinID="DisplayLabel"></asp:Label>
                                        </span><span style="display: block; padding: 2px 0px;">
                                            <asp:Label ID="Label7" runat="server" Text="Amount Paid: " SkinID="Confirmation"></asp:Label><asp:Label
                                                ID="lblAmountPaid" runat="server" SkinID="DisplayLabel"></asp:Label>
                                        </span><span style="display: block; padding: 2px 0px;">
                                            <asp:Label ID="Label9" runat="server" Text="Payment Method: " SkinID="Confirmation"></asp:Label><asp:Label
                                                ID="lblPaymentMethod" runat="server" SkinID="DisplayLabel"></asp:Label>
                                        </span><span style="display: block; padding: 2px 0px;">
                                            <asp:Label ID="Label11" runat="server" Text="Payment Status: " SkinID="Confirmation"></asp:Label><asp:Label
                                                ID="lblPaymentStatus" runat="server" SkinID="DisplayLabel"></asp:Label>
                                        </span></span>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="panelAccountForClientView" ClientIDMode="Static" runat="server" Visible="false">
                        <div class="accountRenew">
                            <p class="para">
                                If you wish to add another package(s) or wish to renew an existing package, this
                                is the right option. Please note, expiry date will be added on for the same package.
                            </p>
                            <div style="margin: 0px 5px; text-align: right;">
                                <asp:Button ID="btnRenewAccount" runat="server" Text="Browse Now" PostBackUrl="~/Admin/Client/AccountRenew.aspx"
                                    SkinID="Button"></asp:Button>
                            </div>
                        </div>
                        <div class="accountSMS">
                            <p class="para">
                                If you wish to notify your registered customers about your promotions via SMS, this
                                is the right option. Please choose a package based on your requirements.
                            </p>
                            <div style="margin: 0px 5px; text-align: right;">
                                <asp:Button ID="btnBuySMSCredit" runat="server" Text="Buy Now" SkinID="Button" PostBackUrl="~/Admin/Client/RechargeSMS.aspx">
                                </asp:Button>
                            </div>
                        </div>
                        <div class="accountTransaction">
                            <p class="para">
                                If you wish to view all your account transaction(s), this is the right option.
                            </p>
                            <div style="margin: 0px 5px; text-align: right;">
                                <asp:Button ID="btnAccountTransactions" runat="server" Text="View now" PostBackUrl="~/Admin/Client/AccountTransactionManagement.aspx"
                                    SkinID="Button"></asp:Button>
                            </div>
                        </div>
                        <asp:Panel ID="pnlMatchDomain" runat="server" CssClass="matchDomain">
                            <p class="para">
                                If you wish to link your domain name to your profile, please match with your account.
                            </p>
                            <div style="margin: 0px 5px; text-align: right;">
                                <asp:Button ID="Button1" runat="server" Text="Match it" PostBackUrl="~/Admin/Client/MatchDomain.aspx"
                                    SkinID="Button"></asp:Button>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlMatchProfile" runat="server" CssClass="matchProfile">
                            <p class="para">
                                If you wish to link your profile to your domain name, please match with your account.
                            </p>
                            <div style="margin: 0px 5px; text-align: right;">
                                <asp:Button ID="Button2" runat="server" Text="Match it" PostBackUrl="~/Admin/Client/MatchProfile.aspx"
                                    SkinID="Button"></asp:Button>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="panelAccountForSuperAdminView" runat="server" ClientIDMode="Static"
                        Visible="false">
                        <fieldset>
                            <legend>Package Details</legend>
                            <div class="div">
                                <label>
                                    <span class="mandatory">*</span> Package:</label>
                                <asp:DropDownList ID="ddlPackage" runat="server" OnSelectedIndexChanged="ddlPackage_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="reqDdlPackage" runat="server" ErrorMessage="*" ControlToValidate="ddlPackage"
                                    Display="Dynamic" InitialValue="Select Below" ValidationGroup="PackageOrderValidation">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="div">
                                <span style="width: 340px; display: inline-block;">
                                    <label>
                                        <span class="mandatory">*</span> Package Option:</label>
                                    <asp:DropDownList ID="ddlPackageOption" runat="server" OnSelectedIndexChanged="ddlPackageOption_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqDdlPackageOption" runat="server" ErrorMessage="*"
                                        ControlToValidate="ddlPackageOption" Display="Dynamic" InitialValue="Select Below"
                                        ValidationGroup="PackageOrderValidation">
                                    </asp:RequiredFieldValidator>
                                </span><span style="margin-left: 10px">
                                    <label>
                                        <span class="mandatory">*</span>Standard Price:</label>
                                    <asp:TextBox ID="txtPrice" runat="server" ReadOnly="true" ToolTip="Enter in Decimal Format i.e. 00.00"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPrice"
                                        ErrorMessage="*" Display="Dynamic" ValidationGroup="PackageOrderValidation">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="regxAmountPaid" runat="server" ErrorMessage="*"
                                        Display="Dynamic" ControlToValidate="txtPrice" ValidationExpression="^[0-9]{1,14}(.[0-9]{2}){0,1}$"
                                        ValidationGroup="PackageOrderValidation">
                                    </asp:RegularExpressionValidator>
                                </span>
                            </div>
                            <div class="separator">
                            </div>
                            <div class="div">
                                <span style="width: 340px; display: inline-block;">
                                    <label>
                                        Promo Code:</label>
                                    <asp:TextBox ID="txtPromoCode" runat="server" OnTextChanged="txtPromoCode_TextChanged"
                                        AutoPostBack="true"></asp:TextBox>
                                </span><span style="margin-left: 10px">
                                    <label>
                                        Deducted Amount:</label>
                                    <asp:TextBox ID="txtDeductedAmount" runat="server" ReadOnly="true"></asp:TextBox>
                                </span>
                            </div>
                            <div class="separator">
                            </div>
                            <div class="div">
                                <span style="width: 340px; display: inline-block;"></span><span style="margin-left: 10px">
                                    <label>
                                        <span class="mandatory">*</span> Final Price:</label>
                                    <asp:TextBox ID="txtFinalPrice" runat="server" ReadOnly="true"></asp:TextBox>
                                </span>
                            </div>
                            <div class="div">
                                <span style="width: 340px; display: inline-block;">
                                    <label>
                                        <span class="mandatory">*</span> Payment Method:</label>
                                    <asp:DropDownList ID="ddlPaymentOption" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqPaymentOption" runat="server" ControlToValidate="ddlPaymentOption"
                                        Display="Dynamic" ErrorMessage="*" ValidationGroup="PackageOrderValidation" InitialValue="Select Below">
                                    </asp:RequiredFieldValidator>
                                </span><span style="margin-left: 10px">
                                    <label>
                                        <span class="mandatory">*</span> Amount Paid:</label>
                                    <asp:TextBox ID="txtAmountPaid" runat="server" ToolTip="Enter in Decimal Format i.e. 00.00"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqtxtAmountPaid" runat="server" ControlToValidate="txtAmountPaid"
                                        ErrorMessage="*" Display="Dynamic" ValidationGroup="PackageOrderValidation">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="* use 0.00 format"
                                        Display="Dynamic" ControlToValidate="txtAmountPaid" ValidationExpression="^[0-9]{1,14}(.[0-9]{2}){0,1}$"
                                        ValidationGroup="PackageOrderValidation">
                                    </asp:RegularExpressionValidator>
                                </span>
                            </div>
                            <div class="separator">
                            </div>
                            <div class="div">
                                <label>
                                    <span class="mandatory">*</span> Order Status:</label>
                                <asp:DropDownList ID="ddlOrderStatus" runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="reqOrderStatus" runat="server" ControlToValidate="ddlOrderStatus"
                                    Display="Dynamic" ErrorMessage="*" ValidationGroup="PackageOrderValidation" InitialValue="Select Below"></asp:RequiredFieldValidator>
                            </div>
                        </fieldset>
                        <div id="SignUpNavigation">
                            <div style="float: left; margin-right: 20px; padding-left: 15px;">
                                <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                            </div>
                            <div style="float: right;">
                                <span style="margin: 0px 15px;">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="PackageOrderValidation"
                                        OnClick="btnSave_Click" SkinID="Button" /></span>
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks" runat="server" id="dvRightLinks">
                <asp:HyperLink ID="hlRenewAccount" runat="server" Text="Renew Account" NavigateUrl="~/Admin/Client/AccountRenew.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="hlBuySMSCredit" runat="server" Text="Buy SMS Credit" NavigateUrl="~/Admin/Client/RechargeSMS.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="hlTransactionHistory" runat="server" Text="Transaction History"
                    NavigateUrl="~/Admin/Client/AccountTransactionManagement.aspx" CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="hlMatchProfile" runat="server" Text="Add Unique Profile" NavigateUrl="~/Admin/Client/MatchProfile.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="hlMatchDomain" runat="server" Text="Add Domain" NavigateUrl="~/Admin/Client/MatchDomain.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
