using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing.Stages
{
    public class PaymentMethodPricingStage : IPricingStage
    {
        public void Execute(PricingContext context)
        {
            if (context.PaymentMethod != null)
            {
                context.PaymentMethodCost.SetOriginalValue(
                    context.PaymentMethod.GetPaymentMethodCost(context.Subtotal.OriginalValue));
            }
        }
    }
}
