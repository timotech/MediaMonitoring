using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    [Table("tbl_BrandAD")]
    public class Identifier
    {
        [Key]
        public long PKID { get; set; }
        public string FK_ProductId { get; set; }
        public string FK_BrandId { get; set; }
        public string BrandADId { get; set; }
        public string Description { get; set; }
        public string FK_AdvertizerId { get; set; }
        public string Flag { get; set; }
        public string Promo { get; set; }
        public string Duration { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string FK_AgencyId { get; set; }
        public decimal? Med { get; set; }
        public string Status { get; set; }
    }
}
