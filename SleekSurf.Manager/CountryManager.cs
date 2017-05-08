using System;
using System.Collections.Generic;
using System.Linq;
using SleekSurf.Entity;
using SleekSurf.DataAccess;
using SleekSurf.FrameWork;

namespace SleekSurf.Manager
{
    public class CountryManager : BaseCountry
    {
        public static Result<CountryDetails> GetCountry(int countryID)
        {
            Result<CountryDetails> result = new Result<CountryDetails>();
            try
            {
                string key = "Countries";
                if (BaseCountry.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CountryDetails>)BizObject.Cache[key];
                    result.EntityList = (from c in result.EntityList
                                         where c.CountryID == countryID
                                         select c).ToList();
                }
                else
                {
                    result.EntityList.Add(SiteProvider.Countries.GetCountry(countryID));
                }
                if (result.EntityList.Count > 0)
                    result.Status = ResultStatus.Success;
                else if (result.EntityList.Count == 0)
                    result.Status = ResultStatus.NotFound;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CountryDetails> GetCountry(string countryName)
        {
            Result<CountryDetails> result = new Result<CountryDetails>();
            try
            {
                string key = "Countries";
                if (BaseCountry.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CountryDetails>)BizObject.Cache[key];
                    result.EntityList = (from c in result.EntityList
                                         where c.CountryName == countryName
                                         select c).ToList();
                }
                else
                {
                    result.EntityList.Add(SiteProvider.Countries.GetCountry(countryName));
                }
                if (result.EntityList.Count > 0)
                    result.Status = ResultStatus.Success;
                else if (result.EntityList.Count == 0)
                    result.Status = ResultStatus.NotFound;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CountryDetails> GetCountryByDialCode(int dialCode)
        {
            Result<CountryDetails> result = new Result<CountryDetails>();
            try
            {
                string key = "Countries";
                if (BaseCountry.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CountryDetails>)BizObject.Cache[key];
                    result.EntityList = (from c in result.EntityList
                                         where c.DialCode == dialCode
                                         select c).ToList();
                }
                else
                {
                    result.EntityList.Add(SiteProvider.Countries.GetCountry(dialCode));
                }
                if (result.EntityList.Count > 0)
                    result.Status = ResultStatus.Success;
                else if (result.EntityList.Count == 0)
                    result.Status = ResultStatus.NotFound;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<CountryDetails> GetCountries()
        {
            Result<CountryDetails> result = new Result<CountryDetails>();
            try
            {
                string key = "Countries";
                if (BaseCountry.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<CountryDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Countries.GetCountries();
                    BaseCountry.CacheData(key, result.EntityList);
                }
                if (result.EntityList.Count > 0)
                    result.Status = ResultStatus.Success;
                else if (result.EntityList.Count == 0)
                    result.Status = ResultStatus.NotFound;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }
    }
}
