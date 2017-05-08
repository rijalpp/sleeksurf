using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SleekSurf.Entity;


namespace SleekSurf.DataAccess.SqlClient
{
    class SqlAdvertisementProvider:AdvertisementProvider
    {
        #region METHODS FOR BACKEND

        public override int InsertAdvertisement(AdvertisementDetails advertisement)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@adID", SqlDbType.NVarChar).Value = advertisement.AdID;
                cmd.Parameters.Add("@adName", SqlDbType.NVarChar).Value = advertisement.AdName;
                cmd.Parameters.Add("@advertiser", SqlDbType.NVarChar).Value = advertisement.Advertiser;
                cmd.Parameters.Add("@contactDetail", SqlDbType.NVarChar).Value = advertisement.ContactDetail;
                cmd.Parameters.Add("@ImageUrl", SqlDbType.NVarChar).Value = advertisement.ImageUrl;
                cmd.Parameters.Add("@navigateUrl", SqlDbType.NVarChar).Value = advertisement.NavigateUrl;
                cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = advertisement.StartDate;
                cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = advertisement.EndDate;
                cmd.Parameters.Add("@amountPaid", SqlDbType.Decimal).Value = advertisement.AmountPaid;
                cmd.Parameters.Add("@displayPosition", SqlDbType.NVarChar).Value = advertisement.DisplayPosition;
                cmd.Parameters.Add("@fitToPanel", SqlDbType.Int).Value = advertisement.FitToPanel;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = advertisement.Published;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = advertisement.ClientID;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = advertisement.Comments;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = advertisement.Email;
                cmd.Parameters.Add("@createdBy", SqlDbType.NVarChar).Value = advertisement.CreatedBy;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int UpdateAdvertisement(AdvertisementDetails advertisement)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@adID", SqlDbType.NVarChar).Value = advertisement.AdID;
                cmd.Parameters.Add("@adName", SqlDbType.NVarChar).Value = advertisement.AdName;
                cmd.Parameters.Add("@advertiser", SqlDbType.NVarChar).Value = advertisement.Advertiser;
                cmd.Parameters.Add("@contactDetail", SqlDbType.NVarChar).Value = advertisement.ContactDetail;
                cmd.Parameters.Add("@ImageUrl", SqlDbType.NVarChar).Value = advertisement.ImageUrl;
                cmd.Parameters.Add("@navigateUrl", SqlDbType.NVarChar).Value = advertisement.NavigateUrl;
                cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = advertisement.StartDate;
                cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = advertisement.EndDate;
                cmd.Parameters.Add("@amountPaid", SqlDbType.Decimal).Value = advertisement.AmountPaid;
                cmd.Parameters.Add("@displayPosition", SqlDbType.NVarChar).Value = advertisement.DisplayPosition;
                cmd.Parameters.Add("@fitToPanel", SqlDbType.Int).Value = advertisement.FitToPanel;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = advertisement.Published;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = advertisement.ClientID;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = advertisement.Comments;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = advertisement.Email;
                cmd.Parameters.Add("@updatedBy", SqlDbType.NVarChar).Value = advertisement.UpdatedBy;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int DeleteAdvertisement(string adID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@adID", SqlDbType.NVarChar).Value = adID;
                cn.Open();

                return ExecuteNonQuery(cmd);
            }
        }

        public override int SetAdvertisementPublishStatus(string adID, bool published)
        {
           using(SqlConnection cn = new SqlConnection(this.ConnectionString))
           {
               SqlCommand cmd = new SqlCommand("spAdvertisementSetPublishStatus", cn);
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.Add("@adID", SqlDbType.NVarChar).Value = adID;
               cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
               cn.Open();
               return ExecuteNonQuery(cmd);
               
           }
        }

