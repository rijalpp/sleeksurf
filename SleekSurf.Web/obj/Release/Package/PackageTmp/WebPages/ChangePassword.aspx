<%@ Page Title="" Language="C#" MasterPageFile="~/SleekSurf.Master" AutoEventWireup="true"
    CodeBehind="ChangePassword.aspx.cs" Inherits="SleekSurf.Web.WebPages.ChangePassword" %>

<%@ Register Src="~/WebPageControls/ChangePassword.ascx" TagName="ucPassword" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="wrapContent">
        <div style="display: inline-block;">
            <div class="pageTitle">
                <p>
                  Change Password
                 </p>
            </div>
            <p class="paragraph">
                Please enter your current password, new password and confirm new password.
            </p>
            <div id="ContactUsForm">
                <uc1:ucPassword ID="ucChangePassword" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
