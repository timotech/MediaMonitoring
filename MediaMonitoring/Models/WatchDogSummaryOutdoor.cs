using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class WatchDogSummaryOutdoor
    {
        public string Advertizer { get; set; }
        public string State { get; set; }
        public string Town { get; set; }
        public string Location { get; set; }
        public string Brand { get; set; }
        public string Quality { get; set; }
        public int Total { get; set; }
        public int Good { get; set; }
        public int Defect { get; set; }
    }
}
