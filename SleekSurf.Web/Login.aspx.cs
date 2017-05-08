using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SleekSurf.FrameWork;

namespace SleekSurf.Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                ((TextBox)LgnLogin.FindControl("UserName")).Focus();
        }

        protected void LgnLogin_LoginError(object sender, EventArgs e)
        {
            //There was a problem logging in the user
            MembershipUser userInfo = Membership.GetUser(LgnLogin.UserName);

            if (userInfo == null)
            {
                //The user entered an invalid username...
                LgnLogin.FailureText = "User doesn't exists with the USERNAME " + LgnLogin.UserName;
            }
            else
            {
                //See if the user is locked out or not approved
                if (!userInfo.IsApproved)
                {
                    LgnLogin.FailureText = "Account NOT APPROVED! A newly created account shpuld be approved by clicking the link sent out to the account email. For other reasons, <a href='"+BasePage.FullBaseUrl+"WebPages/ContactUs.aspx'>contact us</a>.";
                }
                else if (userInfo.IsLockedOut)
                {
                    LgnLogin.FailureText = "Your account has been locked out because of a maximum number of incorrect login attempts. You will NOT be able to login until you contact a site administrator have your account unlocked.";
                }
                else
                {
                    //The password was incorrect
                    LgnLogin.FailureText = "Your login attempt was not successful. Please try again.";
                }
            }

        }
    }
}