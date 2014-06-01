using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Pricing
{
    [Event(Order = 100)]
    public class PriceCalculationStarted : DomainEvent, IPricingEvent
    {
        [JsonIgnore]
        [Reference(Prefix = "")]
        public PricingContext Context { get; private set; }

        [JsonIgnore]
        public IList<IPricingStage> Stages { get; private set; }

        protected PriceCalculationStarted() { }

        public PriceCalculationStarted(PricingContext context, IList<IPricingStage> stages)
        {
            Context = context;
            Stages = stages;
        }
    }
}
