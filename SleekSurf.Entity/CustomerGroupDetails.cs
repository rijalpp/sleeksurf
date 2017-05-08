using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
   public class CustomerGroupDetails
    {
        public string CustomerGroupID { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public bool Published { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ClientID { get; set; }
        public int CustomerCount { get; set; }
    }
}
