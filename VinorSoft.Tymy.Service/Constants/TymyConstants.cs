using System;
using System.Collections.Generic;
using System.Text;

namespace VinorSoft.Tymy.Service.Constants
{
   public class TymyConstants
    {
        public enum StaffType
        {
            StaffType_1 = 1,//admin
            StaffType_2 = 2,//quan ly
            StaffType_3 = 3,//ke toan
            StaffType_4 = 4,//nhan vien phuc vu
        }

        public enum Status
        {
            AddNew=1,
            Pedding=2,
            Finish=3,
        }
    }
}
