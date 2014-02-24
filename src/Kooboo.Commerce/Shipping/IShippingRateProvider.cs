using Kooboo.Commerce.Customers;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Shipping
{
    public interface IShippingRateProvider
    {
        string Name { get; }

        string DisplayName { get; }

        decimal CalculateShippingCost(ShippingMethod method, ShippingCostCalculationContext context);
    }
}
