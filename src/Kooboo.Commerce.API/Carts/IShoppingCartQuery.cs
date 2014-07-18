using Kooboo.CMS.Membership.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Carts
{
    /// <summary>
    /// shopping cart query
    /// all query filter should return self(this) to support fluent api.
    /// </summary>
    public interface IShoppingCartQuery : ICommerceQuery<ShoppingCart>
    {
        IShoppingCartQuery ById(int id);

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
    }
}
