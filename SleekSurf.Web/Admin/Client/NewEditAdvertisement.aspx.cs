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
using System.Drawing;
using System.Web.Security;

namespace SleekSurf.Web.Admin.Client
{
    public partial class NewEditAdvertisement : BasePage
    {
        public AdvertisementDetails advertisement = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdvertisementDetails"] != null)
            {
                if (!IsPostBack)
                {
                    //CHECK IF THE PAYMENT OPTION IS SOCIAL GRANT
                    Result<PackageOrderDetails> tempPackageOrderDetails = ClientPackageManager.SelectRecentDistinctOrdersByClientID(WebContext.Parent.ClientID);
                    if (tempPackageOrderDetails.EntityList.Count > 0)
                    {
                        foreach (PackageOrderDetails detail in tempPackageOrderDetails.EntityList)
                        {
                            if (detail.PaymentOption == PaymentOptionStatus.SocialGrant.ToString())
                                if (HttpContext.Current.User.IsInRole("Admin") || HttpContext.Current.User.IsInRole("AdminUser"))
                                {
                                    ucNewEditAd.PanelUpdateMode.Enabled = false;
                                    ucNewEditAd.TxtStartDate.ReadOnly = true;
                                    ucNewEditAd.TxtEndDate.ReadOnly = true;
                                }
                        }
                    }
                }
                advertisement = (AdvertisementDetails)Session["AdvertisementDetails"];
                lblTitle.Text += " - Update Mode";
            }
            else
            {
                if (!IsPostBack)
                {
                    //CHECK IF THE PAYMENT OPTION IS SOCIAL GRANT
                    Result<PackageOrderDetails> tempPackageOrderDetails = ClientPackageManager.SelectRecentDistinctOrdersByClientID(WebContext.Parent.ClientID);
                    if (tempPackageOrderDetails.EntityList.Count > 0)
                    {
                        foreach (PackageOrderDetails detail in tempPackageOrderDetails.EntityList)
                        {
                            if (detail.PaymentOption == PaymentOptionStatus.SocialGrant.ToString())
                                if (HttpContext.Current.User.IsInRole("Admin") || HttpContext.Current.User.IsInRole("AdminUser"))
                                    Redirector.GoToRequestedPage("~/Admin/Client/AdvertisementManagement.aspx");
                        }
                    }
                }

                lblTitle.Text += " - Add Mode";
                ucNewEditAd.ImgThumb.Visible = false;
            }

            if (!IsPostBack)
            {
                if (Session["AdvertisementDetails"] != null)
                {
                    advertisement = (AdvertisementDetails)Session["AdvertisementDetails"];
                    BindAdvertisement();
                }
            }

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("ClientAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("AdMgmt"))].Selected = true;

            if (WebContext.Sibling != null)
            {
                Menu tmpMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
                tmpMenu.Items[tmpMenu.Items.IndexOf(tmpMenu.FindItem("ClientMgmt"))].Selected = true;
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //if (IsValid)
                SaveAdvertisement();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session.Remove("AdvertisementDetails");
            ClearFields();
            Redirector.GoToRequestedPage("~/Admin/Client/AdvertisementManagement.aspx");
        }

        private void BindAdvertisement()
        {
            advertisement = AdvertisementManager.SelectAdvertisement(advertisement.AdID).EntityList[0];
            string adUrl = string.Format("~/Uploads/{0}/Advertisements/{1}", WebContext.Parent.ClientID, advertisement.ImageUrl);
            ucNewEditAd.AddName = advertisement.AdName;
            ucNewEditAd.ImgThumb.ImageUrl = adUrl;
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
                    string dirUrlP = BasePage.BaseUrl + "Uploads/" + WebContext.Parent.ClientID + "/Advertisements";
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
            advertisement.Published = ucNewEditAd.Published;
            advertisement.Comments = ucNewEditAd.Comments;
            advertisement.ClientID = WebContext.Parent.ClientID;
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            if (string.IsNullOrEmpty(advertisement.AdID))// IF IT IS IN ADD MODE
            {
                if (ucNewEditAd.AddUrlImage.HasFile)// IF THE AD FILE IS ATTACHED
                {
                    advertisement.AdID = WebContext.Parent.ClientID + System.DateTime.Now.ToString("-AD-ddMMyyy-HHmmssfff");
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
                    // string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
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
                    adBody = adBody.Replace("<%ImageUrl%>", BasePage.FullBaseUrl + "Uploads/" + WebContext.Parent.ClientID + "/Advertisements/" + advertisement.ImageUrl);
                    adBody = adBody.Replace("<%width%>", dimension[0]);
                    adBody = adBody.Replace("<%height%>", dimension[1]);
                    adBody = adBody.Replace("<%NavigationUrl%>", advertisement.NavigateUrl);
                    adBody = adBody.Replace("<%ClientName%>", WebContext.Parent.ClientName);

                    string subject = "Advertisement registered successfully.";

                    Helpers.SendEmail(WebContext.Parent.BusinessEmail, WebContext.Parent.ClientName, advertisement.Email, subject, adBody);
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
                    adBodtToOwner = adBodtToOwner.Replace("<%ImageUrl%>", BasePage.FullBaseUrl + "Uploads/" + WebContext.Parent.ClientID + "/Advertisements/" + advertisement.ImageUrl);
                    adBodtToOwner = adBodtToOwner.Replace("<%width%>", dimension[0]);
                    adBodtToOwner = adBodtToOwner.Replace("<%height%>", dimension[1]);
                    adBodtToOwner = adBodtToOwner.Replace("<%NavigationUrl%>", advertisement.NavigateUrl);
                    MembershipUser thisUser = Membership.GetUser(WebContext.Parent.ContactPerson);


                    Helpers.SendEmail(WebContext.Parent.BusinessEmail, WebContext.Parent.ClientName, thisUser.Email, subject, adBodtToOwner);
                    Helpers.SendEmail(WebContext.Parent.BusinessEmail, WebContext.Parent.ClientName, WebContext.Parent.BusinessEmail, subject, adBodtToOwner);
                }
                else//DOESN'T SAVE THE AD IF THE IMAGE IS NOT REQUIRED.
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
            string dirUrlP = BasePage.BaseUrl + "Uploads/" + WebContext.Parent.ClientID + "/Advertisements";
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