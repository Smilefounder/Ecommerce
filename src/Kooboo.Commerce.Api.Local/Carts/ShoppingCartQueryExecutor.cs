using Kooboo.Commerce.Api.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Kooboo.CMS.Sites.Membership;
using Core = Kooboo.Commerce.Carts;

namespace Kooboo.Commerce.Api.Local.Carts
{
    class ShoppingCartQueryExecutor : QueryExecutorBase<ShoppingCart, Core.ShoppingCart>
    {
        public ShoppingCartQueryExecutor(LocalApiContext apiContext)
            : base(apiContext)
        {
        }

        protected override IQueryable<Core.ShoppingCart> CreateLocalQuery()
        {
            return ApiContext.Database.GetRepository<Core.ShoppingCart>().Query().OrderBy(c => c.Id);
        }

        protected override IQueryable<Core.ShoppingCart> ApplyFilter(IQueryable<Core.ShoppingCart> query, QueryFilter filter)
        {
            if (filter.Name == ShoppingCartFilters.ById.Name)
            {
                var cartId = filter.GetParameterValueOrDefault<int>("Id");
                query = query.Where(it => it.Id == cartId);
            }
            else if (filter.Name == ShoppingCartFilters.BySessionId.Name)
            {
                var sessionId = filter.GetParameterValueOrDefault<string>("SessionId");
                query = query.Where(it => it.SessionId == sessionId);
            }

            return query;
        }
    }
}
