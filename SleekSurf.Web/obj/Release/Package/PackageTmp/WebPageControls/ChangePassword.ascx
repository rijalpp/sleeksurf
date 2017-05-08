<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.ascx.cs"
    Inherits="SleekSurf.Web.WebPageControls.ChangePassword" %>
<asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
<asp:ChangePassword ID="cpChangePassword" runat="server" OnChangePasswordError="cpChangePassword_ChangePasswordError">
    <ChangePasswordTemplate>
        <fieldset>
            <legend>Password Details</legend>
            <div id="ChangePassword">
                <div>
                    <label>
                        Current Password</label>
                    <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="CurrentPassword"
                        SetFocusOnError="true" Display="Dynamic" ErrorMessage="Password is required."
                        ToolTip="Password is required." ValidationGroup="valgChangePassword">
                    </asp:RequiredFieldValidator>
                </div>
                <div>
                    <label>
                        New Password</label>
                    <asp:TextBox ID="NewPassword" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="NewPassword"
                        SetFocusOnError="true" Display="Dynamic" ErrorMessage="Password is required."
                        ToolTip="Password is required." ValidationGroup="valgChangePassword"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="NewPassword"
                        SetFocusOnError="true" Display="Dynamic" ValidationExpression="\w{5,}" ErrorMessage="New Password must be at least five characters long"
                        ToolTip="New password must be at least 5 characters long" ValidationGroup="valgChangePassword"></asp:RegularExpressionValidator>
                </div>
                <div>
                    <label>
                        Confirm New Password</label>
                    <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ConfirmNewPassword"
                        SetFocusOnError="true" Display="Dynamic" ErrorMessage="Password is required."
                        ToolTip="Current password is required." ValidationGroup="valgChangePassword"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cmpConfirmNewPassword" runat="server" ControlToCompare="NewPassword"
                        ControlToValidate="ConfirmNewPassword" SetFocusOnError="true" Display="Dynamic"
                        ErrorMessage="New password mismatched." ValidationGroup="valgChangePassword"></asp:CompareValidator>
                </div>
                <div style="text-align:right; padding-top: 10px;">
                    <asp:Button ID="btnChangePassword" runat="server" Text="Change Pwd" CommandName="ChangePassword"
                        SkinID="Button" ValidationGroup="valgChangePassword" />
                </div>
            </div>
        </fieldset>
    </ChangePasswordTemplate>
    <SuccessTemplate>
        <asp:Label ID="lblSuccess" runat="server" Text="Your Password has been changed.<br /> The New Password Information has been sent out to your email."
            CssClass="successMsg"></asp:Label>
    </SuccessTemplate>
    <MailDefinition BodyFileName="~/EmailTemplates/ChangePasswordMail.txt" From="prem.rijal@premrijal.com"
        Subject="SleekSurf: Password changed." IsBodyHtml="true">
    </MailDefinition>
</asp:ChangePassword>
