using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    [Table("tbl_Product")]
    public class Product
    {
        public long PKID { get; set; }
        public string ProductId { get; set; }
        public string Description { get; set; }
        public string FK_CategoryId { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
