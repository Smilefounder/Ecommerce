using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Kooboo.Commerce.API;
using Kooboo.Commerce.API.HAL;

namespace Kooboo.Commerce.Web.Areas.CommerceWebAPI.Controllers
{
    /// <summary>
    /// commerce query api controller base
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CommerceAPIControllerQueryBase<T> : CommerceAPIControllerBase
        where T : IItemResource
    {
        /// <summary>
        /// return all api objects filtered by query string parameters
        /// </summary>
        /// <example>
        /// fdsafds
        /// dfsafdsd
        ///     dfadfsa
        ///         dfdsa
        ///         
        /// </example>
        /// <returns>api objects</returns>
        [HttpGet]
        [Resource("all", itemName: "detail", uri: "/{instance}/{controller}")]
        public virtual IListResource<T> Get()
        {
            var query = BuildQueryFromQueryStrings();
            return query.ToArray();
        }
        /// <summary>
        /// return paginated api objects filtered by query string parameters
        /// </summary>
        /// <param name="pageIndex">current page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>api objects</returns>
        [HttpGet]
        [Resource("list", itemName: "detail", uri: "/{instance}/{controller}/{action}?pageIndex={pageIndex}&pageSize={pageSize}&{rel}={rel}")]
        public virtual IListResource<T> Pagination(int pageIndex, int pageSize)
        {
            var query = BuildQueryFromQueryStrings();
            var objs = query.Pagination(pageIndex, pageSize);
            return objs;
        }
        /// <summary>
        /// return api object filtered by query string parameters
        /// </summary>
        /// <returns>api object</returns>
        [HttpGet]
        [Resource("detail", uri: "/{instance}/{controller}/{id}")]
        public virtual T Get(int id)
        {
            var query = BuildQueryFromQueryStrings();
            return query.FirstOrDefault();
        }
        /// <summary>
        /// return total count of the api objects matches the query filters
        /// </summary>
        /// <returns>total count</returns>
        [HttpGet]
        [Resource("count")]
        public virtual int Count()
        {
            var query = BuildQueryFromQueryStrings();
            return query.Count();
        }
        /// <summary>
        /// build the commerce query filters from query string.
        /// </summary>
        /// <returns>commerce query</returns>
        protected abstract ICommerceQuery<T> BuildQueryFromQueryStrings();
  }
}