using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using SleekSurf.Entity;
using SleekSurf.Manager;

namespace SleekSurf.Web.Admin.Client
{
    public partial class RechargeSMS : System.Web.UI.Page
    {
        private static ClientFeatureDetails clientFeature = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
                clientFeature = ClientManager.SelectClientFeatureDetails(WebContext.Parent.ClientID).EntityList[0];
           
            if (clientFeature != null)
            {
                hlMatchProfile.Visible = !clientFeature.ClientProfile;
                hlMatchDomain.Visible = !clientFeature.ClientDomain;
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("ClientAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("AccountMgmt"))].Selected = true;

            if (WebContext.Sibling != null)
            {
                Menu tmpMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
                tmpMenu.Items[tmpMenu.Items.IndexOf(tmpMenu.FindItem("ClientMgmt"))].Selected = true;
            }
        }
    }
}