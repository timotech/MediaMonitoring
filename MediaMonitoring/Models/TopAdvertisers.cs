using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class TopAdvertisers
    {
        public string Advertiser { get; set; }
        public int Spots { get; set; }
        public double Value { get; set; }
    }

    public class TopAdvertisers2
    {
        public string FK_AdvertizerId { get; set; }
        public string Advertiser { get; set; }
        public int Spots { get; set; }
        public decimal Value { get; set; }
    }

    public class TopAdvertisers3
    {
        public string FK_AdvertizerId { get; set; }
        public string Advertiser { get; set; }
        public int Spots { get; set; }
        public double Value { get; set; }
    }

    public class TopAdvertisersDTO
    {
        public string Advertiser { get; set; }
        public string Value { get; set; }
    }

    public class TopCampaigns
    {
        public DateTime AdDate { get; set; }
        public string Campaign { get; set; }
        public string Station { get; set; }
        public double Duration { get; set; }
        public string Brand { get; set; }
        public string Medium { get; set; }
    }

    public class TopCampaignsViewModel
    {
        public string AdDate { get; set; }
        public string Campaign { get; set; }
        public string Station { get; set; }
        public double Duration { get; set; }
        public string Brand { get; set; }
        public string Medium { get; set; }
    }
}
