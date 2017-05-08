using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SleekSurf.FrameWork;

namespace SleekSurf.Manager
{
    public abstract class BaseCustomer : BizObject
    {
        protected static CustomersElement Settings
        {
            get { return Globals.Settings.Customers; }
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
