using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using SleekSurf.DataAccess;

namespace SleekSurf.Manager
{
    public class EventManager : BaseEvent
    {
        #region ALL METHODS OF EVENTS
        public static Result<EventDetails> InsertEvent(EventDetails eventDetail)
        {
            Result<EventDetails> result = new Result<EventDetails>();
            try
            {
                eventDetail.CreatedBy = BaseEvent.CurrentUser.Identity.Name;
                eventDetail.CreatedInIpAddress = BaseEvent.CurrentUserIP;
                int i = SiteProvider.Events.InsertEvent(eventDetail);
                if (i > 0)
                {
                    result.EntityList.Add(eventDetail);
                    result.Status = ResultStatus.Success;
                    result.Message = "Congratulations! The Event Details have been successfully registered.";
                    BizObject.PurgeCacheItems("events_event");
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
        public static Result<EventDetails> UpdateEvent(EventDetails eventDetail)
        {
            Result<EventDetails> result = new Result<EventDetails>();
            try
            {
                eventDetail.UpdatedBy = BaseEvent.CurrentUser.Identity.Name;
                eventDetail.UpdatedInIpAddress = BaseEvent.CurrentUserIP;
                int i = SiteProvider.Events.UpdateEvent(eventDetail);
                if (i > 0)
                {
                    result.EntityList.Add(eventDetail);
                    result.Status = ResultStatus.Success;
                    result.Message = "Congratulations! The Event Details have been successfully updated.";
                    BizObject.PurgeCacheItems("events_event");
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
        public static Result<EventDetails> UpdateEventPublishStatus(string eventID, bool isActive)
        {
            Result<EventDetails> result = new Result<EventDetails>();
            try
            {
                int i = SiteProvider.Events.UpdateEventActiveStatus(eventID, isActive);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "Congratulations! The Event Details have been successfully updated.";
                    BizObject.PurgeCacheItems("events_event");
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
        public static Result<EventDetails> DeleteEvent(string eventID)
        {
            Result<EventDetails> result = new Result<EventDetails>();
            try
            {
                int i = SiteProvider.Events.DeleteEvent(eventID);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "The Event Detail has been successfully deleted.";
                    BizObject.PurgeCacheItems("events_event");
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
        public static Result<EventDetails> SelectEvent(string eventID)
        {
            Result<EventDetails> result = new Result<EventDetails>();
            string key = "events_event_" + eventID;
            try
            {
                if (BaseEvent.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<EventDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList.Add(SiteProvider.Events.SelectEvent(eventID));
                    BaseEvent.CacheData(key, result.EntityList);
                }
                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);

                result.Status = ResultStatus.Success;
                result.Message = "The record is retrieved as follows";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }
        public static Result<EventDetails> SelectAllEvents(PagingDetails pgObj)
        {
            Result<EventDetails> result = new Result<EventDetails>();
            string key = "events_event_" + pgObj.StartRowIndex + pgObj.PageSize;
            string keyCount = "events_event_count" + pgObj.StartRowIndex + pgObj.PageSize;
            try
            {
                if (BaseEvent.Settings.EnableCaching && (BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null))
                {
                    result.EntityList = (List<EventDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Events.SelectAllEvents(pgObj);
                    BaseEvent.CacheData(key, result.EntityList);
                    BaseEvent.CacheData(keyCount, pgObj.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);

                result.Status = ResultStatus.Success;
                result.Message = "Records are successfully retrieved.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }
        public static Result<EventDetails> selectEventsByDateAndPublication(DateTime? startDate, DateTime? endDate, bool isActive, PagingDetails pgObj)
        {
            Result<EventDetails> result = new Result<EventDetails>();
            string key = "events_event_" + pgObj.StartRowIndex + pgObj.PageSize + startDate + endDate + isActive.ToString();
            string keyCount = "events_event_count_" + pgObj.StartRowIndex + pgObj.PageSize + startDate + endDate + isActive.ToString();
            try
            {
                if (BaseEvent.Settings.EnableCaching && (BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null))
                {
                    result.EntityList = (List<EventDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Events.selectEventsByDateAndPublication(startDate, endDate, isActive, pgObj);
                    BaseEvent.CacheData(key, result.EntityList);
                    BaseEvent.CacheData(keyCount, pgObj.TotalNumber);
                }
                result.Status = ResultStatus.Success;
                result.Message = "Records are successfully retrieved.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;

        }
        private static void SaveImageInSession(List<EventDetails> eventList)
        {
            Dictionary<string, ByteStruct> imageList = new Dictionary<string, ByteStruct>();
            foreach (EventDetails eventDetail in eventList)
            {
                ByteStruct imgStruct = new ByteStruct() { MainImage = eventDetail.EventImage, SupportImage = null };
                imageList.Add(eventDetail.EventID, imgStruct);
            }
            WebContext.ImageList = imageList;
        }
        #endregion

        #region ALL METHODS OF MEDIA GALLERIES

        public static Result<MediaGalleryDetails> InsertMediaGallery(MediaGalleryDetails MediaGalleryDetails)
        {
            Result<MediaGalleryDetails> result = new Result<MediaGalleryDetails>();
            try
            {
                int i = SiteProvider.Events.InsertMediaGallery(MediaGalleryDetails);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "The Media details have been successfully added.";
                    result.EntityList.Add(MediaGalleryDetails);
                    BaseEvent.PurgeCacheItems("galleries_gallery");
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

        public static Result<MediaGalleryDetails> UpdateMediaGallery(MediaGalleryDetails MediaGalleryDetails)
        {
            Result<MediaGalleryDetails> result = new Result<MediaGalleryDetails>();
            try
            {
                int i = SiteProvider.Events.UpdateMediaGallery(MediaGalleryDetails);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "The Media details have been updated";
                    result.EntityList.Add(MediaGalleryDetails);
                    BaseEvent.PurgeCacheItems("galleries_gallery");
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

        public static Result<MediaGalleryDetails> DeleteMediaGallery(string mediaID)
        {
            Result<MediaGalleryDetails> result = new Result<MediaGalleryDetails>();
            try
            {
                int i = SiteProvider.Events.DeleteMediaGallery(mediaID);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "The Media details have been successfully deleted.";
                    BaseEvent.PurgeCacheItems("galleries_gallery");
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

        public static Result<MediaGalleryDetails> SetMediaGalleryActiveStatus(string mediaID, bool isActive)
        {
            Result<MediaGalleryDetails> result = new Result<MediaGalleryDetails>();
            try
            {
                int i = SiteProvider.Events.SetMediaGalleryActiveStatus(mediaID, isActive);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "The Active status for the Media Gallery has been updated.";
                    BizObject.PurgeCacheItems("galleries_gallery_");
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

        public static Result<MediaGalleryDetails> SelectMediaGallery(string mediaID)
        {
            Result<MediaGalleryDetails> result = new Result<MediaGalleryDetails>();
            string key = "galleries_gallery_" + mediaID;
            try
            {
                if (BaseEvent.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<MediaGalleryDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList.Add(SiteProvider.Events.SelectMediaGallery(mediaID));
                    BaseEvent.CacheData(key, result.EntityList);
                }
                result.Status = ResultStatus.Success;
                result.Message = "Media details successfully retrieved.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<MediaGalleryDetails> SelectMediaGalleriesByEventID(string eventID, PagingDetails pgObj)
        {
            Result<MediaGalleryDetails> result = new Result<MediaGalleryDetails>();
            string key = "galleries_gallery_" + eventID + pgObj.StartRowIndex + pgObj.PageSize;
            string keyCount = "galleries_gallery_count_" + eventID + pgObj.StartRowIndex + pgObj.PageSize;
            try
            {
                if (BaseEvent.Settings.EnableCaching && (BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null))
                {
                    result.EntityList = (List<MediaGalleryDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Events.SelectMediaGalleriesByEventID(eventID, pgObj);
                    BaseEvent.CacheData(key, result.EntityList);
                    BaseEvent.CacheData(keyCount, pgObj.TotalNumber);
                }
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    result.Message = "Galleries successfully retrieved.";
                else
                    result.Message = "No Gallery record found in the Database.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<MediaGalleryDetails> SelectMediaGalleriesByEventIDWithPublication(string eventID, bool isActive, PagingDetails pgObj)
        {
            Result<MediaGalleryDetails> result = new Result<MediaGalleryDetails>();
            string key = "galleries_gallery_" + eventID + pgObj.StartRowIndex + pgObj.PageSize + isActive.ToString();
            string keyCount = "galleries_gallery_count_" + eventID + pgObj.StartRowIndex + pgObj.PageSize + isActive.ToString();
            try
            {
                if (BaseEvent.Settings.EnableCaching && (BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null))
                {
                    result.EntityList = (List<MediaGalleryDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Events.SelectMediaGalleriesByEventIDWithPublication(eventID, isActive, pgObj);
                    BaseEvent.CacheData(key, result.EntityList);
                    BaseEvent.CacheData(keyCount, pgObj.TotalNumber);
                }
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    result.Message = "Galleries successfully retrieved.";
                else
                    result.Message = "No Gallery record found in the Database.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<MediaGalleryDetails> SelectMediaGalleriesByPromotionID(string promotionID, PagingDetails pgObj)
        {
            Result<MediaGalleryDetails> result = new Result<MediaGalleryDetails>();
            string key = "galleries_gallery_" + promotionID + pgObj.StartRowIndex + pgObj.PageSize;
            string keyCount = "galleries_gallery_count_" + promotionID + pgObj.StartRowIndex + pgObj.PageSize;
            try
            {
                if (BaseEvent.Settings.EnableCaching && (BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null))
                {
                    result.EntityList = (List<MediaGalleryDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Events.SelectMediaGalleriesByPromotionID(promotionID, pgObj);
                    BaseEvent.CacheData(key, result.EntityList);
                    BaseEvent.CacheData(keyCount, pgObj.TotalNumber);
                }
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    result.Message = "Galleries successfully retrieved.";
                else
                    result.Message = "No Gallery record found in the Database.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<MediaGalleryDetails> SelectMediaGalleriesByPromotionIDWithPublication(string promotionID, bool isActive, PagingDetails pgObj)
        {
            Result<MediaGalleryDetails> result = new Result<MediaGalleryDetails>();
            string key = "galleries_gallery_" + promotionID + pgObj.StartRowIndex + pgObj.PageSize + isActive.ToString();
            string keyCount = "galleries_gallery_count_" + promotionID + pgObj.StartRowIndex + pgObj.PageSize + isActive.ToString();
            try
            {
                if (BaseEvent.Settings.EnableCaching && (BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null))
                {
                    result.EntityList = (List<MediaGalleryDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Events.SelectMediaGalleriesByPromotionIDWithPublication(promotionID, isActive, pgObj);
                    BaseEvent.CacheData(key, result.EntityList);
                    BaseEvent.CacheData(keyCount, pgObj.TotalNumber);
                }
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    result.Message = "Galleries successfully retrieved.";
                else
                    result.Message = "No Gallery record found in the Database.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<MediaGalleryDetails> SelectMediaGalleriesByPromotionIDWithPublication(string promotionID, bool isActive)
        {
            Result<MediaGalleryDetails> result = new Result<MediaGalleryDetails>();
            string key = "galleries_gallery_" + promotionID + isActive.ToString();
            try
            {
                if (BaseEvent.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<MediaGalleryDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Events.SelectMediaGalleriesByPromotionIDWithPublication(promotionID, isActive);
                    BaseEvent.CacheData(key, result.EntityList);
                }
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    result.Message = "Galleries successfully retrieved.";
                else
                    result.Message = "No Gallery record found in the Database.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<MediaGalleryDetails> SelectMediaGalleriesByPromotionIDWithPublication(string promotionID, bool isActive, string type)
        {
            Result<MediaGalleryDetails> result = new Result<MediaGalleryDetails>();
            string key = "galleries_gallery_" + promotionID + isActive.ToString() + type;
            try
            {
                if (BaseEvent.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<MediaGalleryDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Events.SelectMediaGalleriesByPromotionIDWithPublication(promotionID, isActive, type);
                    BaseEvent.CacheData(key, result.EntityList);
                }
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    result.Message = "Galleries successfully retrieved.";
                else
                    result.Message = "No Gallery record found in the Database.";
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
    }
}
