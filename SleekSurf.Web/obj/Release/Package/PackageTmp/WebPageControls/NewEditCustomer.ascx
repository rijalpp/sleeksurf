<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewEditCustomer.ascx.cs"
    Inherits="SleekSurf.Web.WebPageControls.NewEditCustomer" %>
<%@ Register Src="~/WebPageControls/FileUploader.ascx" TagName="FileUpload" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<fieldset>
    <legend>Customer Details</legend><span class="seperator">
        <div id="divFileUpload" runat="server" visible="false">
            <label>
                Profile Url:</label>
            <uc1:FileUpload ID="ucFileUpload" runat="server" />
            <span style="position: absolute; left: 760px; top: 80px;">
                <asp:Image ID="imgThumb" runat="server" Width="100px" EnableViewState="false"></asp:Image>
            </span>
        </div>
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
        <div>
            <label>
                Date of Birth:</label>
            <asp:TextBox ID="txtDOB" runat="server" MaxLength="10"></asp:TextBox>
             <span style="font-size:10px" >(DD/MM/YYYY)</span>
             <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid date format" ControlToValidate="txtDOB"
                                     ValidationExpression="^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/(19\d\d|200[01])" ValidationGroup="valgRegistration">
                                    </asp:RegularExpressionValidator>
        </div>
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
                </asp:RequiredFieldValidator></span><span style="margin-left: 10px">
                    <label>
                        <span class="mandatory">*</span> State/Province:</label>
                    <asp:TextBox ID="txtState" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqStateName" runat="server" ErrorMessage="*" ControlToValidate="txtState"
                        Display="Dynamic" ValidationGroup="valgRegistration">
                    </asp:RequiredFieldValidator></span>
        </div>
        <div>
            <span style="width: 340px; display: inline-block;">
                <label for="zipcode" class="label-float">
                    Zip/PostCode:</label>
                <asp:TextBox ID="txtPostCode" runat="server"></asp:TextBox></span><span style="margin-left: 10px">
                    <label>
                        <span class="mandatory">*</span> Country:</label>
                    <asp:DropDownList ID="ddlCountry" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqCountry" runat="server" ErrorMessage="*" InitialValue="Select Country"
                        ControlToValidate="ddlCountry" Display="Dynamic" ValidationGroup="valgRegistration"></asp:RequiredFieldValidator></span>
        </div>
        <div class="separator">
        </div>
    </span><span>
        <div>
            <span style="width: 340px; display: inline-block;">
                <label>
                    Contact Home:</label>
                <asp:TextBox ID="txtContactHome" runat="server"></asp:TextBox></span> <span style="margin-left: 10px">
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtContactHome"
                        FilterMode="ValidChars" FilterType="Numbers">
                    </asp:FilteredTextBoxExtender>
                    <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtContactHome"
                        runat="server" WatermarkCssClass="waterMark" WatermarkText="Numbers Only">
                    </asp:TextBoxWatermarkExtender>
                    <label>
                        <span class="mandatory">*</span> Contact Mobile:</label>
                    <asp:TextBox ID="txtContactMobile" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqContactMobile" runat="server" ErrorMessage="*"
                        ControlToValidate="txtContactMobile" Display="Dynamic" ValidationGroup="valgRegistration">
                    </asp:RequiredFieldValidator>
                    <asp:FilteredTextBoxExtender ID="fTxtEPostcode" runat="server" TargetControlID="txtContactMobile"
                        FilterMode="ValidChars" FilterType="Numbers">
                    </asp:FilteredTextBoxExtender>
                    <asp:TextBoxWatermarkExtender ID="WaterMarkTxtPostCode" TargetControlID="txtContactMobile"
                        runat="server" WatermarkCssClass="waterMark" WatermarkText="Numbers Only">
                    </asp:TextBoxWatermarkExtender>
                </span>
        </div>
        <div>
            <label>
                <span class="mandatory">*</span> Email:</label>
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator ID="rexEmail" runat="server" Display="Dynamic" ControlToValidate="txtEmail"
                ErrorMessage="Invalid Email Format" ValidationGroup="valgRegistration" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
            </asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator ID="reqEmail" runat="server" ErrorMessage="*" ControlToValidate="txtEmail"
                Display="Dynamic" ValidationGroup="valgRegistration">
            </asp:RequiredFieldValidator>
        </div>
        <div>
            <span style="width: 340px; display: inline-block;">
                <label>
                    <span class="mandatory">*</span> Occupation:</label>
                <asp:DropDownList ID="ddlOccupation" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="reqOccupation" runat="server" ErrorMessage="*" ControlToValidate="ddlOccupation"
                    InitialValue="Select Occupation" Display="Dynamic" ValidationGroup="valgRegistration">
                </asp:RequiredFieldValidator></span>
        </div>
        <asp:Panel ID="panelUpdateStuffs" runat="server" Visible="false">
            <div class="separator">
            </div>
            <div>
                <label>
                    Group:</label>
                <asp:DropDownList ID="ddlCustomerGroup" runat="server">
                </asp:DropDownList>
            </div>
            <div>
                <span style="width: 340px; display: inline-block;">
                    <label>
                        Email Subscription:</label>
                    <asp:CheckBox ID="chkSubscriptionEmail" runat="server" />
                </span><span style="margin-left: 10px">
                    <label>
                        SMS Subscription:</label>
                    <asp:CheckBox ID="chkSubscriptionSMS" runat="server" />
                </span>
            </div>
        </asp:Panel>
    </span>
</fieldset>
