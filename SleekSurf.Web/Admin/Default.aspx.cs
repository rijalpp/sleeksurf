using System;
using SleekSurf.FrameWork;
using SleekSurf.Manager;

namespace SleekSurf.Web.Admin
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.IsInRole("SuperAdmin") || User.IsInRole("SuperAdminUser"))
            {
                Response.Redirect("~/Admin/SuperAdmin/UserManagement.aspx");
            }
            else if (User.IsInRole("MarketingOfficer"))
            {
                Response.Redirect("~/Admin/SuperAdmin/ClientManagement.aspx");
            }
            else if (User.IsInRole("Admin") || (User.IsInRole("AdminUser")))
            {
                Response.Redirect("~/Admin/Client/AccountManagement.aspx");
            }
        }
    }
}