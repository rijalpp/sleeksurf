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
    public partial class NewEditCategory : System.Web.UI.Page
    {
        string CategoryID = null;
        static CategoryDetails category = new CategoryDetails();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Category"] != null)
            {
                CategoryDetails category = (CategoryDetails)Session["Category"];
                CategoryID = category.CategoryID;
                if (!IsPostBack)
                {
                    BindCategory();
                }
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("CategoryMgmt"))].Selected = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearFields();
            Redirector.GoToRequestedPage("~/Admin/SuperAdmin/Categorymanagement.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid)
                SaveCategory();
        }

        private void BindCategory()
        {
            Result<CategoryDetails> result = ClientManager.GetCategory(CategoryID);
            category = result.EntityList[0];
            if (result.Status == ResultStatus.Success)
            {
                txtCategoryName.Text = category.CategoryName;
                txtCategoryDescription.Text = category.Description;
            }
            else
            {
                lblMessage.CssClass = "errorMsg";
                lblMessage.Text = result.Message;
            }
        }

        private void SaveCategory()
        {
            category.CategoryName = txtCategoryName.Text;
            category.Description = txtCategoryDescription.Text;

            Result<CategoryDetails> result = new Result<CategoryDetails>();
            if (CategoryID == null)
            {
                if (ucFileUpload.ImageControl.HasFile && ucFileUpload.ImageControl != null)
                {
                    HttpPostedFile file = ucFileUpload.ImageControl.PostedFile;
                    category.CategoryImage = Helpers.ResizeImage(file.InputStream, 200);
                    category.CategoryID = System.DateTime.Now.ToString("CT-ddMMyyy-HHmmss");
                    result = ClientManager.InsertCategory(category);

                    if (result.Status == ResultStatus.Success)
                    {
                        lblMessage.CssClass = "successMsg";
                        ClearFields();
                    }
                    else
                        lblMessage.CssClass = "errorMsg";

                    lblMessage.Text = result.Message;
                }
                else
                {
                    lblMessage.Text = "Image is required.";
                    lblMessage.CssClass = "errorMsg";
                }
            }
            else
            {
                if (ucFileUpload.ImageControl.HasFile && ucFileUpload.ImageControl != null)
                {
                    HttpPostedFile file = ucFileUpload.ImageControl.PostedFile;
                    category.CategoryImage = Helpers.ResizeImage(file.InputStream, 200);

                }
                category.CategoryID = CategoryID;
                result = ClientManager.UpdateCategory(category);
                if (result.Status == ResultStatus.Success)
                {
                    lblMessage.CssClass = "successMsg";
                    ClearFields();
                }
                else
                    lblMessage.CssClass = "errorMsg";

                lblMessage.Text = result.Message;
            }
        }

        private void ClearFields()
        {
            txtCategoryDescription.Text = string.Empty;
            txtCategoryName.Text = string.Empty;
            Session.Remove("Category");
        }
    }
}