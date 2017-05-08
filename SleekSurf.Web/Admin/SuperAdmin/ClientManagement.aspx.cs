using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using SleekSurf.Entity;
using SleekSurf.Manager;
using System.Transactions;
using System.Web.Security;
using System.IO;

namespace SleekSurf.Web.Admin.SuperAdmin
{
    public partial class ClientManagement : BasePage
    {
        static PagingDetails pgObj = null;
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("ClientMgmt"))].Selected = true;

            if (!IsPostBack)
            {
                pgObj = new PagingDetails();
                pgObj.SearchMode = "DEFAULT";
                SelectAllClients();
            }

            if (User.IsInRole("MarketingOfficer"))
                gvClientManagement.Columns[0].Visible = false;
        }

        protected void gvClientManagement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            WebContext.Parent = ClientManager.SelectClient(gvClientManagement.DataKeys[e.NewSelectedIndex]["ClientID"].ToString()).EntityList[0];
            Redirector.GoToRequestedPage("~/Admin/Client/AccountManagement.aspx");
        }

        protected void gvClientManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                ((CheckBox)e.Row.FindControl("cbDelete")).Attributes.Add("onclick", "javascript:UpdateSelectAllAndDeleteControl('" + gvClientManagement.ClientID + "', 'cbDelete', 'checkAll', 'imgDeleteBtn');");
            else if (e.Row.RowType == DataControlRowType.Footer)
                ((ImageButton)e.Row.FindControl("imgDeleteBtn")).OnClientClick = "return DeleteConfirmation('" + gvClientManagement.ClientID + "', 'cbDelete');";
        }

        protected void gvClientManagement_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvClientManagement.EditIndex = e.NewEditIndex;
            SelectAllClients();
        }

        protected void gvClientManagement_RowCancellingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvClientManagement.EditIndex = -1;
            SelectAllClients();
        }

        protected void gvClientManagement_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string comment = "";
            string clientID = gvClientManagement.DataKeys[e.RowIndex]["ClientID"].ToString();
            ClientDetails client = ClientManager.SelectClient(clientID).EntityList[0];
            CheckBox chkApproved = (CheckBox)gvClientManagement.Rows[e.RowIndex].FindControl("chkPublished");

            if (chkApproved.Checked != client.Published)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        if (chkApproved.Checked)
                        {
                            if (client.Comment == Status.InActiveByDefault.ToString())
                            {
                                string userName = Membership.GetUser(client.ContactPerson).UserName;
                                ActivateUser(userName);
                            }
                            
                            comment = Status.Activated.ToString();
                            ClientManager.PublishClient(clientID, comment);
                        }
                        else
                        {
                            comment = Status.InActiveBySuperAdmin.ToString();
                            ClientManager.UnPublishClient(clientID, comment);
                        }

                        List<Guid> userList = ClientManager.SelectUsers(client.ClientID);
                        MembershipUser user = null;

                        if (client.Comment == Status.InActiveByDeletion.ToString())
                        {
                            foreach (Guid id in userList)
                            {
                                 user = Membership.GetUser(id);
                                 if (user.Comment == Status.InActiveByDeletion.ToString())
                                 {
                                     user.IsApproved = chkApproved.Checked;
                                     user.Comment = Status.Activated.ToString();
                                     Membership.UpdateUser(user);
                                 }
                            }

                        }
                        else
                        {
                            foreach (Guid id in userList)
                            {
                                user = Membership.GetUser(id);
                                if (user.Comment == Status.Activated.ToString())
                                {
                                    //allowing just the contact person to remain active for client maintenance (like renew, etc)
                                    if ((Guid)user.ProviderUserKey != client.ContactPerson)
                                    {
                                        user.IsApproved = chkApproved.Checked;
                                        user.Comment = Status.InActiveBySuperAdminForClient.ToString();
                                    }
                                }
                                else if (user.Comment == Status.InActiveBySuperAdminForClient.ToString())
                                {
                                    user.IsApproved = chkApproved.Checked;
                                    user.Comment = Status.Activated.ToString();
                                }
                                
                                Membership.UpdateUser(user);
                            }

                           
                        }
                        scope.Complete();
                    }//end of try
                    catch(Exception ex)
                    {
                        Helpers.LogError(ex);
                        throw;
                    }

                }//end of transaction scope

            }//// end of top if statement
            gvClientManagement.EditIndex = -1;
            SelectAllClients();
        }

        protected void imgDeleteBtn_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> clientList = new Dictionary<string, string>();
            foreach (GridViewRow row in gvClientManagement.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                        clientList.Add(gvClientManagement.DataKeys[row.RowIndex].Value.ToString(), gvClientManagement.DataKeys[row.RowIndex]["Comment"].ToString());
                }

            }

            //Call the method to Delete records 
            foreach (KeyValuePair<string, string> client in clientList)
            {
                if (client.Value != Status.InActiveByDeletion.ToString())
                {
                    ClientManager.UnPublishClient(client.Key, Status.InActiveByDeletion.ToString());
                    List<Guid> userIDList = ClientManager.SelectUsers(client.Key);
                    foreach (Guid id in userIDList)
                    {
                        MembershipUser userTemp = Membership.GetUser(id);
                        if (userTemp.Comment == Status.Activated.ToString())
                        {
                            userTemp.IsApproved = false;
                            userTemp.Comment = Status.InActiveByDeletion.ToString();
                        }

                        Membership.UpdateUser(userTemp);
                    }
                }
                else
                {
                    int countDelete = 0;

                    try
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            List<Guid> userIDList = ClientManager.SelectUsers(client.Key);

                            foreach (Guid userID in userIDList)
                            {
                                MembershipUser userTemp = Membership.GetUser(userID);
                                Membership.DeleteUser(userTemp.UserName, true);
                            }

                            if (ClientManager.DeleteClient(client.Key))
                            {

                                string dirUrl = BasePage.BaseUrl + "Uploads/" + client.Key;
                                string dirPath = Server.MapPath(dirUrl);
                                if (Directory.Exists(dirPath))
                                    Directory.Delete(dirPath, true);
                                countDelete++;
                            }

                            else if (countDelete > 0)
                            {
                                lblMessage.CssClass = "successMsg";
                                lblMessage.Text = countDelete + " client(s) have been permanently deleted.";
                            }
                            else
                            {
                                lblMessage.CssClass = "errorMsg";
                                lblMessage.Text = "No client(s) have not been deleted because of some technical issues. Please review.";
                            }

                            scope.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.CssClass = "errorMsg";
                        lblMessage.Text = "Problem deleting clients. Please review.";
                        Helpers.LogError(ex);
                    }
                }
            }

            if (pgObj == null)//pgObj becomes null when the directory and files are deleted from the server.
                pgObj = new PagingDetails();

            SelectAllClients();

        }


        private void SelectAllClients()
        {
            if (pgObj.StartRowIndex == 0)
                pgObj.StartRowIndex = 1;
            pgObj.PageSize = Globals.Settings.Clients.PageSize;
            Result<ClientDetails> result = new Result<ClientDetails>();
            switch (pgObj.SearchMode)
            {
                case "DEFAULT":
                    if (!WebContext.CurrentUser.IsInRole("MarketingOfficer"))
                        result = ClientManager.SelectAllClient(pgObj);
                    else
                        result = ClientManager.SelectClientsByCreatedPerson((Guid)Membership.GetUser(WebContext.CurrentUser.Identity.Name).ProviderUserKey, pgObj);
                    break;
                case "KEYWORD":
                    if (!WebContext.CurrentUser.IsInRole("MarketingOfficer"))
                        result = ClientManager.SelectAllClient(pgObj.SearchKey, pgObj);
                    else
                        result = ClientManager.SelectAllClient((Guid)Membership.GetUser(WebContext.CurrentUser.Identity.Name).ProviderUserKey, pgObj.SearchKey, pgObj);
                    break;
            }
            gvClientManagement.DataSource = result.EntityList;
            gvClientManagement.DataBind();
            SetupPaging();

        }

        protected string ProfileImageSource(string clientID, string logoUrl)
        {
            return FullBaseUrl + "Uploads/" + clientID + "/LogoPicture/" + logoUrl;
        }

        protected string GetContactPerson(string UserID)
        {
            Guid Userid = Guid.Parse(UserID);
            CustomUserProfile profile = CustomUserProfile.GetUserProfile(Membership.GetUser(Userid).UserName);
            return profile.FirstName + " " + profile.MiddleName + " " + profile.LastName;
        }

        protected string GetCategory(CategoryDetails category)
        {
            return ClientManager.GetCategory(category.CategoryID).EntityList[0].CategoryName;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtClientName.Text.Trim() != txtClientName.ToolTip)
            {
                pgObj.SearchMode = "KEYWORD";
                pgObj.SearchKey = txtClientName.Text.Trim();
                SelectAllClients();
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

                    string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
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

        #region  custom pager section of gridview

        public struct PageUrl
        {
            private string page;
            private string url;
            public string Page
            {
                get { return page; }
            }
            public string Url
            {
                get { return url; }
            }
            public PageUrl(string page, string url)
            {
                this.page = page;
                this.url = url;
            }
        }


        protected void rptPager_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            int prevPageIndex = 0;
            foreach (RepeaterItem item in rptPager.Items)
            {
                LinkButton btnPager = (LinkButton)item.FindControl("lbtnPagerButton");
                if (btnPager.Enabled == false)
                {
                    prevPageIndex = int.Parse(btnPager.CommandName);
                    break;
                }
            }
            pgObj.StartRowIndex = int.Parse(e.CommandName);
            SearchByPageButtons(prevPageIndex - 1, pgObj.StartRowIndex - 1);
            SelectAllClients();
        }

        // This method will handle the navigation/ paging index
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            int prevPageIndex = 0;
            switch (e.CommandName)
            {
                case "Previous":
                    prevPageIndex = Int32.Parse(lblStartPage.Text);
                    pgObj.StartRowIndex = prevPageIndex - 1;
                    break;

                case "Next":
                    prevPageIndex = Int32.Parse(lblStartPage.Text);
                    pgObj.StartRowIndex = prevPageIndex + 1;
                    break;
            }
            SearchByPageButtons(prevPageIndex - 1, pgObj.StartRowIndex - 1);
            SelectAllClients();
        }

        private void SetupPaging()
        {
            if (gvClientManagement.Rows.Count > 0)
            {
                pnlgvPersonNavigatorTop.Visible = true;
                if (pgObj.PageSize < pgObj.TotalNumber)
                    pnlNavigatorBottom.Visible = true;
                else
                    pnlNavigatorBottom.Visible = false;

                lblStartPage.Text = pgObj.StartRowIndex.ToString();
                int totalPages = Helpers.GetTotalPages(pgObj.TotalNumber, pgObj.PageSize);
                lblTotalPages.Text = Helpers.GetTotalPages(pgObj.TotalNumber, pgObj.PageSize).ToString();
                lblTotalNo.Text = pgObj.TotalNumber.ToString();
                if (rptPager.Items.Count != totalPages)
                {
                    //list the pages and their url as an array
                    PageUrl[] pages = new PageUrl[totalPages];
                    //generate pages url elements
                    pages[0] = new PageUrl("1", "");
                    for (int i = 2; i <= totalPages; i++)
                    {
                        pages[i - 1] = new PageUrl(i.ToString(), "");
                    }
                    //don't generate the link for current page
                    pages[pgObj.StartRowIndex - 1] = new PageUrl((pgObj.StartRowIndex.ToString()), "");
                    //feeds the pages to the repeater
                    rptPager.DataSource = pages;
                    rptPager.DataBind();

                    LinkButton btnPager = (LinkButton)rptPager.Items[pgObj.StartRowIndex - 1].FindControl("lbtnPagerButton");
                    btnPager.CssClass = "currentPage";
                    btnPager.Enabled = false;
                }

                if (int.Parse(lblStartPage.Text) == 1)
                {
                    lbtnPrevious.Enabled = false;
                    lbtnNext.Enabled = true;
                }
                else if (int.Parse(lblStartPage.Text) == totalPages)
                {
                    lbtnNext.Enabled = false;
                    lbtnPrevious.Enabled = true;
                    if (totalPages == 1)
                    {
                        lbtnPrevious.Enabled = false;
                    }
                }
                else
                {
                    lbtnPrevious.Enabled = true;
                    lbtnNext.Enabled = true;
                    //lblDDText.Visible = true;
                }
            }
            else
            {
                pnlgvPersonNavigatorTop.Visible = false;
                pnlNavigatorBottom.Visible = false;//hides the navigation section of gridview.
            }
        }

        private void SearchByPageButtons(int prevPageIndex, int currentPageIndex)
        {
            LinkButton currentButton = (LinkButton)rptPager.Items[currentPageIndex].FindControl("lbtnPagerButton");
            currentButton.CssClass = "currentPage";
            currentButton.Enabled = false;
            LinkButton previousButton = (LinkButton)rptPager.Items[prevPageIndex].FindControl("lbtnPagerButton");
            previousButton.CssClass = "paginationLinkButton";
            previousButton.Enabled = true;
        }

        #endregion

    }
}