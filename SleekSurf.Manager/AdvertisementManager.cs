using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SleekSurf.FrameWork;
using SleekSurf.DataAccess;
using SleekSurf.Entity;

namespace SleekSurf.Manager
{
    public class AdvertisementManager : BaseAdvertisement
    {
        #region METHODS FOR BACKEND

        public static Result<AdvertisementDetails> InsertAdvertisement(AdvertisementDetails advertisement)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                int i = SiteProvider.Advertisements.InsertAdvertisement(advertisement);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(advertisement);
                    result.Message = "Congratulations! The advertisement details have been successfully saved.";
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

        public static Result<AdvertisementDetails> UpdateAdvertisement(AdvertisementDetails advertisement)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                int i = SiteProvider.Advertisements.UpdateAdvertisement(advertisement);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(advertisement);
                    result.Message = "Congratulations! The advertisement details have been successfully updated.";
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

        public static Result<AdvertisementDetails> DeleteAdvertisement(string adID)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                int i = SiteProvider.Advertisements.DeleteAdvertisement(adID);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "The advertisement details have been successfully deleted.";
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

        public static int SetAdvertisementPublishStatus(string adID, bool published)
        {
            int i = 0;
            try
            {
                i = SiteProvider.Advertisements.SetAdvertisementPublishStatus(adID, published);
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
            }

            return i;
        }

        public static Result<AdvertisementDetails> SelectAdvertisement(string adID)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                result.EntityList.Add(SiteProvider.Advertisements.SelectAdvertisement(adID));
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

        public static Result<AdvertisementDetails> SelectAllAdvertisements(PagingDetails pgDetail)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                result.EntityList = SiteProvider.Advertisements.SelectAllAdvertisements(pgDetail);
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

        public static Result<AdvertisementDetails> SelectAdvertisementsByClientID(string clientID, PagingDetails pgDetail)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                result.EntityList = SiteProvider.Advertisements.SelectAdvertisementsByClientID(clientID, pgDetail);
                result.Message = "The following Advertisements are retrieved.";
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

        public static Result<AdvertisementDetails> SelectAdvertisementByClientIDWithPublication(string clientID, PagingDetails pgDetail, bool published)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                result.EntityList = SiteProvider.Advertisements.SelectAdvertisementByClientIDWithPublication(clientID, pgDetail, published);
                result.Message = "The following Advertisements are retrieved.";
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

        public static Result<AdvertisementDetails> SelectAdvertisementsWithClientIDNull(PagingDetails pgDetail)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                result.EntityList = SiteProvider.Advertisements.SelectAdvertisementsWithClientIDNull(pgDetail);
                result.Message = "The following Advertisements are retrieved.";
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

        public static Result<AdvertisementDetails> SelectAdvertisementWithClientIDNullByPublication(bool published, PagingDetails pgDetail)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                result.EntityList = SiteProvider.Advertisements.SelectAdvertisementWithClientIDNullByPublication(published, pgDetail);
                result.Message = "The following Advertisements are retrieved.";
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

        public static Result<AdvertisementDetails> SelectAdvertisementByAdvertiser(string clientID, string advertiser, PagingDetails pgDetail)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                result.EntityList = SiteProvider.Advertisements.SelectAdvertisementByAdvertiser(clientID, advertiser, pgDetail);
                result.Message = "The following Advertisements are retrieved.";
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

        public static Result<AdvertisementDetails> SelectAdvertisementByAdvertiserWithClientNull(string advertiser, PagingDetails pgDetail)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                result.EntityList = SiteProvider.Advertisements.SelectAdvertisementByAdvertiserWithClientNull(advertiser, pgDetail);
                result.Message = "The following Advertisements are retrieved.";
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

        public static Result<AdvertisementDetails> SelectAdvertisementByAdvertiserWithPublication(string clientID, string advertiser, bool published, PagingDetails pgDetail)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                result.EntityList = SiteProvider.Advertisements.SelectAdvertisementByAdvertiserWithPublication(clientID, advertiser, published, pgDetail);
                result.Message = "The following Advertisements are retrieved.";
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

        public static Result<AdvertisementDetails> SelectAdvertisementByAdvertiserWithPublicationWithClientNull(string advertiser, bool published, PagingDetails pgDetail)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                result.EntityList = SiteProvider.Advertisements.SelectAdvertisementByAdvertiserWithPublicationWithClientNull(advertiser, published, pgDetail);
                result.Message = "The following Advertisements are retrieved.";
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

