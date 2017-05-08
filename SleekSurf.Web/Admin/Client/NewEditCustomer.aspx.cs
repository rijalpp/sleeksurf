using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using System.Transactions;
using SleekSurf.Entity;
using SleekSurf.Manager;
using System.IO;
using System.Web.Security;

namespace SleekSurf.Web.Admin.Client
{
    public partial class NewEditCustomer : BasePage
    {
        string customerID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Customer"] != null)
            {
                customerID = Session["Customer"].ToString();
            }

            if (!IsPostBack)
            {

                if (Session["Customer"] != null)
                {
                    lblTitle.Text += " - Update Mode";
                    BindCustomerDetails();
                }
                else
                {
                    lblTitle.Text += " - Add Mode";
                    //imgThumb.Visible = false;
                    ucNewEditCustomer.ImgThumb.Visible = false;
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
            Session.Remove("Customer");
            ucNewEditCustomer.ClearFields();
            Response.Redirect("~/Admin/Client/CustomerManagement.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        SaveCustomer();
                        scope.Complete();
                    }
                    catch(Exception ex)
                    {
                        Helpers.LogError(ex);
                    }
                }
            }
        }

        private void BindCustomerDetails()
        {
            CustomerDetails customer = CustomerManager.SelectCustomer(customerID, WebContext.Parent.ClientID).EntityList[0];
            //profile option
            //SHOW FILE UPLOAD
            ucNewEditCustomer.SetProfileImageVisibility(true);
            string avatarUrl = string.Format("~/Uploads/{0}/CustomersPicture/{1}.jpg", WebContext.Parent.ClientID, customerID);
            if (!string.IsNullOrWhiteSpace(avatarUrl))
            {

                if (File.Exists(Server.MapPath(avatarUrl)))
                    ucNewEditCustomer.ImgThumb.ImageUrl = avatarUrl + "?" + (new DateTime()).Millisecond;
                else
                {
                    if (string.Compare(customer.Gender, "female", true)==0)
                        ucNewEditCustomer.ImgThumb.ImageUrl = "~/App_Themes/SleekTheme/Images/ProfileFemale.png";
                    else
                        ucNewEditCustomer.ImgThumb.ImageUrl = "~/App_Themes/SleekTheme/Images/ProfileMale.png";
                }

            }
            else
            {
                if (string.Compare(customer.Gender, "female", true) == 0)
                    ucNewEditCustomer.ImgThumb.ImageUrl = "~/App_Themes/SleekTheme/Images/ProfileFemale.png";
                else
                    ucNewEditCustomer.ImgThumb.ImageUrl = "~/App_Themes/SleekTheme/Images/ProfileMale.png";
            }

            ucNewEditCustomer.Title = customer.Title;
            ucNewEditCustomer.FirstName = customer.FirstName;
            ucNewEditCustomer.MiddleName = customer.MiddleName;
            ucNewEditCustomer.LastName = customer.LastName;
            ucNewEditCustomer.DOB = customer.DOB;
            ucNewEditCustomer.Gender = customer.Gender;
            ucNewEditCustomer.Occupation = customer.Occupation;
            ucNewEditCustomer.ContactHome = customer.ContactHome;
            ucNewEditCustomer.ContactMobile = customer.ContactMobile;
            ucNewEditCustomer.Email = customer.Email;
            ucNewEditCustomer.AddressLine1 = customer.AddressLine1;
            ucNewEditCustomer.AddressLine2 = customer.AddressLine2;
            ucNewEditCustomer.AddressLine3 = customer.AddressLine3;
            ucNewEditCustomer.City = customer.City;
            ucNewEditCustomer.State = customer.State;
            ucNewEditCustomer.PostCode = customer.PostCode;
            ucNewEditCustomer.CountryID = customer.CountryID.CountryID;
            ucNewEditCustomer.SubscriptionEmail = customer.SubscriptionEmail;
            ucNewEditCustomer.SubscriptionSMS = customer.SubscriptionSMS;
            ucNewEditCustomer.ExtractCustomerGroups(customer.CustomerGroupID);
        }


