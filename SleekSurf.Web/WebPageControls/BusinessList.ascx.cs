using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using SleekSurf.Manager;
using SleekSurf.FrameWork;
using SleekSurf.Entity;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Web.Script.Serialization;
using System.Device.Location;
using SleekSurf.FrameWork.ApiObjects;


namespace SleekSurf.Web.WebPageControls
{
    public partial class BusinessList : System.Web.UI.UserControl
    {
        PagingDetails pEx = new PagingDetails();
        int pageNo = 0;
        bool samePage = false;
        string Name = string.Empty;
        string Cat = string.Empty;
        string Location = string.Empty;
        int totalPageNo = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Page"] != null)
            {
                pageNo = int.Parse(Request.QueryString["Page"]);
            }
            if (Request.QueryString["Name"] != null)
            {
                Name = Request.QueryString["Name"];
                Name = Name.Replace("-", " ");
            }

            if (Request.QueryString["Cat"] != null)
            {
                Cat = Request.QueryString["Cat"];
                Cat = Cat.Replace("-", " ");
            }

            if (Request.QueryString["Location"] != null)
            {
                Location = Request.QueryString["Location"];
                Location = Location.Replace("-", " ");
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!samePage)
                populateControl();
        }

        protected void btnViewProfile_Command(object sender, CommandEventArgs e)
        {
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
            //Redirector.GoToRequestedPage("http://www." + e.CommandArgument.ToString() + ".sleeksurf.com");
            // Redirector.GoToRequestedPage("http://www.sleeksurf.com/" + e.CommandArgument.ToString());
            Redirector.GoToRequestedPage(baseUrl + e.CommandArgument.ToString());
        }

        //PUBLIC PROPERTIES
        public string PubName
        {
            get { return Name; }
        }

        public string PubCat
        {
            get { return Cat; }
        }

        public string PubLocation
        {
            // get { return Location; }
            get
            {
                if (Location == "nearby")
                    return string.Empty;
                else
                    return Location;
            }
        }

        protected void rptrBusinessList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ClientDetails client = (ClientDetails)e.Item.DataItem;
                HtmlGenericControl website = (HtmlGenericControl)e.Item.FindControl("Website");
                HtmlGenericControl fax = (HtmlGenericControl)e.Item.FindControl("ContactFax");
                website.Visible = !string.IsNullOrEmpty(client.BusinessUrl);
                fax.Visible = !string.IsNullOrEmpty(client.ContactFax);
                HtmlAnchor anchorLogo = (HtmlAnchor)e.Item.FindControl("anchorLogo");
                HtmlAnchor anchorBusinessName = (HtmlAnchor)e.Item.FindControl("anchorBusinessName");
                Button btnViewProfile = (Button)e.Item.FindControl("btnViewProfile");
                Result<ClientFeatureDetails> result = ClientManager.SelectClientFeatureDetails(client.ClientID);
                if (result.Status == ResultStatus.Success)
                {
                    if (result.EntityList[0].ClientProfile && !string.IsNullOrEmpty(client.UniqueIdentity))
                    {
                        //anchorLogo.HRef = anchorBusinessName.HRef = "http://www." + client.UniqueIdentity + ".sleeksurf.com";
                        anchorLogo.HRef = anchorBusinessName.HRef = "http://www.sleeksurf.com/" + client.UniqueIdentity;
                        btnViewProfile.Enabled = true;
                        btnViewProfile.Text = "View Profile";
                    }

                }

                //For validation
                TextBox txtSendEmail = e.Item.FindControl("txtSendEmail") as TextBox;
                RequiredFieldValidator rfvSendEmail = e.Item.FindControl("rfvSendEmail") as RequiredFieldValidator;
                RegularExpressionValidator rexBusinessEmail = e.Item.FindControl("rexBusinessEmail") as RegularExpressionValidator;
                ImageButton ibtnSendEmail = e.Item.FindControl("ibtnSendEmail") as ImageButton;

