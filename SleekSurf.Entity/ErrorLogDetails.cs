using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
   public class ErrorLogDetails
    {
        public int LogID { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorSource { get; set; }
        public string ErrorTargetSite { get; set; }
        public string ErrorStackTrace { get; set; }
        public bool ErrorSolved { get; set; }
    }
}
