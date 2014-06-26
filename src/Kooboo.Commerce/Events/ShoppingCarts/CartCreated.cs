using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShoppingCarts
{
    public class CartCreated : Event, IShoppingCartEvent
    {
        public int CartId { get; set; }

        protected CartCreated() { }

        public CartCreated(ShoppingCart cart)
        {
            CartId = cart.Id;
        }
    }
}
