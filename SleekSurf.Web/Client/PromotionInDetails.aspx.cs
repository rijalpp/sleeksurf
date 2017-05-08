using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using SleekSurf.Entity;
using SleekSurf.Manager;

namespace SleekSurf.Web.Client
{
    public partial class PromotionInDetails : WebBasePage
    {
        string uniqueIdentity = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            uniqueIdentity = Page.RouteData.Values["uniqueIdentity"] as string;

            Menu tempMenu = (Menu)Master.Master.FindControl("NavigationMenu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("Promotions"))].Selected = true;

            if (WebContext.ContainsInSession("ClientPromotionID"))
            {
                if (!IsPostBack)
                {
                    string promotionID = (string)Session["ClientPromotionID"];
                    PromotionDetails tempPromotion = ClientManager.SelectPromotion(promotionID, ClientManager.SelectClientByUniqueIdentity(uniqueIdentity).EntityList[0].ClientID).EntityList[0];

                    PromotionImage.ImageUrl = "~/DisplayImage.aspx?ID=" + tempPromotion.PromotionID + "&SECTION=TITLE";
                    PromotionImage.AlternateText = ltrPromoTitle.Text = tempPromotion.Title;
                    ltrStartDate.Text = tempPromotion.StartDate.ToShortDateString();
                    ltrEndDate.Text = tempPromotion.EndDate.ToShortDateString();
                    ltrDescription.Text = tempPromotion.Description;

                    if (tempPromotion.SupportingImage != null)
                    {
                        System.Drawing.Image sketchSupportImage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(tempPromotion.SupportingImage));
                        string makeURL = ResolveClientUrl("~/DisplayImage.aspx?ID=" + tempPromotion.PromotionID + "&SECTION=SUPPORTING");
                        string jScript = "$(document).ready(function () { window.parent.jQuery.fancybox('<img src=\"" + makeURL + "\"  alt=\"" + tempPromotion.Title + "\" />', {'autoDimensions': false, 'width':" + sketchSupportImage.Width + ", 'height':" + sketchSupportImage.Height + ", 'padding': 2, 'modal': false, 'scrolling': 'no', 'centerOnScroll': true, 'onStart':function(){$.fancybox.showActivity();}}); });";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "StartUpImage", jScript, true);
                    }

                    Result<MediaGalleryDetails> imageGallery = EventManager.SelectMediaGalleriesByPromotionIDWithPublication(tempPromotion.PromotionID, true, "Image");

                    if (imageGallery.EntityList.Count > 0)
                    {
                        rptrMediaGallery.DataSource = imageGallery.EntityList;
                        rptrMediaGallery.DataBind();
                    }
                    else
                        PromotionImageGallery.Visible = false;

                    Result<MediaGalleryDetails> videoGallery = EventManager.SelectMediaGalleriesByPromotionIDWithPublication(tempPromotion.PromotionID, true, "Video");

                    if (videoGallery.EntityList.Count > 0)
                    {
                        rptrVideoGallery.DataSource = videoGallery.EntityList;
                        rptrVideoGallery.DataBind();
                    }
                    else
                        PromotionVideoGallery.Visible = false;
                }
            }
            else
                Response.RedirectToRoute("BusinessProfilePromotions", new { uniqueIdentity = this.uniqueIdentity });
        }

        protected void rptrMediaGallery_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                MediaGalleryDetails galleryDetails = (MediaGalleryDetails)e.Item.DataItem;
                HyperLink tempMediaGallery = ((HyperLink)e.Item.FindControl("hlnkMediaGallery"));
                Image imgMediaGallery = (Image)e.Item.FindControl("imgMediaGallery");
                imgMediaGallery.ImageUrl = tempMediaGallery.NavigateUrl = BasePage.FullBaseUrl.Substring(0, BasePage.FullBaseUrl.Length-1)+ galleryDetails.MediaUrl;
            }
        }

        protected void rptrVideoGallery_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HyperLink tempMediaGallery = ((HyperLink)e.Item.FindControl("hlnkMediaGallery"));
                tempMediaGallery.Attributes.Add("EmbedSrc",Server.HtmlDecode(((MediaGalleryDetails)e.Item.DataItem).MediaUrl));
            }
        }
    }
}