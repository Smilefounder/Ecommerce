using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping
{
    public class ShippingRateCalculationContext
    {
        public ShippingMethod ShippingMethod { get; private set; }

        public PriceCalculationContext PricingContext { get; private set; }

        public object ShippingRateProviderConfig { get; private set; }

        public ShippingRateCalculationContext(ShippingMethod shippingMethod, object shippingRateProviderConfig, PriceCalculationContext pricingContext)
        {
            Require.NotNull(shippingMethod, "shippingMethod");
            Require.NotNull(pricingContext, "pricingContext");

            ShippingMethod = shippingMethod;
            ShippingRateProviderConfig = shippingRateProviderConfig;
            PricingContext = pricingContext;
        }
    }
}
