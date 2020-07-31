using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VinorSoft.Tymy.Service.Model;

namespace VinorSoft.Tymy.Service.Entities
{
    public class Staffs: DomainModel
    {
        public string ID { get; set; }
        public string StaffName { get; set; }
        [ForeignKey("StaffTypes")]
        public string StaffTypeId { get; set; }
        public StaffTypes StaffTypes { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
    }
}
