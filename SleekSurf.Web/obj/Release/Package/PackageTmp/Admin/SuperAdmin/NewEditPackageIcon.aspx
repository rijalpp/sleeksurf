<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="NewEditPackageIcon.aspx.cs" Inherits="SleekSurf.Web.Admin.SuperAdmin.NewEditPackageIcon" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: 100%">
                    <asp:Label ID="lblTitle" runat="server" Text="Media Management Portal" EnableViewState="false"></asp:Label></div>
            </div>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            <div id="MediaDetails" style="margin: 0px; width: 800px;">
                <fieldset>
                    <legend>Package Picture Details</legend>
                    <div class="div">
                        <label>
                            <span class="mandatory">*</span> Icon:
                        </label>
                        <asp:FileUpload ID="fuUploadMedia" runat="server" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorFuImage" runat="server"
                            ErrorMessage="Only gif, jpg, png, jpeg files are allowed!" ValidationExpression="^(.*\.([gG][iI][fF]|[jJ][pP][gG]|[jJ][pP][eE][gG]|[bB][mM][pP]|[pP][nN][gG])$)"
                            ControlToValidate="fuUploadMedia" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="reqPictureRequred" runat="server" ControlToValidate="fuUploadMedia"
                            ErrorMessage="Image is required!" ValidationGroup="PackagePictureValidation" Display="Dynamic">
                        </asp:RequiredFieldValidator>
                        <span class="absolutePosition">
                            <asp:Image ID="imgThumb" runat="server" Visible="false" Height="100px" Style="padding: 5px;
                                border: 1px solid #DDDDDD;"></asp:Image>
                        </span>
                    </div>
                    <div class="div">
                        <label>
                            <span class="mandatory">*</span> ToolTip:</label>
                        <asp:TextBox ID="txtTooltip" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqTitle" runat="server" ErrorMessage="*" ControlToValidate="txtTooltip"
                            Display="Dynamic" ValidationGroup="PackagePictureValidation">
                        </asp:RequiredFieldValidator>
                    </div>
                     <div class="div">
                        <label>
                            <span class="mandatory">*</span> Display Order:</label>
                        <asp:TextBox ID="txtDisplayOrder" runat="server" Text="0" ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="ReqDisplayOrder" runat="server" ErrorMessage="*" ControlToValidate="txtDisplayOrder"
                            Display="Dynamic" ValidationGroup="PackagePictureValidation">
                        </asp:RequiredFieldValidator>
                        <asp:FilteredTextBoxExtender ID="fTxtEDisplayOrder" runat="server" TargetControlID="txtDisplayOrder"
                        FilterMode="ValidChars" FilterType="Numbers">
                    </asp:FilteredTextBoxExtender>
                    </div>
                </fieldset>
                <div id="SignUpNavigation" style="width: 800px;">
                    <div style="float: left; margin-right: 20px; padding-left: 15px;">
                        <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                    </div>
                    <div style="float: right;">
                        <span style="margin: 0 15px">
                            <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="PackagePictureValidation"
                                SkinID="Button" OnClick="btnSave_Click" ClientIDMode="Static" /></span>
                        <span style="margin: 0;">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                OnClick="btnCancel_Click" SkinID="Button" />
                        </span>
                    </div>
                </div>
            </div>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks" runat="server" id="dvRightLinks">
                <asp:HyperLink ID="hlAddNewMedia" runat="server" Text="View All Icons" NavigateUrl="~/Admin/SuperAdmin/PackageIconManagement.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
