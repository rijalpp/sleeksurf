<%@ Page Title="" Language="C#" MasterPageFile="~/SleekSurf.Master" AutoEventWireup="true"
    CodeBehind="PasswordRecovery.aspx.cs" Inherits="SleekSurf.Web.PasswordRecovery" %>

<%@ Register Src="~/WebPageControls/PasswordRecovery.ascx" TagName="RecoverPassword"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript">
    $(function () {
        $(".smallTitle span").css("background-color", "#edefff");
    });
</script>
    <div id="PasswordRecovery">
        <fieldset>
            <legend>Recover Your Password</legend>
            <uc:RecoverPassword runat="server" ID="PwdRecovery" />
        </fieldset>
    </div>
</asp:Content>