                txtSendEmail.ValidationGroup = rfvSendEmail.ValidationGroup = rexBusinessEmail.ValidationGroup = ibtnSendEmail.ValidationGroup = "SendEmail" + e.Item.ItemIndex.ToString();
            }
        }

        public void populateControl()
        {
            if (pageNo == 0) pageNo = 1;
            pEx.StartRowIndex = pageNo;
            pEx.PageSize = 5;
            // Retrieve Search string from query string
            string firstPageUrl = "";
            string pagerFormat = "";
            //------------------//
            string businessName = "\"" + string.Join("*\" OR \"", SplitWord(Name)) + "*\"";
            string category = null;
            if (!string.IsNullOrEmpty(Cat))
                category = Cat;
            //GET THE NEAREST BUSINESSES
            if (Location == "nearby")
            {
                searchNearByLocation(businessName, category);
            }
            //GET THE BUSINESSES BASED ON SEARCH
            else
            {
                searchInGeneral(businessName, category);
            }
            
            lblMessage.CssClass = "normalMsg";
            // Display pager
            if ((!string.IsNullOrEmpty(Location)) || (!string.IsNullOrEmpty(Name)))
            {
                if (string.IsNullOrEmpty(Cat))
                {
                    firstPageUrl = Helpers.ToSearch("1", Name, Location);
                    pagerFormat = Helpers.ToSearch("{0}", Name, Location);
                }
                else
                {
                    firstPageUrl = Helpers.ToSearch("1", Name, Cat, Location);
                    pagerFormat = Helpers.ToSearch("{0}", Name, Cat, Location);
                }
            }
            else
            {
                firstPageUrl = Helpers.ToSearch("1");
                pagerFormat = Helpers.ToSearch("{0}");
            }
            int currentPage = pEx.StartRowIndex;
            // firstPageUrl = UrlLink.toCatalog("1");
            totalPageNo = Helpers.GetTotalPages(pEx.TotalNumber, pEx.PageSize);
            topPager.Show(pEx.TotalNumber, pEx.StartRowIndex, totalPageNo, firstPageUrl, pagerFormat, false);
            bottomPager.Show(pEx.TotalNumber, pEx.StartRowIndex, totalPageNo, firstPageUrl, pagerFormat, true);
        }

        protected string GetContactPerson(string UserID)
        {
            Guid Userid = Guid.Parse(UserID);
            CustomUserProfile profile = CustomUserProfile.GetUserProfile(Membership.GetUser(Userid).UserName);
            return profile.FirstName + " " + profile.MiddleName + " " + profile.LastName;
        }

        protected string GetCountry(object Country)
        {
            CountryDetails thisCountry = (CountryDetails)Country;
            return CountryManager.GetCountry(thisCountry.CountryID).EntityList[0].CountryName;
        }

        protected string ProfileImageSource(string clientID, string logoUrl)
        {
            string url = Server.MapPath("~/Uploads/" + clientID + "/LogoPicture/" + logoUrl);
            if (File.Exists(url))
                return "../Uploads/" + clientID + "/LogoPicture/" + logoUrl;
            else
            {
                ClientDetails selectedClient = ClientManager.SelectClient(clientID).EntityList[0];
                if (selectedClient != null)
                {
                    return "../DisplayImage.aspx?ID=" + selectedClient.Category.CategoryID;
                }
                else
                    return "../App_Themes/SleekTheme/Images/BusinessLogoDefault.png";
            }

        }

        private string[] SplitWord(string keywords)
        {
            return Regex.Split(keywords, @"\W+");
        }

        private void searchInGeneral(string businessName, string category)
        {
            try
            {
               string address = "\"" + string.Join("*\" OR \"", SplitWord(Location)) + "*\"";
                rptrBusinessList.DataSource = ClientManager.SelectClientByBusinessNameCategoryAddress(businessName, category, address, pEx).EntityList;
                rptrBusinessList.DataBind();
                if (rptrBusinessList.Items.Count == 0)
                {
                    lblMessage.Text = "No results found in " + Location;
                    TopNavigation.Visible = false;
                    BottomNavigation.Visible = false;
                }
                else
                {
                    lblMessage.Text = pEx.TotalNumber + "<span style='font-weight:normal'> results found in </span> " + Location;
                    TopNavigation.Visible = true;
                    BottomNavigation.Visible = true;
                }

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void searchNearByLocation(string businessName, string category)
        {
            try
            {
                //Getting the current device location
                GeoCoordinate coord = Helpers.CurrentDeviceLocation;
                IpLocationInfo ipLoc = new IpLocationInfo();
                if (coord.IsUnknown == false)
                {
                    ipLoc.latitude = (decimal)coord.Latitude;
                    ipLoc.longitude = (decimal)coord.Longitude;
                }
                else
                {
                    //device location
                    //decimal radius = 0.04M;
                    decimal radius = 0.34M;
                    string ip = Helpers.CurrentUserIP;
                   // ip = "14.202.217.85";//TO: remove on release.
                    string UserIPGeoKey = "UserIPGeo_" + ip;
                    if (Helpers.Cache[UserIPGeoKey] == null)
                    {
                        string geoLocationDetails = string.Empty;
                        string geoAPI = @"http://api.geoio.com/q.php?key=v6XPIplLAcy61Kta&qt=geoip&d=pipe&q=" + ip;
                        geoAPI = @"http://freegeoip.net/json/" + ip;

                        var client = new RestClient();
                        client.EndPoint = @"http://freegeoip.net/json/" + ip;
                        client.Method = HttpVerb.GET;

                        var json = client.MakeRequest();
                        JavaScriptSerializer des = new JavaScriptSerializer();
                        ipLoc = (IpLocationInfo)des.Deserialize(json, typeof(IpLocationInfo));
                        Helpers.CacheData(UserIPGeoKey, ipLoc);
                    }
                    else
                    {
                        ipLoc = (IpLocationInfo)Helpers.Cache[UserIPGeoKey];
                    }
                    rptrBusinessList.DataSource = ClientManager.SelectClientByBusinessNameCategoryGeoLocation(businessName, category, ipLoc.latitude, ipLoc.longitude, radius, pEx).EntityList;
                    rptrBusinessList.DataBind();
                    if (rptrBusinessList.Items.Count == 0)
                    {
                        lblMessage.Text = "No results found in " + Location +" your location.";
                        TopNavigation.Visible = false;
                        BottomNavigation.Visible = false;
                    }
                    else
                    {
                        lblMessage.Text = pEx.TotalNumber + "<span style='font-weight:normal'> results found in </span> " + Location + " your location.";
                        TopNavigation.Visible = true;
                        BottomNavigation.Visible = true;
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected void ibtnSendEmail_Command(object sender, CommandEventArgs e)
        {
            samePage = true;
            int temp = Convert.ToInt32(e.CommandArgument);
            TextBox txtSendEmail = (TextBox)rptrBusinessList.Items[temp].FindControl("txtSendEmail");

            Result<ClientDetails> resultClient = ClientManager.SelectClient(e.CommandName);

            if (resultClient.Status == ResultStatus.Success)
            {
                ClientDetails client = resultClient.EntityList[0];
                //embed image in the emails
                // string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
                string logoUrl = "";
                string logoDisplay = "none";
                if (!string.IsNullOrEmpty(client.LogoUrl))
                {
                    string urlPath = Server.MapPath("~/Uploads/" + client.ClientID + "/LogoPicture/" + client.LogoUrl);
                    if (System.IO.File.Exists(urlPath))
                    {
                        logoUrl = BasePage.FullBaseUrl + "Uploads/" + client.ClientID + "/LogoPicture/" + client.LogoUrl;
                        logoDisplay = "block";
                    }
                }
                string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
                string appPath = Request.PhysicalApplicationPath;

                StreamReader bodyBusinessReader = new StreamReader(appPath + "EmailTemplates/EmailBusinessDetails.txt");
                string bodyBusiness = bodyBusinessReader.ReadToEnd();
                bodyBusinessReader.Close();

                bodyBusiness = bodyBusiness.Replace("<%Logo%>", logoUrl);
                bodyBusiness = bodyBusiness.Replace("<%LogoDisplay%>", logoDisplay);
                bodyBusiness = bodyBusiness.Replace("<%TopBackGround%>", topBackGroundUrl);
                bodyBusiness = bodyBusiness.Replace("<%BusinessName%>", client.ClientName);
                bodyBusiness = bodyBusiness.Replace("<%ContactPerson%>", GetContactPerson(client.ContactPerson.ToString()));
                bodyBusiness = bodyBusiness.Replace("<%ContactNumber%>", client.ContactOffice);
                bodyBusiness = bodyBusiness.Replace("<%BusinessEmail%>", client.BusinessEmail);
                bodyBusiness = bodyBusiness.Replace("<%Address%>", client.Address);
                bodyBusiness = bodyBusiness.Replace("<%Country%>", GetCountry(client.CountryID));

                string otherDetails = string.Empty;

                if (!string.IsNullOrEmpty(client.BusinessUrl))
                {
                    otherDetails += "<span style='display:block; padding:2px 0px;'><span style='display:inline-block; width:100px; font-weight:bold; padding-right:10px; font-size:12px;'>Website: </span><span style='display:inline-block; width:300px; font-size:12px;'>" + client.BusinessUrl + "</span></span>";
                }

                if (!string.IsNullOrEmpty(client.ContactFax))
                {
                    otherDetails += "<span style='display:block; padding:2px 0px;'><span style='display:inline-block; width:100px; font-weight:bold; padding-right:10px; font-size:12px;'>Fax: </span><span style='display:inline-block; width:300px; font-size:12px;'>" + client.ContactFax + "</span></span>";
                }

                if (!string.IsNullOrEmpty(client.UniqueDomain) || !string.IsNullOrEmpty(client.UniqueIdentity)) // IF BUSINESS HAS PROFILE OV BUSINESS.
                {
                    bodyBusiness = bodyBusiness.Replace("<%OtherDetails%>", otherDetails);
                    bodyBusiness = bodyBusiness.Replace("<%otherDescription%>", "For maps, promotions and other information in details, please visit the business profile by clicking below.");
                    if (!string.IsNullOrEmpty(client.UniqueDomain) && !string.IsNullOrEmpty(client.UniqueIdentity))
                        bodyBusiness = bodyBusiness.Replace("<%BusinessProfile%>", "<a href='http://www." + client.UniqueIdentity + ".sleeksurf.com'>http://www." + client.UniqueIdentity + ".sleeksurf.com</a><br /> or <br /> <a href='http://www.sleeksurf.com/" + client.UniqueIdentity + "'>http://www.sleeksurf.com/" + client.UniqueIdentity + "</a> <br />or<br /><a href='" + client.UniqueDomain + "'>'" + client.UniqueDomain + "'</a>");
                    else if (string.IsNullOrEmpty(client.UniqueDomain))
                        bodyBusiness = bodyBusiness.Replace("<%BusinessProfile%>", "<a href='http://www." + client.UniqueIdentity + ".sleeksurf.com'>http://www." + client.UniqueIdentity + ".sleeksurf.com</a><br /> or <br /> <a href='http://www.sleeksurf.com/" + client.UniqueIdentity + "'>http://www.sleeksurf.com/" + client.UniqueIdentity + "</a>");
                    else
                        bodyBusiness = bodyBusiness.Replace("<%BusinessProfile%>", client.UniqueDomain);
                }

                else//IF THE BUSINESS IS ONLY LISTED
                {
                    bodyBusiness = bodyBusiness.Replace("<%OtherDetails%>", "");
                    bodyBusiness = bodyBusiness.Replace("<%otherDescription%>", "");
                    bodyBusiness = bodyBusiness.Replace("<%BusinessProfile%>", "");
                }

                try
                {
                    Helpers.SendEmail("noreply@sleeksurf.com", "SleekSurf", txtSendEmail.Text.Trim(), "Business Details", bodyBusiness);
                    populateControl();
                    Label lblMessage = (Label)rptrBusinessList.Items[temp].FindControl("lblMessage");
                    lblMessage.CssClass = "success";
                    lblMessage.Text = "Details sent to your nominated email.";
                }
                catch (Exception ex)
                {
                    Helpers.LogError(ex);
                    lblMessage.CssClass = "error";
                    lblMessage.Text = "Error Sending Email, Please try later.";
                }

            }
        }

    }

}