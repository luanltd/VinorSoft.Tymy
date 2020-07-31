using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VinorSoft.Tymy.Service.Model;

namespace VinorSoft.Tymy.Service.Entities
{
    public class OrderDetails: DomainModel
    {
        [Required]
        [ForeignKey("Orders")]
        public string OrderId { get; set; }
        public Orders Orders { get; set; }
        [Required]
        [ForeignKey("Products")]
        public string ProductId { get; set; }
        public Products Products { get; set; }

        public double? UnitPrice { get; set; }
        public double? Quantity { get; set; }
        public double? Discount { get; set; }
        [NotMapped]
        public double? Amount
        {
            get
            {
                if (UnitPrice.HasValue && Quantity.HasValue)
                {
                    return UnitPrice.Value * Quantity.Value;
                }
                return 0;
            }
        }

        public bool Active { get; set; }

    }
}
