<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true" CodeBehind="AdvertisementManagement.aspx.cs" Inherits="SleekSurf.Web.Admin.SuperAdmin.AdvertisementManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/JScript/site.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width:45%;">
                    Advertisement Management Portal</div>
                <div class="search"  style="width:55%;">
                    <asp:Label ID="lblAd" runat="server" Text="* Advertiser:" Width="80px" CssClass="label"> </asp:Label>
                    <span class="marginSideBySide">
                        <asp:TextBox ID="txtAdName" runat="server" SkinID="AutoComplete" ToolTip="Search By Advertiser"></asp:TextBox>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtAdName"
                            ValueToCompare="Search By Advertiser" Type="String" Operator="NotEqual" ValidationGroup="valgAdSearch" />
                    </span>
                    <asp:Button ID="btnSearch" runat="server" SkinID="SearchButtonSmall" Text="Search"
                        OnClick="btnSearch_Click" ValidationGroup="valgAdSearch" />
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
                            <span style="margin-right: 2px; font-weight: bold;">Total Record(s):</span> <span>
                                <asp:Label ID="lblTotalNo" runat="server"></asp:Label></span>
                        </div>
                    </asp:Panel>
                </div>
                <div>
                    <asp:GridView ID="gvAdvertisementManagement" runat="server" DataKeyNames="AdID, ImageUrl, Email" AutoGenerateColumns="false"
                        ShowFooter="true" OnRowDataBound="gvAdvertisementManagement_RowDataBound" OnSelectedIndexChanging="gvAdvertisementManagement_SelectedIndexChanging"
                        OnRowUpdating="gvAdvertisementManagement_RowUpdating" OnRowCancelingEdit="gvAdvertisementManagement_CancellingEdit"
                        OnRowEditing="gvAdvertisementManagement_RowEditing">
                        <EmptyDataTemplate>
                            No Advertisement found.
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input id="checkAll" class="styled" type="checkbox" onclick="javascript:SelectOrUnselectAll('<%= this.gvAdvertisementManagement.ClientID %>',this,'cbDelete','imgDeleteBtn')">
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
                            <asp:TemplateField HeaderText="Advertiser">
                                <ItemTemplate>
                                    <asp:Label ID="lblAdvertiser" runat="server"><%# Eval("Advertiser")%></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="120px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Contact">
                                <ItemTemplate>
                                    <asp:Label ID="lblContactDetail" runat="server"><%# Eval("ContactDetail")%></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Period">
                                <ItemTemplate>
                                    <asp:Label ID="lblAdvertisePeriod" runat="server"><%# Eval("StartDate", "{0:dd/MM/yyyy}").ToString()%> - <%# Eval("EndDate", "{0:dd/MM/yyyy}").ToString()%> </asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="135px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amt">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountPaid" runat="server"><%# Eval("AmountPaid","{0:0.00}")%></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="30px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Display">
                                <ItemTemplate>
                                    <asp:Label ID="lblDisplayPosition" runat="server"><%# Eval("DisplayPosition")%></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="40px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Image ID="imgActive" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Active.png"
                                        ToolTip="(In)Active" AlternateText="(In)Active" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkPublished" runat="server" Checked='<%# Eval("Published") %>' Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkEditPublished" runat="server" Checked='<%# Bind("Published") %>' />
                                </EditItemTemplate>
                                <HeaderStyle Width="5px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEmail" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/SendEmail.png"
                                        ToolTip='<%# Bind("Email") %>' AlternateText="Email" />
                                </ItemTemplate>
                                <HeaderStyle Width="5px" />
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Image" EditImageUrl="~/App_Themes/SleekTheme/Images/Edit.png"
                                UpdateImageUrl="~/App_Themes/SleekTheme/Images/Update.png" CancelImageUrl="~/App_Themes/SleekTheme/Images/Cancel.png"
                                ShowEditButton="true" ValidationGroup="valgUpdateAccount">
                                <ItemStyle Width="45px" HorizontalAlign="Center" />
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
                <div style="clear: both; margin: 5px; text-align: center;">
                    <asp:RadioButtonList ID="rbtnOptionList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnOptionList_SelectedIndexChanged"
                        CssClass="radioButtonList" RepeatLayout="UnorderedList">
                        <asp:ListItem Selected="True">All</asp:ListItem>
                        <asp:ListItem>Published</asp:ListItem>
                        <asp:ListItem>Unpublished</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks">
                <asp:HyperLink ID="hlAddNewUser" runat="server" Text="Add New Ad" NavigateUrl="~/Admin/SuperAdmin/NewEditAdvertisement.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>

