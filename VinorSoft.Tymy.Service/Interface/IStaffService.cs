using System;
using System.Collections.Generic;
using System.Text;
using VinorSoft.Tymy.Service.Entities;

namespace VinorSoft.Tymy.Service.Interface
{
    public interface IStaffService
    {
        /// <summary>
        /// Xác thực thông tin login nếu thành công thì trả về token và userinfo
        /// Thất bại trả về thông báo
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool Authencate(string userName, string password);
        string GenerateToken(string username, string secret, int expireMinutes);
        Staffs GetUserInfo(string userName, string password);
        IList<Staffs> GetAll();
        IList<Staffs> GetStaffService(string domainSearch);
        Staffs GetById(string id);
    }
}
