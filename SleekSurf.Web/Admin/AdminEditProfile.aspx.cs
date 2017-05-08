using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using System.Web.Security;
using System.Transactions;
using System.IO;

namespace SleekSurf.Web.Admin
{
    public partial class AdminEditProfile : BasePage
    {
        MembershipUser selectedUser = null;
        List<MembershipUser> userList = new List<MembershipUser>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebContext.GetQueryStringValue("EditUser") == null)
            {
                WebContext.SelectedUser = null;

                if (WebContext.CurrentUser.IsInRole("SuperAdmin") || WebContext.CurrentUser.IsInRole("SuperAdminUser"))
                    WebContext.Parent = null;
            }

            if (WebContext.SelectedUser != null)
            {
                selectedUser = WebContext.SelectedUser as MembershipUser;
            }
            else
            {
                selectedUser = Membership.GetUser(WebContext.CurrentUser.Identity.Name);
            }

            if (!IsPostBack)
            {
                BindProfile(selectedUser.UserName);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (WebContext.GetQueryStringValue("EditUser") != null)
            {
                if (WebContext.Parent != null)
                {
                    Menu tempMenu = (Menu)Master.FindControl("ClientAdminNavMenu").FindControl("Menu");
                    tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("UserMgmt"))].Selected = true;

                    if (WebContext.Sibling != null)
                    {
                        Menu tmpMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
                        tmpMenu.Items[tmpMenu.Items.IndexOf(tmpMenu.FindItem("ClientMgmt"))].Selected = true;
                    }
                }
                else if (WebContext.Sibling != null)
                {
                    Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
                    tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("UserMgmt"))].Selected = true;
                }
            }

            if (WebContext.SelectedUser != null)
            {
                if (fvUserAccount.CurrentMode == FormViewMode.ReadOnly)
                    ((Panel)fvUserAccount.FindControl("pnlRestrictedItems")).Visible = true;
                if (fvUserAccount.CurrentMode == FormViewMode.Edit)
                    ((Panel)fvUserAccount.FindControl("pnlRestrictedItems")).Visible = true;
            }
        }

        #region Account Formview section

        protected void txtEditEmail_TextChanged(object sender, EventArgs e)
        {
            TextBox txtEmail = (TextBox)fvUserAccount.FindControl("txtEditEmail");
            MembershipUserCollection userList = Membership.FindUsersByEmail(txtEmail.Text);
            Label lblErrorEmail = (Label)fvUserAccount.FindControl("lblErrorEmail");
            CompareValidator cmpCompareEmail = (CompareValidator)fvUserAccount.FindControl("cmpCompareEmail");
            if (userList.Count > 0)
            {
                MembershipUser tempUser = null;
                foreach (MembershipUser user in userList)
                {
                    tempUser = user;
                }
                if (tempUser.UserName == selectedUser.UserName)
                {
                    cmpCompareEmail.ValueToCompare = txtEmail.Text;
                    lblErrorEmail.Text = string.Empty;
                }
                else
                {
                    lblErrorEmail.Text = "Email exists!";
                    cmpCompareEmail.ValueToCompare = "";
                }
            }
            else
            {
                lblErrorEmail.Text = "";
                cmpCompareEmail.ValueToCompare = txtEmail.Text;
            }
        }

        protected void fvUserAccount_DataBound(object sender, EventArgs e)
        {
            if (fvUserAccount.DataItem != null)
            {
                if (fvUserAccount.CurrentMode == FormViewMode.Edit)
                {
                    if (string.Compare(WebContext.CurrentUser.Identity.Name, fvUserAccount.DataKey["UserName"].ToString(), true) == 0)
                        ((CheckBox)fvUserAccount.FindControl("chkIsActive")).Enabled = false;

                    CheckBoxList chkRoles = fvUserAccount.FindControl("chkRoles") as CheckBoxList;
                    chkRoles.DataSource = Roles.GetAllRoles();
                    chkRoles.DataBind();
                    foreach (string role in Roles.GetRolesForUser(selectedUser.UserName))
                    {
                        chkRoles.Items.FindByText(role).Selected = true;
                    }
                    if (WebContext.Parent != null)
                    {
                        List<string> roleList = new List<string>();
                        for (int i = 0; i < chkRoles.Items.Count; i++)
                        {
                            if (chkRoles.Items[i].Text == "Admin" || chkRoles.Items[i].Text == "AdminUser")
                            { }
                            else
                                roleList.Add(chkRoles.Items[i].Text);
                        }
                        foreach (string roles in roleList)
                        {
                            chkRoles.Items.Remove(roles);
                        }
                    }
                    else
                    {
                        foreach (var role in Enum.GetValues(typeof(ClientRoles)))
                        {
                            chkRoles.Items.Remove(new ListItem(role.ToString()));
                        }
                    }

                    CheckBox chkEditIsLockedOut = (CheckBox)fvUserAccount.FindControl("pnlRestrictedItems").FindControl("chkIsLockedOut");
                    if (chkEditIsLockedOut.Checked)
                        chkEditIsLockedOut.Enabled = true;
                    else
                        chkEditIsLockedOut.Enabled = false;
                }
            }
        }

        protected void fvUserAccount_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            fvUserAccount.ChangeMode(e.NewMode);
            BindUserAccount(selectedUser.UserName);
        }

        protected void fvUserAccount_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            using (TransactionScope scope = new TransactionScope())
                try
                {
                    UpdateUser();
                    UpdateRolesForUser();
                    fvUserAccount.ChangeMode(FormViewMode.ReadOnly);
                    BindUserAccount(selectedUser.UserName);
                    scope.Complete();
                }
                catch (Exception)
                {
                    e.Cancel = true;
                }
        }

        protected void fvUserAccount_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            e.KeepInEditMode = false;
        }

        #endregion

        private void BindUserAccount(string userName)
        {
            userList.Add(Membership.GetUser(userName));
            fvUserAccount.DataSource = userList;
            fvUserAccount.DataBind();
        }

        protected string GetRolesForUser(string userName)
        {
            string[] roles = Roles.GetRolesForUser(userName);
            string roleList = string.Join(", ", roles);
            return roleList;
        }

        private void UpdateRolesForUser()
        {
            //first remove the user from all roles...
            string[] currRoles = Roles.GetRolesForUser(selectedUser.UserName);
            if (currRoles.Length > 0)
                Roles.RemoveUserFromRoles(selectedUser.UserName, currRoles);
            //and them add the user to the selected roles
            CheckBoxList chklRoles = fvUserAccount.FindControl("pnlRestrictedItems").FindControl("chkRoles") as CheckBoxList;
            List<string> newRoles = new List<string>();
            foreach (ListItem item in chklRoles.Items)
            {
                if (item.Selected)
                    newRoles.Add(item.Text);
            }
            Roles.AddUserToRoles(selectedUser.UserName, newRoles.ToArray());
        }

        private void UpdateUser()
        {
            selectedUser.Email = ((TextBox)fvUserAccount.FindControl("txtEditEmail")).Text;
            selectedUser.IsApproved = ((CheckBox)fvUserAccount.FindControl("chkIsActive")).Checked;
            if (selectedUser.IsApproved)
            {
                if (selectedUser.Comment == Status.InActiveByDefault.ToString())
                    ActivateUser(selectedUser);
                else
                    selectedUser.Comment = Status.Activated.ToString();
            }
            else
                selectedUser.Comment = Status.InActive.ToString();
            CheckBox chkIsUserLockedOut = (CheckBox)fvUserAccount.FindControl("pnlRestrictedItems").FindControl("chkIsLockedOut");
            if (chkIsUserLockedOut.Enabled & !chkIsUserLockedOut.Checked)
            {
                selectedUser.UnlockUser();
            }

            Membership.UpdateUser(selectedUser);
        }

        private void ActivateUser(MembershipUser selectedUser)
        {

            MembershipUser userInfo = selectedUser;

            // string userPassword = userInfo.GetPassword();
            //get profile with the username
            CustomUserProfile profile = CustomUserProfile.GetUserProfile(userInfo.UserName);

            //provides the link to main website
            string mainUrl = "<span style='margin:0px; padding:10px 0px; display:block;'> Visit us at <a style='font-weight:bold;'  href=" + FullBaseUrl + ">" + FullBaseUrl + "</a></span>";
            userInfo.IsApproved = true;
            userInfo.Comment = Status.Activated.ToString();
            Membership.UpdateUser(userInfo);

            //SEND EMAIL
            string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
            
            string userSubject = "Account Activated";
            string appPath = Request.PhysicalApplicationPath;
            StreamReader userBodySR = new StreamReader(appPath + "EmailTemplates/UserEmailAfterVerification.txt");
            string userBody = userBodySR.ReadToEnd();
            userBodySR.Close();

            string mailSender = "";
            if (WebContext.CurrentUser.IsInRole("Admin") || WebContext.CurrentUser.IsInRole("AdminUser"))
            {
                mailSender = WebContext.Parent.ClientName;
                string urlPath = Server.MapPath("~/Uploads/" + WebContext.Parent.ClientID + "/LogoPicture/" + WebContext.Parent.LogoUrl);
                if (System.IO.File.Exists(urlPath))
                {
                    logoUrl = FullBaseUrl + "Uploads/" + WebContext.Parent.ClientID + "/LogoPicture/" + WebContext.Parent.LogoUrl;
                }
            }
            else if ((WebContext.CurrentUser.IsInRole("SuperAdmin") || WebContext.CurrentUser.IsInRole("SuperAdminUser")) && WebContext.Parent != null)
                mailSender = "SleekSurf / " + WebContext.Parent.ClientName;
            else
                mailSender = "SleekSurf";

            userBody = userBody.Replace("<%Logo%>", logoUrl);
            userBody = userBody.Replace("<%Title%>", profile.Title);
            userBody = userBody.Replace("<%FirstName%>", profile.FirstName);
            userBody = userBody.Replace("<%MiddleName%>", profile.MiddleName);
            userBody = userBody.Replace("<%LastName%>", profile.LastName);
            userBody = userBody.Replace("<%UserName%>", userInfo.UserName);
            userBody = userBody.Replace("<%MainWebsite%>", mainUrl);

            string fullName = "";
            CustomUserProfile currentUserprofile;
            if (string.Compare(WebContext.CurrentUser.Identity.Name, "superadmin", true) == 0)
            {
                currentUserprofile = CustomUserProfile.GetUserProfile(WebContext.CurrentUser.Identity.Name);
                fullName = currentUserprofile.FirstName + " " + currentUserprofile.MiddleName + " " + currentUserprofile.LastName;
            }
            userBody = userBody.Replace("<%SenderName%>", fullName);
            userBody = userBody.Replace("<%SenderCompany%>", mailSender);

            UserInfoPartial registeredUser = new UserInfoPartial() { FirstName = profile.FirstName, MiddleName = profile.MiddleName, LastName = profile.LastName, Email = userInfo.Email };

            Helpers.SendEmail(registeredUser, mailSender, userSubject, userBody);
        }

        private void ChangeSecretQuestionAnswer()
        {
            string secretQuestion = txtSecretQuestion.Text;
            string secretAnswer = txtSecretAnswer.Text;
            string userPassword = selectedUser.GetPassword();
            if (selectedUser.ChangePasswordQuestionAndAnswer(userPassword, secretQuestion, secretAnswer))
            {
                UserInfoPartial userInfo = new UserInfoPartial();
                userInfo.FirstName = ucUserProfile.FirstName;
                userInfo.MiddleName = ucUserProfile.MiddleName;
                userInfo.LastName = ucUserProfile.LastName;
                userInfo.Email = selectedUser.Email;
                string subject = "Secret Question and Answer changed. ";
                string body = "Hello: " + selectedUser.UserName + "(" + userInfo.FirstName + " " + userInfo.MiddleName + " " + userInfo.LastName + ") </ br>Your new secret question and answer are as follows:</ br>";
                body += "Secret Question: " + secretQuestion + "</ br>";
                body += "Secret Answer: " + secretAnswer;
                body += "Please change your Secret Question and answer as soon as you retrieve your password.";

                string mailSender = "";
                if (WebContext.CurrentUser.IsInRole("Admin") || WebContext.CurrentUser.IsInRole("AdminUser"))
                    mailSender = WebContext.Parent.ClientName;
                else
                    mailSender = "SleekSurf";
                Helpers.SendEmail(userInfo, mailSender, subject, body);
                lblMessage.Text = "New Secret question and answer have been sent to user.";
                lblMessage.CssClass = "successMsg";
                txtSecretQuestion.Text = string.Empty;
                txtSecretAnswer.Text = string.Empty;
                CollapsiblePanelExtender1.Collapsed = true;
                CollapsiblePanelExtender1.ClientState = "true";
            }
            else
            {
                lblMessage.Text = "New Secret question and answer could not be updated.";
                lblMessage.CssClass = "errorMsg";
            }
        }

        

        private void BindProfile(string userName)
        {
            //make controls visible for update.
            ucUserProfile.SetEnvironmentForUpdateForAdmin();
            //bind user details
            MembershipUser selectedUser = Membership.GetUser(userName);
            userList.Add(selectedUser);
            fvUserAccount.DataSource = userList;
            fvUserAccount.DataBind();
            //bind profile
            CustomUserProfile profile = CustomUserProfile.GetUserProfile(userName);
            profile.ProfileUrl = WebContext.TempInfo.ProfileImageName;//there should be a image holder if the picture exists
            //save profile ends
            ucUserProfile.Title = profile.Title;
            ucUserProfile.FirstName = profile.FirstName;
            ucUserProfile.MiddleName = profile.MiddleName;
            ucUserProfile.LastName = profile.LastName;
            ucUserProfile.DOB = profile.DOB;
            ucUserProfile.Gender = profile.Gender;
            ucUserProfile.Occupation = profile.Occupation;
            ucUserProfile.WebSiteUrl = profile.WebSiteUrl;
            profile.UpdatedBy = (Guid?)Membership.GetUser(WebContext.CurrentUser.Identity.Name).ProviderUserKey;
            //address information
            ucUserProfile.AddressLine1 = profile.Address.AddressLine1;
            ucUserProfile.AddressLine2 = profile.Address.AddressLine2;
            ucUserProfile.AddressLine3 = profile.Address.AddressLine3;
            ucUserProfile.City = profile.Address.City;
            ucUserProfile.State = profile.Address.State;
            ucUserProfile.PostCode = profile.Address.PostCode;
            ucUserProfile.Country = profile.Address.Country;
            //selected value of the country.
            //contact information
            ucUserProfile.ContactHome = profile.Contacts.ContactHome;
            ucUserProfile.ContactMobile = profile.Contacts.ContactMobile;
            //preferences
            ucUserProfile.Theme = profile.Preferences.Theme;
        }

        public bool UpdateProfile()
        {
            string UserName = "";
            if (WebContext.SelectedUser != null)
                UserName = ((MembershipUser)WebContext.SelectedUser).UserName;
            else
                UserName = WebContext.CurrentUser.Identity.Name;
            CustomUserProfile profile = CustomUserProfile.GetUserProfile(UserName);

            //update profile
            profile.Title = ucUserProfile.Title;
            profile.FirstName = ucUserProfile.FirstName;
            profile.MiddleName = ucUserProfile.MiddleName;
            profile.LastName = ucUserProfile.LastName;
            profile.DOB = (DateTime?)ucUserProfile.DOB;
            profile.Gender = ucUserProfile.Gender;
            profile.Occupation = ucUserProfile.Occupation;
            profile.WebSiteUrl = ucUserProfile.WebSiteUrl;
            profile.CreatedDate = System.DateTime.Now;
            profile.IPAddress = Helpers.CurrentUserIP;
            MembershipUser user = Membership.GetUser(UserName);
            profile.CreatedBy = (Guid?)user.ProviderUserKey;
            profile.UpdatedBy = (Guid?)user.ProviderUserKey;
            //address information
            profile.Address.AddressLine1 = ucUserProfile.AddressLine1;
            profile.Address.AddressLine2 = ucUserProfile.AddressLine2;
            profile.Address.AddressLine2 = ucUserProfile.AddressLine3;
            profile.Address.AddressLine3 = ucUserProfile.AddressLine3;
            profile.Address.City = ucUserProfile.City;
            profile.Address.State = ucUserProfile.State;
            profile.Address.PostCode = ucUserProfile.PostCode;
            profile.Address.Country = ucUserProfile.Country;//selected value of the country.
            //contact information
            profile.Contacts.ContactHome = ucUserProfile.ContactHome;
            profile.Contacts.ContactMobile = ucUserProfile.ContactMobile;
            //preferences
            profile.Preferences.Theme = ucUserProfile.Theme;

            try
            {
                if (ucUserProfile.ProfileImageControl.HasFile)
                {
                    string dirUrlP = BasePage.BaseUrl + "Uploads/" + UserName + "/ProfilePicture";
                    if (WebContext.Parent != null)
                        dirUrlP = BasePage.BaseUrl + "Uploads/" + WebContext.Parent.ClientID + "/" + UserName + "/ProfilePicture";
                    //deleted the old profile photo if it exists
                    string dirPathP = Server.MapPath(dirUrlP);
                    if (File.Exists(dirPathP + "/" + profile.ProfileUrl))
                        File.Delete(dirPathP + "/" + profile.ProfileUrl);
                    //create directory and saves new file
                    if (!Directory.Exists(dirPathP))
                        Directory.CreateDirectory(dirPathP);

                    profile.ProfileUrl = DateTime.Now.ToString("P-ddMMyyyy-HHmmss") + ".jpg";
                    string outputfileName = dirPathP + "/" + profile.ProfileUrl;
                    int profileImageSize = 150;
                    ucUserProfile.ProfileImageControl.SaveAs(outputfileName);
                    Helpers.ResizeImage(outputfileName, outputfileName, profileImageSize);
                }
                profile.Save();
                lblMessage.CssClass = "successMsg";
                lblMessage.Text = "Profile updated successfully.";
                return true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error. " + ex.Message;
                lblMessage.CssClass = "errorMsg";
                return false;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Default.aspx");
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            if (IsValid)
                UpdateProfile();
        }

        protected void btnSaveQAndA_Click(object sender, EventArgs e)
        {
            if (IsValid)
                ChangeSecretQuestionAnswer();
        }
    }
}