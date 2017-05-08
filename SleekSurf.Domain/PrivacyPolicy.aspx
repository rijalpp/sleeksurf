<%@ Page Title="" Language="C#" MasterPageFile="~/ClientPageSite.master" AutoEventWireup="true" CodeBehind="PrivacyPolicy.aspx.cs" Inherits="SleekSurf.Domain.PrivacyPolicy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPage" runat="server">
    <div id="PrivacyPolicy">
        <h2 class="header" style="color: #777777;">
            Privacy Policy</h2>
        <asp:Literal ID="ltrPrivacyPolicy" runat="server"></asp:Literal>
    </div>
</asp:Content>