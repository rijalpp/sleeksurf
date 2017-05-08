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
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Services;

namespace SleekSurf.Web.Admin.Client
{
    public partial class CustomerManagement : System.Web.UI.Page
    {
        static PagingDetails pgObj = null;
        string customerID = "";
        string clientID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("ClientAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("CustomerMgmt"))].Selected = true;

            if (WebContext.Sibling != null)
            {
                Menu tmpMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
                tmpMenu.Items[tmpMenu.Items.IndexOf(tmpMenu.FindItem("ClientMgmt"))].Selected = true;
            }

            if (!IsPostBack)
            {
                pgObj = new PagingDetails();
                BindCustomerGroups();
                SearchCustomers();
            }

            if (User.IsInRole("AdminUser"))
                gvCustomerManagement.Columns[0].Visible = false;

            txtNewGroup.Text = string.Empty;

        }

        protected void imgDeleteBtn_Click(object sender, EventArgs e)
        {
            // List<Guid> userList = new List<Guid>();
            List<string> customerIDList = new List<string>();
            foreach (GridViewRow row in gvCustomerManagement.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        customerID = gvCustomerManagement.DataKeys[row.RowIndex]["CustomerID"].ToString();
                        customerIDList.Add(customerID);
                    }

                }

            }

            //Call the method to Delete records 
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (string custID in customerIDList)
                    {
                        CustomerManager.DeleteCustomer(custID);
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
            SearchCustomers();
        }

        protected void gvCustomerManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((CheckBox)e.Row.FindControl("cbDelete")).Attributes.Add("onclick", "javascript:UpdateSelectAllAndDeleteControl('" + gvCustomerManagement.ClientID + "', 'cbDelete', 'checkAll', 'imgDeleteBtn');");

                string customerID = gvCustomerManagement.DataKeys[e.Row.RowIndex]["CustomerID"].ToString();
                string clientID = ((ClientDetails)gvCustomerManagement.DataKeys[e.Row.RowIndex]["ClientID"]).ClientID;
                Result<CustomerDetails> result = new Result<CustomerDetails>();
                result = CustomerManager.SelectCustomer(customerID, clientID);

                CustomerDetails tempCustomer = new CustomerDetails();
                if (result.EntityList.Count > 0)
                    tempCustomer = result.EntityList[0];

                HtmlImage iThumb = e.Row.FindControl("iThumb") as HtmlImage;

                if (tempCustomer != null)
                {
                    string avatarUrl = string.Format("~/Uploads/{0}/CustomersPicture/{1}.jpg", clientID, customerID);
                    if (!string.IsNullOrWhiteSpace(avatarUrl))
                    {

                        if (File.Exists(Server.MapPath(avatarUrl)))
                            iThumb.Src = avatarUrl + "?" + (new DateTime()).Millisecond;
                        else
                        {
                            if(string.Compare(tempCustomer.Gender, "female", true) == 0)
                                iThumb.Src = "~/App_Themes/SleekTheme/Images/ProfileFemale.png";
                            else
                                iThumb.Src = "~/App_Themes/SleekTheme/Images/ProfileMale.png";
                        }

                    }
                    else
                    {
                        if (string.Compare(tempCustomer.Gender, "female", true) == 0)
                            iThumb.Src = "~/App_Themes/SleekTheme/Images/ProfileFemale.png";
                        else
                            iThumb.Src = "~/App_Themes/SleekTheme/Images/ProfileMale.png";
                    }
                }
                else
                {
                    iThumb.Src = "~/App_Themes/SleekTheme/Images/ProfileMale.png";
                }

