using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    [Serializable]
    public class FAQGroupDetails
    {
        public string FaqGroupID { get; set; }
        public ClientDetails Client { get; set; }
        public string GroupName { get; set; }
        public int GroupRank { get; set; }
        public string Comments { get; set; }
    }
}
