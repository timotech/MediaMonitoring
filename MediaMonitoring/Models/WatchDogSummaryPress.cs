using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class WatchDogSummaryPress
    {
        public string FK_AdvertizerId { get; set; }
        public string Advertizer { get; set; }
        public string Brand { get; set; }
        public string Publication { get; set; }
        public string Position { get; set; }
        public decimal Amount { get; set; }
        public int NumberOfSpots { get; set; }
    }
}
