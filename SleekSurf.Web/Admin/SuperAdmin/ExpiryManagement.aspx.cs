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
using System.Web.Security;
using System.IO;

namespace SleekSurf.Web.Admin.SuperAdmin
{
    public partial class ExpiryManagement : System.Web.UI.Page
    {
        static PagingDetails pgObj = null;
        string orderID = "";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("ExpiryMgmt"))].Selected = true;

            if (!IsPostBack)
            {
                pgObj = new PagingDetails();
                if (tabExpiryManagement.ActiveTabIndex == 0)
                    SearchPackageOrderExpiry();
                else
                    SearchAdvertisementExpiry();
            }
        }

        #region EXPIRING PACKAGE REGION

        #region  custom pager section of expiring package order gridview

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
            if (pgObj == null)
                pgObj = new PagingDetails();
            pgObj.StartRowIndex = int.Parse(e.CommandName);
            SearchByPageButtons(prevPageIndex - 1, pgObj.StartRowIndex - 1);
            SearchPackageOrderExpiry();
        }

        // This method will handle the navigation/ paging index
        protected void ChangePage(object sender, CommandEventArgs e)
        {
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
            //bind orders
            SearchPackageOrderExpiry();
        }

        private void SetupPaging()
        {
            if (gvOrderExpiryManagement.Rows.Count > 0)
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

        protected void imgDeleteBtn_Click(object sender, EventArgs e)
        {
            List<string> clientIDList = new List<string>();
            foreach (GridViewRow row in gvOrderExpiryManagement.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        ClientDetails thisClient = (ClientDetails)gvOrderExpiryManagement.DataKeys[row.RowIndex]["Client"];
                        clientIDList.Add(thisClient.ClientID);
                    }

                }

            }

            int countDelete = 0;

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    //Call the method to Delete records 
                    foreach (string clientID in clientIDList)
                    {
                        List<Guid> userIDList = ClientManager.SelectUsers(clientID);
                        foreach (Guid userID in userIDList)
                        {
                            MembershipUser userTemp = Membership.GetUser(userID);
                            Membership.DeleteUser(userTemp.UserName, true);
                        }

                        if (ClientManager.DeleteClient(clientID))
                        {

                            string dirUrl = BasePage.BaseUrl + "Uploads/" + clientID;
                            string dirPath = Server.MapPath(dirUrl);
                            if (Directory.Exists(dirPath))
                                Directory.Delete(dirPath, true);
                            countDelete++;
                        }
                    }

                    if (countDelete == clientIDList.Count)
                    {
                        lblMessage.CssClass = "successMsg";
                        lblMessage.Text = "All " + countDelete + " client(s) have been permanently deleted.";
                    }
                    else if (countDelete > 0)
                    {
                        lblMessage.CssClass = "successMsg";
                        lblMessage.Text = "Only " + countDelete + " client(s) have been permanently deleted. Error occoured while deleting other client(s).";
                    }
                    else
                    {
                        lblMessage.CssClass = "errorMsg";
                        lblMessage.Text = "No client(s) have not been deleted because of some technical issues. Please review.";
                    }

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                lblMessage.CssClass = "errorMsg";
                lblMessage.Text = "Problem deleting clients. Please review.";
                Helpers.LogError(ex);
            }
            // rebind the GridView
            SearchPackageOrderExpiry();
        }

        protected void lbtnClientName_Command(object sender, CommandEventArgs e)
        {
            WebContext.Parent = ClientManager.SelectClient(e.CommandName).EntityList[0];
            Redirector.GoToRequestedPage("~/Admin/Client/UserManagement.aspx");
        }

        protected void gvOrderExpiryManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtnClientName = (LinkButton)e.Row.FindControl("lbtnClientName");
                ImageButton imgEmail = (ImageButton)e.Row.FindControl("imgEmail");
                Literal ltrDaysLeft = (Literal)e.Row.FindControl("ltrDaysLeft");
                DateTime expiryDate = (DateTime)gvOrderExpiryManagement.DataKeys[e.Row.RowIndex]["ExpiryDate"];
                ClientDetails thisClient = (ClientDetails)gvOrderExpiryManagement.DataKeys[e.Row.RowIndex]["Client"];
                thisClient = ClientManager.SelectClient(thisClient.ClientID).EntityList[0];
                if (thisClient != null)
                {
                    lbtnClientName.Text = thisClient.ClientName;
                    lbtnClientName.CommandName = thisClient.ClientID;
                    imgEmail.PostBackUrl = "~/Admin/SendEmail.aspx?To=" + thisClient.BusinessEmail;
                }

                double timeFrame = expiryDate.Subtract(System.DateTime.Now).TotalMinutes;
                if (timeFrame > (60 * 24))
                    ltrDaysLeft.Text = Math.Ceiling(timeFrame / (60 * 24)) + " Days";
                else if (timeFrame > 60)
                    ltrDaysLeft.Text = Math.Ceiling(timeFrame / 60) + " Hours";
                else if (timeFrame > 0)
                    ltrDaysLeft.Text = Math.Ceiling(timeFrame) + " Minutes";
                else
                    ltrDaysLeft.Text = "Expired";
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((ImageButton)e.Row.FindControl("imgDeleteBtn")).OnClientClick = "return DeleteConfirmation('" + gvOrderExpiryManagement.ClientID + "', 'cbDelete');";
            }
        }

        protected void gvOrderExpiryManagement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            orderID = gvOrderExpiryManagement.DataKeys[e.NewSelectedIndex].Values[0].ToString();
            ClientDetails client = (ClientDetails)gvOrderExpiryManagement.DataKeys[e.NewSelectedIndex].Values[1];
            string clientID = client.ClientID;
            Session.Add("PackageOrderDetails", new PackageOrderDetails() { OrderID = orderID, Client = new ClientDetails() { ClientID = clientID } });
            Redirector.GoToRequestedPage("~/Admin/SuperAdmin/ViewPackageOrderDetails.aspx");
        }


        private void SearchPackageOrderExpiry()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Package.PageSize;

            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();

            switch (pgObj.SearchMode)
            {
                case "KEYWORD":
                    switch (pgObj.SearchModeOption)
                    {
                        case "Current":
                            result = ClientPackageManager.SelectPackageOrdersWithExpirationNoticeRangeByClientName(pgObj.SearchKey, pgObj);
                            break;
                        case "Expired":
                            result = ClientPackageManager.SelectExpiredPackageOrdersWithClientName(pgObj.SearchKey, pgObj);
                            break;
                        default:
                            result = ClientPackageManager.SelectExpiringPackageOrdersWithClientName(pgObj.SearchKey, pgObj);
                            break;
                    }
                    break;
                default:
                    switch (pgObj.SearchModeOption)
                    {
                        case "Current":
                            result = ClientPackageManager.SelectPackageOrdersWithExpiryNoticeRange(pgObj);
                            break;
                        case "Expired":
                            result = ClientPackageManager.SelectExpiredPackageOrders(pgObj);
                            break;
                        default:
                            result = ClientPackageManager.SelectExpiringPackageOrders(pgObj);
                            break;
                    }
                    break;
            }

            if (result.Status == ResultStatus.Success)
            {
                gvOrderExpiryManagement.DataSource = result.EntityList;
                gvOrderExpiryManagement.DataBind();
                SetupPaging();
            }
            else
            {
                lblMessage.CssClass = "errorMsg";
                lblMessage.Text = result.Message;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtClientName.Text.Trim() != txtClientName.ToolTip)
            {
                if (pgObj == null)
                    pgObj = new PagingDetails();
                pgObj.StartRowIndex = 1;
                pgObj.SearchMode = "KEYWORD";
                pgObj.SearchKey = txtClientName.Text.Trim();

                foreach (ListItem item in rbtnOptionList.Items)
                {
                    if (item.Selected)
                        pgObj.SearchModeOption = rbtnOptionList.SelectedValue;
                    break;
                }
                SearchPackageOrderExpiry();
            }
            else
            {
                lblMessage.Text = "Client Name is required.";
                lblMessage.CssClass = "errorMsg";
            }
        }

        protected void rbtnOptionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            pgObj.StartRowIndex = 1;
            pgObj.SearchModeOption = rbtnOptionList.SelectedValue;
            SearchPackageOrderExpiry();
        }

        #endregion

        #region EXPIRING ADVERTISEMENT REGION

        #region  custom pager section of expiring advertisement gridview

        protected void rptPagerAd_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            int prevPageIndex = 0;
            foreach (RepeaterItem item in rptPagerAd.Items)
            {
                LinkButton btnPager = (LinkButton)item.FindControl("lbtnPagerButtonAd");
                if (btnPager.Enabled == false)
                {
                    prevPageIndex = int.Parse(btnPager.CommandName);
                    break;
                }
            }
            if (pgObj == null)
                pgObj = new PagingDetails();
            pgObj.StartRowIndex = int.Parse(e.CommandName);
            SearchByPageButtonsAd(prevPageIndex - 1, pgObj.StartRowIndex - 1);
            SearchAdvertisementExpiry();
        }

        // This method will handle the navigation/ paging index
        protected void ChangePageAd(object sender, CommandEventArgs e)
        {
            int prevPageIndex = 0;
            switch (e.CommandName)
            {
                case "Previous":
                    prevPageIndex = Int32.Parse(lblStartPageAd.Text);
                    pgObj.StartRowIndex = prevPageIndex - 1;
                    break;

                case "Next":
                    prevPageIndex = Int32.Parse(lblStartPageAd.Text);
                    pgObj.StartRowIndex = prevPageIndex + 1;
                    break;
            }
            SearchByPageButtonsAd(prevPageIndex - 1, pgObj.StartRowIndex - 1);
            //bind Ads
            SearchAdvertisementExpiry();
        }

        private void SetupPagingAd()
        {
            if (gvExpiringAdManagement.Rows.Count > 0)
            {
                pnlgvPersonNavigatorTopAd.Visible = true;
                if (pgObj.PageSize < pgObj.TotalNumber)
                    pnlNavigatorBottomAd.Visible = true;
                else
                    pnlNavigatorBottomAd.Visible = false;
                lblStartPageAd.Text = pgObj.StartRowIndex.ToString();
                int totalPages = Helpers.GetTotalPages(pgObj.TotalNumber, pgObj.PageSize);
                lblTotalPagesAd.Text = Helpers.GetTotalPages(pgObj.TotalNumber, pgObj.PageSize).ToString();
                lblTotalNoAd.Text = pgObj.TotalNumber.ToString();
                if (rptPagerAd.Items.Count != totalPages)
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
                    rptPagerAd.DataSource = pages;
                    rptPagerAd.DataBind();


                    LinkButton btnPager = (LinkButton)rptPagerAd.Items[pgObj.StartRowIndex - 1].FindControl("lbtnPagerButtonAd");
                    btnPager.CssClass = "currentPage";
                    btnPager.Enabled = false;

                }

                if (int.Parse(lblStartPageAd.Text) == 1)
                {
                    lbtnPreviousAd.Enabled = false;
                    lbtnNextAd.Enabled = true;
                }
                else if (int.Parse(lblStartPageAd.Text) == totalPages)
                {
                    lbtnNextAd.Enabled = false;
                    lbtnPreviousAd.Enabled = true;
                    if (totalPages == 1)
                    {
                        lbtnPreviousAd.Enabled = false;
                    }
                }
                else
                {
                    lbtnPreviousAd.Enabled = true;
                    lbtnNextAd.Enabled = true;
                }
            }
            else
            {
                pnlgvPersonNavigatorTopAd.Visible = false;
                pnlNavigatorBottomAd.Visible = false;//hides the navigation section of gridview.
            }
        }

        private void SearchByPageButtonsAd(int prevPageIndex, int currentPageIndex)
        {
            LinkButton currentButton = (LinkButton)rptPager.Items[currentPageIndex].FindControl("lbtnPagerButtonAd");
            currentButton.CssClass = "currentPage";
            currentButton.Enabled = false;
            LinkButton previousButton = (LinkButton)rptPager.Items[prevPageIndex].FindControl("lbtnPagerButtonAd");
            previousButton.CssClass = "paginationLinkButton";
            previousButton.Enabled = true;
        }

        #endregion

        protected void imgDeleteBtnAd_Click(object sender, EventArgs e)
        {
            string adID = "";
            List<string> adIDList = new List<string>();
            foreach (GridViewRow row in gvExpiringAdManagement.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        adID = gvExpiringAdManagement.DataKeys[row.RowIndex]["AdID"].ToString();
                        adIDList.Add(adID);
                    }

                }

            }

            //Call the method to Delete records 
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (string ID in adIDList)
                    {
                        AdvertisementManager.DeleteAdvertisement(ID);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Helpers.LogError(ex);
                    throw;
                }
            }

            // REBIND THE GRIDVIEW
            SearchAdvertisementExpiry();
        }


        protected void gvExpiringAdManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgEmail = (ImageButton)e.Row.FindControl("imgEmail");
                Literal ltrDaysLeft = (Literal)e.Row.FindControl("ltrDaysLeft");
                DateTime expiryDate = (DateTime)gvExpiringAdManagement.DataKeys[e.Row.RowIndex]["EndDate"];
                string email = gvExpiringAdManagement.DataKeys[e.Row.RowIndex]["Email"].ToString();
                      imgEmail.PostBackUrl = "~/Admin/SendEmail.aspx?To=" + email;

                double timeFrame = expiryDate.Subtract(System.DateTime.Now).TotalMinutes;
                if (timeFrame > (60 * 24))
                    ltrDaysLeft.Text = Math.Ceiling(timeFrame / (60 * 24)) + " Days";
                else if (timeFrame > 60)
                    ltrDaysLeft.Text = Math.Ceiling(timeFrame / 60) + " Hours";
                else if (timeFrame > 0)
                    ltrDaysLeft.Text =  Math.Ceiling(timeFrame) + " Minutes";
                else
                    ltrDaysLeft.Text = "Expired";
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((ImageButton)e.Row.FindControl("imgDeleteBtnAd")).OnClientClick = "return DeleteConfirmation('" + gvExpiringAdManagement.ClientID + "', 'cbDelete');";
            }
        }

        protected void gvExpiringAdManagement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            string adID = gvExpiringAdManagement.DataKeys[e.NewSelectedIndex].Values[0].ToString();
            Session.Add("AdvertisementDetails", new AdvertisementDetails() { AdID = adID });
            Redirector.GoToRequestedPage("~/Admin/SuperAdmin/NewEditAdvertisement.aspx");
        }


        private void SearchAdvertisementExpiry()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Package.PageSize;

            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();

            switch (pgObj.SearchMode)
            {
                case "KEYWORD":
                    switch (pgObj.SearchModeOption)
                    {
                        case "Current":
                            result = AdvertisementManager.SelectExpiringCurrentAdvertisementsByAdvertiser(pgObj.SearchKey, pgObj);
                            break;
                        case "Expired":
                            result = AdvertisementManager.SelectExpiredAdvertisementsByAdvertiser(pgObj.SearchKey, pgObj);
                            break;
                        default:
                            result = AdvertisementManager.SelectExpiringAdvertisementsByAdvertiser(pgObj.SearchKey, pgObj);
                            break;
                    }
                    break;
                default:
                    switch (pgObj.SearchModeOption)
                    {
                        case "Current":
                            result = AdvertisementManager.SelectExpiringCurrentAdvertisements(pgObj);
                            break;
                        case "Expired":
                            result = AdvertisementManager.SelectExpiredAdvertisements(pgObj);
                            break;
                        default:
                            result = AdvertisementManager.SelectExpiringAdvertisements(pgObj);
                            break;
                    }
                    break;
            }

            if (result.Status == ResultStatus.Success)
            {
                gvExpiringAdManagement.DataSource = result.EntityList;
                gvExpiringAdManagement.DataBind();
                SetupPagingAd();
            }
            else
            {
                lblMessageAd.CssClass = "errorMsg";
                lblMessageAd.Text = result.Message;
            }
        }

        protected void btnSearchAd_Click(object sender, EventArgs e)
        {
            if (txtAdvertiser.Text.Trim() != txtAdvertiser.ToolTip)
            {
                if (pgObj == null)
                    pgObj = new PagingDetails();
                pgObj.StartRowIndex = 1;
                pgObj.SearchMode = "KEYWORD";
                pgObj.SearchKey = txtAdvertiser.Text.Trim();

                foreach (ListItem item in rbtnOptionListAd.Items)
                {
                    if (item.Selected)
                        pgObj.SearchModeOption = rbtnOptionListAd.SelectedValue;
                    break;
                }
                SearchAdvertisementExpiry();
            }
            else
            {
                lblMessageAd.Text = "Advertiser is required.";
                lblMessageAd.CssClass = "errorMsg";
            }
        }

        protected void rbtnOptionListAd_SelectedIndexChanged(object sender, EventArgs e)
        {
            pgObj.StartRowIndex = 1;
            pgObj.SearchModeOption = rbtnOptionListAd.SelectedValue;
            SearchAdvertisementExpiry();
        }

        #endregion

        #region INACTIVE BY DEFAULT CLIENT REGION

        #region  custom pager section of inactive by default  gridview

        protected void rptPagerClientStatus_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
         
            int prevPageIndex = 0;
            foreach (RepeaterItem item in rptPagerClientStatus.Items)
            {
                LinkButton btnPager = (LinkButton)item.FindControl("lbtnPagerButtonClientStatus");
                if (btnPager.Enabled == false)
                {
                    prevPageIndex = int.Parse(btnPager.CommandName);
                    break;
                }
            }
            if (pgObj == null)
                pgObj = new PagingDetails();
            pgObj.StartRowIndex = int.Parse(e.CommandName);
            SearchByPageButtonsClientStatus(prevPageIndex - 1, pgObj.StartRowIndex - 1);
            //SearchAdvertisementExpiry();
            SearchClientsStatus();
        }

        // This method will handle the navigation/ paging index
        protected void ChangePageClientStatus(object sender, CommandEventArgs e)
        {
            int prevPageIndex = 0;
            switch (e.CommandName)
            {
                case "Previous":
                    prevPageIndex = Int32.Parse(lblStartPageClientStatus.Text);
                    pgObj.StartRowIndex = prevPageIndex - 1;
                    break;

                case "Next":
                    prevPageIndex = Int32.Parse(lblStartPageClientStatus.Text);
                    pgObj.StartRowIndex = prevPageIndex + 1;
                    break;
            }
            SearchByPageButtonsClientStatus(prevPageIndex - 1, pgObj.StartRowIndex - 1);
            //bind clients
           // SearchAdvertisementExpiry();
            SearchClientsStatus();
        }

        private void SetupPagingClientStatus()
        {
            if (gvClientsByStatus.Rows.Count > 0)
            {
                pnlgvPersonNavigatorTopClientStatus.Visible = true;
                if (pgObj.PageSize < pgObj.TotalNumber)
                    pnlNavigatorBottomClientStatus.Visible = true;
                else
                    pnlNavigatorBottomClientStatus.Visible = false;
                lblStartPageClientStatus.Text = pgObj.StartRowIndex.ToString();
                int totalPages = Helpers.GetTotalPages(pgObj.TotalNumber, pgObj.PageSize);
                lblTotalPagesClientStatus.Text = Helpers.GetTotalPages(pgObj.TotalNumber, pgObj.PageSize).ToString();
                lblTotalNoClientStatus.Text = pgObj.TotalNumber.ToString();
                if (rptPagerClientStatus.Items.Count != totalPages)
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
                    rptPagerClientStatus.DataSource = pages;
                    rptPagerClientStatus.DataBind();


                    LinkButton btnPager = (LinkButton)rptPagerClientStatus.Items[pgObj.StartRowIndex - 1].FindControl("lbtnPagerButtonClientStatus");
                    btnPager.CssClass = "currentPage";
                    btnPager.Enabled = false;

                }

                if (int.Parse(lblStartPageClientStatus.Text) == 1)
                {
                    lbtnPreviousClientStatus.Enabled = false;
                    lbtnNextClientStatus.Enabled = true;
                }
                else if (int.Parse(lblStartPageClientStatus.Text) == totalPages)
                {
                    lbtnNextClientStatus.Enabled = false;
                    lbtnPreviousClientStatus.Enabled = true;
                    if (totalPages == 1)
                    {
                        lbtnPreviousClientStatus.Enabled = false;
                    }
                }
                else
                {
                    lbtnPreviousClientStatus.Enabled = true;
                    lbtnNextClientStatus.Enabled = true;
                }
            }
            else
            {
                pnlgvPersonNavigatorTopClientStatus.Visible = false;
                pnlNavigatorBottomClientStatus.Visible = false;//hides the navigation section of gridview.
            }
        }

        private void SearchByPageButtonsClientStatus(int prevPageIndex, int currentPageIndex)
        {
            LinkButton currentButton = (LinkButton)rptPager.Items[currentPageIndex].FindControl("lbtnPagerButtonClientStatus");
            currentButton.CssClass = "currentPage";
            currentButton.Enabled = false;
            LinkButton previousButton = (LinkButton)rptPager.Items[prevPageIndex].FindControl("lbtnPagerButtonClientStatus");
            previousButton.CssClass = "paginationLinkButton";
            previousButton.Enabled = true;
        }

        #endregion

        protected void imgDeleteBtnClientByStatus_Click(object sender, EventArgs e)
        {
            string clientID = "";
            List<string> clientIDList = new List<string>();
           
            foreach (GridViewRow row in gvClientsByStatus.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        clientID = gvClientsByStatus.DataKeys[row.RowIndex]["ClientID"].ToString();
                        clientIDList.Add(clientID);
                    }

                }

            }

            //Call the method to Delete records 
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (string ID in clientIDList)
                    {
                        ClientManager.DeleteClient(ID);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Helpers.LogError(ex);
                    throw;
                }
            }

            // REBIND THE GRIDVIEW
            //SearchAdvertisementExpiry();
            SearchClientsStatus();
        }


        protected void gvClientsByStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgEmail = (ImageButton)e.Row.FindControl("imgEmail");
                Literal ltrDays = (Literal)e.Row.FindControl("ltrDays");
                DateTime createdDate = (DateTime)gvClientsByStatus.DataKeys[e.Row.RowIndex]["CreatedDate"];
                string email = gvClientsByStatus.DataKeys[e.Row.RowIndex]["BusinessEmail"].ToString();
                imgEmail.PostBackUrl = "~/Admin/SendEmail.aspx?To=" + email;

                //double timeFrame = createdDate.Add(System.DateTime.Now).TotalMinutes;
                double timeFrame = -(createdDate.Subtract(System.DateTime.Now).TotalMinutes);
                if (timeFrame > (60 * 24))
                    ltrDays.Text = Math.Ceiling(timeFrame / (60 * 24)) + " Days";
                else if (timeFrame > 60)
                    ltrDays.Text = Math.Ceiling(timeFrame / 60) + " Hours";
                else if (timeFrame > 0)
                    ltrDays.Text = Math.Ceiling(timeFrame) + " Minutes";
                else
                    ltrDays.Text = "Less than a Minute";
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((ImageButton)e.Row.FindControl("imgDeleteBtnClientByStatus")).OnClientClick = "return DeleteConfirmation('" + gvClientsByStatus.ClientID + "', 'cbDelete');";
            }
        }

        protected void gvClientsByStatus_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            string clientID = gvClientsByStatus.DataKeys[e.NewSelectedIndex].Values[0].ToString();
            Session.Add("ClientStatusDetails", new ClientDetails() { ClientID = clientID });
            Redirector.GoToRequestedPage("~/Admin/SuperAdmin/NewEditClient.aspx");
        }


        private void SearchClientsStatus()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            if (string.IsNullOrEmpty(pgObj.SearchMode))
                pgObj.SearchMode = /*"InActiveByDefault";*/ Status.InActiveByDefault.ToString();
            pgObj.PageSize = Globals.Settings.Package.PageSize;

           // Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            Result<ClientDetails> result = new Result<ClientDetails>();

           result = ClientManager.SelectClientsByStatusAfterGivenDaysOfCreation(2, pgObj.SearchMode, pgObj);
       
            if (result.Status == ResultStatus.Success)
            {
                gvClientsByStatus.DataSource = result.EntityList;
                gvClientsByStatus.DataBind();
                SetupPagingClientStatus();
            }
            else
            {
                lblMessageAd.CssClass = "errorMsg";
                lblMessageAd.Text = result.Message;
            }
        }

        //protected void btnSearchClientStatus_Click(object sender, EventArgs e)
        //{
        //    if (txtClientNameByStatus.Text.Trim() != txtAdvertiser.ToolTip)
        //    {
        //        if (pgObj == null)
        //            pgObj = new PagingDetails();
        //        pgObj.StartRowIndex = 1;
        //        pgObj.SearchMode = "KEYWORD";
        //        pgObj.SearchKey = txtClientNameByStatus.Text.Trim();

        //        foreach (ListItem item in rbtnOptionListClientStatus.Items)
        //        {
        //            if (item.Selected)
        //                pgObj.SearchModeOption = rbtnOptionListClientStatus.SelectedValue;
        //            break;
        //        }
        //        SearchClientsStatus();
        //    }
        //    else
        //    {
        //        lblMessageAd.Text = "Client Name is required.";
        //        lblMessageAd.CssClass = "errorMsg";
        //    }
        //}

        //protected void rbtnOptionListClientStatus_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    pgObj.StartRowIndex = 1;
        //    pgObj.SearchModeOption = rbtnOptionListClientStatus.SelectedValue;
        //    SearchClientsStatus();
        //}

        #endregion

        protected void tabExpiryManagement_ActiveTabChanged(object sender, EventArgs e)
        {
            pgObj = new PagingDetails();
            switch (tabExpiryManagement.ActiveTabIndex)
            {
                case 0:
                    SearchPackageOrderExpiry();
                    break;
                case 1:
                    SearchAdvertisementExpiry();
                    break;
                case 2:
                    SearchClientsStatus();
                    break;
            }
        }
    }
}