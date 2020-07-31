using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VinorSoft.Tymy.Service.Constants;
using VinorSoft.Tymy.Service.Interface;
using VinorSoft.Tymy.Service.Model;

namespace VinorSoft.Tymy.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    // [Description("Quản lý thông tin người dùng")]
    [Authorize]
    public class AuthController
    {
        private IStaffService staffService;
        private readonly AppSettings _appSettings;

        public AuthController(IServiceProvider serviceProvider)
        {
            staffService = serviceProvider.GetRequiredService<IStaffService>();
            _appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;

        }

        [HttpGet]
        [ActionName("Authenticate")]
        [AllowAnonymous]
        public ActionResult<KTAppDomainResult> Authenticate(string username, string password)
        {
            KTAppDomainResult appResult = new KTAppDomainResult();

            try
            {
                var user = this.staffService.GetAll().Where(e => e.Active && e.ID.ToLower() == username.ToLower() && e.StaffTypeId != TymyConstants.StaffType.StaffType_4.ToString()).FirstOrDefault();
                if (user != null)
                {
                    if (!string.IsNullOrEmpty(user.Password) && user.Password == password)
                    {
                        appResult.Success = true;
                        appResult.Data = new
                        {
                            Token = staffService.GenerateToken(username, _appSettings.Secret, _appSettings.Expired),
                            UserInfo = staffService.GetUserInfo(username, password),
                        };

                        LoginContext.Instance.SetCurrentUser(new UserModel()
                        {
                            UserId = user.ID,
                            UserName = user.StaffName,
                            Password = user.Password,
                        });
                    }
                    else
                    {
                        appResult.Success = false;
                        appResult.ResultCode = 2;
                        throw new Exception("Mật khẩu sai.");
                    }
                }
                else
                {
                    appResult.Success = false;
                    appResult.ResultCode = 1;
                    throw new Exception("Tài khoản không tồn tài.");
                }
            }
            catch (Exception ex)
            {
                appResult.Success = false;
                appResult.ResultMessage = ex.Message;
            }
            return appResult;
        }

    }
}
