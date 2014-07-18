using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Shipping.ByWeight
{
    public class ByWeightShippingRateProvider : IShippingRateProvider, IHasCustomShippingRateProviderConfigEditor
    {
        public string Name
        {
            get
            {
                return Strings.ProviderName;
            }
        }

        public Type ConfigType
        {
            get
            {
                return typeof(ByWeightShippingRateProviderConfig);
            }
        }

        public decimal GetShippingRate(ShippingRateCalculationContext context)
        {
            return 0m;
        }

        public string GetEditorVirtualPath(ShippingMethod shippingMethod)
        {
            return "~/Areas/" + Strings.AreaName + "/Views/Config.cshtml";
        }
    }
}