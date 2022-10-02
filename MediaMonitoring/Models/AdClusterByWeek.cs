using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class AdClusterByWeek
    {
        public string Brand { get; set; }
        public int Week { get; set; }
        public int Spots { get; set; }
        public string Station { get; set; }
        public string Category { get; set; }
    }

    public class AdClusterByWeekPress
    {
        public string Brand { get; set; }
        public int Week { get; set; }
        public int Spots { get; set; }
        public string Publication { get; set; }
        public string Category { get; set; }
    }

    public class AdClusterByWeekOutdoor
    {
        public string Brand { get; set; }
        public int Week { get; set; }
        public int Spots { get; set; }
        public string State { get; set; }
        public string Town { get; set; }
        public string Category { get; set; }
    }
}
