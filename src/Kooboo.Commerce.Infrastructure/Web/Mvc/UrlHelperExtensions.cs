using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Routing;

namespace Kooboo.Commerce.Web.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string RouteUrl(this UrlHelper helper, RedirectToRouteResult routeResult)
        {
            return RouteUrl(helper, routeResult, (RouteValueDictionary)null);
        }

        public static string RouteUrl(this UrlHelper helper, RedirectToRouteResult routeResult, object additionalRouteValues)
        {
            return RouteUrl(helper, routeResult, additionalRouteValues == null ? null : new RouteValueDictionary(additionalRouteValues));
        }

        public static string RouteUrl(this UrlHelper helper, RedirectToRouteResult routeResult, RouteValueDictionary additionalRouteValues)
        {
            var routeValues = new RouteValueDictionary(routeResult.RouteValues);
            if (additionalRouteValues != null)
            {
                routeValues.Merge(additionalRouteValues);
            }

            return helper.RouteUrl(routeResult.RouteName, routeValues);
        }
    }
}
