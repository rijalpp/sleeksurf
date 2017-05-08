using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    public class MediaGalleryDetails
    {
        public string MediaID { get; set; }
        public string MediaUrl { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public string MediaType { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public PromotionDetails Promotion { get; set; }
        public EventDetails EventDetail { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
