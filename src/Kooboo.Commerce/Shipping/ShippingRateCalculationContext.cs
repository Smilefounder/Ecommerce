using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping
{
    public class ShippingRateCalculationContext
    {
        public Customer Customer { get; set; }

        public ICollection<ShoppingCartItem> Items { get; set; }

        public Address ShippingAddress { get; set; }
    }
}
