using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class TopBrandsInCategory
    {
        public string Brand { get; set; }
        public double Value { get; set; }
        public int Spots { get; set; }
        public string Style { get; set; }
        [NotMapped]
        public string Colors { get; set; }
    }
}
