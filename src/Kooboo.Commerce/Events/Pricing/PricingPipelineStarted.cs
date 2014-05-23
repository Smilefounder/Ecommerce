using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Pricing
{
    [Event(Category = "Pricing Events", Order = 100)]
    public class PricingPipelineStarted : DomainEvent
    {
        [JsonIgnore]
        [Reference(Prefix = "")]
        public PricingContext Context { get; private set; }

        [JsonIgnore]
        public IList<IPricingStage> Stages { get; private set; }

        protected PricingPipelineStarted() { }

        public PricingPipelineStarted(PricingContext context, IList<IPricingStage> stages)
        {
            Context = context;
            Stages = stages;
        }
    }
}
