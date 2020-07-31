using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VinorSoft.Tymy.Service.Constants;

namespace VinorSoft.Tymy.Service.Entities
{
   public class Notifications
    {
        [Required]
        public Guid ID { get; set; }
        [Required]
        [ForeignKey("Orders")]
        public string OrderId { get; set; }
        public Orders Orders { get; set; }
        public DateTime? Created { get; set; }
        public string StaffName { get; set; }
        public string SellerName { get; set; }
        public int? StatusId { get; set; }
        [Required]
        public bool IsRead { get; set; }
        [Required]
        public bool Active { get; set; }

        public string StaffId { get; set; }

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
    }
}
