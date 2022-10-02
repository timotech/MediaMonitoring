using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class WatchDogOutdoor
    {
        public string FK_AdvertizerId { get; set; }
        public string Advertizer { get; set; }
        public string Contractor { get; set; }
        public string Brand { get; set; }
        public string State { get; set; }
        public string Town { get; set; }
        public string Location { get; set; }
        public DateTime? DateCaptured { get; set; }
        public string Identifier { get; set; }
        public string Quality { get; set; }
        public string Sheet { get; set; }
        public double? AMOUNT { get; set; }
        public string BoardNumber { get; set; }
        public string Feature { get; set; }
        public string Zone { get; set; }
    }
}
