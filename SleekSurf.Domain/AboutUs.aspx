<%@ Page Title="" Language="C#" MasterPageFile="~/ClientPageSite.master" AutoEventWireup="true"
    CodeBehind="AboutUs.aspx.cs" Inherits="SleekSurf.Domain.AboutUs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPage" runat="server">
    <div id="AboutUs">
        <h2 class="header" style="color: #777777;">
            About Us</h2>
        <p>
            <%=SleekSurf.FrameWork.WebContext.ClientProfile.Description.Replace(Environment.NewLine, "<br/>") %>
        </p>
    </div>
    <div id="OurServices" runat="server">
        <h2 class="header" style="color: #777777;">
            Our Services</h2>
        <div id="Services">
            <ul>
                <asp:Repeater ID="rptrServices" runat="server">
                    <ItemTemplate>
                        <li>
                            <%# Eval("ServiceDescription")%>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
    </div>
</asp:Content>
