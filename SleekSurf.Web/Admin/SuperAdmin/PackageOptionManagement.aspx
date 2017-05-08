<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="PackageOptionManagement.aspx.cs" Inherits="SleekSurf.Web.Admin.SuperAdmin.PackageOptionManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/JScript/site.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: 100%;">
                    <asp:Label ID="lblTitle" runat="server" Text="Option Management Portal - " EnableViewState="false"></asp:Label>
                </div>
            </div>
            <div>
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                <div style="margin-top: 10px;">
                    <asp:GridView ID="gvPackageOptionManagement" DataKeyNames="PackageOptionID, DiscountPercentage" runat="server"
                        AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvPackageOptionManagement_RowDataBound"
                        OnRowCommand="gvPackageOptionManagement_RowCommand" OnSelectedIndexChanging="gvPackageOptionManagement_SelectedIndexChanging"
                        OnRowUpdating="gvPackageOptionManagement_RowUpdating" OnRowEditing="gvPackageOptionManagement_RowEditing"
                        OnRowCancelingEdit="gvPackageOptionManagement_RowCancellingEdit">
                        <EmptyDataTemplate>
                            No Package Option for the selected Package found in the Database.
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input id="checkAll" class="styled" type="checkbox" onclick="javascript:SelectOrUnselectAll('<%= this.gvPackageOptionManagement.ClientID %>',this,'cbDelete','imgDeleteBtn')">
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
                            <asp:TemplateField HeaderText="Month" HeaderStyle-Width="35px" ItemStyle-Width="35px"
                                FooterStyle-Width="35px">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Duration") %>' Width="20px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtDuration" runat="server" Text='<%# Bind("Duration") %>' Width="20px"></asp:TextBox>
                                    </div>
                                    <div style="float: left;">
                                        <asp:RequiredFieldValidator ID="reqDuration" runat="server" ControlToValidate="txtDuration"
                                            ErrorMessage="*" ValidationGroup="valUpdatePackageOption" Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <div style="float: left">
                                        <asp:TextBox runat="server" ID="txtDurationInsert" Text='<%# Bind("Duration") %>'
                                            Width="20px"></asp:TextBox>
                                    </div>
                                    <div style="float: left">
                                        <asp:RequiredFieldValidator ID="req1" ControlToValidate="txtDurationInsert" runat="server"
                                            ValidationGroup="valInsertPackageOption" ErrorMessage="*" Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Price" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# Eval("StandardPrice", "{0:c}") %>'
                                        Width="65px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtStandardPrice" runat="server" Text='<%# Bind("StandardPrice", "{0:0.00}") %>'
                                            Width="65px"></asp:TextBox>
                                    </div>
                                    <div style="float: left;">
                                        <asp:RequiredFieldValidator ID="reqStandardPrice" runat="server" ControlToValidate="txtStandardPrice"
                                            ErrorMessage="*" ValidationGroup="valUpdatePackageOption" Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="regxStandardPrice" runat="server" ControlToValidate="txtStandardPrice"
                                            ErrorMessage="*" ValidationExpression="[0-9]+(\.[0-9][0-9]?)?" Display="Dynamic"
                                            ValidationGroup="valUpdatePackageOption">
                                        </asp:RegularExpressionValidator>
                                    </div>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtStandardPriceInsert" runat="server" Text='<%# Bind("StandardPrice") %>'
                                            Width="65px"></asp:TextBox>
                                    </div>
                                    <div style="float: left;">
                                        <asp:RequiredFieldValidator ID="reqStandardPriceInsert" runat="server" ControlToValidate="txtStandardPriceInsert"
                                            ErrorMessage="*" ValidationGroup="valInsertPackageOption" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="regxStandardPrice" runat="server" ControlToValidate="txtStandardPriceInsert"
                                            ErrorMessage="*" ValidationExpression="[0-9]+(\.[0-9][0-9]?)?" Display="Dynamic"
                                            ValidationGroup="valInsertPackageOption">
                                        </asp:RegularExpressionValidator>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Promo Code" HeaderStyle-Width="85px">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("PromoCode") %>' Width="85px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPromoCode" runat="server" Text='<%# Bind("PromoCode") %>' Width="85px"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <div style="float: left;">
                                        <asp:TextBox runat="server" ID="txtPromoCodeInsert" Text='<%# Bind("PromoCode") %>'
                                            Width="85px"></asp:TextBox>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="%" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:Label ID="lblDiscountPercentage" runat="server" Width="20px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtDiscountPercentage" runat="server" Text='<%# Bind("DiscountPercentage") %>'
                                            Width="20px"></asp:TextBox>
                                    </div>
                                    <div style="float: left;">
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDiscountPercentage"
                                            ErrorMessage="*" ValidationExpression="[0-9]+(\.[0-9][0-9]?)?" Display="Dynamic"
                                            ValidationGroup="valUpdatePackageOption">
                                        </asp:RegularExpressionValidator>
                                    </div>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <div style="float: left;">
                                        <asp:TextBox runat="server" ID="txtDiscountPercentageInsert"
                                            Text='<%# Bind("DiscountPercentage") %>' Width="20px"></asp:TextBox>
                                    </div>
                                    <div style="float: left;">
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDiscountPercentageInsert"
                                            ErrorMessage="*" ValidationExpression="[0-9]+(\.[0-9][0-9]?)?" Display="Dynamic"
                                            ValidationGroup="valInsertPackageOption">
                                        </asp:RegularExpressionValidator>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PC StartDate" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("PromoCodeStartDate","{0:d}") %>'
                                        Width="80px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtPromoCodeStartDate" runat="server" Text='<%# Bind("PromoCodeStartDate","{0:d}") %>'
                                            Width="80px"></asp:TextBox>
                                    </div>
                                    <div style="float: left;">
                                        <asp:MaskedEditExtender ID="mexPromoCodeStartDate" runat="server" AutoComplete="true"
                                            TargetControlID="txtPromoCodeStartDate" MaskType="Date" Mask="99/99/9999" CultureName="en-AU">
                                        </asp:MaskedEditExtender>
                                        <asp:MaskedEditValidator ID="mxvPromoCodeStartDate" runat="server" ControlExtender="mexPromoCodeStartDate" CssClass="errorValidator"
                                            ControlToValidate="txtPromoCodeStartDate" IsValidEmpty="true" ErrorMessage="Invalid Date Format"
                                            InvalidValueMessage="Invalid Date Format (dd/mm/yyyy)" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$"
                                            Display="Dynamic" ValidationGroup="valUpdatePackageOption"></asp:MaskedEditValidator>
                                    </div>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <div style="float: left">
                                        <asp:TextBox runat="server" ID="txtPromoCodeStartDateInsert" Text='<%# Bind("PromoCodeStartDate") %>'
                                            Width="80px"></asp:TextBox>
                                    </div>
                                    <div style="float: left">
                                        <asp:MaskedEditExtender ID="mexPromoCodeStartDateInsert" runat="server" AutoComplete="true"
                                            TargetControlID="txtPromoCodeStartDateInsert" MaskType="Date" Mask="99/99/9999"
                                            CultureName="en-AU">
                                        </asp:MaskedEditExtender>
                                        <asp:MaskedEditValidator ID="mxvPromoCodeStartDateInsert" runat="server" ControlExtender="mexPromoCodeStartDateInsert"
                                            ControlToValidate="txtPromoCodeStartDateInsert" IsValidEmpty="true" ErrorMessage="Invalid Date Format" CssClass="errorValidator"
                                            InvalidValueMessage="Invalid Date Format (dd/mm/yyyy)" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$"
                                            Display="Dynamic" ValidationGroup="valInsertPackageOption"></asp:MaskedEditValidator>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PC EndDate" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("PromoCodeEndDate", "{0:d}") %>'
                                        Width="80px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPromoCodeEndDate" runat="server" Text='<%# Bind("PromoCodeEndDate", "{0:d}") %>'
                                        Width="80px"></asp:TextBox>
                                    <div style="float: left;">
                                        <asp:MaskedEditExtender ID="mexPromoCodeEndDate" runat="server" AutoComplete="true"
                                            TargetControlID="txtPromoCodeEndDate" MaskType="Date" Mask="99/99/9999" CultureName="en-AU">
                                        </asp:MaskedEditExtender>
                                        <asp:MaskedEditValidator ID="mxvPromoCodeEndDate" runat="server" ControlExtender="mexPromoCodeEndDate"
                                            ControlToValidate="txtPromoCodeEndDate" IsValidEmpty="true" ErrorMessage="Invalid Date Format" CssClass="errorValidator"
                                            InvalidValueMessage="Invalid Date Format (dd/mm/yyyy)" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$"
                                            Display="Dynamic" ValidationGroup="valUpdatePackageOption"></asp:MaskedEditValidator>
                                    </div>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <div style="float: left">
                                        <asp:TextBox runat="server" ID="txtPromoCodeEndDateInsert" Text='<%# Bind("PromoCodeEndDate") %>'
                                            Width="80px"></asp:TextBox>
                                    </div>
                                    <div style="float: left">
                                        <asp:MaskedEditExtender ID="mexPromoCodeEndDateInsert" runat="server" AutoComplete="true"
                                            TargetControlID="txtPromoCodeEndDateInsert" MaskType="Date" Mask="99/99/9999"
                                            CultureName="en-AU">
                                        </asp:MaskedEditExtender>
                                        <asp:MaskedEditValidator ID="mxvPromoCodeEndDateInsert" runat="server" ControlExtender="mexPromoCodeEndDateInsert"
                                            ControlToValidate="txtPromoCodeEndDateInsert" IsValidEmpty="true" ErrorMessage="Invalid Date Format" CssClass="errorValidator"
                                            InvalidValueMessage="Invalid Date Format (dd/mm/yyyy)" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$"
                                            Display="Dynamic" ValidationGroup="valInsertPackageOption"></asp:MaskedEditValidator>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="5px">
                                <HeaderTemplate>
                                    <asp:Image ID="imgIsActive" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Active.png"
                                        ToolTip="(In)Active" AlternateText="(In)Active" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkPublished" runat="server" Checked='<%# Bind("Published") %>'
                                        Enabled="false" Width="8px" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkPublishedEdit" runat="server" Checked='<%# Bind("Published") %>'
                                        Width="5px" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkPublishedInsert" runat="server" Checked='<%# Bind("Published") %>'
                                        Width="5px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="35px">
                                <EditItemTemplate>
                                    <asp:ImageButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                                        ValidationGroup="valUpdatePackageOption" ImageUrl="~/App_Themes/SleekTheme/Images/Update.png">
                                    </asp:ImageButton>
                                    <asp:ImageButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                        ImageUrl="~/App_Themes/SleekTheme/Images/Cancel.png"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Button ID="LinkButton3" runat="server" CommandName="AddNew"
                                        ValidationGroup="valInsertPackageOption" CssClass="buttonDirect" Text="Add"></asp:Button>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="LinkButton4" runat="server" CausesValidation="False" CommandName="Edit"
                                        ImageUrl="~/App_Themes/SleekTheme/Images/Edit.png"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Image" SelectImageUrl="~/App_Themes/SleekTheme/Images/Select.png"
                                ShowSelectButton="true" HeaderStyle-Width="5px" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div style="margin-top: 20px;">
                    <asp:RadioButtonList ID="rdlPublished" runat="server" CssClass="radioButtonList"
                        RepeatLayout="UnorderedList" AutoPostBack="true" OnSelectedIndexChanged="rdlPublished_SelectedIndexChanged">
                        <asp:ListItem Text="All" Value="All" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Published" Value="PUBLISHED"></asp:ListItem>
                        <asp:ListItem Text="UnPublished" Value="UNPUBLISHED"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks" runat="server" id="dvRightLinks">
                <asp:HyperLink ID="hlDisplayMedia" runat="server" Text="Add New Option" NavigateUrl="~/Admin/SuperAdmin/NewEditPackageOption.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
