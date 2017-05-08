<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true" CodeBehind="PackageManagement.aspx.cs" Inherits="SleekSurf.Web.Admin.SuperAdmin.PackageManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/JScript/site.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title">
                    Package Management Portal.</div>
            </div>
            <div>
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                <div class="paginationSummary">
                    <asp:Panel ID="pnlgvPersonNavigatorTop" runat="server" Visible="false">
                        <!-- wrapper of upper part  -->
                        <div class="summary">
                            <span style="margin-right: 2px">
                                <asp:Label ID="Label7" runat="server" Font-Bold="true" Text="Page "></asp:Label></span>
                            <span style="margin-right: 5px">
                                <asp:Label ID="lblStartPage" runat="server"></asp:Label></span> <span style="margin-right: 5px;
                                    font-weight: bold;">of</span> <span style="margin-right: 5px">
                                        <asp:Label ID="lblTotalPages" runat="server"></asp:Label></span>
                        </div>
                        <div class="summary" style="float: right;">
                            <span style="margin-right: 2px; font-weight: bold;">Total Record(s):</span> <span>
                                <asp:Label ID="lblTotalNo" runat="server"></asp:Label></span>
                        </div>
                    </asp:Panel>
                </div>
                <div>
                    <asp:GridView ID="gvPackageManagement" DataKeyNames="PackageCode" runat="server"
                        AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvPackageManagement_RowDataBound"
                        OnSelectedIndexChanging="gvPackageManagement_SelectedIndexChanging" OnRowUpdating="gvPackageManagement_RowUpdating"
                        OnRowEditing="gvPackageManagement_RowEditing" OnRowCancelingEdit="gvPackageManagement_RowCancellingEdit">
                        <EmptyDataTemplate>
                            No Package found in the Database
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input id="checkAll" class="styled" type="checkbox" onclick="javascript:SelectOrUnselectAll('<%= this.gvPackageManagement.ClientID %>',this,'cbDelete','imgDeleteBtn')">
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
                            <asp:BoundField DataField="PackageName" HeaderText="Name" ReadOnly="true" />
                            <asp:BoundField DataField ="FeatureType" HeaderText="Feature" ReadOnly="true" HeaderStyle-Width="100px" />
                            <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" ReadOnly="true" HeaderStyle-Width="100px" DataFormatString="{0:d}" />
                            <asp:TemplateField HeaderText="IsActive">
                                <HeaderTemplate>
                                    <asp:Image ID="imgDelete" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Active.png"
                                        ToolTip="(In)Active" AlternateText="(In)Active" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkPublished" runat="server" Checked='<%# Bind("Published") %>'
                                        Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkPublished" runat="server" Checked='<%# Bind("Published") %>' />
                                </EditItemTemplate>
                                <HeaderStyle Width="5px" />
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Image" EditImageUrl="~/App_Themes/SleekTheme/Images/Edit.png"
                                UpdateImageUrl="~/App_Themes/SleekTheme/Images/Update.png" CancelImageUrl="~/App_Themes/SleekTheme/Images/Cancel.png"
                                ShowEditButton="true">
                                <ItemStyle Width="55px" HorizontalAlign="Center" />
                            </asp:CommandField>
                            <asp:CommandField ButtonType="Image" SelectImageUrl="~/App_Themes/SleekTheme/Images/Select.png"
                                ShowSelectButton="true" HeaderStyle-Width="5px" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="pagination">
                    <asp:Panel ID="pnlNavigatorBottom" runat="server" Visible="false">
                        <!-- wrapper of lower part  -->
                        <div>
                            <asp:LinkButton ID="lbtnPrevious" runat="server" Text="Previous" CommandName="Previous"
                                OnCommand="ChangePage" SkinID="Pagination"></asp:LinkButton>
                        </div>
                        <div style="padding: 0px 20px;">
                            <asp:Repeater ID="rptPager" runat="server" OnItemCommand="rptPager_ItemCommand">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnPagerButton" runat="server" Text='<%# Eval("Page") %>' CommandName='<%# Eval("Page") %>'
                                        SkinID="Pagination">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div>
                            <asp:LinkButton ID="lbtnNext" runat="server" Text="Next" CommandName="Next" OnCommand="ChangePage"
                                SkinID="Pagination"></asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
                <div>
                    <asp:RadioButtonList ID="rdlPublished" runat="server" CssClass="radioButtonList"
                        RepeatLayout="UnorderedList" AutoPostBack="true" OnSelectedIndexChanged="rdlPublished_SelectedIndexChanged">
                        <asp:ListItem Text="All" Value="All" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Published" Value="true"></asp:ListItem>
                        <asp:ListItem Text="UnPublished" Value="False"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks">
                <asp:HyperLink ID="hlAddNewPackage" runat="server" Text="Add New Package" NavigateUrl="~/Admin/SuperAdmin/NewEditPackage.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
