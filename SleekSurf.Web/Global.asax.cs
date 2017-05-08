using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using SleekSurf.FrameWork;
using System.Web.Profile;

namespace SleekSurf.Web
{
    public class Global : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Ignore("{resource}.axd/{*pathInfo}");
            routes.MapPageRoute("BusinessProfileDefault", "{uniqueIdentity}", "~/Client/Default.aspx");
            routes.MapPageRoute("BusinessProfileHome", "{uniqueIdentity}/Home", "~/Client/Default.aspx");
            routes.MapPageRoute("BusinessProfilePromotion", "{uniqueIdentity}/Promotion", "~/Client/Promotion.aspx");
            routes.MapPageRoute("BusinessProfilePromotions", "{uniqueIdentity}/Promotions", "~/Client/Promotion.aspx");
            routes.MapPageRoute("BusinessProfilePromotionInDetails", "{uniqueIdentity}/Promotions/PromotionInDetails", "~/Client/PromotionInDetails.aspx");
            routes.MapPageRoute("BusinessProfileRegister", "{uniqueIdentity}/Register", "~/Client/RegisterCustomer.aspx");
            routes.MapPageRoute("BusinessProfileAboutUs", "{uniqueIdentity}/AboutUs", "~/Client/AboutUs.aspx");
            routes.MapPageRoute("BusinessProfileContactUs", "{uniqueIdentity}/ContactUs", "~/Client/ContactUs.aspx");
            routes.MapPageRoute("BusinessProfileTermsAndConditions", "{uniqueIdentity}/TermsAndConditions", "~/Client/TermsAndConditions.aspx");
            routes.MapPageRoute("BusinessProfilePrivacyPolicy", "{uniqueIdentity}/PrivacyPolicy", "~/Client/PrivacyPolicy.aspx");
            routes.MapPageRoute("BusinessProfileFAQ", "{uniqueIdentity}/FAQ", "~/Client/FAQs.aspx");
            routes.MapPageRoute("BusinessProfileFAQs", "{uniqueIdentity}/FAQs", "~/Client/FAQs.aspx");
            routes.MapPageRoute("BusinessProfileDefaultRoute", "{uniqueIdentity}/{*wildcard}", "~/Client/Default.aspx");
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        void Profile_MigrateAnonymous(object sender, ProfileMigrateEventArgs e)
        {
            CustomUserProfile anonProfile = CustomUserProfile.GetUserProfile(e.AnonymousID);
        }
    }
}