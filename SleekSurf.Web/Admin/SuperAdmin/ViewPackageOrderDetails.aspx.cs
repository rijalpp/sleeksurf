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

namespace SleekSurf.Web.Admin.SuperAdmin
{
    public partial class ViewPackageOrderDetails : System.Web.UI.Page
    {
        PackageOrderDetails packageOrder = new PackageOrderDetails();
        string orderID = "";
        string clientID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PackageOrderDetails"] != null)
            {
                packageOrder = (PackageOrderDetails)Session["PackageOrderDetails"];
                orderID = packageOrder.OrderID;
                clientID = packageOrder.Client.ClientID;
            }
            if (!IsPostBack)
                bindOrderDetail();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("PackageOrderMgmt"))].Selected = true;
        }

        private void bindOrderDetail()
        {
            ClientDetails thisClient = ClientManager.SelectClient(clientID).EntityList[0];
            ucViewPackageOrder.ClientName = thisClient.ClientName;
            CustomUserProfile clientProfile = CustomUserProfile.GetUserProfile(Membership.GetUser(thisClient.ContactPerson).UserName);
            ucViewPackageOrder.ContactPerson = clientProfile.FirstName + " " + clientProfile.MiddleName + " " + clientProfile.LastName;
            ucViewPackageOrder.ContactPersonEmail = Membership.GetUser(thisClient.ContactPerson).Email;
            ucViewPackageOrder.ClientContactNo = thisClient.ContactOffice;
            ucViewPackageOrder.ClientEmail = thisClient.BusinessEmail;
            ucViewPackageOrder.ClientAddress = thisClient.Address;
    
            packageOrder = ClientPackageManager.SelectPackageOrder(orderID, clientID).EntityList[0];
            ucViewPackageOrder.PackageName = packageOrder.PackageName;
            ucViewPackageOrder.PackageDuration = packageOrder.Duration;
            ucViewPackageOrder.StandardPrice = packageOrder.StandardPrice;
            ucViewPackageOrder.ExpiryDate = packageOrder.ExpiryDate;

            if (!string.IsNullOrEmpty(packageOrder.PromoCode))
            {
                ucViewPackageOrder.PromoCode = packageOrder.PromoCode;
                ucViewPackageOrder.PromoCodeStartDate = packageOrder.PromoCodeStartDate;
                ucViewPackageOrder.PromoCodeEndDate = packageOrder.PromoCodeEndDate;
                ucViewPackageOrder.DiscountPercentage = ClientPackageManager.SelectPackageOption(packageOrder.PromoCode).EntityList[0].DiscountPercentage;
                ucViewPackageOrder.PromoDiscountedAmount = (packageOrder.StandardPrice * ((decimal)packageOrder.DiscountPercentage / 100));
            }

            ucViewPackageOrder.FinalPrice = packageOrder.FinalPrice;
            ucViewPackageOrder.TransactionID = packageOrder.TransactionID;
            ucViewPackageOrder.PurchasedMethod = packageOrder.PaymentOption;
            ucViewPackageOrder.AmountDeducted = packageOrder.AmountDeducted;
            ucViewPackageOrder.PurchasedStatus = packageOrder.OrderStatus;
            ucViewPackageOrder.AmountPaid = packageOrder.FinalPriceAfterDeduction;
            ucViewPackageOrder.PurchasedType = packageOrder.Comments;
            ucViewPackageOrder.PurchasedBy = packageOrder.CreatedBy;
            ucViewPackageOrder.PurchasedDate = packageOrder.RegistrationDate;
            if (packageOrder.OrderStatus == StatusOrder.Refunded.ToString())
            {
                ucViewPackageOrder.DisplayRefundedPanel(true);
                ucViewPackageOrder.AmountPaidByClient = packageOrder.FinalPriceAfterDeduction + packageOrder.AmountDeducted;
            }
        }
    }
}