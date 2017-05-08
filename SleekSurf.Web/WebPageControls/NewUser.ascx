<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewUser.ascx.cs" Inherits="SleekSurf.Web.WebPageControls.NewUser" %>

<div id="SignUpContent">
    <fieldset>
        <legend>Login Details</legend>
        <asp:UpdatePanel ID="upnlUserName" runat="server" RenderMode="Inline">
            <ContentTemplate>
                <div>
                    <label>
                        <span class="mandatory">*</span> Username:</label>
                    <asp:TextBox ID="txtUserName" runat="server" OnTextChanged="txtUserName_TextChanged"
                        AutoPostBack="true"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqUserName" runat="server" ErrorMessage="*" ControlToValidate="txtUserName"
                        Display="Dynamic" ValidationGroup="valgRegistration">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regxUserName" ErrorMessage="* Can't have a space"
                            ControlToValidate="txtUserName" runat="server" Display="Dynamic" ValidationExpression="\S*" ValidationGroup="valgRegistration" />
                    <asp:Label ID="lblErrorUserMsg" runat="server" SkinID="Error"></asp:Label>
                    <asp:CompareValidator ID="cmpUserName" runat="server" ControlToValidate="txtUserName"
                        Display="Dynamic" ValidationGroup="valgRegistration"></asp:CompareValidator>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <label>
                <span class="mandatory">*</span> Password:</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqPassword" runat="server" ErrorMessage="*" ControlToValidate="txtPassword"
                Display="Dynamic" ValidationGroup="valgRegistration">
            </asp:RequiredFieldValidator>
        </div>
        <div>
            <label>
                <span class="mandatory">*</span> Confirm Password:</label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
            <asp:CompareValidator ID="cmpPassword" runat="server" ErrorMessage="Password Mismatch!"
                ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" ValidationGroup="valgRegistration">
            </asp:CompareValidator>
        </div>
        <div>
            <asp:UpdatePanel ID="upnlPassword" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <label>
                        <span class="mandatory">*</span> Email:</label>
                    <asp:TextBox ID="txtEmail" runat="server" OnTextChanged="txtEmail_TextChanged" AutoPostBack="true"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqEmail" runat="server" ErrorMessage="*" ControlToValidate="txtEmail"
                        Display="Dynamic" ValidationGroup="valgRegistration">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="rexEmail" runat="server" Display="Dynamic" ControlToValidate="txtEmail"
                        ErrorMessage="Invalid Email Fromat" ValidationGroup="valgRegistration" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                    </asp:RegularExpressionValidator>
                    <asp:Label ID="lblErrorEmail" runat="server" SkinID="Error"></asp:Label>
                    <asp:CompareValidator ID="cmpCompareEmail" runat="server" ControlToValidate="txtEmail"
                        Display="Dynamic" ValidationGroup="valgRegistration"></asp:CompareValidator>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <label>
                <span class="mandatory">*</span> Secret Question:</label>
            <asp:TextBox ID="txtPasswordQuestion" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqSecretQuestion" runat="server" ErrorMessage="*"
                ControlToValidate="txtPasswordQuestion" Display="Dynamic" ValidationGroup="valgRegistration">
            </asp:RequiredFieldValidator>
        </div>
        <div>
            <label>
                <span class="mandatory">*</span> Secret Answer:</label>
            <asp:TextBox ID="txtPasswordAnswer" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqSecretAnswer" runat="server" ErrorMessage="*"
                ControlToValidate="txtPasswordAnswer" Display="Dynamic" ValidationGroup="valgRegistration">
            </asp:RequiredFieldValidator>
        </div>
        <div>
            <asp:Panel ID="pnlRole" runat="server" Visible="false" CssClass="adminOnly">
                <label>
                    <span class="mandatory">*</span> Role:</label>
                <asp:DropDownList ID="ddlRoles" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="reqRoles" runat="server" ErrorMessage="*" ControlToValidate="ddlRoles"
                    InitialValue="Select Role" Display="Dynamic" ValidationGroup="valgRegistration">
                </asp:RequiredFieldValidator>
            </asp:Panel>
        </div>
    </fieldset>
</div>