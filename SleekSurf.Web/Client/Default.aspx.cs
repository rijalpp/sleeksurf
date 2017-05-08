using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using SleekSurf.Manager;
using System.Web.UI.HtmlControls;
using System.Web.Security;

namespace SleekSurf.Web.Client
{
    public partial class Default : WebBasePage
    {
        const int PAGED_ITEMS = 3;
        string uniqueIdentity = null;
        Result<PromotionDetails> promoList = new Result<PromotionDetails>();

        protected void Page_Load(object sender, EventArgs e)
        {
            uniqueIdentity = Page.RouteData.Values["uniqueIdentity"] as string;

            Menu tempMenu = (Menu)Master.FindControl("NavigationMenu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("Home"))].Selected = true;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (WebContext.ClientProfile != null)
                LoadClient(WebContext.ClientProfile.UniqueIdentity);
            else
                Redirector.GoToSleekSurfWebsite();
        }

        protected void Promotion_Command(object sender, CommandEventArgs e)
        {
            string promoID = (string)e.CommandArgument;
            if (!WebContext.ContainsInSession("ClientPromotionID"))
                Session.Add("ClientPromotionID", promoID);
            else
                Session["ClientPromotionID"] = promoID;

            Response.RedirectToRoute("BusinessProfilePromotionInDetails", new { uniqueIdentity = this.uniqueIdentity });
        }

        private void LoadClient(string uniqueIdentity)
        {
            if (WebContext.ClientProfile != null)
            {
                promoList = ClientManager.SelectCurrentAndUpcomingPromotions(WebContext.ClientProfile.ClientID, 30);

                if (promoList.EntityList.Count == 0)
                {
                    PromotionSlide1.Visible = true;
                    PromotionSlide2.Visible = true;
                }
                else
                {
                    rptrPromotionSlider.DataSource = promoList.EntityList.Take(5);
                    rptrPromotionSlider.DataBind();
                }

                string address = WebContext.ClientProfile.Address;
                lblAddress.Text = address;
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Client");
                body.Attributes.Add("onload", "Initialize(\'" + address + "\');");

                ddlMode.Attributes.Add("onchange", "CalculateRoute(\'" + address + "\');");

                HyperLinkViewLargeMap.CssClass = "FancyboxNewEdit viewInLarge";

                if (promoList.EntityList.Count > 0)
                {
                    lblPromotionHeader.Text = "Our Promotions";
                    ContactDetails.Visible = false;

                    List<string> pages = new List<string>();
                    int pageNumber = 1;
                    for (int i = 0; i < promoList.EntityList.Count; i += PAGED_ITEMS)
                    {
                        pages.Add(pageNumber.ToString());
                        pageNumber++;
                    }

                    rptrPageBlock.DataSource = pages;
                    rptrPageBlock.DataBind();
                }
                else
                {
                    lblPromotionHeader.Text = "Contact Details";
                    Slider.Visible = false;

                    lblContactPerson.Text = GetContactPerson(WebContext.ClientProfile.ContactPerson);
                    lblBusinessEmail.Text = WebContext.ClientProfile.BusinessEmail;
                    lblContactNumber.Text = WebContext.ClientProfile.ContactOffice;

                    if (!string.IsNullOrEmpty(WebContext.ClientProfile.ContactFax))
                        lblFax.Text = WebContext.ClientProfile.ContactFax;
                    else
                        lblFax.Text = "Not available";
                }
            }
            else
                Redirector.GoToHomePage();
        }

        public string GetShortDescription(string description, int length)
        {
            return Helpers.GetShortDescription(description, length);
        }

        protected string GetContactPerson(Guid contactPerson)
        {
            CustomUserProfile profile = CustomUserProfile.GetUserProfile(Membership.GetUser(contactPerson).UserName);
            return profile.FirstName + " " + profile.MiddleName + " " + profile.LastName;
        }

        protected void rptrPageBlock_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptrPromoListTemp = (Repeater)e.Item.FindControl("rptrPromoList");
                rptrPromoListTemp.DataSource = promoList.EntityList.Skip(e.Item.ItemIndex * PAGED_ITEMS).Take(PAGED_ITEMS);
                rptrPromoListTemp.DataBind();
            }
        }

        
    }
}