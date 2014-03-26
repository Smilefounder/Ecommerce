using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Products
{
    /// <summary>
    /// product api
    /// </summary>
    public interface IProductAPI : IProductQuery, IProductAccess
    {
        /// <summary>
        /// create product query
        /// </summary>
        /// <returns>product query</returns>
        IProductQuery Query();
        /// <summary>
        /// create product data access
        /// </summary>
        /// <returns>product data access</returns>
        IProductAccess Access();
    }
}
