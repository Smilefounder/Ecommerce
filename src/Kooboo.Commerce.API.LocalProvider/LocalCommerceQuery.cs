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
        protected IMapper<T, Model> _mapper;

        protected abstract IQueryable<Model> CreateQuery();
        protected abstract IQueryable<Model> OrderByDefault(IQueryable<Model> query);

        protected virtual void EnsureQuery()
        {
            if (_query == null)
                _query = CreateQuery();
        }

        public virtual T[] Pagination(int pageIndex, int pageSize)
        {
            EnsureQuery();
            var objs = OrderByDefault(_query).Skip(pageIndex * pageSize).Take(pageSize).ToArray();
            return objs.Select(o => _mapper.MapTo(o)).ToArray();
        }

        public virtual T FirstOrDefault()
        {
            EnsureQuery();
            var obj = _query.FirstOrDefault();
            if (obj != null)
                return _mapper.MapTo(obj);
            return null;
        }

        public virtual T[] ToArray()
        {
            EnsureQuery();
            var objs = _query.ToArray();
            return objs.Select(o => _mapper.MapTo(o)).ToArray();
        }

        public virtual int Count()
        {
            EnsureQuery();
            return _query.Count();
        }
    }
}
