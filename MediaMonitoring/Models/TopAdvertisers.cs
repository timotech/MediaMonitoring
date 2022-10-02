using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class TopAdvertisers
    {
        public string Advertiser { get; set; }
        public int Spots { get; set; }
        public double Value { get; set; }
    }

    public class TopAdvertisersDTO
    {
        public string Advertiser { get; set; }
        public string Value { get; set; }
    }
}
