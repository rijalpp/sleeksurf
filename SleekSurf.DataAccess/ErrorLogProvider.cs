using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SleekSurf.FrameWork;
using SleekSurf.Entity;
using System.Data;
using System.Data.Common;

namespace SleekSurf.DataAccess
{
    public abstract class ErrorLogProvider:DataAccess
    {
        static  ErrorLogProvider _instance = null;
        public static ErrorLogProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (ErrorLogProvider)Activator.CreateInstance(Type.GetType(Globals.Settings.ErrorLogForm.ProviderType));
                }

                return _instance;
            }
        }

        public ErrorLogProvider()
        {
            this.ConnectionString = Globals.Settings.ErrorLogForm.ConnectionString;
        }

        #region METHODS WORKING WITH THE ERROR LOG

        public abstract int InsertErrorLog(ErrorLogDetails errorLog);
        public abstract List<ErrorLogDetails> SelectErrorLogs(bool solved, DateTime dateFrom, PagingDetails pgDetails);
        public abstract ErrorLogDetails SelectErrorLog(int logID);
        public abstract int SetErrorLogSolvedStatus(int logID, bool solved);
        public abstract int DeleteErrorLog(int logID);

        protected virtual ErrorLogDetails GetErrorLogFromReader(IDataReader reader)
        {
            ErrorLogDetails errorLog = new ErrorLogDetails();
            errorLog.LogID = (int)reader["LogID"];
            errorLog.DateTimeStamp = (DateTime)reader["DateTimeStamp"];
            errorLog.ErrorMessage = reader["ErrorMessage"].ToString();
            errorLog.ErrorSource = reader["ErrorSource"].ToString();
            errorLog.ErrorTargetSite = reader["ErrorTargetSite"].ToString();
            errorLog.ErrorStackTrace = reader["ErrorStackTrace"].ToString();
            errorLog.ErrorSolved = (bool)reader["ErrorSolved"];
            return errorLog;
        }

        protected virtual List<ErrorLogDetails> GetErrorLogCollectionFromReader(IDataReader reader)
        {
            List<ErrorLogDetails> errorLogs = new List<ErrorLogDetails>();
            while (reader.Read())
                errorLogs.Add(GetErrorLogFromReader(reader));
            return errorLogs;
        }
        
        #endregion
    }
}
