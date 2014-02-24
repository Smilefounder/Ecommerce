using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Shipping.ByWeight
{
    [Dependency(typeof(IShippingRateProvider), Key = "Kooboo.Commerce.Shipping.ByWeightShippingRateProvider")]
    public class ByWeightShippingRateProvider : IShippingRateProvider
    {
        public string Name
        {
            get
            {
                return Strings.ProviderName;
            }
        }

        public string DisplayName
        {
            get
            {
                return "Shipping rate by weight";
            }
        }

        public decimal CalculateShippingCost(ShippingMethod method, ShippingCostCalculationContext context)
        {
            throw new NotImplementedException();
        }
    }
}