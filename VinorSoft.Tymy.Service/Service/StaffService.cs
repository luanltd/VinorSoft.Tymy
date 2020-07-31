using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VinorSoft.Tymy.Service.Constants;
using VinorSoft.Tymy.Service.Entities;
using VinorSoft.Tymy.Service.Interface;

namespace VinorSoft.Tymy.Service.Service
{
    public class StaffService : IStaffService
    {
        protected TymyDbContext _dbContext;
        protected IConfiguration _config;
        public StaffService(TymyDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _config = configuration;
        }

        public bool Authencate(string userName, string password)
        {

            var user = this._dbContext.Set<Staffs>().Where(e => e.Active && e.ID == userName&&e.StaffTypeId!=TymyConstants.StaffType.StaffType_4.ToString()).FirstOrDefault();
            var token = string.Empty;
            if (user != null)
            {
                if (user.Password == password)
                {
                    return true;
                }
            }
            return false;

        }

        public Staffs GetUserInfo(string userName, string password)
        {
            return this._dbContext.Set<Staffs>().Where(e => e.Active && e.ID == userName && e.Password == password).FirstOrDefault();
        }
        //Tạo Token khi đăng nhập App
        public string GenerateToken(string username, string secret, int expireMinutes = 20)
        {
            var symmetricKey = Convert.FromBase64String(secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
               {
             new Claim(ClaimTypes.Name, username)
            }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }
        public IList<Staffs> GetAll()
        {
            return this._dbContext.Set<Staffs>().Where(e => e.Active).ToList();
        }

        public IList<Staffs> GetStaffService(string domainSearch)
        {
            return this._dbContext.Set<Staffs>().Where(e => e.Active &&e.StaffTypeId==TymyConstants.StaffType.StaffType_4.ToString() && (string.IsNullOrEmpty(domainSearch) || e.StaffName.ToLower().Contains(domainSearch.ToLower()) ||
          e.ID.ToLower().Contains(domainSearch.ToLower()))).ToList();
        }
        public Staffs GetById(string id)
        {
            return this._dbContext.Staffs.Find(id);
        }
    }
}
