using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using SleekSurf.Manager;

namespace SleekSurf.Web.MasterPageControls
{
    public partial class SearchBusiness : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindCategory();
        }

        private void BindCategory()
        {
            List<CategoryDetails> categoryList = ClientManager.GetCategories().EntityList;
            ddlCategory.DataSource = categoryList;
            ddlCategory.DataValueField = "CategoryName";
            ddlCategory.DataTextField = "CategoryName";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, "Select Business Type");
        }

        public string BusinessCategory
        {
            get
            {
                if (ddlCategory.SelectedIndex == 0)
                    return null;
                return ddlCategory.SelectedItem.Text;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    ddlCategory.SelectedValue = value;
                else
                    ddlCategory.SelectedValue = ddlCategory.Items[0].Value;
            }
        }

        public string BusinessName
        {
            get
            {
                if (txtBusinessName.Text.Length == 0)
                    return "";
                return txtBusinessName.Text;
            }

            set
            {
                txtBusinessName.Text = value;
            }
        }
        public string BusinessLocation
        {
            get
            {
                if (txtLocation.Text.Length == 0)
                    return "";
                else
                    return txtLocation.Text;
            }
            set
            {
                txtLocation.Text = value;
            }
        }

        protected void btnSearchBusiness_Click(object sender, EventArgs e)
        {
            if (txtLocation.Text.Length > 0)
            {

                string business = BusinessName.Replace(" ", "-");
                string category = BusinessCategory;
                string location = BusinessLocation.Replace(" ", "-");
                if (category != null)
                    Redirector.GoToRequestedPage("~/WebPages/BusinessCatalog.aspx?Name=" + business + "&Cat=" + category + "&Location=" + location);
                else
                    Redirector.GoToRequestedPage("~/WebPages/BusinessCatalog.aspx?Name=" + business + "&Location=" + location);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "alert('Location is required!');", true);
            }
        }

        protected void btnNearLocation_Click(object sender, EventArgs e)
        {
            string business = BusinessName.Replace(" ", "-");
            string category = BusinessCategory;
            string location = "nearby";
            Redirector.GoToRequestedPage("~/WebPages/BusinessCatalog.aspx?Name=" + business + "&Cat=" + category + "&Location=" + location);
        }

        public string PrimaryKey(string vPrimaryKey)
        {
            if (null != ViewState[vPrimaryKey])
            {
                return ViewState[vPrimaryKey].ToString();
            }
            if (null != Request.QueryString[vPrimaryKey])
            {
                ViewState[vPrimaryKey] = Request.QueryString[vPrimaryKey];
                return Request.QueryString[vPrimaryKey];
            }
            return string.Empty;
        }

    }
}