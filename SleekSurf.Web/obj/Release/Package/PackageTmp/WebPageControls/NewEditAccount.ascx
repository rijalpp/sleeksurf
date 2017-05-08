<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewEditAccount.ascx.cs"
    Inherits="SleekSurf.Web.WebPageControls.NewEditAccount" %>
<%@ Register Src="~/WebPageControls/NewUser.ascx" TagName="NewUser" TagPrefix="uc" %>
<%@ Register Src="~/WebPageControls/NewEditClient.ascx" TagName="Client" TagPrefix="uc" %>
<%@ Register Src="~/WebPageControls/NewEditUserProfile.ascx" TagName="Profile" TagPrefix="uc" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<div style="clear: both; margin: 5px 0">
    <asp:Label ID="lblResultMessage" runat="server" EnableViewState="false"></asp:Label>
</div>
<br />
<p id="paraStepTitle" runat="server">
                Please follow the simple steps provided below to register.</p>
<asp:Panel ID="pnStepNavigationBar" runat="server">
    <div id="StepNavigationBar">
        <asp:Image runat="server" ID="imgStepType" ImageUrl="~/App_Themes/SleekTheme/Images/Type.png"
            AlternateText="Registration Type" />
        <span id="Step2" runat="server">
            <asp:Image runat="server" ID="imgStepAccount" ImageUrl="~/App_Themes/SleekTheme/Images/Account.png"
                AlternateText="User Account" />
            <span id="Step3" runat="server">
                <asp:Image runat="server" ID="imgStepProfile" ImageUrl="~/App_Themes/SleekTheme/Images/UserProfile.png"
                    AlternateText="User Profile" />
                <span id="Step4" runat="server" style="left: 0px">
                    <asp:Image runat="server" ID="imgStepBusiness" ImageUrl="~/App_Themes/SleekTheme/Images/BusinessProfile.png"
                        AlternateText="Business Profile" />
                    <span id="Step5" runat="server">
                        <asp:Image runat="server" ID="imgStepSummary" ImageUrl="~/App_Themes/SleekTheme/Images/Summary.png"
                            AlternateText="Business Summary" />
                    </span></span></span></span>
    </div>
