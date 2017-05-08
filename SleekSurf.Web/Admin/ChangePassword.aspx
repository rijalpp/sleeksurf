<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="ChangePassword.aspx.cs" Inherits="SleekSurf.Web.Admin.ChangePassword" %>

<%@ Register Src="~/WebPageControls/ChangePassword.ascx" TagName="ucPassword" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <uc1:ucPassword ID="ucChangePassword" runat="server" />
    </div>
</asp:Content>
