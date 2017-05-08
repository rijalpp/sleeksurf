<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="CustomerManagement.aspx.cs" Inherits="SleekSurf.Web.Admin.Client.CustomerManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/JScript/site.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            $(document).click(function () {
                $("#Group").css('display', 'none');
            });

            $("#MoveToGroup").click(function (e) {
                e.stopPropagation();
                $("#Group").toggle("slow");
            });

            $("#ExistingGroup li").click(function () {
                $("#Group").css('display', 'none');
            }
            );

            $('#AddNewGroup li:first').click(function (e) {
                e.stopPropagation();
            });

            var newGroup = $(".newGroup");

            newGroup.focus(function () {
                if (newGroup.val() == this.title) {
                    newGroup.removeClass("waterMark");
                    newGroup.val("");
                    $("[id$=ibtnNewGroup]").css("visibility", "visible");
                    $(".extraSmallLabel").css("visibility", "visible");
                    ValidatorEnable($("#<%= cvNewGroup.ClientID %>")[0], false);
                }
            }).click(function (e) {
                e.stopPropagation();
            });

            newGroup.blur(function () {
                if (newGroup.val() == "") {
                    newGroup.addClass("waterMark");
                    newGroup.val(this.title);
                    $("[id$=ibtnNewGroup]").css("visibility", "hidden");
                    $(".extraSmallLabel").css("visibility", "hidden");
                    ValidatorEnable($("#<%= rfvAddNewGroup.ClientID %>")[0], true);
                }
                else
                    ValidatorEnable($("#<%= rfvAddNewGroup.ClientID %>")[0], false);
            });

            newGroup.blur();

            $("#CustomerGroups .selected").parent().addClass("selected");
        });
    </script>
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title">
                    Customer Management Portal</div>
                <div style="float: right">
                    <div id="MoveAndGroup">
                        <div id="MoveToGroup">
                            <img src="../../App_Themes/SleekTheme/Images/DropDownArrow.gif" alt="Move To" title="Move To" />
                        </div>
                        <div id="Group">
                            <div id="ExistingGroup">
                                <ul>
                                    <asp:Repeater ID="rptrCustomerGroup" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <asp:LinkButton ID="lbtnCustomerGroup" runat="server" CommandArgument='<%# Eval("CustomerGroupID") %>'
                                                    Text='<%# Eval("GroupName") %>' OnCommand="lbtnCustomerGroup_Command"></asp:LinkButton>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                            <asp:Panel ID="AddNewGroup" runat="server" ClientIDMode="Static" DefaultButton="ibtnNewGroup">
                                <ul>
                                    <li><span>
                                        <asp:TextBox ID="txtNewGroup" runat="server" CssClass="newGroup" EnableTheming="false"
                                            ToolTip="Add New Group  " MaxLength="24" />
                                    </span><span>
                                        <asp:RequiredFieldValidator ID="rfvAddNewGroup" ErrorMessage="*" ControlToValidate="txtNewGroup"
                                            runat="server" ValidationGroup="CreateNewGroup" />
                                        <%--<cc1:ValidatorCalloutExtender ID="vceNewGroup" runat="server" TargetControlID="cvNewGroup"></cc1:ValidatorCalloutExtender>--%>
                                    </span><span>
                                        <asp:ImageButton ID="ibtnNewGroup" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Enter.png"
                                            OnClick="ibtnNewGroup_Click" Visible="true" ValidationGroup="CreateNewGroup" />
                                    </span>
                                        <label class="extraSmallLabel">
                                            (Max 24 Characters)</label>
                                        <asp:CompareValidator ID="cvNewGroup" ErrorMessage="Group Aready Exists!" ControlToValidate="txtNewGroup"
                                            Display="Dynamic" Operator="NotEqual" ValidationGroup="CreateNewGroup" runat="server" />
                                    </li>
                                </ul>
                            </asp:Panel>
                        </div>
                    </div>
                    <div style="margin-left: 10px; display: inline-block">
                        <asp:Label ID="lblCustomerName" runat="server" Text="* Cust Name:" Width="90px" CssClass="label"> </asp:Label>
                        <span class="marginSideBySide">
                            <asp:TextBox ID="txtCustomerName" runat="server" SkinID="AutoComplete" ToolTip="Search Customer"></asp:TextBox>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtCustomerName"
                                ValueToCompare="Search Customer" Type="String" Operator="NotEqual" ValidationGroup="valgCustomerSearch" />
                        </span>
                        <asp:Button ID="btnSearch" runat="server" SkinID="SearchButtonSmall" Text="Search"
                            ValidationGroup="valgCustomerSearch" OnClick="btnSearch_Click" />
                    </div>
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
                    <asp:GridView ID="gvCustomerManagement" runat="server" DataKeyNames="CustomerID, ClientID"
                        AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvCustomerManagement_RowDataBound"
                        OnRowUpdating="gvCustomerManagament_RowUpdating" OnRowEditing="gvCustomerManagement_RowEditing"
                        OnRowCancelingEdit="gvCustomerManagement_RowCancellingEdit" OnSelectedIndexChanging="gvCustomerManagement_SelectedIndexChanging">
                        <EmptyDataTemplate>
                            No Customers found in the Database
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input id="checkAll" class="styled" type="checkbox" onclick="javascript:SelectOrUnselectAll('<%= this.gvCustomerManagement.ClientID %>',this,'cbDelete','imgDeleteBtn')">
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
                                        <a href='NewEditCustomer.aspx?CustomerID=<%#Eval("CustomerID") %>'>
                                            <img runat="server" id="iThumb" style="width: 40px; border: none;" alt='<%#Eval("FullName") %>'
                                                title='<%#Eval("FullName") %>' /></a> </span>
                                </ItemTemplate>
                                <HeaderStyle Width="40px" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Name" DataField="FullName" ReadOnly="true" HeaderStyle-Width="200px" />
                            <asp:TemplateField HeaderText="Email">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>' Width="150px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail" runat="server"
                                        ControlToValidate="txtEmail" ErrorMessage="Invalid Email Expression" Display="None"
                                        ValidationGroup="valgUpdateAccount" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                    </asp:RegularExpressionValidator>
                                    <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" TargetControlID="RegularExpressionValidatorEmail"
                                        runat="server">
                                    </cc1:ValidatorCalloutExtender>
                                </EditItemTemplate>
                                <HeaderStyle Width="200px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mobile">
                                <ItemTemplate>
                                    <asp:Label ID="lblContactMobile" runat="server" Text='<%# Bind("ContactMobile") %>'
                                        Width="100px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditContactMobile" runat="server" Text='<%# Bind("ContactMobile") %>'
                                        Width="100px"></asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle Width="100px" />
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
                <asp:HyperLink ID="hlAddNewUser" runat="server" Text="Add New Customer" NavigateUrl="~/Admin/Client/NewEditCustomer.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="hlGroupManagement" runat="server" Text="Group Management" NavigateUrl="~/Admin/Client/CustomerGroupManagement.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <div id="CustomerGroups">
                    <asp:Menu ID="menuCustomerGroup" runat="server" RenderingMode="List" EnableTheming="false"
                        OnMenuItemClick="menuCustomerGroup_MenuItemClick" Width="100%">
                    </asp:Menu>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
