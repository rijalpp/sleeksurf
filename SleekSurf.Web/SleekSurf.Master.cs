using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using System.Web.Security;
using SleekSurf.Entity;
using SleekSurf.Manager;

namespace SleekSurf.Web
{
    public partial class SleekSurf : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                WebContext.CurrentUser = HttpContext.Current.User;
                if (HttpContext.Current.User.IsInRole("Admin") || HttpContext.Current.User.IsInRole("AdminUser"))
                {
                    MembershipUser user = Membership.GetUser(WebContext.CurrentUser.Identity.Name);
                    string clientID = ClientManager.SelectProfileClientID((Guid)user.ProviderUserKey);
                    ClientDetails client = ClientManager.SelectClient(clientID).EntityList[0];
                    WebContext.Parent = client;
                }
            }

            if (!IsPostBack)
            {
                Result<AdvertisementDetails> resultFooterAds = AdvertisementManager.SelectRandomAddsWithoutClient("footer");

                if (resultFooterAds.Status == ResultStatus.NotFound)
                    DefaultFooterAd.Visible = true;
                else
                {
                    rptrFooterAds.DataSource = resultFooterAds.EntityList;
                    rptrFooterAds.DataBind();
                }
            }
        }

        public void DisplayFooterAdsSectionMain(bool assign)
        {
            FooterAdsSectionMain.Visible = assign;
        }

        public void DisplayFooterAdd(bool assign)
        {
            DefaultFooterAd.Visible = assign;
        }

        public string MasterBusinessName
        {
            get { return ucSearchBusiness.BusinessName; }
            set { ucSearchBusiness.BusinessName = value; }
        }

        public string MasterBusinessCategory
        {
            get { return ucSearchBusiness.BusinessCategory; }
            set { ucSearchBusiness.BusinessCategory = value; }

        }

        public string MasterBusinessLocation
        {
            get { return ucSearchBusiness.BusinessLocation; }
            set { ucSearchBusiness.BusinessLocation = value; }
        }

        protected void rptrFooterAds_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            AdvertisementDetails ad = (AdvertisementDetails)e.Item.DataItem;

            Image imgAd = (Image)e.Item.FindControl("imgAd");
            imgAd.ImageUrl = "~/Uploads/" + "Advertisements/" + ad.ImageUrl;

            string[] dimension = Enum.GetName(typeof(AdDimensionFooter), ad.FitToPanel).Replace('d', ' ').Trim().Split('x');
            imgAd.Width = Convert.ToInt32(dimension[0]);
            imgAd.Height = Convert.ToInt32(dimension[1]);
        }
    }
}