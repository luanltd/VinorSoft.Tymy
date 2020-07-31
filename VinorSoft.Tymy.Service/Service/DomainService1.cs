using AutoMapper;
using KTApps.Core;
using KTApps.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using VinorSoft.Tymy.Service.Interface;
using VinorSoft.Tymy.Service.Model;

namespace VinorSoft.Tymy.Service.Service
{
    public class DomainService1<E> : IDomainService1<E> where E : DomainModel
    {

        protected readonly IUnitOfWork unitOfWork;
        protected readonly IMapper mapper;
        protected readonly TymyDbContext dbContext;
        public DomainService1(IUnitOfWork unitOfWork
          , IMapper mapper
          , TymyDbContext dbContext
          )
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        //public IList<E> Get(Expression<Func<E, bool>>[] expressions)
        //{
        //    var queryable = dbContext.Set<E>().Where(expressions).ToList();
        //    return queryable;
        //}
    }
}