                iThumb.Alt = "No Images";
            }
           
            else if(e.Row.RowType == DataControlRowType.Footer)
            {
                ((ImageButton)e.Row.FindControl("imgDeleteBtn")).OnClientClick = "return DeleteConfirmation('" + gvCustomerManagement.ClientID + "', 'cbDelete');";
            }

        }

        protected void gvCustomerManagement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            customerID = gvCustomerManagement.DataKeys[e.NewSelectedIndex].Values[0].ToString();
            Session.Add("Customer", customerID);
            Redirector.GoToRequestedPage("~/Admin/Client/NewEditCustomer.aspx");
        }

        protected void gvCustomerManagement_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCustomerManagement.EditIndex = e.NewEditIndex;
            ViewState["Index"] = e.NewEditIndex;
            SearchCustomers();
        }

        protected void gvCustomerManagament_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                customerID = gvCustomerManagement.DataKeys[e.RowIndex]["CustomerID"].ToString();
                clientID = ((ClientDetails)gvCustomerManagement.DataKeys[e.RowIndex]["ClientID"]).ClientID;
                CustomerDetails customerDetails = CustomerManager.SelectCustomer(customerID, clientID).EntityList[0];
                UpdateCustomer(customerDetails, Convert.ToInt16(ViewState["Index"]));
                gvCustomerManagement.EditIndex = -1;
                SearchCustomers();
            }
        }

        protected void gvCustomerManagement_RowCancellingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCustomerManagement.EditIndex = -1;
            SearchCustomers();
        }

        private void UpdateCustomer(CustomerDetails selectedCustomer, int index)
        {
            selectedCustomer.Email = ((TextBox)gvCustomerManagement.Rows[index].FindControl("txtEmail")).Text;
            selectedCustomer.ContactMobile = ((TextBox)gvCustomerManagement.Rows[index].FindControl("txtEditContactMobile")).Text;
            CustomerManager.UpdateCustomer(selectedCustomer);
        }

        private void BindCustomerGroups()
        {
            List<CustomerGroupDetails> customerGroups = new List<CustomerGroupDetails>();

            foreach (CustomerGroupDetails customerGroup in CustomerManager.SelectAllCustomerGroup(WebContext.Parent.ClientID).EntityList)
            {
                customerGroups.Add(customerGroup);
            }

            CustomerGroupDetails defaultGroup = new CustomerGroupDetails()
            {
                ClientID = WebContext.Parent.ClientID,
                CustomerGroupID = "0",
                GroupName = "Default",
                Description = "Added Programmatically",
                CustomerCount = CustomerManager.CountCustomersInCustomerGroup("0", WebContext.Parent.ClientID)
            };

            if (!customerGroups.Contains(defaultGroup))
                customerGroups.Insert(0, defaultGroup);

            rptrCustomerGroup.DataSource = customerGroups;
            rptrCustomerGroup.DataBind();

            menuCustomerGroup.Items.Clear();
            foreach (CustomerGroupDetails customerGroup in customerGroups)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Text = customerGroup.GroupName + "(" + customerGroup.CustomerCount + ")";
                menuItem.Value = customerGroup.CustomerGroupID;
                if (string.Compare(customerGroup.GroupName, "default", true) == 0)
                    menuItem.Selected = true;
                menuCustomerGroup.Items.Add(menuItem);
            }
        }

        private void SearchCustomerByName()
        {
            pgObj.StartRowIndex = pgObj.StartRowIndex;
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Customers.PageSize;
            string name = txtCustomerName.Text;
            string customerGroupID = menuCustomerGroup.SelectedValue;
            Result<CustomerDetails> result = CustomerManager.SelectCustomerByFullName(name, WebContext.Parent.ClientID, customerGroupID, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvCustomerManagement.DataSource = result.EntityList;
                gvCustomerManagement.DataBind();
                SetupPaging();
            }
        }

        private void SearchCustomerByClient()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Customers.PageSize;
            string name = txtCustomerName.Text;
            string customerGroupID = menuCustomerGroup.SelectedValue;
            Result<CustomerDetails> result = CustomerManager.SelectCustomersByClientID(WebContext.Parent.ClientID, customerGroupID, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvCustomerManagement.DataSource = result.EntityList; ;
                gvCustomerManagement.DataBind();
                SetupPaging();
            }
        }

        private void SearchCustomerByCustomerGroup()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Customers.PageSize;

            string name = txtCustomerName.Text;
            string customerGroupID = menuCustomerGroup.SelectedValue;
            Result<CustomerDetails> result = new Result<CustomerDetails>();
            if (txtCustomerName.Text.Length > 0 && txtCustomerName.Text != "Search Customer")
                result = CustomerManager.SelectCustomerByFullName(name, WebContext.Parent.ClientID, customerGroupID, pgObj);
            else
                result = CustomerManager.SelectCustomersByClientID(WebContext.Parent.ClientID, customerGroupID, pgObj);
            if (result.Status == ResultStatus.Success)
            {
                gvCustomerManagement.DataSource = result.EntityList; ;
                gvCustomerManagement.DataBind();
                SetupPaging();
            }
        }

        private void SearchCustomers()
        {
            switch (pgObj.SearchMode)
            {
                case "Name":
                    SearchCustomerByName();
                    break;
                case "Group":
                    SearchCustomerByCustomerGroup();
                    break;
                default:
                    SearchCustomerByClient();
                    break;
            }
        }

        public void lbtnCustomerGroup_Command(object sender, CommandEventArgs e)
        {
            List<string> customerIDList = new List<string>();
            foreach (GridViewRow row in gvCustomerManagement.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        customerID = gvCustomerManagement.DataKeys[row.RowIndex]["CustomerID"].ToString();
                        customerIDList.Add(customerID);
                    }
                }
            }
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (string custID in customerIDList)
                    {
                        CustomerManager.AssignCustomerToCustomerGroup(custID, WebContext.Parent.ClientID, (string)e.CommandArgument);
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
            SearchCustomers();

            string selectedMenuBeforeRefresh = menuCustomerGroup.SelectedValue;
            List<CustomerGroupDetails> customerGroups = new List<CustomerGroupDetails>();

            foreach (CustomerGroupDetails customerGroup in CustomerManager.SelectAllCustomerGroup(WebContext.Parent.ClientID).EntityList)
            {
                customerGroups.Add(customerGroup);
            }

            CustomerGroupDetails defaultGroup = new CustomerGroupDetails()
            {
                ClientID = WebContext.Parent.ClientID,
                CustomerGroupID = "0",
                GroupName = "Default",
                Description = "Added Programmatically",
                CustomerCount = CustomerManager.CountCustomersInCustomerGroup("0", WebContext.Parent.ClientID)
            };

            if (!customerGroups.Contains(defaultGroup))
                customerGroups.Insert(0, defaultGroup);

            menuCustomerGroup.Items.Clear();
            foreach (CustomerGroupDetails customerGroup in customerGroups)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Text = customerGroup.GroupName + "(" + customerGroup.CustomerCount + ")";
                menuItem.Value = customerGroup.CustomerGroupID;
                if (customerGroup.CustomerGroupID == selectedMenuBeforeRefresh)
                    menuItem.Selected = true;
                menuCustomerGroup.Items.Add(menuItem);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            pgObj.StartRowIndex = 1;
            pgObj.SearchMode = "Name";
            pgObj.SearchKey = txtCustomerName.Text;
            SearchCustomers();
        }

        private string[] SplitWord(string keywords)
        {
            return Regex.Split(keywords, @"\W+");
        }

        protected void menuCustomerGroup_MenuItemClick(object sender, MenuEventArgs e)
        {
            pgObj.SearchMode = "Group";
            pgObj.StartRowIndex = 1;
            SearchCustomers();
        }

        protected void ibtnNewGroup_Click(object sender, ImageClickEventArgs e)
        {
            if (CustomerManager.DoesGroupNameExist(WebContext.Parent.ClientID, txtNewGroup.Text.Trim()) || txtNewGroup.Text.Trim().ToLower() == "default")
            {
                cvNewGroup.ValueToCompare = txtNewGroup.Text;
                cvNewGroup.Validate();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowValidationMsg", "$(document).ready(function () { $('#MoveToGroup').trigger('click');});", true);
            }
            else
            {
                CustomerGroupDetails customerGroup = new CustomerGroupDetails();
                customerGroup.GroupName = txtNewGroup.Text;
                customerGroup.Description = "Add description of " + txtNewGroup.Text;
                customerGroup.Comments = "Add comment of " + txtNewGroup.Text;
                customerGroup.Published = true;
                customerGroup.ClientID = WebContext.Parent.ClientID;
                customerGroup.CreatedBy = HttpContext.Current.User.Identity.Name;
                customerGroup.CustomerGroupID = System.DateTime.Now.ToString("CTG-ddMMyyy-HHmmssfff");

                Result<CustomerGroupDetails> result = new Result<CustomerGroupDetails>();
                result = CustomerManager.InsertCustomerGroup(customerGroup);

                if (result.Status == ResultStatus.Success)
                {
                    txtNewGroup.Text = string.Empty;
                    BindCustomerGroups();
                }
            }
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
            SearchCustomers();
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
            SearchCustomers();
        }

        private void SetupPaging()
        {
            if (gvCustomerManagement.Rows.Count > 0)
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