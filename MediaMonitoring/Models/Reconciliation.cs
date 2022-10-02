using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class Reconciliation
    {
        public DateTime AdDate { get; set; }
        public int CapturedSpots { get; set; }
        public int ScheduledSpots { get; set; }
        public string Program { get; set; }
        public string Stations { get; set; }
        public string ScheduledTime { get; set; }
        public string CapturedTime { get; set; }
        public int CapturedExtra { get; set; }
        public string Brand { get; set; }
        public string Identifier { get; set; }
        public string Client { get; set; }
        public string Status { get; set; }
        public Decimal Spend { get; set; }
        public int ExtraSpots { get; set; }
    }

    public class ReconciliationSummary
    {
        //Stations, Program, NumberBooked, NumberCaptured, case when Position = 1 then UnScheduled else 0 End as UnScheduled, ScheduledTime, Brand, Identifier, Zone, DateSpec, ExtraSpots
        public string Program { get; set; }
        public string Stations { get; set; }
        public int NumberBooked { get; set; }
        public int NumberCaptured { get; set; }
        public int UnScheduled { get; set; }
        public string ScheduledTime { get; set; }
        public string Brand { get; set; }
        public string Identifier { get; set; }
        public string Zone { get; set; }
        public string DateSpec { get; set; }
        public int ExtraSpots { get; set; }
    }
}
