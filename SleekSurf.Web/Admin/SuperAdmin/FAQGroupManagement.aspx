<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="FAQGroupManagement.aspx.cs" Inherits="SleekSurf.Web.Admin.SuperAdmin.FAQGroupManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/JScript/site.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title">
                    FAQ Group Management Portal</div>
            </div>
            <asp:UpdatePanel ID="upnlFaqGroup" runat="server">
                <ContentTemplate>
                    <div>
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                        <!-- FAQ GROUP STARTS -->
                        <div style="margin: 10px 0px;">
                            <asp:GridView ID="gvFaqGroupManagement" runat="server" DataKeyNames="FaqGroupID, Client"
                                OnRowCommand="gvFaqGroupManagement_RowCommand" AutoGenerateColumns="false" ShowFooter="true"
                                OnRowUpdating="gvFaqGroupManagement_RowUpdating" OnRowEditing="gvFaqGroupManagement_RowEditing"
                                OnRowDataBound="gvFaqGroupManagement_RowDataBound" OnRowCancelingEdit="gvFaqGroupManagement_RowCancellingEdit"
                                OnSelectedIndexChanging="gvFaqGroupManagement_SelectedIndexChanging">
                                <EmptyDataTemplate>
                                    No Faq Group found for the Client
                                </EmptyDataTemplate>
                                <EmptyDataRowStyle CssClass="normalMsg" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input id="checkAll" class="styled" type="checkbox" onclick="javascript:SelectOrUnselectAll('<%= this.gvFaqGroupManagement.ClientID %>',this,'cbDelete','imgDeleteBtn')">
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
                                    <asp:TemplateField HeaderText="Group Name">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("GroupName") %>' Width="405px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtGroupName" runat="server" Text='<%# Bind("GroupName") %>' Width="405px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqGroupName" ControlToValidate="txtGroupName" runat="server"
                                                ErrorMessage="*" ValidationGroup="valgFaqGroup"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtGroupNameInsert" runat="server" Text='<%# Bind("GroupName") %>'
                                                Width="405px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqGroupNameInsert" ControlToValidate="txtGroupNameInsert"
                                                runat="server" ErrorMessage="*" ValidationGroup="valgFaqGroupInsert"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <HeaderStyle Width="405px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rank">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRank" runat="server" Text='<%# Eval("GroupRank") %>' Width="40px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlGroupRank" runat="server" SkinID="RankBox">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlGroupRankInsert" runat="server" SkinID="RankBox">
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <HeaderStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="LinkButton4" runat="server" CausesValidation="False" CommandName="Edit"
                                                ImageUrl="~/App_Themes/SleekTheme/Images/Edit.png"></asp:ImageButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                                                ValidationGroup="valgFaqGroup" ImageUrl="~/App_Themes/SleekTheme/Images/Update.png">
                                            </asp:ImageButton>
                                            <asp:ImageButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                                ImageUrl="~/App_Themes/SleekTheme/Images/Cancel.png"></asp:ImageButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button ID="LinkButton3" runat="server" CommandName="AddNew"
                                                ValidationGroup="valgFaqGroupInsert" Text="Add" SkinID="Button"></asp:Button>
                                        </FooterTemplate>
                                        <HeaderStyle Width="122px" />
                                    </asp:TemplateField>
                                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/App_Themes/SleekTheme/Images/Select.png"
                                        ShowSelectButton="true" HeaderStyle-Width="5px" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <!-- FAQ GROUP ENDS -->
                        <div style="margin: 30px 0px 20px;">
                            <asp:Panel ID="pnlFaqs" runat="server">
                                <div class="titleSearch">
                                    <div class="title">
                                        FAQ Management Portal</div>
                                </div>
                                <asp:Label ID="lblMessageFaq" runat="server" EnableViewState="false"></asp:Label>
                                <asp:GridView ID="gvFaqManagement" runat="server" DataKeyNames="FaqID, FaqGroup"
                                    OnRowCommand="gvFaqManagement_RowCommand" AutoGenerateColumns="false" ShowFooter="true"
                                    OnSelectedIndexChanging="gvFaqManagement_SelectedIndexChanging" OnRowDataBound="gvFaqManagement_RowDataBound">
                                    <EmptyDataTemplate>
                                        No Faq found for the selected Faq Group. Please add the Frequentrly asked question and answer
                                        below.
                                    </EmptyDataTemplate>
                                    <EmptyDataRowStyle CssClass="normalMsg" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <input id="checkAll" class="styled" type="checkbox" onclick="javascript:SelectOrUnselectAll('<%= this.gvFaqManagement.ClientID %>',this,'cbDeleteFaq','imgDeleteBtnFaq')">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbDeleteFaq" runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="imgDeleteBtnFaq" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Trash.png"
                                                    OnClick="imgDeleteBtnFaq_Click" ToolTip="Delete Selected Items" AlternateText="Delete" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Question">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuestion" runat="server" Text='<%# this.GetShortDescription(Eval("Question").ToString())%>'
                                                    Width="200px"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="200px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Answer">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAnswer" runat="server" Text='<%# this.GetShortDescription(Eval("Answer").ToString())%>'
                                                    Width="300px"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="300px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rank">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFaqRank" runat="server" Text='<%# Eval("FaqRank") %>' Width="20px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Button ID="LinkButton3" runat="server" CommandName="AddNew"
                                                    Text="Add" SkinID="Button"></asp:Button>
                                            </FooterTemplate>
                                            <HeaderStyle Width="122px" />
                                        </asp:TemplateField>
                                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/App_Themes/SleekTheme/Images/Select.png"
                                            ShowSelectButton="true" HeaderStyle-Width="5px" />
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </div>
                        <div>
                            <asp:Panel ID="pnlFaqAddEdit" runat="server" Visible="false">
                                <div id="PromotionDetails" style="margin: 0px; width: 800px;">
                                    <fieldset>
                                        <legend>FAQ Details</legend>
                                        <div class="div">
                                            <label style="vertical-align: top;">
                                                <span class="mandatory">*</span> Faq Question:</label>
                                            <asp:TextBox ID="txtFaqQuestion" runat="server" Width="620px" TextMode="MultiLine"
                                                Rows="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqtxtFaqQuestion" runat="server" ErrorMessage="*"
                                                ControlToValidate="txtFaqQuestion" Display="Dynamic" ValidationGroup="valgFaq">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                        <div class="div" style="display: table">
                                            <div class="cell">
                                                <label>
                                                    <span class="mandatory">*</span> Answer:</label>
                                            </div>
                                            <div class="cell" style="padding-right: 5px;">
                                                <cc1:Editor ID="editorAnswer" runat="server" Width="640px" />
                                            </div>
                                            <div class="cell">
                                                <asp:RequiredFieldValidator ID="reqDescription" runat="server" ControlToValidate="editorAnswer"
                                                    ValidationGroup="valgFaq" ErrorMessage="*" Display="Dynamic">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="div">
                                            <label>
                                                <span class="mandatory">*</span> Faq Rank:</label>
                                            <asp:DropDownList ID="ddlRankFaq" runat="server" SkinID="RankBox">
                                            </asp:DropDownList>
                                        </div>
                                    </fieldset>
                                    <div id="SignUpNavigation" style="width: 800px;">
                                        <div style="float: left; margin-right: 20px; padding-left: 15px;">
                                            <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                                        </div>
                                        <div style="float: right;">
                                            <span style="margin: 0px;" class="button">
                                                <asp:Button ID="btnHideFaq" runat="server" Text="Hide" OnClick="btnHideFaq_Click"
                                                    SkinID="Button" /></span> <span style="margin: 0px 5px">
                                                        <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="valgFaq" OnClick="btnSave_Click"
                                                            SkinID="Button" /></span>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks">
            </div>
        </div>
    </div>
</asp:Content>
