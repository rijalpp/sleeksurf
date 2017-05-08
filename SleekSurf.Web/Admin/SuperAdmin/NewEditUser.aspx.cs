using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;

namespace SleekSurf.Web.Admin.SuperAdmin
{
    public partial class NewEditUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RadioButtonList radio = ucNewEditAccount.RegoType;
                radio.SelectedValue = "Personal";
                ((MultiView)ucNewEditAccount.FindControl("mvClientDetails")).ActiveViewIndex++;
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("UserMgmt"))].Selected = true;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            DropDownList roleList = ucNewEditAccount.RoleList;

            foreach (var role in Enum.GetValues(typeof(ClientRoles)))
            {
                roleList.Items.Remove(new ListItem(role.ToString()));
            }
        }
    }
}