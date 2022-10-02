using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class ShareOfVoice
    {
        public string Category { get; set; } 
        public string Medium { get; set; }
        public string Brand { get; set; }
        public int Jan { get; set; }
        public int Feb { get; set; }
        public int Mar { get; set; }
        public int Apr { get; set; }
        public int May { get; set; }
        public int Jun { get; set; }
        public int Jul { get; set; }
        public int Aug { get; set; }
        public int Sep { get; set; }
        public int Oct { get; set; }
        public int Nov { get; set; }
        public int Dec { get; set; }
        public int Total { get; set; }
        public decimal SOV { get; set; }
    }

    public class ShareOfVoiceAllMedia
    {
        public string Category { get; set; }
        public string MonthDuration { get; set; }
        public string Brand { get; set; }
        public int TELE { get; set; }
        public int RADIO { get; set; }
        public int PRESS { get; set; }
        public int OUTDOOR { get; set; }
    }
}
