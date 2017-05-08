using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    public class AdvertisementDetails
    {
        public string AdID { get; set; }
        public string AdName { get; set; }
        public string Advertiser { get; set; }
        public string ContactDetail { get; set; }
        public string ImageUrl { get; set; }
        public string NavigateUrl { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal AmountPaid { get; set; }
        public string DisplayPosition { get; set; }
        public int FitToPanel { get; set; }
        public bool Published { get; set; }
        public string ClientID { get; set; }
        public string Comments { get; set; }
        public string Email { get; set; }
        public int ExpiryNotice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
