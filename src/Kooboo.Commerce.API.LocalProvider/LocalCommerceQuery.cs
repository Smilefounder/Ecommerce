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

        public virtual T[] Pagination(int pageIndex, int pageSize)
        {
            CreateQuery();
            var query = OrderByDefault(_query).Select(o => _mapper.MapTo(o)).Skip(pageIndex * pageSize).Take(pageSize);
            return query.ToArray();
        }

        public virtual T FirstOrDefault()
        {
            CreateQuery();
            var query = _query.Select(o => _mapper.MapTo(o)).FirstOrDefault();
            return query;
        }

        public virtual T[] ToArray()
        {
            CreateQuery();
            var query = _query.Select(o => _mapper.MapTo(o));
            return query.ToArray();
        }

        public virtual int Count()
        {
            CreateQuery();
            return _query.Count();
        }


        public abstract void Create(T obj);

        public abstract void Update(T obj);

        public abstract void Delete(T obj);
    }
}
