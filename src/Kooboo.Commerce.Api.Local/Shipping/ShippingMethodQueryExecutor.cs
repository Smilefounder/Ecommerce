﻿using System.Linq;
using Kooboo.Commerce.Api.Shipping;
using Core = Kooboo.Commerce.Shipping;

namespace Kooboo.Commerce.Api.Local.Shipping
{
    public class ShippingMethodQueryExecutor : QueryExecutorBase<ShippingMethod, Core.ShippingMethod>
    {
        public ShippingMethodQueryExecutor(LocalApiContext context)
            : base(context)
        {
        }

        protected override IQueryable<Core.ShippingMethod> CreateLocalQuery()
        {
            return ApiContext.Services.ShippingMethods.Query().OrderBy(it => it.Id);
        }

        protected override IQueryable<Core.ShippingMethod> ApplyFilter(IQueryable<Core.ShippingMethod> query, QueryFilter filter)
        {
            if (filter.Name == ShippingMethodFilters.ById.Name)
            {
                query = query.Where(it => it.Id == (int)filter.Parameters["Id"]);
            }
            else if (filter.Name == ShippingMethodFilters.ByName.Name)
            {
                query = query.Where(it => it.Name == (string)filter.Parameters["Name"]);
            }

            return query;
        }
    }
}
