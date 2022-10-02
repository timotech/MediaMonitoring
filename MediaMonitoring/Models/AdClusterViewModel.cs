using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class AdClusterViewModel
    {
        public IEnumerable<AdCluster> AdClusters { get; set; }
        public IEnumerable<AdClusterPress> AdClusterPresses { get; set; }
        public IEnumerable<AdClusterOutdoor> AdClusterOutdoors { get; set; }
        public IEnumerable<AdClusterByMonth> AdClusterByMonths { get; set; }
        public IEnumerable<AdClusterByMonthPress> AdClusterByMonthPresses { get; set; }
        public IEnumerable<AdClusterByMonthOutdoor> AdClusterByMonthOutdoors { get; set; }
        public IEnumerable<AdClusterByTimeSegment> AdClusterByTimeSegments { get; set; }
        public IEnumerable<AdClusterByTimeSegmentPress> AdClusterByTimeSegmentPresses { get; set; }
        public IEnumerable<AdClusterByTimeSegmentOutdoor> AdClusterByTimeSegmentOutdoors { get; set; }
        public IEnumerable<AdClusterByWeek> AdClusterByWeeks { get; set; }
        public IEnumerable<AdClusterByWeekPress> AdClusterByWeekPresses { get; set; }
        public IEnumerable<AdClusterByWeekOutdoor> AdClusterByWeekOutdoors { get; set; }
        public IEnumerable<AdClusterByMaterialLength> AdClusterByMaterialLengths { get; set; }
        public IEnumerable<AdClusterByMaterialLengthPress> AdClusterByMaterialLengthPresses { get; set; }
        public IEnumerable<AdClusterByMaterialLengthOutdoor> AdClusterByMaterialLengthOutdoors { get; set; }
        public IEnumerable<AdClusterAllMedia> AdClusterAllMedias { get; set; }
    }
}
