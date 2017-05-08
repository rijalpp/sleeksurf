<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true" CodeBehind="ViewPackageOrderDetails.aspx.cs" Inherits="SleekSurf.Web.Admin.SuperAdmin.ViewPackageOrderDetails" %>
<%@ Register Src="~/WebPageControls/ViewPackageOrder.ascx" TagName="PackageOrder" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: 100%">
                    <asp:Label ID="lblTitle" runat="server" Text="Order Management Portal" EnableViewState="false"></asp:Label></div>
            </div>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            <uc1:PackageOrder ID="ucViewPackageOrder" runat="server" />
           
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks" runat="server" id="dvRightLinks">
            </div>
        </div>
    </div>
</asp:Content>
