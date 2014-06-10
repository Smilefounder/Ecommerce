using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public static class ShoppingCartExtensions
    {
        public static IQueryable<ShoppingCart> NotExpired(this IQueryable<ShoppingCart> query)
        {
            return query.Where(x => x.SessionId == null || !x.SessionId.StartsWith("EXPIRED_"));
        }
    }
}
