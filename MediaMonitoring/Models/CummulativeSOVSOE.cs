using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class CummulativeSOVSOE
    {
        public int MonthValue { get; set; }
        public string Month { get; set; }
        //public string Category { get; set; }
        public string Brand { get; set; }
        public string Advertizer { get; set; }
        public string Media { get; set; }
        public double TotalSpend { get; set; }
        public string Region { get; set; }
        public string Station { get; set; }
        public int Spots { get; set; }
        public string Quarter { get; set; }
        public int Duration { get; set; }
        //public double GRP { get; set; }
    }
}
