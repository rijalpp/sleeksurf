using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using System.Transactions;
using SleekSurf.Manager;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace SleekSurf.Web.Admin.Client
{
    public partial class PromotionManagement : System.Web.UI.Page
    {
        string promotionID = "";
        string clientID = "";
        static PagingDetails pgObj = null;
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("ClientAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("PromotionMgmt"))].Selected = true;

            if (WebContext.Sibling != null)
            {
                Menu tmpMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
                tmpMenu.Items[tmpMenu.Items.IndexOf(tmpMenu.FindItem("ClientMgmt"))].Selected = true;
            }

            if (User.IsInRole("AdminUser"))
                gvPromotionManagement.Columns[0].Visible = false;
            
            if (!IsPostBack)
            {
                pgObj = new PagingDetails();
                SearchPromotion();
            }
        }

        protected void imgDeleteBtn_Click(object sender, EventArgs e)
        {
            List<string> promotionIDList = new List<string>();
            foreach (GridViewRow row in gvPromotionManagement.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        promotionID = gvPromotionManagement.DataKeys[row.RowIndex]["PromotionID"].ToString();
                        promotionIDList.Add(promotionID);
                    }

                }

            }

            //Call the method to Delete records 
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string clientID = WebContext.Parent.ClientID;
                    foreach (string promoID in promotionIDList)
                    {
                        ClientManager.DeletePromotion(promoID, clientID);
                    }
                    scope.Complete();
                }
                catch(Exception ex)
                {
                    Helpers.LogError(ex);
                    throw;
                }
            }

            // rebind the GridView
            SearchPromotion();
        }

        protected void gvPromotionManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((CheckBox)e.Row.FindControl("cbDelete")).Attributes.Add("onclick", "javascript:UpdateSelectAllAndDeleteControl('" + gvPromotionManagement.ClientID + "', 'cbDelete', 'checkAll', 'imgDeleteBtn');");

                string promotionID = gvPromotionManagement.DataKeys[e.Row.RowIndex]["PromotionID"].ToString();
                string Section = "TITLE";
                HtmlImage iThumb = e.Row.FindControl("iThumb") as HtmlImage;
                iThumb.Src = "~/DisplayImage.aspx?ID=" + promotionID + "&SECTION=" + Section;
                iThumb.Alt = "No Images";
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((ImageButton)e.Row.FindControl("imgDeleteBtn")).OnClientClick = "return DeleteConfirmation('" + gvPromotionManagement.ClientID + "', 'cbDelete');";
            }
        }

        protected void gvPromotionManagement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            promotionID = gvPromotionManagement.DataKeys[e.NewSelectedIndex].Values[0].ToString();
            Session.Add("Promotion", promotionID);
            Redirector.GoToRequestedPage("~/Admin/Client/NewEditPromotion.aspx");
        }

        protected void gvPromotionManagement_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPromotionManagement.EditIndex = e.NewEditIndex;
            ViewState["Index"] = e.NewEditIndex;
            //rebind the gridview
            SearchPromotion();
        }

        protected void gvPromotionManagament_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                if (WebContext.Parent != null)
                    clientID = WebContext.Parent.ClientID;
                    promotionID = gvPromotionManagement.DataKeys[e.RowIndex]["PromotionID"].ToString();
                    SetPromotionActiveStatus(promotionID, clientID, Convert.ToInt16(ViewState["Index"]));
                    gvPromotionManagement.EditIndex = -1;
                // rebind the gridview
                SearchPromotion();
            }
        }

        protected void gvPromotionManagement_RowCancellingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPromotionManagement.EditIndex = -1;
            // rebind gridview
            SearchPromotion();
        }

        private void SetPromotionActiveStatus(string promotionID, string clientID, int index)
        {
            bool isActive = ((CheckBox)gvPromotionManagement.Rows[index].FindControl("chkEditActive")).Checked;
            ClientManager.SetPromotionActiveStatus(promotionID, clientID, isActive);
        }

        private void SearchAllCurrentPromotion()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Clients.PageSize;
            Result<PromotionDetails> result = ClientManager.SelectAllCurrentPromotion(clientID, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPromotionManagement.DataSource = result.EntityList;
                gvPromotionManagement.DataBind();
                SetupPaging();
            }
        }

        private void SearchAllPromotion()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Clients.PageSize;
            Result<PromotionDetails> result = ClientManager.SelectAllPromotion(clientID, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPromotionManagement.DataSource = result.EntityList;
                gvPromotionManagement.DataBind();
                SetupPaging();
            }
        }

        private void SearchCurrentPromotionByPublication(bool active)
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Clients.PageSize;
            Result<PromotionDetails> result = ClientManager.SelectCurrentPromotionByPublication(clientID, active, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPromotionManagement.DataSource = result.EntityList;
                gvPromotionManagement.DataBind();
                SetupPaging();
            }
        }

        private void SearchAllPromotionByPublication(bool active)
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Clients.PageSize;
            Result<PromotionDetails> result = ClientManager.SelectAllPromotionByPublication(clientID, active, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPromotionManagement.DataSource = result.EntityList;
                gvPromotionManagement.DataBind();
                SetupPaging();
            }
        }

        //NEWLY ADDED FUNCTIONS 12/07/2011

        private void SearchAllCurrentUpcomingPromotions()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Clients.PageSize;
            Result<PromotionDetails> result = ClientManager.SelectAllCurrentUpcomingPromotions(clientID, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPromotionManagement.DataSource = result.EntityList;
                gvPromotionManagement.DataBind();
                SetupPaging();
            }
        }

        private void SearchPublishedCurrentUpcomingPromotions()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Clients.PageSize;
            Result<PromotionDetails> result = ClientManager.SelectPublishedCurrentUpcomingPromotions(clientID, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPromotionManagement.DataSource = result.EntityList;
                gvPromotionManagement.DataBind();
                SetupPaging();
            }
        }

        private void SearchUnPublishedCurrentUpcomingPromotions()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Clients.PageSize;
            Result<PromotionDetails> result = ClientManager.SelectUnPublishedCurrentUpcomingPromotions(clientID, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPromotionManagement.DataSource = result.EntityList;
                gvPromotionManagement.DataBind();
                SetupPaging();
            }
        }

        //PAST PROMOTIONS
        private void SearchAllPastPromotions()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Clients.PageSize;
            Result<PromotionDetails> result = ClientManager.SelectAllPastPromotions(clientID, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPromotionManagement.DataSource = result.EntityList;
                gvPromotionManagement.DataBind();
                SetupPaging();
            }
        }

        private void SearchPublishedPastPromotions()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Clients.PageSize;
            Result<PromotionDetails> result = ClientManager.SelectPublishedPastPromotions(clientID, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPromotionManagement.DataSource = result.EntityList;
                gvPromotionManagement.DataBind();
                SetupPaging();
            }
        }

        private void SearchUnPublishedPastPromotions()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Clients.PageSize;
            Result<PromotionDetails> result = ClientManager.SelectUnPublishedPastPromotions(clientID, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPromotionManagement.DataSource = result.EntityList;
                gvPromotionManagement.DataBind();
                SetupPaging();
            }
        }
        //FUTURE PROMOTIONS
        private void SearchAllUpcomingPromotions()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Clients.PageSize;
            Result<PromotionDetails> result = ClientManager.SelectAllUpcomingPromotions(clientID, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPromotionManagement.DataSource = result.EntityList;
                gvPromotionManagement.DataBind();
                SetupPaging();
            }
        }

        private void SearchPublishedUpcomingPromotions()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Clients.PageSize;
            Result<PromotionDetails> result = ClientManager.SelectPublishedUpcomingPromotions(clientID, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPromotionManagement.DataSource = result.EntityList;
                gvPromotionManagement.DataBind();
                SetupPaging();
            }
        }

        private void SearchUnPublishedUpcomingPromotions()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Clients.PageSize;
            Result<PromotionDetails> result = ClientManager.SelectUnPublishedUpcomingPromotions(clientID, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPromotionManagement.DataSource = result.EntityList;
                gvPromotionManagement.DataBind();
                SetupPaging();
            }
        }

        protected void rbtnList_SelectedIndexChanged(object sender, EventArgs e)
        {
            pgObj.StartRowIndex = 1;
            pgObj.SearchMode = rbtnList.SelectedValue;
            pgObj.SearchModeOption = rbtnOptionList.SelectedValue;
            SearchPromotion();
        }

        protected void rbtnOptionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            pgObj.StartRowIndex = 1;
            pgObj.SearchMode = rbtnList.SelectedValue;
            pgObj.SearchModeOption = rbtnOptionList.SelectedValue;
            SearchPromotion();
        }

        private void SearchPromotion()
        {
            if (WebContext.Parent != null)
                clientID = WebContext.Parent.ClientID;

            switch (pgObj.SearchMode)
            {
                case "CurrentUpcoming":
                    switch (pgObj.SearchModeOption)
                    {
                        case "All":
                            SearchAllCurrentUpcomingPromotions();
                            break;
                        case "Enabled":
                            SearchPublishedCurrentUpcomingPromotions();
                            break;
                        case "Disabled":
                            SearchUnPublishedCurrentUpcomingPromotions();
                            break;
                    }
                    break;
                case "Current":
                    switch (pgObj.SearchModeOption)
                    {
                        case "All":
                            SearchAllCurrentPromotion();
                            break;
                        case "Enabled":
                            SearchCurrentPromotionByPublication(true);
                            break;
                        case "Disabled":
                            SearchCurrentPromotionByPublication(false);
                            break;
                    }
                    break;
                case "Upcoming":
                    switch (pgObj.SearchModeOption)
                    {
                        case "All":
                            SearchAllUpcomingPromotions();
                            break;
                        case "Enabled":
                            SearchPublishedUpcomingPromotions();
                            break;
                        case "Disabled":
                            SearchUnPublishedUpcomingPromotions();
                            break;
                    }
                    break;
                case "All":
                    switch (pgObj.SearchModeOption)
                    {
                        case "All":
                            SearchAllPromotion();
                            break;
                        case "Enabled":
                            SearchAllPromotionByPublication(true);
                            break;
                        case "Disabled":
                            SearchAllPromotionByPublication(false);
                            break;
                    }
                    break;
                case "Past":
                    switch (pgObj.SearchModeOption)
                    {
                        case "All":
                            SearchAllPastPromotions();
                            break;
                        case "Enabled":
                            SearchPublishedPastPromotions();
                            break;
                        case "Disabled":
                            SearchUnPublishedPastPromotions();
                            break;
                    }
                    break;
                default:
                    SearchAllCurrentUpcomingPromotions();
                    break;
            }
        }

        private string[] SplitWord(string keywords)
        {
            return Regex.Split(keywords, @"\W+");
        }


        #region  custom pager section of gridview

        public struct PageUrl
        {
            private string page;
            private string url;
            public string Page
            {
                get { return page; }
            }
            public string Url
            {
                get { return url; }
            }
            public PageUrl(string page, string url)
            {
                this.page = page;
                this.url = url;
            }
        }

        protected void rptPager_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            int prevPageIndex = 0;
            foreach (RepeaterItem item in rptPager.Items)
            {
                LinkButton btnPager = (LinkButton)item.FindControl("lbtnPagerButton");
                if (btnPager.Enabled == false)
                {
                    prevPageIndex = int.Parse(btnPager.CommandName);
                    break;
                }
            }
            pgObj.StartRowIndex = int.Parse(e.CommandName);
            SearchByPageButtons(prevPageIndex - 1, pgObj.StartRowIndex - 1);
            SearchPromotion();
        }

        // This method will handle the navigation/ paging index
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            // PagingObject pgObj = new PagingObject();
            int prevPageIndex = 0;
            switch (e.CommandName)
            {
                case "Previous":
                    prevPageIndex = Int32.Parse(lblStartPage.Text);
                    pgObj.StartRowIndex = prevPageIndex - 1;
                    break;

                case "Next":
                    prevPageIndex = Int32.Parse(lblStartPage.Text);
                    pgObj.StartRowIndex = prevPageIndex + 1;
                    break;
            }
            SearchByPageButtons(prevPageIndex - 1, pgObj.StartRowIndex - 1);
            SearchPromotion();
        }

        private void SetupPaging()
        {
            if (gvPromotionManagement.Rows.Count > 0)
            {
                pnlgvPersonNavigatorTop.Visible = true;
                if (pgObj.PageSize < pgObj.TotalNumber)
                    pnlNavigatorBottom.Visible = true;
                else
                    pnlNavigatorBottom.Visible = false;
                lblStartPage.Text = pgObj.StartRowIndex.ToString();
                int totalPages = Helpers.GetTotalPages(pgObj.TotalNumber, pgObj.PageSize);
                lblTotalPages.Text = Helpers.GetTotalPages(pgObj.TotalNumber, pgObj.PageSize).ToString();
                lblTotalNo.Text = pgObj.TotalNumber.ToString();
                if (rptPager.Items.Count != totalPages)
                {
                    //list the pages and their url as an array
                    PageUrl[] pages = new PageUrl[totalPages];
                    //generate pages url elements
                    pages[0] = new PageUrl("1", "");
                    for (int i = 2; i <= totalPages; i++)
                    {
                        pages[i - 1] = new PageUrl(i.ToString(), "");
                    }
                    //don't generate the link for current page
                    pages[pgObj.StartRowIndex - 1] = new PageUrl((pgObj.StartRowIndex.ToString()), "");
                    //feeds the pages to the repeater
                    rptPager.DataSource = pages;
                    rptPager.DataBind();


                    LinkButton btnPager = (LinkButton)rptPager.Items[pgObj.StartRowIndex - 1].FindControl("lbtnPagerButton");
                    btnPager.CssClass = "currentPage";
                    btnPager.Enabled = false;

                }

                if (int.Parse(lblStartPage.Text) == 1)
                {
                    lbtnPrevious.Enabled = false;
                    lbtnNext.Enabled = true;
                }
                else if (int.Parse(lblStartPage.Text) == totalPages)
                {
                    lbtnNext.Enabled = false;
                    lbtnPrevious.Enabled = true;
                    if (totalPages == 1)
                    {
                        lbtnPrevious.Enabled = false;
                    }
                }
                else
                {
                    lbtnPrevious.Enabled = true;
                    lbtnNext.Enabled = true;
                }
            }
            else
            {
                pnlgvPersonNavigatorTop.Visible = false;
                pnlNavigatorBottom.Visible = false;//hides the navigation section of gridview.
            }
        }

        private void SearchByPageButtons(int prevPageIndex, int currentPageIndex)
        {
            LinkButton currentButton = (LinkButton)rptPager.Items[currentPageIndex].FindControl("lbtnPagerButton");
            currentButton.CssClass = "currentPage";
            currentButton.Enabled = false;
            LinkButton previousButton = (LinkButton)rptPager.Items[prevPageIndex].FindControl("lbtnPagerButton");
            previousButton.CssClass = "paginationLinkButton";
            previousButton.Enabled = true;
        }

        #endregion
    }
}