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
    public abstract class AdvertisementProvider:DataAccess
    {
        static AdvertisementProvider _instance = null;
        public static AdvertisementProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (AdvertisementProvider)Activator.CreateInstance(Type.GetType(Globals.Settings.Advertisement.ProviderType));
                }

                return _instance;
            }
        }
        public AdvertisementProvider()
        {
            this.ConnectionString = Globals.Settings.Advertisement.ConnectionString;
        }

        #region VIRTUAL METHODS
        protected virtual AdvertisementDetails GetAdvertisementFromReader(IDataReader reader)
        {
            AdvertisementDetails advertisement = new AdvertisementDetails();
            advertisement.AdID = reader["AdID"].ToString();
            advertisement.AdName = reader["AdName"].ToString();
            advertisement.Advertiser = reader["Advertiser"].ToString();
            advertisement.ContactDetail = reader["ContactDetail"].ToString();
            advertisement.ImageUrl = reader["ImageUrl"].ToString();
            advertisement.NavigateUrl = reader["NavigateUrl"].ToString();
            advertisement.StartDate = (DateTime)reader["StartDate"];
            advertisement.EndDate = (DateTime)reader["EndDate"];
            advertisement.AmountPaid = (decimal)reader["AmountPaid"];
            advertisement.DisplayPosition = reader["DisplayPosition"].ToString();
            advertisement.FitToPanel = (int)reader["FitToPanel"];
            advertisement.Published = (bool)reader["Published"];
            if (reader["ClientID"] != DBNull.Value)
                advertisement.ClientID = reader["ClientID"].ToString();
            advertisement.Comments = reader["Comments"].ToString();
            advertisement.Email = reader["Email"].ToString();
            advertisement.ExpiryNotice = Convert.ToInt16(reader["ExpiryNotice"]);
            advertisement.CreatedDate = (DateTime)reader["CreatedDate"];
            if(reader["UpdatedDate"] != DBNull.Value)
                advertisement.UpdatedDate = (DateTime)reader["UpdatedDate"];
            advertisement.CreatedBy = reader["CreatedBy"].ToString();
            advertisement.UpdatedBy = reader["UpdatedBy"].ToString();

            return advertisement;
        }

        protected virtual AdvertisementDetails GetAdvertisementFromReaderSelectedFields(IDataReader reader)
        {
            AdvertisementDetails advertisement = new AdvertisementDetails();
            advertisement.AdID = reader["AdID"].ToString();
            advertisement.ImageUrl = reader["ImageUrl"].ToString();
            advertisement.NavigateUrl = reader["NavigateUrl"].ToString();
            advertisement.FitToPanel = Convert.ToInt32(reader["FitToPanel"]);
            return advertisement;
        }
        protected virtual List<AdvertisementDetails> GetAdvertisementCollectionFromReaderWithLimitedFields(IDataReader reader)
        {
            List<AdvertisementDetails> advertisements = new List<AdvertisementDetails>();
            while (reader.Read())
                advertisements.Add(GetAdvertisementFromReaderSelectedFields(reader));
            return advertisements;
        }

        protected virtual List<AdvertisementDetails> GetAdvertisementCollectionFromReader(IDataReader reader)
        {
            List<AdvertisementDetails> advertisements = new List<AdvertisementDetails>();
            while (reader.Read())
                advertisements.Add(GetAdvertisementFromReader(reader));
            return advertisements;
        }
        #endregion

        #region METHODS FOR BACKEND

        public abstract int InsertAdvertisement(AdvertisementDetails advertisement);
        public abstract int UpdateAdvertisement(AdvertisementDetails advertisement);
        public abstract int DeleteAdvertisement(string adID);
        public abstract int SetAdvertisementPublishStatus(string adID, bool published);

        public abstract AdvertisementDetails SelectAdvertisement(string adID);
        public abstract List<AdvertisementDetails> SelectAllAdvertisements(PagingDetails pgDetail);
        public abstract List<AdvertisementDetails> SelectAdvertisementsByClientID(string clientID, PagingDetails pgDetail);
        public abstract List<AdvertisementDetails> SelectAdvertisementByClientIDWithPublication(string clientID, PagingDetails pgDetail, bool published);
        public abstract List<AdvertisementDetails> SelectAdvertisementsWithClientIDNull(PagingDetails pgDetail);
        public abstract List<AdvertisementDetails> SelectAdvertisementWithClientIDNullByPublication(bool published, PagingDetails pgDetail);
        public abstract List<AdvertisementDetails> SelectAdvertisementByAdvertiser(string clientID, string advertiser, PagingDetails pgDetail);
        public abstract List<AdvertisementDetails> SelectAdvertisementByAdvertiserWithClientNull(string advertiser, PagingDetails pgDetail);
        public abstract List<AdvertisementDetails> SelectAdvertisementByAdvertiserWithPublication(string clientID, string advertiser, bool published, PagingDetails pgDetail);
        public abstract List<AdvertisementDetails> SelectAdvertisementByAdvertiserWithPublicationWithClientNull(string advertiser, bool published, PagingDetails pgDetail);
        public abstract List<AdvertisementDetails> SelectAdvertisementsToRemindForRenewal();
        public abstract List<AdvertisementDetails> SelectExpiringAdvertisements(PagingDetails pgDetail);
        public abstract List<AdvertisementDetails> SelectExpiringAdvertisementsByAdvertiser(string advertiser, PagingDetails pgDetail);
        public abstract List<AdvertisementDetails> SelectExpiringCurrentAdvertisements(PagingDetails pgDetail);
        public abstract List<AdvertisementDetails> SelectExpiringCurrentAdvertisementsByAdvertiser(string advertiser, PagingDetails pgDetail);
        public abstract List<AdvertisementDetails> SelectExpiredAdvertisements(PagingDetails pgDetail);
        public abstract List<AdvertisementDetails> SelectExpiredAdvertisementsByAdvertiser(string advertiser, PagingDetails pgDetail);
        public abstract int UpdateAdExpiryNotice(string adID, int expiryNotice);
        
        #endregion

        #region METHODS FOR FRONTEND

        public abstract List<AdvertisementDetails> SelectRandomAddsByClientID(string clientID, int fitToPanel, string displayPosition);
        public abstract List<AdvertisementDetails> SelectRandomAddsWithoutClient(int fitToPanel, string displayPosition);
        public abstract int CountAddsByClientIDWithPublication(string clientID, bool published, string displayPosition);
        public abstract int CountAddsByCluentIDNullWithPublication(bool published, string displayPosition);
        
        #endregion
    }
}
