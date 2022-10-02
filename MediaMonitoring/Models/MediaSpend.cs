using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class MediaSpend
    {
        public string Advertiser { get; set; }
        public int Spots { get; set; }
        public double Value { get; set; }
    }
}
