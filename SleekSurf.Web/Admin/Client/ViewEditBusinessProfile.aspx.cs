using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using SleekSurf.Entity;
using SleekSurf.Manager;
using System.Web.Security;
using System.Transactions;
using System.IO;

namespace SleekSurf.Web.Admin.Client
{
    public partial class ViewEditBusinessProfile : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (WebContext.Parent != null)
                {
                    ucNewEditClient.SetEnvironmentForUpdate();
                    BindBusinessProfile();

                    ClientFeatureDetails clientFeature = ClientManager.SelectClientFeatureDetails(WebContext.Parent.ClientID).EntityList[0];

                    if (clientFeature.Listing && !clientFeature.ClientDomain && !clientFeature.ClientProfile)
                        hlNewEditDataInfo.Visible = false;
                }
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("ClientAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("Profile"))].Selected = true;

            if (WebContext.Sibling != null)
            {
                Menu tmpMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
                tmpMenu.Items[tmpMenu.Items.IndexOf(tmpMenu.FindItem("ClientMgmt"))].Selected = true;
            }

        }

        private void BindBusinessProfile()
        {
            ucNewEditClient.SetEnvironmentForUpdateForAdmin();
            ClientDetails client = WebContext.Parent;
            ucNewEditClient.CategoryID = client.Category.CategoryID;
            ucNewEditClient.ABN = client.ABN;
            ucNewEditClient.UniqueIdentity = client.UniqueIdentity;
            ucNewEditClient.UniqueDomain = "http://" + client.UniqueDomain;
            ucNewEditClient.ClientName = client.ClientName;
            ucNewEditClient.Description = client.Description;
            ucNewEditClient.ContactOffice = client.ContactOffice;
            ucNewEditClient.ContactFax = client.ContactFax;
            ucNewEditClient.BusinessEmail = client.BusinessEmail;
            ucNewEditClient.BusinessUrl = client.BusinessUrl;
            ucNewEditClient.AddressLine1 = client.AddressLine1;
            ucNewEditClient.AddressLine2 = client.AddressLine2;
            ucNewEditClient.AddressLine3 = client.AddressLine3;
            ucNewEditClient.City = client.City;
            ucNewEditClient.State = client.State;
            ucNewEditClient.PostCode = client.PostCode;
            ucNewEditClient.CountryID = client.CountryID.CountryID;
            ucNewEditClient.EstablishedDate = client.EstablishedDate.ToString();
            ucNewEditClient.Published = client.Published;
            ExtractAdminUserList(client.ClientID);
            ucNewEditClient.ContactPerson = client.ContactPerson;
        }

        private void ExtractAdminUserList(string clientID)
        {
            List<Guid> userList = ClientManager.SelectUsers(clientID);
            List<Guid> adminUserList = new List<Guid>();
            foreach (Guid id in userList)
            {
                MembershipUser currentUser = Membership.GetUser(id);
                if (currentUser.Comment != Status.InActiveByDefault.ToString() & Roles.GetRolesForUser(currentUser.UserName).Contains("Admin"))
                    adminUserList.Add(id);
            }

            DropDownList ddlContactPerson = (DropDownList)ucNewEditClient.FindControl("ddlContactPerson");

            foreach (Guid id in adminUserList)
            {
                CustomUserProfile profile = CustomUserProfile.GetUserProfile(Membership.GetUser(id).UserName);
                string Name = profile.FirstName + " " + profile.MiddleName + " " + profile.LastName;
                ddlContactPerson.Items.Add(new ListItem(Name, id.ToString()));

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Default.aspx");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                Result<ClientDetails> result = UpdateClient();
                if (result.Status == ResultStatus.Success)
                    lblMessage.CssClass = "successMsg";
                else
                    lblMessage.CssClass = "errorMsg";

                lblMessage.Text = result.Message;
            }
        }

