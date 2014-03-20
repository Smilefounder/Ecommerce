using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Kooboo.Commerce.API.LocalProvider
{
    public abstract class LocalCommerceQuery<T, Model> : ICommerceQuery<T>
        where T : class, new()
        where Model : class, new()
    {
        protected IQueryable<Model> _query;

        protected abstract IQueryable<Model> CreateQuery();
        protected abstract IQueryable<Model> OrderByDefault(IQueryable<Model> query);
        protected abstract T Map(Model obj);

        protected virtual void OnQueryExecuted()
        { }

        protected virtual void EnsureQuery()
        {
            if (_query == null)
                _query = CreateQuery();
        }

        public virtual T[] Pagination(int pageIndex, int pageSize)
        {
            EnsureQuery();
            var objs = OrderByDefault(_query).Skip(pageIndex * pageSize).Take(pageSize).ToArray();
            var mobjs = objs.Select(o => Map(o)).ToArray();
            OnQueryExecuted();
            return mobjs;
        }

        public virtual T FirstOrDefault()
        {
            EnsureQuery();
            var obj = _query.FirstOrDefault();
            T mobj = null;
            if (obj != null)
                mobj = Map(obj);
            OnQueryExecuted();
            return mobj;
        }

        public virtual T[] ToArray()
        {
            EnsureQuery();
            var objs = _query.ToArray();
            var mobjs = objs.Select(o => Map(o)).ToArray();
            OnQueryExecuted();
            return mobjs;
        }

        public virtual int Count()
        {
            EnsureQuery();
            int count = _query.Count();
            return count;
        }
    }
}
