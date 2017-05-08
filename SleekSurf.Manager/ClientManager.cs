using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using SleekSurf.Entity;
using SleekSurf.DataAccess;
using SleekSurf.FrameWork;
using System.Net;
using System.Xml.Linq;
using System.Web.Security;

namespace SleekSurf.Manager
{
    public class ClientManager : BaseClient
    {
        public static List<string> GetMatchingKeyword(string prefix, int nRecordSet)
        {
            List<string> matchedKeywordList = new List<string>();
            try
            {
                matchedKeywordList = SiteProvider.Clients.GetMatchingKeyword(prefix, nRecordSet);
                return matchedKeywordList;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                throw;
            }
        }

        #region  ALL METHODS OF CLIENTS

        public static Result<ClientDetails> InsertClient(ClientDetails clientDetail)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            try
            {
                int i = SiteProvider.Clients.InsertClient(clientDetail);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(clientDetail);
                    result.Message = "Congratulations! The Client Details have been successfully registered.";
                    BizObject.PurgeCacheItems("clients_client");
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

        public static Result<ClientDetails> UpdateClient(ClientDetails clientDetails)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            try
            {
                int i = SiteProvider.Clients.UpdateClient(clientDetails);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(clientDetails);
                    result.Message = "The Client details have been updated.";
                    BizObject.PurgeCacheItems("clients_client");
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

        public static bool DeleteClient(string clientID)
        {
            bool result = false;
            try
            {
                string key = "clients_client";
                if (SiteProvider.Clients.DeleteClient(clientID) > 0)
                {
                    BizObject.PurgeCacheItems(key);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result = false;
            }
            return result;
        }

        public static bool PublishClient(string clientID, string comment)
        {
            bool result = false;
            try
            {
                string key = "clients_client";
                if (SiteProvider.Clients.PublishClient(clientID, comment) > 0)
                {
                    BizObject.PurgeCacheItems(key);
                    result = true;
                }
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
                result = false;
            }
            return result;
        }

        public static bool UnPublishClient(string clientID, string comment)
        {
            bool result = false;
            try
            {
                string key = "clients_client_";
                if (SiteProvider.Clients.UnPublishClient(clientID, comment) > 0)
                {
                    BizObject.PurgeCacheItems(key);
                    result = true;
                }
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
                result = false;
            }
            return result;
        }

        public static bool SetClientDomain(string clientID, string uniqueDomain)
        {
            bool result = false;
            try
            {
                string key = "clients_client_";
                if (SiteProvider.Clients.UnPublishClient(clientID, uniqueDomain) > 0)
                {
                    BizObject.PurgeCacheItems(key);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result = false;
            }
            return result;
        }

        public static bool CheckClientUniqueIdentity(string uniqueIdentity)
        {
            bool result = false;
            try
            {
                result = SiteProvider.Clients.CheckClientUniqueIdentity(uniqueIdentity) == 0 ? true : false;
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
                throw;
            }
            return result;
        }

        public static bool CheckClientUniqueDomain(string uniqueDomain)
        {
            bool result = false;
            try
            {
                result = SiteProvider.Clients.CheckClientUniqueDomain(uniqueDomain) == 0 ? true : false;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                throw;
            }
            return result;
        }

        public static Result<ClientDetails> SelectClient(string clientID)
        {
            Result<ClientDetails> result = null;
            string key = "clients_client_" + clientID;
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result = new Result<ClientDetails>();
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                    result.Message = "Records successfully retrieved from Cache.";
                    result.Status = ResultStatus.Success;
                }
                else
                {
                    ClientDetails clientdetail = SiteProvider.Clients.SelectClient(clientID);
                    if (clientdetail != null)
                    {
                        result = new Result<ClientDetails>();
                        result.EntityList.Add(clientdetail);
                        if (result.EntityList.Count > 0)
                        {
                            result.Message = "Records successfully retrieved from Database.";
                            result.Status = ResultStatus.Success;
                        }
                        BaseClient.CacheData(key, result.EntityList);
                    }
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

        public static Result<ClientDetails> SelectClientByABN(string abn)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string key = "clients_client_" + abn;
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                    result.Status = ResultStatus.Success;
                }
                else
                {
                    result.EntityList.Add(SiteProvider.Clients.SelectClientByABN(abn));
                    if (result.EntityList.Count > 0)
                    {
                        result.Status = ResultStatus.Success;
                        result.Message = "Records successfully retrieved from Database.";
                    }
                    else if (result.EntityList.Count == 0)
                    {
                        result.Status = ResultStatus.NotFound;
                        result.Message = "No entry in Database.";
                    }
                    BaseClient.CacheData(key, result.EntityList);
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

        public static Result<ClientDetails> SelectClientByUniqueDomain(string uniqueDomain)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string key = "clients_client_" + uniqueDomain;
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                    result.Status = ResultStatus.Success;
                }
                else
                {
                    ClientDetails client = SiteProvider.Clients.SelectClientByUniqueDomain(uniqueDomain);
                    if (client != null)
                        result.EntityList.Add(client);
                    if (result.EntityList.Count > 0)
                    {
                        result.Status = ResultStatus.Success;
                        result.Message = "Records successfully retrieved from Database.";
                    }
                    else if (result.EntityList.Count == 0)
                    {
                        result.Status = ResultStatus.NotFound;
                        result.Message = "No entry in Database.";
                    }
                    BaseClient.CacheData(key, result.EntityList);
                }

                //UPDATE THE PAGE HIT BY CALLING THIS.
                if (result.EntityList.Count > 0 && result.EntityList[0].Published)
                {
                    if (HttpContext.Current.Session[result.EntityList[0].ClientID] == null)
                    {
                        //SAVE THE CLIENT INFORMATION IN SESSION.
                        HttpContext.Current.Session.Add(result.EntityList[0].ClientID, result.EntityList[0].ClientID);

                        result.EntityList[0].PageHit = SiteProvider.Clients.UpdateClientPageHit(result.EntityList[0].ClientID);
                        //ADD PAGEHIT INFORMATION INTO THE DATABASE.
                        string sourceIP = string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"])
                                        ? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
                                        : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        LocationDetails location = new LocationDetails();
                        string sourceIPKey = sourceIP + "Location";
                        if (Globals.Settings.Clients.EnableCaching && BizObject.Cache[sourceIPKey] != null)
                            location = (LocationDetails)BizObject.Cache[sourceIPKey];
                        else
                        {
                            location = HostIpToLocation(sourceIP);
                            BaseClient.CacheData(sourceIPKey, location);
                        }
                        PageHitDetails pageHit = new PageHitDetails();
                        pageHit.PageHitID = result.EntityList[0].ClientID + DateTime.Now.ToString("P-ddMMyy-HHmmssfff");
                        pageHit.Client = new ClientDetails() { ClientID = result.EntityList[0].ClientID };
                        pageHit.DateTimeStamp = DateTime.Now;
                        pageHit.HitType = StatusPageHit.PageHit.ToString();
                        pageHit.IPAddress = sourceIP;
                        if (location != null)
                        {
                            pageHit.Comments = "";
                            pageHit.Location = location;
                        }
                        else
                        {
                            pageHit.Location = new LocationDetails()
                            {
                                CountryCode = "Unknown",
                                CountryName = "Unknown",
                                RegionName = "Unknown",
                                CityName = "Unknown",
                                ZipPostalCode = "Unknown",
                                Timezone = new TimezoneDetails
                                {
                                    TimezoneName = "Unknown",
                                    Gmtoffset = 0.00F,
                                    Dstoffset = false
                                },
                                Position = new LatLongDetails
                                {
                                    Latitude = 0.00f,
                                    Longitude = 0.00f
                                }
                            };

                            pageHit.Comments = "Unidentified";
                        }

                        ClientManager.InsertPageHit(pageHit);
                    }
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

        public static Result<ClientDetails> SelectClientsByCreatedPerson(Guid createdBy)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string key = "clients_client_"+ createdBy.ToString();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectClientsByCreatedPerson(createdBy);
                    BaseClient.CacheData(key, result.EntityList);
                }

                if (result.EntityList.Count > 0)
                {
                    result.Message = "Records successfully retrieved from Database.";
                }
                else if (result.EntityList.Count == 0)
                {
                    result.Message = "No entry in Database.";
                }
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

        public static Result<ClientDetails> SelectClientsByStatusAfterGivenDaysOfCreation(int days, string comment, PagingDetails pgObj)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string key = "clients_client_"+days+"_"+pgObj.StartRowIndex+"_"+pgObj.PageSize+"_"+comment;
            string keyCount = "clients_client_count_" + days + "_" + pgObj.StartRowIndex + "_" + pgObj.PageSize + "_" + comment;
            try
            {
                if (BaseClient.Settings.EnableCaching && (BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null))
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectClientsByStatusAfterGivenDaysOfCreation(days, comment, pgObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pgObj.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                {
                    result.Message = "Records successfully retrieved from Database.";
                }
                else if (result.EntityList.Count == 0)
                {
                    result.Message = "No entry in Database.";
                }
                result.Status = ResultStatus.Success;
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }

            return result;
        }

        public static Result<ClientDetails> SelectClientsByStatusAfterGivenDaysOfCreation(int days, string comment)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string key = "clients_client_" + days + "_" + comment;
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectClientsByStatusAfterGivenDaysOfCreation(days, comment);
                    BaseClient.CacheData(key, result.EntityList);
                }
                if (result.EntityList.Count > 0)
                {
                    result.Message = "Records successfully retrieved from Database.";
                }
                else if (result.EntityList.Count == 0)
                {
                    result.Message = "No entry in Database.";
                }
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

        public static Result<ClientDetails> SelectClientsByCreatedPerson(Guid createdBy, PagingDetails pgObj)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string key = "clients_client_" + pgObj.StartRowIndex +"_"+ pgObj.PageSize+"_"+createdBy.ToString();
            string keyCount = "clients_client_count_" + pgObj.StartRowIndex + "_" + pgObj.PageSize + "_" + createdBy.ToString();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectClientsByCreatedPerson(createdBy, pgObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pgObj.TotalNumber);
                }

                if (result.EntityList.Count > 0)
                {
                    result.Message = "Records successfully retrieved from Database.";
                }
                else if (result.EntityList.Count == 0)
                {
                    result.Message = "No entry in Database.";
                }
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

        public static Result<ClientDetails> SelectClientByBusinessNameCategoryAddress(string businessName, string category, string address, PagingDetails pEx)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();

            try
            {
                string key = "";
                string countTotal = "";
                if (category != null)
                {
                    key = "clients_client_" + businessName + category + address + pEx.StartRowIndex + pEx.PageSize;
                    countTotal = "clients_client_Count" + businessName + category + address + pEx.StartRowIndex + pEx.PageSize;
                }
                else
                {
                    key = "clients_client_" + businessName + category + address + pEx.StartRowIndex + pEx.PageSize;
                    countTotal = "clients_client_Count" + businessName + address + pEx.StartRowIndex + pEx.PageSize;
                }
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null && BizObject.Cache[countTotal] != null)
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                    pEx.TotalNumber = (int)BizObject.Cache[countTotal];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectClientByBusinessNameCategoryAddress(businessName, category, address, pEx);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(countTotal, pEx.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "Records successfully retrieved from Database.";


                    if (HttpContext.Current.Session[BizObject.CurrentUserIP + key] == null)
                    {
                        //SAVE THE CLIENT INFORMATION IN SESSION.
                        HttpContext.Current.Session.Add((BizObject.CurrentUserIP + key), (BizObject.CurrentUserIP + key));

                        //ADD PAGEHIT INFORMATION INTO THE DATABASE.
                        string sourceIP = string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"])
                                        ? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
                                        : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        LocationDetails location = new LocationDetails();
                        string sourceIPKey = sourceIP+"Location";
                        if (BizObject.Cache[sourceIPKey] != null)
                            location = (LocationDetails)BizObject.Cache[sourceIPKey];
                        else
                        {
                            location = HostIpToLocation(sourceIP);
                            BaseClient.CacheData(sourceIPKey, location);
                        }
                        int count = 0;
                        foreach (ClientDetails clientDetails in result.EntityList)
                        {
                            //UPDATE THE SEARCH HIT IN CLIENT TABLE
                            SiteProvider.Clients.UpdateClientSearchHit(clientDetails.ClientID);
                            //UPDATE THE INFORMATION IN PAGEHIT TABLE
                            PageHitDetails pageHit = new PageHitDetails();
                            pageHit.PageHitID = DateTime.Now.ToString("PH-ddMMyy-HHmmssfff") + count.ToString();
                            pageHit.Client = clientDetails;
                            pageHit.DateTimeStamp = DateTime.Now;
                            pageHit.HitType = StatusPageHit.SearchHit.ToString();
                            pageHit.IPAddress = sourceIP;
                            if (location != null)
                            {
                                pageHit.Comments = "";
                                pageHit.Location = location;
                            }
                            else
                            {
                                pageHit.Location = new LocationDetails()
                                {
                                    CountryCode = "Unknown",
                                    CountryName = "Unknown",
                                    RegionName = "Unknown",
                                    CityName = "Unknown",
                                    ZipPostalCode = "Unknown",
                                    Timezone = new TimezoneDetails
                                    {
                                        TimezoneName = "Unknown",
                                        Gmtoffset = 0.00F,
                                        Dstoffset = false
                                    },
                                    Position = new LatLongDetails
                                    {
                                        Latitude = 0.00f,
                                        Longitude = 0.00f
                                    }
                                };

                                pageHit.Comments = "Unidentified";
                            }
                            ClientManager.InsertPageHit(pageHit);
                            count++;
                        }
                    }

                }
                else if (result.EntityList.Count == 0)
                {
                    result.Status = ResultStatus.NotFound;
                    result.Message = "No entry in Database.";
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

        public static Result<ClientDetails> SelectClientByBusinessNameCategoryGeoLocation(string businessName, string category, decimal latitude, decimal longitude, decimal radius, PagingDetails pEx)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();

            try
            {
                string key = "";
                string countTotal = "";
                if (category != null)
                {
                    key = "clients_client_" + businessName + category + longitude + latitude + radius + pEx.StartRowIndex + pEx.PageSize;
                    countTotal = "clients_client_Count" + businessName + category + longitude + latitude + radius + pEx.StartRowIndex + pEx.PageSize;
                }
                else
                {
                    key = "clients_client_" + businessName + category + longitude + latitude + radius + pEx.StartRowIndex + pEx.PageSize;
                    countTotal = "clients_client_Count" + businessName + longitude + latitude + radius + pEx.StartRowIndex + pEx.PageSize;
                }
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null && BizObject.Cache[countTotal] != null)
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                    pEx.TotalNumber = (int)BizObject.Cache[countTotal];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectClientByBusinessNameCategoryGeoLocation(businessName, category, latitude, longitude, radius, pEx);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(countTotal, pEx.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "Records successfully retrieved from Database.";


                    if (HttpContext.Current.Session[BizObject.CurrentUserIP + key] == null)
                    {
                        //SAVE THE CLIENT INFORMATION IN SESSION.
                        HttpContext.Current.Session.Add((BizObject.CurrentUserIP + key), (BizObject.CurrentUserIP + key));

                        //ADD PAGEHIT INFORMATION INTO THE DATABASE.
                        string sourceIP = string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"])
                                        ? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
                                        : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        LocationDetails location = new LocationDetails();
                        string sourceIPKey = sourceIP + "Location";
                        if (BizObject.Cache[sourceIPKey] != null)
                            location = (LocationDetails)BizObject.Cache[sourceIPKey];
                        else
                        {
                            location = HostIpToLocation(sourceIP);
                            BaseClient.CacheData(sourceIPKey, location);
                        }
                        int count = 0;
                        foreach (ClientDetails clientDetails in result.EntityList)
                        {
                            //UPDATE THE SEARCH HIT IN CLIENT TABLE
                            SiteProvider.Clients.UpdateClientSearchHit(clientDetails.ClientID);
                            //UPDATE THE INFORMATION IN PAGEHIT TABLE
                            PageHitDetails pageHit = new PageHitDetails();
                            pageHit.PageHitID = DateTime.Now.ToString("PH-ddMMyy-HHmmssfff") + count.ToString();
                            pageHit.Client = clientDetails;
                            pageHit.DateTimeStamp = DateTime.Now;
                            pageHit.HitType = StatusPageHit.SearchHit.ToString();
                            pageHit.IPAddress = sourceIP;
                            if (location != null)
                            {
                                pageHit.Comments = "";
                                pageHit.Location = location;
                            }
                            else
                            {
                                pageHit.Location = new LocationDetails()
                                {
                                    CountryCode = "Unknown",
                                    CountryName = "Unknown",
                                    RegionName = "Unknown",
                                    CityName = "Unknown",
                                    ZipPostalCode = "Unknown",
                                    Timezone = new TimezoneDetails
                                    {
                                        TimezoneName = "Unknown",
                                        Gmtoffset = 0.00F,
                                        Dstoffset = false
                                    },
                                    Position = new LatLongDetails
                                    {
                                        Latitude = 0.00f,
                                        Longitude = 0.00f
                                    }
                                };

                                pageHit.Comments = "Unidentified";
                            }
                            ClientManager.InsertPageHit(pageHit);
                            count++;
                        }
                    }

                }
                else if (result.EntityList.Count == 0)
                {
                    result.Status = ResultStatus.NotFound;
                    result.Message = "No entry in Database.";
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

        public static Result<ClientDetails> SelectClientByUniqueIdentity(string uniqueIdentity)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string key = "clients_client_" + uniqueIdentity;
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                    result.Status = ResultStatus.Success;
                }
                else
                {
                    ClientDetails client = SiteProvider.Clients.SelectClientByUniqueIdentity(uniqueIdentity);
                    if (client != null)
                        result.EntityList.Add(client);
                    if (result.EntityList.Count > 0)
                    {
                        result.Status = ResultStatus.Success;
                        result.Message = "Records successfully retrieved from Database.";
                    }
                    else if (result.EntityList.Count == 0)
                    {
                        result.Status = ResultStatus.NotFound;
                        result.Message = "No entry in Database.";
                    }
                    BaseClient.CacheData(key, result.EntityList);
                }

                //UPDATE THE PAGE HIT BY CALLING THIS.
                if (result.EntityList.Count > 0 && result.EntityList[0].Published)
                {
                    if (HttpContext.Current.Session[result.EntityList[0].ClientID] == null)
                    {
                        //SAVE THE CLIENT INFORMATION IN SESSION.
                        HttpContext.Current.Session.Add(result.EntityList[0].ClientID, result.EntityList[0].ClientID);

                        result.EntityList[0].PageHit = SiteProvider.Clients.UpdateClientPageHit(result.EntityList[0].ClientID);
                        //ADD PAGEHIT INFORMATION INTO THE DATABASE.
                        string sourceIP = string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"])
                                        ? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
                                        : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        LocationDetails location = new LocationDetails();
                        string sourceIPKey = sourceIP + "Location";
                        if (Globals.Settings.Clients.EnableCaching && BizObject.Cache[sourceIPKey] != null)
                            location = (LocationDetails)BizObject.Cache[sourceIPKey];
                        else
                        {
                            location = HostIpToLocation(sourceIP);
                            BaseClient.CacheData(sourceIPKey, location);
                        }
                        PageHitDetails pageHit = new PageHitDetails();
                        pageHit.PageHitID =result.EntityList[0].ClientID+ DateTime.Now.ToString("P-ddMMyy-HHmmssfff");
                        pageHit.Client = new ClientDetails() { ClientID = result.EntityList[0].ClientID };
                        pageHit.DateTimeStamp = DateTime.Now;
                        pageHit.HitType = StatusPageHit.PageHit.ToString();
                        pageHit.IPAddress = sourceIP;
                        if (location != null)
                        {
                            pageHit.Comments = "";
                            pageHit.Location = location;
                        }
                        else
                        {
                            pageHit.Location = new LocationDetails()
                            {
                                CountryCode = "Unknown",
                                CountryName = "Unknown",
                                RegionName = "Unknown",
                                CityName = "Unknown",
                                ZipPostalCode = "Unknown",
                                Timezone = new TimezoneDetails
                                {
                                    TimezoneName = "Unknown",
                                    Gmtoffset = 0.00F,
                                    Dstoffset = false
                                },
                                Position = new LatLongDetails
                                {
                                    Latitude = 0.00f,
                                    Longitude = 0.00f
                                }
                            };

                            pageHit.Comments = "Unidentified";
                        }

                        ClientManager.InsertPageHit(pageHit);
                    }
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

        public static Result<ClientDetails> SelectAllClient(PagingDetails pgObj)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string key = "clients_client_" + pgObj.StartRowIndex + pgObj.PageSize;
            string keyCount = "clients_client_count_" + pgObj.StartRowIndex + pgObj.PageSize;
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectAllClient(pgObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pgObj.TotalNumber);
                }

                if (result.EntityList.Count > 0)
                {
                    result.Message = "Records successfully retrieved from Database.";
                }
                else if (result.EntityList.Count == 0)
                {
                    result.Message = "No entry in Database.";
                }
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

        public static Result<ClientDetails> SelectAllClient(string prefix, PagingDetails pgObj)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string key = "clients_client_" + pgObj.StartRowIndex + pgObj.PageSize + prefix;
            string keyCount = "clients_client_count_" + pgObj.StartRowIndex + pgObj.PageSize + prefix;
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectAllClient(prefix, pgObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pgObj.TotalNumber);
                }

                if (result.EntityList.Count > 0)
                {
                    result.Message = "Records successfully retrieved from Database.";
                }
                else if (result.EntityList.Count == 0)
                {
                    result.Message = "No entry in Database.";
                }
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

        public static Result<ClientDetails> SelectAllClient(Guid createdby, string prefix, PagingDetails pgObj)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string key = "clients_client_" + createdby.ToString() + "_" + pgObj.StartRowIndex + "_" + pgObj.PageSize + "_" + prefix;
            string keyCount = "clients_client_count_" + createdby.ToString() + "_" + pgObj.StartRowIndex + "_" + pgObj.PageSize + "_" + prefix;
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectAllClient(createdby, prefix, pgObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pgObj.TotalNumber);
                }

                if (result.EntityList.Count > 0)
                {
                    result.Message = "Records successfully retrieved from Database.";
                }
                else if (result.EntityList.Count == 0)
                {
                    result.Message = "No entry in Database.";
                }
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

        public static Result<ClientDetails> SelectClientsByStatus(string clientStatus, PagingDetails pgObj)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string key = "clients_client_" + pgObj.StartRowIndex + "_" + pgObj.PageSize + "_" + clientStatus;
            string keyCount = "clients_client_count_" + pgObj.StartRowIndex + "_" + pgObj.PageSize + "_" + clientStatus;
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectClientsByStatus(clientStatus, pgObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pgObj.TotalNumber);
                }

