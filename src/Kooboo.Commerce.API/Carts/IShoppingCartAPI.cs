using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Carts
{
    /// <summary>
    /// shopping cart api
    /// </summary>
    public interface IShoppingCartAPI : IShoppingCartQuery, IShoppingCartAccess
    {
        /// <summary>
        /// create shopping cart query
        /// </summary>
        /// <returns>shopping cart query</returns>
        IShoppingCartQuery Query();
        /// <summary>
        /// create shopping cart data access
        /// </summary>
        /// <returns>shopping cart data access</returns>
        IShoppingCartAccess Access();
    }
}
