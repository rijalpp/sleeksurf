using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SleekSurf.Entity;

namespace SleekSurf.DataAccess.SqlClient
{
    class SqlCountryProvider : CountryProvider
    {
        public override List<CountryDetails> GetCountries()
        {
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCountrySelectAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                return GetCountryCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override CountryDetails GetCountry(int countryID)
        {
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spSelectCountryByCountryID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@countryID", SqlDbType.Int).Value = countryID;
                conn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetCountryFromReader(reader);
                else
                    return null;
            }
        }

        public override CountryDetails GetCountry(string countryName)
        {
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCountrySelectCountryByName", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@countryName", SqlDbType.Int).Value = countryName;
                conn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetCountryFromReader(reader);
                else
                    return null;
            }
        }

        public override CountryDetails GetCountryByDialCode(int dialCode)
        {
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spCountrySelectByDailCode", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@dialCode", SqlDbType.Int).Value = dialCode;
                conn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetCountryFromReader(reader);
                else
                    return null;
            }
        }
    }
}
