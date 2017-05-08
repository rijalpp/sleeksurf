using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SleekSurf.FrameWork;
using System.Web.UI.WebControls;
using SleekSurf.Web.Admin.UserControls.Interfaces;
using SleekSurf.FrameWork.Interfaces;
using System.ComponentModel.Composition;

namespace SleekSurf.Web.Admin.UserControls.Presenters
{
    public class SANavigationMenuPresenter
    {
        private ISANavigationMenu view;

        [Import]
        private INavigation nav { get; set; }

        public SANavigationMenuPresenter()
        {
            //MEFManager.Compose(this);
            nav = new Navigation();
        }

        public void Initialize(ISANavigationMenu view)
        {
            this.view = view;
        }

        public void LoadMenuItems(Menu menu)
        {
            foreach (SiteMapNode node in nav.SuperAdminNodes())
            {
                MenuItem menuItem = new MenuItem(node.Title);

                menu.Items.Add(menuItem);
            }
        }

        public void MaintainFamilyCycle()
        {
            WebContext.Parent = null;
            HttpContext.Current.Session.Remove("Service");
            HttpContext.Current.Session.Remove("Customer");
            HttpContext.Current.Session.Remove("CustomerGroupDetails");
            HttpContext.Current.Session.Remove("Category");
            HttpContext.Current.Session.Remove("Promotion");
            HttpContext.Current.Session.Remove("EventDetail");
            HttpContext.Current.Session.Remove("Package");
            HttpContext.Current.Session.Remove("PackageOptionDetail_BackEnd");
            HttpContext.Current.Session.Remove("FaqGroupDetails");
            HttpContext.Current.Session.Remove("PromotionMedia");
            HttpContext.Current.Session.Remove("AdvertisementDetails");
        }

        public string GetURLofCorrespondingNodeKey(string key)
        {
            return nav.GetSiteMapNodeFromKey(key, "superAdminNav").Url;
        }

        public void RedirectToCorrespondingPage(string key)
        {
            Redirector.GoToRequestedPage(GetURLofCorrespondingNodeKey(key));
        }
    }
}