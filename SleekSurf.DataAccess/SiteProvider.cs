using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.DataAccess
{
    public static class SiteProvider
    {
        public static ClientProvider Clients
        {
            get { return ClientProvider.Instance; }
        }

        public static CustomerProvider Customers
        {
            get { return CustomerProvider.Instance; }
        }
        public static CountryProvider Countries
        {
            get { return CountryProvider.Instance; }
        }
        public static ClientPackageProvider Packages
        {
            get { return ClientPackageProvider.Instance; }
        }
        public static EventProvider Events
        {
            get { return EventProvider.Instance; }
        }

        public static AdvertisementProvider Advertisements
        {
            get { return AdvertisementProvider.Instance; }
        }

        public static ErrorLogProvider ErrorLogs
        {
            get { return ErrorLogProvider.Instance; }
        }
    }
}
