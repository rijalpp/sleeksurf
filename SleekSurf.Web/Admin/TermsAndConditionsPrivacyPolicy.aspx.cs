using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using SleekSurf.Manager;

namespace SleekSurf.Web.Admin
{
    public partial class TermsAndConditionsPrivacyPolicy : System.Web.UI.Page
    {
        static DataExtenderDetails dataExtender = null;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadBusinessTerms();
        }
        private void LoadBusinessTerms()
        {
            Result<DataExtenderDetails> result = new Result<DataExtenderDetails>();
            if (WebContext.Parent != null)//i.e. THE SUPERADMIN IS LOGGED IN AND UPDATING THE CLIENT DATA
                result = ClientManager.SelectDataExtenderByClient(WebContext.Parent.ClientID);
            else//.i.e.  THE SUPERADMIN IS LOGGED IN AND UPDATING OWN DATA
                result = ClientManager.SelectDataExtenderWithClientNull();

            if (result.EntityList.Count > 0)
                dataExtender = result.EntityList[0];
            else
                dataExtender = null;

            if (dataExtender != null)
            {
                editorTermsAndConditions.Content = dataExtender.TermsAndConditions;
                editorPrivacyAndPolicy.Content = dataExtender.PrivacyAndPolicy;
            }

            ShowHidePanel();
        }

        private void ShowHidePanel()
        {
            if (Request.QueryString["TermsAndConditions"] != null)
            {
                pnlTermsAndCond.Visible = true;
                pnlPrivacyAndPolicy.Visible = false;
            }
            else if (Request.QueryString["PrivacyAndPolicy"] != null)
            {
                pnlTermsAndCond.Visible = false;
                pnlPrivacyAndPolicy.Visible = true;
            }
            else
            {
                pnlTermsAndCond.Visible = true;
                pnlPrivacyAndPolicy.Visible = true;
            }
        }

        private void SaveBusinessTerms()
        {
            Result<DataExtenderDetails> result = new Result<DataExtenderDetails>();

            if (dataExtender == null)
                dataExtender = new DataExtenderDetails();

            dataExtender.PrivacyAndPolicy = editorPrivacyAndPolicy.Content;
            dataExtender.TermsAndConditions = editorTermsAndConditions.Content;
            if (WebContext.Parent == null)
            {
                dataExtender.Client = new ClientDetails() { ClientID = null };
            }
            else
                dataExtender.Client = WebContext.Parent;

            //save into database
            if (dataExtender.DataExtenderID == 0)
                result = ClientManager.InsertDataExtender(dataExtender);
            //update into the database
            else
            {
                if (Request.QueryString["TermsAndConditions"] != null)
                {
                    result = ClientManager.UpdateTermsAndConditions(dataExtender);
                }

                else if (Request.QueryString["PrivacyAndPolicy"] != null)
                {
                    result = ClientManager.UpdatePrivacyAndPolicy(dataExtender);
                }

                else
                {
                    result = ClientManager.UpdateDataExtender(dataExtender);
                }
            }

            if (result.Status == ResultStatus.Success)
                lblMessage.CssClass = "successMsg";
            else
                lblMessage.CssClass = "errorMsg";

            lblMessage.Text = result.Message;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            SaveBusinessTerms();
            LoadBusinessTerms();
        }
    }
}