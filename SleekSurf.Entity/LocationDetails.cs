using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    public class LocationDetails
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public string ZipPostalCode { get; set; }
        public TimezoneDetails Timezone { get; set; }
        public LatLongDetails Position { get; set; }
    }

    public class TimezoneDetails
    {
        public string TimezoneName { get; set; }
        public float Gmtoffset { get; set; }
        public bool Dstoffset { get; set; }
    }

    public class LatLongDetails
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }  
}
