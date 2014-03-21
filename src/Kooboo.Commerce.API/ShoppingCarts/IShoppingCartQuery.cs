using Kooboo.CMS.Membership.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.ShoppingCarts
{
    public interface IShoppingCartQuery : ICommerceQuery<ShoppingCart>
    {
        IShoppingCartQuery BySessionId(string sessionId);
        IShoppingCartQuery ByAccountId(string accountId);

        IShoppingCartQuery LoadWithCustomer();

    }
}