        public static Result<AdvertisementDetails> SelectAdvertisementsToRemindForRenewal()
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                result.EntityList = SiteProvider.Advertisements.SelectAdvertisementsToRemindForRenewal();
                result.Message = "The following Advertisements are retrieved.";
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

        public static Result<AdvertisementDetails> SelectExpiringAdvertisements(PagingDetails pgDetail)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                result.EntityList = SiteProvider.Advertisements.SelectExpiringAdvertisements(pgDetail);
                result.Message = "The following Advertisements are retrieved.";
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

        public static Result<AdvertisementDetails> SelectExpiringAdvertisementsByAdvertiser(string advertiser, PagingDetails pgDetail)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                result.EntityList = SiteProvider.Advertisements.SelectExpiringAdvertisementsByAdvertiser(advertiser, pgDetail);
                result.Message = "The following Advertisements are retrieved.";
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

        public static Result<AdvertisementDetails> SelectExpiringCurrentAdvertisements(PagingDetails pgDetail)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                result.EntityList = SiteProvider.Advertisements.SelectExpiringCurrentAdvertisements(pgDetail);
                result.Message = "The following Advertisements are retrieved.";
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

        public static Result<AdvertisementDetails> SelectExpiringCurrentAdvertisementsByAdvertiser(string advertiser, PagingDetails pgDetail)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                result.EntityList = SiteProvider.Advertisements.SelectExpiringCurrentAdvertisementsByAdvertiser(advertiser, pgDetail);
                result.Message = "The following Advertisements are retrieved.";
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

        public static Result<AdvertisementDetails> SelectExpiredAdvertisements(PagingDetails pgDetail)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                result.EntityList = SiteProvider.Advertisements.SelectExpiredAdvertisements(pgDetail);
                result.Message = "The following Advertisements are retrieved.";
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

        public static Result<AdvertisementDetails> SelectExpiredAdvertisementsByAdvertiser(string advertiser, PagingDetails pgDetail)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                result.EntityList = SiteProvider.Advertisements.SelectExpiredAdvertisementsByAdvertiser(advertiser, pgDetail);
                result.Message = "The following Advertisements are retrieved.";
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

        public static bool UpdateAccountExpiryNotice(string adID, int expiryNotice)
        {
            bool result = false;
            try
            {
                int i = SiteProvider.Advertisements.UpdateAdExpiryNotice(adID, expiryNotice);
                if (i > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result = false;
            }
            return result;
        }

        #endregion

        #region METHODS FOR FRONTEND

        public static Result<AdvertisementDetails> SelectRandomAddsByClientID(string clientID, string displayPosition)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                int fitToPanel = 0;
                int i = SiteProvider.Advertisements.CountAddsByClientIDWithPublication(clientID, true, displayPosition);
                if (i > 0)
                {

                    do
                    {
                        Random random = new Random();
                        fitToPanel = random.Next(1, 5);

                        result.EntityList = SiteProvider.Advertisements.SelectRandomAddsByClientID(clientID, fitToPanel, displayPosition);
                        result.Status = ResultStatus.Success;
                        result.Message = "The following advertisements are found.";
                    } while (result.EntityList.Count == 0);
                }
                else
                {
                    result.EntityList = null;
                    result.Status = ResultStatus.NotFound;
                    result.Message = "No advertisement found.";
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

        public static Result<AdvertisementDetails> SelectRandomAddsWithoutClient(string displayPosition)
        {
            Result<AdvertisementDetails> result = new Result<AdvertisementDetails>();
            try
            {
                int fitToPanel = 0;
                int i = SiteProvider.Advertisements.CountAddsByCluentIDNullWithPublication(true, displayPosition);
                if (i > 0)
                {

                    do
                    {
                        Random random = new Random();
                        fitToPanel = random.Next(1, 5);

                        result.EntityList = SiteProvider.Advertisements.SelectRandomAddsWithoutClient(fitToPanel, displayPosition);
                        result.Status = ResultStatus.Success;
                        result.Message = "The following advertisements are found.";
                    } while (result.EntityList.Count == 0);
                }
                else
                {
                    result.EntityList = null;
                    result.Status = ResultStatus.NotFound;
                    result.Message = "No advertisement found.";
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

        #endregion
    }
}
