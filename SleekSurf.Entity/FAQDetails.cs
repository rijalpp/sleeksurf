using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    [Serializable]
    public class FAQDetails
    {
        public string FaqID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public FAQGroupDetails FaqGroup { get; set; }
        public int FaqRank { get; set; }
        public string Comments { get; set; }
    }
}
