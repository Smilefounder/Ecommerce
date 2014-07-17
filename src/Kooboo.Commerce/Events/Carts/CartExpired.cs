using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Carts
{
    [ActivityEvent(Order = 600)]
    public class CartExpired : Event, ICartEvent
    {
        public int CartId { get; set; }

        protected CartExpired() { }

        public CartExpired(ShoppingCart cart)
        {
            CartId = cart.Id;
        }
    }
}
