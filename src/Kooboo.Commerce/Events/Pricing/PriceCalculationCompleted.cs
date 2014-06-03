using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Pricing
{
    [Event(Order = 300)]
    public class PriceCalculationCompleted : DomainEvent, IPricingEvent
    {
        [JsonIgnore]
        [Reference(Prefix = "")]
        public PricingContext Context { get; private set; }

        protected PriceCalculationCompleted() { }

        public PriceCalculationCompleted(PricingContext context)
        {
            Context = context;
        }
    }
}
