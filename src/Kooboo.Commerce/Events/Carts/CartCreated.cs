using Kooboo.Commerce.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Carts
{
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
