using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Manager;
using SleekSurf.FrameWork;
using System.Data;

namespace SleekSurf.Web.Admin.Client
{
    public partial class ClientAnalytics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindHitAnalytics();
            }
        }

        private void BindHitAnalytics()
        {
            int totalVisits = 0;
            string hitType = WebContext.GetQueryStringValue("HitType").Trim();
            string searchDuration = WebContext.GetQueryStringValue("SearchDuration").Trim();
            DataTable dt = new DataTable();
           
            switch (hitType)
            {
                case "SearchHit":
                    switch (searchDuration)
                    {
                        case "1":
                        case "7":
                        case "30":
                        case "365":
                            DateTime dateFrom = DateTime.Now.AddDays(-(int.Parse(searchDuration)));
                            dt = ClientManager.HitCountrySelect(dateFrom, WebContext.Parent.ClientID, hitType);
                            foreach (DataRow dr in dt.Rows)
                            {
                                totalVisits += int.Parse(dr["NumberOfVisits"].ToString());
                            }
                            lblLocationDescription.Text = "Your profile has been <span style='color: #15ADFF'>found <span style='font: 100px Arial, Verdana, Helvetica, sans-serif; display:block'>" + totalVisits + "</span></span> time(s) since last <span style='color: #15ADFF'>" + searchDuration + "</span> day(s) from following locations.";
                            break;
                        default:
                            dt = ClientManager.HitCountrySelectAll(WebContext.Parent.ClientID, hitType);
                            foreach (DataRow dr in dt.Rows)
                            {
                                totalVisits += int.Parse(dr["NumberOfVisits"].ToString());
                            }
                            lblLocationDescription.Text = "Your profile has been <span style='color: #15ADFF'>found <span style='font: 100px Arial, Verdana, Helvetica, sans-serif; display:block'>" + totalVisits + "</span></span> time(s) since your joined date <span style='color: #15ADFF'>" + WebContext.Parent.CreatedDate.ToShortDateString() + "</span> from following locations.";
                            break;
                    }
                    break;
                default:
                    switch (searchDuration)
                    {
                        case "1":
                        case "7":
                        case "30":
                        case "365":
                            DateTime dateFrom = DateTime.Now.AddDays(-(int.Parse(searchDuration)));
                            dt = ClientManager.HitCountrySelect(dateFrom, WebContext.Parent.ClientID, hitType);
                            foreach (DataRow dr in dt.Rows)
                            {
                                totalVisits += int.Parse(dr["NumberOfVisits"].ToString());
                            }
                            lblLocationDescription.Text = "Your profile has been <span style='color: #15ADFF'>viewed <span style='font: 100px Arial, Verdana, Helvetica, sans-serif; display:block'>" + totalVisits + "</span></span> time(s) since last <span style='color: #15ADFF'>" + searchDuration + "</span> day(s) from following locations.";
                            break;
                        default:
                            dt = ClientManager.HitCountrySelectAll(WebContext.Parent.ClientID, hitType);
                            foreach (DataRow dr in dt.Rows)
                            {
                                totalVisits += int.Parse(dr["NumberOfVisits"].ToString());
                            }
                            lblLocationDescription.Text = "Your profile has been <span style='color: #15ADFF'>viewed <span style='font: 100px Arial, Verdana, Helvetica, sans-serif; display:block'>" + totalVisits + "</span></span> time(s) since your joined date <span style='color: #15ADFF'>" + WebContext.Parent.CreatedDate.ToShortDateString() + "</span> from following locations.";
                            break;
                    }
                    break;
            }

            if (totalVisits > 0)
            {
                rptrLocation.DataSource = dt;
                rptrLocation.DataBind();
            }
            else
                HitLocation.Visible = false;
        }

        protected void rptrLocation_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string hitType = WebContext.GetQueryStringValue("HitType");
            string searchDuration = WebContext.GetQueryStringValue("SearchDuration").Trim();

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string countryName = ((HiddenField)e.Item.FindControl("hfCountry")).Value;
                Repeater rptrLocationsCity = (Repeater)e.Item.FindControl("rptrLocationsCity");

                switch (searchDuration)
                {
                    case "1":
                    case "7":
                    case "30":
                    case "365":
                        DateTime dateFrom = DateTime.Now.AddDays(-(int.Parse(searchDuration)));
                        rptrLocationsCity.DataSource = ClientManager.HitCitySelect(dateFrom, countryName, WebContext.Parent.ClientID, hitType);
                        break;
                    default:
                        rptrLocationsCity.DataSource = ClientManager.HitCitySelectAll(countryName, WebContext.Parent.ClientID, hitType);
                        break;
                }
                
                rptrLocationsCity.DataBind();
            }
        }
    }
}