</asp:Panel>
<div style="display: inline-block; padding-bottom: 10px;">
    <asp:Panel ID="pnSignUp" runat="server" DefaultButton="btnNext">
        <asp:MultiView ID="mvClientDetails" runat="server" ActiveViewIndex="0" OnActiveViewChanged="mvClientDetails_ActiveViewChanged">
            <asp:View ID="vRegoTypeSelection" runat="server">
                <div id="SignUpContent">
                    <fieldset>
                        <legend>Registration Type</legend>
                        <div style="width: 400px; margin: 0px auto; height: 150px;">
                            <span class="loginTitle">Please Select Type of Registration</span>
                            <asp:RadioButtonList ID="rblRegoType" runat="server" RepeatDirection="Vertical" CssClass="regoTypeRadioButton">
                                <asp:ListItem Text="  &nbsp; Personal" Value="Personal"></asp:ListItem>
                                <asp:ListItem Text="&nbsp; Business" Value="Business"></asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="reRegoType" runat="server" ControlToValidate="rblRegoType"
                                displayLabel="Dynamic" ErrorMessage="Select your registration type!" ValidationGroup="valgRegistration">  
                            </asp:RequiredFieldValidator>
                        </div>
                    </fieldset>
                </div>
            </asp:View>
            <asp:View ID="vLoginDetails" runat="server">
                <uc:NewUser ID="NewUser" runat="server" />
            </asp:View>
            <asp:View ID="vProfileDetails" runat="server">
                <uc:Profile ID="UserProfile1" runat="server" />
            </asp:View>
            <asp:View ID="vClientDetails" runat="server">
                <uc:Client ID="ClientDetails1" runat="server" />
            </asp:View>
            <asp:View ID="vSummary" runat="server">
                <div id="SignUpSummary">
                    <fieldset>
                        <legend>Registration Summary</legend>
                        <div class="signUpSummaryDiv">
                            <p style="margin: 0px;">
                                Please confirm the registration details below. Click Save to proceed or Previous
                                to modify details.</p>
                            <asp:Literal ID="ltrSummary" runat="server"></asp:Literal>
                            <p class="sectionSummaryHeader">
                                <asp:Label ID="Label17" runat="server" Font-Bold="true" Text="MEMBERSHIP DETAILS"
                                    ForeColor="#5C9FFF"></asp:Label>
                            </p>
                            <span class="sectionSummary" style="margin: 0px;"><span style="display: block; padding: 2px 0px;">
                                <asp:Label ID="Label24" runat="server" Text="Username: " SkinID="Confirmation"></asp:Label><asp:Label
                                    ID="lblUsername" runat="server" SkinID="DisplayLabel"></asp:Label>
                            </span><span style="display: block; padding: 2px 0px;">
                                <asp:Label ID="Label1" runat="server" Text="Email: " SkinID="Confirmation"></asp:Label><asp:Label
                                    ID="lblEmail" runat="server" SkinID="DisplayLabel"></asp:Label>
                            </span><span style="display: block; padding: 2px 0px;">
                                <asp:Label ID="Label25" runat="server" Text="Secret Question: " SkinID="Confirmation"></asp:Label><asp:Label
                                    ID="lblSQuestion" runat="server" SkinID="DisplayLabel"></asp:Label>
                            </span><span style="display: block; padding: 2px 0px;">
                                <asp:Label ID="Label27" runat="server" Text="Secret Answer: " SkinID="Confirmation"></asp:Label><asp:Label
                                    ID="lblSAnswer" runat="server" SkinID="DisplayLabel"></asp:Label>
                            </span></span>
                            <p class="sectionSummaryHeader">
                                <asp:Label ID="lblPersonalDetails" runat="server" Font-Bold="true" Text="PROFILE DETAILS"
                                    ForeColor="#5C9FFF"></asp:Label>
                            </p>
                            <span class="sectionSummary" style="margin: 0px;"><span style="display: block; padding: 2px 0px;">
                                <asp:Label ID="Label18" runat="server" Text="Name: " SkinID="Confirmation"></asp:Label><asp:Label
                                    ID="lblName" runat="server" SkinID="DisplayLabel"></asp:Label></span> <span style="display: block;
                                        padding: 2px 0px;">
                                        <asp:Label ID="Label20" runat="server" Text="Gender: " SkinID="Confirmation"></asp:Label><asp:Label
                                            ID="lblGender" runat="server" SkinID="DisplayLabel"></asp:Label></span>
                                <span style="display: block; padding: 2px 0px;">
                                    <asp:Label ID="Label21" runat="server" Text="Address: " CssClass="topAlign" SkinID="Confirmation"></asp:Label><asp:Label
                                        ID="lblAddress" runat="server" SkinID="DisplayLabel"></asp:Label></span>
                            </span>
                            <asp:Panel ID="pnClientSummarySection" runat="server" Visible="false">
                                <p class="sectionSummaryHeader">
                                    <asp:Label ID="lblClientDetails" runat="server" Font-Bold="true" Text="CLIENT DETAILS"
                                        ForeColor="#5C9FFF"></asp:Label>
                                </p>
                                <span class="sectionSummary" style="margin: 0px;"><span style="display: block; padding: 2px 0px;">
                                    <asp:Label ID="Label3" runat="server" Text="ABN: " SkinID="Confirmation"></asp:Label><asp:Label
                                        ID="lblABN" runat="server" SkinID="DisplayLabel"></asp:Label></span> <span style="display: block;
                                            padding: 2px 0px;">
                                            <asp:Label ID="Label7" runat="server" Text="Business Type: " SkinID="Confirmation"></asp:Label><asp:Label
                                                ID="lblBusinessType" runat="server" SkinID="DisplayLabel"></asp:Label></span>
                                    <span style="display: block; padding: 2px 0px;">
                                        <asp:Label ID="Label2" runat="server" Text="Business Name: " SkinID="Confirmation"></asp:Label><asp:Label
                                            ID="lblBusinessName" runat="server" SkinID="DisplayLabel"></asp:Label></span>
                                    <span style="display: block; padding: 2px 0px;">
                                        <asp:Label ID="Label6" runat="server" Text="Contact Person: " SkinID="Confirmation"></asp:Label><asp:Label
                                            ID="lblContactPerson" runat="server" SkinID="DisplayLabel"></asp:Label></span>
                                    <span style="display: block; padding: 2px 0px;">
                                        <asp:Label ID="Label11" runat="server" Text="Website: " SkinID="Confirmation"></asp:Label><asp:Label
                                            ID="lblBusinessWebsite" runat="server" SkinID="DisplayLabel"></asp:Label></span>
                                    <span style="display: block; padding: 2px 0px;">
                                        <asp:Label ID="Label4" runat="server" Text="Email: " SkinID="Confirmation"></asp:Label><asp:Label
                                            ID="lblBusinessEmail" runat="server" SkinID="DisplayLabel"></asp:Label></span>
                                    <span style="display: block; padding: 2px 0px;">
                                        <asp:Label ID="Label13" runat="server" Text="Contact(Office): " SkinID="Confirmation"></asp:Label><asp:Label
                                            ID="lblContactOffice" runat="server" SkinID="DisplayLabel"></asp:Label></span>
                                    <span style="display: block; padding: 2px 0px;">
                                        <asp:Label ID="Label26" runat="server" Text="Address: " CssClass="topAlign" SkinID="Confirmation"></asp:Label><asp:Label
                                            ID="lblBusinessAddress" runat="server" SkinID="DisplayLabel"></asp:Label></span>
                                </span>
                            </asp:Panel>
                            <p class="sectionSummaryHeader">
                                <asp:Label ID="Label8" runat="server" Font-Bold="true" Text="ENTER CONFIRMATION CODE"
                                    ForeColor="#5C9FFF"></asp:Label>
                            </p>
                            <span><span style="display: inline-block; padding-top: 10px;">
                                <recaptcha:RecaptchaControl ID="recaptcha" runat="server" PublicKey="6LfNH8ESAAAAAOzhuW3CZ3XUMhJurJdqsY9MnUxs"
                                    PrivateKey="6LfNH8ESAAAAAAueDIxWJN0OmXxH23CtJqqK7MD9" Theme="white" />
                            </span><span style="display: inline-block; vertical-align: top; padding-top: 10px;
                                padding-left: 10px">
                                <asp:Label ID="lblCaptchaMessage" runat="server" SkinID="Error" EnableViewState="false"></asp:Label>
                            </span></span>
                        </div>
                    </fieldset>
                </div>
            </asp:View>
            <asp:View ID="vComplete" runat="server">
                <asp:Label ID="lblTransactionCompleteMessage" runat="server"></asp:Label>
            </asp:View>
        </asp:MultiView>
        <div id="SignUpNavigation">
            <asp:Panel ID="pnlNavigation" runat="server">
                <div style="float: left; margin-right: 20px; padding-left: 15px;">
                    <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                </div>
                <div style="float: right;">
                    <span style="margin: 0px;" class="button">
                        <asp:Button ID="btnPrevious" runat="server" OnClick="btnPrevious_Click" Text="Previous"
                            SkinID="Button" /></span> <span style="margin: 0px 5px" class="button">
                                <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next" SkinID="Button"
                                    ValidationGroup="valgRegistration" /></span> <span style="margin: 0px 5px" skinid="Button">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" SkinID="Button"
                                            ValidationGroup="valgRegistration" /></span>
                </div>
            </asp:Panel>
        </div>
    </asp:Panel>
</div>
