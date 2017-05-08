using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using SleekSurf.Web.com.sleeksurf.webservices.www;
using SleekSurf.FrameWork;
using SleekSurf.Entity;
using SleekSurf.Manager;
using System.IO;
using System.Web.Security;

namespace SleekSurf.Web
{
    /// <summary>
    /// Summary description for AutoCompleteServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [ScriptService]
    public class AutoCompleteLocalService : System.Web.Services.WebService
    {
        AutoCompleteService service = new AutoCompleteService();

        [WebMethod]
        public string[] GetMatchingKeyword(string prefixText, int nRecordSet)
        {
            prefixText = prefixText.Replace("\\", string.Empty);
            return service.GetMatchingKeyword(prefixText, nRecordSet);
        }

        [WebMethod]
        public bool SendClientInformationToEmail(string clientId, string emailAddress)
        {
            try
            {
                Result<ClientDetails> resultClient = ClientManager.SelectClient(clientId);

                if (resultClient.Status == ResultStatus.Success)
                {
                    ClientDetails client = resultClient.EntityList[0];
                    //embed image in the emails
                    // string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
                    string logoUrl = "";
                    string logoDisplay = "none";
                    if (!string.IsNullOrEmpty(client.LogoUrl))
                    {
                        string urlPath = Server.MapPath("~/Uploads/" + client.ClientID + "/LogoPicture/" + client.LogoUrl);
                        if (System.IO.File.Exists(urlPath))
                        {
                            logoUrl = BasePage.FullBaseUrl + "Uploads/" + client.ClientID + "/LogoPicture/" + client.LogoUrl;
                            logoDisplay = "block";
                        }
                    }
                    string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
                    string appPath = HttpContext.Current.Request.PhysicalApplicationPath;

                    StreamReader bodyBusinessReader = new StreamReader(appPath + "EmailTemplates/EmailBusinessDetails.txt");
                    string bodyBusiness = bodyBusinessReader.ReadToEnd();
                    bodyBusinessReader.Close();

                    bodyBusiness = bodyBusiness.Replace("<%Logo%>", logoUrl);
                    bodyBusiness = bodyBusiness.Replace("<%LogoDisplay%>", logoDisplay);
                    bodyBusiness = bodyBusiness.Replace("<%TopBackGround%>", topBackGroundUrl);
                    bodyBusiness = bodyBusiness.Replace("<%BusinessName%>", client.ClientName);
                    bodyBusiness = bodyBusiness.Replace("<%ContactPerson%>", GetContactPerson(client.ContactPerson.ToString()));
                    bodyBusiness = bodyBusiness.Replace("<%ContactNumber%>", client.ContactOffice);
                    bodyBusiness = bodyBusiness.Replace("<%BusinessEmail%>", client.BusinessEmail);
                    bodyBusiness = bodyBusiness.Replace("<%Address%>", client.Address);
                    bodyBusiness = bodyBusiness.Replace("<%Country%>", GetCountry(client.CountryID));

                    string otherDetails = string.Empty;

                    if (!string.IsNullOrEmpty(client.BusinessUrl))
                    {
                        otherDetails += "<span style='display:block; padding:2px 0px;'><span style='display:inline-block; width:100px; font-weight:bold; padding-right:10px; font-size:12px;'>Website: </span><span style='display:inline-block; width:300px; font-size:12px;'>" + client.BusinessUrl + "</span></span>";
                    }

                    if (!string.IsNullOrEmpty(client.ContactFax))
                    {
                        otherDetails += "<span style='display:block; padding:2px 0px;'><span style='display:inline-block; width:100px; font-weight:bold; padding-right:10px; font-size:12px;'>Fax: </span><span style='display:inline-block; width:300px; font-size:12px;'>" + client.ContactFax + "</span></span>";
                    }

                    if (!string.IsNullOrEmpty(client.UniqueDomain) || !string.IsNullOrEmpty(client.UniqueIdentity)) // IF BUSINESS HAS PROFILE OV BUSINESS.
                    {
                        bodyBusiness = bodyBusiness.Replace("<%OtherDetails%>", otherDetails);
                        bodyBusiness = bodyBusiness.Replace("<%otherDescription%>", "For maps, promotions and other information in details, please visit the business profile by clicking below.");
                        if (!string.IsNullOrEmpty(client.UniqueDomain) && !string.IsNullOrEmpty(client.UniqueIdentity))
                            bodyBusiness = bodyBusiness.Replace("<%BusinessProfile%>", "<a href='http://www." + client.UniqueIdentity + ".sleeksurf.com'>http://www." + client.UniqueIdentity + ".sleeksurf.com</a><br /> or <br /> <a href='http://www.sleeksurf.com/" + client.UniqueIdentity + "'>http://www.sleeksurf.com/" + client.UniqueIdentity + "</a> <br />or<br /><a href='" + client.UniqueDomain + "'>'" + client.UniqueDomain + "'</a>");
                        else if (string.IsNullOrEmpty(client.UniqueDomain))
                            bodyBusiness = bodyBusiness.Replace("<%BusinessProfile%>", "<a href='http://www." + client.UniqueIdentity + ".sleeksurf.com'>http://www." + client.UniqueIdentity + ".sleeksurf.com</a><br /> or <br /> <a href='http://www.sleeksurf.com/" + client.UniqueIdentity + "'>http://www.sleeksurf.com/" + client.UniqueIdentity + "</a>");
                        else
                            bodyBusiness = bodyBusiness.Replace("<%BusinessProfile%>", client.UniqueDomain);
                    }

                    else//IF THE BUSINESS IS ONLY LISTED
                    {
                        bodyBusiness = bodyBusiness.Replace("<%OtherDetails%>", "");
                        bodyBusiness = bodyBusiness.Replace("<%otherDescription%>", "");
                        bodyBusiness = bodyBusiness.Replace("<%BusinessProfile%>", "");
                    }

                    try
                    {
                        Helpers.SendEmail("noreply@sleeksurf.com", "SleekSurf", emailAddress, "Business Details", bodyBusiness);
                        //Label lblMessage = (Label)rptrBusinessList.Items[temp].FindControl("lblMessage");
                       // lblMessage.CssClass = "success";
                       // lblMessage.Text = "Details sent to your nominated email.";
                    }
                    catch (Exception ex)
                    {
                        Helpers.LogError(ex);
                       // lblMessage.CssClass = "error";
                       // lblMessage.Text = "Error Sending Email, Please try later.";
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string GetContactPerson(string UserID)
        {
            Guid Userid = Guid.Parse(UserID);
            CustomUserProfile profile = CustomUserProfile.GetUserProfile(Membership.GetUser(Userid).UserName);
            return profile.FirstName + " " + profile.MiddleName + " " + profile.LastName;
        }

        private string GetCountry(object Country)
        {
            CountryDetails thisCountry = (CountryDetails)Country;
            return CountryManager.GetCountry(thisCountry.CountryID).EntityList[0].CountryName;
        }


    }
}
