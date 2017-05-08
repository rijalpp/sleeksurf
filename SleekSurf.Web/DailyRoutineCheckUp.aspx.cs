using System;
using System.Collections.Generic;
using System.Data;
using SleekSurf.Manager;
using SleekSurf.Entity;
using System.Web.Security;
using SleekSurf.FrameWork;
using System.IO;
using System.Transactions;
using System.Text;

namespace SleekSurf.Web
{
    public partial class DailyRoutineCheckUp : System.Web.UI.Page
    {
        SortedList<string, string> checkUp = new SortedList<string, string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            RemindForRenewalOfPackageOrder();
            UpdateClientsWithExpiredAccount();
            RemindForRenewalOfAdvertisement();
            SendRoutineEmailToWebMaster();
            //==============
            StringBuilder sb = new StringBuilder();
            string s = "Prem";
            foreach (char c in s)
            {
                sb.Append(c+" ");
            }
             sb.ToString();
        }

        private void RemindForRenewalOfPackageOrder()
        {
            int listingCount = 0;
            int clientProfileCount = 0;
            int domainCount = 0;
            string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
            string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
            string appPath = Request.PhysicalApplicationPath;

            StreamReader bodyBusinessReader = new StreamReader(appPath + "EmailTemplates/AccountExpiryNotification.txt");
            string bodyBusiness = bodyBusinessReader.ReadToEnd();
            bodyBusinessReader.Close();

            DataTable dt = ClientPackageManager.RemindForRenewalOfPackageOrder();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["PaymentOption"] != PaymentOptionStatus.SocialGrant.ToString())
                {
                    //GET CLIENT DETAILS
                    ClientDetails selectedClient = ClientManager.SelectClient(dr["ClientID"].ToString()).EntityList[0];
                    //GET CONTACT PERSON DETAILS
                    MembershipUser contactPerson = Membership.GetUser(selectedClient.ContactPerson);
                    CustomUserProfile profile = CustomUserProfile.GetUserProfile(contactPerson.UserName);
                    //UPDATE EXPIRYNOTICE
                    string daysDaysLeft = dr["DaysLeft"].ToString();
                    int currentDaysLeft = (int)dr["DaysLeft"];
                    int updatedExpiry = (currentDaysLeft / 11) * 10;

                    if (ClientPackageManager.UpdateAccountExpiryNotice(dr["OrderID"].ToString(), selectedClient.ClientID, updatedExpiry))
                    {
                        //SEND EMAIL TO BUSINESS EMAIL
                        string subject = "AccountExpiration: " + selectedClient.ClientName;
                        string toBusiness = selectedClient.BusinessEmail;

                        List<UserInfoPartial> userList = new List<UserInfoPartial>();
                        UserInfoPartial contactPersonDetails = new UserInfoPartial() { FirstName = profile.FirstName, MiddleName = profile.MiddleName, LastName = profile.LastName, Email = contactPerson.Email };
                        UserInfoPartial businessDetails = new UserInfoPartial() { FirstName = profile.FirstName, MiddleName = profile.MiddleName, LastName = profile.LastName, Email = selectedClient.BusinessEmail };
                        userList.Add(contactPersonDetails);
                        userList.Add(businessDetails);
                        string tempBodyBusiness = bodyBusiness;
                        tempBodyBusiness = tempBodyBusiness.Replace("<%Logo%>", logoUrl);
                        tempBodyBusiness = tempBodyBusiness.Replace("<%TopBackGround%>", topBackGroundUrl);
                        tempBodyBusiness = tempBodyBusiness.Replace("<%CustomerFullName%>", profile.FirstName + " " + profile.LastName);
                        tempBodyBusiness = tempBodyBusiness.Replace("<%DaysLeft%>", currentDaysLeft.ToString());
                        tempBodyBusiness = tempBodyBusiness.Replace("<%BusinessName%>", selectedClient.ClientName);
                        tempBodyBusiness = tempBodyBusiness.Replace("<%PackageType%>", dr["PackageName"].ToString() + " Duration: " + dr["Duration"].ToString() + " Months");
                        tempBodyBusiness = tempBodyBusiness.Replace("<%ExpiryDate%>", dr["ExpiryDate"].ToString());
                        tempBodyBusiness = tempBodyBusiness.Replace("<%WebSite%>", BasePage.FullBaseUrl);

                        Helpers.SendEmail(userList, "SleekSurf", subject, tempBodyBusiness);
                        switch (dr["FeatureType"].ToString())
                        {
                            case "Listing":
                                listingCount++;
                                break;
                            case "ClientProfile":
                                clientProfileCount++;
                                break;
                            case "ClientDomain":
                                domainCount++;
                                break;
                        }
                    }
                }

