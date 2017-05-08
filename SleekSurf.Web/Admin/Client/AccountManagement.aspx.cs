using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using System.Transactions;
using SleekSurf.Manager;
using System.IO;


namespace SleekSurf.Web.Admin.Client
{
    public partial class AccountManagement : System.Web.UI.Page
    {
        private static PackageOrderDetails orderedPackage = null;

        private static ClientFeatureDetails clientFeature = null;

        protected void Page_Load(object sender, EventArgs e)
        {
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

            if (!IsPostBack)
            {
                if (WebContext.GetQueryStringValue("ID") != null)//IF THE ORDERID IS SENT BACK FROM PAYPAL OR BY OTHER MEANS
                    BindOrderDetails();

                BindPaymentOption();//PAYMENT OPRIONS ARE BOUND IN THE DROPDOWN BASED ON ENUM
                BindOrderStatus();//ORDER STATUS ARE BOUND IN THE  DROPDOWN BASED ON ENUM
            }

            if (!IsPostBack)
            {
                BindPackages();//PACKAGES ARE BOUND IN THE DROPDOWN
                clientFeature = ClientManager.SelectClientFeatureDetails(WebContext.Parent.ClientID).EntityList[0];

                if (WebContext.Sibling != null && WebContext.Parent != null)//IF THE SUPERADMIN IS LOGGED IN AND ACCESSING THE CLIENTS INFORMATION
                {
                    BindMostRecentIncompleteOrderDetailsOfClient(WebContext.Parent.ClientID); //BINDS IF THERE ARE ANY INCOMPLETE TRANSACTION FOR THE CLIENT.
                    txtPromoCode.ReadOnly = IsPromoCodeReadOnly();
                    pnlExpressPurchase.Visible = true;

                    if (WebContext.GetQueryStringValue("ID") == null)
                        panelAccountForSuperAdminView.Visible = true;

                    if (WebContext.CurrentUser.IsInRole("MarketingOfficer"))
                    {
                        dvRightLinks.Visible = false;
                        pnlExpressPurchase.Visible = false; 
                    }
                }
                else if (WebContext.Parent != null && WebContext.GetQueryStringValue("ID") == null)
                {
                    panelAccountForClientView.Visible = true;
                    dvRightLinks.Visible = false;
                }
            }

            if (clientFeature != null)
            {
                hlMatchProfile.Visible = !clientFeature.ClientProfile;
                pnlMatchProfile.Visible = !clientFeature.ClientProfile;
                hlMatchDomain.Visible = !clientFeature.ClientDomain;
                pnlMatchDomain.Visible = !clientFeature.ClientDomain;
            }
        }
        #region EVENT LIST

        protected void ddlPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearFields();
            ddlPackageOption.Items.Clear();
            if (ddlPackage.SelectedIndex > 0)
                BindPackageOption(ddlPackage.SelectedValue);

            txtPromoCode.ReadOnly = IsPromoCodeReadOnly();
        }

        protected void ddlPackageOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearFields();
            if (ddlPackageOption.SelectedIndex > 0)
            {
                if (ddlPackage.SelectedValue != "SMS")
                {
                    PackageOptionDetails options = ClientPackageManager.SelectPackageOption(int.Parse(ddlPackageOption.SelectedValue)).EntityList[0];
                    txtPrice.Text = string.Format("{0:0.00}", options.StandardPrice);
                    txtFinalPrice.Text = string.Format("{0:0.00}", options.StandardPrice);
                }
                else
                {
                    txtPrice.Text = string.Format("{0:0.00}", Convert.ToDecimal(ddlPackageOption.SelectedValue));
                    txtFinalPrice.Text = string.Format("{0:0.00}", Convert.ToDecimal(ddlPackageOption.SelectedValue));
                }
            }

