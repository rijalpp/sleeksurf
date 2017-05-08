using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using SleekSurf.Entity;
using SleekSurf.Manager;

namespace SleekSurf.Web.Admin.Client
{
    public partial class AccessSuspended : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (WebContext.Parent != null)
            {
                if (WebContext.Parent.Comment == Status.InActiveBySuperAdmin.ToString())
                {
                    ltrAccountMessage.Text = "Account Suspended by SleekSurf";
                    ltrMessageBoard.Text = "<span style='margin:0px; padding:10px 0px; display:block;'> Hi " + WebContext.CurrentUser.Identity.Name + ", </span>";
                    ltrMessageBoard.Text += "<span style='margin:0px; padding:10px 0px; display:block;'>Your account has been suspended by SleekSurf Team.<br /> please contact us immediately if you think your account has been incorrectly suspended.</span>";
                    pnlInActiveBySuperAdmin.Visible = true;
                }
                else if (WebContext.Parent.Comment == Status.InActiveByAccountExpiration.ToString())
                {
                    Result<PackageOrderDetails> result = ClientPackageManager.SelectRecentDistinctOrdersByClientID(WebContext.Parent.ClientID);
                    if (result.Status == ResultStatus.Success && result.EntityList.Count > 0)
                    {
                        ltrAccountMessage.Text = "All Account packages have expired.";
                        ltrMessageBoard.Text = "<span style='margin:0px; padding:10px 0px; display:block;'> Hi " + WebContext.CurrentUser.Identity.Name + ", </span>";
                        ltrMessageBoard.Text += "<span style='margin:0px; padding:10px 0px; display:block;'>Account package(s) you bought have expired, please renew your account package and enjoy using the features in the package(s).<br /> If you've encountered any difficulties in renewing your package(s), please <a href='http://www.sleeksurf.com/WebPages/ContactUs.aspx'>Contact Us</a>  immediately.</span>";
                    }
                    else
                    {
                        ltrAccountMessage.Text = "Account Purchase Required.";
                        ltrMessageBoard.Text = "<span style='margin:0px; padding:10px 0px; display:block;'> Hi " + WebContext.CurrentUser.Identity.Name + ", </span>";
                        ltrMessageBoard.Text += "<span style='margin:0px; padding:10px 0px; display:block;'>You haven't purchased the Account yet. Please click Purchase button to browse package options.</span>";
                    }
                    pnlInActiveByAccountExpiration.Visible = true;
                }
                else if (WebContext.Parent.Comment == Status.InActive.ToString())
                {
                    ltrAccountMessage.Text = "Inactive Account";
                    ltrMessageBoard.Text = "<span style='margin:0px; padding:10px 0px; display:block;'> Hi " + WebContext.CurrentUser.Identity.Name + ", </span>";
                    ltrMessageBoard.Text += "<span style='margin:0px; padding:10px 0px; display:block;'>Your account is inactive. Please contact us immediately to re-activate.</span>";

                }
                else //if (WebContext.Parent.Comment == Status.InActiveByDefault.ToString())
                {
                    ltrAccountMessage.Text = "Account Purchase Required";
                    ltrMessageBoard.Text = "<span style='margin:0px; padding:10px 0px; display:block;'> Hi " + WebContext.CurrentUser.Identity.Name + ", </span>";
                    ltrMessageBoard.Text += "<span style='margin:0px; padding:10px 0px; display:block;'>You haven't purchased the Account yet. Please click Purchase button to browse package options.</span>";
                    pnlInActiveByDefault.Visible = true;
                }
            }
            else
            {
                ltrAccountMessage.Text = "Technical Error";
                ltrMessageBoard.Text = "<span style='margin:0px; padding:10px 0px; display:block;'>Opps! There is some technical problem reading your details. Please login again or contact us immediately.</span>";
                pnlInActiveBySuperAdmin.Visible = false;
                pnlInActiveByDefault.Visible = false;
                pnlInActiveByAccountExpiration.Visible = false;
                pnlTechnicalError.Visible = true;
            }

        }

        protected void btnInActiveByAccountExpiration_Click(object sender, EventArgs e)
        {

            Redirector.GoToPackagePage();
        }

        protected void btnInActiveBySuperAdmin_Click(object sender, EventArgs e)
        {
            Redirector.GoToRequestedPage("~/WebPages/ContactUs.aspx");
        }

        protected void btnTechnicalError_Click(object sender, EventArgs e)
        {
            Redirector.GoToRequestedPage("~/WebPages/ContactUs.aspx");
        }

        protected void btnInActiveByDefault_Click(object sender, EventArgs e)
        {
            Redirector.GoToPackagePage();
        }

        protected void LoginStatusLS_LoggingOut(object sender, EventArgs e)
        {
            WebContext.ClearSession();
        }
    }
}