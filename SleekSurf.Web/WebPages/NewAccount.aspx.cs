using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using SleekSurf.Manager;

namespace SleekSurf.Web.WebPages
{
    public partial class NewAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RadioButtonList radio = NewAccounrRegistration.RegoType;
                radio.SelectedValue = "Business";
                ((MultiView)NewAccounrRegistration.FindControl("mvClientDetails")).ActiveViewIndex++;
            }

            if (((MultiView)NewAccounrRegistration.FindControl("mvClientDetails")).GetActiveView().ID == "vComplete")
                ltrRegoDesc.Visible = false;

            Result<AdvertisementDetails> resultRightAds = AdvertisementManager.SelectRandomAddsWithoutClient("right");

            if (resultRightAds.Status == ResultStatus.NotFound)
                DefaultRightAd.Visible = true;
            else
            {
                rptrRightAds.DataSource = resultRightAds.EntityList;
                rptrRightAds.DataBind();
            }
        }

        protected void rptrRightAds_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            AdvertisementDetails ad = (AdvertisementDetails)e.Item.DataItem;

            Image imgAd = (Image)e.Item.FindControl("imgAd");
            imgAd.ImageUrl = "~/Uploads/" + "Advertisements/" + ad.ImageUrl;

            string[] dimension = Enum.GetName(typeof(AdDimensionRight), ad.FitToPanel).Replace('d', ' ').Trim().Split('x');
            imgAd.Width = Convert.ToInt32(dimension[0]);
            imgAd.Height = Convert.ToInt32(dimension[1]);
        }
    }
}