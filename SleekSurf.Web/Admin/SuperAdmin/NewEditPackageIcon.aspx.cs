using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SleekSurf.Entity;
using SleekSurf.Manager;
using SleekSurf.FrameWork;
using System.IO;

namespace SleekSurf.Web.Admin.SuperAdmin
{
    public partial class NewEditPackageIcon : System.Web.UI.Page
    {
        string pictureID = "";
        string packageCode = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Package"] != null)
            {
                packageCode = ((PackageDetails)Session["Package"]).PackageCode;
            }
            else
                Redirector.GoToRequestedPage("~/Admin/Superadmin/PackageManagement.aspx");
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("PackageMgmt"))].Selected = true;
        }

        private void SavePackagePicture()
        {
            if (fuUploadMedia.HasFile)
            {
                string fileExtension = Path.GetExtension(fuUploadMedia.PostedFile.FileName);
                pictureID = System.DateTime.Now.ToString("PPI-ddMMyy-HHmmssfff") + fileExtension;
                string toolTip = txtTooltip.Text;
                int i = ClientPackageManager.InsertPackagePicture(pictureID, toolTip);
                if (i > 0)
                {
                    string dirUrl = BasePage.BaseUrl + "Uploads/PackageImages";
                    string dirPath = Server.MapPath(dirUrl);
                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);

                    string outputfileName = dirPath + "/" + pictureID;
                    fuUploadMedia.SaveAs(outputfileName);
                   // Helpers.ResizeImage(outputfileName, outputfileName, 40);
                    ClientPackageManager.AssignPictureToPackage(pictureID, packageCode, Convert.ToInt16(txtDisplayOrder.Text));

                    ClearFields();
                }
            }
            else
                lblMessage.Text = "The picture is required!";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid)
                SavePackagePicture();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Redirector.GoToRequestedPage("~/Admin/Superadmin/PackageIconManagement.aspx");
        }

        private void ClearFields()
        {
            txtDisplayOrder.Text = "0";
            txtTooltip.Text = string.Empty;
        }
    }
}