using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using SleekSurf.Entity;
using System.Web.UI.HtmlControls;
using SleekSurf.Manager;

namespace SleekSurf.Web.Admin.SuperAdmin
{
    public partial class CategoryManagement : BasePage
    {
        static PagingDetails pgobj = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pgobj = new PagingDetails();
                pgobj.SearchMode = "DEFAULT";
                LoadCategories();
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("CategoryMgmt"))].Selected = true;
        }

        protected void gvCategoryManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((CheckBox)e.Row.FindControl("cbDelete")).Attributes.Add("onclick", "javascript:UpdateSelectAllAndDeleteControl('" + gvCategoryManagement.ClientID + "', 'cbDelete', 'checkAll', 'imgDeleteBtn');");

                string categoryID = gvCategoryManagement.DataKeys[e.Row.RowIndex]["CategoryID"].ToString();

                HtmlImage iThumb = e.Row.FindControl("iThumb") as HtmlImage;
                iThumb.Src = "~/DisplayImage.aspx?ID=" + categoryID;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
                ((ImageButton)e.Row.FindControl("imgDeleteBtn")).OnClientClick = "return DeleteConfirmation('" + gvCategoryManagement.ClientID + "', 'cbDelete');";
        }

        protected void gvCategoryManagement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            string categoryID = gvCategoryManagement.DataKeys[e.NewSelectedIndex]["CategoryID"].ToString();
            Session.Add("Category", new CategoryDetails() { CategoryID = categoryID });
            Redirector.GoToRequestedPage("~/Admin/SuperAdmin/NewEditCategory.aspx");
        }

        protected void imgDeleteBtn_Click(object sender, EventArgs e)
        {
            List<string> categoryIDList = new List<string>();
            string categoryID = "";
            foreach (GridViewRow row in gvCategoryManagement.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        categoryID = gvCategoryManagement.DataKeys[row.RowIndex]["CategoryID"].ToString();
                        categoryIDList.Add(categoryID);
                    }

                }

            }
            foreach (string id in categoryIDList)
            {
                Result<CategoryDetails> result = ClientManager.DeleteCategory(id);
                if (result.Status != ResultStatus.Success)
                {
                    lblMessage.CssClass = "errorMsg";
                    lblMessage.Text = result.Message;
                }
            }

            // rebind the GridView
            LoadCategories();
        }

        private void LoadCategories()
        {
            Result<CategoryDetails> result = new Result<CategoryDetails>();
            switch (pgobj.SearchMode)
            {
                case "DEFAULT":
                    result = ClientManager.GetCategories();
                    break;
                case "KEYWORD":
                    result = ClientManager.GetCategories(pgobj.SearchKey);
                    break;
            }
            gvCategoryManagement.DataSource = result.EntityList;
            gvCategoryManagement.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtCategoryName.Text.Trim() != txtCategoryName.ToolTip)
            {
                pgobj.SearchMode = "KEYWORD";
                pgobj.SearchKey = txtCategoryName.Text.Trim();
                LoadCategories();
            }
        }

        protected void gvCategoryManagement_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCategoryManagement.PageIndex = e.NewPageIndex;
            LoadCategories();
        }


    }
}