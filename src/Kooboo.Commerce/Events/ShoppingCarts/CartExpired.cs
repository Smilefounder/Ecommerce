using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShoppingCarts
{
    public class CartExpired : Event, IShoppingCartEvent
    {
        public int CartId { get; set; }

        protected CartExpired() { }

        public CartExpired(ShoppingCart cart)
        {
            CartId = cart.Id;
        }
    }
}
