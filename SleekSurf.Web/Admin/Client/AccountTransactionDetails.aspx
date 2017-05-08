<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="AccountTransactionDetails.aspx.cs" Inherits="SleekSurf.Web.Admin.Client.AccountTransactionDetails" %>

<%@ Register Src="~/WebPageControls/ViewPackageOrder.ascx" TagName="PackageOrder"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function confirm_delete() {
            var dropDownList = document.getElementById('<%= ddlPaymentOption.ClientID %>');
            var selectedValue = dropDownList.value;
            if (selectedValue == 'Select Below') {
                alert('You have to select a refund method.');
                return false;
            }
            if (confirm('Are you sure you want to refund this Transaction?') == true)
                return true;
            else
                return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: 100%;">
                    <asp:Label ID="lblTitle" runat="server" Text="Order Management Portal" EnableViewState="false"></asp:Label></div>
            </div>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            <uc1:PackageOrder ID="ucViewPackageOrder" runat="server" />
            <div style="float: right; margin-top: 10px;" id="divRefund" runat="server" visible="false">
                <span style="margin: 0px 15px;">
                    <asp:DropDownList ID="ddlPaymentOption" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqPaymentOption" runat="server" ControlToValidate="ddlPaymentOption"
                        InitialValue="Select Below" ErrorMessage="*">
                    </asp:RequiredFieldValidator>
                </span><span style="margin: 0px;">
                    <asp:Button ID="btnRefund" runat="server" Text="Refund" SkinID="Button" OnClick="btnRefund_Click" />
                </span>
            </div>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks">
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
