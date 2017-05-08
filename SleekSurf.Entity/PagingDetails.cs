using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    public class PagingDetails
    {
        public int StartRowIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalNumber { get; set; }
        public string SearchMode { get; set; }
        public string SearchModeOption { get; set; }//used only in promotion
        public string SearchKey { get; set; }
    }
}
