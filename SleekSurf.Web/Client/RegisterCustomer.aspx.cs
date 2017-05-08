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
using System.Web.Security;

namespace SleekSurf.Web.Client
{
    public partial class RegisterCustomer : WebBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)(Master.Master.FindControl("NavigationMenu"));
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("Register"))].Selected = true;

            ucNewEditCustomer.ImgThumb.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (WebContext.ClientProfile != null)
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
                customer.ContactMobile = Convert.ToInt64(ucNewEditCustomer.ContactMobile).ToString();
                if (!customer.ContactMobile.StartsWith(countryDialCode.ToString()))
                {
                    customer.ContactMobile = "+" + countryDialCode + customer.ContactMobile;
                }
                else
                    customer.ContactMobile = "+" + customer.ContactMobile;

                customer.Email = ucNewEditCustomer.Email;
                customer.AddressLine1 = ucNewEditCustomer.AddressLine1;
                customer.AddressLine2 = ucNewEditCustomer.AddressLine2;
                customer.AddressLine3 = ucNewEditCustomer.AddressLine3;
                customer.City = ucNewEditCustomer.City;
                customer.State = ucNewEditCustomer.State;
                customer.PostCode = ucNewEditCustomer.PostCode;
                customer.CountryID = new CountryDetails() { CountryID = ucNewEditCustomer.CountryID };
                customer.ClientID = new ClientDetails() { ClientID = WebContext.ClientProfile.ClientID };
                customer.CustomerID = System.DateTime.Now.ToString("CT-ddMMyyy-HHmmssfff");

                Result<CustomerDetails> result = new Result<CustomerDetails>();
                result = CustomerManager.InsertCustomer(customer);
                UploadProfileFile(customer.CustomerID);
                if (result.Status == ResultStatus.Success)
                {
                    lblMessage.CssClass = "successMsg";
                    ucNewEditCustomer.ClearFields();
                    //SEND EMAIL TO THE REGISTERED CUSTOMER
                    string subject = WebContext.ClientProfile.ClientName + " Confirmation of Registration";
                    string logoUrl = "";
                    string logoDisplay = "none";
                    if (!string.IsNullOrEmpty(WebContext.ClientProfile.LogoUrl))
                    {
                        string urlPath = Server.MapPath("~/Uploads/" + WebContext.ClientProfile.ClientID + "/LogoPicture/" + WebContext.ClientProfile.LogoUrl);
                        if (System.IO.File.Exists(urlPath))
                        {
                            logoUrl = BasePage.FullBaseUrl + "Uploads/" + WebContext.ClientProfile.ClientID + "/LogoPicture/" + WebContext.ClientProfile.LogoUrl;
                            logoDisplay = "block";
                        }
                    }
                    string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/Default/Images/MessageBoxTopBackground.png";
                    string appPath = Request.PhysicalApplicationPath;
                    StreamReader customerBodySR = new StreamReader(appPath + "EmailTemplates/CustomerRegistrationConfirmationToCustomer.txt");
                    string customerBody = customerBodySR.ReadToEnd();
                    customerBodySR.Close();
                    customerBody = customerBody.Replace("<%Logo%>", logoUrl);
                    customerBody = customerBody.Replace("<%LogoDisplay%>", logoDisplay);
                    customerBody = customerBody.Replace("<%TopBackGround%>", topBackGroundUrl);
                    customerBody = customerBody.Replace("<%CustomerFullName%>", customer.FirstName + " " + customer.MiddleName + " " + customer.LastName);
                    customerBody = customerBody.Replace("<%ClientName%>", WebContext.ClientProfile.ClientName);
                    customerBody = customerBody.Replace("<%ReplyEmail%>", WebContext.ClientProfile.BusinessEmail);
                    if (string.IsNullOrEmpty(WebContext.ClientProfile.UniqueDomain))
                        customerBody = customerBody.Replace("<%WebSite%>", BasePage.FullBaseUrl + WebContext.ClientProfile.UniqueIdentity + " OR <br /> http://www." + WebContext.ClientProfile.UniqueIdentity + ".sleeksurf.com");
                    else
                        customerBody = customerBody.Replace("<%WebSite%>", WebContext.ClientProfile.UniqueDomain);
                    CustomUserProfile contactPerson = CustomUserProfile.GetUserProfile(Membership.GetUser(WebContext.ClientProfile.ContactPerson).UserName);
                    customerBody = customerBody.Replace("<%ContactPerson%>", contactPerson.FirstName + " " + contactPerson.MiddleName + " " + contactPerson.LastName);

                    Helpers.SendEmail(WebContext.ClientProfile.BusinessEmail, WebContext.ClientProfile.ClientName, customer.Email, subject, customerBody);
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
                    clientBody = clientBody.Replace("<%ClientName%>", WebContext.ClientProfile.ClientName);

                    UserInfoPartial clientAdminBusinessEmail = new UserInfoPartial() { FirstName = WebContext.ClientProfile.ClientName, MiddleName = "", LastName = "", Email = WebContext.ClientProfile.BusinessEmail };
                    UserInfoPartial contactPersonAdminEmail = new UserInfoPartial() { FirstName = contactPerson.FirstName, MiddleName = contactPerson.MiddleName, LastName = contactPerson.LastName, Email = Membership.GetUser(WebContext.ClientProfile.ContactPerson).Email };

                    Helpers.SendEmail(clientAdminBusinessEmail, WebContext.ClientProfile.ClientName, clientSubject, clientBody);
                    Helpers.SendEmail(contactPersonAdminEmail, WebContext.ClientProfile.ClientName, clientSubject, clientBody);
                }
                else
                    lblMessage.CssClass = "errorMsg";

                lblMessage.Text = result.Message;
            }
            else
            {
                lblMessage.Text = "Unable to register your profile in our system. Please contact " + WebContext.ClientProfile.ClientName + " for your registration. Sorry for your inconvinence.";
                lblMessage.CssClass = "errorMsg";
            }
        }

        private void UploadProfileFile(string customerID)
        {
            string dirUrlP = BasePage.BaseUrl + "Uploads/" + WebContext.ClientProfile.ClientID + "/CustomersPicture";
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