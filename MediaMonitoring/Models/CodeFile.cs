using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    [Table("tbl_CodeFile")]
    public class CodeFile
    {
        public long PKID { get; set; }
        public string codefileid { get; set; }
        public string Description { get; set; }
        public string fk_stateid { get; set; }
        public string fk_areaid { get; set; }
        public string fk_branchid { get; set; }
        public string fk_zoneid { get; set; }
        public bool? isactive { get; set; }
        public DateTime? dateupdated { get; set; }
        public decimal? med { get; set; }
	}
}
