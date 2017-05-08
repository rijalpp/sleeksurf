<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="NewEditUser.aspx.cs" Inherits="SleekSurf.Web.Admin.Client.NewEditUser" %>

<%@ Register Src="~/WebPageControls/NewEditAccount.ascx" TagName="NewEditAccount"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title">
                    User Management Portal</div>
            </div>
            <div>
                <uc:NewEditAccount ID="ucNewEditAccount" runat="server" />
            </div>
        </div>
        <div id="RightContentMasterPage">
        </div>
    </div>
</asp:Content>
