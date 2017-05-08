<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true" CodeBehind="NewEditService.aspx.cs" Inherits="SleekSurf.Web.Admin.Client.NewEditService" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width:100%">
                    <asp:Label ID="lblTitle" runat="server" Text="Service Management Portal" EnableViewState="false"></asp:Label></div>
            </div>
            <asp:Label ID="lblMessage" runat="server" ViewStateMode="Disabled"></asp:Label>
            <div id="ServiceDetails" style="margin: 0px; width: 800px;">
                <fieldset>
                    <legend>Service Details</legend>
                    <div class="div">
                        <div class="cell">
                            <label style="display: inline">
                                <span class="mandatory">*</span> Service Description:</label>
                        </div>
                        <div class="cell">
                            <cc1:Editor ID="edServiceDescription" runat="server" Width="600px" Height="300px" />
                        </div>
                        <div class="cell">
                            <asp:RequiredFieldValidator ID="reqServiceDescription" runat="server" ErrorMessage="*"
                                ControlToValidate="edServiceDescription" Display="Dynamic" ValidationGroup="ServiceDetails">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                </fieldset>
                <div id="SignUpNavigation" style="width: 800px;">
                    <div style="float: left; margin-right: 20px; padding-left: 15px;">
                        <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                    </div>
                    <div style="float: right;">
                        <span style="margin: 0 15px">
                            <asp:Button ID="btnSave" runat="server" SkinID="Button" Text="Save" ValidationGroup="ServiceDetails"
                                OnClick="btnSave_Click" /></span>
                         <span style="margin: 0px;">
                              <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                OnClick="btnCancel_Click" SkinID="Button" />
                         </span>
                    </div>
                </div>
            </div>
        </div>
        <div id="RightContentMasterPage">
        </div>
    </div>
</asp:Content>
