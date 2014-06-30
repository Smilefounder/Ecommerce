using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing.Modules
{
    public class TaxCalculationModule : IPriceCalculationModule
    {
        public void Execute(PriceCalculationContext context)
        {
            // TODO: Calculate TAX
            var tax = 0m;
            context.Tax = tax;
        }
    }
}
