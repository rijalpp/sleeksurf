using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using SleekSurf.Manager;
using System.Web.Security;

namespace SleekSurf.Web.WebPageControls
{
    public partial class DisplaySMS : System.Web.UI.UserControl
    {
        static PackageOptionDetails tempOptionDetails = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tempOptionDetails = new PackageOptionDetails();
                BindSMSOptions();
            }
        }

        private void BindSMSOptions()
        {
            foreach (SMSOption option in Enum.GetValues(typeof(SMSOption)))
            {
                string tempOption = Enum.GetName(typeof(SMSOption), option).Replace('C', ' ').Trim();
                rbtnlSMSOption.Items.Add(new ListItem("<span style=' font-weight:bold;'>" + tempOption + "</span> SMS Credit - <span style=' font-weight:bold;'>" + string.Format("{0:c}", Convert.ToDecimal(option)), ((int)option).ToString()));
            }
        }
        protected void btnRecharge_Click(object sender, EventArgs e)
        {
            tempOptionDetails.PackageCode = "SMS";
            tempOptionDetails.Duration = Enum.GetName(typeof(SMSOption), Convert.ToInt32(rbtnlSMSOption.SelectedValue)).Replace('C', ' ').Trim();
            tempOptionDetails.StandardPrice = Convert.ToDecimal(rbtnlSMSOption.SelectedValue);
            tempOptionDetails.FinalPrice = tempOptionDetails.StandardPrice;
            tempOptionDetails.PromoCode = Configuration.GetConfigurationSetting("SMSPromoCode", typeof(string)) as string;
            tempOptionDetails.PromoCodeStartDate = Configuration.GetConfigurationSetting("SMSPromoCodeStartDate", typeof(DateTime)) as DateTime?;
            tempOptionDetails.PromoCodeEndDate = Configuration.GetConfigurationSetting("SMSPromoCodeEndDate", typeof(DateTime)) as DateTime?;
            tempOptionDetails.DiscountPercentage = Convert.ToDouble(Configuration.GetConfigurationSetting("SMSPromoCodeDiscount", typeof(double)));
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
                        return;
                    }
                }
                else
                {
                    lblPromoCodeMessage.Text = "Promo code mismatched!";
                    return;
                }

            }

            mViewSMS.ActiveViewIndex++;

            if (mViewSMS.GetActiveView().ID == "vConfirmation")
            {
                ltrRechargeOption.Text = tempOptionDetails.Duration + " SMS Credit";
                ltrStandardPrice.Text = tempOptionDetails.StandardPrice.ToString("c");
                if (tempOptionDetails.PromoCodeEntered)
                {
                    pnlPromoCodeEntered.Visible = true;
                    decimal discountedPrice = tempOptionDetails.StandardPrice - tempOptionDetails.FinalPrice;
                    ltrDiscountedValue.Text = discountedPrice.ToString("c");
                    ltrDiscountOffer.Text = "( " + tempOptionDetails.DiscountPercentage + "% with Promocode <span style='color:#000000;'>" + tempOptionDetails.PromoCode + "</span> )";
                }

                ltrTotalCost.Text = tempOptionDetails.FinalPrice.ToString("c");
            }
        }
        protected void btnChange_Click(object sender, EventArgs e)
        {
           mViewSMS.ActiveViewIndex--;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string clientID = ClientManager.SelectClientIDByUserID((Guid)(Membership.GetUser(HttpContext.Current.User.Identity.Name).ProviderUserKey));
                if (!string.IsNullOrEmpty(clientID))
                {
                    Result<PackageOrderDetails> resultRecentOrder = ClientPackageManager.SelectRecentPackageOrder(WebContext.Parent.ClientID, tempOptionDetails.PackageCode);
                    PackageOrderDetails recentOrder = null;
                    if (resultRecentOrder.Status == ResultStatus.Success && resultRecentOrder.EntityList.Count > 0)
                        recentOrder = resultRecentOrder.EntityList[0];

                    PackageOrderDetails orderItem = new PackageOrderDetails();
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

                    if (recentOrder != null || recentOrder.ExpiryDate < System.DateTime.Now)
                        orderItem.ExpiryDate = recentOrder.ExpiryDate;

                    orderItem.PackageCode = tempOptionDetails.PackageCode;
                    orderItem.PackageName = tempOptionDetails.PackageCode;//BECAUSE PACKAGENAME AND PACKAGE CODE HAVE SAME NAME IN SMS
                    orderItem.Duration = tempOptionDetails.Duration;

                    if (tempOptionDetails.PromoCodeEntered)
                    {
                        orderItem.PromoCode = tempOptionDetails.PromoCode;
                        orderItem.PromoCodeStartDate = tempOptionDetails.PromoCodeStartDate;
                        orderItem.PromoCodeEndDate = tempOptionDetails.PromoCodeEndDate;
                        orderItem.DiscountPercentage = tempOptionDetails.DiscountPercentage;
                    }

                    orderItem.PaymentOption = PaymentOptionStatus.PayPal.ToString();
                    orderItem.Comments = "Recharge";

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