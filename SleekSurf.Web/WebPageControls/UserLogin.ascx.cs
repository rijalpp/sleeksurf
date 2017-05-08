using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;

namespace SleekSurf.Web.WebPageControls
{
    public partial class UserLogin : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoginStatusLS_LoggingOut(object sender, EventArgs e)
        {
            WebContext.ClearSession();
        }
    }
}