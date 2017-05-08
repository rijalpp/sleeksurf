using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using SleekSurf.Entity;
using System.Transactions;
using System.IO;
using SleekSurf.Manager;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace SleekSurf.Web.Admin.Client
{
    public partial class PromotionGalleryManagement : BasePage
    {
        string mediaID = "";
        string promotionID = "";
        static PagingDetails pgObj = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Promotion"] != null)
                promotionID = Session["Promotion"].ToString();
            else
                Redirector.GoToRequestedPage("~/Admin/Client/PromotionManagement.aspx");

            if (!IsPostBack)
            {
                Session.Remove("PromotionMedia");
                pgObj = new PagingDetails();
                SearchPromotionGallery();
            }

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
                gvPromotionGalleryManagement.Columns[0].Visible = false;

        }

        protected void imgDeleteBtn_Click(object sender, EventArgs e)
        {
            List<MediaGalleryDetails> MediaList = new List<MediaGalleryDetails>();
            foreach (GridViewRow row in gvPromotionGalleryManagement.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        mediaID = gvPromotionGalleryManagement.DataKeys[row.RowIndex]["MediaID"].ToString();
                        string mediaUrl = gvPromotionGalleryManagement.DataKeys[row.RowIndex]["MediaUrl"].ToString();
                        string mediaType = gvPromotionGalleryManagement.DataKeys[row.RowIndex]["MediaType"].ToString();
                        MediaList.Add(new MediaGalleryDetails() { MediaID = mediaID, MediaUrl = mediaUrl, MediaType = mediaType });
                    }
                }

            }

            //DELETE THE SELECTED MEDIAS
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (MediaGalleryDetails media in MediaList)
                    {
                        if (media.MediaType == "Image")
                        {
                            string dirMediaPath = Server.MapPath(media.MediaUrl);
                            if (File.Exists(dirMediaPath))
                                File.Delete(dirMediaPath);
                        }

                        EventManager.DeleteMediaGallery(media.MediaID);
                    }
                    scope.Complete();
                }
                catch(Exception ex)
                {
                    Helpers.LogError(ex);
                    throw;
                }
            }
            // REBIND THE GRIDVIEW
            SearchPromotionGallery();
        }

        protected void gvPromotionGalleryManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((CheckBox)e.Row.FindControl("cbDelete")).Attributes.Add("onclick", "javascript:UpdateSelectAllAndDeleteControl('" + gvPromotionGalleryManagement.ClientID + "', 'cbDelete', 'checkAll', 'imgDeleteBtn');");

                string mediaType = gvPromotionGalleryManagement.DataKeys[e.Row.RowIndex]["MediaType"].ToString();
                 Image imgPromotion = (Image)e.Row.FindControl("imgPromotion");
                 if (mediaType == "Video")
                     imgPromotion.ImageUrl = "~/App_Themes/SleekTheme/Images/DefaultVideoThumbnail.gif";
                 else if (mediaType == "Image")
                     imgPromotion.ImageUrl = "~" + gvPromotionGalleryManagement.DataKeys[e.Row.RowIndex]["MediaUrl"].ToString() + "?" + (new DateTime()).Millisecond;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((ImageButton)e.Row.FindControl("imgDeleteBtn")).OnClientClick = "return DeleteConfirmation('" + gvPromotionGalleryManagement.ClientID + "', 'cbDelete');";
            }
        }

        protected void gvPromotionGalleryManagement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            mediaID = gvPromotionGalleryManagement.DataKeys[e.NewSelectedIndex].Values[0].ToString();
            Session.Add("PromotionMedia", mediaID);
            Redirector.GoToRequestedPage("~/Admin/Client/NewEditPromotionGallery.aspx");
        }

        protected void gvPromotionGalleryManagement_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPromotionGalleryManagement.EditIndex = e.NewEditIndex;
            ViewState["Index"] = e.NewEditIndex;
            //rebind the gridview
            SearchPromotionGallery();
        }

        protected void gvPromotionGalleryManagament_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                mediaID = gvPromotionGalleryManagement.DataKeys[e.RowIndex]["MediaID"].ToString();
                SetPromotionGalleryActiveStatus(mediaID, Convert.ToInt16(ViewState["Index"]));
                gvPromotionGalleryManagement.EditIndex = -1;
                // rebind the gridview
                SearchPromotionGallery();
            }
        }

        protected void gvPromotionGalleryManagement_RowCancellingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPromotionGalleryManagement.EditIndex = -1;
            // rebind gridview
            SearchPromotionGallery();
        }

        private void SetPromotionGalleryActiveStatus(string mediaID, int index)
        {
            bool isActive = ((CheckBox)gvPromotionGalleryManagement.Rows[index].FindControl("chkEditActive")).Checked;
            EventManager.SetMediaGalleryActiveStatus(mediaID, isActive);
        }

        private void SearchAllPromotionGallery()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Events.PageSize;
            Result<MediaGalleryDetails> result = EventManager.SelectMediaGalleriesByPromotionID(promotionID, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPromotionGalleryManagement.DataSource = result.EntityList;
                gvPromotionGalleryManagement.DataBind();
                SetupPaging();
            }
        }

        private void SearchPublishedPromotionGallery()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Events.PageSize;
            Result<MediaGalleryDetails> result = EventManager.SelectMediaGalleriesByPromotionIDWithPublication(promotionID, true, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPromotionGalleryManagement.DataSource = result.EntityList;
                gvPromotionGalleryManagement.DataBind();
                SetupPaging();
            }
        }

        private void SearchUnPublishedPromotionGallery()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Events.PageSize;
            Result<MediaGalleryDetails> result = EventManager.SelectMediaGalleriesByPromotionIDWithPublication(promotionID, false, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvPromotionGalleryManagement.DataSource = result.EntityList;
                gvPromotionGalleryManagement.DataBind();
                SetupPaging();
            }
        }
        //FUTURE PROMOTIONS

        protected void rbtnOptionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            pgObj.StartRowIndex = 1;
            pgObj.SearchMode = rbtnOptionList.SelectedValue;
            SearchPromotionGallery();
        }

        private void SearchPromotionGallery()
        {
            switch (pgObj.SearchMode)
            {
                case "Enabled":
                    SearchPublishedPromotionGallery();
                    break;
                case "Disabled":
                    SearchUnPublishedPromotionGallery();
                    break;
                default:
                    SearchAllPromotionGallery();
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
            SearchPromotionGallery();
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
            SearchPromotionGallery();
        }

        private void SetupPaging()
        {
            if (gvPromotionGalleryManagement.Rows.Count > 0)
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