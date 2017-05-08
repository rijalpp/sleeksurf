using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    public class DataExtenderDetails
    {
        public int DataExtenderID { get; set; }
        public ClientDetails Client { get; set; }
        public string TermsAndConditions { get; set; }
        public string PrivacyAndPolicy { get; set; }
        public string Comments { get; set; }
    }
}
