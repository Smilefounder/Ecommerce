using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API
{
    /// <summary>
    /// page linq extensions
    /// </summary>
    public static class PageLinqExtensions
    {
        /// <summary>
        /// execute query by pagination
        /// </summary>
        /// <typeparam name="T">commerce object</typeparam>
        /// <param name="allItems">the original query</param>
        /// <param name="pageIndex">current page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>paged query result</returns>
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> allItems, int pageIndex, int pageSize)
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
            var totalItemCount = allItems.Count();
            return new PagedList<T>(pageOfItems.ToArray(), pageIndex, pageSize, totalItemCount);
        }
        /// <summary>
        /// execute the query and convert the result items into another commerce object by pagination
        /// </summary>
        /// <typeparam name="U">original commerce object</typeparam>
        /// <typeparam name="T">mapped to the result object</typeparam>
        /// <param name="allItems">the original query</param>
        /// <param name="converter">converter to convert the original object to result object</param>
        /// <param name="pageIndex">current page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>paged query result</returns>
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
        /// <summary>
        /// construct the wrapper from paged list
        /// </summary>
        /// <param name="pagedList">paged list</param>
        public PagedListWrapper(IPagedList<T> pagedList)
        {
            FromPagedList(pagedList);
        }
        /// <summary>
        /// initialize the paging info from paged list
        /// </summary>
        /// <param name="pagedList">paged list</param>
        public void FromPagedList(IPagedList<T> pagedList)
        {
            this.Data = pagedList;
            this.CurrentPageIndex = pagedList.CurrentPageIndex;
            this.PageSize = pagedList.PageSize;
            this.TotalItemCount = pagedList.TotalItemCount;
        }
        /// <summary>
        /// convert this wrapper to paged list
        /// </summary>
        /// <returns>paged list</returns>
        public PagedList<T> ToPagedList()
        {
            return new PagedList<T>(Data, CurrentPageIndex, PageSize, TotalItemCount);
        }
        /// <summary>
        /// result data
        /// </summary>
        public IEnumerable<T> Data { get; set; }
        /// <summary>
        /// current page index
        /// </summary>
        public int CurrentPageIndex { get; set; }
        /// <summary>
        /// page size
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// total result record counts
        /// </summary>
        public int TotalItemCount { get; set; }
    }
}
