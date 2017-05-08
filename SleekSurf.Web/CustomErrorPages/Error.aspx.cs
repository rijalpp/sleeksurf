using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SleekSurf.Web.CustomErrorPages
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ltr404.Visible = (this.Request.QueryString["code"] != null && this.Request.QueryString["code"] == "404");
            ltr408.Visible = (this.Request.QueryString["code"] != null && this.Request.QueryString["code"] == "408");
            ltr505.Visible = (this.Request.QueryString["code"] != null && this.Request.QueryString["code"] == "505");
            ltrError.Visible = (string.IsNullOrEmpty(this.Request.QueryString["code"]));
        }
    }
}