        private Result<ClientDetails> UpdateClient()
        {
            Result<ClientDetails> result = new Result<ClientDetails>();

            string address = ucNewEditClient.AddressLine1 + " " + ucNewEditClient.AddressLine2 + " " + ucNewEditClient.AddressLine3 + " " +
                 ucNewEditClient.City + ", " + ucNewEditClient.State + " " + ucNewEditClient.PostCode + ", " + CountryManager.GetCountry(ucNewEditClient.CountryID).EntityList[0].CountryName;

            var addressResult = Helpers.GetGeocodingSearchResults(address);

            if (addressResult.Elements("result").Count() == 0)
            {
                result.Message = "Oops, the address seems invalid, please verify once again.";
                lblMessage.CssClass = "errorMsg";
                result.Status = ResultStatus.Error;
                return result;
            }
            else if (addressResult.Elements("result").Count() > 1)
            {
                result.Message = "Multiple address found with entered details, please verify once again.";
                lblMessage.CssClass = "normalMsg";
                result.Status = ResultStatus.Error;
                return result;
            }
            else
            {
                ClientDetails client = new ClientDetails();
                client.ClientID = WebContext.Parent.ClientID;
                client.ABN = ucNewEditClient.ABN;
                client.UniqueIdentity = ucNewEditClient.UniqueIdentity;
                if (!string.IsNullOrEmpty(ucNewEditClient.UniqueDomain) && ucNewEditClient.UniqueDomain !="http://")
                    client.UniqueDomain = new Uri(ucNewEditClient.UniqueDomain.ToLower()).Host;
                client.ClientName = ucNewEditClient.ClientName;
                client.ContactPerson = (Guid)ucNewEditClient.ContactPerson;
                client.Description = ucNewEditClient.Description;
                client.Comment = WebContext.Parent.Comment;
                client.ContactOffice = ucNewEditClient.ContactOffice;
                client.BusinessEmail = ucNewEditClient.BusinessEmail;
                client.ContactFax = ucNewEditClient.ContactFax;
                client.BusinessUrl = ucNewEditClient.BusinessUrl;
                client.LogoUrl = WebContext.Parent.LogoUrl;
                client.AddressLine1 = ucNewEditClient.AddressLine1;
                client.AddressLine2 = ucNewEditClient.AddressLine2;
                client.AddressLine3 = ucNewEditClient.AddressLine3;
                client.City = ucNewEditClient.City;
                client.State = ucNewEditClient.State;
                client.PostCode = ucNewEditClient.PostCode;
                client.CountryID = new CountryDetails() { CountryID = ucNewEditClient.CountryID };
                if (!string.IsNullOrEmpty(ucNewEditClient.EstablishedDate))
                    client.EstablishedDate = Convert.ToDateTime(ucNewEditClient.EstablishedDate);
                else
                    client.EstablishedDate = null;
                client.UpdatedBy = (Guid)Membership.GetUser(WebContext.CurrentUser.Identity.Name).ProviderUserKey;
                client.Category = new CategoryDetails() { CategoryID = ucNewEditClient.CategoryID };
                client.Published = ucNewEditClient.Published;
                client.Latitude = Convert.ToDecimal(addressResult.Element("result").Element("geometry").Element("location").Element("lat").Value);
                client.Longitude = Convert.ToDecimal(addressResult.Element("result").Element("geometry").Element("location").Element("lng").Value);
              
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        if (ucNewEditClient.LogoImageStream.HasFile)
                        {
                            //deleted the old profile photo if it exists
                            string dirUrlP = BasePage.BaseUrl + "Uploads/" + WebContext.Parent.ClientID + "/LogoPicture";
                            string dirPathP = Server.MapPath(dirUrlP);
                            if (File.Exists(dirPathP + "/" + client.LogoUrl))
                                File.Delete(dirPathP + "/" + client.LogoUrl);
                            //create directory and saves new file
                            if (!Directory.Exists(dirPathP))
                                Directory.CreateDirectory(dirPathP);

                            client.LogoUrl = System.DateTime.Now.ToString("L-ddMMyyyy-HHmmss") + ".jpg";
                            string outputfileName = dirPathP + "/" + client.LogoUrl;
                            int profileImageSize = 150;
                            ucNewEditClient.LogoImageStream.SaveAs(outputfileName);
                            Helpers.ResizeImage(outputfileName, outputfileName, profileImageSize);
                        }
                        //this block is relevant only if the user with superadmin role is updating
                        if (WebContext.Parent.Published != ucNewEditClient.Published)
                        {
                            if (ucNewEditClient.Published && client.Comment == Status.InActiveByDefault.ToString())
                            {
                                string userName = Membership.GetUser(client.ContactPerson).UserName;
                                ActivateUser(userName);
                            }

                            if (ucNewEditClient.Published)
                                client.Comment = Status.Activated.ToString();
                            else
                                client.Comment = Status.InActiveBySuperAdmin.ToString();
                            List<Guid> userList = ClientManager.SelectUsers(client.ClientID);
                            foreach (Guid id in userList)
                            {
                                MembershipUser user = Membership.GetUser(id);
                                if (user.Comment == Status.Activated.ToString())
                                {
                                    //allowing just the contact person to remain active for client maintenance (like renew, etc)
                                    if ((Guid)user.ProviderUserKey != WebContext.Parent.ContactPerson)
                                    {
                                        user.IsApproved = ucNewEditClient.Published;
                                        user.Comment = Status.InActiveBySuperAdminForClient.ToString();
                                    }
                                }
                                else if (user.Comment == Status.InActiveBySuperAdminForClient.ToString())
                                {
                                    user.IsApproved = ucNewEditClient.Published;
                                    user.Comment = Status.Activated.ToString();
                                }
                                Membership.UpdateUser(user);
                            }
                        }
                        result = ClientManager.UpdateClient(client);
                        WebContext.Parent = client;
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        Helpers.LogError(ex);
                        result.Message = "ERROR! " + ex.Message;
                        result.Status = ResultStatus.Error;
                        result.EntityList = null;
                    }
                    return result;
                }
            }
        }

        private void ActivateUser(string userName)
        {

            MembershipUser userInfo = Membership.GetUser(userName);
            Guid userID = (Guid)userInfo.ProviderUserKey;

            // string userPassword = userInfo.GetPassword();
            //get profile with the username
            CustomUserProfile profile = CustomUserProfile.GetUserProfile(userName);

            //if (userInfo.Comment == Status.InActiveByDefault.ToString())
            //{
            string clientID = null;
            //provides the link to main website
            string mainUrl = "<span style='margin:0px; padding:10px 0px; display:block;'> Visit us at <a style='font-weight:bold;'  href=" + FullBaseUrl + ">" + FullBaseUrl + "</a></span>";
            if (CheckParentExistance((Guid)userInfo.ProviderUserKey, out clientID))
            {
                if (CheckParentUserStatus(clientID, userID))
                {
                    userInfo.IsApproved = true;
                    userInfo.Comment = Status.Activated.ToString();
                    Membership.UpdateUser(userInfo);

                    ClientDetails client = ClientManager.SelectClient(clientID).EntityList[0];

                    string adminUrl = "<span style='margin:0px; padding:10px 0px; display:block;'>Your URL to access Control Panel (Backend) <a style='font-weight:bold;' href='" + FullBaseUrl + "Admin" + "'>" + FullBaseUrl + "Admin" + "</a></span>";
                    string companyProfile = "<span style='margin:0px; padding:10px 0px; display:block;'>Your business URL is: <a style='font-weight:bold;' href='" + FullBaseUrl + client.UniqueIdentity + "'>" + FullBaseUrl + client.UniqueIdentity + "</a></span>";

                    string userSubject = "Account Activated";

                    //string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
                    string logoUrl = "";
                    string logoDisplay = "none";
                    if (!string.IsNullOrEmpty(client.LogoUrl))
                    {
                        string urlPath = Server.MapPath("~/Uploads/" + client.ClientID + "/LogoPicture/" + client.LogoUrl);
                        if (System.IO.File.Exists(urlPath))
                        {
                            logoUrl = FullBaseUrl + "Uploads/" + WebContext.Parent.ClientID + "/LogoPicture/" + WebContext.Parent.LogoUrl;
                            logoDisplay = "block";
                        }
                    }
                    string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
                    string appPath = Request.PhysicalApplicationPath;
                    StreamReader userBodySR = new StreamReader(appPath + "EmailTemplates/BusinessEmailAfterVerification.txt");
                    string userBody = userBodySR.ReadToEnd();
                    userBodySR.Close();
                    userBody = userBody.Replace("<%Logo%>", logoUrl);
                    userBody = userBody.Replace("<%LogoDisplay%>", logoDisplay);
                    userBody = userBody.Replace("<%TopBackGround%>", topBackGroundUrl);
                    userBody = userBody.Replace("<%Title%>", profile.Title);
                    userBody = userBody.Replace("<%FirstName%>", profile.FirstName);
                    userBody = userBody.Replace("<%MiddleName%>", profile.MiddleName);
                    userBody = userBody.Replace("<%LastName%>", profile.LastName);
                    userBody = userBody.Replace("<%UserName%>", userName);
                    userBody = userBody.Replace("<%ControlPanel%>", adminUrl);
                    userBody = userBody.Replace("<%BusinessProfile%>", companyProfile);
                    userBody = userBody.Replace("<%MainWebsite%>", mainUrl);

                    UserInfoPartial registeredUser = new UserInfoPartial() { FirstName = profile.FirstName, MiddleName = profile.MiddleName, LastName = profile.LastName, Email = userInfo.Email };

                    Helpers.SendEmail(registeredUser, "SleekSurf", userSubject, userBody);
                }
            }
        }

        private bool CheckParentExistance(Guid userID, out string clientID)
        {
            clientID = ClientManager.SelectProfileClientID(userID);
            if (string.IsNullOrEmpty(clientID))
                return false;
            else
                return true;
        }

        private bool CheckParentUserStatus(string clientID, Guid userID)
        {
            ClientDetails client = ClientManager.SelectClient(clientID).EntityList[0];
            if (client.Published)
                return true;
            else
            {
                //even if the client is not published, if the newly registered user is the contact person 
                //for the client, then it will be activated by checking the following condition.
                if (client.ContactPerson == userID)
                    return true;

                return false;
            }
        }
    }
}