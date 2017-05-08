<%@ Page Title="" Language="C#" MasterPageFile="~/SleekSurf.Master" AutoEventWireup="true"
    CodeBehind="ViewProfile.aspx.cs" Inherits="SleekSurf.Web.WebPages.ViewProfile" %>

<%@ Register Src="~/WebPageControls/NewEditUserProfile.ascx" TagName="EditProfile"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapContent">
        <div style="display: inline-block">
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            <br />
            <div id="AccountDetails" style="float: left;">
                <fieldset>
                    <legend>Account Details</legend>
                    <asp:FormView ID="fvUserAccount" runat="server" Width="100%" DataKeyNames="ProviderUserKey, UserName"
                        OnDataBound="fvUserAccount_DataBound" OnModeChanging="fvUserAccount_ModeChanging"
                        DefaultMode="ReadOnly" OnItemUpdating="fvUserAccount_ItemUpdating" OnItemUpdated="fvUserAccount_ItemUpdated">
                        <ItemTemplate>
                            <div>
                                <label>
                                    UserName:
                                </label>
                                <asp:Label ID="lblUserName" runat="server" Text='<%# bind("UserName") %>'></asp:Label>
                            </div>
                            <div>
                                <span style="width: 340px; display: inline-block;">
                                    <label>
                                        Role(s):</label>
                                    <asp:Label ID="Label3" runat="server" Text='<%# this.GetRolesForUser(Eval("UserName").ToString()) %>'></asp:Label>
                                </span><span style="margin-left: 10px">
                                    <label>
                                        Comment:</label>
                                    <asp:Label ID="Label1" runat="server" Text='<%# bind("Comment") %>'></asp:Label>
                                </span>
                            </div>
                            <asp:Panel ID="pnlRestrictedItems" runat="server" Visible="false" CssClass="adminOnly">
                                <div>
                                    <span style="width: 340px; display: inline-block;">
                                        <label>
                                            Active:
                                        </label>
                                        <asp:CheckBox ID="chkIsApproved" runat="server" Checked='<%# Eval("IsApproved") %>'
                                            Enabled="false" />
                                    </span><span style="margin-left: 10px">
                                        <label>
                                            Locked Out:</label>
                                        <asp:CheckBox ID="chkIsLockedOut" runat="server" Checked='<%# Eval("IsLockedOut") %>'
                                            Enabled="false" />
                                    </span>
                                </div>
                            </asp:Panel>
                            <div>
                                <label>
                                    Email:
                                </label>
                                <asp:Label ID="txtEmail" runat="server" Text='<%# bind("Email") %>'></asp:Label>
                            </div>
                            <div style="padding: 0px 20px 5px; width: 660px; border-bottom: 1px solid #CCC; margin: 0px auto;
                                text-align: right">
                                <asp:ImageButton ID="lbtnEditAccount" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Edit.png"
                                    AlternateText="Edit" ToolTip="Edit" CommandName="Edit"></asp:ImageButton>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div>
                                <label>
                                    UserName:</label>
                                <asp:Label ID="lblUserName" runat="server" Text='<%# bind("UserName") %>'></asp:Label>
                            </div>
                            <div>
                                <span style="width: 340px; display: inline-block;">
                                    <label>
                                        Role(s):</label>
                                    <asp:Label ID="Label3" runat="server" Text='<%# this.GetRolesForUser(Eval("UserName").ToString()) %>'></asp:Label>
                                </span><span style="margin-left: 10px">
                                    <label>
                                        Comment:</label>
                                    <asp:Label ID="Label2" runat="server" Text='<%# bind("Comment") %>'></asp:Label>
                                </span>
                            </div>
                            <asp:Panel ID="pnlRestrictedItems" runat="server" Visible="false" CssClass="adminOnly">
                                <div>
                                    <span style="width: 340px; display: inline-block;">
                                        <label>
                                            Active:</label>
                                        <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%# Eval("IsApproved") %>' />
                                    </span><span style="margin-left: 10px">
                                        <label>
                                            Locked Out:</label>
                                        <asp:CheckBox ID="chkIsLockedOut" runat="server" Checked='<%# Eval("IsLockedOut") %>' />
                                    </span>
                                </div>
                                <div style="display: inline-block">
                                    <label style="vertical-align: top;">
                                        Roles:</label>
                                    <asp:CheckBoxList ID="chkRoles" runat="server" RepeatLayout="Flow" CssClass="displayBlock">
                                    </asp:CheckBoxList>
                                </div>
                            </asp:Panel>
                            <span>
                                <asp:UpdatePanel ID="upnlEditAccount" runat="server" RenderMode="Inline">
                                    <ContentTemplate>
                                    <div>
                                        <label>
                                            Email:</label>
                                        <asp:TextBox ID="txtEditEmail" runat="server" Text='<%# bind("Email") %>' OnTextChanged="txtEditEmail_TextChanged"
                                            AutoPostBack="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqEmail" runat="server" ErrorMessage="Email Required"
                                            ControlToValidate="txtEditEmail" Display="Dynamic" ValidationGroup="valAccountEdit">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="rexEmail" runat="server" Display="Dynamic" ControlToValidate="txtEditEmail"
                                            ErrorMessage="Invalid Email Fromat" ValidationGroup="valAccountEdit" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                        </asp:RegularExpressionValidator>
                                        <asp:Label ID="lblErrorEmail" runat="server"></asp:Label>
                                        <asp:CompareValidator ID="cmpCompareEmail" runat="server" ControlToValidate="txtEditEmail"
                                            ErrorMessage="*" ValueToCompare='<%# bind("Email") %>' Display="Dynamic" ValidationGroup="valAccountEdit"></asp:CompareValidator>
                                    </div>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </span>
                            <div style="padding: 0px 20px 5px; width: 660px; border-bottom: 1px solid #CCC; margin: 0px auto;
                                text-align: right">
                                <asp:ImageButton ID="lbtnEditAccount" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Update.png"
                                    AlternateText="Update" ToolTip="Update" CommandName="Update" ValidationGroup="valAccountEdit">
                                </asp:ImageButton>
                                &nbsp;&nbsp;
                                <asp:ImageButton ID="lbtnCancelUpdateAccount" runat="server" ImageUrl="~/App_Themes/SleekTheme/Images/Cancel.png"
                                    AlternateText="Cancel" ToolTip="Cancel" CommandName="Cancel"></asp:ImageButton>
                            </div>
                        </EditItemTemplate>
                    </asp:FormView>
                    <asp:UpdatePanel runat="server" ID="upAttachSupportingImage">
                        <ContentTemplate>
                        <asp:Panel ID="pHeader" runat="server" CssClass="adminOnly">
                            <div class="adminOnly cpHeaderWithoutBorder">
                                <asp:Label ID="lblOpen" runat="server" ClientIDMode="Static" />
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pBody" runat="server" CssClass="adminOnly">
                            <div class="adminOnly cpBodyWithoutBorder">
                                <span style="width: 340px; display: inline-block;">
                                    <label>
                                        Secret Question:</label>
                                    <asp:TextBox ID="txtSecretQuestion" runat="server"></asp:TextBox>
                                </span><span style="margin-left: 10px">
                                    <label>
                                        Secret Answer:</label>
                                    <asp:TextBox ID="txtSecretAnswer" runat="server"></asp:TextBox>
                                </span>
                            </div>
                            <div style="padding: 10px 20px 5px; width: 660px; border-bottom: 1px solid #CCC;
                                margin: 0px auto; text-align: right">
                                <asp:ImageButton ID="btnSaveQAndA" runat="server" AlternateText="Save" ToolTip="Save"
                                    ImageUrl="~/App_Themes/SleekTheme/Images/Save.png" OnClick="btnSaveQAndA_Click" />
                            </div>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pBody"
                            CollapseControlID="lblOpen" ExpandControlID="lblOpen" Collapsed="true" TextLabelID="lblOpen"
                            CollapsedText="Reset Secret Question & Answer" ExpandedText="[Hide]" CollapsedSize="0">
                        </asp:CollapsiblePanelExtender>
                    </ContentTemplate>
                        <triggers>
                        <asp:PostBackTrigger ControlID="btnSaveQAndA" />
                    </triggers>
                    </asp:UpdatePanel>
                </fieldset>
            </div>
            <div style="float: left;">
                <uc1:EditProfile ID="ucUserProfile" runat="server" />
            </div>
            <div class="commandControl">
                <div style="float: left; margin-right: 20px; padding-left: 15px;">
                    <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                </div>
                <div style="float: right;">
                    <span style="margin: 0px 15px;">
                        <asp:Button ID="btnUpdateProfile" runat="server" Text="Update" OnClick="btnUpdateProfile_Click"
                            SkinID="Button" ValidationGroup="valgRegistrations" /></span> <span style="margin: 0px;"
                                class="button">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                    OnClick="btnCancel_Click" SkinID="Button" />
                            </span>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
