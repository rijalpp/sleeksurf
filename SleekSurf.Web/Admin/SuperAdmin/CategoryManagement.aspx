<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="CategoryManagement.aspx.cs" Inherits="SleekSurf.Web.Admin.SuperAdmin.CategoryManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/JScript/site.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
        <asp:UpdatePanel ID="upnlCategoryManagement" runat="server">
           <ContentTemplate>
            <div id="TitleSearch">
                <div class="title">
                    Category Management Portal</div>
                <div class="search">
                    <asp:Panel ID="pnSearch" runat="server" ClientIDMode="Static" DefaultButton="btnSearch">
                        <asp:Label ID="lblCategory" runat="server" Text="* Category:" Width="75px" CssClass="label"> </asp:Label>
                        <span class="marginSideBySide">
                            <asp:TextBox ID="txtCategoryName" runat="server" SkinID="AutoComplete" ToolTip="Search Category"></asp:TextBox>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtCategoryName"
                                ValueToCompare="Search Category" Type="String" Operator="NotEqual" ValidationGroup="valgCategorySearch" />
                        </span>
                        <asp:Button ID="btnSearch" runat="server" SkinID="SearchButtonSmall" Text="Search"
                            ValidationGroup="valgCategorySearch" OnClick="btnSearch_Click" />
                    </asp:Panel>
                </div>
            </div>
            <div>
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                <asp:GridView ID="gvCategoryManagement" DataKeyNames="CategoryID" runat="server"
                    PageSize="10" AllowPaging="true" AutoGenerateColumns="false" ShowFooter="true"
                    OnRowDataBound="gvCategoryManagement_RowDataBound" OnSelectedIndexChanging="gvCategoryManagement_SelectedIndexChanging"
                    OnPageIndexChanging="gvCategoryManagement_PageIndexChanging">
                    <EmptyDataTemplate>
                        No Category found in the Database
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <input id="checkAll" class="styled" type="checkbox" onclick="javascript:SelectOrUnselectAll('<%= this.gvCategoryManagement.ClientID %>',this,'cbDelete','imgDeleteBtn')">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbDelete" runat="server" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:ImageButton ID="imgDeleteBtn" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Trash.png"
                                    OnClick="imgDeleteBtn_Click" ToolTip="Delete Selected Items" AlternateText="Delete" />
                            </FooterTemplate>
                            <HeaderStyle Width="5px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Image">
                            <ItemTemplate>
                                <img runat="server" id="iThumb" style="width: 100px; border: 1px solid #46a8f3" alt="CategoryImage" />
                            </ItemTemplate>
                            <HeaderStyle Width="40px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CategoryName" HeaderText="Category" HeaderStyle-Width="120px" />
                        <asp:BoundField DataField="Description" HeaderText="Description" />
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/App_Themes/SleekTheme/Images/Select.png"
                            ShowSelectButton="true" HeaderStyle-Width="5px" />
                    </Columns>
                </asp:GridView>
            </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks">
                <asp:HyperLink ID="hlAddNewUser" runat="server" Text="Add New Category" NavigateUrl="~/Admin/SuperAdmin/NewEditCategory.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
