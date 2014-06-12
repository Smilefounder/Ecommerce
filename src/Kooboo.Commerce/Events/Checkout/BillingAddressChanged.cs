using Kooboo.Commerce.Locations;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Checkout
{
    [Event(Order = 200)]
    public class BillingAddressChanged : BusinessEvent, ICheckoutEvent
    {
        public int CartId { get; set; }

        public int BillingAddressId { get; set; }

        protected BillingAddressChanged() { }

        public BillingAddressChanged(ShoppingCart cart, Address address)
        {
            CartId = cart.Id;
            BillingAddressId = address.Id;
        }
    }
}
