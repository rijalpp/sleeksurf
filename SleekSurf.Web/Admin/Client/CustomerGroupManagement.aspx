<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="CustomerGroupManagement.aspx.cs" Inherits="SleekSurf.Web.Admin.Client.CustomerGroupManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/JScript/site.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title">
                    Customer Group Mgmt Portal</div>
                <div class="search">
                    <asp:Label ID="lblCustomerGroupName" runat="server" Text="* Group Name:" Width="100px"
                        CssClass="label"> </asp:Label>
                    <span class="marginSideBySide">
                        <asp:TextBox ID="txtCustomerGroupName" runat="server" SkinID="AutoComplete" ToolTip="Search Customer"></asp:TextBox>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtCustomerGroupName"
                            ValueToCompare="Search Group" Type="String" Operator="NotEqual" ValidationGroup="valgCustomerSearch" />
                    </span>
                    <asp:Button ID="btnSearch" runat="server" SkinID="SearchButtonSmall" Text="Search"
                        ValidationGroup="valgCustomerSearch" OnClick="btnSearch_Click" />
                </div>
            </div>
            <div>
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                <div>
                    <asp:GridView ID="gvCustomerGroupManagement" runat="server" DataKeyNames="CustomerGroupID, ClientID"
                        PageSize="10" AllowPaging="true" AutoGenerateColumns="false" ShowFooter="true"
                        OnRowDataBound="gvCustomerGroupManagement_RowDataBound" OnRowUpdating="gvCustomerGroupManagement_RowUpdating"
                        OnRowEditing="gvCustomerGroupManagement_RowEditing" OnRowCancelingEdit="gvCustomerGroupManagement_RowCancellingEdit"
                        OnSelectedIndexChanging="gvCustomerGroupManagement_SelectedIndexChanging" OnPageIndexChanging="gvCustomerGroupManagement_PageIndexChanging">
                        <EmptyDataTemplate>
                            No Group found in the Database
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input id="checkAll" class="styled" type="checkbox" onclick="javascript:SelectOrUnselectAll('<%= this.gvCustomerGroupManagement.ClientID %>',this,'cbDelete','imgDeleteBtn')">
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
                            <asp:BoundField HeaderText="Name" DataField="GroupName" ReadOnly="true" HeaderStyle-Width="250px" />
                            <asp:TemplateField HeaderText="Count">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerNumber" runat="server" Text='<%# Eval("CustomerCount") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Comment" DataField="Comments" ReadOnly="true" HeaderStyle-Width="180px" />
                            <asp:TemplateField HeaderText="IsActive">
                                <HeaderTemplate>
                                    <asp:Image ID="imgDelete" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Active.png"
                                        ToolTip="(In)Active" AlternateText="(In)Active" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkPublished" runat="server" Checked='<%# Eval("Published") %>'
                                        Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkEditPublished" runat="server" Checked='<%# Bind("Published") %>' />
                                </EditItemTemplate>
                                <HeaderStyle Width="5px" />
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Image" EditImageUrl="~/App_Themes/SleekTheme/Images/Edit.png"
                                UpdateImageUrl="~/App_Themes/SleekTheme/Images/Update.png" CancelImageUrl="~/App_Themes/SleekTheme/Images/Cancel.png"
                                ShowEditButton="true" ValidationGroup="valgUpdateAccount">
                                <ItemStyle Width="55px" HorizontalAlign="Center" />
                            </asp:CommandField>
                            <asp:CommandField ButtonType="Image" SelectImageUrl="~/App_Themes/SleekTheme/Images/Select.png"
                                ShowSelectButton="true" HeaderStyle-Width="5px" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks">
                <asp:HyperLink ID="hlAddNewUser" runat="server" Text="Add New Group" NavigateUrl="~/Admin/Client/NewEditCustomerGroup.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
