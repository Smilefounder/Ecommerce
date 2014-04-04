using Kooboo.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class PaymentProcessorSettingsControllerBase : CommerceControllerBase
    {
        protected virtual ActionResult NextStep(int methodId)
        {
            var routeValues = RouteValues.From(Request.QueryString).Merge("id", methodId);
            return RedirectToAction("Complete", "PaymentMethod", routeValues);
        }

        protected virtual string NextStepUrl(int methodId)
        {
            var routeValues = RouteValues.From(Request.QueryString)
                                         .Merge("area", "Commerce")
                                         .Merge("id", methodId);
            return Url.Action("Complete", "PaymentMethod", routeValues);
        }
    }
}