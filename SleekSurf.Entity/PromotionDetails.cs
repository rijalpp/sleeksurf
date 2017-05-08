using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    public class PromotionDetails
    {
        public string PromotionID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte[] TitleImage { get; set; }
        public byte[] SupportingImage { get; set; }
        public bool IsActive { get; set; }
        public ClientDetails ClientID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
