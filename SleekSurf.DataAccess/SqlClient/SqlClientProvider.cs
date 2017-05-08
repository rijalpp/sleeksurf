using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SleekSurf.Entity;

namespace SleekSurf.DataAccess.SqlClient
{
    public class SqlClientProvider : ClientProvider
    {
        public override List<string> GetMatchingKeyword(string prefix, int nRecordSet)
        {
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetMatchingKewordsByPrefix", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prefix", SqlDbType.VarChar).Value = prefix;
                cmd.Parameters.Add("@nRecordSet", SqlDbType.Int).Value = nRecordSet;
                conn.Open();
                IDataReader reader = cmd.ExecuteReader();

                List<string> keywordList = new List<string>();

                while (reader.Read())
                {
                    keywordList.Add(reader["MatchingWord"].ToString());
                }

                return keywordList;
            }
        }

        #region Methods of clients

        public override int InsertClient(ClientDetails client)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = client.ClientID;
                cmd.Parameters.Add("@uniqueIdentity", SqlDbType.VarChar).Value = client.UniqueIdentity;
                cmd.Parameters.Add("@abn", SqlDbType.NVarChar).Value = client.ABN;
                cmd.Parameters.Add("@clientName", SqlDbType.VarChar).Value = client.ClientName;
                cmd.Parameters.Add("@contactPerson", SqlDbType.UniqueIdentifier).Value = client.ContactPerson;
                cmd.Parameters.Add("@description", SqlDbType.NText).Value = client.Description;
                cmd.Parameters.Add("@comment", SqlDbType.VarChar).Value = client.Comment;
                cmd.Parameters.Add("@contactOffice", SqlDbType.VarChar).Value = client.ContactOffice;
                cmd.Parameters.Add("@contactFax", SqlDbType.VarChar).Value = client.ContactFax;
                cmd.Parameters.Add("@businessEmail", SqlDbType.VarChar).Value = client.BusinessEmail;
                cmd.Parameters.Add("@businessUrl", SqlDbType.VarChar).Value = client.BusinessUrl;
                cmd.Parameters.Add("@logoUrl", SqlDbType.VarChar).Value = client.LogoUrl;
                cmd.Parameters.Add("@addressLine1", SqlDbType.VarChar).Value = client.AddressLine1;
                cmd.Parameters.Add("@addressLine2", SqlDbType.VarChar).Value = client.AddressLine2;
                cmd.Parameters.Add("@addressLine3", SqlDbType.VarChar).Value = client.AddressLine3;
                cmd.Parameters.Add("@city", SqlDbType.VarChar).Value = client.City;
                cmd.Parameters.Add("@state", SqlDbType.VarChar).Value = client.State;
                cmd.Parameters.Add("@postCode", SqlDbType.VarChar).Value = client.PostCode;
                cmd.Parameters.Add("@countryID", SqlDbType.Int).Value = client.CountryID.CountryID;
                cmd.Parameters.Add("@establishedDate", SqlDbType.DateTime).Value = client.EstablishedDate;
                cmd.Parameters.Add("@createdBy", SqlDbType.UniqueIdentifier).Value = client.CreatedBy;
                cmd.Parameters.Add("@categoryID", SqlDbType.VarChar).Value = client.Category.CategoryID;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = client.Published;
                cmd.Parameters.Add("@latitude", SqlDbType.NVarChar).Value = client.Latitude;
                cmd.Parameters.Add("@longitude", SqlDbType.NVarChar).Value = client.Longitude;
                cmd.Parameters.Add("@uniqueDomain", SqlDbType.NVarChar).Value = client.UniqueDomain;
                cmd.Parameters.Add("@theme", SqlDbType.NVarChar).Value = client.Theme;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }

        }

        public override int UpdateClient(ClientDetails client)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = client.ClientID;
                cmd.Parameters.Add("@uniqueIdentity", SqlDbType.VarChar).Value = client.UniqueIdentity;
                cmd.Parameters.Add("@abn", SqlDbType.NVarChar).Value = client.ABN;
                cmd.Parameters.Add("@clientName", SqlDbType.VarChar).Value = client.ClientName;
                cmd.Parameters.Add("@contactPerson", SqlDbType.UniqueIdentifier).Value = client.ContactPerson;
                cmd.Parameters.Add("@description", SqlDbType.NText).Value = client.Description;
                cmd.Parameters.Add("@comment", SqlDbType.VarChar).Value = client.Comment;
                cmd.Parameters.Add("@contactOffice", SqlDbType.VarChar).Value = client.ContactOffice;
                cmd.Parameters.Add("@contactFax", SqlDbType.VarChar).Value = client.ContactFax;
                cmd.Parameters.Add("@businessEmail", SqlDbType.VarChar).Value = client.BusinessEmail;
                cmd.Parameters.Add("@businessUrl", SqlDbType.VarChar).Value = client.BusinessUrl;
                cmd.Parameters.Add("@logoUrl", SqlDbType.VarChar).Value = client.LogoUrl;
                cmd.Parameters.Add("@addressLine1", SqlDbType.VarChar).Value = client.AddressLine1;
                cmd.Parameters.Add("@addressLine2", SqlDbType.VarChar).Value = client.AddressLine2;
                cmd.Parameters.Add("@addressLine3", SqlDbType.VarChar).Value = client.AddressLine3;
                cmd.Parameters.Add("@city", SqlDbType.VarChar).Value = client.City;
                cmd.Parameters.Add("@state", SqlDbType.VarChar).Value = client.State;
                cmd.Parameters.Add("@postCode", SqlDbType.VarChar).Value = client.PostCode;
                cmd.Parameters.Add("@countryID", SqlDbType.Int).Value = client.CountryID.CountryID;
                cmd.Parameters.Add("@establishedDate", SqlDbType.DateTime).Value = client.EstablishedDate;
                cmd.Parameters.Add("@updatedBy", SqlDbType.UniqueIdentifier).Value = client.UpdatedBy;
                cmd.Parameters.Add("@categoryID", SqlDbType.VarChar).Value = client.Category.CategoryID;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = client.Published;
                cmd.Parameters.Add("@latitude", SqlDbType.NVarChar).Value = client.Latitude;
                cmd.Parameters.Add("@longitude", SqlDbType.NVarChar).Value = client.Longitude;
                cmd.Parameters.Add("@uniqueDomain", SqlDbType.NVarChar).Value = client.UniqueDomain;
                cmd.Parameters.Add("@theme", SqlDbType.NVarChar).Value = client.Theme;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int DeleteClient(string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int PublishClient(string clientID, string comment)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientPublish", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@comment", SqlDbType.VarChar).Value = comment;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int UnPublishClient(string clientID, string comment)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientUnPublish", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@comment", SqlDbType.VarChar).Value = comment;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int SetClientDomain(string clientID, string uniqueDomain)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSetDomain", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@uniqueDomain", SqlDbType.VarChar).Value = uniqueDomain;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int SetClientTheme(string clientID, string theme)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSetTheme", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@theme", SqlDbType.VarChar).Value = theme;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override ClientDetails SelectClient(string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectByClientID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar, 50).Value = clientID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    return GetClientFromReader(reader);
                }
                else
                    return null;
            }
        }

        public override int UpdateClientPageHit(string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientUpdatePageHit", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                ExecuteNonQuery(cmd);
            }

            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientCountPageHit", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                return (int)ExecuteScalar(cmd);
            }

        }

        public override int UpdateClientSearchHit(string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientUpdateSearchHit", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }

        }

        public override ClientDetails SelectClientByABN(string abn)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectByABN", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@abn", SqlDbType.VarChar, 50).Value = abn;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    return GetClientFromReader(reader);
                }
                else
                    return null;
            }
        }

        public override ClientDetails SelectClientByUniqueIdentity(string uniqueIdentity)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectClientByUniqueIdentity", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@uniqueIdentity", SqlDbType.VarChar).Value = uniqueIdentity;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetClientFromReader(reader);
                else
                    return null;
            }
        }

        public override ClientDetails SelectClientByUniqueDomain(string uniqueDomain)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectClientByUniqueDomain", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@uniqueDomain", SqlDbType.VarChar).Value = uniqueDomain;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetClientFromReader(reader);
                else
                    return null;
            }
        }

        public override List<ClientDetails> SelectClientsByCreatedPerson(Guid createdBy)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectByCreatedPersonWithoutPaging", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@createdBy", SqlDbType.UniqueIdentifier).Value = createdBy;
                cn.Open();
                return GetClientCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<ClientDetails> SelectClientsByStatusAfterGivenDaysOfCreation(int days, string comment, PagingDetails pgObj)
        {
            List<ClientDetails> clientList = new List<ClientDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectByStatusAfterGivenDaysOfCreation", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@days", SqlDbType.Int).Value = days;
                cmd.Parameters.Add("@comment", SqlDbType.VarChar).Value = comment;
                cn.Open();
                clientList = GetClientCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientCountByStatusAfterGivenDaysOfCreation", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@days", SqlDbType.Int).Value = days;
                cmd.Parameters.Add("@comment", SqlDbType.VarChar).Value = comment;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return clientList;
        }

        public override List<ClientDetails> SelectClientsByStatusAfterGivenDaysOfCreation(int days, string comment)
        {
            List<ClientDetails> clientList = new List<ClientDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectByStatusAfterGivenDaysOfCreationWithOutPaging", cn);
                cmd.CommandType = CommandType.StoredProcedure;
               
                cmd.Parameters.Add("@days", SqlDbType.Int).Value = days;
                cmd.Parameters.Add("@comment", SqlDbType.VarChar).Value = comment;
                cn.Open();
                clientList = GetClientCollectionFromReader(ExecuteReader(cmd));
            }
            return clientList;
        }

        public override List<ClientDetails> SelectClientsByCreatedPerson(Guid createdBy, PagingDetails pgObj)
        {
            List<ClientDetails> clientList = new List<ClientDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectByCreatedPerson", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@createdBy", SqlDbType.UniqueIdentifier).Value = createdBy;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cn.Open();
                clientList = GetClientCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientCountByCreatedPerson", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@createdBy", SqlDbType.UniqueIdentifier).Value = createdBy;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return clientList;
        }
        public override List<ClientDetails> SelectClientByBusinessNameCategoryAddress(string businessName, string category, string address, PagingDetails pEx)
        {
            List<ClientDetails> clientList = new List<ClientDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectByBusinessNameCategoryAddress", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pEx.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pEx.PageSize;
                if (businessName == null || businessName == "\"*\"")
                    cmd.Parameters.Add("@businessName", SqlDbType.VarChar).Value = "\"\"";
                else
                    cmd.Parameters.Add("@businessName", SqlDbType.VarChar).Value = businessName;
                if (category == "" || category == null)
                    cmd.Parameters.Add("@category", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@category", SqlDbType.VarChar).Value = category;
                if (address == null || address == "\"*\"")
                    cmd.Parameters.Add("@address", SqlDbType.VarChar).Value = "\"\"";
                else
                    cmd.Parameters.Add("@address", SqlDbType.VarChar).Value = address;
                cn.Open();
                clientList = GetClientCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientCountByBusinessNameCategoryAddress", cn1);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = cmd.CreateParameter();
                param.ParameterName = "@totalNumber";
                param.SqlDbType = SqlDbType.Int;
                //param.Value = SqlDbType.Int;
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);

                if (businessName == null || businessName == "\"*\"")
                    cmd.Parameters.Add("@businessName", SqlDbType.VarChar).Value = "\"\"";
                else
                    cmd.Parameters.Add("@businessName", SqlDbType.VarChar).Value = businessName;
                if (category == "" || category == null)
                    cmd.Parameters.Add("@category", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@category", SqlDbType.VarChar).Value = category;
                cmd.Parameters.Add("@address", SqlDbType.VarChar).Value = address;
                cn1.Open();
                cmd.ExecuteScalar();

                pEx.TotalNumber = Convert.ToInt16(param.Value);
            }
            return clientList;
        }

        public override List<ClientDetails> SelectClientByBusinessNameCategoryGeoLocation(string businessName, string category, decimal latitude, decimal longitude, decimal radius, PagingDetails pEx)
        {
            List<ClientDetails> clientList = new List<ClientDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectByBusinessNameCategoryGeoLocation", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pEx.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pEx.PageSize;
                if (businessName == null || businessName == "\"*\"")
                    cmd.Parameters.Add("@businessName", SqlDbType.VarChar).Value = "\"\"";
                else
                    cmd.Parameters.Add("@businessName", SqlDbType.VarChar).Value = businessName;
                if (category == "" || category == null)
                    cmd.Parameters.Add("@category", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@category", SqlDbType.VarChar).Value = category;
                cmd.Parameters.Add("@lat", SqlDbType.VarChar).Value = latitude;
                cmd.Parameters.Add("@lng", SqlDbType.VarChar).Value = longitude;
                cmd.Parameters.Add("@radius", SqlDbType.VarChar).Value = radius;
                cn.Open();
                clientList = GetClientCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientCountByBusinessNameCategoryGeoLocation", cn1);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = cmd.CreateParameter();
                param.ParameterName = "@totalNumber";
                param.SqlDbType = SqlDbType.Int;
                //param.Value = SqlDbType.Int;
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);

                if (businessName == null || businessName == "\"*\"")
                    cmd.Parameters.Add("@businessName", SqlDbType.VarChar).Value = "\"\"";
                else
                    cmd.Parameters.Add("@businessName", SqlDbType.VarChar).Value = businessName;
                if (category == "" || category == null)
                    cmd.Parameters.Add("@category", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@category", SqlDbType.VarChar).Value = category;
                cmd.Parameters.Add("@lat", SqlDbType.VarChar).Value = latitude;
                cmd.Parameters.Add("@lng", SqlDbType.VarChar).Value = longitude;
                cmd.Parameters.Add("@radius", SqlDbType.VarChar).Value = radius;
                cn1.Open();
                cmd.ExecuteScalar();

                pEx.TotalNumber = Convert.ToInt16(param.Value);
            }
            return clientList;
        }
        
        public override int CheckClientUniqueIdentity(string uniqueIdentity)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientCheckUniqueIdentity", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@uniqueIdentity", SqlDbType.VarChar).Value = uniqueIdentity;
                cn.Open();
                return (int)ExecuteScalar(cmd);
            }
        }

        public override int CheckClientUniqueDomain(string uniqueDomain)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientCheckUniqueDomain", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@uniqueDomain", SqlDbType.VarChar).Value = uniqueDomain;
                cn.Open();
                return (int)ExecuteScalar(cmd);
            }
        }

        public override List<ClientDetails> SelectAllClient(PagingDetails pEx)
        {
            List<ClientDetails> clientList = new List<ClientDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pEx.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pEx.PageSize;
                cn.Open();
                clientList = GetClientCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientCountAll", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cn1.Open();
                pEx.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return clientList;
        }

        public override List<ClientDetails> SelectAllClient(string prefix, PagingDetails pgObj)
        {
            List<ClientDetails> clientList = new List<ClientDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectAllWithPrefix", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@prefix", SqlDbType.NVarChar).Value = prefix;
                cn.Open();
                clientList = GetClientCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientCountAllWithPrefix", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prefix", SqlDbType.NVarChar).Value = prefix;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return clientList;
        }

        public override List<ClientDetails> SelectAllClient(Guid createdby, string prefix, PagingDetails pgObj)
        {
            List<ClientDetails> clientList = new List<ClientDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectAllWithPrefixByCreatedPerson", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@createdBy", SqlDbType.UniqueIdentifier).Value = createdby;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@prefix", SqlDbType.NVarChar).Value = prefix;
                cn.Open();
                clientList = GetClientCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientCountAllWithPrefixByCreatedPerson", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@createdBy", SqlDbType.UniqueIdentifier).Value = createdby;
                cmd.Parameters.Add("@prefix", SqlDbType.NVarChar).Value = prefix;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return clientList;
        }

        public override List<ClientDetails> SelectClientsByStatus(string clientStatus, PagingDetails pgObj)
        {
            List<ClientDetails> clientList = new List<ClientDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectAllByStatus", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientStatus", SqlDbType.NVarChar).Value = clientStatus;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cn.Open();
                clientList = GetClientCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientCountAllByStatus", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientStatus", SqlDbType.NVarChar).Value = clientStatus;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return clientList;
        }

        public override List<ClientDetails> SelectClientsByStatus(string clientStatus, Guid createdBy, PagingDetails pgObj)
        {
            List<ClientDetails> clientList = new List<ClientDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectByStatusByCreatedPerson", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientStatus", SqlDbType.NVarChar).Value = clientStatus;
                cmd.Parameters.Add("@createdBy", SqlDbType.UniqueIdentifier).Value = createdBy;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cn.Open();
                clientList = GetClientCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientCountByStatusByCreatedPerson", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientStatus", SqlDbType.NVarChar).Value = clientStatus;
                cmd.Parameters.Add("@createdBy", SqlDbType.UniqueIdentifier).Value = createdBy;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return clientList;
        }

        public override List<ClientDetails> SelectPublishedClient()
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectPublishedClient", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                return GetClientCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<ClientDetails> SelectUnPublishedClient()
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectUnPublishedClient", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                return GetClientCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<ClientDetails> SelectClientsWithPublication(Guid createdBy, bool published)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectByPublicationCreatedPersonWithNoPaging", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@createdBy", SqlDbType.UniqueIdentifier).Value = createdBy;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cn.Open();
                return GetClientCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<ClientDetails> SelectClientsWithPublication(Guid createdBy, bool published, PagingDetails pgObj)
        {
            List<ClientDetails> clientList = new List<ClientDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectByPublicationCreatedPerson", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@createdBy", SqlDbType.UniqueIdentifier).Value = createdBy;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cn.Open();
                clientList = GetClientCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientCountByPublicationCreatedPerson", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@createdBy", SqlDbType.UniqueIdentifier).Value = createdBy;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return clientList;
        }

        public override List<ClientDetails> SelectClientByCategoryIDClientName(string categoryID, string clientName)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientSelectClientByCategoryIDClientName", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@categoryID", SqlDbType.VarChar).Value = categoryID;
                cn.Open();
                return GetClientCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<Guid> SelectUsers(string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spSelectProfileByClientID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                return GetUserIDCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<Guid> SelectUsersWithOutClientID()
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spSelectProfileWithoutClientID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                return GetUserIDCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override string SelectClientIDByUserID(Guid userID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spSelectProfileByUserID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = userID;
                cn.Open();

                return ExecuteScalar(cmd).ToString();
            }
        }

        public override string SelectProfileClientID(Guid userID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spProfileSelectByUserID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = userID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return reader["ClientID"].ToString();
                else
                    return null;
            }
        }

        public override int UpdateProfileForClientID(Guid userID, string clientID)
        {
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spProfileUpdateForClientID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = userID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                conn.Open();
                return ExecuteNonQuery(cmd);
            }
        }
        #endregion

        #region  Mehods of Categories

        public override List<CategoryDetails> GetCategories()
        {
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCategorySelectAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                return GetCategoryCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<CategoryDetails> GetCategories(string prefix)
        {
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCategorySelectAllByPrefix", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prefix", SqlDbType.NVarChar).Value = prefix;
                conn.Open();
                return GetCategoryCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override CategoryDetails GetCategory(string categoryID)
        {
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spSelectCategoryByCategoryID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@categoryID", SqlDbType.VarChar).Value = categoryID;
                conn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    return GetCategoryFromReader(reader);
                }
                else
                    return null;
            }
        }

        public override int InsertCategory(CategoryDetails category)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCategoryInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@categoryID", SqlDbType.VarChar).Value = category.CategoryID;
                cmd.Parameters.Add("@categoryName", SqlDbType.VarChar).Value = category.CategoryName;
                cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = category.Description;
                cmd.Parameters.Add("@categoryImage", SqlDbType.Binary).Value = category.CategoryImage;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = category.Comments;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int UpdateCategory(CategoryDetails category)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCategoryUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@categoryID", SqlDbType.VarChar).Value = category.CategoryID;
                cmd.Parameters.Add("@categoryName", SqlDbType.VarChar).Value = category.CategoryName;
                cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = category.Description;
                cmd.Parameters.Add("@categoryImage", SqlDbType.Binary).Value = category.CategoryImage;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = category.Comments;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int DeleteCategory(string categoryID)
        {
            int i = 0;
            using (SqlConnection cnCheck = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCategoryCheckClients", cnCheck);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@categoryID", SqlDbType.VarChar).Value = categoryID;
                cnCheck.Open();
                i = (int)ExecuteScalar(cmd);
            }
            if (i == 0)
            {
                using (SqlConnection cn = new SqlConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("spCategoryDelete", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@categoryID", SqlDbType.VarChar).Value = categoryID;
                    cn.Open();
                    return ExecuteNonQuery(cmd);
                }
            }
            else
                return 0;
        }

        #endregion

        #region  Methods of Promotions

        public override int InsertPromotion(PromotionDetails promotion)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@promotionID", SqlDbType.NVarChar).Value = promotion.PromotionID;
                cmd.Parameters.Add("@title", SqlDbType.NVarChar).Value = promotion.Title;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = promotion.Description;
                cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = promotion.StartDate;
                cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = promotion.EndDate;
                cmd.Parameters.Add("@titleImage", SqlDbType.VarBinary).Value = promotion.TitleImage;
                cmd.Parameters.Add("@supportingImage", SqlDbType.VarBinary).Value = promotion.SupportingImage;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = promotion.IsActive;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = promotion.ClientID.ClientID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int UpdatePromotion(PromotionDetails promotion)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@promotionID", SqlDbType.NVarChar).Value = promotion.PromotionID;
                cmd.Parameters.Add("@title", SqlDbType.NVarChar).Value = promotion.Title;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = promotion.Description;
                cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = promotion.StartDate;
                cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = promotion.EndDate;
                cmd.Parameters.Add("@titleImage", SqlDbType.VarBinary).Value = promotion.TitleImage;
                cmd.Parameters.Add("@supportingImage", SqlDbType.VarBinary).Value = promotion.SupportingImage;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = promotion.IsActive;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = promotion.ClientID.ClientID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int SetPromotionActiveStatus(string promotionID, string clientID, bool isActive)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionSetActiveStatus", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@promotionID", SqlDbType.NVarChar).Value = promotionID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = isActive;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override PromotionDetails SelectPromotion(string promotionID, string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionSelectByPromotionID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@promotionID", SqlDbType.NVarChar).Value = promotionID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetPromotionFromReader(reader);
                else
                    return null;
            }
        }

        public override List<PromotionDetails> SelectAllCurrentPromotion(string clientID, PagingDetails pObj)
        {
            List<PromotionDetails> promotionList = new List<PromotionDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionSelectAllCurrent", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pObj.PageSize;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                promotionList = GetPromotionCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionCountAllCurrent", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn1.Open();
                pObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return promotionList;
        }

        public override List<PromotionDetails> SelectCurrentPromotionByPublication(string clientID, bool isActive, PagingDetails pObj)
        {
            List<PromotionDetails> promotionList = new List<PromotionDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionSelectCurrentByPublication", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pObj.PageSize;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = isActive;
                cn.Open();
                promotionList = GetPromotionCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionCountCurrentPublication", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = isActive;
                cn1.Open();
                pObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return promotionList;
        }

        public override List<PromotionDetails> SelectAllPromotionByPublication(string clientID, bool isActive, PagingDetails pObj)
        {
            List<PromotionDetails> promotionList = new List<PromotionDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionSelectAllByPublication", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pObj.PageSize;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = isActive;
                cn.Open();
                promotionList = GetPromotionCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionCountAllByPublication", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = isActive;
                cn1.Open();
                pObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return promotionList;
        }

        public override List<PromotionDetails> SelectPublishedCurrentPromotion(string clientID, PagingDetails pObj)
        {
            List<PromotionDetails> promotionList = new List<PromotionDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionSelectPublishedCurrent", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pObj.PageSize;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                promotionList = GetPromotionCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionCountPublishedCurrent", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn1.Open();
                pObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return promotionList;
        }

        public override List<PromotionDetails> SelectAllPromotion(string clientID, PagingDetails pObj)
        {
            List<PromotionDetails> promotionList = new List<PromotionDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionSelectAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pObj.PageSize;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                promotionList = GetPromotionCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionCountAll", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn1.Open();
                pObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return promotionList;
        }

        public override int DeletePromotion(string promotionID, string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@promotionID", SqlDbType.NVarChar).Value = promotionID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override List<PromotionDetails> SelectCurrentAndUpcomingPromotions(string clientID, int selectNRecords)
        {
            List<PromotionDetails> promotionList = new List<PromotionDetails>();
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionSelectCurrentAndUpcomingPromotions", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@selectNRecords", SqlDbType.Int).Value = selectNRecords;
                conn.Open();
                promotionList = GetPromotionCollectionFromReader(ExecuteReader(cmd));
            }
            return promotionList;
        }

        //NEWLY ADDED FUNCTIONS ON 12/07/2011
        //CURRENT AND UPCOMING
        public override List<PromotionDetails> SelectAllCurrentUpcomingPromotions(string clientID, PagingDetails pgObj)
        {
            List<PromotionDetails> promotionList = new List<PromotionDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionSelectAllCurrentUpcoming", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                promotionList = GetPromotionCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionCountAllCurrentUpcoming", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return promotionList;
        }
        public override List<PromotionDetails> SelectPublishedCurrentUpcomingPromotions(string clientID, PagingDetails pgObj)
        {
            List<PromotionDetails> promotionList = new List<PromotionDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionSelectAllCurrentUpcomingPublished", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                promotionList = GetPromotionCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionCountAllCurrentUpcomingPublished", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return promotionList;
        }
        public override List<PromotionDetails> SelectUnPublishedCurrentUpcomingPromotions(string clientID, PagingDetails pgObj)
        {
            List<PromotionDetails> promotionList = new List<PromotionDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionSelectAllCurrentUpcomingUnPublished", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                promotionList = GetPromotionCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionCountAllCurrentUpcomingUnPublished", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return promotionList;
        }
        //PAST PROMOTIONS
        public override List<PromotionDetails> SelectAllPastPromotions(string clientID, PagingDetails pgObj)
        {
            List<PromotionDetails> promotionList = new List<PromotionDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionSelectAllPast", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                promotionList = GetPromotionCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionCountAllPast", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return promotionList;
        }
        public override List<PromotionDetails> SelectPublishedPastPromotions(string clientID, PagingDetails pgObj)
        {
            List<PromotionDetails> promotionList = new List<PromotionDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionSelectAllPastPublished", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                promotionList = GetPromotionCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionCountAllPastPublished", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return promotionList;
        }
        public override List<PromotionDetails> SelectUnPublishedPastPromotions(string clientID, PagingDetails pgObj)
        {
            List<PromotionDetails> promotionList = new List<PromotionDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionSelectAllPastUnPublished", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                promotionList = GetPromotionCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionCountAllPastUnPublished", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return promotionList;
        }
        //FUTURE PROMOTIONS
        public override List<PromotionDetails> SelectAllUpcomingPromotions(string clientID, PagingDetails pgObj)
        {
            List<PromotionDetails> promotionList = new List<PromotionDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionSelectAllUpcoming", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                promotionList = GetPromotionCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionCountAllUpcoming", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return promotionList;
        }
        public override List<PromotionDetails> SelectPublishedUpcomingPromotions(string clientID, PagingDetails pgObj)
        {
            List<PromotionDetails> promotionList = new List<PromotionDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionSelectAllUpcomingPublished", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                promotionList = GetPromotionCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionCountAllUpcomingPublished", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return promotionList;
        }
        public override List<PromotionDetails> SelectUnPublishedUpcomingPromotions(string clientID, PagingDetails pgObj)
        {
            List<PromotionDetails> promotionList = new List<PromotionDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionSelectAllUpcomingUnPublished", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                promotionList = GetPromotionCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPromotionCountAllUpcomingUnPublished", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return promotionList;
        }
        #endregion

        #region Methods of Services

        public override ServiceDetails SelectService(string serviceID, string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spSelectService", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@serviceID", SqlDbType.NVarChar).Value = serviceID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetServiceFromReader(reader);
                else
                    return null;
            }
        }

        public override List<ServiceDetails> SelectServicesByClientID(string clientID, PagingDetails pObj)
        {
            List<ServiceDetails> serviceList = new List<ServiceDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spServiceSelectServicesByClientID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pObj.PageSize;
                cn.Open();
                serviceList = GetServiceCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spServiceCountServicesByClientID", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn1.Open();
                pObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return serviceList;
        }

        public override List<ServiceDetails> SelectServicesByClientID(string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spServiceSelectAllServiceByClientID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                return GetServiceCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override int InsertServiceForClient(ServiceDetails serviceDetails)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spInsertServiceForClient", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@serviceID", SqlDbType.NVarChar).Value = serviceDetails.ServiceID;
                cmd.Parameters.Add("@serviceDescription", SqlDbType.NVarChar).Value = serviceDetails.ServiceDescription;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = serviceDetails.Client.ClientID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int UpdateServiceForClient(ServiceDetails serviceDetails)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spUpdateServiceForClient", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@serviceID", SqlDbType.NVarChar).Value = serviceDetails.ServiceID;
                cmd.Parameters.Add("@serviceDescription", SqlDbType.NVarChar).Value = serviceDetails.ServiceDescription;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = serviceDetails.Client.ClientID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int DeleteServiceForClient(ServiceDetails serviceDetails)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteServiceForClient", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@serviceID", SqlDbType.NVarChar).Value = serviceDetails.ServiceID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = serviceDetails.Client.ClientID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }
        #endregion

        #region  METHODS of PAGEHIT
        public override int InsertPageHit(PageHitDetails pageHit)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPageHitInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pageHitID", SqlDbType.NVarChar).Value = pageHit.PageHitID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = pageHit.Client.ClientID;
                cmd.Parameters.Add("@hitType", SqlDbType.NVarChar).Value = pageHit.HitType;
                cmd.Parameters.Add("@ipAddress", SqlDbType.NVarChar).Value = pageHit.IPAddress;
                cmd.Parameters.Add("@countryCode", SqlDbType.NVarChar).Value = pageHit.Location.CountryCode;
                cmd.Parameters.Add("@countryName", SqlDbType.NVarChar).Value = pageHit.Location.CountryName;
                cmd.Parameters.Add("@regionName", SqlDbType.NVarChar).Value = pageHit.Location.RegionName;
                cmd.Parameters.Add("@cityName", SqlDbType.NVarChar).Value = pageHit.Location.CityName;
                cmd.Parameters.Add("@zipPostalCode", SqlDbType.NVarChar).Value = pageHit.Location.ZipPostalCode;
                cmd.Parameters.Add("@timeZoneName", SqlDbType.NVarChar).Value = pageHit.Location.Timezone.TimezoneName;
                cmd.Parameters.Add("@gmtoffset", SqlDbType.NVarChar).Value = pageHit.Location.Timezone.Gmtoffset;
                cmd.Parameters.Add("@dstoffset", SqlDbType.NVarChar).Value = pageHit.Location.Timezone.Dstoffset;
                cmd.Parameters.Add("@latitude", SqlDbType.NVarChar).Value = pageHit.Location.Position.Latitude;
                cmd.Parameters.Add("@longitude", SqlDbType.NVarChar).Value = pageHit.Location.Position.Longitude;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = pageHit.Comments;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override List<PageHitDetails> SelectPageHit(string clientID, string hitType, PagingDetails pgObj)
        {
            List<PageHitDetails> pageHitList = new List<PageHitDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPageHitSelectByClientID", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@hitType", SqlDbType.NVarChar).Value = hitType;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;

                cn.Open();
                pageHitList = GetPageHitCollectionFromReader(ExecuteReader(cmd));
            }

            pgObj.TotalNumber = CountPageHit(clientID, hitType);
            return pageHitList;
        }

        public override int CountPageHit(string clientID, string pageHitType)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPageHitCountByClientID", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@hitType", SqlDbType.NVarChar).Value = pageHitType;

                cn.Open();
                return (int)ExecuteScalar(cmd);

            }
        }

        public override List<PageHitDetails> SelectPageHitWithDateRange(string clientID, string hitType, DateTime fromDate, DateTime toDate, PagingDetails pgObj)
        {
            List<PageHitDetails> pageHitList = new List<PageHitDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPageHitSelectByClientIDWithDateRange", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@hitType", SqlDbType.NVarChar).Value = hitType;
                cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;

                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;

                cn.Open();

                pageHitList = GetPageHitCollectionFromReader(ExecuteReader(cmd));
                pgObj.TotalNumber = CountPageHitWithDateRange(clientID, hitType, fromDate, toDate);
            }

            return pageHitList;
        }

        public override int CountPageHitWithDateRange(string clientID, string hitType, DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPageHitCountByClientIDWithDateRange", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@hitType", SqlDbType.NVarChar).Value = hitType;
                cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;

                cn.Open();
                return (int)ExecuteScalar(cmd);

            }
        }

        #endregion

        #region  METHODS of FAQGROUP_DETAILS

        public override int InsertFaqGroup(FAQGroupDetails faqGroup)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spFaqGroupInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@faqGroupID", SqlDbType.NVarChar).Value = faqGroup.FaqGroupID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = faqGroup.Client.ClientID;
                cmd.Parameters.Add("@groupName", SqlDbType.NVarChar).Value = faqGroup.GroupName;
                cmd.Parameters.Add("@groupRank", SqlDbType.Int).Value = faqGroup.GroupRank;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = faqGroup.Comments;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int UpdateFaqGroup(FAQGroupDetails faqGroup)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spFaqGroupUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@faqGroupID", SqlDbType.NVarChar).Value = faqGroup.FaqGroupID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = faqGroup.Client.ClientID;
                cmd.Parameters.Add("@groupName", SqlDbType.NVarChar).Value = faqGroup.GroupName;
                cmd.Parameters.Add("@groupRank", SqlDbType.Int).Value = faqGroup.GroupRank;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = faqGroup.Comments;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int DeleteFaqGroup(string faqGroupID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spFaqGroupDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@faqGroupID", SqlDbType.NVarChar).Value = faqGroupID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override FAQGroupDetails SelectFaqGroupDetails(string faqGroupID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spFaqGroupSelect", cn);
                cmd.Parameters.Add("@faqGroupID", SqlDbType.NVarChar).Value = faqGroupID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetFaqGroupFromReader(reader);
                else
                    return null;

            }
        }

        public override List<FAQGroupDetails> SelectFaqGroupDetailsByClient(string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spFaqGroupSelectByClient", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                return GetFaqGroupCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<FAQGroupDetails> SelectFaqGroupWithClientNull()
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spFaqGroupSelectWithClientNull", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                return GetFaqGroupCollectionFromReader(ExecuteReader(cmd));
            }
        }

        #endregion

        #region METHODS THAT WORK WITH FAQ
        public override int InsertFaq(FAQDetails faq)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spFaqInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@faqID", SqlDbType.NVarChar).Value = faq.FaqID;
                cmd.Parameters.Add("@faqGroupID", SqlDbType.NVarChar).Value = faq.FaqGroup.FaqGroupID;
                cmd.Parameters.Add("@question", SqlDbType.NVarChar).Value = faq.Question;
                cmd.Parameters.Add("@answer", SqlDbType.NText).Value = faq.Answer;
                cmd.Parameters.Add("@faqRank", SqlDbType.Int).Value = faq.FaqRank;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = faq.Comments;
                cn.Open();

                return ExecuteNonQuery(cmd);
            }
        }

        public override int UpdateFaq(FAQDetails faq)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spFaqUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@faqID", SqlDbType.NVarChar).Value = faq.FaqID;
                cmd.Parameters.Add("@faqGroupID", SqlDbType.NVarChar).Value = faq.FaqGroup.FaqGroupID;
                cmd.Parameters.Add("@question", SqlDbType.NVarChar).Value = faq.Question;
                cmd.Parameters.Add("@answer", SqlDbType.NText).Value = faq.Answer;
                cmd.Parameters.Add("@faqRank", SqlDbType.Int).Value = faq.FaqRank;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = faq.Comments;
                cn.Open();

                return ExecuteNonQuery(cmd);
            }
        }

        public override int DeleteFaq(string faqID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spFaqDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@faqID", SqlDbType.NVarChar).Value = faqID;
                cn.Open();

                return ExecuteNonQuery(cmd);
            }
        }

        public override FAQDetails SelectFaqDetails(string faqID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spFaqSelect", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@faqID", SqlDbType.NVarChar).Value = faqID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetFaqFromReader(reader);
                else
                    return null;
            }
        }

        public override List<FAQDetails> SelectFaqDetailsByFaqGroup(string faqGroupID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spFaqSelectByFaqGroupID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@faqGroupID", SqlDbType.NVarChar).Value = faqGroupID;
                cn.Open();
                return GetFaqCollectionFromReader(ExecuteReader(cmd));
            }
        }
        #endregion

        #region METHODS THAT WORK WITH DATAEXTENDER
        public override int InsertDataExtender(DataExtenderDetails dataExtenderDetails)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spDataExtenderInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@dataExtenderID", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = dataExtenderDetails.Client.ClientID;
                cmd.Parameters.Add("@privacyAndPolicy", SqlDbType.NText).Value = dataExtenderDetails.PrivacyAndPolicy;
                cmd.Parameters.Add("@termsAndConditions", SqlDbType.NText).Value = dataExtenderDetails.TermsAndConditions;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = dataExtenderDetails.Comments;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }
        public override int UpdatePrivacyAndPolicy(DataExtenderDetails dataExtenderDetails)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spDataExtenderUpdatePrivacyPolicy", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@dataExtender", SqlDbType.Int).Value = dataExtenderDetails.DataExtenderID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = dataExtenderDetails.Client.ClientID;
                cmd.Parameters.Add("@privacyAndPolicy", SqlDbType.NText).Value = dataExtenderDetails.PrivacyAndPolicy;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = dataExtenderDetails.Comments;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }
        public override int UpdateTermsAndConditions(DataExtenderDetails dataExtenderDetails)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spDataExtenderUpdateTermsAndConditions", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@dataExtender", SqlDbType.Int).Value = dataExtenderDetails.DataExtenderID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = dataExtenderDetails.Client.ClientID;
                cmd.Parameters.Add("@termsAndCondtions", SqlDbType.NText).Value = dataExtenderDetails.TermsAndConditions;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = dataExtenderDetails.Comments;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }
        public override int UpdateDataExtender(DataExtenderDetails dataExtenderDetails)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spDataExtenderUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@dataExtenderID", SqlDbType.Int).Value = dataExtenderDetails.DataExtenderID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = dataExtenderDetails.Client.ClientID;
                cmd.Parameters.Add("@termsAndConditions", SqlDbType.NText).Value = dataExtenderDetails.TermsAndConditions;
                cmd.Parameters.Add("@privacyAndPolicy", SqlDbType.NText).Value = dataExtenderDetails.PrivacyAndPolicy;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = dataExtenderDetails.Comments;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int DeleteDataExtender(int dataExtenderID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spDataExtenderDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@dataExtenderID", SqlDbType.Int).Value = dataExtenderID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override DataExtenderDetails SelectDataExtender(int dataExtenderID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spDataExtenderSelect", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@dataExtenderID", SqlDbType.Int).Value = dataExtenderID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetDataExtenderDetailsFromReader(reader);
                else
                    return null;
            }
        }

        public override List<DataExtenderDetails> SelectDataExtenderByClient(string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spDataExtenderSelectByClient", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.NVarChar).Value = clientID;
                cn.Open();
                return GetDataExtenderCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<DataExtenderDetails> SelectDataExtenderWithClientNull()
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spDataExtenderSelectWithClientNull", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                return GetDataExtenderCollectionFromReader(ExecuteReader(cmd));
            }
        }
        #endregion

        #region PAGEHIT ANALYTICS
        public override DataTable HitCountrySelect(DateTime dateFrom, string clientID, string hitType)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spHitCountrySelect", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@hitType", SqlDbType.NVarChar).Value = hitType;
                cn.Open();
                dt.Load(ExecuteReader(cmd));
                return dt;
            }
        }
        public override DataTable HitCountrySelectAll(string clientID, string hitType)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spHitCountrySelectAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@hitType", SqlDbType.NVarChar).Value = hitType;
                cn.Open();
                dt.Load(ExecuteReader(cmd));
                return dt;
            }
        }
        public override DataTable HitCitySelect(DateTime dateFrom, string countryName, string clientID, string hitType)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spHitCitySelect", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@countryName", SqlDbType.NVarChar).Value = countryName;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@hitType", SqlDbType.NVarChar).Value = hitType;
                cn.Open();
                dt.Load(ExecuteReader(cmd));
                return dt;
            }
        }

        public override DataTable HitCitySelectAll(string countryName, string clientID, string hitType)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spHitCitySelectAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@countryName", SqlDbType.NVarChar).Value = countryName;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@hitType", SqlDbType.NVarChar).Value = hitType;
                cn.Open();
                dt.Load(ExecuteReader(cmd));
                return dt;
            }
        }

        #endregion

        #region METHODS THAT WORK WITH SMS CREDIT

        public override int UpdateSMSCredit(string clientID, int newCredit)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientUpdateSMSCredit", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@newCredit", SqlDbType.Int).Value = newCredit;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        #endregion

        #region METHODS THAT WORK WITH CLIENT FEATURES
        public override bool ClientFeatureSetClientProfileStatus(string clientID, bool clientProfile)
        {
            bool result = false;
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientFeatureSetClientProfileStatus", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID",SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@clientProfile", SqlDbType.Bit).Value = clientProfile;
                cn.Open();
                int i = ExecuteNonQuery(cmd);
                if (i > 0)
                    result = true;
            }
            return result;
        }
        public override bool ClientFeatureSetListingStatus(string clientID, bool listing)
        {
            bool result = false;
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientFeatureSetListingStatus", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@listing", SqlDbType.Bit).Value = listing;
                cn.Open();
                int i = ExecuteNonQuery(cmd);
                if (i > 0)
                    result = true;
            }
            return result;
        }
        public override bool ClientFeatureSetClientDomainStatus(string clientID, bool clientDomain)
        {
            bool result = false;
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientFeatureSetClientDomainStatus", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@clientDomain", SqlDbType.Bit).Value = clientDomain;
                cn.Open();
                int i = ExecuteNonQuery(cmd);
                if (i > 0)
                    result = true;
            }
            return result;
        }
        public override ClientFeatureDetails SelectClientFeatureDetails(string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spClientFeatureSelect", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    return GetClientFeatureFromDataReader(reader);
                }
                else
                    return null;
               
            }
        }
        #endregion
    }
}
