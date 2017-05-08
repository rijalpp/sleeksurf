<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PasswordRecovery.ascx.cs"
    Inherits="SleekSurf.Web.WebPageControls.PasswordRecovery" %>
<div style="padding: 0px 20px">
    <asp:PasswordRecovery ID="PasswordRecovery1" runat="server">
        <UserNameTemplate>
            <p style="margin: 0px; color: #606060;">
                Before we re-send your password to you by e-mail, you need to enter the information
                below to help identify your account.
            </p>
            <asp:Panel ID="pnStep1" runat="server" DefaultButton="SubmitButton">
                <div class="sectionsubtitle smallTitle">
                    <span>Step 1: Enter your username</span>
                </div>
                <div id="Step1" style="padding-top: 20px;">
                    <span>
                        <asp:Label runat="server" ID="lblUsername" AssociatedControlID="UserName" Text="Username:"
                            Width="115px" />
                    </span><span>
                        <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valRequireUserName" runat="server" ControlToValidate="UserName"
                            SetFocusOnError="true" Display="Static" ErrorMessage="Username is required."
                            ToolTip="Username is required." ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
                    </span><span>
                        <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" Text="Submit" ValidationGroup="PasswordRecovery1"
                            SkinID="Button" />
                    </span>
                </div>
                <div style="padding-top: 20px;">
                    <asp:Label ID="FailureText" runat="server" SkinID="Error" EnableViewState="False" />
                </div>
                <div class="smallTitle">
                </div>
            </asp:Panel>
        </UserNameTemplate>
        <QuestionTemplate>
            <p style="margin: 0px; color: #606060;">
                Before we re-send your password to you by e-mail, you need to enter the information
                below to help identify your account.
            </p>
            <asp:Panel ID="pnStep2" runat="server" DefaultButton="SubmitButton">
                <div class="sectionsubtitle smallTitle">
                    <span>Step 2: Answer the following question</span>
                </div>
                <div style="padding-top: 20px; display: inline-block;">
                    <div style="display: block; margin-bottom: 5px;">
                        <label style="width: 115px">
                            Username:
                        </label>
                        <asp:Literal ID="UserName" runat="server"></asp:Literal>
                    </div>
                    <div style="display: block; margin-bottom: 5px;">
                        <asp:Label runat="server" ID="lblQuestion" AssociatedControlID="Question" Text="Question:"
                            Width="115px" />
                        <asp:Literal ID="Question" runat="server"></asp:Literal>
                    </div>
                    <div style="display: block">
                        <span style="vertical-align: top;">
                            <asp:Label runat="server" ID="lblAnswer" AssociatedControlID="Answer" Text="Answer:"
                                Width="115px" /></span>
                        <asp:TextBox ID="Answer" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valRequireAnswer" runat="server" ControlToValidate="Answer"
                            SetFocusOnError="true" Display="Static" ErrorMessage="Answer is required." ToolTip="Answer is required."
                            ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
                        <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" Text="Submit" ValidationGroup="PasswordRecovery1"
                            SkinID="Button" />
                    </div>
                    <div style="padding-top: 10px;">
                        <asp:Label ID="FailureText" runat="server" SkinID="Error" EnableViewState="False" />
                    </div>
                    <div class="smallTitle">
                    </div>
                </div>
            </asp:Panel>
        </QuestionTemplate>
        <SuccessTemplate>
            <div id="SuccessMessage">
                <asp:Label runat="server" ID="lblSuccess" CssClass="successMsg" Text="Your Password has been sent out to your nominated e-mail." />
                <span style="margin-top: 15px; float:right;">
                    <asp:Button ID="btnLogin" Text="Login" runat="server" SkinID="Button" PostBackUrl="~/Login.aspx"/>
            </div>
        </SuccessTemplate>
        <MailDefinition BodyFileName="~/EmailTemplates/PasswordRecoveryMail.txt" From="accounts@sleeksurf.com"
            Subject="SleekSurf: your password" IsBodyHtml="true">
        </MailDefinition>
    </asp:PasswordRecovery>
</div>
