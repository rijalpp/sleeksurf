<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="NewEditPromotion.aspx.cs" Inherits="SleekSurf.Web.Admin.Client.NewEditPromotion" %>

<%@ Register Src="~/WebPageControls/FileUploader.ascx" TagName="FileUpload" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ShowUpload() {
            if ($get('<%= this.ucFileUploadSupport.ClientID %>_fileUpload').value.length > 0) {
                $get('upUploadAttachment').style.display = 'block';
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../../Scripts/Fancybox/Fancybox-1.3.3.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('img[id$=imgThumb]').each(function () {
                if ((typeof this.naturalWidth != "undefined" && this.naturalWidth == 0) || this.readyState == 'uninitialized') {
                    $(this).attr('src', '../../App_Themes/SleekTheme/Images/PromotionDefault.png');
                }
            });

            $('.ddlPanel ul li:first-child').after("<li style='padding:0px; border-bottom: 1px solid #CCCCCC; margin: 10px 0px;'></li>");

            $('.ddlPanel ul li:first-child input:checkbox').click(function () {
                var selectionType = $(this).parent().parent().attr("name");

                if (selectionType == "SendEmail")
                    getCustomerListForEmail($(this).val(), $(this).is(":checked"));
                else if (selectionType == "SendSMS")
                    getCustomerListForSMS($(this).val(), $(this).is(":checked"));

                var getReference = $(this).parent().parent().children("ul li:not(:first-child)").children("input:checkbox");
                getReference.attr("disabled", $(this).is(":checked")).each(function () {
                    if ($(this).is(":checked")) {
                        $(this).attr("checked", $(this).is("not(:checked)"))
                        //                        if (selectionType == "SendEmail")
                        //                            getCustomerListForEmail($(this).val(), $(this).is(":checked"));
                        //                        else if (selectionType == "SendSMS")
                        //                            getCustomerListForSMS($(this).val(), $(this).is(":checked"));
                    }
                });
            });

            $('.ddlPanel ul li:not(:first-child) input:checkbox').click(function () {
                var selectionType = $(this).parent().parent().attr("name");

                if (selectionType == "SendEmail")
                    getCustomerListForEmail($(this).val(), $(this).is(":checked"));
                else if (selectionType == "SendSMS")
                    getCustomerListForSMS($(this).val(), $(this).is(":checked"));
            });

            $(document).click(function () {
                $(".ddlPanel").css("display", "none");
            });

            $('#<%=pnUpdateMode.ClientID %> :checkbox').click(function () {
                if ($(this).is(':checked'))
                    $(this).parent().next("div").css("display", "inline-block");
                else
                    $(this).parent().next("div").css("display", "none");
            });

            $(".labelSendTo").click(function (e) {
                e.stopPropagation();
                if ($(this).next(".ddlPanel").css("display") == "none") {
                    $(".ddlPanel").hide();
                    $(this).next(".ddlPanel").show();
                }
                else
                    $(this).next(".ddlPanel").hide();
            });

            $(".ddlPanel").click(function (e) {
                e.stopPropagation();
            });

            $(".customSelection").click(function () {
                $(".ddlPanel").css("display", "none");
                var selectionType = $(this).parent().prev().attr("name");

                //$(this).parent().prev().children("li").children("input:checkbox").attr("checked", false);
                $(this).parent().prev().children("li").children("input:checkbox").attr("checked", false).attr("disabled", false);

                if (selectionType == "SendEmail") {
                    $.fancybox({
                        'padding': 0,
                        'autoDimensions': true,
                        'width': 800,
                        'height': 600,
                        'modal': true,
                        'scrolling': 'auto',
                        'centerOnScroll': true,
                        'type': 'iframe',
                        'href': 'ShowClientCustomers.aspx?For=Email',
                        'onClosed': function () {
                            getCustomCustomerListForEmail();
                        }
                    });
                }
                else if (selectionType == "SendSMS") {
                    $.fancybox({
                        'padding': 0,
                        'autoDimensions': true,
                        'width': 800,
                        'height': 600,
                        'modal': true,
                        'scrolling': 'auto',
                        'centerOnScroll': true,
                        'type': 'iframe',
                        'href': 'ShowClientCustomers.aspx?For=SMS',
                        'onClosed': function () {
                            getCustomCustomerListForSMS();
                        }
                    });
                }
            });

            function getCustomerListForEmail(groupType, checked) {
                var loc = window.location.href;
                loc = (loc.substr(loc.length - 1, 1) == "/") ? loc + "NewEditPromotion.aspx" : loc;

                $.ajax({
                    type: "POST",
                    url: loc + "/RetriveCustomersForEmail",
                    data: '{"groupType":"' + groupType + '", "isChecked": "' + checked + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (customers) {
                        $("[id$=lblCountSelectedCustomerForEmail]").text(customers.d + " Customers Selected");
                    },
                    error: function () {
                        alert("An unexpected error has occurred during processing.");
                    }
                });
            }

            function getCustomerListForSMS(groupType, checked) {
                var loc = window.location.href;
                loc = (loc.substr(loc.length - 1, 1) == "/") ? loc + "NewEditPromotion.aspx" : loc;

                $.ajax({
                    type: "POST",
                    url: loc + "/RetriveCustomersForSMS",
                    data: '{"groupType":"' + groupType + '", "isChecked": "' + checked + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (customers) {
                        $("[id$=lblCountSelectedCustomerForSMS]").text(customers.d + " Customers Selected");
                    },
                    error: function () {
                        alert("An unexpected error has occurred during processing.");
                    }
                });
            }

            function getCustomCustomerListForEmail() {
                var loc = window.location.href;
                loc = (loc.substr(loc.length - 1, 1) == "/") ? loc + "NewEditPromotion.aspx" : loc;

                $.ajax({
                    type: "POST",
                    url: loc + "/CustomRetriveCustomersForEmail",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (customers) {
                        $("[id$=lblCountSelectedCustomerForEmail]").text(customers.d + " Customers Selected");
                    },
                    error: function () {
                        alert("An unexpected error has occurred during processing.");
                    }
                });
            }

            function getCustomCustomerListForSMS() {
                var loc = window.location.href;
                loc = (loc.substr(loc.length - 1, 1) == "/") ? loc + "NewEditPromotion.aspx" : loc;

                $.ajax({
                    type: "POST",
                    url: loc + "/CustomRetriveCustomersForSMS",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (customers) {
                        $("[id$=lblCountSelectedCustomerForSMS]").text(customers.d + " Customers Selected");
                    },
                    error: function () {
                        alert("An unexpected error has occurred during processing.");
                    }
                });
            }
        });
    </script>
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: 100%">
                    <asp:Label ID="lblTitle" runat="server" Text="Promotion Management Portal" EnableViewState="false"></asp:Label></div>
            </div>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            <asp:HiddenField ID="hfPromoID" runat="server" />
            <asp:HiddenField ID="hfClientID" runat="server" />
            <div id="PromotionDetails" style="margin: 0px; width: 800px;">
                <fieldset>
                    <legend>Promotion Details</legend>
                    <div class="div">
                        <label>
                            <span class="mandatory">*</span> Title:</label>
                        <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqTitle" runat="server" ErrorMessage="*" ControlToValidate="txtTitle"
                            Display="Dynamic" ValidationGroup="PromotionValidation">
                        </asp:RequiredFieldValidator>
                    </div>
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
                            ControlToValidate="txtStartDate" ValidationGroup="PromotionValidation" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:MaskedEditExtender AutoComplete="true" ID="MaskedEditExtenderDOB" runat="server"
                            TargetControlID="txtStartDate" MaskType="Date" Mask="99/99/9999" CultureName="en-AU">
                        </asp:MaskedEditExtender>
                        <asp:MaskedEditValidator ID="MaskedEditValidatorDOB" runat="server" ControlExtender="MaskedEditExtenderDOB"
                            ControlToValidate="txtStartDate" IsValidEmpty="false" ErrorMessage="Invalid Date Format"
                            InvalidValueMessage="Invalid Date Format (dd/MM/yyyy)" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$"
                            Display="Dynamic" ValidationGroup="PromotionValidation" ForeColor="Red">
                        </asp:MaskedEditValidator>
                    </div>
                    <div class="div">
                        <span style="display: inline-block;">
                            <label>
                                <span class="mandatory">*</span> End Date:</label>
                            <asp:TextBox ID="txtEndDate" runat="server" ></asp:TextBox>
                            <asp:Image ID="imgEndDate" ImageUrl="~/App_Themes/SleekTheme/Images/Calendar.png" runat="server"
                                Height="28px" />
                        </span>
                        <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkText=" dd/MM/yyyy "
                            TargetControlID="txtEndDate" WatermarkCssClass="waterMark">
                        </asp:TextBoxWatermarkExtender>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" CssClass="calandarTheme"
                            TargetControlID="txtEndDate" PopupButtonID="imgEndDate">
                        </asp:CalendarExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                            ControlToValidate="txtEndDate" ValidationGroup="PromotionValidation" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:MaskedEditExtender AutoComplete="true" ID="MaskedEditExtender1" runat="server"
                            TargetControlID="txtEndDate" MaskType="Date" Mask="99/99/9999" CultureName="en-AU">
                        </asp:MaskedEditExtender>
                        <asp:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtenderDOB"
                            ControlToValidate="txtEndDate" IsValidEmpty="false" ErrorMessage="Invalid Date Format"
                            InvalidValueMessage="Invalid Date Format (dd/MM/yyyy)" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$"
                            Display="Dynamic" ValidationGroup="PromotionValidation" ForeColor="Red">
                        </asp:MaskedEditValidator>
                    </div>
                    <div class="div">
                        <label>
                            Promo Image:</label>
                        <uc1:FileUpload ID="ucFileUpload" runat="server" />
                        <span style="position: absolute; right: 20px; top: 40px;">
                            <asp:Image ID="imgThumb" runat="server" Height="100px" Style="padding: 5px; border: 1px solid #DDDDDD;">
                            </asp:Image>
                        </span>
                    </div>
                    <div class="div" style="display: table">
                        <div class="cell">
                            <label>
                                <span class="mandatory">*</span> Description:</label>
                        </div>
                        <div class="cell" style="padding-right: 5px;">
                            <cc1:Editor ID="editorDescription" runat="server" Width="640px" Height="300px" />
                        </div>
                        <div class="cell">
                            <asp:RequiredFieldValidator ID="reqDescription" runat="server" ControlToValidate="editorDescription"
                                ValidationGroup="PromotionValidation" ErrorMessage="*" Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="div">
                        <label>
                            <span class="mandatory">*</span> Publish:</label>
                        <asp:CheckBox ID="chkPublish" runat="server" />
                    </div>
                    <div class="div">
                        <div style="float: right; margin-bottom: 10px; text-align: right;">
                            <asp:UpdatePanel runat="server" ID="upAttachSupportingImage">
                                <ContentTemplate>
                                    <asp:Panel ID="pHeader" runat="server">
                                        <div class="cpHeader">
                                            <asp:Label ID="lblOpen" runat="server" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="pBody" runat="server">
                                        <div class="cpBody">
                                            <div style="float: left;">
                                                <label style="width: auto;">
                                                    Image:</label>
                                                <uc1:FileUpload ID="ucFileUploadSupport" runat="server" />
                                            </div>
                                            <div style="float: left;">
                                                <asp:UpdateProgress ID="upUploadAttachment" runat="server" DisplayAfter="4000" ClientIDMode="Static">
                                                    <ProgressTemplate>
                                                        <img src="../../App_Themes/SleekTheme/Images/Uploading.gif" alt="Uploading.." />
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pBody"
                                        CollapseControlID="lblOpen" ExpandControlID="lblOpen" Collapsed="true" TextLabelID="lblOpen"
                                        CollapsedText="Attach Supporting Image.." ExpandedText="[Hide]" CollapsedSize="0">
                                    </asp:CollapsiblePanelExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <asp:Panel ID="pnUpdateMode" runat="server" CssClass="pnUpdateMode">
                        <div class="div separator" style="height: auto;">
                        </div>
                        <div class="div" style="height: 25px;">
                            <div style="width: 150px; display: inline-block; padding-top: 6px;">
                                <label>
                                    Send Email:</label>
                                <asp:CheckBox ID="chkSendEmail" runat="server" />
                            </div>
                            <div class="panelDropDown" style="margin-left: 10px; display: none;">
                                <label style="width: 40px">
                                    To:</label><asp:Label ID="lblSendEmail" runat="server" Text="Select Customers" SkinID="SendTo" />
                                <div class="ddlPanel">
                                    <asp:CheckBoxList ID="chkblSendEmail" runat="server" RepeatLayout="UnorderedList"
                                        name="SendEmail">
                                    </asp:CheckBoxList>
                                    <div>
                                        <label class="customSelection">
                                            Custom Selection
                                        </label>
                                    </div>
                                </div>
                                <div class="selectedCustomerLabel">
                                    <asp:Label ID="lblCountSelectedCustomerForEmail" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="div" style="height: 25px;">
                            <div style="width: 150px; display: inline-block; padding-top: 6px;">
                                <label>
                                    Send SMS:</label>
                                <asp:CheckBox ID="chkSendSMS" runat="server" />
                            </div>
                            <div class="panelDropDown" style="margin-left: 10px; display: none;">
                                <label style="width: 40px">
                                    To:</label><asp:Label ID="lblSendSMS" runat="server" Text="Select Customers" SkinID="SendTo" />
                                <div class="ddlPanel">
                                    <asp:CheckBoxList ID="chkblSendSMS" runat="server" RepeatLayout="UnorderedList" name="SendSMS">
                                    </asp:CheckBoxList>
                                    <div>
                                        <label class="customSelection">
                                            Custom Selection
                                        </label>
                                    </div>
                                </div>
                                <div class="selectedCustomerLabel">
                                    <asp:Label ID="lblCountSelectedCustomerForSMS" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="div separator" style="height: auto;">
                        </div>
                        <div class="div">
                            <script type="text/javascript">
                                var promotionID = $('[id$=hfPromoID]').val();
                                var clientID = $('[id$=hfClientID]').val();
                                $(document).ready(function () {
                                    $('#MultipleFilesUpload').uploadify({
                                        'uploader': '../../Scripts/Uploadify-v2.1.4/uploadify.swf',
                                        'script': '../../Upload.ashx',
                                        'scriptData': { 'PromotionID': promotionID, 'ClientID': clientID },
                                        'cancelImg': '../../Scripts/Uploadify-v2.1.4/cancel.png',
                                        'buttonText': 'More Pictures',
                                        'fileDataName': 'UploadedFile',
                                        'fileExt': '*.jpg;*.gif;*.png',
                                        'fileDesc': 'Web Image Files (.JPG, .GIF, .PNG)',
                                        'sizeLimit': 1024000,
                                        'auto': true,
                                        'multi': true
                                    });
                                });
                            </script>
                            <span style="float:left;"><input id="MultipleFilesUpload" name="MultipleFilesUpload" type="file" /></span>
                            <span style="float:left; padding-left:5px;">Maximum size of each file is 1 MB</span>
                        </div>
                    </asp:Panel>
                </fieldset>
                <div id="SignUpNavigation" style="width: 800px;">
                    <div style="float: left; margin-right: 20px; padding-left: 15px;">
                        <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                    </div>
                    <div style="float: right;">
                        <span style="margin: 0 15px">
                            <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="PromotionValidation"
                                OnClick="btnSave_Click" OnClientClick="ShowUpload()" SkinID="Button" /></span>
                        <span style="margin: 0px;">
                              <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                OnClick="btnCancel_Click" SkinID="Button" />
                         </span>
                    </div>
                </div>
            </div>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks" runat="server" id="dvRightLinks">
                <asp:HyperLink ID="hlAddNewMedia" runat="server" Text="Add New Media" NavigateUrl="~/Admin/Client/NewEditPromotionGallery.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
                <asp:HyperLink ID="hlDisplayMedia" runat="server" Text="View All Media" NavigateUrl="~/Admin/Client/PromotionGalleryManagement.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
