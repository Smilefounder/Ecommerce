using Kooboo.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class ShippingRateProviderSettingsControllerBase : CommerceControllerBase
    {
        protected virtual ActionResult NextStep(int methodId)
        {
            return Redirect(NextStepUrl(methodId));
        }

        protected virtual string NextStepUrl(int methodId)
        {
            var routeValues = RouteValues.From(Request.QueryString)
                                         .Merge("area", "Commerce")
                                         .Merge("id", methodId);

            return Url.Action("Complete", "ShippingMethod", routeValues);
        }
    }
}
