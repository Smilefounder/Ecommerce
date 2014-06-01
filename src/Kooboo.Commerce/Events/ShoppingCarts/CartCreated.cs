using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShoppingCarts
{
    [Event(Order = 100)]
    public class CartCreated : DomainEvent, IShoppingCartEvent
    {
        public int CartId { get; set; }

        public CartCreated() { }

        public CartCreated(ShoppingCart cart)
        {
            CartId = cart.Id;
        }
    }
}
