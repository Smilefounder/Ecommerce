using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing.Stages
{
    public class TaxPricingStage : IPricingStage
    {
        public string Name
        {
            get
            {
                return "TaxPricingStage";
            }
        }

        public void Execute(PricingContext context)
        {
            // TODO: Calculate TAX
            var tax = 0m;
            context.Tax.SetOriginalValue(tax);
        }
    }
}
