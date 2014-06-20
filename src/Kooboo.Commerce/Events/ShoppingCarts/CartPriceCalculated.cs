using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShoppingCarts
{
    [Event(Order = 500, ShortName = "Price Calculated")]
    public class CartPriceCalculated : BusinessEvent, IShoppingCartEvent
    {
        public int CartId { get; set; }

        protected CartPriceCalculated() { }

        public CartPriceCalculated(ShoppingCart cart)
        {
            CartId = cart.Id;
        }
    }
}
