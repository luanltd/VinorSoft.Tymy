using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VinorSoft.Tymy.Service.Entities;
using VinorSoft.Tymy.Service.Interface;

namespace VinorSoft.Tymy.Service.Service
{
  public  class ProductService: IProductService
    {
        protected TymyDbContext _dbContext;

        public ProductService(TymyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IList<Products> GetProducts(string domainSearch)
        {
            return this._dbContext.Set<Products>().Where(e => e.Active && (string.IsNullOrEmpty(domainSearch) || e.ProductName.ToLower().Contains(domainSearch.ToLower()) ||
          e.ID.ToLower().Contains(domainSearch.ToLower()))).ToList();
        }
    }
}
