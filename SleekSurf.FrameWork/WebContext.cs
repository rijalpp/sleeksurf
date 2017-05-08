using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Security.Principal;
using SleekSurf.Entity;

namespace SleekSurf.FrameWork
{
    public static class WebContext
    {
        public static TempInfo TempInfo
        {
            get
            {
                if (!ContainsInSession("tempInfos"))
                    SetInSession("tempInfos", new TempInfo());

                return GetFromSession("tempInfos") as TempInfo;
            }

            set
            {
                UpdateInSession("tempInfos", value);
            }
        }

        public static object SelectedUser
        {
            get
            {
                if (ContainsInSession("SelectedUser"))
                    return GetFromSession("SelectedUser");
                return null;
            }
            set
            {
                SetInSession("SelectedUser", value);
            }
        }

        public static IPrincipal CurrentUser
        {
            get
            {
                if (ContainsInSession("CurrentUser"))
                    return GetFromSession("CurrentUser") as IPrincipal;

                return null;
            }
            set
            {
                SetInSession("CurrentUser", value);
            }
        }

        public static IPrincipal Sibling
        {
            get
            {
                if (ContainsInSession("Sibling"))
                    return GetFromSession("Sibling") as IPrincipal;

                return null;
            }
            set
            {
                SetInSession("Sibling", value);
            }
        }

        public static ClientDetails Parent
        {
            get
            {
                if (ContainsInSession("Parent"))
                    return GetFromSession("Parent") as ClientDetails;

                return null;
            }
            set
            {
                SetInSession("Parent", value);
            }
        }

        public static ClientDetails ClientProfile
        {
            get
            {
                if (ContainsInSession("ClientProfile"))
                    return GetFromSession("ClientProfile") as ClientDetails;

                return null;
            }
            set
            {
                SetInSession("ClientProfile", value);
            }
        }

        public static string UsernameToVerify
        {
            get
            {
                return GetQueryStringValue("ID");
            }
        }

        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }

        public static bool ContainsInSession(string key)
        {
            return HttpContext.Current.Session[key] != null;
        }

        public static void RemoveFromSession(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }

        public static string GetQueryStringValue(string key)
        {
            return HttpContext.Current.Request.QueryString.Get(key);
        }

        private static void SetInSession(string key, object value)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
                return;

            if (HttpContext.Current.Session[key] == null)
                HttpContext.Current.Session.Add(key, value);
            else
                UpdateInSession(key, value);
        }

        private static object GetFromSession(string key)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
                return null;

            return HttpContext.Current.Session[key];
        }

        public static Dictionary<string, ByteStruct> ImageList
        {
            get
            {
                if (ContainsInSession("ImageList"))
                    return GetFromSession("ImageList") as Dictionary<string, ByteStruct>;
                return null;
            }
            set
            {
                SetInSession("ImageList", value);
            }
        }

        private static void UpdateInSession(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }
    }
}
