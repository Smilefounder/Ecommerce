using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API
{
    public static class PageLinqExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> allItems, int pageIndex, int pageSize)
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
            var totalItemCount = allItems.Count();
            return new PagedList<T>(pageOfItems.ToArray(), pageIndex, pageSize, totalItemCount);
        }
        public static PagedList<T> ToPagedList<U, T>(this IQueryable<U> allItems, Func<U, T> converter, int pageIndex, int pageSize)
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
            var totalItemCount = allItems.Count();
            var data = pageOfItems.ToArray().Select(o => converter(o));
            return new PagedList<T>(data, pageIndex, pageSize, totalItemCount);
        }
    }

    /// <summary>
    /// add this class to allow the json data possibble to be deserialized to page list objects.
    /// </summary>
    public class PagedListWrapper<T>
    {

        public PagedListWrapper()
        {
        }

        public PagedListWrapper(IPagedList<T> pagedList)
        {
            FromPagedList(pagedList);
        }

        public void FromPagedList(IPagedList<T> pagedList)
        {
            this.Data = pagedList;
            this.CurrentPageIndex = pagedList.CurrentPageIndex;
            this.PageSize = pagedList.PageSize;
            this.TotalItemCount = pagedList.TotalItemCount;
        }

        public PagedList<T> ToPagedList()
        {
            return new PagedList<T>(Data, CurrentPageIndex, PageSize, TotalItemCount);
        }

        public IEnumerable<T> Data { get; set; }

        public int CurrentPageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
    }
}
