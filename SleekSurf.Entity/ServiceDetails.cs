using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    public class ServiceDetails
    {
        public string ServiceID { get; set; }
        public string ServiceDescription { get; set; }
        public ClientDetails Client { get; set; }
    }
}
