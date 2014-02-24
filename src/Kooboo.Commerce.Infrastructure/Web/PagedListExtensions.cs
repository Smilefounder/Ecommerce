using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web
{
    public static class PagedListExtensions
    {
        public static IPagedList<TResult> Transform<T, TResult>(this IPagedList<T> list, Func<T, TResult> transformer)
        {
            var data = new List<TResult>();

            foreach (dynamic item in list)
            {
                data.Add(transformer(item));
            }

            return new PagedList<TResult>(data, list.CurrentPageIndex, list.PageSize, list.TotalItemCount);
        }
    }
}
