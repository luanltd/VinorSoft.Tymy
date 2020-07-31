using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VinorSoft.Tymy.Service.Constants;
using VinorSoft.Tymy.Service.Entities;
using VinorSoft.Tymy.Service.Interface;

namespace VinorSoft.Tymy.Service.Service
{
    public class TableService: ITableService
    {
        protected TymyDbContext _dbContext;
        public TableService(TymyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<Tables> GetAll()
        {
            return this._dbContext.Set<Tables>().Where(e => e.Active).ToList();
        }

        public IList<Tables> GetTableEmpty()
        {
            var orderTable = this._dbContext.Set<Orders>().Where(e =>e.Active && e.StatusId != (int)TymyConstants.Status.AddNew &&e.OrderDate.Value.Date==DateTime.Now.Date).Select(e=>e.TableId).Distinct().ToList();
            var tables = this._dbContext.Set<Tables>().Where(e => e.Active&&!orderTable.Contains(e.ID)).ToList();
            return tables;
        }
    }
}
