using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class AdClusterByTimeSegment
    {
        public string Category { get; set; }
        public string Product { get; set; }
        public string Station { get; set; }
        public string Brand { get; set; }
        public int ZeroToSix { get; set; }
        public int SixToNine { get; set; }
        public int NineToTwelve { get; set; }
        public int TwelveToThree { get; set; }
        public int ThreeToSeven { get; set; }
        public int SevenToTen { get; set; }
        public int TenToTwelve { get; set; }
    }

    public class AdClusterByTimeSegmentPress
    {
        public string Product { get; set; }
        public string Location { get; set; }
        public string Publication { get; set; }
        public string Brand { get; set; }
        public string PrintSize { get; set; }
        public DateTime AdDate { get; set; }
        public int Spots { get; set; }
        public decimal Spend { get; set; }
        public string Identifier { get; set; }
        public string Advertiser { get; set; }
    }

    public class AdClusterByTimeSegmentOutdoor
    {
        public string Product { get; set; }
        public string Location { get; set; }
        public string BoardSize { get; set; }
        public string Brand { get; set; }
        public DateTime AdDate { get; set; }
        public int Spots { get; set; }
        public decimal Spend { get; set; }
        public string Identifier { get; set; }
        public string Advertiser { get; set; }
    }
}
