using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API
{
    /// <summary>
    /// common data access interface for commerce objects
    /// </summary>
    /// <typeparam name="T">commerce object type</typeparam>
    public interface ICommerceAccess<T>
    {
        /// <summary>
        /// create the commerce object
        /// </summary>
        /// <param name="obj">commerce object</param>
        /// <returns>true if successfully created, else false</returns>
        bool Create(T obj);

        /// <summary>
        /// update the commerce object
        /// </summary>
        /// <param name="obj">commerce object</param>
        /// <returns>true if successfully created, else false</returns>
        bool Update(T obj);

        /// <summary>
        /// create/update the commerce object
        /// </summary>
        /// <param name="obj">commerce object</param>
        /// <returns>true if successfully created, else false</returns>
        bool Save(T obj);

        /// <summary>
        /// delete the commerce object
        /// </summary>
        /// <param name="obj">commerce object</param>
        /// <returns>true if successfully created, else false</returns>
        bool Delete(T obj);
    }
}
