using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Manager;
using SleekSurf.Entity;
using SleekSurf.FrameWork;

namespace SleekSurf.Web.WebPages
{
    public partial class FAQs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadFaqs();

            Result<AdvertisementDetails> resultRightAds = AdvertisementManager.SelectRandomAddsWithoutClient("right");

            if (resultRightAds.Status == ResultStatus.NotFound)
                DefaultRightAd.Visible = true;
            else
            {
                rptrRightAds.DataSource = resultRightAds.EntityList;
                rptrRightAds.DataBind();
            }
        }

        protected void rptFaqGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                string faqGroupID = ((Label)item.FindControl("lblFaqGroupID")).Text;
                Repeater rptFaqs = (Repeater)item.FindControl("rptFaqs");

                Result<FAQDetails> FAQResult = ClientManager.SelectFaqByFaqGroup(faqGroupID);
                if (FAQResult.EntityList.Count > 0)
                {
                    rptFaqs.DataSource = FAQResult.EntityList;
                    rptFaqs.DataBind();
                }
                else
                    e.Item.Visible = false;
            }
        }

        private void LoadFaqs()
        {
            rptFaqGroups.DataSource = ClientManager.SelectFaqGroupWithClientNull().EntityList;
            rptFaqGroups.DataBind();
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