<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserLogin.ascx.cs" Inherits="SleekSurf.Web.WebPageControls.UserLogin" %>
<asp:LoginView ID="LoginView1" runat="server" EnableViewState="false">
    <AnonymousTemplate>
        <div style="float: left; margin-right: 15px;">
            <asp:Literal ID="ltrGuest" runat="server" Text="Welcome, <strong>Guest</strong>"></asp:Literal>
        </div>
        <div style="float: left; margin-right: 5px;">
            <a href="../WebPages/NewAccount.aspx">Sign up</a>
        </div>
        <div style="float: left; display: block">
            <asp:LinkButton ID="lnkLogin" runat="server" CssClass="signin" CausesValidation="False"
                EnableViewState="False" ViewStateMode="Disabled"><span>Log In</span></asp:LinkButton>
            <fieldset id="signin_menu">
                <!--  -->
                <asp:Login ID="Login" runat="server" FailureAction="RedirectToLoginPage">
                    <LayoutTemplate>
                        <asp:Panel ID="pnLogin" runat="server" ClientIDMode="Static" DefaultButton="Submit">
                            <div>
                                <asp:Label runat="server" ID="lblUserName" AssociatedControlID="UserName" Text="Username:"
                                    meta:resourcekey="lblUserNameResource1" />
                                <span style="margin-right: 5px;">
                                    <asp:TextBox ID="UserName" runat="server" meta:resourcekey="UserNameResource2" />
                                </span>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                    ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login">*</asp:RequiredFieldValidator>
                            </div>
                            <div>
                                <asp:Label runat="server" ID="lblPassword" AssociatedControlID="Password" Text="Password:"
                                    meta:resourcekey="lblPasswordResource1" />
                                <span style="margin-right: 5px;">
                                    <asp:TextBox ID="Password" runat="server" TextMode="Password" meta:resourcekey="PasswordResource2" />
                                </span>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                    ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login">*</asp:RequiredFieldValidator>
                            </div>
                            <div style="border-bottom: 1px solid #000000; padding-bottom: 5px;">
                                <asp:HyperLink ID="lnkPasswordRecovery" runat="server" NavigateUrl="~/PasswordRecovery.aspx"
                                    meta:resourcekey="lnkPasswordRecoveryResource1" ForeColor="Black">Forgot Password?</asp:HyperLink>
                                <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me" meta:resourcekey="RememberMeResource1"
                                    Visible="false"></asp:CheckBox>
                                <asp:Button ID="Submit" runat="server" CommandName="Login" Text="Login" SkinID="Button"
                                    ValidationGroup="Login" meta:resourcekey="SubmitResource1" />
                            </div>
                        </asp:Panel>
                    </LayoutTemplate>
                </asp:Login>
            </fieldset>
        </div>
    </AnonymousTemplate>
    <LoggedInTemplate>
        <div class="statusPanelFrontEnd">
            <asp:LoginName ID="LoginName1" runat="server" Font-Bold="true" FormatString="Welcome  {0}" />
            <span>
                <asp:HyperLink ID="hyChangePassword" Font-Bold="true" runat="server" Text="Change Password"
                    NavigateUrl="~/WebPages/ChangePassword.aspx"></asp:HyperLink>
            </span><span>
                <asp:HyperLink ID="hyViewProfile" Font-Bold="true" runat="server" Text="View Profile"
                    NavigateUrl="~/WebPages/ViewProfile.aspx"></asp:HyperLink>
            </span><span style="border: none">
                <asp:LoginStatus ID="LoginStatus1" runat="server" LogoutAction="Redirect" LogoutText="Log Out"
                    LogoutPageUrl="~/" Font-Bold="True" OnLoggingOut="LoginStatusLS_LoggingOut" />
            </span>
        </div>
    </LoggedInTemplate>
</asp:LoginView>
