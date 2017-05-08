using System;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using SleekSurf.Manager;

namespace SleekSurf.Domain.WebPageControls
{
    public partial class NewEditCustomer : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ExtractTitle();
                ExtractOccupation();
                ExtractCountries();
            }
            txtDOB.Attributes.Add("onkeyup", "return keyUpActionForDate('" + txtDOB.ClientID + "')");
            txtDOB.Attributes.Add("onkeydown", "return keyDownActionForDate('" + txtDOB.ClientID + "')");
        }
        #region Private Methods

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
        }

        public void ExtractCustomerGroups(string customerGroupID)
        {
            panelUpdateStuffs.Visible = true;

            Result<CustomerGroupDetails> result = CustomerManager.SelectAllCustomerGroup(WebContext.ClientProfile.ClientID);
            if (result.Status == ResultStatus.Success)
            {
                ddlCustomerGroup.DataSource = result.EntityList;
                ddlCustomerGroup.DataTextField = "GroupName";
                ddlCustomerGroup.DataValueField = "CustomerGroupID";
                ddlCustomerGroup.DataBind();
                ddlCustomerGroup.Items.Insert(0, new ListItem("Default", "0"));
            }

            ddlCustomerGroup.SelectedValue = customerGroupID;
        }

        #endregion

        #region public properties and Methods

        public FileUpload AvatarUrlImage
        {
            get { return ucFileUpload.ImageControl; }
        }

        public Image ImgThumb
        {
            get { return imgThumb; }
            set { imgThumb = value; }
        }

        public string Title
        {
            get { return ddlTitle.SelectedValue; }
            set { ddlTitle.SelectedValue = value; }
        }

        public string FirstName
        {
            get
            {
                return txtFirstName.Text;
            }
            set
            {
                txtFirstName.Text = value;
            }
        }

        public string MiddleName
        {
            get
            {
                return txtMiddleName.Text;
            }
            set
            {
                txtMiddleName.Text = value;
            }
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
                if (txtDOB.Text.Length > 0)
                    return Convert.ToDateTime(txtDOB.Text);
                else
                    return null;
            }

            set
            {
                txtDOB.Text = value.ToString();
            }
        }

        public string Gender
        {
            get
            {
                return rblGender.SelectedValue;
            }
            set
            {
                rblGender.SelectedValue = value;
            }
        }

        public string Occupation
        {
            get { return ddlOccupation.SelectedValue; }
            set { ddlOccupation.SelectedValue = value; }
        }

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

        public string Email
        {
            get { return txtEmail.Text; }
            set { txtEmail.Text = value; }
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
            get { return txtState.Text; }
            set { txtState.Text = value; }
        }

        public string PostCode
        {
            get { return txtPostCode.Text; }
            set { txtPostCode.Text = value; }
        }

        public int CountryID
        {
            get { return int.Parse(ddlCountry.SelectedValue); }
            set { ddlCountry.SelectedValue = value.ToString(); }
        }

        public string CustomerGroupID
        {
            get { return ddlCustomerGroup.SelectedValue; }
            set { ddlCustomerGroup.SelectedValue = value; }
        }

        public bool SubscriptionEmail
        {
            get { return chkSubscriptionEmail.Checked; }
            set { chkSubscriptionEmail.Checked = value; }
        }

        public bool SubscriptionSMS
        {
            get { return chkSubscriptionSMS.Checked; }
            set { chkSubscriptionSMS.Checked = value; }
        }

        public void ClearFields()
        {
            ddlTitle.SelectedIndex = 0;
            txtFirstName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtDOB.Text = string.Empty;
            rblGender.SelectedValue = null;
            txtAddressLine1.Text = string.Empty;
            txtAddressLine2.Text = string.Empty;
            txtAddressLine3.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtState.Text = string.Empty;
            txtPostCode.Text = string.Empty;
            ddlCountry.SelectedIndex = 0;
            txtContactHome.Text = string.Empty;
            txtContactMobile.Text = string.Empty;
            txtEmail.Text = string.Empty;
            ddlOccupation.SelectedIndex = 0;
            ddlCustomerGroup.Items.Clear();
            chkSubscriptionEmail.Checked = false;
            chkSubscriptionSMS.Checked = false;
            panelUpdateStuffs.Visible = false;
            ImgThumb.ImageUrl = "";
            ImgThumb.Visible = false;
        }

        #endregion
    }
}