        public override AdvertisementDetails SelectAdvertisement(string adID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSelect", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@adID", SqlDbType.NVarChar).Value = adID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetAdvertisementFromReader(reader);
                else
                    return null;
            }
        }

        public override List<AdvertisementDetails> SelectAllAdvertisements(PagingDetails pgDetail)
        {
            List<AdvertisementDetails> advertisements = new List<AdvertisementDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSelectAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgDetail.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgDetail.PageSize;
                cn.Open();
                advertisements = GetAdvertisementCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementCountAll", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cn1.Open();
                pgDetail.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return advertisements;
        }

        public override List<AdvertisementDetails> SelectAdvertisementsByClientID(string clientID, PagingDetails pgDetail)
        {
            List<AdvertisementDetails> advertisements = new List<AdvertisementDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSelectByClientID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgDetail.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgDetail.PageSize;
                cn.Open();
                advertisements = GetAdvertisementCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementCountByClientID", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn1.Open();
                pgDetail.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return advertisements;
        }

        public override List<AdvertisementDetails> SelectAdvertisementByClientIDWithPublication(string clientID, PagingDetails pgDetail, bool published)
        {
            List<AdvertisementDetails> advertisements = new List<AdvertisementDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSelectByClientIDWithPublication", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgDetail.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgDetail.PageSize;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cn.Open();
                advertisements = GetAdvertisementCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementCountByClientIDWithPublicationBackEnd", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cn1.Open();
                pgDetail.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return advertisements;
        }

        public override List<AdvertisementDetails> SelectAdvertisementsWithClientIDNull(PagingDetails pgDetail)
        {
            List<AdvertisementDetails> advertisements = new List<AdvertisementDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSelectAllWithClientIDNull", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgDetail.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgDetail.PageSize;
                cn.Open();
                advertisements = GetAdvertisementCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementCountAllWithClientIDNull", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cn1.Open();
                pgDetail.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return advertisements;
        }

        public override List<AdvertisementDetails> SelectAdvertisementWithClientIDNullByPublication(bool published, PagingDetails pgDetail)
        {
            List<AdvertisementDetails> advertisements = new List<AdvertisementDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSelectAllWithClientIDNullByPublication", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgDetail.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgDetail.PageSize;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cn.Open();
                advertisements = GetAdvertisementCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementCountAllWithClientIDNullByPublication", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cn1.Open();
                pgDetail.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return advertisements;
        }

        public override List<AdvertisementDetails> SelectAdvertisementByAdvertiser(string clientID, string advertiser, PagingDetails pgDetail)
        {
            List<AdvertisementDetails> advertisements = new List<AdvertisementDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSearchByAdvertiser", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@advertiser", SqlDbType.NVarChar).Value = advertiser;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgDetail.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgDetail.PageSize;
                cn.Open();
                advertisements = GetAdvertisementCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementCountByAdvertis", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@advertiser", SqlDbType.NVarChar).Value = advertiser;
                cn1.Open();
                pgDetail.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return advertisements;
        }

        public override List<AdvertisementDetails> SelectAdvertisementByAdvertiserWithClientNull(string advertiser, PagingDetails pgDetail)
        {
            List<AdvertisementDetails> advertisements = new List<AdvertisementDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSearchByAdvertiserWithClientNull", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@advertiser", SqlDbType.NVarChar).Value = advertiser;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgDetail.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgDetail.PageSize;
                cn.Open();
                advertisements = GetAdvertisementCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementCountByAdvertisWithClientNull", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@advertiser", SqlDbType.NVarChar).Value = advertiser;
                cn1.Open();
                pgDetail.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return advertisements;
        }

        public override List<AdvertisementDetails> SelectAdvertisementByAdvertiserWithPublication(string clientID, string advertiser, bool published, PagingDetails pgDetail)
        {
            List<AdvertisementDetails> advertisements = new List<AdvertisementDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSearchByAdvertiserWithPublication", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@advertiser", SqlDbType.NVarChar).Value = advertiser;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgDetail.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgDetail.PageSize;
                cn.Open();
                advertisements = GetAdvertisementCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementCountByAdvertisWithPublication", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@advertiser", SqlDbType.NVarChar).Value = advertiser;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cn1.Open();
                pgDetail.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return advertisements;
        }

        public override List<AdvertisementDetails> SelectAdvertisementByAdvertiserWithPublicationWithClientNull(string advertiser, bool published, PagingDetails pgDetail)
        {
            List<AdvertisementDetails> advertisements = new List<AdvertisementDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSearchByAdvertiserWithPublicationWithClientNull", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@advertiser", SqlDbType.NVarChar).Value = advertiser;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgDetail.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgDetail.PageSize;
                cn.Open();
                advertisements = GetAdvertisementCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementCountByAdvertisWithPublicationWithClientNull", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@advertiser", SqlDbType.NVarChar).Value = advertiser;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cn1.Open();
                pgDetail.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return advertisements;
        }

        public override List<AdvertisementDetails> SelectAdvertisementsToRemindForRenewal()
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementRemindForRenewal", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                return GetAdvertisementCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<AdvertisementDetails> SelectExpiringAdvertisements(PagingDetails pgDetail)
        {
            List<AdvertisementDetails> advertisements = new List<AdvertisementDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSelectExpiringAd", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgDetail.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgDetail.PageSize;
                cn.Open();
                advertisements = GetAdvertisementCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementCountExpiringAd", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cn1.Open();
                pgDetail.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return advertisements;
        }

        public override List<AdvertisementDetails> SelectExpiringAdvertisementsByAdvertiser(string advertiser, PagingDetails pgDetail)
        {
            List<AdvertisementDetails> advertisements = new List<AdvertisementDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSelectExpiringAdByAdvertiser", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgDetail.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgDetail.PageSize;
                cmd.Parameters.Add("@advertiser", SqlDbType.NVarChar).Value = advertiser;
                cn.Open();
                advertisements = GetAdvertisementCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementCountExpiringAdByAdvertiser", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@advertiser", SqlDbType.NVarChar).Value = advertiser;
                cn1.Open();
                pgDetail.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return advertisements;
        }

        public override List<AdvertisementDetails> SelectExpiringCurrentAdvertisements(PagingDetails pgDetail)
        {
            List<AdvertisementDetails> advertisements = new List<AdvertisementDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSelectExpiringCurrentAd", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgDetail.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgDetail.PageSize;
                cn.Open();
                advertisements = GetAdvertisementCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementCountExpiringCurrentAd", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cn1.Open();
                pgDetail.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return advertisements;
        }

        public override List<AdvertisementDetails> SelectExpiringCurrentAdvertisementsByAdvertiser(string advertiser, PagingDetails pgDetail)
        {
            List<AdvertisementDetails> advertisements = new List<AdvertisementDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSelectExpiringCurrentAdByAdvertiser", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgDetail.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgDetail.PageSize;
                cmd.Parameters.Add("@advertiser", SqlDbType.NVarChar).Value = advertiser;
                cn.Open();
                advertisements = GetAdvertisementCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementCountExpiringCurrentAdByAdvertiser", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@advertiser", SqlDbType.NVarChar).Value = advertiser;
                cn1.Open();
                pgDetail.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return advertisements;
        }

        public override List<AdvertisementDetails> SelectExpiredAdvertisements(PagingDetails pgDetail)
        {
            List<AdvertisementDetails> advertisements = new List<AdvertisementDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSelectExpiredAd", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgDetail.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgDetail.PageSize;
                cn.Open();
                advertisements = GetAdvertisementCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementCountExpiredAd", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cn1.Open();
                pgDetail.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return advertisements;
        }

        public override List<AdvertisementDetails> SelectExpiredAdvertisementsByAdvertiser(string advertiser, PagingDetails pgDetail)
        {
            List<AdvertisementDetails> advertisements = new List<AdvertisementDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSelectExpiredAdByAdvertiser", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgDetail.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgDetail.PageSize;
                cmd.Parameters.Add("@advertiser", SqlDbType.NVarChar).Value = advertiser;
                cn.Open();
                advertisements = GetAdvertisementCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementCountExpiredAdByAdvertiser", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@advertiser", SqlDbType.NVarChar).Value = advertiser;
                cn1.Open();
                pgDetail.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return advertisements;
        }

        public override int UpdateAdExpiryNotice(string adID, int expiryNotice)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementUpdateExpiryNotification", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@adID", SqlDbType.NVarChar).Value = adID;
                cmd.Parameters.Add("@expiryNotice", SqlDbType.Int).Value = expiryNotice;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        #endregion

        #region METHODS FOR FRONTEND

        public override List<AdvertisementDetails> SelectRandomAddsByClientID(string clientID, int fitToPanel, string displayPosition)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSelectRandomByClientID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@fitToPanel", SqlDbType.Int).Value = fitToPanel;
                cmd.Parameters.Add("@displayPosition", SqlDbType.NVarChar).Value = displayPosition;
                cn.Open();
                return GetAdvertisementCollectionFromReaderWithLimitedFields(ExecuteReader(cmd));
            }
        }

        public override List<AdvertisementDetails> SelectRandomAddsWithoutClient(int fitToPanel, string displayPosition)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementSelectRandomWithClientIDNull", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@fitToPanel", SqlDbType.Int).Value = fitToPanel;
                cmd.Parameters.Add("@displayPosition", SqlDbType.NVarChar).Value = displayPosition;
                cn.Open();
                return GetAdvertisementCollectionFromReaderWithLimitedFields(ExecuteReader(cmd));
            }
        }

        public override int CountAddsByClientIDWithPublication(string clientID, bool published, string displayPosition)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementCountByClientIDWithPublication", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cmd.Parameters.Add("@displayPosition", SqlDbType.NVarChar).Value = displayPosition;
                cn.Open();

                return (int)ExecuteScalar(cmd);
            }
        }

        public override int CountAddsByCluentIDNullWithPublication(bool published, string displayPosition)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAdvertisementCountByClientIDNullWithPublication", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cmd.Parameters.Add("@displayPosition", SqlDbType.NVarChar).Value = displayPosition;
                cn.Open();
                return (int)ExecuteScalar(cmd);
            }
        }

        #endregion
    }
}
