<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true" CodeBehind="TermsAndConditionsPrivacyPolicy.aspx.cs" Inherits="SleekSurf.Web.Admin.TermsAndConditionsPrivacyPolicy" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div style="padding-left: 80px">
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" /></div>
        <asp:Panel ID="pnlTermsAndCond" runat="server">
            <div>
                <fieldset>
                    <legend>Terms &amp; Conditions</legend>
                    <div style="padding: 10px 40px;">
                        <cc1:Editor ID="editorTermsAndConditions" runat="server" Height="300px" Style="width: 890px;
                            display: inline-block" />
                    </div>
                </fieldset>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlPrivacyAndPolicy" runat="server">
            <div>
                <fieldset>
                    <legend>Privacy Policy</legend>
                    <div style="padding: 10px 40px;">
                        <cc1:Editor ID="editorPrivacyAndPolicy" runat="server" Height="300px" Style="width: 890px;
                            display: inline-block" />
                    </div>
                </fieldset>
            </div>
        </asp:Panel>
        <div style="margin: 20px 20px 0px 0px;">
            <asp:Panel ID="pnlNavigation" runat="server">
                <div style="float: left; margin-right: 20px; padding-left: 15px;">
                    <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                </div>
                <div style="float: right;">
                    <span style="margin: 0px;" class="button">
                        <asp:Button ID="btnSend" runat="server" Text="Save" SkinID="Button" OnClick="btnSend_Click" /></span>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
