using System;
using System.Collections.Generic;
using System.Text;
using VinorSoft.Tymy.Service.Entities;

namespace VinorSoft.Tymy.Service.Interface
{
  public interface ICustomerService
    {
        IList<Customers> GetCustomers(string search);
    }
}
