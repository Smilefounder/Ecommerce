using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Kooboo.Commerce.Api.RestProvider
{
    /// <summary>
    /// request api query base blass
    /// </summary>
    /// <typeparam name="T">api object type</typeparam>
    public class RestApiQueryBase<T> : RestApiBase, ICommerceQuery<T>
    {
        public RestApiQueryBase()
        {
            ContentType = "application/hal+json";
            Accept = "application/hal+json;application/json";
        }

        public ICommerceQuery<T> Include(string property)
        {
            QueryParameters.Add("LoadWith" + property, "true");
            return this;
        }

        public ICommerceQuery<T> Include<TProperty>(Expression<Func<T, TProperty>> property)
        {
            List<string> propNames = new List<string>();
            var express = property.Body;
            while (express.NodeType != ExpressionType.Parameter)
            {
                string propName = "";
                switch (express.NodeType)
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
            QueryParameters.Add("LoadWith" + expPropName, "true");
            return this;
        }

        /// <summary>
        /// get paginated data that matches the query
        /// </summary>
        /// <param name="pageIndex">current page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>paginated data</returns>
        public virtual IList<T> Pagination(int pageIndex, int pageSize)
        {
            QueryParameters.Add("pageIndex", pageIndex.ToString());
            QueryParameters.Add("pageSize", pageSize.ToString());
            return Get<List<T>>("List");
        }

        /// <summary>
        /// get the first object that matches the query, if not matched returns null
        /// </summary>
        /// <returns>object</returns>
        public virtual T FirstOrDefault()
        {
            return Get<T>(null);
        }

        /// <summary>
        /// get all objects that matches the query
        /// </summary>
        /// <returns>objects</returns>
        public virtual IList<T> ToArray()
        {
            return Get<List<T>>(null);
        }

        /// <summary>
        /// get total hit count that matches the query
        /// </summary>
        /// <returns>total count</returns>
        public virtual int Count()
        {
            return Get<int>("Count");
        }
        /// <summary>
        /// get the api controller name, default is the entity type name
        /// </summary>
        protected override string ApiControllerPath
        {
            get
            {
                return typeof(T).Name;
            }
        }


        public ICommerceQuery<T> WithoutHalLinks()
        {
            //QueryParameters.Add("includeHalLinks", "false");
            ContentType = "application/json";
            Accept = "application/json";
            return this;
        }

        public ICommerceQuery<T> SetHalParameter(string name, object value)
        {
            QueryParameters.Add(string.Format("halParameters.{0}", name), value == null ? "" : value.ToString());;
            return this;
        }
    }
}
