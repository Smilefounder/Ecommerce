using Kooboo.Commerce.Api.Local.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local
{
    public abstract class QueryExecutorBase<T, TSource> : IQueryExecutor<T>
        where T : class
    {
        protected LocalApiContext ApiContext { get; private set; }

        protected QueryExecutorBase(LocalApiContext apiContext)
        {
            ApiContext = apiContext;
        }

        public int Count(Query<T> query)
        {
            return ToLocalQuery(query).Count();
        }

        public T FirstOrDefault(Query<T> query)
        {
            var item = ToLocalQuery(query).FirstOrDefault();
            return item == null ? null : Map(item, new IncludeCollection(query.Includes));
        }

        public List<T> ToList(Query<T> query)
        {
            var items = ToLocalQuery(query).ToList();
            var includes = new IncludeCollection(query.Includes);
            return items.Select(it => Map(it, includes)).ToList();
        }

        private IQueryable<TSource> ToLocalQuery(Query<T> query)
        {
            var localQuery = CreateLocalQuery();
            foreach (var filter in query.Filters)
            {
                localQuery = ApplyFilter(localQuery, filter);
            }

            localQuery = ApplySorts(localQuery, query.Sorts);

            if (query.Start > 0)
            {
                localQuery = localQuery.Skip(query.Start);
            }
            if (query.Limit > 0 && query.Limit != Int32.MaxValue)
            {
                localQuery = localQuery.Take(query.Limit);
            }

            return localQuery;
        }

        protected abstract IQueryable<TSource> CreateLocalQuery();

        protected abstract IQueryable<TSource> ApplyFilter(IQueryable<TSource> query, QueryFilter filter);

        protected virtual IQueryable<TSource> ApplySorts(IQueryable<TSource> query, IEnumerable<Sort> sorts)
        {
            var exp = String.Join(",", sorts.Select(it => it.Field + " " + it.Direction));
            return query.OrderBy(exp) as IQueryable<TSource>;
        }

        protected virtual T Map(TSource source, IncludeCollection includes)
        {
            return ObjectMapper.Map<TSource, T>(source, ApiContext, includes);
        }
    }
}
