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

namespace SleekSurf.Web.Admin.SuperAdmin
{
    public partial class PackageOrderManagement : System.Web.UI.Page
    {
        static PagingDetails pgObj = null;
        string orderID = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                pgObj = new PagingDetails();
                BindOrderStatus();
                SearchOrders();
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("PackageOrderMgmt"))].Selected = true;
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
            SearchOrders();
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
            SearchOrders();
        }

        private void SetupPaging()
        {
            if (gvOrderManagement.Rows.Count > 0)
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
            // List<Guid> userList = new List<Guid>();
            List<string> orderIDList = new List<string>();
            foreach (GridViewRow row in gvOrderManagement.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        orderID = gvOrderManagement.DataKeys[row.RowIndex]["OrderID"].ToString();
                        orderIDList.Add(orderID);
                    }

                }

            }

            //Call the method to Delete records 
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (string orderID in orderIDList)
                    {
                        ClientPackageManager.DeletePackageOrder(orderID);
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
            SearchOrders();
        }

        protected void lbtnClientName_Command(object sender, CommandEventArgs e)
        {
            WebContext.Parent = ClientManager.SelectClient(e.CommandName).EntityList[0];
            Redirector.GoToRequestedPage("~/Admin/Client/UserManagement.aspx");
        }

        protected void gvOrderManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtnClientName = (LinkButton)e.Row.FindControl("lbtnClientName");
                ImageButton imgEmail = (ImageButton)e.Row.FindControl("imgEmail");
                ClientDetails thisClient = (ClientDetails)gvOrderManagement.DataKeys[e.Row.RowIndex]["Client"];
                thisClient = ClientManager.SelectClient(thisClient.ClientID).EntityList[0];
                if (thisClient != null)
                {
                    lbtnClientName.Text = thisClient.ClientName;
                    lbtnClientName.CommandName = thisClient.ClientID;

                    imgEmail.PostBackUrl = "~/Admin/SendEmail.aspx?To=" + thisClient.BusinessEmail;
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((ImageButton)e.Row.FindControl("imgDeleteBtn")).OnClientClick = "return DeleteConfirmation('" + gvOrderManagement.ClientID + "', 'cbDelete');";
            }
        }

        protected void gvOrderManagement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            orderID = gvOrderManagement.DataKeys[e.NewSelectedIndex].Values[0].ToString();
            ClientDetails client = (ClientDetails)gvOrderManagement.DataKeys[e.NewSelectedIndex].Values[1];
            string clientID = client.ClientID;
            Session.Add("PackageOrderDetails", new PackageOrderDetails() { OrderID = orderID, Client = new ClientDetails() { ClientID = clientID } });
            Redirector.GoToRequestedPage("~/Admin/SuperAdmin/ViewPackageOrderDetails.aspx");
        }

        private void SearchOrders()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Package.PageSize;

            string orderStatus = ddlOrderStatus.SelectedValue;
            DateTime fromDate = DateTime.Now.AddDays(-7);
            if (txtDateFrom.Text.Length > 0)
                fromDate = Convert.ToDateTime(txtDateFrom.Text);
            else
                txtDateFrom.Text = fromDate.ToShortDateString();

            gvOrderManagement.DataSource = ClientPackageManager.SelectLatestPackageOrderPerClient(orderStatus, fromDate, pgObj).EntityList;
            gvOrderManagement.DataBind();

            if (string.Compare(ddlOrderStatus.SelectedItem.Text, "verified", true) == 0 || string.Compare(ddlOrderStatus.SelectedItem.Text, "confirmed", true) == 0)
                gvOrderManagement.Columns[0].Visible = false;

            //PAGING OPTION
            SetupPaging();
        }

        private void BindOrderStatus()
        {
            ddlOrderStatus.DataSource = Enum.GetValues(typeof(StatusOrder));
            ddlOrderStatus.DataBind();

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            pgObj.StartRowIndex = 1;
            SearchOrders();
        }
    }
}