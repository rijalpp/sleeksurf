<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewPackageOrder.ascx.cs"
    Inherits="SleekSurf.Web.WebPageControls.ViewPackageOrder" %>
<%@ Register Src="~/WebPageControls/FileUploader.ascx" TagName="FileUpload" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<div id="PackageOrderDetails" style="margin: 0px; width: 800px;">
    <fieldset>
        <legend>Package-Transaction Details</legend>
        <div id="divClientDetails" runat="server">
            <div class="div">
                <p class="sectionSummaryHeader">
                    <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="CLIENT DETAILS" ForeColor="#5C9FFF"></asp:Label>
                </p>
            </div>
            <div class="div">
                <label>
                    Client Name:</label>
                <asp:Label ID="lblClientName" runat="server" CssClass="displayLabel"/>
            </div>
            <div class="div">
                <span style="display: inline-block;">
                    <label>
                        Contact Number:</label>
                    <asp:Label ID="lblContactNo" runat="server" SkinID="DisplayLabelForDetails" />
                </span><span style="margin-left: 10px;">
                    <label>
                        Client Email:</label>
                    <asp:HyperLink ID="hlClientEmail" runat="server"></asp:HyperLink>
                </span>
            </div>
            <div class="div">
                <span style="display: inline-block;">
                    <label>
                        Contact Person:</label>
                    <asp:Label ID="lblContactPerson" runat="server" SkinID="DisplayLabelForDetails" />
                </span><span style="margin-left: 10px;">
                    <label>
                        Contact Email:</label>
                    <asp:HyperLink ID="hlContactPersonEmail" runat="server"></asp:HyperLink>
                </span>
            </div>
            <div class="div">
                <span style="display: inline-block;">
                    <label>
                        Address:</label>
                    <asp:Label ID="lblAddress" runat="server" CssClass="displayLabel" />
                </span>
            </div>
        </div>
        <div class="div">
            <p class="sectionSummaryHeader">
                <asp:Label ID="Label17" runat="server" Font-Bold="true" Text="PACKAGE DETAILS" ForeColor="#5C9FFF"></asp:Label>
            </p>
        </div>
        <div class="div">
            <label>
                Package Name:</label>
            <asp:Label ID="lblPackageName" runat="server" SkinID="DisplayLabelForDetails" />
        </div>
        <div class="div">
            <span style="display: inline-block;">
                <label>
                    Package Option:</label>
                <asp:Label ID="lblPackageDuration" runat="server" SkinID="DisplayLabelForDetails">
                    <asp:Literal ID="ltrPackageDuration" runat="server"></asp:Literal>
                    </asp:Label>
            </span><span style="margin-left: 10px;">
                <label>
                    Standard Price:</label>
                <asp:Label ID="lblStandardPrice" runat="server" SkinID="DisplayLabelForDetails" />
            </span>
        </div>
        <div class="div">
            <label>
                Expiry Date:</label>
            <asp:Label ID="lblExpiryDate" runat="server" SkinID="DisplayLabelForDetails" />
        </div>
        <div class="div separator">
        </div>
        <div class="div">
            <label>
                Promo Code:</label>
            <asp:Label ID="lblPromoCode" runat="server" SkinID="DisplayLabelForDetails" />
        </div>
        <div class="div">
            <span style="display: inline-block;">
                <label>
                    Promocode Period:</label>
                <asp:Label ID="lblPromocodePeriod" runat="server" SkinID="DisplayLabelForDetails">
                    <asp:Literal ID="lblPromoCodeStartDate" runat="server" />
                    -
                    <asp:Literal ID="lblPromoCodeEndDate" runat="server" />
                </asp:Label>
            </span><span style="margin-left: 10px;">
                <label>
                    Discounted Amount:</label>
                <asp:Label ID="lblDiscountedAmount" runat="server" SkinID="DisplayLabelForDetails"
                    Text="0.00" />
            </span>
        </div>
        <div class="div">
            <span style="display: inline-block;">
                <label>
                    Discounted (%):</label>
                <asp:Label ID="lblDiscountPercentage" runat="server" SkinID="DisplayLabelForDetails" />
            </span><span style="margin-left: 10px;">
                <label>
                    Final Price:</label>
                <asp:Label ID="lblFinalPrice" runat="server" SkinID="DisplayLabelForDetails" />
            </span>
        </div>
        <div class="div">
            <p class="sectionSummaryHeader">
                <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="TRANSACTION DETAILS"
                    ForeColor="#5C9FFF"></asp:Label>
            </p>
        </div>
         <div class="div">
            <label>
                Invoice ID:</label>
            <asp:Label ID="lblInvoiceID" runat="server" SkinID="DisplayLabelForDetails" />
        </div>
        <div class="div">
            <label>
                Transaction ID:</label>
            <asp:Label ID="lblTransactionID" runat="server" SkinID="DisplayLabelForDetails" />
        </div>
        <div class="div">
            <span style="display: inline-block;">
                <label>
                    Transaction Method:</label>
                <asp:Label ID="lblPurchasedMethod" runat="server" SkinID="DisplayLabelForDetails" />
            </span><span style="margin-left: 10px;">
                <label>
                    Deducted Amount:</label>
                <asp:Label ID="lblAmountDeducted" runat="server" SkinID="DisplayLabelForDetails" />
            </span>
        </div>
        <div class="div">
            <span style="display: inline-block;">
                <label>
                    Transaction Status:</label>
                <asp:Label ID="lblPurchasedStatus" runat="server" SkinID="DisplayLabelForDetails" />
            </span><span style="margin-left: 10px;">
                <label>
                    Paid Amount:</label>
                <asp:Label ID="lblAmountPaid" runat="server" SkinID="DisplayLabelForDetails" />
            </span>
        </div>
        <div class="div">
            <span style="display: inline-block;">
                <label>
                    Transaction Type:</label>
                <asp:Label ID="lblPurchasedType" runat="server" SkinID="DisplayLabelForDetails" />
            </span><span style="margin-left: 10px;" id="spanRefundedCase" runat="server" visible="false"><label>
                    You paid:</label>
                <asp:Label ID="lblActualAmountPaid" runat="server" SkinID="DisplayLabelForDetails" /></span>
        </div>
        <div class="div">
            <span style="display: inline-block;">
                <label>
                    Transaction By:</label>
                <asp:Label ID="lblPurchasedBy" runat="server" SkinID="DisplayLabelForDetails" />
            </span><span style="margin-left: 10px;">
                <label>
                    Transaction Date:</label>
                <asp:Label ID="lblPurchasedDate" runat="server" SkinID="DisplayLabelForDetails" />
            </span>
        </div>
    </fieldset>
</div>
