using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Shipping.ByWeight
{
    [Dependency(typeof(IShippingRateProvider), Key = "ByWeight")]
    public class ByWeightShippingRateProvider : IShippingRateProvider
    {
        public string Name
        {
            get
            {
                return Strings.ProviderName;
            }
        }

        public decimal GetShippingRate(ShippingMethod method, ShippingRateCalculationContext context)
        {
            return 0m;
        }

        public ShippingRateProviderEditor GetEditor()
        {
            return new ShippingRateProviderEditor("~/Areas/" + Strings.AreaName + "/Views/Config.cshtml");
        }
    }
}