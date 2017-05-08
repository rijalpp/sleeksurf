<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HeaderTopMenu.ascx.cs" Inherits="SleekSurf.Web.MasterPageControls.HeaderTopMenu" %>
<%@ Register Src="~/WebPageControls/UserLogin.ascx" TagName="UserLogin" TagPrefix="uc" %>
<div id="userBarSection">
    <div class="loginDisplay">
        <uc:UserLogin ID="ucUserLogin" runat="server" />
    </div>
</div>