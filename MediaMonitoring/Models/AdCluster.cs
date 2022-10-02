using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class AdCluster
    {
        public string Product { get; set; }
        public string Station { get; set; }
        public string Brand { get; set; }
        public int Sun { get; set; }
        public int Mon { get; set; }
        public int Tue { get; set; }
        public int Wed { get; set; }
        public int Thu { get; set; }
        public int Fri { get; set; }
        public int Sat { get; set; }
    }

    public class AdClusterPress
    {
        public string Product { get; set; }
        public string Publication { get; set; }
        public string Brand { get; set; }
        public int Sun { get; set; }
        public int Mon { get; set; }
        public int Tue { get; set; }
        public int Wed { get; set; }
        public int Thu { get; set; }
        public int Fri { get; set; }
        public int Sat { get; set; }
    }

    public class AdClusterOutdoor
    {
        public string Product { get; set; }
        public string State { get; set; }
        public string Brand { get; set; }
        public int Sun { get; set; }
        public int Mon { get; set; }
        public int Tue { get; set; }
        public int Wed { get; set; }
        public int Thu { get; set; }
        public int Fri { get; set; }
        public int Sat { get; set; }
    }

    public class AdClusterAllMedia
    {
        public string Product { get; set; }
        public string Medium { get; set; }
        public string Brand { get; set; }
        public string Station { get; set; }
        public string TimeBand { get; set; }
        public string Time { get; set; }
        public string PROGRAM { get; set; }
        public DateTime Addate { get; set; }
        public int Spots { get; set; }
        public double Duration { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Advertiser { get; set; }
        public string Quarter { get; set; }
    }
}
