using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using SleekSurf.FrameWork;
using System.Web.Security;

namespace SleekSurf.Web.Admin
{
    public partial class SendEmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMyEmail.Text = Membership.GetUser(WebContext.CurrentUser.Identity.Name).Email.ToString();

                if (!string.IsNullOrEmpty(WebContext.GetQueryStringValue("To")))
                    txtTo.Text = WebContext.GetQueryStringValue("To");
            }
        }

        private List<string> SplitEmailAddresses(string multipleEmailAddresses)
        {
            return multipleEmailAddresses.Split(new char[] { ';', ',', ' ' }).Where(e => e.Length > 0).ToList();
        }

        private double CalculateUploadedFileSize(HttpFileCollection uploadedFiles)
        {
            double size = 0.0;
            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                HttpPostedFile userPostedFile = uploadedFiles[i];
                size += userPostedFile.ContentLength;
            }

            return size / 1024;
        }
        private void ClearFields()
        {
            txtTo.Text = string.Empty;
            txtCc.Text = string.Empty;
            txtBcc.Text = string.Empty;
            txtSubject.Text = string.Empty;
            editorBody.Content = string.Empty;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                List<string> toEmailList = new List<string>();
                List<string> ccEmailList = new List<string>();
                List<string> bccEmailList = new List<string>();
                List<Attachment> attachmentList = new List<Attachment>();

                CustomUserProfile sendersProfile = CustomUserProfile.GetUserProfile(WebContext.CurrentUser.Identity.Name);
                string from = lblMyEmail.Text.Trim();
                string fromName = sendersProfile.FirstName + " " + sendersProfile.MiddleName + " " + sendersProfile.LastName;
                string subject = txtSubject.Text.Length > 0 ? txtSubject.Text : (WebContext.Parent != null ? WebContext.Parent.ClientName : "SleekSurf");
                string body = editorBody.Content;

                if (txtTo.Text != string.Empty)
                    toEmailList = SplitEmailAddresses(txtTo.Text.Trim());

                if (txtCc.Text != string.Empty)
                    ccEmailList = SplitEmailAddresses(txtCc.Text.Trim());

                if (txtBcc.Text != string.Empty)
                    bccEmailList = SplitEmailAddresses(txtBcc.Text.Trim());

                HttpFileCollection uploadedFiles = Request.Files;

                if (CalculateUploadedFileSize(uploadedFiles) <= 11000)
                {
                    if (uploadedFiles.Count > 0)
                    {
                        for (int i = 0; i < uploadedFiles.Count; i++)
                        {
                            HttpPostedFile userPostedFile = uploadedFiles[i];

                            try
                            {
                                if (userPostedFile.ContentLength > 0)
                                    attachmentList.Add(new Attachment(userPostedFile.InputStream, userPostedFile.FileName, userPostedFile.ContentType));
                            }
                            catch (Exception ex)
                            {
                                Helpers.LogError(ex);
                                lblMessage.Text = "Error : " + ex.Message;
                                lblMessage.CssClass = "errorMsg";
                            }
                        }
                    }

                    if (Helpers.SendEmail(from, fromName, toEmailList, ccEmailList, bccEmailList, subject, body, attachmentList))
                    {
                        lblMessage.CssClass = "successMsg";
                        lblMessage.Text = "Email has been sent to your recipients";
                        ClearFields();
                    }
                    else
                    {
                        lblMessage.CssClass = "errorMsg";
                        lblMessage.Text = "Error sending email. Please try again later.";
                    }
                }
                else
                {
                    lblMessage.CssClass = "errorMsg";
                    lblMessage.Text = "Maximum Upload Size limit is 10MB";
                }
            }
        }
    }
}