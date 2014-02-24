using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.Commerce.Web
{
    public static class Routes
    {
        public static RedirectToRouteResult RedirectToAction(string actionName, string controllerName, object routeValues)
        {
            return RedirectToAction(actionName, controllerName, new RouteValueDictionary(routeValues));
        }

        public static RedirectToRouteResult RedirectToAction(string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            routeValues.Add("action", actionName);
            routeValues.Add("controller", controllerName);
            return new RedirectToRouteResult(routeValues);
        }
    }
}
