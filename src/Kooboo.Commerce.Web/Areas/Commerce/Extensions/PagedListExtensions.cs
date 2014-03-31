using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web
{
    public static class PagedListExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> queryable, int? pageNumber, int? pageSize)
        {
            pageNumber = pageNumber ?? 1;
            pageSize = pageSize ?? 50;

            var total = queryable.Count();
            var data = queryable.Skip((pageNumber.Value - 1) * pageSize.Value)
                                .Take(pageSize.Value)
                                .ToArray();

            // In Kooboo CMS, the pageIndex starts from one.
            // But in Commerce we want to follow the normal convention that "index" always starts from zero.
            // But this PagedList is used by the Kooboo UI Framework, 
            // so we follow CMS's meaning of the "pageIndex" in the returned PagedList.
            return new PagedList<T>(data, pageNumber.Value, pageSize.Value, total);
        }

        public static IPagedList<TResult> Transform<T, TResult>(this IPagedList<T> list, Func<T, TResult> transformer)
        {
            var data = list.Select(o => transformer(o));
            return new PagedList<TResult>(data, list.CurrentPageIndex, list.PageSize, list.TotalItemCount);
        }
    }
}
