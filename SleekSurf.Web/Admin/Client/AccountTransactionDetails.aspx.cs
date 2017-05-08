using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Manager;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using System.Web.Security;
using System.IO;

namespace SleekSurf.Web.Admin.Client
{
    public partial class AccountTransactionDetails : System.Web.UI.Page
    {
        PackageOrderDetails packageOrder = new PackageOrderDetails();
        ClientFeatureDetails clientFeature = null;
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
            if (Request.QueryString["OrderID"] != null && Request.QueryString["ClientID"] != null)
            {
                orderID = Request.QueryString["OrderID"];
                clientID = Request.QueryString["ClientID"];
            }
            if (!IsPostBack)
            {
                BindPaymentOption();
                clientFeature = ClientManager.SelectClientFeatureDetails(WebContext.Parent.ClientID).EntityList[0];
                bindOrderDetail();
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

        protected void btnRefund_Click(object sender, EventArgs e)
        {
            packageOrder = ClientPackageManager.SelectPackageOrder(orderID, clientID).EntityList[0];
            if (packageOrder.PackageName != "SMS")
            {
                PackageOrderDetails refundedPackageOrder = packageOrder;
                refundedPackageOrder.OrderStatus = StatusOrder.Refunded.ToString();
                refundedPackageOrder.PaymentOption = ddlPaymentOption.SelectedValue;
                refundedPackageOrder.CreatedBy = WebContext.CurrentUser.Identity.Name;
                refundedPackageOrder.CreatedDate = System.DateTime.Now;
                refundedPackageOrder.Comments = orderID;
                decimal deductionCharge = (refundedPackageOrder.FinalPriceAfterDeduction * 10) / 100;
                refundedPackageOrder.AmountDeducted = deductionCharge;
                int months = 0;
                DateTime packageStartDate = DateTime.Now;
                if (packageOrder.Duration == "0")
                {
                    refundedPackageOrder.FinalPriceAfterDeduction -= deductionCharge;
                }
                else
                {
                    if (refundedPackageOrder.PackageName.EndsWith(" BySleekSurf"))
                        packageStartDate = packageOrder.ExpiryDate.AddDays(-Convert.ToInt32(packageOrder.Duration));
                    else
                        packageStartDate = packageOrder.ExpiryDate.AddMonths(-Convert.ToInt32(packageOrder.Duration));

                    if (packageStartDate > DateTime.Now)
                    {
                        refundedPackageOrder.FinalPriceAfterDeduction -= deductionCharge;
                    }
                    else
                    {
                        months = refundedPackageOrder.ExpiryDate.Subtract(DateTime.Now).Days / 30;
                        decimal amountToRefund = months * (packageOrder.FinalPriceAfterDeduction / Convert.ToInt32(packageOrder.Duration));
                        //deductionCharge = months * (amountToRefund / Convert.ToInt32(packageOrder.Duration));
                        deductionCharge = (amountToRefund * 10 )/ 100;
                        refundedPackageOrder.FinalPriceAfterDeduction = amountToRefund - deductionCharge;
                        packageStartDate = DateTime.Now;
                        //disabling features if the package has expired.
                        PackageDetails package = ClientPackageManager.SelectPackage(refundedPackageOrder.PackageCode).EntityList[0];
                        ClientManager.ClientFeatureSetStatus(clientID, package.FeatureType, false);
                    }
                }

                refundedPackageOrder.OrderID = System.DateTime.Now.ToString("PO-ddMMyyyy-HHmmssfff");
               Result<PackageOrderDetails> result = ClientPackageManager.InsertPackageOrder(refundedPackageOrder);
               if (result.Status == ResultStatus.Success)
               {
                   packageOrder = ClientPackageManager.SelectPackageOrder(orderID, clientID).EntityList[0];
                   packageOrder.OrderStatus = "Verified-Refunded";
                   packageOrder.Comments = refundedPackageOrder.OrderID;
                   packageOrder.ExpiryDate = packageStartDate;
                   ClientPackageManager.UpdatePackageOrder(packageOrder);
                   lblMessage.Text = "The Order has been refunded successfully.";
                   lblMessage.CssClass = "successMsg";
                   //SEND EMAIL TO THE CLIENT
                   string durationType = " Month(s)";
                   if (refundedPackageOrder.PackageName == "SMS")
                       durationType = " SMS Credit(s)";
                   //email sent to all superadmins
                   //GET ALL THE USERNAME WHOSE ROLE IS SUPERADMIN
                   string[] userNameList = Roles.GetUsersInRole("SuperAdmin");

                   List<UserInfoPartial> users = new List<UserInfoPartial>();
                   foreach (string userName in userNameList)
                   {
                       CustomUserProfile profile = CustomUserProfile.GetUserProfile(userName);
                       users.Add(new UserInfoPartial() { FirstName = profile.FirstName, MiddleName = profile.MiddleName, LastName = profile.LastName, Email = Membership.GetUser(userName).Email });
                   }
                   string visibility = "hidden";
                   if (months != 0)
                       visibility = "visible";
                   string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
                   string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
                   //superadmin email
                   string superAdminSubject = "Refund given to " + WebContext.Parent.ClientName;
                   string appPath = Request.PhysicalApplicationPath;
                   StreamReader superAdminSR = new StreamReader(appPath + "EmailTemplates/RefundDetailToAdmin.txt");
                   string superAdminBody = superAdminSR.ReadToEnd();
                   superAdminSR.Close();
      
                   CustomUserProfile clientProfile = CustomUserProfile.GetUserProfile(Membership.GetUser(WebContext.Parent.ContactPerson).UserName);
                   superAdminBody = superAdminBody.Replace("<%Logo%>", logoUrl);
                   superAdminBody = superAdminBody.Replace("<%TopBackGround%>", topBackGroundUrl);
                   superAdminBody = superAdminBody.Replace("<%ClientName%>", WebContext.Parent.ClientName);
                   superAdminBody = superAdminBody.Replace("<%ContactPerson%>", clientProfile.FirstName + " " + clientProfile.MiddleName + " " + clientProfile.LastName);
                   superAdminBody = superAdminBody.Replace("<%TransactionID%>", refundedPackageOrder.TransactionID);
                   superAdminBody = superAdminBody.Replace("<%PackageName%>", refundedPackageOrder.PackageName);
                   superAdminBody = superAdminBody.Replace("<%PackageOption%>", refundedPackageOrder.Duration +" "+ durationType);
                   superAdminBody = superAdminBody.Replace("<%AmountPaid%>", string.Format("{0:0.00}", refundedPackageOrder.FinalPriceAfterDeduction));
                   superAdminBody = superAdminBody.Replace("<%RegistrationDate%>", string.Format("{0:d}", refundedPackageOrder.RegistrationDate));

                   string mailSender = "SleekSurf";

                   Helpers.SendEmail(users, mailSender, superAdminSubject, superAdminBody);
                   //email sent to the registered users below:
                   UserInfoPartial registeredUser = new UserInfoPartial() { FirstName = clientProfile.FirstName, MiddleName = clientProfile.MiddleName, LastName = clientProfile.LastName, Email = Membership.GetUser(WebContext.Parent.ContactPerson).Email };
                   StreamReader userSR = new StreamReader(appPath + "EmailTemplates/RefundDetailToClient.txt");
                   string userBody = userSR.ReadToEnd();
                   userSR.Close();
                   string userSubject = "Package Refund Receipt";
                   userBody = userBody.Replace("<%Logo%>", logoUrl);
                   userBody = userBody.Replace("<%TopBackGround%>", topBackGroundUrl);
                   userBody = userBody.Replace("<%ClientName%>", registeredUser.FirstName + " " + registeredUser.MiddleName + " " + registeredUser.LastName);
                   userBody = userBody.Replace("<%TransactionID%>", refundedPackageOrder.TransactionID);
                   userBody = userBody.Replace("<%PackageName%>", refundedPackageOrder.PackageName);
                   userBody = userBody.Replace("<%PackageOption%>", refundedPackageOrder.Duration + " " + durationType);
                   userBody = userBody.Replace("<%ExpiryDate%>", string.Format("{0:d}", refundedPackageOrder.ExpiryDate));
                   userBody = userBody.Replace("<%YouPaid%>", string.Format("{0:0.00}", packageOrder.FinalPriceAfterDeduction));
                   userBody = userBody.Replace("<%RefundDuration%>", months.ToString()+" "+durationType);
                   userBody = userBody.Replace("<%visible%>", visibility);
                   userBody = userBody.Replace("<%CancellationCharge%>", string.Format("{0:0.00}", refundedPackageOrder.AmountDeducted));
                   userBody = userBody.Replace("<%AmountPaid%>", string.Format("{0:0.00}", refundedPackageOrder.FinalPriceAfterDeduction));
                   userBody = userBody.Replace("<%RegistrationDate%>", string.Format("{0:d}", refundedPackageOrder.RegistrationDate));
                   userBody = userBody.Replace("<%PaymentStatus%>", refundedPackageOrder.OrderStatus);
                   userBody = userBody.Replace("<%PaymentMethod%>", refundedPackageOrder.PaymentOption);

                   string fullName = "";
                   CustomUserProfile currentUserprofile;
                   if (string.Compare(WebContext.CurrentUser.Identity.Name, "superadmin", true) == 0)
                   {
                       currentUserprofile = CustomUserProfile.GetUserProfile(WebContext.CurrentUser.Identity.Name);
                       fullName = currentUserprofile.FirstName + " " + currentUserprofile.MiddleName + " " + currentUserprofile.LastName;
                   }
                   userBody = userBody.Replace("<%SenderName%>", fullName);
                   userBody = userBody.Replace("<%SenderCompany%>", "SleekSurf Team");
                   Helpers.SendEmail(registeredUser, mailSender, userSubject, userBody);
                   Redirector.GoToRequestedPage("~/Admin/Client/AccountManagement.aspx?ID=" + refundedPackageOrder.OrderID);
               }
               else
               {
                   lblMessage.Text = result.Message;
                   lblMessage.CssClass = "errorMsg";
               }
            }
        }

        private void BindPaymentOption()
        {
            ddlPaymentOption.DataSource = Enum.GetValues(typeof(PaymentOptionStatus));
            ddlPaymentOption.DataBind();
            ddlPaymentOption.Items.Insert(0, "Select Below");
        }

        private void bindOrderDetail()
        {
            ucViewPackageOrder.SetClientPanelVisibility(false);

            packageOrder = ClientPackageManager.SelectPackageOrder(orderID, clientID).EntityList[0];
            if (packageOrder.PackageName != "SMS"&& (WebContext.CurrentUser.IsInRole("SuperAdmin") || WebContext.CurrentUser.IsInRole("SuperAdminUser")))
            {
                Result<PackageOrderDetails> result = ClientPackageManager.SelectRecentPackageOrder(clientID, packageOrder.PackageCode);
                if (result.Status == ResultStatus.Success && result.EntityList.Count > 0)
                {
                    if (packageOrder.OrderID == result.EntityList[0].OrderID && packageOrder.ExpiryDate.Subtract(DateTime.Now).Days > 30)
                    {
                        divRefund.Visible = true;
                        btnRefund.Attributes.Add("onclick", "return confirm_delete();");
                    }
                    else
                        divRefund.Visible = false;
                }
            }
            ucViewPackageOrder.PackageName = packageOrder.PackageName;
            ucViewPackageOrder.StandardPrice = packageOrder.StandardPrice;
            ucViewPackageOrder.ExpiryDate = packageOrder.ExpiryDate;

            if (!string.IsNullOrEmpty(packageOrder.PromoCode))
            {
                ucViewPackageOrder.PromoCode = packageOrder.PromoCode;
                ucViewPackageOrder.PromoCodeStartDate = packageOrder.PromoCodeStartDate;
                ucViewPackageOrder.PromoCodeEndDate = packageOrder.PromoCodeEndDate;
                ucViewPackageOrder.DiscountPercentage = ClientPackageManager.SelectPackageOption(packageOrder.PromoCode).EntityList[0].DiscountPercentage;
                ucViewPackageOrder.PromoDiscountedAmount = (packageOrder.StandardPrice * ((decimal)ucViewPackageOrder.DiscountPercentage / 100));
            }

            ucViewPackageOrder.FinalPrice = packageOrder.FinalPrice;
            ucViewPackageOrder.InvoiceID = packageOrder.OrderID;
            ucViewPackageOrder.TransactionID = packageOrder.TransactionID;
            ucViewPackageOrder.PurchasedMethod = packageOrder.PaymentOption;
            ucViewPackageOrder.AmountDeducted = packageOrder.AmountDeducted;
            ucViewPackageOrder.PurchasedStatus = packageOrder.OrderStatus;
            ucViewPackageOrder.AmountPaid = packageOrder.FinalPriceAfterDeduction;
            ucViewPackageOrder.PurchasedType = packageOrder.Comments;
            if (packageOrder.Comments.StartsWith("PO"))
            {
                if (packageOrder.OrderStatus == StatusOrder.Refunded.ToString())
                    ucViewPackageOrder.PurchasedType = "<a href='" + BasePage.FullBaseUrl + "Admin/Client/AccountTransactionDetails.aspx?OrderID=" + packageOrder.Comments + "&ClientID=" + clientID + "'>View Original Transaction</a>";
                else
                    ucViewPackageOrder.PurchasedType = "<a href='" + BasePage.FullBaseUrl + "Admin/Client/AccountTransactionDetails.aspx?OrderID=" + packageOrder.Comments + "&ClientID=" + clientID + "'>View Refunded Transaction</a>";
            }
            ucViewPackageOrder.PurchasedBy = packageOrder.CreatedBy;
            ucViewPackageOrder.PurchasedDate = packageOrder.RegistrationDate;
            ucViewPackageOrder.PackageDuration = packageOrder.Duration;
            if (packageOrder.OrderStatus == StatusOrder.Refunded.ToString())
            {
                ucViewPackageOrder.DisplayRefundedPanel(true);
                ucViewPackageOrder.AmountPaidByClient = packageOrder.FinalPriceAfterDeduction + packageOrder.AmountDeducted;
            }
        }
    }
}