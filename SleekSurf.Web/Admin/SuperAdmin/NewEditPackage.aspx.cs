using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using SleekSurf.Manager;

namespace SleekSurf.Web.Admin.SuperAdmin
{
    public partial class NewEditPackage : System.Web.UI.Page
    {
        string packageCode = null;
        static PackageDetails package = new PackageDetails();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindFeatureTypeList();
            if (Session["Package"] != null)
            {
                PackageDetails package = (PackageDetails)Session["Package"];
                packageCode = package.PackageCode;
                if (!IsPostBack)
                {
                    BindPackage();
                }
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("PackageMgmt"))].Selected = true;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(packageCode))
            {
                lblTitle.Text += " - Add Mode";
                dvRightLinks.Visible = false;
            }
            else
            {
                lblTitle.Text += " - Update Mode";
                dvRightLinks.Visible = true;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearFields();
            Response.Redirect("~/Admin/SuperAdmin/PackageManagement.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid)
                SavePackage();
        }

        private void BindFeatureTypeList()
        {
            ddlFeatureType.DataSource = Enum.GetNames(typeof(FeatureType));
            ddlFeatureType.DataBind();
            ddlFeatureType.Items.Insert(0, "Select Below");
        }

        private void BindPackage()
        {
            Result<PackageDetails> result = ClientPackageManager.SelectPackage(packageCode);
            package = result.EntityList[0];
            if (result.Status == ResultStatus.Success)
            {
                txtPackageName.Text = package.PackageName;
                editorPackageDescription.Content = package.Description;
                ddlFeatureType.SelectedValue = package.FeatureType;
                chkPublished.Checked = package.Published;
            }
            else
            {
                lblMessage.CssClass = "errorMsg";
                lblMessage.Text = result.Message;
            }
        }

        private void SavePackage()
        {
            package.PackageName = txtPackageName.Text;
            package.Description = editorPackageDescription.Content;
            package.FeatureType = ddlFeatureType.SelectedValue;
            package.Published = chkPublished.Checked;
            package.CreatedBy = WebContext.CurrentUser.Identity.Name;
            package.UpdatedBy = WebContext.CurrentUser.Identity.Name;

            Result<PackageDetails> result = new Result<PackageDetails>();
            if (packageCode == null)
            {
                package.PackageCode = System.DateTime.Now.ToString("CPG-ddMMyyy-HHmmss");
                result = ClientPackageManager.InsertPackage(package);
            }
            else
            {
                package.PackageCode = packageCode;
                result = ClientPackageManager.UpdatePackage(package);
            }
            if (result.Status == ResultStatus.Success)
            {
                lblMessage.CssClass = "successMsg";
                ClearFields();
            }
            else
                lblMessage.CssClass = "errorMsg";

            lblMessage.Text = result.Message;
        }

        private void ClearFields()
        {
            packageCode = null;
            txtPackageName.Text = string.Empty;
            editorPackageDescription.Content = string.Empty;
            chkPublished.Checked = false;
            ddlFeatureType.SelectedIndex = 0;
            Session.Remove("Package");
        }
    }
}