using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using VinorSoft.Tymy.Service.Constants;
using VinorSoft.Tymy.Service.Model;

namespace VinorSoft.Tymy.Service.Entities
{
   public class Orders: DomainModel
    {
        [Required]
        public string ID { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? OrderNo { get; set; }
        [ForeignKey("Tables")]
        public string TableId { get; set; }
        public Tables Tables { get; set; }
        [ForeignKey("Customers")]
        public string CustomerId { get; set; }
        public Customers Customers { get; set; }

        [ForeignKey("Staffs")]
        public string StaffID { get; set; }
        public Staffs Staffs { get; set; }
        //nhan vien phuc vu
        public string SellerID { get; set; }
        public int? StatusId { get; set; }

        public string Note { get; set; }

        public Orders()
        {
            OrderDetails = new List<OrderDetails>();
            Notifications = new List<Notifications>();
        }
        public IList<OrderDetails> OrderDetails { get; set; }
        public IList<Notifications> Notifications { get; set; }

        [NotMapped]
        public double? TotalAmount
        {
            get
            {
                if (OrderDetails!=null&& OrderDetails.Count > 0)
                {
                    var total= OrderDetails.Sum(e => (e.UnitPrice.HasValue?e.UnitPrice.Value:0)  *  (e.Quantity.HasValue?e.Quantity.Value:0) - (e.Discount.HasValue?e.Discount.Value:0));
                    return total;
                }
                return 0;
            }
        }
        [NotMapped]
        public string StatusCode
        {
            get
            {
                if (StatusId.HasValue)
                {
                    switch (StatusId)
                    {
                        case (int)TymyConstants.Status.AddNew:
                            {
                                return "Thêm mới";
                            }
                        case (int)TymyConstants.Status.Pedding:
                            {
                                return "Đang xử lý";
                            }
                        case (int)TymyConstants.Status.Finish:
                            {
                                return "Hoàn tất";
                            }
                        default:
                            break;
                    }

                }
                return string.Empty;
            }
        }

        public bool Active { get; set; }

    }
}
