using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SleekSurf.Web.Admin.UserControls.Interfaces;
using System.ComponentModel.Composition;
using SleekSurf.FrameWork.Interfaces;
using SleekSurf.FrameWork;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.Manager;

namespace SleekSurf.Web.Admin.UserControls.Presenters
{
    public class CANavigationMenuPresenter
    {
        private ICANavigationMenu view;

        [Import]
        private INavigation nav { get; set; }

        public void Initialize(ICANavigationMenu view)
        {
            //MEFManager.Compose(this);
            nav = new Navigation();
            this.view = view;
        }

        public void LoadMenuItems(Menu menu)
        {
            if (WebContext.Parent != null)
            {
                ClientFeatureDetails clientFeature = ClientManager.SelectClientFeatureDetails(WebContext.Parent.ClientID).EntityList[0];

                if (clientFeature.Listing && !clientFeature.ClientDomain && !clientFeature.ClientProfile)
                {
                    foreach (SiteMapNode node in nav.ClientAdminNodes())
                    {
                        if (Enum.GetNames(typeof(ListingAccessOnly)).Contains(node.Title))
                        {
                            MenuItem menuItem = new MenuItem(node.Title);
                            menu.Items.Add(menuItem);
                        }
                    }

                    return;
                }
            }

            foreach (SiteMapNode node in nav.ClientAdminNodes())
            {
                MenuItem menuItem = new MenuItem(node.Title);
                menu.Items.Add(menuItem);
            }
           
        }

        public void MaintainFamilyCycle()
        {
            WebContext.SelectedUser = null;
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
            return nav.GetSiteMapNodeFromKey(key, "clientAdminNav").Url;
        }

        public void RedirectToCorrespondingPage(string key)
        {
            Redirector.GoToRequestedPage(GetURLofCorrespondingNodeKey(key));
        }
    }
}