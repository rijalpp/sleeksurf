using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SleekSurf.FrameWork;
using SleekSurf.Entity;
using System.Data;

namespace SleekSurf.DataAccess
{
    public abstract class EventProvider : DataAccess
    {
        static private EventProvider _instance;
        static public EventProvider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = (EventProvider)Activator.CreateInstance(Type.GetType(Globals.Settings.Events.ProviderType));
                return _instance;
            }
        }

        public EventProvider()
        {
            this.ConnectionString = Globals.Settings.Clients.ConnectionString;
            this.EnableCaching = Globals.Settings.Clients.EnableCaching;
            this.CacheDuration = Globals.Settings.Clients.CacheDuration;
        }

        #region ALL METHODS OF EVENTS
        public abstract int InsertEvent(EventDetails eventDetail);
        public abstract int UpdateEvent(EventDetails eventDetail);
        public abstract int UpdateEventActiveStatus(string eventID, bool isActive);
        public abstract int DeleteEvent(string eventID);
        public abstract EventDetails SelectEvent(string eventID);
        public abstract List<EventDetails> SelectAllEvents(PagingDetails pgObj);
        public abstract List<EventDetails> selectEventsByDateAndPublication(DateTime? startDate, DateTime? endDate, bool isActive, PagingDetails pgObj);
        protected virtual EventDetails GetEventFromReader(IDataReader reader)
        {
            EventDetails eventDetail = new EventDetails();
            eventDetail.EventID = reader["EventID"].ToString();
            eventDetail.EventName = reader["EventName"].ToString();
            eventDetail.Description = reader["Description"].ToString();
            if (reader["EventImage"] != DBNull.Value)
                eventDetail.EventImage = (byte[])reader["EventImage"];
            eventDetail.EntryFee = reader["EntryFee"].ToString();
            eventDetail.ContactPerson = reader["ContactPerson"].ToString();
            eventDetail.ContactPhone = reader["ContactPhone"].ToString();
            eventDetail.ContactEmail = reader["ContactEmail"].ToString();
            eventDetail.ContactWebSite = reader["ContactWebSite"].ToString();
            eventDetail.AddressLine1 = reader["AddressLine1"].ToString();
            eventDetail.AddressLine2 = reader["AddressLine2"].ToString();
            eventDetail.AddressLine3 = reader["AddressLine3"].ToString();
            eventDetail.City = reader["City"].ToString();
            eventDetail.State = reader["State"].ToString();
            eventDetail.PostCode = reader["PostCode"].ToString();
            eventDetail.Country = new CountryDetails() { CountryID = (int)reader["CountryID"] };
            eventDetail.StartDate = (DateTime)reader["StartDate"];
            eventDetail.EndDate = (DateTime)reader["EndDate"];
            eventDetail.CreatedDate = (DateTime)reader["CreatedDate"];
            eventDetail.CreatedBy = reader["CreatedBy"].ToString();
            eventDetail.IsActive = (bool)reader["IsActive"];
            if (reader["UpdatedDate"] != DBNull.Value)
            {
                eventDetail.UpdatedDate = (DateTime)reader["UpdatedDate"];
                eventDetail.UpdatedBy = reader["UpdatedBy"].ToString();
            }

            return eventDetail;
        }
        protected virtual List<EventDetails> GetEventCollectionFromReader(IDataReader reader)
        {
            List<EventDetails> eventList = new List<EventDetails>();
            while (reader.Read())
                eventList.Add(GetEventFromReader(reader));
            return eventList;
        }

        #endregion

        #region ALL METHODS OF MEDIA GALLERIES
        public abstract int InsertMediaGallery(MediaGalleryDetails MediaGalleryDetails);
        public abstract int UpdateMediaGallery(MediaGalleryDetails MediaGalleryDetails);
        public abstract int DeleteMediaGallery(string mediaID);
        public abstract int SetMediaGalleryActiveStatus(string mediaID, bool isActive);
        public abstract MediaGalleryDetails SelectMediaGallery(string mediaID);
        public abstract List<MediaGalleryDetails> SelectMediaGalleriesByEventID(string eventID, PagingDetails pgObj);
        public abstract List<MediaGalleryDetails> SelectMediaGalleriesByPromotionID(string promotionID, PagingDetails pgObj);
        public abstract List<MediaGalleryDetails> SelectMediaGalleriesByEventIDWithPublication(string eventID, bool isActive, PagingDetails pgobj);
        public abstract List<MediaGalleryDetails> SelectMediaGalleriesByPromotionIDWithPublication(string promotionID, bool isActive, PagingDetails pgObj);
        public abstract List<MediaGalleryDetails> SelectMediaGalleriesByPromotionIDWithPublication(string promotionID, bool isActive);
        public abstract List<MediaGalleryDetails> SelectMediaGalleriesByPromotionIDWithPublication(string promotionID, bool isActive, string type);

        protected virtual MediaGalleryDetails GetMediaGalleryFromReader(IDataReader reader)
        {
            MediaGalleryDetails MediaGalleryDetails = new MediaGalleryDetails();
            MediaGalleryDetails.MediaID = reader["MediaID"].ToString();
            MediaGalleryDetails.Title = reader["Title"].ToString();
            MediaGalleryDetails.Caption = reader["Caption"].ToString();
            MediaGalleryDetails.MediaUrl = reader["Path"].ToString();
            MediaGalleryDetails.MediaType = reader["Type"].ToString();
            MediaGalleryDetails.Description = reader["Description"].ToString();
            MediaGalleryDetails.IsActive = (bool)reader["IsActive"];
            MediaGalleryDetails.Promotion = new PromotionDetails() { PromotionID = reader["PromotionID"].ToString() };
            MediaGalleryDetails.EventDetail = new EventDetails() { EventID = reader["EventID"].ToString() };
            MediaGalleryDetails.CreatedDate = (DateTime)reader["CreatedDate"];

            return MediaGalleryDetails;
        }
        protected virtual List<MediaGalleryDetails> GetMediaGalleryCollectionFromReader(IDataReader reader)
        {
            List<MediaGalleryDetails> galleryList = new List<MediaGalleryDetails>();
            while (reader.Read())
                galleryList.Add(GetMediaGalleryFromReader(reader));
            return galleryList;
        }
        #endregion

    }
}
