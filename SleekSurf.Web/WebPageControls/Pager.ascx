<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pager.ascx.cs" Inherits="SleekSurf.Web.WebPageControls.Pager" %>
<div id="Pager">
    <span class="left">
        <asp:Literal ID="ltrCurrentpage" runat="server"></asp:Literal>
        <asp:Literal ID="ltrHowManyPages" runat="server"></asp:Literal>
    </span><span class="middle">
        <asp:LinkButton ID="previousLink" runat="server" Text="Prev" SkinID="Pagination"></asp:LinkButton>
        <asp:Repeater ID="pagesRepeater" runat="server">
            <ItemTemplate>
                <asp:LinkButton ID="hyperlink" runat="server" Text='<%# Eval("Page") %>' PostBackUrl='<%# Eval("Url") %>' SkinID="Pagination" />
            </ItemTemplate>
        </asp:Repeater>
        <asp:LinkButton ID="nextLink" runat="server" Text="Next" SkinID="Pagination"></asp:LinkButton>
    </span><span class="right">
        <asp:Literal ID="ltrTotalRecord" runat="server"></asp:Literal>
    </span>
</div>
