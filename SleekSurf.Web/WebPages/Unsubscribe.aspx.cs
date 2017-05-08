using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Manager;
using SleekSurf.Entity;
using SleekSurf.FrameWork;

namespace SleekSurf.Web.WebPages
{
    public partial class Unsubscribe : System.Web.UI.Page
    {
        string customerID = "";
        string clientID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null && Request.QueryString["CID"] != null)
            {
                customerID = Request.QueryString["ID"];
                clientID = Request.QueryString["CID"];
                UnsubscribeCustomer(customerID, clientID);
            }
            else
            {
                ltrVerificationMessage.Text = "Incomplete Information.";
                ltrMessageBoard.Text = "You have provided an incomplete information. Please try again.";
            }
        }

        private void UnsubscribeCustomer(string customerID, string clientID)
        {
            bool result = false;
            try
            {
                result = CustomerManager.SetCustomerToEmailSubscription(customerID, clientID, false);
                if (result)
                {
                    ltrVerificationMessage.Text = "Unsubscription successful.";
                    ltrMessageBoard.Text = "Your email has been removed from our subsctiption list.";
                }
                else
                {
                    ltrVerificationMessage.Text = "Unsubscription unsuccessful";
                    ltrMessageBoard.Text = "The your request could not be fulfilled. Please try again or contact us with your email detail to unsubscribe.";
                }

            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                ltrVerificationMessage.Text = "Opps! Server error.";
                ltrMessageBoard.Text = "The your request could not be fulfilled beacuse of the internal error. We 'll fix that error as soon as possible. We are extremely sorry for the inconvenience.";
            }

            ltrCompanyName.Text = ClientManager.SelectClient(clientID).EntityList[0].ClientName;
        }
    }
}