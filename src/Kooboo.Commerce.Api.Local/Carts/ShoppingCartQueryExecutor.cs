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
            return ApiContext.Services.Carts.Query().OrderBy(c => c.Id);
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
            else if (filter.Name == ShoppingCartFilters.ByAccountId.Name)
            {
                var accountId = filter.GetParameterValueOrDefault<string>("AccountId");
                query = query.Where(it => it.Customer.AccountId == accountId);
            }
            else if (filter.Name == ShoppingCartFilters.ByCurrentCustomer.Name)
            {
                // TODO: Ask ApiContext for Membership
                var httpContext = new HttpContextWrapper(HttpContext.Current);
                var member = httpContext.Membership().GetMembershipUser();
                if (member != null && !String.IsNullOrWhiteSpace(member.UUID))
                {
                    query = query.Where(it => it.Customer.AccountId == member.UUID);
                }
                else
                {
                    // TODO: Ask ApiContext for SessionID
                    query = query.Where(it => it.SessionId == httpContext.Session.SessionID);
                }
            }

            return query;
        }
    }
}
