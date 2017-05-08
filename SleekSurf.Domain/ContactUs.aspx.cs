using System;
using System.IO;
using System.Net.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;

namespace SleekSurf.Domain
{
    public partial class ContactUs : WebBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)(Master.Master.FindControl("NavigationMenu"));
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("Contact Us"))].Selected = true;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            BindGoogleMap();
        }

        protected string GetContactPerson(Guid contactPerson)
        {
            CustomUserProfile profile = CustomUserProfile.GetUserProfile(Membership.GetUser(contactPerson).UserName);
            return profile.FirstName + " " + profile.MiddleName + " " + profile.LastName;

        }

        protected void BindGoogleMap()
        {
            string address = WebContext.ClientProfile.Address;
            lblAddress.Text = address;
            HtmlGenericControl body = (HtmlGenericControl)Master.Master.FindControl("Client");
            body.Attributes.Add("onload", "Initialize(\'" + address + "\');");

            ddlMode.Attributes.Add("onchange", "CalculateRoute(\'" + address + "\');");

            HyperLinkViewLargeMap.CssClass = "FancyboxNewEdit viewInLarge";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string from = txtEmail.Text;
                string fromName = txtName.Text;
                string logoUrl = "";
                string logoDisplay = "none";
                if (!string.IsNullOrEmpty(WebContext.ClientProfile.LogoUrl))
                {
                    logoUrl = Configuration.GetConfigurationSetting("website", typeof(string)) + "/Uploads/" + WebContext.ClientProfile.ClientID + "/LogoPicture/" + WebContext.ClientProfile.LogoUrl;
                    logoDisplay = "block";
                }
                string topBackGroundUrl = Configuration.GetConfigurationSetting("website", typeof(string)) + "/App_Themes/Default/Images/MessageBoxTopBackground.png";
                string appPath = Request.PhysicalApplicationPath;

                StreamReader userBodySR = new StreamReader(appPath + "EmailTemplates/ClientProfileContactUs.txt");
                string body = userBodySR.ReadToEnd();
                userBodySR.Close();

                body = body.Replace("<%Logo%>", logoUrl);
                body = body.Replace("<%TopBackGround%>", topBackGroundUrl);
                body = body.Replace("<%ReceiverFullName%>", GetContactPerson(WebContext.ClientProfile.ContactPerson));
                body = body.Replace("<%SenderFullName%>", fromName);
                body = body.Replace("<%Comments%>", txtComment.Text.Replace(Environment.NewLine, "<br />"));
                body = body.Replace("<%ReplyEmail%>", txtEmail.Text);
                if (txtWebsite.Text.Length > 0)
                    body = body.Replace("<%WebSite%>", "Viewer's Website: " + txtWebsite.Text);
                else
                    body = body.Replace("<%WebSite%>", "");
                string to = WebContext.ClientProfile.BusinessEmail;
                string subject = WebContext.ClientProfile.ClientName + " - Query from your customer.";

                try
                {
                    Helpers.SendEmail(from, fromName, to, subject, body);
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
        }

        private void ClearFields()
        {
            txtName.Text = "";
            txtEmail.Text = "";
            txtWebsite.Text = "";
            txtComment.Text = "";
        }
    }
}