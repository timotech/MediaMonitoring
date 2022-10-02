using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class ReconciliationViewModel
    {
        public IEnumerable<Reconciliation> Reconciliations { get; set; }
        public IEnumerable<ReconciliationSummary> ReconciliationSummaries { get; set; }
    }
}
