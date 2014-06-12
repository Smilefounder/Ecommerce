using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShoppingCarts
{
    [Event(Order = 400)]
    public class CartExpired : BusinessEvent, IShoppingCartEvent
    {
        public int CartId { get; set; }

        protected CartExpired() { }

        public CartExpired(ShoppingCart cart)
        {
            CartId = cart.Id;
        }
    }
}
