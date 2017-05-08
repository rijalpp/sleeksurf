using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using SleekSurf.Entity;
using SleekSurf.Manager;
using System.Transactions;
using System.Web.Security;

namespace SleekSurf.Web.Admin.Client
{
    public partial class NewEditCustomerGroup : BasePage
    {
        string customerGroupID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CustomerGroupDetails"] != null)
            {
                customerGroupID = Session["CustomerGroupDetails"].ToString();
                chkPublished.Enabled = IsSetPublishEnabled(customerGroupID, WebContext.Parent.ClientID);
            }

            if (!IsPostBack)
            {

                if (Session["CustomerGroupDetails"] != null)
                {
                    lblTitle.Text += " - Update Mode";
                    BindCustomerGroupDetails();
                }
                else
                {
                    lblTitle.Text += " - Add Mode";
                    chkPublished.Checked = true;
                    chkPublished.Enabled = false;
                }
            }
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

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session.Remove("CustomerGroupDetails");
            Response.Redirect("~/Admin/Client/CustomerGroupManagement.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        SaveCustomerGroup();
                        scope.Complete();
                    }
                    catch(Exception ex)
                    {
                        Helpers.LogError(ex);
                    }
                }
            }
        }

        protected void txtGroupName_TextChanged(object sender, EventArgs e)
        {
            string groupName = txtGroupName.Text;
            if (!string.IsNullOrEmpty(groupName))
            {
                if (!string.IsNullOrEmpty(customerGroupID))//IN UPDATE MODE
                {
                    if (groupName != CustomerManager.SelectCustomerGroup(customerGroupID, WebContext.Parent.ClientID).EntityList[0].GroupName)
                    {
                        if (CustomerManager.DoesGroupNameExist(WebContext.Parent.ClientID, groupName))
                        {
                            lblGroupNameExists.Text = "GroupName exists!";
                            txtGroupName.Text = string.Empty;
                        }
                    }
                }
                else//IN INSERT MODE
                {
                    if (CustomerManager.DoesGroupNameExist(WebContext.Parent.ClientID, groupName))
                    {
                        lblGroupNameExists.Text = "GroupName exists!";
                        txtGroupName.Text = string.Empty;
                    }
                }
            }
        }

        private int CountCustomers(string customerGroupID, string clientID)
        {
            return CustomerManager.CountCustomersInCustomerGroup(customerGroupID, clientID);
        }

        private bool IsSetPublishEnabled(string customerGroupID, string clientID)
        {
            bool result = false;
            int i = CountCustomers(customerGroupID, clientID);
            if (i == 0)
                result = true;
            return result;
        }

        private void BindCustomerGroupDetails()
        {
            CustomerGroupDetails customerGroup = CustomerManager.SelectCustomerGroup(customerGroupID, WebContext.Parent.ClientID).EntityList[0];

            txtGroupName.Text = customerGroup.GroupName;
            txtDescription.Text = customerGroup.Description;
            txtComment.Text = customerGroup.Comments;
            chkPublished.Checked = customerGroup.Published;
        }

        private void SaveCustomerGroup()
        {
            CustomerGroupDetails customerGroup = new CustomerGroupDetails();
            customerGroup.GroupName = txtGroupName.Text;
            customerGroup.Description = txtDescription.Text;
            customerGroup.Comments = txtComment.Text;
            customerGroup.Published = chkPublished.Checked;
            customerGroup.ClientID = WebContext.Parent.ClientID;

            Result<CustomerGroupDetails> result = new Result<CustomerGroupDetails>();
            if (!string.IsNullOrWhiteSpace(customerGroupID))
            {
                customerGroup.CustomerGroupID = customerGroupID;
                result = CustomerManager.UpdateCustomerGroup(customerGroup);
            }
            else
            {
                customerGroup.CreatedBy = HttpContext.Current.User.Identity.Name;
                customerGroup.CustomerGroupID = System.DateTime.Now.ToString("CTG-ddMMyyy-HHmmssfff");
                result = CustomerManager.InsertCustomerGroup(customerGroup);
            }
            if (result.Status == ResultStatus.Success)
            {
                lblMessage.CssClass = "successMsg";
                Session.Remove("CustomerGroupDetails");
                lblTitle.Text += " - Add Mode";
                ClearFields();
            }
            else
                lblMessage.CssClass = "errorMsg";

            lblMessage.Text = result.Message;
        }

        private void ClearFields()
        {
            txtGroupName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtComment.Text = string.Empty;
            chkPublished.Checked = true;
            chkPublished.Enabled = false;
        }        
    }
}