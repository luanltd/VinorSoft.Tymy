using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VinorSoft.Tymy.Service.Model;

namespace VinorSoft.Tymy.Service.Entities
{
    public class Tables: DomainModel
    {
        [Required]
        public string ID { get; set; }
        public string TableName { get; set; }
        public string TableLocation { get; set; }
        [Required]
        public bool Active { get; set; }
    }
}
