using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.Manager;
using SleekSurf.FrameWork;
using System.Web.Security;

namespace SleekSurf.Web.Admin.Client
{
    public partial class MatchProfile : System.Web.UI.Page
    {
        public static PackageOptionDetails tempOptionDetails = null;
        private static ClientFeatureDetails clientFeature = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                clientFeature = ClientManager.SelectClientFeatureDetails(WebContext.Parent.ClientID).EntityList[0];
                BindPackageType();
            }

            if (clientFeature != null)
            {
                hlMatchProfile.Visible = !clientFeature.ClientProfile;
                hlMatchDomain.Visible = !clientFeature.ClientDomain;
            }

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("ClientAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("AccountMgmt"))].Selected = true;

            if (WebContext.Sibling != null)
            {
                Menu tmpMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
                tmpMenu.Items[tmpMenu.Items.IndexOf(tmpMenu.FindItem("ClientMgmt"))].Selected = true;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (tempOptionDetails != null)
            {
                ltrAddingCost.Text = tempOptionDetails.StandardPrice.ToString("c");
                if (tempOptionDetails.PromoCodeEntered)
                {
                    pnlPromoCodeEntered.Visible = true;
                    decimal discountedPrice = tempOptionDetails.StandardPrice - tempOptionDetails.FinalPrice;
                    ltrDiscountedValue.Text = discountedPrice.ToString("c");
                    ltrDiscountOffer.Text = "( " + tempOptionDetails.DiscountPercentage + "% with Promocode <span style='color:#000000;'>" + tempOptionDetails.PromoCode + "</span> )";
                }
                else
                {
                    pnlPromoCodeEntered.Visible = false;
                    if(tempOptionDetails.StandardPrice != tempOptionDetails.FinalPrice)
                        tempOptionDetails.FinalPrice = tempOptionDetails.FinalPrice + (tempOptionDetails.StandardPrice * ((decimal)tempOptionDetails.DiscountPercentage / 100));

                }

                ltrTotalCost.Text = tempOptionDetails.FinalPrice.ToString("c");
            }
        }

        private void BindPackageType()
        {
            PackageDetails package = ClientPackageManager.SelectPackageByFeatureType(FeatureType.ClientProfile.ToString()).EntityList[0];
            if (package != null)
            {
                tempOptionDetails = ClientPackageManager.SelectPackageOptionsByPackage(package.PackageCode).EntityList.Where(p=>p.Duration == "0").FirstOrDefault();
                tempOptionDetails.FinalPrice = tempOptionDetails.StandardPrice;
                rptPackageIcons.DataSource = ClientPackageManager.SelectPackagePicturesByPackage(package.PackageCode);
                rptPackageIcons.DataBind();
            }
        }

        protected void txtPromoCode_TextChanged(object sender, EventArgs e)
        {
            if (txtPromoCode.Text.Length > 0 && txtPromoCode.Text.Trim() != "I have promo code!")
            {

                if (tempOptionDetails.PromoCode == txtPromoCode.Text)
                {
                    if (DateTime.Now >= tempOptionDetails.PromoCodeStartDate && DateTime.Now <= tempOptionDetails.PromoCodeEndDate)
                    {
                        tempOptionDetails.FinalPrice = tempOptionDetails.StandardPrice - (tempOptionDetails.StandardPrice * ((decimal)tempOptionDetails.DiscountPercentage / 100));
                        tempOptionDetails.PromoCodeEntered = true;
                    }

                    else
                    {
                        lblPromoCodeMessage.Text = "Promo code expired!";
                        tempOptionDetails.PromoCodeEntered = false;
                        return;
                    }
                }
                else
                {
                    lblPromoCodeMessage.Text = "Promo code mismatched!";
                    tempOptionDetails.PromoCodeEntered = false;
                    return;
                }

            }
            else
                tempOptionDetails.PromoCodeEntered = false;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtPromoCode.Text.Length > 0 && txtPromoCode.Text.Trim() != "I have promo code!")
            {

                if (tempOptionDetails.PromoCode == txtPromoCode.Text)
                {
                    if (DateTime.Now >= tempOptionDetails.PromoCodeStartDate && DateTime.Now <= tempOptionDetails.PromoCodeEndDate)
                    {
                    }

                    else
                    {
                        lblPromoCodeMessage.Text = "Promo code expired!";
                        tempOptionDetails.PromoCodeEntered = false;
                        return;
                    }
                }
                else
                {
                    lblPromoCodeMessage.Text = "Promo code mismatched!";
                    tempOptionDetails.PromoCodeEntered = false;
                    return;
                }

            }

            //Save
            SavePackageType();

        }

