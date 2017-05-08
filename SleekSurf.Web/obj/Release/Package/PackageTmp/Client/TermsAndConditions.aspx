<%@ Page Title="" Language="C#" MasterPageFile="~/Client/ClientPageSite.master" AutoEventWireup="true"
    CodeBehind="TermsAndConditions.aspx.cs" Inherits="SleekSurf.Web.Client.TermsAndConditions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPage" runat="server">
    <div id="TermsAndConditions">
        <h2 class="header" style="color:#777777;">Terms And Conditions</h2>
        <asp:Literal ID="ltrTermsAndConditions" runat="server"></asp:Literal>
    </div>
</asp:Content>
