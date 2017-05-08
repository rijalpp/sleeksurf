using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using System.Transactions;
using System.Web.Security;
using SleekSurf.Entity;
using SleekSurf.Manager;
using System.IO;
using System.Drawing;

namespace SleekSurf.Web.WebPageControls
{
     public partial class NewEditAccount : System.Web.UI.UserControl
    {
        static bool viewFlowUp = true;
        public RadioButtonList RegoType
        {
            get { return rblRegoType; }
        }

        public DropDownList RoleList
        {
            get { return (DropDownList)NewUser.FindControl("ddlRoles"); }
        }
        
        protected override void OnPreRender(EventArgs e)
        {
            if (mvClientDetails.GetActiveView().ID == "vClientDetails")
            {
                ClientDetails1.ContactPersonName = UserProfile1.FirstName + " " + UserProfile1.MiddleName + " " + UserProfile1.LastName;
            }

            btnPrevious.Visible = (mvClientDetails.ActiveViewIndex > 0) && (mvClientDetails.ActiveViewIndex != mvClientDetails.Views.Count - 1);
            btnNext.Visible = mvClientDetails.ActiveViewIndex < mvClientDetails.Views.Count - 2;
            btnSave.Visible = mvClientDetails.ActiveViewIndex == mvClientDetails.Views.Count - 2;

            if (mvClientDetails.GetActiveView().ID == "vLoginDetails" && WebContext.CurrentUser != null)
                btnPrevious.Visible = false;

            if (mvClientDetails.GetActiveView().ID == "vComplete")
                pnlNavigation.Visible = false;
            else
                pnlNavigation.Visible = true;


            /******************************Start of Step Navigation Status ********************************************/

            imgStepType.ImageUrl = "~/App_Themes/SleekTheme/Images/Type.png";
            imgStepAccount.ImageUrl = "~/App_Themes/SleekTheme/Images/Account.png";
            imgStepProfile.ImageUrl = "~/App_Themes/SleekTheme/Images/UserProfile.png";
            imgStepBusiness.ImageUrl = "~/App_Themes/SleekTheme/Images/BusinessProfile.png";
            imgStepSummary.ImageUrl = "~/App_Themes/SleekTheme/Images/Summary.png";

            switch (mvClientDetails.GetActiveView().ID)
            {
                case "vRegoTypeSelection":
                    imgStepType.ImageUrl = "~/App_Themes/SleekTheme/Images/TypeActive.png";
                    break;
                case "vLoginDetails":
                    imgStepAccount.ImageUrl = "~/App_Themes/SleekTheme/Images/AccountActive.png";
                    break;
                case "vProfileDetails":
                    imgStepProfile.ImageUrl = "~/App_Themes/SleekTheme/Images/UserProfileActive.png";
                    break;
                case "vClientDetails":
                    imgStepBusiness.ImageUrl = "~/App_Themes/SleekTheme/Images/BusinessProfileActive.png";
                    break;
                case "vSummary":
                    imgStepSummary.ImageUrl = "~/App_Themes/SleekTheme/Images/SummaryActive.png";
                    break;
            }


            /******************************End of Step Navigation Status **********************************************/

            base.OnPreRender(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.Compare(rblRegoType.SelectedValue, "personal", true) == 0)
            {
                mvClientDetails.Views[mvClientDetails.Views.IndexOf(mvClientDetails.FindControl("vClientDetails"))].ID = "HiddenView";
                imgStepBusiness.Visible = false;
            }
            else
            {
                Step4.Attributes.Remove("style");
                imgStepBusiness.Visible = true;
            }
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            viewFlowUp = false;
            mvClientDetails.ActiveViewIndex--;
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            switch (mvClientDetails.GetActiveView().ID)
            {
                case "vLoginDetails":
                    WebContext.TempInfo.Password = NewUser.Password;
                    break;
                case "vProfileDetails":
                    if (UserProfile1.ProfileImageStream.HasFile)
                        WebContext.TempInfo.ProfileStream = new MemoryStream(Helpers.ResizeImage(UserProfile1.ProfileImageStream.FileContent, 100));
                    break;
                case "vClientDetails":
                    if (ClientDetails1.LogoImageStream.HasFile)
                        WebContext.TempInfo.LogoStream = new MemoryStream(Helpers.ResizeImage(ClientDetails1.LogoImageStream.FileContent, 150));
                    break;
            }

            viewFlowUp = true;
            mvClientDetails.ActiveViewIndex++;
        }

