using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Kooboo.Commerce.API.LocalProvider
{
    /// <summary>
    /// local commerce query base class
    /// </summary>
    /// <typeparam name="T">api object type</typeparam>
    /// <typeparam name="Model">entity type</typeparam>
    public abstract class LocalCommerceQuery<T, Model> : ICommerceQuery<T>
        where T : class, new()
        where Model : class, new()
    {
        /// <summary>
        /// entity query for build up fluent api filters
        /// </summary>
        protected IQueryable<Model> _query;
        /// <summary>
        /// create entity query
        /// </summary>
        /// <returns>queryable object</returns>
        protected abstract IQueryable<Model> CreateQuery();
        /// <summary>
        /// use the default order when pagination the query
        /// </summary>
        /// <param name="query">pagination query</param>
        /// <returns>ordered query</returns>
        protected abstract IQueryable<Model> OrderByDefault(IQueryable<Model> query);
        /// <summary>
        /// map the entity to object
        /// </summary>
        /// <param name="obj">entity</param>
        /// <returns>object</returns>
        protected abstract T Map(Model obj);
        /// <summary>
        /// this method will be called after query executed
        /// </summary>
        protected virtual void OnQueryExecuted()
        { }
        /// <summary>
        /// ensure the query is not null
        /// </summary>
        protected virtual void EnsureQuery()
        {
            if (_query == null)
                _query = CreateQuery();
        }
        /// <summary>
        /// get paginated data that matches the query
        /// </summary>
        /// <param name="pageIndex">current page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>paginated data</returns>
        public virtual T[] Pagination(int pageIndex, int pageSize)
        {
            EnsureQuery();
            var objs = OrderByDefault(_query).Skip(pageIndex * pageSize).Take(pageSize).ToArray();
            var mobjs = objs.Select(o => Map(o)).ToArray();
            OnQueryExecuted();
            return mobjs;
        }
        /// <summary>
        /// get the first object that matches the query, if not matched returns null
        /// </summary>
        /// <returns>object</returns>
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
        /// <summary>
        /// get all objects that matches the query
        /// </summary>
        /// <returns>objects</returns>
        public virtual T[] ToArray()
        {
            EnsureQuery();
            var objs = _query.ToArray();
            var mobjs = objs.Select(o => Map(o)).ToArray();
            OnQueryExecuted();
            return mobjs;
        }
        /// <summary>
        /// get total hit count that matches the query
        /// </summary>
        /// <returns>total count</returns>
        public virtual int Count()
        {
            EnsureQuery();
            int count = _query.Count();
            return count;
        }
    }
}
