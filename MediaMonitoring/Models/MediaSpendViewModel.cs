using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class MediaSpendViewModel
    {
        public IEnumerable<MediaSpend> MediaSpends { get; set; }
        public IEnumerable<MediaSpendByBrand> MediaSpendByBrands { get; set; }
        public IEnumerable<MediaSpendByProduct> MediaSpendByProducts { get; set; }
    }
}
