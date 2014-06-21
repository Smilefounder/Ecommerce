using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.ShoppingCarts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShoppingCarts
{
    [Event(Order = 500, ShortName = "Price Calculated")]
    public class CartPriceCalculated : BusinessEvent, IShoppingCartEvent
    {
        public int CartId { get; private set; }

        [JsonIgnore]
        public PricingContext Context { get; private set; }

        protected CartPriceCalculated() { }

        public CartPriceCalculated(ShoppingCart cart, PricingContext context)
        {
            CartId = cart.Id;
            Context = context;
        }
    }
}
