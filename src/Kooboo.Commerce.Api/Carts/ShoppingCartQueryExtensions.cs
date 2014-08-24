using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Api.Carts;

namespace Kooboo.Commerce.Api
{
    public static class ShoppingCartQueryExtensions
    {
        public static Query<ShoppingCart> ById(this Query<ShoppingCart> query, int id)
        {
            query.Filters.Add(ShoppingCartFilters.ById.CreateFilter(new { Id = id }));
            return query;
        }

        public static Query<ShoppingCart> BySessionId(this Query<ShoppingCart> query, string sessionId)
        {
            query.Filters.Add(ShoppingCartFilters.BySessionId.CreateFilter(new { SessionId = sessionId }));
            return query;
        }

        public static Query<ShoppingCart> ByAccountId(this Query<ShoppingCart> query, string accountId)
        {
            query.Filters.Add(ShoppingCartFilters.ByAccountId.CreateFilter(new { AccountId = accountId }));
            return query;
        }
    }
}
