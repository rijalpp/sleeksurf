<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileUploader.ascx.cs"
    Inherits="SleekSurf.Web.WebPageControls.FileUploader" %>
<span>
    <asp:FileUpload ID="fileUpload" runat="server" />
    <asp:RegularExpressionValidator ID="RegularExpressionValidatorFuImage" runat="server"
        ErrorMessage="Only gif, jpg, png, jpeg files are allowed!" ValidationExpression="^(.*\.([gG][iI][fF]|[jJ][pP][gG]|[jJ][pP][eE][gG]|[bB][mM][pP]|[pP][nN][gG])$)"
        ControlToValidate="fileUpload" Display="Dynamic"></asp:RegularExpressionValidator>
</span>