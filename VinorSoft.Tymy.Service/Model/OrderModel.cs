using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VinorSoft.Tymy.Service.Entities;

namespace VinorSoft.Tymy.Service.Model
{
    public class OrderModel
    {
        public string ID { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? OrderNo { get; set; }
        public string TableId { get; set; }
        public string CustomerId { get; set; }
        public string StaffID { get; set; }
        //nhan vien phuc vu
        public string SellerID { get; set; }
        public int? StatusId { get; set; }
        public string Note { get; set; }
        public bool Active { get; set; }
        public OrderModel()
        {
            OrderDetails = new List<OrderDetailModel>();
        }
        public IList<OrderDetailModel> OrderDetails { get; set; }

    }
}
