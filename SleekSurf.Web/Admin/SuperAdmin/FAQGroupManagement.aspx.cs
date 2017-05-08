using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using SleekSurf.Manager;
using SleekSurf.Entity;
using SleekSurf.FrameWork;

namespace SleekSurf.Web.Admin.SuperAdmin
{
    public partial class FAQGroupManagement : System.Web.UI.Page
    {
        string faqID = "";
        string faqGroupID = "";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region    METHODS OF FAQ GROUPS

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("FAQMgmt"))].Selected = true;

            if (Session["FaqGroupDetails"] == null || !IsPostBack)
                pnlFaqs.Visible = false;
            if (!IsPostBack)
                SearchFaqGroup();

        }

        protected void imgDeleteBtn_Click(object sender, EventArgs e)
        {
            List<string> faqGroupIDList = new List<string>();
            foreach (GridViewRow row in gvFaqGroupManagement.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        faqGroupID = gvFaqGroupManagement.DataKeys[row.RowIndex]["FaqGroupID"].ToString();
                        faqGroupIDList.Add(faqGroupID);
                    }

                }

            }

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (string faqGroupID in faqGroupIDList)
                    {
                        ClientManager.DeleteFaqGroup(faqGroupID);
                    }
                    lblMessage.Text = faqGroupIDList.Count + " Record(s) successfully deleted.";
                    lblMessage.CssClass = "successMsg";
                    pnlFaqs.Visible = false;
                    pnlFaqAddEdit.Visible = false;
                    scope.Complete();
                }
                catch(Exception ex)
                {
                    Helpers.LogError(ex);
                    lblMessage.Text = "The Record(s) could not be deleted.";
                    lblMessage.CssClass = "errorMsg";
                }
            }

            // rebind the GridView
            SearchFaqGroup();
        }

        protected void gvFaqGroupManagement_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {
                //PackageOptionDetails optionDetail = new PackageOptionDetails();
                FAQGroupDetails faqGroup = new FAQGroupDetails();
                faqGroup.FaqGroupID = DateTime.Now.ToString("AQG-ddMMyy-HHmmssfff");
                faqGroup.Client = new ClientDetails() { ClientID = null };
                faqGroup.GroupName = ((TextBox)gvFaqGroupManagement.FooterRow.FindControl("txtGroupNameInsert")).Text;
                DropDownList ddlGroupRankInsert = (DropDownList)gvFaqGroupManagement.FooterRow.FindControl("ddlGroupRankInsert");

                faqGroup.GroupRank = int.Parse(ddlGroupRankInsert.SelectedValue);
                Result<FAQGroupDetails> result = new Result<FAQGroupDetails>();
                result = ClientManager.InsertFaqGroup(faqGroup);
                if (result.Status == ResultStatus.Success)
                    lblMessage.CssClass = "successMsg";
                else
                    lblMessage.CssClass = "errorMsg";

                lblMessage.Text = result.Message;
                //REBIND PACKAGES
                SearchFaqGroup();
            }
        }

        protected void gvFaqGroupManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow) //e.Row.RowState == DataControlRowState.Edit
            {
                ((CheckBox)e.Row.FindControl("cbDelete")).Attributes.Add("onclick", "javascript:UpdateSelectAllAndDeleteControl('" + gvFaqGroupManagement.ClientID + "', 'cbDelete', 'checkAll', 'imgDeleteBtn');");
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    Result<FAQGroupDetails> result = ClientManager.SelectFaqGroupWithClientNull();
                    DropDownList ddlGroupRank = (DropDownList)e.Row.FindControl("ddlGroupRank");
                    string faqGroupID = gvFaqGroupManagement.DataKeys[e.Row.RowIndex]["FaqGroupID"].ToString();
                    //int i = 0;
                    int selectedRank = 0;
                    foreach (FAQGroupDetails fg in result.EntityList)
                    {

                        if (faqGroupID == fg.FaqGroupID)
                        {
                            selectedRank = fg.GroupRank;
                        }
                    }

                    ddlGroupRank.DataSource = result.EntityList;
                    ddlGroupRank.DataTextField = "GroupRank";
                    ddlGroupRank.DataValueField = "GroupRank";
                    ddlGroupRank.SelectedValue = selectedRank.ToString();
                    ddlGroupRank.DataBind();
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((ImageButton)e.Row.FindControl("imgDeleteBtn")).OnClientClick = "return DeleteConfirmation('" + gvFaqGroupManagement.ClientID + "', 'cbDelete');";
                Result<FAQGroupDetails> result = ClientManager.SelectFaqGroupWithClientNull();
                DropDownList ddlGroupRank = (DropDownList)e.Row.FindControl("ddlGroupRankInsert");

                ddlGroupRank.DataSource = result.EntityList;
                ddlGroupRank.DataTextField = "GroupRank";
                ddlGroupRank.DataValueField = "GroupRank";
                ddlGroupRank.DataBind();
                if (ddlGroupRank.Items.Count == 1 && ddlGroupRank.Items[0].Value == "0")//THAT MEANS NO RECORD ARE RETRIVED FROM DATABASE AND ONE RECORD IS MAUALLY ADDED ON BINDING TO GRIDVIEW
                    ddlGroupRank.Items.Clear();

                ddlGroupRank.Items.Insert(0, new ListItem("New Rank", (ddlGroupRank.Items.Count + 1).ToString()));
            }
        }

        protected void gvFaqGroupManagement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            faqGroupID = gvFaqGroupManagement.DataKeys[e.NewSelectedIndex].Values[0].ToString();
            Session.Add("FaqGroupDetails", new FAQGroupDetails() { FaqGroupID = faqGroupID });

            SearchFaq(faqGroupID);
        }

        protected void gvFaqGroupManagement_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvFaqGroupManagement.EditIndex = e.NewEditIndex;
            ViewState["Index"] = e.NewEditIndex;
            //rebind the gridview
            SearchFaqGroup();
        }

        protected void gvFaqGroupManagement_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                faqGroupID = gvFaqGroupManagement.DataKeys[e.RowIndex]["FaqGroupID"].ToString();
                UpdateFaqGroup(faqGroupID, Convert.ToInt16(ViewState["Index"]));
                gvFaqGroupManagement.EditIndex = -1;
                // rebind the gridview
                SearchFaqGroup();
            }
        }

        protected void gvFaqGroupManagement_RowCancellingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvFaqGroupManagement.EditIndex = -1;
            // rebind gridview
            SearchFaqGroup();
        }

        private void SearchFaqGroup()
        {
            Result<FAQGroupDetails> result = ClientManager.SelectFaqGroupWithClientNull();
            if (result.Status == ResultStatus.Success)
            {
                if (result.EntityList.Count > 0)
                {
                    gvFaqGroupManagement.DataSource = ClientManager.SelectFaqGroupWithClientNull().EntityList;
                    gvFaqGroupManagement.DataBind();
                }
                else
                {
                    result.EntityList.Add(new FAQGroupDetails() { FaqGroupID = "1-1-1", GroupName = "default" });
                    gvFaqGroupManagement.DataSource = result.EntityList;
                    gvFaqGroupManagement.DataBind();
                    result.EntityList.Clear();//IT SHOULD BE CLEARED.

                    lblMessage.Text = "No Faq Group record found for the client. Please add the faq Group below.";
                    lblMessage.CssClass = "normalMsg";
                    if (gvFaqGroupManagement.Rows.Count > 1)
                    {
                        foreach (GridViewRow gvrow in gvFaqGroupManagement.Rows)
                        {
                            if (gvrow.RowIndex != 0)
                            {
                                gvrow.Visible = false;
                            }
                        }
                    }

                    gvFaqGroupManagement.Rows[0].Cells.Clear();
                    gvFaqGroupManagement.Rows[0].CssClass = "makeInvisible";
                }
            }

        }

        private void UpdateFaqGroup(string faqGroupID, int index)
        {
            FAQGroupDetails faqGroup = new FAQGroupDetails();
            faqGroup.FaqGroupID = faqGroupID;
            faqGroup.Client = new ClientDetails() { ClientID = null };
            faqGroup.GroupName = ((TextBox)gvFaqGroupManagement.Rows[index].FindControl("txtGroupName")).Text;
            faqGroup.GroupRank = int.Parse(((DropDownList)gvFaqGroupManagement.Rows[index].FindControl("ddlGroupRank")).SelectedValue);

            ClientManager.UpdateFaqGroup(faqGroup);
        }

        #endregion

        #region    METHODS OF FAQ

        protected void imgDeleteBtnFaq_Click(object sender, EventArgs e)
        {
            List<string> faqIDList = new List<string>();
            foreach (GridViewRow row in gvFaqManagement.Rows)
            {
                CheckBox chkDelete = (CheckBox)row.FindControl("cbDeleteFaq");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        faqID = gvFaqManagement.DataKeys[row.RowIndex]["FaqID"].ToString();
                        faqIDList.Add(faqID);
                    }

                }

            }

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (string thisfaqID in faqIDList)
                    {
                        ClientManager.DeleteFaq(thisfaqID);
                    }
                    lblMessageFaq.Text = faqIDList.Count.ToString() + " Record(s) successfully deleted";
                    lblMessageFaq.CssClass = "successMsg";
                    scope.Complete();
                }
                catch(Exception ex)
                {
                    Helpers.LogError(ex);
                    lblMessageFaq.Text = "The Record(s) could not be deleted";
                    lblMessageFaq.CssClass = "errorMsg";
                }
            }

            // rebind the GridView
            SearchFaq(((FAQGroupDetails)Session["FaqGroupDetails"]).FaqGroupID);
        }

        protected void gvFaqManagement_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {
                pnlFaqAddEdit.Visible = true;

                ddlRankFaq.DataSource = ClientManager.SelectFaqByFaqGroup(((FAQGroupDetails)Session["FaqGroupDetails"]).FaqGroupID).EntityList;
                ddlRankFaq.DataValueField = "FaqRank";
                ddlRankFaq.DataTextField = "FaqRank";
                ddlRankFaq.DataBind();
                ddlRankFaq.Items.Insert(0, new ListItem("New Rank", (ddlRankFaq.Items.Count + 1).ToString()));

                //CLEAR FIELDS
                editorAnswer.Content = string.Empty;
                txtFaqQuestion.Text = string.Empty;
                Session.Remove("FaqDetails");
            }
        }

        protected void gvFaqManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                ((CheckBox)e.Row.FindControl("cbDeleteFaq")).Attributes.Add("onclick", "javascript:UpdateSelectAllAndDeleteControl('" + gvFaqManagement.ClientID + "', 'cbDeleteFaq', 'checkAll', 'imgDeleteBtnFaq');");
            else if (e.Row.RowType == DataControlRowType.Footer)
                ((ImageButton)e.Row.FindControl("imgDeleteBtnFaq")).OnClientClick = "return DeleteConfirmation('" + gvFaqManagement.ClientID + "', 'cbDeleteFaq');";
        }

        protected void gvFaqManagement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            faqID = gvFaqManagement.DataKeys[e.NewSelectedIndex].Values[0].ToString();
            pnlFaqs.Visible = true;
            SelectFaq(faqID);
        }

        private void SearchFaq(string faqGroupID)
        {
            Result<FAQDetails> result = ClientManager.SelectFaqByFaqGroup(faqGroupID);
            if (result.Status == ResultStatus.Success)
            {
                gvFaqManagement.DataSource = result.EntityList;
                gvFaqManagement.DataBind();
                pnlFaqs.Visible = true;
                pnlFaqAddEdit.Visible = false;
            }

            if (result.EntityList.Count == 0)
            {
                pnlFaqAddEdit.Visible = true;
                ddlRankFaq.DataSource = ClientManager.SelectFaqByFaqGroup(((FAQGroupDetails)Session["FaqGroupDetails"]).FaqGroupID).EntityList;
                ddlRankFaq.DataValueField = "FaqRank";
                ddlRankFaq.DataTextField = "FaqRank";
                ddlRankFaq.DataBind();
                ddlRankFaq.Items.Insert(0, new ListItem("New Rank", (ddlRankFaq.Items.Count + 1).ToString()));
                btnHideFaq.Visible = false;
            }
            else
                btnHideFaq.Visible = true;

        }

        private void SelectFaq(string faqID)
        {
            FAQDetails faq = ClientManager.SelectFaq(faqID).EntityList[0];
            Session.Add("FaqDetails", faq);

            txtFaqQuestion.Text = faq.Question;
            editorAnswer.Content = faq.Answer;

            ddlRankFaq.DataSource = ClientManager.SelectFaqByFaqGroup(faq.FaqGroup.FaqGroupID).EntityList;
            ddlRankFaq.DataValueField = "FaqRank";
            ddlRankFaq.DataTextField = "FaqRank";
            ddlRankFaq.SelectedValue = faq.FaqRank.ToString();
            ddlRankFaq.DataBind();

            pnlFaqAddEdit.Visible = true;
        }

        private void SaveFaq()
        {
            FAQDetails faq = new FAQDetails();
            faq.Question = txtFaqQuestion.Text;
            faq.Answer = editorAnswer.Content;
            faq.FaqRank = int.Parse(ddlRankFaq.SelectedValue);
            faq.FaqGroup = (FAQGroupDetails)Session["FaqGroupDetails"];

            Result<FAQDetails> result = new Result<FAQDetails>();

            if (Session["FaqDetails"] != null)
            {
                faq.FaqID = ((FAQDetails)Session["FaqDetails"]).FaqID;
                result = ClientManager.UpdateFaq(faq);
            }

            else
            {
                faq.FaqID = DateTime.Now.ToString("AQ-ddMMyy-HHmmssfff");
                result = ClientManager.InsertFaq(faq);
            }
            if (result.Status == ResultStatus.Success)
            {
                SearchFaq(((FAQGroupDetails)Session["FaqGroupDetails"]).FaqGroupID);
                ClearFaqFields();
                lblMessageFaq.CssClass = "successMsg";
            }
            else
                lblMessageFaq.CssClass = "errorMsg";

            lblMessageFaq.Text = result.Message;
        }

        private void ClearFaqFields()
        {
            txtFaqQuestion.Text = string.Empty;
            editorAnswer.Content = string.Empty;
            Session.Remove("FaqDetails");
            pnlFaqAddEdit.Visible = false;
        }

        #endregion

        public string GetShortDescription(string description)
        {
            return Helpers.GetShortDescription(description, 10);
        }

        protected void btnHideFaq_Click(object sender, EventArgs e)
        {
            ClearFaqFields();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveFaq();
        }
    }
}