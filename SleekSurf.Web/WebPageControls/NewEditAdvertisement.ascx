<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewEditAdvertisement.ascx.cs"
    Inherits="SleekSurf.Web.WebPageControls.NewEditAdvertisement" %>
<%@ Register Src="~/WebPageControls/FileUploader.ascx" TagName="FileUpload" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<script type="text/javascript">
    function pageLoad(sender, args) {

        $(".showError").hide();

        $("[id$=btnSave]").click(function (e) {
            if ($("[id$=ddlDimension]").val() == "0") {
                e.preventDefault();
                $(".showError").show();
            }
            else
                $(".showError").hide();
        });

        $("#<%= ddlDimension.ClientID %>").change(function () {
            if ($(this).val() != "0") {
                $(".showError").hide();
            }
            else {
                $(".showError").show();
            }
        });
    }
</script>
<div id="imageHolder" runat="server">
    <asp:Image ID="imgThumb" runat="server" EnableViewState="false"></asp:Image>
</div>
<div style="clear: both;">
    <fieldset>
        <legend>Advertisement Details</legend>
        <div class="div">
            <label>
                <span class="mandatory">*</span> Image Url:</label>
            <uc1:FileUpload ID="ucFileUpload" runat="server" />
        </div>
        <div class="div">
            <span>
                <label>
                    <span class="mandatory">*</span> Navigate Url:</label>
                <asp:TextBox ID="txtNavigateUrl" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNavigateUrl"
                    Display="Dynamic" ErrorMessage="*" ValidationGroup="valgAdvertisement">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorWebsite" runat="server"
                    ValidationGroup="valgAdvertisement" ErrorMessage="Invalid URL Expression include http(s)://"
                    ControlToValidate="txtNavigateUrl" Display="Dynamic" ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$">
                </asp:RegularExpressionValidator>
            </span>
        </div>
        <div class="div">
            <label>
                Ad Title:</label>
            <asp:TextBox ID="txtAdName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqAdName" runat="server" ControlToValidate="txtAdName"
                Display="Dynamic" ErrorMessage="*" ValidationGroup="valgAdvertisement"></asp:RequiredFieldValidator>
        </div>
        <div class="div">
            <span style="width: 340px; display: inline-block;">
                <label>
                    <span class="mandatory">*</span> Advertiser:</label>
                <asp:TextBox ID="txtAdvertiser" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqAdvertiser" runat="server" ControlToValidate="txtAdvertiser"
                    Display="Dynamic" ErrorMessage="*" ValidationGroup="valgAdvertisement">  
                </asp:RequiredFieldValidator>
            </span><span style="margin-left: 10px">
                <label>
                    <span class="mandatory">*</span> Contact Details:</label>
                <asp:TextBox ID="txtContactDetails" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtContactDetails"
                    Display="Dynamic" ErrorMessage="*" ValidationGroup="valgAdvertisement">  
                </asp:RequiredFieldValidator>
            </span>
        </div>
        <div class="div">
            <label>
                <span class="mandatory">*</span> Email:</label>
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtEmail"
                Display="Dynamic" ErrorMessage="*" ValidationGroup="valgAdvertisement"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="rexEmail" runat="server" Display="Dynamic" ControlToValidate="txtEmail"
                ErrorMessage="Invalid Email Fromat" ValidationGroup="valgRegistration" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
            </asp:RegularExpressionValidator>
        </div>
        <div class="div separator">
        </div>
        <asp:Panel ID="pnlUpdateMode" runat="server">
            <div class="div">
                <span style="display: inline-block;">
                    <label>
                        <span class="mandatory">*</span> Start Date:</label>
                    <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                    <asp:Image ID="imgStartDate" ImageUrl="~/App_Themes/SleekTheme/Images/Calendar.png"
                        runat="server" Height="28px" />
                </span>
                <asp:TextBoxWatermarkExtender ID="TextBoxWaterMarkDOB" runat="server" WatermarkText=" dd/MM/yyyy "
                    TargetControlID="txtStartDate" WatermarkCssClass="waterMark">
                </asp:TextBoxWatermarkExtender>
                <asp:CalendarExtender ID="CalendarExtenderDOB" runat="server" Format="dd/MM/yyyy"
                    CssClass="calandarTheme" TargetControlID="txtStartDate" PopupButtonID="imgStartDate">
                </asp:CalendarExtender>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
                    ControlToValidate="txtStartDate" ValidationGroup="valgAdvertisement" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:MaskedEditExtender AutoComplete="true" ID="MaskedEditExtenderDOB" runat="server"
                    TargetControlID="txtStartDate" MaskType="Date" Mask="99/99/9999" CultureName="en-AU">
                </asp:MaskedEditExtender>
                <asp:MaskedEditValidator ID="MaskedEditValidatorDOB" runat="server" ControlExtender="MaskedEditExtenderDOB"
                    ControlToValidate="txtStartDate" IsValidEmpty="false" ErrorMessage="Invalid Date Format"
                    InvalidValueMessage="Invalid Date Format (dd/MM/yyyy)" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$"
                    Display="Dynamic" ValidationGroup="valgAdvertisement" ForeColor="Red">
                </asp:MaskedEditValidator>
            </div>
            <div class="div">
                <span style="display: inline-block;">
                    <label>
                        <span class="mandatory">*</span> End Date:</label>
                    <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                    <asp:Image ID="imgEndDate" ImageUrl="~/App_Themes/SleekTheme/Images/Calendar.png"
                        runat="server" Height="28px" />
                </span>
                <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkText=" dd/MM/yyyy "
                    TargetControlID="txtEndDate" WatermarkCssClass="waterMark">
                </asp:TextBoxWatermarkExtender>
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" CssClass="calandarTheme"
                    TargetControlID="txtEndDate" PopupButtonID="imgEndDate">
                </asp:CalendarExtender>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                    ControlToValidate="txtEndDate" ValidationGroup="valgAdvertisement" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:MaskedEditExtender AutoComplete="true" ID="MaskedEditExtender1" runat="server"
                    TargetControlID="txtEndDate" MaskType="Date" Mask="99/99/9999" CultureName="en-AU">
                </asp:MaskedEditExtender>
                <asp:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtenderDOB"
                    ControlToValidate="txtEndDate" IsValidEmpty="false" ErrorMessage="Invalid Date Format"
                    InvalidValueMessage="Invalid Date Format (dd/MM/yyyy)" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$"
                    Display="Dynamic" ValidationGroup="valgAdvertisement" ForeColor="Red">
                </asp:MaskedEditValidator>
            </div>
            <div class="div">
                <label>
                    <span class="mandatory">*</span> Amount Paid:</label>
                <asp:TextBox ID="txtAmountPaid" runat="server" ToolTip=" Enter in Decimal format i.e. 00.00"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqAmountPaid" runat="server" ErrorMessage="*" ControlToValidate="txtAmountPaid"
                    Display="Dynamic" ValidationGroup="valgAdvertisement">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="regxAmountPaid" runat="server" ErrorMessage="* Only currency Format(00.00) allowed"
                    Display="Dynamic" ControlToValidate="txtAmountPaid" ValidationExpression="^[0-9]{1,14}(.[0-9]{2}){0,1}$"
                    ValidationGroup="valgAdvertisement">
                </asp:RegularExpressionValidator>
            </div>
            <div class="div">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <span style="width: 340px; display: inline-block;">
                            <label>
                                <span class="mandatory">*</span> Display Position:</label>
                            <asp:DropDownList ID="ddlDisplayPosition" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDisplayPosition_SelectedIndexChanged">
                                <asp:ListItem>Select Below</asp:ListItem>
                                <asp:ListItem>Right</asp:ListItem>
                                <asp:ListItem>Footer</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqState" runat="server" ErrorMessage="*" ControlToValidate="ddlDisplayPosition"
                                Display="Dynamic" ValidationGroup="valgAdvertisement" InitialValue="Select Below">
                            </asp:RequiredFieldValidator>
                        </span><span style="margin-left: 10px">
                            <label>
                                <span class="mandatory">*</span> Dimension:</label>
                            <asp:DropDownList ID="ddlDimension" runat="server" Enabled="false">
                            </asp:DropDownList>
                            <span class="errorValidator showError">*</span>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:Panel>
        <div class="div separator">
        </div>
        <div class="div">
            <label style="vertical-align: top">
                Comments:</label>
            <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Rows="8" Width="535px"></asp:TextBox>
        </div>
        <div class="div">
            <label>
                Published:</label>
            <asp:CheckBox ID="chkPublished" runat="server" Checked="true" />
        </div>
    </fieldset>
</div>
