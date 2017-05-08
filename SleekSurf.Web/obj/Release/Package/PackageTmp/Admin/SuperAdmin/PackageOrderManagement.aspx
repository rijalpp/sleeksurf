<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="PackageOrderManagement.aspx.cs" Inherits="SleekSurf.Web.Admin.SuperAdmin.PackageOrderManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/JScript/site.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: auto; margin-bottom: 10px;">
                    Package Order Management Portal</div>
                <div class="search" style="width: 100%; text-align: justify;">
                    <span style="display: inline-block;">
                        <label>
                            <span class="mandatory">*</span> Order Status:</label>
                        <asp:DropDownList ID="ddlOrderStatus" runat="server">
                        </asp:DropDownList>
                    </span><span style="margin-left: 10px; display: inline-block;">
                        <label style="width: 100px">
                            Date From:</label>
                        <asp:TextBox ID="txtDateFrom" runat="server"></asp:TextBox>
                        <asp:Image ID="imgStartDate" ImageUrl="~/App_Themes/SleekTheme/Images/Calendar.png"
                            runat="server" Height="28px" Style="float: right; cursor: pointer" />
                        <asp:TextBoxWatermarkExtender ID="TextBoxWaterMarkDOB" runat="server" WatermarkText=" dd/MM/yyyy "
                            TargetControlID="txtDateFrom" WatermarkCssClass="waterMark">
                        </asp:TextBoxWatermarkExtender>
                        <asp:CalendarExtender ID="CalendarExtenderDOB" runat="server" Format="dd/MM/yyyy"
                            CssClass="calandarTheme" TargetControlID="txtDateFrom" PopupButtonID="imgStartDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender AutoComplete="true" ID="MaskedEditExtenderDOB" runat="server"
                            TargetControlID="txtDateFrom" MaskType="Date" Mask="99/99/9999" CultureName="en-AU">
                        </asp:MaskedEditExtender>
                        <asp:MaskedEditValidator ID="MaskedEditValidatorDOB" runat="server" ControlExtender="MaskedEditExtenderDOB"
                            ControlToValidate="txtDateFrom" IsValidEmpty="false" ErrorMessage="Invalid Date Format"
                            InvalidValueMessage="Invalid Date Format (dd/MM/yyyy)" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$"
                            Display="none" ValidationGroup="OrderValidation" ForeColor="Red">
                        </asp:MaskedEditValidator>
                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" TargetControlID="MaskedEditValidatorDOB"
                            runat="server">
                        </asp:ValidatorCalloutExtender>
                    </span><span style="margin-left: 10px">
                        <asp:Button ID="btnSearch" runat="server" SkinID="SearchButtonSmall" Text="Search"
                            OnClick="btnSearch_Click" ValidationGroup="OrderValidation" />
                    </span>
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
                    <asp:GridView ID="gvOrderManagement" runat="server" DataKeyNames="OrderID, Client"
                        AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvOrderManagement_RowDataBound"
                        OnSelectedIndexChanging="gvOrderManagement_SelectedIndexChanging">
                        <EmptyDataTemplate>
                            No Package Orders found in the Database.
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input id="checkAll" class="styled" type="checkbox" onclick="javascript:SelectOrUnselectAll('<%= this.gvOrderManagement.ClientID %>',this,'cbDelete','imgDeleteBtn')">
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
                            <asp:TemplateField HeaderText="ClientDetail">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnClientName" runat="server" OnCommand="lbtnClientName_Command"></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle Width="250px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="PackageName" HeaderText="Package" ReadOnly="true" ControlStyle-Width="100px" />
                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderStatus" runat="server" Text='<%# Eval("OrderStatus") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("RegistrationDate","{0:d}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="End Date" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("ExpiryDate","{0:d}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEmail" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/SendEmail.png"
                                        ToolTip="Send email to this client" AlternateText="Email" />
                                </ItemTemplate>
                                <HeaderStyle Width="5px" />
                            </asp:TemplateField>
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
            </div>
        </div>
    </div>
</asp:Content>
