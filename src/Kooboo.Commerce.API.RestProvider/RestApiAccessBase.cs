using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider
{
    /// <summary>
    /// rest api data access base class
    /// </summary>
    /// <typeparam name="T">api object type</typeparam>
    public class RestApiAccessBase<T> : RestApiQueryBase<T>, ICommerceAccess<T>
    {
        /// <summary>
        /// create object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public virtual bool Create(T obj)
        {
            return Post<bool>(null, obj);
        }

        /// <summary>
        /// update object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public virtual bool Update(T obj)
        {
            return Put<bool>(null, obj);
        }

        /// <summary>
        /// create/update object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public virtual bool Save(T obj)
        {
            return Post<bool>(null, obj);
        }

        /// <summary>
        /// delete object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public virtual bool Delete(T obj)
        {
            return Delete<bool>(null, obj);
        }
    }
}
