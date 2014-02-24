using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Shipping
{
    public interface IShippingRateProviderViews
    {
        string ProviderName { get; }

        RedirectToRouteResult Settings(ShippingMethod method, ControllerContext context);
    }
}
