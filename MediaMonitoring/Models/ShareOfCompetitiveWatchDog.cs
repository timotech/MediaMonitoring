using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class ShareOfCompetitiveWatchDog
    {
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Station { get; set; }
        public string Time { get; set; }
        public string Programme { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Amount { get; set; }
        public string Quality { get; set; }
        public string Identifier { get; set; }
        public string Duration { get; set; }
        public string Day { get; set; }
    }

    public class ShareOfCompetitiveWatchDogPress
    {
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Publication { get; set; }
        public DateTime? IssueDate { get; set; }
        public string Day { get; set; }
        public string Position { get; set; }
        public decimal? Amount { get; set; }
        public string Quality { get; set; }
        public string Identifier { get; set; }
    }

    public class ShareOfCompetitiveWatchDogOutdoor
    {
        public string FK_AdvertizerId { get; set; }
        public string Contractor { get; set; }
        public string State { get; set; }
        public string Sheet { get; set; }
        public string Feature { get; set; }
        public string Region { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Location { get; set; }
        public string Town { get; set; }
        public DateTime? DateMonitored { get; set; }
        public decimal? Amount { get; set; }
        public string Quality { get; set; }
        public string Identifier { get; set; }
        public string Advertiser { get; set; }
        public string BoardNumber { get; set; }
    }
}
