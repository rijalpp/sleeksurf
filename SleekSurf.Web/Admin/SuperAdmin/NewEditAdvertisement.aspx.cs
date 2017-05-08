using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Web.Security;
using SleekSurf.Entity;
using SleekSurf.Manager;
using SleekSurf.FrameWork;

namespace SleekSurf.Web.Admin.SuperAdmin
{
    public partial class NewEditAdvertisement : System.Web.UI.Page
    {
        public AdvertisementDetails advertisement = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdvertisementDetails"] != null)
            {
                advertisement = (AdvertisementDetails)Session["AdvertisementDetails"];
                lblTitle.Text += " - Update Mode";
            }
            else
            {
                lblTitle.Text += " - Add Mode";
                ucNewEditAd.ImgThumb.Visible = false;
            }

            if (!IsPostBack)
            {
                if (Session["AdvertisementDetails"] != null)
                    BindAdvertisement();
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("AdMgmt"))].Selected = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid)
                SaveAdvertisement();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session.Remove("AdvertisementDetails");
            ClearFields();
            Redirector.GoToRequestedPage("~/Admin/SuperAdmin/AdvertisementManagement.aspx");
        }

        private void BindAdvertisement()
        {
            advertisement = AdvertisementManager.SelectAdvertisement(advertisement.AdID).EntityList[0];
            string avatarUrl = string.Format("~/Uploads/Advertisements/{0}", advertisement.ImageUrl);
            ucNewEditAd.AddName = advertisement.AdName;
            ucNewEditAd.ImgThumb.ImageUrl = avatarUrl;
            ucNewEditAd.Advertiser = advertisement.Advertiser;
            ucNewEditAd.ContactDetail = advertisement.ContactDetail;
            ucNewEditAd.Email = advertisement.Email;
            ucNewEditAd.NavigateUrl = advertisement.NavigateUrl;
            ucNewEditAd.StartDate = advertisement.StartDate;
            ucNewEditAd.EndDate = advertisement.EndDate;
            ucNewEditAd.AmountPaid = advertisement.AmountPaid;
            ucNewEditAd.DisplayPosition = advertisement.DisplayPosition;

            if (advertisement.DisplayPosition == "Right")
                ucNewEditAd.ImageHolder.Attributes.Add("class", "adImageRightBack");
            else
                ucNewEditAd.ImageHolder.Attributes.Add("class", "adImageFooterBack");

            ucNewEditAd.DropDownDisplayPosition.Enabled = false;
            ucNewEditAd.Dimension = advertisement.FitToPanel;
            ucNewEditAd.Published = advertisement.Published;
            ucNewEditAd.Comments = advertisement.Comments;
        }

        private void SaveAdvertisement()
        {
            if (advertisement != null)
                advertisement = AdvertisementManager.SelectAdvertisement(advertisement.AdID).EntityList[0];
            else
                advertisement = new AdvertisementDetails();

            advertisement.AdName = ucNewEditAd.AddName;
            advertisement.Advertiser = ucNewEditAd.Advertiser;
            advertisement.ContactDetail = ucNewEditAd.ContactDetail;
            advertisement.Email = ucNewEditAd.Email;
            if (ucNewEditAd.AddUrlImage.HasFile)
            {
                if (!string.IsNullOrEmpty(advertisement.AdID))
                {
                    string dirUrlP = BasePage.BaseUrl + "Uploads/" + "/Advertisements";
                    //deleted the old profile photo if it exists
                    string dirPathP = Server.MapPath(dirUrlP);
                    if (File.Exists(dirPathP + "/" + advertisement.ImageUrl))
                        File.Delete(dirPathP + "/" + advertisement.ImageUrl);
                }

                string fileExtension = Path.GetExtension(ucNewEditAd.AddUrlImage.PostedFile.FileName);
                if (!string.IsNullOrEmpty(advertisement.AdID))
                    advertisement.ImageUrl = advertisement.AdID + fileExtension;
            }

            advertisement.NavigateUrl = ucNewEditAd.NavigateUrl;
            advertisement.StartDate = ucNewEditAd.StartDate;
            advertisement.EndDate = ucNewEditAd.EndDate;
            advertisement.AmountPaid = ucNewEditAd.AmountPaid;
            advertisement.DisplayPosition = ucNewEditAd.DisplayPosition;
            advertisement.FitToPanel = ucNewEditAd.Dimension;
            advertisement.Comments = ucNewEditAd.Comments;
            advertisement.Published = ucNewEditAd.Published;
            advertisement.ClientID = null;//ONLY IN SUPERADMIN SECTION
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            if (string.IsNullOrEmpty(advertisement.AdID))// IF IT IS IN ADD MODE
            {
                if (ucNewEditAd.AddUrlImage.HasFile)// IF THE AD FILE IS ATTACHED
                {
                    advertisement.AdID = System.DateTime.Now.ToString("AD-ddMMyyy-HHmmssfff");
                    string fileExtension = Path.GetExtension(ucNewEditAd.AddUrlImage.PostedFile.FileName);
                    advertisement.ImageUrl = advertisement.AdID + fileExtension;
                    advertisement.CreatedBy = HttpContext.Current.User.Identity.Name;
                    result = AdvertisementManager.InsertAdvertisement(advertisement);

                    string tempDimension = "";
                    string[] dimension;
                    if (string.Compare(advertisement.DisplayPosition, "right", true) == 0)
                    {
                        tempDimension = Enum.GetName(typeof(AdDimensionRight), advertisement.FitToPanel).Replace('d', ' ').Trim();
                        tempDimension = string.Join("px X ", tempDimension.Split('x')) + "px";
                        dimension = Enum.GetName(typeof(AdDimensionRight), advertisement.FitToPanel).Replace('d', ' ').Trim().Split('x');
                    }
                    else
                    {
                        tempDimension = Enum.GetName(typeof(AdDimensionFooter), advertisement.FitToPanel).Replace('d', ' ').Trim();
                        tempDimension = string.Join("px X ", tempDimension.Split('x')) + "px";
                        dimension = Enum.GetName(typeof(AdDimensionFooter), advertisement.FitToPanel).Replace('d', ' ').Trim().Split('x');
                    }

                    //SEND EMAIL
                    string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
                    string logoDisplay = "block";
                    string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
                    string appPath = Request.PhysicalApplicationPath;
                    StreamReader advertiserBodySR = new StreamReader(appPath + "EmailTemplates/AdRegistrationNotificationToAdvertiser.txt");
                    string adBody = advertiserBodySR.ReadToEnd();
                    advertiserBodySR.Close();
                    //SEND EMAIL TO ADVERTISER
                    adBody = adBody.Replace("<%Logo%>", logoUrl);
                    adBody = adBody.Replace("<%LogoDisplay%>", logoDisplay);
                    adBody = adBody.Replace("<%TopBackGround%>", topBackGroundUrl);
                    adBody = adBody.Replace("<%Advertiser%>", advertisement.Advertiser);
                    adBody = adBody.Replace("<%AdTitle%>", advertisement.AdName);
                    adBody = adBody.Replace("<%Dimension%>", tempDimension);
                    adBody = adBody.Replace("<%StartDate%>", string.Format("{0:dd MMMM yyyy}", advertisement.StartDate));
                    adBody = adBody.Replace("<%EndDate%>", string.Format("{0:dd MMMM yyyy}", advertisement.EndDate));
                    adBody = adBody.Replace("<%AmountPaid%>", advertisement.AmountPaid.ToString());
                    adBody = adBody.Replace("<%ImageUrl%>", BasePage.FullBaseUrl+"Uploads/Advertisements/"+ advertisement.ImageUrl);
                    adBody = adBody.Replace("<%width%>", dimension[0]);
                    adBody = adBody.Replace("<%height%>", dimension[1]);
                    adBody = adBody.Replace("<%NavigationUrl%>", advertisement.NavigateUrl);
                    adBody = adBody.Replace("<%ClientName%>", "SleekSurf Team");

                    string subject = "Advertisement registered successfully.";

                    Helpers.SendEmail("noreply@sleeksurf.com", "SleekSurf Team", advertisement.Email, subject, adBody);
                    //SEND EMAIL TO THE BUSINESS OWNER
                    StreamReader adInfoToOwner = new StreamReader(appPath + "EmailTemplates/AdvertisementNotificationToBusinessOwner.txt");
                    string adBodtToOwner = adInfoToOwner.ReadToEnd();
                    adInfoToOwner.Close();

                    adBodtToOwner = adBodtToOwner.Replace("<%Logo%>", logoUrl);
                    adBodtToOwner = adBodtToOwner.Replace("<%LogoDisplay%>", logoDisplay);
                    adBodtToOwner = adBodtToOwner.Replace("<%TopBackGround%>", topBackGroundUrl);
                    adBodtToOwner = adBodtToOwner.Replace("<%AddedBy%>", HttpContext.Current.User.Identity.Name);
                    adBodtToOwner = adBodtToOwner.Replace("<%Advertiser%>", advertisement.Advertiser);
                    adBodtToOwner = adBodtToOwner.Replace("<%AdTitle%>", advertisement.AdName);
                    adBodtToOwner = adBodtToOwner.Replace("<%Dimension%>", tempDimension);
                    adBodtToOwner = adBodtToOwner.Replace("<%StartDate%>", string.Format("{0:dd MMMM yyyy}", advertisement.StartDate));
                    adBodtToOwner = adBodtToOwner.Replace("<%EndDate%>", string.Format("{0:dd MMMM yyyy}", advertisement.EndDate));
                    adBodtToOwner = adBodtToOwner.Replace("<%AmountPaid%>", advertisement.AmountPaid.ToString());
                    adBodtToOwner = adBodtToOwner.Replace("<%ImageUrl%>", BasePage.FullBaseUrl + "Uploads/Advertisements/" + advertisement.ImageUrl);
                    adBodtToOwner = adBodtToOwner.Replace("<%width%>", dimension[0]);
                    adBodtToOwner = adBodtToOwner.Replace("<%height%>", dimension[1]);
                    adBodtToOwner = adBodtToOwner.Replace("<%NavigationUrl%>", advertisement.NavigateUrl);

                     string[] usernameList = Roles.GetUsersInRole("SuperAdmin");
                     List<UserInfoPartial> userInfoList = new List<UserInfoPartial>();
                     foreach (string username in usernameList)
                     {
                         MembershipUser thisUser = Membership.GetUser(username);
                         if (thisUser.IsApproved && !thisUser.IsLockedOut)
                         {
                             CustomUserProfile profile = CustomUserProfile.GetUserProfile(username);
                             UserInfoPartial userInfo = new UserInfoPartial() { FirstName = profile.FirstName, MiddleName = profile.MiddleName, LastName = profile.LastName, Email = thisUser.Email };
                             userInfoList.Add(userInfo);
                         }
                     }

                     Helpers.SendEmail(userInfoList, "noreply@sleeksurf.com", subject, adBody);
                }
                else//DOESN'T SAVE THE ADD IF THE IMAGE IS NOT REQUIRED.
                {
                    lblMessage.CssClass = "errorMsg";
                    result.Message = "Must attach advertise image.";
                }
            }
            else//IF IT IS IN UPDATE MODE
            {
                advertisement.UpdatedBy = HttpContext.Current.User.Identity.Name;
                result = AdvertisementManager.UpdateAdvertisement(advertisement);
            }

            if (result.Status == ResultStatus.Success)
            {
                UploadAddFile(advertisement.AdID);
                ClearFields();
                lblMessage.CssClass = "successMsg";
            }
            else
            {
                lblMessage.CssClass = "errorMsg";
            }

            lblMessage.Text = result.Message;
        }

        private void UploadAddFile(string adID)
        {
            string dirUrlP = BasePage.BaseUrl + "Uploads/Advertisements";
            if (ucNewEditAd.AddUrlImage.HasFile)
            {
                string dirPathP = Server.MapPath(dirUrlP);
                if (!Directory.Exists(dirPathP))
                    Directory.CreateDirectory(dirPathP);
                string fileExtension = Path.GetExtension(ucNewEditAd.AddUrlImage.PostedFile.FileName);
                string outputfileName = dirPathP + "/" + adID + fileExtension;
                ucNewEditAd.AddUrlImage.SaveAs(outputfileName);
            }
        }

        public System.Drawing.Image resize(System.Drawing.Image img, int width, int height)
        {
            Bitmap b = new Bitmap(width, height);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.DrawImage(img, 0, 0, width, height);
            g.Dispose();

            return (System.Drawing.Image)b;
        }

        private void ClearFields()
        {
            Session.Remove("AdvertisementDetails");
            ucNewEditAd.AddName = string.Empty;
            ucNewEditAd.ImgThumb.ImageUrl = string.Empty;
            ucNewEditAd.Advertiser = string.Empty;
            ucNewEditAd.ContactDetail = string.Empty;
            ucNewEditAd.Email = string.Empty;
            ucNewEditAd.NavigateUrl = string.Empty;
            ucNewEditAd.StartDate = null;
            ucNewEditAd.EndDate = null;
            ucNewEditAd.Amount = string.Empty;
            ucNewEditAd.DisplayPosition = "Select Below";
            ucNewEditAd.DropDownDimension.Items.Clear();
            ucNewEditAd.DropDownDimension.Enabled = false;
            ucNewEditAd.Published = false;
            ucNewEditAd.Comments = string.Empty;
            ucNewEditAd.ImageHolder.Visible = false;
            advertisement = null;
        }
    }
}