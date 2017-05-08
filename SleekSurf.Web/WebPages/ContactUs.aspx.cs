using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using System.IO;
using System.Web.Security;
using System.Net.Mail;
using SleekSurf.Entity;
using SleekSurf.Manager;

namespace SleekSurf.Web.WebPages
{
    public partial class ContactUs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Result<AdvertisementDetails> resultRightAds = AdvertisementManager.SelectRandomAddsWithoutClient("right");

            if (resultRightAds.Status == ResultStatus.NotFound)
                DefaultRightAd.Visible = true;
            else
            {
                rptrRightAds.DataSource = resultRightAds.EntityList;
                rptrRightAds.DataBind();
            }
        }

        protected void rptrRightAds_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            AdvertisementDetails ad = (AdvertisementDetails)e.Item.DataItem;

            Image imgAd = (Image)e.Item.FindControl("imgAd");
            imgAd.ImageUrl = "~/Uploads/" + "Advertisements/" + ad.ImageUrl;

            string[] dimension = Enum.GetName(typeof(AdDimensionRight), ad.FitToPanel).Replace('d', ' ').Trim().Split('x');
            imgAd.Width = Convert.ToInt32(dimension[0]);
            imgAd.Height = Convert.ToInt32(dimension[1]);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string from = txtEmail.Text;
            string fromName = txtName.Text;
            string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
            string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
            string appPath = Request.PhysicalApplicationPath;

            StreamReader userBodySR = new StreamReader(appPath + "EmailTemplates/ContactUs.txt");
            string body = userBodySR.ReadToEnd();
            userBodySR.Close();

            body = body.Replace("<%Logo%>", logoUrl);
            body = body.Replace("<%TopBackGround%>", topBackGroundUrl);
            body = body.Replace("<%ReceiverFullName%>", "SleekSurf Team");
            body = body.Replace("<%SenderFullName%>", fromName);
            if (txtBusinessName.Text.Length > 0)
                body = body.Replace("<%SenderBusinessName%>", " (of " + txtBusinessName.Text + ")");
            else
                body = body.Replace("<%SenderBusinessName%>", "");

            body = body.Replace("<%Message%>", txtMessage.Text.Replace(Environment.NewLine, "<br />"));
            body = body.Replace("<%ContactNo%>", txtContact.Text);
            body = body.Replace("<%ReplyEmail%>", txtEmail.Text);
            if (txtWebsite.Text.Length > 0)
                body = body.Replace("<%WebSite%>", "Website: <span style='font-weight: bold;'>" + txtWebsite.Text + "</span>");
            else
                body = body.Replace("<%WebSite%>", "");

            if (txtAddress.Text.Length > 0)
                body = body.Replace("<%Address%>", "Address: <span style='font-weight: bold;'>" + txtAddress.Text + "</span>");
            else
                body = body.Replace("<%Address%>", "");

            string subject = "Viewer's Query - " + txtSubject.Text;

            //email sent to all superadmins
            string[] userNameList = Roles.GetUsersInRole("SuperAdmin");
            List<UserInfoPartial> users = new List<UserInfoPartial>();
            foreach(string userName in userNameList)
            {
                CustomUserProfile profile = CustomUserProfile.GetUserProfile(userName);
                users.Add(new UserInfoPartial() { FirstName = profile.FirstName, MiddleName = profile.MiddleName, LastName = profile.LastName, Email = Membership.GetUser(userName).Email });
            }
           
            try
            {
                Helpers.SendEmail(from, fromName, users, subject, body);
                lblMessage.CssClass = "successMsg";
                lblMessage.Text = "Message has been submitted successfully.";
                ClearFields();
            }
            catch (SmtpException ex)
            {
                Helpers.LogError(ex);
                lblMessage.CssClass = "errorMsg";
                lblMessage.Text = ex.Message;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                lblMessage.CssClass = "errorMsg";
                lblMessage.Text = ex.Message;
            }
        }

        private void ClearFields()
        {
            txtBusinessName.Text = string.Empty;
            txtName.Text = string.Empty;
            txtContact.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtWebsite.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtSubject.Text = string.Empty;
            txtMessage.Text = string.Empty;
        }
    }
}