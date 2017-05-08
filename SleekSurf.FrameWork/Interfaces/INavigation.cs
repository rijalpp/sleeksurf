using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SleekSurf.FrameWork.Interfaces
{
    public interface INavigation
    {
        SiteMapNode CurrentNode { get; }
        List<SiteMapNode> AllNodes();
        List<SiteMapNode> SuperAdminNodes();
        List<SiteMapNode> ClientAdminNodes();
        SiteMapNode GetSiteMapNodeFromKey(string key, string accessRoleNav);
        bool CheckAccessForNode(SiteMapNode node);
        void CheckAccessForCurrentNode();
    }
}
