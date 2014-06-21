using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Shipping.FixedRate
{
    public class FixedShippingRateProvider : IShippingRateProvider
    {
        public string Name
        {
            get
            {
                return "FixedRate";
            }
        }

        public Type ConfigModelType
        {
            get
            {
                return typeof(FixedShippingRateProviderConfig);
            }
        }

        public decimal GetShippingRate(ShippingRateCalculationContext context)
        {
            var config = context.ShippingRateProviderConfig as FixedShippingRateProviderConfig;
            if (config != null)
            {
                return config.ShippingRate;
            }

            return 0m;
        }
    }
}
