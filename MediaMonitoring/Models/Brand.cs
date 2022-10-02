using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    [Table("tbl_Brand")]
    public class Brand
    {
        public long PKID { get; set; }
        public string FK_ProductId { get; set; }
        public string BrandId { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string FK_AdvertizerId { get; set; }
        public string PrgFlag { get; set; }
        public decimal? Med { get; set; }
    }
}
