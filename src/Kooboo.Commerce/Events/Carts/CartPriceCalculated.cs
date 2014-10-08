using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Carts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Carts
{
    public class CartPriceCalculated : ICartEvent
    {
        public int CartId { get; private set; }

        [JsonIgnore]
        public PriceCalculationContext Context { get; private set; }

        public CartPriceCalculated() { }

        public CartPriceCalculated(ShoppingCart cart, PriceCalculationContext context)
        {
            CartId = cart.Id;
            Context = context;
        }
    }
}
