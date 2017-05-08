using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.Manager;
using SleekSurf.FrameWork;

namespace SleekSurf.Web.Admin.SuperAdmin
{
    public partial class NewEditPackageOption : System.Web.UI.Page
    {
        PackageOptionDetails packageoption = new PackageOptionDetails();
        int packageOptionID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Package"] != null)
            {
                PackageDetails packageDetail = (PackageDetails)Session["Package"];
                lblTitle.Text += ClientPackageManager.SelectPackage(packageDetail.PackageCode).EntityList[0].PackageName + " ";
                if (Session["PackageOptionDetail_BackEnd"] != null)
                {
                    packageoption = (PackageOptionDetails)Session["PackageOptionDetail_BackEnd"];
                    packageOptionID = packageoption.PackageOptionID;
                }
                if (!IsPostBack)
                {
                    if (Session["PackageOptionDetail_BackEnd"] != null)
                    {
                        lblTitle.Text += "- Update Mode";
                        BindPackageOption();
                    }
                    else
                        lblTitle.Text += " - Add Mode";
                }
            }
            else
            {
                Redirector.GoToRequestedPage("~/Admin/SuperAdmin/PackageManagement.aspx");
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("PackageMgmt"))].Selected = true;
        }

        private void BindPackageOption()
        {
            packageoption = ClientPackageManager.SelectPackageOption(packageOptionID).EntityList[0];
            packageOptionID = packageoption.PackageOptionID;
            txtDuration.Text = packageoption.Duration;
            if (packageoption.DiscountPercentage > 0)
                txtDiscountPercentage.Text = packageoption.DiscountPercentage.ToString();
            txtPromoCode.Text = packageoption.PromoCode;
            txtPromoStartDate.Text = packageoption.PromoCodeStartDate.ToString();
            txtPromoEndDate.Text = packageoption.PromoCodeEndDate.ToString();
            chkPublished.Checked = packageoption.Published;
            editorDescription.Content = packageoption.Comments;
            txtPrice.Text = string.Format("{0:0.00}", packageoption.StandardPrice);
        }

        private void SavePackageOption()
        {
            PackageDetails package = (PackageDetails)Session["Package"];
            packageoption.PackageCode = package.PackageCode;
            packageoption.Duration = txtDuration.Text;
            packageoption.Comments = editorDescription.Content;
            if (txtDiscountPercentage.Text.Length > 0)
                packageoption.DiscountPercentage = Convert.ToDouble(txtDiscountPercentage.Text);
            packageoption.PromoCode = txtPromoCode.Text;
            if (txtPromoStartDate.Text.Length > 0)
                packageoption.PromoCodeStartDate = Convert.ToDateTime(txtPromoStartDate.Text);
            if (txtPromoEndDate.Text.Length > 0)
                packageoption.PromoCodeEndDate = Convert.ToDateTime(txtPromoEndDate.Text);
            packageoption.Published = chkPublished.Checked;
            packageoption.StandardPrice = Convert.ToDecimal(txtPrice.Text);

            if ((txtDiscountPercentage.Text.Length > 0 && txtPromoStartDate.Text.Length > 0 && txtPromoEndDate.Text.Length > 0 && txtPromoCode.Text.Length > 0) ||
                (txtDiscountPercentage.Text.Length == 0 && txtPromoStartDate.Text.Length == 0 && txtPromoEndDate.Text.Length == 0 && txtPromoCode.Text.Length == 0))
            { }
            else
            {
                lblMessage.Text = "You should either enter the value of all four fields: Promocode, Discount Percentage, Start Date and End Date or leave all of them empty.";
                lblMessage.CssClass = "errorMsg";
                return;
            }

            Result<PackageOptionDetails> result = new Result<PackageOptionDetails>();
            if (packageOptionID == 0)
                result = ClientPackageManager.InsertPackageOption(packageoption);
            else
                result = ClientPackageManager.UpdatePackageOption(packageoption);
            if (result.Status == ResultStatus.Success)
            {
                ClearFields();
                lblMessage.CssClass = "successMsg";
            }

            else
                lblMessage.CssClass = "errorMsg";

            lblMessage.Text = result.Message;
        }

        protected void ClearFields()
        {
            txtDuration.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtDiscountPercentage.Text = string.Empty;
            txtPromoCode.Text = string.Empty;
            txtPromoStartDate.Text = string.Empty;
            txtPromoEndDate.Text = string.Empty;
            chkPublished.Checked = false;
            editorDescription.Content = string.Empty;
            Session.Remove("PackageOptionDetail_BackEnd");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearFields();
            Response.Redirect("~/Admin/SuperAdmin/PackageOptionManagement.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid)
                SavePackageOption();
        }
    }
}