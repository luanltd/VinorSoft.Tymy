using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VinorSoft.Tymy.Service.Model;

namespace VinorSoft.Tymy.Service.Entities
{
   public class Customers: DomainModel
    {
        [Required]
        public string ID { get; set; }
        public string CustomerName { get; set; }
        [Required]
        [ForeignKey("CustomerTypes")]
        public string CustomerTypeId { get; set; }
        public CustomerTypes CustomerTypes { get; set; }
        public string Phone { get; set; }
        [Required]
        public bool Active { get; set; }

    }
}
