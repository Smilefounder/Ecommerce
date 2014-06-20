using Kooboo.Commerce.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing.Stages
{
    public class ShippingPricingStage : IPricingStage
    {
        private IShippingRateProviderFactory _factory;

        public string Name
        {
            get
            {
                return "ShippingPricingStage";
            }
        }

        public ShippingPricingStage(IShippingRateProviderFactory factory)
        {
            _factory = factory;
        }

        public void Execute(PricingContext context)
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

                    context.ShippingCost.SetOriginalValue(shippingCost);
                }
            }
        }
    }
}
