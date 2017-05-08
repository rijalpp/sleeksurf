<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true" CodeBehind="NewEditAdvertisement.aspx.cs" Inherits="SleekSurf.Web.Admin.SuperAdmin.NewEditAdvertisement" %>
<%@ Register Src="~/WebPageControls/NewEditAdvertisement.ascx" TagName="NewEditAd" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: 100%">
                    <asp:Label ID="lblTitle" runat="server" Text="Advertisement Management Portal" EnableViewState="false"></asp:Label></div>
            </div>
            <div id="Advertisement" style="margin: 0px;">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                 <uc1:NewEditAd ID="ucNewEditAd" runat="server" />
            </div>
            <div id="SignUpNavigation">
                <div style="float: left; margin-right: 20px; padding-left: 15px;">
                    <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                </div>
                <div style="float: right;">
                    <span style="margin: 0px 15px;">
                        <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="valgAdvertisement"
                            OnClick="btnSave_Click" SkinID="Button" /></span>
                    <span style="margin: 0px;">
                           <asp:Button ID="btnCancel" runat="server" Text="Cancel"
                                OnClick="btnCancel_Click" SkinID="Button" CausesValidation="false" />
                    </span>
                </div>
            </div>
        </div>
        <div id="RightContentMasterPage">
        </div>
    </div>
</asp:Content>

