<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="ClientManagement.aspx.cs" Inherits="SleekSurf.Web.Admin.SuperAdmin.ClientManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/JScript/site.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('img[id*=imgLogo]').each(function () {
               if ((typeof this.naturalWidth != "undefined" && this.naturalWidth == 0) || this.readyState == 'uninitialized') {
                    $(this).attr("src", "../../App_Themes/SleekTheme/Images/BusinessLogoDefault.png");
                }
            });
        });
    </script>
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title">
                    Client Management Portal</div>
                <div class="search">
                    <asp:Panel ID="pnSearch" runat="server" ClientIDMode="Static" DefaultButton="btnSearch">
                        <asp:Label ID="lblClientName" runat="server" Text="* Client Name:" Width="95px" CssClass="label"> </asp:Label>
                        <span class="marginSideBySide">
                            <asp:TextBox ID="txtClientName" runat="server" SkinID="AutoComplete" ToolTip="Search Client"></asp:TextBox>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtClientName"
                                ValueToCompare="Search Client" Type="String" Operator="NotEqual" ValidationGroup="valgClientSearch" />
                        </span>
                        <asp:Button ID="btnSearch" runat="server" SkinID="SearchButtonSmall" Text="Search"
                            OnClick="btnSearch_Click" ValidationGroup="valgClientSearch" />
                    </asp:Panel>
                </div>
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
                            <span style="margin-right: 2px; font-weight: bold;">Total Record(s): </span><span>
                                <asp:Label ID="lblTotalNo" runat="server"></asp:Label></span>
                        </div>
                    </asp:Panel>
                </div>
                <div>
                    <asp:GridView ID="gvClientManagement" runat="server" AutoGenerateColumns="false"
                        ShowFooter="true" DataKeyNames="ClientID, Published, Comment" OnSelectedIndexChanging="gvClientManagement_SelectedIndexChanging"
                        OnRowUpdating="gvClientManagement_RowUpdating" OnRowEditing="gvClientManagement_RowEditing"
                        OnRowCancelingEdit="gvClientManagement_RowCancellingEdit" OnRowDataBound="gvClientManagement_RowDataBound">
                        <EmptyDataTemplate>
                            No Client found in the Database
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input id="checkAll" class="styled" type="checkbox" onclick="javascript:SelectOrUnselectAll('<%= this.gvClientManagement.ClientID %>',this,'cbDelete','imgDeleteBtn')">
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
                                    <span style="max-height: 40px; max-width: 40px; overflow: hidden; border: 1px solid #46a8f3; display: block;">
                                        <asp:Image ID="imgLogo" runat="server" ImageUrl='<%# this.ProfileImageSource(Eval("ClientID").ToString(), Eval("LogoUrl").ToString()) %>'
                                            Width="40px" ToolTip="Business Logo" AlternateText="Logo" />
                                    </span>
                                </ItemTemplate>
                                <HeaderStyle Width="40px" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="ClientName" DataField="ClientName" ReadOnly="true" HeaderStyle-Width="200px" />
                            <asp:TemplateField HeaderText="Contact Person">
                                <ItemTemplate>
                                    <asp:Label ID="lblContactPerson" runat="server" Text='<%# this.GetContactPerson(Eval("ContactPerson").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="150px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Category">
                                <ItemTemplate>
                                    <asp:Label ID="lblCategory" runat="server" Text='<%# this.GetCategory((SleekSurf.Entity.CategoryDetails)Eval("Category")) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Comment" HeaderText="Status" ReadOnly="true" HeaderStyle-Width="100px" />
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
                                ShowEditButton="true" ValidationGroup="valgUpdateAccount">
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
            </div>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks">
                <asp:HyperLink ID="hlNewEditClient" runat="server" Text="Add New Client" NavigateUrl="~/Admin/SuperAdmin/NewEditClient.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
