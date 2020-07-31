using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using VinorSoft.Tymy.Service.Model;

namespace VinorSoft.Tymy.Service.Interface
{
    public interface IDomainService1<E> where E : DomainModel
    {
        //IList<E> Get(Expression<Func<E, bool>>[] expressions);

    }
}
