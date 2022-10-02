using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class WatchDogViewModel
    {
        public IEnumerable<WatchDog> WatchDogs { get; set; }
        public IEnumerable<WatchDogRadio> WatchDogRadios { get; set; }
        public IEnumerable<WatchDogOutdoor> WatchDogOutdoors { get; set; }
        public IEnumerable<WatchDogPress> WatchDogPresses { get; set; }
        public IEnumerable<WatchDogSummary> WatchDogSummaries { get; set; }
        public IEnumerable<WatchDogSummaryPress> WatchDogSummaryPresses { get; set; }
        public IEnumerable<WatchDogSummaryOutdoor> WatchDogSummaryOutdoors { get; set; }
        public IEnumerable<WatchDogStationAudit> WatchDogStationAudits { get; set; }
        public IEnumerable<WatchDogStationAuditRadio> WatchDogStationAuditRadios { get; set; }
    }
}