                if (result.EntityList.Count > 0)
                {
                    result.Message = "Records successfully retrieved from Database.";
                }
                else if (result.EntityList.Count == 0)
                {
                    result.Message = "No entry in Database.";
                }
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

        public static Result<ClientDetails> SelectClientsByStatus(string clientStatus, Guid createdBy, PagingDetails pgObj)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string key = "clients_client_" + createdBy.ToString() + "_ " + pgObj.StartRowIndex + "_" + pgObj.PageSize + "_" + clientStatus;
            string keyCount = "clients_client_count_" + createdBy.ToString() + "_ " + pgObj.StartRowIndex + "_" + pgObj.PageSize + "_" + clientStatus;
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectClientsByStatus(clientStatus, createdBy, pgObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pgObj.TotalNumber);
                }

                if (result.EntityList.Count > 0)
                {
                    result.Message = "Records successfully retrieved from Database.";
                }
                else if (result.EntityList.Count == 0)
                {
                    result.Message = "No entry in Database.";
                }
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

        public static Result<ClientDetails> SelectPublishedClient()
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string key = "clients_client_Published";
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectPublishedClient();
                    BaseClient.CacheData(key, result.EntityList);
                }
                if (result.EntityList.Count > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "Records successfully retrieved from Database.";
                }
                else if (result.EntityList.Count == 0)
                {
                    result.Status = ResultStatus.NotFound;
                    result.Message = "No entry in Database.";
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

        public static Result<ClientDetails> SelectUnPublishedClient()
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string key = "clients_client_UnPublished";
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectUnPublishedClient();
                    BaseClient.CacheData(key, result.EntityList);
                }
                if (result.EntityList.Count > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "Records successfully retrieved from Database.";
                }
                else if (result.EntityList.Count == 0)
                {
                    result.Status = ResultStatus.NotFound;
                    result.Message = "No entry in Database.";
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

        public static Result<ClientDetails> SelectClientsWithPublication(Guid createdBy, bool published)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string key = "clients_client_"+createdBy.ToString()+"_"+published.ToString();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectClientsWithPublication(createdBy, published);
                    BaseClient.CacheData(key, result.EntityList);
                }
                if (result.EntityList.Count > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "Records successfully retrieved from Database.";
                }
                else if (result.EntityList.Count == 0)
                {
                    result.Status = ResultStatus.NotFound;
                    result.Message = "No entry in Database.";
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

        public static Result<ClientDetails> SelectClientsWithPublication(Guid createdBy, bool published, PagingDetails pgObj)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string key = "clients_client_" + createdBy.ToString() + "_" + published.ToString() + "_" + pgObj.StartRowIndex.ToString() + "_" + pgObj.PageSize;
            string keyCount = "clients_client_count_" + createdBy.ToString() + "_" + published.ToString() + "_" + pgObj.StartRowIndex.ToString() + "_" + pgObj.PageSize; ;
            try
            {
                if (BaseClient.Settings.EnableCaching && (BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null))
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectClientsWithPublication(createdBy, published, pgObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pgObj.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "Records successfully retrieved from Database.";
                }
                else if (result.EntityList.Count == 0)
                {
                    result.Status = ResultStatus.NotFound;
                    result.Message = "No entry in Database.";
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

        public static Result<ClientDetails> SelectClientByCategoryIDClientName(string categoryID, string clientName)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            string key = "clients_client_" + categoryID + clientName;
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ClientDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectClientByCategoryIDClientName(categoryID, clientName);
                    BaseClient.CacheData(key, result.EntityList);
                }

                if (result.EntityList.Count > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "Records successfully retrieved from Database.";
                }
                else if (result.EntityList.Count == 0)
                {
                    result.Status = ResultStatus.NotFound;
                    result.Message = "No entry in Database.";
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

        public static string SelectProfileClientID(Guid userID)
        {
            string clientID = null;
            try
            {
                clientID = SiteProvider.Clients.SelectProfileClientID(userID);
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
                throw;
            }
            return clientID;
        }

        public static Result<ClientDetails> UpdateProfileForClientID(Guid userID, string clientID)
        {
            Result<ClientDetails> result = new Result<ClientDetails>();
            result.EntityList.Add(SiteProvider.Clients.SelectClient(clientID));
            try
            {
                if (SiteProvider.Clients.UpdateProfileForClientID(userID, clientID) > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "Client reference updated in Profile.";
                }
                else
                {
                    result.Status = ResultStatus.NotFound;
                    result.Message = "UserID not found.";
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "Error!! " + ex.Message;
                result.EntityList.Clear();
            }
            return result;
        }

        public static List<Guid> SelectUsers(string clientID)
        {
            try
            {
                return SiteProvider.Clients.SelectUsers(clientID);
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
                return null;
            }
        }

        public static List<Guid> SelectUsersWithOutClient()
        {
            try
            {
                return SiteProvider.Clients.SelectUsersWithOutClientID();
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex, "No Action Is Required!");
                return null;
            }
        }

        public static string SelectClientIDByUserID(Guid userID)
        {
            try
            {
                return SiteProvider.Clients.SelectClientIDByUserID(userID);
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
                return null;
            }
        }

        public static LocationDetails HostIpToLocation(string ip)
        {
            string url = "http://api.ipinfodb.com/v2/ip_query.php?key={0}&ip={1}&timezone=true";

            url = String.Format(url, Configuration.GetConfigurationSetting("GeoLocationAPIKey", typeof(string)), ip);

            HttpWebRequest httpWRequest = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse httpWResponse = (HttpWebResponse)httpWRequest.GetResponse())
            {
                var result = XDocument.Load(httpWResponse.GetResponseStream());
                LocationDetails location = null;
                try
                {
                    location = (from x in result.Descendants("Response")
                                    select new LocationDetails
                                    {
                                        CountryCode = (string)x.Element("CountryCode"),
                                        CountryName = (string)x.Element("CountryName"),
                                        RegionName = (string)x.Element("RegionName"),
                                        CityName = (string)x.Element("City"),
                                        ZipPostalCode = (string)x.Element("ZipPostalCode"),
                                        Timezone = new TimezoneDetails
                                        {
                                            TimezoneName = (string)x.Element("TimezoneName"),
                                            Gmtoffset = (float)x.Element("Gmtoffset"),
                                            Dstoffset = (bool)x.Element("Dstoffset")
                                        },
                                        Position = new LatLongDetails
                                        {
                                            Latitude = (float)x.Element("Latitude"),
                                            Longitude = (float)x.Element("Longitude")
                                        }
                                    }).First();
                }
                catch(Exception ex)
                {
                    Helpers.LogError(ex, "Unable to resolve location by IP Address. No action is required.");
                }

                return location;
            }
        }

        #endregion

        #region ALL MEHODS OF CATEGORIES

        public static Result<CategoryDetails> GetCategories()
        {
            Result<CategoryDetails> result = new Result<CategoryDetails>();
            try
            {
                string key = "Clients_Categories";
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CategoryDetails>)BizObject.Cache[key];
                    result.Message = "Record retrieved from Caching";
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.GetCategories();
                    BaseClient.CacheData(key, result.EntityList);
                }
                if (result.EntityList.Count > 0)
                {
                    Dictionary<string, ByteStruct> imgList = new Dictionary<string, ByteStruct>();
                    foreach (CategoryDetails category in result.EntityList)
                    {
                        imgList.Add(category.CategoryID, new ByteStruct() { MainImage = category.CategoryImage, SupportImage = null });
                    }
                    WebContext.ImageList = imgList;
                }
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

        public static Result<CategoryDetails> GetCategories(string prefix)
        {
            Result<CategoryDetails> result = new Result<CategoryDetails>();
            try
            {
                string key = "Clients_Categories_" + prefix;
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CategoryDetails>)BizObject.Cache[key];
                    result.Message = "Record retrieved from Caching";
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.GetCategories(prefix);
                    BaseClient.CacheData(key, result.EntityList);
                }
                if (result.EntityList.Count > 0)
                {
                    Dictionary<string, ByteStruct> imgList = new Dictionary<string, ByteStruct>();
                    foreach (CategoryDetails category in result.EntityList)
                    {
                        imgList.Add(category.CategoryID, new ByteStruct() { MainImage = category.CategoryImage, SupportImage = null });
                    }
                    WebContext.ImageList = imgList;
                }
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

        public static Result<CategoryDetails> InsertCategory(CategoryDetails category)
        {
            Result<CategoryDetails> result = new Result<CategoryDetails>();
            try
            {
                string key = "Clients_Categories";
                int i = SiteProvider.Clients.InsertCategory(category);
                if (i > 0)
                {
                    result.EntityList.Add(category);
                    result.Status = ResultStatus.Success;
                    result.Message = "The <b>" + category.CategoryName + "</b> Category has been added successfully.";
                    BizObject.PurgeCacheItems(key);
                }
                else
                {
                    result.Message = "The " + category.CategoryName + " category could not be added.";
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR! " + ex.Message;
            }
            return result;
        }

        public static Result<CategoryDetails> UpdateCategory(CategoryDetails category)
        {
            Result<CategoryDetails> result = new Result<CategoryDetails>();
            try
            {
                string key = "Clients_Categories";
                int i = SiteProvider.Clients.UpdateCategory(category);
                if (i > 0)
                {
                    result.EntityList.Add(category);
                    result.Status = ResultStatus.Success;
                    result.Message = "The <b>" + category.CategoryName + "</b> Category has been updated successfully.";
                    BizObject.PurgeCacheItems(key);
                }
                else
                {
                    result.Message = "The " + category.CategoryName + " category could not be updated.";
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR! " + ex.Message;
            }
            return result;
        }

        public static Result<CategoryDetails> GetCategory(string categoryID)
        {
            Result<CategoryDetails> result = new Result<CategoryDetails>();
            try
            {
                string key = "Clients_Categories";
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CategoryDetails>)BizObject.Cache[key];
                    result.EntityList = (result.EntityList.Where(c => c.CategoryID == categoryID)).ToList();
                    result.Message = "Record retrieved from Caching";
                }
                else
                {
                    result.EntityList.Add(SiteProvider.Clients.GetCategory(categoryID));
                }
                if (result.EntityList.Count > 0)
                {
                    Dictionary<string, ByteStruct> imgList = new Dictionary<string, ByteStruct>();
                    imgList.Add(result.EntityList[0].CategoryID, new ByteStruct() { MainImage = result.EntityList[0].CategoryImage, SupportImage = null });
                    WebContext.ImageList = imgList;
                }
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

        public static Result<CategoryDetails> DeleteCategory(string categoryID)
        {
            Result<CategoryDetails> result = new Result<CategoryDetails>();
            try
            {
                string key = "Clients_Categories";
                int i = SiteProvider.Clients.DeleteCategory(categoryID);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "The category has been deleted.";
                    BizObject.PurgeCacheItems(key);
                }
                else
                {
                    result.Message = "The category could not be deleted.";
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR! " + ex.Message;
            }
            return result;
        }

        #endregion

        #region ALL METHODS OF PROMOTIONS

        public static Result<PromotionDetails> InsertPromotion(PromotionDetails promotion)
        {
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                int i = SiteProvider.Clients.InsertPromotion(promotion);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(promotion);
                    result.Message = "Congratulations! The Promotion Details have been successfully registered.";
                    BizObject.PurgeCacheItems("promotions_promotion_");
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

        public static Result<PromotionDetails> UpdatePromotion(PromotionDetails promotion)
        {
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                int i = SiteProvider.Clients.UpdatePromotion(promotion);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(promotion);
                    result.Message = "The Promotion details have been updated.";
                    BizObject.PurgeCacheItems("promotions_promotion_");
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

        public static Result<PromotionDetails> DeletePromotion(string promotionID, string clientID)
        {
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                int i = SiteProvider.Clients.DeletePromotion(promotionID, clientID);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "The Promotion details have been deleted.";
                    BizObject.PurgeCacheItems("promotions_promotion_");
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

        public static Result<PromotionDetails> SetPromotionActiveStatus(string promotionID, string clientID, bool isActive)
        {
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                int i = SiteProvider.Clients.SetPromotionActiveStatus(promotionID, clientID, isActive);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "The Active status for the Promotion has been updated.";
                    BizObject.PurgeCacheItems("promotions_promotion_");
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

        public static Result<PromotionDetails> SelectPromotion(string promotionID, string clientID)
        {
            string key = "promotions_promotion_" + promotionID + clientID;
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PromotionDetails>)BizObject.Cache[key];
                }
                else
                {
                    PromotionDetails promotion = SiteProvider.Clients.SelectPromotion(promotionID, clientID);
                    if (promotion != null)
                        result.EntityList.Add(promotion);
                        BaseClient.CacheData(key, result.EntityList);
                }
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PromotionDetails> SelectAllCurrentPromotion(string clientID, PagingDetails pObj)
        {
            string key = "promotions_promotion_current" + clientID + pObj.StartRowIndex + pObj.PageSize;
            string keyCount = "promotions_promotion_current_count" + clientID + pObj.StartRowIndex + pObj.PageSize;
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null)
                {
                    result.EntityList = (List<PromotionDetails>)BizObject.Cache[key];
                    pObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectAllCurrentPromotion(clientID, pObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pObj.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);

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

        public static Result<PromotionDetails> SelectAllPromotion(string clientID, PagingDetails pObj)
        {
            string key = "promotions_promotion_all_" + clientID + pObj.StartRowIndex + pObj.PageSize;
            string keyCount = "promotions_promotion_all_count" + clientID + pObj.StartRowIndex + pObj.PageSize;
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && (BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null))
                {
                    result.EntityList = (List<PromotionDetails>)BizObject.Cache[key];
                    pObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectAllPromotion(clientID, pObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pObj.TotalNumber);
                }
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PromotionDetails> SelectCurrentPromotionByPublication(string clientID, bool isActive, PagingDetails pObj)
        {
            string key = "promotions_promotion_current" + clientID + isActive.ToString() + pObj.StartRowIndex + pObj.PageSize;
            string keyCount = "promotions_promotion_current_count" + clientID + isActive.ToString() + pObj.StartRowIndex + pObj.PageSize;
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && (BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null))
                {
                    result.EntityList = (List<PromotionDetails>)BizObject.Cache[key];
                    pObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectCurrentPromotionByPublication(clientID, isActive, pObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pObj.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);
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

        public static Result<PromotionDetails> SelectAllPromotionByPublication(string clientID, bool isActive, PagingDetails pObj)
        {
            string key = "promotions_promotion_all" + clientID + isActive.ToString() + pObj.StartRowIndex + pObj.PageSize;
            string keyCount = "promotions_promotion_all_Count" + clientID + isActive.ToString() + pObj.StartRowIndex + pObj.PageSize;
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PromotionDetails>)BizObject.Cache[key];
                    pObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectAllPromotionByPublication(clientID, isActive, pObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pObj.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);

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

        public static Result<PromotionDetails> SelectPublishedCurrentPromotion(string clientID, PagingDetails pObj)
        {
            string key = "promotions_promotion_current_" + clientID + "Published_" + pObj.StartRowIndex + pObj.PageSize;
            string keyCount = "promotions_promotion_current_" + clientID + "Published_Count" + pObj.StartRowIndex + pObj.PageSize;
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PromotionDetails>)BizObject.Cache[key];
                    pObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectPublishedCurrentPromotion(clientID, pObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pObj.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);

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

        public static Result<PromotionDetails> SelectCurrentAndUpcomingPromotions(string clientID, int selectNRecords)
        {
            string key = "promotions_promotion_current" + clientID;
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PromotionDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectCurrentAndUpcomingPromotions(clientID, selectNRecords);
                    BaseClient.CacheData(key, result.EntityList);
                }

                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);

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

        //  NEWSLY ADDED FUNCTION ON 12/07/2011
        //CURRENT AND UPCOMING
        public static Result<PromotionDetails> SelectAllCurrentUpcomingPromotions(string clientID, PagingDetails pObj)
        {
            string key = "promotions_promotion_allcurrentupcoming_" + clientID + pObj.StartRowIndex + pObj.PageSize;
            string keyCount = "promotions_promotion_allcurrentupcoming_count" + clientID + pObj.StartRowIndex + pObj.PageSize;
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PromotionDetails>)BizObject.Cache[key];
                    pObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectAllCurrentUpcomingPromotions(clientID, pObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pObj.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);

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

        public static Result<PromotionDetails> SelectPublishedCurrentUpcomingPromotions(string clientID, PagingDetails pgObj)
        {
            string key = "promotions_promotion_publishedcurrentupcoming_" + clientID + pgObj.StartRowIndex + pgObj.PageSize;
            string keyCount = "promotions_promotion_publishedcurrentupcoming_count" + clientID + pgObj.StartRowIndex + pgObj.PageSize;
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && (BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null))
                {
                    result.EntityList = (List<PromotionDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectPublishedCurrentUpcomingPromotions(clientID, pgObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pgObj.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);

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

        public static Result<PromotionDetails> SelectUnPublishedCurrentUpcomingPromotions(string clientID, PagingDetails pgObj)
        {
            string key = "promotions_promotion_unpublishedcurrentupcoming_" + clientID + pgObj.StartRowIndex + pgObj.PageSize;
            string keyCount = "promotions_promotion_unpublishedcurrentupcoming_count" + clientID + pgObj.StartRowIndex + pgObj.PageSize;
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PromotionDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectUnPublishedCurrentUpcomingPromotions(clientID, pgObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pgObj.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);

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
        //PAST PROMOTIONS
        public static Result<PromotionDetails> SelectAllPastPromotions(string clientID, PagingDetails pgObj)
        {
            string key = "promotions_promotion_allpast_" + clientID + pgObj.StartRowIndex + pgObj.PageSize;
            string keyCount = "promotions_promotion_allpast_count" + clientID + pgObj.StartRowIndex + pgObj.PageSize;
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PromotionDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectAllPastPromotions(clientID, pgObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pgObj.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);

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

        public static Result<PromotionDetails> SelectPublishedPastPromotions(string clientID, PagingDetails pgObj)
        {
            string key = "promotions_promotion_publishedpast_" + clientID + pgObj.StartRowIndex + pgObj.PageSize;
            string keyCount = "promotions_promotion_publishedpast_count" + clientID + pgObj.StartRowIndex + pgObj.PageSize;
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PromotionDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectPublishedPastPromotions(clientID, pgObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pgObj.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);

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

        public static Result<PromotionDetails> SelectUnPublishedPastPromotions(string clientID, PagingDetails pgObj)
        {
            string key = "promotions_promotion_unpublishedpast_" + clientID + pgObj.StartRowIndex + pgObj.PageSize;
            string keyCount = "promotions_promotion_unpublishedpast_count" + clientID + pgObj.StartRowIndex + pgObj.PageSize;
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PromotionDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectUnPublishedPastPromotions(clientID, pgObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pgObj.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);

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

        //FUTURE PROMOTIONS
        public static Result<PromotionDetails> SelectAllUpcomingPromotions(string clientID, PagingDetails pgObj)
        {
            string key = "promotions_promotion_allupcoming_" + clientID + pgObj.StartRowIndex + pgObj.PageSize;
            string keyCount = "promotions_promotion_allupcoming_count" + clientID + pgObj.StartRowIndex + pgObj.PageSize;
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PromotionDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectAllUpcomingPromotions(clientID, pgObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pgObj.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);

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

        public static Result<PromotionDetails> SelectPublishedUpcomingPromotions(string clientID, PagingDetails pgObj)
        {
            string key = "promotions_promotion_publishedupcoming_" + clientID + pgObj.StartRowIndex + pgObj.PageSize;
            string keyCount = "promotions_promotion_publishedupcoming_count" + clientID + pgObj.StartRowIndex + pgObj.PageSize;
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PromotionDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectPublishedUpcomingPromotions(clientID, pgObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pgObj.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);

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

        public static Result<PromotionDetails> SelectUnPublishedUpcomingPromotions(string clientID, PagingDetails pgObj)
        {
            string key = "promotions_promotion_unpublishedupcoming_" + clientID + pgObj.StartRowIndex + pgObj.PageSize;
            string keyCount = "promotions_promotion_unpublishedupcoming_count" + clientID + pgObj.StartRowIndex + pgObj.PageSize;
            Result<PromotionDetails> result = new Result<PromotionDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PromotionDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectUnPublishedUpcomingPromotions(clientID, pgObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pgObj.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                    SaveImageInSession(result.EntityList);

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

        private static void SaveImageInSession(List<PromotionDetails> promotionList)
        {
            Dictionary<string, ByteStruct> imageList = new Dictionary<string, ByteStruct>();
            foreach (PromotionDetails pd in promotionList)
            {
                ByteStruct imgStruct = new ByteStruct() { MainImage = pd.TitleImage, SupportImage = pd.SupportingImage };
                imageList.Add(pd.PromotionID, imgStruct);
            }
            WebContext.ImageList = imageList;
        }

        #endregion

        #region ALL METHODS OF SERVICES

        public static Result<ServiceDetails> SelectService(string serviceID, string clientID)
        {
            Result<ServiceDetails> result = new Result<ServiceDetails>();
            string key = "services_service_" + clientID + serviceID;
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ServiceDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList.Add(SiteProvider.Clients.SelectService(serviceID, clientID));
                    BaseClient.CacheData(key, result.EntityList);
                }

                if (result.EntityList.Count > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "Record successfully retrieved from Database.";
                }
                else if (result.EntityList.Count == 0)
                {
                    result.Status = ResultStatus.NotFound;
                    result.Message = "No entry in Database.";
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

        public static Result<ServiceDetails> SelectServicesForClient(string clientID, PagingDetails pObj)
        {
            string key = "services_service_" + clientID + pObj.StartRowIndex + pObj.PageSize;
            string keyCount = "services_service_Count_" + clientID + pObj.StartRowIndex + pObj.PageSize;

            Result<ServiceDetails> result = new Result<ServiceDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null)
                {
                    result.EntityList = (List<ServiceDetails>)BizObject.Cache[key];
                    pObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectServicesByClientID(clientID, pObj);
                    BaseClient.CacheData(key, result.EntityList);
                    BaseClient.CacheData(keyCount, pObj.TotalNumber);
                }
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

        public static Result<ServiceDetails> SelectServicesForClient(string clientID)
        {
            string key = "services_service_" + clientID;

            Result<ServiceDetails> result = new Result<ServiceDetails>();
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ServiceDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectServicesByClientID(clientID);
                    BaseClient.CacheData(key, result.EntityList);
                }
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

        public static Result<ServiceDetails> InsertServiceForClient(ServiceDetails serviceDetails)
        {
            Result<ServiceDetails> result = new Result<ServiceDetails>();
            try
            {
                int i = SiteProvider.Clients.InsertServiceForClient(serviceDetails);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(serviceDetails);
                    result.Message = "Congratulations! The Service Details have been successfully registered.";
                    BizObject.PurgeCacheItems("services_service_");
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

        public static Result<ServiceDetails> UpdateServiceForClient(ServiceDetails serviceDetails)
        {
            Result<ServiceDetails> result = new Result<ServiceDetails>();
            try
            {
                int i = SiteProvider.Clients.UpdateServiceForClient(serviceDetails);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(serviceDetails);
                    result.Message = "The Promotion details have been updated.";
                    BizObject.PurgeCacheItems("services_service_");
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

        public static Result<ServiceDetails> DeleteServiceForClient(ServiceDetails serviceDetails)
        {
            Result<ServiceDetails> result = new Result<ServiceDetails>();
            try
            {
                int i = SiteProvider.Clients.DeleteServiceForClient(serviceDetails);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "The Promotion details have been deleted.";
                    BizObject.PurgeCacheItems("services_service_" + serviceDetails.Client.ClientID);
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

        #region  METHODS of PAGEHIT
        public static int InsertPageHit(PageHitDetails pageHit)
        {
            int i = 0;
            try
            {
                SiteProvider.Clients.InsertPageHit(pageHit);
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
            }

            return i;
        }

        public static int CountPageHit(string clientID, string pageHitType)
        {
            int i = 0;
            try
            {
                i = SiteProvider.Clients.CountPageHit(clientID, pageHitType);
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
            }
            return i;
        }

        public static int CountPageHitWithDateRange(string clientID, string pageHitType, DateTime fromDate, DateTime toDate)
        {
            int i = 0;
            try
            {
                i = SiteProvider.Clients.CountPageHitWithDateRange(clientID, pageHitType, fromDate, toDate);
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
            }
            return i;
        }

        public static Result<PageHitDetails> SelectPageHits(string clientID, string hitType, PagingDetails pgObj)
        {

            Result<PageHitDetails> result = new Result<PageHitDetails>();
            try
            {
                result.EntityList = SiteProvider.Clients.SelectPageHit(clientID, hitType, pgObj);
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

        public static Result<PageHitDetails> SelectPageHitsWithDateRange(string clientID, string hitType, DateTime fromDate, DateTime toDate, PagingDetails pgObj)
        {

            Result<PageHitDetails> result = new Result<PageHitDetails>();
            try
            {

                result.EntityList = SiteProvider.Clients.SelectPageHitWithDateRange(clientID, hitType, fromDate, toDate, pgObj);
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

        #endregion

        #region  METHODS of FAQGROUP_DETAILS
        public static Result<FAQGroupDetails> InsertFaqGroup(FAQGroupDetails faqGroup)
        {
            Result<FAQGroupDetails> result = new Result<FAQGroupDetails>();
            try
            {
                string key = "faqgroups_faqgroup";
                int i = SiteProvider.Clients.InsertFaqGroup(faqGroup);
                if (i > 0)
                {
                    result.EntityList.Add(faqGroup);
                    result.Status = ResultStatus.Success;
                    result.Message = "The <b>" + faqGroup.GroupName + "</b> FAQ GROUP has been added successfully.";
                    BizObject.PurgeCacheItems(key);
                }
                else
                {
                    result.Message = "The " + faqGroup.GroupName + " FAQ GROUP could not be added.";
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR! " + ex.Message;
            }
            return result;
        }

        public static Result<FAQGroupDetails> UpdateFaqGroup(FAQGroupDetails faqGroup)
        {
            Result<FAQGroupDetails> result = new Result<FAQGroupDetails>();
            try
            {
                string key = "faqgroups_faqgroup";
                int i = SiteProvider.Clients.UpdateFaqGroup(faqGroup);
                if (i > 0)
                {
                    result.EntityList.Add(faqGroup);
                    result.Status = ResultStatus.Success;
                    result.Message = "The <b>" + faqGroup.GroupName + "</b> FAQ GROUP has been updated successfully.";
                    BizObject.PurgeCacheItems(key);
                }
                else
                {
                    result.Message = "The " + faqGroup.GroupName + " FAQ GROUP could not be updated.";
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR! " + ex.Message;
            }
            return result;
        }

        public static Result<FAQGroupDetails> SelectFaqGroup(string faqGroupID)
        {
            Result<FAQGroupDetails> result = new Result<FAQGroupDetails>();
            try
            {
                string key = "faqgroups_faqgroup_" + faqGroupID;
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<FAQGroupDetails>)BizObject.Cache[key];
                    result.EntityList = (result.EntityList.Where(c => c.FaqGroupID == faqGroupID)).ToList();
                    result.Message = "Record retrieved from Caching";
                }
                else
                {
                    result.EntityList.Add(SiteProvider.Clients.SelectFaqGroupDetails(faqGroupID));
                    BaseClient.CacheData(key, result.EntityList);
                    result.Message = "Record retrieved from database";
                }

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

        public static Result<FAQGroupDetails> SelectFaqGroupByClientID(string clientID)
        {
            Result<FAQGroupDetails> result = new Result<FAQGroupDetails>();
            try
            {
                string key = "faqgroups_faqgroup_" + clientID;
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<FAQGroupDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectFaqGroupDetailsByClient(clientID);
                    CacheData(key, result.EntityList);
                }

                result.Status = ResultStatus.Success;
                result.Message = "FAQ GROUP successfully retrieved.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<FAQGroupDetails> SelectFaqGroupWithClientNull()
        {
            Result<FAQGroupDetails> result = new Result<FAQGroupDetails>();
            try
            {
                string key = "faqgroups_faqgroup_NoClient";
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<FAQGroupDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectFaqGroupWithClientNull();
                }

                result.Status = ResultStatus.Success;
                result.Message = "FAQ GROUP successfully retrieved.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<FAQGroupDetails> DeleteFaqGroup(string faqGroupID)
        {
            Result<FAQGroupDetails> result = new Result<FAQGroupDetails>();
            try
            {
                string key = "faqgroups_faqgroup";
                int i = SiteProvider.Clients.DeleteFaqGroup(faqGroupID);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "The FAQ GROUP has been deleted.";
                    BizObject.PurgeCacheItems(key);
                    BizObject.PurgeCacheItems("faqs_faq");
                }
                else
                {
                    result.Message = "The FAQ GROUP could not be deleted.";
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR! " + ex.Message;
            }
            return result;
        }
        #endregion

        #region METHODS THAT WORK WITH FAQ
        public static Result<FAQDetails> InsertFaq(FAQDetails faq)
        {
            Result<FAQDetails> result = new Result<FAQDetails>();
            try
            {
                string key = "faqs_faq";
                int i = SiteProvider.Clients.InsertFaq(faq);
                if (i > 0)
                {
                    result.EntityList.Add(faq);
                    result.Status = ResultStatus.Success;
                    result.Message = "The FAQ has been added successfully.";
                    BizObject.PurgeCacheItems(key);
                }
                else
                {
                    result.Message = "The FAQ could not be added.";
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR! " + ex.Message;
            }
            return result;
        }

        public static Result<FAQDetails> UpdateFaq(FAQDetails faq)
        {
            Result<FAQDetails> result = new Result<FAQDetails>();
            try
            {
                string key = "faqs_faq";
                int i = SiteProvider.Clients.UpdateFaq(faq);
                if (i > 0)
                {
                    result.EntityList.Add(faq);
                    result.Status = ResultStatus.Success;
                    result.Message = "The FAQ has been updated successfully.";
                    BizObject.PurgeCacheItems(key);
                }
                else
                {
                    result.Message = "The FAQ could not be updated.";
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR! " + ex.Message;
            }
            return result;
        }

        public static Result<FAQDetails> DeleteFaq(string faqID)
        {
            Result<FAQDetails> result = new Result<FAQDetails>();
            try
            {
                string key = "faqs_faq";
                int i = SiteProvider.Clients.DeleteFaq(faqID);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "The FAQ has been deleted.";
                    BizObject.PurgeCacheItems(key);
                }
                else
                {
                    result.Message = "The FAQ could not be deleted.";
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR! " + ex.Message;
            }
            return result;
        }

        public static Result<FAQDetails> SelectFaq(string faqID)
        {
            Result<FAQDetails> result = new Result<FAQDetails>();
            try
            {
                string key = "faqs_faq_" + faqID;
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<FAQDetails>)BizObject.Cache[key];
                    result.EntityList = (result.EntityList.Where(c => c.FaqID == faqID)).ToList();
                }
                else
                {
                    result.EntityList.Add(SiteProvider.Clients.SelectFaqDetails(faqID));
                }

                result.Message = "Record retrieved successfully.";
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

        public static Result<FAQDetails> SelectFaqByFaqGroup(string faqGroupID)
        {
            Result<FAQDetails> result = new Result<FAQDetails>();
            try
            {
                string key = "faqs_faq_" + faqGroupID;
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<FAQDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectFaqDetailsByFaqGroup(faqGroupID);
                }

                result.Status = ResultStatus.Success;
                result.Message = "FAQ successfully retrieved.";
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

        #region METHODS THAT WORK WITH DATAEXTENDER
        public static Result<DataExtenderDetails> InsertDataExtender(DataExtenderDetails dataExtender)
        {
            Result<DataExtenderDetails> result = new Result<DataExtenderDetails>();
            try
            {
                string key = "dataextenders_dataextender";
                int i = SiteProvider.Clients.InsertDataExtender(dataExtender);
                if (i > 0)
                {
                    result.EntityList.Add(dataExtender);
                    result.Status = ResultStatus.Success;
                    result.Message = "Data has been added successfully.";
                    BizObject.PurgeCacheItems(key);
                }
                else
                {
                    result.Message = "Unable to add data. Please try again later or contact sleeksurf.";
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR! " + ex.Message;
            }
            return result;
        }

        public static Result<DataExtenderDetails> UpdatePrivacyAndPolicy(DataExtenderDetails privacyAndPolicy)
        {
            Result<DataExtenderDetails> result = new Result<DataExtenderDetails>();
            try
            {
                string key = "dataextenders_dataextender";
                int i = SiteProvider.Clients.UpdatePrivacyAndPolicy(privacyAndPolicy);
                if (i > 0)
                {
                    result.EntityList.Add(privacyAndPolicy);
                    result.Status = ResultStatus.Success;
                    result.Message = "Privacy Policy has been updated successfully.";
                    BizObject.PurgeCacheItems(key);
                }
                else
                {
                    result.Message = "Unable to update Privacy Policy. Please try again later or contact sleeksurf.";
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR! " + ex.Message;
            }
            return result;
        }

        public static Result<DataExtenderDetails> UpdateTermsAndConditions(DataExtenderDetails termsAndConditions)
        {
            Result<DataExtenderDetails> result = new Result<DataExtenderDetails>();
            try
            {
                string key = "dataextenders_dataextender_";
                int i = SiteProvider.Clients.UpdateTermsAndConditions(termsAndConditions);
                if (i > 0)
                {
                    result.EntityList.Add(termsAndConditions);
                    result.Status = ResultStatus.Success;
                    result.Message = "Terms &amp; Conditions has been updated successfully.";
                    BizObject.PurgeCacheItems(key);
                }
                else
                {
                    result.Message = "Unable to update Terms &amp; Conditions. Please try again later or contact sleeksurf.";
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR! " + ex.Message;
            }
            return result;
        }

        public static Result<DataExtenderDetails> UpdateDataExtender(DataExtenderDetails dataExtender)
        {
            Result<DataExtenderDetails> result = new Result<DataExtenderDetails>();
            try
            {
                string key = "dataextenders_dataextender_";
                int i = SiteProvider.Clients.UpdateDataExtender(dataExtender);
                if (i > 0)
                {
                    result.EntityList.Add(dataExtender);
                    result.Status = ResultStatus.Success;
                    result.Message = "Data has been updated successfully.";
                    BizObject.PurgeCacheItems(key);
                }
                else
                {
                    result.Message = "Unable to update data. Please try again later or contact sleeksurf.";
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR! " + ex.Message;
            }
            return result;
        }

        public static Result<DataExtenderDetails> DeleteDataExtender(int dataExtenderID)
        {
            Result<DataExtenderDetails> result = new Result<DataExtenderDetails>();
            try
            {
                string key = "dataextenders_dataextender_";
                int i = SiteProvider.Clients.DeleteDataExtender(dataExtenderID);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "The Data has been deleted.";
                    BizObject.PurgeCacheItems(key);
                }
                else
                {
                    result.Message = "The Data could not be deleted.";
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR! " + ex.Message;
            }
            return result;
        }

        public static Result<DataExtenderDetails> SelectDataExtender(int dataExtenderID)
        {
            Result<DataExtenderDetails> result = new Result<DataExtenderDetails>();
            try
            {
                string key = "dataextenders_dataextender_" + dataExtenderID;
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<DataExtenderDetails>)BizObject.Cache[key];
                    result.EntityList = (result.EntityList.Where(c => c.DataExtenderID == dataExtenderID)).ToList();
                }
                else
                {
                    result.EntityList.Add(SiteProvider.Clients.SelectDataExtender(dataExtenderID));
                    BaseClient.CacheData(key, result.EntityList);
                }

                result.Status = ResultStatus.Success;
                result.Message = "Data successfully retrieved.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<DataExtenderDetails> SelectDataExtenderByClient(string clientID)
        {
            Result<DataExtenderDetails> result = new Result<DataExtenderDetails>();
            try
            {
                string key = "dataextenders_dataextender_" + clientID;
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<DataExtenderDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectDataExtenderByClient(clientID);
                    BaseClient.CacheData(key, result.EntityList);
                }

                result.Status = ResultStatus.Success;
                result.Message = "The Data is successfully retrieved.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<DataExtenderDetails> SelectDataExtenderWithClientNull()
        {
            Result<DataExtenderDetails> result = new Result<DataExtenderDetails>();
            try
            {
                string key = "dataextenders_dataextender_NoClient";
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<DataExtenderDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Clients.SelectDataExtenderWithClientNull();
                }

                result.Status = ResultStatus.Success;
                result.Message = "The Data successfully retrieved.";
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

        #region PAGEHIT ANALYTICS
        public static DataTable HitCountrySelect(DateTime dateFrom, string clientID, string hitType)
        {
            try
            {
                return SiteProvider.Clients.HitCountrySelect(dateFrom, clientID, hitType);
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
                return null;
            }
        }

        public static DataTable HitCountrySelectAll(string clientID, string hitType)
        {
            try
            {
                return SiteProvider.Clients.HitCountrySelectAll(clientID, hitType);
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
                return null;
            }
        }

        public static DataTable HitCitySelect(DateTime dateFrom, string countryName, string clientID, string hitType)
        {
            try
            {
                return SiteProvider.Clients.HitCitySelect(dateFrom, countryName, clientID, hitType);
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
                return null;
            }
        }

        public static DataTable HitCitySelectAll(string countryName, string clientID, string hitType)
        {
            try
            {
                return SiteProvider.Clients.HitCitySelectAll(countryName, clientID, hitType);
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
                return null;
            }
        }
        #endregion
        #region METHODS THAT WORK WITH SMS CREDIT
        public static bool UpdateSMSCredit(string clientID, int newCredit)
        {
            bool result = false;
            try
            {
                int i = SiteProvider.Clients.UpdateSMSCredit(clientID, newCredit);
                if (i > 0)
                {
                    result = true;
                    BizObject.PurgeCacheItems("clients_client");
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
            }

            return result;
        }
        #endregion

        #region METHODS THAT WORK WITH CLIENT FEATURES
        public static bool ClientFeatureSetStatus(string clientID, string featureType, bool setStatus)
        {
            bool result = false;
            switch (featureType)
            {
                case "Listing":
                    result = ClientFeatureSetListingStatus(clientID, setStatus);
                    break;
                case "ClientProfile":
                    result = ClientFeatureSetClientProfileStatus(clientID, setStatus);
                    break;
                case "ClientDomain":
                    result = ClientFeatureSetClientDomainStatus(clientID, setStatus);
                    break;
            }
            return result;
        }
        public static bool ClientFeatureSetClientProfileStatus(string clientID, bool clientProfile)
        {
            bool result = false;
            try
            {
                result = SiteProvider.Clients.ClientFeatureSetClientProfileStatus(clientID, clientProfile);
                BaseClient.PurgeCacheItems("clientFeatures");
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
            }

            return result;
        }
        public static bool ClientFeatureSetListingStatus(string clientID, bool listing)
        {
            bool result = false;
            try
            {
                result = SiteProvider.Clients.ClientFeatureSetListingStatus(clientID, listing);
                BaseClient.PurgeCacheItems("clientFeatures");
            }
            catch(Exception ex)
            {
                Helpers.LogError(ex);
            }

            return result;
        }
        public static bool ClientFeatureSetClientDomainStatus(string clientID, bool clientDomain)
        {
            bool result = false;
            try
            {
                result = SiteProvider.Clients.ClientFeatureSetClientDomainStatus(clientID, clientDomain);
                BaseClient.PurgeCacheItems("clientFeatures");
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
            }

            return result;
        }
        public static Result<ClientFeatureDetails> SelectClientFeatureDetails(string clientID)
        {
            Result<ClientFeatureDetails> result = new Result<ClientFeatureDetails>();
            string key = "clientFeatures_" + clientID;
            try
            {
                if (BaseClient.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<ClientFeatureDetails>)BizObject.Cache[key];
                    result.Message = "Records successfully retrieved from Cache.";
                    result.Status = ResultStatus.Success;
                }
                else
                {
                    result.EntityList.Add(SiteProvider.Clients.SelectClientFeatureDetails(clientID));
                    if (result.EntityList.Count > 0)
                    {
                        result.Message = "Records successfully retrieved from Database.";
                        result.Status = ResultStatus.Success;
                    }
                    BaseClient.CacheData(key, result.EntityList);
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
