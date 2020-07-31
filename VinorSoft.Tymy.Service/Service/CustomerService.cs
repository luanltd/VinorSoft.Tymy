using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VinorSoft.Tymy.Service.Entities;
using VinorSoft.Tymy.Service.Interface;

namespace VinorSoft.Tymy.Service.Service
{
    public class CustomerService:ICustomerService
    {
        protected TymyDbContext _dbContext;

        public CustomerService(TymyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IList<Customers> GetCustomers(string domainSearch)
        {
           return this._dbContext.Set<Customers>().Where(e => e.Active&&(string.IsNullOrEmpty(domainSearch)|| e.CustomerName.ToLower().Contains(domainSearch.ToLower())||
           e.ID.ToLower().Contains(domainSearch.ToLower())||e.Phone.ToLower().Contains(domainSearch.ToLower()))).ToList();
        }
    }
}
