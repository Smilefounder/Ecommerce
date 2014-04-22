using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Kooboo.Commerce.API.HAL;
using System.Web;

namespace Kooboo.Commerce.API.LocalProvider
{
    /// <summary>
    /// local commerce query base class
    /// </summary>
    /// <typeparam name="T">api object type</typeparam>
    /// <typeparam name="Model">entity type</typeparam>
    public abstract class LocalCommerceQuery<T, Model> : IHalContextAware, ICommerceQuery<T>
        where T : IItemResource
        where Model : class, new()
    {
        protected IHalWrapper _halWrapper;

        public LocalCommerceQuery(IHalWrapper halWrapper)
        {
            _halWrapper = halWrapper;
        }

        /// <summary>
        /// entity query for build up fluent api filters
        /// </summary>
        protected IQueryable<Model> _query;
        /// <summary>
        /// include hal links in the result.
        /// </summary>
        protected bool _includeHalLinks = true;
        /// <summary>
        /// create entity query
        /// </summary>
        /// <returns>queryable object</returns>
        protected abstract IQueryable<Model> CreateQuery();
        /// <summary>
        /// use the default order when pagination the query
        /// </summary>
        /// <param name="query">pagination query</param>
        /// <returns>ordered query</returns>
        protected abstract IQueryable<Model> OrderByDefault(IQueryable<Model> query);
        /// <summary>
        /// map the entity to object
        /// </summary>
        /// <param name="obj">entity</param>
        /// <returns>object</returns>
        protected abstract T Map(Model obj);
        /// <summary>
        /// this method will be called after query executed
        /// </summary>
        protected virtual void OnQueryExecuted()
        {
            if (_includeHalLinks)
            {
                // todo: wrap the result with hal links.
            }
            _includeHalLinks = true;
        }

        protected virtual string BuildResourceName(string resourceName)
        {
            return string.Format("{0}:{1}", typeof(T).Name, resourceName).ToLower();
        }

        protected void AppendHalContextParameters(IDictionary<string, object> parameters)
        {
            parameters.Add("instance", HalContext.CommerceInstance);
            parameters.Add("language", HalContext.Language);
            parameters.Add("currency", HalContext.Currency);
        }

        protected virtual IDictionary<string, object> BuildListHalParameters(IListResource<T> data)
        {
            var paras = new Dictionary<string, object>();
            return paras;
        }

        protected virtual IDictionary<string, object> BuildItemHalParameters(T data)
        {
            var paras = new Dictionary<string, object>();
            var propertyInfo = typeof(T).GetProperty("Id");
            if (propertyInfo != null)
            {
                var id = propertyInfo.GetValue(data, null);
                paras.Add("id", id);
            }
            return paras;
        }

        public virtual void WrapHalLinks(IListResource<T> data, string resourceName, IDictionary<string, object> listHalParameters, Func<T, IDictionary<string, object>> itemHalParameterResolver)
        {
            resourceName = BuildResourceName(resourceName);
            _halWrapper.AddLinks(resourceName, data, HalContext, listHalParameters, o => itemHalParameterResolver(o));
        }

        public virtual void WrapHalLinks(T data, string resourceName, IDictionary<string, object> itemHalParameters)
        {
            resourceName = BuildResourceName(resourceName);
            _halWrapper.AddLinks(resourceName, data, HalContext, itemHalParameters);
        }
        /// <summary>
        /// ensure the query is not null
        /// </summary>
        protected virtual void EnsureQuery()
        {
            if (_query == null)
                _query = CreateQuery();
        }
        /// <summary>
        /// get paginated data that matches the query
        /// </summary>
        /// <param name="pageIndex">current page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>paginated data</returns>
        public virtual IListResource<T> Pagination(int pageIndex, int pageSize)
        {
            EnsureQuery();
            var query = OrderByDefault(_query);
            var objs = query.Skip(pageIndex * pageSize).Take(pageSize).ToArray();
            var mobjs = new ListResource<T>(objs.Select(o => Map(o)).ToArray());
            OnQueryExecuted();
            if (_includeHalLinks)
            {
                var listParas = BuildListHalParameters(mobjs);
                AppendHalContextParameters(listParas);
                Func<T, IDictionary<string, object>> itemParasResolver = o =>
                {
                    var itemParas = BuildItemHalParameters(o);
                    AppendHalContextParameters(itemParas);
                    return itemParas;
                };
                int totalItemCount = _query.Count();
                int totalPageCount = Convert.ToInt32(Math.Ceiling((double)totalItemCount / pageSize));
                listParas.Add("page", pageIndex);
                listParas.Add("pageSize", pageSize);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["prev"]))
                    listParas["page"] = pageIndex - 1 <= 0 ? 0 : pageIndex - 1;
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["next"]))
                    listParas["page"] = pageIndex + 1 > totalPageCount - 1 ? totalPageCount - 1 : pageIndex + 1;
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["goto"]))
                    listParas["page"] = HttpContext.Current.Request.QueryString["goto"];
                listParas.Add("pageIndex", listParas["page"]);
                WrapHalLinks(mobjs, "list", listParas, itemParasResolver);
            }
            return mobjs;
        }
        /// <summary>
        /// get the first object that matches the query, if not matched returns null
        /// </summary>
        /// <returns>object</returns>
        public virtual T FirstOrDefault()
        {
            EnsureQuery();
            var obj = _query.FirstOrDefault();
            T mobj = default(T);
            if (obj != null)
                mobj = Map(obj);
            OnQueryExecuted();
            if (_includeHalLinks)
            {
                var itemParas = BuildItemHalParameters(mobj);
                AppendHalContextParameters(itemParas);
                WrapHalLinks(mobj, "detail", itemParas);
            }
            return mobj;
        }
        /// <summary>
        /// get all objects that matches the query
        /// </summary>
        /// <returns>objects</returns>
        public virtual IListResource<T> ToArray()
        {
            EnsureQuery();
            var objs = _query.ToArray();
            var mobjs = new ListResource<T>(objs.Select(o => Map(o)).ToArray());
            OnQueryExecuted();
            if (_includeHalLinks)
            {
                var listParas = BuildListHalParameters(mobjs);
                AppendHalContextParameters(listParas);
                Func<T, IDictionary<string, object>> itemParasResolver = o =>
                {
                    var itemParas = BuildItemHalParameters(o);
                    AppendHalContextParameters(itemParas);
                    return itemParas;
                };
                WrapHalLinks(mobjs, "all", listParas, itemParasResolver);
            }
            return mobjs;
        }
        /// <summary>
        /// get total hit count that matches the query
        /// </summary>
        /// <returns>total count</returns>
        public virtual int Count()
        {
            EnsureQuery();
            int count = _query.Count();
            return count;
        }
        /// <summary>
        /// query data without requesting hal links to save time
        /// </summary>
        public void WithoutHalLinks()
        {
            _includeHalLinks = false;
        }

        public HalContext HalContext { get; set; }
    }
}
