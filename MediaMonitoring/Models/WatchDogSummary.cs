using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class WatchDogSummary
    {
        public string FK_AdvertizerId { get; set; }
        public string Advertizer { get; set; }
        public string Station { get; set; }
        public string Brand { get; set; }
        public int NumberofSpots { get; set; }
        public decimal Amount { get; set; }
        public string Zone { get; set; }
    }
}
