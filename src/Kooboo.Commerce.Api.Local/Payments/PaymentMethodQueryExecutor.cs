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
            return ApiContext.Services.PaymentMethods.Query().Where(it => it.IsEnabled);
        }

        protected override IQueryable<Core.PaymentMethod> ApplyFilter(IQueryable<Core.PaymentMethod> query, QueryFilter filter)
        {
            if (filter.Name == PaymentMethodFilters.ById.Name)
            {
                var id = (int)filter.Parameters["Id"];
                query = query.Where(it => it.Id == id);
            }
            else if (filter.Name == PaymentMethodFilters.ByName.Name)
            {
                var name = (string)filter.Parameters["Name"];
                query = query.Where(it => it.Name == name);
            }
            else if (filter.Name == PaymentMethodFilters.ByUserKey.Name)
            {
                var userKey = (string)filter.Parameters["UserKey"];
                query = query.Where(it => it.UserKey == userKey);
            }

            return query;
        }
    }
}
