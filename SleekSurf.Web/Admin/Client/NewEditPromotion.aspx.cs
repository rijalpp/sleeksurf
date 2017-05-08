using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using SleekSurf.FrameWork;
using SleekSurf.Entity;
using SleekSurf.Manager;
using System.Web.Services;
using System.IO;

namespace SleekSurf.Web.Admin.Client
{
    public partial class NewEditPromotion : BasePage
    {
        PromotionDetails promotion = new PromotionDetails();
        string promotionID = "";
        string clientID = "";
        static private List<CustomerDetails> customerListForEmail;
        static private List<CustomerDetails> customerListForSMS;
        static private bool groupSelection;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebContext.Parent != null)
                clientID = WebContext.Parent.ClientID;
            if (Session["Promotion"] != null)
            {
                promotionID = Session["Promotion"].ToString();
                hfPromoID.Value = promotionID;
                hfClientID.Value = clientID;
            }

            if (!IsPostBack)
            {
                customerListForEmail = new List<CustomerDetails>();
                customerListForSMS = new List<CustomerDetails>();
                groupSelection = false;

                if (Session["Promotion"] != null)
                {
                    lblTitle.Text += " - Update Mode";
                    BindPromotionDetails();
                    BindSendTo();
                    dvRightLinks.Visible = true;
                }
                else
                {
                    lblTitle.Text += " - Add Mode";
                    ClearFields();
                }
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Menu tempMenu = (Menu)Master.FindControl("ClientAdminNavMenu").FindControl("Menu");
            tempMenu.Items[tempMenu.Items.IndexOf(tempMenu.FindItem("PromotionMgmt"))].Selected = true;

            if (WebContext.Sibling != null)
            {
                Menu tmpMenu = (Menu)Master.FindControl("SuperAdminNavMenu").FindControl("Menu");
                tmpMenu.Items[tmpMenu.Items.IndexOf(tmpMenu.FindItem("ClientMgmt"))].Selected = true;
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearFields();
            Response.Redirect("~/Admin/Client/PromotionManagement.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        SavePromotion();
                        scope.Complete();
                    }
                    catch(Exception ex)
                    {
                        Helpers.LogError(ex);
                    }
                }
            }
        }

        [WebMethod()]
        public static int RetriveCustomersForEmail(string groupType, bool isChecked)
        {
            switch (groupType)
            {
                case "All":
                    if (isChecked)
                    {
                        customerListForEmail.Clear();
                        customerListForEmail.AddRange(CustomerManager.SelectAllClientCustomersWithSubscriptionEmail(WebContext.Parent.ClientID, true).EntityList);
                    }
                    else
                    {
                        foreach (CustomerDetails customer in CustomerManager.SelectAllClientCustomersWithSubscriptionEmail(WebContext.Parent.ClientID, true).EntityList)
                        {
                            customerListForEmail.Remove(customer);
                        }
                    }
                    break;
                default:
                    if (isChecked)
                    {
                        if (!groupSelection)
                            customerListForEmail.Clear();
                        groupSelection = true;
                        customerListForEmail.AddRange(CustomerManager.SelectCustomersWithEmailSubscriptionByCustomerGroupID(WebContext.Parent.ClientID, groupType, true).EntityList);
                    }
                    else
                    {
                        foreach (CustomerDetails customer in CustomerManager.SelectCustomersWithEmailSubscriptionByCustomerGroupID(WebContext.Parent.ClientID, groupType, true).EntityList)
                        {
                            customerListForEmail.Remove(customer);
                        }
                    }
                    break;
            }

            return customerListForEmail.Count;
        }

        [WebMethod()]
        public static int RetriveCustomersForSMS(string groupType, string isChecked)
        {
            switch (groupType)
            {
                case "All":
                    if(string.Compare(isChecked, "true", true) == 0)
                    {
                        customerListForSMS.Clear();
                        customerListForSMS.AddRange(CustomerManager.SelectAllClientCustomersWithSubscriptionSMS(WebContext.Parent.ClientID, true).EntityList);
                    }
                    else
                    {
                        foreach (CustomerDetails customer in CustomerManager.SelectAllClientCustomersWithSubscriptionSMS(WebContext.Parent.ClientID, true).EntityList)
                        {
                            customerListForSMS.Remove(customer);
                        }
                    }
                    break;
                default:
                    if (string.Compare(isChecked, "true", true) == 0)
                    {
                        if (!groupSelection)
                            customerListForSMS.Clear();
                        groupSelection = true;
                        customerListForSMS.AddRange(CustomerManager.SelectCustomersWithSMSSubscriptionByCustomerGroupID(WebContext.Parent.ClientID, groupType, true).EntityList);
                    }
                    else
                    {
                        foreach (CustomerDetails customer in CustomerManager.SelectCustomersWithSMSSubscriptionByCustomerGroupID(WebContext.Parent.ClientID, groupType, true).EntityList)
                        {
                            customerListForSMS.Remove(customer);
                        }
                    }
                    break;
            }

            return customerListForSMS.Count;
        }

        [WebMethod()]
        public static int CustomRetriveCustomersForEmail()
        {
            customerListForEmail.Clear();
            groupSelection = false; // to clear custom list created from custom selection option.
            if (HttpContext.Current.Session["CustomCustomerSelectionForEmail"] != null)
                customerListForEmail.AddRange((List<CustomerDetails>)HttpContext.Current.Session["CustomCustomerSelectionForEmail"]);

            return customerListForEmail.Count;
        }

        [WebMethod()]
        public static int CustomRetriveCustomersForSMS()
        {
            customerListForSMS.Clear();
            groupSelection = false; // to clear custom list created from custom selection option.
            if (HttpContext.Current.Session["CustomCustomerSelectionForSMS"] != null)
                customerListForSMS.AddRange((List<CustomerDetails>)HttpContext.Current.Session["CustomCustomerSelectionForSMS"]);

            return customerListForSMS.Count;
        }

        private void BindPromotionDetails()
        {

            promotion = ClientManager.SelectPromotion(promotionID, clientID).EntityList[0];

            string Section = "TITLE";
            imgThumb.ImageUrl = "~/DisplayImage.aspx?ID=" + promotion.PromotionID + "&Section=" + Section;
            txtTitle.Text = promotion.Title;
            txtStartDate.Text = promotion.StartDate.ToString();
            txtEndDate.Text = promotion.EndDate.ToString();
            editorDescription.Content = promotion.Description;
            chkPublish.Checked = promotion.IsActive;
        }

        private void BindSendTo()
        {
            Result<CustomerGroupDetails> resultEmail = CustomerManager.SelectCustomerGroupHavingCustomersBySubscriptionType(WebContext.Parent.ClientID, true, false);
            Result<CustomerGroupDetails> resultSMS = CustomerManager.SelectCustomerGroupHavingCustomersBySubscriptionType(WebContext.Parent.ClientID, false, true);
            if (resultEmail.Status == ResultStatus.Success)
            {
                List<CustomerGroupDetails> customerGroupsEmail = new List<CustomerGroupDetails>();

                foreach (CustomerGroupDetails customerGroup in resultEmail.EntityList)
                    customerGroupsEmail.Add(customerGroup);

                CustomerGroupDetails allEmail = new CustomerGroupDetails()
                {
                    ClientID = WebContext.Parent.ClientID,
                    CustomerGroupID = "All",
                    GroupName = "All",
                    CustomerCount = CustomerManager.SelectAllClientCustomersWithSubscriptionEmail(WebContext.Parent.ClientID, true).EntityList.Count
                };

                CustomerGroupDetails defaultGroupEmail = new CustomerGroupDetails()
                {
                    ClientID = WebContext.Parent.ClientID,
                    CustomerGroupID = "0",
                    GroupName = "Default",
                    CustomerCount = CustomerManager.SelectCustomersWithEmailSubscriptionByCustomerGroupID(WebContext.Parent.ClientID, "0", true).EntityList.Count
                };

                if (!customerGroupsEmail.Contains(defaultGroupEmail))
                {
                    customerGroupsEmail.Insert(0, allEmail);
                    if (defaultGroupEmail.CustomerCount > 0)
                        customerGroupsEmail.Insert(1, defaultGroupEmail);
                }

                var tempCustomerGroupsEmail = (from customer in customerGroupsEmail
                                               select new
                                               {
                                                   CustomerGroupID = customer.CustomerGroupID,
                                                   GroupNameWithCount = string.Format("{0}({1})", customer.GroupName, customer.CustomerCount)
                                               });

                chkblSendEmail.DataSource = tempCustomerGroupsEmail;
                chkblSendEmail.DataTextField = "GroupNameWithCount";
                chkblSendEmail.DataValueField = "CustomerGroupID";
                chkblSendEmail.DataBind();
            }

            if (resultSMS.Status == ResultStatus.Success)
            {
                List<CustomerGroupDetails> customerGroupsSMS = new List<CustomerGroupDetails>();

                foreach (CustomerGroupDetails customerGroup in resultSMS.EntityList)
                    customerGroupsSMS.Add(customerGroup);

                CustomerGroupDetails allSMS = new CustomerGroupDetails()
                {
                    ClientID = WebContext.Parent.ClientID,
                    CustomerGroupID = "All",
                    GroupName = "All",
                    CustomerCount = CustomerManager.SelectAllClientCustomersWithSubscriptionSMS(WebContext.Parent.ClientID, true).EntityList.Count
                };

                CustomerGroupDetails defaultGroupSMS = new CustomerGroupDetails()
                {
                    ClientID = WebContext.Parent.ClientID,
                    CustomerGroupID = "0",
                    GroupName = "Default",
                    CustomerCount = CustomerManager.SelectCustomersWithSMSSubscriptionByCustomerGroupID(WebContext.Parent.ClientID, "0", true).EntityList.Count
                };

                if (!customerGroupsSMS.Contains(defaultGroupSMS))
                {
                    customerGroupsSMS.Insert(0, allSMS);
                    if (defaultGroupSMS.CustomerCount > 0)
                        customerGroupsSMS.Insert(1, defaultGroupSMS);
                }

                var tempCustomerGroupsSMS = (from customer in customerGroupsSMS
                                             select new
                                             {
                                                 CustomerGroupID = customer.CustomerGroupID,
                                                 GroupNameWithCount = string.Format("{0}({1})", customer.GroupName, customer.CustomerCount)
                                             });

                chkblSendSMS.DataSource = tempCustomerGroupsSMS;
                chkblSendSMS.DataTextField = "GroupNameWithCount";
                chkblSendSMS.DataValueField = "CustomerGroupID";
                chkblSendSMS.DataBind();
            }
  
        }

        private void SavePromotion()
        {
            if (chkSendSMS.Checked && WebContext.Parent.SMSCredit < customerListForSMS.Count)
            {
                lblMessage.CssClass = "errorMsg";
                lblMessage.Text = "Not enough SMS credit left to send sms to all selected customers. Please recharge SMS credit.";
                return;
            }

            promotion.Title = txtTitle.Text;
            // promotion.TitleImage
            if (ucFileUpload.ImageControl.HasFile && ucFileUpload.ImageControl != null)
            {
                HttpPostedFile file = ucFileUpload.ImageControl.PostedFile;
                promotion.TitleImage = Helpers.ResizeImage(file.InputStream, 650);
            }

            // promotion.SupportingImage=
            if (ucFileUploadSupport.ImageControl.HasFile && ucFileUploadSupport.ImageControl != null)
            {
                HttpPostedFile file = ucFileUploadSupport.ImageControl.PostedFile;
                promotion.SupportingImage = Helpers.ResizeImage(file.InputStream, 650); ;
            }

            promotion.StartDate = Convert.ToDateTime(txtStartDate.Text);
            promotion.EndDate = Convert.ToDateTime(txtEndDate.Text);
            promotion.Description = editorDescription.Content;
            promotion.IsActive = chkPublish.Checked;
            promotion.ClientID = new ClientDetails() { ClientID = clientID };
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            if (!string.IsNullOrWhiteSpace(promotionID))
            {
                promotion.PromotionID = promotionID;
                result = ClientManager.UpdatePromotion(promotion);
                if (result.Status == ResultStatus.Success)
                {
                    if(chkSendEmail.Checked)
                        SendPromotionToCustomers();

                    if (chkSendSMS.Checked)
                        SendSMSToCustomers();
                }
            }
            else
            {
                promotion.PromotionID = System.DateTime.Now.ToString("PM-ddMMyyy-HHmmssfff");
                result = ClientManager.InsertPromotion(promotion);
            }
            if (result.Status == ResultStatus.Success)
            {
                lblMessage.CssClass = "successMsg";
                Session.Remove("Promotion");
                lblTitle.Text += " - Add Mode";
                ClearFields();
            }
            else
                lblMessage.CssClass = "errorMsg";
            lblMessage.Text = result.Message;
        }

        private void SendPromotionToCustomers()
        {
            string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
            string logoDisplay ="block";
            string urlPath = Server.MapPath("~/Uploads/" + WebContext.Parent.ClientID + "/LogoPicture/" + WebContext.Parent.LogoUrl);
            if (System.IO.File.Exists(urlPath))
            {
                logoUrl = FullBaseUrl + "Uploads/" + WebContext.Parent.ClientID + "/LogoPicture/" + WebContext.Parent.LogoUrl;
            }
            else
                logoDisplay = "none"; 

            string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
            string appPath = Request.PhysicalApplicationPath;
            StreamReader promotionBodySR = new StreamReader(appPath + "EmailTemplates/PromotionNotificationToCustomers.txt");
            string promoBody = promotionBodySR.ReadToEnd();
            promotionBodySR.Close();

            promoBody = promoBody.Replace("<%Logo%>", logoUrl);
            promoBody = promoBody.Replace("<%LogoDisplay%>", logoDisplay);
            promoBody = promoBody.Replace("<%TopBackGround%>", topBackGroundUrl);
            if (!string.IsNullOrEmpty(WebContext.Parent.UniqueDomain) || !string.IsNullOrEmpty(WebContext.Parent.UniqueIdentity))
            {
                if (string.IsNullOrEmpty(WebContext.Parent.UniqueDomain))
                {
                    promoBody = promoBody.Replace("<%ViewOnline%>", FullBaseUrl + WebContext.Parent.UniqueIdentity + "/Promotions");
                    promoBody = promoBody.Replace("<%Feedback%>", FullBaseUrl + WebContext.Parent.UniqueIdentity + "/ContactUs");
                }
                else
                {
                    promoBody = promoBody.Replace("<%ViewOnline%>", WebContext.Parent.UniqueDomain + "/Promotions");
                    promoBody = promoBody.Replace("<%Feedback%>", WebContext.Parent.UniqueDomain + "/ContactUs");
                }

                promoBody = promoBody.Replace("<%DisplayLink%>", "visible");
            }
            else
                promoBody = promoBody.Replace("<%DisplayLink%>", "hidden");

            string Section = "TITLE";
            string promotionPhoto = "";

            promotionPhoto = FullBaseUrl + "DisplayImage.aspx?ID=" + promotionID + "&SECTION=" + Section + "&ClientID=" + WebContext.Parent.ClientID;
            promoBody = promoBody.Replace("<%PromotionPhoto%>", promotionPhoto);
            promoBody = promoBody.Replace("<%PromotionName%>", promotion.Title);
            promoBody = promoBody.Replace("<%StartDate%>", promotion.StartDate.ToShortDateString());
            promoBody = promoBody.Replace("<%EndDate%>", promotion.EndDate.ToShortDateString());
            promoBody = promoBody.Replace("<%Description%>", promotion.Description);

            if (customerListForEmail.Count > 0)
            {
                string from = WebContext.Parent.BusinessEmail;
                string fromName = WebContext.Parent.ClientName;
                string subject = promotion.Title;
                foreach (CustomerDetails customer in customerListForEmail)
                {
                    string tempPromoBody = promoBody;
                    tempPromoBody = tempPromoBody.Replace("<%CustomerFullName%>", customer.FullName);
                    string unSubscribe = FullBaseUrl +"WebPages/Unsubscribe.aspx?ID="+customer.CustomerID+"&CID=" + WebContext.Parent.ClientID;
                    tempPromoBody = tempPromoBody.Replace("<%Unsubscribe%>", unSubscribe);
                    Helpers.SendEmail(from, fromName, customer.Email, subject, tempPromoBody);
                }

                customerListForEmail.Clear();
                Session.Remove("CustomCustomerSelectionForEmail");
            }
        }

        private void SendSMSToCustomers()
        {
            if (customerListForSMS.Count > 0)
            {
                string api = string.Empty, username = string.Empty, password = string.Empty, smsText = string.Empty, webURL = string.Empty;
                api = Configuration.GetConfigurationSetting("SMSGlobalAPI", typeof(string)) as string;
                username = Configuration.GetConfigurationSetting("SMSGlobalUsername", typeof(string)) as string;
                password = Configuration.GetConfigurationSetting("SMSGlobalPwd", typeof(string)) as string;

                if (!string.IsNullOrEmpty(WebContext.Parent.UniqueDomain))
                    webURL = WebContext.Parent.UniqueDomain;
                else if (!string.IsNullOrEmpty(WebContext.Parent.UniqueIdentity))
                    webURL = WebContext.Parent.UniqueIdentity;
                else if (!string.IsNullOrEmpty(WebContext.Parent.BusinessUrl))
                    webURL = WebContext.Parent.BusinessUrl;
                else
                    webURL = "our website";
                foreach (CustomerDetails customer in customerListForSMS)
                {
                    smsText = "Dear "+customer.FirstName+", " + promotion.Title + " is available for limited time. Please visit " + webURL + " for details.";
                    if (smsText.Length >= 160)
                        smsText = smsText.Substring(0, 159);
                    string status = Helpers.SendSMS(api, username, password, WebContext.Parent.ClientName.Substring(0, 11), customer.ContactMobile, smsText);
                }

                if (ClientManager.UpdateSMSCredit(WebContext.Parent.ClientID, -customerListForSMS.Count))
                    WebContext.Parent.SMSCredit -= customerListForSMS.Count;
            }
        }

        private void ClearFields()
        {
            txtTitle.Text = string.Empty;
            txtStartDate.Text = string.Empty;
            txtEndDate.Text = string.Empty;
            editorDescription.Content = string.Empty;
            chkPublish.Checked = false;
            imgThumb.Visible = false;
            CollapsiblePanelExtender1.Collapsed = true;
            CollapsiblePanelExtender1.ClientState = "true";
            dvRightLinks.Visible = false;
            pnUpdateMode.Visible = false;
            dvRightLinks.Visible = false;
            chkSendEmail.Checked = false;
            chkSendSMS.Checked = false;
            Session.Remove("Promotion");
        }
    }
}