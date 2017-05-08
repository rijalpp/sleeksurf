<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowClientCustomers.aspx.cs"
    Inherits="SleekSurf.Web.Admin.Client.ShowClientCustomers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/LoginDialog/jquery-1.4.3.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#checkAll").attr("checked", $("[id*=chkCustomer]:checked").length == $("[id*=chkCustomer]:checkbox").length);
            $("#lblCountSelection").text($("[id*=chkCustomer]:checked").length + " Customers Selected");

            $("#checkAll").click(function () {
                $("[id*=chkCustomer]").attr("checked", $(this).is(":checked"));
                $("#lblCountSelection").text($("[id*=chkCustomer]:checked").length + " Customers Selected");
            });

            $("[id*=chkCustomer]").click(function () {
                $("#checkAll").attr("checked", $("[id*=chkCustomer]:checked").length == $("[id*=chkCustomer]:checkbox").length);
                $("#lblCountSelection").text($("[id*=chkCustomer]:checked").length + " Customers Selected");
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="Container">
        <p class="countDescription">
            <asp:Label ID="lblCountSelection" runat="server"></asp:Label>
        </p>
        <div id="CustomerList">
            <asp:GridView ID="gvCustomerManagement" runat="server" DataKeyNames="CustomerID, ClientID"
                AutoGenerateColumns="false" OnRowDataBound="gvCustomerManagement_RowDataBound" Width="700px">
                <EmptyDataTemplate>
                    No Customers found in the Database
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <input id="checkAll" type="checkbox">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkCustomer" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle Width="5px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Image">
                        <ItemTemplate>
                            <a href='NewEditCustomer.aspx?CustomerID=<%#Eval("CustomerID") %>'>
                                <img runat="server" id="iThumb" style="width: 40px; border: 1px solid #46a8f3;" alt='<%#Eval("FullName") %>'
                                    title='<%#Eval("FullName") %>' /></a>
                        </ItemTemplate>
                        <HeaderStyle Width="40px" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Name" DataField="FullName" ReadOnly="true" HeaderStyle-Width="250px" />
                    <asp:BoundField HeaderText="Email" DataField="Email" ReadOnly="true" HeaderStyle-Width="200px" />
                    <asp:BoundField HeaderText="Mobile" DataField="ContactMobile" ReadOnly="true" HeaderStyle-Width="100px" />
                </Columns>
            </asp:GridView>
        </div>
        <div id="SignUpNavigation" style="width:690px">
            <div style="float: right;">
                <span style="margin: 0px 15px;">
                    <asp:Button ID="btnFinish" runat="server" Text="Finish" SkinID="Button"
                        OnClick="btnFinish_Click" /></span>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
