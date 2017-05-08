using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SleekSurf.FrameWork
{
    public static class Redirector
    {
        private static void Redirect(string url)
        {
            HttpContext.Current.Response.Redirect(url);
        }

        public static void GoToRequestedPage(string url)
        {
            Redirect(url);
        }

        public static void GoToHomePage()
        {
            Redirect("~/Default.aspx");
        }

        public static void GoToSleekSurfWebsite()
        {
            Redirect((string)Configuration.GetConfigurationSetting("Website", typeof(string)));
        }

        public static void GoToWebsiteUnavailablePage()
        {
            Redirect("~/CustomErrorPages/WebsiteUnavailable.aspx");
        }

        public static void GoToAccessDeniedPage()
        {
            Redirect("~/CustomErrorPages/AccessDenied.aspx");
        }

        public static void GoToPackagePage()
        {
            Redirect("~/WebPages/BrowsePackages.aspx");
        }

        public static void GoToAdminHomePage()
        {
            Redirect("~/Admin/Default.aspx");
        }

        public static void GoToAdminAccessDeniedPage()
        {
            Redirect("~/Admin/AccessDenied.aspx");
        }

        public static void GoToAdminAccessSuspendedPage()
        {
            Redirect("~/Admin/Client/AccessSuspended.aspx");
        }

        public static void GoToLoginPage()
        {
            Redirect("~/WebPages/Login.aspx");
        }
    }
}
