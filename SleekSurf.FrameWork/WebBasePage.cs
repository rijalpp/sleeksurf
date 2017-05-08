using System;
using System.Web.UI;

namespace SleekSurf.FrameWork
{
    public class WebBasePage : Page
    {
        public WebBasePage()
        {
            this.PreInit += new EventHandler(WebBasePage_PreInit);
        }

        void WebBasePage_PreInit(object sender, EventArgs e)
        {
            if (WebContext.ClientProfile != null)
            {
                Theme = WebContext.ClientProfile.Theme;
                if(string.IsNullOrEmpty(Theme))
                    Theme = "Default";
            }
            else
                Theme = "Default";
        }
    }
}
