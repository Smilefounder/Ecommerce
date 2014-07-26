using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using Kooboo.Commerce.Api;
using System.Collections.Specialized;

namespace Kooboo.Commerce.Web.Areas.CommerceWebAPI.Controllers
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
        /// <example>      
        /// </example>
        /// <returns>api objects</returns>
        [HttpGet]
        public virtual IList<T> Get()
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
        public virtual IList<T> List(int pageIndex, int pageSize)
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
        public virtual int Count()
        {
            var query = BuildQueryFromQueryStrings();
            return query.Count();
        }
        protected abstract ICommerceQuery<T> BuildQueryFromQueryStrings();
        /// <summary>
        /// build the commerce query filters from query string.
        /// </summary>
        /// <returns>commerce query</returns>
        protected virtual ICommerceQuery<T> BuildLoadWithFromQueryStrings(ICommerceQuery<T> query, NameValueCollection qs)
        {
            if (qs == null || query == null || qs.Count <= 0)
                return query;
            var loadWithProperties = qs.AllKeys.Where(o => o.ToLower().StartsWith("loadwith"));
            if (loadWithProperties != null && loadWithProperties.Count() > 0)
            {
                foreach (var property in loadWithProperties)
                {
                    if (qs[property] == "true")
                    {
                        query = query.Include(property.Substring("LoadWith".Length));
                    }
                }
            }
            return query;
        }
    }
}