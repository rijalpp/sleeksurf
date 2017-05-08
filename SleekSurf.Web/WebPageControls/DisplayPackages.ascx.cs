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
    public partial class DisplayPackages : System.Web.UI.UserControl
    {
        static PackageOptionDetails tempOptionDetails = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tempOptionDetails = new PackageOptionDetails();
                BindPackageList();
            }
        }

        protected void rptrPackages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ListItemType ltype = e.Item.ItemType;
            if (ltype == ListItemType.Item || ltype == ListItemType.AlternatingItem)
            {
                string packageCode = ((Label)e.Item.FindControl("lblPackageCode")).Text;

                List<PackageOptionDetails> optionList = ClientPackageManager.SelectPackageOptionsByPublication(packageCode, true).EntityList;

                RadioButtonList rbtnlPackageOption = (RadioButtonList)e.Item.FindControl("rbtnlPackageOption");
                RequiredFieldValidator rfvPackageOption = (RequiredFieldValidator)e.Item.FindControl("rfvPackageOption");

                Repeater rptPackageIcons = (Repeater)e.Item.FindControl("rptPackageIcons");
                rptPackageIcons.DataSource = ClientPackageManager.SelectPackagePicturesByPackage(packageCode);
                rptPackageIcons.DataBind();

                Button btnPurchase = (Button)e.Item.FindControl("btnPurchase");
                btnPurchase.ValidationGroup = packageCode;
                rfvPackageOption.ValidationGroup = packageCode;

                foreach (PackageOptionDetails p in optionList)
                {
                    rbtnlPackageOption.Items.Add(new ListItem("<span style=' font-weight:bold;'>" + p.Duration + "</span> Months Subscription - <span style=' font-weight:bold;'>" + p.StandardPrice.ToString("c") + "<span style='margin-left: 20px'>" + p.Comments + "</span></span>", p.PackageOptionID.ToString()));
                }

            }
        }

        protected void rptrPackages_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            TextBox txtPromoCode = (TextBox)e.Item.FindControl("txtPromoCode");
            Literal templtrPackageTitle = (Literal)e.Item.FindControl("ltrPackageTitle");
            RadioButtonList rbtnlPackageOption = (RadioButtonList)e.Item.FindControl("rbtnlPackageOption");
            tempOptionDetails = ClientPackageManager.SelectPackageOption(int.Parse(rbtnlPackageOption.SelectedValue.ToString())).EntityList[0];
            tempOptionDetails.FinalPrice = tempOptionDetails.StandardPrice;
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
                        ((Label)e.Item.FindControl("lblPromoCodeMessage")).Text = "Promo code expired!";
                        return;
                    }
                }
                else
                {
                    ((Label)e.Item.FindControl("lblPromoCodeMessage")).Text = "Promo code mismatched!";
                    return;
                }

            }

            //Session.Add("PackageOptionDetails", tempOptionDetails);
            mViewPackage.ActiveViewIndex++;
            if (mViewPackage.GetActiveView().ID == "vConfirmation")
            {
                rptPackageIcons.DataSource = ClientPackageManager.SelectPackagePicturesByPackage(tempOptionDetails.PackageCode);
                rptPackageIcons.DataBind();

                ltrPacakageTitle.Text = templtrPackageTitle.Text;
                ltrPackageOption.Text = tempOptionDetails.Duration + " Months Subscription";
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

        private void BindPackageList()
        {
 
            rptrPackages.DataSource = ClientPackageManager.SelectPackagesByPublication(true).EntityList;
            rptrPackages.DataBind();
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            mViewPackage.ActiveViewIndex--;
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

                    if (recentOrder == null || recentOrder.ExpiryDate < System.DateTime.Now)
                    {
                        orderItem.ExpiryDate = System.DateTime.Now.AddMonths(int.Parse(tempOptionDetails.Duration));

                    }
                    else
                    {
                        orderItem.ExpiryDate = recentOrder.ExpiryDate.AddMonths(int.Parse(tempOptionDetails.Duration));
                    }

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
                    if (recentOrder == null)
                        orderItem.Comments = "New";
                    else
                    {
                        orderItem.Comments = "Renew";
                    }

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