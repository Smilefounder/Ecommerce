using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider
{
    /// <summary>
    /// request api query base blass
    /// </summary>
    /// <typeparam name="T">api object type</typeparam>
    public class RestApiQueryBase<T> : RestApiBase, ICommerceQuery<T>
    {
        /// <summary>
        /// get paginated data that matches the query
        /// </summary>
        /// <param name="pageIndex">current page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>paginated data</returns>
        public virtual T[] Pagination(int pageIndex, int pageSize)
        {
            QueryParameters.Add("pageIndex", pageIndex.ToString());
            QueryParameters.Add("pageSize", pageSize.ToString());
            return Get<T[]>("Pagination");
        }

        /// <summary>
        /// get the first object that matches the query, if not matched returns null
        /// </summary>
        /// <returns>object</returns>
        public virtual T FirstOrDefault()
        {
            return Get<T>("First");
        }

        /// <summary>
        /// get all objects that matches the query
        /// </summary>
        /// <returns>objects</returns>
        public virtual T[] ToArray()
        {
            return Get<T[]>(null);
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
    }
}