        private void SavePackageType()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string clientID = WebContext.Parent.ClientID;
                if (!string.IsNullOrEmpty(clientID))
                {
                    PackageOrderDetails orderItem = new PackageOrderDetails();
                    PackageDetails tempPackage = ClientPackageManager.SelectPackageByFeatureType(FeatureType.ClientDomain.ToString()).EntityList[0];
                   
                    orderItem.OrderID = System.DateTime.Now.ToString("PO-ddMMyyy-HHmmssfff");
                    orderItem.CreatedBy = HttpContext.Current.User.Identity.Name;
                    orderItem.OrderStatus = StatusOrder.WaitingForPayment.ToString();
                    orderItem.Client = WebContext.Parent;

                    orderItem.StandardPrice = tempOptionDetails.StandardPrice;
                    if (tempOptionDetails.PromoCodeEntered)
                        orderItem.DiscountPercentage = tempOptionDetails.DiscountPercentage;
                    orderItem.FinalPrice = tempOptionDetails.FinalPrice;
                    orderItem.FinalPriceAfterDeduction = tempOptionDetails.FinalPrice;
                    orderItem.RegistrationDate = System.DateTime.Now;

                    if (tempPackage != null)
                    {
                        Result<PackageOrderDetails> tempPackageOrderResult = ClientPackageManager.SelectRecentPackageOrder(WebContext.Parent.ClientID, tempPackage.PackageCode);
                        if (tempPackageOrderResult.Status == ResultStatus.Success)
                        {
                            orderItem.ExpiryDate = tempPackageOrderResult.EntityList[0].ExpiryDate;
                        }
                        else
                            orderItem.ExpiryDate = System.DateTime.Now.AddMonths(1);
                    }
                    else
                        orderItem.ExpiryDate = System.DateTime.Now.AddMonths(1); 

                    PackageDetails package = new PackageDetails();
                    package = ClientPackageManager.SelectPackage(tempOptionDetails.PackageCode).EntityList[0];
                    orderItem.PackageCode = package.PackageCode;
                    orderItem.PackageName = package.PackageName;
                    orderItem.Duration = tempOptionDetails.Duration;

                    if (tempOptionDetails.PromoCodeEntered)
                    {
                        orderItem.PromoCode = tempOptionDetails.PromoCode;
                        orderItem.PromoCodeStartDate = tempOptionDetails.PromoCodeStartDate;
                        orderItem.PromoCodeEndDate = tempOptionDetails.PromoCodeEndDate;
                        orderItem.DiscountPercentage = tempOptionDetails.DiscountPercentage;
                    }

                    orderItem.PaymentOption = PaymentOptionStatus.PayPal.ToString();
                    orderItem.Comments = "Matched";

                    Result<PackageOrderDetails> result = ClientPackageManager.InsertPackageOrder(orderItem);
                    if (result.Status == ResultStatus.Success)
                    {
                        Response.Redirect(ClientPackageManager.GetPayPalPaymentUrl(orderItem), false);
                    }

                }
                else
                {
                    lblMessage.Text = "Only the people with business account can purchase the packages.";
                    lblMessage.CssClass = "errorMsg";
                }
            }
            else
            {
                Redirector.GoToLoginPage();
            }
        }
    }
}