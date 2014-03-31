using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing.Stages
{
    public class TaxPricingStage : IPricingStage
    {
        public void Execute(PricingContext context)
        {
            context.Tax.SetOriginalValue(context.Subtotal.OriginalValue * .21m);
        }
    }
}
