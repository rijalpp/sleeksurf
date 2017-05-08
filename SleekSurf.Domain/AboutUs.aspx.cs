using System;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using SleekSurf.Manager;

namespace SleekSurf.Domain
{
    public partial class AboutUs : WebBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)(Master.Master.FindControl("NavigationMenu"));
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("About Us"))].Selected = true;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (WebContext.ClientProfile != null)
            {
                Result<ServiceDetails> serviceResult = GetServices(WebContext.ClientProfile.ClientID);
                if (serviceResult.EntityList.Count > 0)
                {
                    rptrServices.DataSource = serviceResult.EntityList;
                    rptrServices.DataBind();
                }
                else
                    OurServices.Visible = false;
            }
        }

        private Result<ServiceDetails> GetServices(string clientID)
        {
            return ClientManager.SelectServicesForClient(clientID);
        }
    }
}