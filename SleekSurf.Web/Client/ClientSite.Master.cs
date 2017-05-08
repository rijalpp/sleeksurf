using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using SleekSurf.Manager;

namespace SleekSurf.Web.Client
{
    public partial class ClientSite : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uniqueIdentity = Page.RouteData.Values["uniqueIdentity"] as string;

            if (!string.IsNullOrEmpty(uniqueIdentity) && !uniqueIdentity.Contains('.'))
            {
                Result<ClientDetails> clientList = ClientManager.SelectClientByUniqueIdentity(uniqueIdentity);
                if (clientList.EntityList.Count > 0)
                {
                    if (clientList.EntityList[0].Published)
                    {
                        WebContext.ClientProfile = clientList.EntityList[0];
                        Result<ClientFeatureDetails> result = ClientManager.SelectClientFeatureDetails(WebContext.ClientProfile.ClientID);
                        if (!result.EntityList[0].ClientProfile)
                            Redirector.GoToSleekSurfWebsite();

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
                                    hlTermsAndConditions.NavigateUrl = BasePage.FullBaseUrl + uniqueIdentity + "/TermsAndConditions";

                                if (string.IsNullOrEmpty(dataExtender.PrivacyAndPolicy))
                                    hlPrivacyPolicy.Visible = false;
                                else
                                    hlPrivacyPolicy.NavigateUrl = BasePage.FullBaseUrl + uniqueIdentity + "/PrivacyPolicy";
                            }
                            else
                            {
                                hlTermsAndConditions.Visible = false;
                                hlPrivacyPolicy.Visible = false;
                            }
                          

                            Result<FAQGroupDetails> FAQGroupResult = ClientManager.SelectFaqGroupByClientID(WebContext.ClientProfile.ClientID);

                            if (FAQGroupResult.EntityList.Count > 0)
                                hlFAQs.NavigateUrl = BasePage.FullBaseUrl + uniqueIdentity + "/FAQs";
                            else
                                hlFAQs.Visible = false;

                            hlClientWebsite.NavigateUrl = (string.IsNullOrEmpty(WebContext.ClientProfile.BusinessUrl)) ? "" : WebContext.ClientProfile.BusinessUrl;

                            BindNavigationURL(uniqueIdentity);
                        }
                    }
                    else
                        Redirector.GoToSleekSurfWebsite();
                }
                else
                    Redirector.GoToSleekSurfWebsite();
            }

        }

        private void BindNavigationURL(string uniqueIdentity)
        {
            foreach (MenuItem item in NavigationMenu.Items)
            {
                item.NavigateUrl = BasePage.FullBaseUrl + uniqueIdentity + "/" + item.Text.Replace(" ", "");
            }
        }
    }
}