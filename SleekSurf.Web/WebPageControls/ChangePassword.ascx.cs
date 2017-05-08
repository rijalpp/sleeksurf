using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SleekSurf.Web.WebPageControls
{
    public partial class ChangePassword : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void cpChangePassword_ChangePasswordError(object sender, EventArgs e)
        {
            lblMessage.Text = "Error occured. The current password is wrong.";
            lblMessage.CssClass = "errorMsg";
        }
    }
}