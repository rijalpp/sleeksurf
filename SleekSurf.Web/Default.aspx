<%@ Page Title="" Language="C#" MasterPageFile="~/SleekSurf.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="SleekSurf.Web.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript">
    $(function () {
        $("#AdvertisementWrapper").css("display", "none");
    });
</script>
    <div id="TopContent">
        <div class="wrapContent">
            <div class="defineBusiness">
                <p>Re-define Your Business</p>
                <p>Connect right customers with your business and show them what your business offers.</p>
                <asp:ImageButton ImageUrl="~/App_Themes/SleekTheme/Images/AddBusiness.png" runat="server"
                    PostBackUrl="~/WebPages/NewAccount.aspx" />
            </div>
            <div class="featureHighlight">
                <div>
                    <asp:Image ID="imgUniqueURL" ImageUrl="~/App_Themes/SleekTheme/Images/UniqueUrl.png"
                        runat="server" />
                    <p>Get unique profile URL for your business.</p>
                </div>
                <div>
                    <asp:Image ID="imgSearch" ImageUrl="~/App_Themes/SleekTheme/Images/Search.png" runat="server" />
                    <p>Get searched and be selected by customers.</p>
                </div>
            </div>
        </div>
    </div>
    <div id="BottomContent">
        <div class="wrapContent">
            <div class="businessCyclePackage">
                <div>
                    <p>Sleek Surfing Cycle</p>
                    <ul id="CircularFlow">
                        <li class="business"><a>
                            <p>Your Business <span>will be recognised by customers via SleekSurf.</span></p>
                        </a></li>
                        <li class="sleekSurfEngine"><a>
                            <p>Sleeksurf Engine <span>helps to find your business profile</span></p>
                        </a></li>
                        <li class="uniqueProfile"><a>
                            <p>Profile <span>shows your services, features and what you offer to customers.</span></p>
                        </a></li>
                        <li class="connectingPeople"><a>
                            <p>Customers <span>get connected via unique profile Url to your business.</span></p>
                        </a></li>
                    </ul>
                </div>
                
                <div>
                    <p>Sleek Services</p>
                      <p>1. Add your business.</p>
                      <p>2. Get your business listed.</p>
                      <p>3. Get your profile pages created on registration.</p>
                      <p>4. Manage your profile.</p>
                         <ul style="padding-left:40px;">
                             <li>Update the content of your profile pages.</li>
                             <li>Add and Update Services and promotions of your business.</li>
                             <li>Send bulk emails to your customers at once.</li>
                             <li>Get your search and profile view analytics.</li>
                             <li>Send bulk SMS (charges apply).</li>
                         </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
