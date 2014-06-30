using Kooboo.Commerce.Orders.Pricing.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing
{
    public static class PriceCalculationModules
    {
        static readonly PriceCalculationModuleCollection _modules = new PriceCalculationModuleCollection
        {
            typeof(ShippingCostCalculationModule),
            typeof(PaymentMethodCostCalculationModule),
            typeof(TaxCalculationModule),
            typeof(PromotionModule)
        };

        public static PriceCalculationModuleCollection Modules
        {
            get
            {
                return _modules;
            }
        }
    }
}
