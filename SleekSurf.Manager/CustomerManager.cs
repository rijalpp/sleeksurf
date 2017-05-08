using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SleekSurf.Entity;
using SleekSurf.DataAccess;
using SleekSurf.FrameWork;

namespace SleekSurf.Manager
{
    public class CustomerManager : BaseCustomer
    {
        #region CUSTOMER
        
        public static Result<CustomerDetails> InsertCustomer(CustomerDetails customerDetails)
        {
            Result<CustomerDetails> result = new Result<CustomerDetails>();
            try
            {
                int i = SiteProvider.Customers.InsertCustomer(customerDetails);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(customerDetails);
                    result.Message = "Congratulations! The Customer Details have been successfully registered.";
                    BizObject.PurgeCacheItems("customers_customer");
                    BizObject.PurgeCacheItems("customergroups_customer");
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CustomerDetails> UpdateCustomer(CustomerDetails customerDetails)
        {
            Result<CustomerDetails> result = new Result<CustomerDetails>();
            try
            {
                int i = SiteProvider.Customers.UpdateCustomer(customerDetails);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(customerDetails);
                    result.Message = "The Customer details have been updated.";
                    BizObject.PurgeCacheItems("customers_customer");
                    BizObject.PurgeCacheItems("customergroups_customer");
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static bool DeleteCustomer(string customerID)
        {
            bool result = false;
            try
            {
                int i = SiteProvider.Customers.DeleteCustomer(customerID);
                if (i > 0)
                {
                    BizObject.PurgeCacheItems("customers_customer");
                    BizObject.PurgeCacheItems("customergroups_customer");
                    result = true;
                }
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
                result = false;
            }
            return result;

        }

        public static bool AssignCustomerToCustomerGroup(string customerID, string clientID, string customerGroupID)
        {
            bool result = false;
            try
            {
                int i = SiteProvider.Customers.AssignCustomerToCustomerGroup(customerID, clientID, customerGroupID);
                if (i > 0)
                {
                    BizObject.PurgeCacheItems("customers_customer");
                    BizObject.PurgeCacheItems("customergroups_customer");
                    result = true;
                }
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
                result = false;
            }
            return result;
        }

        public static bool SetCustomerToEmailSubscription(string customerID, string clientID, bool subscription)
        {
            bool result = false;
            try
            {
                int i = SiteProvider.Customers.SetCustomerToEmailSubscription(customerID, clientID, subscription);
                if (i > 0)
                {
                    BizObject.PurgeCacheItems("customers_customer");
                    result = true;
                }
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
                result = false;
            }
            return result;
        }

        public static bool SetCustomerToSMSSubscription(string customerID, string clientID, bool subscription)
        {
            bool result = false;
            try
            {
                int i = SiteProvider.Customers.SetCustomerToSMSSubscription(customerID, clientID, subscription);
                if (i > 0)
                {
                    BizObject.PurgeCacheItems("customers_customer");
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result = false;
            }
            return result;
        }

        public static Result<CustomerDetails> SelectCustomer(string customerID, string clientID)
        {
            Result<CustomerDetails> result = new Result<CustomerDetails>();
            string key = "customers_customer_" + customerID + clientID;
            try
            {
                if (BaseCustomer.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CustomerDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList.Add(SiteProvider.Customers.SelectCustomer(customerID, clientID));
                    if (result.EntityList.Count > 0)
                    {
                        result.Message = "Records successfully retrieved from Database.";
                    }
                    else if (result.EntityList.Count == 0)
                    {
                        result.Message = "No entry in Database.";
                    }
                    BaseCustomer.CacheData(key, result.EntityList);
                }
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CustomerDetails> SelectCustomerByEmail(string clientID, string email)
        {
            Result<CustomerDetails> result = new Result<CustomerDetails>();
            string key = "customers_customer_" + clientID + email;
            try
            {
                if (BaseCustomer.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CustomerDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList.Add(SiteProvider.Customers.SelectCustomerByEmail(clientID, email));
                    if (result.EntityList.Count > 0)
                    {
                        result.Message = "Records successfully retrieved from Database.";
                    }
                    else if (result.EntityList.Count == 0)
                    {
                        result.Message = "No entry in Database.";
                    }
                    BaseCustomer.CacheData(key, result.EntityList);
                }
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CustomerDetails> SelectCustomerByFullName(string name, string clientID, string customerGroupID, PagingDetails pEx)
        {
            Result<CustomerDetails> result = new Result<CustomerDetails>();
            string key = "customers_customer_" + name + clientID + customerGroupID + pEx.StartRowIndex + pEx.PageSize;
            string countNum = "customers_customer_Count" + name + clientID + customerGroupID + pEx.StartRowIndex + pEx.PageSize; ;
            try
            {
                if (BaseCustomer.Settings.EnableCaching && BizObject.Cache[key] != null && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CustomerDetails>)BizObject.Cache[key];
                    pEx.TotalNumber = (int)BizObject.Cache[countNum];
                }
                else
                {
                    result.EntityList = SiteProvider.Customers.SelectCustomerByFullName(name, clientID, customerGroupID, pEx);
                    if (result.EntityList.Count > 0)
                    {
                        result.Message = "Records successfully retrieved from Database.";
                    }
                    else if (result.EntityList.Count == 0)
                    {
                        result.Message = "No entry in Database.";
                    }
                    BaseCustomer.CacheData(key, result.EntityList);
                    BaseCustomer.CacheData(countNum, pEx.TotalNumber);
                }
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CustomerDetails> SelectAllCustomer(string customerGroupID)
        {
            Result<CustomerDetails> result = new Result<CustomerDetails>();
            string key = "customers_customer_" + customerGroupID;
            try
            {
                if (BaseCustomer.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CustomerDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Customers.SelectAllCustomer(customerGroupID);
                    if (result.EntityList.Count > 0)
                    {
                        result.Message = "Records successfully retrieved from Database.";
                    }
                    else if (result.EntityList.Count == 0)
                    {
                        result.Message = "No entry in Database.";
                    }
                    BaseCustomer.CacheData(key, result.EntityList);
                }
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CustomerDetails> SelectAllClientCustomersWithSubscriptionEmail(string clientID, bool subscriptionEmail)
        {
            Result<CustomerDetails> result = new Result<CustomerDetails>();
            string key = "customers_customer_" + clientID + "_" + subscriptionEmail + "_EMAIL";
            try
            {
                if (BaseCustomer.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CustomerDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Customers.SelectAllClientCustomersWithSubscriptionEmail(clientID, subscriptionEmail);
                    if (result.EntityList.Count > 0)
                    {
                        result.Message = "Records successfully retrieved from Database.";
                    }
                    else if (result.EntityList.Count == 0)
                    {
                        result.Message = "No entry in Database.";
                    }
                    BaseCustomer.CacheData(key, result.EntityList);
                }
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CustomerDetails> SelectAllClientCustomersWithSubscriptionSMS(string clientID, bool subscriptionSMS)
        {
            Result<CustomerDetails> result = new Result<CustomerDetails>();
            string key = "customers_customer_" + clientID+"_" + subscriptionSMS+"_SMS";
            try
            {
                if (BaseCustomer.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CustomerDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Customers.SelectAllClientCustomersWithSubscriptionSMS(clientID, subscriptionSMS);
                    if (result.EntityList.Count > 0)
                    {
                        result.Message = "Records successfully retrieved from Database.";
                    }
                    else if (result.EntityList.Count == 0)
                    {
                        result.Message = "No entry in Database.";
                    }
                    BaseCustomer.CacheData(key, result.EntityList);
                }
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CustomerDetails> SelectCustomersByClientID(string clientID, string customerGroupID, PagingDetails pEx)
        {
            Result<CustomerDetails> result = new Result<CustomerDetails>();
            string key = "customers_customer_" + clientID + customerGroupID + pEx.StartRowIndex + pEx.PageSize + customerGroupID;
            string countTotal = "customers_customer_Count" + clientID + customerGroupID + pEx.StartRowIndex + pEx.PageSize + customerGroupID;
            try
            {
                if (BaseCustomer.Settings.EnableCaching && BizObject.Cache[key] != null && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CustomerDetails>)BizObject.Cache[key];
                    pEx.TotalNumber = (int)BizObject.Cache[countTotal];
                    result.Message = "Records successfully retrieved from Cache.";
                }
                else
                {
                    result.EntityList = SiteProvider.Customers.SelectCustomerByClientID(clientID, customerGroupID, pEx);
                    if (result.EntityList.Count > 0)
                    {
                        result.Message = "Records successfully retrieved from Database.";
                    }

                    BaseCustomer.CacheData(key, result.EntityList);
                    BaseCustomer.CacheData(countTotal, pEx.TotalNumber);
                }
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CustomerDetails> SelectCustomersWithEmailSubscriptionByCustomerGroupID(string clientID, string customerGroupID, bool subscriptionEmail)
        {
            Result<CustomerDetails> result = new Result<CustomerDetails>();
            string key = "customers_customer_EMAIL_" + clientID + customerGroupID + subscriptionEmail.ToString();
            try
            {
                if (BaseCustomer.Settings.EnableCaching && BizObject.Cache[key] != null && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CustomerDetails>)BizObject.Cache[key];
                    result.Message = "Records successfully retrieved from Cache.";
                }
                else
                {
                    result.EntityList = SiteProvider.Customers.SelectCustomersWithEmailSubscriptionByCustomerGroupID(clientID, customerGroupID, subscriptionEmail);
                    if (result.EntityList.Count > 0)
                    {
                        result.Message = "Records successfully retrieved from Database.";
                    }
                    BaseCustomer.CacheData(key, result.EntityList);
                }
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CustomerDetails> SelectCustomersWithSMSSubscriptionByCustomerGroupID(string clientID, string customerGroupID, bool subscriptionSMS)
        {
            Result<CustomerDetails> result = new Result<CustomerDetails>();
            string key = "customers_customer_SMS_" + clientID + customerGroupID + subscriptionSMS.ToString();
            try
            {
                if (BaseCustomer.Settings.EnableCaching && BizObject.Cache[key] != null && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CustomerDetails>)BizObject.Cache[key];
                    result.Message = "Records successfully retrieved from Cache.";
                }
                else
                {
                    result.EntityList = SiteProvider.Customers.SelectCustomersWithSMSSubscriptionByCustomerGroupID(clientID, customerGroupID, subscriptionSMS);
                    if (result.EntityList.Count > 0)
                    {
                        result.Message = "Records successfully retrieved from Database.";
                    }
                    BaseCustomer.CacheData(key, result.EntityList);
                }
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        #endregion

        #region  CUSTOMER GROUP SECTION

        public static Result<CustomerGroupDetails> InsertCustomerGroup(CustomerGroupDetails customerGroup)
        {
            Result<CustomerGroupDetails> result = new Result<CustomerGroupDetails>();
            try
            {
                int i = SiteProvider.Customers.InsertCustomerGroup(customerGroup);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(customerGroup);
                    result.Message = "Congratulations! The Customer group Details have been successfully registered.";
                    BizObject.PurgeCacheItems("customergroups_customer");
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CustomerGroupDetails> UpdateCustomerGroup(CustomerGroupDetails customerGroup)
        {
            Result<CustomerGroupDetails> result = new Result<CustomerGroupDetails>();
            try
            {
                int i = SiteProvider.Customers.UpdateCustomerGroup(customerGroup);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(customerGroup);
                    result.Message = "The Customer Group details have been updated.";
                    BizObject.PurgeCacheItems("customergroups_customer");
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CustomerGroupDetails> SetCustomerGroupPublishStatus(string customerGroupID, string clientID, bool published)
        {
            Result<CustomerGroupDetails> result = new Result<CustomerGroupDetails>();
            try
            {
                int i = SiteProvider.Customers.SetCustomergroupPublishStatus(customerGroupID,clientID, published);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "The Customer Group details have been updated.";
                    BizObject.PurgeCacheItems("customergroups_customer");
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static bool DeleteCustomerGroup(string customerGroupID, string clientID)
        {
            bool result = false;
            try
            {
                int i = SiteProvider.Customers.DeleteCustomerGroup(customerGroupID, clientID);
                if (i > 0)
                {
                    BizObject.PurgeCacheItems("customergroups_customer");
                    result = true;
                }
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
                result = false;
            }
            return result;

        }

        public static int CountCustomersInCustomerGroup(string customerGroupID, string clientID)
        {
            int i = 0;
            string key = "customergroups_customer_CountBy_" + customerGroupID + clientID;
            try
            {
                if (Globals.Settings.Customers.EnableCaching && BizObject.Cache[key] != null)
                    i = (int)BizObject.Cache[key];
                else
                {
                    i = SiteProvider.Customers.CountCustomersInCustomerGroup(customerGroupID, clientID);
                    BaseCustomer.CacheData(key, i);
                }
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
                i = -1;
            }
            return i;
        }

        public static Result<CustomerGroupDetails> SelectCustomerGroup(string customerGroupID, string clientID)
        {
            Result<CustomerGroupDetails> result = new Result<CustomerGroupDetails>();
            string key = "customergroups_customer_" + customerGroupID;
            try
            {
                if (BaseCustomer.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CustomerGroupDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList.Add(SiteProvider.Customers.SelectCustomerGroup(customerGroupID, clientID));
                    if (result.EntityList.Count > 0)
                    {
                        result.Message = "Records successfully retrieved from Database.";
                    }
                    else if (result.EntityList.Count == 0)
                    {
                        result.Message = "No entry in Database.";
                    }
                    BaseCustomer.CacheData(key, result.EntityList);
                }
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CustomerGroupDetails> SelectAllCustomerGroup(string ClientID)
        {
            Result<CustomerGroupDetails> result = new Result<CustomerGroupDetails>();
            string key = "customergroups_customer_" + ClientID;
            try
            {
                if (BaseCustomer.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CustomerGroupDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Customers.SelectAllCustomerGroup(ClientID);
                    if (result.EntityList.Count > 0)
                    {
                        result.Message = "Records successfully retrieved from Database.";
                    }
                    else if (result.EntityList.Count == 0)
                    {
                        result.Message = "No entry in Database.";
                    }
                    BaseCustomer.CacheData(key, result.EntityList);
                }
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CustomerGroupDetails> SelectCustomerGroupsHavingCustomers(string clientID)
        {
            Result<CustomerGroupDetails> result = new Result<CustomerGroupDetails>();
            string key = "customergroups_customer_WithCustomers_" + clientID;
            try
            {
                if (BaseCustomer.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CustomerGroupDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Customers.SelectCustomerGroupsHavingCustomers(clientID);
                    if (result.EntityList.Count > 0)
                    {
                        result.Message = "Records successfully retrieved from Database.";
                    }
                    else if (result.EntityList.Count == 0)
                    {
                        result.Message = "No entry in Database.";
                    }
                    BaseCustomer.CacheData(key, result.EntityList);
                }
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CustomerGroupDetails> SelectCustomerGroupHavingCustomersBySubscriptionType(string clientID, bool subscriptionEmail, bool subscriptionSMS)
        {
            Result<CustomerGroupDetails> result = new Result<CustomerGroupDetails>();
            string key = "customergroups_customer_WithCustomers_" + clientID + "_" + subscriptionEmail.ToString() + "_" + subscriptionSMS.ToString();
            try
            {
                if (BaseCustomer.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CustomerGroupDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Customers.SelectCustomerGroupHavingCustomersBySubscriptionType(clientID, subscriptionEmail, subscriptionSMS);
                    if (result.EntityList.Count > 0)
                    {
                        result.Message = "Records successfully retrieved from Database.";
                    }
                    else if (result.EntityList.Count == 0)
                    {
                        result.Message = "No entry in Database.";
                    }
                    BaseCustomer.CacheData(key, result.EntityList);
                }
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CustomerGroupDetails> SelectCustomerGroupByName(string clientID, string groupName)
        {
            Result<CustomerGroupDetails> result = new Result<CustomerGroupDetails>();
            string key = "customergroups_customer_ByName_" + clientID + groupName;
            try
            {
                if (BaseCustomer.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CustomerGroupDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Customers.SelectCustomerGroupByName(clientID, groupName);
                    if (result.EntityList.Count > 0)
                    {
                        result.Message = "Records successfully retrieved from Database.";
                    }
                    else if (result.EntityList.Count == 0)
                    {
                        result.Message = "No entry in Database.";
                    }
                    BaseCustomer.CacheData(key, result.EntityList);
                }
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static bool DoesCustomerExist(string clientID, string customerGroupID)
        {
            return SiteProvider.Customers.DoesCustomerExist(clientID, customerGroupID);
        }

        public static bool DoesGroupNameExist(string clientID, string groupName)
        {
            return SiteProvider.Customers.DoesGroupNameExist(clientID, groupName);
        }

        #endregion
    }
}
