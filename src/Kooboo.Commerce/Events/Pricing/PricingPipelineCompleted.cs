using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Pricing
{
    [Event(Category = "Pricing Events", Order = 300)]
    public class PricingPipelineCompleted : DomainEvent
    {
        [JsonIgnore]
        [Reference(Prefix = "")]
        public PricingContext Context { get; private set; }

        protected PricingPipelineCompleted() { }

        public PricingPipelineCompleted(PricingContext context)
        {
            Context = context;
        }
    }
}
