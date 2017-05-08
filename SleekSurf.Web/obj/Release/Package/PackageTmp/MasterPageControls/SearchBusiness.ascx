<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchBusiness.ascx.cs"
    Inherits="SleekSurf.Web.MasterPageControls.SearchBusiness" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div id="BusinessSearch">
    <asp:Panel ID="pnBusinessSearch" runat="server" DefaultButton="btnSearchBusiness">
        <div class="textBoxSearchDiv">
            <p style="margin: 0px; padding-top: 0px; height: 29px; overflow: hidden;">
                <asp:TextBox ID="txtBusinessName" runat="server" SkinID="SearchBox"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderBusinessName" runat="server"
                    TargetControlID="txtBusinessName" WatermarkText="Business Name" WatermarkCssClass="waterMarkSearch" />
            </p>
            <p style="margin: 0px; padding-top: 0px;">
                <asp:Label ID="lblBusinessName" Text="eg: business name, company" runat="server"
                    SkinID="SmallExample"></asp:Label></p>
        </div>
        <div class="textBoxSearchDiv">
            <p style="margin: 0px; padding-top: 0px; height: 29px; overflow: hidden;">
                <asp:DropDownList ID="ddlCategory" runat="server" SkinID="SearchBox" CssClass="waterMarkSearch">
                </asp:DropDownList>
            </p>
            <p style="margin: 0px; padding-top: 0px">
                <asp:Label ID="lblCategory" Text="eg: migration, tax agent" runat="server" SkinID="SmallExample"></asp:Label></p>
        </div>
        <div class="textBoxSearchDiv">
            <p style="margin: 0px; padding-top: 0px; height: 29px; overflow: hidden;">
                <asp:TextBox ID="txtLocation" runat="server" SkinID="SearchBox"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderUsernameLocation" runat="server"
                    TargetControlID="txtLocation" WatermarkText="Location" WatermarkCssClass="waterMarkSearch" />
            </p>
            <p style="margin: 0px; padding-top: 0px">
                <asp:Label ID="lblLocation" Text="eg: city, state, postcode, country" runat="server"
                    SkinID="SmallExample"></asp:Label></p>
        </div>
        <div style="margin: 0px; margin-left: 12px; display:inline-block;">
            <asp:Button ID="btnSearchBusiness" runat="server" OnClick="btnSearchBusiness_Click"
                class="buttonSearchBusiness" />
            <asp:Button ID="btnNearLocation" runat="server" ToolTip="Nearby your location" OnClick="btnNearLocation_Click"
                class="buttonSearchBusinessNearby" />
        </div>
    </asp:Panel>
</div>
