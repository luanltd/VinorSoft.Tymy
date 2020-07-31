using System;
using System.Collections.Generic;
using System.Text;
using VinorSoft.Tymy.Service.Entities;

namespace VinorSoft.Tymy.Service.Interface
{
   public interface ITableService
    {
        IList<Tables> GetAll();
        IList<Tables> GetTableEmpty();
    }
}
