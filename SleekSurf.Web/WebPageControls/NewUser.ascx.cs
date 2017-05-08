using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace SleekSurf.Web.WebPageControls
{
    public partial class NewUser : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.IsInRole("SuperAdmin") || HttpContext.Current.User.IsInRole("SuperAdminUser") ||
                    HttpContext.Current.User.IsInRole("Admin") || HttpContext.Current.User.IsInRole("AdminUser"))
                pnlRole.Visible = true;

            if (!IsPostBack)
                bindRoles();
        }
        private void bindRoles()
        {
            ddlRoles.DataSource = Roles.GetAllRoles();
            ddlRoles.DataBind();
            ddlRoles.Items.Insert(0, "Select Role");
        }

        public string UserName
        {
            get { return txtUserName.Text; }
            set { txtUserName.Text = value; }
        }

        public string Password
        {
            get { return txtPassword.Text; }
            set { txtPassword.Text = value; }
        }

        public string Email
        {
            get { return txtEmail.Text; }
            set { txtEmail.Text = value; }
        }

        public string SecretQuestion
        {
            get { return txtPasswordQuestion.Text; }
            set { txtPasswordQuestion.Text = value; }
        }

        public string SecretAnswer
        {
            get { return txtPasswordAnswer.Text; }
            set { txtPasswordAnswer.Text = value; }
        }

        public string GetRole
        {
            get { return ddlRoles.SelectedItem.Text; }
        }

        protected void txtUserName_TextChanged(object sender, EventArgs e)
        {
            MembershipUser userInfo = Membership.GetUser(txtUserName.Text);
            if (userInfo != null)
            {
                lblErrorUserMsg.Text = "Username exists!";
                cmpUserName.ValueToCompare = "";
                cmpUserName.ErrorMessage = "*";
            }
            else
            {
                lblErrorUserMsg.Text = "";
                cmpUserName.ValueToCompare = txtUserName.Text;
            }
        }
        protected void txtEmail_TextChanged(object sender, EventArgs e)
        {
            MembershipUserCollection userList = Membership.FindUsersByEmail(txtEmail.Text);
            if (userList.Count > 0)
            {
                lblErrorEmail.Text = "Email exists!";
                cmpCompareEmail.ValueToCompare = "";
                cmpCompareEmail.ErrorMessage = "*";
            }
            else
            {
                lblErrorEmail.Text = "";
                cmpCompareEmail.ValueToCompare = txtEmail.Text;
            }
        }
    }
}