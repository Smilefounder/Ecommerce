using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing.Modules
{
    public class PaymentMethodCostCalculationModule : IPriceCalculationModule
    {
        public void Execute(PriceCalculationContext context)
        {
            if (context.PaymentMethod != null)
            {
                context.PaymentMethodCost = context.PaymentMethod.GetPaymentMethodCost(context.Subtotal);
            }
        }
    }
}
