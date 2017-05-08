using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    [Serializable]
    public class CategoryDetails
    {
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] CategoryImage { get; set; }
        public string Comments { get; set; }
    }
}
