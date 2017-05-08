using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using SleekSurf.FrameWork.Interfaces;
using System.ComponentModel.Composition;
using System.Web.Routing;

namespace SleekSurf.FrameWork
{
    [Export(typeof(INavigation))]
    public class Navigation : INavigation
    {
        public List<SiteMapNode> AllNodes()
        {
            List<SiteMapNode> nodes = new List<SiteMapNode>();
            nodes.Add(SiteMap.RootNode);
            foreach (SiteMapNode node in SiteMap.RootNode.GetAllNodes())
                nodes.Add(node);

            return nodes;
        }

        public List<SiteMapNode> SuperAdminNodes()
        {
            List<SiteMapNode> superAdminNodes = new List<SiteMapNode>();
            foreach (SiteMapNode node in AllNodes())
            {
                if (node["superAdminNav"] != null && CheckAccessForNode(node))
                    if (node["superAdminNav"].ToString() == "1")
                        superAdminNodes.Add(node);
            }
            return superAdminNodes;
        }

        public List<SiteMapNode> ClientAdminNodes()
        {
            List<SiteMapNode> clientAdminNodes = new List<SiteMapNode>();
            foreach (SiteMapNode node in AllNodes())
            {
                if (node["clientAdminNav"] != null && CheckAccessForNode(node))
                    if (node["clientAdminNav"].ToString() == "1")
                        clientAdminNodes.Add(node);
            }
            return clientAdminNodes;
        }

        public SiteMapNode GetSiteMapNodeFromKey(string key, string accessRoleNav)
        {
            var node = AllNodes().Where(n => n.Title == key && n[accessRoleNav].ToString() == "1").First();

            return node as SiteMapNode;
        }

        public bool CheckAccessForNode(SiteMapNode node)
        {
            string[] roles = Roles.GetRolesForUser(WebContext.CurrentUser.Identity.Name);

            foreach (string role in roles)
            {
                if (node.Roles.Contains(role))
                    return true;
            }

            return false;
        }

        public void CheckAccessForCurrentNode()
        {
            bool result = CheckAccessForNode(CurrentNode);
            if (result)
                return;
            else
                Redirector.GoToAdminAccessDeniedPage();
        }

        public SiteMapNode CurrentNode
        {
            get
            {
                //Modified below logic for URL routing support
                //if on a valid node already, just return it
                if (SiteMap.CurrentNode != null)
                    return SiteMap.CurrentNode;

                //if routed, then will not be on a valid node. so need to find which node this maps to
                //can hard code as there are limited routes, but let's write a generic method
                PageRouteHandler routable = HttpContext.Current.Request.RequestContext.RouteData.RouteHandler as PageRouteHandler;

                if (routable != null)
                    return SiteMap.Provider.FindSiteMapNodeFromKey(routable.VirtualPath);

                //if no mapping found, return null. should not come here ideally
                return null;
            }
        }
    }
}
