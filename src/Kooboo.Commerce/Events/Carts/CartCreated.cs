using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Carts
{
    [ActivityEvent(Order = 100)]
    public class CartCreated : Event, ICartEvent
    {
        public int CartId { get; set; }

        protected CartCreated() { }

        public CartCreated(ShoppingCart cart)
        {
            CartId = cart.Id;
        }
    }
}
