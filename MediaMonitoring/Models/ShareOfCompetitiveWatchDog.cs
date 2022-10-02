using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class ShareOfCompetitiveWatchDog
    {
        public string Product { get; set; }
        public string Medium { get; set; }
        public string Brand { get; set; }
        public string Location { get; set; }
        public string Station { get; set; }
        public string TimeBand { get; set; }
        public string Time { get; set; }
        public string PROGRAM { get; set; }
        public DateTime Addate { get; set; }
        public int Spots { get; set; }
        public decimal Spend { get; set; }
        public string Identifier { get; set; }
        public int Duration { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Language { get; set; }
        public string Advertiser { get; set; }
        public decimal GrossSpend { get; set; }
        public string Quarter { get; set; }
    }
}
