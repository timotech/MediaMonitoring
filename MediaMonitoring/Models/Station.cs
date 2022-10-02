using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    [Table("tbl_Stations")]
    public class Station
    {
        [Key]
        public long PKID { get; set; }
        public string StationId { get; set; }
        public string Description { get; set; }
        public string FK_StateId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string FK_AreaId { get; set; }
        public string FK_ZoneId { get; set; }
        public string FK_BranchId { get; set; }
        public decimal? MED { get; set; }
        public decimal? FK_GRPRate { get; set; }
        public string FK_TerrestrialCable { get; set; }
        public string STA { get; set; }
	}
}
