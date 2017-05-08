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
    public abstract class ClientProvider : DataAccess
    {
        static private ClientProvider _instance = null;

        static public ClientProvider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = (ClientProvider)Activator.CreateInstance(
                       Type.GetType(Globals.Settings.Clients.ProviderType));
                return _instance;
            }
        }

        public ClientProvider()
        {
            this.ConnectionString = Globals.Settings.Clients.ConnectionString;
            this.EnableCaching = Globals.Settings.Clients.EnableCaching;
            this.CacheDuration = Globals.Settings.Clients.CacheDuration;
        }

        public abstract List<string> GetMatchingKeyword(string prefix, int nRecordSet);

        #region  methods that work with categories
        public abstract List<CategoryDetails> GetCategories();
        public abstract List<CategoryDetails> GetCategories(string prefix);
        public abstract int InsertCategory(CategoryDetails category);
        public abstract int UpdateCategory(CategoryDetails category);
        public abstract int DeleteCategory(string categoryID);
        public abstract CategoryDetails GetCategory(string categoryID);
        protected virtual CategoryDetails GetCategoryFromReader(IDataReader reader)
        {
            CategoryDetails category = new CategoryDetails();
            category.CategoryID = reader["CategoryID"].ToString();
            category.CategoryName = reader["CategoryName"].ToString();
            category.Description = reader["Description"].ToString();
            if (reader["CategoryImage"] != DBNull.Value)
                category.CategoryImage = (byte[])reader["CategoryImage"];
            category.Comments = reader["Comments"].ToString();
            return category;
        }
        protected virtual List<CategoryDetails> GetCategoryCollectionFromReader(IDataReader reader)
        {
            List<CategoryDetails> categories = new List<CategoryDetails>();
            while (reader.Read())
                categories.Add(GetCategoryFromReader(reader));
            return categories;
        }

        #endregion

        #region //METHODS THAT WORK WITH CLIENTS

        public abstract int InsertClient(ClientDetails client);

        public abstract int UpdateClient(ClientDetails client);

        public abstract int PublishClient(string clientID, string comment);

        public abstract int UnPublishClient(string clientID, string comment);

        public abstract int SetClientDomain(string clientID, string uniqueDomain);

        public abstract int SetClientTheme(string clientID, string theme);

        public abstract ClientDetails SelectClient(string clientID);

        public abstract int UpdateClientPageHit(string clientID);

        public abstract int UpdateClientSearchHit(string clientID);

        public abstract int DeleteClient(string clientID);

        public abstract ClientDetails SelectClientByABN(string abn);

        public abstract List<ClientDetails> SelectClientByBusinessNameCategoryAddress(string businessName, string category, string address, PagingDetails pEx);

        public abstract List<ClientDetails> SelectClientByBusinessNameCategoryGeoLocation(string businessName, string category, decimal latitude, decimal longitude, decimal radius, PagingDetails pEx);

        public abstract List<ClientDetails> SelectAllClient(PagingDetails pgObj);

        public abstract List<ClientDetails> SelectAllClient(string prefix, PagingDetails pgObj);

        public abstract List<ClientDetails> SelectAllClient(Guid createdby, string prefix, PagingDetails pgObj); 

        public abstract List<ClientDetails> SelectClientsByStatus(string clientStatus, PagingDetails pgObj);

        public abstract List<ClientDetails> SelectClientsByStatus(string clientStatus, Guid createdBy, PagingDetails pgObj); 

        public abstract List<ClientDetails> SelectPublishedClient();

        public abstract List<ClientDetails> SelectUnPublishedClient();

        public abstract List<ClientDetails> SelectClientsWithPublication(Guid createdBy, bool published);

        public abstract List<ClientDetails> SelectClientsWithPublication(Guid createdBy, bool published, PagingDetails pgObj);

        public abstract List<ClientDetails> SelectClientByCategoryIDClientName(string categoryID, string clientName);

        public abstract ClientDetails SelectClientByUniqueIdentity(string uniqueIdentity);

        public abstract ClientDetails SelectClientByUniqueDomain(string uniqueDomain);

        public abstract List<ClientDetails> SelectClientsByCreatedPerson(Guid createdBy);

        public abstract List<ClientDetails> SelectClientsByCreatedPerson(Guid createdBy, PagingDetails pgObj);

        public abstract List<ClientDetails> SelectClientsByStatusAfterGivenDaysOfCreation(int days, string comment, PagingDetails pgObj);

        public abstract List<ClientDetails> SelectClientsByStatusAfterGivenDaysOfCreation(int days, string comment);
        
        public abstract string SelectProfileClientID(Guid userID);

        public abstract List<Guid> SelectUsersWithOutClientID();

        public abstract string SelectClientIDByUserID(Guid userID);

        public abstract List<Guid> SelectUsers(string clientID);

        public abstract int CheckClientUniqueIdentity(string uniqueIdentity);

        public abstract int CheckClientUniqueDomain(string uniqueDomain);

        public abstract int UpdateProfileForClientID(Guid userID, string clientID);

        protected virtual Guid GetUserIDFromReader(IDataReader reader)
        {
            return Guid.Parse(reader["UserId"].ToString());
        }

        protected virtual ClientDetails GetClientFromReader(IDataReader reader)
        {
            ClientDetails client = new ClientDetails();
            client.ClientID = reader["ClientID"].ToString();
            client.ABN = reader["ABN"].ToString();
            client.UniqueIdentity = reader["UniqueIdentity"].ToString();
            client.UniqueDomain = reader["UniqueDomain"].ToString();
            client.ClientName = reader["ClientName"].ToString();
            client.ContactPerson = (Guid)reader["ContactPerson"];
            client.Description = reader["Description"].ToString();
            client.Comment = reader["Comment"].ToString();
            client.ContactOffice = reader["ContactOffice"].ToString();
            client.ContactFax = reader["ContactFax"].ToString();
            client.BusinessEmail = reader["BusinessEmail"].ToString();
            client.BusinessUrl = reader["BusinessUrl"].ToString();
            client.LogoUrl = reader["LogoUrl"].ToString();
            if (reader["EstablishedDate"] != DBNull.Value)
                client.EstablishedDate = (DateTime?)reader["EstablishedDate"];
            client.AddressLine1 = reader["AddressLine1"].ToString();
            client.AddressLine2 = reader["AddressLine2"].ToString();
            client.AddressLine3 = reader["AddressLine3"].ToString();
            client.City = reader["City"].ToString();
            client.State = reader["State"].ToString();
            client.PostCode = reader["PostCode"].ToString();
            client.CountryID = new CountryDetails() { CountryID = (int)reader["CountryID"] };
            if (reader["SearchHit"] != DBNull.Value)
                client.SearchHit = (int?)reader["SearchHit"];
            if (reader["PageHit"] != DBNull.Value)
                client.PageHit = (int?)reader["PageHit"];
            if (reader["FreeSearchHit"] != DBNull.Value)
                client.FreeSearchHit = (int?)reader["FreeSearchHit"];
            if (reader["SMSCredit"] != DBNull.Value)
                client.SMSCredit = (int?)reader["SMSCredit"];
            client.Category = new CategoryDetails() { CategoryID = reader["CategoryID"].ToString() };
            client.CreatedDate = (DateTime)reader["CreatedDate"];
            client.CreatedBy = (Guid)reader["CreatedBy"];
            client.UpdatedDate = (DateTime)reader["UpdatedDate"];
            client.UpdatedBy = (Guid)reader["UpdatedBy"];
            client.Published = (bool)reader["Published"];
            if (reader["Latitude"] != DBNull.Value)
                client.Latitude = Convert.ToDecimal(reader["Latitude"]);
            if (reader["Longitude"] != DBNull.Value)
                client.Longitude = Convert.ToDecimal(reader["Longitude"]);
            client.Theme = reader["Theme"].ToString();
            return client;
        }

        protected virtual List<Guid> GetUserIDCollectionFromReader(IDataReader reader)
        {
            List<Guid> userIDList = new List<Guid>();
            while (reader.Read())
                userIDList.Add(GetUserIDFromReader(reader));
            return userIDList;
        }

        protected virtual List<ClientDetails> GetClientCollectionFromReader(IDataReader reader)
        {
            List<ClientDetails> clients = new List<ClientDetails>();
            while (reader.Read())
                clients.Add(GetClientFromReader(reader));
            return clients;
        }

        #endregion

        #region //methods for client promotions

        public abstract int InsertPromotion(PromotionDetails promotion);
        public abstract int UpdatePromotion(PromotionDetails promotion);
        public abstract int SetPromotionActiveStatus(string promotionID, string clientID, bool isActive);
        public abstract PromotionDetails SelectPromotion(string promotionID, string clientID);
        public abstract List<PromotionDetails> SelectAllCurrentPromotion(string clientID, PagingDetails pObj);
        public abstract List<PromotionDetails> SelectCurrentPromotionByPublication(string clientID, bool isActive, PagingDetails pObj);
        public abstract List<PromotionDetails> SelectAllPromotionByPublication(string clientID, bool isActive, PagingDetails pObj);
        public abstract List<PromotionDetails> SelectAllPromotion(string clientID, PagingDetails pObj);
        public abstract List<PromotionDetails> SelectPublishedCurrentPromotion(string clientID, PagingDetails pObj);
        public abstract List<PromotionDetails> SelectCurrentAndUpcomingPromotions(string clientID, int selectNRecords);
        //NEW FUNCTIONS ADDED
        //CURRENT AND FUTURE
        public abstract List<PromotionDetails> SelectAllCurrentUpcomingPromotions(string clientID, PagingDetails pgObj);
        public abstract List<PromotionDetails> SelectPublishedCurrentUpcomingPromotions(string clientID, PagingDetails pgObj);
        public abstract List<PromotionDetails> SelectUnPublishedCurrentUpcomingPromotions(string clientID, PagingDetails pgObj);
        //PAST PROMOTIONS
        public abstract List<PromotionDetails> SelectAllPastPromotions(string clientID, PagingDetails pgObj);
        public abstract List<PromotionDetails> SelectPublishedPastPromotions(string clientID, PagingDetails pgObj);
        public abstract List<PromotionDetails> SelectUnPublishedPastPromotions(string clientID, PagingDetails pgObj);
        //FUTURE PROMOTIONS
        public abstract List<PromotionDetails> SelectAllUpcomingPromotions(string clientID, PagingDetails pgObj);
        public abstract List<PromotionDetails> SelectPublishedUpcomingPromotions(string clientID, PagingDetails pgObj);
        public abstract List<PromotionDetails> SelectUnPublishedUpcomingPromotions(string clientID, PagingDetails pgObj);
        public abstract int DeletePromotion(string promotionID, string clientID);
        protected virtual PromotionDetails GetPromotionFromReader(IDataReader reader)
        {
            PromotionDetails promotion = new PromotionDetails();
            promotion.PromotionID = reader["PromotionID"].ToString();
            promotion.Title = reader["Title"].ToString();
            promotion.Description = reader["Description"].ToString();
            promotion.StartDate = (DateTime)reader["StartDate"];
            promotion.EndDate = (DateTime)reader["EndDate"];
            if (reader["TitleImage"] != DBNull.Value)
                promotion.TitleImage = (byte[])reader["TitleImage"];
            if (reader["SupportingImage"] != DBNull.Value)
                promotion.SupportingImage = (byte[])reader["SupportingImage"];
            promotion.IsActive = (bool)reader["IsActive"];
            promotion.ClientID = new ClientDetails() { ClientID = reader["ClientID"].ToString() };
            promotion.CreatedDate = (DateTime)reader["CreatedDate"];
            return promotion;
        }
        protected virtual List<PromotionDetails> GetPromotionCollectionFromReader(IDataReader reader)
        {
            List<PromotionDetails> promotionList = new List<PromotionDetails>();
            while (reader.Read())
                promotionList.Add(GetPromotionFromReader(reader));
            return promotionList;
        }

        #endregion

        #region Methods of services
        public abstract ServiceDetails SelectService(string serviceID, string clientID);
        public abstract List<ServiceDetails> SelectServicesByClientID(string clientID, PagingDetails pObj);
        public abstract int InsertServiceForClient(ServiceDetails serviceDetails);
        public abstract int UpdateServiceForClient(ServiceDetails serviceDetails);
        public abstract int DeleteServiceForClient(ServiceDetails serviceDetails);
        public abstract List<ServiceDetails> SelectServicesByClientID(string clientID);
        protected virtual ServiceDetails GetServiceFromReader(IDataReader reader)
        {
            ServiceDetails serviceDetail = new ServiceDetails();
            serviceDetail.ServiceID = reader["ServiceID"].ToString();
            serviceDetail.ServiceDescription = reader["ServiceDescription"].ToString();
            serviceDetail.Client = new ClientDetails() { ClientID = reader["ClientID"].ToString() };
            return serviceDetail;
        }
        protected virtual List<ServiceDetails> GetServiceCollectionFromReader(IDataReader reader)
        {
            List<ServiceDetails> serviceList = new List<ServiceDetails>();
            while (reader.Read())
                serviceList.Add(GetServiceFromReader(reader));
            return serviceList;
        }
        #endregion

        #region  METHODS THAT WORK WITH PAGEHIT

        #region HIT ANALYTICS
        public abstract DataTable HitCountrySelect(DateTime dateFrom, string clientID, string hitType);
        public abstract DataTable HitCitySelect(DateTime dateFrom, string countryName, string clientID, string hitType);
        public abstract DataTable HitCountrySelectAll(string clientID, string hitType);
        public abstract DataTable HitCitySelectAll(string countryName, string clientID, string hitType);
       
        #endregion

        public abstract int InsertPageHit(PageHitDetails pageHit);

        public abstract List<PageHitDetails> SelectPageHit(string clientID, string hitType, PagingDetails pgObj);

        public abstract int CountPageHit(string clientID, string pageHitType);

        public abstract List<PageHitDetails> SelectPageHitWithDateRange(string clientID, string hitType, DateTime fromDate, DateTime toDate, PagingDetails pgObj);

        public abstract int CountPageHitWithDateRange(string clientID, string hitType, DateTime fromDate, DateTime toDate);

        public virtual PageHitDetails GetPageHitFromReader(IDataReader reader, bool all, bool countryMode, bool cityMode, bool zipPostMode)
        {
            PageHitDetails pageHit = new PageHitDetails();
            if (all)
            {
                pageHit.PageHitID = reader["PageHitID"].ToString();
                pageHit.Client = new ClientDetails() { ClientID = reader["ClientID"].ToString() };
                pageHit.DateTimeStamp = (DateTime)reader["DatetimeStamp"];
                pageHit.HitType = reader["HitType"].ToString();
                pageHit.Comments = reader["Comments"].ToString();
                //LOCATION OF PAGEHIT
                pageHit.Location = new LocationDetails()
                {
                    CountryCode = reader["CountryCode"].ToString(),
                    CountryName = reader["CountryName"].ToString(),
                    CityName = reader["CityName"].ToString(),
                    ZipPostalCode = reader["ZipPostal"].ToString(),
                    Timezone = new TimezoneDetails()
                    {
                        Dstoffset = (bool)reader["Dstoffset"],
                        Gmtoffset = (float)reader["Gmtoffset"],
                        TimezoneName = reader["TimezoneName"].ToString()
                    },

                    Position = new LatLongDetails()
                    {
                        Latitude = (float)reader["Latitude"],
                        Longitude = (float)reader["Longitude"]
                    }
                };
            }
            else
            {
                if (countryMode)
                {
                    pageHit.Location = new LocationDetails() { CountryName = reader["CountryName"].ToString() };
                }
                if (cityMode)
                {
                    pageHit.Location = new LocationDetails() { CityName = reader["CityName"].ToString() };
                }
            }


            return pageHit;
        }

        public virtual List<PageHitDetails> GetPageHitCollectionFromReader(IDataReader reader)
        {
            List<PageHitDetails> pageHitList = new List<PageHitDetails>();
            while (reader.Read())
                pageHitList.Add(GetPageHitFromReader(reader, true, false, false, false));
            return pageHitList;
        }

        public virtual List<PageHitDetails> GetPageHitCollectionFromReader(IDataReader reader, bool all, bool countryMode, bool cityMode, bool zipPostMode)
        {
            List<PageHitDetails> pageHitList = new List<PageHitDetails>();
            while (reader.Read())
                pageHitList.Add(GetPageHitFromReader(reader, all, countryMode, cityMode, zipPostMode));
            return pageHitList;
        }

        #endregion

        #region METHODS THAT WORK WITH FAQ_GROUP

        public abstract int InsertFaqGroup(FAQGroupDetails faqGroup);
        public abstract int UpdateFaqGroup(FAQGroupDetails faqGroup);
        public abstract int DeleteFaqGroup(string faqGroupID);
        public abstract FAQGroupDetails SelectFaqGroupDetails(string faqGroupID);
        public abstract List<FAQGroupDetails> SelectFaqGroupDetailsByClient(string clientID);
        public abstract List<FAQGroupDetails> SelectFaqGroupWithClientNull();

        protected virtual FAQGroupDetails GetFaqGroupFromReader(IDataReader reader)
        {
            FAQGroupDetails faqGroup = new FAQGroupDetails();
            faqGroup.FaqGroupID = reader["FaqGroupID"].ToString();
            faqGroup.Client = new ClientDetails() { ClientID = reader["ClientID"].ToString() };
            faqGroup.GroupName = reader["GroupName"].ToString();
            faqGroup.GroupRank = (int)reader["GroupRank"];
            faqGroup.Comments = reader["Comments"].ToString();

            return faqGroup;
        }
        protected virtual List<FAQGroupDetails> GetFaqGroupCollectionFromReader(IDataReader reader)
        {
            List<FAQGroupDetails> faqGroupList = new List<FAQGroupDetails>();
            while (reader.Read())
                faqGroupList.Add(GetFaqGroupFromReader(reader));
            return faqGroupList;
        }

        #endregion

        #region METHODS THAT WORK WITH FAQ

        public abstract int InsertFaq(FAQDetails faq);
        public abstract int UpdateFaq(FAQDetails faq);
        public abstract int DeleteFaq(string faqID);
        public abstract FAQDetails SelectFaqDetails(string faqID);
        public abstract List<FAQDetails> SelectFaqDetailsByFaqGroup(string faqGroupID);

        protected virtual FAQDetails GetFaqFromReader(IDataReader reader)
        {
            FAQDetails faqDetail = new FAQDetails();
            faqDetail.FaqID = reader["FaqID"].ToString();
            faqDetail.FaqGroup = new FAQGroupDetails() { FaqGroupID = reader["FaqGroupID"].ToString() };
            faqDetail.Question = reader["Question"].ToString();
            faqDetail.Answer = reader["Answer"].ToString();
            faqDetail.FaqRank = (int)reader["FaqRank"];
            faqDetail.Comments = reader["Comments"].ToString();

            return faqDetail;
        }

        protected virtual List<FAQDetails> GetFaqCollectionFromReader(IDataReader reader)
        {
            List<FAQDetails> faqList = new List<FAQDetails>();
            while (reader.Read())
                faqList.Add(GetFaqFromReader(reader));
            return faqList;
        }

        #endregion

        #region METHODS THAT WORK WITH DATAEXTENDER
        public abstract int InsertDataExtender(DataExtenderDetails dataExtenderDetails);
        public abstract int UpdatePrivacyAndPolicy(DataExtenderDetails dataExtenderDetails);
        public abstract int UpdateTermsAndConditions(DataExtenderDetails dataExtenderDetails);
        public abstract int UpdateDataExtender(DataExtenderDetails dataExtenderDetails);
        public abstract int DeleteDataExtender(int dataExtenderID);

        public abstract List<DataExtenderDetails> SelectDataExtenderByClient(string clientID);
        public abstract List<DataExtenderDetails> SelectDataExtenderWithClientNull();
        public abstract DataExtenderDetails SelectDataExtender(int dataExtenderID);

        protected virtual DataExtenderDetails GetDataExtenderDetailsFromReader(IDataReader reader)
        {
            DataExtenderDetails dataExtender = new DataExtenderDetails();

            dataExtender.DataExtenderID = (int)reader["DataExtenderID"];
            dataExtender.Client = new ClientDetails() { ClientID = reader["ClientID"].ToString() };
            dataExtender.PrivacyAndPolicy = reader["PrivacyAndPolicy"].ToString();
            dataExtender.TermsAndConditions = reader["TermsAndConditions"].ToString();
            dataExtender.Comments = reader["Comments"].ToString();

            return dataExtender;
        }

        protected virtual List<DataExtenderDetails> GetDataExtenderCollectionFromReader(IDataReader reader)
        {
            List<DataExtenderDetails> dataExtenderList = new List<DataExtenderDetails>();
            while (reader.Read())
                dataExtenderList.Add(GetDataExtenderDetailsFromReader(reader));
            return dataExtenderList;
        }

        #endregion

        #region METHODS THAT WORK WITH SMS CREDIT

        public abstract int UpdateSMSCredit(string clientID, int newCredit);

        #endregion

        #region METHODS THAT WORK WITH CLIENT FEATURES

        public abstract bool ClientFeatureSetListingStatus(string clientID, bool listing);
        public abstract bool ClientFeatureSetClientProfileStatus(string clientID, bool clientProfile);
        public abstract bool ClientFeatureSetClientDomainStatus(string clientID, bool clientDomain);
        public abstract ClientFeatureDetails SelectClientFeatureDetails(string clientID);

        public virtual ClientFeatureDetails GetClientFeatureFromDataReader(IDataReader reader)
        {
            ClientFeatureDetails clientFeature = new ClientFeatureDetails();
            clientFeature.Client = new ClientDetails() { ClientID = reader["ClientID"].ToString() };
            clientFeature.Listing = (bool)reader["Listing"];
            clientFeature.ClientProfile = (bool)reader["ClientProfile"];
            clientFeature.ClientDomain = (bool)reader["ClientDomain"];

            return clientFeature;
        }

        public virtual List<ClientFeatureDetails> GetClientFeatureCollectionFromReader(IDataReader reader)
        {
            List<ClientFeatureDetails> clientFeatureList = new List<ClientFeatureDetails>();
            while (reader.Read())
                clientFeatureList.Add(GetClientFeatureFromDataReader(reader));
            return clientFeatureList;
        }

        #endregion
    }
}
