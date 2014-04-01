using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing.Stages
{
    public class PreparingPricingStage : IPricingStage
    {
        public void Execute(PricingContext context)
        {
            context.Subtotal.SetOriginalValue(context.Items.Sum(x => x.Subtotal.OriginalValue));
        }
    }
}
