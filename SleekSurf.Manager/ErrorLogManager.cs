using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SleekSurf.Entity;
using SleekSurf.FrameWork;
using SleekSurf.DataAccess;

namespace SleekSurf.Manager
{
    public class ErrorLogManager
    {
        public static int InsertErrorLog(ErrorLogDetails errorLog)
        {
            int i = 0;
            try
            {
                i = SiteProvider.ErrorLogs.InsertErrorLog(errorLog);
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
            }
            return i;
        }

        public static Result<ErrorLogDetails> SelectErrorLogs(bool solved, DateTime dateFrom, PagingDetails pgDetails)
        {
            Result<ErrorLogDetails> result = new Result<ErrorLogDetails>();
            try
            {
                result.EntityList = SiteProvider.ErrorLogs.SelectErrorLogs(solved, dateFrom, pgDetails);
                result.Status = ResultStatus.Success;
                result.Message = "Records are successfully retrieved.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
                Helpers.LogError(ex);
            }
            return result;
        }

        public static Result<ErrorLogDetails> SelectErrorLog(int logID)
        {
            Result<ErrorLogDetails> result = new Result<ErrorLogDetails>();
            try
            {
              result.EntityList.Add(SiteProvider.ErrorLogs.SelectErrorLog(logID));
              result.Status = ResultStatus.Success;
              result.Message = "The record is retrieved as follows";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
                Helpers.LogError(ex);
            }
            return result;
        }

        public static int SetErrorLogSolvedStatus(int logID, bool solved)
        {
            int i = 0;
            try
            {
                i = SiteProvider.ErrorLogs.SetErrorLogSolvedStatus(logID, solved);
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
            }
            return i;
        }

        public static int DeleteErrorLog(int logID)
        {
            int i = 0;
            try
            {
                i = SiteProvider.ErrorLogs.DeleteErrorLog(logID);
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
            }
            return i;
        }
    }
}
