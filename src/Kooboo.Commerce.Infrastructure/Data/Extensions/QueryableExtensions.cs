using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Kooboo.Commerce
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> query, string path)
            where T : class
        {
            return System.Data.Entity.QueryableExtensions.Include<T>(query, path);
        }

        public static IQueryable<T> Include<T, TProperty>(this IQueryable<T> query, Expression<Func<T, TProperty>> path)
            where T : class
        {
            return System.Data.Entity.QueryableExtensions.Include<T, TProperty>(query, path);
        }

        public static Pagination<T> Paginate<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            var total = query.Count();
            var items = query.Skip(pageIndex * pageSize)
                             .Take(pageSize)
                             .ToList();

            return new Pagination<T>(items, pageIndex, pageSize, total);
        }
    }
}
