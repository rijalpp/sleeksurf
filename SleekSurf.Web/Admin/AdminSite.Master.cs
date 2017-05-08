using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel.Composition;
using SleekSurf.FrameWork.Interfaces;
using SleekSurf.FrameWork;
using System.Web.Security;
using SleekSurf.Manager;
using SleekSurf.Entity;
using System.Transactions;
using System.IO;

namespace SleekSurf.Web.Admin
{
    public partial class AdminSite : System.Web.UI.MasterPage
    {
        [Import]
        private INavigation nav { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            nav = new Navigation();

            WebContext.CurrentUser = HttpContext.Current.User;
            nav.CheckAccessForCurrentNode();
            if (nav.CurrentNode["pageTitle"] != null && !String.IsNullOrEmpty(nav.CurrentNode["pageTitle"]))
                Page.Title = nav.CurrentNode["pageTitle"].ToString();
            else
                Page.Title = nav.CurrentNode.Title + " - Admin";

            if (HttpContext.Current.User.IsInRole("SuperAdmin") || HttpContext.Current.User.IsInRole("SuperAdminUser") || HttpContext.Current.User.IsInRole("MarketingOfficer"))
            {
                WebContext.Sibling = WebContext.CurrentUser;
                if (WebContext.CurrentUser.IsInRole("MarketingOfficer"))
                    liPrivacyPolicy.Visible = liTermsAndConditions.Visible = false;
            }

            if (HttpContext.Current.User.IsInRole("Admin") || HttpContext.Current.User.IsInRole("AdminUser"))
            {
                MembershipUser user = Membership.GetUser(WebContext.CurrentUser.Identity.Name);
                string clientID = ClientManager.SelectProfileClientID((Guid)user.ProviderUserKey);
                ClientDetails client = ClientManager.SelectClient(clientID).EntityList[0];
                WebContext.Parent = client;

                if (!WebContext.Parent.Published)
                    Redirector.GoToAdminAccessSuspendedPage();

                ClientFeatureDetails clientFeature = ClientManager.SelectClientFeatureDetails(WebContext.Parent.ClientID).EntityList[0];

                if (clientFeature.Listing && !clientFeature.ClientDomain && !clientFeature.ClientProfile)
                {
                    liPrivacyPolicy.Visible = liTermsAndConditions.Visible = false;
                    if (!(Enum.GetNames(typeof(ListingAccessOnly)).Contains(nav.CurrentNode.Title)))
                        Redirector.GoToAdminAccessDeniedPage();
                }
                else
                {
                    string tempMessage = string.Empty;
                    bool displayMessage = false;
                    if (clientFeature.ClientProfile && string.IsNullOrEmpty(WebContext.Parent.UniqueIdentity))
                    {
                        tempMessage += "<span style=\"font: 18px Arial, Verdana, Helvetica, sans-serif; color: #15ADFF; text-align: left; padding-left: 20px; display: block; margin: 5px 0px\"> Unique URL ID </span>";
                        displayMessage = true;
                    }
                    if (clientFeature.ClientDomain && string.IsNullOrEmpty(WebContext.Parent.UniqueDomain))
                    {
                        tempMessage += "<span style=\"font: 18px Arial, Verdana, Helvetica, sans-serif; color: #15ADFF; text-align: left; padding-left: 20px; display: block; margin: 5px 0px\"> Unique Domain </span>";
                        displayMessage = true;
                    }

                    if (Session["ShowEntryMessage"] == null && displayMessage)
                    {
                        string jScript = "$(document).ready(function () { window.parent.jQuery.fancybox('<p class=\"locationDescription\">Our record shows that some of your profile detail(s) are missing, please update following detail(s) to utilize the full features of your package(s).<div style=\"margin: 10px 0px;\">" + tempMessage + "</div><p class=\"locationDescription\" style=\"text-align:right; padding-right: 10px;\"><a href=\"/Admin/Client/ViewEditBusinessProfile.aspx\" style=\"color: #15ADFF;\">Click here</a> to update profile.</p></p>', {'autoDimensions': false, 'width':600, 'height':220, 'padding': 10, 'modal': false, 'scrolling': 'no', 'centerOnScroll': true}); });";
                        Session.Add("ShowEntryMessage", jScript);
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "StartUpMessage", jScript, true);
                    }
                }

                Result<PackageOrderDetails> tempPackageOrderDetails = ClientPackageManager.SelectRecentDistinctOrdersByClientID(WebContext.Parent.ClientID);
                if (tempPackageOrderDetails.EntityList.Count > 0)
                {
                    int countExpiredOrders = 0;
                    ltrPackageCount.Text = tempPackageOrderDetails.EntityList.Count.ToString();
                    rptrPackages.DataSource = tempPackageOrderDetails.EntityList;
                    rptrPackages.DataBind();

                    foreach (PackageOrderDetails detail in tempPackageOrderDetails.EntityList)
                    {
                        if (detail.ExpiryDate < System.DateTime.Now)
                        {
                            PackageDetails package = ClientPackageManager.SelectPackage(detail.PackageCode).EntityList[0];
                            ClientManager.ClientFeatureSetStatus(WebContext.Parent.ClientID, package.FeatureType, false);
                            countExpiredOrders++;
                        }
                        else if (detail.OrderStatus == StatusOrder.Verified.ToString())
                        {
                            if (WebContext.Parent.Comment == Status.InActiveByAccountExpiration.ToString())
                            {

                                if (WebContext.Parent != null && !WebContext.Parent.Published)
                                {
                                    using (TransactionScope scope = new TransactionScope())
                                    {
                                        try
                                        {
                                            WebContext.Parent.Comment = Status.Activated.ToString();
                                            ClientManager.PublishClient(clientID, WebContext.Parent.Comment);
                                            WebContext.Parent = ClientManager.SelectClient(clientID).EntityList[0];
                                            //
                                            List<Guid> userList = ClientManager.SelectUsers(WebContext.Parent.ClientID);
                                            foreach (Guid id in userList)
                                            {
                                                MembershipUser userTemp = Membership.GetUser(id);
                                                if (userTemp.Comment == Status.InActiveByAccountExpiration.ToString())
                                                {
                                                    userTemp.IsApproved = true;
                                                    userTemp.Comment = Status.Activated.ToString();
                                                }
                                                Membership.UpdateUser(userTemp);

                                            }
                                            scope.Complete();
                                        }//end of try
                                        catch (Exception ex)
                                        {
                                            Helpers.LogError(ex);
                                            throw;
                                        }

                                    }//end of transaction scope
                                }
                            }
                            if (detail.PackageCode != "SMS")
                            {
                                ClientFeatureDetails featureDetails = ClientManager.SelectClientFeatureDetails(WebContext.Parent.ClientID).EntityList[0];
                                switch (ClientPackageManager.SelectPackage(detail.PackageCode).EntityList[0].FeatureType)
                                {
                                    case "Listing":
                                        if (!featureDetails.Listing)
                                            ClientManager.ClientFeatureSetListingStatus(WebContext.Parent.ClientID, true);
                                        break;
                                    case "ClientProfile":
                                        if (!featureDetails.ClientProfile)
                                            ClientManager.ClientFeatureSetClientProfileStatus(WebContext.Parent.ClientID, true);
                                        break;
                                    case "ClientDomain":
                                        if (!featureDetails.ClientDomain)
                                            ClientManager.ClientFeatureSetClientDomainStatus(WebContext.Parent.ClientID, true);
                                        break;
                                }
                            }
                        }
                    }

                    if (countExpiredOrders == tempPackageOrderDetails.EntityList.Count)
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            try
                            {
                                WebContext.Parent.Comment = Status.InActiveByAccountExpiration.ToString();
                                ClientManager.UnPublishClient(WebContext.Parent.ClientID, WebContext.Parent.Comment);
                                List<Guid> userList = ClientManager.SelectUsers(WebContext.Parent.ClientID);
                                foreach (Guid id in userList)
                                {
                                    MembershipUser userTemp = Membership.GetUser(id);
                                    if (userTemp.Comment == Status.Activated.ToString())
                                    {
                                        //allowing just the contact person to remain active for client maintenance (like renew, etc)
                                        if ((Guid)userTemp.ProviderUserKey != WebContext.Parent.ContactPerson)
                                        {
                                            userTemp.IsApproved = false;
                                            userTemp.Comment = Status.InActiveByAccountExpiration.ToString();
                                        }
                                    }

                                    Membership.UpdateUser(userTemp);
                                }
                                scope.Complete();
                            }//end of try
                            catch (Exception ex)
                            {
                                Helpers.LogError(ex);
                                throw;
                            }

                        }//end of transaction scope

                        Redirector.GoToAdminAccessSuspendedPage();
                    }
                }
                else
                {
                    WebContext.Parent.Comment = Status.InActiveByDefault.ToString();
                    ClientManager.UnPublishClient(WebContext.Parent.ClientID, WebContext.Parent.Comment);
                    Redirector.GoToAdminAccessSuspendedPage();
                }
                accountSummaryPanelForClient.Visible = true;
            }
            if (string.Compare(WebContext.CurrentUser.Identity.Name, "superadmin", true)==0)
            {
                ((HyperLink)lvBackEndMasterPage.FindControl("hlEditProfile")).Enabled = false;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (WebContext.Sibling != null && WebContext.Parent != null)
            {
                SuperAdminNavMenu.Visible = true;
                ClientAdminNavMenu.Visible = true;

                ClientMenu.Attributes.Add("class", "innerMenu");
                ContentBody.Attributes.Add("style", "top:-200px;");
                ContentFooter.Attributes.Add("style", "top:-200px;");

                BusinessName.Visible = true;
                lblClientBusinessName.Text = WebContext.Parent.ClientName;
            }
            else if (WebContext.Sibling != null && WebContext.Parent == null)
            {
                SuperAdminNavMenu.Visible = true;
            }
            else if (WebContext.Sibling == null && WebContext.Parent != null)
            {
                ClientAdminNavMenu.Visible = true;
            }
            else
            {
                SuperAdminNavMenu.Visible = false;
                ClientAdminNavMenu.Visible = false;
            }

            if (WebContext.CurrentUser != null)
            {
                if (HttpContext.Current.User.IsInRole("Admin") || HttpContext.Current.User.IsInRole("AdminUser"))
                    ltrSMSCredit.Text = string.IsNullOrEmpty(WebContext.Parent.SMSCredit.ToString()) ? "0" : WebContext.Parent.SMSCredit.ToString();
                if (string.Compare(WebContext.CurrentUser.Identity.Name, "superadmin", true) == 0)
                {
                    ((HyperLink)lvBackEndMasterPage.FindControl("hlEditProfile")).Enabled = false;
                    imgProfile.ImageUrl = "~/App_Themes/SleekTheme/Images/ProfileMale.png";
                    imgProfile.AlternateText = "SuperAdmin";
                }
                else
                {
                    CustomUserProfile profile = CustomUserProfile.GetUserProfile(WebContext.CurrentUser.Identity.Name);
                    if (string.IsNullOrWhiteSpace(profile.ProfileUrl))
                    {
                        if (string.Compare(profile.Gender, "female", true) == 0)
                            imgProfile.ImageUrl = "~/App_Themes/SleekTheme/Images/ProfileFemale.png";
                        else
                            imgProfile.ImageUrl = "~/App_Themes/SleekTheme/Images/ProfileMale.png";
                    }
                    else
                    {
                        if (WebContext.CurrentUser.IsInRole("Admin") || WebContext.CurrentUser.IsInRole("AdminUser"))
                        {
                            string imagePath = "~/Uploads/" + WebContext.Parent.ClientID + "/" + WebContext.CurrentUser.Identity.Name + "/ProfilePicture/" + profile.ProfileUrl;
                            if (File.Exists(Server.MapPath(imagePath)))
                                imgProfile.ImageUrl = imagePath;
                            else
                            {
                                if(string.Compare(profile.Gender, "female", true) == 0)
                                    imgProfile.ImageUrl = "~/App_Themes/SleekTheme/Images/ProfileFemale.png";
                                else
                                    imgProfile.ImageUrl = "~/App_Themes/SleekTheme/Images/ProfileMale.png";
                            }
                        }
                        else
                        {
                            string imagePath = "~/Uploads/" + WebContext.CurrentUser.Identity.Name + "/ProfilePicture/" + profile.ProfileUrl;
                            if (File.Exists(Server.MapPath(imagePath)))
                                imgProfile.ImageUrl = imagePath;
                            else
                            {
                                if (string.Compare(profile.Gender, "female", true)==0)
                                    imgProfile.ImageUrl = "~/App_Themes/SleekTheme/Images/ProfileFemale.png";
                                else
                                    imgProfile.ImageUrl = "~/App_Themes/SleekTheme/Images/ProfileMale.png";
                            }
                        }
                    }

                    imgProfile.ToolTip = imgProfile.AlternateText = profile.FirstName + " " + profile.MiddleName + " " + profile.LastName;
                }
            }
        }

        protected void LoginStatusLS_LoggingOut(object sender, EventArgs e)
        {
            WebContext.ClearSession();
        }

        public void SetSANavMenuVisibility(bool value)
        {
            SuperAdminNavMenu.Visible = value;
        }

        public void SetCANavMenuVisibility(bool value)
        {
            ClientAdminNavMenu.Visible = value;
        }

        protected void rbtnGeneral_CheckedChanged(object sender, EventArgs e)
        {
            string hitType = "PageHit", searchDuration = "All";

            if (rbtnPageHit.Checked)
                hitType = "PageHit";
            else if (rbtnSearchHit.Checked)
                hitType = "SearchHit";

            if (rbtnAll.Checked)
                searchDuration = "All";
            else if (rbtnDay.Checked)
                searchDuration = "1";
            else if (rbtnWeek.Checked)
                searchDuration = "7";
            else if (rbtnMonth.Checked)
                searchDuration = "30";
            else if (rbtnYear.Checked)
                searchDuration = "365";

            string makeURL = ResolveClientUrl("~/Admin/Client/ClientAnalytics.aspx?HitType=" + hitType + "&SearchDuration= " + searchDuration);
            string jScript = "window.parent.jQuery.fancybox({'href':'" + makeURL + "', 'type':'iframe', 'padding': 0, 'width': 700, 'height': 600, 'modal': false, 'scrolling': 'no', 'centerOnScroll': true, 'showCloseButton': true})";

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Analytics", jScript, true);

        }

        protected void rptrPackages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltrPackageName = (Literal)e.Item.FindControl("ltrPackageName");
                Literal ltrExpiryDate = (Literal)e.Item.FindControl("ltrExpiryDate");
                PackageOrderDetails details = (PackageOrderDetails)e.Item.DataItem;
                if (details.PackageName.EndsWith(" BySleekSurf"))
                    ltrPackageName.Text = ltrPackageName.Text.Replace(" BySleekSurf", string.Empty);
            }
        }

        protected void rbtnPageHit_CheckedChanged(object sender, EventArgs e)
        {
            rbtnGeneral_CheckedChanged(sender, e);
        }

        protected void rbtnSearchHit_CheckedChanged(object sender, EventArgs e)
        {
            rbtnGeneral_CheckedChanged(sender, e);
        }

        protected void rbtnAll_CheckedChanged(object sender, EventArgs e)
        {
            rbtnGeneral_CheckedChanged(sender, e);
        }

        protected void rbtnDay_CheckedChanged(object sender, EventArgs e)
        {
            rbtnGeneral_CheckedChanged(sender, e);
        }

        protected void rbtnWeek_CheckedChanged(object sender, EventArgs e)
        {
            rbtnGeneral_CheckedChanged(sender, e);
        }

        protected void rbtnMonth_CheckedChanged(object sender, EventArgs e)
        {
            rbtnGeneral_CheckedChanged(sender, e);
        }

        protected void rbtnYear_CheckedChanged(object sender, EventArgs e)
        {
            rbtnGeneral_CheckedChanged(sender, e);
        }
    }
}