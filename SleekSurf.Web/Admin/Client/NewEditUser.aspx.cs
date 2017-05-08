using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;

namespace SleekSurf.Web.Admin.Client
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
            Menu tempMenu = (Menu)Master.FindControl("ClientAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("UserMgmt"))].Selected = true;

            if (WebContext.Sibling != null)
            {
                Menu tmpMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
                tmpMenu.Items[tmpMenu.Items.IndexOf(tmpMenu.FindItem("ClientMgmt"))].Selected = true;
            }

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            DropDownList roleList = ucNewEditAccount.RoleList;
            string[] clientRoles = Enum.GetNames(typeof(ClientRoles));
            List<string> notClientRoles = new List<string>();

            foreach (ListItem item in roleList.Items)
            {
                if (!clientRoles.Contains(item.Text) && item.Text != "Select Role")
                    notClientRoles.Add(item.Text);
            }

            foreach (string role in notClientRoles)
                roleList.Items.Remove(role);
        }
    }
}