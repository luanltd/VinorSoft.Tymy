using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Text;
using VinorSoft.Tymy.Service.Entities;

namespace VinorSoft.Tymy.Service.Interface
{
   public interface IOrderService
    {
        IList<Orders> GetAll_User();
        IList<Orders> GetAllOrderDetail_User(DateTime? date);
        Orders GetById(string orderId);
        int Delete(string orderId);
        int Save(Orders orders);
        int Cancel(string orderId);
        int TotalOrder();
    }
}
