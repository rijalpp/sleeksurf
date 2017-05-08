using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SleekSurf.FrameWork;

namespace SleekSurf.Web.WebPageControls
{
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
    public partial class Pager : System.Web.UI.UserControl
    {
        public void Show(int totalRecords, int currentPage, int howManyPages, string firstPageUrl, string pageUrlFromat, bool showPages)
        {
            if (howManyPages > 0)
            {
                this.Visible = true;

                //displays current page and no of pages
                ltrCurrentpage.Text = string.Format("<span>Page </span>{0} ", currentPage);
                ltrHowManyPages.Text = string.Format("<span>of </span>{0}", howManyPages);

                //display total no of records
                ltrTotalRecord.Text = string.Format("<span>Total Record(s): </span>{0}", totalRecords);

                //creates the previous link
                if (currentPage == 1)
                {
                    previousLink.Enabled = false;
                    nextLink.Enabled = true;
                }
                else
                {
                    NameValueCollection query = Request.QueryString;
                    string paramName, newQueryString = "?";
                    for (int i = 0; i < query.Count; i++)
                        if (query.AllKeys[i] != null)
                            if ((paramName = query.AllKeys[i].ToString()).ToUpper() != "PAGE")
                                newQueryString += paramName + "=" + query[i] + "&";
                    nextLink.PostBackUrl = Request.Url.AbsolutePath + newQueryString + "Page=" + (currentPage + 1).ToString();
                    previousLink.PostBackUrl = Request.Url.AbsolutePath + newQueryString + "Page=" + (currentPage - 1).ToString();
                }
                //creates the next link
                if (currentPage == howManyPages)
                {
                    previousLink.Enabled = true;
                    nextLink.Enabled = false;
                }
                else
                {
                    NameValueCollection query = Request.QueryString;
                    string paramName, newQueryString = "?";
                    for (int i = 0; i < query.Count; i++)
                        if (query.AllKeys[i] != null)
                            if ((paramName = query.AllKeys[i].ToString()).ToUpper() != "PAGE")
                                newQueryString += paramName + "=" + query[i] + "&";
                    nextLink.PostBackUrl = Request.Url.AbsolutePath + newQueryString + "Page=" + (currentPage + 1).ToString();
                    previousLink.PostBackUrl = Request.Url.AbsolutePath + newQueryString + "Page=" + (currentPage - 1).ToString();
                }

                if (howManyPages == 1)
                {
                    nextLink.Visible = false;
                    previousLink.Visible = false;
                }

                if (showPages)
                {
                    //hiding other informations
                    ltrCurrentpage.Visible = false;
                    ltrHowManyPages.Visible = false;
                    ltrTotalRecord.Visible = false;
                    if (howManyPages > 1)
                    {
                        //list the pages and their url as an array
                        PageUrl[] pages = new PageUrl[howManyPages];
                        //generate pages url elements
                        pages[0] = new PageUrl("1", firstPageUrl);
                        for (int i = 2; i <= howManyPages; i++)
                        {
                            pages[i - 1] = new PageUrl(i.ToString(), string.Format(pageUrlFromat, i));
                        }
                        //don't generate the link for current page
                        pages[currentPage - 1] = new PageUrl((currentPage.ToString()), "");
                        //feeds the pages to the repeater
                        pagesRepeater.DataSource = pages;
                        pagesRepeater.DataBind();


                        LinkButton btnPager = (LinkButton)pagesRepeater.Items[currentPage - 1].FindControl("hyperlink");
                        btnPager.CssClass = "currentPage";
                        btnPager.Enabled = false;

                        if (currentPage == 1)
                        {
                            previousLink.Enabled = false;
                            nextLink.Enabled = true;
                        }
                        else if (currentPage == howManyPages)
                        {
                            previousLink.Enabled = true;
                            nextLink.Enabled = false;
                        }
                        else
                        {
                            previousLink.Enabled = true;
                            nextLink.Enabled = true;
                        }
                    }
                }
                else
                {
                    previousLink.Visible = false;
                    nextLink.Visible = false;
                    pagesRepeater.Visible = false;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}