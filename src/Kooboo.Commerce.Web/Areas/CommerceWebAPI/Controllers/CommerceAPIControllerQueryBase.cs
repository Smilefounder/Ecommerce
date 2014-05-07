using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
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
        /// </example>
        /// <returns>api objects</returns>
        [HttpGet]
        [Resource("all", itemName: "detail", uri: "/{instance}/{controller}", PropertyResourceProvider = typeof(DefaultPropertyResourceProvider))]
        public virtual IListResource<T> Get()
        {
            var query = BuildQueryFromQueryStrings();
            BuildHalParameters(query);
            return query.ToArray();
        }
        /// <summary>
        /// return paginated api objects filtered by query string parameters
        /// </summary>
        /// <param name="pageIndex">current page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>api objects</returns>
        [HttpGet]
        [Resource("list", itemName: "detail", uri: "/{instance}/{controller}/{action}?pageIndex={pageIndex}&pageSize={pageSize}", PropertyResourceProvider = typeof(DefaultPropertyResourceProvider), ImplicitLinksProvider = typeof(PaginationImplictLinksProvider))]
        public virtual IListResource<T> List(int pageIndex, int pageSize)
        {
            var query = BuildQueryFromQueryStrings();
            BuildHalParameters(query);
            var objs = query.Pagination(pageIndex, pageSize);
            return objs;
        }
        /// <summary>
        /// return api object filtered by query string parameters
        /// </summary>
        /// <returns>api object</returns>
        [HttpGet]
        [Resource("detail", uri: "/{instance}/{controller}/{id}", PropertyResourceProvider = typeof(DefaultPropertyResourceProvider))]
        public virtual T Get(int id)
        {
            var query = BuildQueryFromQueryStrings();
            BuildHalParameters(query);
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
            BuildHalParameters(query);
            return query.Count();
        }
        /// <summary>
        /// build the commerce query filters from query string.
        /// </summary>
        /// <returns>commerce query</returns>
        protected abstract ICommerceQuery<T> BuildQueryFromQueryStrings();

        /// <summary>
        /// build hal paramters from query string
        /// query string:
        /// includeHalLinks=false:  exclude the hal links in the return result.
        /// halParameters.xxx: set hal parameter value
        /// </summary>
        /// <param name="query"></param>
        protected virtual void BuildHalParameters(ICommerceQuery<T> query)
        {
            var qs = Request.RequestUri.ParseQueryString();
            if (qs["includeHalLinks"] == "false")
            {
                query.WithoutHalLinks();
            }
            else
            {
                var halParas = qs.AllKeys.Where(o => o.StartsWith("halParameters.")).Select(o => o.Replace("halParameters.", ""));
                if (halParas.Count() > 0)
                {
                    foreach (var key in halParas)
                    {
                        query.SetHalParameter(key, qs[key]);
                    }
                }
            }
        }
    }
}