<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="NewEditPromotionGallery.aspx.cs" Inherits="SleekSurf.Web.Admin.Client.NewEditPromotionGallery" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('img[id$=imgThumb]').each(function () {
                if ((typeof this.naturalWidth != "undefined" && this.naturalWidth == 0) || this.readyState == 'uninitialized') {
                    $(this).attr('src', '../../App_Themes/SleekTheme/Images/PromotionDefault.png');
                }
            });

            $("#btnSave").click(function () {
                var tempDiv = $('<div/>');
                tempDiv.html($('#txtPathUrl').val());
                tempDiv.html(tempDiv.children("iframe, embed, object")); // OR two commented lines below
                //                tempDiv.html(tempDiv.children());
                //                $(':not(iframe, embed, object)', tempDiv).remove();
                $("#txtPathUrl").val(tempDiv.html());
                $("#txtPathUrl").val($('<div/>').text($("#txtPathUrl").val()).html());
            });

            $(".menuTab ul li").hover(function () {
                $('#txtPathUrl').val($('<div/>').text($("#txtPathUrl").val()).html());
            }, function () {
                $('#txtPathUrl').val($('<div/>').html($("#txtPathUrl").val()).text());
            });
        });

        function pageLoad(sender, args) {
            $(".absolutePosition iframe,.absolutePosition object,.absolutePosition embed").attr("style", "height:215px; width:325px;");
         }
    </script>
    <div id="WrapperMasterPage">
        <div id="LeftContentMasterPage">
            <div id="TitleSearch">
                <div class="title" style="width: 100%">
                    <asp:Label ID="lblTitle" runat="server" Text="Media Management Portal" EnableViewState="false"></asp:Label></div>
            </div>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            <asp:HiddenField ID="hfPromoID" runat="server" />
            <asp:HiddenField ID="hfClientID" runat="server" />
            <div id="MediaDetails" style="margin: 0px; width: 800px;">
                <fieldset>
                    <legend>Media Details</legend>
                    <div class="div">
                        <label>
                            <span class="mandatory">*</span> Title:</label>
                        <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqTitle" runat="server" ErrorMessage="*" ControlToValidate="txtTitle"
                            Display="Dynamic" ValidationGroup="PromotionValidation">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="div">
                        <label style="vertical-align: top;">
                            Caption:</label>
                        <asp:TextBox ID="txtCaption" runat="server"></asp:TextBox>
                    </div>
                    <asp:UpdatePanel ID="upnlSelectMediaType" runat="server">
                       <ContentTemplate>
                       
                    <div class="div">
                        <label style="vertical-align: top;">
                            <span class="mandatory">*</span> Media Type:</label>
                        <asp:DropDownList ID="ddlMediaType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMediaType_SelectedIndexChanged">
                            <asp:ListItem>Select Type</asp:ListItem>
                            <asp:ListItem>Image</asp:ListItem>
                            <asp:ListItem>Video</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqGender" runat="server" ErrorMessage="*" ControlToValidate="ddlMediaType"
                            InitialValue="Select Type" Display="Dynamic" ValidationGroup="PromotionValidation"></asp:RequiredFieldValidator>
                    </div>
                    <div class="div">
                        <label style="vertical-align:top">
                            <span class="mandatory">*</span> Embed Video:</label>
                        <asp:TextBox ID="txtPathUrl" runat="server" TextMode="MultiLine" ClientIDMode="Static" Rows="5" Width="262px" EnableViewState="false"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderPathUrl" runat="server"
                            TargetControlID="txtPathUrl" WatermarkText="Paste Embeded Code Here" WatermarkCssClass="waterMark" />
                        <label style="margin-left:2px">
                            OR</label>
                    </div>
                    <div class="div">
                        <label>
                            <span class="mandatory">*</span> Image Only:
                        </label>
                        <asp:FileUpload ID="fuUploadMedia" runat="server" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorFuImage" runat="server"
                            ErrorMessage="Only gif, jpg, png, jpeg files are allowed!" ValidationExpression="^(.*\.([gG][iI][fF]|[jJ][pP][gG]|[jJ][pP][eE][gG]|[bB][mM][pP]|[pP][nN][gG])$)"
                            ControlToValidate="fuUploadMedia" Display="Dynamic"></asp:RegularExpressionValidator>
                        <span class="absolutePosition">
                            <asp:Image ID="imgThumb" runat="server" Visible="false" Height="100px" Style="padding: 5px; border: 1px solid #DDDDDD;">
                            </asp:Image>
                            <asp:Literal ID="ltrVideoThumb" runat="server" Visible="false"></asp:Literal>
                        </span>
                    </div>

                    </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="div" style="display: table">
                        <div class="cell">
                            <label>
                                Description:</label>
                        </div>
                        <div class="cell" style="padding-right: 5px;">
                            <cc1:Editor ID="editorDescription" runat="server" Width="640px" />
                        </div>
                        <div class="cell">
                        </div>
                    </div>
                    <div class="div">
                        <label>
                            <span class="mandatory">*</span> Publish:</label>
                        <asp:CheckBox ID="chkPublish" runat="server" />
                    </div>
                </fieldset>
                <div id="SignUpNavigation" style="width: 800px;">
                    <div style="float: left; margin-right: 20px; padding-left: 15px;">
                        <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
                    </div>
                    <div style="float: right;">
                        <span style="margin: 0 15px">
                            <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="PromotionValidation"
                                SkinID="Button" OnClick="btnSave_Click" ClientIDMode="Static" /></span>
                         <span style="margin:0;">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                OnClick="btnCancel_Click" SkinID="Button" />
                        </span>
                    </div>
                </div>
            </div>
        </div>
        <div id="RightContentMasterPage">
            <div class="rightLinks" runat="server" id="dvRightLinks">
                <asp:HyperLink ID="hlAddNewMedia" runat="server" Text="View All Media" NavigateUrl="~/Admin/Client/PromotionGalleryManagement.aspx"
                    CssClass="hyperLink addHyperLink"></asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
