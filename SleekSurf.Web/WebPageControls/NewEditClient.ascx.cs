using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.Manager;
using SleekSurf.FrameWork;

namespace SleekSurf.Web.WebPageControls
{
    public partial class NewEditClient : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!HttpContext.Current.User.IsInRole("SuperAdmin"))
                pnlPublish.Visible = false;
            if (!IsPostBack)
            {
                ExtractTheme();
                ExtractCategories();
                ExtractAustralianStatesAndTerritories();
                ExtractCountries();
                cmpABN.ValueToCompare = ABN.ToLower();
                cmpUniqueName.ValueToCompare = "";
            }
        }

        protected void txtABN_TextChanged(object sender, EventArgs e)
        {
            ClientDetails client = null;
            //check whether the client exists
            Result<ClientDetails> result = ClientManager.SelectClientByABN(txtABN.Text);
            if (result.EntityList.Count > 0)
                client = result.EntityList[0];
            //if client exists show message and set empty value to comparefield validator unequal to the value of txtABN
            if (client != null)
            {
                if (WebContext.Parent != null)
                {
                    if (client.ABN == WebContext.Parent.ABN)
                    {
                        lblErrorABNMsg.Text = "";
                        cmpABN.ValueToCompare = txtABN.Text;
                    }
                    else
                    {
                        lblErrorABNMsg.Text = "ABN exists!";
                        cmpABN.ValueToCompare = "";
                        cmpABN.ErrorMessage = "*";
                    }
                }

                else if (client.ABN == txtABN.Text.Trim())
                {
                    lblErrorABNMsg.Text = "ABN exists!";
                    cmpABN.ValueToCompare = "";
                    cmpABN.ErrorMessage = "*";
                }
            }
            //if the record doesn't exist, set the value of comparefile validator equal to that of txtABN
            else
            {
                lblErrorABNMsg.Text = "";
                cmpABN.ValueToCompare = txtABN.Text;
            }
        }

        protected void txtUniqueIdentity_TextChanged(object sender, EventArgs e)
        {
            if (txtUniqueIdentity.Text.Length > 0)
            {
                if (!ClientManager.CheckClientUniqueIdentity(txtUniqueIdentity.Text))
                {
                    if (WebContext.Parent != null)
                    {
                        if (txtUniqueIdentity.Text == WebContext.Parent.UniqueIdentity)
                        {
                            lblErrorMsg.Text = "";
                            cmpUniqueName.ValueToCompare = "";
                        }
                        else
                        {
                            lblErrorMsg.Text = "Unique Name exists!";
                            cmpUniqueName.ValueToCompare = txtUniqueIdentity.Text;
                            cmpUniqueName.ErrorMessage = "*";
                        }
                    }
                    else
                    {
                        lblErrorMsg.Text = "Unique Name exists!";
                        cmpUniqueName.ValueToCompare = txtUniqueIdentity.Text;
                        cmpUniqueName.ErrorMessage = "*";
                    }
                }
                else
                {
                    lblErrorMsg.Text = "";
                    cmpUniqueName.ValueToCompare = "";
                }
            }
            else
            {
                lblErrorMsg.Text = "";
                cmpUniqueName.ValueToCompare = "";
            }
        }

        protected void txtUniqueDomain_TextChanged(object sender, EventArgs e)
        {
            if (!ClientManager.CheckClientUniqueDomain(new Uri(txtUniqueDomain.Text.ToLower()).Host))
            {
                if (WebContext.Parent != null)
                {
                    if (txtUniqueDomain.Text == WebContext.Parent.UniqueDomain)
                    {
                        lblErrorDomain.Text = "";
                        cmpUniqueDomain.ValueToCompare = "";
                    }
                    else
                    {
                        lblErrorDomain.Text = "Unique Domain exists!";
                        cmpUniqueDomain.ValueToCompare = txtUniqueDomain.Text;
                        cmpUniqueDomain.ErrorMessage = "*";
                    }
                }
                else
                {
                    lblErrorMsg.Text = "Unique Domain exists!";
                    cmpUniqueDomain.ValueToCompare = txtUniqueDomain.Text;
                    cmpUniqueDomain.ErrorMessage = "*";
                }
            }
            else
            {
                lblErrorDomain.Text = "";
                cmpUniqueDomain.ValueToCompare = "";
            }
        }

        private void ExtractTheme()
        {
            ddlTheme.DataSource = Helpers.GetThemes();
            ddlTheme.DataBind();
            ddlTheme.SelectedIndex = ddlTheme.Items.IndexOf(new ListItem("Default"));
        }

        private void ExtractCategories()
        {
            Result<CategoryDetails> result = ClientManager.GetCategories();
            if (result.Status == ResultStatus.Success)
            {
                ddlCategory.DataSource = result.EntityList;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, "Select Category");
            }
        }

        private void ExtractAustralianStatesAndTerritories()
        {
            ddlState.DataSource = Enum.GetNames(typeof(AustralianStatesAndTerritories));
            ddlState.DataBind();
            ddlState.Items.Insert(0, "Select State");
        }

        private void ExtractCountries()
        {
            Result<CountryDetails> result = CountryManager.GetCountries();
            if (result.Status == ResultStatus.Success)
            {
                ddlCountry.DataSource = result.EntityList;
                ddlCountry.DataTextField = "CountryName";
                ddlCountry.DataValueField = "CountryID";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, "Select Country");
            }

            ddlCountry.SelectedIndex = ddlCountry.Items.IndexOf(ddlCountry.Items.FindByText(Configuration.GetConfigurationSetting("HomeCountry", typeof(string)) as string));
        }

        public string CategoryID
        {
            get { return ddlCategory.SelectedValue; }
            set { ddlCategory.SelectedValue = value; }
        }

        public string BusinessClientID
        {
            get { return hfClientID.Value; }
            set { hfClientID.Value = value; }
        }

        public string UniqueIdentity
        {
            get { return txtUniqueIdentity.Text; }
            set { txtUniqueIdentity.Text = value; }
        }

        public string UniqueDomain
        {
            get { return txtUniqueDomain.Text; }
            set { txtUniqueDomain.Text = value; }
        }

        public string ABN
        {
            get { return txtABN.Text; }
            set { txtABN.Text = value; }
        }
        public string ClientName
        {
            get { return txtClientName.Text; }
            set { txtClientName.Text = value; }
        }
        public string ContactPersonName
        {
            get { return txtContactPerson.Text; }
            set { txtContactPerson.Text = value; }
        }

        public Guid ContactPerson
        {
            get { return Guid.Parse(ddlContactPerson.SelectedValue); }
            set { ddlContactPerson.SelectedValue = value.ToString(); }
        }
        public string Description
        {
            get { return txtBusinessDescription.Text; }
            set { txtBusinessDescription.Text = value; }
        }
        public string ContactOffice
        {
            get { return txtContactOffice.Text; }
            set { txtContactOffice.Text = value; }
        }
        public string ContactFax
        {
            get { return txtContactFax.Text; }
            set { txtContactFax.Text = value; }
        }
        public string BusinessEmail
        {
            get { return txtBusinessEmail.Text; }
            set { txtBusinessEmail.Text = value; }
        }
        public string BusinessUrl
        {
            get { return txtBusinessUrl.Text; }
            set { txtBusinessUrl.Text = value; }
        }
        public FileUpload LogoImageStream
        {
            get { return LogoLoader.ImageControl; }
            //set { fuLogoUrl.PostedFile.FileName = value; }
        }
        public string AddressLine1
        {
            get { return txtAddressLine1.Text; }
            set { txtAddressLine1.Text = value; }
        }
        public string AddressLine2
        {
            get { return txtAddressLine2.Text; }
            set { txtAddressLine2.Text = value; }
        }
        public string AddressLine3
        {
            get { return txtAddressLine3.Text; }
            set { txtAddressLine3.Text = value; }
        }
        public string City
        {
            get { return txtCity.Text; }
            set { txtCity.Text = value; }
        }
        public string State
        {
            get { return ddlState.SelectedValue; }
            set { ddlState.SelectedValue = value; }
        }
        public string PostCode
        {
            get { return txtPostcode.Text; }
            set { txtPostcode.Text = value; }
        }
        public int CountryID
        {
            get { return int.Parse(ddlCountry.SelectedValue); }
            set { ddlCountry.SelectedValue = value.ToString(); }
        }

        public string EstablishedDate
        {
            get { return txtEstablishedDate.Text; }
            set { txtEstablishedDate.Text = value; }
        }

        public bool Published
        {
            get { return chkPublished.Checked; }
            set { chkPublished.Checked = value; }
        }

        public string Theme
        {
            get { return ddlTheme.SelectedValue; }
            set { ddlTheme.SelectedValue = value; }
        }

        public void SetEnvironmentForUpdate()
        {
            txtContactPerson.Visible = false;
            ddlContactPerson.Visible = true;
            pnlPublish.Visible = true;
        }

        public void SetEnvironmentForUpdateForAdmin()
        {
            if (WebContext.Parent != null)
            {
                ClientFeatureDetails clientFeature = ClientManager.SelectClientFeatureDetails(WebContext.Parent.ClientID).EntityList[0];
                if (clientFeature.ClientDomain)
                    pnlUniqueDomain.Visible = true;
                if (clientFeature.ClientProfile)
                    pnlUniqueUrl.Visible = true;
                pnlEstablishedDate.Visible = true;
                pnlBusinessLogo.Visible = true;
                pnlFax.Visible = true;
                pnlTheme.Visible = true;
            }
        }
    }
}