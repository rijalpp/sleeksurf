using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SleekSurf.Entity;
using System.Data.SqlClient;
using System.Data;

namespace SleekSurf.DataAccess.SqlClient
{
    public class SqlEventProvider : EventProvider
    {
        #region ALL METHODS OF EVENTS
        public override int InsertEvent(EventDetails eventDetail)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spEventInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@eventID", SqlDbType.NVarChar).Value = eventDetail.EventID;
                cmd.Parameters.Add("@eventName", SqlDbType.NVarChar).Value = eventDetail.EventName;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = eventDetail.Description;
                cmd.Parameters.Add("@eventImage", SqlDbType.VarBinary).Value = eventDetail.EventImage;
                cmd.Parameters.Add("@entryFee", SqlDbType.NVarChar).Value = eventDetail.EntryFee;
                cmd.Parameters.Add("@contactPerson", SqlDbType.NVarChar).Value = eventDetail.ContactPerson;
                cmd.Parameters.Add("@contactPhone", SqlDbType.NVarChar).Value = eventDetail.ContactPhone;
                cmd.Parameters.Add("@contactEmail", SqlDbType.NVarChar).Value = eventDetail.ContactEmail;
                cmd.Parameters.Add("@contactWebSite", SqlDbType.NVarChar).Value = eventDetail.ContactWebSite;
                cmd.Parameters.Add("@addressLine1", SqlDbType.NVarChar).Value = eventDetail.AddressLine1;
                cmd.Parameters.Add("@addressLine2", SqlDbType.NVarChar).Value = eventDetail.AddressLine2;
                cmd.Parameters.Add("@addressLine3", SqlDbType.NVarChar).Value = eventDetail.AddressLine3;
                cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = eventDetail.City;
                cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = eventDetail.State;
                cmd.Parameters.Add("@postCode", SqlDbType.NVarChar).Value = eventDetail.PostCode;
                cmd.Parameters.Add("@countryID", SqlDbType.Int).Value = eventDetail.Country.CountryID;
                cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = eventDetail.StartDate;
                cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = eventDetail.EndDate;
                cmd.Parameters.Add("@createdBy", SqlDbType.NVarChar).Value = eventDetail.CreatedBy;
                cmd.Parameters.Add("@createdInIpAddress", SqlDbType.NVarChar).Value = eventDetail.CreatedInIpAddress;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = eventDetail.IsActive;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }
        public override int UpdateEvent(EventDetails eventDetail)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spEventUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@eventID", SqlDbType.NVarChar).Value = eventDetail.EventID;
                cmd.Parameters.Add("@eventName", SqlDbType.NVarChar).Value = eventDetail.EventName;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = eventDetail.Description;
                cmd.Parameters.Add("@eventImage", SqlDbType.VarBinary).Value = eventDetail.EventImage;
                cmd.Parameters.Add("@entryFee", SqlDbType.NVarChar).Value = eventDetail.EntryFee;
                cmd.Parameters.Add("@contactPerson", SqlDbType.NVarChar).Value = eventDetail.ContactPerson;
                cmd.Parameters.Add("@contactPhone", SqlDbType.NVarChar).Value = eventDetail.ContactPhone;
                cmd.Parameters.Add("@contactEmail", SqlDbType.NVarChar).Value = eventDetail.ContactEmail;
                cmd.Parameters.Add("@contactWebSite", SqlDbType.NVarChar).Value = eventDetail.ContactWebSite;
                cmd.Parameters.Add("@addressLine1", SqlDbType.NVarChar).Value = eventDetail.AddressLine1;
                cmd.Parameters.Add("@addressLine2", SqlDbType.NVarChar).Value = eventDetail.AddressLine2;
                cmd.Parameters.Add("@addressLine3", SqlDbType.NVarChar).Value = eventDetail.AddressLine3;
                cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = eventDetail.City;
                cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = eventDetail.State;
                cmd.Parameters.Add("@postCode", SqlDbType.NVarChar).Value = eventDetail.PostCode;
                cmd.Parameters.Add("@countryID", SqlDbType.Int).Value = eventDetail.Country.CountryID;
                cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = eventDetail.StartDate;
                cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = eventDetail.EndDate;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = eventDetail.IsActive;
                cmd.Parameters.Add("@updatedBy", SqlDbType.NVarChar).Value = eventDetail.UpdatedBy;
                cmd.Parameters.Add("@updatedInIpAddress", SqlDbType.NVarChar).Value = eventDetail.UpdatedInIpAddress;
                cn.Open();

