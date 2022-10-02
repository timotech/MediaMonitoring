using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class WatchDogPress
    {
        public string FK_AdvertizerId { get; set; }
        public string Advertizer { get; set; }
        public string Brand { get; set; }
        public string Publication { get; set; }
        public DateTime? IssueDate { get; set; }
        public string Position { get; set; }
        public double? Rate { get; set; }
        public string Quality { get; set; }
        public string Identifier { get; set; }
    }
}
