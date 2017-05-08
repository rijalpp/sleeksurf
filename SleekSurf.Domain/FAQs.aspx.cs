using System;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using SleekSurf.Manager;


namespace SleekSurf.Domain
{
    public partial class FAQs : WebBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)(Master.Master.FindControl("NavigationMenu"));
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("Home"))].Selected = true;

            if (!IsPostBack)
                LoadFaqs();
        }

        protected void rptFaqGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                string faqGroupID = ((Label)item.FindControl("lblFaqGroupID")).Text;
                Repeater rptFaqs = (Repeater)item.FindControl("rptFaqs");

                Result<FAQDetails> FAQResult = ClientManager.SelectFaqByFaqGroup(faqGroupID);
                if (FAQResult.EntityList.Count > 0)
                {
                    rptFaqs.DataSource = FAQResult.EntityList;
                    rptFaqs.DataBind();
                }
                else
                    e.Item.Visible = false;
            }
        }

        private void LoadFaqs()
        {
            Result<FAQGroupDetails> FAQGroupResult = ClientManager.SelectFaqGroupByClientID(WebContext.ClientProfile.ClientID);
            if (FAQGroupResult.Status == ResultStatus.Success)
            {
                if (FAQGroupResult.EntityList.Count > 0)
                {
                    rptFaqGroups.DataSource = FAQGroupResult.EntityList;
                    rptFaqGroups.DataBind();
                }
                else
                    Redirector.GoToRequestedPage(BasePage.FullBaseUrl + WebContext.ClientProfile.UniqueIdentity);
            }
            else
                Redirector.GoToRequestedPage(BasePage.FullBaseUrl + WebContext.ClientProfile.UniqueIdentity);
        }
    }
}