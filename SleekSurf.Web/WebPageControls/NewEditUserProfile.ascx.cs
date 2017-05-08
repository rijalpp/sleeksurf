using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using SleekSurf.Manager;
using SleekSurf.Entity;

namespace SleekSurf.Web.WebPageControls
{
    public partial class NewEditUserProfile : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ExtractTitle();
                ExtractOccupation();
                ExtractCountries();
                ExtractAustralianStatesAndTerritories();
                ExtractTheme();
            }
        }

        private void ExtractTitle()
        {
            ddlTitle.DataSource = Enum.GetValues(typeof(Helpers.Title));
            ddlTitle.DataBind();
            ddlTitle.Items.Insert(0, "Select Title");
        }

        private void ExtractOccupation()
        {
            ddlOccupation.DataSource = Enum.GetValues(typeof(Helpers.Occupation));
            ddlOccupation.DataBind();
            ddlOccupation.Items.Insert(0, "Select Occupation");
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

        private void ExtractAustralianStatesAndTerritories()
        {
            ddlState.DataSource = Enum.GetNames(typeof(AustralianStatesAndTerritories));
            ddlState.DataBind();
            ddlState.Items.Insert(0, "Select State");
        }

        private void ExtractTheme()
        {
            ddlTheme.DataSource = Helpers.GetThemes();
            ddlTheme.DataBind();
            ddlTheme.SelectedIndex = ddlTheme.Items.IndexOf(new ListItem("Default"));
        }

        public FileUpload ProfileImageStream
        {
            get { return FileUploaderProfile.ImageControl; }
        }

        public string Title
        {
            get { return ddlTitle.SelectedValue; }
            set { ddlTitle.SelectedValue = value; }
        }
        public string FirstName
        {
            get { return txtFirstName.Text; }
            set { txtFirstName.Text = value; }
        }
        public string MiddleName
        {
            get { return txtMiddleName.Text; }
            set { txtMiddleName.Text = value; }
        }
        public string LastName
        {
            get { return txtLastName.Text; }
            set { txtLastName.Text = value; }
        }
        public DateTime? DOB
        {
            get
            {
                if (!string.IsNullOrEmpty(txtDOB.Text))
                    return Convert.ToDateTime(txtDOB.Text);
                else
                    return null;
            }
            set { txtDOB.Text = value.ToString(); }
        }
        public string Gender
        {
            get { return rblGender.SelectedValue; }
            set { rblGender.SelectedValue = value; }
        }
        public string Occupation
        {
            get { return ddlOccupation.SelectedValue; }
            set { ddlOccupation.SelectedValue = value; }
        }
        public string WebSiteUrl
        {
            get { return txtWebsiteUrl.Text; }
            set { txtWebsiteUrl.Text = value; }
        }
        //address section
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
        public string Country
        {
            get { return ddlCountry.SelectedValue; }
            set { ddlCountry.SelectedValue = value; }
        }
        //contact no
        public string ContactHome
        {
            get { return txtContactHome.Text; }
            set { txtContactHome.Text = value; }
        }
        public string ContactMobile
        {
            get { return txtContactMobile.Text; }
            set { txtContactMobile.Text = value; }
        }
        //preference
        public string Theme
        {
            get { return ddlTheme.SelectedValue; }
            set { ddlTheme.SelectedValue = value; }
        }
        //just added on 13 May 2011 to checke if the control has file while updating.
        public FileUpload ProfileImageControl
        {
            get { return FileUploaderProfile.ImageControl; }
        }

        public void SetEnvironmentForUpdateForAdmin()
        {
            pnlDateOfBirth.Visible = true;
            pnlProfileUrl.Visible = true;
            spanProfileContact.Visible = true;
            divSeparator.Visible = true;
        }
    }
}