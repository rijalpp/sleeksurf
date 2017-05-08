using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SleekSurf.FrameWork
{
    public class BasePage : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public static string BaseUrl
        {
            get
            {
                string url = HttpContext.Current.Request.ApplicationPath;
                if (url.EndsWith("/"))
                    return url;
                else
                    return url + "/";
            }
        }

        public static string FullBaseUrl
        {
            get
            {
                return HttpContext.Current.Request.Url.AbsoluteUri.Replace(
                   HttpContext.Current.Request.Url.PathAndQuery, "") + BaseUrl;
            }
        }
        //search specific codes
        public string PrimaryKey(string vPrimaryKey)
        {
            if (null != ViewState[vPrimaryKey])
            {
                return ViewState[vPrimaryKey].ToString();
            }
            if (null != Request.QueryString[vPrimaryKey])
            {
                ViewState[vPrimaryKey] = Request.QueryString[vPrimaryKey];
                return Request.QueryString[vPrimaryKey];
            }
            return string.Empty;
        }
    }
}