                checkUp.Add("<span style='font-weight:bold;'>Listing Business Expiration Notification</span>", "<span style='font-weight:bold;'>" + listingCount + "</span> Email(s) to Client(s) for ");
                checkUp.Add("<span style='font-weight:bold;'>Client Profile Expiration Notification</span>", "<span style='font-weight:bold;'>" + clientProfileCount + "</span> Email(s) to Client(s) for ");
                checkUp.Add("<span style='font-weight:bold;'>Client Domain Expiration Notification</span>", "<span style='font-weight:bold;'>" + domainCount + "</span> Email(s) to Client(s) for ");
            }
        }

        private void UpdateClientsWithExpiredAccount()
        {
            int listingCount = 0;
            int clientProfileCount = 0;
            int allPackageFeatureCount = 0;
            int domainCount = 0;
            Result<PackageOrderDetails> result = ClientPackageManager.SelectExpiredAccounts();
            if (result.Status == ResultStatus.Success)
            {
                foreach (PackageOrderDetails packageDetail in result.EntityList)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            PackageDetails package = null;
                            Result<PackageDetails> resultPackage = ClientPackageManager.SelectPackage(packageDetail.PackageCode);
                            if (resultPackage != null && resultPackage.EntityList.Count > 0)
                                package = ClientPackageManager.SelectPackage(packageDetail.PackageCode).EntityList[0];
                            ClientFeatureDetails selectedClientFeatureTemp = null;
                            Result<ClientFeatureDetails> featureResultTemp = ClientManager.SelectClientFeatureDetails(packageDetail.Client.ClientID);
                            if ((featureResultTemp.Status == ResultStatus.Success && featureResultTemp.EntityList.Count > 0) && package != null )
                            {
                                selectedClientFeatureTemp = featureResultTemp.EntityList[0];
                                switch (package.FeatureType)
                                {
                                    case "Listing":
                                        if (selectedClientFeatureTemp.Listing)
                                        {
                                            ClientManager.ClientFeatureSetListingStatus(packageDetail.Client.ClientID, false);
                                            listingCount++;
                                        }
                                        break;
                                    case "ClientProfile":
                                        if (selectedClientFeatureTemp.ClientProfile)
                                        {
                                            ClientManager.ClientFeatureSetClientProfileStatus(packageDetail.Client.ClientID, false);
                                            clientProfileCount++;
                                        }
                                        break;
                                    case "ClientDomain":
                                        if (selectedClientFeatureTemp.ClientDomain)
                                        {
                                            ClientManager.ClientFeatureSetClientDomainStatus(packageDetail.Client.ClientID, false);
                                            domainCount++;
                                        }
                                        break;
                                }
                            }
                            
                            ClientFeatureDetails selectedClientFeature = null;
                            Result<ClientFeatureDetails> featureResult = ClientManager.SelectClientFeatureDetails(packageDetail.Client.ClientID);
                            if (featureResult.Status == ResultStatus.Success && featureResult.EntityList.Count >0)
                            {
                                selectedClientFeature = featureResult.EntityList[0];
                                if (!selectedClientFeature.Listing && !selectedClientFeature.ClientProfile && !selectedClientFeature.ClientDomain)
                                {
                                    ClientDetails selectedClient = ClientManager.SelectClient(packageDetail.Client.ClientID).EntityList[0];
                                    if (selectedClient != null)
                                    {
                                        selectedClient.Comment = Status.InActiveByAccountExpiration.ToString();
                                        ClientManager.UnPublishClient(selectedClient.ClientID, selectedClient.Comment);
                                        List<Guid> userList = ClientManager.SelectUsers(selectedClient.ClientID);
                                        foreach (Guid id in userList)
                                        {
                                            MembershipUser userTemp = Membership.GetUser(id);
                                            if (userTemp.Comment == Status.Activated.ToString())
                                            {
                                                //allowing just the contact person to remain active for client maintenance (like renew, etc)
                                                if ((Guid)userTemp.ProviderUserKey != selectedClient.ContactPerson)
                                                {
                                                    userTemp.IsApproved = false;
                                                    userTemp.Comment = Status.InActiveByAccountExpiration.ToString();
                                                }
                                            }

                                            Membership.UpdateUser(userTemp);
                                        }

                                        allPackageFeatureCount++;
                                    }
                                }
                            }
                            scope.Complete();
                        }//end of try
                        catch (Exception ex)
                        {
                            Helpers.LogError(ex);
                        }

                    }//end of transaction scope
                }
                checkUp.Add("<span style='font-weight:bold;'>Account Expiration</span>", "<span style='font-weight:bold;'>" + allPackageFeatureCount + "</span> Client(s) set to ");
                checkUp.Add("<span style='font-weight:bold;'>Listing Business Expiration</span>", "<span style='font-weight:bold;'>" + listingCount + "</span> Client(s) set to ");
                checkUp.Add("<span style='font-weight:bold;'>Client Profile Expiration</span>", "<span style='font-weight:bold;'>" + clientProfileCount + "</span> Client(s) set to ");
                checkUp.Add("<span style='font-weight:bold;'>Client Domain Expiration</span>", "<span style='font-weight:bold;'>" + domainCount + "</span> Client(s) set to ");
            }
        }

        private void RemindForRenewalOfAdvertisement()
        {
            string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
            string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
            string appPath = Request.PhysicalApplicationPath;

            StreamReader bodyAdReader = new StreamReader(appPath + "EmailTemplates/AdvertisementExpiryNotification.txt");
            string bodyAd = bodyAdReader.ReadToEnd();
            bodyAdReader.Close();
            Result<AdvertisementDetails> result = AdvertisementManager.SelectAdvertisementsToRemindForRenewal();
            if (result.Status == ResultStatus.Success && result.EntityList != null)
            {
                foreach (AdvertisementDetails adDetail in result.EntityList)
                {
                    TimeSpan ts = ((DateTime)adDetail.EndDate).Subtract(DateTime.Now);
                    int daysLeft = ts.Days;
                    if (daysLeft < 0)
                        AdvertisementManager.SetAdvertisementPublishStatus(adDetail.AdID, false);

                    int updatedExpiry = (daysLeft / 3) * 2;
                    //UPDATE EXPIRY NOTICE
                    AdvertisementManager.UpdateAccountExpiryNotice(adDetail.AdID, updatedExpiry);
                    //SEND EMAIL TO THE ADVERTISER
                    string subject = "Advertisement Expiration: "+adDetail.Advertiser;
                    string tempBodyAd = bodyAd;

                    tempBodyAd = tempBodyAd.Replace("<%Logo%>", logoUrl);
                    tempBodyAd = tempBodyAd.Replace("<%TopBackGround%>", topBackGroundUrl);
                    tempBodyAd = tempBodyAd.Replace("<%Advertiser%>", adDetail.Advertiser);
                    tempBodyAd = tempBodyAd.Replace("<%DaysLeft%>", daysLeft.ToString());
                    tempBodyAd = tempBodyAd.Replace("<%AdTitle%>", adDetail.AdName);
                    string tempDimension ="";
                    if(string.Compare(adDetail.DisplayPosition, "right", true)==0)
                    {
                        tempDimension = Enum.GetName(typeof(AdDimensionRight), adDetail.FitToPanel).Replace('d', ' ').Trim();
                        tempDimension = string.Join("px X ", tempDimension.Split('x')) + "px";
                    }
                    else
                    {
                        tempDimension = Enum.GetName(typeof(AdDimensionFooter), adDetail.FitToPanel).Replace('d', ' ').Trim();
                        tempDimension = string.Join("px X ", tempDimension.Split('x')) + "px";
                    }
                    tempBodyAd = tempBodyAd.Replace("<%Dimension%>", tempDimension);
                    tempBodyAd = tempBodyAd.Replace("<%ExpiryDate%>", ((DateTime)adDetail.EndDate).ToShortDateString());

                    Helpers.SendEmail("noreply@sleeksurf.com", "SleekSurf", adDetail.Email, subject, tempBodyAd);

                }

                checkUp.Add("<span style='font-weight:bold;'>Advertisement Expiration</span>", "<span style='font-weight:bold;'>" + result.EntityList.Count + "</span> Email(s) to Advertiser(s) for ");
            }
        }

        private void SendRoutineEmailToWebMaster()
        {
            string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
            string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
            string appPath = Request.PhysicalApplicationPath;
            StreamReader srdr = new StreamReader(appPath + "EmailTemplates/DailyRoutineCheckup.txt");
            string dailyRoutineBody = srdr.ReadToEnd();
            srdr.Close();
            string description = "Daily check ups has been completed for today at " + System.DateTime.Now;
            dailyRoutineBody = dailyRoutineBody.Replace("<%Logo%>", logoUrl);
            dailyRoutineBody = dailyRoutineBody.Replace("<%TopBackGround%>", topBackGroundUrl);
            dailyRoutineBody = dailyRoutineBody.Replace("<%description%>", description);
            string descriptionSummary = string.Empty;
            foreach (KeyValuePair<string, string> thisFunction in checkUp)
            {
                descriptionSummary += "<span style='display:block; margin:5px;'>" + thisFunction.Value + " " + thisFunction.Key + "</span>";
            }
            dailyRoutineBody = dailyRoutineBody.Replace("<%No%>", descriptionSummary);

            Helpers.SendEmail("admin@sleeksurf.com", "Sleeksurf Admin", "webmasters@sleeksurf.com", "Routine CheckUps", dailyRoutineBody);
        }

        private void RemindForPurchaseOfInActiveByDefaultAccount()
        {
            string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
            string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
            string appPath = Request.PhysicalApplicationPath;

            StreamReader bodyAdReader = new StreamReader(appPath + "EmailTemplates/AdvertisementExpiryNotification.txt");
            string bodyAd = bodyAdReader.ReadToEnd();
            bodyAdReader.Close();
            Result<AdvertisementDetails> result = AdvertisementManager.SelectAdvertisementsToRemindForRenewal();
            if (result.Status == ResultStatus.Success && result.EntityList != null)
            {
                foreach (AdvertisementDetails adDetail in result.EntityList)
                {
                    TimeSpan ts = ((DateTime)adDetail.EndDate).Subtract(DateTime.Now);
                    int daysLeft = ts.Days;
                    if (daysLeft < 0)
                        AdvertisementManager.SetAdvertisementPublishStatus(adDetail.AdID, false);

                    int updatedExpiry = (daysLeft / 3) * 2;
                    //UPDATE EXPIRY NOTICE
                    AdvertisementManager.UpdateAccountExpiryNotice(adDetail.AdID, updatedExpiry);
                    //SEND EMAIL TO THE ADVERTISER
                    string subject = "Advertisement Expiration: " + adDetail.Advertiser;
                    string tempBodyAd = bodyAd;

                    tempBodyAd = tempBodyAd.Replace("<%Logo%>", logoUrl);
                    tempBodyAd = tempBodyAd.Replace("<%TopBackGround%>", topBackGroundUrl);
                    tempBodyAd = tempBodyAd.Replace("<%Advertiser%>", adDetail.Advertiser);
                    tempBodyAd = tempBodyAd.Replace("<%DaysLeft%>", daysLeft.ToString());
                    tempBodyAd = tempBodyAd.Replace("<%AdTitle%>", adDetail.AdName);
                    string tempDimension = "";
                    if (string.Compare(adDetail.DisplayPosition, "right", true) == 0)
                    {
                        tempDimension = Enum.GetName(typeof(AdDimensionRight), adDetail.FitToPanel).Replace('d', ' ').Trim();
                        tempDimension = string.Join("px X ", tempDimension.Split('x')) + "px";
                    }
                    else
                    {
                        tempDimension = Enum.GetName(typeof(AdDimensionFooter), adDetail.FitToPanel).Replace('d', ' ').Trim();
                        tempDimension = string.Join("px X ", tempDimension.Split('x')) + "px";
                    }
                    tempBodyAd = tempBodyAd.Replace("<%Dimension%>", tempDimension);
                    tempBodyAd = tempBodyAd.Replace("<%ExpiryDate%>", ((DateTime)adDetail.EndDate).ToShortDateString());

                    Helpers.SendEmail("noreply@sleeksurf.com", "SleekSurf", adDetail.Email, subject, tempBodyAd);

                }

                checkUp.Add("<span style='font-weight:bold;'>Advertisement Expiration</span>", "<span style='font-weight:bold;'>" + result.EntityList.Count + "</span> Email(s) to Advertiser(s) for ");
            }
        }
    }
}