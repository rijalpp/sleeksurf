<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="PromotionGalleryManagement.aspx.cs" Inherits="SleekSurf.Web.Admin.Client.PromotionGalleryManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/JScript/site.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('img[id*=imgPromotion]').each(function () {
                if ((typeof this.naturalWidth != "undefined" && this.naturalWidth == 0) || this.readyState == 'uninitialized') {
                    $(this).attr('src', '../../App_Themes/SleekTheme/Images/PromotionDefault.png');
                }
            });
        });
    </script>
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: 100%">
                    Promotion Gallery Management Portal</div>
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
                    <asp:GridView ID="gvPromotionGalleryManagement" runat="server" DataKeyNames="MediaID, MediaUrl, MediaType"
                        AutoGenerateColumns="false" ShowFooter="true" OnRowUpdating="gvPromotionGalleryManagament_RowUpdating"
                        OnRowEditing="gvPromotionGalleryManagement_RowEditing" OnRowDataBound="gvPromotionGalleryManagement_RowDataBound"
                        OnRowCancelingEdit="gvPromotionGalleryManagement_RowCancellingEdit" OnSelectedIndexChanging="gvPromotionGalleryManagement_SelectedIndexChanging">
                        <EmptyDataTemplate>
                            No Media gallery found for the Promotion.
                        </EmptyDataTemplate>
                        <EmptyDataRowStyle CssClass="normalMsg" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input id="checkAll" class="styled" type="checkbox" onclick="javascript:SelectOrUnselectAll('<%= this.gvPromotionGalleryManagement.ClientID %>',this,'cbDelete','imgDeleteBtn')">
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
                                    <span style="max-height: 40px; max-width: 40px; overflow: hidden; border: 1px solid #46a8f3; display: block;">
                                        <asp:Image ID="imgPromotion" ImageUrl='<%# Bind("MediaUrl","~{0}") %>' runat="server"
                                            AlternateText="PromoMedia" Style="width: 40px;" />
                                    </span>
                                </ItemTemplate>
                                <HeaderStyle Width="40px" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Title" DataField="Title" ReadOnly="true" />
                            <asp:TemplateField HeaderText="Type">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("MediaType") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IsActive">
                                <HeaderTemplate>
                                    <asp:Image ID="imgDelete" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Active.png"
                                        ToolTip="(In)Active" AlternateText="(In)Active" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Eval("IsActive") %>' Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkEditActive" runat="server" Checked='<%# Eval("IsActive") %>' />
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
                <div style="clear: both; margin: 5px; text-align: center;">
                    <asp:RadioButtonList ID="rbtnOptionList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnOptionList_SelectedIndexChanged"
                        CssClass="radioButtonList" RepeatLayout="UnorderedList">
                        <asp:ListItem Selected="True">All</asp:ListItem>
                        <asp:ListItem>Enabled</asp:ListItem>
                        <asp:ListItem>Disabled</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks">
                <asp:HyperLink ID="hlAddNewEditMedia" runat="server" Text="Add New Media" NavigateUrl="~/Admin/Client/NewEditPromotionGallery.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="hlNewEditPromotion" runat="server" Text="View Parent Promotion"
                    NavigateUrl="~/Admin/Client/NewEditPromotion.aspx" CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
