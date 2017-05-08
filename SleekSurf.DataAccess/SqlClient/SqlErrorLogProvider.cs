using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SleekSurf.Entity;

namespace SleekSurf.DataAccess.SqlClient
{
    public class SqlErrorLogProvider:ErrorLogProvider
    {
        public override int InsertErrorLog(ErrorLogDetails errorLog)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spErrorLogInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@errorMessage", SqlDbType.NVarChar).Value = errorLog.ErrorMessage;
                cmd.Parameters.Add("@errorSource", SqlDbType.NVarChar).Value = errorLog.ErrorSource;
                cmd.Parameters.Add("@errorTargetSite", SqlDbType.NVarChar).Value = errorLog.ErrorTargetSite;
                cmd.Parameters.Add("@errorStackTrace", SqlDbType.NVarChar).Value = errorLog.ErrorStackTrace;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override List<ErrorLogDetails> SelectErrorLogs(bool solved, DateTime dateFrom, PagingDetails pgDetails)
        {
            List<ErrorLogDetails> errorList = new List<ErrorLogDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spErrorLogsSelectWithDateAndSolvedStatus", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@solved", SqlDbType.Bit).Value = solved;
                cmd.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgDetails.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgDetails.PageSize;
                cn.Open();
                errorList = GetErrorLogCollectionFromReader(ExecuteReader(cmd));
            }
            using(SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spErrorLogsCountWithDateAndSolvedStatus", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@solved", SqlDbType.Bit).Value = solved;
                cmd.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = dateFrom;
                cn1.Open();
                pgDetails.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return errorList;
        }

        public override ErrorLogDetails SelectErrorLog(int logID)
        {
           using(SqlConnection cn = new SqlConnection(this.ConnectionString))
           {
               SqlCommand cmd = new SqlCommand("spErrorLogSelect", cn);
               cmd.Parameters.Add("@logID", SqlDbType.Int).Value = logID;
               cn.Open();
               IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
               if (reader.Read())
                   return GetErrorLogFromReader(reader);
               else
                   return
                       null;


           }
        }

        public override int SetErrorLogSolvedStatus(int logID, bool solved)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spErrorLogSetSlovedStatus", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@logID", SqlDbType.Int).Value = logID;
                cmd.Parameters.Add("@solved", SqlDbType.Bit).Value = solved;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int DeleteErrorLog(int logID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spErrorLogDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@logID", SqlDbType.Int).Value = logID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }
    }
}
