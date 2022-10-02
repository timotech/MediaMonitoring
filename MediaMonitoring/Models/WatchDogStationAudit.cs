using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class WatchDogStationAudit
    {
        public string AdvertizerId { get; set; }
        public string Program { get; set; }
        public string Brand { get; set; }
        public string Day { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Duration { get; set; }
        public string Quality { get; set; }
        public string Identifier { get; set; }
        public string Advertizer { get; set; }
        public string Station { get; set; }
        public string Zone { get; set; }
        public decimal? Amount { get; set; }
        public string State { get; set; }
    }

    public class WatchDogStationAuditRadio
    {
        public string AdvertizerId { get; set; }
        public string Program { get; set; }
        public string Brand { get; set; }
        public string Day { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public double? Duration { get; set; }
        public string Quality { get; set; }
        public string Identifier { get; set; }
        public string Advertizer { get; set; }
        public string Station { get; set; }
        public string Zone { get; set; }
        public decimal? Amount { get; set; }
        public string State { get; set; }
    }
}
