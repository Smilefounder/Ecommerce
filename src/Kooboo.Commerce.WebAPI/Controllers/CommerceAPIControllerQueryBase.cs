using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Kooboo.Commerce.API;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    /// <summary>
    /// commerce query api controller base
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CommerceAPIControllerQueryBase<T> : CommerceAPIControllerBase
    {
        /// <summary>
        /// return all api objects filtered by query string parameters
        /// </summary>
        /// <returns>api objects</returns>
        [HttpGet]
        public virtual IEnumerable<T> Get()
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
        public virtual IEnumerable<T> Pagination(int pageIndex, int pageSize)
        {
            var query = BuildQueryFromQueryStrings();
            return query.Pagination(pageIndex, pageSize);
        }
        /// <summary>
        /// return api object filtered by query string parameters
        /// </summary>
        /// <returns>api object</returns>
        [HttpGet]
        public virtual T First()
        {
            var query = BuildQueryFromQueryStrings();
            return query.FirstOrDefault();
        }
        /// <summary>
        /// return total count of the api objects matches the query filters
        /// </summary>
        /// <returns>total count</returns>
        [HttpGet]
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