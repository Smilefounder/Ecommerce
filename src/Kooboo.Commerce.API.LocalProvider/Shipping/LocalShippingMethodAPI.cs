using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Shipping;
using Kooboo.Commerce.Shipping.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Shipping
{
    [Dependency(typeof(IShippingMethodAPI))]
    public class LocalShippingMethodAPI : LocalShippingMethodQuery, IShippingMethodAPI
    {
        public LocalShippingMethodAPI(IShippingMethodService shippingMethodService)
            : base(shippingMethodService)
        {
        }
    }
}
