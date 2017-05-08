using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SleekSurf.FrameWork;
using SleekSurf.DataAccess;
using System.Configuration;

namespace SleekSurf.Manager
{
    public abstract class BaseClient : BizObject
    {
        protected static ClientsElement Settings
        {
            get { return Globals.Settings.Clients; }
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
