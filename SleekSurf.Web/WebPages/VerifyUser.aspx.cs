using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.Manager;
using SleekSurf.FrameWork;
using System.IO;
using System.Web.Security;

namespace SleekSurf.Web.WebPages
{
    public partial class VerifyUser : BasePage
    {
        Guid userID;
        string userName;
        string logoUrl = "";
        string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
        protected void Page_Load(object sender, EventArgs e)
        {
            ActivateUser();
        }

        private void ActivateUser()
        {
            string appPath = Request.PhysicalApplicationPath;
            //PROVIDES THE LINK TO THE MAIN WEBSITE
            ltrMainPage.Text = "<span style='margin:0px; padding-top:5px; display:block;'><span style='display:inline-block; width:150px;'> Provider's Website</span> : <a style='font-weight:bold;'  href=" + FullBaseUrl + ">" + FullBaseUrl + "</a></span>";
            if (Request.QueryString["ID"] != null)
            {
                userName = WebContext.UsernameToVerify.Decrypt();

                if (string.IsNullOrEmpty(userName))
                {
                    ltrVerificationMessage.Text = "Invalid Request";
                    ltrMessageBoard.Text = "Sorry! your request could not be processed this time, please try again by clicking the link provided in your email.";
                    ltrMessageBoard.Text += " If this problem occurs again, please <a href='"+FullBaseUrl+"WebPages/ContactUs.aspx'>contact us</a>";
                }
                else
                {
                    MembershipUser userInfo = Membership.GetUser(userName);
                    userID = (Guid)userInfo.ProviderUserKey;
                    if (userInfo == null)
                    {
                        ltrVerificationMessage.Text = "User Information Not Found";
                        ltrMessageBoard.Text = "User Information could not be found. Please make sure you are registered or your account has not been deleted.";
                    }
                    else
                    {
                        CustomUserProfile profile = CustomUserProfile.GetUserProfile(userName);

                        if (userInfo.Comment == Status.InActiveByDefault.ToString())
                        {
                            string clientID = null;
                            if (CheckParentExistance((Guid)userInfo.ProviderUserKey, out clientID))
                            {
                                if (CheckParentUserStatus(clientID))
                                {
                                    userInfo.IsApproved = true;
                                    userInfo.Comment = Status.Activated.ToString();
                                    Membership.UpdateUser(userInfo);
                                    ClientManager.PublishClient(clientID, Status.Activated.ToString());

                                    ltrVerificationMessage.Text = "Account Verified";
                                    ltrMessageBoard.Text = "<span style='margin:0px; padding:5px 0px; display:block;'> Thank you! " + profile.Title + " " + profile.FirstName + " " + profile.MiddleName + " " + profile.LastName + ", </span>";
                                    ltrMessageBoard.Text += "<span style = 'margin:0px; padding:5px 0px; display:block;'>Your account has been verified now. You can log in and use our services.</span>";
                                    ltrMessageBoard.Text += "<span style='margin:0px; padding:10px 0px 0px 0px; display:block;'><span style='display:inline-block; width:150px;'>Username</span> : <span style='font-weight:bold;'>" + userName + "</span></span>";
                                    //make panel visible
                                    ClientDetails client = ClientManager.SelectClient(clientID).EntityList[0];
                                    pnlCompanyUrl.Visible = true;
                                    if (!string.IsNullOrEmpty(client.UniqueIdentity))
                                        ltrCompanyProfile.Text = "<span style='margin:0px; padding-top:5px; display:block;'><span style='display:inline-block; width:150px'>Profile URL</span> : <a style='font-weight:bold;' href='" + FullBaseUrl + client.UniqueIdentity + "'>" + FullBaseUrl + client.UniqueIdentity + "</a> OR <br /><span style='display:inline-block; width:150px'>&nbsp;</span>&nbsp;&nbsp;&nbsp;<a style='font-weight:bold;' href='http://www." + client.UniqueIdentity + ".sleeksurf.com'>http://www." + client.UniqueIdentity + ".sleeksurf.com</a></span>";
                                    if (!string.IsNullOrEmpty(client.UniqueDomain))
                                        ltrCompanyProfile.Text += "<br /><span style='margin:0px; padding-top:5px; display:block;'><span style='display:inline-block; width:150px'>Domain URL</span> : <a style='font-weight:bold;' href='" + client.UniqueDomain + "</a> OR <br /><span style='display:inline-block; width:150px'>&nbsp;</span>&nbsp;&nbsp;&nbsp;<a style='font-weight:bold;' href='http://www." + client.UniqueIdentity + ".sleeksurf.com'>http://www." + client.UniqueIdentity + ".sleeksurf.com</a></span>";

                                    pnlAdminUrl.Visible = true;
                                    ltrAdminUrl.Text = "<span style='margin:0px; padding-top:5px; display:block;'><span style='display:inline-block; width:150px'>Control Panel (Backend)</span> : <a style='font-weight:bold;' href='" + FullBaseUrl + "Admin" + "'>" + FullBaseUrl + "Admin" + "</a></span>";

                                    string userSubject = "Account Activated";

                                    string logoDisplay = "none";
                                    if (!string.IsNullOrEmpty(client.LogoUrl))
                                    {
                                        string urlPath = Server.MapPath("~/Uploads/" + client.ClientID + "/LogoPicture/" + client.LogoUrl);
                                        if (System.IO.File.Exists(urlPath))
                                        {
                                            logoUrl = FullBaseUrl + "Uploads/" + client.ClientID + "/LogoPicture/" + client.LogoUrl;
                                            imgLogo.Src = logoUrl;
                                            logoDisplay = "block";
                                        }
                                        else
                                            imgLogo.Visible = false;
                                    }

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
                                    userBody = userBody.Replace("<%ControlPanel%>", FullBaseUrl + "Admin");
                                    if (!string.IsNullOrEmpty(client.UniqueIdentity))
                                        userBody = userBody.Replace("<%BusinessProfile%>", FullBaseUrl + client.UniqueIdentity + " OR <br /> http://www." + client.UniqueIdentity + ".sleeksurf.com");
                                    else
                                        userBody = userBody.Replace("<%BusinessProfile%>", "Not available");
                                    if (!string.IsNullOrEmpty(client.UniqueDomain))
                                        userBody = userBody.Replace("<%DomainUrl%>", client.UniqueDomain);
                                    else
                                        userBody = userBody.Replace("<%DomainUrl%>", "Not available");
                                    userBody = userBody.Replace("<%MainWebsite%>", FullBaseUrl);
                                    //DETERMINE WHETHER THE USER ADDED WAS BY COMPANY OR SELF
                                    string thisBusinessID = ClientManager.SelectClientIDByUserID((Guid)(Membership.GetUser(userInfo.UserName).ProviderUserKey));
                                    if (Roles.GetRolesForUser(userInfo.UserName).Contains("AdminUser"))//IF THE USER IS ADDED BY BY THE COMPANY
                                    {
                                        ltrCommanyName.Text = ClientManager.SelectClient(thisBusinessID).EntityList[0].ClientName;
                                    }
                                    else if (Roles.GetRolesForUser(userInfo.UserName).Contains("Admin"))
                                    {
                                        Guid contactPerson = ClientManager.SelectClient(thisBusinessID).EntityList[0].ContactPerson;
                                        if ((Guid)userInfo.ProviderUserKey != contactPerson)//IF THE USER IS ADDED BY THE COMPANY AND HAS ADMIN ROLE
                                        {
                                            ltrCommanyName.Text = ClientManager.SelectClient(thisBusinessID).EntityList[0].ClientName;
                                        }
                                        else//IF THE USER HAS ADMIN ROLE BUT NOT ADDED BY THE COMPANY.
                                        {
                                            pnlMainPage.Visible = true;
                                            ltrCommanyName.Text = "SleekSurf Team";
                                        }
                                    }
                                    else
                                    {
                                        pnlMainPage.Visible = true;
                                        ltrCommanyName.Text = "SleekSurf Team";
                                    }

                                    userBody = userBody.Replace("<%CompanyName%>", ltrCommanyName.Text);

                                    UserInfoPartial registeredUser = new UserInfoPartial() { FirstName = profile.FirstName, MiddleName = profile.MiddleName, LastName = profile.LastName, Email = userInfo.Email };

                                    Helpers.SendEmail(registeredUser, "SleekSurf", userSubject, userBody);
                                }
                                else
                                {
                                    ltrVerificationMessage.Text = "Account not activated";
                                    ltrMessageBoard.Text = "Your account could not be activated. Please contact your Admin";
                                }
                            }
                            else//FOR NORMAL USERS
                            {
                                userInfo.IsApproved = true;
                                userInfo.Comment = Status.Activated.ToString();
                                Membership.UpdateUser(userInfo);

                                ltrVerificationMessage.Text = "Account Verified";
                                ltrMessageBoard.Text = "<p> Thank you " + profile.Title + " " + profile.FirstName + " " + profile.MiddleName + " " + profile.LastName + ", </p>";
                                ltrMessageBoard.Text += "<span style = 'margin:0px; padding:5px 0px; display:block;'>Your account has been verified now. You can log in and use our services.</span>";
                                ltrMessageBoard.Text += "<span style='margin:0px; padding:5px 0px; display:block;'><span style='display:inline-block; width:150px;'>Username</span> : <span style='font-weight:bold;'>" + userName + "</span></span>";
                                ltrMessageBoard.Text += "<span style='margin:0px; padding:5px 0px; display:block;'><span style='display:inline-block; width:150px;'>Login page</span> : <span style='font-weight:bold;'>" + FullBaseUrl + "Login.aspx</span></span>";

                                //SEND EMAIL

                                string userSubject = "Account Activated";

                                logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
                                StreamReader userBodySR = new StreamReader(appPath + "EmailTemplates/UserEmailAfterVerification.txt");
                                string userBody = userBodySR.ReadToEnd();
                                userBodySR.Close();

                                userBody = userBody.Replace("<%Logo%>", logoUrl);
                                userBody = userBody.Replace("<%TopBackGround%>", topBackGroundUrl);
                                userBody = userBody.Replace("<%Title%>", profile.Title);
                                userBody = userBody.Replace("<%FirstName%>", profile.FirstName);
                                userBody = userBody.Replace("<%MiddleName%>", profile.MiddleName);
                                userBody = userBody.Replace("<%LastName%>", profile.LastName);
                                userBody = userBody.Replace("<%UserName%>", userName);
                                userBody = userBody.Replace("<%MainWebsite%>", FullBaseUrl);
                                userBody = userBody.Replace("<%SenderName%>", "SleekSurf Team");
                                userBody = userBody.Replace("<%SenderCompany%>", "SleekSurf");

                                UserInfoPartial registeredUser = new UserInfoPartial() { FirstName = profile.FirstName, MiddleName = profile.MiddleName, LastName = profile.LastName, Email = userInfo.Email };

                                Helpers.SendEmail(registeredUser, "SleekSurf", userSubject, userBody);
                                ltrCommanyName.Text = "SleekSurf Team";
                            }
                        }
                        else if (userInfo.Comment == Status.Activated.ToString())
                        {
                            string clientID = null;
                            lbtnSendLinkToEmail.Visible = true;
                            if (CheckParentExistance((Guid)userInfo.ProviderUserKey, out clientID))
                            {
                                if (CheckParentUserStatus(clientID))
                                {
                                    userInfo.IsApproved = true;
                                    userInfo.Comment = Status.Activated.ToString();
                                    Membership.UpdateUser(userInfo);


                                    ltrVerificationMessage.Text = "Account is Active";
                                    ltrMessageBoard.Text = "<p> Hi " + profile.Title + " " + profile.FirstName + " " + profile.MiddleName + " " + profile.LastName + ", </p>";
                                    ltrMessageBoard.Text += "<p>Your account is already active and you can use our services after logging in.</p>";
                                    ltrMessageBoard.Text += "<span style='margin:0px; padding:10px 0px 0px 0px; display:block;'><span style='display:inline-block; width:150px;'>Username</span> : <span style='font-weight:bold;'>" + userName + "</span></span>";
                                    //make panel visible
                                    ClientDetails client = ClientManager.SelectClient(clientID).EntityList[0];
                                    pnlCompanyUrl.Visible = true;
                                    if (!string.IsNullOrEmpty(client.UniqueIdentity))
                                        ltrCompanyProfile.Text = "<span style='margin:0px; padding-top:5px; display:block;'><span style='display:inline-block; width:150px'>Profile URL</span> : <a style='font-weight:bold;' href='" + FullBaseUrl + client.UniqueIdentity + "'>" + FullBaseUrl + client.UniqueIdentity + "</a> OR <br /><span style='display:inline-block; width:150px'>&nbsp;</span>&nbsp;&nbsp;&nbsp;<a style='font-weight:bold;' href='http://www." + client.UniqueIdentity + ".sleeksurf.com'>http://www." + client.UniqueIdentity + ".sleeksurf.com</a></span>";
                                    if (!string.IsNullOrEmpty(client.UniqueDomain))
                                        ltrCompanyProfile.Text += "<br /><span style='margin:0px; padding-top:5px; display:block;'><span style='display:inline-block; width:150px'>Domain URL</span> : <a style='font-weight:bold;' href='" + client.UniqueDomain + "</a> OR <br /><span style='display:inline-block; width:150px'>&nbsp;</span>&nbsp;&nbsp;&nbsp;<a style='font-weight:bold;' href='http://www." + client.UniqueIdentity + ".sleeksurf.com'>http://www." + client.UniqueIdentity + ".sleeksurf.com</a></span>";
                                    pnlAdminUrl.Visible = true;
                                    ltrAdminUrl.Text = "<span style='margin:0px; padding-top:5px; display:block;'><span style='display:inline-block; width:150px'>Control Panel (Backend)</span> : <a style='font-weight:bold;' href='" + FullBaseUrl + "Admin" + "'>" + FullBaseUrl + "Admin" + "</a></span>";
                                    string thisBusinessID = ClientManager.SelectClientIDByUserID((Guid)(Membership.GetUser(userInfo.UserName).ProviderUserKey));
                                    ClientDetails clientDetail = ClientManager.SelectClient(thisBusinessID).EntityList[0];
                                    if (!string.IsNullOrEmpty(clientDetail.LogoUrl))
                                    {
                                        string urlPath = Server.MapPath("~/Uploads/" + clientDetail.ClientID + "/LogoPicture/" + clientDetail.LogoUrl);
                                        if (System.IO.File.Exists(urlPath))
                                        {
                                            imgLogo.Src = FullBaseUrl + "Uploads/" + clientDetail.ClientID + "/LogoPicture/" + clientDetail.LogoUrl;
                                        }
                                        else
                                            imgLogo.Visible = false;
                                    }
                                    if (Roles.GetRolesForUser(userInfo.UserName).Contains("AdminUser"))//IF THE USER IS ADDED BY BY THE COMPANY
                                    {
                                        ltrCommanyName.Text = clientDetail.ClientName;
                                    }
                                    else if (Roles.GetRolesForUser(userInfo.UserName).Contains("Admin"))
                                    {
                                        Guid contactPerson = ClientManager.SelectClient(thisBusinessID).EntityList[0].ContactPerson;
                                        if ((Guid)userInfo.ProviderUserKey != contactPerson)//IF THE USER IS ADDED BY THE COMPANY AND HAS ADMIN ROLE
                                        {
                                            ltrCommanyName.Text = ClientManager.SelectClient(thisBusinessID).EntityList[0].ClientName;
                                        }
                                        else//IF THE USER HAS ADMIN ROLE BUT NOT ADDED BY THE COMPANY.
                                        {
                                            pnlMainPage.Visible = true;
                                            ltrCommanyName.Text = "SleekSurf Team";
                                        }
                                    }
                                    else
                                    {
                                        pnlMainPage.Visible = true;
                                        ltrCommanyName.Text = "SleekSurf Team";
                                    }
                                }
                            }
                            else
                            {
                                ltrVerificationMessage.Text = "Account is Active";
                                ltrMessageBoard.Text = "<p> Hi " + profile.Title + " " + profile.FirstName + " " + profile.MiddleName + " " + profile.LastName + ", </p>";
                                ltrMessageBoard.Text += "<p>Your account is already active and you can use our services after logging in.</p>";
                                ltrMessageBoard.Text += "<span style='margin:0px; padding:10px 0px 0px 0px; display:block;'><span style='display:inline-block; width:150px;'>Username</span> : <span style='font-weight:bold;'>" + userName + "</span></span>";
                                pnlAdminUrl.Visible = true;
                                ltrAdminUrl.Text = "<span style='margin:0px; padding-top:5px; display:block;'><span style='display:inline-block; width:150px'>Control Panel (Backend)</span> : <a style='font-weight:bold;' href='" + FullBaseUrl + "Admin" + "'>" + FullBaseUrl + "Admin" + "</a></span>";
                                pnlMainPage.Visible = true;
                                ltrCommanyName.Text = "SleekSurf Team";
                                lbtnSendLinkToEmail.Visible = false;
                            }
                        }
                        else
                        {
                            ltrVerificationMessage.Text = "Deactivated by Administrator";
                            ltrMessageBoard.Text = "<p> Hi " + userName + ", </p>";
                            ltrMessageBoard.Text += "<p>Your account has been deactivated by Administrator. Please contact for further enquiry.</p>";
                        }
                    }
                }
            }
            else
            {
                Redirector.GoToHomePage();
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

        private bool CheckParentUserStatus(string clientID)
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

        protected void lbtnSendLinkToEmail_Click(object sender, EventArgs e)
        {
            string appPath = Request.PhysicalApplicationPath;
            string userSubject = "Account Activated";

            MembershipUser userInfo = Membership.GetUser(userName);
            userID = (Guid)userInfo.ProviderUserKey;
            CustomUserProfile profile = CustomUserProfile.GetUserProfile(userName);

            string clientID = null;
            if (CheckParentExistance((Guid)userInfo.ProviderUserKey, out clientID))
            {
                if (CheckParentUserStatus(clientID))
                {
                    ClientDetails client = ClientManager.SelectClient(clientID).EntityList[0];
                    string logoDisplay = "none";
                    if (!string.IsNullOrEmpty(client.LogoUrl))
                    {
                        string urlPath = Server.MapPath("~/Uploads/" + client.ClientID + "/LogoPicture/" + client.LogoUrl);
                        if (System.IO.File.Exists(urlPath))
                        {
                            logoUrl = FullBaseUrl + "Uploads/" + client.ClientID + "/LogoPicture/" + client.LogoUrl;
                            logoDisplay = "block";
                        }
                    }

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
                    userBody = userBody.Replace("<%ControlPanel%>", FullBaseUrl + "Admin");
                    if (!string.IsNullOrEmpty(client.UniqueIdentity))
                        userBody = userBody.Replace("<%BusinessProfile%>", FullBaseUrl + client.UniqueIdentity + " OR <br /> http://www." + client.UniqueIdentity + ".sleeksurf.com");
                    else
                        userBody = userBody.Replace("<%BusinessProfile%>", "Not available");
                    if (!string.IsNullOrEmpty(client.UniqueDomain))
                        userBody = userBody.Replace("<%DomainUrl%>", client.UniqueDomain);
                    else
                        userBody = userBody.Replace("<%DomainUrl%>", "Not available");
                    userBody = userBody.Replace("<%MainWebsite%>", FullBaseUrl);
                    //DETERMINE WHETHER THE USER ADDED WAS BY COMPANY OR SELF
                    string thisBusinessID = ClientManager.SelectClientIDByUserID((Guid)(Membership.GetUser(userInfo.UserName).ProviderUserKey));
                    if (Roles.GetRolesForUser(userInfo.UserName).Contains("AdminUser"))//IF THE USER IS ADDED BY BY THE COMPANY
                    {
                        ltrCommanyName.Text = ClientManager.SelectClient(thisBusinessID).EntityList[0].ClientName;
                    }
                    else if (Roles.GetRolesForUser(userInfo.UserName).Contains("Admin"))
                    {
                        Guid contactPerson = ClientManager.SelectClient(thisBusinessID).EntityList[0].ContactPerson;
                        if ((Guid)userInfo.ProviderUserKey != contactPerson)//IF THE USER IS ADDED BY THE COMPANY AND HAS ADMIN ROLE
                        {
                            ltrCommanyName.Text = ClientManager.SelectClient(thisBusinessID).EntityList[0].ClientName;
                        }
                        else//IF THE USER HAS ADMIN ROLE BUT NOT ADDED BY THE COMPANY.
                        {
                            pnlMainPage.Visible = true;
                            ltrCommanyName.Text = "SleekSurf Team";
                        }
                    }
                    else
                    {
                        pnlMainPage.Visible = true;
                        ltrCommanyName.Text = "SleekSurf Team";
                    }

                    userBody = userBody.Replace("<%CompanyName%>", ltrCommanyName.Text);

                    UserInfoPartial registeredUser = new UserInfoPartial() { FirstName = profile.FirstName, MiddleName = profile.MiddleName, LastName = profile.LastName, Email = userInfo.Email };

                    if (Helpers.SendEmail(registeredUser, ltrCommanyName.Text, userSubject, userBody))
                    {
                        ltrEmailMessage.Visible = true;
                        ltrEmailMessage.CssClass = "success";
                        ltrEmailMessage.Text = "Details sent to your nominated email.";
                    }
                    else
                    {
                        ltrEmailMessage.CssClass = "error";
                        ltrEmailMessage.Text = "Error Sending Email, Please try later.";
                    }
                }
            }
        }
    }
}