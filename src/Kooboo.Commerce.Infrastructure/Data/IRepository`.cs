using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public interface IRepository<T> where T : class
    {
        ICommerceDatabase Database { get; }

        T Find(params object[] ids);

        T Find(Expression<Func<T, bool>> predicate);

        IQueryable<T> Query();

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
