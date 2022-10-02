using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class AdClusterByMaterialLength
    {
        public string Category { get; set; }
        public string Product { get; set; }
        public string Station { get; set; }
        public string Brand { get; set; }
        public int FiveSecs { get; set; }
        public int TenSecs { get; set; }
        public int FifteenSecs { get; set; }
        public int ThirtySecs { get; set; }
        public int FortyFiveSecs { get; set; }
        public int SixtySecs { get; set; }
        public int OneMin30Sec { get; set; }
        public int Others { get; set; }
    }

    public class AdClusterByMaterialLengthPress
    {
        public string Category { get; set; }
        public string Product { get; set; }
        public string Publication { get; set; }
        public string Brand { get; set; }
        public int NumberofOccurence { get; set; }
    }

    public class AdClusterByMaterialLengthOutdoor
    {
        public string Category { get; set; }
        public string Product { get; set; }
        public string State { get; set; }
        public string Brand { get; set; }
        public int NumberofOccurence { get; set; }
        public string Town { get; set; }
        public string Location { get; set; }
    }
}
