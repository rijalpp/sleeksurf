using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using SleekSurf.Manager;

namespace SleekSurf.Web.Admin.SuperAdmin
{
    public partial class PackageManagement : System.Web.UI.Page
    {
        PagingDetails pgObj = new PagingDetails();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                SearchPackages();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("PackageMgmt"))].Selected = true;
        }

        private void SearchPackages()
        {
            switch (pgObj.SearchMode)
            {
                case "IS_PUBLISHED":
                    SearchPackagesByPublication();
                    break;
                default:
                    SearchAllPackages();
                    break;
            }
        }

        private void SearchAllPackages()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Package.PageSize;
            Result<PackageDetails> result = ClientPackageManager.SelectAllPackage(pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPackageManagement.DataSource = result.EntityList;
                gvPackageManagement.DataBind();
                SetupPaging();
            }
        }

        private void SearchPackagesByPublication()
        {
            pgObj.StartRowIndex = pgObj.StartRowIndex;
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Package.PageSize;
            // string name = pgObj.SearchKey;
            bool published = bool.Parse(pgObj.SearchKey);
            Result<PackageDetails> result = ClientPackageManager.SelectPackagesByPublication(published, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPackageManagement.DataSource = result.EntityList;
                gvPackageManagement.DataBind();
                SetupPaging();
            }
        }

        protected void gvPackageManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((ImageButton)e.Row.FindControl("imgDeleteBtn")).OnClientClick = "return DeleteConfirmation('" + gvPackageManagement.ClientID + "', 'cbDelete');";
            }
        }

        protected void gvPackageManagement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            string packageCode = gvPackageManagement.DataKeys[e.NewSelectedIndex]["PackageCode"].ToString();
            Session.Add("Package", new PackageDetails() { PackageCode = packageCode });
            Redirector.GoToRequestedPage("~/Admin/SuperAdmin/NewEditPackage.aspx");
        }

        protected void gvPackageManagement_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPackageManagement.EditIndex = e.NewEditIndex;
            SearchPackages();
        }

        protected void gvPackageManagement_RowCancellingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPackageManagement.EditIndex = -1;
            SearchPackages();
        }

        protected void gvPackageManagement_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string packageCode = gvPackageManagement.DataKeys[e.RowIndex]["PackageCode"].ToString();
            PackageDetails packageDetail = ClientPackageManager.SelectPackage(packageCode).EntityList[0];
            CheckBox chkApproved = (CheckBox)gvPackageManagement.Rows[e.RowIndex].FindControl("chkPublished");
            ClientPackageManager.UpdatePackageByPublication(packageCode, chkApproved.Checked);
            gvPackageManagement.EditIndex = -1;
            SearchPackages();
        }

        protected void rdlPublished_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdlPublished.SelectedValue == "All")
                pgObj.SearchMode = "ALL";
            else
                pgObj.SearchMode = "IS_PUBLISHED";

            pgObj.SearchKey = rdlPublished.SelectedValue;
            SearchPackages();
        }

        protected void imgDeleteBtn_Click(object sender, EventArgs e)
        {
            List<string> categoryIDList = new List<string>();
            string packageCode = "";
            foreach (GridViewRow row in gvPackageManagement.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        packageCode = gvPackageManagement.DataKeys[row.RowIndex]["PackageCode"].ToString();
                        categoryIDList.Add(packageCode);
                    }

                }

            }

            foreach (string id in categoryIDList)
            {
                Result<PackageDetails> result = ClientPackageManager.DeletePackage(id);
                if (result.Status != ResultStatus.Success)
                {
                    lblMessage.CssClass = "errorMsg";
                    lblMessage.Text = result.Message;
                }
            }

            // rebind the GridView
            SearchPackages();
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
            SearchPackages();
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
            SearchPackages();
        }

        private void SetupPaging()
        {
            if (gvPackageManagement.Rows.Count > 0)
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
                    //lblDDText.Visible = true;
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