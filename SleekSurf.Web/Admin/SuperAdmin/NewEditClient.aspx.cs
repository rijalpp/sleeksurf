using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SleekSurf.Web.Admin.SuperAdmin
{
    public partial class NewEditClient : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RadioButtonList radio = ucNewEditAccount.RegoType;
                radio.SelectedValue = "Business";
                ((MultiView)ucNewEditAccount.FindControl("mvClientDetails")).ActiveViewIndex++;
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("ClientMgmt"))].Selected = true;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            DropDownList roleList = ucNewEditAccount.RoleList;
            roleList.Items.Remove("SuperAdmin");
            roleList.Items.Remove("SuperAdminUser");
            roleList.Items.Remove("AdminUser");
            roleList.Items.Remove("Customer");
            roleList.Items.Remove("User");
        }
    }
}