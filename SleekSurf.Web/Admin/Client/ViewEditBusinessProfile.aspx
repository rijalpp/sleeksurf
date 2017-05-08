<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true" CodeBehind="ViewEditBusinessProfile.aspx.cs" Inherits="SleekSurf.Web.Admin.Client.ViewEditBusinessProfile" %>
<%@ Register Src="~/WebPageControls/NewEditClient.ascx" TagName="NewEditClient" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage" style="width:740px">
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            <div style="float: left;">
                <uc:NewEditClient runat="server" ID="ucNewEditClient" />
            </div>
            <div class="commandControl">
                <div style="float: left; margin-right: 20px; padding-left: 15px;">
                    <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                </div>      
                <div style="float: right;">
                    <span style="margin: 0px 15px;">
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"
                            SkinID="Button" ValidationGroup="valgRegistration" /></span>
                     <span style="margin: 0px;">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                OnClick="btnCancel_Click" SkinID="Button" /></span>
                </div>
            </div>
        </div>
        <div id="RightContentMasterPage" style="width: 224px">
            <div class="rightLinks" style="margin-top:20px">
                <asp:HyperLink ID="hlNewEditDataInfo" runat="server" Text="Add/Edit T &amp; C and/or P P" ToolTip="Add/Edit Terms &amp; Conditions and/or Privacy Policy" NavigateUrl="~/Admin/TermsAndConditionsPrivacyPolicy.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
