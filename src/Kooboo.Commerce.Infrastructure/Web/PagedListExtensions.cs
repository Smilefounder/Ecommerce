using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web
{
    public static class PagedListExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> queryable, int? page, int? pageSize)
        {
            int pi = page ?? 0;
            int ps = pageSize ?? 50;
            int total = queryable.Count();
            IEnumerable<T> data = queryable.Skip(pi * ps).Take(ps).ToArray();
            return new PagedList<T>(data, pi, ps, total);
        }

        public static IPagedList<TResult> Transform<T, TResult>(this IPagedList<T> list, Func<T, TResult> transformer)
        {
            var data = list.Select(o => transformer(o));
            return new PagedList<TResult>(data, list.CurrentPageIndex, list.PageSize, list.TotalItemCount);
        }
    }
}