        private void SaveCustomer()
        {
            CustomerDetails customer = new CustomerDetails();
            customer.Title = ucNewEditCustomer.Title;
            customer.FirstName = ucNewEditCustomer.FirstName;
            customer.MiddleName = ucNewEditCustomer.MiddleName;
            customer.LastName = ucNewEditCustomer.LastName;
            // if (txtDOB.Text.Length > 0) not required as it's already checked in user control
            customer.DOB = ucNewEditCustomer.DOB;
            customer.Gender = ucNewEditCustomer.Gender;
            customer.Occupation = ucNewEditCustomer.Occupation;
            if (!string.IsNullOrEmpty(ucNewEditCustomer.ContactHome))
                customer.ContactHome = Convert.ToInt64(ucNewEditCustomer.ContactHome).ToString();
            customer.ContactMobile = Convert.ToInt64(ucNewEditCustomer.ContactMobile).ToString();
            customer.Email = ucNewEditCustomer.Email;
            customer.AddressLine1 = ucNewEditCustomer.AddressLine1;
            customer.AddressLine2 = ucNewEditCustomer.AddressLine2;
            customer.AddressLine3 = ucNewEditCustomer.AddressLine3;
            customer.City = ucNewEditCustomer.City;
            customer.State = ucNewEditCustomer.State;
            customer.PostCode = ucNewEditCustomer.PostCode;
            customer.CountryID = new CountryDetails() { CountryID = ucNewEditCustomer.CountryID };
            int countryDialCode = CountryManager.GetCountry(ucNewEditCustomer.CountryID).EntityList[0].DialCode;
            if (!string.IsNullOrEmpty(ucNewEditCustomer.ContactHome))
            {
                customer.ContactHome = Convert.ToInt64(ucNewEditCustomer.ContactHome).ToString();
                if (!customer.ContactHome.StartsWith(countryDialCode.ToString()))
                {
                    customer.ContactHome = "+" + countryDialCode + customer.ContactHome;
                }
                else
                    customer.ContactHome = "+" + customer.ContactHome;
            }

            if (!customer.ContactMobile.StartsWith(countryDialCode.ToString()))
            {
                customer.ContactMobile = "+" + countryDialCode + customer.ContactMobile;
            }
            else
                customer.ContactMobile = "+" + customer.ContactMobile;

            customer.ClientID = new ClientDetails() { ClientID = WebContext.Parent.ClientID };
            Guid currentUserID = (Guid)Membership.GetUser(HttpContext.Current.User.Identity.Name).ProviderUserKey;
            customer.CreatedBy = currentUserID;
            customer.UpdatedBy = currentUserID;
            Result<CustomerDetails> result = new Result<CustomerDetails>();
            if (!string.IsNullOrWhiteSpace(customerID))
            {
                customer.CustomerID = customerID;
                customer.SubscriptionEmail = ucNewEditCustomer.SubscriptionEmail;
                customer.SubscriptionSMS = ucNewEditCustomer.SubscriptionSMS;
                customer.CustomerGroupID = ucNewEditCustomer.CustomerGroupID;
                result = CustomerManager.UpdateCustomer(customer);
            }
            else
            {
                customer.CustomerID = System.DateTime.Now.ToString("CT-ddMMyyy-HHmmssfff");
                result = CustomerManager.InsertCustomer(customer);

                if (result.Status == ResultStatus.Success)
                {
                    //SEND EMAIL TO THE REGISTERED CUSTOMER
                    string subject = WebContext.Parent.ClientName + " Confirmation of Registration";
                    //string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
                    string logoUrl = "";
                    string logoDisplay = "none";
                    if (!string.IsNullOrEmpty(WebContext.Parent.LogoUrl))
                    {
                        string urlPath = Server.MapPath("~/Uploads/" + WebContext.Parent.ClientID + "/LogoPicture/" + WebContext.Parent.LogoUrl);
                        if (System.IO.File.Exists(urlPath))
                        {
                            logoUrl = FullBaseUrl + "Uploads/" + WebContext.Parent.ClientID + "/LogoPicture/" + WebContext.Parent.LogoUrl;
                            logoDisplay = "block";
                        }
                    }
                    string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
                    string appPath = Request.PhysicalApplicationPath;
                    StreamReader customerBodySR = new StreamReader(appPath + "EmailTemplates/CustomerRegistrationConfirmationToCustomer.txt");
                    string customerBody = customerBodySR.ReadToEnd();
                    customerBodySR.Close();
                    customerBody = customerBody.Replace("<%Logo%>", logoUrl);
                    customerBody = customerBody.Replace("<%LogoDisplay%>", logoDisplay);
                    customerBody = customerBody.Replace("<%TopBackGround%>", topBackGroundUrl);
                    customerBody = customerBody.Replace("<%CustomerFullName%>", customer.FirstName + " " + customer.MiddleName + " " + customer.LastName);
                    customerBody = customerBody.Replace("<%ClientName%>", WebContext.Parent.ClientName);
                    customerBody = customerBody.Replace("<%ReplyEmail%>", WebContext.Parent.BusinessEmail);
                    customerBody = customerBody.Replace("<%WebSite%>", FullBaseUrl + WebContext.Parent.UniqueIdentity + " OR <br /> http://www." + WebContext.Parent.UniqueIdentity + ".sleeksurf.com");
                    CustomUserProfile contactPerson = CustomUserProfile.GetUserProfile(Membership.GetUser(WebContext.Parent.ContactPerson).UserName);
                    customerBody = customerBody.Replace("<%ContactPerson%>", contactPerson.FirstName + " " + contactPerson.MiddleName + " " + contactPerson.LastName);

                    Helpers.SendEmail(WebContext.Parent.BusinessEmail, WebContext.Parent.ClientName, customer.Email, subject, customerBody);
                    //SEND EMAIL TO THE CONTACT PERSON OF THE CLIENT
                    string clientSubject = "Confirmation of Customer Registration";
                    StreamReader clientBodySR = new StreamReader(appPath + "EmailTemplates/CustomerRegistrationConfirmationToClient.txt");
                    string clientBody = clientBodySR.ReadToEnd();
                    clientBodySR.Close();
                    clientBody = clientBody.Replace("<%Logo%>", logoUrl);
                    clientBody = clientBody.Replace("<%LogoDisplay%>", logoDisplay);
                    clientBody = clientBody.Replace("<%TopBackGround%>", topBackGroundUrl);
                    clientBody = clientBody.Replace("<%CustomerName%>", customer.FirstName + " " + customer.MiddleName + " " + customer.LastName);
                    clientBody = clientBody.Replace("<%Address%>", customer.Address);
                    clientBody = clientBody.Replace("<%Email%>", customer.Email);
                    clientBody = clientBody.Replace("<%Mobile%>", customer.ContactMobile);
                    clientBody = clientBody.Replace("<%HomeNo%>", customer.ContactHome);
                    clientBody = clientBody.Replace("<%ClientName%>", WebContext.Parent.ClientName);

                    UserInfoPartial clientAdminBusinessEmail = new UserInfoPartial() { FirstName = WebContext.Parent.ClientName, MiddleName = "", LastName = "", Email = WebContext.Parent.BusinessEmail };
                    UserInfoPartial contactPersonAdminEmail = new UserInfoPartial() { FirstName = contactPerson.FirstName, MiddleName = contactPerson.MiddleName, LastName = contactPerson.LastName, Email = Membership.GetUser(WebContext.Parent.ContactPerson).Email };

                    Helpers.SendEmail(clientAdminBusinessEmail, WebContext.Parent.ClientName, clientSubject, clientBody);
                    Helpers.SendEmail(contactPersonAdminEmail, WebContext.Parent.ClientName, clientSubject, clientBody);
                }
            }

            UploadProfileFile(customer.CustomerID);
            if (result.Status == ResultStatus.Success)
            {
                lblMessage.CssClass = "successMsg";
                Session.Remove("Customer");
                lblTitle.Text += " - Add Mode";
                ucNewEditCustomer.ClearFields();
            }
            else
                lblMessage.CssClass = "errorMsg";

            lblMessage.Text = result.Message;
        }

        private void UploadProfileFile(string customerID)
        {
            string dirUrlP = BasePage.BaseUrl + "Uploads/" + WebContext.Parent.ClientID + "/CustomersPicture";
            if (ucNewEditCustomer.AvatarUrlImage.HasFile)
            {
                string dirPathP = Server.MapPath(dirUrlP);
                if (!Directory.Exists(dirPathP))
                    Directory.CreateDirectory(dirPathP);
                string outputfileName = dirPathP + "/" + customerID + ".jpg";
                int profileImageSize = 150;
                ucNewEditCustomer.AvatarUrlImage.SaveAs(outputfileName);
                Helpers.ResizeImage(outputfileName, outputfileName, profileImageSize);
            }
        }
    }
}