using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public static class ShoppingCartExtensions
    {
        public static ShoppingCart ById(this IQueryable<ShoppingCart> query, int cartId)
        {
            return query.FirstOrDefault(c => c.Id == cartId);
        }
    }
}
