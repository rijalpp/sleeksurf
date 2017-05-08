<%@ Page Title="" Language="C#" MasterPageFile="~/ClientPageSite.master" AutoEventWireup="true" CodeBehind="RegisterCustomer.aspx.cs" Inherits="SleekSurf.Domain.RegisterCustomer" %>
<%@ Register Src="~/WebPageControls/NewEditCustomer.ascx" TagName="NewEditCutsomer" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
 <script language="javascript" type="text/javascript">
      <script type="text/javascript">
    //DATE INPUT FUNCTION STARTS HERE
    //key input validation for date starts
    //date code
    function keyUpActionForDate(txtDate) {
        document.getElementById(txtDate).style.borderColor = "";
        //document.getElementById('divMessage').innerHTML="";
        NumericCheck(txtDate);
        if (event.keyCode != 8) {
            var varDate = document.getElementById(txtDate).value;
            if (varDate.length == 2 || varDate.length == 5)
                document.getElementById(txtDate).value = varDate + "/";
            else if (varDate.length == 3 && varDate.substring(2) != "/")
                document.getElementById(txtDate).value = varDate.substring(0, 2) + "/" + varDate.substring(2);
            else if (varDate.length == 6 && varDate.substring(5) != "/")
                document.getElementById(txtDate).value = varDate.substring(0, 5) + "/" + varDate.substring(5);
        }
    }
    function NumericCheck(txtDate) {
        //debugger;
        var varDate = document.getElementById(txtDate).value;
        if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || (event.keyCode == 8) || (event.keyCode == 12) || (event.keyCode == 27)) {
            return true;
        }
        else {
            document.getElementById(txtDate).value = varDate.substring(0, varDate.length - 1);
            return false;
        }
    }
    //key input validation for date ends
    //key down evenr
    var isShift = false;
    function keyDownActionForDate(txtDate) {
        if (event.keyCode == 16)
            isShift = true;
        //Validate that its Numeric
        if (((event.keyCode >= 48 && event.keyCode <= 57) || event.keyCode == 8 || event.keyCode <= 37 || event.keyCode <= 39 || (event.keyCode >= 96 && event.keyCode <= 105)) && isShift == false)
            return true;
        else
            return false;
    }
</script>
 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPage" runat="server">
    <div style="padding-bottom: 20px;">
        <div>
            <h2 class="header" style="color: #777777;">
                Customer Registration</h2>
            <p>
                Please fill up the form below in order to stay in touch with us with our latest
                promotions, offers, news and many more.
            </p>
        </div>
        <div id="SignUpContentDetails" style="margin: 0px;">
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            <uc1:NewEditCutsomer ID="ucNewEditCustomer" runat="server"></uc1:NewEditCutsomer>
        </div>
        <div id="SignUpNavigation">
            <div style="float: left; margin-right: 20px; padding-left: 15px;">
                <span class="mandatory">( * ) <span class="mandatoryText">Mandatory Fields</span></span>
            </div>
            <div style="float: right;">
                <span style="margin: 0px 15px;">
                    <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="valgRegistration"
                        SkinID="Button" OnClick="btnSave_Click" /></span>
            </div>
        </div>
    </div>
</asp:Content>
