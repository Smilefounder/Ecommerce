using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Kooboo.Commerce.API.LocalProvider
{
    public abstract class LocalCommerceQuery<T, Model> : ICommerceQuery<T>
    {
        protected IQueryable<Model> _query;
        protected IMapper<T, Model> _mapper;

        protected abstract IQueryable<Model> CreateQuery();
        protected abstract IQueryable<Model> OrderByDefault(IQueryable<Model> query);

        protected Expression<Func<Model, T>> Select()
        {
            return o => _mapper.MapTo(o);
        }

        public T[] Pagination(int pageIndex, int pageSize)
        {
            var query = OrderByDefault(_query).Select(o => _mapper.MapTo(o)).Skip(pageIndex * pageSize).Take(pageSize);
            return query.ToArray();
        }

        public T FirstOrDefault()
        {
            var query = _query.Select(o => _mapper.MapTo(o)).FirstOrDefault();
            return query;
        }

        public T[] ToArray()
        {
            var query = _query.Select(o => _mapper.MapTo(o));
            return query.ToArray();
        }

        public int Count()
        {
            return _query.Count();
        }
    }
}
