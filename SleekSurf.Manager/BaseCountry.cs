using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SleekSurf.FrameWork;

namespace SleekSurf.Manager
{
    public abstract class BaseCountry : BizObject
    {
        protected static CountryElement Settings
        {
            get { return Globals.Settings.Countries; }
        }
        protected static void CacheData(string key, object data)
        {
            if (Settings.EnableCaching && data != null)
            {
                BizObject.Cache.Insert(key, data, null, DateTime.Now.AddSeconds(Settings.CacheDuration), TimeSpan.Zero);
            }
        }
    }
}
