using Kooboo.Commerce.Api.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core = Kooboo.Commerce.Payments;

namespace Kooboo.Commerce.Api.Local.Payments
{
    class PaymentMethodQueryExecutor : QueryExecutorBase<PaymentMethod, Core.PaymentMethod>
    {
        public PaymentMethodQueryExecutor(LocalApiContext context)
            : base(context)
        {
        }

        protected override IQueryable<Core.PaymentMethod> CreateLocalQuery()
        {
            return ApiContext.Services.PaymentMethods.Query();
        }

        protected override IQueryable<Core.PaymentMethod> ApplyFilter(IQueryable<Core.PaymentMethod> query, QueryFilter filter)
        {
            if (filter.Name == PaymentMethodFilters.ById.Name)
            {
                query = query.Where(it => it.Id == (int)filter.Parameters["Id"]);
            }
            else if (filter.Name == PaymentMethodFilters.ByName.Name)
            {
                query = query.Where(it => it.Name == (string)filter.Parameters["Name"]);
            }
            else if (filter.Name == PaymentMethodFilters.ByUserKey.Name)
            {
                query = query.Where(it => it.UserKey == (string)filter.Parameters["UserKey"]);
            }

            return query;
        }
    }
}
