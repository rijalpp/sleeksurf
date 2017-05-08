using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using SleekSurf.Entity;
using SleekSurf.Manager;
using System.IO;
using System.Transactions;
using System.Text.RegularExpressions;

namespace SleekSurf.Web.Admin.Client
{
    public partial class NewEditPromotionGallery : BasePage
    {
        MediaGalleryDetails media = new MediaGalleryDetails();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Promotion"] != null)
            {
                media.Promotion = new PromotionDetails() { PromotionID = Session["Promotion"].ToString() };
                hfPromoID.Value = media.Promotion.PromotionID;

                if (Session["PromotionMedia"] != null)
                    media.MediaID = Session["PromotionMedia"].ToString();
                else
                {
                    lblTitle.Text += " - Add Mode";
                    imgThumb.Visible = false;
                }
            }

            if (!IsPostBack)
            {

                if (Session["PromotionMedia"] != null)
                {
                    lblTitle.Text += " - Update Mode";
                    BindMediaGallery();
                }

                CheckMediaUploadType();
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("ClientAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("PromotionMgmt"))].Selected = true;

            if (WebContext.Sibling != null)
            {
                Menu tmpMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
                tmpMenu.Items[tmpMenu.Items.IndexOf(tmpMenu.FindItem("ClientMgmt"))].Selected = true;
            }

        }

        private void BindMediaGallery()
        {
            media = EventManager.SelectMediaGallery(media.MediaID).EntityList[0];
            txtTitle.Text = media.Title;
            txtCaption.Text = media.Caption;
            if (media.MediaType == "Video")
            {
                ltrVideoThumb.Text = Server.HtmlDecode(media.MediaUrl);
                ltrVideoThumb.Visible = true;
            }
            else
            {
                imgThumb.ImageUrl = "~" + media.MediaUrl;
                imgThumb.Visible = true;
            }
            editorDescription.Content = media.Description;
            hfPromoID.Value = media.Promotion.PromotionID;
            ddlMediaType.SelectedValue = media.MediaType;
            chkPublish.Checked = media.IsActive;
        }

        private void SaveMedia()
        {
            if (Session["PromotionMedia"] != null)
            {
                media.MediaID = Session["PromotionMedia"].ToString();
                media = EventManager.SelectMediaGallery(media.MediaID).EntityList[0];
            }
            media.Title = txtTitle.Text;
            media.Caption = txtCaption.Text;
            media.MediaType = ddlMediaType.SelectedValue;
            media.Description = editorDescription.Content;
            if (txtPathUrl.Text.Length > 0 && (!fuUploadMedia.HasFile))
                media.MediaUrl = Server.HtmlEncode(Server.HtmlDecode(txtPathUrl.Text));

            media.Promotion = new PromotionDetails() { PromotionID = hfPromoID.Value };
            media.EventDetail = new EventDetails();
            media.IsActive = chkPublish.Checked;
            Result<MediaGalleryDetails> result = new Result<MediaGalleryDetails>();

            if (!string.IsNullOrWhiteSpace(media.MediaID))
            {
                if (txtPathUrl.Text.Length == 0 && (fuUploadMedia.HasFile))
                    media.MediaUrl = BasePage.BaseUrl + "Uploads/" + WebContext.Parent.ClientID + "/Promotions/" + media.Promotion.PromotionID + "/" + media.MediaID + ".jpg";
                result = EventManager.UpdateMediaGallery(media);
            }
            else
            {
                if (txtPathUrl.Text.Length == 0 && !(fuUploadMedia.HasFile))
                {
                    lblMessage.CssClass = "errorMsg";
                    lblMessage.Text = "You should either enter a video link or upload an image";
                    return;
                }
                else if (txtPathUrl.Text.Length == 0 && ddlMediaType.SelectedValue == "Video")
                {
                    lblMessage.CssClass = "errorMsg";
                    lblMessage.Text = "Please enter an video before you add or select Image Type";
                    return;
                }
                else
                {
                    media.MediaID = System.DateTime.Now.ToString("PP-ddMMyyy-HHmmssfff");
                    if (txtPathUrl.Text.Length == 0 && (fuUploadMedia.HasFile))
                        media.MediaUrl = BasePage.BaseUrl + "Uploads/" + WebContext.Parent.ClientID + "/Promotions/" + media.Promotion.PromotionID + "/" + media.MediaID + ".jpg";
                    result = EventManager.InsertMediaGallery(media);
                }
            }

            if (result.Status == ResultStatus.Success)
            {
                if (txtPathUrl.Text.Length == 0 && (fuUploadMedia.HasFile))
                {
                    string dirUrl = BasePage.BaseUrl + "Uploads/" + WebContext.Parent.ClientID + "/Promotions/" + media.Promotion.PromotionID;
                    string dirPath = Server.MapPath(dirUrl);
                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);

                    string outputfileName = dirPath + "/" + media.MediaID + ".jpg";
                    fuUploadMedia.SaveAs(outputfileName);
                    Helpers.ResizeImage(outputfileName, outputfileName, 600);
                }

                lblMessage.CssClass = "successMsg";
                lblTitle.Text += " - Add Mode";
                ClearFields();
                Session.Remove("PromotionMedia");
            }
            else
                lblMessage.CssClass = "errorMsg";
            lblMessage.Text = result.Message;
        }

        private void ClearFields()
        {
            txtTitle.Text = string.Empty;
            txtCaption.Text = string.Empty;
            txtPathUrl.Text = string.Empty;
            ddlMediaType.SelectedIndex = 0;
            chkPublish.Checked = false;
            editorDescription.Content = string.Empty;
            chkPublish.Checked = false;
            imgThumb.ImageUrl = string.Empty;
            imgThumb.Visible = false;
            ltrVideoThumb.Text = string.Empty;
            ltrVideoThumb.Visible = false;
            Session.Remove("PromotionMedia");
        }

        private void CheckMediaUploadType()
        {
            if (ddlMediaType.SelectedIndex == 0)
            {
                txtPathUrl.Enabled = false;
                txtPathUrl.Text = string.Empty;

                fuUploadMedia.Enabled = false;
            }
            else if (ddlMediaType.SelectedItem.Text == "Image")
            {
                txtPathUrl.Text = string.Empty;
                txtPathUrl.Enabled = false;
                fuUploadMedia.Enabled = true;
                ltrVideoThumb.Visible = false;
                imgThumb.Visible = true;
            }
            else if (ddlMediaType.SelectedItem.Text == "Video")
            {
                txtPathUrl.Enabled = true;
                fuUploadMedia.Enabled = false;
                ltrVideoThumb.Visible = true;
                imgThumb.Visible = false;
            }
        }

        protected void ddlMediaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckMediaUploadType();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearFields();
            Response.Redirect("~/Admin/Client/PromotionGalleryManagement.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if (IsValid)
                    {
                        if (txtPathUrl.Text.Length > 0 && fuUploadMedia.HasFile)
                        {
                            lblMessage.Text = "Please embed either a video link or upload an image. Don't add both.";
                            txtPathUrl.Text = string.Empty;
                            lblMessage.CssClass = "errorMsg";
                        }
                        else
                            SaveMedia();
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Helpers.LogError(ex);
                }
            }
        }
    }
}