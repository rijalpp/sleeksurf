using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;

namespace SleekSurf.Domain.CustomErrorPages
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ltrClientName.Text = WebContext.ClientProfile.ClientName; 
        }
    }
}