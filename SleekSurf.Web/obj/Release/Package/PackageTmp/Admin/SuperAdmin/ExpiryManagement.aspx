<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="ExpiryManagement.aspx.cs" Inherits="SleekSurf.Web.Admin.SuperAdmin.ExpiryManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/JScript/site.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: auto; margin-bottom: 10px;">
                    Expiry Management Portal</div>
            </div>
            <asp:TabContainer ID="tabExpiryManagement" runat="server" OnActiveTabChanged="tabExpiryManagement_ActiveTabChanged"
                AutoPostBack="true">
                <asp:TabPanel ID="tpnlPackageorder" runat="server" HeaderText="Accounts">
                    <ContentTemplate>
                        <div class="normalBody" style="display: inline-block;">
                            <div class="search" style="float: right; margin-top: 10px;">
                                <label style="width: 100px">
                                    <span class="mandatory">*</span> Client Name:</label>
                                <span style="width: 215px; text-align: left; display: inline-block;">
                                    <asp:TextBox ID="txtClientName" runat="server"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="txtClientNameWaterMark" TargetControlID="txtClientName"
                                        runat="server" WatermarkText="Search By ClientName" WatermarkCssClass="waterMark">
                                    </asp:TextBoxWatermarkExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                        ControlToValidate="txtClientName" Display="Dynamic" ValidationGroup="OrderValidation">
                                    </asp:RequiredFieldValidator>
                                </span><span>
                                    <asp:Button ID="btnSearch" runat="server" SkinID="SearchButtonSmall" Text="Search"
                                        OnClick="btnSearch_Click" ValidationGroup="OrderValidation" />
                                </span>
                            </div>
                            <div style="clear: both; padding-top: 10px;">
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
                                    <asp:GridView ID="gvOrderExpiryManagement" runat="server" DataKeyNames="OrderID, Client, ExpiryDate"
                                        AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvOrderExpiryManagement_RowDataBound"
                                        OnSelectedIndexChanging="gvOrderExpiryManagement_SelectedIndexChanging" Width="100%">
                                        <EmptyDataTemplate>
                                            No Package Orders Expiry found in the Database.
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <input id="checkAll" class="styled" type="checkbox" onclick="javascript:SelectOrUnselectAll('<%= this.gvOrderExpiryManagement.ClientID %>',this,'cbDelete','imgDeleteBtn')">
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
                                            <asp:TemplateField HeaderText="Expiry Date" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("ExpiryDate","{0:d}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Days Left" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltrDaysLeft" runat="server"></asp:Literal>
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
                                <div style="margin: 5px; text-align: center;">
                                    <asp:RadioButtonList ID="rbtnOptionList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnOptionList_SelectedIndexChanged"
                                        CssClass="radioButtonList" RepeatLayout="UnorderedList">
                                        <asp:ListItem Selected="True">All</asp:ListItem>
                                        <asp:ListItem>Current</asp:ListItem>
                                        <asp:ListItem>Expired</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tpnlAdvertisement" runat="server" HeaderText="Advertisements">
                    <ContentTemplate>
                        <div class="normalBody" style="display: inline-block;">
                            <div class="search" style="float: right; margin-top: 10px;">
                                <label style="width: 80px">
                                    <span class="mandatory">*</span> Advertiser:</label>
                                <span style="width: 215px; text-align: left; display: inline-block;">
                                    <asp:TextBox ID="txtAdvertiser" runat="server"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtAdvertiser"
                                        runat="server" WatermarkText="Search By Advertiser" WatermarkCssClass="waterMark">
                                    </asp:TextBoxWatermarkExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                        ControlToValidate="txtAdvertiser" Display="Dynamic" ValidationGroup="AdValidation">
                                    </asp:RequiredFieldValidator>
                                </span><span>
                                    <asp:Button ID="btnSearchAd" runat="server" SkinID="SearchButtonSmall" Text="Search"
                                        OnClick="btnSearchAd_Click" ValidationGroup="AdValidation" />
                                </span>
                            </div>
                            <div style="clear: both; padding-top: 10px;">
                                <asp:Label ID="lblMessageAd" runat="server" EnableViewState="false"></asp:Label>
                                <div class="paginationSummary">
                                    <asp:Panel ID="pnlgvPersonNavigatorTopAd" runat="server" Visible="false">
                                        <!-- wrapper of upper part  -->
                                        <div class="summary">
                                            <span style="margin-right: 2px">
                                                <asp:Label runat="server" Font-Bold="true" Text="Page "></asp:Label></span>
                                            <span style="margin-right: 5px">
                                                <asp:Label ID="lblStartPageAd" runat="server"></asp:Label></span> <span style="margin-right: 5px;
                                                    font-weight: bold;">of</span> <span style="margin-right: 5px">
                                                        <asp:Label ID="lblTotalPagesAd" runat="server"></asp:Label></span>
                                        </div>
                                        <div class="summary" style="float: right;">
                                            <span style="margin-right: 2px; font-weight: bold;">Total Record(s): </span><span>
                                                <asp:Label ID="lblTotalNoAd" runat="server"></asp:Label></span>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div>
                                    <asp:GridView ID="gvExpiringAdManagement" runat="server" DataKeyNames="AdID, EndDate, Email"
                                        AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvExpiringAdManagement_RowDataBound"
                                        OnSelectedIndexChanging="gvExpiringAdManagement_SelectedIndexChanging" Width="100%">
                                        <EmptyDataTemplate>
                                            No Advertisement Expiry found in the Database.
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <input id="checkAll" class="styled" type="checkbox" onclick="javascript:SelectOrUnselectAll('<%= this.gvExpiringAdManagement.ClientID %>',this,'cbDelete','imgDeleteBtnAd')">
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbDelete" runat="server" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgDeleteBtnAd" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Trash.png"
                                                        OnClick="imgDeleteBtnAd_Click" ToolTip="Delete Selected Items" AlternateText="Delete" />
                                                </FooterTemplate>
                                                <HeaderStyle Width="5px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Advertiser">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltrAdvertiser" runat="server" Text='<%#Eval("Advertiser") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle Width="250px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contact No" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltrContactDetails" runat="server" Text='<%# Eval("ContactDetail") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Expiry Date" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("EndDate","{0:d}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Days Left" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltrDaysLeft" runat="server"></asp:Literal>
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
                                    <asp:Panel ID="pnlNavigatorBottomAd" runat="server" Visible="false">
                                        <!-- wrapper of lower part  -->
                                        <div>
                                            <asp:LinkButton ID="lbtnPreviousAd" runat="server" Text="Previous" CommandName="Previous"
                                                OnCommand="ChangePageAd" SkinID="Pagination"></asp:LinkButton>
                                        </div>
                                        <div style="padding: 0px 20px;">
                                            <asp:Repeater ID="rptPagerAd" runat="server" OnItemCommand="rptPager_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnPagerButtonAd" runat="server" Text='<%# Eval("Page") %>'
                                                        CommandName='<%# Eval("Page") %>' SkinID="Pagination">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <div>
                                            <asp:LinkButton ID="lbtnNextAd" runat="server" Text="Next" CommandName="Next" OnCommand="ChangePageAd"
                                                SkinID="Pagination"></asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div style="clear: both; margin: 5px; text-align: center;">
                                    <asp:RadioButtonList ID="rbtnOptionListAd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnOptionListAd_SelectedIndexChanged"
                                        CssClass="radioButtonList" RepeatLayout="UnorderedList">
                                        <asp:ListItem Selected="True">All</asp:ListItem>
                                        <asp:ListItem>Current</asp:ListItem>
                                        <asp:ListItem>Expired</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tpnlClientInActiveByDefault" runat="server" HeaderText="Clients - Inactive By Default">
                  <ContentTemplate>
                  <div class="normalBody" style="display: inline-block;">
                           <%-- <div class="search" style="float: right; margin-top: 10px;">
                                <label style="width: 80px">
                                    <span class="mandatory">*</span> Client Name:</label>
                                <span style="width: 215px; text-align: left; display: inline-block;">
                                    <asp:TextBox ID="txtClientNameByStatus" runat="server"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderClientName" TargetControlID="txtClientNameByStatus"
                                        runat="server" WatermarkText="Search By Client Name" WatermarkCssClass="waterMark">
                                    </asp:TextBoxWatermarkExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                                        ControlToValidate="txtClientNameByStatus" Display="Dynamic" ValidationGroup="StatusValidation">
                                    </asp:RequiredFieldValidator>
                                </span><span>
                                    <asp:Button ID="btnSearchClientByStatus" runat="server" SkinID="SearchButtonSmall" Text="Search"
                                         ValidationGroup="StatusValidation" />
                                </span>
                            </div>--%>
                            <div style="clear: both; padding-top: 10px;">
                                <asp:Label ID="lblMessageClientStatus" runat="server" EnableViewState="false"></asp:Label>
                                <div class="paginationSummary">
                                    <asp:Panel ID="pnlgvPersonNavigatorTopClientStatus" runat="server" Visible="false">
                                        <!-- wrapper of upper part  -->
                                        <div class="summary">
                                            <span style="margin-right: 2px">
                                                <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Page "></asp:Label></span>
                                            <span style="margin-right: 5px">
                                                <asp:Label ID="lblStartPageClientStatus" runat="server"></asp:Label></span> <span style="margin-right: 5px;
                                                    font-weight: bold;">of</span> <span style="margin-right: 5px">
                                                        <asp:Label ID="lblTotalPagesClientStatus" runat="server"></asp:Label></span>
                                        </div>
                                        <div class="summary" style="float: right;">
                                            <span style="margin-right: 2px; font-weight: bold;">Total Record(s): </span><span>
                                                <asp:Label ID="lblTotalNoClientStatus" runat="server"></asp:Label></span>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div>
                                    <asp:GridView ID="gvClientsByStatus" runat="server" DataKeyNames="ClientID, CreatedDate, BusinessEmail"
                                        AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvClientsByStatus_RowDataBound"
                                        OnSelectedIndexChanging="gvClientsByStatus_SelectedIndexChanging" Width="100%">
                                        <EmptyDataTemplate>
                                            No Clients with the given status found.
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <input id="checkAll" class="styled" type="checkbox" onclick="javascript:SelectOrUnselectAll('<%= this.gvClientsByStatus.ClientID %>',this,'cbDelete','imgDeleteBtnClientByStatus')">
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbDelete" runat="server" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgDeleteBtnClientByStatus" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Trash.png"
                                                        OnClick="imgDeleteBtnClientByStatus_Click" ToolTip="Delete Selected Items" AlternateText="Delete" />
                                                </FooterTemplate>
                                                <HeaderStyle Width="5px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Client Name">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltrAdvertiser" runat="server" Text='<%#Eval("ClientName") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle Width="250px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comment" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltrContactDetails" runat="server" Text='<%# Eval("Comment") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Created Date" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("CreatedDate","{0:d}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Days" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltrDays" runat="server"></asp:Literal>
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
                                    <asp:Panel ID="pnlNavigatorBottomClientStatus" runat="server" Visible="false">
                                        <!-- wrapper of lower part  -->
                                        <div>
                                            <asp:LinkButton ID="lbtnPreviousClientStatus" runat="server" Text="Previous" CommandName="Previous"
                                                OnCommand="ChangePageClientStatus" SkinID="Pagination"></asp:LinkButton>
                                        </div>
                                        <div style="padding: 0px 20px;">
                                            <asp:Repeater ID="rptPagerClientStatus" runat="server" OnItemCommand="rptPagerClientStatus_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnPagerButtonClientStatus" runat="server" Text='<%# Eval("Page") %>'
                                                        CommandName='<%# Eval("Page") %>' SkinID="Pagination">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <div>
                                            <asp:LinkButton ID="lbtnNextClientStatus" runat="server" Text="Next" CommandName="Next" OnCommand="ChangePageClientStatus"
                                                SkinID="Pagination"></asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                </div>
                               <%-- <div style="clear: both; margin: 5px; text-align: center;">
                                    <asp:RadioButtonList ID="rbtnOptionListClientStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnOptionListClientStatus_SelectedIndexChanged"
                                        CssClass="radioButtonList" RepeatLayout="UnorderedList">
                                        <asp:ListItem Selected="True">InActiveByDefault</asp:ListItem>
                                        <asp:ListItem>InActive</asp:ListItem>
                                        <asp:ListItem>InActiveBySuperAdmin</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>--%>
                            </div>
                        </div>
                  </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks">
            </div>
        </div>
    </div>
</asp:Content>
