using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Shipping.ByWeight
{
    public class ByWeightShippingRateProviderConfig
    {
        public List<ByWeightShippingRule> Rules { get; set; }

        public ByWeightShippingRateProviderConfig()
        {
            Rules = new List<ByWeightShippingRule>();
        }
    }
}