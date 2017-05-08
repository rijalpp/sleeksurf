using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.FrameWork
{
    public class Result<T> where T : class
    {
        public List<T> EntityList { get; set; }
        public ResultStatus Status { get; set; }
        public string Message { get; set; }

        public Result()
        {
            EntityList = new List<T>();
            Status = ResultStatus.Fail;
            Message = string.Empty;
        }
    }

    public enum ResultStatus
    {
        Success = 1,
        Fail,
        FailUsername,
        FailPassword,
        UsernameExists,
        EmailExists,
        Inactive,
        NotFound,
        Error = -1
    }
}
