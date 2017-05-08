﻿using System;
using System.Web.UI.WebControls;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using SleekSurf.Manager;

namespace SleekSurf.Domain
{
    public partial class TermsAndConditions : WebBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)(Master.Master.FindControl("NavigationMenu"));
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("Home"))].Selected = true;

            if (!IsPostBack)
            {
                Result<DataExtenderDetails> dataExtenderResult = ClientManager.SelectDataExtenderByClient(WebContext.ClientProfile.ClientID);

                if (dataExtenderResult.Status == ResultStatus.Success && dataExtenderResult.EntityList.Count > 0)
                {
                    DataExtenderDetails dataExtender = dataExtenderResult.EntityList[0];
                    if (dataExtender != null && string.IsNullOrEmpty(dataExtender.TermsAndConditions))
                        Redirector.GoToRequestedPage(BasePage.FullBaseUrl + "/" + WebContext.ClientProfile.UniqueIdentity);
                    else
                        ltrTermsAndConditions.Text = dataExtender.TermsAndConditions;
                }
                else
                    Redirector.GoToRequestedPage(BasePage.FullBaseUrl + WebContext.ClientProfile.UniqueIdentity);
            }
        }
    }
}