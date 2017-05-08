using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    [Serializable]
    public class CountryDetails
    {
        public int CountryID { get; set; }
        public int DialCode { get; set; }
        public string CountryName { get; set; }
        public string Description { get; set; }
    }
}
