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

        T Find(params object[] id);

        T Find(Expression<Func<T, bool>> predicate);

        IQueryable<T> Query();

        IQueryable<T> Query(Expression<Func<T, bool>> predicate);

        void Insert(T entity);

        void Update(T entity);

        void Update(T entity, object values);

        void Update(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> update);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> predicate);
    }
}
