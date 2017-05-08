<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true" CodeBehind="NewEditClient.aspx.cs" Inherits="SleekSurf.Web.Admin.SuperAdmin.NewEditClient" %>
<%@ Register Src="~/WebPageControls/NewEditAccount.ascx" TagName="NewEditAccount"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title">
                    Client Management Portal</div>
            </div>
            <uc:NewEditAccount ID="ucNewEditAccount" runat="server" />
        </div>
        <div id="RightContentMasterPage">
        </div>
    </div>
</asp:Content>
