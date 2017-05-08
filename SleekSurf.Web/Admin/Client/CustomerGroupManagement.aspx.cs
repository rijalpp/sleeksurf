using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Manager;
using SleekSurf.FrameWork;
using SleekSurf.Entity;
using System.Transactions;
using System.Text.RegularExpressions;

namespace SleekSurf.Web.Admin.Client
{
    public partial class CustomerGroupManagement : System.Web.UI.Page
    {
        static string searchCustomerName = null;
        string customerGroupID = "";
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
                searchCustomerName = null;
                SearchCustomerGroups();
            }

            if (User.IsInRole("AdminUser"))
                gvCustomerGroupManagement.Columns[0].Visible = false;
        }

        protected void imgDeleteBtn_Click(object sender, EventArgs e)
        {
            // List<Guid> userList = new List<Guid>();
            List<string> customerGroupIDList = new List<string>();
            foreach (GridViewRow row in gvCustomerGroupManagement.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        customerGroupID = gvCustomerGroupManagement.DataKeys[row.RowIndex]["CustomerGroupID"].ToString();
                        customerGroupIDList.Add(customerGroupID);
                    }

                }

            }

            //Call the method to Delete records 
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (string custID in customerGroupIDList)
                    {
                        if (CustomerManager.DoesCustomerExist(WebContext.Parent.ClientID, custID))
                        {
                            string GroupName = CustomerManager.SelectCustomerGroup(custID, WebContext.Parent.ClientID).EntityList[0].GroupName;
                            lblMessage.CssClass = "errorMsg";
                            lblMessage.Text = "The Customer Group <span style='text-decoration:underline;'>" + GroupName + "</span> contains customers. Please remove the customers from the group before you delete.";
                            //REBIND THE GRIDVIEW
                            SearchCustomerGroups();
                            return;
                        }
                        CustomerManager.DeleteCustomerGroup(custID, WebContext.Parent.ClientID);
                    }
                    scope.Complete();
                    lblMessage.CssClass = "successMsg";
                    lblMessage.Text = "The selected Group(s) successfully deleted.";
                }
                catch(Exception ex)
                {
                    Helpers.LogError(ex);
                    lblMessage.Text = "The selected group(s) could not be deleted.";
                    throw;
                }
            }

            // rebind the GridView
            SearchCustomerGroups();
        }

        protected void gvCustomerGroupManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((CheckBox)e.Row.FindControl("cbDelete")).Attributes.Add("onclick", "javascript:UpdateSelectAllAndDeleteControl('" + gvCustomerGroupManagement.ClientID + "', 'cbDelete', 'checkAll', 'imgDeleteBtn');");

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    string customerGroupID = gvCustomerGroupManagement.DataKeys[e.Row.RowIndex]["CustomerGroupID"].ToString();
                    string clientID = gvCustomerGroupManagement.DataKeys[e.Row.RowIndex]["ClientID"].ToString();
                    CheckBox chkEditPublished = e.Row.FindControl("chkEditPublished") as CheckBox;
                    int customerCount = int.Parse(((Label)e.Row.FindControl("lblCustomerNumber")).Text);
                    chkEditPublished.Enabled = customerCount > 0 ? false : true;
                    if (chkEditPublished.Enabled == false)
                        chkEditPublished.ToolTip = "This Customer-Group can't be disabled because it has existing customer(s).";
                }
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((ImageButton)e.Row.FindControl("imgDeleteBtn")).OnClientClick = "return DeleteConfirmation('" + gvCustomerGroupManagement.ClientID + "', 'cbDelete');";
            }
        }

        protected void gvCustomerGroupManagement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            customerGroupID = gvCustomerGroupManagement.DataKeys[e.NewSelectedIndex].Values[0].ToString();
            Session.Add("CustomerGroupDetails", customerGroupID);
            Redirector.GoToRequestedPage("~/Admin/Client/NewEditCustomerGroup.aspx");
        }

        protected void gvCustomerGroupManagement_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCustomerGroupManagement.EditIndex = e.NewEditIndex;
            ViewState["Index"] = e.NewEditIndex;
            SearchCustomerGroups();
        }

        protected void gvCustomerGroupManagement_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                customerGroupID = gvCustomerGroupManagement.DataKeys[e.RowIndex]["CustomerGroupID"].ToString();
                clientID = gvCustomerGroupManagement.DataKeys[e.RowIndex]["ClientID"].ToString();
                CustomerGroupDetails customerGroupDetails = CustomerManager.SelectCustomerGroup(customerGroupID, clientID).EntityList[0];
                UpdateCustomerGroup(customerGroupDetails, Convert.ToInt16(ViewState["Index"]));
                gvCustomerGroupManagement.EditIndex = -1;
                SearchCustomerGroups();
            }
        }

        protected void gvCustomerGroupManagement_RowCancellingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCustomerGroupManagement.EditIndex = -1;
            SearchCustomerGroups();
        }

        protected void gvCustomerGroupManagement_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomerGroupManagement.PageIndex = e.NewPageIndex;
            SearchCustomerGroups();
        }

        private void UpdateCustomerGroup(CustomerGroupDetails selectedCustomerGroup, int index)
        {
            selectedCustomerGroup.Published = ((CheckBox)gvCustomerGroupManagement.Rows[index].FindControl("chkEditPublished")).Checked;
            CustomerManager.SetCustomerGroupPublishStatus(selectedCustomerGroup.CustomerGroupID, selectedCustomerGroup.ClientID, selectedCustomerGroup.Published);
        }

        private void SearchCustomerGroups()
        {
            if (searchCustomerName == null)
                gvCustomerGroupManagement.DataSource = CustomerManager.SelectAllCustomerGroup(WebContext.Parent.ClientID).EntityList;
            else
                gvCustomerGroupManagement.DataSource = CustomerManager.SelectCustomerGroupByName(WebContext.Parent.ClientID, searchCustomerName).EntityList;
            gvCustomerGroupManagement.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            searchCustomerName = txtCustomerGroupName.Text;
            SearchCustomerGroups();
        }

        private string[] SplitWord(string keywords)
        {
            return Regex.Split(keywords, @"\W+");
        }
    }
}