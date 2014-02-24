using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Pricing;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping
{
    public class ShippingCostCalculationContext
    {
        public Customer Customer { get; set; }

        public ICollection<PricingItem> Items { get; set; }

        public Address ShippingAddress { get; set; }
    }
}
