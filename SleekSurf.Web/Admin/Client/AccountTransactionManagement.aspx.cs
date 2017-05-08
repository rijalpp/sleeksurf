using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using SleekSurf.Manager;
using SleekSurf.Entity;

namespace SleekSurf.Web.Admin.Client
{
    public partial class AccountTransactionManagement : System.Web.UI.Page
    {
        private static PagingDetails pgObj = null;
        private static ClientFeatureDetails clientFeature = null;
        string orderID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                clientFeature = ClientManager.SelectClientFeatureDetails(WebContext.Parent.ClientID).EntityList[0];
                pgObj = new PagingDetails();
                SearchOrders();
            }
            if (clientFeature != null)
            {
                hlMatchProfile.Visible = !clientFeature.ClientProfile;
                hlMatchDomain.Visible = !clientFeature.ClientDomain;
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("ClientAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("AccountMgmt"))].Selected = true;

            if (WebContext.Sibling != null)
            {
                Menu tmpMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
                tmpMenu.Items[tmpMenu.Items.IndexOf(tmpMenu.FindItem("ClientMgmt"))].Selected = true;
            }
            else
            {
                gvOrderManagement.Columns[0].Visible = false;
                gvOrderManagement.Columns[5].Visible = false;
            }
        }

        protected void imgDeleteBtn_Click(object sender, EventArgs e)
        {
            // List<Guid> userList = new List<Guid>();
            List<string> promotionIDList = new List<string>();
            foreach (GridViewRow row in gvOrderManagement.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        orderID = gvOrderManagement.DataKeys[row.RowIndex]["OrderID"].ToString();
                        ClientPackageManager.DeletePackageOrder(orderID);
                    }

                }

            }

            SearchOrders();
        }

        private void SearchOrders()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Package.PageSize;

            gvOrderManagement.DataSource = ClientPackageManager.SelectPackageOrderByClientID(WebContext.Parent.ClientID, pgObj).EntityList;
            gvOrderManagement.DataBind();
            SetupPaging();
        }

        protected void gvOrderManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal ltrPackageName = (Literal)e.Row.FindControl("ltrPackageName");
                string tempPackageName = gvOrderManagement.DataKeys[e.Row.RowIndex]["PackageName"].ToString().Trim();
                if (tempPackageName.EndsWith(" BySleekSurf"))
                    tempPackageName = tempPackageName.Replace(" BySleekSurf", "");

                ltrPackageName.Text = tempPackageName.Trim();

                if (e.Row.RowIndex == 0)
                {
                    string orderStatus = ((Label)e.Row.FindControl("lblOrderStatus")).Text;
                    if (orderStatus != "Verified" && orderStatus != "Refunded" && orderStatus != "Verified-Refunded")
                    {
                        ((ImageButton)e.Row.FindControl("btnEdit")).Visible = true;
                    }
                    else
                        ((ImageButton)e.Row.FindControl("btnEdit")).Visible = false;
                }
                else
                {
                    ((ImageButton)e.Row.FindControl("btnEdit")).Visible = false;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)//APPLIES TO ALL ROWS IF IT'S DataRow
            {
                string orderStatus = ((Label)e.Row.FindControl("lblOrderStatus")).Text;
                if (orderStatus != "Verified" && orderStatus != "Refunded" && orderStatus != "Verified-Refunded")
                {
                    ((CheckBox)e.Row.FindControl("cbDelete")).Attributes.Add("onclick", "javascript:UpdateSelectAllAndDeleteControl('" + gvOrderManagement.ClientID + "', 'cbDelete', 'checkAll', 'imgDeleteBtn');");
                }
                else
                {
                    ((CheckBox)e.Row.FindControl("cbDelete")).Visible = false;
                }

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
                ((ImageButton)e.Row.FindControl("imgDeleteBtn")).OnClientClick = "return DeleteConfirmation('" + gvOrderManagement.ClientID + "', 'cbDelete');";
        }

        protected void gvOrderManagement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            orderID = gvOrderManagement.DataKeys[e.NewSelectedIndex].Values[0].ToString();
            ClientDetails client = (ClientDetails)gvOrderManagement.DataKeys[e.NewSelectedIndex].Values[1];
            string clientID = client.ClientID;
            Session.Add("PackageOrderDetails", new PackageOrderDetails() { OrderID = orderID, Client = new ClientDetails() { ClientID = clientID } });
            Redirector.GoToRequestedPage("~/Admin/Client/AccountTransactionDetails.aspx");
        }

        protected void gvOrderManagement_RowEditing(object sender, GridViewEditEventArgs e)
        {
            orderID = gvOrderManagement.DataKeys[e.NewEditIndex].Values[0].ToString();
            Redirector.GoToRequestedPage("~/Admin/Client/AccountManagement.aspx");
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
    }
}