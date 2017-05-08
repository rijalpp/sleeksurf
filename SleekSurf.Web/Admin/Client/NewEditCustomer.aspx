<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="NewEditCustomer.aspx.cs" Inherits="SleekSurf.Web.Admin.Client.NewEditCustomer" %>

<%@ Register Src="~/WebPageControls/NewEditCustomer.ascx" TagName="NewEditCutsomer"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: 100%">
                    <asp:Label ID="lblTitle" runat="server" Text="Customer Management Portal" EnableViewState="false"></asp:Label></div>
            </div>
            <div id="SignUpContentDetails" style="margin: 0px;">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                <uc1:NewEditCutsomer ID="ucNewEditCustomer" runat="server"></uc1:NewEditCutsomer>
            </div>
            <div id="SignUpNavigation">
                <div style="float: left; margin-right: 20px; padding-left: 15px;">
                    <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                </div>
                <div style="float: right;">
                    <span style="margin: 0px 15px;">
                        <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="valgRegistration"
                            OnClick="btnSave_Click" SkinID="Button" /></span>
                    <span style="margin: 0px;">
                           <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                OnClick="btnCancel_Click" SkinID="Button" />
                    </span>
                </div>
            </div>
        </div>
        <div id="RightContentMasterPage">
        </div>
    </div>
</asp:Content>
