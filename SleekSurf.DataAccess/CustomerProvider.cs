using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using SleekSurf.FrameWork;
using SleekSurf.Entity;

namespace SleekSurf.DataAccess
{
    public abstract class CustomerProvider : DataAccess
    {
        static private CustomerProvider _instance = null;
        static public CustomerProvider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = (CustomerProvider)Activator.CreateInstance(Type.GetType(Globals.Settings.Customers.ProviderType));
                return _instance;
            }
        }

        public CustomerProvider()
        {
            this.ConnectionString = Globals.Settings.Customers.ConnectionString;
            this.EnableCaching = Globals.Settings.Customers.EnableCaching;
            this.CacheDuration = Globals.Settings.Customers.CacheDuration;
        }

        public abstract int InsertCustomer(CustomerDetails customer);

        public abstract int UpdateCustomer(CustomerDetails customer);

        public abstract int DeleteCustomer(string customerID);

        public abstract int AssignCustomerToCustomerGroup(string customerID, string clientID, string customerGroupID);

        public abstract int SetCustomerToEmailSubscription(string customerID, string clientID, bool subscription);

        public abstract int SetCustomerToSMSSubscription(string customerID, string clientID, bool subscription);

        public abstract CustomerDetails SelectCustomer(string customerID, string clientID);

        public abstract CustomerDetails SelectCustomerByEmail(string clientID, string email);

        public abstract List<CustomerDetails> SelectCustomerByFullName(string name, string clientID, string customerGroupID, PagingDetails pEx);

        public abstract List<CustomerDetails> SelectAllCustomer(string customerGroupID);

        public abstract List<CustomerDetails> SelectAllClientCustomersWithSubscriptionEmail(string clientID, bool subscriptionEmail);

        public abstract List<CustomerDetails> SelectAllClientCustomersWithSubscriptionSMS(string clientID, bool subscriptionSMS);

        public abstract List<CustomerDetails> SelectCustomerByClientID(string clientID, string customerGroupID, PagingDetails pEx);

        public abstract List<CustomerDetails> SelectCustomersWithEmailSubscriptionByCustomerGroupID(string clientID, string customerGroupID, bool subscriptionEmail);

        public abstract List<CustomerDetails> SelectCustomersWithSMSSubscriptionByCustomerGroupID(string clientID, string customerGroupID, bool subscriptionSMS);

        protected virtual CustomerDetails GetCustomerFromReader(IDataReader reader)
        {
            CustomerDetails customer = new CustomerDetails();
            customer.CustomerID = reader["CustomerID"].ToString();
            customer.AvatarUrl = reader["AvatarUrl"].ToString();
            customer.Title = reader["Title"].ToString();
            customer.FirstName = reader["FirstName"].ToString();
            customer.MiddleName = reader["MiddleName"].ToString();
            customer.LastName = reader["LastName"].ToString();
            if (reader["DOB"] != DBNull.Value)
                customer.DOB = (DateTime?)reader["DOB"];
            customer.Gender = reader["Gender"].ToString();
            customer.Occupation = reader["Occupation"].ToString();
            customer.ContactHome = reader["ContactHome"].ToString();
            customer.ContactMobile = reader["ContactMobile"].ToString();
            customer.Email = reader["Email"].ToString();
            customer.AddressLine1 = reader["AddressLine1"].ToString();
            customer.AddressLine2 = reader["AddressLine2"].ToString();
            customer.AddressLine3 = reader["AddressLine3"].ToString();
            customer.City = reader["City"].ToString();
            customer.State = reader["State"].ToString();
            customer.PostCode = reader["PostCode"].ToString();
            customer.CountryID = new CountryDetails() { CountryID = (int)reader["CountryID"] };
            customer.ClientID = new ClientDetails() { ClientID = reader["ClientID"].ToString() };
            customer.CreatedDate = (DateTime)reader["CreatedDate"];
            customer.CreatedBy = (Guid)reader["CreatedBy"];
            customer.UpdatedDate = (DateTime)reader["UpdatedDate"];
            customer.UpdatedBy = (Guid)reader["UpdatedBy"];
            customer.Comments = reader["Comments"].ToString();
            customer.CustomerGroupID = reader["CustomerGroupID"].ToString();
            if (reader["SubscriptionEmail"] != DBNull.Value)
                customer.SubscriptionEmail = (bool)reader["SubscriptionEmail"];
            if (reader["SubscriptionSMS"] != DBNull.Value)
                customer.SubscriptionSMS = (bool)reader["SubscriptionSMS"];
            return customer;
        }

        protected virtual List<CustomerDetails> GetCustomerCollectionFromReader(IDataReader reader)
        {
            List<CustomerDetails> customerList = new List<CustomerDetails>();
            while (reader.Read())
                customerList.Add(GetCustomerFromReader(reader));
            return customerList;
        }

        #region CUSTOMER GROUPS

        public abstract int InsertCustomerGroup(CustomerGroupDetails customerGroup);
        public abstract int UpdateCustomerGroup(CustomerGroupDetails customerGroup);
        public abstract int DeleteCustomerGroup(string customerGroupID, string clientID);
        public abstract int SetCustomergroupPublishStatus(string customerGoupID, string clientID, bool published);
        public abstract int CountCustomersInCustomerGroup(string customerGroupID, string clientID);
        public abstract CustomerGroupDetails SelectCustomerGroup(string customerGroupID, string clientID);
        public abstract List<CustomerGroupDetails> SelectAllCustomerGroup(string clientID);
        public abstract List<CustomerGroupDetails> SelectCustomerGroupsHavingCustomers(string clientID);
        public abstract List<CustomerGroupDetails> SelectCustomerGroupHavingCustomersBySubscriptionType(string clientID, bool subscriptionEmail, bool subscriptionSMS);
        public abstract List<CustomerGroupDetails> SelectCustomerGroupByName(string clientID, string groupName);
        public abstract bool DoesCustomerExist(string clientID, string customerGroupID);
        public abstract bool DoesGroupNameExist(string clientID, string groupName);

        protected virtual CustomerGroupDetails GetCustomerGroupFromReader(IDataReader reader)
        {
            CustomerGroupDetails customerGroup = new CustomerGroupDetails();
            customerGroup.CustomerGroupID = reader["CustomerGroupID"].ToString();
            customerGroup.GroupName = reader["GroupName"].ToString();
            customerGroup.Description = reader["Description"].ToString();
            customerGroup.Comments = reader["Comments"].ToString();
            customerGroup.Published = (bool)reader["Published"];
            customerGroup.CreatedDate = (DateTime)reader["CreatedDate"];
            customerGroup.CreatedBy = reader["CreatedBy"].ToString();
            customerGroup.ClientID = reader["ClientID"].ToString();
            if (reader["CustomerCount"] != DBNull.Value)
                customerGroup.CustomerCount = (int)reader["CustomerCount"];
            return customerGroup;
        }

        protected virtual CustomerGroupDetails GetCustomerGroupIDNameFromReader(IDataReader reader)
        {
            CustomerGroupDetails customerGroup = new CustomerGroupDetails();
            customerGroup.CustomerGroupID = reader["CustomerGroupID"].ToString();
            customerGroup.GroupName = reader["GroupName"].ToString();
            customerGroup.CustomerCount = (int)reader["CustomerCount"];

            return customerGroup;
        }

        protected virtual List<CustomerGroupDetails> GetCustomerGroupCollectionFromReader(IDataReader reader)
        {
            List<CustomerGroupDetails> customerGroupList = new List<CustomerGroupDetails>();
            while (reader.Read())
                customerGroupList.Add(GetCustomerGroupFromReader(reader));
            return customerGroupList;
        }

        protected virtual List<CustomerGroupDetails> GetCustomerGroupIDNameCollectionFromReader(IDataReader reader)
        {
            List<CustomerGroupDetails> customerGroupList = new List<CustomerGroupDetails>();
            while (reader.Read())
                customerGroupList.Add(GetCustomerGroupIDNameFromReader(reader));
            return customerGroupList;
        }

        #endregion
    }
}
