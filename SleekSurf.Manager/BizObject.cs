using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Security.Principal;

namespace SleekSurf.Manager
{
    public abstract class BizObject
    {
        protected const int MAXROWS = int.MaxValue;
        protected static Cache Cache
        {
            get { return HttpContext.Current.Cache; }
        }
        protected static IPrincipal CurrentUser
        {
            get { return HttpContext.Current.User; }
        }
        protected static string CurrentUserName
        {
            get
            {
                string userName = "";
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                    userName = HttpContext.Current.User.Identity.Name;
                return userName;
            }
        }
        protected static string CurrentUserIP
        {
            get { return HttpContext.Current.Request.UserHostAddress; }
        }
        protected static int GetPageIndex(int startRowIndex, int maximumRows)
        {
            if (maximumRows <= 0)
                return 0;
            else
                return (int)Math.Floor((double)startRowIndex / (double)maximumRows);
        }
        protected static string EncodeText(string content)
        {
            content = HttpUtility.HtmlEncode(content);
            content = content.Replace(" ", "&nbsp;&nbsp;").Replace("\n", "<br />");
            return content;
        }
        protected static string ConvertNullToToEmptyString(string input)
        {
            return (input == null ? "" : input);
        }
        protected static void PurgeCacheItems(string prefix)
        {
            List<string> itemsToRemove = new List<string>();
            IDictionaryEnumerator enumerator = BizObject.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString().ToLower().StartsWith(prefix.ToLower()))
                    itemsToRemove.Add(enumerator.Key.ToString());
            }
            foreach (string itemToRemove in itemsToRemove)
                BizObject.Cache.Remove(itemToRemove);
        }

    }
}
