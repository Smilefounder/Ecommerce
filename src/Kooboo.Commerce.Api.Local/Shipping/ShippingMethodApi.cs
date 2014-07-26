using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Shipping;
using Kooboo.Commerce.Shipping.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Shipping
{
    [Dependency(typeof(IShippingMethodApi))]
    public class ShippingMethodApi : ShippingMethodQuery, IShippingMethodApi
    {
        public ShippingMethodApi(IShippingMethodService shippingMethodService)
            : base(shippingMethodService)
        {
        }
    }
}