        protected void mvClientDetails_ActiveViewChanged(object sender, EventArgs e)
        {
            if (mvClientDetails.GetActiveView().ID == "vRegoTypeSelection")
            {
                if (mvClientDetails.Views.Contains(mvClientDetails.FindControl("HiddenView")))
                {
                    mvClientDetails.Views[mvClientDetails.Views.IndexOf(mvClientDetails.FindControl("HiddenView"))].ID = "vClientDetails";
                }
            }

            if (mvClientDetails.GetActiveView().ID == "HiddenView")
            {
                if (viewFlowUp)
                    mvClientDetails.ActiveViewIndex++;
                else
                    mvClientDetails.ActiveViewIndex--;
            }
            if (mvClientDetails.GetActiveView().ID == "vSummary")
                ShowSummary();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Recaptcha.RecaptchaControl myCaptcha = (Recaptcha.RecaptchaControl)vSummary.FindControl("recaptcha");
            myCaptcha.Validate();
            bool answer = myCaptcha.IsValid;
            if (answer)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ((TempInfo)Session["tempInfos"]).SuccessStatus = false;

                    if (CreateAccount() == MembershipCreateStatus.Success)
                    {
                        if (SaveProfile())
                        {
                            ((TempInfo)Session["tempInfos"]).SuccessStatus = true;
                            if (string.Compare(rblRegoType.SelectedValue, "business", true) == 0)
                            {
                                Result<ClientDetails> result = SaveClient();
                                if (result.Status == ResultStatus.Success)
                                {
                                    scope.Complete();
                                    ((TempInfo)Session["tempInfos"]).SuccessStatus = true;
                                }
                                else
                                {
                                    lblResultMessage.Text = "Error Occured in Business Profile Section";
                                    lblResultMessage.CssClass = "errorMsg";
                                    ((TempInfo)Session["tempInfos"]).SuccessStatus = false;
                                }
                            }
                            else
                            {
                                if (WebContext.Parent != null)
                                {
                                    Result<ClientDetails> result = ClientManager.UpdateProfileForClientID((Guid)Membership.GetUser(NewUser.UserName).ProviderUserKey, WebContext.Parent.ClientID);
                                }
                                scope.Complete();
                            }

                            if (((TempInfo)Session["tempInfos"]).SuccessStatus)
                            {
                                UploadUserImages();
                                using (TransactionScope emailScope = new TransactionScope())
                                {
                                    //email sent to all superadmins
                                    string[] userNameList = Roles.GetUsersInRole("SuperAdmin");
                                    List<UserInfoPartial> users = new List<UserInfoPartial>();
                                    foreach(string userName in userNameList)
                                    {
                                        CustomUserProfile profile = CustomUserProfile.GetUserProfile(userName);
                                        users.Add(new UserInfoPartial() { FirstName = profile.FirstName, MiddleName = profile.MiddleName, LastName = profile.LastName, Email = Membership.GetUser(userName).Email });
                                    }
                                     //embed image in the emails
                                    string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
                                    string logoDisplay = "block";
                                    string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
                                    string appPath = Request.PhysicalApplicationPath;

                                    string mailSender = "";
                                    if (WebContext.CurrentUser.IsInRole("Admin") || WebContext.CurrentUser.IsInRole("AdminUser"))
                                        mailSender = WebContext.Parent.ClientName;
                                    else
                                        mailSender = "SleekSurf";

                                    if (WebContext.Parent == null)
                                    {
                                        //superadmin email
                                        string superAdminSubject = "New user created.";
                                        StreamReader superAdminSR = new StreamReader(appPath + "EmailTemplates/NotifyRegistrationToSuperAdmin.txt");
                                        string superAdminBody = superAdminSR.ReadToEnd();
                                        superAdminSR.Close();
                                        // superAdminBody = string.Format(superAdminBody, NewUser.UserName, UserProfile1.FirstName, UserProfile1.MiddleName, UserProfile1.LastName, UserProfile1.State, UserProfile1.Country);
                                        superAdminBody = superAdminBody.Replace("<%Logo%>", logoUrl);
                                        superAdminBody = superAdminBody.Replace("<%TopBackGround%>", topBackGroundUrl);
                                        superAdminBody = superAdminBody.Replace("<%UserName%>", NewUser.UserName);
                                        superAdminBody = superAdminBody.Replace("<%FirstName%>", UserProfile1.FirstName);
                                        superAdminBody = superAdminBody.Replace("<%MiddleName%>", UserProfile1.MiddleName);
                                        superAdminBody = superAdminBody.Replace("<%LastName%>", UserProfile1.LastName);
                                        superAdminBody = superAdminBody.Replace("<%State%>", UserProfile1.State);
                                        superAdminBody = superAdminBody.Replace("<%Country%>", CountryManager.GetCountry(int.Parse(UserProfile1.Country)).EntityList[0].CountryName);

                                        Helpers.SendEmail(users, mailSender, superAdminSubject, superAdminBody);
                                    }

                                    //email sent to the registered users below:
                                    UserInfoPartial registeredUser = new UserInfoPartial() { FirstName = UserProfile1.FirstName, MiddleName = UserProfile1.MiddleName, LastName = UserProfile1.LastName, Email = NewUser.Email };
                                    StreamReader userSR = new StreamReader(appPath + "EmailTemplates/VerifyUser.txt");
                                    string userBody = userSR.ReadToEnd();
                                    userSR.Close();

                                    string companyName = "";
                                    if (Roles.GetRolesForUser(NewUser.UserName).Contains("AdminUser"))
                                    {
                                        companyName = WebContext.Parent.ClientName;
                                    }
                                    else if (Roles.GetRolesForUser(NewUser.UserName).Contains("Admin"))
                                    {
                                        if (WebContext.Parent != null)
                                        {
                                            companyName = WebContext.Parent.ClientName;
                                            if (!string.IsNullOrEmpty(WebContext.Parent.LogoUrl))
                                            {
                                                string urlPath = Server.MapPath("~/Uploads/" + WebContext.Parent.ClientID + "/LogoPicture/" + WebContext.Parent.LogoUrl);
                                                if (System.IO.File.Exists(urlPath))
                                                {
                                                    logoUrl = BasePage.FullBaseUrl + "Uploads/" + WebContext.Parent.ClientID + "/LogoPicture/" + WebContext.Parent.LogoUrl;
                                                }
                                                else
                                                    logoDisplay = "none"; 
                                            }
                                        }

                                        else
                                            companyName = "SleekSurf Team";
                                    }

                                    else
                                        companyName = "SleekSurf Team";

                                    string userSubject = "Account Verification";
                                    string urlName = BasePage.FullBaseUrl + "WebPages/VerifyUser.aspx?ID=" + Server.UrlEncode(NewUser.UserName.Trim().Encrypt());
                                    userBody = userBody.Replace("<%Logo%>", logoUrl);
                                    userBody = userBody.Replace("<%logoDisplay%>", logoDisplay);
                                    userBody = userBody.Replace("<%TopBackGround%>", topBackGroundUrl);
                                    userBody = userBody.Replace("<%UserName%>", NewUser.UserName);
                                    userBody = userBody.Replace("<%VerificationUrl%>", urlName);
                                    userBody = userBody.Replace("<%CompanyName%>", companyName);
                                    Helpers.SendEmail(registeredUser, mailSender, userSubject, userBody);
                                    emailScope.Complete();
                                }

                                WebContext.RemoveFromSession("tempInfos");
                                pnStepNavigationBar.Visible = false;
                                mvClientDetails.ActiveViewIndex++;
                            }
                        }
                        else
                        {
                            lblResultMessage.Text = "Error Occured in User Profile Section";
                            lblResultMessage.CssClass = "errorMsg";
                        }
                    }
                    else
                    {
                        lblResultMessage.Text += "Error Occured While Creating Account.";
                        lblResultMessage.CssClass = "errorMsg";
                    }

                }
            }
            else
                lblCaptchaMessage.Text = "Please enter the text in the captcha.";
        }

        public void ShowSummary()
        {
            lblUsername.Text = NewUser.UserName.Trim();
            lblEmail.Text = NewUser.Email.Trim();
            lblSQuestion.Text = NewUser.SecretQuestion.Trim();
            lblSAnswer.Text = NewUser.SecretAnswer.Trim();


            lblName.Text = UserProfile1.Title.Trim() + " " + UserProfile1.FirstName.Trim() + " " + UserProfile1.MiddleName.Trim() + " " + UserProfile1.LastName.Trim();
            //if (!string.IsNullOrWhiteSpace(UserProfile1.DOB.ToString()))
            //    lblDOB.Text = ((DateTime)UserProfile1.DOB).ToShortDateString();
            //else
            //    lblDOB.Text = "Not Supplied.";

            lblGender.Text = UserProfile1.Gender.Trim();
            //lblOccupation.Text = UserProfile1.Occupation.Trim();

            //if (!string.IsNullOrWhiteSpace(UserProfile1.WebSiteUrl))
            //    lblWebsite.Text = UserProfile1.WebSiteUrl.Trim();
            //else
            //    lblWebsite.Text = "Not Supplied.";

            //if (!string.IsNullOrWhiteSpace(UserProfile1.ContactHome))
            //    lblLandline.Text = UserProfile1.ContactHome.Trim();
            //else
            //    lblLandline.Text = "Not Supplied.";

            //if (!string.IsNullOrWhiteSpace(UserProfile1.ContactMobile))
            //    lblMobile.Text = UserProfile1.ContactMobile.Trim();
            //else
            //    lblMobile.Text = "Not Supplied.";

            if (UserProfile1.AddressLine1 != "")
                lblAddress.Text = UserProfile1.AddressLine1.Trim() + "<br/>";

            if (UserProfile1.AddressLine2 != "")
                lblAddress.Text += UserProfile1.AddressLine2.Trim() + "<br/>";

            if (UserProfile1.AddressLine3 != "")
                lblAddress.Text += UserProfile1.AddressLine3.Trim() + "<br/>";

            lblAddress.Text += UserProfile1.City.Trim() + "<br/>" + UserProfile1.State.Trim() + " " + UserProfile1.PostCode.Trim() + "<br/>";
            lblAddress.Text += CountryManager.GetCountry(int.Parse(UserProfile1.Country)).EntityList[0].CountryName.Trim();

            if (string.Compare(rblRegoType.SelectedValue, "business", true) == 0)
            {
                pnClientSummarySection.Visible = true;

                if (!string.IsNullOrWhiteSpace(ClientDetails1.ABN))
                    lblABN.Text = ClientDetails1.ABN.Trim();
                else
                    lblABN.Text = "Not Supplied.";

                //lblUniqueUrlID.Text = ClientDetails1.UniqueIdentity.Trim();
                lblBusinessType.Text = ClientManager.GetCategory(ClientDetails1.CategoryID).EntityList[0].CategoryName.Trim();

                //if (!string.IsNullOrWhiteSpace(ClientDetails1.EstablishedDate.ToString()))
                //    lblEstablishedDate.Text = (Convert.ToDateTime(ClientDetails1.EstablishedDate)).ToShortDateString();
                //else
                //    lblEstablishedDate.Text = "Not Supplied.";

                lblBusinessName.Text = ClientDetails1.ClientName.Trim();
                lblContactPerson.Text = ClientDetails1.ContactPersonName.Trim();

                if (!string.IsNullOrWhiteSpace(ClientDetails1.BusinessUrl))
                    lblBusinessWebsite.Text = ClientDetails1.BusinessUrl.Trim();
                else
                    lblBusinessWebsite.Text = "Not Supplied.";

                if (!string.IsNullOrWhiteSpace(ClientDetails1.BusinessEmail))
                    lblBusinessEmail.Text = ClientDetails1.BusinessEmail.Trim();
                else
                    lblBusinessEmail.Text = "Not Supplied.";

                if (!string.IsNullOrWhiteSpace(ClientDetails1.ContactOffice))
                    lblContactOffice.Text = ClientDetails1.ContactOffice.Trim();
                else
                    lblContactOffice.Text = "Not Supplied.";

                //if (!string.IsNullOrWhiteSpace(ClientDetails1.ContactFax))
                //    lblFax.Text = ClientDetails1.ContactFax.Trim();
                //else
                //    lblFax.Text = "Not Supplied.";

                if (ClientDetails1.AddressLine1 != "")
                    lblBusinessAddress.Text = ClientDetails1.AddressLine1.Trim() + "<br/>";

                if (ClientDetails1.AddressLine2 != "")
                    lblBusinessAddress.Text += ClientDetails1.AddressLine2.Trim() + "<br/>";

                if (ClientDetails1.AddressLine3 != "")
                    lblBusinessAddress.Text += ClientDetails1.AddressLine3.Trim() + "<br/>";

                lblBusinessAddress.Text += UserProfile1.City.Trim() + "<br/>" + UserProfile1.State.Trim() + " " + UserProfile1.PostCode.Trim() + "<br/>";
                lblBusinessAddress.Text += CountryManager.GetCountry(int.Parse(UserProfile1.Country)).EntityList[0].CountryName.Trim();
            }
        }

        private MembershipCreateStatus CreateAccount()
        {
            MembershipCreateStatus createStatus;
            MembershipUser newUser = Membership.CreateUser(NewUser.UserName, WebContext.TempInfo.Password, NewUser.Email, NewUser.SecretQuestion, NewUser.SecretAnswer, false, out createStatus);
            switch (createStatus)
            {
                case MembershipCreateStatus.Success:
                    newUser.Comment = Status.InActiveByDefault.ToString();
                    Membership.UpdateUser(newUser);
                    if (AddUserToRole(newUser))
                    {
                        lblTransactionCompleteMessage.Text = "Congratulations! Your account has been created successfully.<br />";
                        lblTransactionCompleteMessage.Text += "You have to verify the account to activate by clicking the link sent out to your nominated Account Email.<br /><br />";
                        lblTransactionCompleteMessage.Text += "If you haven't received any email for more than 15 minutes after you created the account,  please <a href='" + BasePage.FullBaseUrl + "WebPages/ContactUs.aspx' class ='anchor'> contact us </a>.";
                        paraStepTitle.Visible = false;

                        if (WebContext.Parent != null || WebContext.Sibling !=null)
                        {
                            lblTransactionCompleteMessage.Text = "New account has been created successfully.<br />";
                            lblTransactionCompleteMessage.Text += "Please advise the new account holder to activate his/her account by clicking the link sent out to his/her nominated account email.<br /><br />";
                            lblTransactionCompleteMessage.Text += "If the person hasn't received any email for more than 15 minutes after the account was created, please ask him/her to <a href='" + BasePage.FullBaseUrl + "WebPages/ContactUs.aspx' class ='anchor'> contact us </a>.";
                        }

                        lblTransactionCompleteMessage.CssClass = "successMsg";

                        registrationCompleted = true;
                    }
                    else
                        createStatus = MembershipCreateStatus.UserRejected;
                    break;
                case MembershipCreateStatus.DuplicateUserName:
                    lblResultMessage.Text = "A user with this username already exists.";
                    lblResultMessage.CssClass = "errorMsg";
                    break;

                case MembershipCreateStatus.DuplicateEmail:
                    lblResultMessage.Text = "This email address already exists in the database.";
                    lblResultMessage.CssClass = "errorMsg";
                    break;
                case MembershipCreateStatus.InvalidEmail:
                    lblResultMessage.Text = "The email address you provided is invalid.";
                    lblResultMessage.CssClass = "errorMsg";
                    break;
                case MembershipCreateStatus.InvalidAnswer:
                    lblResultMessage.Text = "The security answer was invalid.";
                    lblResultMessage.CssClass = "errorMsg";
                    break;
                case MembershipCreateStatus.InvalidPassword:
                    lblResultMessage.Text = "The password you provided is invalid. It must be at least five characters long.";
                    lblResultMessage.CssClass = "errorMsg";
                    break;
                default:
                    lblResultMessage.Text = "There was an unknown error; the user account was NOT created.";
                    lblResultMessage.CssClass = "errorMsg";
                    break;
            }

            return createStatus;

        }

        private bool AddUserToRole(MembershipUser newUser)
        {
            try
            {
                if (HttpContext.Current.User.IsInRole("SuperAdmin") || HttpContext.Current.User.IsInRole("SuperAdminUser") ||
                    HttpContext.Current.User.IsInRole("Admin") || HttpContext.Current.User.IsInRole("AdminUser"))
                {
                    Roles.AddUserToRole(newUser.UserName, NewUser.GetRole);
                }

                else
                {
                    if (string.Compare(rblRegoType.SelectedValue, "personal", true) == 0)
                        Roles.AddUserToRole(newUser.UserName, "User");
                    else if (string.Compare(rblRegoType.SelectedValue, "business", true) == 0)
                        Roles.AddUserToRole(newUser.UserName, "Admin");
                }
                return true;
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
                return false;
            }
        }

        public bool SaveProfile()
        {
            string UserName = NewUser.UserName;
            CustomUserProfile profile = CustomUserProfile.GetUserProfile(UserName);
            //save profile.
            profile.ProfileUrl = DateTime.Now.ToString("P-ddMMyyyy-HHmmss") + ".jpg";
            WebContext.TempInfo.ProfileImageName = profile.ProfileUrl;
            //save profile ends
            profile.Title = UserProfile1.Title;
            profile.FirstName = UserProfile1.FirstName;
            profile.MiddleName = UserProfile1.MiddleName;
            profile.LastName = UserProfile1.LastName;
            profile.DOB = (DateTime?)UserProfile1.DOB;
            profile.Gender = UserProfile1.Gender;
            if (UserProfile1.Occupation == "Select Occupation")
                profile.Occupation = null;
            else
                profile.Occupation = UserProfile1.Occupation;
            profile.WebSiteUrl = UserProfile1.WebSiteUrl;
            profile.CreatedDate = System.DateTime.Now;
            profile.IPAddress = Helpers.CurrentUserIP;
            MembershipUser user = Membership.GetUser(UserName);
            profile.CreatedBy = (Guid?)user.ProviderUserKey;
            profile.UpdatedBy = (Guid?)user.ProviderUserKey;
            //address information
            profile.Address.AddressLine1 = UserProfile1.AddressLine1;
            profile.Address.AddressLine2 = UserProfile1.AddressLine2;
            profile.Address.AddressLine2 = UserProfile1.AddressLine3;
            profile.Address.AddressLine3 = UserProfile1.AddressLine3;
            profile.Address.City = UserProfile1.City;
            profile.Address.State = UserProfile1.State;
            profile.Address.PostCode = UserProfile1.PostCode;
            profile.Address.Country = UserProfile1.Country;//selected value of the country.
            //contact information
            profile.Contacts.ContactHome = UserProfile1.ContactHome;
            profile.Contacts.ContactMobile = UserProfile1.ContactMobile;
            //preferences
            profile.Preferences.Theme = UserProfile1.Theme;
            try
            {
                profile.Save();
                return true;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                return false;
            }
        }

        private Result<ClientDetails> SaveClient()
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string address = ClientDetails1.AddressLine1 + " " + ClientDetails1.AddressLine2 + " " + ClientDetails1.AddressLine3 + " " +
                 ClientDetails1.City + ", " + ClientDetails1.State + " " + ClientDetails1.PostCode + ", " + CountryManager.GetCountry(ClientDetails1.CountryID).EntityList[0].CountryName;

            var addressResult = Helpers.GetGeocodingSearchResults(address);

            if (addressResult.Elements("result").Count() == 0)
            {
                result.Message = "Oops, the address seems invalid, please verify once again.";
                result.Status = ResultStatus.Error;
                return result;
            }
            else if (addressResult.Elements("result").Count() > 1)
            {
                result.Message = "Multiple address found with entered details, please verify once again.";
                result.Status = ResultStatus.Error;
                return result;
            }
            else
            {
                string userName = NewUser.UserName;
                CustomUserProfile profile = CustomUserProfile.GetUserProfile(userName);
                ClientDetails client = new ClientDetails();
                client.ClientID = System.DateTime.Now.ToString("C-ddMMyyy-HHmmssfff");
                ClientDetails1.BusinessClientID = client.ClientID;
                client.UniqueIdentity = ClientDetails1.UniqueIdentity; //nullable
                client.UniqueDomain = ClientDetails1.UniqueDomain; //nullable
                client.ABN = ClientDetails1.ABN; //nullable
                client.ClientName = ClientDetails1.ClientName; //required
                client.ContactPerson = (Guid)Membership.GetUser(userName).ProviderUserKey;//required
                client.Description = ClientDetails1.Description; //nullable
                client.Comment = Status.InActiveByDefault.ToString(); //not nullable (its not user input)
                client.ContactOffice = ClientDetails1.ContactOffice; // not nullable
                client.BusinessEmail = ClientDetails1.BusinessEmail; // nullable
                client.ContactFax = ClientDetails1.ContactFax; //nullable
                client.BusinessUrl = ClientDetails1.BusinessUrl; // nullable
                client.LogoUrl = System.DateTime.Now.ToString("L-ddMMyyyy-HHmmss") + ".jpg"; //nullable
                WebContext.TempInfo.LogoImageName = client.LogoUrl; 
                client.AddressLine1 = ClientDetails1.AddressLine1; // not nullable
                client.AddressLine2 = ClientDetails1.AddressLine2;
                client.AddressLine3 = ClientDetails1.AddressLine3;
                client.City = ClientDetails1.City; //not nullable
                client.State = ClientDetails1.State; // not nullable
                client.PostCode = ClientDetails1.PostCode; //not nullable
                client.CountryID = new CountryDetails() { CountryID = ClientDetails1.CountryID }; // nut nullable
                if (!string.IsNullOrEmpty(ClientDetails1.EstablishedDate))
                    client.EstablishedDate = Convert.ToDateTime(ClientDetails1.EstablishedDate);
                else
                    client.EstablishedDate = null;
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                    client.CreatedBy = (Guid)Membership.GetUser(HttpContext.Current.User.Identity.Name).ProviderUserKey;
                else
                    client.CreatedBy = (Guid)Membership.GetUser(userName).ProviderUserKey;
                client.Category = new CategoryDetails() { CategoryID = ClientDetails1.CategoryID };
                client.Published = false; //It was false before.
                client.Latitude = Convert.ToDecimal(addressResult.Element("result").Element("geometry").Element("location").Element("lat").Value);
                client.Longitude = Convert.ToDecimal(addressResult.Element("result").Element("geometry").Element("location").Element("lng").Value);
                client.Theme = ClientDetails1.Theme;

                result = ClientManager.InsertClient(client);
                if (result.Status == ResultStatus.Success)
                {
                    result = ClientManager.UpdateProfileForClientID(client.ContactPerson, client.ClientID);
                    addDefaultListingPackageOnRegistration(client);
                    addDefaultProfilePackageOnRegistration(client);
                }
                return result;
            }
        }

        private void UploadUserImages()
        {
            bool isBusiness = false;
            //client logo upload
            if (string.Compare(rblRegoType.SelectedValue, "business", true) == 0)
            {
                if (WebContext.TempInfo.LogoStream != null)
                {
                    System.Drawing.Image imgLogo = System.Drawing.Image.FromStream(WebContext.TempInfo.LogoStream);
                    string dirUrl = BasePage.BaseUrl + "Uploads/" + ClientDetails1.BusinessClientID + "/LogoPicture";
                    string dirPath = Server.MapPath(dirUrl);
                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);
                    string outPutLogoName = dirPath + "/" + WebContext.TempInfo.LogoImageName;
                    imgLogo.Save(outPutLogoName);
                }
                isBusiness = true;
            }
            //Profile photo upload
            if (WebContext.TempInfo.ProfileStream != null)
            {
                System.Drawing.Image imgProfile = System.Drawing.Image.FromStream(WebContext.TempInfo.ProfileStream);
                string dirUrlP = BasePage.BaseUrl + "Uploads/" + NewUser.UserName + "/ProfilePicture";
                if (isBusiness)
                    dirUrlP = BasePage.BaseUrl + "Uploads/" + ClientDetails1.BusinessClientID + "/" + NewUser.UserName + "/ProfilePicture";
                else if(WebContext.Parent != null)
                    dirUrlP = BasePage.BaseUrl + "Uploads/" + WebContext.Parent.ClientID + "/" + NewUser.UserName + "/ProfilePicture";
                string dirPathP = Server.MapPath(dirUrlP);
                if (!Directory.Exists(dirPathP))
                    Directory.CreateDirectory(dirPathP);
                string outputfileName = dirPathP + "/" + WebContext.TempInfo.ProfileImageName;
                imgProfile.Save(outputfileName);
            }
        }

        private bool registrationCompleted = false;
        public bool RegistrationCompleted
        {
            get { return registrationCompleted; }
        }
         // add default listing and provile order
        private void addDefaultListingPackageOnRegistration(ClientDetails client)
        {
            PackageOrderDetails listingDetails = new PackageOrderDetails();
            listingDetails.OrderID = System.DateTime.Now.ToString("PO-ddMMyyyy-HHmmssfff");
            listingDetails.TransactionID = "TXN-" + listingDetails.OrderID;
            listingDetails.CreatedBy = HttpContext.Current.User.Identity.Name;
            listingDetails.OrderStatus = StatusOrder.Verified.ToString();
            listingDetails.Client = client;
            listingDetails.PaymentOption = PaymentOptionStatus.Free.ToString();
            listingDetails.RegistrationDate = System.DateTime.Now;
            //retrieve the package details
            //listing only
            PackageDetails package = ClientPackageManager.SelectPackage("CPG-03012012-144349").EntityList[0];
            listingDetails.PackageCode = package.PackageCode; //package version 1
            listingDetails.PackageName = package.PackageName;
            listingDetails.Comments = "New";
            listingDetails.ExpiryDate = DateTime.MaxValue;
            listingDetails.Duration = "1";
            ClientPackageManager.InsertPackageOrder(listingDetails);
            ClientManager.ClientFeatureSetListingStatus(client.ClientID, true);
        }

        private void addDefaultProfilePackageOnRegistration(ClientDetails client)
        {
            PackageOrderDetails profileDetails = new PackageOrderDetails();
            profileDetails.OrderID = System.DateTime.Now.ToString("PO-ddMMyyyy-HHmmssfff");
            profileDetails.TransactionID = "TXN-" + profileDetails.OrderID;
            profileDetails.CreatedBy = HttpContext.Current.User.Identity.Name;
            profileDetails.OrderStatus = StatusOrder.Verified.ToString();
            profileDetails.Client = client;
            profileDetails.PaymentOption = PaymentOptionStatus.Free.ToString();
            profileDetails.RegistrationDate = System.DateTime.Now;
            //retrieve the package details
            PackageDetails package = ClientPackageManager.SelectPackage("CPG-20122011-234910").EntityList[0];
            profileDetails.PackageCode = package.PackageCode; //package version 1
            profileDetails.PackageName = package.PackageName;
            profileDetails.Comments = "New";
            profileDetails.ExpiryDate = DateTime.MaxValue;
            profileDetails.Duration = "1";
            ClientPackageManager.InsertPackageOrder(profileDetails);
            ClientManager.ClientFeatureSetClientProfileStatus(client.ClientID, true);
        }
        
         

    }
}