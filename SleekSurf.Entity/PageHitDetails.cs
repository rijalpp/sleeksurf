using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    public class PageHitDetails
    {
        public string PageHitID { get; set; }
        public ClientDetails Client { get; set; }
        public DateTime? DateTimeStamp { get; set; }
        public string HitType { get; set; }
        public string IPAddress { get; set; }
        public LocationDetails Location { get; set; }
        public string Comments { get; set; }
    }
}
