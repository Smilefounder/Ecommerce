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
        where T : class, IItemResource, new()
        where Model : class, new()
    {
        protected IHalWrapper _halWrapper;
        protected IDictionary<string, object> _halParameters;
        protected IMapper<T, Model> _mapper;
        protected List<string> _includeComplexPropertyNames = new List<string>();

        public LocalCommerceQuery(IHalWrapper halWrapper, IMapper<T, Model> mapper)
        {
            _halWrapper = halWrapper;
            _mapper = mapper;
            _halParameters = new Dictionary<string, object>();
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
        protected virtual T Map(Model obj)
        {
            return _mapper.MapTo(obj, _includeComplexPropertyNames.ToArray());
        }
        /// <summary>
        /// this method will be called after query executed
        /// </summary>
        protected virtual void OnQueryExecuted()
        {
            _includeComplexPropertyNames.Clear();
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
            if (_halParameters != null)
            {
                foreach(var kvp in _halParameters)
                {
                    listHalParameters[kvp.Key] = kvp.Value;
                }
            }
            _halWrapper.AddLinks(resourceName, data, HalContext, listHalParameters, o => 
                {
                    var paras = itemHalParameterResolver(o);
                    if (_halParameters != null)
                    {
                        foreach (var kvp in _halParameters)
                        {
                            paras[kvp.Key] = kvp.Value;
                        }
                    }
                    return paras;
                });
        }

        public virtual void WrapHalLinks(T data, string resourceName, IDictionary<string, object> itemHalParameters)
        {
            resourceName = BuildResourceName(resourceName);
            if (_halParameters != null)
            {
                foreach (var kvp in _halParameters)
                {
                    itemHalParameters[kvp.Key] = kvp.Value;
                }
            }
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

        public ICommerceQuery<T> Include(string property)
        {
            _includeComplexPropertyNames.Add(property);
            return this;
        }
        public ICommerceQuery<T> Include<TProperty>(Expression<Func<T, TProperty>> property)
        {
            List<string> propNames = new List<string>();
            var express = property.Body;
            while(express.NodeType != ExpressionType.Parameter)
            {
                string propName = "";
                switch(express.NodeType)
                {
                    case ExpressionType.MemberAccess:
                        propName = ((MemberExpression)express).Member.Name;
                        propNames.Insert(0, propName);
                        express = ((MemberExpression)express).Expression;
                        break;
                    //case ExpressionType.Call:
                    //    var args = ((MethodCallExpression)express).Arguments;
                }
            }
            string expPropName = string.Join(".", propNames.ToArray());
            _includeComplexPropertyNames.Add(expPropName);
            return this;
        }

        //private string DecodeExpression(Expression expression, List<string> propNames)
        //{
        //    switch(expression.NodeType)
        //    {
        //        case ExpressionType.Parameter:
        //            break;
        //        case ExpressionType.MemberAccess:
        //            var propName = ((MemberExpression)expression).Member.Name;
        //            propNames.Insert(0, propName);
        //            expression = ((MemberExpression)expression).Expression;
        //            break;

        //    }
                
        //}

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
                
                listParas.Add("pageIndex", pageIndex);
                listParas.Add("pageSize", pageSize);
                listParas.Add("totalItemCount", totalItemCount);
                
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
            if (_includeHalLinks && obj != null)
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
        public ICommerceQuery<T> WithoutHalLinks()
        {
            _includeHalLinks = false;
            return this;
        }

        /// <summary>
        /// set hal parameter value
        /// </summary>
        /// <param name="name">hal parameter name</param>
        /// <param name="value">hal parameter value</param>
        public ICommerceQuery<T> SetHalParameter(string name, object value)
        {
            _halParameters[name] = value;
            return this;
        }

        public HalContext HalContext { get; set; }
    }
}
