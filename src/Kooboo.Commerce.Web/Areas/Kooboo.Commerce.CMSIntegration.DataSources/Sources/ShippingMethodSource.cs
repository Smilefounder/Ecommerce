using Kooboo.Commerce.API.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    public class ShippingMethodSource : ApiCommerceSource
    {
        public ShippingMethodSource()
            : base("ShippingMethods", typeof(IShippingMethodQuery), typeof(ShippingMethod))
        {
        }
    }
}