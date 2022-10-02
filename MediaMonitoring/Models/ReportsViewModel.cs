using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class ReportsViewModel
    {
        public string Operation { get; set; }
        [Required(ErrorMessage ="Please Select Begin Date")]
        public DateTime BeginDate { get; set; }
        [Required(ErrorMessage = "Please Select End Date")]
        public DateTime EndDate { get; set; }
        public string MediaType { get; set; }
        [Required(ErrorMessage = "Please Select A Region Or All Regions")]
        public string Zone { get; set; }
        public string Brands { get; set; }
        public string Identifiers { get; set; }
        public string Stations { get; set; }
        public string Filter { get; set; }
        [NotMapped]
        public IFormFile fleUploadExcel { get; set; }
        public string ExcelPath { get; set; }
    }
}
