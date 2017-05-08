using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using SleekSurf.Manager;

namespace SleekSurf.Web.Admin.Client
{
    public partial class NewEditService : System.Web.UI.Page
    {
        string serviceID = null;
        ServiceDetails serviceDetails = new ServiceDetails();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Service"] != null)
            {
                serviceID = ((ServiceDetails)Session["Service"]).ServiceID;
                serviceDetails.ServiceID = serviceID;
                if (!IsPostBack)
                {
                    lblTitle.Text += " - Update Mode";
                    BindServiceDetails();
                }
            }
            else
            {
                if (!IsPostBack)
                    lblTitle.Text += " - Add Mode";
            }

            if (WebContext.Parent != null)
                serviceDetails.Client = new ClientDetails() { ClientID = WebContext.Parent.ClientID };

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("ClientAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("ServiceMgmt"))].Selected = true;

            if (WebContext.Sibling != null)
            {
                Menu tmpMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
                tmpMenu.Items[tmpMenu.Items.IndexOf(tmpMenu.FindItem("ClientMgmt"))].Selected = true;
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session.Remove("Service");
            Response.Redirect("~/Admin/Client/ServiceManagement.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                if (edServiceDescription.Content.Trim() != string.Empty)
                {
                    SaveService();
                    edServiceDescription.Content = string.Empty;
                }
                if (Session["Service"] == null)
                    lblTitle.Text += " - Add Mode";
            }
        }

        private void BindServiceDetails()
        {
            serviceDetails = ClientManager.SelectService(serviceID, WebContext.Parent.ClientID).EntityList[0];
            edServiceDescription.Content = serviceDetails.ServiceDescription;
        }

        private void SaveService()
        {
            Result<ServiceDetails> result = new Result<ServiceDetails>();
            serviceDetails.ServiceDescription = edServiceDescription.Content;

            if (!string.IsNullOrWhiteSpace(serviceDetails.ServiceID))
            {
                result = ClientManager.UpdateServiceForClient(serviceDetails);
                if (result.Status == ResultStatus.Success)
                {
                    lblMessage.CssClass = "successMsg";
                    Session.Remove("Service");
                }
                else
                    lblMessage.CssClass = "errorMsg";

                lblMessage.Text = result.Message;
            }
            else
            {
                serviceDetails.ServiceID = System.DateTime.Now.ToString("SV-ddMMyyy-HHmmssfff");
                result = ClientManager.InsertServiceForClient(serviceDetails);
                if (result.Status == ResultStatus.Success)
                    lblMessage.CssClass = "successMsg";
                else
                    lblMessage.CssClass = "errorMsg";

                lblMessage.Text = result.Message;
            }
        }
    }
}