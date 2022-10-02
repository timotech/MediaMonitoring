using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class ShareOfExpenditure
    {
        public string Category { get; set; }
        public string Medium { get; set; }
        public string Brand { get; set; }
        public decimal Jan { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Apr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal Jul { get; set; }
        public decimal Aug { get; set; }
        public decimal Sep { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dec { get; set; }
        public decimal Total { get; set; }
        public decimal SOE { get; set; }
    }

    public class ShareOfExpeditureAllMedia
    {
        public string Category { get; set; }
        public string MonthDuration { get; set; }
        public string Brand { get; set; }
        public double TELE { get; set; }
        public double RADIO { get; set; }
        public double PRESS { get; set; }
        public double OUTDOOR { get; set; }
    }
}
