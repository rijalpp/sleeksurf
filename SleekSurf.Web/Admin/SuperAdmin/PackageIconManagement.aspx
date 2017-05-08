<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="PackageIconManagement.aspx.cs" Inherits="SleekSurf.Web.Admin.SuperAdmin.PackageIconManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/JScript/site.js" type="text/javascript"></script>
    <script type="text/javascript">
        function allownumbers(e) {
            var key = window.event ? e.keyCode : e.which;
            var keychar = String.fromCharCode(key);
            var reg = new RegExp("[0-9.]")
            if (key == 8) {
                keychar = String.fromCharCode(key);
            }
            if (key == 13) {
                key = 8;
                keychar = String.fromCharCode(key);
            }
            return reg.test(keychar);
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: 100%">
                    Package Icon Management Portal</div>
            </div>
            <div>
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                <div>
                    <asp:GridView ID="gvPackagePictureManagement" runat="server" DataKeyNames="PictureID, PackageCode"
                        AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvPackagePictureManagement_RowDataBound"
                        OnSelectedIndexChanging="gvPackagePictureManagement_SelectedIndexChanging" OnRowEditing="gvPackagePictureManagement_RowEditing"
                        OnRowUpdating="gvPackagePictureManagement_RowUpdating" OnRowCancelingEdit="gvPackagePictureManagement_RowCancellingEdit">
                        <EmptyDataTemplate>
                            No Package Picture found in the database.
                        </EmptyDataTemplate>
                        <EmptyDataRowStyle CssClass="normalMsg" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input id="checkAll" class="styled" type="checkbox" onclick="javascript:SelectOrUnselectAll('<%= this.gvPackagePictureManagement.ClientID %>',this,'cbDelete','imgDeleteBtn')">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbDelete" runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="imgDeleteBtn" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Trash.png"
                                        OnClick="imgDeleteBtn_Click" />
                                </FooterTemplate>
                                <HeaderStyle Width="5px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Image">
                                <ItemTemplate>
                                    <span style="max-height: 40px; max-width: 40px; overflow: hidden; border: 1px solid #46a8f3;
                                        display: block;">
                                        <asp:Image ID="imgPackageIcon" ImageUrl='<%# Bind("PictureID","~/Uploads/PackageImages/{0}") %>'
                                            runat="server" AlternateText="packageIcon" Style="width: 40px;" />
                                    </span>
                                </ItemTemplate>
                                <HeaderStyle Width="40px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ToolTip">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("PictureDescription") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPictureDescription" runat="server" Text='<%# Bind("PictureDescription") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("DisplayOrder") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDisplayOrder" runat="server" Text='<%# Eval("DisplayOrder") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle Width="50px" />
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Image" EditImageUrl="~/App_Themes/SleekTheme/Images/Edit.png"
                                UpdateImageUrl="~/App_Themes/SleekTheme/Images/Update.png" CancelImageUrl="~/App_Themes/SleekTheme/Images/Cancel.png"
                                ShowEditButton="true" ValidationGroup="valgUpdateAccount">
                                <ItemStyle Width="55px" HorizontalAlign="Center" />
                            </asp:CommandField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks">
                <asp:HyperLink ID="hlAddNewEditMedia" runat="server" Text="Add New Feature Icon"
                    NavigateUrl="~/Admin/SuperAdmin/NewEditPackageIcon.aspx" CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="hlNewEditPromotion" runat="server" Text="View Parent Package"
                    NavigateUrl="~/Admin/SuperAdmin/NewEditPackage.aspx" CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
