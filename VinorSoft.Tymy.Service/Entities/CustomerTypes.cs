using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VinorSoft.Tymy.Service.Model;

namespace VinorSoft.Tymy.Service.Entities
{
   public class CustomerTypes: DomainModel
    {
        [Required]
        public string ID { get; set; }
        public string Name { get; set; }
        [Required]
        public bool Active { get; set; }
    }
}
