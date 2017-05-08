using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.Manager;
using SleekSurf.FrameWork;
using System.Transactions;

namespace SleekSurf.Web.Admin.SuperAdmin
{
    public partial class PackageIconManagement : System.Web.UI.Page
    {
        string packageCode = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Package"] != null)
            {
                packageCode = ((PackageDetails)Session["Package"]).PackageCode;
                if (!IsPostBack)
                    BindPackagePictures();
            }
            else
                Redirector.GoToRequestedPage("~/Admin/Superadmin/PackageManagement.aspx");
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("PackageMgmt"))].Selected = true;
        }

        protected void imgDeleteBtn_Click(object sender, EventArgs e)
        {
            string pictureID = "";
            string imgPath = "";
            List<string> promotionIDList = new List<string>();

            foreach (GridViewRow row in gvPackagePictureManagement.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            try
                            {
                                pictureID = gvPackagePictureManagement.DataKeys[row.RowIndex]["PictureID"].ToString();
                                ClientPackageManager.DeletePackagePicture(pictureID);
                                imgPath = string.Format("~/Uploads/PackageImages/{0}", pictureID);
                                if (System.IO.File.Exists(Server.MapPath(imgPath)))
                                    System.IO.File.Delete(Server.MapPath(imgPath));
                                scope.Complete();
                            }
                            catch (Exception ex)
                            {
                                Helpers.LogError(ex);
                                lblMessage.Text = "Error occured while deleting some Package Pictures.";
                            }
                        }
                    }
                }

            }

            BindPackagePictures();
        }

        protected void gvPackagePictureManagement_RowDataBound(object sender, GridViewRowEventArgs e )
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((CheckBox)e.Row.FindControl("cbDelete")).Attributes.Add("onclick", "javascript:UpdateSelectAllAndDeleteControl('" + gvPackagePictureManagement.ClientID + "', 'cbDelete', 'checkAll', 'imgDeleteBtn');");
                if (e.Row.RowState == DataControlRowState.Edit)
                    ((TextBox)e.Row.FindControl("txtDisplayOrder")).Attributes.Add("onkeypress", "javascript:return allownumbers(event);");
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
                ((ImageButton)e.Row.FindControl("imgDeleteBtn")).OnClientClick = "return DeleteConfirmation('" + gvPackagePictureManagement.ClientID + "', 'cbDelete');";
        }

        protected void gvPackagePictureManagement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            string pictureID = gvPackagePictureManagement.DataKeys[e.NewSelectedIndex].Values[0].ToString();
            Session.Add("PackageIcon", pictureID);
            Redirector.GoToRequestedPage("~/Admin/SuperAdmin/NewEditPackageIcon.aspx");
        }

        protected void gvPackagePictureManagement_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPackagePictureManagement.EditIndex = e.NewEditIndex;
            ViewState["Index"] = e.NewEditIndex;
            //rebind the gridview
            BindPackagePictures();
        }

        protected void gvPackagePictureManagement_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                string pictureID = gvPackagePictureManagement.DataKeys[e.RowIndex]["PictureID"].ToString();
                string packageCode = gvPackagePictureManagement.DataKeys[e.RowIndex]["PackageCode"].ToString();
                UpdatePackagePicture(pictureID, packageCode, Convert.ToInt16(ViewState["Index"])); 
                gvPackagePictureManagement.EditIndex = -1;
                // REBIND THE GRIDVIEW
                BindPackagePictures();
            }
        }

        private void UpdatePackagePicture(string pictureID, string packageCode, int index )
        {
            string packageDescription = ((TextBox)gvPackagePictureManagement.Rows[index].FindControl("txtPictureDescription")).Text;
            int displayOrder = 0;
            if (!string.IsNullOrEmpty(((TextBox)gvPackagePictureManagement.Rows[index].FindControl("txtDisplayOrder")).Text))
                displayOrder = Convert.ToInt16(((TextBox)gvPackagePictureManagement.Rows[index].FindControl("txtDisplayOrder")).Text);
            ClientPackageManager.UpdatePackagePicture(pictureID, packageCode, packageDescription, displayOrder);
        }

        protected void gvPackagePictureManagement_RowCancellingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPackagePictureManagement.EditIndex = -1;
            // REBIND THE GRIDVIEW
            BindPackagePictures();
        }


        private void BindPackagePictures()
        {
            gvPackagePictureManagement.DataSource = ClientPackageManager.SelectPackagePicturesByPackage(packageCode);
            gvPackagePictureManagement.DataBind();
        }
    }
}