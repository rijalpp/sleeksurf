using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.FrameWork.ApiObjects
{
    /// <summary>
    /// Used to load the data from http://freegeoip.net/json/14.202.217.85
    /// </summary>
    public class IpLocationInfo
    {
        public string ip { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region_code { get; set; }
        public string region_name { get; set; }
        public string city { get; set; }
        public string zip_code { get; set; }
        public string time_zone { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public string metro_code { get; set; }
    }
}
