using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Pricing
{
    [Event(Order = 200)]
    public class PriceCalculationStageCompleted : BusinessEvent, IPricingEvent
    {
        [Param]
        public string StageName { get; private set; }

        protected PriceCalculationStageCompleted() { }

        public PriceCalculationStageCompleted(string stageName)
        {
            Require.NotNullOrEmpty(stageName, "stageName");
            StageName = stageName;
        }
    }
}
