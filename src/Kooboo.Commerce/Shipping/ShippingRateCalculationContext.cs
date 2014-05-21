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
        public PricingContext PricingContext { get; private set; }

        public ShippingRateCalculationContext(PricingContext pricingContext)
        {
            PricingContext = pricingContext;
        }
    }
}
