using Kooboo.Commerce.Data;
using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public static class PaginationExtensions
    {
        /// <summary>
        /// Transofrms to Kooboo IPagedList instance. Note that 'PageIndex' in IPagedList starts from one, not zero.
        /// </summary>
        public static IPagedList<T> ToPagedList<T>(this Pagination<T> pagination)
        {
            IEnumerable<T> items = pagination;
            return new PagedList<T>(items, pagination.PageIndex + 1, pagination.PageSize, pagination.TotalItems);
        }

        public static IPagedList ToPagedList(this Pagination pagination)
        {
            IEnumerable<object> items = pagination.OfType<object>();
            return new PagedList<object>(items, pagination.PageIndex + 1, pagination.PageSize, pagination.TotalItems);
        }
    }
}
