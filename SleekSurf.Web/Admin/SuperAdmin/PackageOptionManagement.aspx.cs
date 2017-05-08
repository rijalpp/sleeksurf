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
    public partial class PackageOptionManagement : System.Web.UI.Page
    {
        string packageCode = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Package"] != null)
            {
                Session.Remove("PackageOptionDetail_BackEnd");// remove any previously selected packageOption.
                PackageDetails package = (PackageDetails)Session["Package"];
                packageCode = package.PackageCode;
                lblTitle.Text += ClientPackageManager.SelectPackage(packageCode).EntityList[0].PackageName;
                if (!IsPostBack)
                {
                    SearchPackageOptions();
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

        private void SearchPackageOptions()
        {
            switch (rdlPublished.SelectedValue)
            {
                case "PUBLISHED":
                case "UNPUBLISHED":
                    SearchPackageOptionsByPublication();
                    break;
                case "ALL":
                    SearchAllPackageOptions();
                    break;
                default:
                    SearchAllPackageOptions();
                    break;
            }


        }

        private void SearchAllPackageOptions()
        {
            Result<PackageOptionDetails> result = ClientPackageManager.SelectPackageOptionsByPackage(packageCode);
            if (result.Status == ResultStatus.Success)
            {

                if (result.EntityList.Count > 0)
                {
                    gvPackageOptionManagement.DataSource = result.EntityList;
                    gvPackageOptionManagement.DataBind();
                }
                else
                {
                    result.EntityList.Add(new PackageOptionDetails() { PackageOptionID = 1, PackageCode = "1-1-1" });
                    gvPackageOptionManagement.DataSource = result.EntityList;
                    gvPackageOptionManagement.DataBind();
                    int totalColumn = gvPackageOptionManagement.Rows[0].Cells.Count;

                    gvPackageOptionManagement.Rows[0].Cells.Clear();

                    gvPackageOptionManagement.Rows[0].Cells.Add(new TableCell());

                    gvPackageOptionManagement.Rows[0].Cells[0].ColumnSpan = totalColumn;

                    gvPackageOptionManagement.Rows[0].Cells[0].Visible = false;
                    lblMessage.Text = "No Package option record found for the Package. Please add the package option below.";
                    lblMessage.CssClass = "normalMsg";
                    if (gvPackageOptionManagement.Rows.Count > 1)
                    {
                        foreach (GridViewRow gvrow in gvPackageOptionManagement.Rows)
                        {
                            if (gvrow.RowIndex != 0)
                            {
                                gvrow.Visible = false;
                            }
                        }
                    }
                }
            }
        }

        private void SearchPackageOptionsByPublication()
        {
            // string name = pgObj.SearchKey;
            bool published = rdlPublished.SelectedValue == "PUBLISHED" ? true : false;
            Result<PackageOptionDetails> result = ClientPackageManager.SelectPackageOptionsByPublication(packageCode, published);
            if (result.Status == ResultStatus.Success)
            {
                if (result.EntityList.Count > 0)
                {
                    gvPackageOptionManagement.DataSource = result.EntityList;
                    gvPackageOptionManagement.DataBind();
                }
                else
                {
                    int totalColumn = gvPackageOptionManagement.Rows[0].Cells.Count;
                    gvPackageOptionManagement.Rows[0].Cells.Clear();
                    gvPackageOptionManagement.Rows[0].Cells.Add(new TableCell());
                    gvPackageOptionManagement.Rows[0].Cells[0].ColumnSpan = totalColumn;
                    gvPackageOptionManagement.Rows[0].Cells[0].Visible = false;
                    lblMessage.Text = "No Package option record found for the Package. <br /> Please add the package option below.";
                    lblMessage.CssClass = "normalMsg";
                    if (gvPackageOptionManagement.Rows.Count > 1)
                    {
                        foreach (GridViewRow gvrow in gvPackageOptionManagement.Rows)
                        {
                            if (gvrow.RowIndex != 0)
                            {
                                gvrow.Visible = false;
                            }
                        }
                    }
                }
            }
        }

        protected void gvPackageOptionManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                //Label txtDiscountPercentage = (Label)e.Row.FindControl("txtDiscountPercentage");
                Label lblDiscountPercentage = (Label)e.Row.FindControl("lblDiscountPercentage");
                string discountPercentage = gvPackageOptionManagement.DataKeys[e.Row.RowIndex]["DiscountPercentage"].ToString().Trim();
                if (discountPercentage == "0")
                    discountPercentage = string.Empty;
                lblDiscountPercentage.Text = discountPercentage;
            }

            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == (DataControlRowState.Alternate|DataControlRowState.Edit) || e.Row.RowState == DataControlRowState.Edit))
            {
                TextBox txtDiscountPercentage = (TextBox)e.Row.FindControl("txtDiscountPercentage");
                string discountPercentage = gvPackageOptionManagement.DataKeys[e.Row.RowIndex]["DiscountPercentage"].ToString().Trim();
                if (discountPercentage == "0")
                    discountPercentage = string.Empty;
                txtDiscountPercentage.Text = discountPercentage;
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((ImageButton)e.Row.FindControl("imgDeleteBtn")).OnClientClick = "return DeleteConfirmation('" + gvPackageOptionManagement.ClientID + "', 'cbDelete');";
            }
        }

        protected void gvPackageOptionManagement_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {
                if (Session["Package"] != null)
                {
                    PackageOptionDetails optionDetail = new PackageOptionDetails();
                    PackageDetails packageDetails = (PackageDetails)Session["Package"];
                    optionDetail.PackageCode = packageDetails.PackageCode;
                    optionDetail.Duration = ((TextBox)gvPackageOptionManagement.FooterRow.FindControl("txtDurationInsert")).Text;
                    optionDetail.StandardPrice = Convert.ToDecimal(((TextBox)gvPackageOptionManagement.FooterRow.FindControl("txtStandardPriceInsert")).Text);
                    TextBox txtDiscountPercentageInsert = (TextBox)gvPackageOptionManagement.FooterRow.FindControl("txtDiscountPercentageInsert");
                    if (txtDiscountPercentageInsert.Text.Length > 0)
                        optionDetail.DiscountPercentage = Convert.ToDouble(txtDiscountPercentageInsert.Text);
                    else
                        optionDetail.DiscountPercentage = 0.00;
                    TextBox txtPromoCodeInsert = (TextBox)gvPackageOptionManagement.FooterRow.FindControl("txtPromoCodeInsert");
                    optionDetail.PromoCode = txtPromoCodeInsert.Text;
                    TextBox txtPromoStartDateInsert = (TextBox)gvPackageOptionManagement.FooterRow.FindControl("txtPromoCodeStartDateInsert");
                    if (txtPromoStartDateInsert.Text.Length > 0)
                        optionDetail.PromoCodeStartDate = Convert.ToDateTime(txtPromoStartDateInsert.Text);
                    else
                        optionDetail.PromoCodeStartDate = null;
                    TextBox txtPromoEndDateInsert = (TextBox)gvPackageOptionManagement.FooterRow.FindControl("txtPromoCodeEndDateInsert");
                    if (txtPromoEndDateInsert.Text.Length > 0)
                        optionDetail.PromoCodeEndDate = Convert.ToDateTime(txtPromoEndDateInsert.Text);
                    else
                        optionDetail.PromoCodeEndDate = null;
                    optionDetail.Published = ((CheckBox)gvPackageOptionManagement.FooterRow.FindControl("chkPublishedInsert")).Checked;

                    if ((txtPromoCodeInsert.Text.Length > 0 && txtDiscountPercentageInsert.Text.Length > 0 && txtPromoStartDateInsert.Text.Length > 0 && txtPromoEndDateInsert.Text.Length > 0) ||
                        (txtPromoCodeInsert.Text.Length == 0 && txtDiscountPercentageInsert.Text.Length == 0 && txtPromoStartDateInsert.Text.Length == 0 && txtPromoEndDateInsert.Text.Length == 0))
                    { }
                    else
                    {
                        lblMessage.Text = "You should either enter the value of all four fields: Promocode, Discount Percentage, Start Date and End Date or leave all of them empty.";
                        lblMessage.CssClass = "errorMsg";
                        return;
                    }

                    Result<PackageOptionDetails> result = new Result<PackageOptionDetails>();
                    result = ClientPackageManager.InsertPackageOption(optionDetail);
                    if (result.Status == ResultStatus.Success)
                        lblMessage.CssClass = "successMsg";
                    else
                        lblMessage.CssClass = "errorMsg";

                    lblMessage.Text = result.Message;
                    //REBIND PACKAGES
                    SearchPackageOptions();
                }
            }
        }

        protected void gvPackageOptionManagement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            int packageOptionID = int.Parse(gvPackageOptionManagement.DataKeys[e.NewSelectedIndex]["PackageOptionID"].ToString());
            Session.Add("PackageOptionDetail_BackEnd", new PackageOptionDetails() { PackageOptionID = packageOptionID });
            Redirector.GoToRequestedPage("~/Admin/SuperAdmin/NewEditPackageOption.aspx");
        }

        protected void gvPackageOptionManagement_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPackageOptionManagement.EditIndex = e.NewEditIndex;
            SearchPackageOptions();
        }

        protected void gvPackageOptionManagement_RowCancellingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPackageOptionManagement.EditIndex = -1;
            SearchPackageOptions();
        }

        protected void gvPackageOptionManagement_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string packageOptionID = gvPackageOptionManagement.DataKeys[e.RowIndex]["PackageOptionID"].ToString();
            PackageOptionDetails optionDetail = ClientPackageManager.SelectPackageOption(Convert.ToInt32(packageOptionID)).EntityList[0];
            optionDetail.Duration = ((TextBox)gvPackageOptionManagement.Rows[e.RowIndex].FindControl("txtDuration")).Text;
            optionDetail.StandardPrice = Convert.ToDecimal(((TextBox)gvPackageOptionManagement.Rows[e.RowIndex].FindControl("txtStandardPrice")).Text);
            TextBox txtpromoCode = (TextBox)gvPackageOptionManagement.Rows[e.RowIndex].FindControl("txtPromoCode");

            TextBox txtDiscountPercentage = (TextBox)gvPackageOptionManagement.Rows[e.RowIndex].FindControl("txtDiscountPercentage");

            TextBox txtPromoStartDate = (TextBox)gvPackageOptionManagement.Rows[e.RowIndex].FindControl("txtPromoCodeStartDate");

            TextBox txtPromoEndDate = (TextBox)gvPackageOptionManagement.Rows[e.RowIndex].FindControl("txtPromoCodeEndDate");

            optionDetail.Published = ((CheckBox)gvPackageOptionManagement.Rows[e.RowIndex].FindControl("chkPublishedEdit")).Checked;
            if ((txtpromoCode.Text.Length > 0 && txtDiscountPercentage.Text.Length > 0 && txtPromoStartDate.Text.Length > 0 && txtPromoEndDate.Text.Length > 0) ||
                (txtpromoCode.Text.Length == 0 && txtDiscountPercentage.Text.Length == 0 && txtPromoStartDate.Text.Length == 0 && txtPromoEndDate.Text.Length == 0))
            {
                optionDetail.PromoCode = txtpromoCode.Text;
                if (txtDiscountPercentage.Text.Length > 0)
                    optionDetail.DiscountPercentage = Convert.ToDouble(txtDiscountPercentage.Text);
                else
                    optionDetail.DiscountPercentage = 0.00;
                if (txtPromoStartDate.Text.Length > 0)
                    optionDetail.PromoCodeStartDate = Convert.ToDateTime(txtPromoStartDate.Text);
                else
                    optionDetail.PromoCodeStartDate = null;
                if (txtPromoEndDate.Text.Length > 0)
                    optionDetail.PromoCodeEndDate = Convert.ToDateTime(txtPromoEndDate.Text);
                else
                    optionDetail.PromoCodeEndDate = null;
            }
            else
            {
                lblMessage.Text = "You should either enter the value of all four fields: Promocode, Discount Percentage, Start Date and End Date or leave all of them empty.";
                lblMessage.CssClass = "errorMsg";
                return;
            }
            Result<PackageOptionDetails> result = new Result<PackageOptionDetails>();
            result = ClientPackageManager.UpdatePackageOption(optionDetail);
            if (result.Status == ResultStatus.Success)
                lblMessage.CssClass = "successMsg";
            else
                lblMessage.CssClass = "errorMsg";
            lblMessage.Text = result.Message;

            gvPackageOptionManagement.EditIndex = -1;
            SearchPackageOptions();
        }

        protected void rdlPublished_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchPackageOptions();
        }

        protected void imgDeleteBtn_Click(object sender, EventArgs e)
        {
            List<int> optionIDList = new List<int>();
            int packageOptionID = 0;
            foreach (GridViewRow row in gvPackageOptionManagement.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        packageOptionID = Convert.ToInt32(gvPackageOptionManagement.DataKeys[row.RowIndex]["PackageOptionID"].ToString());
                        optionIDList.Add(packageOptionID);
                    }

                }

            }

            foreach (int id in optionIDList)
            {
                Result<PackageOptionDetails> result = ClientPackageManager.DeletePackageOption(id);
                if (result.Status != ResultStatus.Success)
                {
                    lblMessage.CssClass = "errorMsg";
                    lblMessage.Text = result.Message;
                }
            }

            // rebind the GridView
            SearchPackageOptions();
        }
    }
}