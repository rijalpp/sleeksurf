using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using SleekSurf.Entity;
using SleekSurf.FrameWork;

namespace SleekSurf.DataAccess
{
    public abstract class CountryProvider : DataAccess
    {
        static private CountryProvider _instance = null;
        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        static public CountryProvider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = (CountryProvider)Activator.CreateInstance(
                       Type.GetType(Globals.Settings.Countries.ProviderType));
                return _instance;
            }
        }

        public CountryProvider()
        {
            this.ConnectionString = Globals.Settings.Clients.ConnectionString;
            this.EnableCaching = Globals.Settings.Clients.EnableCaching;
            this.CacheDuration = Globals.Settings.Clients.CacheDuration;
        }
        protected virtual CountryDetails GetCountryFromReader(IDataReader reader)
        {
            CountryDetails country = new CountryDetails();
            country.CountryID = (int)reader["CountryID"];
            if(reader["DialCode"] != DBNull.Value)
            country.DialCode = (int)reader["DialCode"];
            country.CountryName = reader["CountryName"].ToString();
            country.Description = reader["Description"].ToString();
            return country;
        }

        public virtual List<CountryDetails> GetCountryCollectionFromReader(IDataReader reader)
        {
            List<CountryDetails> countries = new List<CountryDetails>();
            while (reader.Read())
            {
                countries.Add(GetCountryFromReader(reader));
            }
            return countries;
        }
        public abstract List<CountryDetails> GetCountries();
        public abstract CountryDetails GetCountry(int countryID);
        public abstract CountryDetails GetCountry(string countryName);
        public abstract CountryDetails GetCountryByDialCode(int dialCode);

    }
}
