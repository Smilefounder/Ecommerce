using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Shipping.ByWeight
{
    [Dependency(typeof(IShippingRateProviderViews), Key = "Kooboo.Commerce.Shipping.ByWeight.ByWeightShippingRateProviderViews")]
    public class ByWeightShippingRateProviderViews : IShippingRateProviderViews
    {
        public string ProviderName
        {
            get
            {
                return Strings.ProviderName;
            }
        }

        public System.Web.Mvc.RedirectToRouteResult Settings(ShippingMethod method, ControllerContext controllerContext)
        {
            return Routes.RedirectToAction(
                "Index", "Home", new { methodId = method.Id, area = Strings.AreaName });
        }
    }
}