using System;
using System.Collections.Generic;
using System.Text;

namespace VinorSoft.Tymy.Service.Model
{
   public class OrderDetailModel
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public double? UnitPrice { get; set; }
        public double? Quantity { get; set; }
        public double? Discount { get; set; }
        public bool Active { get; set; }
    }
}
