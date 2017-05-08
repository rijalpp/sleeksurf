using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using SleekSurf.Manager;

namespace SleekSurf.Domain
{
    public partial class ClientSite : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rawUrl = Request.Url.ToString();
            string uniqueDomain = Request.Url.Host;
           
            //string rawUrl = "http://www.c1group.com.au";
            //string uniqueDomain = new Uri("http://www.c1group.com.au").Host;

            if (!string.IsNullOrEmpty(uniqueDomain))
            {
                Result<ClientDetails> clientList = ClientManager.SelectClientByUniqueDomain(uniqueDomain);
                if (clientList.EntityList.Count > 0)
                {
                    if (clientList.EntityList[0].Published)
                    {
                        WebContext.ClientProfile = clientList.EntityList[0];
                        Result<ClientFeatureDetails> result = ClientManager.SelectClientFeatureDetails(WebContext.ClientProfile.ClientID);
                        if (!result.EntityList[0].ClientDomain)
                            Redirector.GoToWebsiteUnavailablePage();

                        if (WebContext.ClientProfile != null)
                        {
                            lblClientName.Text = WebContext.ClientProfile.ClientName;
                            lblClientBusinessName.Text = WebContext.ClientProfile.ClientName;

                            Result<DataExtenderDetails> dataExtenderResult = ClientManager.SelectDataExtenderByClient(WebContext.ClientProfile.ClientID);
                            DataExtenderDetails dataExtender = new DataExtenderDetails();

                            if (dataExtenderResult != null && dataExtenderResult.EntityList.Count > 0)
                            {
                                dataExtender = dataExtenderResult.EntityList[0];


                                if (string.IsNullOrEmpty(dataExtender.TermsAndConditions))
                                    hlTermsAndConditions.Visible = false;
                                else
                                    hlTermsAndConditions.NavigateUrl = "~/TermsAndConditions.aspx";

                                if (string.IsNullOrEmpty(dataExtender.PrivacyAndPolicy))
                                    hlPrivacyPolicy.Visible = false;
                                else
                                    hlPrivacyPolicy.NavigateUrl = "~/PrivacyPolicy.aspx";
                            }
                            else
                            {
                                hlTermsAndConditions.Visible = false;
                                hlPrivacyPolicy.Visible = false;
                            }


                            Result<FAQGroupDetails> FAQGroupResult = ClientManager.SelectFaqGroupByClientID(WebContext.ClientProfile.ClientID);

                            if (FAQGroupResult.EntityList.Count > 0)
                                hlFAQs.NavigateUrl =  "~/FAQs.aspx";
                            else
                                hlFAQs.Visible = false;

                            hlClientWebsite.NavigateUrl = (string.IsNullOrEmpty(WebContext.ClientProfile.BusinessUrl)) ? "" : WebContext.ClientProfile.BusinessUrl;
                        }
                    }
                    else
                        Redirector.GoToSleekSurfWebsite();
                }
                else
                    Redirector.GoToSleekSurfWebsite();
            }

        }
    }
}