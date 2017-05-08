using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using SleekSurf.Entity;
using System.Web.Security;
using SleekSurf.Manager;
using System.IO;
using System.Transactions;

namespace SleekSurf.Web.Admin.SuperAdmin
{
    public partial class UserManagement : BasePage
    {
        static PagingDetails pgobj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("UserMgmt"))].Selected = true;

            if (!IsPostBack)
            {
                pgobj = new PagingDetails();
                pgobj.SearchMode = "DEFAULT";
                LoadAllUsers();
            }
        }

        protected void gvUserManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((CheckBox)e.Row.FindControl("cbDelete")).InputAttributes["class"] = "styled";
                ((CheckBox)e.Row.FindControl("chkIsActive")).InputAttributes["class"] = "styled";
                ((CheckBox)e.Row.FindControl("chkIsLockedOut")).InputAttributes["class"] = "styled";

                ((CheckBox)e.Row.FindControl("cbDelete")).Attributes.Add("onclick", "javascript:UpdateSelectAllAndDeleteControl('" + gvUserManagement.ClientID + "', 'cbDelete', 'checkAll', 'imgDeleteBtn');");

                Guid userID = Guid.Parse(gvUserManagement.DataKeys[e.Row.RowIndex].Values["ProviderUserKey"].ToString());
                string[] roles = Roles.GetRolesForUser(Membership.GetUser(userID).UserName);
                Label lblRoles = (Label)e.Row.FindControl("lblRoles");
                if (lblRoles != null)
                {
                    lblRoles.Text = string.Join(", ", roles);
                }
                if (gvUserManagement.EditIndex != -1)
                {
                    CheckBox chkEditIsLockedOut = (CheckBox)e.Row.FindControl("chkIsLockedOut");
                    if (chkEditIsLockedOut.Checked)
                        chkEditIsLockedOut.Enabled = true;
                    else
                        chkEditIsLockedOut.Enabled = false;
                }

                if (string.Compare(gvUserManagement.DataKeys[e.Row.RowIndex]["UserName"].ToString(), WebContext.CurrentUser.Identity.Name, true) == 0)
                {
                    ((CheckBox)e.Row.FindControl("chkIsActive")).Enabled = false;
                    ((CheckBox)e.Row.FindControl("cbDelete")).Enabled = false;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((ImageButton)e.Row.FindControl("imgDeleteBtn")).OnClientClick = "return DeleteConfirmation('" + gvUserManagement.ClientID + "', 'cbDelete');";
            }
        }

        private void LoadAllUsers()
        {
            List<Guid> userIDList = new List<Guid>();
            userIDList = ClientManager.SelectUsersWithOutClient();
            List<MembershipUser> userList = new List<MembershipUser>();

            if (pgobj == null)
                pgobj = new PagingDetails();

            switch (pgobj.SearchMode)
            {
                case "DEFAULT":
                    foreach (Guid id in userIDList)
                    {
                        MembershipUser tempUser = Membership.GetUser(id);
                        if (tempUser != null)
                        {
                            userList.Add(tempUser);
                        }
                    }
                    break;
                case "KEYWORD":
                    MembershipUser thisUser = Membership.GetUser(pgobj.SearchKey);
                    if (thisUser != null)
                    {
                        foreach (Guid id in userIDList)
                        {
                            if ((Guid)thisUser.ProviderUserKey == id)
                                userList.Add(Membership.GetUser(id));
                        }
                    }
                    break;
            }

            gvUserManagement.DataSource = userList;
            gvUserManagement.DataBind();
        }

        protected void imgDeleteBtn_Click(object sender, EventArgs e)
        {
            string tempMessage = string.Empty;

            List<Guid> userList = new List<Guid>();
            Guid userID = new Guid();
            foreach (GridViewRow row in gvUserManagement.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        userID = Guid.Parse(gvUserManagement.DataKeys[row.RowIndex].Value.ToString());
                        if (userID != Guid.Parse(Membership.GetUser(WebContext.CurrentUser.Identity.Name).ProviderUserKey.ToString()))
                            userList.Add(userID);
                        else
                        {
                            tempMessage = "Can't delete your account by yourself. Please contact senior or equivalent level admin.";
                            return;
                        }
                    }

                }

            }

            //Call the method to Delete records 
            foreach (Guid id in userList)
            {

                MembershipUser user = Membership.GetUser(id);

                if (user.Comment != Status.InActiveByDeletion.ToString())
                {
                    user.Comment = Status.InActiveByDeletion.ToString();
                    user.IsApproved = false;
                    Membership.UpdateUser(user);
                }
                else
                {
                    try
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            Membership.DeleteUser(user.UserName);

                            string dirUrl = BasePage.BaseUrl + "Uploads/" + user.UserName;
                            string dirPath = Server.MapPath(dirUrl);
                            if (Directory.Exists(dirPath))
                                Directory.Delete(dirPath, true);

                            scope.Complete();
                        }

                    }
                    catch (Exception ex)
                    {
                        Helpers.LogError(ex);
                    }
                }
            }

            if (userList.Count > 0)
            {
                lblMessage.CssClass = "successMsg";
                lblMessage.Text = userList.Count + " records have been affected.<br/>";
                if (!string.IsNullOrEmpty(tempMessage))
                    lblMessage.Text += tempMessage;
            }
            else
            {
                lblMessage.CssClass = "normalMsg";
                lblMessage.Text = userList.Count + " records have been deleted.";
            }

            // rebind the GridView
            LoadAllUsers();
        }

        public string FullName(string username)
        {
            CustomUserProfile profile = CustomUserProfile.GetUserProfile(username);
            return profile.FirstName + " " + profile.MiddleName + " " + profile.LastName;
        }

        public string ProfileImageSource(string username)
        {
            CustomUserProfile profile = CustomUserProfile.GetUserProfile(username);
            string fileName = profile.ProfileUrl;

            if (string.IsNullOrWhiteSpace(fileName))
            {
                if(string.Compare(profile.Gender, "female", true) == 0)
                    return "~/App_Themes/SleekTheme/Images/ProfileFemale.png";
                else
                    return "~/App_Themes/SleekTheme/Images/ProfileMale.png";
            }

            string avatarUrl = "~/Uploads/" + username + "/ProfilePicture/" + fileName;

            if (!File.Exists(Server.MapPath(avatarUrl)))
            {
                if (string.Compare(profile.Gender, "female", true) == 0)
                    return "~/App_Themes/SleekTheme/Images/ProfileFemale.png";
                else
                    return "~/App_Themes/SleekTheme/Images/ProfileMale.png";
            }

            return avatarUrl;
        }

        protected void gvUserManagement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {

            Guid userID = Guid.Parse(gvUserManagement.DataKeys[e.NewSelectedIndex].Values[0].ToString());
            WebContext.SelectedUser = Membership.GetUser(userID);
            Redirector.GoToRequestedPage("~/Admin/AdminEditProfile.aspx?EditUser=True");
        }

        protected void gvUserManagement_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUserManagement.EditIndex = e.NewEditIndex;
            ViewState["Index"] = e.NewEditIndex;
            LoadAllUsers();
        }

        protected void txtEmail_TextChanged(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(ViewState["Index"].ToString());
            CompareValidator cmpCompareEmail = (CompareValidator)gvUserManagement.Rows[index].FindControl("cmpCompareEmail");
            AjaxControlToolkit.ValidatorCalloutExtender tempVCE = (AjaxControlToolkit.ValidatorCalloutExtender)gvUserManagement.Rows[index].FindControl("ValidatorCalloutExtender1");
            MembershipUser thisUser = Membership.GetUser(gvUserManagement.DataKeys[index]["UserName"].ToString());
            TextBox txtEmail = (TextBox)gvUserManagement.Rows[index].FindControl("txtEmail");
            string email = txtEmail.Text;

            tempVCE.Enabled = true;
            if (CheckEmail(thisUser, email))
            {
                cmpCompareEmail.ValueToCompare = email;

            }
            else
            {
                cmpCompareEmail.ValueToCompare = "Already exists or invalid";
            }
        }

        protected void gvUserManagament_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                MembershipUser thisUser = Membership.GetUser(gvUserManagement.DataKeys[e.RowIndex]["UserName"].ToString());
                UpdateUser(thisUser, e.RowIndex);
                gvUserManagement.EditIndex = -1;
                LoadAllUsers();
            }
        }

        protected void gvUserManagement_RowCancellingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUserManagement.EditIndex = -1;
            LoadAllUsers();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() != txtUsername.ToolTip)
            {
                pgobj.SearchMode = "KEYWORD";
                pgobj.SearchKey = txtUsername.Text.Trim();
                LoadAllUsers();
            }
        }

        protected void gvUserManagement_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUserManagement.PageIndex = e.NewPageIndex;
            LoadAllUsers();
        }

        private void UpdateUser(MembershipUser selectedUser, int index)
        {
            selectedUser.Email = ((TextBox)gvUserManagement.Rows[index].FindControl("txtEmail")).Text;
            selectedUser.IsApproved = ((CheckBox)gvUserManagement.Rows[index].FindControl("chkIsActive")).Checked;
            if (selectedUser.IsApproved)
            {
                if (selectedUser.Comment == Status.InActiveByDefault.ToString())
                    ActivateUser(selectedUser);
                else
                    selectedUser.Comment = Status.Activated.ToString();
            }
            else
            {
                if (selectedUser.Comment == Status.Activated.ToString())
                    selectedUser.Comment = Status.InActive.ToString();
            }

            CheckBox chkEditIsLockedOut = (CheckBox)gvUserManagement.Rows[index].FindControl("chkIsLockedOut");
            if (chkEditIsLockedOut.Enabled & !chkEditIsLockedOut.Checked)
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
            string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
            string userSubject = "Account Activated";
            string appPath = Request.PhysicalApplicationPath;
            StreamReader userBodySR = new StreamReader(appPath + "EmailTemplates/UserEmailAfterVerification.txt");
            string userBody = userBodySR.ReadToEnd();
            userBodySR.Close();

            userBody = userBody.Replace("<%Logo%>", logoUrl);
            userBody = userBody.Replace("<%TopBackGround%>", topBackGroundUrl);
            userBody = userBody.Replace("<%Title%>", profile.Title);
            userBody = userBody.Replace("<%FirstName%>", profile.FirstName);
            userBody = userBody.Replace("<%MiddleName%>", profile.MiddleName);
            userBody = userBody.Replace("<%LastName%>", profile.LastName);
            userBody = userBody.Replace("<%UserName%>", userInfo.UserName);
            userBody = userBody.Replace("<%MainWebsite%>", mainUrl);

            UserInfoPartial registeredUser = new UserInfoPartial() { FirstName = profile.FirstName, MiddleName = profile.MiddleName, LastName = profile.LastName, Email = userInfo.Email };
            Helpers.SendEmail(registeredUser, "SleekSurf", userSubject, userBody);
        }

        private bool CheckEmail(MembershipUser selectedUser, string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            MembershipUserCollection userList = Membership.FindUsersByEmail(email);
            if (userList.Count == 0)
                return true;
            else if (userList.Count > 0)
            {
                MembershipUser tempUser = null;
                foreach (MembershipUser user in userList)
                {
                    tempUser = user;
                }
                if (tempUser.UserName == selectedUser.UserName)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

    }
}