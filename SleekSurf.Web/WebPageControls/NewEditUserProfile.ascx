<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewEditUserProfile.ascx.cs"
    Inherits="SleekSurf.Web.WebPageControls.NewEditUserProfile" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/WebPageControls/FileUploader.ascx" TagName="ucFileUploader" TagPrefix="uc" %>
<div id="SignUpContentDetails">
    <fieldset>
        <legend>Profile Details</legend><span>
            <asp:Panel ID="pnlProfileUrl" runat="server" Visible="false">
                <label>
                    Profile Url:</label>
                <uc:ucFileUploader ID="FileUploaderProfile" runat="server" />
            </asp:Panel>
            <div>
                <span style="width: 340px; display: inline-block;">
                    <label>
                        <span class="mandatory">*</span> Title:</label>
                    <asp:DropDownList ID="ddlTitle" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqTitle" runat="server" ErrorMessage="*" ControlToValidate="ddlTitle"
                        InitialValue="Select Title" Display="Dynamic" ValidationGroup="valgRegistration">
                    </asp:RequiredFieldValidator>
                </span><span style="margin-left: 10px">
                    <label>
                        <span class="mandatory">*</span> First Name:</label>
                    <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                        ControlToValidate="txtFirstName" Display="Dynamic" ValidationGroup="valgRegistration">
                    </asp:RequiredFieldValidator>
                </span>
            </div>
            <div>
                <span style="width: 340px; display: inline-block;">
                    <label>
                        Middle Name:</label>
                    <asp:TextBox ID="txtMiddleName" runat="server"></asp:TextBox>
                </span><span style="margin-left: 10px">
                    <label>
                        <span class="mandatory">*</span> Last Name:</label>
                    <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqLastName" runat="server" ErrorMessage="*" ControlToValidate="txtLastName"
                        Display="Dynamic" ValidationGroup="valgRegistration">
                    </asp:RequiredFieldValidator>
                </span>
            </div>
            <asp:Panel ID="pnlDateOfBirth" runat="server" Visible="false">
                <label>
                    Date of Birth:</label>
                <asp:TextBox ID="txtDOB" runat="server"></asp:TextBox>
                <asp:MaskedEditExtender ID="mexDOB" runat="server" AutoComplete="true" TargetControlID="txtDOB"
                    MaskType="Date" Mask="99/99/9999" CultureName="en-AU">
                </asp:MaskedEditExtender>
                <asp:MaskedEditValidator ID="mxvDOB" runat="server" ControlExtender="mexDOB" ControlToValidate="txtDOB" CssClass="errorValidator"
                    IsValidEmpty="true" ErrorMessage="Invalid Date Format" InvalidValueMessage="Invalid Date Format (dd/mm/yyyy)"
                    ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$"
                    Display="Dynamic" ValidationGroup="valgRegistration" Font-Size="12px"></asp:MaskedEditValidator>
            </asp:Panel>
            <div>
                <span>
                    <label>
                        <span class="mandatory">*</span> Gender:</label>
                    <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                        <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="reqGender" runat="server" ControlToValidate="rblGender"
                        Display="Dynamic" ErrorMessage="Select your gender." ValidationGroup="valgRegistration">  
                    </asp:RequiredFieldValidator>
                </span>
            </div>
            <div class="separator">
            </div>
        </span><span>
            <div>
                <label>
                    <span class="mandatory">*</span> Address Line 1:</label>
                <asp:TextBox ID="txtAddressLine1" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqAddressLine1" runat="server" ErrorMessage="*"
                    ControlToValidate="txtAddressLine1" Display="Dynamic" ValidationGroup="valgRegistration">
                </asp:RequiredFieldValidator>
            </div>
            <div>
                <span style="width: 340px; display: inline-block;">
                    <label>
                        Address Line 2 :</label>
                    <asp:TextBox ID="txtAddressLine2" runat="server"></asp:TextBox></span> <span style="margin-left: 10px">
                        <label>
                            Address Line 3 :</label>
                        <asp:TextBox ID="txtAddressLine3" runat="server"></asp:TextBox></span>
            </div>
            <div>
                <span style="width: 340px; display: inline-block;">
                    <label>
                        <span class="mandatory">*</span> City/Suburb:</label>
                    <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqCity" runat="server" ErrorMessage="*" ControlToValidate="txtCity"
                        Display="Dynamic" ValidationGroup="valgRegistration">
                    </asp:RequiredFieldValidator>
                </span><span style="margin-left: 10px">
                    <label>
                        <span class="mandatory">*</span> State:</label>
                    <asp:DropDownList ID="ddlState" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="*" Display="Dynamic"
                        ControlToValidate="ddlState" ValidationGroup="valgRegistration" InitialValue="Select State"
                        runat="server" />
                </span>
            </div>
            <div>
                <span style="width: 340px; display: inline-block;">
                    <label>
                        <span class="mandatory">*</span> PostCode:</label>
                    <asp:TextBox ID="txtPostcode" runat="server" MaxLength="4"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="fTxtEPostcode" runat="server" TargetControlID="txtPostcode"
                        FilterMode="ValidChars" FilterType="Numbers">
                    </asp:FilteredTextBoxExtender>
                    <asp:TextBoxWatermarkExtender ID="WaterMarkTxtPostCode" TargetControlID="txtPostcode"
                        runat="server" WatermarkCssClass="waterMark" WatermarkText="Four digits only">
                    </asp:TextBoxWatermarkExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                        ControlToValidate="txtPostcode" Display="Dynamic" ValidationGroup="valgRegistration"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ErrorMessage="*"
                        ControlToValidate="txtPostcode" runat="server" ValidationExpression="^\d{4}$"
                        Display="Dynamic" ValidationGroup="valgRegistration" />
                </span><span style="margin-left: 10px">
                    <label>
                        <span class="mandatory">*</span> Country:</label>
                    <asp:DropDownList ID="ddlCountry" runat="server" Enabled="false">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqCountry" runat="server" ErrorMessage="*" InitialValue="Select Country"
                        ControlToValidate="ddlCountry" Display="Dynamic" ValidationGroup="valgRegistration"></asp:RequiredFieldValidator></span>
            </div>
            <div class="separator" id="divSeparator" runat="server" visible="false">
            </div>
        </span><span id="spanProfileContact" runat="server" visible="false">
            <div>
                <label>
                    Contact Home:</label>
                <asp:TextBox ID="txtContactHome" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorContactOffice" runat="server"
                    ValidationGroup="valgRegistration" ErrorMessage="Invalid Phone Number must start with +61 or area code"
                    ControlToValidate="txtContactHome" Display="Dynamic" ValidationExpression="^(\+61|0)[2378]([0-9]){8}$">
                </asp:RegularExpressionValidator>
            </div>
            <div>
                <label>
                    Contact Mobile:</label>
                <asp:TextBox ID="txtContactMobile" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="valgRegistration"
                    ErrorMessage="Invalid Mobile Number must start with +61 or 0" ControlToValidate="txtContactMobile"
                    Display="Dynamic" ValidationExpression="^(\+61|0)[4]([0-9]){8}$">
                </asp:RegularExpressionValidator>
            </div>
            <div>
                <span style="width: 340px; display: inline-block;">
                    <label>
                        Occupation:</label>
                    <asp:DropDownList ID="ddlOccupation" runat="server">
                    </asp:DropDownList>
                </span><span style="margin-left: 10px">
                    <label>
                        Theme:</label>
                    <asp:DropDownList ID="ddlTheme" runat="server">
                    </asp:DropDownList>
                </span>
            </div>
            <div>
                <label>
                    Personal Website:</label>
                <asp:TextBox ID="txtWebsiteUrl" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorWebsite" runat="server"
                    ValidationGroup="valgRegistration" ErrorMessage="Invalid URL Expression include http(s)://"
                    ControlToValidate="txtWebsiteUrl" Display="Dynamic" ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$">
                </asp:RegularExpressionValidator></div>
        </span>
    </fieldset>
</div>
