<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SANavigationMenu.ascx.cs" Inherits="SleekSurf.Web.Admin.UserControls.SANavigationMenu" %>

<div id="TabbedMenu">
    <asp:Menu ID="Menu" CssClass="menuTab" StaticMenuItemStyle-CssClass="tab" Orientation="Horizontal"
        runat="server" OnMenuItemClick="Menu_MenuItemClick">
        <StaticItemTemplate>
            <span>
                <asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Text") %>' />
            </span>
            <div>
            </div>
        </StaticItemTemplate>
    </asp:Menu>
    <div class="tabbedMenuBase">
    </div>
</div>