                return ExecuteNonQuery(cmd);
            }
        }
        public override int UpdateEventActiveStatus(string eventID, bool isActive)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spEvetSetActiveStatus", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@eventID", SqlDbType.NVarChar).Value = eventID;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = isActive;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }
        public override int DeleteEvent(string eventID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spEventDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@eventID", SqlDbType.NVarChar).Value = eventID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }
        public override EventDetails SelectEvent(string eventID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spEventSelect", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@eventID", SqlDbType.NVarChar).Value = eventID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetEventFromReader(reader);
                else
                    return null;
            }
        }
        public override List<EventDetails> SelectAllEvents(PagingDetails pgObj)
        {
            List<EventDetails> eventList = new List<EventDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spEventSelectAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cn.Open();
                eventList = GetEventCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spEventCountAll", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return eventList;
        }
        public override List<EventDetails> selectEventsByDateAndPublication(DateTime? startDate, DateTime? endDate, bool isActive, PagingDetails pgObj)
        {
            List<EventDetails> eventList = new List<EventDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spEventSelectByDateAndPublication", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = isActive;
                cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = startDate;
                cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = endDate;
                cn.Open();
                eventList = GetEventCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spEventCountByDateAndPublication", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = isActive;
                cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = startDate;
                cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = endDate;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return eventList;
        }
        #endregion

        #region ALL METHODS OF MEDIA GALLERIES
        public override int InsertMediaGallery(MediaGalleryDetails MediaGalleryDetails)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spMediaGalleryInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mediaID", SqlDbType.NVarChar).Value = MediaGalleryDetails.MediaID;
                cmd.Parameters.Add("@title", SqlDbType.NVarChar).Value = MediaGalleryDetails.Title;
                cmd.Parameters.Add("@caption", SqlDbType.NVarChar).Value = MediaGalleryDetails.Caption;
                cmd.Parameters.Add("@path", SqlDbType.NVarChar).Value = MediaGalleryDetails.MediaUrl;
                cmd.Parameters.Add("@type", SqlDbType.NVarChar).Value = MediaGalleryDetails.MediaType;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = MediaGalleryDetails.Description;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = MediaGalleryDetails.IsActive;
                cmd.Parameters.Add("@promotionID", SqlDbType.NVarChar).Value = MediaGalleryDetails.Promotion.PromotionID;
                cmd.Parameters.Add("@eventID", SqlDbType.NVarChar).Value = MediaGalleryDetails.EventDetail.EventID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int UpdateMediaGallery(MediaGalleryDetails MediaGalleryDetails)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spMediaGalleryUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mediaID", SqlDbType.NVarChar).Value = MediaGalleryDetails.MediaID;
                cmd.Parameters.Add("@title", SqlDbType.NVarChar).Value = MediaGalleryDetails.Title;
                cmd.Parameters.Add("@caption", SqlDbType.NVarChar).Value = MediaGalleryDetails.Caption;
                cmd.Parameters.Add("@path", SqlDbType.NVarChar).Value = MediaGalleryDetails.MediaUrl;
                cmd.Parameters.Add("@type", SqlDbType.NVarChar).Value = MediaGalleryDetails.MediaType;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = MediaGalleryDetails.Description;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = MediaGalleryDetails.IsActive;
                cmd.Parameters.Add("promotionID", SqlDbType.NVarChar).Value = MediaGalleryDetails.Promotion.PromotionID;
                cmd.Parameters.Add("eventID", SqlDbType.NVarChar).Value = MediaGalleryDetails.EventDetail.EventID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int DeleteMediaGallery(string mediaID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spMediaGalleryDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mediaID", SqlDbType.NVarChar).Value = mediaID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int SetMediaGalleryActiveStatus(string mediaID, bool isActive)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spMediaGallerySetActiveStatus", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mediaID", SqlDbType.NVarChar).Value = mediaID;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = isActive;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override MediaGalleryDetails SelectMediaGallery(string mediaID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spMediaGallerySelect", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mediaID", SqlDbType.NVarChar).Value = mediaID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetMediaGalleryFromReader(reader);
                else
                    return null;
            }
        }

        public override List<MediaGalleryDetails> SelectMediaGalleriesByEventID(string eventID, PagingDetails pgObj)
        {
            List<MediaGalleryDetails> galleryList = new List<MediaGalleryDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spMediaGallerySelectByEventID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@eventID", SqlDbType.NVarChar).Value = eventID;
                cn.Open();
                galleryList = GetMediaGalleryCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spMediaGalleryCountByEventID", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@eventID", SqlDbType.NVarChar).Value = eventID;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return galleryList;
        }

        public override List<MediaGalleryDetails> SelectMediaGalleriesByPromotionID(string promotionID, PagingDetails pgObj)
        {
            List<MediaGalleryDetails> galleryList = new List<MediaGalleryDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spMediaGallerySelectByPromotionID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@promotionID", SqlDbType.NVarChar).Value = promotionID;
                cn.Open();
                galleryList = GetMediaGalleryCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spMediaGalleryCountByPromotionID", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@promotionID", SqlDbType.NVarChar).Value = promotionID;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return galleryList;
        }

        public override List<MediaGalleryDetails> SelectMediaGalleriesByEventIDWithPublication(string eventID, bool isActive, PagingDetails pgObj)
        {
            List<MediaGalleryDetails> galleryList = new List<MediaGalleryDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spMediaGallerySelectByEventIDWithPublication", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@eventID", SqlDbType.NVarChar).Value = eventID;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = isActive;
                cn.Open();
                galleryList = GetMediaGalleryCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spMediaGalleryCountByEventIDWithPublication", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@eventID", SqlDbType.NVarChar).Value = eventID;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = isActive;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return galleryList;
        }

        public override List<MediaGalleryDetails> SelectMediaGalleriesByPromotionIDWithPublication(string promotionID, bool isActive, PagingDetails pgObj)
        {
            List<MediaGalleryDetails> galleryList = new List<MediaGalleryDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spMediaGallerySelectByPromotionIDWithPublication", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@promotionID", SqlDbType.NVarChar).Value = promotionID;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = isActive;
                cn.Open();
                galleryList = GetMediaGalleryCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spMediaGalleryCountByPromotionIDWithPublication", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@promotionID", SqlDbType.NVarChar).Value = promotionID;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = isActive;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return galleryList;
        }

        public override List<MediaGalleryDetails> SelectMediaGalleriesByPromotionIDWithPublication(string promotionID, bool isActive)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spMediaGallerySelectByPromotionIDAndPublicationWithAllType", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@promotionID", SqlDbType.NVarChar).Value = promotionID;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = isActive;
                cn.Open();
                return GetMediaGalleryCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<MediaGalleryDetails> SelectMediaGalleriesByPromotionIDWithPublication(string promotionID, bool isActive, string type)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spMediaGallerySelectByPromotionIDPublicationAndMediaType", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@promotionID", SqlDbType.NVarChar).Value = promotionID;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = isActive;
                cmd.Parameters.Add("@type", SqlDbType.NVarChar).Value = type;
                cn.Open();
                return GetMediaGalleryCollectionFromReader(ExecuteReader(cmd));
            }
        }

        #endregion
    }
}
