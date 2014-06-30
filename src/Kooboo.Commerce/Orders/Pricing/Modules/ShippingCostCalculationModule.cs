using Kooboo.Commerce.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing.Modules
{
    public class ShippingCostCalculationModule : IPriceCalculationModule
    {
        private IShippingRateProviderFactory _factory;
        
        public ShippingCostCalculationModule(IShippingRateProviderFactory factory)
        {
            _factory = factory;
        }

        public void Execute(PriceCalculationContext context)
        {
            if (context.ShippingMethod != null)
            {
                var provider = _factory.FindByName(context.ShippingMethod.ShippingRateProviderName);
                if (provider != null)
                {
                    object shippingRateProviderConfig = null;
                    if (provider.ConfigModelType != null)
                    {
                        shippingRateProviderConfig = context.ShippingMethod.LoadShippingRateProviderConfig(provider.ConfigModelType);
                    }

                    var shippingCost = provider.GetShippingRate(
                        new ShippingRateCalculationContext(context.ShippingMethod, shippingRateProviderConfig, context));

                    context.ShippingCost = shippingCost;
                }
            }
        }
    }
}
