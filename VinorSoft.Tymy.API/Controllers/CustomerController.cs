using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using VinorSoft.Tymy.Service.Entities;
using VinorSoft.Tymy.Service.Interface;
using VinorSoft.Tymy.Service.Service;

namespace VinorSoft.Tymy.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CustomerController
    {
        private ICustomerService customerService;

        public CustomerController(IServiceProvider serviceProvider)
        {
            customerService= serviceProvider.GetRequiredService<ICustomerService>();
        }

        //[HttpGet]
        //[ActionName("GetCustomer")]

        //public IList<Customers> GetAllCustomers()
        //{
        //    IList<Customers> customers = new List<Customers>();
        //    try
        //    {
        //         customers = customerService.GetCustomers();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    return customers;
        //}
    }
}
