using Kooboo.CMS.Membership.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.ShoppingCarts
{
    /// <summary>
    /// shopping cart query
    /// all query filter should return self(this) to support fluent api.
    /// </summary>
    public interface IShoppingCartQuery : ICommerceQuery<ShoppingCart>
    {
        /// <summary>
        /// add session id filter to query
        /// </summary>
        /// <param name="sessionId">session id</param>
        /// <returns>shopping cart query</returns>
        IShoppingCartQuery BySessionId(string sessionId);
        /// <summary>
        /// add account id filter to query
        /// </summary>
        /// <param name="accountId">account id</param>
        /// <returns>shopping cart query</returns>
        IShoppingCartQuery ByAccountId(string accountId);

        /// <summary>
        /// load shopping cart with customer info
        /// </summary>
        /// <returns>shopping cart query</returns>
        IShoppingCartQuery LoadWithCustomer();

        IShoppingCartQuery LoadWithBrands();

        IShoppingCartQuery LoadWithProductPrices();

        IShoppingCartQuery LoadWithProductImages();
    }
}
