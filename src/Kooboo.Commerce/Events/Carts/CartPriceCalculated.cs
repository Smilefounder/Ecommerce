using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.ShoppingCarts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Carts
{
    [ActivityEvent(Order = 500)]
    public class CartPriceCalculated : Event, ICartEvent
    {
        public int CartId { get; private set; }

        [JsonIgnore]
        public PriceCalculationContext Context { get; private set; }

        protected CartPriceCalculated() { }

        public CartPriceCalculated(ShoppingCart cart, PriceCalculationContext context)
        {
            CartId = cart.Id;
            Context = context;
        }
    }
}
