using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using SleekSurf.Manager;
using System.IO;
using System.Web.UI.HtmlControls;

namespace SleekSurf.Web.Admin.Client
{
    public partial class ShowClientCustomers : System.Web.UI.Page
    {
        string selectionFor = "";
        List<CustomerDetails> tempSessionCustomers = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            selectionFor = WebContext.GetQueryStringValue("For").Trim();
            if (!IsPostBack)
            {
                Result<CustomerDetails> result = new Result<CustomerDetails>();
                if(string.Compare(selectionFor, "email", true) == 0)
                {
                    tempSessionCustomers = (List<CustomerDetails>)Session["CustomCustomerSelectionForEmail"];
                    result = CustomerManager.SelectAllClientCustomersWithSubscriptionEmail(WebContext.Parent.ClientID, true);

                }
                else if(string.Compare(selectionFor, "sms", true) == 0)
                {
                    tempSessionCustomers = (List<CustomerDetails>)Session["CustomCustomerSelectionForSMS"];
                    result = CustomerManager.SelectAllClientCustomersWithSubscriptionSMS(WebContext.Parent.ClientID, true);

                }

                if (result.Status == ResultStatus.Success)
                {
                    gvCustomerManagement.DataSource = result.EntityList;
                    gvCustomerManagement.DataBind();
                }
            }
        }

        protected void gvCustomerManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string customerID = gvCustomerManagement.DataKeys[e.Row.RowIndex]["CustomerID"].ToString();
                string clientID = ((ClientDetails)gvCustomerManagement.DataKeys[e.Row.RowIndex]["ClientID"]).ClientID;
                Result<CustomerDetails> result = new Result<CustomerDetails>();
                result = CustomerManager.SelectCustomer(customerID, clientID);

                CustomerDetails tempCustomer = new CustomerDetails();
                if (result.EntityList.Count > 0)
                    tempCustomer = result.EntityList[0];

                HtmlImage iThumb = e.Row.FindControl("iThumb") as HtmlImage;
                CheckBox chkCustomer = e.Row.FindControl("chkCustomer") as CheckBox;

                if (tempCustomer != null)
                {
                    if (tempSessionCustomers != null)
                        chkCustomer.Checked = tempSessionCustomers.Contains(tempCustomer);

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
        }

        protected void btnFinish_Click(object sender, EventArgs e)
        {
            List<CustomerDetails> customSelection = new List<CustomerDetails>();

            foreach (GridViewRow row in gvCustomerManagement.Rows)
            {
                CheckBox chkSelected = (CheckBox)row.FindControl("chkCustomer");
                string customerID = "", clientID = "";
                if (chkSelected !=null && chkSelected.Checked)
                {
                    customerID = gvCustomerManagement.DataKeys[row.RowIndex]["CustomerID"].ToString();
                    clientID = ((ClientDetails)gvCustomerManagement.DataKeys[row.RowIndex]["ClientID"]).ClientID;

                    Result<CustomerDetails> result = CustomerManager.SelectCustomer(customerID, clientID);

                    if (result.Status == ResultStatus.Success && result.EntityList[0] != null)
                        customSelection.Add(result.EntityList[0]);
                }
            }

            if(string.Compare(selectionFor, "email", true) == 0)
            {
                if (!WebContext.ContainsInSession("CustomCustomerSelectionForEmail"))
                    Session.Add("CustomCustomerSelectionForEmail", customSelection);
                else
                    Session["CustomCustomerSelectionForEmail"] = customSelection;
            }
            else if(string.Compare(selectionFor, "sms", true) == 0)
            {
                if (!WebContext.ContainsInSession("CustomCustomerSelectionForSMS"))
                    Session.Add("CustomCustomerSelectionForSMS", customSelection);
                else
                    Session["CustomCustomerSelectionForSMS"] = customSelection;
            }

            ClientScript.RegisterStartupScript(GetType(), "FancyBoxClose", "parent.jQuery.fancybox.close()", true);
        }
    }
}