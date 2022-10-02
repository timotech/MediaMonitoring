using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class MediaSpendByProduct
    {
        public string Product { get; set; }
        public int Spots { get; set; }
        public double Value { get; set; }
    }
}
