using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SleekSurf.Entity;

namespace SleekSurf.DataAccess.SqlClient
{
    public class SqlCustomerProvider : CustomerProvider
    {
        public override int InsertCustomer(CustomerDetails customer)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customerID", SqlDbType.VarChar).Value = customer.CustomerID;
                cmd.Parameters.Add("@avatarUrl", SqlDbType.VarChar).Value = customer.AvatarUrl;
                cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = customer.Title;
                cmd.Parameters.Add("@firstName", SqlDbType.VarChar).Value = customer.FirstName;
                cmd.Parameters.Add("@middleName", SqlDbType.VarChar).Value = customer.MiddleName;
                cmd.Parameters.Add("@lastName", SqlDbType.VarChar).Value = customer.LastName;
                cmd.Parameters.Add("@dOB", SqlDbType.DateTime).Value = customer.DOB;
                cmd.Parameters.Add("@gender", SqlDbType.VarChar).Value = customer.Gender;
                cmd.Parameters.Add("@occupation", SqlDbType.VarChar).Value = customer.Occupation;
                cmd.Parameters.Add("@contactHome", SqlDbType.VarChar).Value = customer.ContactHome;
                cmd.Parameters.Add("@contactMobile", SqlDbType.VarChar).Value = customer.ContactMobile;
                cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = customer.Email;
                cmd.Parameters.Add("@addressLine1", SqlDbType.VarChar).Value = customer.AddressLine1;
                cmd.Parameters.Add("@addressLine2", SqlDbType.VarChar).Value = customer.AddressLine2;
                cmd.Parameters.Add("@addressLine3", SqlDbType.VarChar).Value = customer.AddressLine3;
                cmd.Parameters.Add("@city", SqlDbType.VarChar).Value = customer.City;
                cmd.Parameters.Add("@state", SqlDbType.VarChar).Value = customer.State;
                cmd.Parameters.Add("@postCode", SqlDbType.VarChar).Value = customer.PostCode;
                cmd.Parameters.Add("@createdBy", SqlDbType.UniqueIdentifier).Value = customer.CreatedBy;
                cmd.Parameters.Add("@updatedBy", SqlDbType.UniqueIdentifier).Value = customer.UpdatedBy;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = customer.ClientID.ClientID;
                cmd.Parameters.Add("@countryID", SqlDbType.VarChar).Value = customer.CountryID.CountryID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int UpdateCustomer(CustomerDetails customer)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customerID", SqlDbType.VarChar).Value = customer.CustomerID;
                cmd.Parameters.Add("@avatarUrl", SqlDbType.VarChar).Value = customer.AvatarUrl;
                cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = customer.Title;
                cmd.Parameters.Add("@firstName", SqlDbType.VarChar).Value = customer.FirstName;
                cmd.Parameters.Add("@middleName", SqlDbType.VarChar).Value = customer.MiddleName;
                cmd.Parameters.Add("@lastName", SqlDbType.VarChar).Value = customer.LastName;
                cmd.Parameters.Add("@dOB", SqlDbType.DateTime).Value = customer.DOB;
                cmd.Parameters.Add("@gender", SqlDbType.VarChar).Value = customer.Gender;
                cmd.Parameters.Add("@occupation", SqlDbType.VarChar).Value = customer.Occupation;
                cmd.Parameters.Add("@contactHome", SqlDbType.VarChar).Value = customer.ContactHome;
                cmd.Parameters.Add("@contactMobile", SqlDbType.VarChar).Value = customer.ContactMobile;
                cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = customer.Email;
                cmd.Parameters.Add("@addressLine1", SqlDbType.VarChar).Value = customer.AddressLine1;
                cmd.Parameters.Add("@addressLine2", SqlDbType.VarChar).Value = customer.AddressLine2;
                cmd.Parameters.Add("@addressLine3", SqlDbType.VarChar).Value = customer.AddressLine3;
                cmd.Parameters.Add("@city", SqlDbType.VarChar).Value = customer.City;
                cmd.Parameters.Add("@state", SqlDbType.VarChar).Value = customer.State;
                cmd.Parameters.Add("@postCode", SqlDbType.VarChar).Value = customer.PostCode;
                cmd.Parameters.Add("@updatedBy", SqlDbType.UniqueIdentifier).Value = customer.UpdatedBy;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = customer.ClientID.ClientID;
                cmd.Parameters.Add("@countryID", SqlDbType.VarChar).Value = customer.CountryID.CountryID;
                cmd.Parameters.Add("@customerGroupID", SqlDbType.NVarChar).Value = customer.CustomerGroupID;
                cmd.Parameters.Add("@subscriptionEmail", SqlDbType.Bit).Value = customer.SubscriptionEmail;
                cmd.Parameters.Add("@subscriptionSMS", SqlDbType.Bit).Value = customer.SubscriptionSMS;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int DeleteCustomer(string customerID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customerID", SqlDbType.NVarChar).Value = customerID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int AssignCustomerToCustomerGroup(string customerID, string clientID, string customerGroupID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerAssignToGroup", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customerID", SqlDbType.VarChar).Value = customerID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@customerGroupID", SqlDbType.VarChar).Value = customerGroupID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int SetCustomerToEmailSubscription(string customerID, string clientID, bool subscription)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomersSetSubscriptionEmail", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customerID", SqlDbType.VarChar).Value = customerID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@subscriptionEmail", SqlDbType.Bit).Value = subscription;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int SetCustomerToSMSSubscription(string customerID, string clientID, bool subscription)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomersSetSubscriptionSMS", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customerID", SqlDbType.VarChar).Value = customerID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@subscriptionSMS", SqlDbType.Bit).Value = subscription;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override CustomerDetails SelectCustomer(string customerID, string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerSelectByCustomerID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customerID", SqlDbType.VarChar, 50).Value = customerID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    return GetCustomerFromReader(reader);
                }
                else
                    return null;
            }
        }

        public override CustomerDetails SelectCustomerByEmail(string clientID, string email)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    return GetCustomerFromReader(reader);
                }
                return null;
            }
        }

        public override List<CustomerDetails> SelectCustomerByFullName(string name, string clientID, string customerGroupID, PagingDetails pEx)
        {
            List<CustomerDetails> customerList = new List<CustomerDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerGetCustomerByFullName", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pEx.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pEx.PageSize;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@customerGroupID", SqlDbType.NVarChar).Value = customerGroupID;
                cn.Open();
                customerList = GetCustomerCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerCountCustomerByFullName", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customerGroupID", SqlDbType.NVarChar).Value = customerGroupID;
                SqlParameter param = cmd.CreateParameter();
                param.ParameterName = "@totalNumber";
                param.SqlDbType = SqlDbType.Int;
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn1.Open();
                ExecuteScalar(cmd);
                pEx.TotalNumber = Convert.ToInt16(param.Value);
            }
            return customerList;
        }

        public override List<CustomerDetails> SelectAllCustomer(string customerGroupID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerSelectAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customerGroupID", SqlDbType.NVarChar).Value = customerGroupID;
                cn.Open();
                return GetCustomerCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<CustomerDetails> SelectAllClientCustomersWithSubscriptionEmail(string clientID, bool subscriptionEmail)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerSelectAllCustomersWithSubscriptionEmail", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.NVarChar).Value = clientID;
                cmd.Parameters.Add("@subscriptionEmail", SqlDbType.Bit).Value = subscriptionEmail;
                cn.Open();
                return GetCustomerCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<CustomerDetails> SelectAllClientCustomersWithSubscriptionSMS(string clientID, bool subscriptionSMS)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerSelectAllCustomersWithSubscriptionSMS", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.NVarChar).Value = clientID;
                cmd.Parameters.Add("@subscriptionSMS", SqlDbType.Bit).Value = subscriptionSMS;
                cn.Open();
                return GetCustomerCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<CustomerDetails> SelectCustomerByClientID(string clientID, string customerGroupID, PagingDetails pEx)
        {
            List<CustomerDetails> customerList = new List<CustomerDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerSelectCustomersByClientID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pEx.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pEx.PageSize;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@customerGroupID", SqlDbType.NVarChar).Value = customerGroupID; 
                cn.Open();
                customerList = GetCustomerCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerCountCustomersByClientID", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@customerGroupID", SqlDbType.NVarChar).Value = customerGroupID;
                cn1.Open();
                pEx.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return customerList;
        }

        public override List<CustomerDetails> SelectCustomersWithEmailSubscriptionByCustomerGroupID(string clientID, string customerGroupID, bool subscriptionEmail)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerGroupSelectCustomerWithEmailSubsctiptionByCustomerGroupID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@customerGroupID", SqlDbType.NVarChar).Value = customerGroupID;
                cmd.Parameters.Add("@subscriptionEmail", SqlDbType.Bit).Value = subscriptionEmail;
                cn.Open();
                return GetCustomerCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<CustomerDetails> SelectCustomersWithSMSSubscriptionByCustomerGroupID(string clientID, string customerGroupID, bool subscriptionSMS)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerGroupSelectCustomerWithSMSSubsctiptionByCustomerGroupID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@customerGroupID", SqlDbType.NVarChar).Value = customerGroupID;
                cmd.Parameters.Add("@subscriptionSMS", SqlDbType.Bit).Value = subscriptionSMS;
                cn.Open();
                return GetCustomerCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<CustomerGroupDetails> SelectCustomerGroupByName(string clientID, string groupName)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerGroupSelectByName", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@groupName", SqlDbType.NVarChar).Value = groupName;
                cn.Open();
                return GetCustomerGroupCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override bool DoesGroupNameExist(string clientID, string groupName)
        {
            bool result = false;
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerGroupCheckExistence", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@groupName", SqlDbType.NVarChar).Value = groupName;
                cn.Open();
                int i = (int)ExecuteScalar(cmd);
                if (i > 0)
                    result = true;
                return result;
            }
        }

        public override bool DoesCustomerExist(string clientID, string customerGroupID)
        {
            bool result = false;
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerGroupCountCustomersByGroup", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@customerGroupID", SqlDbType.NVarChar).Value = customerGroupID;
                cn.Open();
                int i = (int)ExecuteScalar(cmd);
                if (i > 0)
                    result = true;
                return result;
            }
        }

        #region CUSTOMER GROUP DETAILS

        public override int InsertCustomerGroup(CustomerGroupDetails customerGroup)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerGroupInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customerGroupID", SqlDbType.NVarChar).Value = customerGroup.CustomerGroupID;
                cmd.Parameters.Add("@groupName", SqlDbType.NVarChar).Value = customerGroup.GroupName;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = customerGroup.Description;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = customerGroup.Comments;
                cmd.Parameters.Add("@createdBy", SqlDbType.NVarChar).Value = customerGroup.CreatedBy;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = customerGroup.ClientID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int UpdateCustomerGroup(CustomerGroupDetails customerGroup)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerGroupUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customerGroupID", SqlDbType.NVarChar).Value = customerGroup.CustomerGroupID;
                cmd.Parameters.Add("@groupName", SqlDbType.NVarChar).Value = customerGroup.GroupName;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = customerGroup.Description;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = customerGroup.Comments;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = customerGroup.ClientID;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = customerGroup.Published;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int DeleteCustomerGroup(string customerGroupID, string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerGroupDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customerGroupID", SqlDbType.NVarChar).Value = customerGroupID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int SetCustomergroupPublishStatus(string customerGoupID, string clientID, bool published)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerGroupSetUpblishStatus", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customerGroupID", SqlDbType.NVarChar).Value = customerGoupID;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int CountCustomersInCustomerGroup(string customerGroupID, string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerGroupCountCustomersByGroup", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customerGroupID", SqlDbType.NVarChar).Value = customerGroupID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                return (int)ExecuteScalar(cmd);
            }
        }

        public override CustomerGroupDetails SelectCustomerGroup(string customerGroupID, string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerGroupSelect", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customerGroupID", SqlDbType.NVarChar).Value = customerGroupID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetCustomerGroupFromReader(reader);
                else
                    return null;
            }
        }

        public override List<CustomerGroupDetails> SelectAllCustomerGroup(string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerGroupSelectAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                return GetCustomerGroupCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<CustomerGroupDetails> SelectCustomerGroupsHavingCustomers(string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerGroupSelectHavingCustomers", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                return GetCustomerGroupIDNameCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<CustomerGroupDetails> SelectCustomerGroupHavingCustomersBySubscriptionType(string clientID, bool subscriptionEmail, bool subscriptionSMS)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomerGroupSelectHavingCustomersBySubscriptionType", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@subscriptionEmail", SqlDbType.Bit).Value = subscriptionEmail;
                cmd.Parameters.Add("@subscriptionSMS", SqlDbType.Bit).Value = subscriptionSMS;
                cn.Open();
                return GetCustomerGroupIDNameCollectionFromReader(ExecuteReader(cmd));
            }
        }

        #endregion
    }
}
