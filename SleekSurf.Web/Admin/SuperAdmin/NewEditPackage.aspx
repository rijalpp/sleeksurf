<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="NewEditPackage.aspx.cs" Inherits="SleekSurf.Web.Admin.SuperAdmin.NewEditPackage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: 100%">
                    <asp:Label ID="lblTitle" runat="server" Text="Package Management Portal" EnableViewState="false"></asp:Label></div>
            </div>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <div id="PackageDetails" style="margin: 0px; width: 800px;">
                <fieldset>
                    <legend>Package Details</legend>
                    <div class="div">
                        <label>
                            <span class="mandatory">*</span> Package Name:</label>
                        <asp:TextBox ID="txtPackageName" runat="server" Width="610px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPackageName"
                            ErrorMessage="*" Display="Dynamic" ValidationGroup="valgPackage"></asp:RequiredFieldValidator>
                    </div>
                    <div class="div" style="display: table">
                        <div class="cell">
                            <label>
                                <span class="mandatory">*</span> Description:
                            </label>
                        </div>
                        <div class="cell" style="padding-right: 5px;">
                            <cc1:Editor ID="editorPackageDescription" runat="server" Width="630px" />
                        </div>
                        <div class="cell">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="editorPackageDescription"
                                ErrorMessage="*" Display="Dynamic" ValidationGroup="valgPackage"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="div">
                        <span style="width: 340px; display: inline-block;">
                          <label><span class="mandatory">*</span> Feature Type:</label>
                          <asp:DropDownList ID="ddlFeatureType" runat="server"></asp:DropDownList>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                    ControlToValidate="ddlFeatureType" Display="Dynamic" InitialValue="Select Below" ValidationGroup="valgPackage">
                </asp:RequiredFieldValidator>
                        </span><span style="margin-left: 10px">
                            <label>
                                Published:
                            </label>
                            <asp:CheckBox ID="chkPublished" runat="server" /></span>
                    </div>
                </fieldset>
            </div>
            <div id="SignUpNavigation" style="width: 800px;">
                <div style="float: left; margin-right: 20px; padding-left: 15px;">
                    <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                </div>
                <div style="float: right;">
                    <span style="margin: 0 15px">
                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" ValidationGroup="valgPackage"
                            SkinID="Button" /></span> <span style="margin: 0;">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                    OnClick="btnCancel_Click" SkinID="Button" />
                            </span>
                </div>
            </div>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks" runat="server" id="dvRightLinks">
                <asp:HyperLink ID="hlAddNewMedia" runat="server" Text="Add New Option" NavigateUrl="~/Admin/SuperAdmin/NewEditPackageOption.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="hlDisplayMedia" runat="server" Text="View All Options" NavigateUrl="~/Admin/SuperAdmin/PackageOptionManagement.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="HyperLink1" runat="server" Text="Add New Feature Icon" NavigateUrl="~/Admin/SuperAdmin/NewEditPackageIcon.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="HyperLink2" runat="server" Text="View All Icons" NavigateUrl="~/Admin/SuperAdmin/PackageIconManagement.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
