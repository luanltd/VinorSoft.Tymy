using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VinorSoft.Tymy.Service.Model;

namespace VinorSoft.Tymy.Service.Entities
{
   public class Products: DomainModel
    {
        [Required]
        public string ID { get; set; }
        public string ProductName { get; set; }
        [Required]
        [ForeignKey("ProductGroups")]
        public string ProductGroupId { get; set; }
        public ProductGroups ProductGroups { get; set; }
        [Required]
        public bool Active { get; set; }
        public double? Price { get; set; }
    }
}
