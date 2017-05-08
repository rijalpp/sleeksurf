<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true" CodeBehind="NewEditCategory.aspx.cs" Inherits="SleekSurf.Web.Admin.SuperAdmin.NewEditCategory" %>
<%@ Register Src="~/WebPageControls/FileUploader.ascx" TagName="FileUpload" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ShowUpload() {
            if ($get('<%= this.ucFileUpload.ClientID %>_fileUpload').value.length > 0) {
                $get('upUploadAttachment').style.display = 'block';
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title">
                    Category Management Portal</div>
            </div>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            <div id="CategoryDetails" style="margin: 0px; width: 800px;">
                <fieldset>
                    <legend>Category Details</legend>
                    <div class="div">
                        <label>
                            <span class="mandatory">*</span> Category Name:</label>
                        <asp:TextBox ID="txtCategoryName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqCategoryName" runat="server" ErrorMessage="*"
                            ControlToValidate="txtCategoryName" Display="Dynamic" ValidationGroup="CategoryDetails"></asp:RequiredFieldValidator>
                    </div>
                    <div class="div">
                        <span style="float: left">
                            <label>
                                <span class="mandatory">*</span> Category Image:</label>
                            <uc1:FileUpload ID="ucFileUpload" runat="server"/>
                        </span><span style="float: left">
                            <asp:UpdateProgress ID="upUploadAttachment" runat="server" DisplayAfter="4000" ClientIDMode="Static">
                                <ProgressTemplate>
                                    <img src="../../App_Themes/SleekTheme/Images/Uploading.gif" alt="Uploading.." />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </span>
                    </div>
                    <div class="div">
                        <label style="vertical-align: top">
                            Description:
                        </label>
                        <asp:TextBox ID="txtCategoryDescription" runat="server" TextMode="MultiLine" Rows="8"
                            Width="535px"></asp:TextBox>
                    </div>
                </fieldset>
                <div id="SignUpNavigation" style="width: 800px;">
                    <div style="float: left; margin-right: 20px; padding-left: 15px;">
                        <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                    </div>
                    <div style="float: right;">
                        <span style="margin: 0 15px">
                            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" ValidationGroup="CategoryDetails"
                                OnClientClick="ShowUpload()" SkinID="Button" /></span>
                        <span style="margin:0;">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"  OnClick="btnCancel_Click" SkinID="Button" />
                        </span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="RightContentMasterPage">
    </div>
</asp:Content>
