using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using VinorSoft.Tymy.Service.Constants;
using VinorSoft.Tymy.Service.Interface;
using VinorSoft.Tymy.Service.Model;

namespace VinorSoft.Tymy.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DashboardController
    {
        private ITableService tableService;
        private IOrderService orderService;
        private INotificationService notificationService;
        public DashboardController(IServiceProvider serviceProvider)
        {
            tableService = serviceProvider.GetRequiredService<ITableService>();
            orderService = serviceProvider.GetRequiredService<IOrderService>();
            notificationService = serviceProvider.GetRequiredService<INotificationService>();

        }
        //Số đơn hàng trong ngày
        [HttpGet]
        [ActionName("TotalOrderToDay")]
        public ActionResult<KTAppDomainResult> TotalOrderToDay()
        {
            KTAppDomainResult appResult = new KTAppDomainResult();
            try
            {
                var totalOrder = orderService.GetAll_User().Where(e => e.OrderDate.Value.Date == DateTime.Now.Date).Count();
                appResult.Success = true;
                appResult.Data = new
                {
                    TotalOrder = totalOrder > 0 ? totalOrder : 0,
                };
            }
            catch (Exception ex)
            {
                appResult.Success = false;
                appResult.ResultMessage = ex.Message;
            }
            return appResult;
        }
        //Tổng số bàn còn trống
        [HttpGet]
        [ActionName("TotalTableEmpty")]
        public ActionResult<KTAppDomainResult> TableEmpty()
        {
            KTAppDomainResult appResult = new KTAppDomainResult();
            try
            {
                //var t = tableService.GetAll().Where(e => e.Active).Count();
                //var totalOrder = orderService.GetAll().Where(e =>e.OrderDate.Value.Date==DateTime.Now.Date&& e.StatusId != (int)TymyConstants.Status.AddNew).Count();
                var tables = tableService.GetTableEmpty();
                var totalTable = 0;
                if (tables != null && tables.Count > 0)
                {
                    totalTable = tables.Count();
                }
                appResult.Success = true;
                appResult.Data = new
                {
                    TotalTableEmpty = totalTable,
                };
            }
            catch (Exception ex)
            {
                appResult.Success = false;
                appResult.ResultMessage = ex.Message;
            }
            return appResult;
        }
        //lấy lên danh sách đơn hàng trong ngày (2 đơn)
        [HttpGet]
        [ActionName("Get2Order")]
        public ActionResult<KTAppDomainResult> Get2Order()
        {
            KTAppDomainResult appResult = new KTAppDomainResult();
            try
            {
                var orderList = orderService.GetAllOrderDetail_User(null).OrderByDescending(e => e.OrderDate).Take(2).ToList();
                var totalOrder = orderService.GetAll_User().Count();
                var isShowMore = false;
                if (totalOrder > 2)
                {
                    isShowMore = true;
                }
                appResult.Success = true;
                appResult.Data = new
                {
                    Orders = orderList,
                    IsShowMore = isShowMore,
                };
            }
            catch (Exception ex)
            {
                appResult.Success = false;
                appResult.ResultMessage = ex.Message;
            }
            return appResult;
        }

        //lấy lên danh sách đơn hàng trong ngày theo User
        [HttpGet]
        [ActionName("GetAllOrder")]
        public ActionResult<KTAppDomainResult> GetAllOrder(string date)
        {
            KTAppDomainResult appResult = new KTAppDomainResult();
            try
            {
                var dateSearch = DateTime.Now.Date;
                if (!string.IsNullOrEmpty(date))
                {
                    dateSearch = DateTime.Parse(date).Date;
                }
                var orderList = orderService.GetAllOrderDetail_User(dateSearch).OrderByDescending(e => e.OrderDate).ToList();
                appResult.Success = true;
                appResult.Data = new
                {
                    Orders = orderList,
                };
            }
            catch (Exception ex)
            {
                appResult.Success = false;
                appResult.ResultMessage = ex.Message;
            }
            return appResult;
        }
        //lấy số lượng thông báo chưa đọc
        [HttpGet]
        [ActionName("TotalNotificationNotRead")]
        public ActionResult<KTAppDomainResult> GetTotalNotificationNotRead()
        {
            KTAppDomainResult appResult = new KTAppDomainResult();
            try
            {
                var notifications = notificationService.GetNotificationNotRead();
                var totalNotification = 0;
                if (notifications != null && notifications.Count > 0)
                {
                    totalNotification = notifications.Count();
                }
                appResult.Success = true;
                appResult.Data = new
                {
                    TotalNotificationNotRead = totalNotification,
                };
            }
            catch (Exception ex)
            {
                appResult.Success = false;
                appResult.ResultMessage = ex.Message;
            }
            return appResult;
        }

        //Lấy danh sách notification
        [HttpGet]
        [ActionName("GetNotificationList")]
        public ActionResult<KTAppDomainResult> GetNotification()
        {
            KTAppDomainResult appResult = new KTAppDomainResult();
            try
            {
                var notifications = notificationService.GetNotifications();
                appResult.Success = true;
                appResult.Data = new
                {
                    NotificationList = notifications,
                };
            }
            catch (Exception ex)
            {
                appResult.Success = false;
                appResult.ResultMessage = ex.Message;
            }
            return appResult;
        }

        //Cập nhập trạng thái đã đọc
        [HttpGet]
        [ActionName("UpdateNotification")]
        public ActionResult<KTAppDomainResult> UpdateNotification(Guid notificationId)
        {
            KTAppDomainResult appResult = new KTAppDomainResult();
            try
            {
                if(notificationId!=null&&notificationId != Guid.Empty)
                {
                    var result = notificationService.UpdateIsRead(notificationId);
                    if (result > 0)
                    {
                        appResult.Success = true;

                    }
                }
                else
                {
                    throw new Exception("NotificationId là thông tin bắt buộc");
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
