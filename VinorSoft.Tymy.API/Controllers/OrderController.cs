using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using VinorSoft.Tymy.Service.Entities;
using VinorSoft.Tymy.Service.Interface;
using VinorSoft.Tymy.Service.Model;

namespace VinorSoft.Tymy.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class OrderController
    {
        private IOrderService orderService;
        private ICustomerService customerService;
        private IProductService productService;
        private ITableService tableService;
        private IStaffService staffService;
        private INotificationService notificationService;

        private readonly IMapper mapper;
        public OrderController(IServiceProvider serviceProvider, IMapper mapper)
        {
            orderService = serviceProvider.GetRequiredService<IOrderService>();
            customerService = serviceProvider.GetRequiredService<ICustomerService>();
            productService = serviceProvider.GetRequiredService<IProductService>();
            tableService = serviceProvider.GetRequiredService<ITableService>();
            staffService = serviceProvider.GetRequiredService<IStaffService>();
            notificationService = serviceProvider.GetRequiredService<INotificationService>();

            this.mapper = mapper;
        }

        //Chi tiết đơn hàng
        [HttpGet]
        [ActionName("GetOrder")]
        public ActionResult<KTAppDomainResult> GetOrderDetail(string orderId)
        {
            KTAppDomainResult appResult = new KTAppDomainResult();
            try
            {
                if (!string.IsNullOrEmpty(orderId))
                {
                    var order = orderService.GetById(orderId);
                    if (order != null)
                    {
                        appResult.Success = true;
                        appResult.Data = new
                        {
                            OrderList = order
                        };
                    }
                }
                else
                {
                    throw new Exception("OrderId là thông tin bắt buộc");
                }
             
            }
            catch (Exception ex)
            { 

                appResult.Success = false;
                appResult.ResultMessage = ex.Message;
            }
            return appResult;
        }
        //Danh sách khách hàng
        [HttpGet]
        [ActionName("GetCustomer")]
        public ActionResult<KTAppDomainResult> GetCustomer(string domainSearch)
        {
            KTAppDomainResult appResult = new KTAppDomainResult();
            try
            {
                var customers = customerService.GetCustomers(domainSearch);
                if (customers != null&&customers.Count>0)
                {
                    appResult.Success = true;
                    appResult.Data = new
                    {
                        CusomterList = customers
                    };
                }
            }
            catch (Exception ex)
            {

                appResult.Success = false;
                appResult.ResultMessage = ex.Message;
            }
            return appResult;
        }

        //Danh sách sản phẩm
        [HttpGet]
        [ActionName("GetProduct")]
        public ActionResult<KTAppDomainResult> GetProduct(string domainSearch)
        {
            KTAppDomainResult appResult = new KTAppDomainResult();
            try
            {
                var products = productService.GetProducts(domainSearch);
                if (products != null && products.Count > 0)
                {
                    appResult.Success = true;
                    appResult.Data = new
                    {
                        ProductList = products
                    };
                }
            }
            catch (Exception ex)
            {

                appResult.Success = false;
                appResult.ResultMessage = ex.Message;
            }
            return appResult;
        }

        //Danh sách bàn còn trống
        [HttpGet]
        [ActionName("GetTableEmpty")]
        public ActionResult<KTAppDomainResult> GetTableEmpty()
        {
            KTAppDomainResult appResult = new KTAppDomainResult();
            try
            {
                var tables = tableService.GetTableEmpty();
                if (tables != null && tables.Count > 0)
                {
                    appResult.Success = true;
                    appResult.Data = new
                    {
                        TableList = tables
                    };
                }
            }
            catch (Exception ex)
            {

                appResult.Success = false;
                appResult.ResultMessage = ex.Message;
            }
            return appResult;
        }


        //Danh sách nhân viên phục vụ
        [HttpGet]
        [ActionName("GetStaffService")]
        public ActionResult<KTAppDomainResult> GetStaffService(string domainSearch)
        {
            KTAppDomainResult appResult = new KTAppDomainResult();
            try
            {
                var staffs = staffService.GetStaffService(domainSearch);
                if (staffs != null && staffs.Count > 0)
                {
                    appResult.Success = true;
                    appResult.Data = new
                    {
                        StaffList = staffs
                    };
                }
            }
            catch (Exception ex)
            {

                appResult.Success = false;
                appResult.ResultMessage = ex.Message;
            }
            return appResult;
        }

        [HttpPost]
        [ActionName("SaveOder")]
        public ActionResult<KTAppDomainResult> Save([FromBody] OrderModel orderModel)
        {
            KTAppDomainResult appResult = new KTAppDomainResult();
            try
            {
                var order = mapper.Map<Orders>(orderModel);
                if (order != null)
                {
                    if (string.IsNullOrEmpty(order.ID))
                    {
                        var totalOrder = orderService.TotalOrder() + 1;
                        order.ID= $"{DateTime.Now.Year}-{totalOrder}";
                        order.OrderNo = totalOrder;
                    }
                    var result = orderService.Save(order);
                    if (result > 0)
                    {
                        appResult.Success = true;
                        order.OrderDetails = null;
                        appResult.Data = new
                        {
                            Order = order,
                        };
                        //Lưu thông báo
                        if (!string.IsNullOrEmpty(order.CustomerId))
                        {
                            var staffName = string.Empty;
                            var sellerName = string.Empty;
                            var staffId = string.Empty;
                            if (!string.IsNullOrEmpty(order.StaffID))
                            {
                                var staff = staffService.GetById(order.StaffID);
                                if (staff != null)
                                    staffName = staff.StaffName;
                                staffId = order.StaffID;
                                
                            }
                            if (!string.IsNullOrEmpty(order.SellerID))
                            {
                                var seller = staffService.GetById(order.SellerID);
                                if (seller != null)
                                    sellerName = seller.StaffName;
                            }
                            var notification = new Notifications();
                            notification.ID = Guid.NewGuid();
                            notification.IsRead = false;
                            notification.OrderId = order.ID;
                            notification.Active = true;
                            notification.StatusId = order.StatusId;
                            notification.SellerName = sellerName;
                            notification.StaffName = staffName; 
                            notification.StaffId = staffId;
                            notification.Created = DateTime.Now;
                            notificationService.Save(notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                appResult.Success = false;
                appResult.ResultMessage = ex.Message;
            }

            return appResult;
          
        }

        //Chi tiết đơn hàng
        [HttpGet]
        [ActionName("CancelOrder")]
        public ActionResult<KTAppDomainResult> CancelOrder(string orderId)
        {
            KTAppDomainResult appResult = new KTAppDomainResult();
            try
            {
                var order = orderService.Cancel(orderId);
                if (order>0)
                {
                    appResult.Success = true;

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
