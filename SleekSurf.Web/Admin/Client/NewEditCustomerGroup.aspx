<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="NewEditCustomerGroup.aspx.cs" Inherits="SleekSurf.Web.Admin.Client.NewEditCustomerGroup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: 100%">
                    <asp:Label ID="lblTitle" runat="server" Text="Customer Group Management Portal" EnableViewState="false"></asp:Label></div>
            </div>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            <div id="SignUpContentDetails" style="margin: 0px;">
                <fieldset>
                    <legend>Customer Group Details</legend>
                    <div>
                        <label>
                            <span class="mandatory">*</span> Group Name:</label>
                    <asp:UpdatePanel ID ="upnlGroupName" runat="server" RenderMode="Inline">
                       <ContentTemplate>
                       
                        <asp:TextBox ID="txtGroupName" runat="server" OnTextChanged="txtGroupName_TextChanged"
                            AutoPostBack="true"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqAddressLine1" runat="server" ErrorMessage="*"
                            ControlToValidate="txtGroupName" Display="Dynamic" ValidationGroup="valgRegistration">
                        </asp:RequiredFieldValidator>
                        <asp:Label ID="lblGroupNameExists" runat="server" SkinID="Error" EnableViewState="false" />
                     </ContentTemplate>
                    </asp:UpdatePanel>
                    </div>
                    <div>
                        <label>
                            Comment:</label>
                        <asp:TextBox ID="txtComment" runat="server" Width="535px"></asp:TextBox>
                    </div>
                    <div>
                        <label style="vertical-align: top">
                            Description:</label>
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="8" Width="535px"></asp:TextBox>
                    </div>
                    <div>
                        <label>
                            <span class="mandatory">*</span> Published:</label>
                        <asp:CheckBox ID="chkPublished" runat="server" />
                    </div>
                </fieldset>
            </div>
            <div id="SignUpNavigation">
                <div style="float: left; margin-right: 20px; padding-left: 15px;">
                    <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                </div>
                <div style="float: right;">
                    <span style="margin: 0px 15px;">
                        <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="valgRegistration"
                            OnClick="btnSave_Click" SkinID="Button" /></span> <span style="margin: 0px;">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                    OnClick="btnCancel_Click" SkinID="Button" />
                            </span>
                </div>
            </div>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks">
                <asp:HyperLink ID="hlGroupManagement" runat="server" Text="Group Management" NavigateUrl="~/Admin/Client/CustomerGroupManagement.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
