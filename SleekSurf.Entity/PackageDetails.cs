using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    public class PackageDetails
    {
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public bool Published { get; set; }
        public string Status { get; set; }
        public string FeatureType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
