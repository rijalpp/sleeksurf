<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="UserManagement.aspx.cs" Inherits="SleekSurf.Web.Admin.SuperAdmin.UserManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/JScript/site.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title">
                    User Management Portal</div>
                <div class="search">
                    <asp:Panel ID="pnSearch" runat="server" ClientIDMode="Static" DefaultButton="btnSearch">
                        <asp:Label ID="lblUsername" runat="server" Text="* Username: " Width="80px" CssClass="label"> </asp:Label>
                        <span class="marginSideBySide">
                            <asp:TextBox ID="txtUsername" runat="server" SkinID="AutoComplete" ToolTip="Search Username"></asp:TextBox>
                            <asp:CompareValidator runat="server" ErrorMessage="*" ControlToValidate="txtUsername"
                                ValueToCompare="Search Username" Type="String" Operator="NotEqual" ValidationGroup="valgSearchUsername" />
                        </span>
                        <asp:Button ID="btnSearch" runat="server" SkinID="SearchButtonSmall" Text="Search"
                            ValidationGroup="valgSearchUsername" OnClick="btnSearch_Click" />
                    </asp:Panel>
                </div>
            </div>
            <div>
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                <asp:GridView ID="gvUserManagement" runat="server" DataKeyNames="ProviderUserKey, UserName"
                    PageSize="10" AllowPaging="true" AutoGenerateColumns="false" OnRowDataBound="gvUserManagement_RowDataBound"
                    ShowFooter="true" OnRowUpdating="gvUserManagament_RowUpdating" OnRowEditing="gvUserManagement_RowEditing"
                    OnRowCancelingEdit="gvUserManagement_RowCancellingEdit" OnPageIndexChanging="gvUserManagement_PageIndexChanging"
                    OnSelectedIndexChanging="gvUserManagement_SelectedIndexChanging">
                    <EmptyDataTemplate>
                        No Users found in the Database
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <input id="checkAll" class="styled" type="checkbox" onclick="javascript:SelectOrUnselectAll('<%= this.gvUserManagement.ClientID %>',this,'cbDelete','imgDeleteBtn')">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbDelete" runat="server" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:ImageButton ID="imgDeleteBtn" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Trash.png"
                                    ToolTip="Delete Selected Items" AlternateText="Delete" OnClick="imgDeleteBtn_Click" />
                            </FooterTemplate>
                            <HeaderStyle Width="5px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Image">
                            <ItemTemplate>
                                <span style="max-height: 40px; max-width: 40px; overflow: hidden; border: 1px solid #46a8f3; display: block;">
                                    <asp:Image ID="imgProfile" runat="server" ImageUrl='<%# this.ProfileImageSource(Eval("UserName").ToString()) %>'
                                        Width="40px" ToolTip='<%# this.FullName(Eval("UserName").ToString()) %>' />
                                </span>
                            </ItemTemplate>
                            <HeaderStyle Width="40px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="UserName" DataField="UserName" ReadOnly="true" HeaderStyle-Width="150px" />
                        <asp:TemplateField HeaderText="Roles">
                            <ItemTemplate>
                                <asp:Label ID="lblRoles" runat="server"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email">
                            <ItemTemplate>
                                <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:UpdatePanel runat="server" ID="upnlEmail">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>' OnTextChanged="txtEmail_TextChanged"
                                            AutoPostBack="true" Width="150px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqEmail" runat="server" ControlToValidate="txtEmail"
                                            Display="None" ErrorMessage="Email is required." ValidationGroup="valgUpdateAccount">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail" runat="server"
                                            ControlToValidate="txtEmail" ErrorMessage="Invalid Email Expression" Display="None"
                                            ValidationGroup="valgUpdateAccount" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                        </asp:RegularExpressionValidator>
                                        <asp:CompareValidator ID="cmpCompareEmail" runat="server" ControlToValidate="txtEmail"
                                            ValueToCompare='<%# Bind("Email") %>' ErrorMessage="Email already exists." Display="None"
                                            ValidationGroup="valgUpdateAccount"></asp:CompareValidator>
                                        <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" TargetControlID="reqEmail"
                                            runat="server">
                                        </cc1:ValidatorCalloutExtender>
                                        <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" TargetControlID="RegularExpressionValidatorEmail"
                                            runat="server">
                                        </cc1:ValidatorCalloutExtender>
                                        <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" TargetControlID="cmpCompareEmail"
                                            runat="server" Enabled="false">
                                        </cc1:ValidatorCalloutExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </EditItemTemplate>
                            <HeaderStyle Width="150px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Status" DataField="Comment" ReadOnly="true" HeaderStyle-Width="100px" />
                        <asp:TemplateField HeaderText="IsActive">
                            <HeaderTemplate>
                                <asp:Image ID="imgDelete" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Active.png"
                                    ToolTip="(In)Active" AlternateText="(In)Active" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%# Bind("IsApproved") %>'
                                    Enabled="false" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%# Bind("IsApproved") %>' />
                            </EditItemTemplate>
                            <HeaderStyle Width="5px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Image ID="imgLockOut" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/UserLockOut.png"
                                    ToolTip="User Lock Out" AlternateText="User Lock Out" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsLockedOut" runat="server" Checked='<%# Bind("IsLockedOut") %>'
                                    Enabled="false" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkIsLockedOut" runat="server" Checked='<%# Bind("IsLockedOut") %>' />
                            </EditItemTemplate>
                            <HeaderStyle Width="5px" />
                        </asp:TemplateField>
                        <asp:CommandField ButtonType="Image" EditImageUrl="~/App_Themes/SleekTheme/Images/Edit.png"
                            UpdateImageUrl="~/App_Themes/SleekTheme/Images/Update.png" CancelImageUrl="~/App_Themes/SleekTheme/Images/Cancel.png"
                            ShowEditButton="true" ValidationGroup="valgUpdateAccount">
                            <ItemStyle Width="55px" HorizontalAlign="Center" />
                        </asp:CommandField>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/App_Themes/SleekTheme/Images/Select.png"
                            ShowSelectButton="true" HeaderStyle-Width="5px" SelectText="Select" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks">
                <asp:HyperLink ID="hlAddNewUser" runat="server" Text="Add New User" NavigateUrl="~/Admin/SuperAdmin/NewEditUser.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
