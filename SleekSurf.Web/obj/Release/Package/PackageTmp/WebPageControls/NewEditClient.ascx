<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewEditClient.ascx.cs"
    Inherits="SleekSurf.Web.WebPageControls.NewEditClient" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/WebPageControls/FileUploader.ascx" TagName="ClientPhotoLoader"
    TagPrefix="uc" %>
<div id="SignUpContentDetails">
    <fieldset>
        <legend>Client Details</legend>
        <asp:HiddenField ID="hfClientID" runat="server" />
        <span>
            <div>
                <label>
                    Business ABN/ACN:</label>
                <asp:UpdatePanel ID="upnlABN" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:TextBox ID="txtABN" runat="server" AutoPostBack="true" CausesValidation="true"
                            OnTextChanged="txtABN_TextChanged"></asp:TextBox>
                        <asp:Label ID="lblErrorABNMsg" runat="server" SkinID="Error"></asp:Label>
                        <asp:CompareValidator ID="cmpABN" runat="server" ControlToValidate="txtABN" ErrorMessage="*"
                            Display="Dynamic" ValidationGroup="valgRegistration"></asp:CompareValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ErrorMessage="Must have either 9 or 11"
                            ControlToValidate="txtABN" runat="server" Display="Dynamic" ValidationExpression="^(\d{11})|(\d{9})$" />
                        <asp:FilteredTextBoxExtender ID="ftxtEABN" runat="server" TargetControlID="txtABN"
                            FilterMode="ValidChars" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:Panel ID="pnlUniqueUrl" runat="server" Visible="false">
                <label>
                    Unique URL ID:</label>
                <asp:UpdatePanel ID="upnlUniqueidentity" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:TextBox ID="txtUniqueIdentity" runat="server" onblur='this.value=this.value.toLowerCase()'
                            OnTextChanged="txtUniqueIdentity_TextChanged" MaxLength="24" AutoPostBack="true"
                            CausesValidation="true"></asp:TextBox>
                        <asp:Label ID="lblErrorMsg" runat="server" SkinID="Error"></asp:Label>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ErrorMessage="Can't have '.' or space in unique url"
                            ControlToValidate="txtUniqueIdentity" runat="server" Display="Dynamic" ValidationExpression="\w*[^\.]" />
                        <asp:CompareValidator ID="cmpUniqueName" runat="server" ControlToValidate="txtUniqueIdentity"
                            ErrorMessage="*" Display="Dynamic" ValidationGroup="valgRegistration" Type="String"
                            Operator="NotEqual"></asp:CompareValidator>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <span style="display: block;">
                    <label>
                    </label>
                    <label class="extraSmallLabel" style="padding-left: 0px;">
                        (Max 24 Characters)</label>
                </span>
            </asp:Panel>
            <asp:Panel ID="pnlUniqueDomain" runat="server" Visible="false">
                <label>
                    Unique Domain:</label>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:TextBox ID="txtUniqueDomain" runat="server" onblur='this.value=this.value.toLowerCase()'
                            OnTextChanged="txtUniqueDomain_TextChanged" AutoPostBack="true" CausesValidation="true"
                            Width="265px"></asp:TextBox>
                        <asp:Label ID="lblErrorDomain" runat="server" SkinID="Error"></asp:Label>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ErrorMessage="Invalid! Domain must include http/https/ftp."
                            ControlToValidate="txtUniqueDomain" runat="server" Display="Dynamic" ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$" />
                        <asp:CompareValidator ID="cmpUniqueDomain" runat="server" ControlToValidate="txtUniqueDomain"
                            ErrorMessage="*" Display="Dynamic" ValidationGroup="valgRegistration" Type="String"
                            Operator="NotEqual"></asp:CompareValidator>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <div>
                <label>
                    <span class="mandatory">*</span> Category:</label>
                <asp:DropDownList ID="ddlCategory" runat="server" Width="272px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="reqGender" runat="server" ErrorMessage="*" ControlToValidate="ddlCategory"
                    InitialValue="Select Category" Display="Dynamic" ValidationGroup="valgRegistration"></asp:RequiredFieldValidator>
            </div>
            <asp:Panel ID="pnlEstablishedDate" runat="server" Visible="false">
                <label>
                    Established Date:</label>
                <asp:TextBox ID="txtEstablishedDate" runat="server"></asp:TextBox>
                <asp:MaskedEditExtender ID="mexDOB" runat="server" AutoComplete="true" TargetControlID="txtEstablishedDate"
                    MaskType="Date" Mask="99/99/9999" CultureName="en-AU">
                </asp:MaskedEditExtender>
                <asp:MaskedEditValidator ID="mxvDOB" runat="server" ControlExtender="mexDOB" ControlToValidate="txtEstablishedDate"
                    IsValidEmpty="true" ErrorMessage="Invalid Date Format" InvalidValueMessage="Invalid Date Format (dd/mm/yyyy)"
                    ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$" CssClass="errorValidator"
                    Display="Dynamic" ValidationGroup="valgRegistration"></asp:MaskedEditValidator>
            </asp:Panel>
            <asp:Panel ID="pnlBusinessLogo" runat="server" Visible="false">
                <label>
                    Business Logo:</label>
                <uc:ClientPhotoLoader runat="server" ID="LogoLoader" />
            </asp:Panel>
            <div>
                <label>
                    <span class="mandatory">*</span> Business Name:</label>
                <asp:TextBox ID="txtClientName" runat="server" Width="265px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqClientName" runat="server" ErrorMessage="*" ControlToValidate="txtClientName"
                    Display="Dynamic" ValidationGroup="valgRegistration"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label>
                    <span class="mandatory">*</span> Contact Person:</label>
                <asp:TextBox ID="txtContactPerson" runat="server" Width="265px" ReadOnly="true"></asp:TextBox>
                <asp:DropDownList ID="ddlContactPerson" runat="server" Visible="false">
                </asp:DropDownList>
            </div>
            <div class="separator">
            </div>
        </span><span>
            <div>
                <label>
                    <span class="mandatory">*</span> Address Line1:
                </label>
                <asp:TextBox ID="txtAddressLine1" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqAddressLine1" runat="server" ErrorMessage="*"
                    ControlToValidate="txtAddressLine1" Display="Dynamic" ValidationGroup="valgRegistration">
                </asp:RequiredFieldValidator>
            </div>
            <div>
                <span style="width: 340px; display: inline-block;">
                    <label>
                        Address Line2:
                    </label>
                    <asp:TextBox ID="txtAddressLine2" runat="server"></asp:TextBox></span> <span style="margin-left: 10px">
                        <label>
                            Address Line3:
                        </label>
                        <asp:TextBox ID="txtAddressLine3" runat="server"></asp:TextBox></span>
            </div>
            <div>
                <span style="width: 340px; display: inline-block;">
                    <label>
                        <span class="mandatory">*</span> City/Suburb:</label>
                    <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqCity" runat="server" ErrorMessage="*" ControlToValidate="txtCity"
                        Display="Dynamic" ValidationGroup="valgRegistration">
                    </asp:RequiredFieldValidator></span> <span style="margin-left: 10px">
                        <label>
                            <span class="mandatory">*</span> State:</label>
                        <asp:DropDownList ID="ddlState" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqState" runat="server" ErrorMessage="*" ControlToValidate="ddlState"
                            Display="Dynamic" ValidationGroup="valgRegistration" InitialValue="Select State">
                        </asp:RequiredFieldValidator>
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
                        runat="server" WatermarkCssClass="waterMark" WatermarkText="Four ditigs only">
                    </asp:TextBoxWatermarkExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ErrorMessage="*"
                        ControlToValidate="txtPostcode" runat="server" ValidationExpression="^\d{4}$"
                        ValidationGroup="valgRegistration" />
                </span><span style="margin-left: 10px">
                    <label>
                        <span class="mandatory">*</span> Country:</label>
                    <asp:DropDownList ID="ddlCountry" runat="server" Enabled="false">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqCountry" runat="server" ErrorMessage="*" ControlToValidate="ddlCountry"
                        InitialValue="Select Country" Display="Dynamic" ValidationGroup="valgRegistration">
                    </asp:RequiredFieldValidator></span>
            </div>
            <div class="separator">
            </div>
        </span><span>
            <div>
                <label>
                    Business Website:</label>
                <asp:TextBox ID="txtBusinessUrl" runat="server" Width="265px"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorWebsite" runat="server"
                    ValidationGroup="valgRegistration" ErrorMessage="Invalid! URL Expression must include http(s):// and no space allowed."
                    ControlToValidate="txtBusinessUrl" Display="Dynamic" ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$">
                </asp:RegularExpressionValidator>
            </div>
            <div>
                <label>
                    <span class="mandatory">*</span> Business Email:</label>
                <asp:TextBox ID="txtBusinessEmail" runat="server" Width="265px"></asp:TextBox>
                <asp:RegularExpressionValidator ID="rexBusinessEmail" runat="server" Display="Dynamic"
                    ControlToValidate="txtBusinessEmail" ErrorMessage="Invalid Email Format" ValidationGroup="valgRegistration"
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                </asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="reqBusinessEmail" runat="server" ErrorMessage="*"
                    ControlToValidate="txtBusinessEmail" Display="Dynamic" ValidationGroup="valgRegistration">
                </asp:RequiredFieldValidator>
            </div>
            <div>
                <label>
                    <span class="mandatory">*</span> Contact(Office):</label>
                <asp:TextBox ID="txtContactOffice" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqContactOffice" runat="server" ErrorMessage="*"
                    ControlToValidate="txtContactOffice" Display="Dynamic" ValidationGroup="valgRegistration"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorContactOffice" runat="server"
                    ValidationGroup="valgRegistration" ErrorMessage="Invalid Phone Number must start with +61 or area code"
                    ControlToValidate="txtContactOffice" Display="Dynamic" ValidationExpression="^(\+61|0)[2-478]([0-9]){8}$">
                </asp:RegularExpressionValidator>
            </div>
            <asp:Panel ID="pnlFax" runat="server" Visible="false">
                <label>
                    Fax:</label>
                <asp:TextBox ID="txtContactFax" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorFax" runat="server"
                    ValidationGroup="valgRegistration" ErrorMessage="Invalid Fax Number must start with +61 or area code"
                    ControlToValidate="txtContactFax" Display="Dynamic" ValidationExpression="^(\+61|0)[2378]([0-9]){8}$">
                </asp:RegularExpressionValidator>
            </asp:Panel>
            <div class="separator">
            </div>
        </span><span>
            <div>
                <label style="vertical-align: top">
                    <span class="mandatory">*</span> Description:</label>
                <asp:TextBox ID="txtBusinessDescription" runat="server" TextMode="MultiLine" Rows="8"
                    Width="535px"></asp:TextBox>
                <span style="vertical-align: top;">
                    <asp:RequiredFieldValidator ID="reqBusinessDescription" runat="server" ErrorMessage="*"
                        ControlToValidate="txtBusinessDescription" Display="Dynamic" ValidationGroup="valgRegistration"></asp:RequiredFieldValidator>
                </span>
            </div>
            <asp:Panel ID="pnlTheme" runat="server" Visible="false">
                <span style="width: 340px; display: inline-block;">
                    <label>
                        Theme:</label>
                    <asp:DropDownList ID="ddlTheme" runat="server">
                    </asp:DropDownList>
                </span>
            </asp:Panel>
            <asp:Panel ID="pnlPublish" runat="server" Visible="false">
                <span style="margin-left: 10px">
                    <label>
                        Published:</label>
                    <asp:CheckBox ID="chkPublished" runat="server" />
                </span>
            </asp:Panel>
        </span>
    </fieldset>
</div>
