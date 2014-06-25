using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Paging;

namespace Kooboo.Commerce.Web.Mvc.Paging
{
    public static class QueryableExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> queryable, int? page, int? pageSize)
        {
            return PageLinqExtensions.ToPagedList(queryable, page ?? 1, pageSize ?? 50);
        }
    }
}