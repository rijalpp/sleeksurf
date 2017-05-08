using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Manager;
using SleekSurf.FrameWork;
using SleekSurf.Entity;

namespace SleekSurf.Web.Client
{
    public partial class ClientPageSite : System.Web.UI.MasterPage
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Result<AdvertisementDetails> resultRightAds = AdvertisementManager.SelectRandomAddsByClientID(WebContext.ClientProfile.ClientID, "right");

                if (resultRightAds.Status == ResultStatus.NotFound)
                    DefaultRightAd.Visible = true;
                else
                {
                    rptrRightAds.DataSource = resultRightAds.EntityList;
                    rptrRightAds.DataBind();
                }

                Result<AdvertisementDetails> resultFooterAds = AdvertisementManager.SelectRandomAddsByClientID(WebContext.ClientProfile.ClientID, "footer");

                if (resultFooterAds.Status == ResultStatus.NotFound)
                    DefaultFooterAd.Visible = true;
                else
                {
                    rptrFooterAds.DataSource = resultFooterAds.EntityList;
                    rptrFooterAds.DataBind();
                }
            }
        }

        protected void rptrRightAds_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            AdvertisementDetails ad = (AdvertisementDetails)e.Item.DataItem;

            Image imgAd = (Image)e.Item.FindControl("imgAd");
            imgAd.ImageUrl = "~/Uploads/" + WebContext.ClientProfile.ClientID + "/Advertisements/" + ad.ImageUrl;

            string[] dimension = Enum.GetName(typeof(AdDimensionRight),ad.FitToPanel).Replace('d', ' ').Trim().Split('x');
            imgAd.Width = Convert.ToInt32(dimension[0]);
            imgAd.Height = Convert.ToInt32(dimension[1]);
        }

        protected void rptrFooterAds_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            AdvertisementDetails ad = (AdvertisementDetails)e.Item.DataItem;

            Image imgAd = (Image)e.Item.FindControl("imgAd");
            imgAd.ImageUrl = "~/Uploads/" + WebContext.ClientProfile.ClientID + "/Advertisements/" + ad.ImageUrl;

            string[] dimension = Enum.GetName(typeof(AdDimensionFooter), ad.FitToPanel).Replace('d', ' ').Trim().Split('x');
            imgAd.Width = Convert.ToInt32(dimension[0]);
            imgAd.Height = Convert.ToInt32(dimension[1]);
        }
    }
}