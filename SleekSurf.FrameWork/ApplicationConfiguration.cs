using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using System.Web.Configuration;

namespace SleekSurf.FrameWork
{
    public class ApplicationConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("defaultConnectionStringName", DefaultValue = "LocalSqlServer")]
        public string DefaultConnectionStringName
        {
            get { return (string)base["defaultConnectionStringName"]; }
            set { base["defaultConectionStringName"] = value; }
        }
        [ConfigurationProperty("defaultCacheDuration", DefaultValue = "600")]
        public int DefaultCacheDuration
        {
            get { return (int)base["defaultCacheDuration"]; }
            set { base["defaultCacheDuration"] = value; }
        }
        [ConfigurationProperty("clients", IsRequired = true)]
        public ClientsElement Clients
        {
            get { return (ClientsElement)base["clients"]; }
        }
        [ConfigurationProperty("customers", IsRequired = true)]
        public CustomersElement Customers
        {
            get { return (CustomersElement)base["customers"]; }
        }

        [ConfigurationProperty("countries", IsRequired = true)]
        public CountryElement Countries
        {
            get { return (CountryElement)base["countries"]; }
        }

        [ConfigurationProperty("events", IsRequired = true)]
        public EventElement Events
        {
            get { return (EventElement)base["events"]; }
        }

        [ConfigurationProperty("contactForm", IsRequired = true)]
        public ContactFormElement ContactForm
        {
            get { return (ContactFormElement)base["contactForm"]; }
        }

        [ConfigurationProperty("package", IsRequired = true)]
        public PackageElement Package
        {
            get { return (PackageElement)base["package"]; }
        }

        [ConfigurationProperty("errorLogForm", IsRequired = true)]
        public ErrorLogElement ErrorLogForm
        {
            get { return (ErrorLogElement)base["errorLogForm"]; }
        }
        [ConfigurationProperty("advertisement", IsRequired = true)]
        public AdvertisementElement Advertisement
        {
            get { return (AdvertisementElement)base["advertisement"]; }
        }
    }

    public class ContactFormElement : ConfigurationElement
    {
        [ConfigurationProperty("mailSubject", DefaultValue = "{0}: {1}")]
        public string MailSubject
        {
            get
            {
                return (string)base["mailSubject"];
            }
            set
            {
                base["mailSubject"] = value;
            }
        }
        [ConfigurationProperty("mailTo", IsRequired = true)]
        public string MailTo
        {
            get { return (string)base["mailTo"]; }
            set { base["mailTo"] = value; }
        }
        [ConfigurationProperty("mailCC")]
        public string MailCC
        {
            get { return (string)base["mailCC"]; }
            set { base["mailCC"] = value; }
        }
    }

    public class ClientsElement : ConfigurationElement
    {
        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            get { return (string)base["connectionStringName"]; }
            set { base["connectionStringName"] = value; }
        }
        public string ConnectionString
        {
            get
            {
                string connStringName = (string.IsNullOrEmpty(this.ConnectionStringName) ?
                      Globals.Settings.DefaultConnectionStringName : this.ConnectionStringName);
                return WebConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            }
        }
        [ConfigurationProperty("providerType", DefaultValue = "SleekSurf.DataAccess.SqlClient.SqlClientsProvider")]
        public string ProviderType
        {
            get { return (string)base["providerType"]; }
            set { base["providerType"] = value; }
        }
        [ConfigurationProperty("enableCaching", DefaultValue = "true")]
        public bool EnableCaching
        {
            get { return (bool)base["enableCaching"]; }
            set { base["enableCaching"] = value; }
        }
        [ConfigurationProperty("cacheDuration")]
        public int CacheDuration
        {
            get
            {
                int duration = (int)base["cacheDuration"];
                return (duration > 0 ? duration : Globals.Settings.DefaultCacheDuration);
            }
            set
            {
                base["cacheDuration"] = value;
            }
        }
        [ConfigurationProperty("pageSize")]
        public int PageSize
        {
            get { return (int)base["pageSize"]; }
            set { base["pageSize"] = value; }
        }
    }

    public class CustomersElement : ConfigurationElement
    {
        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            get { return (string)base["connectionStringName"]; }
            set { base["connectionStringName"] = value; }
        }
        public string ConnectionString
        {
            get
            {
                string connStringName = (string.IsNullOrEmpty(this.ConnectionStringName) ?
                      Globals.Settings.DefaultConnectionStringName : this.ConnectionStringName);
                return WebConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            }
        }
        [ConfigurationProperty("providerType", DefaultValue = "SleekSurf.DataAccess.SqlClient.SqlCustomerProvider")]
        public string ProviderType
        {
            get { return (string)base["providerType"]; }
            set { base["providerType"] = value; }
        }
        [ConfigurationProperty("enableCaching", DefaultValue = "true")]
        public bool EnableCaching
        {
            get { return (bool)base["enableCaching"]; }
            set { base["enableCaching"] = value; }
        }
        [ConfigurationProperty("cacheDuration")]
        public int CacheDuration
        {
            get
            {
                int duration = (int)base["cacheDuration"];
                return (duration > 0 ? duration : Globals.Settings.DefaultCacheDuration);
            }
            set
            {
                base["cacheDuration"] = value;
            }
        }
        [ConfigurationProperty("pageSize")]
        public int PageSize
        {
            get { return (int)base["pageSize"]; }
            set { base["pageSize"] = value; }
        }
    }

    public class CountryElement : ConfigurationElement
    {
        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            get { return (string)base["connectionStringName"]; }
            set { base["connectionStringName"] = value; }
        }
        public string ConnectionString
        {
            get
            {
                string connStringName = (string.IsNullOrEmpty(this.ConnectionStringName) ?
                      Globals.Settings.DefaultConnectionStringName : this.ConnectionStringName);
                return WebConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            }
        }
        [ConfigurationProperty("providerType", DefaultValue = "SleekSurf.DataAccess.SqlClient.SqlCountryProvider")]
        public string ProviderType
        {
            get { return (string)base["providerType"]; }
            set { base["providerType"] = value; }
        }
        [ConfigurationProperty("enableCaching", DefaultValue = "true")]
        public bool EnableCaching
        {
            get { return (bool)base["enableCaching"]; }
            set { base["enableCaching"] = value; }
        }
        [ConfigurationProperty("cacheDuration")]
        public int CacheDuration
        {
            get
            {
                int duration = (int)base["cacheDuration"];
                return (duration > 0 ? duration : Globals.Settings.DefaultCacheDuration);
            }
            set
            {
                base["cacheDuration"] = value;
            }
        }
        [ConfigurationProperty("pageSize")]
        public int PageSize
        {
            get { return (int)base["pageSize"]; }
            set { base["pageSize"] = value; }
        }
    }

    public class EventElement : ConfigurationElement
    {
        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            get { return (string)base["connectionStringName"]; }
            set { base["connectionStringName"] = value; }
        }
        public string ConnectionString
        {
            get
            {
                string connStringName = (string.IsNullOrEmpty(this.ConnectionStringName) ?
                      Globals.Settings.DefaultConnectionStringName : this.ConnectionStringName);
                return WebConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            }
        }
        [ConfigurationProperty("providerType", DefaultValue = "SleekSurf.DataAccess.SqlClient.SqlEventProvider")]
        public string ProviderType
        {
            get { return (string)base["providerType"]; }
            set { base["providerType"] = value; }
        }
        [ConfigurationProperty("enableCaching", DefaultValue = "true")]
        public bool EnableCaching
        {
            get { return (bool)base["enableCaching"]; }
            set { base["enableCaching"] = value; }
        }
        [ConfigurationProperty("cacheDuration")]
        public int CacheDuration
        {
            get
            {
                int duration = (int)base["cacheDuration"];
                return (duration > 0 ? duration : Globals.Settings.DefaultCacheDuration);
            }
            set
            {
                base["cacheDuration"] = value;
            }
        }
        [ConfigurationProperty("pageSize")]
        public int PageSize
        {
            get { return (int)base["pageSize"]; }
            set { base["pageSize"] = value; }
        }
    }

    public class PackageElement : ConfigurationElement
    {
        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            get { return (string)base["connectionStringName"]; }
            set { base["connectionStringName"] = value; }
        }
        public string ConnectionString
        {
            get
            {
                string connStringName = (string.IsNullOrEmpty(this.ConnectionStringName) ?
                      Globals.Settings.DefaultConnectionStringName : this.ConnectionStringName);
                return WebConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            }
        }
        [ConfigurationProperty("providerType", DefaultValue = "SleekSurf.DataAccess.SqlClient.SqlClientPackageProvider")]
        public string ProviderType
        {
            get { return (string)base["providerType"]; }
            set { base["providerType"] = value; }
        }
        [ConfigurationProperty("sandboxMode", DefaultValue = false)]
        public bool SandBoxMode
        {
            get { return (bool)base["sandboxMode"]; }
            set { base["sandboxMode"] = value; }
        }
        [ConfigurationProperty("businessEmail", DefaultValue = "sleeksurf@gmail.com")]
        public string BusinessEmail
        {
            get { return (string)base["businessEmail"]; }
            set { base["businessEmail"] = value; }
        }
        [ConfigurationProperty("currencyCode", DefaultValue = "AUD")]
        public string CurrencyCode
        {
            get { return (string)base["currencyCode"]; }
            set { base["currencyCode"] = value; }
        }
        [ConfigurationProperty("enableCaching", DefaultValue = "true")]
        public bool EnableCaching
        {
            get { return (bool)base["enableCaching"]; }
            set { base["enableCaching"] = value; }
        }
        [ConfigurationProperty("cacheDuration")]
        public int CacheDuration
        {
            get
            {
                int duration = (int)base["cacheDuration"];
                return (duration > 0 ? duration : Globals.Settings.DefaultCacheDuration);
            }
            set
            {
                base["cacheDuration"] = value;
            }
        }
        [ConfigurationProperty("pageSize")]
        public int PageSize
        {
            get { return (int)base["pageSize"]; }
            set { base["pageSize"] = value; }
        }
    }

    public class ErrorLogElement : ConfigurationElement
    {
        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            get { return (string)base["connectionStringName"]; }
            set { base["connectionStringName"] = value; }
        }
        public string ConnectionString
        {
            get
            {
                string connStringName = (string.IsNullOrEmpty(this.ConnectionStringName) ?
                      Globals.Settings.DefaultConnectionStringName : this.ConnectionStringName);
                return WebConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            }
        }
        [ConfigurationProperty("providerType", DefaultValue = "SleekSurf.DataAccess.SqlClient.SqlErrorLogProvider")]
        public string ProviderType
        {
            get { return (string)base["providerType"]; }
            set { base["providerType"] = value; }
        }
        [ConfigurationProperty("enableErrorLog", DefaultValue = "true")]
        public bool EnableErrorLog
        {
            get { return (bool)base["enableErrorLog"]; }
            set { base["enableErrorLog"] = value; }
        }

        [ConfigurationProperty("errorLogEmail")]
        public string ErrorLogEmail
        {
            get { return (string)base["errorLogEmail"]; }
            set { base["errorLogEmail"] = value; }
        }
        [ConfigurationProperty("from")]
        public string From
        {
            get { return (string)base["from"]; }
            set { base["from"] = value; }
        }
    }

    public class AdvertisementElement : ConfigurationElement
    {
        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            get { return (string)base["connectionStringName"]; }
            set { base["connectionStringName"] = value; }
        }
        public string ConnectionString
        {
            get
            {
                string connStringName = (string.IsNullOrEmpty(this.ConnectionStringName) ?
                      Globals.Settings.DefaultConnectionStringName : this.ConnectionStringName);
                return WebConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            }
        }
        [ConfigurationProperty("providerType", DefaultValue = "SleekSurf.DataAccess.SqlClient.SqlAdvertisementProvider")]
        public string ProviderType
        {
            get { return (string)base["providerType"]; }
            set { base["providerType"] = value; }
        }
    }
}
