<%@ Page Title="" Language="C#" MasterPageFile="~/SleekSurf.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SleekSurf.Web.WebPages.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div id="Wrapper" class="adminWrapperSettings" style="background-color: #EDEFFF;">
        <div id="LoginContentWithSignUp">
            <asp:Login ID="LgnLogin" runat="server" DestinationPageUrl="~/WebPages/BrowsePackages.aspx"
                Width="100%">
                <LayoutTemplate>
                    <div id="LoginWithSignUp">
                        <span class="joinNow">Not a member yet? <span class="joinNowColor"><a href="NewAccount.aspx">
                            Join Now</a></span></span>
                        <asp:Panel ID="pnLogin" runat="server" DefaultButton="LoginButton">
                            <div>
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" SkinID="AdminLoginBox">Username:</asp:Label>
                                <asp:TextBox ID="UserName" runat="server" SkinID="AdminLoginBox"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                    ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="LgnLogin">*</asp:RequiredFieldValidator>
                            </div>
                            <div>
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" SkinID="AdminLoginBox">Password:</asp:Label>
                                <asp:TextBox ID="Password" runat="server" TextMode="Password" SkinID="AdminLoginBox"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                    ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="LgnLogin">*</asp:RequiredFieldValidator>
                            </div>
                            <div style="margin: 20px 0px; padding: 0px 20px;">
                                <span style="margin-right: 20px; display: inline-block; width: 155px;" class="forgetPassword">
                                    <a href="../PasswordRecovery.aspx" class="forgetPassword">Forgot Password</a> ?
                                    |</span> <span style="display: inline-block;">
                                        <asp:Button ID="LoginButton" runat="server" CommandName="Login" ValidationGroup="LgnLogin"
                                            Text="Login" SkinID="Button" />
                                    </span>
                            </div>
                            <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." Visible="false" />
                            <div style="color: Red; text-align: center; clear: both;">
                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                            </div>
                        </asp:Panel>
                    </div>
                </LayoutTemplate>
            </asp:Login>
        </div>
    </div>
</asp:Content>
