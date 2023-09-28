using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class MediaSpendMix
    {
        public string Category { get; set; }
        public string Advertizer { get; set; }
        public string AdvertizerId { get; set; }
        public string Brand { get; set; }
        public double TELE { get; set; }
        public double RADIO { get; set; }
        public double PRESS { get; set; }
        public double OUTDOOR { get; set; }
        public double Total { get; set; }
    }
}
