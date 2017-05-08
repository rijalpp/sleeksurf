using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    public class ClientFeatureDetails
    {
        public ClientDetails Client { get; set; }
        public bool Listing { get; set; }
        public bool ClientProfile { get; set; }
        public bool ClientDomain { get; set; }
        public string Comments { get; set; }
    }
}
