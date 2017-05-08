using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using SleekSurf.Entity;
using SleekSurf.Manager;
using SleekSurf.FrameWork;

namespace SleekSurf.Web.Admin.SuperAdmin
{
    public partial class AdvertisementManagement : BasePage
    {
        static PagingDetails pgObj = null;
        string adID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("AdMgmt"))].Selected = true;

            if (!IsPostBack)
            {
                pgObj = new PagingDetails();
                SearchAdvertisements();
            }

            if (User.IsInRole("MarketingOfficer"))
                gvAdvertisementManagement.Columns[0].Visible = false;

        }

        protected void imgDeleteBtn_Click(object sender, EventArgs e)
        {
            string imageUrl = "";
            string imgPath = "";
            List<string> promotionIDList = new List<string>();

            foreach (GridViewRow row in gvAdvertisementManagement.Rows)
            {
                    CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                    if (chkDelete != null)
                    {
                        if (chkDelete.Checked)
                        {
                            using (TransactionScope scope = new TransactionScope())
                            {
                                try
                                {
                                    adID = gvAdvertisementManagement.DataKeys[row.RowIndex]["AdID"].ToString();
                                    imageUrl = gvAdvertisementManagement.DataKeys[row.RowIndex]["ImageUrl"].ToString();
                                    AdvertisementManager.DeleteAdvertisement(adID);
                                    imgPath = string.Format("~/Uploads/Advertisements/{0}", imageUrl);
                                    if (System.IO.File.Exists(Server.MapPath(imgPath)))
                                        System.IO.File.Delete(Server.MapPath(imgPath));
                                    scope.Complete();
                                }
                                catch (Exception ex)
                                {
                                    Helpers.LogError(ex);
                                    lblMessage.Text = "Error occured while deleting some advertisements.";
                                }
                            }
                    }
                }

            }

            SearchAdvertisements();
        }

        protected void gvAdvertisementManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((CheckBox)e.Row.FindControl("cbDelete")).Attributes.Add("onclick", "javascript:UpdateSelectAllAndDeleteControl('" + gvAdvertisementManagement.ClientID + "', 'cbDelete', 'checkAll', 'imgDeleteBtn');");

                string email = gvAdvertisementManagement.DataKeys[e.Row.RowIndex]["Email"].ToString();
                ((ImageButton)e.Row.FindControl("imgEmail")).PostBackUrl = "~/Admin/SendEmail.aspx?To=" + email;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
                ((ImageButton)e.Row.FindControl("imgDeleteBtn")).OnClientClick = "return DeleteConfirmation('" + gvAdvertisementManagement.ClientID + "', 'cbDelete');";
        }

        protected void gvAdvertisementManagement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            adID = gvAdvertisementManagement.DataKeys[e.NewSelectedIndex].Values[0].ToString();
            Session.Add("AdvertisementDetails", new AdvertisementDetails() { AdID = adID });
            Redirector.GoToRequestedPage("~/Admin/SuperAdmin/NewEditAdvertisement.aspx");
        }

        protected void gvAdvertisementManagement_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvAdvertisementManagement.EditIndex = e.NewEditIndex;
            SearchAdvertisements();
        }

        protected void gvAdvertisementManagement_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                adID = gvAdvertisementManagement.DataKeys[e.RowIndex]["AdID"].ToString();
                int index = Convert.ToInt16(ViewState["Index"]);
                bool published = ((CheckBox)gvAdvertisementManagement.Rows[e.RowIndex].FindControl("chkEditPublished")).Checked;
                AdvertisementManager.SetAdvertisementPublishStatus(adID, published);
                gvAdvertisementManagement.EditIndex = -1;
                SearchAdvertisements();
            }
        }

        protected void gvAdvertisementManagement_CancellingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAdvertisementManagement.EditIndex = -1;
            SearchAdvertisements();
        }

        private void SearchAdvertisements()
        {
                if (pgObj.StartRowIndex == 0)
                    pgObj.StartRowIndex = 1;
                pgObj.PageSize = Globals.Settings.Clients.PageSize;
                Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
                switch (pgObj.SearchMode)
                {
                    case "KEYWORD":
                        switch (pgObj.SearchModeOption)
                        {
                            case "Published":
                                result = AdvertisementManager.SelectAdvertisementByAdvertiserWithPublicationWithClientNull(pgObj.SearchKey, true, pgObj);
                                break;
                            case "Unpublished":
                                result = AdvertisementManager.SelectAdvertisementByAdvertiserWithPublicationWithClientNull(pgObj.SearchKey, false, pgObj);
                                break;
                            default:
                                result = AdvertisementManager.SelectAdvertisementByAdvertiserWithClientNull(pgObj.SearchKey, pgObj);
                                break;
                        }
                        break;
                    default:
                        switch (pgObj.SearchModeOption)
                        {
                            case "Published":
                                result = AdvertisementManager.SelectAdvertisementWithClientIDNullByPublication(true, pgObj);
                                break;
                            case "Unpublished":
                                result = AdvertisementManager.SelectAdvertisementWithClientIDNullByPublication(false, pgObj);
                                break;
                            default:
                                result = AdvertisementManager.SelectAdvertisementsWithClientIDNull(pgObj);
                                break;
                        }
                        break;
                }

                if (result.Status == ResultStatus.Success)
                {
                    gvAdvertisementManagement.DataSource = result.EntityList;
                    gvAdvertisementManagement.DataBind();
                    SetupPaging();
                }

                else
                {
                    lblMessage.CssClass = "errorMsg";
                    lblMessage.Text = result.Message;
                }
        }

        public string GetShortDescription(string description)
        {
            return Helpers.GetShortDescription(description, 16);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtAdName.Text.Trim() != txtAdName.ToolTip)
            {
                pgObj.StartRowIndex = 1;
                pgObj.SearchMode = "KEYWORD";
                pgObj.SearchKey = txtAdName.Text.Trim();
                foreach (ListItem item in rbtnOptionList.Items)
                {
                    if (item.Selected)
                        pgObj.SearchModeOption = rbtnOptionList.SelectedValue;
                    break;
                }
                SearchAdvertisements();
            }
        }

        protected void rbtnOptionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            pgObj.StartRowIndex = 1;
            pgObj.SearchModeOption = rbtnOptionList.SelectedValue;
            SearchAdvertisements();
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
            SearchAdvertisements();
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
            SearchAdvertisements();
        }

        private void SetupPaging()
        {
            if (gvAdvertisementManagement.Rows.Count > 0)
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