            txtPromoCode.ReadOnly = IsPromoCodeReadOnly();

        }

        protected bool IsPromoCodeReadOnly()
        {
            if (ddlPackageOption != null && ddlPackageOption.SelectedIndex > 0)
                return false;
            else
                return true;
        }

        protected void txtPromoCode_TextChanged(object sender, EventArgs e)
        {
            decimal finalPrice = 0.00M;
            if (ddlPackageOption != null && ddlPackageOption.SelectedIndex > 0)
            {
                PackageOptionDetails option = ClientPackageManager.SelectPackageOption(int.Parse(ddlPackageOption.SelectedValue)).EntityList[0];

                if (txtPromoCode.Text.Length > 0)
                {
                    if (option.PromoCode == txtPromoCode.Text)
                    {
                        if (DateTime.Now >= option.PromoCodeStartDate && DateTime.Now <= option.PromoCodeEndDate)
                        {
                            if (orderedPackage != null)
                            {
                                orderedPackage.PromoCode = option.PromoCode;
                                orderedPackage.PromoCodeStartDate = option.PromoCodeStartDate;
                                orderedPackage.PromoCodeEndDate = option.PromoCodeEndDate;
                                orderedPackage.DiscountPercentage = option.DiscountPercentage;
                            }
                            decimal promoCodeDiscount = (option.StandardPrice * ((decimal)option.DiscountPercentage / 100));
                            finalPrice = option.StandardPrice - promoCodeDiscount;
                            txtFinalPrice.Text = string.Format("{0:0.00}", finalPrice);
                            txtDeductedAmount.Text = string.Format("{0:0.00}", promoCodeDiscount);
                        }
                        else
                        {
                            txtFinalPrice.Text = string.Format("{0:0.00}", option.StandardPrice);
                            txtDeductedAmount.Text = string.Empty;
                            lblMessage.Text = "Promocode is expired!.";
                            lblMessage.CssClass = "errorMsg";
                        }

                    }
                    else
                    {
                        txtFinalPrice.Text = string.Format("{0:0.00}", option.StandardPrice);
                        txtDeductedAmount.Text = string.Empty;
                        lblMessage.Text = "Promocode mismatched.";
                        lblMessage.CssClass = "errorMsg";
                    }
                }
                else
                {
                    txtFinalPrice.Text = string.Format("{0:0.00}", option.StandardPrice);
                    txtDeductedAmount.Text = string.Empty;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SavePackageOrder();
        }

        protected void btnTopUp_Click(object sender, EventArgs e)
        {
            if (WebContext.Sibling != null && WebContext.Parent != null)
            {
                Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
                int divident = 1;
                PackageOptionDetails option = new PackageOptionDetails();
                if (ddlExpressPackage.SelectedValue != "SMS")
                {
                    option = ClientPackageManager.SelectPackageOptionsByPackage(ddlExpressPackage.SelectedValue).EntityList.Where(c => c.Duration == "1").FirstOrDefault();
                    divident = 30;
                }
                else
                {
                    option = new PackageOptionDetails() { StandardPrice = Convert.ToDecimal((int)SMSOption.C100)};
                    divident = 100;
                }
                PackageOrderDetails newOrderDetails = new PackageOrderDetails();
                newOrderDetails.OrderID = System.DateTime.Now.ToString("PO-ddMMyyyy-HHmmssfff");
                newOrderDetails.TransactionID = "TXN-" + newOrderDetails.OrderID;
                newOrderDetails.CreatedBy = HttpContext.Current.User.Identity.Name;
                newOrderDetails.OrderStatus = StatusOrder.Verified.ToString();
                newOrderDetails.Client = WebContext.Parent;
                newOrderDetails.PaymentOption = PaymentOptionStatus.Free.ToString();
                newOrderDetails.FinalPrice = (option.StandardPrice / divident) * Convert.ToInt32(txtNoOfDays.Text.Trim());
                newOrderDetails.StandardPrice = (option.StandardPrice / divident) * Convert.ToInt32(txtNoOfDays.Text.Trim());
                newOrderDetails.AmountDeducted = (option.StandardPrice / divident) * Convert.ToInt32(txtNoOfDays.Text.Trim());
                newOrderDetails.FinalPriceAfterDeduction = 0.00M;
                newOrderDetails.RegistrationDate = System.DateTime.Now;
                //RETRIEVE THE PACKAGE DETAILS
                newOrderDetails.PackageCode = ddlExpressPackage.SelectedValue;
                newOrderDetails.PackageName = ddlExpressPackage.SelectedItem.Text + " BySleekSurf";
                newOrderDetails.Duration = txtNoOfDays.Text.Trim();

                //RETRIEVE IF THE CLIENT HAS ANY EXISTING PACKAGE.
                Result<PackageOrderDetails> tempPackageOrderDetails = ClientPackageManager.SelectRecentPackageOrder(WebContext.Parent.ClientID, ddlExpressPackage.SelectedValue);
                PackageOrderDetails recentOrder = null;

                if (tempPackageOrderDetails.EntityList.Count > 0)
                    recentOrder = tempPackageOrderDetails.EntityList[0];
                if (ddlExpressPackage.SelectedValue != "SMS")
                {
                    if (recentOrder == null || recentOrder.ExpiryDate < System.DateTime.Now)
                    {
                        newOrderDetails.ExpiryDate = System.DateTime.Now.AddDays(int.Parse(newOrderDetails.Duration));
                        if (recentOrder == null)
                            newOrderDetails.Comments = "New";
                    }
                    else
                    {
                        newOrderDetails.ExpiryDate = recentOrder.ExpiryDate.AddDays(int.Parse(newOrderDetails.Duration));
                        newOrderDetails.Comments = "Renew";
                    }
                }
                else
                {
                    newOrderDetails.Comments = "Recharge";
                    Result<PackageOrderDetails> orderdetailsWithHighestExpiryDate = ClientPackageManager.SelectPackageorderWithHighestExpiryDate(WebContext.Parent.ClientID);
                    if (orderdetailsWithHighestExpiryDate.Status == ResultStatus.Success)
                        newOrderDetails.ExpiryDate = orderdetailsWithHighestExpiryDate.EntityList[0].ExpiryDate;
                    else
                        newOrderDetails.ExpiryDate = System.DateTime.Now.AddYears(1);
                }

                result = ClientPackageManager.InsertPackageOrder(newOrderDetails);
                if (result.Status == ResultStatus.Success)
                    orderedPackage = newOrderDetails;


                if (result.Status == ResultStatus.Success)
                {
                    lblMessage.CssClass = "successMsg";


                    using (TransactionScope emailScope = new TransactionScope())
                    {
                        string durationType = " Day(s)";
                        if (ddlExpressPackage.SelectedValue == "SMS")
                            durationType = " SMS Credit(s)";
                        //email sent to all superadmins
                        string[] userNameList = Roles.GetUsersInRole("SuperAdmin");
                        List<UserInfoPartial> users = new List<UserInfoPartial>();
                        foreach(string userName in userNameList)
                        {
                            CustomUserProfile profile = CustomUserProfile.GetUserProfile(userName);
                            users.Add(new UserInfoPartial() { FirstName = profile.FirstName, MiddleName = profile.MiddleName, LastName = profile.LastName, Email = Membership.GetUser(userName).Email });
                        }
                       
                        //embed image in the emails
                        string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
                        string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
                        //superadmin email
                        string superAdminSubject = "Payment received from " + WebContext.Parent.ClientName;
                        string appPath = Request.PhysicalApplicationPath;
                        StreamReader superAdminSR = new StreamReader(appPath + "EmailTemplates/PaymentDetailsForAdmin.txt");
                        string superAdminBody = superAdminSR.ReadToEnd();
                        superAdminSR.Close();
                        // superAdminBody = string.Format(superAdminBody, NewUser.UserName, UserProfile1.FirstName, UserProfile1.MiddleName, UserProfile1.LastName, UserProfile1.State, UserProfile1.Country);
                        CustomUserProfile clientProfile = CustomUserProfile.GetUserProfile(Membership.GetUser(WebContext.Parent.ContactPerson).UserName);
                        superAdminBody = superAdminBody.Replace("<%Logo%>", logoUrl);
                        superAdminBody = superAdminBody.Replace("<%TopBackGround%>", topBackGroundUrl);
                        superAdminBody = superAdminBody.Replace("<%ClientName%>", WebContext.Parent.ClientName);
                        superAdminBody = superAdminBody.Replace("<%ContactPerson%>", clientProfile.FirstName + " " + clientProfile.MiddleName + " " + clientProfile.LastName);
                        superAdminBody = superAdminBody.Replace("<%TransactionID%>", orderedPackage.TransactionID);
                        string packageName = orderedPackage.PackageName.Replace(" BySleekSurf", "");
                        superAdminBody = superAdminBody.Replace("<%PackageName%>", packageName);
                        superAdminBody = superAdminBody.Replace("<%PackageOption%>", orderedPackage.Duration + durationType);
                        superAdminBody = superAdminBody.Replace("<%AmountPaid%>", string.Format("{0:0.00}", orderedPackage.FinalPriceAfterDeduction));
                        superAdminBody = superAdminBody.Replace("<%RegistrationDate%>", string.Format("{0:d}", orderedPackage.RegistrationDate));

                        string mailSender = "SleekSurf";

                        Helpers.SendEmail(users, mailSender, superAdminSubject, superAdminBody);
                        //email sent to the registered users below:
                        UserInfoPartial registeredUser = new UserInfoPartial() { FirstName = clientProfile.FirstName, MiddleName = clientProfile.MiddleName, LastName = clientProfile.LastName, Email = Membership.GetUser(WebContext.Parent.ContactPerson).Email };
                        StreamReader userSR = new StreamReader(appPath + "EmailTemplates/PaymentDetailsForClient.txt");
                        string userBody = userSR.ReadToEnd();
                        userSR.Close();
                        string userSubject = "Package Payment Receipt";
                        userBody = userBody.Replace("<%Logo%>", logoUrl);
                        userBody = userBody.Replace("<%TopBackGround%>", topBackGroundUrl);
                        userBody = userBody.Replace("<%ClientName%>", registeredUser.FirstName + " " + registeredUser.MiddleName + " " + registeredUser.LastName);
                        userBody = userBody.Replace("<%TransactionID%>", orderedPackage.TransactionID);
                        if (packageName.Contains("Domain"))
                            userBody = userBody.Replace("<%PackageName%>", packageName + "<span style='font-size:8px;'> points to IP Address: 50.61.232.230</span>");
                        else
                            userBody = userBody.Replace("<%PackageName%>", packageName);
                        userBody = userBody.Replace("<%PackageOption%>", orderedPackage.Duration + durationType);
                        userBody = userBody.Replace("<%AmountPaid%>", string.Format("{0:0.00}", orderedPackage.FinalPriceAfterDeduction));
                        userBody = userBody.Replace("<%RegistrationDate%>", string.Format("{0:d}", orderedPackage.RegistrationDate));
                        userBody = userBody.Replace("<%ExpiryDate%>", string.Format("{0:d}", orderedPackage.ExpiryDate));
                        userBody = userBody.Replace("<%PaymentStatus%>", orderedPackage.OrderStatus);
                        userBody = userBody.Replace("<%PaymentMethod%>", orderedPackage.PaymentOption);

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
                        emailScope.Complete();
                    }

                    Redirector.GoToRequestedPage("~/Admin/Client/AccountManagement.aspx?ID=" + orderedPackage.OrderID);

                    ClearOrderForm();
                }
                else
                    lblMessage.CssClass = "errorMsg";

                lblMessage.Text = result.Message;
            }
        }

        #endregion

        #region FUNCTION LIST

        private void BindOrderDetails()//BINDS ORDER DETAILS BASED ON QUERYSTRING.
        {
            string orderID = WebContext.GetQueryStringValue("ID");
            string clientID = WebContext.Parent.ClientID;
            PackageOrderDetails orderDetail = ClientPackageManager.SelectPackageOrder(orderID).EntityList[0];
            if (orderDetail.PackageCode != "SMS" && orderDetail.OrderStatus != StatusOrder.Refunded.ToString())
            {
                PackageDetails package = ClientPackageManager.SelectPackage(orderDetail.PackageCode).EntityList[0];
                ClientManager.ClientFeatureSetStatus(clientID, package.FeatureType, true);
            }

            //PUBLISH ALL USERS FOR THE CLIENTS.
            if (WebContext.Parent != null && !WebContext.Parent.Published)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        WebContext.Parent.Comment = Status.Activated.ToString();
                        ClientManager.PublishClient(clientID, WebContext.Parent.Comment);
                      
                        List<Guid> userList = ClientManager.SelectUsers(WebContext.Parent.ClientID);
                        foreach (Guid id in userList)
                        {
                            MembershipUser userTemp = Membership.GetUser(id);
                            if (userTemp.Comment == Status.InActiveByAccountExpiration.ToString())
                            {
                                userTemp.IsApproved = true;
                                userTemp.Comment = Status.Activated.ToString();
                            }
                            Membership.UpdateUser(userTemp);

                        }
                        scope.Complete();
                    }//end of try
                    catch (Exception ex)
                    {
                        Helpers.LogError(ex);
                        throw;
                    }

                }//end of transaction scope
            }

            lblMessage.Text = "The Order details have been updated successfully.";
            lblMessage.CssClass = "successMsg";

            if (orderDetail.PackageName.Trim() == "SMS" || orderDetail.PackageName.Trim() == "SMS BySleekSurf")
            {
                orderDetail.PackageName = orderDetail.PackageName.Replace(" BySleekSurf", "");
                lblPackageOption.Text = orderDetail.Duration + " SMS Credits";
                if(orderDetail.OrderStatus == StatusOrder.Refunded.ToString())
                ClientManager.UpdateSMSCredit(WebContext.Parent.ClientID, -Convert.ToInt32(orderDetail.Duration));
                else
                    ClientManager.UpdateSMSCredit(WebContext.Parent.ClientID, Convert.ToInt32(orderDetail.Duration));
            }
            else if (orderDetail.PackageName.Trim().EndsWith(" BySleekSurf"))
            {
                orderDetail.PackageName = orderDetail.PackageName.Replace(" BySleekSurf", "");
                lblPackageOption.Text = orderDetail.Duration + " Day(s)";
            }
            else
            {
                if(orderDetail.Duration == "0")
                    lblPackageOption.Text = "Matched";
                else
                    lblPackageOption.Text = orderDetail.Duration + " Month(s)";
            }
                
            lblPackageName.Text = orderDetail.PackageName;
            lblRegistrationDate.Text = string.Format("{0:d}", orderDetail.RegistrationDate);
            lblExpiryDate.Text = string.Format("{0:d}", orderDetail.ExpiryDate);
            lblInvoiceID.Text = orderDetail.OrderID;
            lblTransactionID.Text = orderDetail.TransactionID;
            if (orderDetail.PaymentOption == PaymentOptionStatus.PayPal.ToString())
                lblAmountPaid.Text = string.Format("{0:0.00}", orderDetail.FinalPrice);
            else
                lblAmountPaid.Text = string.Format("{0:0.00}", orderDetail.FinalPriceAfterDeduction);
            lblPaymentMethod.Text = orderDetail.PaymentOption;
            lblPaymentStatus.Text = orderDetail.OrderStatus;

            panelTransactionReceipt.Visible = true;
        }

        private void BindMostRecentIncompleteOrderDetailsOfClient(string clientID)
        {
            Result<PackageOrderDetails> resultPackageOrderDetails = ClientPackageManager.SelectMostRecentIncompletePackageOrder(clientID);

            if (resultPackageOrderDetails.EntityList.Count > 0)
                orderedPackage = resultPackageOrderDetails.EntityList[0];

            if (orderedPackage != null && orderedPackage.OrderStatus != StatusOrder.Verified.ToString())
            {
                ddlPackage.Items[ddlPackage.Items.IndexOf(ddlPackage.Items.FindByText(orderedPackage.PackageName))].Selected = true;
                BindPackageOption(ddlPackage.SelectedValue);
                //PACKAGE OPTION
                ddlPackageOption.Items[ddlPackageOption.Items.IndexOf(ddlPackageOption.Items.FindByText(orderedPackage.Duration))].Selected = true;
                txtPromoCode.Text = orderedPackage.PromoCode;

                ddlOrderStatus.Items[ddlOrderStatus.Items.IndexOf(ddlOrderStatus.Items.FindByText(orderedPackage.OrderStatus))].Selected = true;
                txtPrice.Text = string.Format("{0:0.00}", orderedPackage.StandardPrice);
                if (!string.IsNullOrEmpty(orderedPackage.PromoCode))
                {
                    txtPromoCode.Text = orderedPackage.PromoCode;
                    decimal deductedAmount = orderedPackage.StandardPrice - orderedPackage.FinalPrice;
                    txtDeductedAmount.Text = string.Format("{0:0.00}", deductedAmount);
                }
                ddlPaymentOption.Items[ddlPaymentOption.Items.IndexOf(ddlPaymentOption.Items.FindByText(orderedPackage.PaymentOption))].Selected = true;
                txtFinalPrice.Text = string.Format("{0:0.00}", orderedPackage.FinalPrice);
                txtAmountPaid.Text = string.Format("{0:0.00}", orderedPackage.FinalPriceAfterDeduction);

                //MAKE PANEL VISIBLE
                panelAccountForSuperAdminView.Visible = true;
            }
            else
                orderedPackage = null;
        }//BINDS LATEST INCOMPLETE TRANSACTION

        private void BindPackages()
        {
            ddlPackage.DataSource = ClientPackageManager.SelectAllPackageWithoutPaging().EntityList;
            ddlPackage.DataValueField = "PackageCode";
            ddlPackage.DataTextField = "PackageName";
            ddlPackage.DataBind();
            ddlPackage.Items.Insert(0, "Select Below");
            if (!WebContext.CurrentUser.IsInRole("MarketingOfficer"))
            {
                ddlPackage.Items.Add(new ListItem("SMS", "SMS"));

                ddlExpressPackage.DataSource = ClientPackageManager.SelectAllPackageWithoutPaging().EntityList;
                ddlExpressPackage.DataValueField = "PackageCode";
                ddlExpressPackage.DataTextField = "PackageName";
                ddlExpressPackage.DataBind();
                ddlExpressPackage.Items.Add(new ListItem("SMS", "SMS"));
            }

        }

        private void BindPackageOption(string packageCode)
        {
            if (packageCode != "SMS")
            {
                ddlPackageOption.DataSource = ClientPackageManager.SelectPackageOptionsByPackage(packageCode).EntityList;
                ddlPackageOption.DataValueField = "PackageOptionID";
                ddlPackageOption.DataTextField = "Duration";
            }
            else
            {
                foreach (SMSOption option in Enum.GetValues(typeof(SMSOption)))
                {
                    string tempOption = Enum.GetName(typeof(SMSOption), option).Replace('C', ' ').Trim();
                    ddlPackageOption.Items.Add(new ListItem(tempOption, ((int)option).ToString()));
                }
            }
            ddlPackageOption.DataBind();
            Result<PackageDetails> resultPackage = ClientPackageManager.SelectPackage(packageCode);
            if (resultPackage.Status == ResultStatus.Success)
            {
                string featureType = ClientPackageManager.SelectPackage(packageCode).EntityList[0].FeatureType;
                ClientFeatureDetails thisClientFeature = ClientManager.SelectClientFeatureDetails(WebContext.Parent.ClientID).EntityList[0];
                switch (featureType)
                {
                    case "ClientProfile":
                        if (thisClientFeature.ClientProfile)
                            ddlPackageOption.Items.Remove(ddlPackageOption.Items.FindByText("0"));
                        break;
                    case "ClientDomain":
                        if (thisClientFeature.ClientDomain)
                            ddlPackageOption.Items.Remove(ddlPackageOption.Items.FindByText("0"));
                        break;
                }
            }
           
            ddlPackageOption.Items.Insert(0, "Select Below");
        }

        private void BindPaymentOption()
        {
            ddlPaymentOption.DataSource = Enum.GetValues(typeof(PaymentOptionStatus));
            ddlPaymentOption.DataBind();
            ddlPaymentOption.Items.Insert(0, "Select Below");
        }

        private void BindOrderStatus()
        {
            ddlOrderStatus.DataSource = Enum.GetValues(typeof(StatusOrder));
            ddlOrderStatus.DataBind();
            ddlOrderStatus.Items.Insert(0, "Select Below");
            if (WebContext.CurrentUser.IsInRole("MarketingOfficer"))
                ddlOrderStatus.Items.Remove(ddlOrderStatus.Items.FindByText(StatusOrder.Refunded.ToString()));
        }

        private void SavePackageOrder()
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            if (orderedPackage != null)
            {
                if (txtPromoCode.Text.Length > 0)
                {
                    orderedPackage.PromoCode = txtPromoCode.Text;
                }
                orderedPackage.PackageCode = ddlPackage.SelectedValue;
                orderedPackage.PackageName = ddlPackage.SelectedItem.Text;
                orderedPackage.Duration = ddlPackageOption.SelectedItem.Text;
                orderedPackage.TransactionID = "TXN-" + orderedPackage.OrderID;
                orderedPackage.StandardPrice = Convert.ToDecimal(txtPrice.Text);
                orderedPackage.FinalPrice = Convert.ToDecimal(txtFinalPrice.Text);
                orderedPackage.FinalPriceAfterDeduction = Convert.ToDecimal(txtAmountPaid.Text);
                orderedPackage.AmountDeducted = orderedPackage.FinalPrice - Convert.ToDecimal(txtAmountPaid.Text);

                orderedPackage.OrderStatus = ddlOrderStatus.SelectedValue;
                orderedPackage.PaymentOption = ddlPaymentOption.SelectedValue;

                orderedPackage.RegistrationDate = DateTime.Now;

                //RETRIEVE IF THE CLIENT HAS ANY EXISTING PACKAGE.
                PackageOrderDetails recentOrder = null;
                Result<PackageOrderDetails> resultRecentOrder = ClientPackageManager.SelectRecentPackageOrder(WebContext.Parent.ClientID, ddlPackage.SelectedValue);
                if (resultRecentOrder.Status == ResultStatus.Success && resultRecentOrder.EntityList.Count > 0)
                    recentOrder = resultRecentOrder.EntityList[0];

                if (ddlPackage.SelectedValue != "SMS")//IF THE ORDER IS NOT FOR SMS
                {
                    PackageOptionDetails options = ClientPackageManager.SelectPackageOption(int.Parse(ddlPackageOption.SelectedValue)).EntityList[0];
                    if (ddlPackageOption.SelectedItem.Text.Trim() != "0")//IF THE SELECTED OPTION IS NOT PROFILE-DOMAIN MATCHING
                    {
                        if (recentOrder == null || recentOrder.ExpiryDate < System.DateTime.Now)
                        {
                            orderedPackage.ExpiryDate = System.DateTime.Now.AddMonths(int.Parse(options.Duration));
                            if (recentOrder == null)
                                orderedPackage.Comments = "New";
                        }
                        else
                        {
                            orderedPackage.ExpiryDate = recentOrder.ExpiryDate.AddMonths(int.Parse(options.Duration));
                            orderedPackage.Comments = "Renew";
                        }
                    }
                    else
                    {
                        string tempFeatureType = ClientPackageManager.SelectPackage(ddlPackage.SelectedValue).EntityList[0].FeatureType;
                        PackageDetails tempPackage = null;
                        switch(tempFeatureType)
                        {
                            case "ClientProfile":
                                tempPackage = ClientPackageManager.SelectPackageByFeatureType(FeatureType.ClientDomain.ToString()).EntityList[0];
                                if (tempPackage != null)
                                {
                                    Result<PackageOrderDetails> tempPackageOrderResult = ClientPackageManager.SelectRecentPackageOrder(WebContext.Parent.ClientID, tempPackage.PackageCode);
                                    if (tempPackageOrderResult.Status == ResultStatus.Success)
                                    {
                                        orderedPackage.ExpiryDate = tempPackageOrderResult.EntityList[0].ExpiryDate;
                                    }
                                    else
                                        orderedPackage.ExpiryDate = System.DateTime.Now.AddMonths(1); 
                                }
                                else
                                    orderedPackage.ExpiryDate = System.DateTime.Now.AddMonths(1); 
                                break;
                            case "ClientDomain":
                                tempPackage = ClientPackageManager.SelectPackageByFeatureType(FeatureType.ClientProfile.ToString()).EntityList[0];
                                if (tempPackage != null)
                                {
                                    Result<PackageOrderDetails> tempPackageOrderResult = ClientPackageManager.SelectRecentPackageOrder(WebContext.Parent.ClientID, tempPackage.PackageCode);
                                    if (tempPackageOrderResult.Status == ResultStatus.Success)
                                    {
                                        orderedPackage.ExpiryDate = tempPackageOrderResult.EntityList[0].ExpiryDate;
                                    }
                                    else
                                        orderedPackage.ExpiryDate = System.DateTime.Now.AddMonths(1); 
                                }
                                else
                                    orderedPackage.ExpiryDate = System.DateTime.Now.AddMonths(1); 
                                break;
                        }
                        orderedPackage.Comments = "Matched";
                    }

                    result = ClientPackageManager.UpdatePackageOrder(orderedPackage);
                }
                else//IF THE ORDER IS FOR SMS
                {
                    Result<PackageOrderDetails> orderdetailsWithHighestExpiryDate = ClientPackageManager.SelectPackageorderWithHighestExpiryDate(WebContext.Parent.ClientID);
                    if (orderdetailsWithHighestExpiryDate.Status == ResultStatus.Success)
                        orderedPackage.ExpiryDate = orderdetailsWithHighestExpiryDate.EntityList[0].ExpiryDate;
                    else
                        orderedPackage.ExpiryDate = System.DateTime.Now.AddYears(1);
                    orderedPackage.Comments = "Recharge";
                    result = ClientPackageManager.UpdatePackageOrder(orderedPackage);
                }
            }
            else
            {
                PackageOrderDetails newOrderDetails = new PackageOrderDetails();
                newOrderDetails.OrderID = System.DateTime.Now.ToString("PO-ddMMyyyy-HHmmssfff");
                newOrderDetails.TransactionID = "TXN-" + newOrderDetails.OrderID;
                newOrderDetails.CreatedBy = HttpContext.Current.User.Identity.Name;
                newOrderDetails.OrderStatus = ddlOrderStatus.SelectedItem.Text;
                newOrderDetails.Client = WebContext.Parent;
                newOrderDetails.PaymentOption = ddlPaymentOption.SelectedItem.Text;
                newOrderDetails.FinalPrice = Convert.ToDecimal(txtFinalPrice.Text);
                newOrderDetails.StandardPrice = Convert.ToDecimal(txtPrice.Text);
                newOrderDetails.AmountDeducted = newOrderDetails.FinalPrice - Convert.ToDecimal(txtAmountPaid.Text);
                newOrderDetails.FinalPriceAfterDeduction = Convert.ToDecimal(txtAmountPaid.Text);
                newOrderDetails.RegistrationDate = System.DateTime.Now;
                //RETRIEVE IF THE CLIENT HAS ANY EXISTING PACKAGE.
                PackageOrderDetails recentOrder = null;
                Result<PackageOrderDetails> tempPackageOrderDetails = ClientPackageManager.SelectRecentPackageOrder(WebContext.Parent.ClientID, ddlPackage.SelectedValue);

                if (tempPackageOrderDetails.EntityList.Count > 0)
                    recentOrder = tempPackageOrderDetails.EntityList[0];

                //RETRIEVE THE PACKAGE DETAILS
                if (ddlPackage.SelectedValue != "SMS")// IF THE PACKAGE IS NOT SMS CREDIT
                {
                    PackageDetails package = ClientPackageManager.SelectPackage(ddlPackage.SelectedValue).EntityList[0];
                    if (package != null)
                    {
                        newOrderDetails.PackageCode = package.PackageCode;
                        newOrderDetails.PackageName = package.PackageName;
                    }

                    //RETIREVE THE PACKAGE OPTION DETAILS.
                    PackageOptionDetails packageOption = ClientPackageManager.SelectPackageOption(int.Parse(ddlPackageOption.SelectedValue)).EntityList[0];
                    newOrderDetails.Duration = packageOption.Duration;

                    if (txtPromoCode.Text.Length > 0)
                    {
                        newOrderDetails.PromoCode = packageOption.PromoCode;
                        newOrderDetails.PromoCodeStartDate = packageOption.PromoCodeStartDate;
                        newOrderDetails.PromoCodeEndDate = packageOption.PromoCodeEndDate;
                        newOrderDetails.DiscountPercentage = packageOption.DiscountPercentage;
                    }

                    if (ddlPackageOption.SelectedItem.Text.Trim() != "0")
                    {
                        if (recentOrder == null || recentOrder.ExpiryDate < System.DateTime.Now)
                        {
                            newOrderDetails.ExpiryDate = System.DateTime.Now.AddMonths(int.Parse(packageOption.Duration));
                            if (recentOrder == null)
                                newOrderDetails.Comments = "New";
                        }
                        else
                        {
                            newOrderDetails.ExpiryDate = recentOrder.ExpiryDate.AddMonths(int.Parse(packageOption.Duration));
                            newOrderDetails.Comments = "Renew";
                        }
                    }
                    else
                    {
                        string tempFeatureType = ClientPackageManager.SelectPackage(ddlPackage.SelectedValue).EntityList[0].FeatureType;
                        PackageDetails tempPackage = null;
                        switch (tempFeatureType)
                        {
                            case "ClientProfile":
                                tempPackage = ClientPackageManager.SelectPackageByFeatureType(FeatureType.ClientDomain.ToString()).EntityList[0];
                                if (tempPackage != null)
                                {
                                    Result<PackageOrderDetails> tempPackageOrderResult = ClientPackageManager.SelectRecentPackageOrder(WebContext.Parent.ClientID, tempPackage.PackageCode);
                                    if (tempPackageOrderResult.Status == ResultStatus.Success)
                                    {
                                        newOrderDetails.ExpiryDate = tempPackageOrderResult.EntityList[0].ExpiryDate;
                                    }
                                    else
                                        newOrderDetails.ExpiryDate = System.DateTime.Now.AddMonths(1);
                                }
                                else
                                    newOrderDetails.ExpiryDate = System.DateTime.Now.AddMonths(1);
                                break;
                            case "ClientDomain":
                                tempPackage = ClientPackageManager.SelectPackageByFeatureType(FeatureType.ClientProfile.ToString()).EntityList[0];
                                if (tempPackage != null)
                                {
                                    Result<PackageOrderDetails> tempPackageOrderResult = ClientPackageManager.SelectRecentPackageOrder(WebContext.Parent.ClientID, tempPackage.PackageCode);
                                    if (tempPackageOrderResult.Status == ResultStatus.Success)
                                    {
                                        newOrderDetails.ExpiryDate = tempPackageOrderResult.EntityList[0].ExpiryDate;
                                    }
                                    else
                                        newOrderDetails.ExpiryDate = System.DateTime.Now.AddMonths(1);
                                }
                                else
                                    newOrderDetails.ExpiryDate = System.DateTime.Now.AddMonths(1);
                                break;
                        }

                        newOrderDetails.Comments = "Matched";
                    }

                    result = ClientPackageManager.InsertPackageOrder(newOrderDetails);
                    if (result.Status == ResultStatus.Success)
                        orderedPackage = newOrderDetails;

                }
                else //IF THE PACKAGE IS SMS CREDIT
                {
                    newOrderDetails.PackageCode = ddlPackage.SelectedValue;
                    newOrderDetails.PackageName = ddlPackage.SelectedValue;
                    newOrderDetails.Duration = ddlPackageOption.SelectedItem.Text;
                   Result<PackageOrderDetails> orderdetailsWithHighestExpiryDate = ClientPackageManager.SelectPackageorderWithHighestExpiryDate(WebContext.Parent.ClientID);
                    if (orderdetailsWithHighestExpiryDate.Status == ResultStatus.Success)
                        newOrderDetails.ExpiryDate = orderdetailsWithHighestExpiryDate.EntityList[0].ExpiryDate;
                    else
                        newOrderDetails.ExpiryDate = System.DateTime.Now.AddYears(1);
                    newOrderDetails.Comments = "Recharge";

                    result = ClientPackageManager.InsertPackageOrder(newOrderDetails);
                    if (result.Status == ResultStatus.Success)                        
                        orderedPackage = newOrderDetails;
                }

            }

            if (result.Status == ResultStatus.Success)
            {
                lblMessage.CssClass = "successMsg";

                if (ddlOrderStatus.SelectedValue == StatusOrder.Verified.ToString())
                {
                    using (TransactionScope emailScope = new TransactionScope())
                    {
                        string durationType = " Month(s)";
                        if (ddlPackage.SelectedValue == "SMS")
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
                        //embed image in the emails
                        string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
                        string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
                        //superadmin email
                        string superAdminSubject = "Payment received from " + WebContext.Parent.ClientName;
                        string appPath = Request.PhysicalApplicationPath;
                        StreamReader superAdminSR = new StreamReader(appPath + "EmailTemplates/PaymentDetailsForAdmin.txt");
                        string superAdminBody = superAdminSR.ReadToEnd();
                        superAdminSR.Close();
                        // superAdminBody = string.Format(superAdminBody, NewUser.UserName, UserProfile1.FirstName, UserProfile1.MiddleName, UserProfile1.LastName, UserProfile1.State, UserProfile1.Country);
                        CustomUserProfile clientProfile = CustomUserProfile.GetUserProfile(Membership.GetUser(WebContext.Parent.ContactPerson).UserName);
                        superAdminBody = superAdminBody.Replace("<%Logo%>", logoUrl);
                        superAdminBody = superAdminBody.Replace("<%TopBackGround%>", topBackGroundUrl);
                        superAdminBody = superAdminBody.Replace("<%ClientName%>", WebContext.Parent.ClientName);
                        superAdminBody = superAdminBody.Replace("<%ContactPerson%>", clientProfile.FirstName + " " + clientProfile.MiddleName + " " + clientProfile.LastName);
                        superAdminBody = superAdminBody.Replace("<%TransactionID%>", orderedPackage.TransactionID);
                        superAdminBody = superAdminBody.Replace("<%PackageName%>", orderedPackage.PackageName);
                        superAdminBody = superAdminBody.Replace("<%PackageOption%>", orderedPackage.Duration + durationType);
                        superAdminBody = superAdminBody.Replace("<%AmountPaid%>", string.Format("{0:0.00}", orderedPackage.FinalPriceAfterDeduction));
                        superAdminBody = superAdminBody.Replace("<%RegistrationDate%>", string.Format("{0:d}", orderedPackage.RegistrationDate));

                        string mailSender = "SleekSurf";

                        Helpers.SendEmail(users, mailSender, superAdminSubject, superAdminBody);
                        //email sent to the registered users below:
                        UserInfoPartial registeredUser = new UserInfoPartial() { FirstName = clientProfile.FirstName, MiddleName = clientProfile.MiddleName, LastName = clientProfile.LastName, Email = Membership.GetUser(WebContext.Parent.ContactPerson).Email };
                        StreamReader userSR = new StreamReader(appPath + "EmailTemplates/PaymentDetailsForClient.txt");
                        string userBody = userSR.ReadToEnd();
                        userSR.Close();
                        string userSubject = "Package Payment Receipt";
                        userBody = userBody.Replace("<%Logo%>", logoUrl);
                        userBody = userBody.Replace("<%TopBackGround%>", topBackGroundUrl);
                        userBody = userBody.Replace("<%ClientName%>", registeredUser.FirstName + " " + registeredUser.MiddleName + " " + registeredUser.LastName);
                        userBody = userBody.Replace("<%TransactionID%>", orderedPackage.TransactionID);
                        if (orderedPackage.PackageName.Contains("Domain"))
                            userBody = userBody.Replace("<%PackageName%>", orderedPackage.PackageName+"<span style='font-size:8px;'> points to IP Address: 50.61.232.230 </span>");
                        else
                        userBody = userBody.Replace("<%PackageName%>", orderedPackage.PackageName);
                        userBody = userBody.Replace("<%PackageOption%>", orderedPackage.Duration + durationType);
                        userBody = userBody.Replace("<%AmountPaid%>", string.Format("{0:0.00}", orderedPackage.FinalPriceAfterDeduction));
                        userBody = userBody.Replace("<%RegistrationDate%>", string.Format("{0:d}", orderedPackage.RegistrationDate));
                        userBody = userBody.Replace("<%ExpiryDate%>", string.Format("{0:d}", orderedPackage.ExpiryDate));
                        userBody = userBody.Replace("<%PaymentStatus%>", orderedPackage.OrderStatus);
                        userBody = userBody.Replace("<%PaymentMethod%>", orderedPackage.PaymentOption);

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
                        emailScope.Complete();
                    }

                    Redirector.GoToRequestedPage("~/Admin/Client/AccountManagement.aspx?ID=" + orderedPackage.OrderID);
                }

                ClearOrderForm();
            }
            else
                lblMessage.CssClass = "errorMsg";

            lblMessage.Text = result.Message;
        }

        private void ClearOrderForm()
        {
            ddlPackage.SelectedIndex = 0;
            ddlPackageOption.Items.Clear();
            ddlPaymentOption.SelectedIndex = 0;
            ddlOrderStatus.SelectedIndex = 0;
            txtFinalPrice.Text = string.Empty;
            txtAmountPaid.Text = string.Empty;
            txtDeductedAmount.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtPromoCode.Text = string.Empty;
            orderedPackage = null;
            txtPromoCode.ReadOnly = false;
        }

        private void ClearFields()
        {
            ddlPaymentOption.SelectedIndex = 0;
            ddlOrderStatus.SelectedIndex = 0;
            txtFinalPrice.Text = string.Empty;
            txtAmountPaid.Text = string.Empty;
            txtDeductedAmount.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtPromoCode.Text = string.Empty;
            if (orderedPackage != null)
            {
                orderedPackage.PromoCode = string.Empty;
                orderedPackage.PromoCodeStartDate = null;
                orderedPackage.PromoCodeEndDate = null;
                orderedPackage.DiscountPercentage = 0.00;
                orderedPackage.StandardPrice = 0.00M;
                orderedPackage.FinalPrice = 0.00M;
                orderedPackage.AmountDeducted = 0.00M;
                orderedPackage.FinalPriceAfterDeduction = 0.00M;
            }
            orderedPackage = null;
        }
        #endregion
    }
}