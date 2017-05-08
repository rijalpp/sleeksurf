using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    public class PackageOptionDetails
    {
        public int PackageOptionID { get; set; }
        public string PackageCode { get; set; }
        public string Duration { get; set; }
        public decimal StandardPrice { get; set; }
        public double DiscountPercentage { get; set; }
        public string PromoCode { get; set; }
        public DateTime? PromoCodeStartDate { get; set; }
        public DateTime? PromoCodeEndDate { get; set; }
        public bool Published { get; set; }
        public decimal FinalPrice { get; set; }
        public bool PromoCodeEntered { get; set; }
        public string Comments { get; set; }
    }
}
