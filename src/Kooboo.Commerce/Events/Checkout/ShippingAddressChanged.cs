using Kooboo.Commerce.Locations;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Checkout
{
    [Event(Order = 100)]
    public class ShippingAddressChanged : DomainEvent, ICheckoutEvent
    {
        public int CartId { get; set; }

        public int ShippingAddressId { get; set; }

        protected ShippingAddressChanged() { }

        public ShippingAddressChanged(ShoppingCart cart, Address address)
        {
            CartId = cart.Id;
            ShippingAddressId = address.Id;
        }
    }
}
