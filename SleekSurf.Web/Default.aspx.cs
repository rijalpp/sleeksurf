using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;

namespace SleekSurf.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string subDomain = Helpers.ExtractSubDomain(Request.Url);
            if (!string.IsNullOrEmpty(subDomain))
                Response.RedirectToRoute("BusinessProfileDefault", new { uniqueIdentity = subDomain });
        }
    }
}